using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
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
        "deleteaccountcheck", "deleteaccount"
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
            case "profile": return Profile();
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
       AppMaster.master  -  drawer (naam, ID, menu)
       ===================================================================== */

    private ApiResult Profile()
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

        JArray menu = new JArray();
        menu.Add(MenuItem("Home", "fa fa-home", "home", "home"));
        menu.Add(MenuItem("Purchase Coupon", "fa fa-ticket-alt", "couponlist", "couponlist"));
        menu.Add(MenuItem("Subscription Now", "fa fa-crown", "subscription", "subscriptionpackages"));
        menu.Add(MenuItem("Activation Points", "fa fa-chart-bar", "monthly", "monthlypackages"));
        menu.Add(MenuItem("Account Delete", "fa fa-globe", "deleteaccount", "deleteaccountcheck"));
        menu.Add(MenuItem("Logout", "fa fa-sign-out-alt", "logout", "logout"));

        JObject data = new JObject();
        data["profile"] = p;
        data["walletbalance"] = GetBalance(member.FormNo);
        data["menu"] = menu;
        return Success("", data);
    }

    private static JObject MenuItem(string title, string icon, string screen, string reqtype)
    {
        JObject o = new JObject();
        o["title"] = title;
        o["icon"] = icon;
        o["screen"] = screen;
        o["reqtype"] = reqtype;
        return o;
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
        int transId;
        if (!TryInt("kitid", out kitId))
            return Fail(400, "kitid is required.");
        if (!TryInt("transid", out transId))
            return Fail(400, "transid (numeric, unique per attempt) is required.");

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

        // Double click / duplicate request guard
        if (!InsertTrans("Insert into Trnactive (Transid, Rectimestamp) values(@Transid, getdate())", transId))
            return Fail(409, "Duplicate request. Please try again.");

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
        if (AppApiCore.Int(ds.Tables[0].Rows[0], "MemberOk") != 1)
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
        int transId;
        if (!TryInt("kitid", out kitId))
            return Fail(400, "kitid is required.");
        if (!TryInt("transid", out transId))
            return Fail(400, "transid (numeric, unique per attempt) is required.");

        DataSet ds = GetSubscriptionPackages();
        if (AppApiCore.Int(ds.Tables[0].Rows[0], "MemberOk") != 1)
            return Fail(403, "This ID is blocked. Please contact the Admin.");

        DataRow kit = ds.Tables[1].AsEnumerable().FirstOrDefault(r => AppApiCore.Int(r, "KitId") == kitId);
        if (kit == null || !AppApiCore.Bool(kit, "IsAllowed"))
            return Fail(400, "This package is not available for your ID.");

        decimal amount = AppApiCore.Dec(kit, "JoinAmount");
        return StartGatewayPayment(kitId, AppApiCore.Str(kit, "KitName"), amount, transId, SubscriptionWebHookUrl, "Appsubscription_now");
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
        int transId;
        if (!TryInt("kitid", out kitId))
            return Fail(400, "kitid is required.");
        if (!TryInt("transid", out transId))
            return Fail(400, "transid (numeric, unique per attempt) is required.");

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
        return StartGatewayPayment(kitId, AppApiCore.Str(kit, "KitName"), amount, transId, MonthlyWebHookUrl, "Appmonthly_activation_points");
    }

    /* =====================================================================
       Payment gateway (allupi) - subscription aur monthly dono ke liye
       ===================================================================== */

    /// <summary>
    /// Web page wala flow: Trnjoining -> OnlineTransaction -> allupi login -> LoginTransaction
    /// -> InitiateTransactionAsync -> payment url. Activation webhook page karta hai.
    /// </summary>
    private ApiResult StartGatewayPayment(int kitId, string kitName, decimal amount, int transId, string webHookUrl, string pageName)
    {
        if (amount <= 0)
            return Fail(400, "Invalid package amount.");

        if (!InsertTrans("Insert into Trnjoining(Transid) values(@Transid)", transId))
            return Fail(409, "Try Again After Some Time.!");

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
        int transId;
        if (!TryInt("transid", out transId))
            return Fail(400, "transid (numeric, unique per attempt) is required.");

        DataRow r = CheckDeleteAccount();
        string msg = AppApiCore.Str(r, "Msg");
        if (msg.ToUpper() != "OK")
            return Fail(400, msg != "" ? msg : "Account delete request not allowed.");

        if (!InsertTrans("Insert into Trnactivecadmin (Transid, Rectimestamp) values(@Transid, getdate())", transId))
            return Fail(409, "Try Later.!");

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
       Wallet (member + master utility wallet)
       ===================================================================== */

    private static decimal GetBalance(int formNo)
    {
        DataSet ds = SqlHelper.ExecuteDataset(AppApiCore.Constr1, CommandType.Text, "Select Balance From dbo.ufnGetBalance(@FormNo, 'B')",
            new SqlParameter("@FormNo", SqlDbType.Int) { Value = formNo });
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

    /// <summary>Duplicate guard table (Trnactive / Trnjoining / Trnactivecadmin) - insert fail = duplicate.</summary>
    private static bool InsertTrans(string sql, int transId)
    {
        try
        {
            return SqlHelper.ExecuteNonQuery(AppApiCore.Constr, CommandType.Text, sql,
                new SqlParameter("@Transid", SqlDbType.Int) { Value = transId }) > 0;
        }
        catch (SqlException)
        {
            return false;
        }
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
