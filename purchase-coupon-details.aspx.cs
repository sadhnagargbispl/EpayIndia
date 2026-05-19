using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using System.Xml;
public partial class purchase_coupon_details : System.Web.UI.Page
{
    ModuleFunction objModuleFun = new ModuleFunction();
    DAL ObjDAL = new DAL();
    string IsoStart;
    string IsoEnd;
    string constr = System.Configuration.ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = System.Configuration.ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string kitid_;
    protected void Page_Load(object sender, EventArgs e)
    {
        IsoStart = ObjDAL.Isostart;
        IsoEnd = ObjDAL.IsoEnd;
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                BtnProceedToPay.Attributes.Add("onclick", DisableTheButton(this.Page, BtnProceedToPay));

                if (!string.IsNullOrEmpty(Request["kitid"]))
                {
                    kitid_ = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["kitid"]));
                    FillKit(kitid_);
                    //FillDis(kitid_);
                }

                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringActive(6);
                }
            }
            else
            {
                Response.Redirect("login.aspx", false);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    private string DisableTheButton(Control pge, Control btn)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");
        return sb.ToString();
    }
    private void FillKit(string kitid)
    {
        try
        {
            DataSet ds = new DataSet();
            string sql = IsoStart + "Exec Sp_GetKitAmount @KitID " + IsoEnd;
            SqlParameter[] parameters = {new SqlParameter("@KitID", SqlDbType.VarChar) { Value = kitid }};
            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql, parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                LblPakageAmount.Text = ds.Tables[0].Rows[0]["Kitamount"].ToString();
                LblDiscount.Text = ds.Tables[0].Rows[0]["discount"].ToString();
                Lbltickets.Text = ds.Tables[0].Rows[0]["Tickets"].ToString();
                LblAMount.Text = ds.Tables[0].Rows[0]["couponprice"].ToString();
                Lblwallet.Text = ds.Tables[0].Rows[0]["Kitamount"].ToString();
                LblSaveamount.Text = ds.Tables[0].Rows[0]["saveamount"].ToString();
                OriginalPrice.Text = ds.Tables[0].Rows[0]["couponprice"].ToString();
                Discount.Text = ds.Tables[0].Rows[0]["discount"].ToString();
                LblPackageName.Text  = ds.Tables[0].Rows[0]["kitname"].ToString();
                DiscountLess.Text = ds.Tables[0].Rows[0]["saveamount"].ToString();
                LblWalletCredit.Text = ds.Tables[0].Rows[0]["Kitamount"].ToString();
                LblYouPay.Text = ds.Tables[0].Rows[0]["Kitamount"].ToString();

                LblUserName.Text = Session["MemName"].ToString();
                LblUserID.Text = Session["IDNo"].ToString();
                TxtAmount.Text = (Convert.ToDecimal(ds.Tables[0].Rows[0]["Kitamount"])).ToString("0.00");
                GetBalance();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void GetBalance()
    {
        try
        {
            DataTable dt = new DataTable();
            string str = IsoStart + " Select * From dbo.ufnGetBalance(@FormNo, 'B')" + IsoEnd;

            // Using parameterized query to avoid SQL injection
            SqlParameter[] parameters = {
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = Convert.ToInt32(Session["Formno"]) }
        };

            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str, parameters).Tables[0];

            if (dt.Rows.Count > 0)
            {
                Lblwalletbalance.Text = Convert.ToString(dt.Rows[0]["Balance"]);
            }
            else
            {
                Lblwalletbalance.Text = "0";
            }

            Session["ServiceWallet"] = Lblwalletbalance.Text;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    //private void FillDis(string kitid)
    //{
    //    try
    //    {
    //        DataSet ds = new DataSet();
    //        string sql = "Exec Sp_GetKitDisDetailsNew @KitID";

    //        // Using parameterized query to avoid SQL injection
    //        SqlParameter[] parameters = {
    //        new SqlParameter("@KitID", SqlDbType.Int) { Value = kitid }
    //    };

    //        ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql, parameters);

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Dis"].ToString()))
    //            {
    //                LblFoodDis.Text = ds.Tables[0].Rows[0]["Dis"].ToString();
    //                LblFoodUse.Text = ds.Tables[0].Rows[0]["Uses"].ToString();
    //                LblFoodTerms.Text = ds.Tables[0].Rows[0]["Trmscon"].ToString();
    //                DivMDescription_Food.Visible = false;
    //            }
    //            else
    //            {
    //                DivMDescription_Food.Visible = false;
    //            }
    //        }
    //        else
    //        {
    //            DivMDescription_Food.Visible = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}
    public string GenerateRandomStringActive(int iLength)
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
    protected void MM_VoucherPurchase()
    {
        string query = "";
        string scrName = "";
        try
        {
            string StrSql = "Insert into Trnactive (Transid, Rectimestamp) values(@Transid, getdate())";
            SqlParameter[] parameters = {
            new SqlParameter("@Transid", SqlDbType.Int) { Value = HdnCheckTrnns.Value }
        };

            //int updateeffect = ObjDAL.SaveData(StrSql, parameters);
            string strSql = "Insert into Trnactive (Transid, Rectimestamp) values(" + HdnCheckTrnns.Value + ", GETDATE())";
            int updateeffect = ObjDAL.SaveData(strSql);

            if (updateeffect > 0)
            {
                string sql = "";
                string BillNo = GenerateRandomStringActive(6);
                sql = "Exec Sp_MMVoucherPurchase @FormNo, @KitId, @Amount, @PackageName, @BillNo";

                SqlParameter[] purchaseParameters = {
                new SqlParameter("@FormNo", SqlDbType.Int) { Value = Convert.ToInt32(Session["Formno"]) },
                new SqlParameter("@KitId", SqlDbType.Int) { Value = kitid_ },
                new SqlParameter("@Amount", SqlDbType.Decimal) { Value = Convert.ToDecimal(TxtAmount.Text) },
                new SqlParameter("@PackageName", SqlDbType.VarChar, 255) { Value = LblPackageName.Text },
                new SqlParameter("@BillNo", SqlDbType.VarChar, 50) { Value = BillNo }
            };

                DataTable dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql, purchaseParameters).Tables[0];

                if (dt.Rows[0]["Result"].ToString().ToUpper() == "SUCCESS")
                {
                    GetBalance();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", string.Format("alert('Package Purchase Successfully!!');location.replace('thankyou.aspx?billno={0}&kitid={1}');", BillNo, kitid_), true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Id Activation/Upgrade Not Successfully!!');", true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private double Amount()
    {
        try
        {
            DataTable dt = new DataTable();
            double RtrVal = 0;
            string Str = IsoStart + "Select balance From dbo.ufnGetBalance(@FormNo, 'B')" + IsoEnd;

            SqlParameter[] parameters = {
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = Convert.ToInt32(Session["FormNo"]) }
        };

            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Str, parameters).Tables[0];

            if (dt.Rows.Count > 0)
            {
                RtrVal = Convert.ToDouble(dt.Rows[0]["Balance"]);
            }

            return RtrVal;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            return 0; // Return 0 or handle the error as needed
        }
    }
    protected void BtnProceedToPay_Click(object sender, EventArgs e)
    {
        string scrName = "";
        if (Convert.ToDouble(TxtAmount.Text) > Amount())
        {
            scrName = "<SCRIPT language='javascript'>alert('Insufficient Balance!! ');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
        }
        else
        {
            string FinalAmount = Convert.ToDecimal(TxtAmount.Text).ToString();
            PackageCheckResult result = Fund_PackageCondtion_Check();
            if (result.Result == "FAILED")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", string.Format("alert('" + result.Msg + "');location.replace('index.aspx');"), true);
            }
            else
            {
                MM_VoucherPurchase();
                //string RemainingBal = Fund_Balance_Check();

                //if (Convert.ToDecimal(RemainingBal) >= Convert.ToDecimal(FinalAmount))
                //{
                //    var response = Fun_SameMasterDebitEntrty(LblUserID.Text.Trim(), Convert.ToInt32(kitid_), Convert.ToDecimal(FinalAmount));
                //    if (response.ToString().ToUpper() == "OK")
                //    {

                //    }
                //}
                //else
                //{
                //    scrName = "<SCRIPT language='javascript'>alert('Insufficient Balance In Utility Wallet.Please Contact To Admin.!');</SCRIPT>";
                //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrName, false);
                //}
            }
        }

    }
    public PackageCheckResult Fund_PackageCondtion_Check()
    {
        string sResult = "", sResuldt = "";
        try
        {
            DataSet ds = new DataSet();
            string sql = "Exec Sp_GetPachageCondtion '" + Session["formno"] + "'";
            ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                sResuldt = ds.Tables[0].Rows[0]["Result"].ToString();
                sResult = ds.Tables[0].Rows[0]["Msg"].ToString();
            }
        }
        catch (Exception ex)
        {
            sResuldt = "Failed";
            sResult = "Error occurred while checking package condition.";
        }

        return new PackageCheckResult
        {
            Result = sResuldt,
            Msg = sResult
        };
    }
    //protected void BtnProceedToPay_Click(object sender, EventArgs e)
    //{
    //    string Strqueryquer = "Insert into Trnjoining(Transid)values(" + HdnCheckTrnns.Value + ")";
    //    int isOk1 = 0;
    //    try
    //    {
    //        isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Strqueryquer));
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //    if (isOk1 > 0)
    //    {
    //        string OrderId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    //        string sql = "INSERT INTO OnlineTransaction(Orderid, Orderdate, Amount, name,kitid,FormNo,idno,EpinNo) " +
    //                     "VALUES('" + OrderId + "', GETDATE(), '" + LblPakageAmount.Text + "','" + Session["MemName"] + "','" + kitid_ + "','" + Session["formno"] + "','" + Session["idno"] + "','Purchase')";

    //        int i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql);
    //        if (i > 0)
    //        {
    //            GenerateQrCode(OrderId, LblPakageAmount.Text);
    //        }
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Try Again After Some Time.!');location.replace('purchase-coupon.aspx');", true);
    //        return;
    //    }
    //}
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
    //                         "(ReqID, Formno, Request, postdata, Req_From, OrderID) VALUES " +
    //                         "('" + sResult + "', '0', '" + URL + "', '" + postData +
    //                         "', 'Loginpurchase', '" + Orderid + "')";

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

    //        string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + str + "' WHERE ReqID = '" + sResult + "' AND Req_From = 'Loginpurchase'";

    //        int x_res = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);

    //        data = convertJsonStringToDataSet(str);

    //        string auth = data.Tables[1].Rows[0]["token"].ToString();

    //        string sql = "INSERT INTO LoginTransaction " +
    //                     "(TId, Username, role, token, refreshToken, name, transactionid, amount) VALUES (" +
    //                     "'" + data.Tables[1].Rows[0]["ID"] + "'," +
    //                     "'" + data.Tables[1].Rows[0]["username"] + "'," +
    //                     "'" + data.Tables[1].Rows[0]["role"] + "'," +
    //                     "'" + data.Tables[1].Rows[0]["token"] + "'," +
    //                     "'" + data.Tables[1].Rows[0]["refreshToken"] + "'," +
    //                     "'" + data.Tables[1].Rows[0]["name"] + "'," +
    //                     "'" + Orderid + "'," +
    //                     "'" + Amount + "')";

    //        int x = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql);

    //        Response.Write(data.Tables[1]);

    //        CheckLiveRateCoin(auth, Orderid, Amount);
    //    }
    //    catch (Exception ex)
    //    {
    //        string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET ErrorMsg = '" + ex.Message +
    //                         "' WHERE ReqID = '" + sResult +
    //                         "' AND Req_From = 'Loginpurchase'";

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
    //            postdata += "\"webHookURL\":\"https://epayindia.in/PaymentGatewayPurchase.aspx\"}";

    //            string sql_req = "INSERT INTO Tbl_ApiRequest_ResponsePaymentGateway " +
    //                             "(ReqID, Formno, Request, postdata, Req_From, OrderID) VALUES " +
    //                             "('" + sResult + "', '" + Session["formno"] + "', '" + url + "', '" +
    //                             postdata + "', 'InitiateTransactionAsyncPurchase', '" + orderid + "')";

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

    //            string sql_res = "UPDATE Tbl_ApiRequest_ResponsePaymentGateway SET Response = '" + str + "' WHERE ReqID = '" + sResult + "' AND Req_From = 'InitiateTransactionAsyncPurchase'";

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
    //                             "' AND Req_From = 'InitiateTransactionAsyncPurchase'";

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
    //public DataSet convertJsonStringToDataSet(string jsonString)
    //{
    //    XmlDocument xd = new XmlDocument();

    //    jsonString = "{ \"rootNode\": {" +
    //                 jsonString.Trim().TrimStart('{').TrimEnd('}') +
    //                 "} }";

    //    xd = JsonConvert.DeserializeXmlNode(jsonString);

    //    DataSet ds = new DataSet();
    //    ds.ReadXml(new XmlNodeReader(xd));

    //    return ds;
    //}
}
public class PackageCheckResult
{
    public string Result { get; set; }
    public string Msg { get; set; }
}
