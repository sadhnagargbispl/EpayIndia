using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Web.UI;

public partial class ForgotPassword : System.Web.UI.Page
{
    public SqlConnection objSqlConnection;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack) { }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtIDNo.Text))
            {
                ShowAlert("ID No. cannot be left blank."); return;
            }
            if (string.IsNullOrWhiteSpace(txtemail.Text))
            {
                ShowAlert("Please enter your Email ID."); return;
            }

            DAL objdal = new DAL();
            string IDNo = txtIDNo.Text
                .Replace("'", "").Replace(";", "").Replace("=", "").Replace("-", "");

            string sql = objdal.Isostart + " Exec Sp_MemberForgotPassw '" + IDNo + "'" + objdal.IsoEnd;
            DataTable Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];

            if (Dt.Rows.Count == 0)
            {
                ShowAlert("Invalid User ID. Please check and try again."); return;
            }

            string dbEmail = Dt.Rows[0]["Email"].ToString();
            string Username = Dt.Rows[0]["Idno"].ToString();
            string Password = Dt.Rows[0]["Passw"].ToString();
            string TranPassw = Dt.Rows[0]["EPassw"].ToString();
            string MemName = Dt.Rows[0]["MemName"].ToString();
            string CompName = Dt.Rows[0]["CompName"].ToString();
            string WebSite = Dt.Rows[0]["WebSite"].ToString();

            // Store mail credentials in session
            Session["CompMail"] = Dt.Rows[0]["CompMail"];
            Session["MailPass"] = Dt.Rows[0]["mailPass"];
            Session["MailHost"] = Dt.Rows[0]["mailHost"];
            Session["CompName"] = CompName;
            Session["CompWeb"] = WebSite;

            if (!txtemail.Text.Trim().Equals(dbEmail.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                ShowAlert("Email ID does not match our records."); return;
            }

            // ── Send password-recovery email ─────────────────
            bool sent = SendForgotPasswordEmail(Username, dbEmail, MemName, Password, TranPassw, CompName, WebSite);

            if (sent)
            {
                // ── Send password-reset confirmation email ────
                SendResetConfirmationEmail(dbEmail, MemName, CompName, WebSite);

                ScriptManager.RegisterStartupScript(this, GetType(), "Key",
                    "alert('Your password has been sent to your registered Email ID!');", true);
                txtIDNo.Text = "";
                txtemail.Text = "";
            }
            else
            {
                ShowAlert("Could not send email. Please try again later or contact support.");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    // ──────────────────────────────────────────────────────────────────
    // EMAIL 1 – Forgot Password OTP / Credentials Email
    // ──────────────────────────────────────────────────────────────────
    public bool SendForgotPasswordEmail(
        string IdNo, string Email, string MemberName,
        string Password, string EPassword, string CompName, string Website)
    {
        try
        {
            string subject = "Password Reset – " + CompName;

            string body = @"
<!DOCTYPE html>
<html lang='en'>
<head>
<meta charset='UTF-8'/>
<meta name='viewport' content='width=device-width,initial-scale=1.0'/>
<title>Forgot Password</title>
<style>
  body{margin:0;padding:0;background:#0f172a;font-family:'Segoe UI',Arial,sans-serif;}
  .wrap{max-width:560px;margin:40px auto;background:#1e293b;border-radius:16px;overflow:hidden;border:1px solid rgba(99,179,237,0.15);}
  .header{background:linear-gradient(135deg,#0ea5e9,#6366f1);padding:36px 32px;text-align:center;}
  .logo{font-size:26px;font-weight:800;color:#fff;letter-spacing:-0.5px;}
  .logo span{opacity:0.7;}
  .tagline{font-size:12px;color:rgba(255,255,255,0.7);margin-top:4px;letter-spacing:2px;text-transform:uppercase;}
  .body{padding:36px 32px;}
  .greeting{font-size:18px;font-weight:700;color:#e2e8f0;margin-bottom:12px;}
  .text{font-size:14px;color:#94a3b8;line-height:1.7;margin-bottom:24px;}
  .cred-card{background:#0f172a;border:1px solid rgba(99,179,237,0.2);border-radius:12px;padding:20px 24px;margin-bottom:24px;}
  .cred-row{display:flex;justify-content:space-between;align-items:center;padding:10px 0;border-bottom:1px solid rgba(99,179,237,0.08);}
  .cred-row:last-child{border-bottom:none;}
  .cred-label{font-size:11px;color:#64748b;text-transform:uppercase;letter-spacing:1px;font-weight:600;}
  .cred-value{font-size:14px;color:#38bdf8;font-family:'Courier New',monospace;font-weight:700;}
  .alert-box{background:rgba(251,191,36,0.08);border:1px solid rgba(251,191,36,0.25);border-radius:10px;padding:14px 18px;font-size:13px;color:#fbbf24;margin-bottom:24px;}
  .btn{display:inline-block;background:linear-gradient(135deg,#0ea5e9,#6366f1);color:#fff!important;text-decoration:none;padding:14px 32px;border-radius:10px;font-size:14px;font-weight:700;letter-spacing:0.3px;}
  .footer{background:#0f172a;padding:20px 32px;text-align:center;font-size:11px;color:#475569;border-top:1px solid rgba(99,179,237,0.1);}
  .footer a{color:#38bdf8;text-decoration:none;}
  .divider{height:1px;background:linear-gradient(90deg,transparent,rgba(99,179,237,0.15),transparent);margin:0 0 24px;}
</style>
</head>
<body>
<div class='wrap'>
  <div class='header'>
    <div class='logo'>⚡ " + CompName + @"</div>
    <div class='tagline'>Secure Digital Payments</div>
  </div>
  <div class='body'>
    <p class='greeting'>Dear " + MemberName + @",</p>
    <p class='text'>We received a request to retrieve your account credentials. Your login details are provided below. Please keep this information confidential and do not share it with anyone.</p>
    <div class='cred-card'>
      <div class='cred-row'>
        <span class='cred-label'>User ID</span>
        <span class='cred-value'>" + IdNo + @"</span>
      </div>
      <div class='cred-row'>
        <span class='cred-label'>Login Password</span>
        <span class='cred-value'>" + Password + @"</span>
      </div>
      <div class='cred-row'>
        <span class='cred-label'>Transaction Password</span>
        <span class='cred-value'>" + EPassword + @"</span>
      </div>
    </div>
    <div class='alert-box'>
      ⚠ If you did not request this password reset, please contact our support team immediately and change your password.
    </div>
    <div class='divider'></div>
    <div style='text-align:center;margin-bottom:24px;'>
      <a href='" + Website + @"' class='btn'>Login to Portal →</a>
    </div>
    <p style='font-size:12px;color:#475569;text-align:center;'>Trouble clicking? Copy this link:<br/><a href='" + Website + @"' style='color:#38bdf8;'>" + Website + @"</a></p>
  </div>
  <div class='footer'>
    © " + DateTime.Now.Year + " <a href='" + Website + @"'>" + CompName + @"</a> · All rights reserved.<br/>
    This is an automated message. Please do not reply directly to this email.
  </div>
</div>
</body>
</html>";

            return DispatchEmail(Email, subject, body);
        }
        catch { return false; }
    }

    // ──────────────────────────────────────────────────────────────────
    // EMAIL 2 – Password Reset Confirmation
    // ──────────────────────────────────────────────────────────────────
    public bool SendResetConfirmationEmail(string Email, string MemberName, string CompName, string Website)
    {
        try
        {
            string subject = "Your Password Has Been Successfully Updated – " + CompName;
            string resetTime = DateTime.Now.ToString("dd MMM yyyy, hh:mm tt");

            string body = @"
<!DOCTYPE html>
<html lang='en'>
<head>
<meta charset='UTF-8'/>
<meta name='viewport' content='width=device-width,initial-scale=1.0'/>
<title>Password Reset Confirmation</title>
<style>
  body{margin:0;padding:0;background:#0f172a;font-family:'Segoe UI',Arial,sans-serif;}
  .wrap{max-width:560px;margin:40px auto;background:#1e293b;border-radius:16px;overflow:hidden;border:1px solid rgba(52,211,153,0.15);}
  .header{background:linear-gradient(135deg,#059669,#0ea5e9);padding:36px 32px;text-align:center;}
  .logo{font-size:26px;font-weight:800;color:#fff;letter-spacing:-0.5px;}
  .tagline{font-size:12px;color:rgba(255,255,255,0.7);margin-top:4px;letter-spacing:2px;text-transform:uppercase;}
  .success-icon{width:64px;height:64px;background:rgba(52,211,153,0.15);border:2px solid rgba(52,211,153,0.4);border-radius:50%;margin:0 auto 16px;display:flex;align-items:center;justify-content:center;font-size:28px;line-height:64px;text-align:center;}
  .body{padding:36px 32px;}
  .greeting{font-size:18px;font-weight:700;color:#e2e8f0;margin-bottom:12px;}
  .text{font-size:14px;color:#94a3b8;line-height:1.7;margin-bottom:24px;}
  .info-card{background:#0f172a;border:1px solid rgba(52,211,153,0.15);border-radius:12px;padding:20px 24px;margin-bottom:24px;}
  .info-row{display:flex;justify-content:space-between;align-items:center;padding:10px 0;border-bottom:1px solid rgba(52,211,153,0.08);}
  .info-row:last-child{border-bottom:none;}
  .info-label{font-size:11px;color:#64748b;text-transform:uppercase;letter-spacing:1px;font-weight:600;}
  .info-value{font-size:13px;color:#34d399;font-family:'Courier New',monospace;font-weight:600;}
  .alert-box{background:rgba(248,113,113,0.08);border:1px solid rgba(248,113,113,0.25);border-radius:10px;padding:14px 18px;font-size:13px;color:#f87171;margin-bottom:24px;}
  .btn{display:inline-block;background:linear-gradient(135deg,#059669,#0ea5e9);color:#fff!important;text-decoration:none;padding:14px 32px;border-radius:10px;font-size:14px;font-weight:700;letter-spacing:0.3px;}
  .footer{background:#0f172a;padding:20px 32px;text-align:center;font-size:11px;color:#475569;border-top:1px solid rgba(52,211,153,0.1);}
  .footer a{color:#34d399;text-decoration:none;}
  .divider{height:1px;background:linear-gradient(90deg,transparent,rgba(52,211,153,0.15),transparent);margin:0 0 24px;}
  .badge{display:inline-block;background:rgba(52,211,153,0.1);color:#34d399;border:1px solid rgba(52,211,153,0.3);padding:4px 12px;border-radius:20px;font-size:11px;font-weight:700;letter-spacing:1px;text-transform:uppercase;margin-bottom:20px;}
</style>
</head>
<body>
<div class='wrap'>
  <div class='header'>
    <div style='text-align:center;'>
      <div style='font-size:40px;margin-bottom:8px;'>✓</div>
      <div class='logo'>⚡ " + CompName + @"</div>
      <div class='tagline'>Account Security</div>
    </div>
  </div>
  <div class='body'>
    <span class='badge'>✓ Password Updated</span>
    <p class='greeting'>Dear " + MemberName + @",</p>
    <p class='text'>Your account password has been <strong style='color:#34d399;'>successfully changed</strong>. This is a confirmation that the password reset process was completed on your account.</p>
    <div class='info-card'>
      <div class='info-row'>
        <span class='info-label'>Account</span>
        <span class='info-value'>" + MemberName + @"</span>
      </div>
      <div class='info-row'>
        <span class='info-label'>Action</span>
        <span class='info-value'>Password Reset</span>
      </div>
      <div class='info-row'>
        <span class='info-label'>Date &amp; Time</span>
        <span class='info-value'>" + resetTime + @"</span>
      </div>
      <div class='info-row'>
        <span class='info-label'>Status</span>
        <span class='info-value' style='color:#34d399;'>✓ Successful</span>
      </div>
    </div>
    <div class='alert-box'>
      🔒 If you did not perform this action, please <strong>reset your password immediately</strong> and contact our support team. Your account security is our highest priority.
    </div>
    <div class='divider'></div>
    <div style='text-align:center;margin-bottom:24px;'>
      <a href='" + Website + @"' class='btn'>Go to Portal →</a>
    </div>
  </div>
  <div class='footer'>
    © " + DateTime.Now.Year + " <strong><a href='" + Website + @"'>" + CompName + @"</a> Security Team</strong><br/>
    This is an automated security notification. Please do not reply to this email.
  </div>
</div>
</body>
</html>";

            return DispatchEmail(Email, subject, body);
        }
        catch { return false; }
    }

    // ──────────────────────────────────────────────────────────────────
    // Shared SMTP dispatcher
    // ──────────────────────────────────────────────────────────────────
    private bool DispatchEmail(string toAddress, string subject, string htmlBody)
    {
        try
        {
            var from = new MailAddress(Session["CompMail"].ToString());
            var to = new MailAddress(toAddress);
            var msg = new MailMessage(from, to)
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            var smtp = new SmtpClient(Session["MailHost"].ToString())
            {
                Port = 587,
                EnableSsl = false,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(
                    Session["CompMail"].ToString(),
                    Session["MailPass"].ToString()
                )
            };

            smtp.Send(msg);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void ShowAlert(string message)
    {
        ScriptManager.RegisterStartupScript(
            this, GetType(), "alertMessage",
            "alert('" + message.Replace("'", "\\'") + "')", true);
    }

    protected void Page_LoadComplete(object sender, EventArgs e) { }
    protected void Page_Unload(object sender, EventArgs e) { }
}
