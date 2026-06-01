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
public partial class Apppurchase_coupon_details : System.Web.UI.Page
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
                Response.Redirect("Applogin.aspx", false);
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
            SqlParameter[] parameters = { new SqlParameter("@KitID", SqlDbType.VarChar) { Value = kitid } };
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
                LblPackageName.Text = ds.Tables[0].Rows[0]["kitname"].ToString();
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", string.Format("alert('Package Purchase Successfully!!');location.replace('AppThankYou.aspx?billno={0}&kitid={1}');", BillNo, kitid_), true);
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
            PackageCheckResultS result = Fund_PackageCondtion_Check();
            if (result.Result == "FAILED")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", string.Format("alert('" + result.Msg + "');location.replace('WebApp.aspx');"), true);
            }
            else
            {
                
                string RemainingBal = Fund_Balance_Check();

                if (Convert.ToDecimal(RemainingBal) >= Convert.ToDecimal(FinalAmount))
                {
                    var response = Fun_SameMasterDebitEntrty(LblUserID.Text.Trim(), Convert.ToInt32(kitid_), Convert.ToDecimal(FinalAmount));
                    if (response.ToString().ToUpper() == "OK")
                    {
                        MM_VoucherPurchase();
                    }
                }
                else
                {
                    scrName = "<SCRIPT language='javascript'>alert('Insufficient Balance In Utility Wallet.Please Contact To Admin.!');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrName, false);
                }
            }
        }

    }
    public string Fund_Balance_Check()
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;

        string postData = "";
        string URL = "";
        string Str = "";
        string balance = "";

        try
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            URL = "http://masteradmin.bisplindia.in/DTProcess";
            HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create(URL);
            tRequest.Method = "POST";
            tRequest.ContentType = "application/json";

            postData = "{\"reqtype\": \"getwalletbalance\",\"companyid\": \"120\",\"actype\": \"M\",\"key\": \"kgFt9rswQ6hDu3Pm\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            string sql_req = "INSERT INTO Tbl_ApiRequest_Response(ReqID, Formno, Request, postdata, ForType) " +
                             "VALUES('" + sResult + "','" + Convert.ToInt32(HttpContext.Current.Session["FormNo"]) + "', '" + URL + "', '" + postData + "', 'BalancemasterWalletApp')";
            int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_req));

            tRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            WebResponse tResponse = tRequest.GetResponse();
            using (Stream dataStream = tResponse.GetResponseStream())
            using (StreamReader tReader = new StreamReader(dataStream))
            {
                Str = tReader.ReadToEnd();
            }

            string sql_res = "Update Tbl_ApiRequest_Response Set Response = '" + Str.Trim() +
                             "' Where ReqID = '" + sResult.Trim() + "' AND ForType = 'BalancemasterWalletApp'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));

            try
            {
                DataSet data = convertJsonStringToDataSet(Str);
                balance = Convert.ToDecimal(data.Tables[1].Rows[0]["Balance"]).ToString();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;

                string errorQry = "INSERT INTO TrnLogData(ErrorText, LogDate, Url, WalletAddress, PostData, formno) " +
                                  "VALUES('" + errorMsg + "', GETDATE(),'" + URL + "','','" + Str + "','98')";
                int x1 = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, errorQry);

                balance = "0";
            }
        }
        catch (Exception ex)
        {
            string sql_res = "Update Tbl_ApiRequest_Response Set Response = '" + ex.Message +
                             "' Where ReqID = '" + sResult.Trim() + "' AND ForType = 'BalancemasterWalletApp'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
            balance = "0";
        }

        return balance;
    }
    public string Fun_SameMasterDebitEntrty(string username, int kitid, decimal amount)
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
        sResult = formatted_datetime;

        string status = "";
        string responseString = string.Empty;
        DataSet data = new DataSet();

        try
        {
            string MaxVoucherNo_ = "2" + DateTime.Now.ToString("ddMMyyyyHHmmssfff");
            long maxVno_ = Convert.ToInt64(MaxVoucherNo_) + Convert.ToInt64(HttpContext.Current.Session["Formno"].ToString());
            string reqNo = maxVno_.ToString();

            string deductionDetails = "Amount Deduction By Id Activation of IdNo: " + username;
            string deductionRefno = "Master/" + kitid + "/" + Convert.ToInt32(HttpContext.Current.Session["FormNo"]) + "/" + sResult;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            string URL = "http://masteradmin.bisplindia.in/DTProcess";
            HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create(URL);
            tRequest.Method = "POST";
            tRequest.ContentType = "application/json";

            string postData = "{\"reqtype\":\"debitamount\",\"tokenno\":\"DF78F4C276874E46BFC151C3065ABE1A\",\"id\":\"120\",\"amount\":\"" + amount + "\"," +
                              "\"narration\":\"" + deductionDetails + "\",\"refno\":\"" + deductionRefno + "\",\"vtype\":\"D\",\"actype\":\"M\",\"useactype\":\"M\",\"key\":\"kgFt9rswQ6hDu3Pm\"}";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            string sql_req = "INSERT INTO Tbl_ApiRequest_Response (ReqID, Formno, Request, postdata, ForType) " +
                             "VALUES ('" + sResult + "','" + Convert.ToInt32(HttpContext.Current.Session["FormNo"]) + "','" + URL + "','" + postData + "','MasterWalletDebitApp')";
            int x_Req = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_req));

            tRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            WebResponse tResponse = tRequest.GetResponse();
            using (Stream dataStream = tResponse.GetResponseStream())
            using (StreamReader tReader = new StreamReader(dataStream))
            {
                responseString = tReader.ReadToEnd();
            }

            string sql_res = "UPDATE Tbl_ApiRequest_Response SET Response = '" + responseString.Trim() + "' WHERE ReqID = '" + sResult + "' AND ForType = 'MasterWalletDebitApp'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));

            data = convertJsonStringToDataSet(responseString);
            status = data.Tables[0].Rows[0]["response"].ToString();
        }
        catch (Exception ex)
        {
            string sql_res = "UPDATE Tbl_ApiRequest_Response SET Response = '" + ex.Message + "' WHERE ReqID = '" + sResult.Trim() + "' AND ForType = 'MasterWalletDebitApp'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, sql_res));
            status = "failed";
        }

        return status;
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
    public PackageCheckResultS Fund_PackageCondtion_Check()
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

        return new PackageCheckResultS
        {
            Result = sResuldt,
            Msg = sResult
        };
    }
    
}
public class PackageCheckResultS
{
    public string Result { get; set; }
    public string Msg { get; set; }
}
