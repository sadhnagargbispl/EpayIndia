using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
//using DocumentFormat.OpenXml.Wordprocessing;

//using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Activities.Expressions;
using System.Activities.Statements;
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

public partial class PetroCardFinalPurchaseNew : System.Web.UI.Page
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
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!string.IsNullOrEmpty(Request["kitid"]))
                {
                    kitid_ = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["kitid"]));
                }
                if (!Page.IsPostBack)
                {
                    Hdnkitid.Value = kitid_;
                    FillKit(Hdnkitid.Value);
                    txtMemberId.Text = Session["IdNo"] != null ? Session["IdNo"].ToString() : string.Empty;
                    GetName();
                    FillState();
                    FillGender();
                    Session["FinalWithdrawAmount"] = null;
                    Session["ReqAmount"] = null;
                    Session["OtpCount"] = 0;
                    Session["OtpTime"] = null;
                    Session["Retry"] = null;
                    Session["OTP_"] = null;
                    HdnCheckTrnns.Value = GenerateRandomString(6);

                    if (GetReqStatus() == false)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Request already Pending.!');location.replace('Index.aspx');", true);
                        return;
                    }
                    string str = " select count(*) as Cnt from " + ObjDal.dBName + "..repurchincomeINR where formno = '" + Convert.ToInt32(Session["Formno"]) + "' AND kitid in (10002,10003,10004)";
                    DataTable dts = new DataTable();
                    dts = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
                    if (dts.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dts.Rows[0]["Cnt"]) > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Already Purchase Petro Card Kit.!');location.replace('Index.aspx');", true);
                            return;
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("Logout.aspx");
            }

        }
        catch (Exception ex)
        {

        }
    }
    public bool GetReqStatus()
    {
        bool result = false;
        string str = " exec SP_GetReqidStatus '" + Convert.ToInt32(Session["Formno"]) + "' ";
        DataTable dts = new DataTable();
        dts = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
        if (dts.Rows.Count > 0)
        {
            if (Convert.ToInt32(dts.Rows[0]["Cnt"]) > 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }
        return result;
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

            str = ObjDal.Isostart + " Exec Sp_GetMemberName '" + txtMemberId.Text + "'" + ObjDal.IsoEnd;
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
            query = ObjDal.Isostart + " select * from  " + ObjDal.dBName + "..INR_kitmaster where kitid = '" + kitid + "'" + ObjDal.IsoEnd;
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
        string str = " select count(*) as Cnt from " + ObjDal.dBName + "..repurchincomeINR where formno = '" + Convert.ToInt32(Session["Formno"]) + "' AND kitid in (10002,10003,10004)";
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
                var billNo = GenerateRandomStringactive(6);
                string sql = "";
                sql = "EXEC Sp_PaymentPetroCardReqINR '" + txtMemberId.Text + "','" + Hdnkitid.Value + "', '','USDT', '" + Convert.ToInt32(Session["Formno"]) + "','" + txtAmount.Text + "','" + billNo + "',";
                sql += "'" + TxtMemberName.Text + "','" + TxtEmail.Text + "','" + Txtmonileno.Text + "','" + Txtpanno.Text + "','" + TxtDOB.Text + "','" + TxtAddress.Text + "','" + Txtpincode.Text + "',";
                sql += "'" + ddlState.SelectedItem.Text + "','" + TxtCity.Text + "','" + TxtDistrict.Text + "','" + TxtWhatsappNo.Text + "','" + DDlGender.SelectedValue + "';";
                DataTable dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                if (dt.Rows[0]["Result"].ToString().ToUpper() == "SUCCESS")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Petro Card Purchase Successfully.!');location.replace('PETROCARDPurchaseNew.aspx');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Petro Card Purchase Not Successfully!!');", true);
                }
            }
            else
            {
                Response.Redirect("PETROCARDPurchaseNew.aspx");
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
}
