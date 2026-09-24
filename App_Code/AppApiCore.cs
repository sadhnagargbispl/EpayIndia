using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Mobile App API (AppApi.aspx / AppWebBridge.aspx) ke common helpers:
/// token, member, DB, external HTTP call aur logging.
/// DB objects ki script: DBScripts/AppApi_Setup.sql
/// </summary>
public static class AppApiCore
{
    public static string Constr
    {
        get { return ConfigurationManager.ConnectionStrings["constr"].ConnectionString; }
    }

    public static string Constr1
    {
        get { return ConfigurationManager.ConnectionStrings["constr1"].ConnectionString; }
    }

    /// <summary>AppWebBridge.aspx wala token (home ke web links) kitne din chale.</summary>
    public const int BridgeTokenValidDays = 1;

    /* ---------------- Token ---------------- */

    public static string NewToken()
    {
        byte[] bytes = new byte[32];
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(bytes);
        }
        return ToHex(bytes);
    }

    /// <summary>DB mein token ka SHA-256 hash hi save hota hai.</summary>
    public static string HashToken(string token)
    {
        using (SHA256 sha = SHA256.Create())
        {
            return ToHex(sha.ComputeHash(Encoding.UTF8.GetBytes(token ?? "")));
        }
    }

    private static string ToHex(byte[] bytes)
    {
        StringBuilder sb = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }

    /// <summary>Token valid ho to member lautata hai, warna null.</summary>
    public static AppMember ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token) || token.Length > 200)
            return null;

        DataSet ds = SqlHelper.ExecuteDataset(Constr, CommandType.Text,
            "EXEC Sp_AppApi_TokenValidate @TokenHash",
            new SqlParameter("@TokenHash", SqlDbType.Char, 64) { Value = HashToken(token.Trim()) });

        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return null;

        DataRow r = ds.Tables[0].Rows[0];
        AppMember m = new AppMember();
        m.FormNo = Int(r, "FormNo");
        m.IdNo = Str(r, "IDNo");
        m.Name = (Str(r, "MemFirstName") + " " + Str(r, "MemLastName")).Trim();
        m.FirstName = Str(r, "MemFirstName");
        m.Mobile = Str(r, "Mobl");
        m.Email = Str(r, "Email");
        m.Passw = Str(r, "Passw");
        m.IsBlock = Str(r, "IsBlock");
        m.ActiveStatus = Str(r, "ActiveStatus");
        return m;
    }

    /* ---------------- DataRow helpers (column na ho / NULL ho to default) ---------------- */

    public static string Str(DataRow r, string col)
    {
        if (r == null || !r.Table.Columns.Contains(col) || r[col] == DBNull.Value)
            return "";
        return Convert.ToString(r[col]).Trim();
    }

    public static decimal Dec(DataRow r, string col)
    {
        decimal d;
        return decimal.TryParse(Str(r, col), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0;
    }

    public static int Int(DataRow r, string col)
    {
        decimal d = Dec(r, col);
        return (int)d;
    }

    public static bool Bool(DataRow r, string col)
    {
        string v = Str(r, col).ToUpper();
        return v == "1" || v == "TRUE" || v == "Y" || v == "YES";
    }

    /* ---------------- Log masking ---------------- */

    private static readonly Regex SecretJson = new Regex(
        "(\"(?:passwd|password|pwd|token|securityCode|merchantID|key|tokenno)\"\\s*:\\s*)\"[^\"]*\"",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>Log mein password / token / keys "****" ho jate hain.</summary>
    public static string Mask(string json)
    {
        if (string.IsNullOrEmpty(json))
            return json;
        return SecretJson.Replace(json, "$1\"****\"");
    }

    public static string ClientIp(HttpRequest req)
    {
        string ip = req.Headers["CF-Connecting-IP"];
        if (string.IsNullOrWhiteSpace(ip))
            ip = req.Headers["X-Forwarded-For"];
        if (string.IsNullOrWhiteSpace(ip))
            ip = req.UserHostAddress;
        if (ip != null && ip.Contains(","))
            ip = ip.Split(',')[0];
        return (ip ?? "").Trim();
    }

    /// <summary>Unique request id: yyyyMMddHHmmssfff + 3 random digit.</summary>
    public static string NewReqId()
    {
        return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(0, 999).ToString().PadLeft(3, '0');
    }

    /// <summary>DB log fail ho jaye to Logs/AppApi_yyyyMMdd.txt mein likhta hai.</summary>
    public static void FileLog(string text)
    {
        try
        {
            string folder = HttpContext.Current != null
                ? HttpContext.Current.Server.MapPath("~/Logs")
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            File.AppendAllText(Path.Combine(folder, "AppApi_" + DateTime.Now.ToString("yyyyMMdd") + ".txt"),
                DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.fff") + " | " + text + Environment.NewLine);
        }
        catch
        {
        }
    }
}

/// <summary>Token se mila hua logged-in member.</summary>
public class AppMember
{
    public int FormNo;
    public string IdNo;
    public string Name;
    public string FirstName;
    public string Mobile;
    public string Email;
    public string Passw;
    public string IsBlock;
    public string ActiveStatus;
}

/// <summary>
/// Ek API request ka log: Tbl_AppApiLog (1 row) + Tbl_AppApiExtCallLog (bahar ki calls).
/// Logging kabhi API ko fail nahi karti - DB log na ho paye to file mein jata hai.
/// </summary>
public class AppApiLogger
{
    public long LogId;
    public string ReqID;
    private readonly Stopwatch watch = Stopwatch.StartNew();

    public AppApiLogger()
    {
        ReqID = AppApiCore.NewReqId();
    }

    public void Start(string reqType, HttpRequest req, string deviceId, string appVersion, string requestJson)
    {
        try
        {
            DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr, CommandType.Text,
                "EXEC Sp_AppApiLog_Insert @ReqID, @ReqType, @HttpMethod, @IPAddress, @UserAgent, @DeviceId, @AppVersion, @RequestJson",
                new SqlParameter("@ReqID", SqlDbType.VarChar, 30) { Value = ReqID },
                new SqlParameter("@ReqType", SqlDbType.VarChar, 50) { Value = Db(Cut(reqType, 50)) },
                new SqlParameter("@HttpMethod", SqlDbType.VarChar, 10) { Value = Db(req.HttpMethod) },
                new SqlParameter("@IPAddress", SqlDbType.VarChar, 50) { Value = Db(Cut(AppApiCore.ClientIp(req), 50)) },
                new SqlParameter("@UserAgent", SqlDbType.NVarChar, 500) { Value = Db(Cut(req.UserAgent, 500)) },
                new SqlParameter("@DeviceId", SqlDbType.NVarChar, 200) { Value = Db(Cut(deviceId, 200)) },
                new SqlParameter("@AppVersion", SqlDbType.VarChar, 30) { Value = Db(Cut(appVersion, 30)) },
                new SqlParameter("@RequestJson", SqlDbType.NVarChar, -1) { Value = Db(AppApiCore.Mask(requestJson)) });
            LogId = Convert.ToInt64(ds.Tables[0].Rows[0]["LogId"]);
        }
        catch (Exception ex)
        {
            AppApiCore.FileLog("LOG START FAILED ReqID=" + ReqID + " ReqType=" + reqType + " Err=" + ex.Message
                + " Request=" + AppApiCore.Mask(requestJson));
        }
    }

    public void End(string reqType, AppMember member, string status, int code, string responseJson,
                    string refNo, string errorMsg, string stackTrace)
    {
        int ms = (int)watch.ElapsedMilliseconds;
        try
        {
            if (LogId <= 0)
                throw new Exception("LogId missing (start log fail hua tha)");

            SqlHelper.ExecuteNonQuery(AppApiCore.Constr, CommandType.Text,
                "EXEC Sp_AppApiLog_Update @LogId, @ReqType, @FormNo, @IdNo, @Status, @ResponseCode, @ResponseJson, @RefNo, @ErrorMsg, @StackTrace, @DurationMs",
                new SqlParameter("@LogId", SqlDbType.BigInt) { Value = LogId },
                new SqlParameter("@ReqType", SqlDbType.VarChar, 50) { Value = Db(Cut(reqType, 50)) },
                new SqlParameter("@FormNo", SqlDbType.Int) { Value = member != null ? (object)member.FormNo : DBNull.Value },
                new SqlParameter("@IdNo", SqlDbType.VarChar, 50) { Value = member != null ? Db(member.IdNo) : DBNull.Value },
                new SqlParameter("@Status", SqlDbType.VarChar, 20) { Value = status },
                new SqlParameter("@ResponseCode", SqlDbType.Int) { Value = code },
                new SqlParameter("@ResponseJson", SqlDbType.NVarChar, -1) { Value = Db(AppApiCore.Mask(responseJson)) },
                new SqlParameter("@RefNo", SqlDbType.VarChar, 100) { Value = Db(Cut(refNo, 100)) },
                new SqlParameter("@ErrorMsg", SqlDbType.NVarChar, 4000) { Value = Db(Cut(errorMsg, 4000)) },
                new SqlParameter("@StackTrace", SqlDbType.NVarChar, -1) { Value = Db(stackTrace) },
                new SqlParameter("@DurationMs", SqlDbType.Int) { Value = ms });
        }
        catch (Exception ex)
        {
            AppApiCore.FileLog("LOG END FAILED ReqID=" + ReqID + " ReqType=" + reqType + " Status=" + status
                + " Code=" + code + " RefNo=" + refNo + " Err=" + errorMsg + " LogErr=" + ex.Message
                + " Response=" + AppApiCore.Mask(responseJson));
        }

        // Error wali request file mein bhi, taaki DB down ho tab bhi trace mile
        if (status == "ERROR")
        {
            AppApiCore.FileLog("ERROR ReqID=" + ReqID + " ReqType=" + reqType + " RefNo=" + refNo
                + " Err=" + errorMsg + Environment.NewLine + stackTrace);
        }
    }

    public void Ext(string callName, string url, string requestBody, string responseBody,
                    int httpStatus, string errorMsg, int durationMs)
    {
        try
        {
            SqlHelper.ExecuteNonQuery(AppApiCore.Constr, CommandType.Text,
                "EXEC Sp_AppApiExtLog_Insert @LogId, @ReqID, @CallName, @Url, @RequestBody, @ResponseBody, @HttpStatus, @ErrorMsg, @DurationMs",
                new SqlParameter("@LogId", SqlDbType.BigInt) { Value = LogId > 0 ? (object)LogId : DBNull.Value },
                new SqlParameter("@ReqID", SqlDbType.VarChar, 30) { Value = ReqID },
                new SqlParameter("@CallName", SqlDbType.VarChar, 50) { Value = callName },
                new SqlParameter("@Url", SqlDbType.NVarChar, 500) { Value = Db(Cut(url, 500)) },
                new SqlParameter("@RequestBody", SqlDbType.NVarChar, -1) { Value = Db(AppApiCore.Mask(requestBody)) },
                new SqlParameter("@ResponseBody", SqlDbType.NVarChar, -1) { Value = Db(AppApiCore.Mask(responseBody)) },
                new SqlParameter("@HttpStatus", SqlDbType.Int) { Value = httpStatus > 0 ? (object)httpStatus : DBNull.Value },
                new SqlParameter("@ErrorMsg", SqlDbType.NVarChar, 4000) { Value = Db(Cut(errorMsg, 4000)) },
                new SqlParameter("@DurationMs", SqlDbType.Int) { Value = durationMs });
        }
        catch (Exception ex)
        {
            AppApiCore.FileLog("EXT LOG FAILED ReqID=" + ReqID + " Call=" + callName + " Url=" + url
                + " HttpStatus=" + httpStatus + " Err=" + errorMsg + " LogErr=" + ex.Message
                + " Response=" + AppApiCore.Mask(responseBody));
        }
    }

    /// <summary>
    /// Bahar ki API call (JSON). Har call Tbl_AppApiExtCallLog mein log hoti hai.
    /// Network / HTTP error par exception throw karta hai.
    /// </summary>
    public string HttpCall(string callName, string method, string url, string body, Dictionary<string, string> headers)
    {
        Stopwatch sw = Stopwatch.StartNew();
        string responseText = "";
        int status = 0;
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/json";
            request.Timeout = 60000;
            request.ReadWriteTimeout = 60000;
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> h in headers)
                {
                    request.Headers.Add(h.Key, h.Value);
                }
            }

            if (method == "POST")
            {
                byte[] bytes = Encoding.UTF8.GetBytes(body ?? "");
                request.ContentLength = bytes.Length;
                using (Stream s = request.GetRequestStream())
                {
                    s.Write(bytes, 0, bytes.Length);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                status = (int)response.StatusCode;
                responseText = reader.ReadToEnd();
            }

            Ext(callName, url, body, responseText, status, null, (int)sw.ElapsedMilliseconds);
            return responseText;
        }
        catch (WebException wex)
        {
            HttpWebResponse errResp = wex.Response as HttpWebResponse;
            if (errResp != null)
            {
                status = (int)errResp.StatusCode;
                try
                {
                    using (StreamReader reader = new StreamReader(errResp.GetResponseStream()))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                catch
                {
                }
            }
            Ext(callName, url, body, responseText, status, wex.Message, (int)sw.ElapsedMilliseconds);
            throw new Exception(callName + " call failed: " + wex.Message, wex);
        }
        catch (Exception ex)
        {
            Ext(callName, url, body, responseText, status, ex.Message, (int)sw.ElapsedMilliseconds);
            throw;
        }
    }

    private static object Db(string v)
    {
        return string.IsNullOrEmpty(v) ? (object)DBNull.Value : v;
    }

    private static string Cut(string v, int max)
    {
        if (v == null)
            return null;
        return v.Length > max ? v.Substring(0, max) : v;
    }
}
