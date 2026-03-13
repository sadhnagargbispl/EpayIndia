using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class ApprovePaymentReqs : System.Web.UI.Page
{
    string scrname;
    DAL objDAL = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.BtnSearch.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnSearch));
            this.btnApproove.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnApproove));
            this.BtnRejects.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnRejects));
            if (!IsPostBack)
            {
                HdnCheckTrnns.Value = GenerateRandomString(6);
                BindPaymode();
                if (Session["AStatus"] != null)
                {
                    if (Request.QueryString.HasKeys())
                    {
                        TxtMemID.Text = ClearInject(Request.QueryString["key"]);
                        BindData(" AND b.IDNo='" + ClearInject(Request.QueryString["key"]) + "'");
                    }
                    else
                    {
                        BindData();
                    }
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
    private string DisableTheButton(Control pge, Control btn)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
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
    public void BindPaymode()
    {
        try
        {
            DataTable dtData = new DataTable();
            string SqlStr = objDAL.IsoStart + " exec Sp_Getpaymode " + objDAL.IsoEnd;
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, SqlStr).Tables[0];
            ddlPayment.DataSource = dtData;
            ddlPayment.DataValueField = "PId";
            ddlPayment.DataTextField = "PayMode";
            ddlPayment.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public void BindData(string Condition = "")
    {
        try
        {
            string idno = "";
            string startDate;
            string endDate;
            DataTable dtData = new DataTable();
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd-MMM-yyyy");
            idno = !string.IsNullOrWhiteSpace(ClearInject(TxtMemID.Text)) ? ClearInject(TxtMemID.Text) : "";
            if (string.IsNullOrWhiteSpace(ClearInject(txtStartDate.Text)))
            {
                startDate = "12-oct-2017";
            }
            else
            {
                startDate = ClearInject(txtStartDate.Text);
            }
            if (string.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                endDate = formattedDate;
            }
            else
            {
                endDate = ClearInject(txtEndDate.Text);
            }
            string sql = objDAL.IsoStart + "exec Sp_GetWalletApproveReport '" + idno + "','" + startDate + "','" + endDate + "',";
            sql += "'" + RbtStatus.SelectedValue + "','N','" + ddlPayment.SelectedValue + "'" + objDAL.IsoEnd;

            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            GvData.DataSource = dtData;
            GvData.DataBind();
            Session["GData"] = dtData;
            GvData.Visible = true;
            if (dtData.Rows.Count > 0)
            {
                btnApproove.Visible = true;
                BtnRejects.Visible = true;
            }
            else
            {
                btnApproove.Visible = false;
                BtnRejects.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string Condition = "";
            BindData(Condition);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    protected void GvData_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox ChkSelectAll = (CheckBox)e.Row.FindControl("ChkSelectAll");
                ChkSelectAll.Attributes.Add("onclick", "javascript:SelectAll('" + ChkSelectAll.ClientID + "')");
            }
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    private void AprvAction(string AprvType, string ApprvStatus)
    {
        try
        {
            string sql = "";
            CheckBox Chk;
            Label lbl;
            Label lblReqno;
            Label lblIdno;
            Label LblAmount;
            Label lblPaymode;
            int Cnt = 0;
            string Remark = "";
            string email = "";
            int i = 0;
            string Sql_Str = "Insert into Trnfundtransferbyadmin (Transid) values(" + HdnCheckTrnns.Value + ")";
            try
            {
                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql_Str));
            }
            catch (Exception ex)
            {

            }
            if (i > 0)
            {
                foreach (GridViewRow Gvr in GvData.Rows)
                {
                    Chk = (CheckBox)Gvr.FindControl("chkSelect");
                    lbl = (Label)Gvr.FindControl("LblGrpID");
                    lblReqno = (Label)Gvr.FindControl("LblReqNo");
                    lblIdno = (Label)Gvr.FindControl("LblIdNo");
                    LblAmount = (Label)Gvr.FindControl("lblAmount");
                    lblPaymode = (Label)Gvr.FindControl("LblPaymode");
                    email = ((Label)Gvr.FindControl("LblEmail")).Text;
                    if (Chk.Checked == true && Chk.Enabled == true)
                    {
                        if (AprvType == "Y")
                        {

                            Remark = " Approve Payment Request On ReqNo:" + lblReqno.Text + " for Idno:" + lblIdno.Text + "";
                            sql = sql + "INSERT INTO RejtVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID,Status,Rn) SELECT ISNULL(Max(VoucherNo)+1,1001),'" + DateTime.Now.ToString("dd-MMM-yyyy") + "'," +
                                "'0','" + lbl.Text + "'," + LblAmount.Text + ",'Amount Added by Payment  Req.No.:" + lblReqno.Text + ".','Req/" + lblReqno.Text + "','S','C',Convert(Varchar,Getdate(),112),'" + Session["CurrentSessn"] + "','A',1 FROM TrnVoucher;";
                        }
                        else
                        {
                            Remark = " Reject Payment Request On ReqNo:" + lblReqno.Text + " for Idno:" + lblIdno.Text + "";
                        }
                        sql = sql + ";Update WalletReq SET  IsApprove = '" + AprvType + "',ApproveDate=GEtdate(),ApproveBy='" + Convert.ToString(Session["UserID"]) + "',ApproveRemark='" + TxtARemark.Text + "' where FormNo='" + lbl.Text + "' And ReqNo='" + lblReqno.Text + "'";
                        sql = sql + "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp)Values" +
                            "('" + Convert.ToInt32(Session["UserID"]) + "','" + Session["UserName"] + "','Approve Payment Request ','Approve Payment Request','" + Remark + "',Getdate())";
                        Cnt = Cnt + 1;
                    }
                }

                String strSql = " BEGIN TRY BEGIN TRANSACTION " + sql + " COMMIT TRANSACTION  END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH";
                int a = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strSql));
                string MsgTxt = "";
                if (AprvType == "Y")
                {
                    MsgTxt = "Approved";
                }
                else
                {
                    MsgTxt = "Rejected";
                }
                if (a != 0 && Cnt > 0)
                {
                    lblMsg.Text = "" + Cnt + " Requests " + MsgTxt + " Successfully.";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                    TxtARemark.Text = "";
                    BindData();
                    Response.Redirect("ApprovePaymentReqs.aspx");
                }
                else
                {
                    lblMsg.Text = "Not " + MsgTxt;
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Try After Some Time.!');location.replace('ApprovePaymentReqs.aspx');", true);
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }


    }
    protected void btnApproove_Click(object sender, EventArgs e)
    {
        try
        {
            DivRemark.Visible = true;
            btnApprove.Visible = true;
            BtnReject.Visible = false;
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }

    }
    protected void BtnRejects_Click(object sender, EventArgs e)
    {
        try
        {
            DivRemark.Visible = true;
            btnApproove.Visible = false;
            BtnReject.Visible = true;
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            if ((TxtARemark.Text).Trim() == "")
            {
                scrname = "<SCRIPT language='javascript'>alert('Enter Remark.');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Enter Remark.');", true);
                return;
            }
            AprvAction("Y", "A");
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            if ((TxtARemark.Text).Trim() == "")
            {
                scrname = "<SCRIPT language='javascript'>alert('Enter Remark.');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Enter Remark.');", true);
                return;
            }
            AprvAction("R", "R");
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
            StrObj = StrObj.Trim();
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return StrObj;
    }
}