using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

/// <summary>
/// Handles Sandbox.co.in authentication and shared HTTP concerns.
/// Every /authenticate call is logged to SandboxApiLog.
/// </summary>
public static class SandboxAuth
{
    private static readonly string apiKey = ConfigurationManager.AppSettings["SandboxApiKey"];
    private static readonly string apiSecret = ConfigurationManager.AppSettings["SandboxApiSecret"];
    private static readonly string apiVersion = ConfigurationManager.AppSettings["SandboxApiVersion"];
    private static readonly string baseUrl =
        (ConfigurationManager.AppSettings["SandboxBaseUrl"] ?? "https://api.sandbox.co.in").TrimEnd('/');

    // Sandbox access tokens are valid for ~24 hours. Cache instead of hitting
    // /authenticate before every single verification call.
    private static string cachedToken;
    private static DateTime cachedTokenExpiry = DateTime.MinValue;
    private static readonly object tokenLock = new object();

    public static string ApiKey { get { return apiKey; } }
    public static string ApiVersion { get { return apiVersion; } }
    public static string BaseUrl { get { return baseUrl; } }

    public static int TimeoutMs
    {
        get
        {
            int ms;
            return int.TryParse(ConfigurationManager.AppSettings["SandboxTimeoutMs"], out ms) ? ms : 30000;
        }
    }

    /// <summary>
    /// Enables TLS 1.2 / 1.1. Older .NET Framework targets default to SSL3/TLS1.0,
    /// which Sandbox rejects at the handshake stage with
    /// "Could not create SSL/TLS secure channel".
    /// </summary>
    public static void EnsureTls()
    {
        try
        {
            ServicePointManager.SecurityProtocol =
                (SecurityProtocolType)3072 |   // Tls12
                (SecurityProtocolType)768;     // Tls11
        }
        catch { /* older framework - ignore */ }
    }

    public static string GetAccessToken(bool forceRefresh = false)
    {
        lock (tokenLock)
        {
            if (!forceRefresh && !string.IsNullOrEmpty(cachedToken) && DateTime.UtcNow < cachedTokenExpiry)
                return cachedToken;

            EnsureTls();

            string url = baseUrl + "/authenticate";

            var logHeaders = new Dictionary<string, string>
            {
                { "x-api-key", apiKey },          // masked by SandboxApiLog before storage
                { "x-api-secret", apiSecret },    // masked by SandboxApiLog before storage
                { "x-api-version", apiVersion },
                { "Content-Type", "application/json" },
                { "Accept", "application/json" }
            };

            var sw = Stopwatch.StartNew();
            int? statusCode = null;
            string responseBody = "";
            string responseHeaders = "";
            bool ok = false;
            string errMsg = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Headers.Add("x-api-key", apiKey);
                request.Headers.Add("x-api-secret", apiSecret);
                request.Headers.Add("x-api-version", apiVersion);
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Timeout = TimeoutMs;
                request.ContentLength = 0;

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    statusCode = (int)response.StatusCode;
                    responseHeaders = SandboxApiLog.FormatHeaders(response.Headers);
                    responseBody = reader.ReadToEnd();

                    var js = new JavaScriptSerializer();
                    var obj = js.Deserialize<Dictionary<string, object>>(responseBody);

                    if (obj == null || !obj.ContainsKey("access_token") || obj["access_token"] == null)
                    {
                        errMsg = "access_token missing in response.";
                        throw new Exception("Sandbox Auth: " + errMsg);
                    }

                    cachedToken = obj["access_token"].ToString();
                    cachedTokenExpiry = DateTime.UtcNow.AddHours(23);   // safety margin
                    ok = true;
                    return cachedToken;
                }
            }
            catch (WebException wex)
            {
                var httpRes = wex.Response as HttpWebResponse;
                if (httpRes != null)
                {
                    statusCode = (int)httpRes.StatusCode;
                    responseHeaders = SandboxApiLog.FormatHeaders(httpRes.Headers);
                }
                responseBody = ReadWebExceptionBody(wex);
                errMsg = responseBody;
                throw new Exception("Sandbox Auth Error: " + responseBody);
            }
            catch (Exception ex)
            {
                if (errMsg == null) errMsg = ex.Message;
                throw;
            }
            finally
            {
                sw.Stop();
                SandboxApiLog.Write(
                    "AUTHENTICATE", url, "POST",
                    SandboxApiLog.FormatHeaders(logHeaders), "",
                    statusCode, responseHeaders, responseBody,
                    ok, errMsg, sw.ElapsedMilliseconds);
            }
        }
    }
    /// <summary>
    /// Local development switch. When true no HTTP request is made at all and a
    /// response is synthesised from the caller's own input, so the end-to-end flow
    /// can be exercised without live credentials or API credits.
    /// Must be false in production.
    /// </summary>
    public static bool SimulateMode
    {
        get
        {
            bool b;
            return bool.TryParse(ConfigurationManager.AppSettings["SandboxSimulateMode"], out b) && b;
        }
    }
    /// <summary>
    /// On test-api this must match the saved example verbatim.
    /// On the live API it is free text sent to the ITD as the stated purpose.
    /// </summary>
    public static string PanReason
    {
        get
        {
            string r = ConfigurationManager.AppSettings["SandboxPanReason"];
            return string.IsNullOrEmpty(r) ? "For onboarding customers" : r;
        }
    }

    /// <summary>
    /// When true, the documented sample PAN / name / DOB are sent instead of the
    /// member's real values, so the mock server returns its canned success response.
    /// MUST be false in production.
    /// </summary>
    public static bool UseTestExample
    {
        get
        {
            bool b;
            return bool.TryParse(ConfigurationManager.AppSettings["SandboxUseTestExample"], out b) && b;
        }
    }
    /// <summary>
    /// Safely reads an error body. wex.Response is NULL on timeout / DNS failure,
    /// so calling GetResponseStream() directly would throw NullReferenceException
    /// and hide the real problem.
    /// </summary>
    public static string ReadWebExceptionBody(WebException wex)
    {
        if (wex == null) return "Unknown error.";
        if (wex.Response == null)
            return wex.Status + " - " + wex.Message;

        try
        {
            using (var stream = wex.Response.GetResponseStream())
            {
                if (stream == null) return wex.Message;
                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }
        catch
        {
            return wex.Message;
        }
    }

    public static void InvalidateToken()
    {
        lock (tokenLock)
        {
            cachedToken = null;
            cachedTokenExpiry = DateTime.MinValue;
        }
    }
}