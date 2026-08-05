using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PetroCardPurchaseNew : System.Web.UI.Page
{
    private string query;
    private DataTable Dt = new DataTable();
    DAL objDal = new DAL();
    private DAL Obj = new DAL();
    private string IsoStart;
    private string IsoEnd;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {

            }
            else
            {
                Response.Redirect("logout.aspx");
            }
            if (!IsPostBack)
            {
                HdnCheckTrnns.Value = GenerateRandomStringActive(6);
                FillKit();
                FundWalletGetBalance();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void FundWalletGetBalance()
    {
        try
        {
            DataTable dt = new DataTable();
            string str = " Select * From dbo.ufnGetBalance('" + Convert.ToInt32(Session["Formno"]) + "','R')";
            dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];
            if (dt.Rows.Count > 0)
            {
                HdnWalletBalance.Value = Convert.ToString(dt.Rows[0]["Balance"]);
            }
            else
            {
                HdnWalletBalance.Value = "0";
            }
            Session["ServiceWallet"] = HdnWalletBalance.Value;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public string GenerateRandomStringActive(int iLength)
    {
        try
        {
            Random rdm = new Random();
            char[] allowChrs = "123456789".ToCharArray();
            string sResult = "";

            for (int i = 0; i < iLength; i++)
            {
                sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
            }
            return sResult;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FillKit()
    {
        try
        {
            Dt = new DataTable();
            query = "Exec Sp_getKitPetroINRNew '" + Session["formno"] + "'";
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            Session["ShopFund"] = Dt;
            if (Dt.Rows.Count > 0)
            {
                rptKitDetails.DataSource = Dt;
                rptKitDetails.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnProceedToPay_Click(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;
        RepeaterItem item = (RepeaterItem)clickedButton.NamingContainer;
        TextBox txtAmount = item.FindControl("TxtAmount") as TextBox;
        HtmlGenericControl availableBal = item.FindControl("AvailableBal") as HtmlGenericControl;
        Label lblKitid = item.FindControl("LblKitid") as Label;
        if (txtAmount != null)
        {
            HdnWalletBalance.Value = availableBal.InnerText; // Get the text inside the spa
            string Amount = txtAmount.Text;
            string kitid = lblKitid.Text;

            string str = " select count(*) as Cnt from " + objDal.dBName + "..repurchincomeINR where formno = '" + Convert.ToInt32(Session["Formno"]) + "' AND kitid in (2,3,4)";
            DataTable dts = new DataTable();
            dts = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString, CommandType.Text, str).Tables[0];
            if (dts.Rows.Count > 0)
            {
                if (Convert.ToInt32(dts.Rows[0]["Cnt"]) > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Already Purchase Petro Card Kit.!');location.replace('Index.aspx');", true);
                    return;
                }
                else
                {
                    FUNPETROCARDPurchase(Amount, kitid);
                }
            }
            else
            {
                FUNPETROCARDPurchase(Amount, kitid);
            }
        }
    }
    public string GenerateRandomStringactive(int length)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        StringBuilder sResult = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            sResult.Append(allowChrs[rdm.Next(0, allowChrs.Length)]);
        }

        return sResult.ToString();
    }
    protected void FUNPETROCARDPurchase(string txtAmount, string kitid)
    {
        try
        {
            string query = "";
            string strSql = "Insert into Trnactive (Transid, Rectimestamp) values(" + HdnCheckTrnns.Value + ", GETDATE())";
            int updateEffect = objDal.SaveData(strSql);

            if (updateEffect > 0)
            {
                if (Convert.ToDecimal(Session["ServiceWallet"]) >= Convert.ToDecimal(txtAmount))
                {
                    var billNo = GenerateRandomStringactive(6);
                    string sql = "";
                    sql = "EXEC Sp_PaymentPetroCard '" + Session["idno"] + "','" + kitid + "', '','USDT', '" + Convert.ToInt32(Session["Formno"]) + "','" + txtAmount + "','" + billNo + "'";
                    DataTable dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                    if (dt.Rows[0]["Result"].ToString().ToUpper() == "SUCCESS")
                    {
                        FundWalletGetBalance();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Id Activation/Upgrade Successfully!!');location.replace('PETROCARDPurchase.aspx');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Id Activation/Upgrade Not Successfully!!');", true);
                    }
                }
                else
                {
                    //var scrName = "<SCRIPT language='javascript'>alert('Insufficient Balance.!');</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Insufficient Balance.!');location.replace('PETROCARDPurchase.aspx');", true);
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrName, false);
                }
            }
            else
            {
                Response.Redirect("PETROCARDPurchase.aspx");
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    //protected void rptKitDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
    //    {
    //        Button cmdSave1 = e.Item.FindControl("cmdSave1") as Button;
    //        if (cmdSave1 != null)
    //        {
    //            // Bind the JavaScript function dynamically
    //            BtnProceedToPay.Attributes.Add("onclick", DisableTheButton(this.Page, BtnProceedToPay));
    //        }
    //    }
    //}
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
}