using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

[Serializable]
public class AadhaarOtpResult
{
    public bool Success { get; set; }
    public string RefId { get; set; }          // required for the verify step
    public string Message { get; set; }
    public string TransactionId { get; set; }
    public string RequestJson { get; set; }
    public string RawResponse { get; set; }
    public int? HttpStatusCode { get; set; }
    public long DurationMs { get; set; }
    public string ErrorMessage { get; set; }

    /// <summary>
    /// True when the failure was ours - no credits, bad keys, provider down -
    /// rather than the member's. Drives a different message and keeps the OTP
    /// reference alive so a retry costs nothing extra once our side is fixed.
    /// </summary>
    public bool IsServiceError { get; set; }
}

[Serializable]
public class AadhaarVerifyResult
{
    public bool Success { get; set; }
    public string Status { get; set; }         // VALID / INVALID
    public string Message { get; set; }
    public string Name { get; set; }
    public string Dob { get; set; }            // dd-MM-yyyy as returned
    public string YearOfBirth { get; set; }
    public string Gender { get; set; }         // M / F
    public string CareOf { get; set; }
    public string FullAddress { get; set; }

    // Address components
    public string House { get; set; }
    public string Street { get; set; }
    public string Landmark { get; set; }
    public string Vtc { get; set; }            // village / town / city
    public string SubDistrict { get; set; }
    public string PostOffice { get; set; }
    public string District { get; set; }
    public string State { get; set; }
    public string Pincode { get; set; }
    public string Country { get; set; }

    public string PhotoBase64 { get; set; }
    public string TransactionId { get; set; }
    public string RequestJson { get; set; }
    public string RawResponse { get; set; }
    public int? HttpStatusCode { get; set; }
    public long DurationMs { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsServiceError { get; set; }
}

public static class SandboxAadhaarVerify
{
    private static readonly Regex AadhaarRegex = new Regex("^[2-9][0-9]{11}$", RegexOptions.Compiled);

    #region Format, checksum, masking, hashing

    /// <summary>
    /// Structural check only. A real Aadhaar never starts with 0 or 1 and carries
    /// a Verhoeff checksum, both validated here so a mistyped number never reaches
    /// the paid API.
    /// </summary>
    public static bool IsValidAadhaarFormat(string aadhaar)
    {
        if (string.IsNullOrEmpty(aadhaar)) return false;
        string a = Clean(aadhaar);
        return AadhaarRegex.IsMatch(a) && VerhoeffCheck(a);
    }

    public static string Clean(string aadhaar)
    {
        if (string.IsNullOrEmpty(aadhaar)) return "";
        return aadhaar.Trim().Replace(" ", "").Replace("-", "");
    }

    private static readonly int[,] d = {
        {0,1,2,3,4,5,6,7,8,9},{1,2,3,4,0,6,7,8,9,5},{2,3,4,0,1,7,8,9,5,6},
        {3,4,0,1,2,8,9,5,6,7},{4,0,1,2,3,9,5,6,7,8},{5,9,8,7,6,0,4,3,2,1},
        {6,5,9,8,7,1,0,4,3,2},{7,6,5,9,8,2,1,0,4,3},{8,7,6,5,9,3,2,1,0,4},
        {9,8,7,6,5,4,3,2,1,0}
    };

    private static readonly int[,] p = {
        {0,1,2,3,4,5,6,7,8,9},{1,5,7,6,2,8,3,0,9,4},{5,8,0,3,7,9,6,1,4,2},
        {8,9,1,6,0,4,3,5,2,7},{9,4,5,3,1,2,6,8,7,0},{4,2,8,6,5,7,3,9,0,1},
        {2,7,9,3,8,0,6,4,1,5},{7,0,4,6,9,1,3,2,5,8}
    };

    /// <summary>
    /// UIDAI uses the Verhoeff algorithm for the 12th digit. This catches typos
    /// that a plain length check would let through.
    /// </summary>
    private static bool VerhoeffCheck(string num)
    {
        try
        {
            int c = 0;
            char[] reversed = num.ToCharArray();
            Array.Reverse(reversed);

            for (int i = 0; i < reversed.Length; i++)
                c = d[c, p[i % 8, (int)char.GetNumericValue(reversed[i])]];

            return c == 0;
        }
        catch { return false; }
    }

    /// <summary>
    /// Returns XXXXXXXX1234 - the only form permitted for display and storage.
    /// </summary>
    public static string MaskAadhaar(string aadhaar)
    {
        string a = Clean(aadhaar);
        return a.Length < 4 ? "XXXXXXXXXXXX" : "XXXXXXXX" + a.Substring(a.Length - 4);
    }

    /// <summary>
    /// SHA-256 of the raw number. Enables duplicate detection without retaining
    /// the number itself, which is restricted under the Aadhaar Act.
    /// </summary>
    public static string HashAadhaar(string aadhaar)
    {
        string a = Clean(aadhaar);
        if (a.Length == 0) return "";

        using (var sha = SHA256.Create())
        {
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(a));
            var sb = new StringBuilder();
            foreach (byte b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    #endregion

    #region Error classification

    /// <summary>
    /// Separates our problems from the member's. Someone who typed everything
    /// correctly should never be told their Aadhaar failed because our wallet
    /// ran dry or UIDAI was down.
    /// </summary>
    private class ClassifiedError
    {
        public string Message;
        public bool IsServiceError;
    }

    private static ClassifiedError Classify(int? status, string body, string parsedMessage)
    {
        var c = new ClassifiedError { Message = parsedMessage, IsServiceError = false };
        string b = body ?? "";

        // Billing and entitlement: ours, always.
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

        // Authentication: bad or expired keys, ours.
        if (status == 401 || status == 403)
        {
            c.IsServiceError = true;
            c.Message = "Verification service is not available right now. Please contact support.";
            c.Message += DevDetail(parsedMessage);
            return c;
        }

        // Upstream outage at UIDAI.
        if (status == 502 || status == 503 || status == 504)
        {
            c.IsServiceError = true;
            c.Message = "The UIDAI service is temporarily unavailable. Please try again in a few minutes.";
            return c;
        }

        if (status == 429)
        {
            c.IsServiceError = true;
            c.Message = "Too many requests. Please wait a minute and try again.";
            return c;
        }

        // 400/422 are genuine input problems, so the provider's own wording is
        // more useful to the member than anything generic.
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

    // ===================== STEP 1: Generate OTP =====================

    public static AadhaarOtpResult GenerateOtp(string aadhaarNumber)
    {
        var result = new AadhaarOtpResult();
        var js = new JavaScriptSerializer();

        string aadhaar = Clean(aadhaarNumber);

        if (!IsValidAadhaarFormat(aadhaar))
        {
            result.Success = false;
            result.ErrorMessage = "Invalid Aadhaar number.";
            return result;
        }

        string url = SandboxAuth.BaseUrl + "/kyc/aadhaar/okyc/otp";
        string jsonPayload = "";
        string logHeaderText = "";
        string responseHeaders = "";
        var sw = Stopwatch.StartNew();

        try
        {
            SandboxAuth.EnsureTls();
            string accessToken = SandboxAuth.GetAccessToken();

            var payload = new Dictionary<string, object>
            {
                { "@entity", "in.co.sandbox.kyc.aadhaar.okyc.otp.request" },   // REQUIRED
                { "aadhaar_number", aadhaar },
                { "consent", "y" },
                { "reason", "For KYC" }
            };

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

                var obj = js.Deserialize<Dictionary<string, object>>(res);
                result.TransactionId = GetStr(obj, "transaction_id");

                var data = obj != null && obj.ContainsKey("data")
                    ? obj["data"] as Dictionary<string, object> : null;

                if (data == null)
                {
                    result.Success = false;
                    result.ErrorMessage = ExtractApiMessage(res, js);
                }
                else
                {
                    // reference_id arrives as a JSON number, not a string, so it is
                    // read through GetStr rather than cast directly.
                    result.RefId = GetStr(data, "reference_id");
                    result.Message = GetStr(data, "message");
                    result.Success = !string.IsNullOrEmpty(result.RefId);

                    if (!result.Success)
                        result.ErrorMessage = "Reference id missing in response.";
                }
            }
        }
        catch (WebException wex)
        {
            // Non-2xx responses arrive here by design: HttpWebRequest throws rather
            // than returning. The status code and body are still readable.
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
                "AADHAAR_OTP_GENERATE", url, "POST",
                logHeaderText, jsonPayload,
                result.HttpStatusCode, responseHeaders, result.RawResponse,
                result.Success, result.ErrorMessage, result.DurationMs);
        }

        return result;
    }

    // ===================== STEP 2: Verify OTP =====================

    public static AadhaarVerifyResult VerifyOtp(string refId, string otp)
    {
        var result = new AadhaarVerifyResult();
        var js = new JavaScriptSerializer();

        if (string.IsNullOrEmpty(refId))
        {
            result.Success = false;
            result.ErrorMessage = "Reference id missing. Please request a new OTP.";
            return result;
        }

        if (string.IsNullOrEmpty(otp) || otp.Trim().Length < 4)
        {
            result.Success = false;
            result.ErrorMessage = "Please enter the OTP.";
            return result;
        }

        string url = SandboxAuth.BaseUrl + "/kyc/aadhaar/okyc/otp/verify";
        string jsonPayload = "";
        string logHeaderText = "";
        string responseHeaders = "";
        var sw = Stopwatch.StartNew();

        try
        {
            SandboxAuth.EnsureTls();
            string accessToken = SandboxAuth.GetAccessToken();

            var payload = new Dictionary<string, object>
            {
                // Note this entity differs from the OTP-generate one.
                { "@entity", "in.co.sandbox.kyc.aadhaar.okyc.request" },
                { "reference_id", refId.Trim() },
                { "otp", otp.Trim() }
            };

            jsonPayload = js.Serialize(payload);
            result.RequestJson = jsonPayload;

            var logHeaders = new Dictionary<string, string>
            {
                { "Authorization", accessToken },
                { "x-api-key", SandboxAuth.ApiKey },
                { "Content-Type", "application/json" },
                { "Accept", "application/json" }
            };
            logHeaderText = SandboxApiLog.FormatHeaders(logHeaders);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Headers.Add("Authorization", accessToken);
            request.Headers.Add("x-api-key", SandboxAuth.ApiKey);
            // x-api-version is not listed for this endpoint in the API reference,
            // so the request is kept identical to the documented curl sample.
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
                "AADHAAR_OTP_VERIFY", url, "POST",
                logHeaderText, jsonPayload,
                result.HttpStatusCode, responseHeaders, result.RawResponse,
                result.Success, result.ErrorMessage, result.DurationMs);
        }

        return result;
    }

    private static void ParseVerifyResponse(string res, JavaScriptSerializer js, AadhaarVerifyResult result)
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

        result.Status = GetStr(data, "status");
        result.Message = GetStr(data, "message");
        result.Name = GetStr(data, "name");
        result.Dob = GetStr(data, "date_of_birth");
        result.YearOfBirth = GetStr(data, "year_of_birth");
        result.Gender = GetStr(data, "gender");
        result.CareOf = GetStr(data, "care_of");
        result.PhotoBase64 = GetStr(data, "photo");

        // UIDAI pads full_address with a trailing space and sometimes doubles the
        // separators, so it is normalised before anything downstream stores it.
        result.FullAddress = NormaliseAddress(GetStr(data, "full_address"));

        // "address" is a nested object, not a string. Calling ToString() on it
        // yields the dictionary type name, which is how garbage ends up on screen.
        var addr = data.ContainsKey("address") ? data["address"] as Dictionary<string, object> : null;
        if (addr != null)
        {
            result.House = GetStr(addr, "house");
            result.Street = GetStr(addr, "street");
            result.Landmark = GetStr(addr, "landmark");
            result.Vtc = GetStr(addr, "vtc");
            result.SubDistrict = GetStr(addr, "subdistrict");
            result.PostOffice = GetStr(addr, "post_office");
            result.District = GetStr(addr, "district");
            result.State = GetStr(addr, "state");
            result.Pincode = GetStr(addr, "pincode");
            result.Country = GetStr(addr, "country");

            // subdistrict is frequently blank while post_office carries the same
            // information, so it is used as the tehsil fallback.
            if (string.IsNullOrEmpty(result.SubDistrict))
                result.SubDistrict = result.PostOffice;
        }

        // Compose the address when full_address is absent from the response.
        if (string.IsNullOrEmpty(result.FullAddress) && addr != null)
        {
            var parts = new List<string>();
            foreach (string s in new[] { result.House, result.Street, result.Landmark,
                                         result.Vtc, result.SubDistrict, result.District,
                                         result.State, result.Pincode })
            {
                if (!string.IsNullOrEmpty(s)) parts.Add(s.Trim());
            }
            result.FullAddress = string.Join(", ", parts.ToArray());
        }

        result.Success = string.Equals(result.Status, "VALID", StringComparison.OrdinalIgnoreCase);

        if (!result.Success && string.IsNullOrEmpty(result.ErrorMessage))
            result.ErrorMessage = string.IsNullOrEmpty(result.Message)
                ? "Aadhaar could not be verified." : result.Message;
    }

    /// <summary>
    /// UIDAI addresses arrive with trailing spaces, repeated separators and empty
    /// segments left behind by blank fields. Cleaning them here keeps the stored
    /// value tidy rather than pushing the problem into every consumer.
    /// </summary>
    private static string NormaliseAddress(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";

        s = s.Trim();
        s = Regex.Replace(s, @"\s+", " ");         // collapse runs of whitespace
        s = Regex.Replace(s, @"(,\s*){2,}", ", "); // collapse ", , ," into ", "
        s = s.Trim().TrimEnd(',').Trim();

        return s;
    }

    #region Parsing helpers

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

    #endregion
}