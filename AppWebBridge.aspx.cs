using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Mobile app ke WebView se site ka koi page kholne ke liye:
///   AppWebBridge.aspx?token={app token}&amp;page=StoreRedirect.aspx
/// Token se wahi Session ban jata hai jo AppLogin.aspx banata hai, phir page par redirect.
/// Sirf isi site ke .aspx page allowed hain (open redirect nahi hoga).
/// </summary>
public partial class AppWebBridge : System.Web.UI.Page
{
    private static readonly Regex AllowedPage = new Regex(@"^[A-Za-z0-9_\-]+\.aspx(\?[A-Za-z0-9_\-=&%.+]*)?$");

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();

        AppApiLogger log = new AppApiLogger();
        string page = (Request.QueryString["page"] ?? "WebApp.aspx").Trim().TrimStart('~', '/');
        log.Start("webbridge", Request, "", "", "{\"page\":\"" + HttpUtility.JavaScriptStringEncode(page) + "\"}");

        AppMember member = null;
        string status = "FAILED";
        string error = null;
        string next = "AppLogout.aspx";

        try
        {
            if (!AllowedPage.IsMatch(page))
            {
                error = "Page not allowed";
            }
            else
            {
                member = AppApiCore.ValidateToken(Request.QueryString["token"], AppApiCore.BridgeDevice);
                if (member == null)
                    error = "Invalid / expired token";
                else if (member.IsBlock.ToUpper() == "Y")
                    error = "ID blocked";
                else if (!SetSession(member))
                    error = "sp_Login1 returned no row";
                else
                {
                    status = "OK";
                    next = page;
                }
            }
        }
        catch (Exception ex)
        {
            status = "ERROR";
            error = ex.Message;
            log.End("webbridge", member, status, 500, null, page, error, ex.ToString());
            Response.Redirect(next, false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        log.End("webbridge", member, status, status == "OK" ? 302 : 401, "{\"redirect\":\"" + HttpUtility.JavaScriptStringEncode(next) + "\"}", page, error, null);
        Response.Redirect(next, false);
        Context.ApplicationInstance.CompleteRequest();
    }

    /// <summary>AppLogin.aspx -> enterHomePg() jaisa hi Session.</summary>
    private bool SetSession(AppMember member)
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec sp_Login1 @Uid, @Pwd",
            new SqlParameter("@Uid", SqlDbType.VarChar, 100) { Value = member.IdNo },
            new SqlParameter("@Pwd", SqlDbType.VarChar, 100) { Value = member.Passw });
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return false;

        DataRow r = ds.Tables[0].Rows[0];
        Session["Run"] = 0;
        Session["Status"] = "OK";
        Session["IDNo"] = r["IDNo"];
        Session["FormNo"] = r["Formno"];
        Session["MemName"] = r["MemFirstName"] + " " + r["MemLastName"];
        Session["MobileNo"] = r["Mobl"];
        Session["MemKit"] = r["KitID"];
        Session["Package"] = r["KitName"];
        Session["Position"] = r["fld3"];
        Session["Doj"] = string.Format("{0:dd-MMM-yyyy}", r["Doj"]);
        Session["DOA"] = string.Format("{0:dd-MMM-yyyy}", r["Upgradedate"]);
        Session["Address"] = r["Address1"];
        Session["IsFranchise"] = r["Fld5"];
        Session["ActiveStatus"] = r["ActiveStatus"];
        Session["MemPassw"] = r["Passw"];
        Session["MFormno"] = r["MFormNo"];
        Session["MemUpliner"] = r["UplnFormno"];
        Session["MID"] = r["MID"];
        Session["EMail"] = r["Email"];
        Session["profilepic"] = r["profilepic"];
        Session["Panno"] = r["Panno"];
        Session["ActivationDate"] = r["ActivationDate"];
        Session["MemEPassw"] = r["Epassw"];
        return true;
    }
}
