//using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class BuyPromoCode : System.Web.UI.Page
{
    string scrname;
    DAL Obj;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                this.CmdSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.CmdSave));
                BtnOtp.Attributes.Add("onclick", DisableTheButton(Page, BtnOtp));
                ResendOtp.Attributes.Add("onclick", DisableTheButton(Page, ResendOtp));
                if (!Page.IsPostBack)
                {
                    Session["OtpCount"] = 0;
                    Session["OtpTime"] = null;
                    Session["Retry"] = null;
                    Session["OTP_"] = null;
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                    TxtCredit.Text = GetBalance();
                    FillDrop();
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
    private void FillDrop()
    {
        try
        {
            Obj = new DAL();
            DataTable dtMaster = new DataTable();
            string str = "select * from PurchaseMaster where Activestatus = 'Y'";
            dtMaster = Obj.GetData(str);
            if (dtMaster.Rows.Count > 0)
            {
                DDlCode.DataSource = dtMaster;
                DDlCode.DataValueField = "Type";
                DDlCode.DataTextField = "Name";
                DDlCode.DataBind();
                TxtAmount.Text = dtMaster.Rows[0]["Amount"].ToString();
                // txtCouponCount.Text = dtMaster.Rows[0]["MinQty"].ToString();
            }
        }
        catch (Exception ex)
        {
            // optional: log exception
            // Obj.WriteToFile(ex.Message);
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
    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "").Trim();
            return StrObj;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = $"{path}:  {DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff")} {Environment.NewLine}";
            Obj.WriteToFile($"{text}{ex.Message}");
            Response.Write("Try later.");
            return ""; // Or handle the exception as per your application's requirements
        }
    }
    protected void Txtqty_TextChanged(object sender, EventArgs e)
    {
        decimal qty = decimal.Parse(Txtqty.Text);
        decimal amount = decimal.Parse(TxtAmount.Text);

        decimal totalAmount = qty * amount;

        TxtFinalAmount.Text = totalAmount.ToString();
        CmdSave.Enabled = true;
        errMsg.Visible = false;
        if (Convert.ToDouble(Session["ServiceWallet"]) < Convert.ToDouble(TxtFinalAmount.Text))
        {
            Label2.Text = "Insufficient Balance";
            Label2.ForeColor = System.Drawing.Color.Red;
            Label2.Visible = true;
            CmdSave.Enabled = false;
            return;
        }
        else
        {
            CmdSave.Enabled = true;
            Label2.Visible = false;
            return;
        }

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
    protected void CmdSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Txtqty.Text))
        {
            errMsg.Text = "Please Enter Qty.!";
            CmdSave.Enabled = false;
            errMsg.Visible = true;
            return;
        }
        else
        {
            CmdSave.Enabled = true;
            errMsg.Visible = false;
        }
        int qty = int.Parse(Txtqty.Text);
        if (qty < 5)
        {
            errMsg.Text = "Please purchase at least 5 coupons.";
            CmdSave.Enabled = false;
            errMsg.Visible = true;
            return;
        }
        else
        {
            CmdSave.Enabled = true;
            errMsg.Visible = false;
        }
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        sResult = current_datetime + random_number.ToString().PadLeft(3, '0');
        SendOtp(sResult);
    }
    public string GetBalance()
    {
        string URL = string.Empty;
        string sResult = string.Empty;
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;

        string TokenBalance = string.Empty;
        DataSet data = new DataSet();

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // TLS 1.2
            URL = "https://uatwavemoneyapp.wavemoney.app/api/shop/check_wallet?token=Xr21X8isdf867asdf86adf26wIxwFMyq3ve&username=" + Session["IDNo"].ToString() + "&password=" + Session["MemPassw"] + "&action=checkewallet";   // FULL URL hona chahiye
            string sqlReq = @"INSERT INTO Tbl_ApiRequest_ResponsePayment
                      (ReqID, Request, postdata, IsWebUser)
                      VALUES (@ReqID, @Request, @PostData, @IsWebUser)";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq,
                new SqlParameter("@ReqID", sResult),
                new SqlParameter("@Request", URL),
                new SqlParameter("@PostData", ""),
                new SqlParameter("@IsWebUser", "get_user_balance")
            );

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json";
            string responseText = "";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        responseText = reader.ReadToEnd(); // error JSON bhi read hoga
                    }
                }
                else
                {
                    responseText = ex.Message;
                }
            }

            // ---- Log Response ----
            string sqlRes = @"UPDATE Tbl_ApiRequest_ResponsePayment 
                      SET Response = @Response 
                      WHERE ReqID = @ReqID";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes,
                new SqlParameter("@Response", responseText),
                new SqlParameter("@ReqID", sResult)
            );
            data = convertJsonStringToDataSet(responseText);
            TokenBalance = data.Tables[0].Rows[0]["usd_inr_balance"].ToString();
            Session["ServiceWallet"] = TokenBalance;
        }
        catch (Exception ex)
        {
            string sqlRes = @"UPDATE Tbl_ApiRequest_ResponsePayment 
                      SET Response = @Response 
                      WHERE ReqID = @ReqID";

            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes,
                new SqlParameter("@Response", ex.ToString()),
                new SqlParameter("@ReqID", sResult)
            );
        }

        return TokenBalance;
        //string URL = string.Empty;
        //string sResult = string.Empty;
        //string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //int random_number = new Random().Next(0, 999);
        //string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        //sResult = formatted_datetime;
        //string postData = string.Empty;
        //string TokenBalance = string.Empty;
        //string str = string.Empty;
        //DataSet data = new DataSet();
        //try
        //{
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        //    URL = "https://uatwavemoney.onewave.app/api/shop/get_user_balance";
        //    string apiKey = "Xr21X8iqmq8n212gj26wIxwFMyq3ve";
        //    string Token = "Bearer " + Session["Token"].ToString();
        //    string sqlReq = "INSERT INTO Tbl_ApiRequest_ResponsePayment (ReqID, Request, postdata,IsWebUser)VALUES (@ReqID, @Request, @PostData,@IsWebUser)";
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlReq,
        //        new SqlParameter("@ReqID", sResult.Trim()),
        //        new SqlParameter("@Request", URL),
        //        new SqlParameter("@PostData", postData),
        //         new SqlParameter("@IsWebUser", "get_user_balance")
        //    );
        //    HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create(URL);
        //    tRequest.Method = "POST";
        //    tRequest.ContentType = "application/json";
        //    tRequest.Headers.Add("key", apiKey);
        //    tRequest.Headers.Add("Authorization", Token);
        //    //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    //tRequest.ContentLength = byteArray.Length;
        //    //using (Stream dataStream = tRequest.GetRequestStream())
        //    //{
        //    //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    //}
        //    tRequest.ContentLength = 0;
        //    string responseText = "";
        //    try
        //    {
        //        using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
        //        using (Stream dataStream = tResponse.GetResponseStream())
        //        using (StreamReader tReader = new StreamReader(dataStream))
        //        {
        //            responseText = tReader.ReadToEnd();
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Response != null)
        //        {
        //            using (var errorResponse = (HttpWebResponse)ex.Response)
        //            using (var dataStream = errorResponse.GetResponseStream())
        //            using (var reader = new StreamReader(dataStream))
        //            {
        //                // READ JSON EVEN IF STATUS = 400
        //                responseText = reader.ReadToEnd();
        //            }
        //        }
        //        else
        //        {
        //            responseText = ex.Message;
        //        }
        //    }
        //    // Log API response
        //    string sqlRes = "UPDATE Tbl_ApiRequest_ResponsePayment SET Response = @Response WHERE ReqID = @ReqID";
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes,
        //        new SqlParameter("@Response", responseText),
        //        new SqlParameter("@ReqID", sResult.Trim())
        //    );
        //    // Parse JSON
        //    data = convertJsonStringToDataSet(responseText);
        //    TokenBalance = data.Tables[1].Rows[0]["inr_balance"].ToString();
        //    Session["ServiceWallet"] = data.Tables[1].Rows[0]["inr_balance"].ToString();
        //}
        //catch (Exception ex)
        //{
        //    // Log error response
        //    string sqlRes = "UPDATE Tbl_ApiRequest_ResponsePayment SET Response = @Response WHERE ReqID = @ReqID";
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sqlRes,
        //        new SqlParameter("@Response", ex.ToString()),
        //        new SqlParameter("@ReqID", sResult.Trim())
        //    );
        //}
        //return TokenBalance;
    }
    public string SendOtp(string OrderNo)
    {
        string URL = "";
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        sResult = current_datetime + random_number.ToString().PadLeft(3, '0');

        string message = "";
        string res = "";
        DataSet data = new DataSet();

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            // ---- QUERY STRING (GET) ----
            string amount = TxtFinalAmount.Text;
            string orderId = OrderNo;
            string remark = OrderNo;
            URL = "https://uatwavemoneyapp.wavemoney.app/api/shop/deduct_wallet?token=Xr21X8isdf867asdf86adf26wIxwFMyq3ve&username=" + Session["IDNo"].ToString() + "&password=" + Session["MemPassw"] + "&action=deductewallet&orderId=" + orderId + "&amount=" + amount + "&remark=" + remark + "&walletType=USD";
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
                @"INSERT INTO Tbl_ApiRequest_ResponsePayment
          (ReqID,Request,PostData,IsWebUser,order_id)
          VALUES(@ReqID,@Request,@PostData,@IsWebUser,@order_id)",
                new SqlParameter("@ReqID", sResult),
                new SqlParameter("@Request", URL),
                new SqlParameter("@PostData", ""),
                new SqlParameter("@IsWebUser", "send_deduct_otp"),
                new SqlParameter("@order_id", OrderNo)
            );
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json";
            string responseText = "";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                else
                {
                    responseText = ex.Message;
                }
            }

            // ---- LOG RESPONSE ----
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
                "UPDATE Tbl_ApiRequest_ResponsePayment SET Response=@Response WHERE ReqID=@ReqID",
                new SqlParameter("@Response", responseText),
                new SqlParameter("@ReqID", sResult)
            );

            // ---- PARSE RESPONSE ----
            data = convertJsonStringToDataSet(responseText);
            res = data.Tables[0].Rows[0]["response"].ToString();
            message = data.Tables[0].Rows[0]["msg"].ToString();
            if (res.ToUpper().Contains("OTP_SENT"))
            {
                Session["OtpTime"] = "";
                Session["Retry"] = "1";
                Session["OTP_"] = data.Tables[0].Rows[0]["otp"].ToString();
                Session["order_id"] = OrderNo;
                Session["transaction_id"] = "";

                SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
                    @"INSERT INTO AdminLogin
              (transaction_id,order_id,deducted_amount,otp_expiry,OTP,FormNo)
              VALUES (@transaction_id,@order_id,@deducted_amount,@otp_expiry,@OTP,@FormNo)",
                    new SqlParameter("@transaction_id", ""),
                    new SqlParameter("@order_id", OrderNo),
                    new SqlParameter("@deducted_amount", data.Tables[0].Rows[0]["deductamount"]),
                    new SqlParameter("@otp_expiry", ""),
                    new SqlParameter("@OTP", data.Tables[0].Rows[0]["otp"]),
                    new SqlParameter("@FormNo", Session["formno"])
                );

                Txtqty.Enabled = false;
                DDlCode.Enabled = false;
                Label1.Visible = false;
                divotp.Visible = true;
                CmdSave.Visible = false;
                BtnOtp.Visible = true;
                ResendOtp.Visible = false;

                ScriptManager.RegisterClientScriptBlock(
                    this.Page, this.GetType(),
                    "msg",
                    "alert('" + message + "');",
                    true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(
                    this.Page, this.GetType(),
                    "msg",
                    "alert('" + message + "');",
                    true);
            }
        }
        catch (Exception ex)
        {
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
                "UPDATE Tbl_ApiRequest_ResponsePayment SET Response=@Response WHERE ReqID=@ReqID",
                new SqlParameter("@Response", ex.ToString()),
                new SqlParameter("@ReqID", sResult)
            );
        }

        return "";
        //string URL = "";
        //string sResult = "";
        //string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //int random_number = new Random().Next(0, 999);
        //sResult = current_datetime + random_number.ToString().PadLeft(3, '0');
        //string postData = "";
        //string message = "";
        //DataSet data = new DataSet();
        //try
        //{
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        //    URL = "https://uatwavemoney.onewave.app/api/shop/send_deduct_otp";
        //    string apiKey = "Xr21X8iqmq8n212gj26wIxwFMyq3ve";
        //    string Token = "Bearer " + Session["Token"].ToString();
        //    postData = "{\"amount\": \"" + TxtFinalAmount.Text + "\",\"order_id\": \"" + "test-" + OrderNo + "\",\"remark\": \"" + "test-" + OrderNo + "\"}";
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, "INSERT INTO Tbl_ApiRequest_ResponsePayment (ReqID,Request,PostData,IsWebUser,order_id)VALUES(@ReqID,@Request,@PostData,@IsWebUser,@order_id)",
        //    new SqlParameter("@ReqID", sResult), new SqlParameter("@Request", URL), new SqlParameter("@PostData", postData), new SqlParameter("@IsWebUser", "send_deduct_otp"), new SqlParameter("@order_id", OrderNo));
        //    HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create(URL);
        //    tRequest.Method = "POST";
        //    tRequest.ContentType = "application/json";
        //    tRequest.ServicePoint.Expect100Continue = true;
        //    tRequest.Headers.Add("key", apiKey);
        //    tRequest.Headers.Add("Authorization", Token);
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    tRequest.ContentLength = byteArray.Length;
        //    using (Stream dataStream = tRequest.GetRequestStream())
        //    {
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //    }
        //    string responseText = "";
        //    try
        //    {
        //        using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
        //        using (Stream dataStream = tResponse.GetResponseStream())
        //        using (StreamReader tReader = new StreamReader(dataStream))
        //        {
        //            responseText = tReader.ReadToEnd();
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Response != null)
        //        {
        //            using (var errorResponse = (HttpWebResponse)ex.Response)
        //            using (var dataStream = errorResponse.GetResponseStream())
        //            using (var reader = new StreamReader(dataStream))
        //            {
        //                responseText = reader.ReadToEnd();
        //            }
        //        }
        //        else
        //        {
        //            responseText = ex.Message;
        //        }
        //    }

        //    // UPDATE API RESPONSE LOG
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
        //        "UPDATE Tbl_ApiRequest_ResponsePayment SET Response=@Response WHERE ReqID=@ReqID",
        //        new SqlParameter("@Response", responseText),
        //        new SqlParameter("@ReqID", sResult)
        //    );
        //    data = convertJsonStringToDataSet(responseText);
        //    message = data.Tables[0].Rows[0]["message"].ToString();
        //    // SUCCESS CASE
        //    if (message.ToUpper() == "OTP SENT SUCCESSFULLY. PLEASE VERIFY TO COMPLETE TRANSACTION.")
        //    {
        //        Session["OtpTime"] = data.Tables[1].Rows[0]["otp_expiry"].ToString();
        //        Session["Retry"] = "1";
        //        Session["OTP_"] = data.Tables[1].Rows[0]["OTP"].ToString();
        //        Session["order_id"] = data.Tables[1].Rows[0]["order_id"].ToString();
        //        Session["transaction_id"] = data.Tables[1].Rows[0]["transaction_id"].ToString();
        //        // INSERT INTO AdminLogin (USE PARAMETERS)
        //        SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
        //            "INSERT INTO AdminLogin (transaction_id,order_id,deducted_amount,otp_expiry,OTP,FormNo) " +
        //            "VALUES (@transaction_id,@order_id,@deducted_amount,@otp_expiry,@OTP,@FormNo)",
        //            new SqlParameter("@transaction_id", data.Tables[1].Rows[0]["transaction_id"].ToString()),
        //            new SqlParameter("@order_id", data.Tables[1].Rows[0]["order_id"].ToString()),
        //            new SqlParameter("@deducted_amount", data.Tables[1].Rows[0]["deducted_amount"].ToString()),
        //            new SqlParameter("@otp_expiry", data.Tables[1].Rows[0]["otp_expiry"].ToString()),
        //            new SqlParameter("@OTP", data.Tables[1].Rows[0]["OTP"].ToString()),
        //            new SqlParameter("@FormNo", Session["formno"].ToString())
        //        );
        //        Txtqty.Enabled = false;
        //        DDlCode.Enabled = false;
        //        Label1.Visible = false;
        //        divotp.Visible = true;
        //        CmdSave.Visible = false;
        //        BtnOtp.Visible = true;
        //        ResendOtp.Visible = true;
        //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
        //            "msg", "alert('OTP sent successfully. Please verify to complete transaction.');", true);

        //        return "";
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
        //            "msg", "alert('" + message + "');", true);
        //        return "";
        //    }
        //}
        //catch (Exception ex)
        //{
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
        //        "UPDATE Tbl_ApiRequest_ResponsePayment SET Response=@Response WHERE ReqID=@ReqID",
        //        new SqlParameter("@Response", ex.ToString()),
        //        new SqlParameter("@ReqID", sResult)
        //    );

        //    return "";
        //}
    }
    public string VerifyOTp(string transaction_id, string order_id, string Otp)
    {
        string URL = "";
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        sResult = current_datetime + random_number.ToString().PadLeft(3, '0');

        string message = "";
        string res = "";
        DataSet data = new DataSet();

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            string apiKey = "";
            string Token = "";

            // ---- BUILD GET URL ----
            URL = "https://uatwavemoneyapp.wavemoney.app/api/shop/verify_deduct_wallet?token=Xr21X8isdf867asdf86adf26wIxwFMyq3ve&username=" + Session["IDNo"].ToString() + "&password=" + Session["MemPassw"] + "&action=verifydeduct&otp=" + Otp + "&orderId=" + order_id;
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
                @"INSERT INTO Tbl_ApiRequest_ResponsePayment
          (ReqID,Request,PostData,IsWebUser,order_id)
          VALUES(@ReqID,@Request,@PostData,@IsWebUser,@order_id)",
                new SqlParameter("@ReqID", sResult),
                new SqlParameter("@Request", URL),
                new SqlParameter("@PostData", ""),
                new SqlParameter("@IsWebUser", "deduct_amount"),
                new SqlParameter("@order_id", order_id)
            );

            // ---- GET REQUEST ----
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.UserAgent = "Mozilla/5.0";
            string responseText = "";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                else
                {
                    responseText = ex.Message;
                }
            }

            // ---- LOG RESPONSE ----
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
                "UPDATE Tbl_ApiRequest_ResponsePayment SET Response=@Response WHERE ReqID=@ReqID",
                new SqlParameter("@Response", responseText),
                new SqlParameter("@ReqID", sResult)
            );

            // ---- PARSE RESPONSE ----
            data = convertJsonStringToDataSet(responseText);
            message = data.Tables[0].Rows[0]["msg"].ToString();
            res = data.Tables[0].Rows[0]["response"].ToString();
            // ---- SUCCESS ----
            if (res.ToUpper() == "OK")
            {
                VerifyVoucher(data.Tables[0].Rows[0]["voucherno"].ToString(), order_id);
                //string Strqueryquer = "Insert into Trnjoining(Transid)values(" + HdnCheckTrnns.Value + ")";
                //int isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Strqueryquer));

                //if (isOk1 > 0)
                //{
                //    string query =
                //        "INSERT INTO CouponPurchase (FormNo, Code, Amount, Qty, FinalAmount, OrderNo) " +
                //        "VALUES ('" + Session["formno"] + "', '" + DDlCode.SelectedValue + "', '" +
                //        TxtAmount.Text + "','" + Txtqty.Text + "', '" + TxtFinalAmount.Text + "', '" +
                //        HdnCheckTrnns.Value + "');" +
                //        "Exec InsertMemberCoupons '" + Session["formno"] + "','" +
                //        TxtAmount.Text + "','" + Txtqty.Text + "','" +
                //        HdnCheckTrnns.Value + "','" + DDlCode.SelectedValue + "'";

                //    int i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, query));

                //    string message1 = i > 0
                //        ? "Coupon purchase successfully.!"
                //        : "Try Again Later.!";

                //    string script = "window.onload=function(){alert('" + message1 +
                //                    "');window.location='BuyPromoCode.aspx';}";
                //    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                //}
                //else
                //{
                //    Response.Redirect("BuyPromoCode.aspx");
                //}
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
                    "msg", "alert('" + message + "');", true);
            }
        }
        catch (Exception ex)
        {
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
                "UPDATE Tbl_ApiRequest_ResponsePayment SET Response=@Response WHERE ReqID=@ReqID",
                new SqlParameter("@Response", ex.ToString()),
                new SqlParameter("@ReqID", sResult)
            );
        }

        return "";
        //string URL = "";
        //string sResult = "";
        //string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //int random_number = new Random().Next(0, 999);
        //sResult = current_datetime + random_number.ToString().PadLeft(3, '0');
        //string postData = "";
        //string message = "";
        //DataSet data = new DataSet();
        //try
        //{
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        //    URL = "https://uatwavemoney.onewave.app/api/shop/deduct_amount";
        //    string apiKey = "Xr21X8iqmq8n212gj26wIxwFMyq3ve";
        //    string Token = "Bearer " + Session["Token"].ToString();
        //    postData = "{ \"order_id\": \"" + order_id +
        //   "\", \"transaction_id\": \"" + transaction_id +
        //   "\", \"otp\": " + Otp + " }";
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, "INSERT INTO Tbl_ApiRequest_ResponsePayment (ReqID,Request,PostData,IsWebUser,order_id)VALUES(@ReqID,@Request,@PostData,@IsWebUser,@order_id)",
        //    new SqlParameter("@ReqID", sResult), new SqlParameter("@Request", URL), new SqlParameter("@PostData", postData), new SqlParameter("@IsWebUser", "deduct_amount"), new SqlParameter("@order_id", order_id));
        //    HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create(URL);
        //    tRequest.Method = "POST";
        //    tRequest.ContentType = "application/json";

        //    // ⭐ REQUIRED FIXES
        //    tRequest.ServicePoint.Expect100Continue = true;
        //    tRequest.UserAgent = "Mozilla/5.0";
        //    tRequest.Accept = "application/json";   // ✔ correct way

        //    // API headers
        //    tRequest.Headers.Add("key", apiKey);
        //    tRequest.Headers.Add("Authorization", Token);

        //    // POST body
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    tRequest.ContentLength = byteArray.Length;

        //    using (Stream dataStream = tRequest.GetRequestStream())
        //    {
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //    }

        //    //HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create(URL);
        //    //tRequest.Method = "POST";
        //    //tRequest.ContentType = "application/json";
        //    //tRequest.Headers.Add("key", apiKey);
        //    //tRequest.Headers.Add("Authorization", Token);
        //    //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    //tRequest.ContentLength = byteArray.Length;
        //    //using (Stream dataStream = tRequest.GetRequestStream())
        //    //{
        //    //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    //}
        //    string responseText = "";
        //    try
        //    {
        //        using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
        //        using (Stream dataStream = tResponse.GetResponseStream())
        //        using (StreamReader tReader = new StreamReader(dataStream))
        //        {
        //            responseText = tReader.ReadToEnd();
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        if (ex.Response != null)
        //        {
        //            using (var errorResponse = (HttpWebResponse)ex.Response)
        //            using (var dataStream = errorResponse.GetResponseStream())
        //            using (var reader = new StreamReader(dataStream))
        //            {
        //                responseText = reader.ReadToEnd();
        //            }
        //        }
        //        else
        //        {
        //            responseText = ex.Message;
        //        }
        //    }

        //    // UPDATE API RESPONSE LOG
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
        //        "UPDATE Tbl_ApiRequest_ResponsePayment SET Response=@Response WHERE ReqID=@ReqID",
        //        new SqlParameter("@Response", responseText),
        //        new SqlParameter("@ReqID", sResult)
        //    );
        //    data = convertJsonStringToDataSet(responseText);
        //    message = data.Tables[0].Rows[0]["message"].ToString();
        //    // SUCCESS CASE
        //    if (message.ToUpper() == "TRANSACTION SUCCESSFUL")
        //    {
        //        try
        //        {
        //            string Strqueryquer = "";
        //            Strqueryquer = "Insert into Trnjoining(Transid)values(" + HdnCheckTrnns.Value + ")";
        //            int isOk1 = 0;
        //            try
        //            {
        //                isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Strqueryquer));
        //            }
        //            catch (Exception ex)
        //            {
        //                isOk1 = 0;
        //            }
        //            if (isOk1 > 0)
        //            {
        //                string query = "INSERT INTO CouponPurchase (FormNo, Code, Amount, Qty, FinalAmount, OrderNo) " +
        //       "VALUES ('" + Session["formno"] + "', '" + DDlCode.SelectedValue + "', '" + TxtAmount.Text + "','" + Txtqty.Text + "', '" + TxtFinalAmount.Text + "', '" + HdnCheckTrnns.Value + "');" +
        //       "Exec InsertMemberCoupons '" + Session["formno"] + "','" + TxtAmount.Text + "','" + Txtqty.Text + "','" + HdnCheckTrnns.Value + "','" + DDlCode.SelectedValue + "'";
        //                int i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, query));
        //                string message1 = "";
        //                if (i > 0)
        //                    message1 = "Coupon purchase successfully.!";
        //                else
        //                    message1 = "Try Again Later.!";

        //                string url = "BuyPromoCode.aspx";
        //                string script = "window.onload = function(){ alert('";
        //                script += message1;
        //                script += "');";
        //                script += "window.location = '";
        //                script += url;
        //                script += "'; }";
        //                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
        //                CmdSave.Visible = true;
        //                return "";
        //            }
        //            else
        //            {
        //                Response.Redirect("BuyPromoCode.Aspx");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Label1.Text = "Error while saving. Please try again.";
        //        }
        //        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
        //        //    "msg", "alert('OTP sent successfully. Please verify to complete transaction.');", true);

        //        return "";
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
        //            "msg", "alert('Invalid OTP');", true);
        //        return "";
        //    }
        //}
        //catch (Exception ex)
        //{
        //    SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
        //        "UPDATE Tbl_ApiRequest_ResponsePayment SET Response=@Response WHERE ReqID=@ReqID",
        //        new SqlParameter("@Response", ex.ToString()),
        //        new SqlParameter("@ReqID", sResult)
        //    );

        //    return "";
        //}
    }
    public string VerifyVoucher(string voucherno, string order_id)
    {
        string URL = "";
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        sResult = current_datetime + random_number.ToString().PadLeft(3, '0');

        string message = "";
        string res = "";
        DataSet data = new DataSet();

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            string apiKey = "";
            string Token = "";

            // ---- BUILD GET URL ----
            URL = "https://uatwavemoneyapp.wavemoney.app/api/shop/check_voucher?token=Xr21X8isdf867asdf86adf26wIxwFMyq3ve&username=" + Session["IDNo"].ToString() + "&password=" + Session["MemPassw"] + "&action=checkvoucher&voucherno=" + voucherno;
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
                @"INSERT INTO Tbl_ApiRequest_ResponsePayment
          (ReqID,Request,PostData,IsWebUser,order_id)
          VALUES(@ReqID,@Request,@PostData,@IsWebUser,@order_id)",
                new SqlParameter("@ReqID", sResult),
                new SqlParameter("@Request", URL),
                new SqlParameter("@PostData", ""),
                new SqlParameter("@IsWebUser", "check_voucher"),
                new SqlParameter("@order_id", order_id)
            );

            // ---- GET REQUEST ----
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.UserAgent = "Mozilla/5.0";
            string responseText = "";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        responseText = reader.ReadToEnd();
                    }
                }
                else
                {
                    responseText = ex.Message;
                }
            }

            // ---- LOG RESPONSE ----
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
                "UPDATE Tbl_ApiRequest_ResponsePayment SET Response=@Response WHERE ReqID=@ReqID",
                new SqlParameter("@Response", responseText),
                new SqlParameter("@ReqID", sResult)
            );

            // ---- PARSE RESPONSE ----
            data = convertJsonStringToDataSet(responseText);
            message = data.Tables[0].Rows[0]["msg"].ToString();
            res = data.Tables[0].Rows[0]["response"].ToString();
            // ---- SUCCESS ----
            if (res.ToUpper() == "OK")
            {
                string Strqueryquer = "Insert into Trnjoining(Transid)values(" + HdnCheckTrnns.Value + ")";
                int isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Strqueryquer));

                if (isOk1 > 0)
                {
                    string query =
                        "INSERT INTO CouponPurchase (FormNo, Code, Amount, Qty, FinalAmount, OrderNo) " +
                        "VALUES ('" + Session["formno"] + "', '" + DDlCode.SelectedValue + "', '" +
                        TxtAmount.Text + "','" + Txtqty.Text + "', '" + TxtFinalAmount.Text + "', '" +
                        HdnCheckTrnns.Value + "');" +
                        "Exec InsertMemberCoupons '" + Session["formno"] + "','" +
                        TxtAmount.Text + "','" + Txtqty.Text + "','" +
                        HdnCheckTrnns.Value + "','" + DDlCode.SelectedValue + "'";

                    int i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, query));

                    string message1 = i > 0
                        ? "Coupon purchase successfully.!"
                        : "Try Again Later.!";

                    string script = "window.onload=function(){alert('" + message1 +
                                    "');window.location='BuyPromoCode.aspx';}";
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                }
                else
                {
                    Response.Redirect("BuyPromoCode.aspx");
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
                    "msg", "alert('" + message + "');", true);
            }
        }
        catch (Exception ex)
        {
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text,
                "UPDATE Tbl_ApiRequest_ResponsePayment SET Response=@Response WHERE ReqID=@ReqID",
                new SqlParameter("@Response", ex.ToString()),
                new SqlParameter("@ReqID", sResult)
            );
        }
        return "";
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
    protected void BtnOtp_Click(object sender, EventArgs e)
    {
        try
        {
            string scrname = "";
            string email = "";
            DataTable dt = new DataTable();
            string transPassw = TxtOtp.Text.Trim();
            DataTable dt1 = new DataTable();
            Session["OtpCount"] = Convert.ToInt32(Session["OtpCount"]) + 1;
            if (Session["OTP_"] != null && Session["OTP_"].ToString() == transPassw)
            {
                string query = "SELECT * FROM AdminLogin AS a WHERE OTP = '" + Session["OTP_"] + "' AND formno = '" + Session["FormNo"] + "' ORDER BY ID DESC";
                dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    VerifyOTp(Session["transaction_id"].ToString(), Session["order_id"].ToString(), Session["OTP_"].ToString());
                }
            }
            else
            {
                TxtOtp.Text = "";

                if (Convert.ToInt32(Session["OtpCount"]) >= 3)
                {
                    Session["OtpCount"] = 0;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "msg", "alert('You have tried 3 times with invalid OTP.\\nPlease generate OTP again.');", true);
                    ResendOtp.Visible = true;
                    BtnOtp.Visible = false;
                    divotp.Visible = false;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "msg", "alert('Invalid OTP.');", true);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception
        }

    }
    protected void ResendOtp_Click(object sender, EventArgs e)
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        sResult = current_datetime + random_number.ToString().PadLeft(3, '0');
        SendOtp(sResult);
    }
}