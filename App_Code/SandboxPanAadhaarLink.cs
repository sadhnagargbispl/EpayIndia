using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

public class PanAadhaarLinkResult
{
    public bool Success { get; set; }             // the call itself worked and returned data
    public bool Linked { get; set; }              // aadhaar_seeding_status == "y"
    public string SeedingStatus { get; set; }
    public string Message { get; set; }           // message returned by the ITD
    public string TransactionId { get; set; }
    public string RequestJson { get; set; }       // exactly what was posted
    public string RawResponse { get; set; }       // exactly what came back
    public int? HttpStatusCode { get; set; }
    public long DurationMs { get; set; }
    public string ErrorMessage { get; set; }
}

/// <summary>
/// Checks with the Income Tax Department, through Sandbox, whether a PAN is
/// seeded with a given Aadhaar number. Every call is logged to SandboxApiLog.
/// </summary>
public static class SandboxPanAadhaarLink
{
    private static readonly Regex AadhaarRegex = new Regex("^[2-9][0-9]{11}$", RegexOptions.Compiled);

    public static PanAadhaarLinkResult CheckLink(string panNumber, string aadhaarNumber)
    {
        var result = new PanAadhaarLinkResult();

        string pan = (panNumber ?? "").Trim().ToUpper();
        string aadhaar = Regex.Replace(aadhaarNumber ?? "", "[^0-9]", "");

        // Callers validate first; this is the last line before a paid call.
        if (!SandboxPanVerify.IsValidPanFormat(pan))
        {
            result.ErrorMessage = "Invalid PAN format.";
            return result;
        }

        if (!AadhaarRegex.IsMatch(aadhaar))
        {
            result.ErrorMessage = "Invalid Aadhaar number.";
            return result;
        }

        if (SandboxAuth.SimulateMode)
            return Simulate(pan, aadhaar);

        try
        {
            result = Execute(pan, aadhaar, false);

            // 401 usually means a stale cached token - refresh once and retry.
            if (!result.Success && result.ErrorMessage != null &&
                (result.ErrorMessage.Contains("401") ||
                 result.ErrorMessage.IndexOf("Unauthorized", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                SandboxAuth.InvalidateToken();
                result = Execute(pan, aadhaar, true);
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    private static string BuildPayload(string pan, string aadhaar, JavaScriptSerializer js)
    {
        var payload = new Dictionary<string, object>
        {
            { "@entity", "in.co.sandbox.kyc.pan_aadhaar.status" },
            { "pan", pan },
            { "aadhaar_number", aadhaar },
            { "consent", "y" },
            { "reason", SandboxAuth.PanReason }
        };
        return js.Serialize(payload);
    }

    private static PanAadhaarLinkResult Simulate(string pan, string aadhaar)
    {
        var result = new PanAadhaarLinkResult();
        var js = new JavaScriptSerializer();
        var sw = Stopwatch.StartNew();

        string url = "[SIMULATED] " + SandboxAuth.BaseUrl + "/kyc/pan-aadhaar/status";
        string requestJson = BuildPayload(pan, aadhaar, js);

        var envelope = new Dictionary<string, object>
        {
            { "code", 200 },
            { "timestamp", DateTime.UtcNow.Ticks },
            { "transaction_id", "SIM-" + Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper() },
            { "data", new Dictionary<string, object>
                {
                    { "@entity", "in.co.sandbox.kyc.pan_aadhaar.status.response" },
                    { "aadhaar_seeding_status", "y" },
                    { "message", "SIMULATED RESPONSE - Your PAN is linked to Aadhaar Number XXXX XXXX " +
                                 aadhaar.Substring(aadhaar.Length - 4) }
                }
            }
        };

        string responseJson = js.Serialize(envelope);

        result.RequestJson = requestJson;
        result.RawResponse = responseJson;
        result.HttpStatusCode = 200;

        // Parsed through the same path as a live response, so a parsing bug shows
        // up in simulation rather than at go-live.
        ParseSuccess(responseJson, js, result);

        sw.Stop();
        result.DurationMs = sw.ElapsedMilliseconds;

        SandboxApiLog.Write(
            "PAN_AADHAAR_LINK_SIMULATED", url, "POST",
            "(simulated - no request sent)", requestJson,
            result.HttpStatusCode, "(simulated)", responseJson,
            result.Success, result.ErrorMessage, result.DurationMs);

        return result;
    }

    private static PanAadhaarLinkResult Execute(string pan, string aadhaar, bool forceTokenRefresh)
    {
        var result = new PanAadhaarLinkResult();
        var js = new JavaScriptSerializer();

        string url = SandboxAuth.BaseUrl + "/kyc/pan-aadhaar/status";
        string jsonPayload = "";
        string logHeaderText = "";
        string responseHeaders = "";
        var sw = Stopwatch.StartNew();

        try
        {
            SandboxAuth.EnsureTls();

            string accessToken = SandboxAuth.GetAccessToken(forceTokenRefresh);

            jsonPayload = BuildPayload(pan, aadhaar, js);
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
            request.Headers.Add("Authorization", accessToken);   // raw token, no "Bearer"
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

            // transaction_id is carried inside the logged response body.
            SandboxApiLog.Write(
                "PAN_AADHAAR_LINK", url, "POST",
                logHeaderText, jsonPayload,
                result.HttpStatusCode, responseHeaders, result.RawResponse,
                result.Success, result.ErrorMessage, result.DurationMs);
        }

        return result;
    }

    private static void ParseSuccess(string res, JavaScriptSerializer js, PanAadhaarLinkResult result)
    {
        Dictionary<string, object> obj = null;
        try { obj = js.Deserialize<Dictionary<string, object>>(res); }
        catch { }

        if (obj == null)
        {
            result.Success = false;
            result.ErrorMessage = "Empty or unreadable response from Sandbox API.";
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
        result.SeedingStatus = GetStr(data, "aadhaar_seeding_status");
        result.Message = GetStr(data, "message");
        result.Linked = string.Equals(result.SeedingStatus, "y", StringComparison.OrdinalIgnoreCase);
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
}
