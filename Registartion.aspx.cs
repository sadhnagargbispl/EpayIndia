using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Irony;
using System.Configuration;
using System.Web.UI;
using System.Web;

using System.Web.UI.WebControls;
using System.Net.Http;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.ServiceModel.Activities;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Office2016.Excel;

public partial class Registartion : System.Web.UI.Page
{
    private double _dblAvailLeg = 0;
    private clsGeneral dbGeneral = new clsGeneral();
    private cls_DataAccess dbConnect;
    private DAL ObjDAL;
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dRead;
    public string DsnName, UserName, Passw, role, token, refreshToken, name;
    private string strQuery, strCaptcha;
    //private System.Data.DataTable tmpTable = new System.Data.DataTable();
    //private AccClass.MyAccClass.NewClass QryCls = new AccClass.MyAccClass.NewClass();
    private int minSpnsrNoLen, minScrtchLen;
    private string Authorization;
    private double Upln, dblSpons, dblState, dblBank, dblIdNo;
    private string dblDistrict, dblTehsil, IfSC;
    private string dblPlan;
    private DateTime CurrDt;
    private string scrname;
    private string LastInsertID = "";
    private string InVoiceNo;
    private int SupplierId;
    private string BillNo;
    private string TaxType;
    private string BillDate;
    private int SBillNo;
    private string SoldBy = "WR";
    private string FType;
    private string IsoStart;
    private string IsoEnd;
    private SqlConnection cnn;
    private DataSet Ds = new DataSet();
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    private DataTable dt = new DataTable();
    System.Data.DataTable tmpTable = new System.Data.DataTable();
    public DateTime Now { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["SessID"] = 1;
        cnn = new SqlConnection(constr);
        dbConnect = new cls_DataAccess((string)Application["Connect"]);
        try
        {
            var str1 = "exec('Create table Trnjoining ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," + "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnjoining] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')";
            int i = 0;
            i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str1);
        }
        catch (Exception ex)
        {
        }
        try
        {
            if (!Page.IsPostBack)
            {
                if (RadioButtonUserType.SelectedValue.ToUpper() == "ONE WAVE USER")
                {
                    DivUserSelectTypeName.Visible = false;
                    DivUserSelectTypeMobile.Visible = false;
                    DivUserSelectTypeEmail.Visible = true;
                    DivUserSelectTypePassword.Visible = false;
                    BtnProceedToPay.Visible = false;
                    BtnVerify.Visible = true;
                }
                else
                {
                    DivUserSelectTypeName.Visible = true;
                    DivUserSelectTypeMobile.Visible = true;
                    DivUserSelectTypeEmail.Visible = true;
                    DivUserSelectTypePassword.Visible = true;
                    BtnProceedToPay.Visible = true;
                    BtnVerify.Visible = false;
                }
                Session["OtpCount"] = 0;
                Session["OtpTime"] = null;
                Session["Retry"] = null;
                Session["OTP_"] = null;
                HdnCheckTrnns.Value = GenerateRandomStringactive(6);
                int LegNo = 1;
                if (LegNo == 1)
                    RbtnLegNo.SelectedIndex = 0;
                else
                    RbtnLegNo.SelectedIndex = 1;
                RbtnLegNo.Enabled = false;
                Session["iLeg"] = LegNo;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public string GenerateRandomStringactive(int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i <= iLength - 1; i++)
            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
        return sResult;
    }
    protected void txtemail_TextChanged(object sender, System.EventArgs e)
    {
        if (txtemail.Text != "")
        {
            DataTable DtEmail = new DataTable();
            DataSet DsEmail = new DataSet();
            string strSql = " select Count(Email) as Emailcount from M_Membermaster where Email='" + txtemail.Text.Trim() + "'";
            DsEmail = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            DtEmail = DsEmail.Tables[0];
            if (Convert.ToInt32(DtEmail.Rows[0]["Emailcount"].ToString()) >= 1)
            //if (DtEmail.Rows.Count > 1)
            {
                BtnProceedToPay.Enabled = true;
                scrname = "<SCRIPT language='javascript'>alert('Already Registerd by this Emailid.');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                txtemail.Text = "";
                return;
            }
        }
    }
    public bool IsValidMobileNumber(string mobileNumber)
    {
        string regexPattern = "^[0-9]{10}$";
        Regex regex = new Regex(regexPattern);
        return regex.IsMatch(mobileNumber);
    }
    protected void txtmobl_TextChanged(object sender, System.EventArgs e)
    {     
        if (!string.IsNullOrEmpty(txtmobl.Text))
        {
            string moblno = txtmobl.Text;
            string check = moblno.Substring(0, 1);

            if (check == "0")
            {
                txtmobl.Text = "";
                BtnProceedToPay.Enabled = true;
                string scrname = "<SCRIPT language='javascript'>alert('Invalid Mobile No.!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
        }
        if (IsValidMobileNumber(txtmobl.Text))
        {
            if (txtmobl.Text != "")
            {
                DataTable DtEmail = new DataTable();
                DataSet DsEmail = new DataSet();
                string strSql = " select Count(mobl) as Mobile from M_Membermaster where mobl='" + txtmobl.Text.Trim() + "' ";
                DsEmail = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
                DtEmail = DsEmail.Tables[0];
                if (Convert.ToInt32(DtEmail.Rows[0]["Mobile"]) >= 1)
                {
                    BtnProceedToPay.Enabled = true;
                    scrname = "<SCRIPT language='javascript'>alert('Already Registerd by this MobileNo.');" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                    txtmobl.Text = "";
                    return;
                }
            }
        }
        else
        {
            scrname = "<SCRIPT language='javascript'>alert(Enter Valid Mobile No.');" + "</SCRIPT>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            txtmobl.Text = "0";
            return;
        }    
    }
    private string ClearInject(string StrObj)
    {
        StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
        return StrObj.Trim();
    }
    private string Val(string v)
    {
        throw new NotImplementedException();
    }
    public string Fun_VerifyEmail()
    {
        string URL = string.Empty;
        string sResult = string.Empty;
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;
        string postData = string.Empty;
        string message = string.Empty;
        string str = string.Empty;
        DataSet data = new DataSet();
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

             URL = "https://uatwavemoney.onewave.app/api/shop/auth_login";
            string apiKey = "Xr21X8iqmq8n212gj26wIxwFMyq3ve";
            string email = txtemail.Text.Trim().ToLower();

             postData = "{\"email\": \"" + email + "\"}";

            // Log API request (Use parameters instead of string concatenation)
            string sqlReq = "INSERT INTO Tbl_ApiRequest_ResponsePayment (ReqID, Request, postdata) " +
                            "VALUES (@ReqID, @Request, @PostData)";
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq,
                new SqlParameter("@ReqID", sResult.Trim()),
                new SqlParameter("@Request", URL),
                new SqlParameter("@PostData", postData)
            );

            // Prepare HTTP request
            HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create(URL);
            tRequest.Method = "POST";
            tRequest.ContentType = "application/json";
            tRequest.Headers.Add("key", apiKey);

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            //string responseText = "";

            //using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
            //using (Stream dataStream = tResponse.GetResponseStream())
            //using (StreamReader tReader = new StreamReader(dataStream))
            //{
            //    responseText = tReader.ReadToEnd();
            //}
            string responseText = "";

            try
            {
                using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
                using (Stream dataStream = tResponse.GetResponseStream())
                using (StreamReader tReader = new StreamReader(dataStream))
                {
                    responseText = tReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                // 400/404/500 responses come here
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    using (var dataStream = errorResponse.GetResponseStream())
                    using (var reader = new StreamReader(dataStream))
                    {
                        // READ JSON EVEN IF STATUS = 400
                        responseText = reader.ReadToEnd();
                    }
                }
                else
                {
                    responseText = ex.Message;
                }
            }

            // Log API response
            string sqlRes = "UPDATE Tbl_ApiRequest_ResponsePayment SET Response = @Response WHERE ReqID = @ReqID";
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes,
                new SqlParameter("@Response", responseText),
                new SqlParameter("@ReqID", sResult.Trim())
            );

            // Parse JSON
             data = convertJsonStringToDataSet(responseText);

            string statusCode = data.Tables[0].Rows[0]["status"].ToString();
             message = data.Tables[0].Rows[0]["message"].ToString();
        }
        catch (Exception ex)
        {
            // Log error response
            string sqlRes = "UPDATE Tbl_ApiRequest_ResponsePayment SET Response = @Response WHERE ReqID = @ReqID";
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes,
                new SqlParameter("@Response", ex.ToString()),
                new SqlParameter("@ReqID", sResult.Trim())
            );
        }
        return message;
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
    protected void BtnProceedToPay_Click(object sender, EventArgs e)
    {
        errMsg.Text = "";
        if (Txtname.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Name.');" + "</SCRIPT>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        if (txtmobl.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Mobile No.');" + "</SCRIPT>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        if (!string.IsNullOrEmpty(txtmobl.Text))
        {
            string moblno = txtmobl.Text;
            string check = moblno.Substring(0, 1);

            if (check == "0")
            {
                txtmobl.Text = "";
                BtnProceedToPay.Enabled = true;
                string scrname = "<SCRIPT language='javascript'>alert('Invalid Mobile No.!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            string mobileNumber = txtmobl.Text;

            if (Regex.IsMatch(mobileNumber, @"^\d{10,}$"))
            {
                //   MessageBox.Show("Valid mobile number.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                txtmobl.Text = "";
                BtnProceedToPay.Enabled = true;
                string scrname = "<SCRIPT language='javascript'>alert('Invalid Mobile No.!');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }

        }
        if (txtemail.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Email id.');" + "</SCRIPT>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        if (txtemail.Text != "")
        {
            DataTable DtEmail = new DataTable();
            DataSet DsEmail = new DataSet();
            string strSql = " select Count(Email) as Email from M_Membermaster where Email = '" + txtemail.Text.Trim() + "'";
            DsEmail = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            DtEmail = DsEmail.Tables[0];
            if (DtEmail.Rows.Count > 1)
            {
                BtnProceedToPay.Enabled = true;
                scrname = "<SCRIPT language='javascript'>alert('Already Registerd by this Emailid.');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                txtemail.Text = "";
                return;
            }
        }
        if (TxtPasswd.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Password.');" + "</SCRIPT>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        SaveIntoDB();
    }
    public void SaveIntoDB()
    {
        try
        {
            int updateeffect;
            string StrSql2 = "Insert into Trnjoining (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
            updateeffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, StrSql2));
            if (updateeffect > 0)
            {
                string strQry = "";
                string strDOB = "", strDOM = "", strDOJ, s;
                int iLeg;
                char cGender, cMarried;
                cGender = 'M';
                cMarried = 'N';
                string Aadharno = "";
                string HostIp = Context.Request.UserHostAddress.ToString();
                int DistrictCode, CityCode, VillageCode;
                BtnProceedToPay.Enabled = false;
                string s1 = "";
                try
                {
                    //if (Validt_SpnsrDtl("") == "OK")
                    //{
                    iLeg = 1;
                    string q = "";
                    int i = 0;
                    DataTable Dt = new DataTable();
                    int BankCode = 0;
                    int AreaCode = 0;
                    AreaCode = 0;
                    string RegestType = "IN";
                    int PostalAreaCode = 0;
                    if (dblDistrict == null)
                        dblDistrict = "";
                    DistrictCode = 0;
                    CityCode = 0;
                    VillageCode = 0;
                    dblPlan = "0";
                    InVoiceNo = "0";
                    if (Convert.ToInt32(Session["SessID"]) == 0)
                        Session["SessID"] = 1;
                    string Name = "";
                    string fathername = "";
                    string passw = "";
                    string lastname = "";
                    passw = TxtPasswd.Text;
                    fathername = ClearInject(txtfather.Text).ToString().ToUpper();
                    Name = ClearInject(Txtname.Text).ToString().ToUpper();
                    lastname = ClearInject(txtlastname.Text).ToString().ToUpper();
                    strQry = " insert into m_memberMaster (SessId,IdNo,CardNo,FormNo,KitId,UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo,";
                    strQry += "MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation,NomineeName,Address1,Address2,Post,";
                    strQry += "Tehsil,City,District,StateCode,CountryId,PinCode,PhN1,Fax,Mobl,MarrgDate,Passw,Doj,Relation,PanNo,";
                    strQry += "BankID,MICRCode,BranchName,EMail,BV,UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp,";
                    strQry += " PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,IFSCode,Acno,AreaName,AreaCode,Fld2,AadharNo3,RegType,RegNo)";
                    strQry += "Values(" + Session["SessID"] + ",'" + txtemail.Text + "',0,0,1,0,0," + iLeg + ",0,";
                    strQry += "1,'" + ClearInject(Name).ToString().ToUpper() + "','" + ClearInject(lastname).ToString().ToUpper() + "','','" + ClearInject(fathername).ToString().ToUpper() + "',";
                    strQry += "getdate(),'" + cGender + "','','','" + ClearInject(TxtAddress.Text).ToString() + "','','";
                    strQry += "" + "','" + dblTehsil + "','" + txtTehsil.Text + "','" + txtDistrict.Text + "',0,1,";
                    strQry += "'" + txtPinCode.Text + "','0','CHOOSE ACCOUNT TYPE','" + txtmobl.Text + "',getdate(),'" + ClearInject(passw) + "',";
                    strQry += "Getdate(),'','','" + dblBank + "','','','" + ClearInject(txtemail.Text) + "',0,0,'" + ClearInject(passw) + "',";
                    strQry += "'" + ClearInject(passw) + "','N','" + InVoiceNo + "','0','" + HostIp + "','0','0','','0','','', '','N','','','',";
                    strQry += "'" + VillageCode + "','','','" + RegestType + "','" + RadioButtonUserType.SelectedValue + "')";
                    int isOk = 0;
                    isOk = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strQry));
                    LastInsertID = "0";
                    if (isOk > 0)
                    {
                        string membername = "";
                        string Email = "";
                        string Password = "";
                        string str = "";
                        int kitid = 0;
                        str = "EXEC Sp_GetProfile ";
                        DataTable Dt1 = new DataTable();
                        Dt1 = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];
                        string Lastformno = "";
                        if (Dt1.Rows.Count > 0)
                        {
                            membername = Dt1.Rows[0]["MemfirstName"] + " " + Dt1.Rows[0]["MemLastName"];
                            Email = Dt1.Rows[0]["Email"].ToString();
                            LastInsertID = Dt1.Rows[0]["IDNO"].ToString();
                            Lastformno = Dt1.Rows[0]["Formno"].ToString();
                            Password = Dt1.Rows[0]["Passw"].ToString();
                            Session["Kit"] = Dt1.Rows[0]["IsBill"].ToString();
                            kitid = Convert.ToInt32(Dt1.Rows[0]["kitid"].ToString());
                        }
                        else
                            LastInsertID = "10001";
                        BtnProceedToPay.Enabled = true;
                        Session["LASTID"] = LastInsertID;
                        Session["Join"] = "YES";
                        Response.Redirect("thankyou.aspx", false);
                    }
                    else
                    {
                        BtnProceedToPay.Enabled = true;
                        scrname = "<SCRIPT language='javascript'>alert('Try Again Later.');" + "</SCRIPT>";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Try Again Later.');", true);
                    }
                }
                catch (Exception e)
                {
                    BtnProceedToPay.Enabled = true;
                    scrname = "<SCRIPT language='javascript'>alert('" + e.Message + "');" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + e.Message + "');", true);
                    string path = HttpContext.Current.Request.Url.AbsoluteUri;
                    Response.Write("Try later.");
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('This id already register.!');location.replace('Registartion.aspx');", true);
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "location.replace('Registartion.aspx');", true);
            return;
        }
    }
    protected void RadioButtonUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioButtonUserType.SelectedValue.ToUpper() == "ONE WAVE USER")
        {
            DivUserSelectTypeName.Visible = false;
            DivUserSelectTypeMobile.Visible = false;
            DivUserSelectTypeEmail.Visible = true;
            DivUserSelectTypePassword.Visible = false;
            BtnProceedToPay.Visible = false;
            BtnVerify.Visible = true;
        }
        else
        {
            DivUserSelectTypeName.Visible = true;
            DivUserSelectTypeMobile.Visible = true;
            DivUserSelectTypeEmail.Visible = true;
            DivUserSelectTypePassword.Visible = true;
            BtnProceedToPay.Visible = true;
            BtnVerify.Visible = false;
        }
    }
    protected void BtnVerify_Click(object sender, EventArgs e)
    {
        if (txtemail.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Please Enter Email id.');" + "</SCRIPT>";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            return;
        }
        if (txtemail.Text != "")
        {
            DataTable DtEmail = new DataTable();
            DataSet DsEmail = new DataSet();
            string strSql = " select Count(Email) as Email from M_Membermaster where Email = '" + txtemail.Text.Trim() + "'";
            DsEmail = SqlHelper.ExecuteDataset(constr, CommandType.Text, strSql);
            DtEmail = DsEmail.Tables[0];
            if (DtEmail.Rows.Count > 1)
            {
                BtnProceedToPay.Enabled = true;
                scrname = "<SCRIPT language='javascript'>alert('Already Registerd by this Emailid.');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                txtemail.Text = "";
                return;
            }
        }
        string message = Fun_VerifyEmail();
        if (message.ToString().ToUpper() == "ACCESS TOKEN FETCH SUCCESSFUL")
        {
            LblNameEmail.Text = "Verified";
            DivUserSelectTypeName.Visible = true;
            DivUserSelectTypeMobile.Visible = true;
            DivUserSelectTypeEmail.Visible = true;
            DivUserSelectTypePassword.Visible = true;
            txtemail.Enabled = false;
            BtnProceedToPay.Visible = true;
            BtnVerify.Visible = false;
        }
        else
        {
            LblNameEmail.Text = "New User";
            DivUserSelectTypeName.Visible = true;
            DivUserSelectTypeMobile.Visible = true;
            DivUserSelectTypeEmail.Visible = true;
            DivUserSelectTypePassword.Visible = true;
            txtemail.Enabled = false;
            BtnProceedToPay.Visible = true;
            BtnVerify.Visible = false;
            //scrname = "<SCRIPT language='javascript'>alert('User not found.');" + "</SCRIPT>";
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
            //txtemail.Text = "";
            //return;
        }
    }
}

