using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebApp : System.Web.UI.Page
{
    // web.config -> connectionStrings -> "EpayConn"
    private string Cs
    {
        get { return ConfigurationManager.ConnectionStrings["constr"].ConnectionString; }
    }

    // Banner count JS slider ke liye chahiye (.aspx mein <%= BannerCount %>)
    public int BannerCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    BindAll();
                }

            }

            else
            {
                Response.Redirect("AppLogout.aspx", false);
            }


        }
        catch (Exception ex)
        {
            // Handle exception
        }

    }

    private void BindAll()
    {
        // 1. Banner slider
        DataTable banners = GetTable(
            "SELECT bg_class,tag,title,subtitle,cta,cta_icon,cta_dark,url,[target],deco " +
            "FROM BannerSlider WHERE active = 1 ORDER BY sort_order");
        BannerCount = banners.Rows.Count;
        rptBanner.DataSource = banners;
        rptBanner.DataBind();
        rptBannerDots.DataSource = banners;
        rptBannerDots.DataBind();

        // 2. Quick access
        //rptQuick.DataSource = GetTable(
        //    "SELECT label,icon,color,url,[target] FROM QuickAccess " +
        //    "WHERE is_active = 1 ORDER BY sort_order");
        //rptQuick.DataBind();

        //// 3. Categories
        //rptCategories.DataSource = GetTable(
        //    "SELECT label,icon,url,[target],is_default FROM Categories " +
        //    "WHERE is_active = 1 ORDER BY sort_order");
        //rptCategories.DataBind();

        //// 4. Featured services
        //rptFeatured.DataSource = GetTable(
        //    "SELECT name,description,emoji,img_bg,badge,badge_icon,url,[target] FROM FeaturedServices " +
        //    "WHERE is_active = 1 ORDER BY sort_order");
        //rptFeatured.DataBind();

        // 5. Special offers (sirf abhi valid)
        //rptOffers.DataSource = GetTable(
        //    "SELECT bg_class,tag,title,subtitle,deco,url,[target] FROM SpecialOffers " +
        //    "WHERE is_active = 1 AND (valid_upto IS NULL OR valid_upto >= CAST(GETDATE() AS DATE)) " +
        //    "ORDER BY sort_order");
        //rptOffers.DataBind();

        // 6. All services
        rptServices.DataSource = GetTable(
            "SELECT name,icon,color,url,[target] FROM AllServices " +
            "WHERE is_active = 1 ORDER BY sort_order");
        rptServices.DataBind();

        // 7. Best selling
        rptBest.DataSource = GetTable(
            "SELECT [rank],name,icon,icon_bg,rank_bg,users,url,[target] FROM BestSelling " +
            "WHERE is_active = 1 ORDER BY [rank]");
        rptBest.DataBind();

        // 8. Trust statistics
        rptTrust.DataSource = GetTable(
            "SELECT value,label,icon,icon_color,icon_bg FROM TrustStatistics " +
            "WHERE is_active = 1 ORDER BY sort_order");
        rptTrust.DataBind();

        // 9. Promo banners (top + bottom alag alag)
        //rptPromoTop.DataSource = GetTable(
        //    "SELECT bg_class,deco,tag,title,subtitle,cta,btn_inv,url,[target] FROM PromoBanner " +
        //    "WHERE is_active = 1 AND position = 'top'");
        //rptPromoTop.DataBind();

        rptPromoBottom.DataSource = GetTable(
            "SELECT bg_class,deco,tag,title,subtitle,cta,btn_inv,url,[target] FROM PromoBanner " +
            "WHERE is_active = 1 AND position = 'bottom'");
        rptPromoBottom.DataBind();
    }

    /// <summary>Ek query run karke DataTable return karta hai.</summary>
    private DataTable GetTable(string sql)
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(Cs))
        using (SqlCommand cmd = new SqlCommand(sql, con))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
        return dt;
    }

    /* ---------- Template helper methods (.aspx <%# ... %> mein call hote hain) ---------- */

    protected string Target(object o)
    {
        string t = (o == null || o == DBNull.Value) ? "" : o.ToString();
        return string.IsNullOrEmpty(t) ? "_self" : t;
    }

    protected string Dark(object o)
    {
        return (o != null && o != DBNull.Value && Convert.ToBoolean(o)) ? "dark" : "";
    }

    protected string Inv(object o)
    {
        return (o != null && o != DBNull.Value && Convert.ToBoolean(o)) ? "inv" : "";
    }

    protected string ActiveChip(object o)
    {
        return (o != null && o != DBNull.Value && Convert.ToBoolean(o)) ? "active" : "";
    }

    // BestSelling rank badge: NULL ho to default gold CSS, warna inline silver gradient
    protected string RankStyle(object o)
    {
        if (o == null || o == DBNull.Value || string.IsNullOrEmpty(o.ToString()))
            return "";
        return "style=\"background:" + o + "\"";
    }
}
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//public partial class WebApp : System.Web.UI.Page
//{
//    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
//    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
//    public string shopEnc = "";
//    public string gvEnc = "";
//    public string utilityEnc = "";
//    public string foodEnc = "";
//    public string movieEnc = "";
//    protected void Page_Load(object sender, EventArgs e)
//    {
//        try
//        {

//            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
//            {
//                if (!Page.IsPostBack)
//                {
//                    DataTable dt = new DataTable();
//                    string sql = "select profilepic, * from M_MemberMaster where formno=" + Session["FormNo"];
//                    dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
//                    if (dt.Rows.Count > 0)
//                    {
//                        ddUserName.InnerText = dt.Rows[0]["memfirstname"].ToString();
//                    }
//                }

//            }

//            else
//            {
//                Response.Redirect("AppLogout.aspx", false);
//            }


//        }
//        catch (Exception ex)
//        {
//            // Handle exception
//        }

//    }
//}