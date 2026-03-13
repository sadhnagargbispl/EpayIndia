using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Logout : System.Web.UI.Page
{
    DAL objDal = new DAL();
    protected void Page_Load(object sender, EventArgs e)
    {
       
       Response.Cache.SetCacheability(HttpCacheability.NoCache);
       Response.ExpiresAbsolute = DateTime.Now;
       Session.Abandon();
       Response.Cookies.Remove("");
       Response.Cookies.Clear();

        //int userId = Convert.ToInt32(Session["UserID"]);
        //string username =Session["UserName"].ToString();
        //string sql = "Update " + objDal.tblUserMaster + " set LastLogOutTime='" + DateTime.Now.ToString() + "',LoginStatus='N' where UserId='" + userId + "' AND UserName like'%" + username + "%' AND " + objDal.activeCondition;
        //int a = objDal.UpdateData(sql);

        string nextpage = "Default.aspx";
       Response.Write("<script language=javascript>");

       Response.Write("{");
       Response.Write(" var Backlen=history.length;");

       Response.Write(" history.go(-Backlen);");
       Response.Write(" window.location.href='" + nextpage + "'; ");

       Response.Write("}");
       Response.Write("</script>");

       Session["IDARR"] = null;
       Session["AStatus"] = "";
       Session["Username"] = "";
       Session["Idno"] = "";
       Session["MemName"] = "";
       Session["FSessID"] = "";
       Session["KitId"] = "";
       Session["Uid"] = "";

        Response.Redirect("Default.aspx");
    }
}