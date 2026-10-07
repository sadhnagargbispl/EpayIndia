using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

public class PanVerifyResult
{
    public bool Success { get; set; }
    public string Status { get; set; }            // valid / invalid
    public string FullName { get; set; }          // name returned by NSDL
    public string Category { get; set; }          // individual / company etc.
    public string Remarks { get; set; }
    public bool NameMatch { get; set; }
    public bool DobMatch { get; set; }
    public string AadhaarSeedingStatus { get; set; }
    public string TransactionId { get; set; }
    public string RequestJson { get; set; }       // exactly what was posted
    public string RawResponse { get; set; }       // exactly what came back
    public int? HttpStatusCode { get; set; }
    public long DurationMs { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsTestExample { get; set; }   // true when the mock example was sent
}

public static class SandboxPanVerify
{
    private static readonly Regex PanRegex =
        new Regex("^[A-Z]{5}[0-9]{4}[A-Z]$", RegexOptions.Compiled);

    public static bool IsValidPanFormat(string pan)
    {
        return !string.IsNullOrEmpty(pan) && PanRegex.IsMatch(pan.Trim().ToUpper());
    }

    /// <param name="dateOfBirth">Must be dd/MM/yyyy. Pass null to skip.</param>
    public static PanVerifyResult VerifyPan(string panNumber, string nameAsPerPan, string dateOfBirth = null)
    {
        var result = new PanVerifyResult();

        if (!IsValidPanFormat(panNumber))
        {
            result.Success = false;
            result.ErrorMessage = "Invalid PAN format.";
            return result;
        }

        if (string.IsNullOrEmpty(nameAsPerPan))
        {
            result.Success = false;
            result.ErrorMessage = "Name is required for PAN verification.";
            return result;
        }
        if (SandboxAuth.SimulateMode)
            return Simulate(panNumber, nameAsPerPan, dateOfBirth);

        try
        {
            result = Execute(panNumber, nameAsPerPan, dateOfBirth, false);

            // 401/403 usually means a stale cached token - refresh once and retry.
            if (!result.Success && result.ErrorMessage != null &&
                (result.ErrorMessage.Contains("401") ||
                 result.ErrorMessage.IndexOf("Unauthorized", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                SandboxAuth.InvalidateToken();
                result = Execute(panNumber, nameAsPerPan, dateOfBirth, true);
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = ex.Message;
        }
        //try
        //{
        //    result = Execute(panNumber, nameAsPerPan, dateOfBirth, false);

        //    // 401/403 usually means a stale cached token - refresh once and retry.
        //    if (!result.Success && result.ErrorMessage != null &&
        //        (result.ErrorMessage.Contains("401") ||
        //         result.ErrorMessage.IndexOf("Unauthorized", StringComparison.OrdinalIgnoreCase) >= 0))
        //    {
        //        SandboxAuth.InvalidateToken();
        //        result = Execute(panNumber, nameAsPerPan, dateOfBirth, true);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    result.Success = false;
        //    result.ErrorMessage = ex.Message;
        //}

        return result;
    }
    private static PanVerifyResult Simulate(string panNumber, string nameAsPerPan, string dateOfBirth)
    {
        var result = new PanVerifyResult();
        var js = new JavaScriptSerializer();
        var sw = Stopwatch.StartNew();

        string url = "[SIMULATED] " + SandboxAuth.BaseUrl + "/kyc/pan/verify";
        string pan = panNumber.Trim().ToUpper();
        string name = nameAsPerPan.Trim();

        var payload = new Dictionary<string, object>
        {
            { "@entity", "in.co.sandbox.kyc.pan_verification.request" },
            { "pan", pan },
            { "name_as_per_pan", name },
            { "consent", "Y" },
            { "reason", SandboxAuth.PanReason }
        };
        if (!string.IsNullOrEmpty(dateOfBirth))
            payload.Add("date_of_birth", dateOfBirth);

        string requestJson = js.Serialize(payload);

        // The 4th character of a PAN encodes holder type: P individual, C company,
        // F firm, H HUF, T trust. Echoing it back keeps the simulation realistic.
        string category = "individual";
        if (pan.Length >= 4)
        {
            switch (pan[3])
            {
                case 'C': category = "company"; break;
                case 'F': category = "firm"; break;
                case 'H': category = "hindu undivided family"; break;
                case 'T': category = "trust"; break;
                case 'A': category = "association of persons"; break;
                default: category = "individual"; break;
            }
        }

        var data = new Dictionary<string, object>
        {
            { "@entity", "in.co.sandbox.kyc.pan_verification.response" },
            { "pan", pan },
            { "full_name", name },
            { "status", "valid" },
            { "category", category },
            { "name_as_per_pan_match", true },
            { "date_of_birth_match", !string.IsNullOrEmpty(dateOfBirth) },
            { "aadhaar_seeding_status", "Y" },
            { "remarks", "SIMULATED RESPONSE - no verification was performed" }
        };

        var envelope = new Dictionary<string, object>
        {
            { "code", 200 },
            { "timestamp", DateTime.UtcNow.Ticks },
            { "transaction_id", "SIM-" + Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper() },
            { "data", data }
        };

        string responseJson = js.Serialize(envelope);

        result.RequestJson = requestJson;
        result.RawResponse = responseJson;
        result.HttpStatusCode = 200;

        // Parsed through the same path as a live response, so any bug in
        // ParseSuccess surfaces here too rather than hiding until go-live.
        ParseSuccess(responseJson, js, result);

        sw.Stop();
        result.DurationMs = sw.ElapsedMilliseconds;

        SandboxApiLog.Write(
            "PAN_VERIFY_SIMULATED", url, "POST",
            "(simulated - no request sent)", requestJson,
            result.HttpStatusCode, "(simulated)", responseJson,
            result.Success, null, result.DurationMs);

        return result;
    }
    private static PanVerifyResult Execute(string panNumber, string nameAsPerPan,string dateOfBirth, bool forceTokenRefresh)
    {
        var result = new PanVerifyResult();
        var js = new JavaScriptSerializer();

        string url = SandboxAuth.BaseUrl + "/kyc/pan/verify";
        string jsonPayload = "";
        string logHeaderText = "";
        string responseHeaders = "";
        var sw = Stopwatch.StartNew();

        try
        {
            SandboxAuth.EnsureTls();

            string accessToken = SandboxAuth.GetAccessToken(forceTokenRefresh);

            // test-api.sandbox.co.in is a mock that replays saved examples. The request
            // body must match one byte for byte, so in test mode the user's real values
            // are swapped for the documented example. Live API ignores this entirely.
            string sendPan = panNumber.Trim().ToUpper();
            string sendName = nameAsPerPan.Trim();
            string sendDob = dateOfBirth;

            //if (SandboxAuth.UseTestExample)
            //{
            //    sendPan = "XXXPX1234A";
            //    sendName = "John Ronald Doe";
            //    sendDob = "11/11/2001";
            //    result.IsTestExample = true;
            //}

            var payload = new Dictionary<string, object>
            {
                { "@entity", "in.co.sandbox.kyc.pan_verification.request" },   // REQUIRED by Sandbox
                { "pan", sendPan },
                { "name_as_per_pan", sendName },
                { "consent", "Y" },
                { "reason", SandboxAuth.PanReason }
            };

            if (!string.IsNullOrEmpty(sendDob))
                payload.Add("date_of_birth", sendDob);   // dd/MM/yyyy

            jsonPayload = js.Serialize(payload);
            result.RequestJson = jsonPayload;

            var logHeaders = new Dictionary<string, string>
            {
                { "Authorization", accessToken },      // masked before storage
                { "x-api-key", SandboxAuth.ApiKey },   // masked before storage
                { "x-api-version", SandboxAuth.ApiVersion },
                { "Content-Type", "application/json" },
                { "Accept", "application/json" }
            };
            logHeaderText = SandboxApiLog.FormatHeaders(logHeaders);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Headers.Add("Authorization", accessToken);
            request.Headers.Add("x-api-key", SandboxAuth.ApiKey);
            request.Headers.Add("x-api-version", SandboxAuth.ApiVersion);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.Timeout = SandboxAuth.TimeoutMs;
            request.ReadWriteTimeout = SandboxAuth.TimeoutMs;

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonPayload);
            request.ContentLength = byteArray.Length;
            using (Stream ds = request.GetRequestStream())
                ds.Write(byteArray, 0, byteArray.Length);

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                result.HttpStatusCode = (int)response.StatusCode;
                responseHeaders = SandboxApiLog.FormatHeaders(response.Headers);
                string res = reader.ReadToEnd();
                result.RawResponse = res;
                ParseSuccess(res, js, result);
            }

            // In test mode the mock echoes the example, not the member's data.
            // Force the match flags so the flow can be exercised end to end.
            //if (result.Success && result.IsTestExample)
            //{
            //    result.NameMatch = true;
            //    result.DobMatch = true;
            //}
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
            result.ErrorMessage = ExtractApiMessage(body, js);

            if (result.HttpStatusCode.HasValue)
                result.ErrorMessage = "[" + result.HttpStatusCode.Value + "] " + result.ErrorMessage;

            // 404 on the mock server means the payload did not match a saved example.
            // Nothing is wrong with the integration; the values simply are not canned.
            if (result.HttpStatusCode == 404 && SandboxAuth.BaseUrl.Contains("test-api"))
            {
                result.ErrorMessage = "Test environment accepts only the documented sample request. " +
                                      "Set SandboxUseTestExample to true, or switch SandboxBaseUrl " +
                                      "to the live endpoint. (" + result.ErrorMessage + ")";
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
                "PAN_VERIFY", url, "POST",
                logHeaderText, jsonPayload,
                result.HttpStatusCode, responseHeaders, result.RawResponse,
                result.Success, result.ErrorMessage, result.DurationMs);
        }

        return result;
    }
    private static void ParseSuccess(string res, JavaScriptSerializer js, PanVerifyResult result)
    {
        var obj = js.Deserialize<Dictionary<string, object>>(res);

        if (obj == null)
        {
            result.Success = false;
            result.ErrorMessage = "Empty response from Sandbox API.";
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

        result.Success = true;
        result.Status = GetStr(data, "status");
        result.FullName = GetStr(data, "full_name");
        result.Category = GetStr(data, "category");
        result.Remarks = GetStr(data, "remarks");
        result.AadhaarSeedingStatus = GetStr(data, "aadhaar_seeding_status");
        result.NameMatch = GetBool(data, "name_as_per_pan_match");
        result.DobMatch = GetBool(data, "date_of_birth_match");
    }

    private static string ExtractApiMessage(string body, JavaScriptSerializer js)
    {
        if (string.IsNullOrEmpty(body)) return "No response from Sandbox API.";
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
}