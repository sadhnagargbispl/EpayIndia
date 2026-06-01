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

public partial class Appmonthly_activation_points : System.Web.UI.Page
{
    DAL Obj;
    DAL ObjDAL;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {

                if (!Page.IsPostBack)
                {
                    BindPackages();
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                    // BindPackages();
                }

            }

            else
            {
                Response.Redirect("AppLogin.aspx", false);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected string GetKitIcon(object kitIdObj)
    {
        int kitId = 0;

        if (kitIdObj != null && int.TryParse(kitIdObj.ToString(), out kitId))
        {
            switch (kitId)
            {
                case 1: return "⚡";
                case 2: return "🚀";
                case 3: return "💎";
                case 4: return "👑";
                default: return "🎁"; // fallback icon
            }
        }

        return "❓"; // null/invalid fallback
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
    protected void BindPackages()
    {
        string query = @"Exec Sp_GetMonthlyDetails ";

        DataTable dt = SqlHelper.ExecuteDataset(constr.ToString(), CommandType.Text, query).Tables[0];

        rptPackages.DataSource = dt;
        rptPackages.DataBind();
    }
    public bool CheckMonthlyActivation(string formNo, out string message)
    {
        bool isAllowed = false;
        message = "";

        using (SqlConnection con = new SqlConnection(constr1))
        using (SqlCommand cmd = new SqlCommand("sp_SetMonthlyActivation", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FormNo", formNo.Trim());

            con.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    isAllowed = Convert.ToBoolean(dr["IsAllowed"]);
                    message = dr["Msg"].ToString();
                }
            }
        }

        return isAllowed;
    }
    public string GetName()
    {
        try
        {
            string str = " Exec Sp_GetMemberName '" + Session["idno"] + "'";
            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];

            if (dt.Rows[0]["Isblock"].ToString() == "Y")
            {
                string scrName = "<script language='javascript'>alert('This ID is blocked. Please contact the Admin.');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
                return "";
            }
            else if (dt.Rows[0]["ActiveStatus"].ToString() == "N")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('This Id Not Active Please Active First.!');location.replace('WebApp.aspx');", true);
                return "";
            }
            else if (dt.Rows[0]["ActiveStatus"].ToString() == "Y")
            {
                return "OK";

            }
            else
            {
                return "OK";
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
        return null;
    }
    protected void rptPackages_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Activate")
        {
            try
            {
                // Check Member
                string result = GetName();

                if (result != "OK")
                {
                    return;
                }

                // Monthly Activation Validation
                string msg = "";

                bool allowed = CheckMonthlyActivation(
                    Session["FormNo"].ToString(),
                    out msg);

                if (!allowed)
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "Key",
                        "alert('" + msg + "');",
                        true);
                    return;
                }

                // Get Repeater Values
                HiddenField hdnKitId =
                    (HiddenField)e.Item.FindControl("hdnKitId");

                HiddenField hdnAmount =
                    (HiddenField)e.Item.FindControl("hdnAmount");

                string kitId = hdnKitId.Value;
                string amount = hdnAmount.Value;

                // Encode Data
                string data =
                    "KitId=" + kitId +
                    "&Amount=" + amount;

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
                    string sql = "INSERT INTO OnlineTransaction(Orderid, Orderdate, Amount, name,kitid,FormNo,idno,ForType) " +
                                 "VALUES('" + OrderId + "', GETDATE(), '" + hdnAmount.Value + "','" + Session["MemName"] + "','" + hdnKitId.Value + "','" + Session["formno"] + "','" + Session["idno"] + "','A')";

                    int i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql);
                    if (i > 0)
                    {
                        GenerateQrCode(OrderId, hdnAmount.Value);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Try Again After Some Time.!');location.replace('Appmonthly-activation-points.aspx');", true);
                    return;
                }
                //Response.Redirect(
                //    "PaymentGateway.aspx?data=" +
                //    Server.UrlEncode(encodedData));
            }
            catch (Exception ex)
            {
                string path =
                    HttpContext.Current.Request.Url.AbsoluteUri;

                string text =
                    path + ": " +
                    DateTime.Now.ToString(
                    "dd-MMM-yyyy hh:mm:ss:fff ") +
                    Environment.NewLine;
            }
        }
    }
    public string GenerateQrCode(string Orderid, string Amount)
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
            string postData = "{\"merchantID\":\"29159c34-8f20-49d8-a867-4618325f2f74\",\"securityCode\":\"a0a1649a-a91c-4861-baed-38422f686d6f\"}";
            string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
                             "(ReqID, Formno, Request, postdata, Req_From, OrderID,PageName) VALUES " +
                             "('" + sResult + "', '0', '" + URL + "', '" + postData +
                             "', 'AppLoginClaimMonth', '" + Orderid + "','Appmonthly_activation_points')";

            int x_Req = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);

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

            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + str + "' WHERE ReqID = '" + sResult + "' AND Req_From = 'AppLoginClaimMonth'";

            int x_res = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);

            data = convertJsonStringToDataSet(str);

            string auth = data.Tables[1].Rows[0]["token"].ToString();

            string sql = "INSERT INTO LoginTransaction " +
                         "(TId, Username, role, token, refreshToken, name, transactionid, amount,FormNo) VALUES (" +
                         "'" + data.Tables[1].Rows[0]["ID"] + "'," +
                         "'" + data.Tables[1].Rows[0]["username"] + "'," +
                         "'" + data.Tables[1].Rows[0]["role"] + "'," +
                         "'" + data.Tables[1].Rows[0]["token"] + "'," +
                         "'" + data.Tables[1].Rows[0]["refreshToken"] + "'," +
                         "'" + data.Tables[1].Rows[0]["name"] + "'," +
                         "'" + Orderid + "'," +
                         "'" + Amount + "','" + Session["formno"] + "')";

            int x = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql);

            Response.Write(data.Tables[1]);

            CheckLiveRateCoin(auth, Orderid, Amount);
        }
        catch (Exception ex)
        {
            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" + ex.Message +
                             "' WHERE ReqID = '" + sResult +
                             "' AND Req_From = 'AppLoginClaimMonth'";

            int x_res = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);
        }

        return str;
    }
    protected string CheckLiveRateCoin(string auth, string orderid, string amount)
    {
        try
        {
            string str = string.Empty;
            decimal value = 0;
            string code = "";
            DataSet ds = new DataSet();
            DataSet data;
            DataSet dsLogin = new DataSet();

            string completeUrl = "https://allupi.com/api/InitiateTransactionAsync";
            string responseString = string.Empty;
            string CustomerOTPmessage = "";
            string url = "";

            string sResult = string.Empty;
            string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int random_number = new Random().Next(0, 999);
            string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
            sResult = formatted_datetime;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                WebRequest tRequest = (HttpWebRequest)WebRequest.Create(completeUrl);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add("X-Auth", auth);

                string postdata = "{\"requestedId\":\"" + orderid + "\",";
                postdata += "\"amount\":" + amount + ",\"upiId\":\"82215511\",";
                postdata += "\"serverHookURL\":\"https://epayindia.in/AppLogin.aspx\",";
                postdata += "\"webHookURL\":\"https://epayindia.in/Paymentgatewayapp.aspx\"}";

                string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
                                 "(ReqID, Formno, Request, postdata, Req_From, OrderID,PageName) VALUES " +
                                 "('" + sResult + "', '" + Session["formno"] + "', '" + url + "', '" +
                                 postdata + "', 'AppInitiateTransactionAsyncClaimMonth', '" + orderid + "','Appmonthly_activation_points')";

                int x_Req = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);

                byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
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

                string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + str + "' WHERE ReqID = '" + sResult + "' AND Req_From = 'AppInitiateTransactionAsyncClaimMonth'";

                int x_res = SqlHelper.ExecuteNonQuery(
                    constr,
                    CommandType.Text,
                    sql_res
                );

                data = convertJsonStringToDataSet(str);

                Response.Write(str);

                if (data.Tables[0].Rows[0]["url"].ToString() != "")
                {
                    Response.Redirect(data.Tables[0].Rows[0]["url"].ToString());
                }
            }
            catch (Exception ex)
            {
                string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" +
                                 ex.Message + "' WHERE ReqID = '" + sResult +
                                 "' AND Req_From = 'AppInitiateTransactionAsyncClaimMonth'";

                SqlHelper.ExecuteNonQuery(
                    constr,
                    CommandType.Text,
                    sql_res
                );
            }

            return url;
        }
        catch
        {
            return string.Empty;
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