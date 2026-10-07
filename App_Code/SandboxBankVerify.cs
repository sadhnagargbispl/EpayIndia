using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

[Serializable]
public class IfscLookupResult
{
    public bool Success { get; set; }
    public string Ifsc { get; set; }
    public string Bank { get; set; }
    public string BankCode { get; set; }
    public string Branch { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string State { get; set; }
    public string Micr { get; set; }
    public string Contact { get; set; }
    public bool Neft { get; set; }
    public bool Imps { get; set; }
    public bool Rtgs { get; set; }
    public bool Upi { get; set; }

    public string RequestUrl { get; set; }
    public string RawResponse { get; set; }
    public int? HttpStatusCode { get; set; }
    public long DurationMs { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsServiceError { get; set; }
}

[Serializable]
public class BankVerifyResult
{
    public bool Success { get; set; }
    public bool AccountExists { get; set; }
    public string NameAtBank { get; set; }
    public string Message { get; set; }
    public string Utr { get; set; }             // penny drop only
    public string AmountDeposited { get; set; } // penny drop only
    public string Mode { get; set; }            // pennyless / pennydrop
    public string TransactionId { get; set; }

    public string RequestUrl { get; set; }
    public string RawResponse { get; set; }
    public int? HttpStatusCode { get; set; }
    public long DurationMs { get; set; }
    public string ErrorMessage { get; set; }

    /// <summary>
    /// True when the failure was ours - no credits, bad keys, provider down -
    /// rather than the member's. Drives a different message and a different
    /// recovery path on screen.
    /// </summary>
    public bool IsServiceError { get; set; }
}

public static class SandboxBankVerify
{
    private static readonly Regex IfscRegex =
        new Regex("^[A-Z]{4}0[A-Z0-9]{6}$", RegexOptions.Compiled);

    private static readonly Regex AccountRegex =
        new Regex("^[0-9]{6,20}$", RegexOptions.Compiled);

    #region Validation and masking

    /// <summary>
    /// IFSC is four letters, a mandatory zero, then six alphanumerics.
    /// The fifth character is reserved by RBI and is always 0.
    /// </summary>
    public static bool IsValidIfsc(string ifsc)
    {
        if (string.IsNullOrEmpty(ifsc)) return false;
        return IfscRegex.IsMatch(ifsc.Trim().ToUpper());
    }

    public static bool IsValidAccountNumber(string acno)
    {
        if (string.IsNullOrEmpty(acno)) return false;
        return AccountRegex.IsMatch(acno.Trim());
    }

    /// <summary>
    /// Keeps only the last four digits. Full account numbers should not appear
    /// in logs or on screen once verification is done.
    /// </summary>
    public static string MaskAccount(string acno)
    {
        if (string.IsNullOrEmpty(acno)) return "";
        string a = acno.Trim();
        if (a.Length <= 4) return new string('X', a.Length);
        return new string('X', a.Length - 4) + a.Substring(a.Length - 4);
    }

    /// <summary>
    /// Compares the member's registered name with the name the bank holds.
    /// Returns the share of the bank's name words that also appear in the
    /// member's name, so initials and dropped middle names do not fail outright.
    /// </summary>
    public static double NameMatchScore(string memberName, string bankName)
    {
        if (string.IsNullOrEmpty(memberName) || string.IsNullOrEmpty(bankName)) return 0;

        string[] a = Tokenise(memberName);
        string[] b = Tokenise(bankName);
        if (a.Length == 0 || b.Length == 0) return 0;

        int hits = 0;
        foreach (string wb in b)
        {
            foreach (string wa in a)
            {
                if (wa == wb) { hits++; break; }

                // A single letter is treated as an initial for the other token.
                if (wa.Length == 1 && wb.StartsWith(wa)) { hits++; break; }
                if (wb.Length == 1 && wa.StartsWith(wb)) { hits++; break; }
            }
        }

        return (double)hits / b.Length;
    }

    private static string[] Tokenise(string s)
    {
        s = Regex.Replace(s.ToUpper(), @"[^A-Z ]", " ");
        s = Regex.Replace(s, @"\b(MR|MRS|MS|SHRI|SMT|DR|KUMARI|LATE)\b", " ");
        return s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }

    #endregion

    #region Config

    /// <summary>
    /// pennyless verifies without moving money and costs less. pennydrop
    /// deposits Re 1 and returns a UTR, which some compliance policies require.
    /// </summary>
    public static bool UsePennyDrop
    {
        get
        {
            string m = ConfigurationManager.AppSettings["SandboxBankVerifyMode"];
            return !string.IsNullOrEmpty(m) &&
                   m.Trim().Equals("pennydrop", StringComparison.OrdinalIgnoreCase);
        }
    }

    public static bool EnforceNameMatch
    {
        get
        {
            bool b;
            return !bool.TryParse(ConfigurationManager.AppSettings["SandboxBankEnforceNameMatch"], out b) || b;
        }
    }

    #endregion

    #region Error classification

    private class ClassifiedError
    {
        public string Message;
        public bool IsServiceError;
    }

    private static ClassifiedError Classify(int? status, string body, string parsedMessage)
    {
        var c = new ClassifiedError { Message = parsedMessage, IsServiceError = false };
        string b = body ?? "";

        if (status == 403 &&
            (b.IndexOf("credit", StringComparison.OrdinalIgnoreCase) >= 0 ||
             b.IndexOf("balance", StringComparison.OrdinalIgnoreCase) >= 0 ||
             b.IndexOf("quota", StringComparison.OrdinalIgnoreCase) >= 0))
        {
            c.IsServiceError = true;
            c.Message = "Verification service is temporarily unavailable. Please contact support.";
            c.Message += DevDetail(parsedMessage);
            return c;
        }

        if (status == 401 || status == 403)
        {
            c.IsServiceError = true;
            c.Message = "Verification service is not available right now. Please contact support.";
            c.Message += DevDetail(parsedMessage);
            return c;
        }

        if (status == 502 || status == 503 || status == 504)
        {
            c.IsServiceError = true;
            c.Message = "The bank verification service is temporarily unavailable. Please try again in a few minutes.";
            return c;
        }

        if (status == 429)
        {
            c.IsServiceError = true;
            c.Message = "Too many requests. Please wait a minute and try again.";
            return c;
        }

        // 404 on these endpoints means the IFSC or the account was not found,
        // which is genuine member input to correct.
        return c;
    }

    /// <summary>
    /// The member-facing wording deliberately hides the cause, which is unhelpful
    /// while developing. The raw reason is always in the log, and this surfaces it
    /// on screen too when running locally.
    /// </summary>
    private static string DevDetail(string raw)
    {
        try
        {
            if (System.Web.HttpContext.Current != null &&
                System.Web.HttpContext.Current.Request.IsLocal &&
                !string.IsNullOrEmpty(raw))
            {
                return " [DEV: " + raw + "]";
            }
        }
        catch { }
        return "";
    }

    #endregion

    // ===================== IFSC lookup =====================

    /// <summary>
    /// Looks up branch details for an IFSC. Cheap compared with account
    /// verification, so it runs first and lets the member confirm the branch
    /// before any account call is paid for.
    ///
    /// This endpoint returns a FLAT object with upper-case keys - there is no
    /// "data" wrapper, unlike every other endpoint in the suite.
    /// </summary>
    public static IfscLookupResult LookupIfsc(string ifsc)
    {
        var result = new IfscLookupResult();
        var js = new JavaScriptSerializer();

        string code = (ifsc ?? "").Trim().ToUpper();

        if (!IsValidIfsc(code))
        {
            result.Success = false;
            result.ErrorMessage = "Invalid IFSC code.";
            return result;
        }

        string url = SandboxAuth.BaseUrl + "/bank/" + code;
        result.RequestUrl = url;

        string logHeaderText = "";
        string responseHeaders = "";
        var sw = Stopwatch.StartNew();

        try
        {
            SandboxAuth.EnsureTls();
            string accessToken = SandboxAuth.GetAccessToken();

            var logHeaders = new Dictionary<string, string>
            {
                { "Authorization", accessToken },      // masked before storage
                { "x-api-key", SandboxAuth.ApiKey },   // masked before storage
                { "x-api-version", SandboxAuth.ApiVersion },
                { "Accept", "application/json" }
            };
            logHeaderText = SandboxApiLog.FormatHeaders(logHeaders);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            request.Headers.Add("x-api-key", SandboxAuth.ApiKey);
            request.Headers.Add("x-api-version", SandboxAuth.ApiVersion);
            request.Accept = "application/json";
            request.Timeout = SandboxAuth.TimeoutMs;
            request.ReadWriteTimeout = SandboxAuth.TimeoutMs;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                result.HttpStatusCode = (int)response.StatusCode;
                responseHeaders = SandboxApiLog.FormatHeaders(response.Headers);
                string res = reader.ReadToEnd();
                result.RawResponse = res;

                var obj = js.Deserialize<Dictionary<string, object>>(res);
                if (obj == null || obj.Count == 0)
                {
                    result.Success = false;
                    result.ErrorMessage = "Empty response from the IFSC service.";
                }
                else
                {
                    result.Ifsc = GetStr(obj, "IFSC");
                    result.Bank = GetStr(obj, "BANK");
                    result.BankCode = GetStr(obj, "BANKCODE");
                    result.Branch = GetStr(obj, "BRANCH");
                    result.Address = GetStr(obj, "ADDRESS");
                    result.City = GetStr(obj, "CITY");
                    result.District = GetStr(obj, "DISTRICT");
                    result.State = GetStr(obj, "STATE");
                    result.Micr = GetStr(obj, "MICR");
                    result.Contact = GetStr(obj, "CONTACT");
                    result.Neft = GetBool(obj, "NEFT");
                    result.Imps = GetBool(obj, "IMPS");
                    result.Rtgs = GetBool(obj, "RTGS");
                    result.Upi = GetBool(obj, "UPI");

                    result.Success = !string.IsNullOrEmpty(result.Bank) ||
                                     !string.IsNullOrEmpty(result.Branch);

                    if (!result.Success)
                        result.ErrorMessage = "IFSC not found.";
                }
            }
        }
        catch (WebException wex)
        {
            var httpRes = wex.Response as HttpWebResponse;
            if (httpRes != null)
            {
                result.HttpStatusCode = (int)httpRes.StatusCode;
                responseHeaders = SandboxApiLog.FormatHeaders(httpRes.Headers);
            }

            string body = SandboxAuth.ReadWebExceptionBody(wex);
            result.Success = false;
            result.RawResponse = body;

            if (result.HttpStatusCode == 404)
            {
                result.ErrorMessage = "IFSC code not found. Please check and try again.";
            }
            else
            {
                var c = Classify(result.HttpStatusCode, body, ExtractApiMessage(body, js));
                result.IsServiceError = c.IsServiceError;
                result.ErrorMessage = result.HttpStatusCode.HasValue
                    ? "[" + result.HttpStatusCode.Value + "] " + c.Message
                    : c.Message;
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = ex.Message;
        }
        finally
        {
            sw.Stop();
            result.DurationMs = sw.ElapsedMilliseconds;

            SandboxApiLog.Write(
                "BANK_IFSC_LOOKUP", url, "GET",
                logHeaderText, "",
                result.HttpStatusCode, responseHeaders, result.RawResponse,
                result.Success, result.ErrorMessage, result.DurationMs);
        }

        return result;
    }

    // ===================== Account verification =====================

    /// <summary>
    /// Confirms the account exists and returns the name the bank holds against it.
    /// Penny-less is used by default; penny drop is available through config for
    /// policies that need a UTR as evidence.
    ///
    /// Both endpoints are GET with path parameters and return a "data" wrapper.
    /// </summary>
    public static BankVerifyResult VerifyAccount(string accountNumber, string ifsc)
    {
        var result = new BankVerifyResult();
        var js = new JavaScriptSerializer();

        string acno = (accountNumber ?? "").Trim();
        string code = (ifsc ?? "").Trim().ToUpper();

        if (!IsValidAccountNumber(acno))
        {
            result.Success = false;
            result.ErrorMessage = "Invalid account number.";
            return result;
        }

        if (!IsValidIfsc(code))
        {
            result.Success = false;
            result.ErrorMessage = "Invalid IFSC code.";
            return result;
        }

        bool pennyDrop = UsePennyDrop;
        result.Mode = pennyDrop ? "pennydrop" : "pennyless";

        string url = SandboxAuth.BaseUrl + "/bank/" + code + "/accounts/" + acno +
                     (pennyDrop ? "/verify" : "/penniless-verify");

        // The full account number sits in the path, so the logged URL is masked.
        result.RequestUrl = SandboxAuth.BaseUrl + "/bank/" + code + "/accounts/" +
                            MaskAccount(acno) + (pennyDrop ? "/verify" : "/penniless-verify");

        string logHeaderText = "";
        string responseHeaders = "";
        var sw = Stopwatch.StartNew();

        try
        {
            SandboxAuth.EnsureTls();
            string accessToken = SandboxAuth.GetAccessToken();

            var logHeaders = new Dictionary<string, string>
            {
                { "Authorization", accessToken },
                { "x-api-key", SandboxAuth.ApiKey },
                { "x-api-version", SandboxAuth.ApiVersion },
                { "x-accept-cache", "false" },
                { "Accept", "application/json" }
            };
            logHeaderText = SandboxApiLog.FormatHeaders(logHeaders);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            request.Headers.Add("x-api-key", SandboxAuth.ApiKey);
            request.Headers.Add("x-api-version", SandboxAuth.ApiVersion);

            // A cached result would defeat the purpose: the point of this call is
            // to confirm the account is live right now, before payouts start.
            request.Headers.Add("x-accept-cache", "false");

            request.Accept = "application/json";
            request.Timeout = SandboxAuth.TimeoutMs;
            request.ReadWriteTimeout = SandboxAuth.TimeoutMs;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                result.HttpStatusCode = (int)response.StatusCode;
                responseHeaders = SandboxApiLog.FormatHeaders(response.Headers);
                string res = reader.ReadToEnd();
                result.RawResponse = res;
                ParseVerifyResponse(res, js, result);
            }
        }
        catch (WebException wex)
        {
            var httpRes = wex.Response as HttpWebResponse;
            if (httpRes != null)
            {
                result.HttpStatusCode = (int)httpRes.StatusCode;
                responseHeaders = SandboxApiLog.FormatHeaders(httpRes.Headers);
            }

            string body = SandboxAuth.ReadWebExceptionBody(wex);
            result.Success = false;
            result.RawResponse = body;

            var c = Classify(result.HttpStatusCode, body, ExtractApiMessage(body, js));
            result.IsServiceError = c.IsServiceError;
            result.ErrorMessage = result.HttpStatusCode.HasValue
                ? "[" + result.HttpStatusCode.Value + "] " + c.Message
                : c.Message;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = ex.Message;
        }
        finally
        {
            sw.Stop();
            result.DurationMs = sw.ElapsedMilliseconds;

            SandboxApiLog.Write(
                pennyDrop ? "BANK_PENNY_DROP" : "BANK_PENNY_LESS",
                result.RequestUrl, "GET",
                logHeaderText, "",
                result.HttpStatusCode, responseHeaders, result.RawResponse,
                result.Success, result.ErrorMessage, result.DurationMs);
        }

        return result;
    }

    private static void ParseVerifyResponse(string res, JavaScriptSerializer js, BankVerifyResult result)
    {
        var obj = js.Deserialize<Dictionary<string, object>>(res);

        if (obj == null)
        {
            result.Success = false;
            result.ErrorMessage = "Empty response from the bank verification service.";
            return;
        }

        result.TransactionId = GetStr(obj, "transaction_id");

        var data = obj.ContainsKey("data") ? obj["data"] as Dictionary<string, object> : null;
        if (data == null)
        {
            result.Success = false;
            result.ErrorMessage = ExtractApiMessage(res, js);
            return;
        }

        // account_exists is a JSON boolean, not the "Y"/"N" string the older
        // implementation assumed.
        result.AccountExists = GetBool(data, "account_exists");
        result.NameAtBank = GetStr(data, "name_at_bank");
        result.Message = GetStr(data, "message");
        result.Utr = GetStr(data, "utr");
        result.AmountDeposited = GetStr(data, "amount_deposited");

        result.Success = result.AccountExists;

        if (!result.Success && string.IsNullOrEmpty(result.ErrorMessage))
            result.ErrorMessage = string.IsNullOrEmpty(result.Message)
                ? "Account could not be verified." : result.Message;
    }

    #region Parsing helpers

    private static string ExtractApiMessage(string body, JavaScriptSerializer js)
    {
        if (string.IsNullOrEmpty(body)) return "No response from the verification service.";
        try
        {
            var obj = js.Deserialize<Dictionary<string, object>>(body);
            if (obj != null)
            {
                string msg = GetStr(obj, "message");
                if (!string.IsNullOrEmpty(msg)) return msg;
                msg = GetStr(obj, "error");
                if (!string.IsNullOrEmpty(msg)) return msg;
            }
        }
        catch { }
        return body.Length > 300 ? body.Substring(0, 300) : body;
    }

    private static string GetStr(Dictionary<string, object> d, string key)
    {
        return (d != null && d.ContainsKey(key) && d[key] != null) ? d[key].ToString() : "";
    }

    private static bool GetBool(Dictionary<string, object> d, string key)
    {
        if (d == null || !d.ContainsKey(key) || d[key] == null) return false;
        bool b;
        return bool.TryParse(d[key].ToString(), out b) && b;
    }

    #endregion
}