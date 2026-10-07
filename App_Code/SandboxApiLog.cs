using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Single logging surface for every KYC provider call - PAN, Aadhaar and Bank.
/// Everything lands in one table (KycApiLog), separated by KycType and RecordType
/// rather than by having a table per document type.
///
/// Two rules drive this class:
///   1. Logging must NEVER break the business flow - every method swallows its own errors.
///   2. Nothing is lost silently - if the database write fails the record goes to disk.
/// </summary>
public static class SandboxApiLog
{
    private static readonly string connStr =
        ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    private static bool IsEnabled
    {
        get
        {
            bool b;
            return !bool.TryParse(ConfigurationManager.AppSettings["SandboxLogEnabled"], out b) || b;
        }
    }

    #region Per-request context

    /// <summary>
    /// Set once from the page before making API calls so the static API classes
    /// can stamp every row with member and client info. Held in HttpContext.Items
    /// so concurrent users never mix.
    /// </summary>
    public class CallContext
    {
        public long FormNo { get; set; }
        public string ClientIP { get; set; }
        public string UserAgent { get; set; }
        public string SessionId { get; set; }
        public string PageName { get; set; }
    }

    private const string ContextKey = "__SandboxCallContext";
    private const string LastWireIdKey = "__SandboxLastWireLogId";

    public static void SetContext(CallContext ctx)
    {
        try
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items[ContextKey] = ctx;
        }
        catch { }
    }

    public static CallContext GetContext()
    {
        try
        {
            if (HttpContext.Current != null && HttpContext.Current.Items[ContextKey] != null)
                return (CallContext)HttpContext.Current.Items[ContextKey];
        }
        catch { }
        return new CallContext();
    }

    /// <summary>
    /// LogId of the most recent WIRE record written during this request, so the
    /// ATTEMPT row can point back at the exact HTTP exchange.
    /// </summary>
    public static long? LastApiLogId
    {
        get
        {
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Items[LastWireIdKey] != null)
                    return (long)HttpContext.Current.Items[LastWireIdKey];
            }
            catch { }
            return null;
        }
        private set
        {
            try
            {
                if (HttpContext.Current != null && value.HasValue)
                    HttpContext.Current.Items[LastWireIdKey] = value.Value;
            }
            catch { }
        }
    }

    #endregion

    #region WIRE logging

    /// <summary>
    /// Records one raw HTTP exchange. Called from a finally block so a failed call
    /// is logged just as reliably as a successful one.
    /// KycType is derived from apiName, which keeps the API classes unchanged.
    /// </summary>
    public static long? Write(
        string apiName,
        string endpoint,
        string httpMethod,
        string requestHeaders,
        string requestBody,
        int? httpStatusCode,
        string responseHeaders,
        string responseBody,
        bool isSuccess,
        string errorMessage,
        long durationMs)
    {
        if (!IsEnabled) return null;

        CallContext ctx = GetContext();
        string kycType = DeriveKycType(apiName);

        try
        {
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("sp_LogKycApi", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30;

                cmd.Parameters.AddWithValue("@FormNo", ctx.FormNo);
                cmd.Parameters.AddWithValue("@KycType", Nz(kycType));
                cmd.Parameters.AddWithValue("@RecordType", "WIRE");
                cmd.Parameters.AddWithValue("@Stage", Nz(apiName));
                cmd.Parameters.AddWithValue("@FinalResult", isSuccess ? "VERIFIED" : "FAILED");
                cmd.Parameters.AddWithValue("@Endpoint", Nz(endpoint));
                cmd.Parameters.AddWithValue("@HttpMethod", Nz(httpMethod));
                cmd.Parameters.AddWithValue("@HttpStatusCode", (object)httpStatusCode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@RequestHeaders", Nz(Mask(requestHeaders)));
                cmd.Parameters.AddWithValue("@RequestBody", Nz(Mask(requestBody)));
                cmd.Parameters.AddWithValue("@ResponseHeaders", Nz(responseHeaders));
                cmd.Parameters.AddWithValue("@ResponseBody", Nz(Mask(responseBody)));
                cmd.Parameters.AddWithValue("@DurationMs", durationMs);
                cmd.Parameters.AddWithValue("@ErrorMessage", Nz(errorMessage));
                cmd.Parameters.AddWithValue("@PageName", Nz(ctx.PageName));
                cmd.Parameters.AddWithValue("@ClientIP", Nz(ctx.ClientIP));
                cmd.Parameters.AddWithValue("@UserAgent", Nz(ctx.UserAgent));
                cmd.Parameters.AddWithValue("@SessionId", Nz(ctx.SessionId));

                con.Open();
                object o = cmd.ExecuteScalar();

                if (o != null && o != DBNull.Value)
                {
                    long id = Convert.ToInt64(o);
                    LastApiLogId = id;
                    return id;
                }
            }
        }
        catch (Exception dbEx)
        {
            WriteToFile("WIRE", new Dictionary<string, string>
            {
                { "DbError",         dbEx.Message },
                { "FormNo",          ctx.FormNo.ToString() },
                { "KycType",         kycType },
                { "ApiName",         apiName },
                { "Endpoint",        endpoint },
                { "RequestBody",     Mask(requestBody) },
                { "HttpStatusCode",  httpStatusCode.HasValue ? httpStatusCode.Value.ToString() : "" },
                { "ResponseBody",    Mask(responseBody) },
                { "ErrorMessage",    errorMessage },
                { "ClientIP",        ctx.ClientIP }
            });
        }

        return null;
    }

    /// <summary>
    /// AUTHENTICATE is shared infrastructure, everything else is named after the
    /// document it verifies.
    /// </summary>
    private static string DeriveKycType(string apiName)
    {
        if (string.IsNullOrEmpty(apiName)) return "UNKNOWN";
        string n = apiName.ToUpper();

        if (n.StartsWith("PAN")) return "PAN";
        if (n.StartsWith("AADHAAR")) return "AADHAAR";
        if (n.StartsWith("BANK")) return "BANK";
        if (n.StartsWith("AUTH")) return "AUTH";
        return "UNKNOWN";
    }

    #endregion

    #region ATTEMPT logging

    /// <summary>
    /// One business-level record per submission attempt, for any KYC type.
    /// Called at EVERY exit point including validation rejections that never
    /// reach the API, so no submission is invisible in the audit trail.
    ///
    /// Deliberately independent of the save procedures: those run in transactions
    /// that can roll back, and an audit record must not vanish with them.
    /// </summary>
    public class Attempt
    {
        public long FormNo { get; set; }
        public string KycType { get; set; }      // PAN / AADHAAR / BANK
        public string Stage { get; set; }
        public string FinalResult { get; set; }  // VERIFIED / FAILED
        public string FailReason { get; set; }

        public string RefNo { get; set; }        // masked identifier
        public string RefNoHash { get; set; }
        public string NameEntered { get; set; }
        public string DobEntered { get; set; }
        public string ExtraRef { get; set; }

        public string ApiStatus { get; set; }
        public string ApiName { get; set; }
        public string ApiDob { get; set; }
        public string ApiGender { get; set; }
        public string ApiAddress { get; set; }
        public string ApiCategory { get; set; }
        public bool? NameMatch { get; set; }
        public bool? DobMatch { get; set; }
        public string ApiTxnId { get; set; }
        public int? HttpStatusCode { get; set; }
        public long? DurationMs { get; set; }
        public string ErrorMessage { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }

        public string PostData { get; set; }
        public bool ConsentGiven { get; set; }
        public string ApiCareOf { get; set; }
        public string ApiPincode { get; set; }
    }

    public static void WriteAttempt(Attempt a)
    {
        if (a == null) return;

        CallContext ctx = GetContext();

        try
        {
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("sp_LogKycApi", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30;

                cmd.Parameters.AddWithValue("@FormNo", a.FormNo);
                cmd.Parameters.AddWithValue("@KycType", Nz(a.KycType));
                cmd.Parameters.AddWithValue("@RecordType", "ATTEMPT");
                cmd.Parameters.AddWithValue("@Stage", Nz(a.Stage));
                cmd.Parameters.AddWithValue("@FinalResult", Nz(a.FinalResult));
                cmd.Parameters.AddWithValue("@FailReason", Nz(Trunc(a.FailReason, 500)));

                cmd.Parameters.AddWithValue("@RefNo", Nz(a.RefNo));
                cmd.Parameters.AddWithValue("@RefNoHash", Nz(a.RefNoHash));
                cmd.Parameters.AddWithValue("@NameEntered", Nz(a.NameEntered));
                cmd.Parameters.AddWithValue("@DobEntered", Nz(a.DobEntered));
                cmd.Parameters.AddWithValue("@ExtraRef", Nz(a.ExtraRef));

                cmd.Parameters.AddWithValue("@ApiStatus", Nz(a.ApiStatus));
                cmd.Parameters.AddWithValue("@ApiName", Nz(a.ApiName));
                cmd.Parameters.AddWithValue("@ApiDob", Nz(a.ApiDob));
                cmd.Parameters.AddWithValue("@ApiGender", Nz(a.ApiGender));
                cmd.Parameters.AddWithValue("@ApiAddress", Nz(a.ApiAddress));
                cmd.Parameters.AddWithValue("@ApiCategory", Nz(a.ApiCategory));
                cmd.Parameters.AddWithValue("@NameMatch",
                    a.NameMatch.HasValue ? (object)(a.NameMatch.Value ? "Y" : "N") : DBNull.Value);
                cmd.Parameters.AddWithValue("@DobMatch",
                    a.DobMatch.HasValue ? (object)(a.DobMatch.Value ? "Y" : "N") : DBNull.Value);
                cmd.Parameters.AddWithValue("@ApiTxnId", Nz(a.ApiTxnId));

                cmd.Parameters.AddWithValue("@HttpStatusCode", (object)a.HttpStatusCode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DurationMs", (object)a.DurationMs ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ErrorMessage", Nz(a.ErrorMessage));
                cmd.Parameters.AddWithValue("@RequestBody", Nz(Mask(a.RequestBody)));
                cmd.Parameters.AddWithValue("@ResponseBody", Nz(Mask(a.ResponseBody)));

                cmd.Parameters.AddWithValue("@LinkedLogId",
                    LastApiLogId.HasValue ? (object)LastApiLogId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@PageName", Nz(ctx.PageName));
                cmd.Parameters.AddWithValue("@PostData", Nz(Mask(a.PostData)));
                cmd.Parameters.AddWithValue("@ConsentGiven", a.ConsentGiven ? "Y" : "N");
                cmd.Parameters.AddWithValue("@ClientIP", Nz(ctx.ClientIP));
                cmd.Parameters.AddWithValue("@UserAgent", Nz(ctx.UserAgent));
                cmd.Parameters.AddWithValue("@SessionId", Nz(ctx.SessionId));
                cmd.Parameters.AddWithValue("@ApiCareOf", Nz(a.ApiCareOf));
                cmd.Parameters.AddWithValue("@ApiPincode", Nz(a.ApiPincode));
                con.Open();
                cmd.ExecuteScalar();
            }
        }
        catch (Exception ex)
        {
            WriteToFile("ATTEMPT", new Dictionary<string, string>
            {
                { "DbError",      ex.Message },
                { "FormNo",       a.FormNo.ToString() },
                { "KycType",      a.KycType },
                { "Stage",        a.Stage },
                { "FinalResult",  a.FinalResult },
                { "FailReason",   a.FailReason },
                { "RefNo",        a.RefNo },
                { "NameEntered",  a.NameEntered },
                { "DobEntered",   a.DobEntered },
                { "ApiStatus",    a.ApiStatus },
                { "RequestBody",  Mask(a.RequestBody) },
                { "ResponseBody", Mask(a.ResponseBody) },
                { "ErrorMessage", a.ErrorMessage },
                { "PostData",     Mask(a.PostData) }
            });
        }
    }

    #endregion

    #region Fallback, masking, helpers

    /// <summary>
    /// Last-resort sink. One file per day under ~/App_Data/SandboxLogs/.
    /// Plain text so it can be read without tooling during an incident.
    /// </summary>
    public static void WriteToFile(string kind, Dictionary<string, string> fields)
    {
        try
        {
            string dir = HttpContext.Current != null
                ? HttpContext.Current.Server.MapPath("~/App_Data/SandboxLogs/")
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\SandboxLogs\\");

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            string file = Path.Combine(dir, "kyc_" + DateTime.Now.ToString("yyyyMMdd") + ".log");

            var sb = new StringBuilder();
            sb.AppendLine("========================================================");
            sb.AppendLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] " + kind);
            foreach (var kv in fields)
                sb.AppendLine(kv.Key + ": " + kv.Value);
            sb.AppendLine();

            File.AppendAllText(file, sb.ToString(), Encoding.UTF8);
        }
        catch
        {
            // Disk full or no write permission. Nothing further can be done here
            // without risking the user's transaction.
        }
    }

    /// <summary>
    /// Redacts credentials and identity numbers before anything is persisted.
    /// Tokens, keys, Aadhaar numbers, OTPs and the UIDAI photo blob never reach storage.
    /// </summary>
    public static string Mask(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        try
        {
            // Header form:  x-api-key: abc123
            text = Regex.Replace(text, @"(?i)(x-api-secret\s*:\s*)([^\r\n]+)", "$1***MASKED***");
            text = Regex.Replace(text, @"(?i)(x-api-key\s*:\s*)([^\r\n]+)", "$1***MASKED***");
            text = Regex.Replace(text, @"(?i)(authorization\s*:\s*)([^\r\n]+)", "$1***MASKED***");

            // JSON form:  "access_token": "abc123"
            text = Regex.Replace(text, "(?i)(\"access_token\"\\s*:\\s*\")([^\"]*)(\")", "$1***MASKED***$3");
            text = Regex.Replace(text, "(?i)(\"api_secret\"\\s*:\\s*\")([^\"]*)(\")", "$1***MASKED***$3");

            // Aadhaar must never reach a log in full - keep only the last four digits.
            text = Regex.Replace(text, "(?i)(\"aadhaar_number\"\\s*:\\s*\")(\\d{8})(\\d{4})(\")", "$1XXXXXXXX$3$4");
            text = Regex.Replace(text, "(?i)(\"otp\"\\s*:\\s*\")([^\"]*)(\")", "$1***MASKED***$3");

            // The UIDAI photo is a large base64 blob with no audit value.
            text = Regex.Replace(text, "(?i)(\"photo\"\\s*:\\s*\")([^\"]{40,})(\")", "$1[PHOTO OMITTED]$3");

            // Bare 12-digit Aadhaar appearing anywhere else, e.g. inside PostData.
            text = Regex.Replace(text, @"(?<![0-9])([2-9][0-9]{7})([0-9]{4})(?![0-9])", "XXXXXXXX$2");
        }
        catch { }

        return text;
    }

    public static string FormatHeaders(Dictionary<string, string> headers)
    {
        if (headers == null || headers.Count == 0) return "";

        var sb = new StringBuilder();
        foreach (var kv in headers)
            sb.AppendLine(kv.Key + ": " + kv.Value);

        return sb.ToString().TrimEnd();
    }

    public static string FormatHeaders(System.Net.WebHeaderCollection headers)
    {
        if (headers == null) return "";

        var sb = new StringBuilder();
        foreach (string key in headers.AllKeys)
            sb.AppendLine(key + ": " + headers[key]);

        return sb.ToString().TrimEnd();
    }

    private static object Nz(string s)
    {
        return string.IsNullOrEmpty(s) ? (object)DBNull.Value : s;
    }

    private static string Trunc(string s, int max)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return s.Length <= max ? s : s.Substring(0, max);
    }

    #endregion
}