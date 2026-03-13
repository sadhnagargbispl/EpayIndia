using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
public partial class StackWithdrawalreport : System.Web.UI.Page
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
            //btnviapprove.Attributes.Add("onclick", DisableTheButton(Page, btnviapprove));
            BtnApproveAll.Attributes.Add("onclick", DisableTheButton(Page, BtnApproveAll));
            btnRejectAll.Attributes.Add("onclick", DisableTheButton(Page, btnRejectAll));
            if (!IsPostBack)
            {
                //FUN_SP_GETWALLETNAME();
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
            string WalletAddress = "";
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
            string sql = objDAL.IsoStart + "exec Sp_FundStackWithReportNew '" + idno + "','" + startDate + "','" + endDate + "','" + RbtStatus.SelectedValue + "','N'" + objDAL.IsoEnd;
            dtData = new DataTable();
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            GvData.DataSource = dtData;

            GvData.DataBind();
            Session["GData"] = dtData;
            GvData.Visible = true;
            if (dtData.Rows.Count > 0)
            {
                //lblCount.Text = "Total: " + dtData.Rows.Count;
                Lblamount.Text = "Total: " + dtData.Rows.Count;
                //lbladmincharge.Text = "AdminCharge: " + dtData.Compute("Sum(Admincharge)", "");
                //Lblnetamount.Text = "Net Amount: " + dtData.Compute("Sum(NetAmount)", "");
                lblCount.Visible = true;
                Lblamount.Visible = true;
                lbladmincharge.Visible = true;
                Lblnetamount.Visible = true;
                btnExport.Enabled = true;
                BtnApproveAll.Enabled = true;
                btnRejectAll.Enabled = true;
            }
            else
            {
                btnExport.Enabled = false;
                BtnApproveAll.Enabled = false;
                btnRejectAll.Enabled = false;
                lblCount.Visible = false;
                Lblamount.Visible = false;
                lbladmincharge.Visible = false;
                Lblnetamount.Visible = false;
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
            string sql = objDAL.IsoStart + "exec Sp_FundStackWithReportNew '" + idno + "','" + startDate + "','" + endDate + "','" + RbtStatus.SelectedValue + "','Y'" + objDAL.IsoEnd;
            dtTemp = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            dg.DataSource = dtTemp;
            dg.DataBind();
            ExportToExcel("StackWithdrawalReport.xls", dg);
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
            int i = 0;
            string updateeffect = "";
            int cnt = 0;
            int cnt1 = 0;
            CheckBox Chk;
            string FormNo = "";
            string NetAmount = "";
            string ReqID = "";
            string IdNo = "";
            string messageUpdate = "";
            string billno = "";
            string rid = "";
            string Sql_Str = "Insert into Trnfundtransferbyadmin (Transid) values(" + HdnCheckTrnns.Value + ")";
            i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql_Str));

            if (i > 0)
            {
                foreach (GridViewRow Gvr in GvData.Rows)
                {
                    Chk = (CheckBox)Gvr.FindControl("chkSelect");
                    if (Chk.Checked)
                    {
                        string Id = ((Label)Gvr.FindControl("LblID")).Text;
                        FormNo = ((Label)Gvr.FindControl("LblFormNo")).Text;
                        IdNo = ((Label)Gvr.FindControl("LblIDNo")).Text;
                        NetAmount = ((Label)Gvr.FindControl("lblNetAmount")).Text;
                        ReqID = ((Label)Gvr.FindControl("LblDated")).Text;
                        billno = ((Label)Gvr.FindControl("LblWalletType")).Text;
                        rid = ((Label)Gvr.FindControl("rid")).Text;
                        string ds_DeductWallet_Response = "success";
                        if (ds_DeductWallet_Response.ToUpper() == "SUCCESS")
                        {
                            string Qry = " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values";
                            Qry += "('" + Session["UserID"] + "','" + Session["UserName"] + "','Investment Withdrawal','Investment Withdrawal Approve','" + ds_DeductWallet_Response.ToUpper() + "',Getdate(),'" + (FormNo) + "');";
                            string MaxVoucherNo_ = "3" + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
                            long maxVno_ = Convert.ToInt64(MaxVoucherNo_) + Convert.ToInt64(FormNo);
                            MaxVoucherNo_ = maxVno_.ToString();
                            Qry += " exec Sp_FundWithdrawalCrdit '" + Convert.ToInt64(FormNo) + "','" + ReqID.Trim() + "','" + MaxVoucherNo_ + "','" + NetAmount + "','" + billno + "','" + rid + "';";
                            int a = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Qry));
                            updateeffect = "success";
                            cnt++;
                        }
                        else
                        {
                            updateeffect = "failed";
                            cnt1++;
                        }
                    }
                }

                if (updateeffect.ToUpper() == "SUCCESS")
                {
                    messageUpdate = " " + cnt + " Regular Investment Withdrawal Approved Successfully.";
                }
                else
                {
                    messageUpdate = "Your Request Is Reject Please Contact To Admin.!";
                }
            }
            else
            {
                messageUpdate = "Try After Some Time.!";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + messageUpdate + "');location.replace('StackWithdrawalreport.aspx');", true);
        }
        catch (Exception ex)
        {
            string scrname = "<SCRIPT language='javascript'>alert('" + ex.Message + "');</SCRIPT>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Redirect", scrname, true);
        }
        FillDetail();
    }
    protected void btnRejectAll_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            int updateeffect = 0;
            int cnt = 0;
            int cnt1 = 0;
            CheckBox Chk;
            string FormNo = "";
            string NetAmount = "";
            string ReqID = "";
            string IdNo = "";
            string WeekNo = "";
            string hash_ = "";
            string StrQuery = "";
            string Panno_WalletAddress = "";
            string messageUpdate = "";
            string WalletType = "";

            string Sql_Str = "Insert into Trnfundtransferbyadmin (Transid) values(" + HdnCheckTrnns.Value + ")";
            i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql_Str));

            if (i > 0)
            {
                foreach (GridViewRow Gvr in GvData.Rows)
                {
                    Chk = (CheckBox)Gvr.FindControl("chkSelect");
                    if (Chk.Checked)
                    {
                        string Id = ((Label)Gvr.FindControl("LblID")).Text;
                        FormNo = ((Label)Gvr.FindControl("LblFormNo")).Text;
                        IdNo = ((Label)Gvr.FindControl("LblIDNo")).Text;
                        WeekNo = ((Label)Gvr.FindControl("LblWeek")).Text;
                        NetAmount = ((Label)Gvr.FindControl("lblNetAmount")).Text;
                        ReqID = ((Label)Gvr.FindControl("LblID")).Text;
                        Panno_WalletAddress = ((Label)Gvr.FindControl("LblPanno")).Text;
                        WalletType = ((Label)Gvr.FindControl("LblWalletType")).Text;
                        // Using a fully numeric date-time format
                        string MaxVoucherNo_ = "3" + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
                        long maxVno_ = Convert.ToInt64(MaxVoucherNo_) + Convert.ToInt64(FormNo);
                        MaxVoucherNo_ = maxVno_.ToString();

                        string MaxVoucherNo1_ = "4" + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
                        long maxVno1_ = Convert.ToInt64(MaxVoucherNo1_) + Convert.ToInt64(FormNo);
                        MaxVoucherNo1_ = maxVno1_.ToString();

                        string MaxVoucherNo2_ = "5" + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
                        long maxVno2_ = Convert.ToInt64(MaxVoucherNo2_) + Convert.ToInt64(FormNo);
                        MaxVoucherNo2_ = maxVno2_.ToString();

                        StrQuery = " exec Sp_StackWithdrawalreportalRejected '" + Convert.ToInt64(FormNo) + "','" + ReqID.Trim() + "','" + hash_.Trim() + "',";
                        StrQuery += "'" + MaxVoucherNo_ + "','" + MaxVoucherNo1_ + "','" + MaxVoucherNo2_ + "','" + WalletType + "',";
                        StrQuery += "'" + Session["UserID"] + "','" + Session["UserName"] + "';";
                        StrQuery += "insert into ApiReqResponse(Formno, Orderid, WalletAddress, PrivateKey, Request, Response, ApiStatus, RectimeStamp, ";
                        StrQuery += "ApiType, TxnHash, AMount, PostData) ";
                        StrQuery += "Values('" + FormNo + "','" + ReqID + "','" + Panno_WalletAddress.Trim() + "','',";
                        StrQuery += "'" + Session["SinglePayout"].ToString() + "','','" + hash_ + "',getdate(),'singlePayout',";
                        StrQuery += "'" + hash_ + "','" + Convert.ToDouble(NetAmount) + "','Failed Case');";

                        updateeffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, StrQuery));
                        string Qry = " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values";
                        Qry += "('" + Session["UserID"] + "','" + Session["UserName"] + "','StackWithdrawalreportal','StackWithdrawalreportal Reject','',Getdate(),'" + (FormNo) + "');";
                        int a = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Qry));
                        cnt++;
                    }
                }

                if (updateeffect != 0)
                {
                    messageUpdate = " " + cnt + " Withdrawal Rejected Successfully.!";
                }
                else
                {
                    messageUpdate = "Server Timeout, Try After Some Time.!";
                }
            }
            else
            {
                messageUpdate = "Try After Some Time.!";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + messageUpdate + "');location.replace('StackWithdrawalreport.aspx');", true);
        }
        catch (Exception ex)
        {
            string scrname = "<SCRIPT language='javascript'>alert('" + ex.Message + "');</SCRIPT>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Redirect", scrname, true);
        }

        FillDetail();

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
    protected void RbtStatus_SelectedIndexChanged(object sender, EventArgs e)
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
    protected void DDlWallet_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDetail();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}

