using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

/// <summary>
/// Sandbox DigiLocker: session init, user profile, aur e-Aadhaar document (XML) -
/// address XML se aata hai (profile me address nahi hota).
/// Keys: Sendbox_* (agar hon), warna PAN/Bank KYC wali Sandbox* keys.
/// </summary>
public static class SandboxDigiLocker
{
    // Host SandboxBaseUrl se (PAN/Bank KYC wala hi) - test keys sirf test-api par chalti hain.
    private static string InitUrl { get { return SandboxAuth.BaseUrl + "/kyc/digilocker/sessions/init"; } }
    private static string ProfileUrl { get { return SandboxAuth.BaseUrl + "/kyc/digilocker/sessions/{sid}/user/profile"; } }
    private static string DocUrl { get { return SandboxAuth.BaseUrl + "/kyc/digilocker/sessions/{sid}/documents/aadhaar"; } }

    private static readonly HttpClient Http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };

    static SandboxDigiLocker()
    {
        ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072; // TLS 1.2
       //  ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
    }

    // Epayindia: web.config me purani Sendbox_* keys alag account ki hain, isliye pehle
    // PAN/Bank KYC wali Sandbox* keys (key + secret ek hi account ke). Na hon tabhi Sendbox_*.
    private static string ApiKey { get { return Cfg("SandboxApiKey") ?? Cfg("Sendbox_xapikey"); } }
    private static string ApiSecret { get { return Cfg("SandboxApiSecret") ?? Cfg("Sendbox_xapisecret"); } }
    private static string AuthUrl { get { return SandboxAuth.BaseUrl + "/authenticate"; } }

    private static string Cfg(string key)
    {
        string v = ConfigurationManager.AppSettings[key];
        return string.IsNullOrWhiteSpace(v) ? null : v.Trim();
    }

    private class Resp { public int Http; public JObject Json; public string Raw; }

    /// <summary>
    /// Ek HTTP call ka request/response, log ke liye. Secret, token aur authorization_url
    /// pehle hi hata diye jaate hain. Exception pe bhi entry banti hai (Error me).
    /// </summary>
    public class ApiCall
    {
        public string Step, Method, Url, RequestBody, ResponseBody, Error;
        public int? HttpStatus;
        public int DurationMs;
    }

    // ---------------- Public (sync) API ----------------

    /// <summary>DigiLocker session banata hai. authUrl me redirect link aata hai.</summary>
    public static AadhaarOtpResult CreateSession(string redirectUrl, out string authUrl, List<ApiCall> calls = null)
    {
        var res = new AadhaarOtpResult();
        authUrl = null;
        try
        {
            authUrl = Task.Run(() => CreateSessionAsync(redirectUrl, res, calls)).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            res.Success = false;
            res.IsServiceError = true;
            res.ErrorMessage = (ex is TaskCanceledException)
                ? "DigiLocker service timed out. Please try again."
                : "Could not start DigiLocker. Please try again.";
            Trace.TraceError("DL CreateSession: " + ex);
        }
        return res;
    }

    /// <summary>
    /// Profile (name, DOB, gender) + e-Aadhaar XML (address, care-of, masked uid).
    /// maskedUid me xxxxxxxx1234 aata hai - caller PAN wale Aadhaar se last 4 match kare.
    /// </summary>
    public static AadhaarVerifyResult FetchAadhaar(string sessionId, out string maskedUid, List<ApiCall> calls = null)
    {
        var v = new AadhaarVerifyResult();
        var h = new Holder();
        try
        {
            Task.Run(() => FetchAsync(sessionId, v, h, calls)).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            v.Success = false;
            v.IsServiceError = true;
            v.ErrorMessage = (ex is TaskCanceledException)
                ? "DigiLocker service timed out. Please try again."
                : "Could not read your Aadhaar from DigiLocker. Please try again.";
            Trace.TraceError("DL FetchAadhaar: " + ex);
        }
        maskedUid = h.MaskedUid;
        return v;
    }

    private class Holder { public string MaskedUid; }

    // ---------------- Internals ----------------

    /// <summary>Entry pehle hi list me jaati hai, taaki timeout/exception wali call bhi log ho.</summary>
    private static ApiCall Track(List<ApiCall> calls, string step, HttpMethod m, string url, string body)
    {
        var c = new ApiCall { Step = step, Method = m.Method, Url = url, RequestBody = body };
        if (calls != null) lock (calls) calls.Add(c);
        return c;
    }

    private static string MaskKey(string k)
    {
        if (string.IsNullOrEmpty(k)) return "(empty)";
        return k.Length <= 4 ? "XXXX" : new string('X', k.Length - 4) + k.Substring(k.Length - 4);
    }

    private static async Task<string> AuthAsync(string step, List<ApiCall> calls)
    {
        var sw = Stopwatch.StartNew();
        var log = Track(calls, step, HttpMethod.Post, AuthUrl,
                        "headers: x-api-key=" + MaskKey(ApiKey) + ", x-api-secret=[REMOVED] | body: {}");
        try
        {
            var req = new HttpRequestMessage(HttpMethod.Post, AuthUrl);
            req.Headers.Add("x-api-key", ApiKey);
            req.Headers.Add("x-api-secret", ApiSecret);
            if (!string.IsNullOrEmpty(SandboxAuth.ApiVersion)) req.Headers.Add("x-api-version", SandboxAuth.ApiVersion);
            req.Content = new StringContent("{}", null, "application/json");

            var resp = await Http.SendAsync(req).ConfigureAwait(false);
            string json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            log.HttpStatus = (int)resp.StatusCode;
            log.ResponseBody = Sanitize(json);

            if (!resp.IsSuccessStatusCode) return "";
            var o = JObject.Parse(json);
            return (string)o["access_token"] ?? "";
        }
        catch (Exception ex)
        {
            log.Error = ex.GetType().Name + ": " + ex.Message;
            throw;
        }
        finally
        {
            log.DurationMs = (int)sw.ElapsedMilliseconds;
        }
    }

    private static async Task<Resp> CallAsync(HttpMethod m, string url, string token, string body,
                                              string step, List<ApiCall> calls)
    {
        var sw = Stopwatch.StartNew();
        var log = Track(calls, step, m, url, body);
        try
        {
            var req = new HttpRequestMessage(m, url);
            req.Headers.Add("Authorization", token);
            req.Headers.Add("x-api-key", ApiKey);
            if (!string.IsNullOrEmpty(SandboxAuth.ApiVersion)) req.Headers.Add("x-api-version", SandboxAuth.ApiVersion);
            if (body != null) req.Content = new StringContent(body, null, "application/json");

            var resp = await Http.SendAsync(req).ConfigureAwait(false);
            string raw = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            log.HttpStatus = (int)resp.StatusCode;
            log.ResponseBody = Sanitize(raw);

            JObject obj = null;
            try { obj = JObject.Parse(raw); } catch { }
            return new Resp { Http = (int)resp.StatusCode, Json = obj, Raw = raw };
        }
        catch (Exception ex)
        {
            log.Error = ex.GetType().Name + ": " + ex.Message;
            throw;
        }
        finally
        {
            log.DurationMs = (int)sw.ElapsedMilliseconds;
        }
    }

    private static async Task<string> CreateSessionAsync(string redirectUrl, AadhaarOtpResult res, List<ApiCall> calls)
    {
        var sw = Stopwatch.StartNew();

        string token = await AuthAsync("INIT_AUTH", calls).ConfigureAwait(false);
        if (string.IsNullOrEmpty(token))
        {
            res.Success = false;
            res.IsServiceError = true;
            res.ErrorMessage = "Service authentication failed. Please try again.";
            return null;
        }

        // C# 5 compatible (Epayindia web.config me Roslyn codedom nahi hai)
        var options = new JObject();
        options.Add("verification_method", new JArray("aadhaar"));
        options.Add("pinless", true);
        options.Add("usernameless", true);

        var body = new JObject();
        body.Add("@entity", "in.co.sandbox.kyc.digilocker.session.request");
        body.Add("flow", "signin");
        body.Add("redirect_url", redirectUrl);
        body.Add("doc_types", new JArray("aadhaar"));
        body.Add("options", options);
        string bodyStr = body.ToString(Newtonsoft.Json.Formatting.None);

        var r = await CallAsync(HttpMethod.Post, InitUrl, token, bodyStr, "INIT_SESSION", calls).ConfigureAwait(false);

        res.RequestJson = bodyStr;
        res.RawResponse = Sanitize(r.Raw);
        res.HttpStatusCode = r.Http;
        res.DurationMs = (int)sw.ElapsedMilliseconds;
        res.TransactionId = (string)(r.Json == null ? null : r.Json["transaction_id"]);

        if (r.Json == null || (int?)r.Json["code"] != 200 || r.Json["data"] == null)
        {
            res.Success = false;
            res.IsServiceError = r.Http >= 500;
            res.ErrorMessage = r.Http >= 500
                ? "DigiLocker service is temporarily unavailable. Please try again."
                : ((string)(r.Json == null ? null : r.Json["message"]) ?? "Unable to start DigiLocker session.");
            return null;
        }

        string sid = (string)r.Json["data"]["session_id"];
        string url = (string)r.Json["data"]["authorization_url"];
        if (string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(url))
        {
            res.Success = false;
            res.ErrorMessage = "Invalid response from DigiLocker service.";
            return null;
        }

        res.Success = true;
        res.RefId = sid;
        return url;
    }

    private static async Task FetchAsync(string sid, AadhaarVerifyResult v, Holder h, List<ApiCall> calls)
    {
        var sw = Stopwatch.StartNew();
        v.Success = false;

        string token = await AuthAsync("FETCH_AUTH", calls).ConfigureAwait(false);
        if (string.IsNullOrEmpty(token))
        {
            v.IsServiceError = true;
            v.ErrorMessage = "Service authentication failed. Please try again.";
            return;
        }

        string url = ProfileUrl.Replace("{sid}", sid);
        var p = await CallAsync(HttpMethod.Get, url, token, null, "FETCH_PROFILE", calls).ConfigureAwait(false);

        v.RequestJson = "GET " + url;
        v.RawResponse = Sanitize(p.Raw);
        v.HttpStatusCode = p.Http;
        v.DurationMs = (int)sw.ElapsedMilliseconds;
        v.TransactionId = (string)(p.Json == null ? null : p.Json["transaction_id"]);

        if (p.Json == null || (int?)p.Json["code"] != 200 || p.Json["data"] == null)
        {
            // Member ne Deny/cancel kiya to Sandbox 523 "Invalid session status" deta hai - ye service down nahi hai.
            string pmsg = (string)(p.Json == null ? null : p.Json["message"]) ?? "";
            if (pmsg.IndexOf("session status", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                v.IsServiceError = false;
                v.ErrorMessage = "DigiLocker consent was not completed. Please try again and click Allow.";
                return;
            }

            v.IsServiceError = p.Http >= 500;
            v.ErrorMessage = p.Http >= 500
                ? "DigiLocker service is temporarily unavailable. Please try again."
                : ((string)(p.Json == null ? null : p.Json["message"]) ?? "DigiLocker verification was not completed. Please try again.");
            return;
        }

        var d = p.Json["data"];

        if ((bool?)d["eaadhaar"] != true)
        {
            v.ErrorMessage = "Aadhaar is not available in your DigiLocker account. Please link Aadhaar in DigiLocker and try again.";
            return;
        }

        v.Name = (string)d["name"] ?? "";

        string pg = ((string)d["gender"] ?? "").ToLowerInvariant();
        v.Gender = pg == "male" ? "M" : (pg == "female" ? "F" : (pg == "" ? "" : "T"));

        // DOB epoch ms me aata hai
        string dob = "";
        try
        {
            var tok = d["date_of_birth"];
            long ms;
            if (tok != null && tok.Type != JTokenType.Null &&
                long.TryParse(tok.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out ms) && ms != 0)
            {
                // IST me badlo, warna UTC midnight se pehle wali date ek din peeche aa sakti hai
                dob = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ms).AddHours(5.5)
                        .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }
        catch { }
        v.Dob = dob;

        // 2) e-Aadhaar document: files[].url = pre-signed XML link
        string docUrl = DocUrl.Replace("{sid}", sid);
        var doc = await CallAsync(HttpMethod.Get, docUrl, token, null, "FETCH_DOCUMENT", calls).ConfigureAwait(false);
        v.RequestJson = "GET " + url + " ; GET " + docUrl;
        v.RawResponse = Sanitize(p.Raw) + " | " + Sanitize(doc.Raw);
        v.HttpStatusCode = doc.Http;

        if (doc.Json == null || (int?)doc.Json["code"] != 200)
        {
            v.DurationMs = (int)sw.ElapsedMilliseconds;
            v.IsServiceError = doc.Http >= 500;
            v.ErrorMessage = doc.Http >= 500
                ? "DigiLocker service is temporarily unavailable. Please try again."
                : ((string)(doc.Json == null ? null : doc.Json["message"]) ?? "Unable to fetch your Aadhaar from DigiLocker.");
            return;
        }

        var files = doc.Json["data"] == null ? null : doc.Json["data"]["files"] as JArray;
        if (files == null || files.Count == 0)
        {
            v.DurationMs = (int)sw.ElapsedMilliseconds;
            v.ErrorMessage = "Your Aadhaar document is not available in DigiLocker.";
            return;
        }

        var file = files.FirstOrDefault(f => ((string)(f["metadata"] == null ? null : f["metadata"]["ContentType"]) ?? "").Contains("xml"))
                   ?? files[0];
        string fileUrl = (string)file["url"];
        if (string.IsNullOrEmpty(fileUrl))
        {
            v.DurationMs = (int)sw.ElapsedMilliseconds;
            v.ErrorMessage = "Your Aadhaar document link is missing. Please try again.";
            return;
        }

        // 3) XML download (koi custom header nahi). Log me link ka signature
        // (query string) aur XML ki photo nahi jaati.
        string xml;
        var xsw = Stopwatch.StartNew();
        var xlog = Track(calls, "FETCH_XML", HttpMethod.Get, fileUrl.Split('?')[0], null);
        try
        {
            var xresp = await Http.GetAsync(fileUrl).ConfigureAwait(false);
            xml = await xresp.Content.ReadAsStringAsync().ConfigureAwait(false);
            xlog.HttpStatus = (int)xresp.StatusCode;
            xlog.ResponseBody = StripPhoto(xml);
            xresp.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            xlog.Error = ex.GetType().Name + ": " + ex.Message;
            throw;
        }
        finally
        {
            xlog.DurationMs = (int)xsw.ElapsedMilliseconds;
        }

        var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null };
        XDocument xdoc;
        using (var sr = new StringReader(xml))
        using (var xr = XmlReader.Create(sr, settings))
        {
            xdoc = XDocument.Load(xr);
        }

        var uidData = Find(xdoc, "UidData");
        var poi = Find(xdoc, "Poi");
        var poa = Find(xdoc, "Poa");

        v.DurationMs = (int)sw.ElapsedMilliseconds;

        if (poa == null)
        {
            Trace.TraceError("DL: Poa not found. XML: " + StripPhoto(xml));
            v.ErrorMessage = "Could not read the address from your Aadhaar document.";
            return;
        }

        string house = Attr(poa, "house");
        string street = Attr(poa, "street");
        string landmark = Attr(poa, "lm", "landmark");
        string loc = Attr(poa, "loc");
        string vtc = Attr(poa, "vtc");
        string po = Attr(poa, "po");
        string subdist = Attr(poa, "subdist");
        string dist = Attr(poa, "dist");
        string state = Attr(poa, "state");
        string pin = Attr(poa, "pc");

        v.FullAddress = string.Join(", ",
            new[] { house, street, landmark, loc, vtc, po, subdist, dist, state, pin }
                .Where(x => !string.IsNullOrWhiteSpace(x)));
        v.Pincode = pin;
        v.District = dist;
        v.State = state;
        v.Vtc = !string.IsNullOrEmpty(vtc) ? vtc : loc;
        v.SubDistrict = subdist;
        v.CareOf = Attr(poa, "co", "careof");

        // Profile me na ho to XML se
        if (string.IsNullOrEmpty(v.Name)) v.Name = Attr(poi, "name");
        if (string.IsNullOrEmpty(v.Gender)) v.Gender = Attr(poi, "gender");
        if (string.IsNullOrEmpty(v.Dob)) v.Dob = Attr(poi, "dob");

        h.MaskedUid = Attr(uidData, "uid");   // xxxxxxxx1234
        v.Status = "VALID";
        v.Success = true;
    }

    private static string Attr(XElement e, params string[] names)
    {
        if (e == null) return "";
        foreach (var n in names)
        {
            var a = e.Attribute(n);
            if (a != null && !string.IsNullOrWhiteSpace(a.Value)) return a.Value.Trim();
        }
        return "";
    }

    private static XElement Find(XDocument doc, string localName)
    {
        return doc.Descendants().FirstOrDefault(x => x.Name.LocalName == localName);
    }

    private static string StripPhoto(string xml)
    {
        if (string.IsNullOrEmpty(xml)) return xml;
        return Regex.Replace(xml, "<Pht>.*?</Pht>", "<Pht/>", RegexOptions.Singleline);
    }

    /// <summary>Log me jaane se pehle token, authorization url hatata hai, mobile mask karta hai.</summary>
    private static string Sanitize(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return null;
        try
        {
            var tok = JToken.Parse(raw);
            var sensitive = new[] { "authorization_url", "url", "access_token", "code_verifier",
                                    "code_challenge", "photo", "pht" };
            // SelectTokens values deta hai, JProperty nahi - isliye Descendants() se properties lo
            var props = tok is JContainer
                ? ((JContainer)tok).Descendants().OfType<JProperty>().ToList()
                : new List<JProperty>();
            foreach (var prop in props)
            {
                string n = prop.Name.ToLowerInvariant();
                if (sensitive.Contains(n))
                    prop.Value = "[REMOVED]";
                else if (n == "mobile" && prop.Value.Type == JTokenType.String)
                {
                    string m = (string)prop.Value;
                    prop.Value = m.Length >= 4 ? new string('X', m.Length - 4) + m.Substring(m.Length - 4) : "XXXX";
                }
            }
            return tok.ToString(Newtonsoft.Json.Formatting.None);
        }
        catch
        {
            return raw.Length > 4000 ? raw.Substring(0, 4000) : raw;
        }
    }
}
