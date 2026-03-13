using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
public partial class FundWithdraw : System.Web.UI.Page
{
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    public string formNo;
    string sql = "";
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnviapprove.Attributes.Add("onclick", DisableTheButton(Page, btnviapprove));
            BtnApproveAll.Attributes.Add("onclick", DisableTheButton(Page, BtnApproveAll));
            btnRejectAll.Attributes.Add("onclick", DisableTheButton(Page, btnRejectAll));
            if (!IsPostBack)
            {
                HdnCheckTrnns.Value = GenerateRandomString(6);
                txtMemId.Text = "";
                GvData.Visible = false;
                btnExport.Enabled = false;
                BtnApproveAll.Enabled = false;
                btnRejectAll.Enabled = false;
                if (Session["AStatus"] != null)
                {

                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public string GenerateRandomString(int iLength)
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        return sResult;
    }
    protected void GvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox ChkSelectAll = (CheckBox)e.Row.FindControl("ChkSelectAll");
                ChkSelectAll.Attributes.Add("onclick", "javascript:SelectAll('" + ChkSelectAll.ClientID + "')");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        try
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
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FillDetail()
    {
        try
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
            string sql = objDAL.IsoStart + "exec Sp_FundWithReportNewINR '" + idno + "','" + startDate + "','" + endDate + "','" + RbtStatus.SelectedValue + "','N'" + objDAL.IsoEnd;
            dtData = new DataTable();
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            GvData.DataSource = dtData;
            GvData.DataBind();
            Session["GData"] = dtData;
            GvData.Visible = true;
            if (dtData.Rows.Count > 0)
            {
                lblCount.Text = "Total: " + dtData.Rows.Count;
                Lblamount.Text = "Requested Amount : " + dtData.Compute("Sum(Amount)", "");
                lbladmincharge.Text = "Service Fees : " + dtData.Compute("Sum(AdminCharge)", "");
                Lblnetamount.Text = "Released Amount : " + dtData.Compute("Sum(NetAmount)", "");
                btnExport.Enabled = true;
                BtnApproveAll.Enabled = true;
                btnRejectAll.Enabled = true;
            }
            else
            {
                lblCount.Text = "Total: " + 0;
                Lblamount.Text = "Amount: " + 0;
                lbladmincharge.Text = "Admin charge: " + 0;
                Lblnetamount.Text = "Net Amount: " + 0;
                btnExport.Enabled = false;
                BtnApproveAll.Enabled = false;
                btnRejectAll.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            FillDetail();
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
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
            string sql = objDAL.IsoStart + "exec Sp_FundWithReportNewINR '" + idno + "','" + startDate + "','" + endDate + "','" + RbtStatus.SelectedValue + "','Y'" + objDAL.IsoEnd;
            dtTemp = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            dg.DataSource = dtTemp;
            dg.DataBind();
            ExportToExcel("WithdrawalReport.xls", dg);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }

    }
    private void ExportToExcel(string strFileName, DataGrid dg)
    {
        try
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
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
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnApproveAll_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox Chk;
            int cnt = 0;
            int cnt1 = 0;
            int updateeffect;
            Label LblId = new Label();
            Label LblMobl = new Label();
            Label LblFromDate = new Label();
            Label LblTodate = new Label();
            string msg = string.Empty;
            TextBox txtRemark = new TextBox();
            int i = 0;
            string Sql_Str = "Insert into Trnfundtransferbyadmin (Transid) values(" + HdnCheckTrnns.Value + ")";
            try
            {
                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql_Str));
            }
            catch (Exception Ex)
            {

            }
            if (i > 0)
            {
                foreach (GridViewRow Gvr in GvData.Rows)
                {
                    Chk = (CheckBox)Gvr.FindControl("chkSelect");
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
                        string Remark = "";

                        Remark = "Withdrawal Approved Of Idno " + TxtIDNo.Text + " For WeekNo:" + LnblWeek.Text + " By " + Session["UserName"];
                        string Sql = "";
                        Sql = "Update FundWithdrawls Set Status = 'A',IssueDate = GETDATE(),Remark = '" + txtRemark.Text + "',UserId = '" + Session["UserID"] + "',UserName = '" + Session["UserName"] + "' Where ReqID = " + LblId.Text + ";" +
                              "Update TrnVoucher Set Userid='" + Session["UserId"] + "' Where DrTo='" + FormNo + "' And vtype='W' And VoucherDate='" + DateOn + "' And Narration Like 'Fund Debited Againest Bank Withdrawal on " + DateOn + " with req. no." + Id + "';" +
                              " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" +
                              "('" + Session["UserID"] + "','" + Session["UserName"] + "','Fund WithDrawals','Fund Withdrawls Approve','" + Remark + "',Getdate(),'" + FormNo + "')";

                        cnt1++;
                        updateeffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql));
                        cnt++;
                        if (updateeffect != 0)
                        {
                            msg = cnt1.ToString() + " Withdrawal Approved Successfully.!";
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + cnt1 + "Withdrawal Approved Successfully.');", true);
                        }
                        else
                        {
                            msg = "Server Timeout, Try After Some Time.!";
                        }
                    }
                }
            }
            else
            {
                msg = "Try After Some Time.!";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + msg + "');location.replace('FundWithdraw.aspx');", true);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    public DataSet convertJsonStringToDataSet(string jsonString)
    {
        try
        {
            XmlDocument xd = new XmlDocument();
            jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
            xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
            DataSet ds = new DataSet();
            ds.ReadXml(new XmlNodeReader(xd));
            return ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static string Base64Encode(string plainText)
    {
        try
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void btnRejectAll_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox Chk;
            int cnt = 0;

            string msg = string.Empty;
            int updateeffect = 0;
            Label LblId = new Label();
            string sql = "";
            Label LblMobl = new Label();
            Label LblFromDate = new Label();
            Label LblTodate = new Label();
            int i = 0;
            string Sql_Str = "Insert into Trnfundtransferbyadmin (Transid) values(" + HdnCheckTrnns.Value + ")";
            try
            {
                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql_Str));
            }
            catch (Exception Ex)
            {

            }
            if (i > 0)
            {
                foreach (GridViewRow Gvr in GvData.Rows)
                {
                    Chk = (CheckBox)Gvr.FindControl("chkSelect");
                    if (Chk.Checked)
                    {
                        string Id = ((Label)Gvr.FindControl("LblID")).Text;
                        string FormNo = ((Label)Gvr.FindControl("LblFormNo")).Text;
                        string DateOn = ((Label)Gvr.FindControl("LblDate")).Text;
                        string IdNo = ((Label)Gvr.FindControl("LblIDNo")).Text;
                        string WeekNo = ((Label)Gvr.FindControl("LblWeek")).Text;
                        string TxtRemark = ((TextBox)Gvr.FindControl("TxtRemarks")).Text;
                        string lblAdminCharge = ((Label)Gvr.FindControl("lblAdminCharge")).Text;
                        string lblNetAmount = ((Label)Gvr.FindControl("lblNetAmount")).Text;
                        StringBuilder Str_TrnFun = new StringBuilder();
                        // Prepare remarks for rejected transaction
                        string Remark = "Request Rejected: Fund Debited Against Bank Withdrawal with req. no. " + Id;
                        string RemarkBC = "Request Rejected: Fund Debited Against Bank Service Fees with req. no. " + Id;
                        string refNo = "REJECTReq/" + Id;
                        string voucherDate = DateTime.Now.ToString("dd-MMM-yyyy");
                        string sessQuery = "(SELECT MAX(SessID) FROM M_SessnMaster)";
                        // 1. Append UPDATE statement
                        Str_TrnFun.Append("UPDATE FundWithdrawls SET ");
                        Str_TrnFun.Append("Status = 'R', ");
                        Str_TrnFun.Append("IssueDate = GETDATE(), ");
                        Str_TrnFun.Append("Remark = '" + TxtRemark.Replace("'", "''") + "', ");
                        Str_TrnFun.Append("UserId = '" + Convert.ToString(Session["UserID"]) + "', ");
                        Str_TrnFun.Append("UserName = '" + Convert.ToString(Session["UserName"]) + "' ");
                        Str_TrnFun.Append("WHERE ReqID = " + Id + ";");
                        // 2. Append First INSERT (Withdrawal reversal)
                        Str_TrnFun.Append("INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) ");
                        Str_TrnFun.Append("SELECT ISNULL(MAX(VoucherNo)+1,1001),'" + voucherDate + "','0','" + FormNo + "',");
                        Str_TrnFun.Append(Convert.ToDecimal(lblNetAmount) + ",'" + Remark + "','" + refNo + "','M','C',CONVERT(VARCHAR,GETDATE(),112),");
                        Str_TrnFun.Append(sessQuery + " FROM TrnVoucher;");
                        // 3. Append Second INSERT (Admin Charge reversal)
                        Str_TrnFun.Append("INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) ");
                        Str_TrnFun.Append("SELECT ISNULL(MAX(VoucherNo)+1,1001),'" + voucherDate + "','0','" + FormNo + "',");
                        Str_TrnFun.Append(Convert.ToDecimal(lblAdminCharge) + ",'" + RemarkBC + "','" + refNo + "','M','C',CONVERT(VARCHAR,GETDATE(),112),");
                        Str_TrnFun.Append(sessQuery + " FROM TrnVoucher;");
                        updateeffect = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_TrnFun.ToString());
                        cnt++;
                    }
                }
                if (updateeffect != 0)
                {
                    msg = cnt.ToString() + " Withdrawal Rejected Successfully.!";
                }
                else
                {
                    msg = "Server Timeout, Try After Some Time.!";
                }
            }
            else
            {
                msg = "Try After Some Time.!";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + msg + "');location.replace('FundWithdraw.aspx');", true);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + msg + "');", true);
            //FillDetail();
            //DivRemark.Visible = false;
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    protected void btnviapprove_Click(object sender, EventArgs e)
    {
        try
        {
            ViAprvAction("Y", "A");
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    private void ViAprvAction(string AprvType, string ApprvStatus)
    {
        try
        {
            CheckBox Chk;
            string Msg = string.Empty;
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
                Chk = (CheckBox)Gvr.FindControl("chkSelect");
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
                    sql = "Update FundWithdrawls Set Status='" + ApprvStatus + "',IssueDate=GETDATE(),UserId='" + (Session["UserID"]) + "',UserName='" + Session["UserName"] + "' Where ReqID=" + LblId.Text + ";" +
                          " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" +
                          "('" + (Session["UserID"]) + "','" + Session["UserName"] + "','Fund WithDrawals','Fund Withdrawls Virtual Approve','" + Remark + "',Getdate(),'" + FormNo + "')";
                    cnt1++;
                    updateeffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql));
                    cnt++;
                }
            }
            if (updateeffect != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + cnt1 + "Withdrawal Virtual Approved Successfully.!');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Server Timeout, Try After Some Time.!');", true);
            }
            FillDetail();
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            // Rebind the data to the GridView
            FillDetail();
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }

}

