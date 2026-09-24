using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;

/// <summary>
/// ePay Mobile App API - ek hi endpoint, "reqtype" se kaam decide hota hai.
///
///   POST  /AppApi.aspx
///   Header: Content-Type: application/json
///   Body  : {"reqtype":"home","userid":"...","passwd":"...", ...}
///           userid + passwd har reqtype mein zaroori hain, sirf common list
///           (PublicReqTypes) mein nahi. Har call par sp_Login1 se check hota hai.
///
/// Response hamesha valid JSON (Newtonsoft se banta hai):
///   {"response":"OK|FAILED","code":200,"msg":"","reqtype":"","reqid":"","data":{}}
///
/// Pages ke hisaab se reqtype ki list aur samples: AppApiDoc.html
/// Log: Tbl_AppApiLog + Tbl_AppApiExtCallLog (script: DBScripts/AppApi_Setup.sql)
/// </summary>
public partial class AppApi : System.Web.UI.Page
{
    // ---- Bahar ki services (web pages mein bhi yahi values use ho rahi hain) ----
    private const string MasterWalletUrl = "http://masteradmin.bisplindia.in/DTProcess";
    private const string MasterWalletCompanyId = "120";
    private const string MasterWalletKey = "kgFt9rswQ6hDu3Pm";
    private const string MasterWalletTokenNo = "DF78F4C276874E46BFC151C3065ABE1A";

    private const string AllUpiLoginUrl = "https://allupi.com/api/login";
    private const string AllUpiInitiateUrl = "https://allupi.com/api/InitiateTransactionAsync";
    private const string AllUpiMerchantId = "29159c34-8f20-49d8-a867-4618325f2f74";
    private const string AllUpiSecurityCode = "a0a1649a-a91c-4861-baed-38422f686d6f";
    private const string AllUpiUpiId = "82215511";
    private const string AllUpiServerHookUrl = "https://epayindia.in/Login.aspx";
    private const string SubscriptionWebHookUrl = "https://epayindia.in/PaymentGatewayPurchase.aspx";   // Appsubscription-now jaisa
    private const string MonthlyWebHookUrl = "https://epayindia.in/Paymentgatewayapp.aspx";             // Appmonthly-activation-points jaisa
    private const string PetroWebHookUrl = "https://epayindia.in/PetroCardPaymentGatewayPurchase.aspx"; // PetroCardFinalPurchase jaisa

    // PETROCARDPurchase.aspx: cpanel PAN KYC auto-login link ki key (TripleDES)
    private const string CpanelLoginKey = "sg75b79-nj48dh02";
    private static readonly int[] PetroKitIds = { 12, 13, 14 };

    // Web page -> app screen (home ke url aur drawer menu ke liye)
    private static readonly Dictionary<string, string> ScreenMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "WebApp.aspx", "home" },
        { "Apppurchase-coupon.aspx", "couponlist" },
        { "Appsubscription-now.aspx", "subscription" },
        { "Appmonthly-activation-points.aspx", "monthly" },
        { "DeleteAccount.aspx", "deleteaccount" }
    };

    // Common list (sabke liye same) - inme userid / passwd nahi chahiye
    private static readonly HashSet<string> PublicReqTypes = new HashSet<string> { "couponlist", "monthlypackages" };

    private static readonly HashSet<string> KnownReqTypes = new HashSet<string>
    {
        "login", "logout", "profile", "home", "walletbalance",
        "couponlist", "coupondetail", "couponpurchase", "orderdetail", "redeemurl",
        "subscriptionpackages", "subscriptionpay",
        "monthlypackages", "monthlyactivate", "paymentstatus",
        "deleteaccountcheck", "deleteaccount",
        "purchasehistory", "petrocardreport",
        "petrocardkits", "petrocardform", "petrocardpurchase"
    };

    private AppApiLogger log;
    private JObject req;
    private AppMember member;
    private string reqType = "";
    private string bridgeToken = "";   // home ke web links (AppWebBridge.aspx) ke liye
    private string refNo = "";   // OrderId / BillNo / Debit RefNo - log mein reconciliation ke liye

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        Response.ContentType = "application/json";
        Response.ContentEncoding = Encoding.UTF8;
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();

        log = new AppApiLogger();
        ApiResult result;
        Exception error = null;

        try
        {
            string body = ReadBody();
            req = ParseJson(body);
            reqType = Val("reqtype").ToLower();
            log.Start(reqType, Request, Val("deviceid"), Val("appversion"), body);

            if (Request.HttpMethod != "POST")
                result = Fail(405, "Only POST method is allowed.");
            else if (req == null)
                result = Fail(400, "Invalid JSON request.");
            else if (reqType == "")
                result = Fail(400, "reqtype is required.");
            else if (!KnownReqTypes.Contains(reqType))
                result = Fail(400, "Invalid reqtype.");
            else
                result = Dispatch();
        }
        catch (Exception ex)
        {
            error = ex;
            result = Fail(500, "Something went wrong. Please try again later.");
        }

        JObject output = new JObject();
        output["response"] = result.Ok ? "OK" : "FAILED";
        output["code"] = result.Code;
        output["msg"] = result.Msg ?? "";
        output["reqtype"] = reqType;
        output["reqid"] = log.ReqID;
        output["data"] = result.Data ?? JValue.CreateNull();
        string json = output.ToString(Formatting.None);

        Response.Write(json);

        string logStatus = (error != null || result.InternalError != null) ? "ERROR" : (result.Ok ? "OK" : "FAILED");
        string logError = error != null ? error.Message : (result.InternalError ?? (result.Ok ? null : result.Msg));
        log.End(reqType, member, logStatus, result.Code, json, refNo, logError, error != null ? error.ToString() : null);

        Context.ApplicationInstance.CompleteRequest();
    }

    /// <summary>Page ka HTML render nahi karna - sirf JSON jata hai.</summary>
    protected override void Render(HtmlTextWriter writer)
    {
    }

    private ApiResult Dispatch()
    {
        if (!PublicReqTypes.Contains(reqType))
        {
            if (ClearInject(Val("userid")) == "" || ClearInject(Val("passwd")) == "")
                return Fail(400, "userid and passwd are required.");

            DataRow loginRow = CheckLogin();
            if (loginRow == null)
                return Fail(401, "Please Enter valid UserName or Password.");

            member = MemberFrom(loginRow);
            if (member.IsBlock.ToUpper() == "Y")
                return Fail(403, "This ID is blocked. Please contact the Admin.");

            if (reqType == "login")
                return Login(loginRow);
        }

        switch (reqType)
        {
            case "logout": return Logout();
            case "profile": return Profile_();
            case "home": return Home();
            case "walletbalance": return WalletBalance();
            case "couponlist": return CouponList();
            case "coupondetail": return CouponDetail();
            case "couponpurchase": return CouponPurchase();
            case "orderdetail": return OrderDetail();
            case "redeemurl": return RedeemUrl();
            case "subscriptionpackages": return SubscriptionPackages();
            case "subscriptionpay": return SubscriptionPay();
            case "monthlypackages": return MonthlyPackages();
            case "monthlyactivate": return MonthlyActivate();
            case "paymentstatus": return PaymentStatus();
            case "deleteaccountcheck": return DeleteAccountCheck();
            case "deleteaccount": return DeleteAccount();
            case "purchasehistory": return PurchaseHistory();
            case "petrocardreport": return PetroCardReport();
            case "petrocardkits": return PetroCardKits();
            case "petrocardform": return PetroCardForm();
            case "petrocardpurchase": return PetroCardPurchase();
        }
        return Fail(400, "Invalid reqtype.");
    }

    /* =====================================================================
       AppLogin.aspx  /  AppLogout.aspx
       ===================================================================== */

    /// <summary>AppLogin.aspx -> enterHomePg() jaisa: sp_Login1. Galat ID / password = null.</summary>
    private DataRow CheckLogin()
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec sp_Login1 @Uid, @Pwd",
            new SqlParameter("@Uid", SqlDbType.VarChar, 100) { Value = ClearInject(Val("userid")) },
            new SqlParameter("@Pwd", SqlDbType.VarChar, 100) { Value = ClearInject(Val("passwd")) });

        return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
    }

    private static AppMember MemberFrom(DataRow r)
    {
        AppMember m = new AppMember();
        m.FormNo = AppApiCore.Int(r, "Formno");
        m.IdNo = AppApiCore.Str(r, "IDNo");
        m.FirstName = AppApiCore.Str(r, "MemFirstName");
        m.Name = (m.FirstName + " " + AppApiCore.Str(r, "MemLastName")).Trim();
        m.Mobile = AppApiCore.Str(r, "Mobl");
        m.Email = AppApiCore.Str(r, "Email");
        m.Passw = AppApiCore.Str(r, "Passw");
        m.IsBlock = AppApiCore.Str(r, "IsBlock");
        m.ActiveStatus = AppApiCore.Str(r, "ActiveStatus");
        return m;
    }

    private ApiResult Login(DataRow r)
    {
        JObject m = new JObject();
        m["formno"] = member.FormNo;
        m["idno"] = member.IdNo;
        m["name"] = (AppApiCore.Str(r, "MemFirstName") + " " + AppApiCore.Str(r, "MemLastName")).Trim();
        m["firstname"] = AppApiCore.Str(r, "MemFirstName");
        m["lastname"] = AppApiCore.Str(r, "MemLastName");
        m["mobile"] = AppApiCore.Str(r, "Mobl");
        m["email"] = AppApiCore.Str(r, "Email");
        m["kitid"] = AppApiCore.Int(r, "KitID");
        m["package"] = AppApiCore.Str(r, "KitName");
        m["doj"] = DateStr(r, "Doj");
        m["upgradedate"] = DateStr(r, "Upgradedate");
        m["activationdate"] = DateStr(r, "ActivationDate");
        m["activestatus"] = AppApiCore.Str(r, "ActiveStatus");
        m["profilepic"] = AppApiCore.Str(r, "profilepic");

        JObject data = new JObject();
        data["member"] = m;
        return Success("Login successful.", data);
    }

    /// <summary>App local data saaf kare; server par web links (bridge) ke token band.</summary>
    private ApiResult Logout()
    {
        SqlHelper.ExecuteNonQuery(AppApiCore.Constr, CommandType.Text, "EXEC Sp_AppApi_TokenRevoke @FormNo",
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = member.FormNo });
        return Success("Logout successful.", null);
    }

    /// <summary>
    /// Home ke web links (AppWebBridge.aspx) ke liye 1 din ka token.
    /// Har member ka ek hi active bridge token rehta hai (naya banne par purana band).
    /// </summary>
    private string CreateBridgeToken()
    {
        string token = AppApiCore.NewToken();
        SqlHelper.ExecuteNonQuery(AppApiCore.Constr, CommandType.Text,
            "EXEC Sp_AppApi_TokenCreate @FormNo, @IdNo, @TokenHash, @DeviceId, @IPAddress, @ValidDays",
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = member.FormNo },
            new SqlParameter("@IdNo", SqlDbType.VarChar, 50) { Value = member.IdNo },
            new SqlParameter("@TokenHash", SqlDbType.Char, 64) { Value = AppApiCore.HashToken(token) },
            new SqlParameter("@DeviceId", SqlDbType.NVarChar, 200) { Value = "WEBBRIDGE" },
            new SqlParameter("@IPAddress", SqlDbType.VarChar, 50) { Value = DbNull(AppApiCore.ClientIp(Request)) },
            new SqlParameter("@ValidDays", SqlDbType.Int) { Value = AppApiCore.BridgeTokenValidDays });
        return token;
    }

    /* =====================================================================
       AppMaster.master  -  drawer (naam, ID, balance)
       ===================================================================== */

    private ApiResult Profile_()
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr, CommandType.Text, "EXEC Sp_App_GetMemberProfile @FormNo",
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = member.FormNo });
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return Fail(404, "Member not found.");

        DataRow r = ds.Tables[0].Rows[0];
        string first = AppApiCore.Str(r, "MemFirstName");

        JObject p = new JObject();
        p["formno"] = member.FormNo;
        p["idno"] = AppApiCore.Str(r, "IDNo");
        p["name"] = (first + " " + AppApiCore.Str(r, "MemLastName")).Trim();
        p["firstname"] = first;
        p["initial"] = first.Length > 0 ? first.Substring(0, 1).ToUpper() : "";
        p["mobile"] = AppApiCore.Str(r, "Mobl");
        p["email"] = AppApiCore.Str(r, "Email");
        p["profilepic"] = AppApiCore.Str(r, "ProfilePic");
        p["kitid"] = AppApiCore.Int(r, "KitId");
        p["package"] = AppApiCore.Str(r, "KitName");
        p["doj"] = AppApiCore.Str(r, "Doj");
        p["activationdate"] = AppApiCore.Str(r, "ActivationDate");
        p["activestatus"] = AppApiCore.Str(r, "ActiveStatus");

        JObject data = new JObject();
        data["profile"] = p;
        data["walletbalance"] = GetBalance(member.FormNo);
        return Success("", data);
    }

    /* =====================================================================
       WebApp.aspx  -  home
       ===================================================================== */

    private ApiResult Home()
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr, CommandType.Text, "EXEC Sp_App_GetHomeData");
        bridgeToken = CreateBridgeToken();

        JObject data = new JObject();
        data["banners"] = TableToArray(ds, 0);
        data["services"] = TableToArray(ds, 1);
        data["bestselling"] = TableToArray(ds, 2);
        data["trust"] = TableToArray(ds, 3);
        data["promos"] = TableToArray(ds, 4);
        return Success("", data);
    }

    /// <summary>
    /// Home ki table ko JSON array banata hai. Key = column name lowercase, "_" hata ke
    /// (bg_class -> bgclass). Jahan "url" hai wahan "screen" aur "openurl" bhi jodta hai.
    /// </summary>
    private JArray TableToArray(DataSet ds, int index)
    {
        JArray arr = new JArray();
        if (ds.Tables.Count <= index)
            return arr;

        DataTable dt = ds.Tables[index];
        foreach (DataRow r in dt.Rows)
        {
            JObject o = new JObject();
            foreach (DataColumn c in dt.Columns)
            {
                o[c.ColumnName.ToLower().Replace("_", "")] = ToJValue(r[c]);
            }
            if (dt.Columns.Contains("url"))
            {
                string url = AppApiCore.Str(r, "url");
                o["screen"] = ScreenFor(url);
                o["openurl"] = OpenUrlFor(url);
            }
            arr.Add(o);
        }
        return arr;
    }

    private static JToken ToJValue(object v)
    {
        if (v == null || v == DBNull.Value)
            return "";
        if (v is bool)
            return (bool)v;
        if (v is DateTime)
            return ((DateTime)v).ToString("dd-MMM-yyyy");
        if (v is int || v is long || v is short || v is byte)
            return Convert.ToInt64(v);
        if (v is decimal || v is double || v is float)
            return Convert.ToDecimal(v);
        return Convert.ToString(v).Trim();
    }

    private static string ScreenFor(string url)
    {
        string page = (url ?? "").Split('?')[0].Trim().TrimStart('~', '/');
        string screen;
        return ScreenMap.TryGetValue(page, out screen) ? screen : "";
    }

    /// <summary>
    /// Web link ko app mein kholne wala URL:
    /// http/https link waisa hi; site ka .aspx page AppWebBridge.aspx se (token se session ban jata hai).
    /// </summary>
    private string OpenUrlFor(string url)
    {
        url = (url ?? "").Trim();
        if (url == "" || url.StartsWith("#") || url.StartsWith("javascript", StringComparison.OrdinalIgnoreCase))
            return "";
        if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return url;
        return SiteRoot() + "AppWebBridge.aspx?page=" + HttpUtility.UrlEncode(url.TrimStart('~', '/'))
               + "&token=" + HttpUtility.UrlEncode(bridgeToken);
    }

    private string SiteRoot()
    {
        string scheme = Request.IsLocal ? Request.Url.Scheme : "https";
        return scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
    }

    private ApiResult WalletBalance()
    {
        JObject data = new JObject();
        data["walletbalance"] = GetBalance(member.FormNo);
        return Success("", data);
    }

    /* =====================================================================
       Apppurchase-coupon.aspx  /  Apppurchase-coupon-details.aspx
       ===================================================================== */

    private ApiResult CouponList()
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec Sp_GetAllPageckeDetail");

        JObject data = new JObject();
        data["movies"] = CouponArray(ds, 0, "MOVIE");
        data["food"] = CouponArray(ds, 1, "FOOD");
        return Success("", data);
    }

    private static JArray CouponArray(DataSet ds, int index, string category)
    {
        JArray arr = new JArray();
        if (ds.Tables.Count <= index)
            return arr;

        foreach (DataRow r in ds.Tables[index].Rows)
        {
            JObject o = new JObject();
            o["kitid"] = AppApiCore.Int(r, "kitid");
            o["kitname"] = AppApiCore.Str(r, "kitname");
            o["category"] = category;
            o["discount"] = AppApiCore.Dec(r, "discount");
            o["tickets"] = AppApiCore.Str(r, "Tickets");
            o["couponprice"] = AppApiCore.Dec(r, "couponprice");
            o["kitamount"] = AppApiCore.Dec(r, "kitamount");
            o["saveamount"] = AppApiCore.Dec(r, "saveamount");
            arr.Add(o);
        }
        return arr;
    }

    private ApiResult CouponDetail()
    {
        int kitId;
        if (!TryInt("kitid", out kitId))
            return Fail(400, "kitid is required.");

        DataRow kit = GetCouponKit(kitId);
        if (kit == null)
            return Fail(404, "Package not found.");

        decimal amount = AppApiCore.Dec(kit, "Kitamount");
        decimal balance = GetBalance(member.FormNo);

        JObject data = new JObject();
        data["kitid"] = kitId;
        data["kitname"] = AppApiCore.Str(kit, "kitname");
        data["kitamount"] = amount;
        data["couponprice"] = AppApiCore.Dec(kit, "couponprice");
        data["discount"] = AppApiCore.Dec(kit, "discount");
        data["saveamount"] = AppApiCore.Dec(kit, "saveamount");
        data["tickets"] = AppApiCore.Str(kit, "Tickets");
        data["walletcredit"] = amount;
        data["youpay"] = amount;
        data["walletbalance"] = balance;
        data["canpay"] = balance >= amount;
        data["idno"] = member.IdNo;
        data["name"] = member.Name;

        DataSet dsDis = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec Sp_GetKitDisDetailsNew @KitID",
            new SqlParameter("@KitID", SqlDbType.Int) { Value = kitId });
        DataRow dis = dsDis.Tables.Count > 0 && dsDis.Tables[0].Rows.Count > 0 ? dsDis.Tables[0].Rows[0] : null;
        data["description"] = AppApiCore.Str(dis, "Dis");
        data["howtouse"] = AppApiCore.Str(dis, "Uses");
        data["terms"] = AppApiCore.Str(dis, "Trmscon");

        return Success("", data);
    }

    /// <summary>
    /// Apppurchase-coupon-details.aspx -> BtnProceedToPay_Click ka flow.
    /// Amount hamesha DB se (app se aaya amount use nahi hota).
    /// </summary>
    private ApiResult CouponPurchase()
    {
        int kitId;
        if (!TryInt("kitid", out kitId))
            return Fail(400, "kitid is required.");
        if (IsDuplicateRequest())
            return Fail(409, DuplicateMsg);

        DataRow kit = GetCouponKit(kitId);
        if (kit == null)
            return Fail(404, "Package not found.");

        decimal amount = AppApiCore.Dec(kit, "Kitamount");
        string packageName = AppApiCore.Str(kit, "kitname");
        if (amount <= 0)
            return Fail(400, "Invalid package amount.");

        if (amount > GetBalance(member.FormNo))
            return Fail(400, "Insufficient Balance!!");

        // Package condition (Sp_GetPachageCondtion)
        DataSet dsCond = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec Sp_GetPachageCondtion @FormNo",
            new SqlParameter("@FormNo", SqlDbType.VarChar, 20) { Value = member.FormNo.ToString() });
        if (dsCond.Tables.Count > 0 && dsCond.Tables[0].Rows.Count > 0
            && AppApiCore.Str(dsCond.Tables[0].Rows[0], "Result").ToUpper() == "FAILED")
        {
            return Fail(400, AppApiCore.Str(dsCond.Tables[0].Rows[0], "Msg"));
        }

        if (InsertTrans("Insert into Trnactive (Transid, Rectimestamp) values(@Transid, getdate())") == 0)
            return Fail(500, "Try later.");

        if (MasterWalletBalance() < amount)
            return Fail(400, "Insufficient Balance In Utility Wallet.Please Contact To Admin.!");

        string debitRef = "Master/" + kitId + "/" + member.FormNo + "/" + log.ReqID;
        refNo = debitRef;
        if (MasterWalletDebit(amount, debitRef).ToUpper() != "OK")
            return Fail(400, "Payment could not be processed. Please try again later.");

        string billNo = RandomDigits(6);
        refNo = debitRef + " | Bill:" + billNo;

        string purchaseResult;
        try
        {
            DataSet dsBuy = SqlHelper.ExecuteDataset(AppApiCore.Constr, CommandType.Text,
                "Exec Sp_MMVoucherPurchase @FormNo, @KitId, @Amount, @PackageName, @BillNo",
                new SqlParameter("@FormNo", SqlDbType.Int) { Value = member.FormNo },
                new SqlParameter("@KitId", SqlDbType.Int) { Value = kitId },
                new SqlParameter("@Amount", SqlDbType.Decimal) { Value = amount },
                new SqlParameter("@PackageName", SqlDbType.VarChar, 255) { Value = packageName },
                new SqlParameter("@BillNo", SqlDbType.VarChar, 50) { Value = billNo });
            purchaseResult = dsBuy.Tables.Count > 0 && dsBuy.Tables[0].Rows.Count > 0
                ? AppApiCore.Str(dsBuy.Tables[0].Rows[0], "Result") : "";
        }
        catch (Exception ex)
        {
            purchaseResult = "EXCEPTION: " + ex.Message;
        }

        if (purchaseResult.ToUpper() != "SUCCESS")
        {
            ApiResult failed = Fail(500, "Purchase not successful. Please contact support with reqid.");
            failed.InternalError = "MASTER WALLET DEBITED BUT VOUCHER NOT CREATED (reconcile). DebitRef=" + debitRef
                                   + " BillNo=" + billNo + " Amount=" + amount + " Sp_MMVoucherPurchase=" + purchaseResult;
            return failed;
        }

        JObject data = new JObject();
        data["billno"] = billNo;
        data["kitid"] = kitId;
        data["kitname"] = packageName;
        data["amount"] = amount;
        data["walletbalance"] = GetBalance(member.FormNo);
        data["redeemtype"] = RedeemTypeFor(kitId);
        return Success("Package Purchase Successfully!!", data);
    }

    private DataRow GetCouponKit(int kitId)
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec Sp_GetKitAmount @KitID",
            new SqlParameter("@KitID", SqlDbType.VarChar, 20) { Value = kitId.ToString() });
        return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
    }

    /* =====================================================================
       AppThankYou.aspx  -  order detail + Redeem Now
       ===================================================================== */

    private ApiResult OrderDetail()
    {
        string billNo = ClearInject(Val("billno"));
        if (billNo == "")
            return Fail(400, "billno is required.");

        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec Sp_GetBillNoWiseDetail @BillNo",
            new SqlParameter("@BillNo", SqlDbType.VarChar, 50) { Value = billNo });
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return Fail(404, "Order not found.");

        DataRow r = ds.Tables[0].Rows[0];

        // SP FormNo lautata ho to dusre member ka order na dikhe
        string ownerFormNo = AppApiCore.Str(r, "FormNo");
        if (ownerFormNo != "" && ownerFormNo != member.FormNo.ToString())
            return Fail(404, "Order not found.");

        int kitId = AppApiCore.Int(r, "KitId");
        int reqKitId;
        if (kitId == 0 && TryInt("kitid", out reqKitId))
            kitId = reqKitId;

        JObject data = new JObject();
        data["billno"] = billNo;
        data["ordernumber"] = AppApiCore.Str(r, "OrderNumber");
        data["kitid"] = kitId;
        data["kitname"] = AppApiCore.Str(r, "Kitname");
        data["orderdate"] = DateStr(r, "OrderDate");
        data["amount"] = AppApiCore.Dec(r, "kitamount");
        data["redeemtype"] = RedeemTypeFor(kitId);
        return Success("", data);
    }

    /// <summary>FoodBookingRedirect.aspx / MovieBookingRedirect.aspx ka URL.</summary>
    private ApiResult RedeemUrl()
    {
        string type = Val("type").ToUpper();
        int kitId;
        if (type == "" && TryInt("kitid", out kitId))
            type = RedeemTypeFor(kitId);

        string baseUrl;
        string logKey;
        if (type == "FOOD")
        {
            baseUrl = "https://food.epayindia.in/controller.aspx";
            logKey = "8173842B-3E77-4C5A-AAB8-C9F13562C4BD";
        }
        else if (type == "MOVIE")
        {
            baseUrl = "https://movie.epayindia.in/controller.aspx";
            logKey = "14040576-4E2C-499B-A46B-7192579BA03E";
        }
        else
        {
            return Fail(400, "type (FOOD / MOVIE) or a food/movie kitid is required.");
        }

        string userInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(member.IdNo + ";" + member.Passw));

        JObject data = new JObject();
        data["redeemtype"] = type;
        data["url"] = baseUrl + "?user_info=" + userInfo + "&log_key=" + logKey;
        return Success("", data);
    }

    /// <summary>AppThankYou.aspx -> BtnProceedToPay_Click wali kit range.</summary>
    private static string RedeemTypeFor(int kitId)
    {
        if (kitId >= 1001 && kitId <= 1004)
            return "FOOD";
        if (kitId >= 1005 && kitId <= 1007)
            return "MOVIE";
        return "";
    }

    /* =====================================================================
       Appsubscription-now.aspx
       ===================================================================== */

    private ApiResult SubscriptionPackages()
    {
        DataSet ds = GetSubscriptionPackages();
        if (!AppApiCore.Bool(ds.Tables[0].Rows[0], "MemberOk"))
            return Fail(403, "This ID is blocked. Please contact the Admin.");

        JArray arr = new JArray();
        foreach (DataRow r in ds.Tables[1].Rows)
        {
            JObject o = new JObject();
            o["kitid"] = AppApiCore.Int(r, "KitId");
            o["kitname"] = AppApiCore.Str(r, "KitName");
            o["amount"] = AppApiCore.Dec(r, "JoinAmount");
            o["isallowed"] = AppApiCore.Bool(r, "IsAllowed");
            arr.Add(o);
        }

        JObject data = new JObject();
        data["packages"] = arr;
        return Success("", data);
    }

    private ApiResult SubscriptionPay()
    {
        int kitId;
        if (!TryInt("kitid", out kitId))
            return Fail(400, "kitid is required.");
        if (IsDuplicateRequest())
            return Fail(409, DuplicateMsg);

        DataSet ds = GetSubscriptionPackages();
        if (!AppApiCore.Bool(ds.Tables[0].Rows[0], "MemberOk"))
            return Fail(403, "This ID is blocked. Please contact the Admin.");

        DataRow kit = ds.Tables[1].AsEnumerable().FirstOrDefault(r => AppApiCore.Int(r, "KitId") == kitId);
        if (kit == null || !AppApiCore.Bool(kit, "IsAllowed"))
            return Fail(400, "This package is not available for your ID.");

        decimal amount = AppApiCore.Dec(kit, "JoinAmount");
        return StartGatewayPayment(kitId, AppApiCore.Str(kit, "KitName"), amount, SubscriptionWebHookUrl, "Appsubscription_now");
    }

    private DataSet GetSubscriptionPackages()
    {
        return SqlHelper.ExecuteDataset(AppApiCore.Constr, CommandType.Text, "EXEC Sp_App_GetSubscriptionPackages @FormNo",
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = member.FormNo });
    }

    /* =====================================================================
       Appmonthly-activation-points.aspx
       ===================================================================== */

    private ApiResult MonthlyPackages()
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr, CommandType.Text, "Exec Sp_GetMonthlyDetails");

        JArray arr = new JArray();
        if (ds.Tables.Count > 0)
        {
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                JObject o = new JObject();
                o["kitid"] = AppApiCore.Int(r, "KitId");
                o["kitname"] = AppApiCore.Str(r, "KitName");
                o["amount"] = AppApiCore.Dec(r, "kitamount");
                o["validdays"] = AppApiCore.Int(r, "MonthlyDay");
                o["cashbackpoints"] = AppApiCore.Dec(r, "CashbackPoints");
                arr.Add(o);
            }
        }

        JObject data = new JObject();
        data["packages"] = arr;
        return Success("", data);
    }

    private ApiResult MonthlyActivate()
    {
        int kitId;
        if (!TryInt("kitid", out kitId))
            return Fail(400, "kitid is required.");
        if (IsDuplicateRequest())
            return Fail(409, DuplicateMsg);

        // GetName() - block / inactive check
        DataSet dsMem = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec Sp_GetMemberName @IdNo",
            new SqlParameter("@IdNo", SqlDbType.VarChar, 50) { Value = member.IdNo });
        if (dsMem.Tables.Count == 0 || dsMem.Tables[0].Rows.Count == 0)
            return Fail(404, "Member not found.");
        DataRow mem = dsMem.Tables[0].Rows[0];
        if (AppApiCore.Str(mem, "Isblock").ToUpper() == "Y")
            return Fail(403, "This ID is blocked. Please contact the Admin.");
        if (AppApiCore.Str(mem, "ActiveStatus").ToUpper() == "N")
            return Fail(400, "This Id Not Active Please Active First.!");

        // CheckMonthlyActivation()
        DataSet dsAllow = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec sp_SetMonthlyActivation @FormNo = @FormNo",
            new SqlParameter("@FormNo", SqlDbType.NVarChar, 20) { Value = member.FormNo.ToString() });
        DataRow allow = dsAllow.Tables.Count > 0 && dsAllow.Tables[0].Rows.Count > 0 ? dsAllow.Tables[0].Rows[0] : null;
        if (allow == null || !AppApiCore.Bool(allow, "IsAllowed"))
        {
            string msg = AppApiCore.Str(allow, "Msg");
            return Fail(400, msg != "" ? msg : "Monthly activation is not allowed.");
        }

        DataSet dsPkg = SqlHelper.ExecuteDataset(AppApiCore.Constr, CommandType.Text, "Exec Sp_GetMonthlyDetails");
        DataRow kit = dsPkg.Tables.Count > 0
            ? dsPkg.Tables[0].AsEnumerable().FirstOrDefault(r => AppApiCore.Int(r, "KitId") == kitId)
            : null;
        if (kit == null)
            return Fail(404, "Package not found.");

        decimal amount = AppApiCore.Dec(kit, "kitamount");
        return StartGatewayPayment(kitId, AppApiCore.Str(kit, "KitName"), amount, MonthlyWebHookUrl, "Appmonthly_activation_points");
    }

    /* =====================================================================
       Payment gateway (allupi) - subscription aur monthly dono ke liye
       ===================================================================== */

    /// <summary>
    /// Web page wala flow: Trnjoining -> OnlineTransaction -> allupi login -> LoginTransaction
    /// -> InitiateTransactionAsync -> payment url. Activation webhook page karta hai.
    /// </summary>
    private ApiResult StartGatewayPayment(int kitId, string kitName, decimal amount, string webHookUrl, string pageName)
    {
        if (amount <= 0)
            return Fail(400, "Invalid package amount.");

        if (InsertTrans("Insert into Trnjoining(Transid) values(@Transid)") == 0)
            return Fail(500, "Try Again After Some Time.!");

        string orderId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        refNo = orderId;
        string amountStr = amount.ToString("0.00", CultureInfo.InvariantCulture);

        SqlHelper.ExecuteNonQuery(AppApiCore.Constr, CommandType.Text,
            "INSERT INTO OnlineTransaction(Orderid, Orderdate, Amount, name, kitid, FormNo, idno, ForType) " +
            "VALUES(@Orderid, GETDATE(), @Amount, @Name, @KitId, @FormNo, @IdNo, 'A')",
            new SqlParameter("@Orderid", SqlDbType.VarChar, 50) { Value = orderId },
            new SqlParameter("@Amount", SqlDbType.VarChar, 20) { Value = amountStr },
            new SqlParameter("@Name", SqlDbType.NVarChar, 200) { Value = member.Name },
            new SqlParameter("@KitId", SqlDbType.Int) { Value = kitId },
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = member.FormNo },
            new SqlParameter("@IdNo", SqlDbType.VarChar, 50) { Value = member.IdNo });

        return InitiateUpiPayment(orderId, amount, kitId, kitName, webHookUrl, pageName);
    }

    /// <summary>
    /// allupi login -> LoginTransaction -> InitiateTransactionAsync -> payment url.
    /// Order (OnlineTransaction / PetroOnlineTransaction) pehle ban chuka hona chahiye.
    /// Webhook page LoginTransaction update karke activation / purchase karta hai.
    /// </summary>
    private ApiResult InitiateUpiPayment(string orderId, decimal amount, int kitId, string kitName, string webHookUrl, string pageName)
    {
        string amountStr = amount.ToString("0.00", CultureInfo.InvariantCulture);

        // 1. allupi login
        JObject loginBody = new JObject();
        loginBody["merchantID"] = AllUpiMerchantId;
        loginBody["securityCode"] = AllUpiSecurityCode;
        string loginResp = log.HttpCall("ALLUPI_LOGIN", "POST", AllUpiLoginUrl, loginBody.ToString(Formatting.None), null);

        JObject auth = FindObjectWithKey(ParseJson(loginResp), "token");
        if (auth == null || JStr(auth, "token") == "")
        {
            ApiResult failed = Fail(500, "Payment gateway not available. Please try again later.");
            failed.InternalError = "allupi login response me token nahi mila. OrderId=" + orderId;
            return failed;
        }

        SqlHelper.ExecuteNonQuery(AppApiCore.Constr, CommandType.Text,
            "INSERT INTO LoginTransaction (TId, Username, role, token, refreshToken, name, transactionid, amount, FormNo) " +
            "VALUES (@TId, @Username, @Role, @Token, @RefreshToken, @Name, @TransactionId, @Amount, @FormNo)",
            new SqlParameter("@TId", SqlDbType.NVarChar, 100) { Value = JStr(auth, "ID") },
            new SqlParameter("@Username", SqlDbType.NVarChar, 200) { Value = JStr(auth, "username") },
            new SqlParameter("@Role", SqlDbType.NVarChar, 100) { Value = JStr(auth, "role") },
            new SqlParameter("@Token", SqlDbType.NVarChar, -1) { Value = JStr(auth, "token") },
            new SqlParameter("@RefreshToken", SqlDbType.NVarChar, -1) { Value = JStr(auth, "refreshToken") },
            new SqlParameter("@Name", SqlDbType.NVarChar, 200) { Value = JStr(auth, "name") },
            new SqlParameter("@TransactionId", SqlDbType.VarChar, 50) { Value = orderId },
            new SqlParameter("@Amount", SqlDbType.VarChar, 20) { Value = amountStr },
            new SqlParameter("@FormNo", SqlDbType.VarChar, 20) { Value = member.FormNo.ToString() });

        // 2. Transaction initiate -> payment url
        JObject initBody = new JObject();
        initBody["requestedId"] = orderId;
        initBody["amount"] = amount;
        initBody["upiId"] = AllUpiUpiId;
        initBody["serverHookURL"] = AllUpiServerHookUrl;
        initBody["webHookURL"] = webHookUrl;

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("X-Auth", JStr(auth, "token"));
        string initResp = log.HttpCall("ALLUPI_INITIATE", "POST", AllUpiInitiateUrl, initBody.ToString(Formatting.None), headers);

        JObject initJson = ParseJson(initResp);
        JObject urlObj = FindObjectWithKey(initJson, "url");
        string paymentUrl = urlObj != null ? JStr(urlObj, "url") : "";
        if (paymentUrl == "")
        {
            ApiResult failed = Fail(500, "Payment URL not found. Please try again later.");
            failed.InternalError = "allupi InitiateTransactionAsync response me url nahi mila. OrderId=" + orderId + " Page=" + pageName;
            return failed;
        }

        JObject data = new JObject();
        data["orderid"] = orderId;
        data["kitid"] = kitId;
        data["kitname"] = kitName;
        data["amount"] = amount;
        data["paymenturl"] = paymentUrl;
        return Success("Payment initiated.", data);
    }

    private ApiResult PaymentStatus()
    {
        string orderId = ClearInject(Val("orderid"));
        if (orderId == "")
            return Fail(400, "orderid is required.");

        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr, CommandType.Text, "EXEC Sp_App_GetPaymentStatus @OrderId, @FormNo",
            new SqlParameter("@OrderId", SqlDbType.VarChar, 50) { Value = orderId },
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = member.FormNo });
        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return Fail(404, "Order not found.");

        DataRow r = ds.Tables[0].Rows[0];
        JObject data = new JObject();
        data["orderid"] = orderId;
        data["kitid"] = AppApiCore.Int(r, "KitId");
        data["kitname"] = AppApiCore.Str(r, "KitName");
        data["ordertype"] = AppApiCore.Str(r, "OrderType");
        data["amount"] = AppApiCore.Dec(r, "Amount");
        data["orderdate"] = AppApiCore.Str(r, "OrderDate");
        data["status"] = AppApiCore.Str(r, "Status");
        data["responsedate"] = AppApiCore.Str(r, "ResponseDate");
        return Success("", data);
    }

    /* =====================================================================
       DeleteAccount.aspx  -  hamesha logged-in member ki ID (dusri ID nahi)
       ===================================================================== */

    private ApiResult DeleteAccountCheck()
    {
        DataRow r = CheckDeleteAccount();
        string msg = AppApiCore.Str(r, "Msg");
        if (msg.ToUpper() != "OK")
            return Fail(400, msg != "" ? msg : "Account delete request not allowed.");

        JObject data = new JObject();
        data["idno"] = member.IdNo;
        data["name"] = AppApiCore.Str(r, "MemName");
        return Success("", data);
    }

    private ApiResult DeleteAccount()
    {
        if (IsDuplicateRequest())
            return Fail(409, DuplicateMsg);

        DataRow r = CheckDeleteAccount();
        string msg = AppApiCore.Str(r, "Msg");
        if (msg.ToUpper() != "OK")
            return Fail(400, msg != "" ? msg : "Account delete request not allowed.");

        if (InsertTrans("Insert into Trnactivecadmin (Transid, Rectimestamp) values(@Transid, getdate())") == 0)
            return Fail(500, "Try Later.!");

        refNo = "DeleteAccount/" + member.IdNo;
        SqlHelper.ExecuteNonQuery(AppApiCore.Constr, CommandType.Text,
            "Exec SA_UpdateDeleteAccount @IdNo; " +
            "insert into UserHistory(UserId, UserName, PageName, Activity, ModifiedFlds, RecTimeStamp, MemberId) " +
            "Values (0, '', 'Delete Account Update', 'Delete Account Update (App)', @Remark, Getdate(), @IdNo)",
            new SqlParameter("@IdNo", SqlDbType.VarChar, 50) { Value = member.IdNo },
            new SqlParameter("@Remark", SqlDbType.VarChar, 200) { Value = "Delete Account Update " + member.IdNo });

        return Success("Your account deletion request has been received. Our team will contact you if any further information is required.!", null);
    }

    private DataRow CheckDeleteAccount()
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec CheckDeleteAccount @IdNo",
            new SqlParameter("@IdNo", SqlDbType.VarChar, 50) { Value = member.IdNo });
        return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
    }

    /* =====================================================================
       MyPurchaseDetail.aspx  /  PetroCardPurchaseReport.aspx
       ===================================================================== */

    /// <summary>MyPurchaseDetail.aspx -> FillData(): coupon purchase records (naya pehle).</summary>
    private ApiResult PurchaseHistory()
    {
        DataTable dt = SqlHelper.ExecuteDataset(AppApiCore.Constr, CommandType.Text,
            "SELECT C.billno, C.Repurchincome, A.kitname, C.kitid, " +
            "REPLACE(CONVERT(VARCHAR, C.billdate, 106), ' ', '-') AS billdate " +
            "FROM MM_kitmaster AS A INNER JOIN Repurchincome_MM AS C ON A.kitid = C.kitid " +
            "WHERE C.formno = @FormNo ORDER BY C.rid DESC",
            new SqlParameter("@FormNo", SqlDbType.VarChar, 20) { Value = member.FormNo.ToString() }).Tables[0];

        JArray arr = new JArray();
        foreach (DataRow r in dt.Rows)
        {
            JObject o = new JObject();
            o["billno"] = AppApiCore.Str(r, "billno");
            o["amount"] = AppApiCore.Dec(r, "Repurchincome");
            o["kitid"] = AppApiCore.Int(r, "kitid");
            o["kitname"] = AppApiCore.Str(r, "kitname");
            o["orderdate"] = AppApiCore.Str(r, "billdate");
            o["redeemtype"] = RedeemTypeFor(AppApiCore.Int(r, "kitid"));
            arr.Add(o);
        }
        return Success("", Paged(arr, "records"));
    }

    /// <summary>
    /// PetroCardPurchaseReport.aspx -> FillDetail(): GetPetroCartReportINR.
    /// Web grid SP ke saare columns dikhata hai (AutoGenerateColumns), isliye yahan bhi
    /// "columns" (title + key) aur rows generic jaate hain.
    /// </summary>
    private ApiResult PetroCardReport()
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec GetPetroCartReportINR @FormNo",
            new SqlParameter("@FormNo", SqlDbType.VarChar, 20) { Value = member.FormNo.ToString() });

        JArray columns = new JArray();
        JArray rows = new JArray();
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            List<string> keys = new List<string>();
            foreach (DataColumn c in dt.Columns)
            {
                string key = JsonKey(c.ColumnName);
                string unique = key;
                for (int n = 2; keys.Contains(unique); n++)
                    unique = key + n;
                keys.Add(unique);

                JObject col = new JObject();
                col["key"] = unique;
                col["title"] = c.ColumnName;
                columns.Add(col);
            }
            foreach (DataRow r in dt.Rows)
            {
                JObject o = new JObject();
                for (int i = 0; i < dt.Columns.Count; i++)
                    o[keys[i]] = ToJValue(r[i]);
                rows.Add(o);
            }
        }

        JObject data = Paged(rows, "records");
        data.AddFirst(new JProperty("columns", columns));
        return Success("", data);
    }

    /* =====================================================================
       PETROCARDPurchase.aspx  /  PetroCardFinalPurchase.aspx
       ===================================================================== */

    /// <summary>PETROCARDPurchase.aspx: wallet balance, PAN KYC status aur kits (har kit ka status).</summary>
    private ApiResult PetroCardKits()
    {
        PanKyc kyc = GetPanKyc();
        int purchasedKitId = GetPurchasedPetroKitId();

        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec Sp_getKitPetro @FormNo",
            new SqlParameter("@FormNo", SqlDbType.VarChar, 20) { Value = member.FormNo.ToString() });

        JArray kits = new JArray();
        if (ds.Tables.Count > 0)
        {
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                int kitId = AppApiCore.Int(r, "kitid");

                // rptKitDetails_ItemDataBound wali states
                string status;
                string message;
                if (!kyc.Verified)
                {
                    status = "KYC_REQUIRED";
                    message = "PAN verification is mandatory before purchasing a Petro Card Package.";
                }
                else if (purchasedKitId > 0 && purchasedKitId == kitId)
                {
                    status = "PURCHASED";
                    message = "Already Purchased";
                }
                else if (purchasedKitId > 0)
                {
                    status = "LOCKED";
                    message = "You have already purchased a Petro Card Kit. Only one kit is allowed per member.";
                }
                else
                {
                    status = "AVAILABLE";
                    message = "";
                }

                JObject o = new JObject();
                o["kitid"] = kitId;
                o["kitname"] = AppApiCore.Str(r, "kitdisplayname");
                o["amount"] = AppApiCore.Dec(r, "kitamount");
                o["amountdisplay"] = AppApiCore.Str(r, "kitamountdisp");
                o["image"] = AbsoluteUrl(AppApiCore.Str(r, "img"));
                o["icon"] = AppApiCore.Str(r, "Icon");
                o["theme"] = AppApiCore.Str(r, "Theme");
                o["themelabel"] = AppApiCore.Str(r, "ThemeLabel");
                o["benefits"] = AppApiCore.Str(r, "Benf");
                o["status"] = status;
                o["canbuy"] = status == "AVAILABLE";
                o["message"] = message;
                kits.Add(o);
            }
        }

        JObject data = new JObject();
        data["walletbalance"] = GetBalance(member.FormNo, "S", AppApiCore.Constr);
        data["pankyc"] = PanKycJson(kyc);
        data["purchasedkitid"] = purchasedKitId;
        data["kits"] = kits;
        return Success("", data);
    }

    /// <summary>PetroCardFinalPurchase.aspx Page_Load: checks + form ka data.</summary>
    private ApiResult PetroCardForm()
    {
        int kitId;
        DataRow kit;
        PanKyc kyc;
        ApiResult check = PetroCardChecks(out kitId, out kit, out kyc);
        if (check != null)
            return check;

        DataRow mem = GetPetroMember();
        if (mem == null)
            return Fail(404, "Invalid ID Does Not Exist");

        string email = AppApiCore.Str(mem, "Email");
        string mobile = AppApiCore.Str(mem, "mobl");
        string pan = kyc.PanNo != "" ? kyc.PanNo : AppApiCore.Str(mem, "panno");

        JObject m = new JObject();
        m["idno"] = member.IdNo;
        m["name"] = AppApiCore.Str(mem, "memname");
        m["email"] = email;
        m["emaileditable"] = email == "";
        m["mobile"] = mobile == "0" ? "" : mobile;
        m["mobileeditable"] = mobile == "" || mobile == "0";
        m["panno"] = pan;
        m["paneditable"] = pan == "";

        JObject k = new JObject();
        k["kitid"] = kitId;
        k["kitname"] = AppApiCore.Str(kit, "KitName");
        k["amount"] = AppApiCore.Dec(kit, "KitAmount");

        JArray modes = new JArray();
        modes.Add(new JObject { { "code", "WALLET" }, { "name", "Wallet" } });
        modes.Add(new JObject { { "code", "PG" }, { "name", "Payment Gateway" } });

        JArray wallets = new JArray();
        DataSet dsW = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec Sp_GetWalletTypePetroCardINR");
        if (dsW.Tables.Count > 0)
        {
            foreach (DataRow r in dsW.Tables[0].Rows)
            {
                string code = AppApiCore.Str(r, "Actype");
                if (code == "" || code.ToUpper() == "Z")
                    continue;
                JObject w = new JObject();
                w["code"] = code;
                w["name"] = AppApiCore.Str(r, "WalletName");
                w["balance"] = GetBalance(member.FormNo, code, AppApiCore.Constr1);
                wallets.Add(w);
            }
        }

        JObject data = new JObject();
        data["kit"] = k;
        data["member"] = m;
        data["paymentmodes"] = modes;
        data["wallettypes"] = wallets;
        data["genders"] = CodeList(SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec Sp_getGender"),
                                   "GenderCode", "GenderName");
        data["states"] = CodeList(SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text,
                                   "SELECT StateCode, StateName FROM epayind..M_STateDivMaster WHERE ActiveStatus = 'Y' AND RowStatus = 'Y' ORDER BY StateCode"),
                                   "StateCode", "StateName");
        return Success("", data);
    }

    /// <summary>PetroCardFinalPurchase.aspx -> BtnSubmit_Click: WALLET se seedha purchase, PG se UPI payment url.</summary>
    private ApiResult PetroCardPurchase()
    {
        string mode = Val("paymentmode").ToUpper();
        string walletType = Val("wallettype");
        if (mode != "WALLET" && mode != "PG")
            return Fail(400, "Please Select Payment Mode.!");
        if (mode == "WALLET" && (walletType == "" || walletType.ToUpper() == "Z"))
            return Fail(400, "Please Select Wallet Type.!");

        if (IsDuplicateRequest())
            return Fail(409, DuplicateMsg);

        int kitId;
        DataRow kit;
        PanKyc kyc;
        ApiResult check = PetroCardChecks(out kitId, out kit, out kyc);
        if (check != null)
            return check;

        DataRow mem = GetPetroMember();
        if (mem == null)
            return Fail(404, "Invalid ID Does Not Exist");

        // Web jaisa: DB mein value hai to wahi (field locked), warna app wali
        string dbEmail = AppApiCore.Str(mem, "Email");
        string dbMobile = AppApiCore.Str(mem, "mobl");
        string dbPan = AppApiCore.Str(mem, "panno");
        string name = AppApiCore.Str(mem, "memname");
        string email = dbEmail != "" ? dbEmail : Val("email");
        string mobile = dbMobile != "" && dbMobile != "0" ? dbMobile : Val("mobile");
        string pan = (kyc.PanNo != "" ? kyc.PanNo : (dbPan != "" ? dbPan : Val("panno"))).ToUpper();
        string whatsapp = Val("whatsappno");
        string gender = Val("gender");
        string dob = Val("dob");
        string address = Val("address");
        string pincode = Val("pincode");
        string stateCode = Val("statecode");
        string city = Val("city");
        string district = Val("district");

        DateTime dobDate;
        if (gender == "" || gender.ToUpper() == "Z")
            return Fail(400, "Please Select Gender.!");
        if (email == "")
            return Fail(400, "Please Enter Email Id.!");
        if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            return Fail(400, "Please Enter valid Email ID.!");
        if (!System.Text.RegularExpressions.Regex.IsMatch(mobile, @"^[1-9]\d{9}$"))
            return Fail(400, "Please Enter valid mobile Number.!");
        if (!System.Text.RegularExpressions.Regex.IsMatch(whatsapp, @"^[1-9]\d{9}$"))
            return Fail(400, "Please Enter valid Whatsapp Number.!");
        if (!System.Text.RegularExpressions.Regex.IsMatch(pan, @"^[A-Z]{5}[0-9]{4}[A-Z]$"))
            return Fail(400, "Please Enter valid Pan No.!");
        if (!DateTime.TryParseExact(dob, "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dobDate))
            return Fail(400, "Please Enter Date Of Birth (dd-MMM-yyyy).!");
        if (city == "")
            return Fail(400, "Please Enter City.!");
        if (district == "")
            return Fail(400, "Please Enter District.!");

        DataSet dsState = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text,
            "SELECT StateName FROM epayind..M_STateDivMaster WHERE StateCode = @StateCode AND ActiveStatus = 'Y' AND RowStatus = 'Y'",
            new SqlParameter("@StateCode", SqlDbType.VarChar, 20) { Value = stateCode });
        if (stateCode == "" || stateCode == "0" || dsState.Tables[0].Rows.Count == 0)
            return Fail(400, "Please Select State.!");
        string stateName = AppApiCore.Str(dsState.Tables[0].Rows[0], "StateName");

        decimal amount = AppApiCore.Dec(kit, "KitAmount");
        string kitName = AppApiCore.Str(kit, "KitName");
        string amountStr = amount.ToString("0.00", CultureInfo.InvariantCulture);
        if (amount <= 0)
            return Fail(400, "Invalid package amount.");

        if (mode == "WALLET" && GetBalance(member.FormNo, walletType, AppApiCore.Constr) < amount)
            return Fail(400, "You do not have enough balance for Purchasing.");

        if (InsertTrans("Insert into Trnactive (Transid, Rectimestamp) values(@Transid, getdate())") == 0)
            return Fail(500, "Try Again After Some Time.!");

        if (mode == "PG")
        {
            string orderId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            refNo = orderId;
            SqlHelper.ExecuteNonQuery(AppApiCore.Constr, CommandType.Text,
                "INSERT INTO PetroOnlineTransaction(Orderid, Orderdate, Amount, FormNo, Kitid, memberID, Name, Mobl, WhatsappNo, Email, Panno, dob, Address1, Pincode, City, District, Statename, StateCode, Gender) " +
                "VALUES(@Orderid, GETDATE(), @Amount, @FormNo, @KitId, @MemberId, @Name, @Mobl, @Whatsapp, @Email, @Pan, @Dob, @Address, @Pincode, @City, @District, @StateName, @StateCode, @Gender)",
                new SqlParameter("@Orderid", SqlDbType.VarChar, 50) { Value = orderId },
                new SqlParameter("@Amount", SqlDbType.VarChar, 20) { Value = amountStr },
                new SqlParameter("@FormNo", SqlDbType.VarChar, 20) { Value = member.FormNo.ToString() },
                new SqlParameter("@KitId", SqlDbType.VarChar, 10) { Value = kitId.ToString() },
                new SqlParameter("@MemberId", SqlDbType.VarChar, 50) { Value = member.IdNo },
                new SqlParameter("@Name", SqlDbType.NVarChar, 200) { Value = name },
                new SqlParameter("@Mobl", SqlDbType.VarChar, 20) { Value = mobile },
                new SqlParameter("@Whatsapp", SqlDbType.VarChar, 20) { Value = whatsapp },
                new SqlParameter("@Email", SqlDbType.VarChar, 200) { Value = email },
                new SqlParameter("@Pan", SqlDbType.VarChar, 20) { Value = pan },
                new SqlParameter("@Dob", SqlDbType.VarChar, 20) { Value = dob },
                new SqlParameter("@Address", SqlDbType.NVarChar, 500) { Value = address },
                new SqlParameter("@Pincode", SqlDbType.VarChar, 10) { Value = pincode },
                new SqlParameter("@City", SqlDbType.NVarChar, 100) { Value = city },
                new SqlParameter("@District", SqlDbType.NVarChar, 100) { Value = district },
                new SqlParameter("@StateName", SqlDbType.NVarChar, 100) { Value = stateName },
                new SqlParameter("@StateCode", SqlDbType.VarChar, 20) { Value = stateCode },
                new SqlParameter("@Gender", SqlDbType.VarChar, 10) { Value = gender });

            return InitiateUpiPayment(orderId, amount, kitId, kitName, PetroWebHookUrl, "PetroCardFinalPurchase");
        }

        // WALLET -> FUNPETROCARDPurchase()
        string billNo = RandomDigits(6);
        refNo = refNo + " | Bill:" + billNo;
        string result;
        try
        {
            DataSet dsBuy = SqlHelper.ExecuteDataset(AppApiCore.Constr, CommandType.Text,
                "EXEC Sp_PaymentPetroCardINR @IdNo, @KitId, '', 'USDT', @FormNo, @Amount, @BillNo, @Name, @Email, @Mobl, @Pan, @Dob, @Address, @Pincode, @StateName, @City, @District, @Whatsapp, @WalletType, @Gender",
                new SqlParameter("@IdNo", SqlDbType.VarChar, 50) { Value = member.IdNo },
                new SqlParameter("@KitId", SqlDbType.VarChar, 10) { Value = kitId.ToString() },
                new SqlParameter("@FormNo", SqlDbType.Int) { Value = member.FormNo },
                new SqlParameter("@Amount", SqlDbType.VarChar, 20) { Value = amountStr },
                new SqlParameter("@BillNo", SqlDbType.VarChar, 50) { Value = billNo },
                new SqlParameter("@Name", SqlDbType.NVarChar, 200) { Value = name },
                new SqlParameter("@Email", SqlDbType.VarChar, 200) { Value = email },
                new SqlParameter("@Mobl", SqlDbType.VarChar, 20) { Value = mobile },
                new SqlParameter("@Pan", SqlDbType.VarChar, 20) { Value = pan },
                new SqlParameter("@Dob", SqlDbType.VarChar, 20) { Value = dob },
                new SqlParameter("@Address", SqlDbType.NVarChar, 500) { Value = address },
                new SqlParameter("@Pincode", SqlDbType.VarChar, 10) { Value = pincode },
                new SqlParameter("@StateName", SqlDbType.NVarChar, 100) { Value = stateName },
                new SqlParameter("@City", SqlDbType.NVarChar, 100) { Value = city },
                new SqlParameter("@District", SqlDbType.NVarChar, 100) { Value = district },
                new SqlParameter("@Whatsapp", SqlDbType.VarChar, 20) { Value = whatsapp },
                new SqlParameter("@WalletType", SqlDbType.VarChar, 10) { Value = walletType },
                new SqlParameter("@Gender", SqlDbType.VarChar, 10) { Value = gender });
            result = dsBuy.Tables.Count > 0 && dsBuy.Tables[0].Rows.Count > 0 ? AppApiCore.Str(dsBuy.Tables[0].Rows[0], "Result") : "";
        }
        catch (Exception ex)
        {
            result = "EXCEPTION: " + ex.Message;
        }

        if (result.ToUpper() != "SUCCESS")
        {
            ApiResult failed = Fail(400, "Petro Card Purchase Not Successfully!!");
            failed.InternalError = "Sp_PaymentPetroCardINR=" + result + " BillNo=" + billNo + " KitId=" + kitId;
            return failed;
        }

        JObject data = new JObject();
        data["paymentmode"] = "WALLET";
        data["billno"] = billNo;
        data["kitid"] = kitId;
        data["kitname"] = kitName;
        data["amount"] = amount;
        data["walletbalance"] = GetBalance(member.FormNo, walletType, AppApiCore.Constr);
        return Success("Petro Card Purchase Successfully.!", data);
    }

    /// <summary>Kit valid (12/13/14), PAN verified, pehle koi petro kit nahi liya. Sab theek = null.</summary>
    private ApiResult PetroCardChecks(out int kitId, out DataRow kit, out PanKyc kyc)
    {
        kit = null;
        kyc = null;
        if (!TryInt("kitid", out kitId) || !PetroKitIds.Contains(kitId))
            return Fail(400, "Invalid Petro Card kit.");

        kyc = GetPanKyc();
        if (!kyc.Verified)
            return Fail(400, "PAN verification is mandatory before purchasing a Petro Card Kit.");

        if (GetPurchasedPetroKitId() > 0)
            return Fail(400, "You have already purchased a Petro Card Kit. Only one kit is allowed per member.");

        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text,
            "SELECT KitAmount, KitName FROM epayind..M_kitmaster WHERE kitid = @KitId",
            new SqlParameter("@KitId", SqlDbType.Int) { Value = kitId });
        if (ds.Tables[0].Rows.Count == 0)
            return Fail(404, "Package not found.");
        kit = ds.Tables[0].Rows[0];
        return null;
    }

    private class PanKyc
    {
        public string Status = "NOTSUBMITTED";
        public bool Verified;
        public string PanNo = "";
    }

    /// <summary>USP_GetPanKycStatus: VERIFIED / PENDING / REJECTED / NOTSUBMITTED.</summary>
    private PanKyc GetPanKyc()
    {
        PanKyc kyc = new PanKyc();
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec USP_GetPanKycStatus @FormNo",
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = member.FormNo });
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow r = ds.Tables[0].Rows[0];
            kyc.Status = AppApiCore.Str(r, "StatusText").ToUpper();
            if (kyc.Status == "")
                kyc.Status = "NOTSUBMITTED";
            kyc.Verified = r.Table.Columns.Contains("IsVerified") ? AppApiCore.Int(r, "IsVerified") == 1 : kyc.Status == "VERIFIED";
            kyc.PanNo = AppApiCore.Str(r, "PanNo").ToUpper();
        }
        return kyc;
    }

    /// <summary>PETROCARDPurchase.aspx -> LoadPanKycStatus() ke banner texts + cpanel KYC link.</summary>
    private JObject PanKycJson(PanKyc kyc)
    {
        string title, message, button;
        switch (kyc.Status)
        {
            case "VERIFIED":
                title = "PAN Verified";
                message = "Your PAN verification is complete. You can proceed with your Petro Card Package purchase.";
                button = "";
                break;
            case "REJECTED":
                title = "PAN Verification Rejected";
                message = "Your PAN details were rejected during verification. Please re-submit correct PAN details from your account to continue.";
                button = "Re-submit PAN";
                break;
            case "PENDING":
                title = "PAN Verification Pending";
                message = "Your PAN details have been submitted and are under review. You can purchase a Petro Card Package once the verification is approved.";
                button = "View KYC Status";
                break;
            default:
                title = "PAN Verification Required";
                message = "PAN verification is mandatory before purchasing a Petro Card Package. Please complete your PAN KYC from your account.";
                button = "Verify PAN Now";
                break;
        }

        JObject o = new JObject();
        o["status"] = kyc.Status;
        o["verified"] = kyc.Verified;
        o["title"] = title;
        o["message"] = message;
        o["buttontext"] = button;
        o["kycurl"] = kyc.Verified ? "" : CpanelKycUrl();
        return o;
    }

    /// <summary>Web jaisa cpanel auto-login link (PAN KYC page). ID mein ghante ka code hai, isliye har baar naya lo.</summary>
    private string CpanelKycUrl()
    {
        string lgnT;
        using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            des.Key = md5.ComputeHash(Encoding.ASCII.GetBytes(CpanelLoginKey));
            des.Mode = CipherMode.ECB;
            byte[] buffer = Encoding.ASCII.GetBytes("uid=" + member.IdNo + "&pwd=" + member.Passw);
            lgnT = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }

        DateTime now = DateTime.Now;
        string id = now.Day.ToString() + (now.Hour - 1).ToString() + now.Year.ToString() + (now.Month - 1).ToString();
        return "https://cpanel.epayindia.in/Default.aspx?lgnT=" + lgnT + "&ID=" + id + "&RedirectTo=PANKYC";
    }

    /// <summary>Member ne pehle se koi Petro kit (12/13/14) liya hai to uska kitid, warna 0.</summary>
    private int GetPurchasedPetroKitId()
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text,
            "SELECT TOP 1 kitid FROM epayind..repurchincome WHERE formno = @FormNo AND kitid IN (12, 13, 14) ORDER BY kitid",
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = member.FormNo });
        return ds.Tables[0].Rows.Count > 0 ? AppApiCore.Int(ds.Tables[0].Rows[0], "kitid") : 0;
    }

    /// <summary>Sp_GetMemberNamer: naam, email, mobile, pan (form prefill).</summary>
    private DataRow GetPetroMember()
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Exec Sp_GetMemberNamer @IdNo",
            new SqlParameter("@IdNo", SqlDbType.VarChar, 50) { Value = member.IdNo });
        return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0] : null;
    }

    /// <summary>Dropdown list -> [{code, name}], "--Select--" (Z / 0) hata ke.</summary>
    private static JArray CodeList(DataSet ds, string codeCol, string nameCol)
    {
        JArray arr = new JArray();
        if (ds.Tables.Count == 0)
            return arr;
        foreach (DataRow r in ds.Tables[0].Rows)
        {
            string code = AppApiCore.Str(r, codeCol);
            if (code == "" || code == "0" || code.ToUpper() == "Z")
                continue;
            arr.Add(new JObject { { "code", code }, { "name", AppApiCore.Str(r, nameCol) } });
        }
        return arr;
    }

    private string AbsoluteUrl(string path)
    {
        if (string.IsNullOrEmpty(path) || path.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return path;
        return SiteRoot() + path.TrimStart('~', '/');
    }

    /// <summary>"Order No." -> "orderno" (sirf a-z 0-9).</summary>
    private static string JsonKey(string columnName)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char ch in (columnName ?? "").ToLowerInvariant())
        {
            if ((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9'))
                sb.Append(ch);
        }
        return sb.Length > 0 ? sb.ToString() : "col";
    }

    /// <summary>
    /// Optional paging: "page" (1 se) aur "pagesize" (default 20, max 100).
    /// page na bheja ho to saare records.
    /// </summary>
    private JObject Paged(JArray all, string name)
    {
        int page;
        int pageSize;
        if (!TryInt("pagesize", out pageSize))
            pageSize = 20;
        pageSize = Math.Min(pageSize, 100);

        JObject data = new JObject();
        data["total"] = all.Count;
        if (TryInt("page", out page))
        {
            data["page"] = page;
            data["pagesize"] = pageSize;
            data["totalpages"] = (all.Count + pageSize - 1) / pageSize;
            data[name] = new JArray(all.Skip((page - 1) * pageSize).Take(pageSize));
        }
        else
        {
            data[name] = all;
        }
        return data;
    }

    /* =====================================================================
       Wallet (member + master utility wallet)
       ===================================================================== */

    private static decimal GetBalance(int formNo)
    {
        return GetBalance(formNo, "B", AppApiCore.Constr1);
    }

    /// <summary>ufnGetBalance(formno, wallet type). Petro Card pages 'S' / Actype wallet use karte hain.</summary>
    private static decimal GetBalance(int formNo, string walletType, string connection)
    {
        DataSet ds = SqlHelper.ExecuteDataset(connection, CommandType.Text, "Select Balance From dbo.ufnGetBalance(@FormNo, @Type)",
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = formNo },
            new SqlParameter("@Type", SqlDbType.VarChar, 10) { Value = walletType });
        return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? AppApiCore.Dec(ds.Tables[0].Rows[0], "Balance") : 0;
    }

    /// <summary>Fund_Balance_Check() - company ka master utility wallet balance.</summary>
    private decimal MasterWalletBalance()
    {
        JObject body = new JObject();
        body["reqtype"] = "getwalletbalance";
        body["companyid"] = MasterWalletCompanyId;
        body["actype"] = "M";
        body["key"] = MasterWalletKey;

        try
        {
            string resp = log.HttpCall("MASTER_BALANCE", "POST", MasterWalletUrl, body.ToString(Formatting.None), null);
            JObject obj = FindObjectWithKey(ParseJson(resp), "Balance");
            decimal bal;
            return obj != null && decimal.TryParse(JStr(obj, "Balance"), NumberStyles.Any, CultureInfo.InvariantCulture, out bal) ? bal : 0;
        }
        catch
        {
            return 0;   // web page jaisa: error = 0 balance (call already ext log mein hai)
        }
    }

    /// <summary>Fun_SameMasterDebitEntrty() - "OK" mile to debit hua.</summary>
    private string MasterWalletDebit(decimal amount, string debitRef)
    {
        JObject body = new JObject();
        body["reqtype"] = "debitamount";
        body["tokenno"] = MasterWalletTokenNo;
        body["id"] = MasterWalletCompanyId;
        body["amount"] = amount.ToString(CultureInfo.InvariantCulture);
        body["narration"] = "Amount Deduction By Id Activation of IdNo: " + member.IdNo;
        body["refno"] = debitRef;
        body["vtype"] = "D";
        body["actype"] = "M";
        body["useactype"] = "M";
        body["key"] = MasterWalletKey;

        try
        {
            string resp = log.HttpCall("MASTER_DEBIT", "POST", MasterWalletUrl, body.ToString(Formatting.None), null);
            JObject obj = FindObjectWithKey(ParseJson(resp), "response");
            return obj != null ? JStr(obj, "response") : "";
        }
        catch
        {
            return "failed";
        }
    }

    /* =====================================================================
       Helpers
       ===================================================================== */

    private string ReadBody()
    {
        // Purani apps "MyJson" form field bhejti hain (ProccessApiWithK jaisa) - woh bhi chalega
        if (Request.Form["MyJson"] != null)
            return Request.Form["MyJson"];

        Request.InputStream.Position = 0;
        using (StreamReader reader = new StreamReader(Request.InputStream, Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }

    private static JObject ParseJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;
        try
        {
            return JObject.Parse(json);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>Request body ki value (key case-insensitive), na ho to "".</summary>
    private string Val(string key)
    {
        if (req == null)
            return "";
        JToken t = req.GetValue(key, StringComparison.OrdinalIgnoreCase);
        if (t == null || t.Type == JTokenType.Null)
            return "";
        return t.ToString().Trim();
    }

    private bool TryInt(string key, out int value)
    {
        return int.TryParse(Val(key), NumberStyles.Integer, CultureInfo.InvariantCulture, out value) && value > 0;
    }

    /// <summary>JSON mein (kisi bhi level par) pehla object jisme ye key ho.</summary>
    private static JObject FindObjectWithKey(JToken token, string key)
    {
        if (token == null)
            return null;

        JObject obj = token as JObject;
        if (obj != null)
        {
            if (obj.GetValue(key, StringComparison.OrdinalIgnoreCase) != null)
                return obj;
            foreach (JProperty p in obj.Properties())
            {
                JObject found = FindObjectWithKey(p.Value, key);
                if (found != null)
                    return found;
            }
        }

        JArray arr = token as JArray;
        if (arr != null)
        {
            foreach (JToken item in arr)
            {
                JObject found = FindObjectWithKey(item, key);
                if (found != null)
                    return found;
            }
        }
        return null;
    }

    private static string JStr(JObject obj, string key)
    {
        JToken t = obj == null ? null : obj.GetValue(key, StringComparison.OrdinalIgnoreCase);
        return t == null || t.Type == JTokenType.Null ? "" : t.ToString().Trim();
    }

    /// <summary>
    /// Trnactive / Trnjoining / Trnactivecadmin mein server ka banaya Transid (9 digit random).
    /// Purane record se takra jaye to naye number se dobara (3 baar). Transid lautata hai, fail = 0.
    /// </summary>
    private int InsertTrans(string sql)
    {
        Random rdm = new Random();
        for (int attempt = 0; attempt < 3; attempt++)
        {
            int transId = rdm.Next(100000000, 1000000000);
            try
            {
                if (SqlHelper.ExecuteNonQuery(AppApiCore.Constr, CommandType.Text, sql,
                        new SqlParameter("@Transid", SqlDbType.Int) { Value = transId }) > 0)
                {
                    refNo = string.IsNullOrEmpty(refNo) ? "Trans:" + transId : refNo + " | Trans:" + transId;
                    return transId;
                }
            }
            catch (SqlException)
            {
            }
        }
        return 0;
    }

    private const string DuplicateMsg = "Your previous request is in process. Please wait a moment and try again.";

    /// <summary>
    /// Double tap / retry se bachav: same member ki same reqtype pichhle 20 second mein
    /// chal rahi ho (STARTED) ya ho chuki ho (OK) to true. (Sp_AppApi_DuplicateGuard)
    /// </summary>
    private bool IsDuplicateRequest()
    {
        if (log.LogId <= 0)
            return false;   // log row hi nahi bani to check nahi ho sakta

        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr, CommandType.Text,
            "EXEC Sp_AppApi_DuplicateGuard @LogId, @FormNo, @IdNo, @ReqType, @Seconds",
            new SqlParameter("@LogId", SqlDbType.BigInt) { Value = log.LogId },
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = member.FormNo },
            new SqlParameter("@IdNo", SqlDbType.VarChar, 50) { Value = member.IdNo },
            new SqlParameter("@ReqType", SqlDbType.VarChar, 50) { Value = reqType },
            new SqlParameter("@Seconds", SqlDbType.Int) { Value = 20 });
        return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && AppApiCore.Int(ds.Tables[0].Rows[0], "Pending") > 0;
    }

    private static string ClearInject(string s)
    {
        return (s ?? "").Replace(";", "").Replace("'", "").Replace("=", "").Trim();
    }

    private static string DateStr(DataRow r, string col)
    {
        if (r == null || !r.Table.Columns.Contains(col) || r[col] == DBNull.Value)
            return "";
        if (r[col] is DateTime)
            return ((DateTime)r[col]).ToString("dd-MMM-yyyy");
        return Convert.ToString(r[col]).Trim();
    }

    private static string RandomDigits(int length)
    {
        Random rdm = new Random();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            sb.Append(rdm.Next(1, 10));
        }
        return sb.ToString();
    }

    private static object DbNull(string v)
    {
        return string.IsNullOrEmpty(v) ? (object)DBNull.Value : v;
    }

    private static ApiResult Success(string msg, JToken data)
    {
        return new ApiResult { Ok = true, Code = 200, Msg = msg, Data = data };
    }

    private static ApiResult Fail(int code, string msg)
    {
        return new ApiResult { Ok = false, Code = code, Msg = msg };
    }

    private class ApiResult
    {
        public bool Ok;
        public int Code;
        public string Msg;
        public JToken Data;
        public string InternalError;   // sirf log mein jata hai, app ko nahi
    }
}
