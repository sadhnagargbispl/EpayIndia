using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// The PAN-Aadhaar link check shared by PanKYC.aspx and Registration.aspx, plus
/// reading and writing the linked Aadhaar on the member record.
///
/// m_MemberMaster keeps the Aadhaar split across AadharNo / AadharNo2 / AAdharNo3,
/// four digits each - that is how the joining insert has always written it - so
/// every read and write here follows the same layout. A full number sitting in
/// AadharNo alone is also accepted on read, for older rows.
/// </summary>
public static class PanAadhaarLinkCheck
{
    private static string Cs
    {
        get { return ConfigurationManager.ConnectionStrings["constr"].ConnectionString; }
    }

    /// <summary>Result returned to the page's AJAX call as response.d.</summary>
    public class Response
    {
        public bool ok { get; set; }       // the check ran (a clean "not linked" is still ok)
        public bool linked { get; set; }
        public string msg { get; set; }
    }

    public static string CleanAadhaar(string aadhaar)
    {
        return Regex.Replace(aadhaar ?? "", "[^0-9]", "");
    }

    /// <summary>
    /// Runs every free check, then the paid link check. On success the PAN and
    /// Aadhaar are written to the two given Session keys, which are cleared first
    /// so a failed re-check can never leave an earlier success standing.
    /// </summary>
    /// <param name="formNo">0 during joining, when no member row exists yet.</param>
    /// <param name="isPanFree">The page's own PAN duplicate rule.</param>
    /// <param name="panAlreadyVerified">True blocks the call: nothing left to check.</param>
    public static Response Run(long formNo, string pan, string aadhaar, bool consent,
                               Func<string, bool> isPanFree, bool panAlreadyVerified,
                               string sessPanKey, string sessAadhaarKey)
    {
        HttpContext ctx = HttpContext.Current;

        pan = (pan ?? "").Trim().ToUpper();
        aadhaar = CleanAadhaar(aadhaar);

        // Checked before the Session keys are cleared: once the PAN is verified the
        // linked Aadhaar in Session is still needed (registration step 3 is locked
        // to it), and a stray call must not wipe it.
        if (panAlreadyVerified)
            return Fail(formNo, pan, aadhaar, consent, "PAN already verified",
                        "Your PAN is already verified.");

        ctx.Session[sessPanKey] = null;
        ctx.Session[sessAadhaarKey] = null;

        try
        {
            // ---------- Free checks: no API call unless all pass ----------
            if (!SandboxPanVerify.IsValidPanFormat(pan))
                return Fail(formNo, pan, aadhaar, consent, "Invalid PAN format",
                            "Invalid PAN format. Example: ABCDE1234F");

            // Regex plus the Verhoeff checksum: a mistyped number is caught here
            // instead of costing a paid call, and Aadhaar OKYC applies the same rule
            // when this number is reused for the OTP.
            if (!SandboxAadhaarVerify.IsValidAadhaarFormat(aadhaar))
                return Fail(formNo, pan, aadhaar, consent, "Invalid Aadhaar format or checksum",
                            "Please enter a valid 12-digit Aadhaar number.");

            if (!consent)
                return Fail(formNo, pan, aadhaar, consent, "Consent not given",
                            "Please give consent to verify your PAN details.");

            if (!isPanFree(pan))
                return Fail(formNo, pan, aadhaar, consent, "PAN already registered with another ID",
                            "This PAN is already registered with another ID.");

            if (!IsAadhaarFree(aadhaar, formNo))
                return Fail(formNo, pan, aadhaar, consent, "Aadhaar already registered with another ID",
                            "This Aadhaar is already registered with another ID.");

            // ---------- Paid call ----------
            PanAadhaarLinkResult r = SandboxPanAadhaarLink.CheckLink(pan, aadhaar);

            if (!r.Success)
                return Fail(formNo, pan, aadhaar, consent, "API call failed: " + r.ErrorMessage,
                            "Could not check PAN-Aadhaar link: " + r.ErrorMessage, r);

            if (!r.Linked)
                return Fail(formNo, pan, aadhaar, consent,
                            "Not linked (seeding status: " + r.SeedingStatus + ") " + r.Message,
                            "Your PAN is not linked with this Aadhaar number. Please link your " +
                            "PAN with Aadhaar first, then try again.", r);

            ctx.Session[sessPanKey] = pan;
            ctx.Session[sessAadhaarKey] = aadhaar;

            WriteAttempt(formNo, pan, aadhaar, consent, "LINKED", null, r);
            return new Response { ok = true, linked = true, msg = "PAN is linked with Aadhaar. You can now verify your PAN." };
        }
        catch (Exception ex)
        {
            return Fail(formNo, pan, aadhaar, consent, "Exception: " + ex.Message,
                        "Could not check PAN-Aadhaar link: " + ex.Message);
        }
    }

    private static Response Fail(long formNo, string pan, string aadhaar, bool consent,
                                 string reason, string userMsg, PanAadhaarLinkResult r = null)
    {
        WriteAttempt(formNo, pan, aadhaar, consent, "FAILED", reason, r);
        return new Response { ok = r != null && r.Success, linked = false, msg = userMsg };
    }

    /// <summary>Attempt row for a link check, KycType PAN, Stage AADHAAR_LINK.</summary>
    public static void WriteAttempt(long formNo, string pan, string aadhaar, bool consent,
                                    string finalResult, string reason, PanAadhaarLinkResult r)
    {
        try
        {
            SandboxApiLog.WriteAttempt(new SandboxApiLog.Attempt
            {
                FormNo = formNo,
                KycType = "PAN",
                Stage = "AADHAAR_LINK",
                FinalResult = finalResult,
                FailReason = reason,

                RefNo = pan,
                ExtraRef = string.IsNullOrEmpty(aadhaar) ? null : SandboxAadhaarVerify.MaskAadhaar(aadhaar),

                ApiStatus = r == null ? null : r.SeedingStatus,
                ApiTxnId = r == null ? null : r.TransactionId,
                HttpStatusCode = r == null ? null : r.HttpStatusCode,
                DurationMs = r == null ? (long?)null : r.DurationMs,
                ErrorMessage = r == null ? null : r.ErrorMessage,
                RequestBody = r == null ? null : r.RequestJson,
                ResponseBody = r == null ? null : r.RawResponse,

                ConsentGiven = consent
            });
        }
        catch { }
    }

    /// <summary>
    /// True only when every check ran and none found the Aadhaar under another
    /// member. Fails closed: a check that cannot run counts as a duplicate.
    /// </summary>
    public static bool IsAadhaarFree(string aadhaar, long formNo)
    {
        try
        {
            const string sql = @"SELECT
                (SELECT COUNT(1) FROM KycVerify WHERE IdProofNo = @Aadhaar AND FormNo <> @FormNo) +
                (SELECT COUNT(1) FROM m_MemberMaster
                  WHERE Formno <> @FormNo
                    AND (AadharNo = @Aadhaar
                         OR ISNULL(AadharNo, '') + ISNULL(AadharNo2, '') + ISNULL(AAdharNo3, '') = @Aadhaar))";

            using (SqlConnection con = new SqlConnection(Cs))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add("@Aadhaar", SqlDbType.VarChar, 20).Value = aadhaar;
                    cmd.Parameters.Add("@FormNo", SqlDbType.Decimal).Value = formNo;
                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0) return false;
                }

                using (SqlCommand cmd = new SqlCommand("sp_CheckAadhaarUnique", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FormNo", formNo);
                    cmd.Parameters.AddWithValue("@AadhaarHash", SandboxAadhaarVerify.HashAadhaar(aadhaar));
                    cmd.Parameters.AddWithValue("@AadhaarPlain", aadhaar);
                    cmd.Parameters.AddWithValue("@AadhaarMasked", SandboxAadhaarVerify.MaskAadhaar(aadhaar));

                    object o = cmd.ExecuteScalar();
                    return o != null && o != DBNull.Value && Convert.ToInt32(o) == 1;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// The saved Aadhaar: KycVerify.IdProofNo first, then the member record. Both
    /// can hold masked or legacy values, so "" is returned unless the number is valid.
    /// </summary>
    public static string ReadSavedAadhaar(long formNo)
    {
        try
        {
            const string sql = @"SELECT
                (SELECT TOP 1 IdProofNo FROM KycVerify WHERE FormNo = @FormNo) AS IdProofNo,
                (SELECT TOP 1 AadharNo FROM m_MemberMaster WHERE Formno = @FormNo) AS AadharNo,
                (SELECT TOP 1 ISNULL(AadharNo, '') + ISNULL(AadharNo2, '') + ISNULL(AAdharNo3, '')
                   FROM m_MemberMaster WHERE Formno = @FormNo) AS AadharJoined";

            using (SqlConnection con = new SqlConnection(Cs))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@FormNo", SqlDbType.Decimal).Value = formNo;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0) return "";

                    foreach (string col in new[] { "IdProofNo", "AadharNo", "AadharJoined" })
                    {
                        object o = dt.Rows[0][col];
                        string v = o == DBNull.Value ? "" : o.ToString().Trim();
                        if (SandboxAadhaarVerify.IsValidAadhaarFormat(v)) return v;
                    }
                }
            }
        }
        catch { }

        return "";
    }

    /// <summary>
    /// Writes the full Aadhaar to the member record in the joining layout, four
    /// digits per column. Used after Address KYC is verified. Errors are swallowed:
    /// the KYC itself is already saved.
    /// </summary>
    public static void SaveMemberAadhaar(long formNo, string aadhaar)
    {
        if (formNo <= 0 || !SandboxAadhaarVerify.IsValidAadhaarFormat(aadhaar)) return;

        try
        {
            using (SqlConnection con = new SqlConnection(Cs))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE m_MemberMaster SET AadharNo = @A1, AadharNo2 = @A2, AAdharNo3 = @A3 WHERE Formno = @FormNo", con))
            {
                cmd.Parameters.Add("@A1", SqlDbType.VarChar, 4).Value = aadhaar.Substring(0, 4);
                cmd.Parameters.Add("@A2", SqlDbType.VarChar, 4).Value = aadhaar.Substring(4, 4);
                cmd.Parameters.Add("@A3", SqlDbType.VarChar, 4).Value = aadhaar.Substring(8, 4);
                cmd.Parameters.Add("@FormNo", SqlDbType.Decimal).Value = formNo;
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch { }
    }

    /// <summary>
    /// The Aadhaar on record in masked form, for read-only display. Address KYC
    /// stores only the masked number in KycVerify.IdProofNo, so for most verified
    /// members no full number exists; an already-masked value is returned as is.
    /// Returns "" when nothing usable is on record.
    /// </summary>
    public static string ReadAadhaarForDisplay(long formNo)
    {
        string full = ReadSavedAadhaar(formNo);
        if (full != "") return SandboxAadhaarVerify.MaskAadhaar(full);

        try
        {
            using (SqlConnection con = new SqlConnection(Cs))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT TOP 1 IdProofNo FROM KycVerify WHERE FormNo = @FormNo", con))
            {
                cmd.Parameters.Add("@FormNo", SqlDbType.Decimal).Value = formNo;
                con.Open();

                object o = cmd.ExecuteScalar();
                string v = o == null || o == DBNull.Value ? "" : o.ToString().Trim().ToUpper();

                // Only a properly masked Aadhaar: eight X's and the last four digits.
                if (Regex.IsMatch(v, "^X{8}[0-9]{4}$")) return v;
            }
        }
        catch { }

        return "";
    }

    /// <summary>
    /// Stores the linked Aadhaar for Address KYC to pick up, unless Address KYC is
    /// already verified. Errors are swallowed: callers run this after the PAN is
    /// saved, and the PAN must not be reported as failed because of it.
    /// </summary>
    public static void SaveLinkedAadhaar(long formNo, string aadhaar)
    {
        if (formNo <= 0 || !SandboxAadhaarVerify.IsValidAadhaarFormat(aadhaar)) return;

        try
        {
            const string sql = @"
                IF NOT EXISTS (SELECT 1 FROM KycVerify WHERE FormNo = @FormNo AND IsAddrssVerified = 'Y')
                BEGIN
                    UPDATE m_MemberMaster SET AadharNo = @A1, AadharNo2 = @A2, AAdharNo3 = @A3 WHERE Formno = @FormNo;
                    UPDATE KycVerify SET IdProofNo = @Aadhaar WHERE FormNo = @FormNo;
                END";

            using (SqlConnection con = new SqlConnection(Cs))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@A1", SqlDbType.VarChar, 4).Value = aadhaar.Substring(0, 4);
                cmd.Parameters.Add("@A2", SqlDbType.VarChar, 4).Value = aadhaar.Substring(4, 4);
                cmd.Parameters.Add("@A3", SqlDbType.VarChar, 4).Value = aadhaar.Substring(8, 4);
                cmd.Parameters.Add("@Aadhaar", SqlDbType.VarChar, 20).Value = aadhaar;
                cmd.Parameters.Add("@FormNo", SqlDbType.Decimal).Value = formNo;
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch { }
    }
}
