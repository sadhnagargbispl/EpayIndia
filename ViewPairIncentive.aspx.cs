using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewPairIncentive : System.Web.UI.Page
{
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    ModuleFunction objModuleFun;
    string ReqNo;
    DataTable Dt = new DataTable();
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["AStatus"] == null)
        {
            Response.Redirect("Logout.aspx");
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string scrname;
            if (!Page.IsPostBack)
            {
                if (Session["AStatus"].ToString() == "OK")
                {
                    if (!string.IsNullOrEmpty(Request["Sessid"]))
                    {
                        BindData();
                    }
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'> window.top.location.reload();" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close", scrname, false);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
    public void BindData(string SrchCond = "")
    {
        try
        {
            string cond = "";
            string formno = "";

            string strSql = objDAL.IsoStart + " exec sp_GetTeamGeneration '" + Request["formno"] + "','" + Request["SessID"] + "' " + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];
            Session["GDatapair"] = Dt;
            if (Dt.Rows.Count > 0)
            {
                GvData.DataSource = Dt;
                GvData.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);

        }
    }
    protected void RptDirects_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["GDatapair"];
            GvData.DataBind();
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
}
