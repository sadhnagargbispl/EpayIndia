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

        try
        {
            DataSet data;

            string UrlR = "https://allupi.com/api/Status?RequestedId=" + orderid;

            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(UrlR);

            request.Headers.Add("X-Auth", auth);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = 0;

            string responseBody = "";

            using (HttpWebResponse response =
                (HttpWebResponse)request.GetResponse())
            {
                using (Stream receiveStream =
                    response.GetResponseStream())
                {
                    using (StreamReader readStream =
                        new StreamReader(receiveStream))
                    {
                        responseBody = readStream.ReadToEnd();
                    }
                }
            }

            data = convertJsonStringToDataSet(responseBody);

            string status = "FAILED";

            if (data.Tables.Count > 1 &&
                data.Tables[1].Rows.Count > 0 &&
                data.Tables[1].Columns.Contains("status"))
            {
                status =
                    Convert.ToString(
                        data.Tables[1].Rows[0]["status"]);
            }

            string str =
                "UPDATE LoginTransaction " +
                "SET Status='" + status + "'," +
                "response='" + responseBody.Replace("'", "''") + "'," +
                "Responsedate=GETDATE() " +
                "WHERE TransactionId='" + orderid + "'";

            SqlHelper.ExecuteNonQuery(
                constr,
                CommandType.Text,
                str);

            if (status.Equals("SUCCESS",
                StringComparison.OrdinalIgnoreCase))
            {
                int Nx = SqlHelper.ExecuteNonQuery(
                    constr,
                    CommandType.Text,
                    "EXEC sp_MonthlyActivation '" + orderid + "'");

                if (Nx > 0)
                {
                    Response.Clear();
                    Response.Redirect("~/monthly-activation-points.aspx",false);

                    Context.ApplicationInstance
                        .CompleteRequest();

                    return "";
                }
            }
            Response.Clear();

            Response.Redirect(
                "https://epayindia.in/Login.aspx",
                false);

            Context.ApplicationInstance
                .CompleteRequest();

            return "";
            //if (status.Equals("PENDING", StringComparison.OrdinalIgnoreCase))
            //{
            //    Response.Clear();

            //    Response.Redirect(
            //        "https://epayindia.in/Login.aspx",
            //        false);

            //    Context.ApplicationInstance
            //        .CompleteRequest();

            //    return "";
            //}

            //Response.Clear();

            //Response.Redirect( "~/PaymentgatewayMonthly.aspx?requestedId=" + orderid,false);

            //Context.ApplicationInstance
            //    .CompleteRequest();

            //return "";
        }
        catch (Exception ex)
        {
            File.AppendAllText(
                Server.MapPath("~/Logs/StatusError.txt"),
                DateTime.Now +
                Environment.NewLine +
                ex.ToString() +
                Environment.NewLine);
            Response.Clear();
            Response.Redirect("~/PaymentgatewayMonthly.aspx?requestedId=" + orderid,false);

            Context.ApplicationInstance
                .CompleteRequest();

            return "";
        }
    }
    //private string CheckLiveRateCoin(string auth, string orderid)
    //{
    //    string url = "";
    //    string sResult = string.Empty;

    //    string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //    int random_number = new Random().Next(0, 999);
    //    string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
    //    sResult = formatted_datetime;

    //    try
    //    {
    //        DataSet data;
    //        string UrlR = "https://allupi.com/api/Status?RequestedId=" + orderid;

    //        string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
    //                         "(ReqID, Formno, Request, postdata, Req_From, OrderID,PageName) VALUES " +
    //                         "('" + sResult + "', '0', '" + UrlR + "', '" + UrlR +
    //                         "', 'StatusCheckClaimMonth', '" + orderid + "','PaymentgatewayMonthly')";

    //        SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);

    //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlR);
    //        request.Headers.Add("X-Auth", auth);
    //        request.Method = "POST";
    //        request.ContentType = "application/json";
    //        request.ContentLength = 0;

    //        HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
    //        string responseBody = "";

    //        using (Stream receiveStream = response1.GetResponseStream())
    //        using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
    //        {
    //            responseBody = readStream.ReadToEnd();
    //        }

    //        string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" +
    //                         responseBody + "' WHERE ReqID = '" + sResult +
    //                         "' AND Req_From = 'StatusCheckClaimMonth'";

    //        SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);

    //        data = convertJsonStringToDataSet(responseBody);

    //        string status = "";

    //        if (string.IsNullOrEmpty(data.Tables[1].Rows[0]["status"].ToString()))
    //        {
    //            status = "FAILED";
    //        }
    //        else
    //        {
    //            status = data.Tables[1].Rows[0]["status"].ToString();
    //        }

    //        string str = "UPDATE LoginTransaction SET Status='" + status +
    //                     "', response='" + responseBody +
    //                     "', Responsedate=GETDATE() WHERE TransactionId='" + orderid + "'";

    //        SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str);

    //        if (status.ToUpper() == "SUCCESS")
    //        {
    //            str = "EXEC sp_MonthlyActivation '" + orderid + "'";
    //            int Nx = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, str);

    //            if (Nx > 0)
    //            {
    //                // SendWalletTransactionEmail(Session["email"].ToString(), Session["MemName"].ToString(), txtAmount.Text, "Debit", 0, orderid);
    //                Response.Redirect("monthly-activation-points.aspx", false);
    //            }
    //        }
    //        else if (status.ToUpper() == "PENDING")
    //        {
    //            Response.Redirect("https://epayindia.in/Login.aspx", false);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" +
    //                         ex.Message + "' WHERE ReqID = '" + sResult +
    //                         "' AND Req_From = 'StatusCheckClaimMonth'";

    //        SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);
    //    }

    //    return url;
    //}
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