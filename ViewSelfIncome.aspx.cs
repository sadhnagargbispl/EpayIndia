using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewSelfIncome : System.Web.UI.Page
{
    
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    DataTable Dt = new DataTable();
    string ReqNo;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        objDAL = new DAL();
        string scrname;
       
        string ReqNo;
        if (string.IsNullOrEmpty(Request["IdNo"]) == false && string.IsNullOrEmpty(Request["Sessid"]) == false)
        {
        }

        if (!Page.IsPostBack)
        {
            if (Session["AStatus"] != null)
               
            {
                if (string.IsNullOrEmpty(Request["IdNo"]) == false && string.IsNullOrEmpty(Request["Sessid"]) == false)
                {
                    //LblNo.Text = " Request No :" + ReqNo;
                    BindData();
                }
                GetFormNo();
            }
            else
            {
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();" + "</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
            }

        }
       
       
    }
    private string GetFormNo()
    {
        objDAL = new DAL();
        string idNo;
        string formno;
        idNo = Request["Idno"];
        string qry = objDAL.IsoStart + "Select FormNo from " + objDAL.DBName + "..M_MemberMaster where IdNo='" + idNo + "'" + objDAL.IsoEnd ;
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
            string sql = " exec Sp_GetSelfIncomeView '" + (formno) + "','" + Request["Sessid"] + "'" ;
            dtData = new DataTable();
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            GvData.DataSource = dtData;
            GvData.DataBind();
            Session["GData"] = dtData;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void GvData_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}