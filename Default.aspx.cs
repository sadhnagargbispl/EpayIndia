using System;
using System.Linq;
using System.Data;
using System.Web.UI;
using System.Configuration;
public partial class _Default : System.Web.UI.Page
{
    string strScript;
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();

    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    private void enterHomePg(string uid, string Pwd)
    {
        try
        {
            if (uid.Length > 0 & Pwd.Length > 0)
            {

                string qry = objDAL.IsoStart + " Exec Sp_AdminLogin '" + uid + "','" + Pwd + "'" + objDAL.IsoEnd;
                dtData = new DataTable();
                dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
                if (dtData.Rows.Count == 0)
                {
                    strScript = "<script language='javascript'>alert('Please Enter valid UserName or Password.');</script>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", strScript, false);
                    Response.Redirect("Default.aspx");
                }
                else
                {

                    Session["UserID"] = dtData.Rows[0]["UserId"];
                    Session["UserName"] = dtData.Rows[0]["UserName"];
                    Session["GroupID"] = dtData.Rows[0]["GroupId"];
                    Session["SGroupID"] = dtData.Rows[0]["GroupId"];
                    Session["grpID"] = "";
                    Session["grdIndex"] = 0;
                    Session["UserPermission"] = 1;
                    Session["OtpCount"] = 0;
                    Session["Mobile"] = dtData.Rows[0]["MobileNo"];
                    Session["AStatus"] = "OK";

                    string adminHome = "~/Home.aspx";
                    Response.Redirect(adminHome,false);
                }


            }
        }
        catch (Exception ex)
        {

            strScript = "<script language='javascript'>alert('"+ ex.Message  +"');</script>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", strScript, false);
        }
    }

    protected void BtnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string uid, pwd;
            bool isValid = true;
            if (isValid == true)
            {
                uid = TxtUID.Text;
                pwd = TxtPWD.Text;
                if (((uid != null) & (pwd != null)))
                {
                    enterHomePg(uid, pwd);
                }
            }
            else
            {
                strScript = "<script language='javascript'>alert('Invalid Verification Code.');</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", strScript, false);
            }
        }
        catch (Exception ex)
        {
            strScript = "<script language='javascript'>alert('" + ex.Message + "');</script>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", strScript, false);

        }

    }
}
