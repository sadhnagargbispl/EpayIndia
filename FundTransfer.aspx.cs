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
public partial class FundTransfer : System.Web.UI.Page
{

    DataTable Dt = new DataTable();
    DAL objDAL = new DAL();
    DataSet Ds = new DataSet();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            BtnFundTransfer.Attributes.Add("onclick", DisableTheButton(Page, BtnFundTransfer));

            try
            {
                var str = "exec('Create table Trnfundtransferbyadmin ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," + "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnfundtransferbyadmin] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')";
                int i = 0;
                i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str);
            }
            catch (Exception Ex)
            {
            }

            try
            {

                var str1 = "Exec('CREATE PROCEDURE [dbo].[sp_funtransfer] (   " + " @formno  numeric(18,0),   " + " @Voucherdate datetime,   " + " @crto numeric(18,0),@drto numeric(18,0), @Amount numeric(18,2),  " + " @Narration varchar(max), @Refno varchar(250), " + " @AcType char(1),@VType char(1), " + " @WSessID int, @Txtaremark nvarchar(Max), " + " @UserId Numeric(18,0), " + " @UserName nvarchar(200), " + " @PageName nvarchar(200),  " + " @Activity nvarchar(200),  " + " @Remark nvarchar(Max) ,    " + " @Hdntransid numeric(18,0)  " + " )  " + " AS  " + " BEGIN      " + " DECLARE @LocalError INT,  " + " @ErrorMessage VARCHAR(4000)  " + " DECLARE @Identity INT     " + " DECLARE @MainIdentity INT  " + " BEGIN TRY       " + " BEGIN TRANSACTION TestTransaction  " + " begin    " + " Declare @MaxVouNo As Numeric(18,0);      " + " Select @MaxVouNo=Cast(Convert(nvarchar(20),GetDate(),112)+Replace(Convert(nvarchar(20),GetDate(),114),'':'','''') as Numeric(18,0)); " + " Insert into Trnfundtransferbyadmin (Transid)values(@Hdntransid)        " + " INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,RecTimeStamp,VTYpe,SessID,WSessID) " + " values(@MaxVouNo,@Voucherdate,@crto,@drto,@Amount,@Narration,@Refno,@AcType,Getdate(),@VType,Convert(Varchar,Getdate(),112),@WSessID)   " + " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,Memberid) " + " Values(@UserId,@UserName,@PageName,@Activity,@Remark,Getdate(),@formno)  " + " end " + " COMMIT TRANSACTION TestTransaction      " + "    Select ''Success'' As result, 1 As Count    " + " RETURN     " + " END TRY    " + " BEGIN CATCH   " + " SELECT @LocalError = ERROR_NUMBER(),@ErrorMessage = ERROR_MESSAGE()   " + " Select ''Faild'' As result, 1 As Count  " + " IF (XACT_STATE()) <> 0     " + " BEGIN ROLLBACK TRANSACTION TestTransaction " + " END    " + " RAISERROR (''sp_funtransfer: %d: %s'',16,1,@LocalError,@ErrorMessage);   " + " RETURN (0)   " + " END CATCH   " + " END ') ";
                int j = 0;
                j = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str1);
            }
            catch (Exception Ex)
            {
            }



            if (Session["AStatus"] != null)
            {
                Session["PageName"] = " Wallet / Wallet Transfer  ";
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomString(6);
                    FillWallet();
                }
            }
            else
                Response.Redirect("logout.aspx");
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
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
    public string GenerateRandomString(int iLength)
    {
        string sResult = "";
        try
        {
            Random rdm = new Random();
            char[] allowChrs = "123456789".ToCharArray();
            for (int i = 0; i <= iLength - 1; i++)
            {
                sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return sResult;
    }
    private void FillWallet()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr, "sp_GetWallettype");
            Rbtnwallet.DataSource = Ds.Tables[0];
            Rbtnwallet.DataValueField = "Actype";
            Rbtnwallet.DataTextField = "Walletname";
            Rbtnwallet.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    protected void BtnFundTransfer_Click(object sender, EventArgs e)
    {
        try
        {

            if (Rbtnwallet.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Please Select Wallet Type!!');", true);
                return;
            }

            string query = "";
            string formNo;
            string voucherNo = "";

            formNo = TxtFormNo.Text;

            DateTime dat = DateTime.Now;

            string date1 = string.Format("{0:dd-MMM-yyyy}", dat);

            string sessid = string.Format("{0:yyyyMMdd}", dat);

            lblError.Text = "";
            BtnFundTransfer.Enabled = false;

            string Remark = "";
            string sql = objDAL.IsoStart + "select IsNull (Max(VoucherNo+1),1) as VoucherNo from " + objDAL.DBName + "..TrnVoucher" + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                voucherNo = Convert.ToString(Dt.Rows[0]["VoucherNo"]);
            }
            if (RbtAccount.SelectedValue == "C")
            {

                Remark = TxtFund.Text + " Rs. Credited In " + Rbtnwallet.SelectedItem.Text + " To IdNo " + TxtIDNo.Text + "  ";
                query = query + "Exec sp_funtransfer '" + formNo + "','" + date1 + "',0,'" + formNo + "', '" + TxtFund.Text + "', " + "'" + TxtRemarks.Text + "','Credit/" + formNo + "','" + Rbtnwallet.SelectedValue + "', " + "'C', 1,'" + Session["UserID"] + "','" + Session["UserID"] + "'" + " ,'" + Session["UserName"] + "','" + Rbtnwallet.SelectedItem.Text + " Credit Transfer','" + Remark + "','" + formNo + "', " + HdnCheckTrnns.Value + "";
            }
            else if (RbtAccount.SelectedValue == "D")
            {
                Remark = TxtFund.Text + " Rs. Debited In " + Rbtnwallet.SelectedItem.Text + " To IdNo " + TxtIDNo.Text + "  ";

                query = "Exec sp_funtransfer '" + formNo + "','" + date1 + "','" + formNo + "',0, '" + TxtFund.Text + "', " + "'" + TxtRemarks.Text + "','Debit/" + formNo + "','" + Rbtnwallet.SelectedValue + "','D',1,'" + Session["UserID"] + "','" + Session["UserID"] + "' " + " ,'" + Session["UserName"] + "','" + Rbtnwallet.SelectedItem.Text + " Debit Transfer','" + Remark + "','" + formNo + "', " + HdnCheckTrnns.Value + "";
            }
            string StrSql = " Begin Try Begin Transaction " + query + " Commit Transaction  End Try   BEGIN CATCH       ROLLBACK Transaction END CATCH      ";

            int Result = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, StrSql));
            
            if (Result > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Amount " + RbtAccount.SelectedItem.Text + " Successfully!!');location.replace('FundTransfer.aspx');", true);
                TxtFund.Text = ""; TxtIDNo.Text = ""; TxtFormNo.Text = ""; TxtRemarks.Text = ""; LblMemName.Text = ""; LblAmount.Text = "";

                BtnFundTransfer.Enabled = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Amount Transfer UnSuccessfully!!');location.replace('fundtransfer.aspx');", true);
                BtnFundTransfer.Enabled = false;
            }
        }


        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void Check_IdNo()

    {
        try
        {
            string StrSql = objDAL.IsoStart + " Select LTRIM(RTRIM(Prefix)) +' '+ MemFirstName+' '+ MemLastName as MemName,FormNo,Mobl   ";
            StrSql = StrSql + " From " + objDAL.DBName + "..M_MemberMaster WHERE IDNO ='" + TxtIDNo.Text + "'" + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, StrSql).Tables[0];
            if (Dt.Rows.Count == 0)
            {
                LblMemName.Text = " Please enter correct Member ID.";
                TxtIDNo.Text = "";
                BtnFundTransfer.Enabled = false;
            }
            else
            {
                LblMemName.Text = Dt.Rows[0]["MemName"].ToString();
                LblMobl.Text = Dt.Rows[0]["Mobl"].ToString();
                //LblMemName.ForeColor = Drawing.Color.Black;
                TxtFormNo.Text = Dt.Rows[0]["FormNo"].ToString();
                BtnFundTransfer.Enabled = true;
                CheckBalance();
            }
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    protected void CheckBalance()
    {
        try
        {
            DataTable dt = new DataTable();
            string str = objDAL.IsoStart + " Select Balance From  dbo.ufnGetBalance(" + TxtFormNo.Text + ",'" + Rbtnwallet.SelectedValue + "')" + objDAL.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Session["FundBalance"] = dt.Rows[0]["Balance"];
                LblAmount.Text =  dt.Rows[0]["Balance"].ToString();
                LblAmount.Visible = true;
            }
            else
            {
                LblAmount.Visible = false;
            }
        }

        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void Rbtnwallet_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (TxtIDNo.Text != "")
            {
                CheckBalance();
            }
        }
        catch (Exception ex)
        {
            string scrname = "<SCRIPT language='javascript'>alert('" + ex.Message + "');</SCRIPT>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scrname, true);
        }
    }

    protected void TxtIDNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Check_IdNo();
        }
        catch (Exception ex)
        {
            string scrname = "<SCRIPT language='javascript'>alert('" + ex.Message + "');</SCRIPT>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scrname, true);
        }
    }
}
