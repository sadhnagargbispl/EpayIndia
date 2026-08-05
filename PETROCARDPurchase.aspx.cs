using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PETROCARDPurchase : System.Web.UI.Page
{
    private string query;
    private DataTable Dt = new DataTable();
    DAL objDal = new DAL();

    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private static readonly TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
    private static readonly MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
    private static readonly string key = "sg75b79-nj48dh02";
    // ── State flags ──
    private int _purchasedKitId = 0;
    private decimal _walletBalance = 0;
    private bool _isPanVerified = false;
    private string _kycUrl = ConfigurationManager.AppSettings["KycPageUrl"] ?? "KycUpload.aspx";

    #region ── Page events ──

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] == null || Session["Status"].ToString() != "OK")
            {
                Response.Redirect("login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                HdnCheckTrnns.Value = GenerateRandomStringActive(6);

                FundWalletGetBalance();   // 1. balance
                LoadPanKycStatus();       // 2. PAN status
                FillKit();                // 3. cards
            }
        }
        catch (Exception ex)
        {
            objDal.WriteToFile("PetroPurchase Page_Load: " + ex.Message);
            ShowError("Something went wrong. Please refresh the page.");
        }
    }

    #endregion

    #region ── Wallet balance ──

    protected void FundWalletGetBalance()
    {
        try
        {
            string str = " Select * From dbo.ufnGetBalance('"
                       + Convert.ToInt32(Session["Formno"]) + "','S')";

            DataTable dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];

            HdnWalletBalance.Value = (dt.Rows.Count > 0 && dt.Rows[0]["Balance"] != DBNull.Value)
                                     ? Convert.ToString(dt.Rows[0]["Balance"])
                                     : "0";

            Session["ServiceWallet"] = HdnWalletBalance.Value;

            decimal.TryParse(HdnWalletBalance.Value, out _walletBalance);
            lblWalletBalance.Text = "Rs. " + _walletBalance.ToString("N2");
        }
        catch (Exception ex)
        {
            _walletBalance = 0;
            HdnWalletBalance.Value = "0";
            lblWalletBalance.Text = "Rs. 0.00";
            objDal.WriteToFile("FundWalletGetBalance: " + ex.Message);
        }
    }

    #endregion

    #region ── PAN KYC status ──

    /// <summary>KycVerify table se PAN status check karke banner set karta hai</summary>
    private void LoadPanKycStatus()
    {
        try
        {
            string sql = "Exec USP_GetPanKycStatus " + Convert.ToInt32(Session["Formno"]);
            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];

            string status = dt.Rows.Count > 0
                            ? Convert.ToString(dt.Rows[0]["StatusText"])
                            : "NOTSUBMITTED";

            _isPanVerified = (status == "VERIFIED");
            Session["PanVerified"] = _isPanVerified ? "Y" : "N";

            pnlKyc.Visible = true;
            string lgnT = Encrypt("uid=" + Session["IDNO"] + "&pwd=" + Session["MemPassw"]);

            string result = DateTime.Now.Day.ToString()
                          + (DateTime.Now.Hour - 1).ToString()
                          + DateTime.Now.Year.ToString()
                          + (DateTime.Now.Month - 1).ToString();

            string url = "https://cpanel.epayindia.in/Default.aspx?lgnT="
                       + lgnT
                       + "&ID=" + result
                       + "&RedirectTo=PANKYC";

            switch (status)
            {
                case "VERIFIED":
                    pnlKyc.CssClass = "map-kyc verified";
                    litKycIcon.Text = "&#10003;";
                    litKycTitle.Text = "PAN Verified";
                    litKycMsg.Text = "Your PAN verification is complete. You can proceed with your Petro Card Package purchase.";
                    lnkKyc.Visible = false;
                    litKycChip.Visible = true;
                    litKycChip.Text = "<span class=\"map-kyc-chip\">Verified</span>";
                    break;

                case "REJECTED":
                    pnlKyc.CssClass = "map-kyc rejected";
                    litKycIcon.Text = "&#10007;";
                    litKycTitle.Text = "PAN Verification Rejected";
                    litKycMsg.Text = "Your PAN details were rejected during verification. "
                                       + "Please re-submit correct PAN details from your account to continue.";
                    lnkKyc.Visible = true;
                    lnkKyc.Text = "Re-submit PAN &#8594;";
                    lnkKyc.NavigateUrl = url;
                    break;

                case "PENDING":
                    pnlKyc.CssClass = "map-kyc";
                    litKycIcon.Text = "&#9203;";
                    litKycTitle.Text = "PAN Verification Pending";
                    litKycMsg.Text = "Your PAN details have been submitted and are under review. "
                                       + "You can purchase a Petro Card Package once the verification is approved.";
                    lnkKyc.Visible = true;
                    lnkKyc.Text = "View KYC Status &#8594;";
                    lnkKyc.NavigateUrl = url;
                    break;

                default:   // NOTSUBMITTED
                    pnlKyc.CssClass = "map-kyc";
                    litKycIcon.Text = "&#128179;";
                    litKycTitle.Text = "PAN Verification Required";
                    litKycMsg.Text = "PAN verification is mandatory before purchasing a Petro Card Package. "
                                       + "Please complete your PAN KYC from your account.";
                    lnkKyc.Visible = true;
                    lnkKyc.Text = "Verify PAN Now &#8594;";
                    lnkKyc.NavigateUrl = url;
                    break;
            }
        }
        catch (Exception ex)
        {
            _isPanVerified = false;
            Session["PanVerified"] = "N";
            objDal.WriteToFile("LoadPanKycStatus: " + ex.Message);

            pnlKyc.Visible = true;
            pnlKyc.CssClass = "map-kyc";
            litKycIcon.Text = "&#128179;";
            litKycTitle.Text = "PAN Verification Required";
            litKycMsg.Text = "We could not check your PAN verification status. Please complete your PAN KYC from your account.";
            lnkKyc.Visible = true;
            lnkKyc.Text = "Verify PAN Now &#8594;";
            lnkKyc.NavigateUrl = _kycUrl;
        }
    }

    #endregion

    #region ── Kit binding ──

    private void FillKit()
    {
        try
        {
            _purchasedKitId = GetPurchasedKitId();

            Dt = new DataTable();
            query = "Exec Sp_getKitPetro '" + Session["formno"] + "'";
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, query).Tables[0];

            Session["ShopFund"] = Dt;

            rptKitDetails.DataSource = Dt;
            rptKitDetails.DataBind();
        }
        catch (Exception ex)
        {
            objDal.WriteToFile("FillKit: " + ex.Message);
            ShowError("Unable to load Petro Card Packages. Please try again.");
        }
    }

    /// <summary>Already purchased petro kit id (0 = koi nahi)</summary>
    private int GetPurchasedKitId()
    {
        try
        {
            string str = " Select Top 1 kitid From " + objDal.dBName + "..repurchincome "
                       + " Where formno = " + Convert.ToInt32(Session["Formno"])
                       + " And kitid In (12,13,14) Order By kitid ";

            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];

            if (dt.Rows.Count > 0 && dt.Rows[0]["kitid"] != DBNull.Value)
                return Convert.ToInt32(dt.Rows[0]["kitid"]);

            return 0;
        }
        catch (Exception ex)
        {
            objDal.WriteToFile("GetPurchasedKitId: " + ex.Message);
            return 0;
        }
    }
    public static byte[] MD5Hash(string value)
    {
        return MD5.ComputeHash(Encoding.ASCII.GetBytes(value));
    }

    public static string Encrypt(string stringToEncrypt)
    {
        DES.Key = MD5Hash(key);
        DES.Mode = CipherMode.ECB;
        byte[] buffer = ASCIIEncoding.ASCII.GetBytes(stringToEncrypt);
        return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
    }

    public static string Decrypt(string encryptedString)
    {
        try
        {
            DES.Key = MD5Hash(key);
            DES.Mode = CipherMode.ECB;
            byte[] buffer = Convert.FromBase64String(encryptedString);
            return ASCIIEncoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }
        catch (Exception ex)
        {
            return DBNull.Value.ToString();
        }
    }

    protected void rptKitDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        DataRowView row = (DataRowView)e.Item.DataItem;

        int kitId = Convert.ToInt32(row["kitid"]);
        decimal kitAmount = Convert.ToDecimal(row["kitamount"]);
        string theme = Convert.ToString(row["Theme"]);

        HtmlGenericControl divCard = (HtmlGenericControl)e.Item.FindControl("divCard");
        Panel pnlTier = (Panel)e.Item.FindControl("pnlTier");
        Panel pnlStatus = (Panel)e.Item.FindControl("pnlStatus");
        Label lblStatus = (Label)e.Item.FindControl("lblStatus");
        HyperLink lnkBuy = (HyperLink)e.Item.FindControl("lnkBuy");
        Label lblNote = (Label)e.Item.FindControl("lblNote");

        string cardClass = "map-card " + theme;
        lblNote.Visible = false;

        // ══ STATE 0 : PAN verify nahi hai ══
        if (!_isPanVerified)
        {
            cardClass += " locked";

            lnkBuy.CssClass = "map-card-btn";
            lnkBuy.Text = "Buy Now &#8594;";
            string lgnT = Encrypt("uid=" + Session["IDNO"] + "&pwd=" + Session["MemPassw"]);

            string result = DateTime.Now.Day.ToString()
                          + (DateTime.Now.Hour - 1).ToString()
                          + DateTime.Now.Year.ToString()
                          + (DateTime.Now.Month - 1).ToString();

            string url = "https://cpanel.epayindia.in/Default.aspx?lgnT="
                       + lgnT
                       + "&ID=" + result
                       + "&RedirectTo=PANKYC";

            lnkBuy.NavigateUrl = "javascript:void(0);";
            lnkBuy.Attributes["onclick"] =
                string.Format("return PetroKycRequired('{0}');", url.Replace("'", "\\'"));
            //lnkBuy.NavigateUrl = "javascript:void(0);";
            //lnkBuy.Attributes["onclick"] =
            //    string.Format("return PetroKycRequired('{0}');", _kycUrl.Replace("'", "\\'"));

            divCard.Attributes["class"] = cardClass;
            return;
        }

        // ══ STATE 1 : Yahi kit already purchase ho chuka hai ══
        if (_purchasedKitId > 0 && _purchasedKitId == kitId)
        {
            cardClass += " purchased";

            pnlTier.Visible = false;
            pnlStatus.Visible = true;
            pnlStatus.CssClass = "map-status-ribbon";
            lblStatus.Text = "&#10003; Purchased";

            lnkBuy.CssClass = "map-card-btn done";
            lnkBuy.Text = "&#10003; Already Purchased";
            lnkBuy.NavigateUrl = "javascript:void(0);";
            lnkBuy.Enabled = false;
        }
        // ══ STATE 2 : Doosra kit liya hua hai ══
        else if (_purchasedKitId > 0)
        {
            cardClass += " locked";

            lnkBuy.CssClass = "map-card-btn";
            lnkBuy.Text = "Buy Now &#8594;";
            lnkBuy.NavigateUrl = "javascript:void(0);";
            lnkBuy.Attributes["onclick"] = "return PetroKitLocked();";
        }
        // ══ STATE 3 : Balance kam hai ══
        else if (_walletBalance < kitAmount)
        {
            decimal shortfall = kitAmount - _walletBalance;

            lnkBuy.CssClass = "map-card-btn";
            lnkBuy.Text = "Buy Now &#8594;";
            lnkBuy.NavigateUrl = "javascript:void(0);";
            lnkBuy.Attributes["onclick"] =
                string.Format("return PetroLowBalance('{0}','{1}','{2}');",
                              kitAmount.ToString("N2"),
                              _walletBalance.ToString("N2"),
                              shortfall.ToString("N2"));
        }
        // ══ STATE 4 : Sab theek ══
        else
        {
            lnkBuy.CssClass = "map-card-btn";
            lnkBuy.Text = "Buy Now &#8594;";
            lnkBuy.NavigateUrl = string.Format("PetroCardFinalPurchase.aspx?KitID={0}",
                                 HttpUtility.UrlEncode(Crypto.Encrypt(kitId.ToString())));
            lnkBuy.Attributes["onclick"] = "return PetroBuyNow(this);";
        }

        divCard.Attributes["class"] = cardClass;
    }

    #endregion

    #region ── Helpers ──

    private void ShowError(string msg)
    {
        Label2.Text = Server.HtmlEncode(msg);
        pnlError.Visible = true;
    }

    public string GenerateRandomStringActive(int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        StringBuilder sResult = new StringBuilder();

        for (int i = 0; i < iLength; i++)
            sResult.Append(allowChrs[rdm.Next(0, allowChrs.Length)]);

        return sResult.ToString();
    }

    #endregion
}