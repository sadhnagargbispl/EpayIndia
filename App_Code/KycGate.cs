using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

/// <summary>
/// Enforces the order the three KYC steps must happen in: PAN, then Address,
/// then Bank.
///
/// The order is not cosmetic. PAN verification establishes the member's legal
/// name against NSDL, and every later step is checked against it - the bank
/// account name in particular. Without PAN first there is nothing authoritative
/// to compare anything to.
/// </summary>
public static class KycGate
{
    private static readonly string constr =
        ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    /// <summary>
    /// Where a member is sent when they land on a step they have not earned yet,
    /// or when a hard mismatch stops the flow.
    /// </summary>
    public static string LandingPage
    {
        get
        {
            string p = ConfigurationManager.AppSettings["KycLandingPage"];
            return string.IsNullOrEmpty(p) ? "index.aspx" : p;
        }
    }

    public class Status
    {
        public bool PanVerified { get; set; }
        public bool AddressVerified { get; set; }
        public bool BankVerified { get; set; }

        /// <summary>
        /// The name PAN verification settled on. Empty until PAN is done.
        /// </summary>
        public string PanName { get; set; }

        public string Panno { get; set; }
    }

    public static Status Load(long formNo)
    {
        var s = new Status();

        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand("sp_GetKycGateStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30;
                cmd.Parameters.AddWithValue("@FormNo", formNo);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0) return s;

                    DataRow r = dt.Rows[0];
                    s.PanVerified = Convert.ToInt32(r["PanVerified"]) == 1;
                    s.AddressVerified = Convert.ToInt32(r["AddressVerified"]) == 1;
                    s.BankVerified = Convert.ToInt32(r["BankVerified"]) == 1;
                    s.PanName = Convert.ToString(r["PanName"]).Trim();
                    s.Panno = Convert.ToString(r["Panno"]).Trim();
                }
            }
        }
        catch
        {
            // Fail closed. An unreadable gate must not become an open one, or a
            // member could reach Bank KYC with no verified identity behind it.
        }

        return s;
    }

    /// <summary>
    /// Carries a message across a redirect. Session rather than a query string:
    /// the text is written straight into an alert, and a query string would let
    /// anyone craft a link that puts arbitrary content in front of a member.
    /// </summary>
    public const string MsgKey = "KycGateMessage";

    //public static void Redirect(Page page, string target, string message)
    //{
    //    if (!string.IsNullOrEmpty(message))
    //        HttpContext.Current.Session[MsgKey] = message;

    //    // false plus CompleteRequest avoids the ThreadAbortException that
    //    // Response.Redirect(url) throws and that every catch block would swallow.
    //    page.Response.Redirect(target, false);
    //    page.Context.ApplicationInstance.CompleteRequest();
    //}
    public static void Redirect(Page page, string target, string message)
    {
        if (!string.IsNullOrEmpty(message))
            HttpContext.Current.Session[MsgKey] = message;

        // Avoid ThreadAbortException
        page.Response.Redirect(target, false);

        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    /// <summary>
    /// Renders and clears any pending gate message. Call from the landing page and
    /// from each KYC page so the reason for a redirect is never lost.
    /// </summary>
    public static void ShowPendingMessage(Page page)
    {
        try
        {
            object m = HttpContext.Current.Session[MsgKey];
            if (m == null) return;

            HttpContext.Current.Session[MsgKey] = null;

            string text = m.ToString()
                .Replace("\\", "\\\\").Replace("'", "\\'")
                .Replace("\r", " ").Replace("\n", " ");

            // ScriptManager where one exists, ClientScript otherwise: the landing
            // page may be a plain form with no partial-rendering support.
            if (ScriptManager.GetCurrent(page) != null)
                ScriptManager.RegisterStartupScript(page, page.GetType(),
                    "kycGateMsg", "alert('" + text + "');", true);
            else
                page.ClientScript.RegisterStartupScript(page.GetType(),
                    "kycGateMsg", "alert('" + text + "');", true);
        }
        catch { }
    }
}