using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebApp : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    public string shopEnc = "";
    public string gvEnc = "";
    public string utilityEnc = "";
    public string foodEnc = "";
    public string movieEnc = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    DataTable dt = new DataTable();
                    string sql = "select profilepic, * from M_MemberMaster where formno=" + Session["FormNo"];
                    dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        ddUserName.InnerText = dt.Rows[0]["memfirstname"].ToString();
                    }
                }

            }

            else
            {
                Response.Redirect("AppLogout.aspx", false);
            }

           
        }
        catch (Exception ex)
        {
            // Handle exception
        }

    }
}