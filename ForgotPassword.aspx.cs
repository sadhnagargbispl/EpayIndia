using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Xml;

public partial class ForgotPassword : System.Web.UI.Page
{
    string uid;
    string Pwd;
    string Memberid;
    string type;
    string scrname;
    DAL obj = new DAL();
    ModuleFunction objModuleFun;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string IsoStart;
    string IsoEnd;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            getData();
            if (Application["WebStatus"] != null)
            {
                if (Application["WebStatus"].ToString() == "N")
                {
                    Session.Abandon();
                    Response.Write("<big><b>" + Application["WebMessage"] + "</b></big>");
                    Response.End();
                    return;
                }
            }
            //if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            //{
            //    Response.Redirect("Index.aspx", false);
            //    return;
            //}

            string strURL = HttpContext.Current.Request.Url.AbsoluteUri;
            string url = "";
            string Str;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            if (!Page.IsPostBack)
            {
                HdnForgotStep.Value = "0";
                HdnForgotResult.Value = "";

                if (Request["lgnT"] != null)
                {
                    ModuleFunction objModuleFun = new ModuleFunction();

                    Str = Crypto.Decrypt(Request["lgnT"].Replace(" ", "+"));

                    Str = Str.Replace("uid=", "þ").Replace("&pwd=", "þ").Replace("&mobile=", "þ");
                    string[] qrystr = Str.Split(new string[] { "þ" }, StringSplitOptions.None);

                    if (Str.Contains("þ"))
                    {
                        int UIdIndx = Str.IndexOf("&pwd");
                        uid = qrystr[1].ToString();
                        Pwd = qrystr[2].ToString();
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                        return;
                    }
                }
                else if (Request["uid"] != null)
                {
                    uid = Request["uid"];
                    Pwd = Request["pwd"];
                    type = Request["ref"];
                    uid = uid.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                    Pwd = Pwd.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                }
                if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(Pwd))
                {
                  
                }
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void getData()
    {
        cls_DataAccess dbConnect = new cls_DataAccess(constr1);
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
    private string ClearInject(string StrObj)
    {
        if (string.IsNullOrEmpty(StrObj)) return "";
        StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
        return StrObj.Trim();
    }

  
    // =============================================
    // FORGOT - STEP 1: VALIDATE USER + SEND EMAIL
    // =============================================
    protected void BtnForgotSubmit_Click(object sender, EventArgs e)
    {
        string fid = ClearInject(txtForgotID.Text.Trim());
        string femail = txtForgotEmail.Text.Trim();

        if (string.IsNullOrEmpty(fid) || string.IsNullOrEmpty(femail))
        {
            HdnForgotResult.Value = "Please fill in all fields.";
            HdnForgotStep.Value = "1";
            return;
        }

        try
        {
            string sql = obj.Isostart + " Exec Sp_MemberForgotPassw '" + fid + "'" + obj.IsoEnd;
            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];

            if (dt.Rows.Count == 0)
            {
                HdnForgotResult.Value = "User ID not found. Please check.";
                HdnForgotStep.Value = "1";
                return;
            }

            string dbEmail = dt.Rows[0]["Email"].ToString();
            if (!femail.Equals(dbEmail, StringComparison.OrdinalIgnoreCase))
            {
                HdnForgotResult.Value = "Email ID does not match our records.";
                HdnForgotStep.Value = "1";
                return;
            }

            Session["ForgotUID"] = dt.Rows[0]["Idno"].ToString();
            Session["ForgotPassw"] = dt.Rows[0]["Passw"].ToString();
            Session["ForgotEPassw"] = dt.Rows[0]["EPassw"].ToString();
            Session["ForgotMemName"] = dt.Rows[0]["MemName"].ToString();
            Session["ForgotEmail"] = dbEmail;
            Session["CompMail"] = dt.Rows[0]["CompMail"];
            Session["MailPass"] = dt.Rows[0]["mailPass"];
            Session["MailHost"] = dt.Rows[0]["mailHost"];
            Session["CompName"] = dt.Rows[0]["CompName"];
            Session["CompWeb"] = dt.Rows[0]["WebSite"];

            string otp = GenerateOTP();
            Session["ForgotOTP"] = otp;
            Session["ForgotOTPExpiry"] = DateTime.Now.AddMinutes(10);

            SendForgotOTPMail(dbEmail, dt.Rows[0]["MemName"].ToString(), otp);

            HdnForgotResult.Value = "OK";
            HdnForgotStep.Value = "1";
        }
        catch (Exception ex)
        {
            HdnForgotResult.Value = "Error: " + ex.Message;
            HdnForgotStep.Value = "1";
        }
    }

    // =============================================
    // FORGOT - STEP 2: VERIFY OTP + SEND PASSWORD
    // =============================================
    protected void BtnForgotVerifyOTP_Click(object sender, EventArgs e)
    {
        string entered = HdnForgotOTP.Value.Trim();
        string stored = Session["ForgotOTP"] != null ? Session["ForgotOTP"].ToString() : "";
        DateTime expiry = Session["ForgotOTPExpiry"] != null ? Convert.ToDateTime(Session["ForgotOTPExpiry"]) : DateTime.MinValue;

        if (string.IsNullOrEmpty(entered) || entered.Length != 6)
        {
            HdnForgotResult.Value = "Please enter the complete 6-digit OTP.";
            HdnForgotStep.Value = "2";
            return;
        }

        if (DateTime.Now > expiry)
        {
            HdnForgotResult.Value = "OTP has expired. Please request a new one.";
            HdnForgotStep.Value = "2";
            return;
        }

        if (entered != stored)
        {
            HdnForgotResult.Value = "Invalid OTP. Please check and try again.";
            HdnForgotStep.Value = "2";
            return;
        }

        try
        {
            string fuid = Session["ForgotUID"] != null ? Session["ForgotUID"].ToString() : "";
            string fname = Session["ForgotMemName"] != null ? Session["ForgotMemName"].ToString() : "";
            string femail = Session["ForgotEmail"] != null ? Session["ForgotEmail"].ToString() : "";

            string password = GenerateSecurePassword();
            string strQry = "Update M_MemberMaster Set Passw = '" + password + "',E_MainPassw='" + password + "',epassw = '" + password + "' Where idno = '" + txtForgotID.Text.Trim() + "';";
            int i = obj.SaveData(strQry);

            SendResetConfirmationEmail(femail, fname, fuid, password, password);

            foreach (string k in new[] { "ForgotUID", "ForgotPassw", "ForgotEPassw", "ForgotMemName", "ForgotEmail", "ForgotOTP", "ForgotOTPExpiry" })
                Session.Remove(k);

            HdnForgotResult.Value = "OK";
            HdnForgotStep.Value = "2";
        }
        catch (Exception ex)
        {
            HdnForgotResult.Value = "Error sending email: " + ex.Message;
            HdnForgotStep.Value = "2";
        }
    }

    // =============================================
    // GENERATE SECURE RANDOM PASSWORD
    // =============================================
    public static string GenerateSecurePassword(int length = 12)
    {
        const string validChars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";

        char[] password = new char[length];

        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] data = new byte[length];
            rng.GetBytes(data);

            for (int i = 0; i < length; i++)
            {
                password[i] = validChars[data[i] % validChars.Length];
            }
        }

        return new string(password);
    }

    // =============================================
    // FORGOT OTP EMAIL (Orange Theme)
    // =============================================
    private void SendForgotOTPMail(string toEmail, string memberName, string otp)
    {
        string compName = Session["CompName"] != null ? Session["CompName"].ToString() : "ePay Digital";
        string subject = "Password Reset OTP - " + compName;
        string body = @"
<!DOCTYPE html><html><head><meta charset='utf-8'>
<style>
  body{margin:0;padding:0;background:#f4f6fb;font-family:Arial,sans-serif}
  .outer{width:100%;background:#f4f6fb;padding:32px 16px}
  .card{width:520px;margin:0 auto;background:#fff;border-radius:12px;overflow:hidden;border:1px solid #e2e8f0}
  .hdr{background:linear-gradient(135deg,#f97316,#e84000);padding:28px 32px}
  .hdr h2{margin:0;font-size:20px;color:#fff;font-weight:700}
  .hdr p{margin:4px 0 0;font-size:12px;color:rgba(255,255,255,0.7)}
  .bdy{padding:28px 32px}
  .greeting{font-size:15px;color:#1e293b;font-weight:600;margin-bottom:12px}
  .txt{font-size:13px;color:#475569;line-height:1.7;margin-bottom:20px}
  .otp-box{background:#fff4ef;border:1px solid #ffd4c2;border-radius:10px;padding:24px;text-align:center;margin-bottom:20px}
  .otp-lbl{font-size:11px;color:#e84000;font-weight:700;letter-spacing:1.5px;text-transform:uppercase;margin-bottom:10px}
  .otp-code{font-size:40px;font-weight:800;letter-spacing:14px;color:#e84000;font-family:'Courier New',monospace}
  .otp-valid{font-size:12px;color:#64748b;margin-top:10px}
  .warn{background:#fef9c3;border:1px solid #fde68a;border-radius:8px;padding:12px 16px;font-size:13px;color:#92400e;margin-bottom:0}
  .ftr{background:#f8fafc;border-top:1px solid #e2e8f0;padding:18px 32px;font-size:12px;color:#94a3b8;text-align:center}
</style></head><body>
<div class='outer'><div class='card'>
  <div class='hdr'><h2>Password Reset OTP</h2><p>" + compName + @"</p></div>
  <div class='bdy'>
    <p class='greeting'>Dear " + memberName + @",</p>
    <p class='txt'>We received a request to reset your account password. Use the OTP below to proceed.</p>
    <div class='otp-box'>
      <p class='otp-lbl'>Your One-Time Password</p>
      <p class='otp-code'>" + otp + @"</p>
      <p class='otp-valid'>Valid for: <b>10 Minutes</b></p>
    </div>
    <div class='warn'>WARNING: If you did not request this, please ignore this email or contact support immediately.</div>
  </div>
  <div class='ftr'>&copy; " + DateTime.Now.Year + @" " + compName + @" &middot; Automated Security Alert</div>
</div></div>
</body></html>";
        DispatchMail(toEmail, subject, body);
    }

    // =============================================
    // PASSWORD RESET CONFIRMATION EMAIL
    // =============================================
    private void SendResetConfirmationEmail(string email, string name, string idNo, string pass, string epass)
    {
        string compName = Session["CompName"] != null ? Session["CompName"].ToString() : "ePay Digital";
        string website = Session["CompWeb"] != null ? Session["CompWeb"].ToString() : "#";
        string subject = "Your Password Has Been Successfully Updated - " + compName;
        string resetTime = DateTime.Now.ToString("dd MMM yyyy, hh:mm tt");
        string body = @"
<!DOCTYPE html><html><head><meta charset='utf-8'>
<style>
  body{margin:0;padding:0;background:#f4f6fb;font-family:Arial,sans-serif}
  .outer{width:100%;background:#f4f6fb;padding:32px 16px}
  .card{width:520px;margin:0 auto;background:#fff;border-radius:12px;overflow:hidden;border:1px solid #e2e8f0}
  .hdr{background:linear-gradient(135deg,#166534,#16a34a);padding:28px 32px;text-align:center}
  .hdr h2{margin:0;font-size:18px;color:#fff;font-weight:700}
  .hdr p{margin:4px 0 0;font-size:12px;color:rgba(255,255,255,0.65)}
  .bdy{padding:28px 32px}
  .badge{display:inline-block;background:#f0fdf4;color:#16a34a;border:1px solid #bbf7d0;padding:4px 14px;border-radius:20px;font-size:11px;font-weight:700;letter-spacing:.8px;text-transform:uppercase;margin-bottom:16px}
  .greeting{font-size:15px;font-weight:600;color:#1e293b;margin-bottom:12px}
  .txt{font-size:13px;color:#475569;line-height:1.7;margin-bottom:20px}
  .info-card{background:#f8fafc;border:1px solid #e2e8f0;border-radius:10px;overflow:hidden;margin-bottom:20px}
  .info-row{display:flex;justify-content:space-between;padding:11px 18px;border-bottom:1px solid #f1f5f9}
  .info-row:last-child{border-bottom:none}
  .info-lbl{font-size:11px;font-weight:700;color:#64748b;text-transform:uppercase;letter-spacing:.8px;min-width:160px}
  .info-val{font-size:13px;font-weight:600;color:#1e293b;text-align:right;flex:1}
  .info-val.green{color:#16a34a}
  .warn{background:#fef2f2;border:1px solid #fecaca;border-radius:8px;padding:12px 16px;font-size:13px;color:#b91c1c;margin-bottom:0}
  .ftr{background:#f8fafc;border-top:1px solid #e2e8f0;padding:18px 32px;font-size:12px;color:#94a3b8;text-align:center}
  .ftr a{color:#e84000;text-decoration:none}
</style></head><body>
<div class='outer'><div class='card'>
  <div class='hdr'><h2>Password Updated Successfully</h2><p>" + compName + @"</p></div>
  <div class='bdy'>
    <span class='badge'>Confirmed</span>
    <p class='greeting'>Dear " + name + @",</p>
    <p class='txt'>Your account password has been <strong>successfully changed</strong>. Here are the details of this action:</p>
    <div class='info-card'>
      <div class='info-row'>
        <span class='info-lbl'>Account</span>
        <span class='info-val'>" + name + @"</span>
      </div>
      <div class='info-row'>
        <span class='info-lbl'>Action</span>
        <span class='info-val'>Password Reset</span>
      </div>
      <div class='info-row'>
        <span class='info-lbl'>User ID</span>
        <span class='info-val'>" + idNo + @"</span>
      </div>
      <div class='info-row'>
        <span class='info-lbl'>Login Password</span>
        <span class='info-val'>" + pass + @"</span>
      </div>
      <div class='info-row'>
        <span class='info-lbl'>Trans. Password</span>
        <span class='info-val'>" + epass + @"</span>
      </div>
      <div class='info-row'>
        <span class='info-lbl'>Date &amp; Time</span>
        <span class='info-val'>" + resetTime + @"</span>
      </div>
      <div class='info-row' style='border-bottom:none;'>
        <span class='info-lbl'>Status</span>
        <span class='info-val green'>Successful</span>
      </div>
    </div>
    <div class='warn'>If you did not perform this action, please reset your password immediately and contact our support team.</div>
  </div>
  <div class='ftr'>&copy; " + DateTime.Now.Year + @" <a href='" + website + @"'>" + compName + @" Security Team</a></div>
</div></div>
</body></html>";
        DispatchMail(email, subject, body);
    }

    // =============================================
    // SHARED SMTP DISPATCHER
    // =============================================
    private void DispatchMail(string toAddress, string subject, string htmlBody)
    {
        var msg = new MailMessage(
            new MailAddress(Session["CompMail"].ToString()),
            new MailAddress(toAddress))
        {
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        var smtp = new SmtpClient(Session["MailHost"].ToString())
        {
            Port = 587,
            EnableSsl = true,
            UseDefaultCredentials = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new System.Net.NetworkCredential(
                Session["CompMail"].ToString(),
                Session["MailPass"].ToString())
        };
        smtp.Send(msg);
    }

    // =============================================
    // GENERATE 6-DIGIT OTP
    // =============================================
    private string GenerateOTP()
    {
        return new Random().Next(100000, 999999).ToString();
    }
}


