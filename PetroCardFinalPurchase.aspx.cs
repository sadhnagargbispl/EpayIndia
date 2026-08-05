using DocumentFormat.OpenXml.Bibliography;
//using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PetroCardFinalPurchase : System.Web.UI.Page
{
    SqlConnection Conn;
    SqlCommand Comm;
    SqlDataAdapter Adp;
    SqlDataReader Dr;
    ModuleFunction objModuleFun = new ModuleFunction();
    DAL ObjDal = new DAL();
    string Query;
    int Todate;
    int BankID = 0;
    string BranchName = string.Empty;
    string PayName = string.Empty;
    string IFSCode = string.Empty;
    string Acno = string.Empty;
    string PanNo = string.Empty;
    string MobileNo = string.Empty;
    string ISpan = string.Empty;
    clsGeneral clsgen = new clsGeneral();
    string scrname = string.Empty;
    string IsoStart;
    string IsoEnd;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string kitid_;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.BtnSubmit.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnSubmit));

            // ── Login check ──
            if (Session["Status"] == null || Session["Status"].ToString() != "OK")
            {
                Response.Redirect("login.aspx");
                return;
            }

            // ── KitID decrypt ──
            if (!string.IsNullOrEmpty(Request["kitid"]))
            {
                try
                {
                    kitid_ = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["kitid"]));
                }
                catch
                {
                    kitid_ = string.Empty;   // decrypt fail = invalid
                }
            }

            if (Page.IsPostBack)
                return;

            // ══ STEP 1 : KitID valid hai? ══
            if (string.IsNullOrEmpty(kitid_) ||
                !new string[] { "12", "13", "14" }.Contains(kitid_.Trim()))
            {
                Response.Redirect("PETROCARDPurchase.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // ══ STEP 2 : PAN verification mandatory ══
            string kycUrl = ConfigurationManager.AppSettings["KycPageUrl"] ?? "KycUpload.aspx";

            string panSql = "Exec USP_GetPanKycStatus " + Convert.ToInt32(Session["Formno"]);
            DataTable dtPan = SqlHelper.ExecuteDataset(constr1, CommandType.Text, panSql).Tables[0];

            if (dtPan.Rows.Count == 0 || Convert.ToInt32(dtPan.Rows[0]["IsVerified"]) != 1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key",
                    "alert('PAN verification is mandatory before purchasing a Petro Card Kit.');"
                    + "location.replace('" + kycUrl.Replace("'", "\\'") + "');", true);
                return;
            }

            // ══ STEP 3 : Pehle se purchase to nahi kiya? ══
            if (IsPetroKitAlreadyPurchased())
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key",
                    "alert('You have already purchased a Petro Card Kit. Only one kit is allowed per member.');"
                    + "location.replace('PETROCARDPurchase.aspx');", true);
                return;
            }

            // ══ STEP 4 : Wallet + Kit amount load ══
            FillWallettype();

            Hdnkitid.Value = kitid_;
            FillKit(Hdnkitid.Value);          // txtAmount + lblCardAmount set

            FundWalletGetBalance();
            AvailableBal.Text = Amount().ToString("N2");

            // ══ STEP 5 : Ab balance check (amount load hone ke BAAD) ══
            if (!GetAmountStatus())
            {
                decimal bal = Amount();
                decimal amt = SafeAmount();
                decimal shortfall = amt - bal;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key",
                    "alert('Insufficient Balance!\\n\\n"
                    + "Kit Amount        : Rs. " + amt.ToString("N2") + "\\n"
                    + "Available Balance : Rs. " + bal.ToString("N2") + "\\n"
                    + "Shortfall         : Rs. " + shortfall.ToString("N2") + "\\n\\n"
                    + "Please add funds to your wallet and try again.');"
                    + "location.replace('PETROCARDPurchase.aspx');", true);
                return;
            }

            // ══ STEP 6 : Form data ══
            txtMemberId.Text = Session["IdNo"] != null ? Session["IdNo"].ToString() : string.Empty;

            GetName();
            FillState();
            FillGender();

            // PAN verified value se auto-fill (GetName ke BAAD)
            string verifiedPan = Convert.ToString(dtPan.Rows[0]["PanNo"]);
            if (!string.IsNullOrEmpty(verifiedPan))
            {
                Txtpanno.Text = verifiedPan;
                Txtpanno.ReadOnly = true;
            }

            Session["FinalWithdrawAmount"] = null;
            Session["ReqAmount"] = null;
            Session["OtpCount"] = 0;
            Session["OtpTime"] = null;
            Session["Retry"] = null;
            Session["OTP_"] = null;

            HdnCheckTrnns.Value = GenerateRandomString(6);
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            ObjDal.WriteToFile(text + ex.Message + Environment.NewLine + ex.StackTrace);

            Response.Redirect("PETROCARDPurchase.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
    /// <summary>Member ne pehle se koi Petro Kit (12/13/14) le rakha hai?</summary>
    private bool IsPetroKitAlreadyPurchased()
    {
        try
        {
            string str = " Select Count(*) As Cnt From " + ObjDal.dBName + "..repurchincome "
                       + " Where formno = " + Convert.ToInt32(Session["Formno"])
                       + " And kitid In (12,13,14) ";

            DataTable dts = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];

            return dts.Rows.Count > 0 && Convert.ToInt32(dts.Rows[0]["Cnt"]) > 0;
        }
        catch (Exception ex)
        {
            ObjDal.WriteToFile("IsPetroKitAlreadyPurchased: " + ex.Message);
            return true;   // shak ho to aage mat badhne do
        }
    }

    /// <summary>txtAmount ko safely decimal me convert karta hai</summary>
    private decimal SafeAmount()
    {
        decimal amt;
        return decimal.TryParse(txtAmount.Text, out amt) ? amt : 0;
    }
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        this.BtnSubmit.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnSubmit));
    //        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
    //        {
    //            if (!string.IsNullOrEmpty(Request["kitid"]))
    //            {
    //                kitid_ = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["kitid"]));
    //            }
    //            if (!Page.IsPostBack)
    //            {
    //                FillWallettype();
    //                if (!GetAmountStatus())
    //                {
    //                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('You Are Not Authorised For Petro Card Purchase.Because Of Your Wallet Balance Is Low.!');location.replace('Index.aspx');", true);
    //                    return;
    //                }
    //                FundWalletGetBalance();
    //                AvailableBal.Text = Amount().ToString();
    //                Hdnkitid.Value = kitid_;
    //                FillKit(Hdnkitid.Value);
    //                txtMemberId.Text = Session["IdNo"] != null ? Session["IdNo"].ToString() : string.Empty;
    //                GetName();
    //                FillState();
    //                FillGender();
    //                Session["FinalWithdrawAmount"] = null;
    //                Session["ReqAmount"] = null;
    //                Session["OtpCount"] = 0;
    //                Session["OtpTime"] = null;
    //                Session["Retry"] = null;
    //                Session["OTP_"] = null;
    //                HdnCheckTrnns.Value = GenerateRandomString(6);
    //                string str = " select count(*) as Cnt from " + ObjDal.dBName + "..repurchincome where formno = '" + Convert.ToInt32(Session["Formno"]) + "' AND kitid in (12,13,14)";
    //                DataTable dts = new DataTable();
    //                dts = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
    //                if (dts.Rows.Count > 0)
    //                {
    //                    if (Convert.ToInt32(dts.Rows[0]["Cnt"]) > 0)
    //                    {
    //                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Already Purchase Petro Card Kit.!');location.replace('Index.aspx');", true);
    //                        return;
    //                    }
    //                }
    //            }

    //        }
    //        else
    //        {
    //            Response.Redirect("login.aspx");
    //        }

    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    private void FillWallettype()
    {
        try
        {
            DataTable dt = new DataTable();

            // Update query for stored procedure without formno parameter
           string query = IsoStart + "Exec Sp_GetWalletTypePetroCardINR " + IsoEnd;

            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                ddlWalletType.DataSource = dt;
                ddlWalletType.DataTextField = "WalletName";
                ddlWalletType.DataValueField = "Actype";
                ddlWalletType.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Handle the exception if needed
        }
    }
    protected void ddlWalletType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!GetAmountStatus())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('You Are Not Authorised For Petro Card Purchase.Because Of Your Wallet Balance Is Low.!');location.replace('Index.aspx');", true);
            return;
        }
        FundWalletGetBalance();
        AvailableBal.Text = Amount().ToString();
    }
    private Decimal Amount()
    {
        try
        {
            Decimal RtrVal = 0;
            string Str = ObjDal.Isostart + "Select balance From dbo.ufnGetBalance('" + Session["FormNo"] + "','" + ddlWalletType.SelectedValue + "')" + ObjDal.IsoEnd;
            SqlConnection cnn = new SqlConnection(constr1);
            using (SqlDataReader dr = SqlHelper.ExecuteReader(cnn, CommandType.Text, Str))
            {
                if (dr.Read())
                {
                    RtrVal = Convert.ToDecimal(dr["Balance"]);
                }
            }
            return RtrVal;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private bool GetAmountStatus()
    {
        try
        {
            bool result = false;
            DataTable dt = new DataTable();
            string strSql = ObjDal.Isostart + "Select balance From dbo.ufnGetBalance('" + Session["FormNo"] + "','" + ddlWalletType.SelectedValue + "')" + ObjDal.IsoEnd;
            DataSet ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            dt = ds.Tables[0];

            if (Convert.ToDouble(dt.Rows[0]["balance"]) < Convert.ToDouble(txtAmount.Text))
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FillGender()
    {
        try
        {
            var obj = new DAL();
            string query = ObjDal.Isostart + "Exec Sp_getGender " + ObjDal.IsoEnd;
            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            if (dt.Rows.Count > 0)
            {
                DDlGender.DataSource = dt;
                DDlGender.DataValueField = "GenderCode";
                DDlGender.DataTextField = "GenderName";
                DDlGender.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    private void FillState()
    {
        try
        {
            var obj = new DAL();
            string query = ObjDal.Isostart + "SELECT StateCode, StateName FROM " + ObjDal.dBName + "..M_STateDivMaster WHERE ActiveStatus = 'Y' AND RowStatus = 'Y' ORDER BY StateCode" + ObjDal.IsoEnd;
            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ddlState.DataSource = dt;
                ddlState.DataValueField = "StateCode";
                ddlState.DataTextField = "StateName";
                ddlState.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }
    public string GetName()
    {
        try
        {
            string str = string.Empty;
            DataTable dt = new DataTable();

            str = ObjDal.Isostart + " Exec Sp_GetMemberNamer '" + txtMemberId.Text + "'" + ObjDal.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];

            if (dt.Rows.Count == 0)
            {
                txtMemberId.Text = string.Empty;
                TxtMemberName.Text = string.Empty;
                HdnMemberMacAdrs.Value = string.Empty;
                HdnMemberTopupseq.Value = string.Empty;

                string scrName = "<SCRIPT language='javascript'>alert('Invalid ID Does Not Exist');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
            }
            else
            {
                if (dt.Rows[0]["Isblock"].ToString() == "Y")
                {
                    txtMemberId.Text = string.Empty;
                    TxtMemberName.Text = string.Empty;
                    HdnMemberMacAdrs.Value = string.Empty;
                    HdnMemberTopupseq.Value = string.Empty;

                    string scrName = "<SCRIPT language='javascript'>alert('This Id is blocked. Please Contact Admin.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
                    return string.Empty;
                }
                else
                {
                    TxtMemberName.Text = dt.Rows[0]["memname"].ToString();
                    HdnMemberMacAdrs.Value = dt.Rows[0]["MacAdrs"].ToString();
                    HdnMemberTopupseq.Value = dt.Rows[0]["Topupseq"].ToString();
                    MemberStatus.Value = dt.Rows[0]["ActiveStatus"].ToString();
                    hdnFormno.Value = dt.Rows[0]["Formno"].ToString();
                    TxtEmail.Text = dt.Rows[0]["Email"].ToString();
                    
                    if (TxtEmail.Text == "")
                    {
                        TxtEmail.Enabled = true;
                    }
                    else
                    {
                        TxtEmail.Enabled = false;
                    }
                    Txtmonileno.Text = dt.Rows[0]["mobl"].ToString();
                    if (Txtmonileno.Text == "0")
                    {
                        Txtmonileno.Enabled = true;
                    }
                    else
                    {
                        Txtmonileno.Enabled = false;
                    }
                    LblMobile.Text = string.Empty;
                    Txtpanno.Text = dt.Rows[0]["panno"].ToString();
                    if (Txtpanno.Text == "")
                    {
                        Txtpanno.Enabled = true;
                    }
                    else
                    {
                        Txtpanno.Enabled = false;
                    }
                    return "OK";
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
        }

        return string.Empty;
    }
    protected void FillKit(string kitid)
    {
        try
        {
            string query = "";
            DataTable dt;
            dt = new DataTable();
            query = ObjDal.Isostart + " select * from  " + ObjDal.dBName + "..M_kitmaster where kitid = '" + kitid + "'" + ObjDal.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            if (dt.Rows.Count > 0)
            {
                txtAmount.Text = dt.Rows[0]["KitAmount"].ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    public string GenerateRandomString(int iLength)
    {
        string currentDatetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int randomNumber = new Random().Next(0, 999);
        string formattedDatetime = currentDatetime + randomNumber.ToString().PadLeft(3, '0');
        return formattedDatetime;
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please Wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(((Page)pge).GetPostBackEventReference(btn));
        sb.Append(";");
        return sb.ToString();
    }
    static bool IsValidMobileNumber(string number)
    {
        return Regex.IsMatch(number, @"^[1-9]\d{9}$");
    }
    static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    }
    static bool IsValidPAN(string pan)
    {
        return Regex.IsMatch(pan.ToUpper(), @"^[A-Z]{5}[0-9]{4}[A-Z]$");
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (ddlWalletType.SelectedValue == "Z")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Please Select Wallet Type.!');", true);
            return;
        }

        if (!GetAmountStatus())
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('You do not have enough balance for Purchasing.');location.replace('Index.aspx');", true);
            return;
        }
        if (DDlGender.SelectedValue == "Z")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Please Select Gender.!');", true);
            return;
        }
        if (TxtEmail.Text == "")
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Enter Email Id.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        else if ((IsValidEmail(TxtEmail.Text)) == false)
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Enter valid Email ID.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        else if (Txtmonileno.Text == "0")
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Enter Mobile No.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        else if ((IsValidMobileNumber(Txtmonileno.Text)) == false)
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Enter valid mobile Number.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        else if (TxtWhatsappNo.Text == "0")
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Enter Whatsapp No.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        else if ((IsValidMobileNumber(TxtWhatsappNo.Text)) == false)
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Enter valid Whatsapp Number.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        else if (Txtpanno.Text == "")
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Enter Pan No.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        else if ((IsValidPAN(Txtpanno.Text)) == false)
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Enter valid Pan No.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        else if (TxtDOB.Text == "")
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Enter Date Of Birth.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        else if (ddlState.SelectedValue == "0")
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Select State.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        else if (TxtCity.Text == "")
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Enter City.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        else if (TxtDistrict.Text == "")
        {
            string scrname = "<SCRIPT language='javascript'>alert('Please Enter District.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        string str = " select count(*) as Cnt from " + ObjDal.dBName + "..repurchincome where formno = '" + Convert.ToInt32(Session["Formno"]) + "' AND kitid in (12,13,14)";
        DataTable dts = new DataTable();
        dts = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
        if (dts.Rows.Count > 0)
        {
            if (Convert.ToInt32(dts.Rows[0]["Cnt"]) > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Already Purchase Petro Card Kit.!');location.replace('Index.aspx');", true);
                return;
            }
            else
            {
                FUNPETROCARDPurchase();
            }
        }
        else
        {
            FUNPETROCARDPurchase();
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
    protected void FUNPETROCARDPurchase()
    {
        try
        {
            string query = "";
            string strSql = "Insert into Trnactive (Transid,Rectimestamp)values(" + HdnCheckTrnns.Value + ", GETDATE())";
            int updateEffect = ObjDal.SaveData(strSql);

            if (updateEffect > 0)
            {
                if (Convert.ToDecimal(Session["ServiceWallet"]) >= Convert.ToDecimal(txtAmount.Text))
                {
                    var billNo = GenerateRandomStringactive(6);
                    string sql = "";
                    sql = "EXEC Sp_PaymentPetroCardINR '" + txtMemberId.Text + "','" + Hdnkitid.Value + "', '','USDT', '" + Convert.ToInt32(Session["Formno"]) + "','" + txtAmount.Text + "','" + billNo + "',";
                    sql += "'" + TxtMemberName.Text + "','" + TxtEmail.Text + "','" + Txtmonileno.Text + "','" + Txtpanno.Text + "','" + TxtDOB.Text + "','" + TxtAddress.Text + "','" + Txtpincode.Text + "',";
                    sql += "'" + ddlState.SelectedItem.Text + "','" + TxtCity.Text + "','" + TxtDistrict.Text + "','" + TxtWhatsappNo.Text + "','" + ddlWalletType.SelectedValue + "','" + DDlGender.SelectedValue + "';";
                    DataTable dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                    if (dt.Rows[0]["Result"].ToString().ToUpper() == "SUCCESS")
                    {
                        FundWalletGetBalance();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Petro Card Purchase Successfully.!');location.replace('PETROCARDPurchase.aspx');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Petro Card Purchase Not Successfully!!');", true);
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
            ObjDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void FundWalletGetBalance()
    {
        try
        {
            DataTable dt = new DataTable();
            string str = " Select * From dbo.ufnGetBalance('" + Convert.ToInt32(Session["Formno"]) + "','" + ddlWalletType.SelectedValue + "')";
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
}
