using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class TokenFundWithdraw : System.Web.UI.Page
{
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    ModuleFunction objModuleFun = new ModuleFunction();
    public string formNo;
    string sql = "";
    string Condition = "";
    string scrname = "";
    string IsoStart;
    string IsoEnd;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

        BtnApproveAll.Attributes.Add("onclick", DisableTheButton(Page, BtnApproveAll));
        IsoStart = objDAL.IsoStart;
        IsoEnd = objDAL.IsoEnd;
        objDAL = new DAL();
        objModuleFun = new ModuleFunction();
        if (!Page.IsPostBack)
        {
            txtMemId.Text = "";
            GvData.Visible = false;
            btnExport.Enabled = false;
            if (Session["AStatus"] == "OK")
            {
            }
        }
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");
        return sb.ToString();
    }
    protected void GvData_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            System.Web.UI.WebControls.CheckBox ChkSelectAll = (System.Web.UI.WebControls.CheckBox)e.Row.FindControl("ChkSelectAll");
            ChkSelectAll.Attributes.Add("onclick", "javascript:SelectAll('" + ChkSelectAll.ClientID + "')");
        }
    }
    private void FillDetail()
    {
        string idno = "";
        string startDate;
        string endDate;
        DateTime currentDate = DateTime.Now;
        string formattedDate = currentDate.ToString("dd-MMM-yyyy");
        idno = !string.IsNullOrWhiteSpace(txtMemId.Text) ? txtMemId.Text.Trim() : "";
        if (string.IsNullOrWhiteSpace(txtStartDate.Text))
        {
            startDate = "12-oct-2017";
        }
        else
        {
            startDate = txtStartDate.Text;
        }
        if (string.IsNullOrWhiteSpace(txtEndDate.Text))
        {
            endDate = formattedDate;
        }
        else
        {
            endDate = txtEndDate.Text;
        }
        string sql = IsoStart + "exec Sp_FundWithReportNewToken '" + idno + "','" + startDate + "','" + endDate + "','" + RbtStatus.SelectedValue + "','N'" + IsoEnd;
        dtData = new DataTable();
        dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
        GvData.DataSource = dtData;
        GvData.DataBind();
        Session["GData"] = dtData;
        GvData.Visible = true;
        if (dtData.Rows.Count > 0)
        {
            btnExport.Enabled = true;
        }
        else
        {
            btnExport.Enabled = false;
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        FillDetail();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTemp = new DataTable();
            DataGrid dg = new DataGrid();
            string idno = "";
            string startDate;
            string endDate;
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd-MMM-yyyy");
            if (!string.IsNullOrWhiteSpace(txtMemId.Text))
            {
                idno = txtMemId.Text.Trim();
            }
            else
            {
                idno = "";
            }
            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                startDate = "12-oct-2017";
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrEmpty(txtEndDate.Text))
            {
                endDate = formattedDate;
            }
            else
            {
                endDate = txtEndDate.Text;
            }
            string sql = "exec Sp_FundWithReportNewToken '" + idno + "','" + startDate + "','" + endDate + "','" + RbtStatus.SelectedValue + "','Y'";
            dtTemp = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            dg.DataSource = dtTemp;
            dg.DataBind();
            ExportToExcel("WithdrawalReport.xls", dg);
        }
        catch (Exception ex)
        {
        }

    }
    private void ExportToExcel(string strFileName, DataGrid dg)
    {
        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);

        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.xls";
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
        Response.Charset = "";

        dg.EnableViewState = false;
        dg.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }
    protected void BtnApproveAll_Click(object sender, EventArgs e)
    {
        int cnt = 0;
        int cnt1 = 0;
        int updateeffect = 0;
        Label LblId = new Label();
        Label LblMobl = new Label();
        Label LblFromDate = new Label();
        Label LblTodate = new Label();
        TextBox txtRemark = new TextBox();

        foreach (GridViewRow Gvr in GvData.Rows)
        {
            System.Web.UI.WebControls.CheckBox Chk = (System.Web.UI.WebControls.CheckBox)Gvr.FindControl("chkSelect");
            if (Chk.Checked)
            {
                LblId.Text = ((Label)Gvr.FindControl("LblID")).Text;
                TxtIDNo.Text = ((Label)Gvr.FindControl("LblIDNo")).Text;
                TxtName.Text = ((Label)Gvr.FindControl("LblPayeeName")).Text;
                TxtAmount.Text = ((Label)Gvr.FindControl("LblAmount")).Text;
                LblMobl.Text = ((Label)Gvr.FindControl("Lblmobl")).Text;
                LnblWeek.Text = ((Label)Gvr.FindControl("LblWeek")).Text;
                txtRemark.Text = ((TextBox)Gvr.FindControl("TxtRemarks")).Text;
                string Id = ((Label)Gvr.FindControl("LblID")).Text;
                string FormNo = ((Label)Gvr.FindControl("LblFormNo")).Text;
                string DateOn = ((Label)Gvr.FindControl("LblDate")).Text;

                string Remark = "Withdrawal Approved Of Idno " + TxtIDNo.Text + " For WeekNo:" + LnblWeek.Text + " By " + Session["UserName"];

                string sql = "Update TrnFundwithdrawls Set Status='A',IssueDate=GETDATE(),Remark='" + txtRemark.Text + "',UserId='" + Session["UserID"] + "',UserName='" + Session["UserName"] + "' Where ReqID=" + LblId.Text + ";" +
                    "Update TrnVoucher Set Userid='" + Session["UserId"] + "' Where DrTo='" + FormNo + "' And vtype='W' And VoucherDate='" + DateOn + "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " + DateOn + " with req. no." + Id + "';" +
                    " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" +
                    "('" + Session["UserID"] + "','" + Session["UserName"] + "','Fund WithDrawals','Fund Withdrawls Approve','" + Remark + "',Getdate(),'" + FormNo + "')";

                cnt1++;

                updateeffect = objDAL.SaveData(sql);

                cnt++;
            }
        }

        string scrname = "";
        if (updateeffect != 0)
        {
            scrname = "<SCRIPT language='javascript'>alert('" + cnt1 + " Withdrawal Approved  Successfully.');</SCRIPT>";
        }
        else
        {
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');</SCRIPT>";
        }

        Page.ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
        FillDetail();
    }

    public DataSet convertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
        xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));
        return ds;
    }
    public static string Base64Encode(string plainText)
    {
        byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    protected void btnRejectAll_Click(object sender, EventArgs e)
    {
        int cnt = 0;
        int updateeffect = 0;
        string sql = "";
        string scrname = "";
        for (int i = 0; i < GvData.Rows.Count; i++)
        {
            GridViewRow Gvr = GvData.Rows[i];
            System.Web.UI.WebControls.CheckBox Chk = (System.Web.UI.WebControls.CheckBox)Gvr.FindControl("chkSelect");
            if (Chk.Checked)
            {
                string Id = ((Label)Gvr.FindControl("LblID")).Text;
                string FormNo = ((Label)Gvr.FindControl("LblFormNo")).Text;
                string IdNo = ((Label)Gvr.FindControl("LblIDNo")).Text;
                string WeekNo = ((Label)Gvr.FindControl("LblWeek")).Text;
                string TxtRemark = ((TextBox)Gvr.FindControl("TxtRemarks")).Text;
                string Remark = "Withdrawal Rejected Of Idno " + IdNo + " for WeekNo:" + WeekNo + " By " + Session["UserName"];

                sql += "Update TrnFundwithdrawls Set Status='R',IssueDate=GETDATE(),Remark='" + TxtRemark + "',UserId='" + Session["UserID"] + "',UserName='" + Session["UserName"] + "' Where ReqID=" + Id + ";";
                sql += " INSERT INTO RejtVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID,Status,Rn) ";
                sql += "  SELECT 0,'" + DateTime.Now.ToString("dd-MMM-yyyy") + "',0, '" + FormNo + "',Amount,";
                sql += "  Replace(Replace(Narration,'Debited','Credited'),'with ','(Reject) with '),'Req/" + Id + "','M','C',convert(varchar,getdate(),112),";
                sql += "  (Select Max(SessID) from M_SessnMaster),'R',Row_Number() Over(Order by voucherid) FROM TrnVoucher  Where Drto =  '" + FormNo + "' And refno = 'Req/" + Id + "';";

                updateeffect = objDAL.SaveData(sql);
                //SendSMS(TxtName.Text, LblTodate.Text, LblFromDate.Text, TxtAmount.Text, LblMobl.Text)
                cnt++;
            }
        }

        if (updateeffect != 0)
        {
            scrname = "<SCRIPT language='javascript'>alert('" + cnt + " Withdrawal Rejected Successfully.');</SCRIPT>";
            FillDetail();
        }
        else
        {
            scrname = "<SCRIPT language='javascript'>alert('Server Timeout, Try After Some Time.');</SCRIPT>";
        }
        Page.ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
        FillDetail();
        DivRemark.Visible = false;
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvData.PageIndex = e.NewPageIndex;
        // Rebind the data to the GridView
        FillDetail();
    }

}

