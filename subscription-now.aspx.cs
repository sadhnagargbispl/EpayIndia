using ClosedXML.Excel;
using Irony.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class subscription_now : System.Web.UI.Page
{
    DAL Obj;
    DAL ObjDAL;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null &&
                Session["Status"].ToString() == "OK")
            {
                if (!IsPostBack)
                {
                   // this.CmdSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.CmdSave));
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                    // First Validate Member
                    Check_IdNo();

                    // Load Allowed Kits
                    fillkit(LblCondition.Text.Trim());

                    // Bind Packages
                    BindPackages();
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string DisableTheButton(Control pge, Control btn)
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
    public string GenerateRandomStringJoining(int length)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i < length; i++)
        {
            sResult += allowChrs[rdm.Next(allowChrs.Length)];
        }

        return sResult;
    }
    protected bool IsKitAllowed(int kitId)
    {
        try
        {
            DataTable dt = Session["KitTable"] as DataTable;

            if (dt == null || dt.Rows.Count == 0)
                return false;

            return dt.AsEnumerable()
                .Any(r => Convert.ToInt32(r["KitId"]) == kitId);
        }
        catch
        {
            return false;
        }
    }
    protected void BindPackages()
    {
        string query = @"SELECT KitId,
                            KitName,
                            JoinAmount
                     FROM m_kitmaster
                     WHERE KitId IN (4,6,7,11)";

        DataTable dt = SqlHelper.ExecuteDataset(constr.ToString(), CommandType.Text, query).Tables[0];

        rptPackages.DataSource = dt;
        rptPackages.DataBind();
    }
    private bool Check_IdNo()
    {
        try
        {
            DataTable dtKit = Session["MKit"] as DataTable;

            string Sql =
                "SELECT a.Formno,a.Idno," +
                "a.MemFirstName + ' ' + a.MemLastName AS MemName," +
                "a.IsTopup,a.KitId,a.LegNo,b.MACAdrs,b.TopUpSeq," +
                "b.KitName,a.BV,b.BV AS KBv," +
                "a.ActiveStatus,a.Planid,a.IsBlock " +
                "FROM M_MemberMaster a " +
                "INNER JOIN M_KitMaster b ON a.KitId=b.KitId " +
                "WHERE b.RowStatus='Y' " +
                "AND a.IsBlock='N' " +
                "AND a.IDNo='" + Session["idno"].ToString() + "'";

            DataTable Dt_ = SqlHelper.ExecuteDataset(
                constr,
                CommandType.Text,
                Sql).Tables[0];

            if (Dt_.Rows.Count == 0)
                return false;

            DataRow mRow = Dt_.Rows[0];

            if (mRow["ActiveStatus"].ToString() == "Y")
            {
                Sql =
                    "SELECT a.KitId,b.ForType,b.TopupSeq " +
                    "FROM RepurchIncome a " +
                    "INNER JOIN M_KitMaster b ON a.KitId=b.KitId " +
                    "WHERE a.FormNo=" + mRow["Formno"] +
                    " AND b.ForType = 'S' AND a.KitId<>0 " +
                    "ORDER BY b.TopupSeq DESC;";

                DataTable Dt__ = SqlHelper.ExecuteDataset(
                    constr,
                    CommandType.Text,
                    Sql).Tables[0];

                if (Dt__ != null && Dt__.Rows.Count > 0)
                {
                    int maxTopupSeq = Dt__.AsEnumerable()
                        .Max(r => Convert.ToInt32(r["TopupSeq"]));

                    LblCondition.Text =
                        " AND TopupSeq > " + maxTopupSeq;

                    return true;
                }

                LblCondition.Text = "";
                return true;
            }
            else
            {
                Sql =
                    "SELECT TopupSeq FROM M_KitMaster " +
                    "WHERE KitId=" + mRow["KitId"];

                object seq = SqlHelper.ExecuteScalar(
                    constr,
                    CommandType.Text,
                    Sql);

                LblCondition.Text =
                    " AND TopupSeq > " +
                    Convert.ToInt32(seq);

                return true;
            }
        }
        catch
        {
            return false;
        }
    }
    protected void fillkit(string condition = "")
    {
        try
        {
            string query =
                "select * from m_kitmaster " +
                "where activestatus='Y' " +
                "and ForType = 'S' " +
                "AND kitid <> 1 " +
                condition +
                " Order By KitId ";

            DataTable Dt = SqlHelper.ExecuteDataset(
                constr,
                CommandType.Text,
                query).Tables[0];

            Session["KitTable"] = Dt;
            Session["MKit"] = Dt;
        }
        catch
        {
        }
    }
   
    protected void rptPackages_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "PayNow")
        {
            RepeaterItem item = e.Item;

            HiddenField hdnKitId =
                (HiddenField)item.FindControl("hdnKitId");

            HiddenField hdnAmount =
                (HiddenField)item.FindControl("hdnAmount");

            int selectedKitId =
                Convert.ToInt32(hdnKitId.Value);

            // Final Security Check
            if (!IsKitAllowed(selectedKitId))
                return;

            string data = "KitId=" + hdnKitId.Value +
                          "&Amount=" + hdnAmount.Value;

            string encodedData =
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(data));
            string Strqueryquer = "Insert into Trnjoining(Transid)values(" + HdnCheckTrnns.Value + ")";
            int isOk1 = 0;
            try
            {
                isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Strqueryquer));
            }
            catch (Exception ex)
            {
                
            }
            if (isOk1 > 0)
            {
                string OrderId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string sql = "INSERT INTO OnlineTransaction(Orderid, Orderdate, Amount, name,kitid,FormNo,idno) " +
                             "VALUES('" + OrderId + "', GETDATE(), '" + hdnAmount.Value + "','" + Session["MemName"] + "','" + hdnKitId.Value + "','" + Session["formno"] + "','" + Session["idno"] + "')";

                int i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql);
                if (i > 0)
                {
                    GenerateQrCode(OrderId, hdnAmount.Value);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Try Again After Some Time.!');location.replace('subscription-now.aspx');", true);
                return;
            }
            //Response.Redirect(
            //    "PaymentGateway.aspx?data=" +
            //    Server.UrlEncode(encodedData));
        }
    }
    public string GenerateQrCode(string Orderid, string Amount)
    {
        string str = "";
        string sResult = DateTime.Now.ToString("yyyyMMddHHmmssfff") +
                         new Random().Next(100, 999);
        string auth = "";
        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string URL = "https://allupi.com/api/login";

            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(URL);

            request.Method = "POST";
            request.ContentType = "application/json";

            string postData = "{\"merchantID\":\"29159c34-8f20-49d8-a867-4618325f2f74\",\"securityCode\":\"a0a1649a-a91c-4861-baed-38422f686d6f\"}";
            string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
                             "(ReqID, Formno, Request, postdata, Req_From, OrderID,PageName) VALUES " +
                             "('" + sResult + "', '0', '" + URL + "', '" + postData +
                             "', 'LoginClaim', '" + Orderid + "','subscription_now')";

            int x_Req = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);
            byte[] dataBytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = dataBytes.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response =
                (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader =
                    new StreamReader(response.GetResponseStream()))
                {
                    str = reader.ReadToEnd();
                }
            }
            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + str + "' WHERE ReqID = '" + sResult + "' AND Req_From = 'LoginClaim'";

            int x_res = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);
            DataSet ds = convertJsonStringToDataSet(str);

            if (ds.Tables.Count < 2 ||
                ds.Tables[1].Rows.Count == 0)
            {
                throw new Exception("Login API Response Invalid");
            }

             auth = Convert.ToString(ds.Tables[1].Rows[0]["token"]);

            string sql =
                @"INSERT INTO LoginTransaction
            (TId,Username,role,token,refreshToken,name,transactionid,amount,FormNo)
            VALUES
            (
            '" + ds.Tables[1].Rows[0]["ID"] + @"',
            '" + ds.Tables[1].Rows[0]["username"] + @"',
            '" + ds.Tables[1].Rows[0]["role"] + @"',
            '" + ds.Tables[1].Rows[0]["token"] + @"',
            '" + ds.Tables[1].Rows[0]["refreshToken"] + @"',
            '" + ds.Tables[1].Rows[0]["name"] + @"',
            '" + Orderid + @"',
            '" + Amount + @"','" + Session["formno"] + @"'
            )";

            SqlHelper.ExecuteNonQuery(
                constr,
                CommandType.Text,
                sql);

            return CheckLiveRateCoin(auth, Orderid, Amount);
        }
        catch (Exception ex)
        {
            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" + ex.Message + "' WHERE ReqID = '" + sResult + "' AND Req_From = 'LoginClaim'";
            int x_res = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);
            File.AppendAllText(
                Server.MapPath("~/Logs/SubscriptionPaymentError.txt"),
                DateTime.Now + " GenerateQrCode : " +
                ex + Environment.NewLine);
           // return CheckLiveRateCoin(auth, Orderid, Amount);
            throw;
        }
    }
    protected string CheckLiveRateCoin(string auth, string orderid, string amount)
    {
        string redirectUrl = "";
        string sResult = DateTime.Now.ToString("yyyyMMddHHmmssfff") +
                        new Random().Next(100, 999);

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string apiUrl =
                "https://allupi.com/api/InitiateTransactionAsync";

            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(apiUrl);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("X-Auth", auth);

            string postData =
                "{"
                + "\"requestedId\":\"" + orderid + "\","
                + "\"amount\":" + amount + ","
                + "\"upiId\":\"82215511\","
                + "\"serverHookURL\":\"https://epayindia.in/Login.aspx\","
                + "\"webHookURL\":\"https://epayindia.in/paymentsubscriptionpanel.aspx\""
                + "}";
            string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
                             "(ReqID, Formno, Request, postdata, Req_From, OrderID,PageName) VALUES " +
                             "('" + sResult + "', '" + Session["formno"] + "', '" + apiUrl + "', '" +
                             postData + "', 'InitiateTransactionAsyncClaim', '" + orderid + "','subscription_now')";

            int x_Req = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);
            byte[] bytes = Encoding.UTF8.GetBytes(postData);

            request.ContentLength = bytes.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            string responseText = "";

            using (HttpWebResponse response =
                (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader =
                    new StreamReader(response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
            }

            File.AppendAllText(
                Server.MapPath("~/Logs/SubscriptionAllUPIResponse.txt"),
                DateTime.Now +
                " OrderId=" + orderid +
                " Response=" + responseText +
                Environment.NewLine);
            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + responseText + "' WHERE ReqID = '" + sResult + "' AND Req_From = 'InitiateTransactionAsyncClaim'";

            int x_res = SqlHelper.ExecuteNonQuery(
                constr,
                CommandType.Text,
                sql_res
            );
            DataSet ds =
                convertJsonStringToDataSet(responseText);

            if (ds.Tables.Count > 0 &&
                ds.Tables[0].Rows.Count > 0 &&
                ds.Tables[0].Columns.Contains("url"))
            {
                redirectUrl =
                    Convert.ToString(ds.Tables[0].Rows[0]["url"]);
            }

            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                Response.Redirect(redirectUrl);
            }
           // Response.Redirect(redirectUrl);
            throw new Exception(
                "Payment URL not found in API response.");

        }
        catch (Exception ex)
        {
            
            File.AppendAllText(
                Server.MapPath("~/Logs/SubscriptionRedirectError.txt"),
                DateTime.Now + " " +
                ex + Environment.NewLine);
            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" +
                             ex.Message + "' WHERE ReqID = '" + sResult +
                             "' AND Req_From = 'InitiateTransactionAsyncClaim'";

            SqlHelper.ExecuteNonQuery(
                constr,
                CommandType.Text,
                sql_res
            );
            //Response.Redirect(redirectUrl);
            return "";
        }

    }
    //public string GenerateQrCode(string Orderid, string Amount)
    //{
    //    string str = string.Empty;
    //    decimal value = 0;
    //    string Code = "";
    //    DataSet ds = new DataSet();
    //    DataSet data;
    //    string sResult = string.Empty;

    //    string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //    int random_number = new Random().Next(0, 999);
    //    string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
    //    sResult = formatted_datetime;

    //    try
    //    {
    //        ServicePointManager.Expect100Continue = true;
    //        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

    //        string URL = "https://allupi.com/api/login";

    //        WebRequest tRequest = WebRequest.Create(URL);
    //        tRequest.Method = "POST";
    //        tRequest.ContentType = "application/json";
    //        string postData = "{\"merchantID\":\"29159c34-8f20-49d8-a867-4618325f2f74\",\"securityCode\":\"a0a1649a-a91c-4861-baed-38422f686d6f\"}";
    //        string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
    //                         "(ReqID, Formno, Request, postdata, Req_From, OrderID,PageName) VALUES " +
    //                         "('" + sResult + "', '0', '" + URL + "', '" + postData +
    //                         "', 'LoginClaim', '" + Orderid + "','subscription_now')";

    //        int x_Req = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);

    //        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
    //        tRequest.ContentLength = byteArray.Length;

    //        using (Stream dataStream = tRequest.GetRequestStream())
    //        {
    //            dataStream.Write(byteArray, 0, byteArray.Length);
    //        }

    //        WebResponse tResponse = tRequest.GetResponse();
    //        using (Stream responseStream = tResponse.GetResponseStream())
    //        using (StreamReader tReader = new StreamReader(responseStream))
    //        {
    //            str = tReader.ReadToEnd();
    //        }

    //        string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + str + "' WHERE ReqID = '" + sResult + "' AND Req_From = 'LoginClaim'";

    //        int x_res = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);

    //        data = convertJsonStringToDataSet(str);

    //        string auth = data.Tables[1].Rows[0]["token"].ToString();

    //        string sql = "INSERT INTO LoginTransaction " +
    //                     "(TId, Username, role, token, refreshToken, name, transactionid, amount,FormNo) VALUES (" +
    //                     "'" + data.Tables[1].Rows[0]["ID"] + "'," +
    //                     "'" + data.Tables[1].Rows[0]["username"] + "'," +
    //                     "'" + data.Tables[1].Rows[0]["role"] + "'," +
    //                     "'" + data.Tables[1].Rows[0]["token"] + "'," +
    //                     "'" + data.Tables[1].Rows[0]["refreshToken"] + "'," +
    //                     "'" + data.Tables[1].Rows[0]["name"] + "'," +
    //                     "'" + Orderid + "'," +
    //                     "'" + Amount + "','" + Session["formno"] + "')";

    //        int x = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql);

    //        Response.Write(data.Tables[1]);

    //        CheckLiveRateCoin(auth, Orderid, Amount);
    //    }
    //    catch (Exception ex)
    //    {
    //        string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" + ex.Message +
    //                         "' WHERE ReqID = '" + sResult +
    //                         "' AND Req_From = 'LoginClaim'";

    //        int x_res = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);
    //    }

    //    return str;
    //}
    //protected string CheckLiveRateCoin(string auth, string orderid, string amount)
    //{
    //    try
    //    {
    //        string str = string.Empty;
    //        decimal value = 0;
    //        string code = "";
    //        DataSet ds = new DataSet();
    //        DataSet data;
    //        DataSet dsLogin = new DataSet();

    //        string completeUrl = "https://allupi.com/api/InitiateTransactionAsync";
    //        string responseString = string.Empty;
    //        string CustomerOTPmessage = "";
    //        string url = "";

    //        string sResult = string.Empty;
    //        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //        int random_number = new Random().Next(0, 999);
    //        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
    //        sResult = formatted_datetime;

    //        try
    //        {
    //            ServicePointManager.Expect100Continue = true;
    //            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

    //            WebRequest tRequest = (HttpWebRequest)WebRequest.Create(completeUrl);
    //            tRequest.Method = "POST";
    //            tRequest.ContentType = "application/json";
    //            tRequest.Headers.Add("X-Auth", auth);

    //            string postdata = "{\"requestedId\":\"" + orderid + "\",";
    //            postdata += "\"amount\":" + amount + ",\"upiId\":\"82215511\",";
    //            postdata += "\"serverHookURL\":\"https://epayindia.in/Login.aspx\",";
    //            postdata += "\"webHookURL\":\"https://epayindia.in/paymentsubscriptionpanel.aspx\"}";
    //            //postdata += "\"webHookURL\":\"https://epayindia.in/paymentsubscriptionpanel.aspx\"}";

    //            string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
    //                             "(ReqID, Formno, Request, postdata, Req_From, OrderID,PageName) VALUES " +
    //                             "('" + sResult + "', '" + Session["formno"] + "', '" + url + "', '" +
    //                             postdata + "', 'InitiateTransactionAsyncClaim', '" + orderid + "','subscription_now')";

    //            int x_Req = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);

    //            byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
    //            tRequest.ContentLength = byteArray.Length;

    //            using (Stream dataStream = tRequest.GetRequestStream())
    //            {
    //                dataStream.Write(byteArray, 0, byteArray.Length);
    //            }

    //            WebResponse tResponse = tRequest.GetResponse();
    //            using (Stream responseStream = tResponse.GetResponseStream())
    //            using (StreamReader tReader = new StreamReader(responseStream))
    //            {
    //                str = tReader.ReadToEnd();
    //            }

    //            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + str + "' WHERE ReqID = '" + sResult + "' AND Req_From = 'InitiateTransactionAsyncClaim'";

    //            int x_res = SqlHelper.ExecuteNonQuery(
    //                constr,
    //                CommandType.Text,
    //                sql_res
    //            );

    //            data = convertJsonStringToDataSet(str);

    //            Response.Write(str);

    //            if (data.Tables[0].Rows[0]["url"].ToString() != "")
    //            {
    //                Response.Redirect(data.Tables[0].Rows[0]["url"].ToString());
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" +
    //                             ex.Message + "' WHERE ReqID = '" + sResult +
    //                             "' AND Req_From = 'InitiateTransactionAsyncClaim'";

    //            SqlHelper.ExecuteNonQuery(
    //                constr,
    //                CommandType.Text,
    //                sql_res
    //            );
    //        }

    //        return url;
    //    }
    //    catch
    //    {
    //        return string.Empty;
    //    }
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