using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class PaymentgatewayMonthly : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string folderPath = Server.MapPath("~/Logs");

            // Create Logs folder if not exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Current page name
            string pageName = Path.GetFileNameWithoutExtension(Request.PhysicalPath);

            // Date wise log file
            string fileName = pageName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            string logPath = Path.Combine(folderPath, fileName);

            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine("=================================================");
                sw.WriteLine("DATE TIME : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));

                sw.WriteLine("PAGE NAME : " + pageName);

                sw.WriteLine("FULL URL : " + Request.Url.ToString());

                sw.WriteLine("HTTP METHOD : " + Request.HttpMethod);

                sw.WriteLine("USER IP : " + Request.UserHostAddress);

                sw.WriteLine("USER AGENT : " + Request.UserAgent);

                sw.WriteLine("RAW requestedId : " + Convert.ToString(Request["requestedId"]));

                sw.WriteLine("---------------- QUERY STRING ----------------");

                foreach (string key in Request.QueryString.AllKeys)
                {
                    try
                    {
                        sw.WriteLine(key + " = " + Request.QueryString[key]);
                    }
                    catch
                    {
                    }
                }

                sw.WriteLine("---------------- FORM DATA ----------------");

                foreach (string key in Request.Form.AllKeys)
                {
                    try
                    {
                        sw.WriteLine(key + " = " + Request.Form[key]);
                    }
                    catch
                    {
                    }
                }

                sw.WriteLine("---------------- RAW BODY ----------------");

                try
                {
                    if (Request.InputStream != null)
                    {
                        Request.InputStream.Position = 0;

                        using (StreamReader reader = new StreamReader(Request.InputStream))
                        {
                            string body = reader.ReadToEnd();

                            if (!string.IsNullOrWhiteSpace(body))
                            {
                                sw.WriteLine(body);
                            }
                        }
                    }
                }
                catch (Exception exBody)
                {
                    sw.WriteLine("RAW BODY ERROR : " + exBody.Message);
                }

                sw.WriteLine("=================================================");
                sw.WriteLine();
            }

            if (!IsPostBack)
            {
                string requestedId = "";

                // QueryString
                if (!string.IsNullOrWhiteSpace(Convert.ToString(Request.QueryString["requestedId"])))
                {
                    requestedId = Convert.ToString(Request.QueryString["requestedId"]);
                }

                // Form Data
                if (string.IsNullOrWhiteSpace(requestedId))
                {
                    requestedId = Convert.ToString(Request.Form["requestedId"]);
                }

                // Fallback
                if (string.IsNullOrWhiteSpace(requestedId))
                {
                    requestedId = Convert.ToString(Request["requestedId"]);
                }

                // Clean malformed callback value
                if (!string.IsNullOrWhiteSpace(requestedId))
                {
                    // remove extra query part
                    if (requestedId.Contains("?"))
                    {
                        requestedId = requestedId.Split('?')[0];
                    }

                    // remove duplicate merged value
                    if (requestedId.Contains(","))
                    {
                        requestedId = requestedId.Split(',')[0];
                    }

                    requestedId = requestedId.Trim();
                }

                // Final log after clean
                using (StreamWriter sw = new StreamWriter(logPath, true))
                {
                    sw.WriteLine("FINAL requestedId : " + requestedId);
                    sw.WriteLine();
                }

                // Validation
                if (!string.IsNullOrWhiteSpace(requestedId))
                {
                    GenerateQrCode(requestedId);
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(logPath, true))
                    {
                        sw.WriteLine("GenerateQrCode NOT called because requestedId missing");
                        sw.WriteLine();
                    }

                    Response.Write("requestedId missing");
                }
            }
        }
        catch (Exception ex)
        {
            try
            {
                string folderPath = Server.MapPath("~/Logs");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string pageName = Path.GetFileNameWithoutExtension(Request.PhysicalPath);

                string errorFileName =
                    pageName + "_Error_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                string errorPath = Path.Combine(folderPath, errorFileName);

                using (StreamWriter sw = new StreamWriter(errorPath, true))
                {
                    sw.WriteLine("=================================================");
                    sw.WriteLine("DATE TIME : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));

                    sw.WriteLine("PAGE NAME : " + pageName);

                    sw.WriteLine("FULL URL : " + Request.Url.ToString());

                    sw.WriteLine("ERROR MESSAGE : " + ex.Message);

                    sw.WriteLine("STACK TRACE : ");
                    sw.WriteLine(ex.StackTrace);

                    sw.WriteLine("=================================================");
                    sw.WriteLine();
                }
            }
            catch
            {
            }
        }
    }
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Request["requestedId"] != null)
    //        {
    //            string sRequestData = HttpContext.Current.Request.Url.ToString();
    //            GenerateQrCode(Request["requestedId"]);
    //        }
    //        else
    //        {
    //            Response.Redirect("Default.aspx", false);
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        Response.Write("{\"response\":\"FAILED\"}");
    //    }

    //}
    public string GenerateQrCode(string Orderid)
    {
        string str = string.Empty;
        decimal value = 0;
        string Code = "";
        DataSet ds = new DataSet();
        DataSet data;

        string sResult = string.Empty;
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            string URL = "https://allupi.com/api/login";
            WebRequest tRequest = WebRequest.Create(URL);
            tRequest.Method = "POST";
            tRequest.ContentType = "application/json";
            tRequest.ContentLength = 0;
            string postData = "{\"merchantID\":\"29159c34-8f20-49d8-a867-4618325f2f74\",\"securityCode\":\"a0a1649a-a91c-4861-baed-38422f686d6f\"}";
            string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
                             "(ReqID, Formno, Request, postdata, Req_From, OrderID,PageName) VALUES " +
                             "('" + sResult + "', '0', '" + URL + "', '" + postData +
                             "', 'LoginClaimLoginMonth', '" + Orderid + "','PaymentgatewayMonthly')";

            int x_Req = SqlHelper.ExecuteNonQuery(
                constr,
                CommandType.Text,
                sql_req
            );

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            WebResponse tResponse = tRequest.GetResponse();
            using (Stream responseStream = tResponse.GetResponseStream())
            using (StreamReader tReader = new StreamReader(responseStream))
            {
                str = tReader.ReadToEnd();
            }

            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + str +
                             "' WHERE ReqID = '" + sResult +
                             "' AND Req_From = 'LoginClaimLoginMonth'";

            int x_res = SqlHelper.ExecuteNonQuery(
                constr,
                CommandType.Text,
                sql_res
            );

            data = convertJsonStringToDataSet(str);

            string auth = data.Tables[1].Rows[0]["token"].ToString();

            CheckLiveRateCoin(auth, Orderid);
        }
        catch (Exception ex)
        {
            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" +
                             ex.Message + "' WHERE ReqID = '" + sResult +
                             "' AND Req_From = 'LoginClaimLoginMonth'";

            SqlHelper.ExecuteNonQuery(
                constr,
                CommandType.Text,
                sql_res
            );
        }

        return str;
    }
    private string CheckLiveRateCoin(string auth, string orderid)
    {
        string url = "";
        string sResult = string.Empty;

        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;

        try
        {
            DataSet data;
            string UrlR = "https://allupi.com/api/Status?RequestedId=" + orderid;

            string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
                             "(ReqID, Formno, Request, postdata, Req_From, OrderID,PageName) VALUES " +
                             "('" + sResult + "', '0', '" + UrlR + "', '" + UrlR +
                             "', 'StatusCheckClaimMonth', '" + orderid + "','PaymentgatewayMonthly')";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlR);
            request.Headers.Add("X-Auth", auth);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = 0;

            HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
            string responseBody = "";

            using (Stream receiveStream = response1.GetResponseStream())
            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
            {
                responseBody = readStream.ReadToEnd();
            }

            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" +
                             responseBody + "' WHERE ReqID = '" + sResult +
                             "' AND Req_From = 'StatusCheckClaimMonth'";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);

            data = convertJsonStringToDataSet(responseBody);

            string status = "";

            if (string.IsNullOrEmpty(data.Tables[1].Rows[0]["status"].ToString()))
            {
                status = "FAILED";
            }
            else
            {
                status = data.Tables[1].Rows[0]["status"].ToString();
            }

            string str = "UPDATE LoginTransaction SET Status='" + status +
                         "', response='" + responseBody +
                         "', Responsedate=GETDATE() WHERE TransactionId='" + orderid + "'";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str);

            if (status.ToUpper() == "SUCCESS")
            {
                str = "EXEC sp_MonthlyActivation '" + orderid + "'";
                int Nx = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str);

                if (Nx > 0)
                {
                    // SendWalletTransactionEmail(Session["email"].ToString(), Session["MemName"].ToString(), txtAmount.Text, "Debit", 0, orderid);
                    Response.Redirect("monthly-activation-points.aspx", false);
                }
            }
            else if (status.ToUpper() == "PENDING")
            {
                Response.Redirect("https://epayindia.in/Login.aspx", false);
            }
        }
        catch (Exception ex)
        {
            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" +
                             ex.Message + "' WHERE ReqID = '" + sResult +
                             "' AND Req_From = 'StatusCheckClaimMonth'";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);
        }

        return url;
    }
    private void SendWalletTransactionEmail(string email, string name, string amount, string txnType, string balance, string txnId)
    {
        string compName = Session["CompName"] != null ? Session["CompName"].ToString() : "ePay Digital";
        string website = Session["CompWeb"] != null ? Session["CompWeb"].ToString() : "#";
        string subject = "Wallet Transaction Alert - " + compName;
        string txnTime = DateTime.Now.ToString("dd MMM yyyy, hh:mm tt");

        // Credit ho to green, Debit ho to red
        string typeColor = txnType.ToLower() == "credit" ? "#16a34a" : "#dc2626";

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
  .warn{background:#fef2f2;border:1px solid #fecaca;border-radius:8px;padding:12px 16px;font-size:13px;color:#b91c1c}
  .ftr{background:#f8fafc;border-top:1px solid #e2e8f0;padding:18px 32px;font-size:12px;color:#94a3b8;text-align:center}
  .ftr a{color:#16a34a;text-decoration:none}
</style></head><body>
<div class='outer'><div class='card'>
  <div class='hdr'><h2>Wallet Transaction Alert</h2><p>" + compName + @"</p></div>
  <div class='bdy'>
    <span class='badge'>Transaction</span>
    <p class='greeting'>Dear " + name + @",</p>
    <p class='txt'>A transaction has been <strong>recorded in your wallet</strong>. Here are the details:</p>
    <div class='info-card'>
      <div class='info-row'>
        <span class='info-lbl'>Account</span>
        <span class='info-val'>" + name + @"</span>
      </div>
      <div class='info-row'>
        <span class='info-lbl'>Amount</span>
        <span class='info-val'>&#8377;" + amount + @"</span>
      </div>
      <div class='info-row'>
        <span class='info-lbl'>Transaction Type</span>
        <span class='info-val' style='color:" + typeColor + @";'>" + txnType + @"</span>
      </div>
      <div class='info-row'>
        <span class='info-lbl'>Available Balance</span>
        <span class='info-val'>&#8377;" + balance + @"</span>
      </div>
      <div class='info-row'>
        <span class='info-lbl'>Transaction ID</span>
        <span class='info-val'>" + txnId + @"</span>
      </div>
      <div class='info-row'>
        <span class='info-lbl'>Date &amp; Time</span>
        <span class='info-val'>" + txnTime + @"</span>
      </div>
      <div class='info-row' style='border-bottom:none;'>
        <span class='info-lbl'>Status</span>
        <span class='info-val' style='color:#16a34a;'>Successful</span>
      </div>
    </div>
    <div class='warn'>If you did not perform this transaction, please contact our support team immediately.</div>
  </div>
  <div class='ftr'>&copy; " + DateTime.Now.Year + @" <a href='" + website + @"'>" + compName + @" Security Team</a>
  &nbsp;|&nbsp; You can view full details in your dashboard.</div>
</div></div>
</body></html>";

        DispatchMail(email, subject, body);
    }
    private void DispatchMail(string toAddress, string subject, string htmlBody)
    {
        try
        {
            // SMTP credentials — Session se (login flow)
            string compMail = Session["CompMail"] != null ? Session["CompMail"].ToString() : "";
            string mailPass = Session["MailPass"] != null ? Session["MailPass"].ToString() : "";
            string mailHost = Session["MailHost"] != null ? Session["MailHost"].ToString() : "";

            // Fallback — Web.config se (agar Session expire ho)
            if (string.IsNullOrEmpty(compMail)) compMail = ConfigurationManager.AppSettings["CompMail"];
            if (string.IsNullOrEmpty(mailPass)) mailPass = ConfigurationManager.AppSettings["MailPass"];
            if (string.IsNullOrEmpty(mailHost)) mailHost = ConfigurationManager.AppSettings["MailHost"];

            if (string.IsNullOrEmpty(compMail) || string.IsNullOrEmpty(mailHost))
                throw new Exception("SMTP credentials not configured.");

            var msg = new MailMessage(
                new MailAddress(compMail),
                new MailAddress(toAddress))
            {
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            // UTF-8 encoding — Hindi/rupee symbol ke liye
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;

            var smtp = new SmtpClient(mailHost)
            {
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(compMail, mailPass),
                Timeout = 30000  // 30 seconds
            };

            smtp.Send(msg);
            msg.Dispose();
        }
        catch (SmtpException ex)
        {
            // SMTP error — log karo, page crash mat karo
            System.Diagnostics.Debug.WriteLine("SMTP Error: " + ex.Message);
            throw; // caller ko bata do
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("DispatchMail Error: " + ex.Message);
            throw;
        }
    }
    public DataSet convertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();

        jsonString = "{ \"rootNode\": {" +
                     jsonString.Trim().TrimStart('{').TrimEnd('}') +
                     "} }";

        xd = JsonConvert.DeserializeXmlNode(jsonString);

        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));

        return ds;
    }
}