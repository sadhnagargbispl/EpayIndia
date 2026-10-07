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

public partial class Registration : System.Web.UI.Page
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
                    Response.Redirect("logout.aspx", false);
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

                // A fresh visit must never inherit a PAN/Aadhaar verification that
                // belonged to an earlier registration attempt in the same session.
                ResetKycState();
                FillRegState();
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

        SetApiLogContext();

        // Painted on every request. A step change made by a button handler runs
        // after Page_Load and repaints, so this only settles the default view.
        ShowStep(CurrentStep);
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

                // No Placement ID entered (Uplnr = 0): there is nothing to check
                // against the sponsor's downline, so skip this validation.
                if (Session["Uplnr"].ToString() != "0" && !ValidatePlacement())
                {
                    ShowAlert("Place Under Does Not Exist In Sponsor Downline!!");
                    vsblCtrl(false, true);
                    return Validt_SpnsrDtls;
                }
            }
            // Leg availability is no longer checked here (no Left/Right "already used" message).
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
            string strSql = IsoStart + "Select * from " + ObjDAL.dBName + "..M_MemberMaster where IdNo='" + txtRefralId.Text + "'" + IsoEnd;
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
            IsPanCard = IsPanStepVerified ? 'Y' : 'N';   // set by step 2
            string aadhaarFull = (IsAadhaarStepVerified && Session[SessAadhaarNo] != null)
                ? ClearInject(Session[SessAadhaarNo].ToString())
                : "";
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

                    // Filled by the Aadhaar step when the pincode resolved against the
                    // masters; still 0 when that step was skipped.
                    dblState = ToCode(StateCode.Value);
                    DistrictCode = (int)ToCode(HDistrictCode.Value);
                    CityCode = (int)ToCode(HCityCode.Value);
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
                   "PID, Paymode, ChDDNo, ChDDBankID, ChDDBank, ChddDate, ChDDBranch, IsPanCard, AadharNo, Fld5, walletaddress, usercode) " +
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
                   "'" + (TxtDDDate.Text) + "', '" + ClearInject(TxtIssueBranch.Text) + "', '" + IsPanCard + "', '" + ClearInject(aadhaarFull) + "', " +
                   "'" + Session["TransIDJoin"] + "', '" + ClearInject(TxtWalletaddress.Text) + "', '" + ddlMobileNAme.Text + "')";

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

                            // The member row exists only now, so the PAN/Aadhaar that were
                            // verified during the wizard are written to KYC here. Failures
                            // are swallowed on purpose - the joining itself is already
                            // committed and must not be rolled back over a KYC write.
                            PersistKycAfterRegistration(Dtsms);

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
        // No "position already used" check here any more: it fired an alert for
        // both Left and Right and disabled the Complete Joining button.
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
            // The name now comes from the verified PAN / Aadhaar; the Step 4 box
            // is used only when both were skipped.
            ResolveMemberName();

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

    // ==================================================================
    //  4-STEP JOINING WIZARD
    //  1 Basic details -> 2 PAN KYC -> 3 Aadhaar KYC -> 4 Terms & Submit
    //
    //  Steps 2 and 3 are optional: Skip carries no field requirement at all,
    //  Next makes every field of that step mandatory and runs the same paid
    //  verification that PanKYC.aspx / AddressKYC.aspx run after login.
    //  Nothing is written to the database until step 4, because the member
    //  row does not exist before that.
    //
    //  The member's name is no longer typed in step 1. It comes from the
    //  verified PAN (preferred) or the verified Aadhaar, and the Aadhaar
    //  name must match the PAN name. Only when both steps are skipped does
    //  step 4 ask for the name.
    // ==================================================================

    #region Wizard state

    // The verified PAN, the Aadhaar number and the UIDAI record stay server-side.
    // A hidden field can be edited from the browser and none of this may be
    // round-tripped through the client.
    private const string SessPanVerified = "RegPanVerified";
    private const string SessPanNo = "RegPanNo";
    private const string SessPanName = "RegPanName";
    private const string SessPanDob = "RegPanDob";
    private const string SessPanDobText = "RegPanDobText";
    private const string SessPanStatus = "RegPanStatus";
    private const string SessPanTxnId = "RegPanTxnId";
    private const string SessPanCategory = "RegPanCategory";

    private const string SessAadhaarRefId = "RegAadhaarRefId";
    private const string SessAadhaarNo = "RegAadhaarPending";
    private const string SessAadhaarVerified = "RegAadhaarVerified";
    private const string SessAadhaarRecord = "RegAadhaarUidRecord";

    // Set by the PAN-Aadhaar link check on step 2. The PAN must match before the
    // paid PAN verify runs, and the Aadhaar is the one step 3 is locked to.
    private const string SessLinkedPan = "RegLinkedPan";
    private const string SessLinkedAadhaar = "RegLinkedAadhaar";

    // The link check is a public, paid page method, so it only runs for a session
    // that has passed step 1, and only a few times per session.
    private const string SessStep1Done = "RegStep1Done";
    private const string SessLinkChecks = "RegLinkCheckCount";
    private const int MaxLinkChecksPerSession = 5;

    private int CurrentStep
    {
        get { return ViewState["RegStep"] == null ? 1 : Convert.ToInt32(ViewState["RegStep"]); }
        set { ViewState["RegStep"] = value; }
    }

    private bool PanSkipped
    {
        get { return ViewState["RegPanSkipped"] != null && (bool)ViewState["RegPanSkipped"]; }
        set { ViewState["RegPanSkipped"] = value; }
    }

    private bool AadhaarSkipped
    {
        get { return ViewState["RegAadhaarSkipped"] != null && (bool)ViewState["RegAadhaarSkipped"]; }
        set { ViewState["RegAadhaarSkipped"] = value; }
    }

    private bool IsPanStepVerified
    {
        get { return Session[SessPanVerified] != null && Session[SessPanVerified].ToString() == "Y"; }
    }

    private bool IsAadhaarStepVerified
    {
        get
        {
            return Session[SessAadhaarVerified] != null
                && Session[SessAadhaarVerified].ToString() == "Y"
                && Session[SessAadhaarRecord] != null;
        }
    }

    private void ResetKycState()
    {
        Session[SessPanVerified] = null;
        Session[SessPanNo] = null;
        Session[SessPanName] = null;
        Session[SessPanDob] = null;
        Session[SessPanDobText] = null;
        Session[SessPanStatus] = null;
        Session[SessPanTxnId] = null;
        Session[SessPanCategory] = null;

        Session[SessAadhaarRefId] = null;
        Session[SessAadhaarNo] = null;
        Session[SessAadhaarVerified] = null;
        Session[SessAadhaarRecord] = null;

        Session[SessLinkedPan] = null;
        Session[SessLinkedAadhaar] = null;
        Session[SessStep1Done] = null;

        PanSkipped = false;
        AadhaarSkipped = false;
        CurrentStep = 1;
    }

    /// <summary>
    /// The Aadhaar step 3 is locked to: the number confirmed as linked with the
    /// PAN, once that PAN is verified. Null when the PAN was skipped or not verified.
    /// </summary>
    private string LockedAadhaar
    {
        get
        {
            if (!IsPanStepVerified || Session[SessLinkedAadhaar] == null) return null;
            string a = Session[SessLinkedAadhaar].ToString();
            return SandboxAadhaarVerify.IsValidAadhaarFormat(a) ? a : null;
        }
    }

    /// <summary>
    /// Shows one panel and paints the step bar. Called on every request, and again
    /// by any handler that moves the member to a different step.
    /// </summary>
    private void ShowStep(int step)
    {
        if (step < 1) step = 1;
        if (step > 4) step = 4;

        CurrentStep = step;

        pnlStep1.Visible = step == 1;
        pnlStep2.Visible = step == 2;
        pnlStep3.Visible = step == 3;
        pnlStep4.Visible = step == 4;

        regStep1.Attributes["class"] = "reg-step " + ChipClass(1, step, false);
        regStep2.Attributes["class"] = "reg-step " + ChipClass(2, step, PanSkipped);
        regStep3.Attributes["class"] = "reg-step " + ChipClass(3, step, AadhaarSkipped);
        regStep4.Attributes["class"] = "reg-step " + ChipClass(4, step, false);

        litBrandNote.Text = BrandNote(step);
        PaintStepButtons();
    }

    /// <summary>
    /// A KYC step that is already verified must not offer to verify it again or to
    /// skip past it. Re-running the check spends another paid API credit on a
    /// result we already hold, and skipping would discard a verification the
    /// member has completed. Both are replaced by a plain Next, plus a Change
    /// button for the one case that is genuinely left: a different card or number.
    ///
    /// Runs on every request, because the member can arrive back on either step
    /// with the verification already sitting in session.
    /// </summary>
    private void PaintStepButtons()
    {
        // ---------- Step 2: PAN ----------
        bool panDone = IsPanStepVerified;

        btnPanSkip.Visible = !panDone;
        btnPanNext.Visible = !panDone;
        btnPanContinue.Visible = panDone;
        btnPanEdit.Visible = panDone;

        // Locked once verified: an edited field would stop matching the record the
        // API actually confirmed, and nothing would catch the difference.
        txtPanCard.Enabled = !panDone;
        txtPanFullName.Enabled = !panDone;
        txtPanDob.Enabled = !panDone;
        chkPanConsent.Enabled = !panDone;
        txtPanAadhaar.Enabled = !panDone;
        btnPanCheckLink.Visible = !panDone;

        // Lets the page restore the "linked" state after a postback. Empty once the
        // PAN is verified, so the link message never shows on a closed step.
        hfPanLinkedPan.Value = !panDone && Session[SessLinkedPan] != null
            ? Session[SessLinkedPan].ToString() : "";

        litPanFootNote.Text = panDone
            ? "This PAN is already verified. <b>Next</b> moves on without another check. "
            + "Use <b>Change PAN</b> only if you need to verify a different card."
            : "<b>Skip</b> requires nothing at all. <b>Verify &amp; Continue</b> makes all "
            + "three fields mandatory and runs a live PAN check.";

        // ---------- Step 3: Aadhaar ----------
        bool aadhaarDone = IsAadhaarStepVerified;

        btnAadhaarSkip.Visible = !aadhaarDone;
        btnAadhaarEdit.Visible = aadhaarDone;
        btnAadhaarNext.Text = aadhaarDone ? "Next" : "Continue";

        // A verified PAN fixes which Aadhaar may be verified here: the one linked
        // with it. With the PAN skipped, any number can be entered.
        string locked = LockedAadhaar;
        if (locked != null) txtAadhaarNo.Text = locked;
        txtAadhaarNo.ReadOnly = locked != null;
        lblAadhaarLinkedNote.Visible = locked != null;

        litAadhaarFootNote.Text = aadhaarDone
            ? "This Aadhaar is already verified. <b>Next</b> moves on without another OTP. "
            + "Use <b>Change Aadhaar</b> only if you need to verify a different number."
            : "<b>Skip</b> requires nothing at all. <b>Continue</b> needs your Aadhaar "
            + "verified with an OTP first.";
    }

    /// <summary>
    /// The line at the foot of the brand panel. It answers the question the member
    /// actually has on the step they are looking at, so it changes with the step
    /// rather than repeating one generic sentence throughout.
    /// </summary>
    private static string BrandNote(int step)
    {
        switch (step)
        {
            case 2:
                return "Verified online with the Income Tax Department. Skipping is fine, "
                     + "but payouts carry 20% TDS until a PAN is verified.";
            case 3:
                return "Your address comes straight from UIDAI once the OTP is confirmed.";
            case 4:
                return "Your member ID and password are e-mailed the moment you submit. "
                     + "Bank KYC is completed later, after login.";
            default:
                return "Both KYC steps can be skipped now and finished later from your "
                     + "dashboard. No document upload is needed anywhere.";
        }
    }

    private static string ChipClass(int chip, int current, bool skipped)
    {
        if (chip == current) return "now";
        if (chip > current) return "wait";
        return skipped ? "skip" : "done";
    }

    private void Alert(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "regmsg",
            "alert('" + (message ?? "").Replace("\\", "\\\\").Replace("'", "\\'")
                               .Replace("\r", " ").Replace("\n", " ") + "');", true);
    }

    private static double ToCode(string value)
    {
        double d;
        if (double.TryParse((value ?? "").Trim(), out d)) return d;
        return 0;
    }

    private static string Trunc(string s, int max)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return s.Length <= max ? s : s.Substring(0, max);
    }

    private static string TitleCase(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;

        return System.Globalization.CultureInfo.InvariantCulture
            .TextInfo.ToTitleCase(s.Trim().ToLowerInvariant());
    }

    /// <summary>
    /// TextMode="Date" posts back as yyyy-MM-dd, but Sandbox expects dd/MM/yyyy.
    /// The other formats cover a browser that fell back to a plain text box.
    /// </summary>
    private bool TryParseDob(string input, out DateTime dob)
    {
        dob = DateTime.MinValue;
        if (string.IsNullOrEmpty(input)) return false;

        string[] formats =
        {
            "yyyy-MM-dd",
            "dd'/'MM'/'yyyy",
            "dd-MM-yyyy",
            "dd-MM-yyyy HH:mm:ss",
            "dd'/'MM'/'yyyy HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss"
        };

        if (DateTime.TryParseExact(input.Trim(), formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dob))
            return true;

        return DateTime.TryParse(input.Trim(),
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out dob);
    }

    /// <summary>
    /// Letters only, upper-case, split into words and sorted, so word order
    /// ("KUMAR RAKESH" vs "RAKESH KUMAR"), dots and extra spaces do not matter.
    /// Remove the Array.Sort line to make the order strict.
    /// </summary>
    private static string[] NameTokens(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return new string[0];

        var sb = new System.Text.StringBuilder();
        foreach (char c in s.ToUpperInvariant())
            sb.Append(char.IsLetter(c) ? c : ' ');

        string[] t = sb.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        Array.Sort(t, StringComparer.Ordinal);
        return t;
    }

    private static bool NamesMatch(string a, string b)
    {
        string[] x = NameTokens(a), y = NameTokens(b);
        if (x.Length == 0 || x.Length != y.Length) return false;
        for (int i = 0; i < x.Length; i++)
            if (x[i] != y[i]) return false;
        return true;
    }

    /// <summary>
    /// The member's name comes from KYC: verified PAN first, then verified Aadhaar.
    /// Only when both were skipped is the Step 4 box used. Returns the name that
    /// will be saved.
    /// </summary>
    private string ResolveMemberName()
    {
        if (IsPanStepVerified && Session[SessPanName] != null)
        {
            txtFrstNm.Text = Session[SessPanName].ToString().Trim().ToUpper();
        }
        else if (IsAadhaarStepVerified)
        {
            var v = (AadhaarVerifyResult)Session[SessAadhaarRecord];
            if (!string.IsNullOrWhiteSpace(v.Name))
                txtFrstNm.Text = v.Name.Trim().ToUpper();
        }
        return txtFrstNm.Text.Trim();
    }

    #endregion

    #region Wizard - audit logging

    private void SetApiLogContext()
    {
        try
        {
            SandboxApiLog.SetContext(new SandboxApiLog.CallContext
            {
                // No member row exists during joining, so every attempt is logged
                // against FormNo 0 and tied back by the session id.
                FormNo = 0,
                ClientIP = GetClientIp(),
                UserAgent = Request.UserAgent,
                SessionId = Session.SessionID,
                PageName = "Registration.aspx"
            });
        }
        catch { }
    }

    private string GetClientIp()
    {
        try
        {
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ip))
            {
                // Header can be a comma-separated chain; the first entry is the origin.
                string[] parts = ip.Split(',');
                if (parts.Length > 0) return parts[0].Trim();
            }
            return Request.UserHostAddress;
        }
        catch { return null; }
    }

    /// <summary>
    /// Captures the posted form fields for the audit record. ViewState noise and
    /// the OTP are skipped; SandboxApiLog masks any Aadhaar digits before storage.
    /// </summary>
    private string CapturePostData()
    {
        try
        {
            var sb = new System.Text.StringBuilder();
            foreach (string key in Request.Form.AllKeys)
            {
                if (key == null) continue;
                if (key.StartsWith("__")) continue;
                if (key.IndexOf("VIEWSTATE", StringComparison.OrdinalIgnoreCase) >= 0) continue;
                if (key.IndexOf("Otp", StringComparison.OrdinalIgnoreCase) >= 0) continue;
                if (key.IndexOf("Passw", StringComparison.OrdinalIgnoreCase) >= 0) continue;

                sb.AppendLine(key + " = " + Request.Form[key]);
            }
            return sb.ToString().Trim();
        }
        catch { return null; }
    }

    private void LogPanAttempt(string stage, string finalResult, string failReason,
                               string pan, string name, string dobText, PanVerifyResult r = null)
    {
        SandboxApiLog.WriteAttempt(new SandboxApiLog.Attempt
        {
            FormNo = 0,
            KycType = "PAN",
            Stage = stage,
            FinalResult = finalResult,
            FailReason = failReason,

            RefNo = pan,
            NameEntered = name,
            DobEntered = dobText,

            ApiStatus = r == null ? null : r.Status,
            ApiName = r == null ? null : r.FullName,
            ApiCategory = r == null ? null : r.Category,
            NameMatch = r == null ? (bool?)null : r.NameMatch,
            DobMatch = r == null ? (bool?)null : r.DobMatch,
            ApiTxnId = r == null ? null : r.TransactionId,
            HttpStatusCode = r == null ? null : r.HttpStatusCode,
            DurationMs = r == null ? (long?)null : r.DurationMs,
            ErrorMessage = r == null ? null : r.ErrorMessage,
            RequestBody = r == null ? null : r.RequestJson,
            ResponseBody = r == null ? null : r.RawResponse,

            PostData = CapturePostData(),
            ConsentGiven = chkPanConsent.Checked
        });
    }

    private void LogAadhaarAttempt(string stage, string finalResult, string failReason,
                                   string aadhaarPlain, AadhaarVerifyResult v = null,
                                   AadhaarOtpResult otp = null)
    {
        var a = new SandboxApiLog.Attempt
        {
            FormNo = 0,
            KycType = "AADHAAR",
            Stage = stage,
            FinalResult = finalResult,
            FailReason = failReason,
            RefNo = SandboxAadhaarVerify.MaskAadhaar(aadhaarPlain),
            RefNoHash = SandboxAadhaarVerify.HashAadhaar(aadhaarPlain),
            ExtraRef = Session[SessAadhaarRefId] == null ? null : Session[SessAadhaarRefId].ToString(),
            PostData = CapturePostData(),
            ConsentGiven = chkAadhaarConsent.Checked
        };

        if (otp != null)
        {
            a.ApiTxnId = otp.TransactionId;
            a.HttpStatusCode = otp.HttpStatusCode;
            a.DurationMs = otp.DurationMs;
            a.ErrorMessage = otp.ErrorMessage;
            a.RequestBody = otp.RequestJson;
            a.ResponseBody = otp.RawResponse;
        }

        if (v != null)
        {
            a.ApiStatus = v.Status;
            a.ApiName = v.Name;
            a.ApiDob = v.Dob;
            a.ApiGender = v.Gender;
            a.ApiCareOf = v.CareOf;
            a.ApiAddress = v.FullAddress;
            a.ApiPincode = v.Pincode;
            a.ApiTxnId = v.TransactionId;
            a.HttpStatusCode = v.HttpStatusCode;
            a.DurationMs = v.DurationMs;
            a.ErrorMessage = v.ErrorMessage;
            a.RequestBody = v.RequestJson;
            a.ResponseBody = v.RawResponse;
        }

        SandboxApiLog.WriteAttempt(a);
    }

    #endregion

    #region Step 1 - Basic details

    protected void btnStep1Next_Click(object sender, EventArgs e)
    {
        try
        {
            // Same field rules as the final submit, minus the name (it comes from
            // KYC, or from step 4) and the terms checkbox (step 4).
            if (string.IsNullOrWhiteSpace(txtRefralId.Text))
            {
                Alert("Sponsor ID is required");
                return;
            }
            if (string.IsNullOrWhiteSpace(ddlCountryNAme.SelectedValue) || ddlCountryNAme.SelectedValue == "0")
            {
                Alert("Please select country");
                return;
            }
            if (!IsValidMobile(txtMobileNo.Text.Trim()))
            {
                Alert("Enter valid mobile number");
                return;
            }
            if (!IsValidEmail(txtEMailId.Text.Trim()))
            {
                Alert("Enter valid email address");
                return;
            }

            // Caught here rather than at the end, so a duplicate is not discovered
            // only after the member has paid for a PAN and an Aadhaar verification.
            if (IsAlreadyRegistered("Email", txtEMailId.Text.Trim()))
            {
                Alert("Already Registered by this Email ID.");
                return;
            }
            if (IsAlreadyRegistered("Mobl", txtMobileNo.Text.Trim()))
            {
                Alert("Already Registered by this Mobile Number.");
                return;
            }

            Session[SessStep1Done] = "Y";
            ShowStep(2);
        }
        catch (Exception ex)
        {
            Alert("Error: " + ex.Message);
        }
    }

    private bool IsAlreadyRegistered(string column, string value)
    {
        try
        {
            string sql = "SELECT COUNT(*) AS cnt FROM M_MemberMaster WHERE " + column + " = @val";

            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@val", SqlDbType.VarChar, 200).Value = value ?? "";
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
        catch
        {
            // A lookup that cannot run must not block joining; SaveIntoDB re-checks
            // both values before the insert.
            return false;
        }
    }

    #endregion

    #region Step 2 - PAN KYC

    /// <summary>
    /// Called via jQuery AJAX from the PAN box, no postback.
    /// Returns: "OK" | "DUPLICATE" | "ERROR"
    /// </summary>
    [System.Web.Services.WebMethod]
    public static string CheckPanUniqueReg(string pan)
    {
        try
        {
            string cs = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            return IsPanFreeCore(cs, pan) ? "OK" : "DUPLICATE";
        }
        catch
        {
            return "ERROR";
        }
    }

    private static bool IsPanFreeCore(string cs, string pan)
    {
        const string sql = @"SELECT COUNT(Panno) AS cnt FROM M_MemberMaster 
                             WHERE Panno <> '' AND Panno = @panno";

        using (SqlConnection conn = new SqlConnection(cs))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.Add("@panno", SqlDbType.VarChar, 20).Value = (pan ?? "").Trim().ToUpper();
            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) == 0;
        }
    }

    /// <summary>
    /// Called via jQuery AJAX from step 2. Same checks as PanKYC.aspx; FormNo is 0
    /// because no member row exists yet. On success the PAN and Aadhaar go into
    /// Session, which is what btnPanNext_Click trusts - never the browser.
    /// </summary>
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static PanAadhaarLinkCheck.Response CheckPanAadhaarLinkReg(string pan, string aadhaar, bool consent)
    {
        HttpContext ctx = HttpContext.Current;

        if (ctx.Session == null || ctx.Session[SessStep1Done] == null)
            return new PanAadhaarLinkCheck.Response
            {
                ok = false,
                linked = false,
                msg = "Your session has expired. Please refresh the page and start again."
            };

        int used = ctx.Session[SessLinkChecks] == null ? 0 : Convert.ToInt32(ctx.Session[SessLinkChecks]);
        if (used >= MaxLinkChecksPerSession)
            return new PanAadhaarLinkCheck.Response
            {
                ok = false,
                linked = false,
                msg = "Too many attempts. Please skip PAN for now and complete PAN KYC after login."
            };

        try
        {
            string ip = ctx.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            ip = string.IsNullOrEmpty(ip) ? ctx.Request.UserHostAddress : ip.Split(',')[0].Trim();

            SandboxApiLog.SetContext(new SandboxApiLog.CallContext
            {
                FormNo = 0,
                ClientIP = ip,
                UserAgent = ctx.Request.UserAgent,
                SessionId = ctx.Session.SessionID,
                PageName = "Registration.aspx"
            });
        }
        catch { }

        string cs = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        bool panVerified = ctx.Session[SessPanVerified] != null && ctx.Session[SessPanVerified].ToString() == "Y";

        // Counted up front: a failed HTTP call can still be billed, and the page
        // validates the inputs before calling, so honest members never hit invalid ones.
        ctx.Session[SessLinkChecks] = used + 1;

        return PanAadhaarLinkCheck.Run(0, pan, aadhaar, consent,
            p => IsPanFreeCore(cs, p), panVerified,
            SessLinkedPan, SessLinkedAadhaar);
    }

    protected void btnPanBack_Click(object sender, EventArgs e)
    {
        ShowStep(1);
    }

    /// <summary>
    /// Moves on from an already verified PAN. No validation and no API call: the
    /// verification in session is what step 4 saves.
    /// </summary>
    protected void btnPanContinue_Click(object sender, EventArgs e)
    {
        ShowStep(3);
    }

    /// <summary>
    /// Releases a verified PAN so a different card can be checked. What was typed
    /// stays on screen to be corrected, but the verification is dropped, so the
    /// member has to pass the API check again before continuing.
    /// </summary>
    protected void btnPanEdit_Click(object sender, EventArgs e)
    {
        Session[SessPanVerified] = null;
        Session[SessPanNo] = null;
        Session[SessPanName] = null;
        Session[SessLinkedPan] = null;
        Session[SessLinkedAadhaar] = null;
        PanSkipped = false;

        txtPanNo.Text = "";
        txtFrstNm.Text = "";
        divPanFetched.Visible = false;
        lblPanMsg.Text = "";

        ShowStep(2);
    }

    /// <summary>
    /// Skip means exactly that: no PAN is captured and no field on this step is
    /// required. The member can still complete PAN KYC after login.
    /// </summary>
    protected void btnPanSkip_Click(object sender, EventArgs e)
    {
        Session[SessPanVerified] = null;
        Session[SessPanNo] = null;
        Session[SessPanName] = null;
        Session[SessLinkedPan] = null;
        Session[SessLinkedAadhaar] = null;
        PanSkipped = true;

        txtPanNo.Text = "";
        txtFrstNm.Text = "";
        divPanFetched.Visible = false;
        lblPanMsg.Text = "";

        LogPanAttempt("SKIPPED", "SKIPPED", "Member skipped PAN during joining",
                      txtPanCard.Text.Trim().ToUpper(), txtPanFullName.Text.Trim(), txtPanDob.Text.Trim());

        ShowStep(3);
    }

    protected void btnPanNext_Click(object sender, EventArgs e)
    {
        string pan = "", nameAsPerPan = "", dobForApi = "";

        try
        {
            pan = txtPanCard.Text.Trim().ToUpper();
            nameAsPerPan = txtPanFullName.Text.Trim();
            string dobInput = txtPanDob.Text.Trim();

            // ---------- Format validation (free) ----------
            if (!SandboxPanVerify.IsValidPanFormat(pan))
            {
                LogPanAttempt("VALIDATION", "FAILED", "Invalid PAN format", pan, nameAsPerPan, dobInput);
                Alert("Invalid PAN format. Correct format: ABCDE1234F");
                return;
            }

            if (nameAsPerPan.Length < 3)
            {
                LogPanAttempt("VALIDATION", "FAILED", "Name too short", pan, nameAsPerPan, dobInput);
                Alert("Please enter your full name exactly as printed on the PAN card.");
                return;
            }

            DateTime dob;
            if (!TryParseDob(dobInput, out dob))
            {
                LogPanAttempt("VALIDATION", "FAILED", "DOB could not be parsed", pan, nameAsPerPan, dobInput);
                Alert("Please enter a valid Date of Birth.");
                return;
            }

            if (dob > DateTime.Today.AddYears(-18) || dob < new DateTime(1900, 1, 1))
            {
                LogPanAttempt("VALIDATION", "FAILED", "DOB out of allowed range", pan, nameAsPerPan, dobInput);
                Alert("Date of Birth is not valid. Applicant must be at least 18 years old.");
                return;
            }

            // Slashes are quoted and the culture is pinned: in a custom format string
            // an unquoted "/" is the culture's date separator placeholder, not a
            // literal, so a server whose separator is "-" would silently emit
            // 03-02-2003 and Sandbox would reject it with 422.
            dobForApi = dob.ToString("dd'/'MM'/'yyyy", System.Globalization.CultureInfo.InvariantCulture);

            if (!chkPanConsent.Checked)
            {
                LogPanAttempt("CONSENT", "FAILED", "Consent not given", pan, nameAsPerPan, dobForApi);
                Alert("Please give consent to verify your PAN details.");
                return;
            }

            // The link check must have passed for this exact PAN in this session.
            string linkedPan = Session[SessLinkedPan] == null ? null : Session[SessLinkedPan].ToString();
            if (!string.Equals(linkedPan, pan, StringComparison.Ordinal))
            {
                LogPanAttempt("AADHAAR_LINK", "FAILED", "PAN-Aadhaar link not confirmed for this PAN",
                              pan, nameAsPerPan, dobForApi);
                Alert("Please check your PAN-Aadhaar link first. PAN can be verified only after it is linked with Aadhaar.");
                return;
            }

            // ---------- Duplicate check (free, DB only) ----------
            if (!IsPanFreeCore(constr, pan))
            {
                LogPanAttempt("DUPLICATE", "FAILED", "PAN already registered with another ID",
                              pan, nameAsPerPan, dobForApi);
                Alert("Pan card already registered with another ID.");
                return;
            }

            // ---------- PAN verification (PAID) ----------
            var panResult = SandboxPanVerify.VerifyPan(pan, nameAsPerPan, dobForApi);

            bool apiPassed = panResult.Success
                && string.Equals(panResult.Status, "valid", StringComparison.OrdinalIgnoreCase)
                && panResult.NameMatch
                && panResult.DobMatch;

            if (!apiPassed)
            {
                string reason, userMsg;

                if (!panResult.Success)
                {
                    reason = "API call failed: " + panResult.ErrorMessage;
                    userMsg = "PAN Verification Failed: " + panResult.ErrorMessage;
                }
                else if (!string.Equals(panResult.Status, "valid", StringComparison.OrdinalIgnoreCase))
                {
                    reason = "API returned status: " + panResult.Status;
                    userMsg = "Invalid PAN Number. Please check the PAN and try again.";
                }
                else if (!panResult.NameMatch)
                {
                    reason = "Name mismatch against PAN records";
                    userMsg = "Name does not match the PAN records. Please enter the name exactly as printed on your PAN card.";
                }
                else
                {
                    reason = "DOB mismatch against PAN records";
                    userMsg = "Date of Birth does not match the PAN records. Please check and try again.";
                }

                LogPanAttempt("API", "FAILED", reason, pan, nameAsPerPan, dobForApi, panResult);
                Alert(userMsg);
                return;
            }

            // ---------- Verified. Held in session until step 4 ----------
            Session[SessPanVerified] = "Y";
            Session[SessPanNo] = pan;
            Session[SessPanName] = nameAsPerPan;
            Session[SessPanDob] = dob;
            Session[SessPanDobText] = dobForApi;
            Session[SessPanStatus] = panResult.Status;
            Session[SessPanTxnId] = panResult.TransactionId;
            Session[SessPanCategory] = panResult.Category;
            PanSkipped = false;

            // Carried into the member record by SaveIntoDB.
            txtPanNo.Text = pan;

            // The member's name is the one verified against the PAN records.
            txtFrstNm.Text = nameAsPerPan.ToUpper();

            SetDobDropdowns(dob);

            lblPanFetchedNo.Text = pan;
            lblPanFetchedName.Text = string.IsNullOrEmpty(panResult.FullName) ? nameAsPerPan : panResult.FullName;
            // The API returns this lowercase ("individual"), which reads as a stray
            // value sitting next to properly cased fields.
            lblPanFetchedCategory.Text = TitleCase(panResult.Category);
            divPanFetched.Visible = true;

            LogPanAttempt("API", "VERIFIED", null, pan, nameAsPerPan, dobForApi, panResult);

            // Step 3 is now locked to the PAN-linked Aadhaar. An OTP sent to, or a
            // record verified for, any other number no longer applies.
            string pendingAadhaar = Session[SessAadhaarNo] == null ? null : Session[SessAadhaarNo].ToString();
            if (pendingAadhaar != null && pendingAadhaar != LockedAadhaar)
                ResetAadhaarStep("PAN verified with a different linked Aadhaar; earlier Aadhaar step discarded");

            // An Aadhaar verified earlier under a different name no longer matches
            // this PAN, so it is discarded and has to be verified again.
            if (IsAadhaarStepVerified)
            {
                var av = (AadhaarVerifyResult)Session[SessAadhaarRecord];
                if (!NamesMatch(av.Name, nameAsPerPan))
                    ResetAadhaarStep("PAN name does not match the earlier verified Aadhaar name; Aadhaar step discarded");
            }

            ShowStep(3);
        }
        catch (Exception ex)
        {
            LogPanAttempt("EXCEPTION", "FAILED", ex.Message, pan, nameAsPerPan, dobForApi);
            Alert("Error: " + ex.Message);
        }
    }

    /// <summary>
    /// The joining insert builds MemDOB out of these three lists, so a date of
    /// birth that has just been confirmed against the PAN records is pushed into
    /// them. Anything the lists do not carry is left alone - the DOB is filled in
    /// as a convenience here, it is not a gate.
    /// </summary>
    private void SetDobDropdowns(DateTime dob)
    {
        try
        {
            var ci = System.Globalization.CultureInfo.InvariantCulture;

            System.Web.UI.WebControls.ListItem day = ddlDOBdt.Items.FindByValue(dob.ToString("dd", ci));
            System.Web.UI.WebControls.ListItem month = ddlDOBmnth.Items.FindByValue(dob.ToString("MMM", ci).ToUpper());
            System.Web.UI.WebControls.ListItem year = ddlDOBYr.Items.FindByValue(dob.ToString("yyyy", ci));

            if (day != null) ddlDOBdt.SelectedValue = day.Value;
            if (month != null) ddlDOBmnth.SelectedValue = month.Value;
            if (year != null) ddlDOBYr.SelectedValue = year.Value;
        }
        catch { }
    }

    #endregion

    #region Step 3 - Aadhaar KYC

    private void FillRegState()
    {
        try
        {
            string query = ObjDAL.Isostart +
                "SELECT StateCode, StateName FROM " + ObjDAL.dBName +
                "..M_STateDivMaster WHERE ActiveStatus='Y' AND RowStatus='Y' ORDER BY StateCode" + ObjDAL.IsoEnd;

            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
            ddlRegState.DataSource = dt;
            ddlRegState.DataValueField = "StateCode";
            ddlRegState.DataTextField = "StateName";
            ddlRegState.DataBind();
        }
        catch { }
    }

    protected void ddlRegState_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateCode.Value = ddlRegState.SelectedValue;
    }

    /// <summary>
    /// Swaps the alert class rather than setting ForeColor: the message renders as
    /// a full alert box, and an inline colour would sit on top of its background
    /// instead of matching it. An empty label is hidden by CSS.
    /// </summary>
    private void AadhaarMsg(string text, bool isError)
    {
        lblAadhaarMsg.Text = text;
        lblAadhaarMsg.CssClass = isError ? "rw-alert rw-alert-error" : "rw-alert rw-alert-ok";
    }

    /// <summary>
    /// Checks the shared KYC log by hash. FormNo 0 is passed because no member row
    /// exists yet, so nothing is excluded from the duplicate search.
    /// </summary>
    private bool IsAadhaarUnique(string aadhaarPlain)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand("sp_CheckAadhaarUnique", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FormNo", 0);
                cmd.Parameters.AddWithValue("@AadhaarHash", SandboxAadhaarVerify.HashAadhaar(aadhaarPlain));
                cmd.Parameters.AddWithValue("@AadhaarPlain", aadhaarPlain);
                cmd.Parameters.AddWithValue("@AadhaarMasked", SandboxAadhaarVerify.MaskAadhaar(aadhaarPlain));

                con.Open();
                object o = cmd.ExecuteScalar();
                return o != null && Convert.ToInt32(o) == 1;
            }
        }
        catch
        {
            // Fail closed: an unverifiable duplicate check must not let a possible
            // duplicate through.
            return false;
        }
    }

    /// <summary>
    /// Aadhaar OKYC bills the OTP send and the verify separately. When the account
    /// has just failed on credits or auth, sending another OTP charges us for a
    /// verification that cannot succeed, so the flow stops before the paid call.
    /// </summary>
    private bool HasRecentServiceFailure()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand("sp_HasRecentServiceFailure", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WindowMinutes", 15);
                con.Open();
                object o = cmd.ExecuteScalar();
                return o != null && Convert.ToInt32(o) == 1;
            }
        }
        catch { return false; }
    }

    protected void btnSendOtpReg_Click(object sender, EventArgs e)
    {
        // With a verified PAN, the PAN-linked number from Session wins over
        // anything the browser posts.
        string aadhaar = LockedAadhaar ?? SandboxAadhaarVerify.Clean(txtAadhaarNo.Text);

        try
        {
            if (!SandboxAadhaarVerify.IsValidAadhaarFormat(aadhaar))
            {
                LogAadhaarAttempt("VALIDATION", "FAILED", "Invalid Aadhaar format or checksum", aadhaar);
                AadhaarMsg("Please enter a valid 12-digit Aadhaar number.", true);
                return;
            }

            if (!chkAadhaarConsent.Checked)
            {
                LogAadhaarAttempt("CONSENT", "FAILED", "Consent not given", aadhaar);
                AadhaarMsg("Please give consent before verifying your Aadhaar.", true);
                return;
            }

            // Duplicate check runs before the paid call so a duplicate never burns
            // an API credit.
            if (!IsAadhaarUnique(aadhaar))
            {
                LogAadhaarAttempt("DUPLICATE", "FAILED", "Aadhaar already registered with another ID", aadhaar);
                AadhaarMsg("This Aadhaar is already registered with another ID.", true);
                return;
            }

            if (HasRecentServiceFailure())
            {
                LogAadhaarAttempt("OTP_SEND", "FAILED",
                    "Blocked: a credit or auth failure was logged in the last 15 minutes", aadhaar);
                AadhaarMsg("Verification service is temporarily unavailable. " +
                           "Please try again later, or Skip and complete Aadhaar KYC after login.", true);

                btnSendOtpReg.Enabled = true;
                txtAadhaarNo.Enabled = true;
                chkAadhaarConsent.Enabled = true;
                return;
            }

            var otpResult = SandboxAadhaarVerify.GenerateOtp(aadhaar);

            if (!otpResult.Success)
            {
                LogAadhaarAttempt("OTP_SEND", "FAILED", otpResult.ErrorMessage, aadhaar, null, otpResult);
                AadhaarMsg(otpResult.IsServiceError
                    ? otpResult.ErrorMessage
                    : "OTP could not be sent: " + otpResult.ErrorMessage, true);

                // Nothing was sent, so the form stays open for a retry. The
                // client-side handler disabled the button on click.
                btnSendOtpReg.Enabled = true;
                txtAadhaarNo.Enabled = true;
                chkAadhaarConsent.Enabled = true;
                return;
            }

            Session[SessAadhaarRefId] = otpResult.RefId;
            Session[SessAadhaarNo] = aadhaar;
            Session[SessAadhaarVerified] = null;
            Session[SessAadhaarRecord] = null;

            LogAadhaarAttempt("OTP_SEND", "VERIFIED", null, aadhaar, null, otpResult);

            // Send OTP is replaced by the OTP panel, which carries its own Resend
            // button. Leaving both visible invites a second OTP request that
            // invalidates the reference id the member is about to submit against.
            btnSendOtpReg.Visible = false;
            divAadhaarOtp.Visible = true;
            txtAadhaarNo.Enabled = false;
            chkAadhaarConsent.Enabled = false;
            AadhaarMsg("OTP sent to your Aadhaar-linked mobile number.", false);
        }
        catch (Exception ex)
        {
            LogAadhaarAttempt("EXCEPTION", "FAILED", ex.Message, aadhaar);
            AadhaarMsg("Error: " + ex.Message, true);
        }
        finally
        {
            ShowStep(3);
        }
    }

    protected void btnVerifyOtpReg_Click(object sender, EventArgs e)
    {
        string aadhaar = Session[SessAadhaarNo] == null ? "" : Session[SessAadhaarNo].ToString();

        try
        {
            if (Session[SessAadhaarRefId] == null)
            {
                LogAadhaarAttempt("OTP_VERIFY", "FAILED", "Session expired before OTP verification", aadhaar);
                AadhaarMsg("Session expired. Please request a new OTP.", true);
                divAadhaarOtp.Visible = false;
                btnSendOtpReg.Visible = true;
                txtAadhaarNo.Enabled = true;
                chkAadhaarConsent.Enabled = true;
                return;
            }

            var v = SandboxAadhaarVerify.VerifyOtp(Session[SessAadhaarRefId].ToString(), txtAadhaarOtp.Text.Trim());

            if (!v.Success)
            {
                LogAadhaarAttempt("OTP_VERIFY", "FAILED", v.ErrorMessage, aadhaar, v);
                AadhaarMsg(v.IsServiceError
                    ? v.ErrorMessage
                    : "Verification failed: " + v.ErrorMessage, true);

                // The reference id survives a failed verify, so the OTP panel stays
                // open and the member can retry with the same OTP - no second charge.
                divAadhaarOtp.Visible = true;
                btnVerifyOtpReg.Enabled = true;
                return;
            }

            // With a verified PAN, the name UIDAI returns must be the same as the
            // name verified on the PAN. The record is not kept on a mismatch.
            if (IsPanStepVerified)
            {
                string panName = Session[SessPanName] == null ? "" : Session[SessPanName].ToString();

                if (!NamesMatch(v.Name, panName))
                {
                    LogAadhaarAttempt("NAME_MATCH", "FAILED",
                        "UIDAI name does not match PAN name", aadhaar, v);

                    ResetAadhaarStep("Aadhaar name did not match PAN name");

                    AadhaarMsg("Name on Aadhaar (" + Server.HtmlEncode(v.Name ?? "") +
                               ") does not match the name on PAN (" + Server.HtmlEncode(panName) +
                               "). Both must be the same. You can Skip this step and complete Address KYC after login.", true);
                    return;   // finally{} repaints step 3
                }
            }

            // ---- Populate the confirm panel from the UIDAI record ----
            txtRegAddress.Text = (v.FullAddress ?? "").ToUpper();
            txtRegPincode.Text = v.Pincode ?? "";

            bool resolved = ResolveByPincode(v.Pincode);

            if (resolved)
            {
                // The pincode master and the UIDAI record can disagree - a pincode
                // may map to a different district than the one Aadhaar carries.
                // Neither source is automatically right, so the mismatch is shown
                // rather than silently resolved in favour of the lookup.
                bool districtDiffers =
                    !string.IsNullOrEmpty(v.District) &&
                    !string.IsNullOrEmpty(txtRegDistrict.Text) &&
                    txtRegDistrict.Text.Trim().IndexOf(v.District.Trim(), StringComparison.OrdinalIgnoreCase) < 0 &&
                    v.District.Trim().IndexOf(txtRegDistrict.Text.Trim(), StringComparison.OrdinalIgnoreCase) < 0;

                if (districtDiffers)
                    AadhaarMsg("Aadhaar verified. Note: pincode " + v.Pincode + " maps to " +
                               txtRegDistrict.Text + ", while your Aadhaar shows " + v.District +
                               ". Please confirm which is correct.", true);
                else
                    AadhaarMsg("Aadhaar verified successfully for " + v.Name + ".", false);
            }
            else
            {
                // Pincode missing from the master: match the state by name, then
                // resolve district and city within it so codes are not left at zero.
                ResolveStateByName(v.State);

                txtRegDistrict.Text = (v.District ?? "").ToUpper();
                txtRegCity.Text = (v.Vtc ?? "").ToUpper();
                HDistrictCode.Value = "0";
                HCityCode.Value = "0";

                ResolveDistrictCityByName(StateCode.Value, v.District, v.Vtc);

                if (HDistrictCode.Value == "0" || HCityCode.Value == "0")
                    AadhaarMsg("Aadhaar verified. Pincode " + v.Pincode +
                               " is not in our master, please confirm district and city.", true);
                else
                    AadhaarMsg("Aadhaar verified successfully for " + v.Name + ".", false);
            }

            // ---- Right-hand summary ----
            ShowUidRow(trUidName, lblUidName, v.Name);
            ShowUidRow(trUidDob, lblUidDob, v.Dob);
            ShowUidRow(trUidGender, lblUidGender,
                       v.Gender == "M" ? "Male" : (v.Gender == "F" ? "Female" : v.Gender));
            ShowUidRow(trUidCareOf, lblUidCareOf, v.CareOf);

            lblUidAadhaar.Text = SandboxAadhaarVerify.MaskAadhaar(aadhaar);
            lblUidAddress.Text = v.FullAddress;
            lblUidPincode.Text = v.Pincode;
            lblUidTxnId.Text = v.TransactionId;
            trUidTxnId.Visible = !string.IsNullOrEmpty(v.TransactionId);
            divUidFetched.Visible = true;

            // The verified record is kept server-side: step 4 must save what UIDAI
            // returned, not whatever the browser posts back.
            Session[SessAadhaarRecord] = v;
            Session[SessAadhaarVerified] = "Y";
            AadhaarSkipped = false;

            LogAadhaarAttempt("OTP_VERIFY", "VERIFIED", null, aadhaar, v);

            divAadhaarOtp.Visible = false;
            divAadhaarEntry.Visible = false;
            divAadhaarConfirm.Visible = true;
            txtAadhaarOtp.Text = "";
        }
        catch (Exception ex)
        {
            LogAadhaarAttempt("EXCEPTION", "FAILED", ex.Message, aadhaar);
            AadhaarMsg("Error: " + ex.Message, true);
        }
        finally
        {
            ShowStep(3);
        }
    }

    /// <summary>
    /// Shows a detail row only when there is a value behind it. A blank row under
    /// the verified-record heading reads as a field UIDAI returned empty, when in
    /// fact it is a field we never fetched.
    /// </summary>
    private static void ShowUidRow(System.Web.UI.HtmlControls.HtmlControl row,
                                   System.Web.UI.WebControls.Label lbl, string value)
    {
        lbl.Text = value ?? "";
        row.Visible = !string.IsNullOrEmpty(lbl.Text);
    }

    protected void btnAadhaarBack_Click(object sender, EventArgs e)
    {
        ShowStep(2);
    }

    /// <summary>
    /// Releases a verified Aadhaar so a different number can be checked. The whole
    /// OTP cycle starts over, so the fetched record is cleared with it - keeping
    /// one number's address on screen while another is being verified would be
    /// worse than showing nothing.
    /// </summary>
    protected void btnAadhaarEdit_Click(object sender, EventArgs e)
    {
        ResetAadhaarStep("Member released a verified Aadhaar to verify a different number");
        ShowStep(3);
    }

    /// <summary>
    /// Drops any OTP in flight and any verified Aadhaar record, and reopens the
    /// entry form. The reason is written to the audit log.
    /// </summary>
    private void ResetAadhaarStep(string reason)
    {
        string aadhaar = Session[SessAadhaarNo] == null ? "" : Session[SessAadhaarNo].ToString();

        Session[SessAadhaarRefId] = null;
        Session[SessAadhaarNo] = null;
        Session[SessAadhaarVerified] = null;
        Session[SessAadhaarRecord] = null;
        AadhaarSkipped = false;

        TxtAAdhar1.Text = "";
        TxtAadhar2.Text = "";
        TxtAadhar3.Text = "";

        divAadhaarConfirm.Visible = false;
        divUidFetched.Visible = false;
        divAadhaarOtp.Visible = false;

        divAadhaarEntry.Visible = true;
        btnSendOtpReg.Visible = true;
        btnSendOtpReg.Enabled = true;

        txtAadhaarNo.Text = "";
        txtAadhaarNo.Enabled = true;
        txtAadhaarOtp.Text = "";
        chkAadhaarConsent.Checked = false;
        chkAadhaarConsent.Enabled = true;
        lblAadhaarMsg.Text = "";

        LogAadhaarAttempt("RESET", "INFO", reason, aadhaar);
    }

    /// <summary>
    /// Skip means exactly that: no Aadhaar and no address field is required. The
    /// member can still complete Address KYC after login.
    /// </summary>
    protected void btnAadhaarSkip_Click(object sender, EventArgs e)
    {
        string aadhaar = Session[SessAadhaarNo] == null ? "" : Session[SessAadhaarNo].ToString();

        Session[SessAadhaarRefId] = null;
        Session[SessAadhaarNo] = null;
        Session[SessAadhaarVerified] = null;
        Session[SessAadhaarRecord] = null;
        AadhaarSkipped = true;

        TxtAAdhar1.Text = "";
        TxtAadhar2.Text = "";
        TxtAadhar3.Text = "";

        LogAadhaarAttempt("SKIPPED", "SKIPPED", "Member skipped Aadhaar during joining", aadhaar);

        FillSummary();
        ShowStep(4);
    }

    protected void btnAadhaarNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsAadhaarStepVerified)
            {
                Alert("Please verify your Aadhaar with OTP before continuing, or press Skip.");
                ShowStep(3);
                return;
            }

            var v = (AadhaarVerifyResult)Session[SessAadhaarRecord];
            string aadhaar = Session[SessAadhaarNo].ToString();

            if (string.IsNullOrWhiteSpace(txtRegDistrict.Text) || string.IsNullOrWhiteSpace(txtRegCity.Text))
            {
                Alert("Please confirm district and city before continuing.");
                ShowStep(3);
                return;
            }

            // Stored in full, exactly as the member entered it, split across the
            // three four-digit columns the member record already has.
            TxtAAdhar1.Text = aadhaar.Substring(0, 4);
            TxtAadhar2.Text = aadhaar.Substring(4, 4);
            TxtAadhar3.Text = aadhaar.Substring(8, 4);

            // The address comes from the verified record, not the textbox, so a
            // tampered postback cannot substitute a different address.
            txtAddLn1.Text = Trunc((v.FullAddress ?? "").ToUpper(), 180);
            txtPinCode.Text = string.IsNullOrEmpty(v.Pincode) ? txtRegPincode.Text.Trim() : v.Pincode;
            ddlDistrict.Text = Trunc(txtRegDistrict.Text.Trim().ToUpper(), 180);
            ddlTehsil.Text = Trunc(txtRegCity.Text.Trim().ToUpper(), 180);
            txtStateName.Text = ddlRegState.SelectedItem == null ? "" : ddlRegState.SelectedItem.Text;
            StateCode.Value = ddlRegState.SelectedValue;

            FillSummary();
            ShowStep(4);
        }
        catch (Exception ex)
        {
            Alert("Error: " + ex.Message);
            ShowStep(3);
        }
    }

    #endregion

    #region Step 3 - Location resolution

    /// <summary>
    /// Resolves state, district and city codes from the pincode returned by UIDAI.
    /// Returns false when the pincode is absent from the master.
    ///
    /// The pincode lives on M_VillageMaster, not M_CityStatemaster, so the lookup
    /// starts there and walks up to city, district and state. A pincode typically
    /// covers several villages that all share one city, hence the DISTINCT.
    /// </summary>
    private bool ResolveByPincode(string pincode)
    {
        try
        {
            int pin;
            if (!int.TryParse((pincode ?? "").Trim(), out pin) || pin == 0) return false;

            string sql = ObjDAL.Isostart +
                "select distinct a.Statename, b.DistrictName, c.CityName, " +
                "a.StateCode, b.DistrictCode, c.CityCode " +
                "from " + ObjDAL.dBName + "..M_VillageMaster as v with (nolock) " +
                "inner join " + ObjDAL.dBName + "..M_CityStatemaster as c with (nolock) " +
                    "on c.CityCode = v.CityCode and c.ActivEstatus='Y' " +
                "inner join " + ObjDAL.dBName + "..M_DistrictMaster as b with (nolock) " +
                    "on b.DistrictCode = c.DistrictCode and b.ActiveStatus='Y' " +
                "inner join " + ObjDAL.dBName + "..M_STateDivMaster as a with (nolock) " +
                    "on a.StateCode = b.StateCode and a.ActivEstatus='Y' " +
                "where v.PinCode = '" + pin + "' and v.ActiveStatus='Y'" + ObjDAL.IsoEnd;

            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count == 0) return false;

            string code = dt.Rows[0]["StateCode"].ToString();
            if (ddlRegState.Items.FindByValue(code) != null) ddlRegState.SelectedValue = code;
            StateCode.Value = code;
            txtStateName.Text = dt.Rows[0]["Statename"].ToString();

            txtRegDistrict.Text = dt.Rows[0]["DistrictName"].ToString();
            HDistrictCode.Value = dt.Rows[0]["DistrictCode"].ToString();
            txtRegCity.Text = dt.Rows[0]["CityName"].ToString();
            HCityCode.Value = dt.Rows[0]["CityCode"].ToString();

            // More than one city under the same pincode means the first row is a
            // guess, and the member should look at it before continuing.
            return dt.Rows.Count == 1;
        }
        catch { return false; }
    }

    /// <summary>
    /// First fallback when the pincode is absent. UIDAI spellings differ from
    /// internal ones ("Odisha" vs "Orissa"), so the lookup is fuzzy.
    /// </summary>
    private void ResolveStateByName(string stateName)
    {
        if (string.IsNullOrEmpty(stateName)) return;

        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand("sp_GetStateCodeByName", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StateName", stateName);
                con.Open();

                object o = cmd.ExecuteScalar();
                if (o != null && o != DBNull.Value)
                {
                    string code = o.ToString();
                    if (ddlRegState.Items.FindByValue(code) != null)
                    {
                        ddlRegState.SelectedValue = code;
                        txtStateName.Text = ddlRegState.SelectedItem.Text;
                    }
                    StateCode.Value = code;
                }
            }
        }
        catch { }
    }

    /// <summary>
    /// Second fallback. Once the state is known, district and city are matched
    /// within it so the saved record carries real master codes instead of zeros.
    /// </summary>
    private void ResolveDistrictCityByName(string stateCode, string districtName, string cityName)
    {
        if (string.IsNullOrEmpty(stateCode)) return;

        try
        {
            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand("sp_ResolveDistrictCity", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StateCode", stateCode);
                cmd.Parameters.AddWithValue("@DistrictName", (object)districtName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CityName", (object)cityName ?? DBNull.Value);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 0) return;

                    DataRow r = dt.Rows[0];

                    if (dt.Columns.Contains("DistrictCode")) HDistrictCode.Value = r["DistrictCode"].ToString();
                    if (dt.Columns.Contains("CityCode")) HCityCode.Value = r["CityCode"].ToString();

                    // Prefer the master's spelling when a match was found, so the
                    // saved record stays consistent with the rest of the system.
                    string dName = dt.Columns.Contains("DistrictName") ? r["DistrictName"].ToString() : "";
                    string cName = dt.Columns.Contains("CityName") ? r["CityName"].ToString() : "";

                    if (!string.IsNullOrEmpty(dName)) txtRegDistrict.Text = dName.ToUpper();
                    if (!string.IsNullOrEmpty(cName)) txtRegCity.Text = cName.ToUpper();
                }
            }
        }
        catch { }
    }

    #endregion

    #region Step 4 - Terms and submit

    protected void btnStep4Back_Click(object sender, EventArgs e)
    {
        ShowStep(3);
    }

    private void FillSummary()
    {
        // Name: PAN, else Aadhaar, else typed here. The entry box appears only
        // when neither KYC step was verified.
        string memberName = ResolveMemberName();
        divNameEntry.Visible = !(IsPanStepVerified || IsAadhaarStepVerified);

        lblSumRefral.Text = Server.HtmlEncode(txtRefralId.Text.Trim()) +
            (string.IsNullOrEmpty(lblRefralNm.Text) ? "" : " (" + lblRefralNm.Text + ")");
        lblSumName.Text = Server.HtmlEncode(memberName);
        lblSumMobile.Text = Server.HtmlEncode(ddlMobileNAme.Text + " " + txtMobileNo.Text.Trim());
        lblSumEmail.Text = Server.HtmlEncode(txtEMailId.Text.Trim());

        const string pillOk = "<span class='rw-pill rw-pill-ok'><i class='fas fa-check-circle'></i>Verified</span>";
        const string pillSkip = "<span class='rw-pill rw-pill-skip'><i class='fas fa-minus-circle'></i>Skipped</span>";
        const string noteOpen = "<div class='rw-hint' style='margin-top:6px'>";

        if (IsPanStepVerified)
            lblSumPan.Text = pillOk + noteOpen + Server.HtmlEncode(Session[SessPanNo].ToString()) + "</div>";
        else
            lblSumPan.Text = pillSkip + noteOpen +
                             "20% TDS will apply on payout until your PAN is verified.</div>";

        if (IsAadhaarStepVerified)
        {
            var v = (AadhaarVerifyResult)Session[SessAadhaarRecord];
            lblSumAadhaar.Text = pillOk + noteOpen +
                                 Server.HtmlEncode(SandboxAadhaarVerify.MaskAadhaar(Session[SessAadhaarNo].ToString())) +
                                 "<br />" + Server.HtmlEncode(v.FullAddress ?? "") + "</div>";
        }
        else
        {
            lblSumAadhaar.Text = pillSkip + noteOpen +
                                 "You can complete Address KYC after login.</div>";
        }
    }

    /// <summary>
    /// Writes the KYC verified during the wizard against the member row that the
    /// joining insert has just created. Everything here is best-effort: the
    /// joining is already committed and must never be undone by a KYC failure,
    /// and whatever fails can still be redone from PanKYC.aspx / AddressKYC.aspx.
    /// </summary>
    private void PersistKycAfterRegistration(DataTable dtLogin)
    {
        long formNo = 0;

        try
        {
            if (dtLogin != null && dtLogin.Rows.Count > 0
                && dtLogin.Columns.Contains("formno")
                && dtLogin.Rows[0]["formno"] != DBNull.Value)
            {
                formNo = Convert.ToInt64(dtLogin.Rows[0]["formno"]);
            }
        }
        catch { }

        if (formNo <= 0) return;

        if (IsPanStepVerified)
        {
            SavePanKycForNewMember(formNo);

            // Address KYC after login picks this number up, even when the Aadhaar
            // step was skipped here. Skipped automatically once Address KYC is verified.
            PanAadhaarLinkCheck.SaveLinkedAadhaar(formNo, LockedAadhaar);
        }
        if (IsAadhaarStepVerified) SaveAddressKycForNewMember(formNo);

        // Consumed. Clearing prevents a stale session from re-saving these against
        // a second joining done from the same browser.
        ResetKycState();
    }

    private void SavePanKycForNewMember(long formNo)
    {
        try
        {
            string pan = Session[SessPanNo].ToString();
            string name = Session[SessPanName].ToString();
            DateTime dob = Convert.ToDateTime(Session[SessPanDob]);
            string dobText = Session[SessPanDobText].ToString();

            string remark = "PAN verified at joining for " + name + " on " + DateTime.Now +
                            " | PAN: " + pan +
                            " | DOB: " + dobText +
                            " | ApiStatus: " + (Session[SessPanStatus] ?? "-") +
                            " | TxnId: " + (Session[SessPanTxnId] ?? "-") +
                            " | Category: " + (Session[SessPanCategory] ?? "-");

            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand("sp_SavePanKYC", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;

                cmd.Parameters.AddWithValue("@FormNo", formNo);
                cmd.Parameters.AddWithValue("@MemFirstName", name);
                cmd.Parameters.AddWithValue("@Panno", pan);
                cmd.Parameters.AddWithValue("@MemDob", dob);
                cmd.Parameters.AddWithValue("@DobText", dobText);
                cmd.Parameters.AddWithValue("@Remark", remark);
                cmd.Parameters.AddWithValue("@ApiTxnId", Session[SessPanTxnId] ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ApiStatus", Session[SessPanStatus] ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PanCategory", Session[SessPanCategory] ?? (object)DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            LogPanAttempt("SAVE", "FAILED", "Post-joining PAN save failed: " + ex.Message,
                          Session[SessPanNo] == null ? "" : Session[SessPanNo].ToString(),
                          Session[SessPanName] == null ? "" : Session[SessPanName].ToString(),
                          Session[SessPanDobText] == null ? "" : Session[SessPanDobText].ToString());
        }
    }

    private void SaveAddressKycForNewMember(long formNo)
    {
        string aadhaar = Session[SessAadhaarNo] == null ? "" : Session[SessAadhaarNo].ToString();

        try
        {
            var v = (AadhaarVerifyResult)Session[SessAadhaarRecord];
            string memName = txtFrstNm.Text.Trim().ToUpper();
            string maskedAadhaar = SandboxAadhaarVerify.MaskAadhaar(aadhaar);

            string remark = "Address KYC verified at joining via Aadhaar OKYC by " + memName + " on " + DateTime.Now +
                            " | Aadhaar: " + maskedAadhaar +
                            " | UIDAI name: " + v.Name +
                            " | DOB: " + v.Dob +
                            " | Gender: " + v.Gender +
                            " | Pincode: " + v.Pincode +
                            " | TxnId: " + v.TransactionId;

            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand cmd = new SqlCommand("sp_SaveAddressKYC", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;

                cmd.Parameters.AddWithValue("@FormNo", formNo);
                cmd.Parameters.AddWithValue("@MemName", memName);
                cmd.Parameters.AddWithValue("@Remark", remark);
                cmd.Parameters.AddWithValue("@Address1", Trunc((v.FullAddress ?? "").ToUpper(), 180));
                cmd.Parameters.AddWithValue("@Tehsil", Trunc((v.SubDistrict ?? "").ToUpper(), 180));
                cmd.Parameters.AddWithValue("@City", Trunc(txtRegCity.Text.Trim().ToUpper(), 180));
                cmd.Parameters.AddWithValue("@District", Trunc(txtRegDistrict.Text.Trim().ToUpper(), 180));
                cmd.Parameters.AddWithValue("@StateCode", ddlRegState.SelectedValue);
                cmd.Parameters.AddWithValue("@Pincode", string.IsNullOrEmpty(v.Pincode) ? txtRegPincode.Text.Trim() : v.Pincode);
                cmd.Parameters.AddWithValue("@AreaCode", "0");
                cmd.Parameters.AddWithValue("@CityCode", string.IsNullOrEmpty(HCityCode.Value) ? "0" : HCityCode.Value);
                cmd.Parameters.AddWithValue("@DistrictCode", string.IsNullOrEmpty(HDistrictCode.Value) ? "0" : HDistrictCode.Value);
                cmd.Parameters.AddWithValue("@Idtype", GetAadhaarIdType());

                // The full number, as entered. The masked form is still used in the
                // remark above, so the free-text audit line does not repeat it.
                cmd.Parameters.AddWithValue("@IdProofNo", aadhaar);

                cmd.Parameters.AddWithValue("@AadhaarName", (object)v.Name ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AadhaarDob", (object)v.Dob ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AadhaarGender", (object)v.Gender ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ApiTxnId", (object)v.TransactionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsVerified", "Y");

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            LogAadhaarAttempt("SAVE", "FAILED", "Post-joining Address save failed: " + ex.Message, aadhaar);
        }
    }

    /// <summary>
    /// This flow only ever produces an Aadhaar-backed record, so the id type is
    /// looked up rather than chosen.
    /// </summary>
    private string GetAadhaarIdType()
    {
        try
        {
            string sql = ObjDAL.Isostart +
                "SELECT TOP 1 Id FROM " + ObjDAL.dBName + "..M_IdTypeMaster " +
                "WHERE ACTIVESTATUS='Y' AND (IdType LIKE 'Aadhaar%' OR IdType LIKE 'Aadhar%')" + ObjDAL.IsoEnd;

            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0) return dt.Rows[0]["Id"].ToString();
        }
        catch { }

        return "0";
    }

    #endregion
}
//using ClosedXML.Excel;
//using System;
//using System.CodeDom;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//using System.IO;
//using System.Net.Mail;
//using System.Net;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Activities.Expressions;
//using System.Activities;
//using System.ServiceModel.Activities;
//using DocumentFormat.OpenXml.Presentation;
//using DocumentFormat.OpenXml.Spreadsheet;

//public partial class Registartion : System.Web.UI.Page
//{
//    private double _dblAvailLeg = 0;
//    private cls_DataAccess dbConnect;
//    private DAL ObjDAL = new DAL();
//    private SqlCommand cmd = new SqlCommand();
//    private SqlDataReader dRead;
//    public string DsnName, UserName, Passw;
//    private string strQuery, strCaptcha;
//    private DataTable tmpTable = new DataTable();
//    private int minSpnsrNoLen, minScrtchLen;
//    private double Upln, dblSpons, dblState, dblBank, dblIdNo;
//    private string dblDistrict, dblTehsil, IfSC;
//    private string dblPlan;
//    private DateTime CurrDt;
//    private string scrname;
//    private string LastInsertID = "";
//    private string Email = "";
//    private string InVoiceNo;
//    private int SupplierId;
//    private string BillNo;
//    private string TaxType;
//    private string BillDate;
//    private int SBillNo;
//    private string SoldBy = "WR";
//    private string FType;
//    private string Password = "";
//    private string membername = "";
//    private string clsGeneral = "";
//    private clsGeneral dbGeneral = new clsGeneral();
//    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
//    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
//    private SqlConnection cnn;
//    DataTable Dt = new DataTable();
//    string IsoStart;
//    string IsoEnd;
//    protected void getData()
//    {
//        cls_DataAccess dbConnect = new cls_DataAccess(constr);
//        DAL objdal = new DAL();
//        try
//        {
//            SqlDataReader dRead;
//            SqlCommand cmd;
//            DataTable dtCompany = new DataTable();
//            if (Application["dtCompany"] == null)
//            {
//                if (dbConnect.cnnObject == null)
//                {
//                    dbConnect.OpenConnection();
//                }
//                DataSet ds = new DataSet();
//                SqlDataAdapter adp = new SqlDataAdapter();
//                string strQ = objdal.Isostart + " select * from " + objdal.dBName + " ..M_CompanyMaster" + objdal.IsoEnd;
//                adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
//                adp.Fill(ds);
//                dtCompany = ds.Tables[0];
//                Application["dtCompany"] = dtCompany;
//            }
//            else
//            {
//                if (dbConnect.cnnObject == null)
//                {
//                    dbConnect.OpenConnection();
//                }
//                DataSet ds = new DataSet();
//                SqlDataAdapter adp = new SqlDataAdapter();
//                string strQ = objdal.Isostart + " select * from " + objdal.dBName + " ..M_CompanyMaster" + objdal.IsoEnd;
//                adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
//                adp.Fill(ds);
//                dtCompany = ds.Tables[0];
//                Application["dtCompany"] = dtCompany;
//            }

//            if (dtCompany.Rows.Count > 0)
//            {
//                Session["CompName"] = dtCompany.Rows[0]["CompName"];
//                Session["CompAdd"] = dtCompany.Rows[0]["CompAdd"];
//                Session["CompWeb"] = string.IsNullOrEmpty(dtCompany.Rows[0]["WebSite"].ToString()) ? "index.asp" : dtCompany.Rows[0]["WebSite"];
//                Session["Title"] = dtCompany.Rows[0]["CompTitle"];
//                Session["CompMail"] = dtCompany.Rows[0]["CompMail"];
//                Session["CompMobile"] = dtCompany.Rows[0]["MobileNo"];
//                Session["ClientId"] = dtCompany.Rows[0]["smsSenderId"];
//                Session["SmsId"] = dtCompany.Rows[0]["smsUserNm"];
//                Session["SmsPass"] = dtCompany.Rows[0]["smPass"];
//                Session["MailPass"] = dtCompany.Rows[0]["mailPass"];
//                Session["MailHost"] = dtCompany.Rows[0]["mailHost"];
//                Session["AdminWeb"] = dtCompany.Rows[0]["AdminWeb"];
//                Session["CompCST"] = dtCompany.Rows[0]["CompCSTNo"];
//                Session["CompState"] = dtCompany.Rows[0]["CompState"];
//                Session["CompDate"] = Convert.ToDateTime(dtCompany.Rows[0]["RecTimeStamp"]).ToString("dd-MMM-yyyy");
//                Session["Spons"] = "KL223344";
//                Session["CompWeb1"] = dtCompany.Rows[0]["WebSite"];
//                Session["CompMovieWeb"] = "";
//                Session["SmsAPI"] = "";
//                Session["CompShortUrl"] = dtCompany.Rows[0]["UrlShort"];
//                Session["LogoUrl"] = dtCompany.Rows[0]["LogoUrl"];
//            }
//            else
//            {
//                Session["CompName"] = "";
//                Session["CompAdd"] = "";
//                Session["CompWeb"] = "";
//                Session["Title"] = "Welcome";
//            }

//            DataTable dtConfig = new DataTable();
//            if (Application["dtConfig"] == null)
//            {
//                if (dbConnect.cnnObject == null)
//                {
//                    dbConnect.OpenConnection();
//                }
//                string strQ = objdal.Isostart + " select * from " + objdal.dBName + "..M_ConfigMaster " + objdal.IsoEnd;
//                DataSet ds = new DataSet();
//                SqlDataAdapter adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
//                adp.Fill(ds);
//                dtConfig = ds.Tables[0];
//                Application["dtConfig"] = dtConfig;
//            }
//            else
//            {
//                dtConfig = (DataTable)Application["dtConfig"];
//            }

//            if (dtConfig.Rows.Count > 0)
//            {
//                Session["IsGetExtreme"] = dtConfig.Rows[0]["IsGetExtreme"];
//                Session["IsTopUp"] = dtConfig.Rows[0]["IsTopUp"];
//                Session["IsSendSMS"] = dtConfig.Rows[0]["IsSendSMS"];
//                Session["IdNoPrefix"] = dtConfig.Rows[0]["IdNoPrefix"];
//                Session["IsFreeJoin"] = dtConfig.Rows[0]["IsFreeJoin"];
//                Session["IsStartJoin"] = dtConfig.Rows[0]["IsStartJoin"];
//                Session["JoinStartFrm"] = dtConfig.Rows[0]["JoinStartFrm"];
//                Session["IsSubPlan"] = dtConfig.Rows[0]["IsSubPlan"];
//                Session["Logout"] = dtConfig.Rows[0]["LogoutPg"];
//            }
//            else
//            {
//                Session["IsGetExtreme"] = "N";
//                Session["IsTopUp"] = "N";
//                Session["IsSendSMS"] = "N";
//                Session["IdNoPrefix"] = "";
//                Session["IsFreeJoin"] = "N";
//                Session["IsStartJoin"] = "N";
//                Session["JoinStartFrm"] = "01-Sep-2011";
//                Session["IsSubPlan"] = "N";
//                Session["Logout"] = "https://djiomart.com/";
//            }
//        }
//        catch (Exception ex)
//        {
//            // handle exception
//        }
//        DataTable dtMsession = new DataTable();
//        if (Application["dtMsession"] == null)
//        {
//            if (dbConnect.cnnObject == null)
//            {
//                dbConnect.OpenConnection();
//            }
//            DataSet ds = new DataSet();
//            SqlDataAdapter adp = new SqlDataAdapter();
//            string strQ = objdal.Isostart + " select Max(SEssid) as SessID from " + objdal.dBName + "..D_Monthlypaydetail  " + objdal.IsoEnd;
//            adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
//            adp.Fill(ds);
//            dtMsession = ds.Tables[0];
//            Application["dtMsession"] = dtMsession;
//        }
//        else
//        {
//            dtMsession = (DataTable)Application["dtMsession"];
//        }

//        if (dtMsession.Rows.Count > 0)
//        {
//            Session["MaxSessn"] = dtMsession.Rows[0]["SessID"];
//        }
//        else
//        {
//            Session["MaxSessn"] = "";
//        }

//        DataTable dtsession = new DataTable();
//        if (Application["dtsession"] == null)
//        {
//            if (dbConnect.cnnObject == null)
//            {
//                dbConnect.OpenConnection();
//            }
//            DataSet ds = new DataSet();
//            SqlDataAdapter adp = new SqlDataAdapter();
//            string strQ = objdal.Isostart + " select Max(SEssid) as SessID from " + objdal.dBName + "..m_SessnMaster  " + objdal.IsoEnd;
//            adp = new SqlDataAdapter(strQ, dbConnect.cnnObject);
//            adp.Fill(ds);

//            dtsession = ds.Tables[0];
//            Application["dtsession"] = dtsession;
//        }
//        else
//        {
//            dtsession = (DataTable)Application["dtsession"];
//        }

//        if (dtsession.Rows.Count > 0)
//        {
//            Session["CurrentSessn"] = dtsession.Rows[0]["SessID"];
//        }
//        else
//        {
//            Session["CurrentSessn"] = "";
//        }
//        if (dbConnect.cnnObject != null)
//        {
//            if (dbConnect.cnnObject.State == ConnectionState.Open)
//            {
//                dbConnect.cnnObject.Close();
//            }
//        }

//    }
//    protected void Page_Load(object sender, EventArgs e)
//    {
//        this.CmdSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.CmdSave));
//        //this.BtnOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnOtp));
//        //this.ResendOtp.Attributes.Add("onclick", DisableTheButton(this.Page, this.ResendOtp));
//        try
//        {
//            if (Application["WebStatus"] == null)
//            {
//                if (Application["WebStatus"] != null && Application["WebStatus"].ToString() == "N")
//                {
//                    Session.Abandon();
//                    Response.Redirect("default.aspx", false);
//                }
//            }
//            cnn = new SqlConnection(constr1);
//            dbConnect = new cls_DataAccess((string)Application["Connect"]);
//            Response.Cache.SetCacheability(HttpCacheability.NoCache);
//            txtUplinerId.Text = (txtUplinerId.Text).Replace("'", "").Replace("=", "").Replace(";", "");
//            string sr = "";
//            string[] sbstr;
//            string Key = "";
//            string K = "";
//            if (!Page.IsPostBack)
//            {
//                Session["OtpCount"] = 0;
//                Session["OtpTime"] = null;
//                Session["OTP_"] = null;
//                Session["Retry"] = null;
//                HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
//                getData();
//                Session["OtpCount"] = 0;
//                ClrCtrl();
//                RbtnLegNo.Items.Add("Left");
//                RbtnLegNo.Items.Add("Right");

//                RbtnLegNo.Items[0].Selected = true;

//                if (!string.IsNullOrEmpty(Request.QueryString["s"]))
//                {
//                    K = Request["s"];
//                    K = K.Replace(" ", "+");
//                    sr = Crypto.Decrypt(K);

//                    sbstr = sr.Split('/');
//                    string UplinerFormno = sbstr[1];

//                    string s = IsoStart + " select * from " + ObjDAL.dBName + "..M_MemberMaster where Formno='" + UplinerFormno + "'" + IsoEnd;
//                    DataSet Ds = new DataSet();
//                    Ds = SqlHelper.ExecuteDataset(cnn, CommandType.Text, s);

//                    DataTable dt;
//                    dt = new DataTable();
//                    dt = Ds.Tables[0];
//                    if (dt.Rows.Count > 0)
//                        txtUplinerId.Text = dt.Rows[0]["Idno"].ToString();
//                    string LegNo = sbstr[3];

//                    txtUplinerId.ReadOnly = true;
//                    txtRefralId.Text = Session["Idno"].ToString();

//                    if (LegNo == "1")
//                    {
//                        RbtnLegNo.SelectedIndex = 0;
//                    }
//                    else
//                    {
//                        RbtnLegNo.SelectedIndex = 1;
//                    }
//                    RbtnLegNo.Enabled = false;
//                    Session["iLeg"] = LegNo;
//                }

//                if (Request.QueryString["ref"] != null)
//                {
//                    string req = Request.QueryString["ref"].Replace(" ", "+");
//                    string str = Crypto.Decrypt(req);
//                    string[] rfAr = str.Split('/');

//                    if (rfAr.Length >= 1)
//                    {
//                        if (!string.IsNullOrEmpty(rfAr[0]) && rfAr[1] == "0")
//                        {
//                            txtRefralId.Text = GetIDno(rfAr[0]);
//                        refLink:
//                            ;
//                        }
//                        else if (!string.IsNullOrEmpty(rfAr[0]) && rfAr[1] == "1")
//                        {
//                            txtRefralId.Text = GetIDno(rfAr[0]);
//                            RbtnLegNo.SelectedIndex = 0;
//                            RbtnLegNo.Enabled = false;
//                            RbtnLegNo.Items[1].Attributes.Add("style", "visibility:hidden");

//                        refLink:
//                            ;
//                        }
//                        else if (!string.IsNullOrEmpty(rfAr[0]) && rfAr[1] == "2")
//                        {
//                            txtRefralId.Text = GetIDno(rfAr[0]);
//                            RbtnLegNo.SelectedIndex = 1;
//                            RbtnLegNo.Enabled = false;
//                            RbtnLegNo.Items[0].Attributes.Add("style", "visibility:hidden");

//                        refLink:
//                            ;
//                        }
//                    }
//                }
//                if (!string.IsNullOrEmpty(Request.QueryString["RefFormNo"]))
//                {
//                    txtRefralId.Text = Get_IDNoUp(Request.QueryString["RefFormNo"]);
//                    TxtWalletaddress.Text = HiddenField4.Value;
//                refLink:
//                    ;


//                    //TxtWalletaddress.ReadOnly = true;
//                }
//                if (txtRefralId.Text.Trim() != "")
//                {
//                    FillReferral(cnn);
//                    txtRefralId.ReadOnly = true;
//                }

//                FillPaymode(cnn);

//                dbGeneral.Fill_Date_box(ddlDOBdt, ddlDOBmnth, ddlDOBYr, 1940, DateTime.Now.AddYears(-18).Year);
//                dbGeneral.Fill_Date_box(DDlMDay, DDLMMonth, DDLMYear, 1940, DateTime.Now.Year);
//                FillBankMaster(cnn);
//                // FillStateMaster()
//                FillCountryMasterName();
//                //FillCountryMasterCode();
//                FindSession();
//                GetConfigDtl(cnn);
//                // sendSMS()
//                vsblCtrl(false, true);
//            }

//            try
//            {
//                Session["Dsessid"] = 0;
//            }
//            catch
//            {
//            }


//            if (Session["IsGetExtreme"].ToString() == "N")
//            {
//                rwSpnsr.Visible = false;
//            }
//            else
//            {
//                rwSpnsr.Visible = false;
//            }

//        }
//        catch (Exception ex)
//        {

//        }
//    }
//    private string ClearInject(string strObj)
//    {
//        strObj = strObj.Replace(";", "").Replace("'", "").Replace("=", "");
//        return strObj.Trim();
//    }
//    private string GetIDno(string Mid)
//    {
//        string Result = "";
//        try
//        {
//            DataTable dt = new DataTable();

//            string strSql = IsoStart + "Select IDNO from " + ObjDAL.dBName + "..M_MemberMAster Where MID = '" + Mid + "' " + IsoEnd;
//            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];

//            if ((dt.Rows.Count > 0))
//                Result = dt.Rows[0]["IDNO"].ToString();
//        }
//        catch (Exception ex)
//        {
//            throw new Exception(ex.Message);
//        }
//        return Result;
//    }
//    private string DisableTheButton(System.Web.UI.Control pge, System.Web.UI.Control btn)
//    {
//        try
//        {
//            System.Text.StringBuilder sb = new System.Text.StringBuilder();
//            sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
//            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
//            sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
//            sb.Append("this.value = 'Please wait...';");
//            sb.Append("this.disabled = true;");
//            sb.Append(pge.Page.GetPostBackEventReference(btn));
//            sb.Append(";");
//            return sb.ToString();
//        }
//        catch (Exception ex)
//        {
//            throw new Exception(ex.Message);
//        }
//    }
//    private string Get_IDNoUp(string myFormNo)
//    {
//        try
//        {
//            string idNo = "";
//            DataTable dt = new DataTable();
//            DataSet ds = new DataSet();
//            string strSql = "SELECT idno FROM M_MemberMaster WHERE formno = '" + myFormNo + "'";

//            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
//            dt = ds.Tables[0];

//            if (dt.Rows.Count > 0)
//            {
//                idNo = dt.Rows[0]["idno"].ToString();
//            }

//            return idNo;
//        }
//        catch (Exception ex)
//        {
//            Response.Write("Try later.");
//            return null; // Ensure a return in case of exception
//        }
//    }
//    private void FillCountryMasterName()
//    {
//        try
//        {
//            DataTable dt = new DataTable();
//            string strQuery = "Exec Sp_GetCountry";
//            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
//            ddlCountryNAme.DataSource = dt;
//            ddlCountryNAme.DataValueField = "CId";
//            ddlCountryNAme.DataTextField = "CountryName";
//            ddlCountryNAme.DataBind();
//            // ddlCountryName.SelectedIndex = 90;
//        }
//        catch (Exception ex)
//        {
//            // Handle exception
//        }
//    }
//    public string GenerateRandomStringJoining(int iLength)
//    {
//        Random rdm = new Random();
//        char[] allowChrs = "123456789".ToCharArray();
//        string sResult = "";

//        for (int i = 0; i < iLength; i++)
//        {
//            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
//        }

//        return sResult;
//    }
//    private void FillPaymode(SqlConnection cnn)
//    {
//        try
//        {
//            DataTable dt = new DataTable();
//            DataSet ds = new DataSet();
//            string strSql = IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_PayModeMaster WHERE ActiveStatus='Y' " + IsoEnd;

//            if (Session["DtPayMode"] == null)
//            {
//                ds = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql);
//                dt = ds.Tables[0];
//                Session["DtPayMode"] = dt;
//            }
//            else
//            {
//                dt = (DataTable)Session["DtPayMode"];
//            }

//            if (dt.Rows.Count > 0)
//            {
//                DdlPaymode.DataSource = dt;
//                DdlPaymode.DataValueField = "PID";
//                DdlPaymode.DataTextField = "Paymode";
//                DdlPaymode.DataBind();
//            }
//        }
//        catch (Exception ex)
//        {
//            Response.Write("Try later.");
//        }
//    }
//    private void GetConfigDtl(SqlConnection cnn)
//    {
//        try
//        {
//            DataTable dt = new DataTable();
//            DataSet ds = new DataSet();
//            string strSql = IsoStart + "select *  from " + ObjDAL.dBName + "..M_ConfigMaster " + IsoEnd;

//            //if (Session["DtConfigDetail"] == null)
//            //{
//            ds = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql);
//            dt = ds.Tables[0];
//            Session["DtConfigDetail"] = dt;
//            //}
//            //else
//            //{
//            //    dt = (DataTable)Session["DtConfigDetail"];
//            //}

//            if (dt.Rows.Count > 0)
//            {
//                Session["IsGetExtreme"] = dt.Rows[0]["IsGetExtreme"];
//                Session["IsTopUp"] = dt.Rows[0]["IsTopUp"];
//                Session["IsSendSMS"] = dt.Rows[0]["IsSendSMS"];
//                Session["IdNoPrefix"] = dt.Rows[0]["IdNoPrefix"];
//                Session["IsFreeJoin"] = dt.Rows[0]["IsFreeJoin"];
//                Session["IsStartJoin"] = dt.Rows[0]["IsStartJoin"];
//                Session["JoinStartFrm"] = dt.Rows[0]["JoinStartFrm"];
//                Session["IsSubPlan"] = dt.Rows[0]["IsSubPlan"];
//            }
//            else
//            {
//                Session["IsGetExtreme"] = "N";
//                Session["IsTopUp"] = "N";
//                Session["IsSendSMS"] = "N";
//                Session["IdNoPrefix"] = "";
//                Session["IsFreeJoin"] = "N";
//                Session["IsStartJoin"] = "N";
//                Session["JoinStartFrm"] = "01-Sep-2011";
//                Session["IsSubPlan"] = "N";
//            }
//        }
//        catch
//        {
//            Session["CompName"] = "";
//            Session["CompAdd"] = "";
//            Session["CompWeb"] = "";
//        }
//    }
//    protected void vsblCtrl(bool isVsbl, bool isOnlyDv)
//    {
//        try
//        {
//            if (!isOnlyDv)
//            {
//                txtUplinerId.Enabled = !isVsbl;
//                txtRefralId.Enabled = !isVsbl;
//            }
//        }
//        catch (Exception ex)
//        {
//            Response.Write("Try later.");
//        }
//    }
//    public string GenerateRandomString(int iLength)
//    {
//        Random rdm = new Random();
//        char[] allowChrs = "123456789".ToCharArray();
//        string sResult = "";

//        for (int i = 0; i < iLength; i++)
//        {
//            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
//        }

//        return sResult;
//    }
//    private void ClrCtrl()
//    {
//        // txtAddLn2.Text = "";
//        txtAddLn1.Text = "";
//        txtEMailId.Text = "";
//        txtFNm.Text = "";
//        txtFrstNm.Text = "";
//        txtMobileNo.Text = "";
//        txtNominee.Text = "";
//        txtPanNo.Text = "";
//        txtPhNo.Text = "";
//        txtPinCode.Text = "";
//        txtRelation.Text = "";
//        txtUplinerId.Text = "";
//        lblUplnrNm.Text = "";
//        ddlDistrict.Text = "";
//        ddlTehsil.Text = "";
//        TxtBranchName.Text = "";
//        TxtAccountNo.Text = "";
//        txtIfsCode.Text = "";
//        txtRefralId.Text = "";
//        lblRefralNm.Text = "";
//        txtUplinerId.Enabled = true;
//        txtRefralId.Enabled = true;


//        RbtnLegNo.Enabled = true;
//    }
//    private void FillBankMaster(SqlConnection Cnn)
//    {
//        try
//        {
//            DataTable dt = new DataTable();

//            if (Session["DtBankMaster"] == null)
//            {
//                DataSet Ds = new DataSet();
//                string strSql = IsoStart + "SELECT BankCode as Bid, BANKNAME as Bank FROM " + ObjDAL.dBName + "..M_BankMaster WHERE ACTIVESTATUS='Y' and Rowstatus='Y' ORDER BY BankName" + IsoEnd;
//                Ds = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, strSql);
//                dt = Ds.Tables[0];
//                Session["DtBankMaster"] = dt;
//            }
//            else
//            {
//                dt = (DataTable)Session["DtBankMaster"];
//            }

//            if (dt.Rows.Count > 0)
//            {
//                CmbBank.DataSource = dt;
//                CmbBank.DataValueField = "Bid";
//                CmbBank.DataTextField = "Bank";
//                CmbBank.DataBind();
//                CmbBank.SelectedIndex = 0;
//            }

//            TxtBank.Text = CmbBank.SelectedItem.Text;
//        }
//        catch (Exception ex)
//        {
//            Response.Write("Try later.");
//        }
//    }
//    public string Validt_SpnsrDtl()
//    {
//        string Validt_SpnsrDtls = string.Empty;

//        try
//        {
//            // Sanitize input
//            txtRefralId.Text = txtRefralId.Text.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
//            txtUplinerId.Text = txtUplinerId.Text.Trim().Replace("'", "").Replace("=", "").Replace(";", "");

//            // Check Referral ID
//            if (!string.IsNullOrEmpty(txtRefralId.Text))
//            {
//                try
//                {
//                    DataTable dt = new DataTable();
//                    string strSql = IsoStart +
//                        "Select FormNo, MemFirstName + ' ' + MemLastName as MemName, ActiveStatus " +
//                        "from " + ObjDAL.dBName + "..M_MemberMaster where Idno='" + txtRefralId.Text + "'" + IsoEnd;

//                    using (DataSet ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql))
//                    {
//                        dt = ds.Tables[0];
//                    }

//                    if (dt.Rows.Count == 0)
//                    {
//                        ShowAlert("Sponsor ID Not Exist.");
//                        vsblCtrl(false, true);
//                        return Validt_SpnsrDtls;
//                    }
//                    else
//                    {
//                        Session["Kitid"] = 1;
//                        Session["Bv"] = 0;
//                        Session["JoinStatus"] = "N";
//                        Session["RP"] = 0;
//                        Validt_SpnsrDtls = "OK";
//                        Session["Refral"] = dt.Rows[0]["FormNo"].ToString();
//                        lblRefralNm.Text = dt.Rows[0]["MemName"].ToString();
//                    }
//                }
//                catch (Exception)
//                {
//                    ShowAlert("Please check sponsor ID.");
//                    return Validt_SpnsrDtls;
//                }
//            }
//            else
//            {
//                ShowAlert("Check Sponsor ID.");
//                txtRefralId.Focus();
//                return Validt_SpnsrDtls;
//            }

//            // Check Upliner ID
//            if (Session["IsGetExtreme"].ToString() == "N")
//            {
//                if (!string.IsNullOrEmpty(txtUplinerId.Text))
//                {
//                    try
//                    {
//                        DataTable dt = new DataTable();
//                        string strSql = IsoStart +
//                            "Select FormNo, MemFirstName + ' ' + MemLastName as MemName " +
//                            "from " + ObjDAL.dBName + "..M_MemberMaster where Idno='" + txtUplinerId.Text + "'" + IsoEnd;

//                        using (DataSet ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql))
//                        {
//                            dt = ds.Tables[0];
//                        }

//                        if (dt.Rows.Count == 0)
//                        {
//                            ShowAlert("Sponsor ID Not Exist.");
//                            vsblCtrl(false, true);
//                            return Validt_SpnsrDtls;
//                        }
//                        Session["Uplnr"] = dt.Rows[0]["FormNo"].ToString();
//                        Validt_SpnsrDtls = "OK";
//                        lblUplnrNm.Text = dt.Rows[0]["MemName"].ToString();
//                    }
//                    catch (Exception)
//                    {
//                        ShowAlert("Incorrect Place under ID.");
//                        return Validt_SpnsrDtls;
//                    }
//                }
//                else
//                {
//                    txtUplinerId.Text = "0";
//                    lblUplnrNm.Text = string.Empty;
//                    Session["Uplnr"] = "0";
//                }

//                if (!ValidatePlacement())
//                {
//                    ShowAlert("Place Under Does Not Exist In Sponsor Downline!!");
//                    vsblCtrl(false, true);
//                    return Validt_SpnsrDtls;
//                }
//            }
//            if (Session["IsGetExtreme"].ToString() == "N" && !string.IsNullOrWhiteSpace(txtUplinerId.Text))
//            {
//                if (!checkAvailLeg())
//                {
//                    Validt_SpnsrDtls = string.Empty;
//                    vsblCtrl(false, true);
//                    return Validt_SpnsrDtls;
//                }
//            }
//            RbtnLegNo.Enabled = false;
//            txtUplinerId.Enabled = false;
//            txtRefralId.Enabled = false;
//        }
//        catch (Exception)
//        {
//            // Handle unexpected errors
//        }

//        return Validt_SpnsrDtls;
//    }
//    private void ShowAlert(string message)
//    {
//        string scrname = "<SCRIPT language='javascript'>alert('" + message + "');</SCRIPT>";
//        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Alert", scrname, false);
//    }
//    private bool ValidatePlacement()
//    {
//        if (Session["Refral"].ToString() != Session["Uplnr"].ToString())
//        {
//            DataTable dt = new DataTable();
//            string strSql = IsoStart +
//                "Select * from " + ObjDAL.dBName + "..R_MemTreeRelation " +
//                "where FormNo=" + Session["Refral"] + " And FormNoDwn=" + Session["Uplnr"] + " " + IsoEnd;

//            using (DataSet ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql))
//            {
//                dt = ds.Tables[0];
//            }

//            if (dt.Rows.Count == 0)
//            {
//                return false;
//            }
//        }
//        return true;
//    }
//    private void FindSession()
//    {
//        try
//        {
//            Session["SessID"] = 1;
//            return;
//        }
//        catch (Exception ex)
//        {
//            Response.Write("Try later.");
//        }

//        try
//        {
//            DataTable dt = new DataTable();
//            DataSet Ds = new DataSet();
//            string strSql = ObjDAL.Isostart + "Select Max(SessId) as SessId from " + ObjDAL.dBName + "..M_SessnMaster  " + ObjDAL.IsoEnd;
//            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0]; ;
//            dt = Dt;
//            if (dt.Rows.Count > 0)
//            {
//                Session["SessID"] = dt.Rows[0]["SessID"];
//            }
//            else
//            {
//                errMsg.Text = "Session Not Exist. Please Enter New Session.";
//                return;
//            }
//        }
//        catch (Exception ex)
//        {
//            throw new Exception(ex.Message);
//        }
//    }
//    private bool checkAvailLeg()
//    {
//        try
//        {
//            int iLegNo = 0;
//            int iformNo = 0;

//            if (RbtnLegNo.SelectedIndex == 0)
//            {
//                iLegNo = 1;
//            }
//            else if (RbtnLegNo.SelectedIndex == 1)
//            {
//                iLegNo = 2;
//            }
//            else
//            {
//                string scrname = "<SCRIPT language='javascript'>alert('Choose Position.');</SCRIPT>";
//                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//                return false;
//            }

//            DataTable dt = new DataTable();
//            DataSet Ds = new DataSet();
//            string strSql = IsoStart + "Select * from " + ObjDAL.dBName + "..M_MemberMaster where IdNo='" + txtUplinerId.Text + "'" + IsoEnd;
//            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
//            dt = Ds.Tables[0];

//            if (dt.Rows.Count > 0)
//            {
//                iformNo = Convert.ToInt32(dt.Rows[0]["FormNo"]);
//            }
//            else
//            {
//                errMsg.Text = "Check Placeunder Id.";
//                string scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
//                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//                return false;
//            }

//            DataTable dt12 = new DataTable();
//            DataSet Ds12 = new DataSet();
//            string strSql12 = IsoStart + "SELECT COUNT(*) AS CNT FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE uplnformno = " + iformNo + " And LegNo = " + iLegNo + IsoEnd;
//            Ds12 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql12);
//            dt12 = Ds12.Tables[0];

//            if (dt12.Rows.Count > 0 && Convert.ToInt32(dt12.Rows[0]["CNT"]) > 0)
//            {
//                errMsg.Text = (iLegNo == 1 ? "LEFT" : "RIGHT") + " Position already used, please select correct Position!";
//                string scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
//                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//                CmdSave.Enabled = false;
//                return false;
//            }
//            else
//            {
//                errMsg.Visible = false;
//                CmdSave.Enabled = true;
//                _dblAvailLeg = iformNo;
//                return true;
//            }
//        }
//        catch (Exception ex)
//        {
//            Response.Write("Try later.");
//            return false;
//        }
//    }
//    protected void txtUplinerId_TextChanged(object sender, EventArgs e)
//    {
//        FillSponsor(ref cnn);
//    }
//    private void FillSponsor(ref SqlConnection Cnn)
//    {
//        try
//        {
//            errMsg.Text = "";
//            lblErrEpin.Text = "";
//            int i = 0;
//            txtUplinerId.Text = txtUplinerId.Text.Trim().Replace(";", "").Replace("'", "").Replace("=", "");

//            DataTable dt = new DataTable();
//            DataSet Ds = new DataSet();
//            string strSql = IsoStart + " Select FormNo,MemFirstName + ' ' + MemLastName as MemName from " + ObjDAL.dBName +
//                            "..M_MemberMaster where IDNo='" + txtUplinerId.Text + "'" + IsoEnd;
//            Ds = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, strSql);
//            dt = Ds.Tables[0];

//            if (dt.Rows.Count > 0)
//            {
//                lblUplnrNm.Text = dt.Rows[0]["MemName"].ToString();
//                Session["Uplnr"] = dt.Rows[0]["FormNo"].ToString();
//                i += 1;
//            }
//            else
//            {
//                errMsg.Text = "Invalid PlaceUnder ID!!";
//                lblErrEpin.Text = "Invalid PlaceUnder ID!!";
//                string scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
//                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//            }

//            if (i == 1)
//            {
//                checkAvailLeg();
//            }
//        }
//        catch (Exception ex)
//        {
//            Response.Write("Try later.");
//        }
//    }
//    private void FillReferral(SqlConnection Cnn)
//    {
//        try
//        {
//            lblErrEpin.Text = "";
//            errMsg.Text = "";
//            txtRefralId.Text = txtRefralId.Text.Trim().Replace(";", "").Replace("'", "").Replace("=", "");

//            DataTable dt = new DataTable();
//            DataSet Ds = new DataSet();
//            string strSql = IsoStart + "Select FormNo,MemFirstName + ' ' + MemLastName as MemName,ActiveStatus from " +
//                            ObjDAL.dBName + "..M_MemberMaster where IDNo='" + txtRefralId.Text + "' and IsBlock='N' " + IsoEnd;
//            Ds = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, strSql);
//            dt = Ds.Tables[0];

//            if (dt.Rows.Count == 0)
//            {
//                string scrname = "<SCRIPT language='javascript'>alert('No such record/This ID is Flashed./This Id Not Active!!');</SCRIPT>";
//                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//                txtRefralId.Text = "";
//                return;
//            }
//            //else if (dt.Rows[0]["ActiveStatus"].ToString() == "N")
//            //{
//            //    string scrname = "<SCRIPT language='javascript'>alert('This ID is not eligible for sponsor.');</SCRIPT>";
//            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//            //    return;
//            //}
//            else
//            {
//                lblRefralNm.Text = dt.Rows[0]["MemName"].ToString();
//            }
//        }
//        catch (Exception ex)
//        {
//            Response.Write("Try later.");
//        }
//    }
//    protected void CmdCancel_Click(object sender, EventArgs e)
//    {
//        ClrCtrl();
//    }
//    protected void txtRefralId_TextChanged(object sender, EventArgs e)
//    {
//        try
//        {
//            FillReferral(cnn);
//        }
//        catch (Exception ex)
//        {
//            // Handle the exception if necessary
//        }
//    }
//    protected void txtMobileNo_TextChanged(object sender, EventArgs e)
//    {
//        try
//        {
//            if (!string.IsNullOrEmpty(txtMobileNo.Text))
//            {
//                string moblno = txtMobileNo.Text;
//                string check = moblno.Substring(0, 1);

//                if (check == "0")
//                {
//                    txtMobileNo.Text = "";
//                    CmdSave.Enabled = true;
//                    chkterms.Checked = false;
//                    string scrname = "<SCRIPT language='javascript'>alert('Invalid Mobile No.!');</SCRIPT>";
//                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//                    return;
//                }
//            }

//            if (!string.IsNullOrEmpty(txtMobileNo.Text))
//            {
//                DataTable Dt1 = new DataTable();
//                DataSet Dsmob = new DataSet();
//                string strSql = IsoStart + "select Count(mobl) as mobileno from " + ObjDAL.dBName + "..M_Membermaster where Mobl='" + txtMobileNo.Text.Trim() + "' " + IsoEnd;
//                Dsmob = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql);
//                Dt1 = Dsmob.Tables[0];

//                if (Convert.ToInt32(Dt1.Rows[0]["mobileno"]) >= 10000)
//                {
//                    txtMobileNo.Text = "";
//                    CmdSave.Enabled = true;
//                    chkterms.Checked = false;
//                    string scrname = "<SCRIPT language='javascript'>alert('Already Registered by this Mobile Number.');</SCRIPT>";
//                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//                    return;
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            // Handle the exception
//        }
//    }
//    protected void txtEMailId_TextChanged(object sender, EventArgs e)
//    {
//        try
//        {
//            DataTable DtEmail = new DataTable();
//            DataSet DsEmail = new DataSet();
//            string strSql = IsoStart + "select Count(Email) as Email from " + ObjDAL.dBName + "..M_Membermaster where Email='" + txtEMailId.Text.Trim() + "' " + IsoEnd;
//            DsEmail = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
//            DtEmail = DsEmail.Tables[0];

//            if (Convert.ToInt32(DtEmail.Rows[0]["Email"]) >= 100000)
//            {
//                txtEMailId.Text = "";
//                CmdSave.Enabled = true;
//                chkterms.Checked = false;
//                LblEmainID.Visible = true;
//                LblEmainID.Text = "Already Registered by this Email ID.!";
//                return;
//            }
//            else
//            {
//                LblEmainID.Visible = false;
//            }
//        }
//        catch (Exception ex)
//        {
//            // Handle the exception
//        }
//    }
//    protected void ddlCountryNAme_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        FillCountryMasterCode();
//    }
//    private void FillCountryMasterCode()
//    {
//        try
//        {
//            DataTable dt = new DataTable();
//            string strQuery = IsoStart + "SELECT StdCode FROM " + ObjDAL.dBName + "..M_CountryMaster WHERE ACTIVESTATUS='Y' AND Cid = '" + ddlCountryNAme.SelectedValue + "' " + IsoEnd;
//            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];

//            if (dt.Rows.Count > 0)
//            {
//                ddlMobileNAme.Text = dt.Rows[0]["StdCode"].ToString();
//            }
//        }
//        catch (Exception ex)
//        {
//            // Handle the exception
//        }
//    }
//    public void SaveIntoDB()
//    {
//        try
//        {
//            char IsPanCard;
//            string strQry = "";
//            string strDOB, strDOM, strDOJ, s;
//            int iLeg;
//            char cGender, cMarried; // Declare variables
//            cGender = 'M';          // Assign value
//            cMarried = 'N';        // Assign value

//            string hostIp = Context.Request.UserHostAddress; // Retrieve and assign IP address
//            string HostIp = Context.Request.UserHostAddress.ToString();
//            int DistrictCode, CityCode, VillageCode;
//            CmdSave.Enabled = false;
//            string s1 = "";
//            if (txtEMailId.Text == "")
//            {
//                chkterms.Checked = false;
//                CmdSave.Enabled = true;
//                scrname = "<SCRIPT language='javascript'>alert('Enter Email-Id.');" + "</SCRIPT>";
//                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Enter Email-Id.');", true);
//                return;
//            }
//            if (txtMobileNo.Text == "")
//            {
//                chkterms.Checked = false;
//                CmdSave.Enabled = true;
//                scrname = "<SCRIPT language='javascript'>alert('Enter Mobile No.');" + "</SCRIPT>";
//                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Enter Mobile No.');", true);
//                return;
//            }
//            if (!string.IsNullOrWhiteSpace(txtEMailId.Text)) // Check if txtEMailId is not empty
//            {
//                DataTable dtEmail = new DataTable(); // Initialize DataTable
//                DataSet dsEmail = new DataSet(); // Initialize DataSet
//                string strSql = IsoStart + " select Count(Email) as Email from " + ObjDAL.dBName + "..M_Membermaster where Email='" + txtEMailId.Text.Trim() + "' " + IsoEnd;

//                dsEmail = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql); // Execute the SQL query
//                dtEmail = dsEmail.Tables[0]; // Get the first DataTable from DataSet

//                if (Convert.ToInt32(dtEmail.Rows[0]["Email"]) >= 1) // Check if the email already exists
//                {
//                    CmdSave.Enabled = true; // Enable the save command
//                    chkterms.Checked = false; // Uncheck the terms checkbox
//                    string scrname = "<script language='javascript'>alert('Already Registered by this Email ID.');</script>"; // Prepare alert script
//                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false); // Register script block
//                    return; // Exit the method
//                }
//            }
//            if (!string.IsNullOrWhiteSpace(txtMobileNo.Text)) // Check if txtMobileNo is not empty
//            {
//                DataTable dt1 = new DataTable(); // Initialize DataTable
//                DataSet dsmob = new DataSet(); // Initialize DataSet
//                string strSql = IsoStart + "select Count(mobl) as mobileno from " + ObjDAL.dBName + "..M_Membermaster where Mobl='" + txtMobileNo.Text.Trim() + "' " + IsoEnd;

//                dsmob = SqlHelper.ExecuteDataset(cnn, CommandType.Text, strSql); // Execute the SQL query
//                dt1 = dsmob.Tables[0]; // Get the first table from the dataset

//                if (Convert.ToInt32(dt1.Rows[0]["mobileno"]) >= 1) // Check if the mobile number is already registered
//                {
//                    CmdSave.Enabled = true; // Enable the save command
//                    chkterms.Checked = false; // Uncheck the terms checkbox
//                    string scrname = "<script language='javascript'>alert('Already Registered by this Mobile Number.');</script>"; // Prepare alert script
//                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false); // Register script block
//                    return; // Exit the method
//                }
//            }

//            try
//            {
//                if (Validt_SpnsrDtl() == "OK")
//                {
//                    iLeg = Convert.ToInt32(Session["iLeg"]);
//                    if ((RbtnLegNo.SelectedIndex == 0))
//                        iLeg = 1;
//                    else if ((RbtnLegNo.SelectedIndex == 1))
//                        iLeg = 2;
//                    else
//                    {
//                        chkterms.Checked = false;
//                        CmdSave.Enabled = true;
//                        scrname = "<SCRIPT language='javascript'>alert('Choose Position.');" + "</SCRIPT>";
//                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Choose Position.');", true);
//                        RbtnLegNo.Enabled = true;
//                        return;
//                    }
//                    TxtPasswd.Text = GenerateRandomString(6);

//                    if (TxtPasswd.Text == "")
//                    {
//                        chkterms.Checked = false;
//                        CmdSave.Enabled = true;
//                        scrname = "<SCRIPT language='javascript'>alert('Enter Password.');" + "</SCRIPT>";
//                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Enter Password.');", true);
//                        return;
//                    }
//                    string q = "";
//                    int i = 0;
//                    DataTable Dt;
//                    int BankCode = 0;
//                    if (CmbBank.SelectedItem.Text.ToUpper() == "OTHERS") // Check if the selected bank is "OTHERS"
//                    {
//                        if (!string.IsNullOrWhiteSpace(TxtBank.Text)) // Check if TxtBank is not empty
//                        {
//                            DataTable dt = new DataTable(); // Initialize DataTable
//                            DataSet ds = new DataSet(); // Initialize DataSet
//                            q = IsoStart + "Select * from " + ObjDAL.dBName + "..M_BankMaster where BankName='" + TxtBank.Text.Trim() + "' and Activestatus='Y' and RowStatus='Y' " + IsoEnd;

//                            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, q); // Execute the SQL query
//                            dt = ds.Tables[0]; // Get the first table from the dataset

//                            if (dt.Rows.Count == 0) // If no records found
//                            {
//                                q = "";
//                                q = "insert into M_BankMaster (BankCode, BankName, AcNo, IFSCode, Remarks, ActiveStatus, LastModified, UserCode, UserId, IPAdrs, RowStatus) " +
//                                    "Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode, '" + TxtBank.Text.ToUpper() + "', '0', '0', " +
//                                    "'', 'Y', 'Add by " + Session["IdNo"] + " at " + DateTime.Now.ToString() + "', '" + Session["MemName"] + "', " +
//                                    "'" + Convert.ToString(Session["FormNo"]) + "', '', 'Y' From M_BankMaster";

//                                i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, q)); // Execute the insert query

//                                if (i > 0) // If the insert was successful
//                                {
//                                    string qs = IsoStart + " select Max(BankCode) as BankCode from " + ObjDAL.dBName + "..M_BankMaster where ActiveStatus='Y' and RowStatus='Y'" + IsoEnd;
//                                    DataTable dtRead = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qs).Tables[0]; // Get the max BankCode

//                                    if (dtRead.Rows.Count > 0)
//                                    {
//                                        dblBank = Convert.ToInt32(dtRead.Rows[0]["BankCode"]); // Get the BankCode
//                                    }
//                                }
//                            }
//                            else // If a record exists
//                            {
//                                dblBank = Convert.ToInt32(dt.Rows[0]["BankCode"]); // Get the existing BankCode
//                            }
//                        }
//                    }
//                    else // If the selected bank is not "OTHERS"
//                    {
//                        dblBank = Convert.ToInt32(CmbBank.SelectedValue); // Get the selected value
//                    }

//                    int AreaCode = 0;
//                    AreaCode = 0;
//                    string RegestType = "";
//                    if (RbCategory.SelectedValue == "IN") // Check if the selected value is "IN"
//                    {
//                        RegestType = "IN"; // Assign "IN" to RegestType
//                    }
//                    else
//                    {
//                        RegestType = CbSubCategory.SelectedValue; // Assign the selected value of CbSubCategory to RegestType
//                    }

//                    int PostalAreaCode = 0;
//                    strDOB = ddlDOBdt.Text + "-" + ddlDOBmnth.Text + "-" + ddlDOBYr.Text; // Concatenate day, month, and year for date of birth
//                    strDOM = DDlMDay.Text + "-" + DDLMMonth.Text + "-" + DDLMYear.Text; // Concatenate day, month, and year for date of marriage
//                    strDOJ = DateTime.Now.ToString("dd-MMM-yyyy"); // Format the server date as "dd-MMM-yyyy"
//                    string dblDistrict = ClearInject(ddlDistrict.Text.ToUpper()); // Get and clear injected text for district
//                    string dblTehsil = ClearInject(ddlTehsil.Text.ToUpper()); // Get and clear injected text for tehsil

//                    if (string.IsNullOrEmpty(dblDistrict))
//                    {
//                        dblDistrict = "";
//                    }

//                    dblState = 0;
//                    DistrictCode = 0;
//                    CityCode = 0;
//                    VillageCode = 0;
//                    IfSC = ClearInject(txtIfsCode.Text.ToUpper());

//                    dblPlan = "0";
//                    InVoiceNo = "0";

//                    if (Session["SessID"] == null || (int)Session["SessID"] == 0)
//                    {
//                        FindSession();
//                    }

//                    string Name = "";
//                    string fathername = "";

//                    if (RbCategory.SelectedValue == "IN")
//                    {
//                        Name = ClearInject(txtFrstNm.Text.ToUpper());
//                        fathername = ClearInject(txtFNm.Text.ToUpper());
//                    }
//                    else
//                    {
//                        fathername = ClearInject(txtFrstNm.Text.ToUpper());
//                        Name = ClearInject(TxtCompanyName.Text.ToUpper());
//                    }
//                    if (!string.IsNullOrWhiteSpace(TxtAccountNo.Text) || !string.IsNullOrWhiteSpace(txtIfsCode.Text.Trim()))
//                    {
//                        if (string.IsNullOrWhiteSpace(TxtAccountNo.Text))
//                        {
//                            chkterms.Checked = false;
//                            CmdSave.Enabled = true;
//                            string script = "alert('Enter Account No.');";
//                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
//                            return;
//                        }

//                        if (CmbBank.SelectedValue == "0") // Assuming SelectedValue is a string
//                        {
//                            chkterms.Checked = false;
//                            CmdSave.Enabled = true;
//                            string script = "alert('Choose Bank Name.');";
//                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
//                            return;
//                        }

//                        if (string.IsNullOrWhiteSpace(TxtBranchName.Text))
//                        {
//                            chkterms.Checked = false;
//                            CmdSave.Enabled = true;
//                            string script = "alert('Enter Branch Name.');";
//                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
//                            return;
//                        }

//                        if (string.IsNullOrWhiteSpace(DDLAccountType.SelectedValue))
//                        {
//                            chkterms.Checked = false;
//                            CmdSave.Enabled = true;
//                            string script = "alert('Enter Account Name.');";
//                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
//                            return;
//                        }

//                        if (string.IsNullOrWhiteSpace(txtIfsCode.Text))
//                        {
//                            chkterms.Checked = false;
//                            CmdSave.Enabled = true;
//                            string script = "alert('Enter IFSC Code.');";
//                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", script, true);
//                            return;
//                        }
//                    }

//                    var Strquery = "Insert into Trnjoining (Transid) values(" + HdnCheckTrnns.Value + ")";
//                    int UpdateData = 0;
//                    UpdateData = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Strquery));
//                    if (UpdateData > 0)
//                    {
//                        //TxtPasswd.Text = GenerateRandomString(6);
//                        strQry = "INSERT INTO m_memberMaster(SessId, IdNo, CardNo, FormNo, KitId, UpLnFormNo, RefId, LegNo, RefLegNo, RefFormNo, " +
//                   "MemFirstName, MemLastName, MemRelation, MemFName, MemDOB, MemGender, MemOccupation, NomineeName, Address1, Address2, Post, " +
//                   "Tehsil, City, District, StateCode, CountryId, PinCode, PhN1, Fax, Mobl, MarrgDate, Passw, Doj, Relation, PanNo, " +
//                   "BankID, MICRCode, BranchName, EMail, BV, UpGrdSessId, E_MainPassw, EPassw, ActiveStatus, billNo, RP, HostIp, " +
//                   "PID, Paymode, ChDDNo, ChDDBankID, ChDDBank, ChddDate, ChDDBranch, IsPanCard, AadharNo, AadharNo2, AAdharNo3, Fld5, walletaddress, usercode) " +
//                   "VALUES (" + Convert.ToInt32(Session["SessID"]) + ", '0', 0, 0, " + Convert.ToInt32(Session["Kitid"]) + ", " +
//                   Convert.ToInt32(Session["Uplnr"]) + ", 0, '" + iLeg + "', 0, " + Convert.ToInt32(Session["Refral"]) + ", '" + ClearInject(txtFrstNm.Text.ToUpper()) + "', " +
//                   "'', '" + CmbType.SelectedValue + "', '" + ClearInject(txtFNm.Text.ToUpper()) + "', '" + strDOB + "', '" + cGender + "', '', " +
//                   "'" + ClearInject(txtNominee.Text.ToUpper()) + "', '" + ClearInject(txtAddLn1.Text.ToUpper()) + "', '', '', '" + dblTehsil + "', " +
//                   "'" + dblTehsil + "', '" + dblDistrict + "', " + dblState + ", " + ddlCountryNAme.SelectedValue + ", '" + txtPinCode.Text + "', " +
//                   "'" + txtPhNo.Text + "', 'CHOOSE ACCOUNT TYPE', '" + txtMobileNo.Text + "', '" + strDOM + "', '" + ClearInject(TxtPasswd.Text) + "', " +
//                   "GETDATE(), '" + ClearInject(txtRelation.Text.ToUpper()) + "', '" + ClearInject(txtPanNo.Text.ToUpper()) + "', " + dblBank + ", " +
//                   "'" + (ClearInject(TxtMICR.Text.ToUpper())) + "', '" + (TxtBranchName.Text.ToUpper()) + "', '" + ClearInject(txtEMailId.Text) + "', " +
//                   Convert.ToInt32(Session["Bv"]) + ", 0, '" + ClearInject(TxtPasswd.Text) + "', '" + ClearInject(TxtPasswd.Text) + "', '" + Session["JoinStatus"] + "', " +
//                   "'" + InVoiceNo + "', '" + Session["RP"] + "', '" + HostIp + "', " + Convert.ToInt32(DdlPaymode.SelectedValue) + ", " +
//                   "'" + (DdlPaymode.SelectedItem.Text.ToUpper()) + "', '" + ClearInject(TxtDDNo.Text) + "', '0', '" + ClearInject(TxtIssueBank.Text.ToUpper()) + "', " +
//                   "'" + (TxtDDDate.Text) + "', '" + ClearInject(TxtIssueBranch.Text) + "', 'N', '" + ClearInject(TxtAAdhar1.Text) + "', " +
//                   "'" + ClearInject(TxtAadhar2.Text) + "', '" + ClearInject(TxtAadhar3.Text) + "', '" + Session["TransIDJoin"] + "', '" + ClearInject(TxtWalletaddress.Text) + "', '" + ddlMobileNAme.Text + "')";

//                        int isOk = 0;
//                        int retryqry = 0;
//                    Savedata:
//                        ;
//                        isOk = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strQry));
//                        LastInsertID = "0";
//                        if ((isOk > 0))
//                        {
//                            string membername = "";
//                            string SPONSORID1 = "";
//                            string SPONSORnAME = "";
//                            string Doj = "";
//                            string kitamount = "";
//                            string Email = "";
//                            string Password = "";
//                            string EPassword = "";
//                            DataTable Dtsms = new DataTable();
//                            string strSql = string.Empty;

//                            // Execute stored procedure to get login details
//                            strSql = IsoStart + " EXEC Sp_GetLoginDetail " + IsoEnd;
//                            Dtsms = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql).Tables[0];

//                            if (Dtsms.Rows.Count > 0)
//                            {
//                                membername = Dtsms.Rows[0]["MemfirstName"].ToString() + " " + Dtsms.Rows[0]["MemLastName"].ToString();
//                                SPONSORID1 = Dtsms.Rows[0]["SPONSORID"].ToString();
//                                SPONSORnAME = Dtsms.Rows[0]["SPONSORnAME"].ToString();
//                                Doj = Dtsms.Rows[0]["JoiningDate"].ToString();
//                                kitamount = Dtsms.Rows[0]["kitamount"].ToString();
//                                Email = Dtsms.Rows[0]["Email"].ToString();
//                                LastInsertID = Dtsms.Rows[0]["IDNO"].ToString();
//                                Password = Dtsms.Rows[0]["Passw"].ToString();
//                                EPassword = Dtsms.Rows[0]["ePassw"].ToString();
//                                Session["Kit"] = Dtsms.Rows[0]["IsBill"];

//                                //FUND_LOGIN_CHECK(Dtsms.Rows[0]["IDNO"].ToString(), Dtsms.Rows[0]["Passw"].ToString(), Dtsms.Rows[0]["formno"].ToString());
//                            }
//                            else
//                            {
//                                LastInsertID = "10001";
//                            }


//                            CmdSave.Enabled = true;
//                            SendToMemberMail(LastInsertID, Email, membername, Password, EPassword);
//                            Session["LASTID"] = LastInsertID;
//                            Session["Join"] = "YES";
//                            Response.Redirect("Welcome.Aspx?IDNo=" + LastInsertID, false);
//                        }
//                        else
//                        {
//                            if (retryqry <= 2)
//                            {
//                                retryqry += 1;
//                                goto Savedata;
//                            }
//                            CmdSave.Enabled = true;
//                            chkterms.Checked = false;
//                            scrname = "<SCRIPT language='javascript'>alert('Try Again Later.');" + "</SCRIPT>";
//                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Try Again Later.');", true);
//                        }
//                    }
//                    else
//                    {
//                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('This id already register.!');location.replace('Registration.aspx');", true);
//                        return;
//                    }

//                }
//            }
//            catch (Exception e)
//            {
//                CmdSave.Enabled = true;
//                chkterms.Checked = false;
//                string scrname = "<SCRIPT language='javascript'>alert('" + e.Message + "');</SCRIPT>";
//                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + e.Message + "');", true);

//                string path = HttpContext.Current.Request.Url.AbsoluteUri;
//                string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
//                ObjDAL.WriteToFile(text + e.Message);
//                Response.Write("Try later.");
//                return;
//            }

//        }
//        catch (Exception ex)
//        {
//            dbConnect.closeConnection();
//        }
//    }
//    protected void RbtnLegNo_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        checkAvailLeg();
//    }
//    public bool SendMail(string otp)
//    {
//        try
//        {
//            string strMsg = "";
//            string emailAddress = txtEMailId.Text.Trim();
//            System.Net.Mail.MailAddress sendFrom = new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
//            System.Net.Mail.MailAddress sendTo = new System.Net.Mail.MailAddress(emailAddress);
//            System.Net.Mail.MailMessage myMessage = new System.Net.Mail.MailMessage(sendFrom, sendTo);

//            strMsg = "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\"> " +
//                     "<tr>" +
//                     "<td>" +
//                     "Your OTP for Registration is <span style=\"font-weight: bold;\">" + otp + "</span> (valid for 5 minutes)." +
//                     "<br />" +
//                     "</td>" +
//                     "</tr>" +
//                     "</table>";

//            myMessage.Subject = "Thanks For Connecting!!!";
//            myMessage.Body = strMsg;
//            myMessage.IsBodyHtml = true;

//            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
//            smtp.UseDefaultCredentials = false;
//            smtp.Port = 587;
//            smtp.EnableSsl = false;
//            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
//            smtp.Credentials = new System.Net.NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());

//            smtp.Send(myMessage);

//            txtRefralId.Enabled = false;
//            txtUplinerId.Enabled = false;
//            TxtWalletaddress.Enabled = false;
//            txtFrstNm.Enabled = false;
//            txtMobileNo.Enabled = false;
//            txtEMailId.Enabled = false;
//            ddlCountryNAme.Enabled = false;
//            RbtnLegNo.Enabled = false;
//            chkterms.Enabled = false;

//            return true;
//        }
//        catch (Exception ex)
//        {
//            throw new Exception(ex.Message);
//        }
//    }
//    public bool SendToMemberMail(string IdNo, string Email, string MemberName, string Password, string TransactionPassword)
//    {
//        try
//        {
//            System.Net.Mail.MailAddress sendFrom =
//                new System.Net.Mail.MailAddress(Session["CompMail"].ToString());
//            System.Net.Mail.MailAddress sendTo =
//                new System.Net.Mail.MailAddress(Email);
//            System.Net.Mail.MailMessage myMessage =
//                new System.Net.Mail.MailMessage(sendFrom, sendTo);

//            string strMsg = @"
//<!DOCTYPE html>
//<html lang='en'>
//<head>
//<meta charset='UTF-8'>
//<meta name='viewport' content='width=device-width,initial-scale=1.0'>
//<title>Welcome to ePay Digital India</title>
//<style>
//  body{margin:0;padding:0;background:#f4f6fb;font-family:Arial,Helvetica,sans-serif}
//  table{border-collapse:collapse}
//  .outer{width:100%;background:#f4f6fb;padding:32px 16px}
//  .card{width:560px;margin:0 auto;background:#ffffff;border-radius:12px;overflow:hidden;border:1px solid #e2e8f0}
//  .header{background:#0f2b5b;padding:32px 36px 28px}
//  .header-brand{margin:0 0 8px;font-size:12px;color:#93c5fd;letter-spacing:0.5px;font-family:Arial,sans-serif}
//  .header-title{margin:0;font-size:20px;font-weight:bold;color:#ffffff;line-height:1.35;font-family:Arial,sans-serif}
//  .body{padding:28px 36px}
//  .p{margin:0 0 16px;font-size:14px;color:#1e293b;line-height:1.75;font-family:Arial,sans-serif}
//  .name{color:#1a4db3;font-weight:bold}
//  .cred-table{width:100%;background:#f8faff;border-radius:8px;margin:0 0 20px;border:1px solid #dbeafe}
//  .cred-row-top{padding:12px 20px 8px;border-bottom:1px solid #e2e8f0}
//  .cred-row-mid{padding:8px 20px;border-bottom:1px solid #e2e8f0}
//  .cred-row-bot{padding:8px 20px 12px}
//  .cred-label{font-size:13px;color:#64748b;font-family:Arial,sans-serif}
//  .cred-val{font-size:13px;font-weight:bold;color:#1e293b;font-family:'Courier New',monospace;text-align:right}
//  .btn{display:inline-block;background:#1a4db3;color:#ffffff;text-decoration:none;padding:11px 26px;border-radius:8px;font-size:14px;font-weight:bold;font-family:Arial,sans-serif;margin:0 0 20px}
//  .warn{font-size:13px;color:#92400e;background:#fff7ed;border:1px solid #fed7aa;border-radius:8px;padding:12px 16px;margin:0 0 20px;line-height:1.65;font-family:Arial,sans-serif}
//  .footer{border-top:1px solid #e2e8f0;padding:20px 36px}
//  .footer-p{margin:0;font-size:13px;color:#64748b;line-height:1.65;font-family:Arial,sans-serif}
//  .footer-name{color:#1e293b;font-weight:bold}
//</style>
//</head>
//<body>
//<div class='outer'>
//  <div class='card'>

//    <div class='header'>
//      <p class='header-brand'>ePay Digital India Pvt. Ltd.</p>
//      <p class='header-title'>Welcome to ePay Digital India &ndash;<br>Your Account is Ready</p>
//    </div>

//    <div class='body'>
//      <p class='p'>Dear <span class='name'>" + MemberName + @"</span>,</p>
//      <p class='p'>Welcome to <strong>ePay Digital India Pvt. Ltd.</strong> &mdash; your gateway to smart digital services and earning opportunities.</p>
//      <p class='p'>Your account has been successfully created. Please find your login credentials below:</p>

//      <table class='cred-table'>
//        <tr>
//          <td class='cred-row-top'>
//            <table width='100%'><tr>
//              <td class='cred-label'>User ID</td>
//              <td class='cred-val'>" + IdNo + @"</td>
//            </tr></table>
//          </td>
//        </tr>
//        <tr>
//          <td class='cred-row-mid'>
//            <table width='100%'><tr>
//              <td class='cred-label'>Login Password</td>
//              <td class='cred-val'>" + Password + @"</td>
//            </tr></table>
//          </td>
//        </tr>
//        <tr>
//          <td class='cred-row-bot'>
//            <table width='100%'><tr>
//              <td class='cred-label'>Transaction Password</td>
//              <td class='cred-val'>" + TransactionPassword + @"</td>
//            </tr></table>
//          </td>
//        </tr>
//      </table>

//      <a href='https://epayindia.in/' class='btn' style='color: white;'>Login Now &rarr;</a>

//      <p class='warn'>For your security, we strongly recommend changing your password after your first login.</p>

//      <p class='p' style='margin:0'>If you need any assistance, our support team is always here to help.</p>
//    </div>

//    <div class='footer'>
//      <p class='footer-p'>Warm regards,<br><span class='footer-name'>Team ePay Digital India Pvt. Ltd.</span></p>
//    </div>

//  </div>
//</div>
//</body>
//</html>";

//            myMessage.Subject = "Welcome to ePay Digital India – Your Account is Ready";
//            myMessage.Body = strMsg;
//            myMessage.IsBodyHtml = true;

//            System.Net.Mail.SmtpClient smtp =
//                new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
//            smtp.Port = 587;
//            smtp.EnableSsl = true;
//            smtp.UseDefaultCredentials = false;
//            smtp.Credentials =
//                new System.Net.NetworkCredential(
//                    Session["CompMail"].ToString(),
//                    Session["MailPass"].ToString()
//                );

//            smtp.Send(myMessage);
//            return true;
//        }
//        catch (Exception)
//        {
//            Response.Write("Mail could not be sent. Please try again later.");
//            return false;
//        }
//    }
//    public static bool IsValidEmail(string email)
//    {
//        if (string.IsNullOrWhiteSpace(email))
//            return false;

//        return System.Text.RegularExpressions.Regex.IsMatch(
//            email,
//            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
//            System.Text.RegularExpressions.RegexOptions.IgnoreCase
//        );
//    }
//    public static bool IsValidMobile(string mobile)
//    {
//        if (string.IsNullOrWhiteSpace(mobile))
//            return false;

//        return System.Text.RegularExpressions.Regex.IsMatch(
//            mobile,
//            @"^\d{10}$"
//        );
//    }
//    public string ValidateRegistration(string sponsorId, string name, string country, string mobile, string email, bool isTermsAccepted)
//    {
//        if (string.IsNullOrWhiteSpace(sponsorId))
//            return "Sponsor ID is required";
//        if (string.IsNullOrWhiteSpace(name))
//            return "Name is required";
//        if (string.IsNullOrWhiteSpace(country) || country == "0")
//            return "Please select country";
//        if (string.IsNullOrWhiteSpace(mobile))
//            return "Mobile No. is required";
//        if (!IsValidMobile(mobile))
//            return "Enter valid mobile number";
//        if (string.IsNullOrWhiteSpace(email))
//            return "Email is required";
//        if (!IsValidEmail(email))
//            return "Enter valid email address";

//        if (!isTermsAccepted)
//            return "Please accept terms & conditions";

//        return "OK";
//    }
//    protected void CmdSave_Click(object sender, EventArgs e)
//    {
//        try
//        {
//            string result = ValidateRegistration(txtRefralId.Text.Trim(), txtFrstNm.Text.Trim(), ddlCountryNAme.SelectedValue, txtMobileNo.Text.Trim(), txtEMailId.Text.Trim(), chkterms.Checked);
//            if (result != "OK")
//            {
//                string scrname = "<SCRIPT language='javascript'>alert('" + result + "');</SCRIPT>";
//                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//                return;
//            }
//            else
//            {
//                SaveIntoDB();
//            }
//            //if (!chkterms.Checked)
//            //{
//            //    string scrname = "<SCRIPT language='javascript'>alert('Please select Terms and Conditions');</SCRIPT>";
//            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//            //    return;
//            //}
//            //else
//            //{

//            //    SaveIntoDB();
//            //}

//        }
//        catch (Exception ex)
//        {
//            throw new Exception(ex.Message);
//        }
//    }
//    protected void BtnOtp_Click(object sender, EventArgs e)
//    {
//        //try
//        //{
//        //    string scrname = "";
//        //    string transPassw = TxtOtp.Text;
//        //    transPassw = transPassw.Trim();
//        //    DataTable dt1 = new DataTable();
//        //    ObjDAL = new DAL();
//        //    Session["OtpCount"] = Convert.ToInt32(Session["OtpCount"]) + 1;

//        //    if (Session["OTP_"] != null && Session["OTP_"].ToString() == TxtOtp.Text.Trim())
//        //    {
//        //        string query = "SELECT TOP 1 * FROM " + ObjDAL.dBName + "..AdminLogin AS a WHERE EmailID = '" + txtEMailId.Text.Trim() + "' ";
//        //        query += "AND emailotp = '" + TxtOtp.Text.Trim() + "' AND ForType = 'Registartion' ORDER BY AID DESC";
//        //        dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];

//        //        if (dt1.Rows.Count > 0)
//        //        {
//        //            SaveIntoDB();
//        //        }
//        //    }
//        //    else
//        //    {
//        //        TxtOtp.Text = "";

//        //        if (Convert.ToInt32(Session["OtpCount"]) >= 3)
//        //        {
//        //            Session["OtpCount"] = 0;
//        //            scrname = "<script language='javascript'>alert('You have tried 3 times with invalid OTP.\\n Please generate OTP again.');</script>";
//        //            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('You have tried 3 times with invalid OTP.\\n Please generate OTP again.');", true);
//        //            ResendOtp.Visible = true;
//        //            BtnOtp.Visible = false;
//        //            divOtp.Visible = false;
//        //        }
//        //        else
//        //        {
//        //            scrname = "<script language='javascript'>alert('Invalid OTP.');</script>";
//        //            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Invalid OTP.');", true);
//        //        }
//        //    }
//        //}
//        //catch (Exception ex)
//        //{
//        //    throw new Exception(ex.Message);
//        //}
//    }
//    protected void ResendOtp_Click(object sender, EventArgs e)
//    {
//        try
//        {
//            Session["OTP_"] = "";
//            int otp = 0;
//            Random rs = new Random();
//            otp = rs.Next(100001, 999999);

//            if (SendMail(otp.ToString()))
//            {
//                string emailId = txtEMailId.Text.ToString();
//                string memberName = "";
//                string mobileNo = "0";
//                string sms = "";
//                ObjDAL = new DAL();
//                int result = 0;
//                string query = "";

//                query = "INSERT INTO AdminLogin (UserID, Username, Passw, MobileNo, OTP, LoginTime, emailotp, EmailID, ForType) " +
//                        "VALUES ('0', '" + memberName + "', '" + TxtOtp.Text + "', '" + mobileNo + "', '" + otp + "', GETDATE(), '" + otp + "', " +
//                        "'" + txtEMailId.Text.Trim() + "', 'Registartion')";

//                result = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, query));

//                if (result > 0)
//                {
//                    Session["OTP_"] = otp;
//                    divOtp.Visible = true;
//                    BtnOtp.Visible = true;
//                    ResendOtp.Visible = true;
//                    string scrname = "<script language='javascript'>alert('OTP Sent On Mail');</script>";
//                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//                    return;
//                }
//                else
//                {
//                    string scrname = "<script language='javascript'>alert('Try Later');</script>";
//                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//                    return;
//                }
//            }
//            else
//            {
//                string scrname = "<script language='javascript'>alert('OTP Try Later');</script>";
//                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
//                return;
//            }
//        }
//        catch (Exception ex)
//        {
//            throw new Exception(ex.Message);
//        }

//    }
//}
