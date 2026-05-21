using ClosedXML.Excel;
using System;
using System.CodeDom;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Activities.Expressions;
using System.Activities;
using System.ServiceModel.Activities;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;

public partial class Registartion : System.Web.UI.Page
{
    private double _dblAvailLeg = 0;
    private cls_DataAccess dbConnect;
    private DAL ObjDAL = new DAL();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dRead;
    public string DsnName, UserName, Passw;
    private string strQuery, strCaptcha;
    private DataTable tmpTable = new DataTable();
    private int minSpnsrNoLen, minScrtchLen;
    private double Upln, dblSpons, dblState, dblBank, dblIdNo;
    private string dblDistrict, dblTehsil, IfSC;
    private string dblPlan;
    private DateTime CurrDt;
    private string scrname;
    private string LastInsertID = "";
    private string Email = "";
    private string InVoiceNo;
    private int SupplierId;
    private string BillNo;
    private string TaxType;
    private string BillDate;
    private int SBillNo;
    private string SoldBy = "WR";
    private string FType;
    private string Password = "";
    private string membername = "";
    private string clsGeneral = "";
    private clsGeneral dbGeneral = new clsGeneral();
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private SqlConnection cnn;
    DataTable Dt = new DataTable();
    string IsoStart;
    string IsoEnd;
    protected void getData()
    {
        cls_DataAccess dbConnect = new cls_DataAccess(constr);
        DAL objdal = new DAL();
        try
        {
            SqlDataReader dRead;
            SqlCommand cmd;
            DataTable dtCompany = new DataTable();
            if (Application["dtCompany"] == null)
            {
                if (dbConnect.cnnObject == null)
                {
                    dbConnect.OpenConnection();
                }
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter();
                string strQ = objdal.Isostart + " select * from " + objdal.dBName + " ..M_CompanyMaster" + objdal.IsoEnd;
                adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
                adp.Fill(ds);
                dtCompany = ds.Tables[0];
                Application["dtCompany"] = dtCompany;
            }
            else
            {
                if (dbConnect.cnnObject == null)
                {
                    dbConnect.OpenConnection();
                }
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter();
                string strQ = objdal.Isostart + " select * from " + objdal.dBName + " ..M_CompanyMaster" + objdal.IsoEnd;
                adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
                adp.Fill(ds);
                dtCompany = ds.Tables[0];
                Application["dtCompany"] = dtCompany;
            }

            if (dtCompany.Rows.Count > 0)
            {
                Session["CompName"] = dtCompany.Rows[0]["CompName"];
                Session["CompAdd"] = dtCompany.Rows[0]["CompAdd"];
                Session["CompWeb"] = string.IsNullOrEmpty(dtCompany.Rows[0]["WebSite"].ToString()) ? "index.asp" : dtCompany.Rows[0]["WebSite"];
                Session["Title"] = dtCompany.Rows[0]["CompTitle"];
                Session["CompMail"] = dtCompany.Rows[0]["CompMail"];
                Session["CompMobile"] = dtCompany.Rows[0]["MobileNo"];
                Session["ClientId"] = dtCompany.Rows[0]["smsSenderId"];
                Session["SmsId"] = dtCompany.Rows[0]["smsUserNm"];
                Session["SmsPass"] = dtCompany.Rows[0]["smPass"];
                Session["MailPass"] = dtCompany.Rows[0]["mailPass"];
                Session["MailHost"] = dtCompany.Rows[0]["mailHost"];
                Session["AdminWeb"] = dtCompany.Rows[0]["AdminWeb"];
                Session["CompCST"] = dtCompany.Rows[0]["CompCSTNo"];
                Session["CompState"] = dtCompany.Rows[0]["CompState"];
                Session["CompDate"] = Convert.ToDateTime(dtCompany.Rows[0]["RecTimeStamp"]).ToString("dd-MMM-yyyy");
                Session["Spons"] = "KL223344";
                Session["CompWeb1"] = dtCompany.Rows[0]["WebSite"];
                Session["CompMovieWeb"] = "";
                Session["SmsAPI"] = "";
                Session["CompShortUrl"] = dtCompany.Rows[0]["UrlShort"];
                Session["LogoUrl"] = dtCompany.Rows[0]["LogoUrl"];
            }
            else
            {
                Session["CompName"] = "";
                Session["CompAdd"] = "";
                Session["CompWeb"] = "";
                Session["Title"] = "Welcome";
            }

            DataTable dtConfig = new DataTable();
            if (Application["dtConfig"] == null)
            {
                if (dbConnect.cnnObject == null)
                {
                    dbConnect.OpenConnection();
                }
                string strQ = objdal.Isostart + " select * from " + objdal.dBName + "..M_ConfigMaster " + objdal.IsoEnd;
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
                adp.Fill(ds);
                dtConfig = ds.Tables[0];
                Application["dtConfig"] = dtConfig;
            }
            else
            {
                dtConfig = (DataTable)Application["dtConfig"];
            }

            if (dtConfig.Rows.Count > 0)
            {
                Session["IsGetExtreme"] = dtConfig.Rows[0]["IsGetExtreme"];
                Session["IsTopUp"] = dtConfig.Rows[0]["IsTopUp"];
                Session["IsSendSMS"] = dtConfig.Rows[0]["IsSendSMS"];
                Session["IdNoPrefix"] = dtConfig.Rows[0]["IdNoPrefix"];
                Session["IsFreeJoin"] = dtConfig.Rows[0]["IsFreeJoin"];
                Session["IsStartJoin"] = dtConfig.Rows[0]["IsStartJoin"];
                Session["JoinStartFrm"] = dtConfig.Rows[0]["JoinStartFrm"];
                Session["IsSubPlan"] = dtConfig.Rows[0]["IsSubPlan"];
                Session["Logout"] = dtConfig.Rows[0]["LogoutPg"];
            }
            else
            {
                Session["IsGetExtreme"] = "N";
                Session["IsTopUp"] = "N";
                Session["IsSendSMS"] = "N";
                Session["IdNoPrefix"] = "";
                Session["IsFreeJoin"] = "N";
                Session["IsStartJoin"] = "N";
                Session["JoinStartFrm"] = "01-Sep-2011";
                Session["IsSubPlan"] = "N";
                Session["Logout"] = "https://djiomart.com/";
            }
        }
        catch (Exception ex)
        {
            // handle exception
        }
        DataTable dtMsession = new DataTable();
        if (Application["dtMsession"] == null)
        {
            if (dbConnect.cnnObject == null)
            {
                dbConnect.OpenConnection();
            }
            DataSet ds = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter();
            string strQ = objdal.Isostart + " select Max(SEssid) as SessID from " + objdal.dBName + "..D_Monthlypaydetail  " + objdal.IsoEnd;
            adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
            adp.Fill(ds);
            dtMsession = ds.Tables[0];
            Application["dtMsession"] = dtMsession;
        }
        else
        {
            dtMsession = (DataTable)Application["dtMsession"];
        }

        if (dtMsession.Rows.Count > 0)
        {
            Session["MaxSessn"] = dtMsession.Rows[0]["SessID"];
        }
        else
        {
            Session["MaxSessn"] = "";
        }

        DataTable dtsession = new DataTable();
        if (Application["dtsession"] == null)
        {
            if (dbConnect.cnnObject == null)
            {
                dbConnect.OpenConnection();
            }
            DataSet ds = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter();
            string strQ = objdal.Isostart + " select Max(SEssid) as SessID from " + objdal.dBName + "..m_SessnMaster  " + objdal.IsoEnd;
            adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
            adp.Fill(ds);

            dtsession = ds.Tables[0];
            Application["dtsession"] = dtsession;
        }
        else
        {
            dtsession = (DataTable)Application["dtsession"];
        }

        if (dtsession.Rows.Count > 0)
        {
            Session["CurrentSessn"] = dtsession.Rows[0]["SessID"];
        }
        else
        {
            Session["CurrentSessn"] = "";
        }
        if (dbConnect.cnnObject != null)
        {
            if (dbConnect.cnnObject.State == ConnectionState.Open)
            {
                dbConnect.cnnObject.Close();
            }
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CmdSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.CmdSave));
        //this.BtnOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnOtp));
        //this.ResendOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.ResendOtp));
        try
        {
            if (Application["WebStatus"] == null)
            {
                if (Application["WebStatus"] != null && Application["WebStatus"].ToString() == "N")
                {
                    Session.Abandon();
                    Response.Redirect("default.aspx", false);
                }
            }
            cnn = new SqlConnection(constr1);
            dbConnect = new cls_DataAccess((string)Application["Connect"]);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            txtUplinerId.Text = (txtUplinerId.Text).Replace("'", "").Replace("=", "").Replace(";", "");
            string sr = "";
            string[] sbstr;
            string Key = "";
            string K = "";
            if (!Page.IsPostBack)
            {
                Session["OtpCount"] = 0;
                Session["OtpTime"] = null;
                Session["OTP_"] = null;
                Session["Retry"] = null;
                HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                getData();
                Session["OtpCount"] = 0;
                ClrCtrl();
                RbtnLegNo.Items.Add("Left");
                RbtnLegNo.Items.Add("Right");

                RbtnLegNo.Items[0].Selected = true;

                if (!string.IsNullOrEmpty(Request.QueryString["s"]))
                {
                    K = Request["s"];
                    K = K.Replace(" ", "+");
                    sr = Crypto.Decrypt(K);

                    sbstr = sr.Split('/');
                    string UplinerFormno = sbstr[1];

                    string s = IsoStart + " select * from " + ObjDAL.dBName + "..M_MemberMaster where Formno='" + UplinerFormno + "'" + IsoEnd;
                    DataSet Ds = new DataSet();
                    Ds = SqlHelper.ExecuteDataset(cnn, CommandType.Text, s);

                    DataTable dt;
                    dt = new DataTable();
                    dt = Ds.Tables[0];
                    if (dt.Rows.Count > 0)
                        txtUplinerId.Text = dt.Rows[0]["Idno"].ToString();
                    string LegNo = sbstr[3];

                    txtUplinerId.ReadOnly = true;
                    txtRefralId.Text = Session["Idno"].ToString();

                    if (LegNo == "1")
                    {
                        RbtnLegNo.SelectedIndex = 0;
                    }
                    else
                    {
                        RbtnLegNo.SelectedIndex = 1;
                    }
                    RbtnLegNo.Enabled = false;
                    Session["iLeg"] = LegNo;
                }

                if (Request.QueryString["ref"] != null)
                {
                    string req = Request.QueryString["ref"].Replace(" ", "+");
                    string str = Crypto.Decrypt(req);
                    string[] rfAr = str.Split('/');

                    if (rfAr.Length >= 1)
                    {
                        if (!string.IsNullOrEmpty(rfAr[0]) && rfAr[1] == "0")
                        {
                            txtRefralId.Text = GetIDno(rfAr[0]);
                        refLink:
                            ;
                        }
                        else if (!string.IsNullOrEmpty(rfAr[0]) && rfAr[1] == "1")
                        {
                            txtRefralId.Text = GetIDno(rfAr[0]);
                            RbtnLegNo.SelectedIndex = 0;
                            RbtnLegNo.Enabled = false;
                            RbtnLegNo.Items[1].Attributes.Add("style", "visibility:hidden");

                        refLink:
                            ;
                        }
                        else if (!string.IsNullOrEmpty(rfAr[0]) && rfAr[1] == "2")
                        {
                            txtRefralId.Text = GetIDno(rfAr[0]);
                            RbtnLegNo.SelectedIndex = 1;
                            RbtnLegNo.Enabled = false;
                            RbtnLegNo.Items[0].Attributes.Add("style", "visibility:hidden");

                        refLink:
                            ;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(Request.QueryString["RefFormNo"]))
                {
                    txtRefralId.Text = Get_IDNoUp(Request.QueryString["RefFormNo"]);
                    TxtWalletaddress.Text = HiddenField4.Value;
                refLink:
                    ;


                    //TxtWalletaddress.ReadOnly = true;
                }
                if (txtRefralId.Text.Trim() != "")
                {
                    FillReferral(cnn);
                    txtRefralId.ReadOnly = true;
                }

                FillPaymode(cnn);

                dbGeneral.Fill_Date_box(ddlDOBdt, ddlDOBmnth, ddlDOBYr, 1940, DateTime.Now.AddYears(-18).Year);
                dbGeneral.Fill_Date_box(DDlMDay, DDLMMonth, DDLMYear, 1940, DateTime.Now.Year);
                FillBankMaster(cnn);
                // FillStateMaster()
                FillCountryMasterName();
                //FillCountryMasterCode();
                FindSession();
                GetConfigDtl(cnn);
                // sendSMS()
                vsblCtrl(false, true);
            }

            try
            {
                Session["Dsessid"] = 0;
            }
            catch
            {
            }


            if (Session["IsGetExtreme"].ToString() == "N")
            {
                rwSpnsr.Visible = false;
            }
            else
            {
                rwSpnsr.Visible = false;
            }

        }
        catch (Exception ex)
        {

        }
    }
    private string ClearInject(string strObj)
    {
        strObj = strObj.Replace(";", "").Replace("'", "").Replace("=", "");
        return strObj.Trim();
    }
    private string GetIDno(string Mid)
    {
        string Result = "";
        try
        {
            DataTable dt = new DataTable();

            string strSql = IsoStart + "Select IDNO from " + ObjDAL.dBName + "..M_MemberMAster Where MID = '" + Mid + "' " + IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];

            if ((dt.Rows.Count > 0))
                Result = dt.Rows[0]["IDNO"].ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return Result;
    }
    private string DisableTheButton(System.Web.UI.Control pge, System.Web.UI.Control btn)
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
    private string Get_IDNoUp(string myFormNo)
    {
        try
        {
            string idNo = "";
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string strSql = "SELECT idno FROM M_MemberMaster WHERE formno = '" + myFormNo + "'";

            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                idNo = dt.Rows[0]["idno"].ToString();
            }

            return idNo;
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
            return null; // Ensure a return in case of exception
        }
    }
    private void FillCountryMasterName()
    {
        try
        {
            DataTable dt = new DataTable();
            string strQuery = "Exec Sp_GetCountry";
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
            ddlCountryNAme.DataSource = dt;
            ddlCountryNAme.DataValueField = "CId";
            ddlCountryNAme.DataTextField = "CountryName";
            ddlCountryNAme.DataBind();
            // ddlCountryName.SelectedIndex = 90;
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }
    public string GenerateRandomStringJoining(int iLength)
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
    private void FillPaymode(SqlConnection cnn)
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string strSql = IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_PayModeMaster WHERE ActiveStatus='Y' " + IsoEnd;

            if (Session["DtPayMode"] == null)
            {
                ds = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql);
                dt = ds.Tables[0];
                Session["DtPayMode"] = dt;
            }
            else
            {
                dt = (DataTable)Session["DtPayMode"];
            }

            if (dt.Rows.Count > 0)
            {
                DdlPaymode.DataSource = dt;
                DdlPaymode.DataValueField = "PID";
                DdlPaymode.DataTextField = "Paymode";
                DdlPaymode.DataBind();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    private void GetConfigDtl(SqlConnection cnn)
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string strSql = IsoStart + "select *  from " + ObjDAL.dBName + "..M_ConfigMaster " + IsoEnd;

            //if (Session["DtConfigDetail"] == null)
            //{
            ds = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql);
            dt = ds.Tables[0];
            Session["DtConfigDetail"] = dt;
            //}
            //else
            //{
            //    dt = (DataTable)Session["DtConfigDetail"];
            //}

            if (dt.Rows.Count > 0)
            {
                Session["IsGetExtreme"] = dt.Rows[0]["IsGetExtreme"];
                Session["IsTopUp"] = dt.Rows[0]["IsTopUp"];
                Session["IsSendSMS"] = dt.Rows[0]["IsSendSMS"];
                Session["IdNoPrefix"] = dt.Rows[0]["IdNoPrefix"];
                Session["IsFreeJoin"] = dt.Rows[0]["IsFreeJoin"];
                Session["IsStartJoin"] = dt.Rows[0]["IsStartJoin"];
                Session["JoinStartFrm"] = dt.Rows[0]["JoinStartFrm"];
                Session["IsSubPlan"] = dt.Rows[0]["IsSubPlan"];
            }
            else
            {
                Session["IsGetExtreme"] = "N";
                Session["IsTopUp"] = "N";
                Session["IsSendSMS"] = "N";
                Session["IdNoPrefix"] = "";
                Session["IsFreeJoin"] = "N";
                Session["IsStartJoin"] = "N";
                Session["JoinStartFrm"] = "01-Sep-2011";
                Session["IsSubPlan"] = "N";
            }
        }
        catch
        {
            Session["CompName"] = "";
            Session["CompAdd"] = "";
            Session["CompWeb"] = "";
        }
    }
    protected void vsblCtrl(bool isVsbl, bool isOnlyDv)
    {
        try
        {
            if (!isOnlyDv)
            {
                txtUplinerId.Enabled = !isVsbl;
                txtRefralId.Enabled = !isVsbl;
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    public string GenerateRandomString(int iLength)
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
    private void ClrCtrl()
    {
        // txtAddLn2.Text = "";
        txtAddLn1.Text = "";
        txtEMailId.Text = "";
        txtFNm.Text = "";
        txtFrstNm.Text = "";
        txtMobileNo.Text = "";
        txtNominee.Text = "";
        txtPanNo.Text = "";
        txtPhNo.Text = "";
        txtPinCode.Text = "";
        txtRelation.Text = "";
        txtUplinerId.Text = "";
        lblUplnrNm.Text = "";
        ddlDistrict.Text = "";
        ddlTehsil.Text = "";
        TxtBranchName.Text = "";
        TxtAccountNo.Text = "";
        txtIfsCode.Text = "";
        txtRefralId.Text = "";
        lblRefralNm.Text = "";
        txtUplinerId.Enabled = true;
        txtRefralId.Enabled = true;


        RbtnLegNo.Enabled = true;
    }
    private void FillBankMaster(SqlConnection Cnn)
    {
        try
        {
            DataTable dt = new DataTable();

            if (Session["DtBankMaster"] == null)
            {
                DataSet Ds = new DataSet();
                string strSql = IsoStart + "SELECT BankCode as Bid, BANKNAME as Bank FROM " + ObjDAL.dBName + "..M_BankMaster WHERE ACTIVESTATUS='Y' and Rowstatus='Y' ORDER BY BankName" + IsoEnd;
                Ds = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, strSql);
                dt = Ds.Tables[0];
                Session["DtBankMaster"] = dt;
            }
            else
            {
                dt = (DataTable)Session["DtBankMaster"];
            }

            if (dt.Rows.Count > 0)
            {
                CmbBank.DataSource = dt;
                CmbBank.DataValueField = "Bid";
                CmbBank.DataTextField = "Bank";
                CmbBank.DataBind();
                CmbBank.SelectedIndex = 0;
            }

            TxtBank.Text = CmbBank.SelectedItem.Text;
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    public string Validt_SpnsrDtl()
    {
        string Validt_SpnsrDtls = string.Empty;

        try
        {
            // Sanitize input
            txtRefralId.Text = txtRefralId.Text.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
            txtUplinerId.Text = txtUplinerId.Text.Trim().Replace("'", "").Replace("=", "").Replace(";", "");

            // Check Referral ID
            if (!string.IsNullOrEmpty(txtRefralId.Text))
            {
                try
                {
                    DataTable dt = new DataTable();
                    string strSql = IsoStart +
                        "Select FormNo, MemFirstName + ' ' + MemLastName as MemName, ActiveStatus " +
                        "from " + ObjDAL.dBName + "..M_MemberMaster where Idno='" + txtRefralId.Text + "'" + IsoEnd;

                    using (DataSet ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql))
                    {
                        dt = ds.Tables[0];
                    }

                    if (dt.Rows.Count == 0)
                    {
                        ShowAlert("Sponsor ID Not Exist.");
                        vsblCtrl(false, true);
                        return Validt_SpnsrDtls;
                    }
                    else
                    {
                        Session["Kitid"] = 1;
                        Session["Bv"] = 0;
                        Session["JoinStatus"] = "N";
                        Session["RP"] = 0;
                        Validt_SpnsrDtls = "OK";
                        Session["Refral"] = dt.Rows[0]["FormNo"].ToString();
                        lblRefralNm.Text = dt.Rows[0]["MemName"].ToString();
                    }
                }
                catch (Exception)
                {
                    ShowAlert("Please check sponsor ID.");
                    return Validt_SpnsrDtls;
                }
            }
            else
            {
                ShowAlert("Check Sponsor ID.");
                txtRefralId.Focus();
                return Validt_SpnsrDtls;
            }

            // Check Upliner ID
            if (Session["IsGetExtreme"].ToString() == "N")
            {
                if (!string.IsNullOrEmpty(txtUplinerId.Text))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        string strSql = IsoStart +
                            "Select FormNo, MemFirstName + ' ' + MemLastName as MemName " +
                            "from " + ObjDAL.dBName + "..M_MemberMaster where Idno='" + txtUplinerId.Text + "'" + IsoEnd;

                        using (DataSet ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql))
                        {
                            dt = ds.Tables[0];
                        }

                        if (dt.Rows.Count == 0)
                        {
                            ShowAlert("Sponsor ID Not Exist.");
                            vsblCtrl(false, true);
                            return Validt_SpnsrDtls;
                        }
                        Session["Uplnr"] = dt.Rows[0]["FormNo"].ToString();
                        Validt_SpnsrDtls = "OK";
                        lblUplnrNm.Text = dt.Rows[0]["MemName"].ToString();
                    }
                    catch (Exception)
                    {
                        ShowAlert("Incorrect Place under ID.");
                        return Validt_SpnsrDtls;
                    }
                }
                else
                {
                    txtUplinerId.Text = "0";
                    lblUplnrNm.Text = string.Empty;
                    Session["Uplnr"] = "0";
                }

                if (!ValidatePlacement())
                {
                    ShowAlert("Place Under Does Not Exist In Sponsor Downline!!");
                    vsblCtrl(false, true);
                    return Validt_SpnsrDtls;
                }
            }
            if (Session["IsGetExtreme"].ToString() == "N" && !string.IsNullOrWhiteSpace(txtUplinerId.Text))
            {
                if (!checkAvailLeg())
                {
                    Validt_SpnsrDtls = string.Empty;
                    vsblCtrl(false, true);
                    return Validt_SpnsrDtls;
                }
            }
            RbtnLegNo.Enabled = false;
            txtUplinerId.Enabled = false;
            txtRefralId.Enabled = false;
        }
        catch (Exception)
        {
            // Handle unexpected errors
        }

        return Validt_SpnsrDtls;
    }
    private void ShowAlert(string message)
    {
        string scrname = "<SCRIPT language='javascript'>alert('" + message + "');</SCRIPT>";
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Alert", scrname, false);
    }
    private bool ValidatePlacement()
    {
        if (Session["Refral"].ToString() != Session["Uplnr"].ToString())
        {
            DataTable dt = new DataTable();
            string strSql = IsoStart +
                "Select * from " + ObjDAL.dBName + "..R_MemTreeRelation " +
                "where FormNo=" + Session["Refral"] + " And FormNoDwn=" + Session["Uplnr"] + " " + IsoEnd;

            using (DataSet ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql))
            {
                dt = ds.Tables[0];
            }

            if (dt.Rows.Count == 0)
            {
                return false;
            }
        }
        return true;
    }
    private void FindSession()
    {
        try
        {
            Session["SessID"] = 1;
            return;
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }

        try
        {
            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = ObjDAL.Isostart + "Select Max(SessId) as SessId from " + ObjDAL.dBName + "..M_SessnMaster  " + ObjDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0]; ;
            dt = Dt;
            if (dt.Rows.Count > 0)
            {
                Session["SessID"] = dt.Rows[0]["SessID"];
            }
            else
            {
                errMsg.Text = "Session Not Exist. Please Enter New Session.";
                return;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private bool checkAvailLeg()
    {
        try
        {
            int iLegNo = 0;
            int iformNo = 0;

            if (RbtnLegNo.SelectedIndex == 0)
            {
                iLegNo = 1;
            }
            else if (RbtnLegNo.SelectedIndex == 1)
            {
                iLegNo = 2;
            }
            else
            {
                string scrname = "<SCRIPT language='javascript'>alert('Choose Position.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return false;
            }

            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = IsoStart + "Select * from " + ObjDAL.dBName + "..M_MemberMaster where IdNo='" + txtUplinerId.Text + "'" + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            dt = Ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                iformNo = Convert.ToInt32(dt.Rows[0]["FormNo"]);
            }
            else
            {
                errMsg.Text = "Check Placeunder Id.";
                string scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return false;
            }

            DataTable dt12 = new DataTable();
            DataSet Ds12 = new DataSet();
            string strSql12 = IsoStart + "SELECT COUNT(*) AS CNT FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE uplnformno = " + iformNo + " And LegNo = " + iLegNo + IsoEnd;
            Ds12 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql12);
            dt12 = Ds12.Tables[0];

            if (dt12.Rows.Count > 0 && Convert.ToInt32(dt12.Rows[0]["CNT"]) > 0)
            {
                errMsg.Text = (iLegNo == 1 ? "LEFT" : "RIGHT") + " Position already used, please select correct Position!";
                string scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                CmdSave.Enabled = false;
                return false;
            }
            else
            {
                errMsg.Visible = false;
                CmdSave.Enabled = true;
                _dblAvailLeg = iformNo;
                return true;
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
            return false;
        }
    }
    protected void txtUplinerId_TextChanged(object sender, EventArgs e)
    {
        FillSponsor(ref cnn);
    }
    private void FillSponsor(ref SqlConnection Cnn)
    {
        try
        {
            errMsg.Text = "";
            lblErrEpin.Text = "";
            int i = 0;
            txtUplinerId.Text = txtUplinerId.Text.Trim().Replace(";", "").Replace("'", "").Replace("=", "");

            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = IsoStart + " Select FormNo,MemFirstName + ' ' + MemLastName as MemName from " + ObjDAL.dBName +
                            "..M_MemberMaster where IDNo='" + txtUplinerId.Text + "'" + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, strSql);
            dt = Ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                lblUplnrNm.Text = dt.Rows[0]["MemName"].ToString();
                Session["Uplnr"] = dt.Rows[0]["FormNo"].ToString();
                i += 1;
            }
            else
            {
                errMsg.Text = "Invalid PlaceUnder ID!!";
                lblErrEpin.Text = "Invalid PlaceUnder ID!!";
                string scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            }

            if (i == 1)
            {
                checkAvailLeg();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    private void FillReferral(SqlConnection Cnn)
    {
        try
        {
            lblErrEpin.Text = "";
            errMsg.Text = "";
            txtRefralId.Text = txtRefralId.Text.Trim().Replace(";", "").Replace("'", "").Replace("=", "");

            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = IsoStart + "Select FormNo,MemFirstName + ' ' + MemLastName as MemName,ActiveStatus from " +
                            ObjDAL.dBName + "..M_MemberMaster where IDNo='" + txtRefralId.Text + "' and IsBlock='N' " + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, strSql);
            dt = Ds.Tables[0];

            if (dt.Rows.Count == 0)
            {
                string scrname = "<SCRIPT language='javascript'>alert('No such record/This ID is Flashed./This Id Not Active!!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                txtRefralId.Text = "";
                return;
            }
            //else if (dt.Rows[0]["ActiveStatus"].ToString() == "N")
            //{
            //    string scrname = "<SCRIPT language='javascript'>alert('This ID is not eligible for sponsor.');</SCRIPT>";
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            //    return;
            //}
            else
            {
                lblRefralNm.Text = dt.Rows[0]["MemName"].ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    protected void CmdCancel_Click(object sender, EventArgs e)
    {
        ClrCtrl();
    }
    protected void txtRefralId_TextChanged(object sender, EventArgs e)
    {
        try
        {
            FillReferral(cnn);
        }
        catch (Exception ex)
        {
            // Handle the exception if necessary
        }
    }
    protected void txtMobileNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtMobileNo.Text))
            {
                string moblno = txtMobileNo.Text;
                string check = moblno.Substring(0, 1);

                if (check == "0")
                {
                    txtMobileNo.Text = "";
                    CmdSave.Enabled = true;
                    chkterms.Checked = false;
                    string scrname = "<SCRIPT language='javascript'>alert('Invalid Mobile No.!');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(txtMobileNo.Text))
            {
                DataTable Dt1 = new DataTable();
                DataSet Dsmob = new DataSet();
                string strSql = IsoStart + "select Count(mobl) as mobileno from " + ObjDAL.dBName + "..M_Membermaster where Mobl='" + txtMobileNo.Text.Trim() + "' " + IsoEnd;
                Dsmob = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql);
                Dt1 = Dsmob.Tables[0];

                if (Convert.ToInt32(Dt1.Rows[0]["mobileno"]) >= 10000)
                {
                    txtMobileNo.Text = "";
                    CmdSave.Enabled = true;
                    chkterms.Checked = false;
                    string scrname = "<SCRIPT language='javascript'>alert('Already Registered by this Mobile Number.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
        }
    }
    protected void txtEMailId_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable DtEmail = new DataTable();
            DataSet DsEmail = new DataSet();
            string strSql = IsoStart + "select Count(Email) as Email from " + ObjDAL.dBName + "..M_Membermaster where Email='" + txtEMailId.Text.Trim() + "' " + IsoEnd;
            DsEmail = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            DtEmail = DsEmail.Tables[0];

            if (Convert.ToInt32(DtEmail.Rows[0]["Email"]) >= 100000)
            {
                txtEMailId.Text = "";
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                LblEmainID.Visible = true;
                LblEmainID.Text = "Already Registered by this Email ID.!";
                return;
            }
            else
            {
                LblEmainID.Visible = false;
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
        }
    }
    protected void ddlCountryNAme_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCountryMasterCode();
    }
    private void FillCountryMasterCode()
    {
        try
        {
            DataTable dt = new DataTable();
            string strQuery = IsoStart + "SELECT StdCode FROM " + ObjDAL.dBName + "..M_CountryMaster WHERE ACTIVESTATUS='Y' AND Cid = '" + ddlCountryNAme.SelectedValue + "' " + IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];

            if (dt.Rows.Count > 0)
            {
                ddlMobileNAme.Text = dt.Rows[0]["StdCode"].ToString();
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
        }
    }
    public void SaveIntoDB()
    {
        try
        {
            char IsPanCard;
            string strQry = "";
            string strDOB, strDOM, strDOJ, s;
            int iLeg;
            char cGender, cMarried; // Declare variables
            cGender = 'M';          // Assign value
            cMarried = 'N';        // Assign value

            string hostIp = Context.Request.UserHostAddress; // Retrieve and assign IP address
            string HostIp = Context.Request.UserHostAddress.ToString();
            int DistrictCode, CityCode, VillageCode;
            CmdSave.Enabled = false;
            string s1 = "";
            if (txtEMailId.Text == "")
            {
                chkterms.Checked = false;
                CmdSave.Enabled = true;
                scrname = "<SCRIPT language='javascript'>alert('Enter Email-Id.');" + "</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Enter Email-Id.');", true);
                return;
            }
            if (txtMobileNo.Text == "")
            {
                chkterms.Checked = false;
                CmdSave.Enabled = true;
                scrname = "<SCRIPT language='javascript'>alert('Enter Mobile No.');" + "</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Enter Mobile No.');", true);
                return;
            }
            if (!string.IsNullOrWhiteSpace(txtEMailId.Text)) // Check if txtEMailId is not empty
            {
                DataTable dtEmail = new DataTable(); // Initialize DataTable
                DataSet dsEmail = new DataSet(); // Initialize DataSet
                string strSql = IsoStart + " select Count(Email) as Email from " + ObjDAL.dBName + "..M_Membermaster where Email='" + txtEMailId.Text.Trim() + "' " + IsoEnd;

                dsEmail = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql); // Execute the SQL query
                dtEmail = dsEmail.Tables[0]; // Get the first DataTable from DataSet

                if (Convert.ToInt32(dtEmail.Rows[0]["Email"]) >= 1) // Check if the email already exists
                {
                    CmdSave.Enabled = true; // Enable the save command
                    chkterms.Checked = false; // Uncheck the terms checkbox
                    string scrname = "<script language='javascript'>alert('Already Registered by this Email ID.');</script>"; // Prepare alert script
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false); // Register script block
                    return; // Exit the method
                }
            }
            if (!string.IsNullOrWhiteSpace(txtMobileNo.Text)) // Check if txtMobileNo is not empty
            {
                DataTable dt1 = new DataTable(); // Initialize DataTable
                DataSet dsmob = new DataSet(); // Initialize DataSet
                string strSql = IsoStart + "select Count(mobl) as mobileno from " + ObjDAL.dBName + "..M_Membermaster where Mobl='" + txtMobileNo.Text.Trim() + "' " + IsoEnd;

                dsmob = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql); // Execute the SQL query
                dt1 = dsmob.Tables[0]; // Get the first table from the dataset

                if (Convert.ToInt32(dt1.Rows[0]["mobileno"]) >= 1) // Check if the mobile number is already registered
                {
                    CmdSave.Enabled = true; // Enable the save command
                    chkterms.Checked = false; // Uncheck the terms checkbox
                    string scrname = "<script language='javascript'>alert('Already Registered by this Mobile Number.');</script>"; // Prepare alert script
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false); // Register script block
                    return; // Exit the method
                }
            }

            try
            {
                if (Validt_SpnsrDtl() == "OK")
                {
                    iLeg = Convert.ToInt32(Session["iLeg"]);
                    if ((RbtnLegNo.SelectedIndex == 0))
                        iLeg = 1;
                    else if ((RbtnLegNo.SelectedIndex == 1))
                        iLeg = 2;
                    else
                    {
                        chkterms.Checked = false;
                        CmdSave.Enabled = true;
                        scrname = "<SCRIPT language='javascript'>alert('Choose Position.');" + "</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Choose Position.');", true);
                        RbtnLegNo.Enabled = true;
                        return;
                    }
                    TxtPasswd.Text = GenerateRandomString(6);

                    if (TxtPasswd.Text == "")
                    {
                        chkterms.Checked = false;
                        CmdSave.Enabled = true;
                        scrname = "<SCRIPT language='javascript'>alert('Enter Password.');" + "</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Enter Password.');", true);
                        return;
                    }
                    string q = "";
                    int i = 0;
                    DataTable Dt;
                    int BankCode = 0;
                    if (CmbBank.SelectedItem.Text.ToUpper() == "OTHERS") // Check if the selected bank is "OTHERS"
                    {
                        if (!string.IsNullOrWhiteSpace(TxtBank.Text)) // Check if TxtBank is not empty
                        {
                            DataTable dt = new DataTable(); // Initialize DataTable
                            DataSet ds = new DataSet(); // Initialize DataSet
                            q = IsoStart + "Select * from " + ObjDAL.dBName + "..M_BankMaster where BankName='" + TxtBank.Text.Trim() + "' and Activestatus='Y' and RowStatus='Y' " + IsoEnd;

                            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, q); // Execute the SQL query
                            dt = ds.Tables[0]; // Get the first table from the dataset

                            if (dt.Rows.Count == 0) // If no records found
                            {
                                q = "";
                                q = "insert into M_BankMaster (BankCode, BankName, AcNo, IFSCode, Remarks, ActiveStatus, LastModified, UserCode, UserId, IPAdrs, RowStatus) " +
                                    "Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode, '" + TxtBank.Text.ToUpper() + "', '0', '0', " +
                                    "'', 'Y', 'Add by " + Session["IdNo"] + " at " + DateTime.Now.ToString() + "', '" + Session["MemName"] + "', " +
                                    "'" + Convert.ToString(Session["FormNo"]) + "', '', 'Y' From M_BankMaster";

                                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, q)); // Execute the insert query

                                if (i > 0) // If the insert was successful
                                {
                                    string qs = IsoStart + " select Max(BankCode) as BankCode from " + ObjDAL.dBName + "..M_BankMaster where ActiveStatus='Y' and RowStatus='Y'" + IsoEnd;
                                    DataTable dtRead = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qs).Tables[0]; // Get the max BankCode

                                    if (dtRead.Rows.Count > 0)
                                    {
                                        dblBank = Convert.ToInt32(dtRead.Rows[0]["BankCode"]); // Get the BankCode
                                    }
                                }
                            }
                            else // If a record exists
                            {
                                dblBank = Convert.ToInt32(dt.Rows[0]["BankCode"]); // Get the existing BankCode
                            }
                        }
                    }
                    else // If the selected bank is not "OTHERS"
                    {
                        dblBank = Convert.ToInt32(CmbBank.SelectedValue); // Get the selected value
                    }

                    int AreaCode = 0;
                    AreaCode = 0;
                    string RegestType = "";
                    if (RbCategory.SelectedValue == "IN") // Check if the selected value is "IN"
                    {
                        RegestType = "IN"; // Assign "IN" to RegestType
                    }
                    else
                    {
                        RegestType = CbSubCategory.SelectedValue; // Assign the selected value of CbSubCategory to RegestType
                    }

                    int PostalAreaCode = 0;
                    strDOB = ddlDOBdt.Text + "-" + ddlDOBmnth.Text + "-" + ddlDOBYr.Text; // Concatenate day, month, and year for date of birth
                    strDOM = DDlMDay.Text + "-" + DDLMMonth.Text + "-" + DDLMYear.Text; // Concatenate day, month, and year for date of marriage
                    strDOJ = DateTime.Now.ToString("dd-MMM-yyyy"); // Format the server date as "dd-MMM-yyyy"
                    string dblDistrict = ClearInject(ddlDistrict.Text.ToUpper()); // Get and clear injected text for district
                    string dblTehsil = ClearInject(ddlTehsil.Text.ToUpper()); // Get and clear injected text for tehsil

                    if (string.IsNullOrEmpty(dblDistrict))
                    {
                        dblDistrict = "";
                    }

                    dblState = 0;
                    DistrictCode = 0;
                    CityCode = 0;
                    VillageCode = 0;
                    IfSC = ClearInject(txtIfsCode.Text.ToUpper());

                    dblPlan = "0";
                    InVoiceNo = "0";

                    if (Session["SessID"] == null || (int)Session["SessID"] == 0)
                    {
                        FindSession();
                    }

                    string Name = "";
                    string fathername = "";

                    if (RbCategory.SelectedValue == "IN")
                    {
                        Name = ClearInject(txtFrstNm.Text.ToUpper());
                        fathername = ClearInject(txtFNm.Text.ToUpper());
                    }
                    else
                    {
                        fathername = ClearInject(txtFrstNm.Text.ToUpper());
                        Name = ClearInject(TxtCompanyName.Text.ToUpper());
                    }
                    if (!string.IsNullOrWhiteSpace(TxtAccountNo.Text) || !string.IsNullOrWhiteSpace(txtIfsCode.Text.Trim()))
                    {
                        if (string.IsNullOrWhiteSpace(TxtAccountNo.Text))
                        {
                            chkterms.Checked = false;
                            CmdSave.Enabled = true;
                            string script = "alert('Enter Account No.');";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
                            return;
                        }

                        if (CmbBank.SelectedValue == "0") // Assuming SelectedValue is a string
                        {
                            chkterms.Checked = false;
                            CmdSave.Enabled = true;
                            string script = "alert('Choose Bank Name.');";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(TxtBranchName.Text))
                        {
                            chkterms.Checked = false;
                            CmdSave.Enabled = true;
                            string script = "alert('Enter Branch Name.');";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(DDLAccountType.SelectedValue))
                        {
                            chkterms.Checked = false;
                            CmdSave.Enabled = true;
                            string script = "alert('Enter Account Name.');";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(txtIfsCode.Text))
                        {
                            chkterms.Checked = false;
                            CmdSave.Enabled = true;
                            string script = "alert('Enter IFSC Code.');";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
                            return;
                        }
                    }

                    var Strquery = "Insert into Trnjoining (Transid) values(" + HdnCheckTrnns.Value + ")";
                    int UpdateData = 0;
                    UpdateData = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Strquery));
                    if (UpdateData > 0)
                    {
                        //TxtPasswd.Text = GenerateRandomString(6);
                        strQry = "INSERT INTO m_memberMaster(SessId, IdNo, CardNo, FormNo, KitId, UpLnFormNo, RefId, LegNo, RefLegNo, RefFormNo, " +
                   "MemFirstName, MemLastName, MemRelation, MemFName, MemDOB, MemGender, MemOccupation, NomineeName, Address1, Address2, Post, " +
                   "Tehsil, City, District, StateCode, CountryId, PinCode, PhN1, Fax, Mobl, MarrgDate, Passw, Doj, Relation, PanNo, " +
                   "BankID, MICRCode, BranchName, EMail, BV, UpGrdSessId, E_MainPassw, EPassw, ActiveStatus, billNo, RP, HostIp, " +
                   "PID, Paymode, ChDDNo, ChDDBankID, ChDDBank, ChddDate, ChDDBranch, IsPanCard, AadharNo, AadharNo2, AAdharNo3, Fld5, walletaddress, usercode) " +
                   "VALUES (" + Convert.ToInt32(Session["SessID"]) + ", '0', 0, 0, " + Convert.ToInt32(Session["Kitid"]) + ", " +
                   Convert.ToInt32(Session["Uplnr"]) + ", 0, '" + iLeg + "', 0, " + Convert.ToInt32(Session["Refral"]) + ", '" + ClearInject(txtFrstNm.Text.ToUpper()) + "', " +
                   "'', '" + CmbType.SelectedValue + "', '" + ClearInject(txtFNm.Text.ToUpper()) + "', '" + strDOB + "', '" + cGender + "', '', " +
                   "'" + ClearInject(txtNominee.Text.ToUpper()) + "', '" + ClearInject(txtAddLn1.Text.ToUpper()) + "', '', '', '" + dblTehsil + "', " +
                   "'" + dblTehsil + "', '" + dblDistrict + "', " + dblState + ", " + ddlCountryNAme.SelectedValue + ", '" + txtPinCode.Text + "', " +
                   "'" + txtPhNo.Text + "', 'CHOOSE ACCOUNT TYPE', '" + txtMobileNo.Text + "', '" + strDOM + "', '" + ClearInject(TxtPasswd.Text) + "', " +
                   "GETDATE(), '" + ClearInject(txtRelation.Text.ToUpper()) + "', '" + ClearInject(txtPanNo.Text.ToUpper()) + "', " + dblBank + ", " +
                   "'" + (ClearInject(TxtMICR.Text.ToUpper())) + "', '" + (TxtBranchName.Text.ToUpper()) + "', '" + ClearInject(txtEMailId.Text) + "', " +
                   Convert.ToInt32(Session["Bv"]) + ", 0, '" + ClearInject(TxtPasswd.Text) + "', '" + ClearInject(TxtPasswd.Text) + "', '" + Session["JoinStatus"] + "', " +
                   "'" + InVoiceNo + "', '" + Session["RP"] + "', '" + HostIp + "', " + Convert.ToInt32(DdlPaymode.SelectedValue) + ", " +
                   "'" + (DdlPaymode.SelectedItem.Text.ToUpper()) + "', '" + ClearInject(TxtDDNo.Text) + "', '0', '" + ClearInject(TxtIssueBank.Text.ToUpper()) + "', " +
                   "'" + (TxtDDDate.Text) + "', '" + ClearInject(TxtIssueBranch.Text) + "', 'N', '" + ClearInject(TxtAAdhar1.Text) + "', " +
                   "'" + ClearInject(TxtAadhar2.Text) + "', '" + ClearInject(TxtAadhar3.Text) + "', '" + Session["TransIDJoin"] + "', '" + ClearInject(TxtWalletaddress.Text) + "', '" + ddlMobileNAme.Text + "')";

                        int isOk = 0;
                        int retryqry = 0;
                    Savedata:
                        ;
                        isOk = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strQry));
                        LastInsertID = "0";
                        if ((isOk > 0))
                        {
                            string membername = "";
                            string SPONSORID1 = "";
                            string SPONSORnAME = "";
                            string Doj = "";
                            string kitamount = "";
                            string Email = "";
                            string Password = "";
                            string EPassword = "";
                            DataTable Dtsms = new DataTable();
                            string strSql = string.Empty;

                            // Execute stored procedure to get login details
                            strSql = IsoStart + " EXEC Sp_GetLoginDetail " + IsoEnd;
                            Dtsms = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];

                            if (Dtsms.Rows.Count > 0)
                            {
                                membername = Dtsms.Rows[0]["MemfirstName"].ToString() + " " + Dtsms.Rows[0]["MemLastName"].ToString();
                                SPONSORID1 = Dtsms.Rows[0]["SPONSORID"].ToString();
                                SPONSORnAME = Dtsms.Rows[0]["SPONSORnAME"].ToString();
                                Doj = Dtsms.Rows[0]["JoiningDate"].ToString();
                                kitamount = Dtsms.Rows[0]["kitamount"].ToString();
                                Email = Dtsms.Rows[0]["Email"].ToString();
                                LastInsertID = Dtsms.Rows[0]["IDNO"].ToString();
                                Password = Dtsms.Rows[0]["Passw"].ToString();
                                EPassword = Dtsms.Rows[0]["ePassw"].ToString();
                                Session["Kit"] = Dtsms.Rows[0]["IsBill"];

                                //FUND_LOGIN_CHECK(Dtsms.Rows[0]["IDNO"].ToString(), Dtsms.Rows[0]["Passw"].ToString(), Dtsms.Rows[0]["formno"].ToString());
                            }
                            else
                            {
                                LastInsertID = "10001";
                            }


                            CmdSave.Enabled = true;
                            SendToMemberMail(LastInsertID, Email, membername, Password, EPassword);
                            Session["LASTID"] = LastInsertID;
                            Session["Join"] = "YES";
                            Response.Redirect("Welcome.Aspx?IDNo=" + LastInsertID, false);
                        }
                        else
                        {
                            if (retryqry <= 2)
                            {
                                retryqry += 1;
                                goto Savedata;
                            }
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            scrname = "<SCRIPT language='javascript'>alert('Try Again Later.');" + "</SCRIPT>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Try Again Later.');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('This id already register.!');location.replace('Registration.aspx');", true);
                        return;
                    }

                }
            }
            catch (Exception e)
            {
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                string scrname = "<SCRIPT language='javascript'>alert('" + e.Message + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + e.Message + "');", true);

                string path = HttpContext.Current.Request.Url.AbsoluteUri;
                string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
                ObjDAL.WriteToFile(text + e.Message);
                Response.Write("Try later.");
                return;
            }

        }
        catch (Exception ex)
        {
            dbConnect.closeConnection();
        }
    }
    protected void RbtnLegNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        checkAvailLeg();
    }
    public bool SendMail(string otp)
    {
        try
        {
            string strMsg = "";
            string emailAddress = txtEMailId.Text.Trim();
            System.Net.Mail.MailAddress sendFrom = new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            System.Net.Mail.MailAddress sendTo = new System.Net.Mail.MailAddress(emailAddress);
            System.Net.Mail.MailMessage myMessage = new System.Net.Mail.MailMessage(sendFrom, sendTo);

            strMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\"> " +
                     "<tr>" +
                     "<td>" +
                     "Your OTP for Registration is <span style=\"font-weight: bold;\">" + otp + "</span> (valid for 5 minutes)." +
                     "<br />" +
                     "</td>" +
                     "</tr>" +
                     "</table>";

            myMessage.Subject = "Thanks For Connecting!!!";
            myMessage.Body = strMsg;
            myMessage.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.EnableSsl = false;
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());

            smtp.Send(myMessage);

            txtRefralId.Enabled = false;
            txtUplinerId.Enabled = false;
            TxtWalletaddress.Enabled = false;
            txtFrstNm.Enabled = false;
            txtMobileNo.Enabled = false;
            txtEMailId.Enabled = false;
            ddlCountryNAme.Enabled = false;
            RbtnLegNo.Enabled = false;
            chkterms.Enabled = false;

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public bool SendToMemberMail(string IdNo, string Email, string MemberName, string Password, string TransactionPassword)
    {
        try
        {
            System.Net.Mail.MailAddress sendFrom =
                new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
            System.Net.Mail.MailAddress sendTo =
                new System.Net.Mail.MailAddress(Email);
            System.Net.Mail.MailMessage myMessage =
                new System.Net.Mail.MailMessage(sendFrom, sendTo);

            string strMsg = @"
<!DOCTYPE html>
<html lang='en'>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width,initial-scale=1.0'>
<title>Welcome to ePay Digital India</title>
<style>
  body{margin:0;padding:0;background:#f4f6fb;font-family:Arial,Helvetica,sans-serif}
  table{border-collapse:collapse}
  .outer{width:100%;background:#f4f6fb;padding:32px 16px}
  .card{width:560px;margin:0 auto;background:#ffffff;border-radius:12px;overflow:hidden;border:1px solid #e2e8f0}
  .header{background:#0f2b5b;padding:32px 36px 28px}
  .header-brand{margin:0 0 8px;font-size:12px;color:#93c5fd;letter-spacing:0.5px;font-family:Arial,sans-serif}
  .header-title{margin:0;font-size:20px;font-weight:bold;color:#ffffff;line-height:1.35;font-family:Arial,sans-serif}
  .body{padding:28px 36px}
  .p{margin:0 0 16px;font-size:14px;color:#1e293b;line-height:1.75;font-family:Arial,sans-serif}
  .name{color:#1a4db3;font-weight:bold}
  .cred-table{width:100%;background:#f8faff;border-radius:8px;margin:0 0 20px;border:1px solid #dbeafe}
  .cred-row-top{padding:12px 20px 8px;border-bottom:1px solid #e2e8f0}
  .cred-row-mid{padding:8px 20px;border-bottom:1px solid #e2e8f0}
  .cred-row-bot{padding:8px 20px 12px}
  .cred-label{font-size:13px;color:#64748b;font-family:Arial,sans-serif}
  .cred-val{font-size:13px;font-weight:bold;color:#1e293b;font-family:'Courier New',monospace;text-align:right}
  .btn{display:inline-block;background:#1a4db3;color:#ffffff;text-decoration:none;padding:11px 26px;border-radius:8px;font-size:14px;font-weight:bold;font-family:Arial,sans-serif;margin:0 0 20px}
  .warn{font-size:13px;color:#92400e;background:#fff7ed;border:1px solid #fed7aa;border-radius:8px;padding:12px 16px;margin:0 0 20px;line-height:1.65;font-family:Arial,sans-serif}
  .footer{border-top:1px solid #e2e8f0;padding:20px 36px}
  .footer-p{margin:0;font-size:13px;color:#64748b;line-height:1.65;font-family:Arial,sans-serif}
  .footer-name{color:#1e293b;font-weight:bold}
</style>
</head>
<body>
<div class='outer'>
  <div class='card'>

    <div class='header'>
      <p class='header-brand'>ePay Digital India Pvt. Ltd.</p>
      <p class='header-title'>Welcome to ePay Digital India &ndash;<br>Your Account is Ready</p>
    </div>

    <div class='body'>
      <p class='p'>Dear <span class='name'>" + MemberName + @"</span>,</p>
      <p class='p'>Welcome to <strong>ePay Digital India Pvt. Ltd.</strong> &mdash; your gateway to smart digital services and earning opportunities.</p>
      <p class='p'>Your account has been successfully created. Please find your login credentials below:</p>

      <table class='cred-table'>
        <tr>
          <td class='cred-row-top'>
            <table width='100%'><tr>
              <td class='cred-label'>User ID</td>
              <td class='cred-val'>" + IdNo + @"</td>
            </tr></table>
          </td>
        </tr>
        <tr>
          <td class='cred-row-mid'>
            <table width='100%'><tr>
              <td class='cred-label'>Login Password</td>
              <td class='cred-val'>" + Password + @"</td>
            </tr></table>
          </td>
        </tr>
        <tr>
          <td class='cred-row-bot'>
            <table width='100%'><tr>
              <td class='cred-label'>Transaction Password</td>
              <td class='cred-val'>" + TransactionPassword + @"</td>
            </tr></table>
          </td>
        </tr>
      </table>

      <a href='https://epayindia.in/' class='btn' style='color: white;'>Login Now &rarr;</a>

      <p class='warn'>For your security, we strongly recommend changing your password after your first login.</p>

      <p class='p' style='margin:0'>If you need any assistance, our support team is always here to help.</p>
    </div>

    <div class='footer'>
      <p class='footer-p'>Warm regards,<br><span class='footer-name'>Team ePay Digital India Pvt. Ltd.</span></p>
    </div>

  </div>
</div>
</body>
</html>";

            myMessage.Subject = "Welcome to ePay Digital India – Your Account is Ready";
            myMessage.Body = strMsg;
            myMessage.IsBodyHtml = true;

            System.Net.Mail.SmtpClient smtp =
                new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials =
                new System.Net.NetworkCredential(
                    Session["CompMail"].ToString(),
                    Session["MailPass"].ToString()
                );

            smtp.Send(myMessage);
            return true;
        }
        catch (Exception)
        {
            Response.Write("Mail could not be sent. Please try again later.");
            return false;
        }
    }
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(
            email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );
    }
    public static bool IsValidMobile(string mobile)
    {
        if (string.IsNullOrWhiteSpace(mobile))
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(
            mobile,
            @"^\d{10}$"
        );
    }
    public string ValidateRegistration(string sponsorId, string name, string country, string mobile, string email, bool isTermsAccepted)
    {
        if (string.IsNullOrWhiteSpace(sponsorId))
            return "Sponsor ID is required";
        if (string.IsNullOrWhiteSpace(name))
            return "Name is required";
        if (string.IsNullOrWhiteSpace(country) || country == "0")
            return "Please select country";
        if (string.IsNullOrWhiteSpace(mobile))
            return "Mobile No. is required";
        if (!IsValidMobile(mobile))
            return "Enter valid mobile number";
        if (string.IsNullOrWhiteSpace(email))
            return "Email is required";
        if (!IsValidEmail(email))
            return "Enter valid email address";

        if (!isTermsAccepted)
            return "Please accept terms & conditions";

        return "OK";
    }
    protected void CmdSave_Click(object sender, EventArgs e)
    {
        try
        {
            string result = ValidateRegistration(txtRefralId.Text.Trim(), txtFrstNm.Text.Trim(), ddlCountryNAme.SelectedValue, txtMobileNo.Text.Trim(), txtEMailId.Text.Trim(), chkterms.Checked);
            if (result != "OK")
            {
                string scrname = "<SCRIPT language='javascript'>alert('" + result + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            else
            {
                SaveIntoDB();
            }
            //if (!chkterms.Checked)
            //{
            //    string scrname = "<SCRIPT language='javascript'>alert('Please select Terms and Conditions');</SCRIPT>";
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
            //    return;
            //}
            //else
            //{

            //    SaveIntoDB();
            //}

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnOtp_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    string scrname = "";
        //    string transPassw = TxtOtp.Text;
        //    transPassw = transPassw.Trim();
        //    DataTable dt1 = new DataTable();
        //    ObjDAL = new DAL();
        //    Session["OtpCount"] = Convert.ToInt32(Session["OtpCount"]) + 1;

        //    if (Session["OTP_"] != null && Session["OTP_"].ToString() == TxtOtp.Text.Trim())
        //    {
        //        string query = "SELECT TOP 1 * FROM " + ObjDAL.dBName + "..AdminLogin AS a WHERE EmailID = '" + txtEMailId.Text.Trim() + "' ";
        //        query += "AND emailotp = '" + TxtOtp.Text.Trim() + "' AND ForType = 'Registartion' ORDER BY AID DESC";
        //        dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];

        //        if (dt1.Rows.Count > 0)
        //        {
        //            SaveIntoDB();
        //        }
        //    }
        //    else
        //    {
        //        TxtOtp.Text = "";

        //        if (Convert.ToInt32(Session["OtpCount"]) >= 3)
        //        {
        //            Session["OtpCount"] = 0;
        //            scrname = "<script language='javascript'>alert('You have tried 3 times with invalid OTP.\\n Please generate OTP again.');</script>";
        //            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('You have tried 3 times with invalid OTP.\\n Please generate OTP again.');", true);
        //            ResendOtp.Visible = true;
        //            BtnOtp.Visible = false;
        //            divOtp.Visible = false;
        //        }
        //        else
        //        {
        //            scrname = "<script language='javascript'>alert('Invalid OTP.');</script>";
        //            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Invalid OTP.');", true);
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}
    }
    protected void ResendOtp_Click(object sender, EventArgs e)
    {
        try
        {
            Session["OTP_"] = "";
            int otp = 0;
            Random rs = new Random();
            otp = rs.Next(100001, 999999);

            if (SendMail(otp.ToString()))
            {
                string emailId = txtEMailId.Text.ToString();
                string memberName = "";
                string mobileNo = "0";
                string sms = "";
                ObjDAL = new DAL();
                int result = 0;
                string query = "";

                query = "INSERT INTO AdminLogin (UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID, ForType) " +
                        "VALUES ('0', '" + memberName + "', '" + TxtOtp.Text + "', '" + mobileNo + "', '" + otp + "', GETDATE(), '" + otp + "', " +
                        "'" + txtEMailId.Text.Trim() + "', 'Registartion')";

                result = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, query));

                if (result > 0)
                {
                    Session["OTP_"] = otp;
                    divOtp.Visible = true;
                    BtnOtp.Visible = true;
                    ResendOtp.Visible = true;
                    string scrname = "<script language='javascript'>alert('OTP Sent On Mail');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    return;
                }
                else
                {
                    string scrname = "<script language='javascript'>alert('Try Later');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    return;
                }
            }
            else
            {
                string scrname = "<script language='javascript'>alert('OTP Try Later');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}
