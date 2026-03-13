using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewdirectIncome : System.Web.UI.Page
{
    DataTable dtData = new DataTable();
    DAL objDal = new DAL();
    DataTable Dt = new DataTable();
    string ReqNo;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        {
            string scrname;
            string ReqNo;
            if (string.IsNullOrEmpty(Request["IdNo"]) == false && string.IsNullOrEmpty(Request["Sessid"]) == false)
            {
                //ReqNo = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["IdNo"]));
            }

            if (!Page.IsPostBack)
            {

                if (Session["AStatus"] != null)
                {
                    if (string.IsNullOrEmpty(Request["IdNo"]) == false && string.IsNullOrEmpty(Request["Sessid"]) == false)
                    {
                        BindData();
                    }
                    GetFormNo();
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'> window.top.location.reload();" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close", scrname, false);
                }
            }

        }
    }
    private string GetFormNo()
    {

        DAL Obj = new DAL();
        string idNo;
        string formno;
        idNo = Request["Idno"];
        string qry = Obj.IsoStart + "Select FormNo from " + objDal.DBName + ".. M_MemberMaster where IdNo='" + idNo + "'" + Obj.IsoEnd;
        Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
        if (Dt.Rows.Count > 0)
        {
            formno = Dt.Rows[0]["FormNo"].ToString();
        }
        else
        {
            formno = "0";
        }
        return formno;
    }

    public void BindData(string SrchCond = "")
    {
        try
        {
            string formno = GetFormNo();
            string sql = objDal.IsoStart + " exec Sp_GetTeamTradingOverridingBonus  '" + (formno) + "','" + Request["Sessid"] + "'" + objDal.IsoEnd;
            dtData = new DataTable();
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            GvData.DataSource = dtData;
            GvData.DataBind();
            Session["GData"] = dtData;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void GvData_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["GData"];
            GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

}