using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Welcome : System.Web.UI.Page
{
    private DAL ObjDal = new DAL();
    private string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                string strcondition = "";
                string str = "";

                DataTable dt = new DataTable();
                if (Request["id"] != null)
                {
                    //if (Request["id"] != Session["LASTID"].ToString())
                    //{
                    //    string scrname = "<SCRIPT language='javascript'>alert('Try Again Later.');" + "</SCRIPT>";
                    //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Invalid Access.');", true);
                    //    return;
                    //}

                    strcondition = " and mMst.IDNo=''" + Request["id"] + "''";
                }
                else
                {
                    if (Session["JOIN"] != null && Session["JOIN"].ToString() == "YES")
                    {
                        strcondition = " and mMst.IDNo=''" + Session["LASTID"] + "''";
                    }
                    else if (Session["Status"] != null && Session["Status"].ToString() == "OK")
                    //else if (Session["Status"].ToString() == "OK")
                    {
                        strcondition = " and mMst.FormNo=''" + Convert.ToInt32(Session["Formno"]) + "''";
                    }
                    else
                    {
                        Response.Redirect("Logout.aspx");
                        Response.End();
                    }
                }

                DataSet ds1 = new DataSet();
                str = ObjDal.Isostart + "exec sp_MemDtl '" + strcondition + "'" + ObjDal.IsoEnd;
                dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    LblIdno.Text = dt.Rows[0]["Idno"].ToString();
                    LblName.Text = dt.Rows[0]["Memname"].ToString();
                    LblIdno1.Text = dt.Rows[0]["Idno"].ToString();
                    LblName1.Text = dt.Rows[0]["Memname"].ToString();
                    lblDoj.Text = Convert.ToDateTime(dt.Rows[0]["Doj"]).ToString("dd-MMM-yyyy");
                    lblDoj1.Text = Convert.ToDateTime(dt.Rows[0]["Doj"]).ToString("dd-MMM-yyyy");
                    lblPassw.Text = dt.Rows[0]["Passw"].ToString();
                    lblTransPassw.Text = dt.Rows[0]["EPassw"].ToString();

                    if (Session["JOIN"] != null && Session["JOIN"].ToString() == "YES")
                    {
                        Session["JOIN"] = "FINISH";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BtnHome_ServerClick(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("index.aspx");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void SendHtmlFormattedEmail(string recepientEmail, string subject, string body)
    {
        try
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Session["CompMail"].ToString());
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(new MailAddress(recepientEmail));
            SmtpClient smtp = new SmtpClient();
            smtp.Host = Session["MailHost"].ToString();
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = Session["CompMail"].ToString();
            NetworkCred.Password = Session["MailPass"].ToString();
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Send(mailMessage);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    private string PopulateBody(string IdNo, string Email, string MemberName, string Password, string EPassword, string Panno, string PlacementId, string PlacementName, string ReceiptNo, string ApplicationNo, string Address, string City, string District, string Statename, string kitName, string Kitamount, string Mobileno, string Doj)
    {
        try
        {
            string body = string.Empty;
            StreamReader reader = new StreamReader(Server.MapPath("~/welcome.htm"));
            body = reader.ReadToEnd();
            body = body.Replace("{CompName}", Session["CompName"].ToString());
            body = body.Replace("{Idno}", IdNo);
            body = body.Replace("{Name}", MemberName);
            body = body.Replace("{Address}", Address);
            body = body.Replace(" {City}", City);
            body = body.Replace(" {District}", District);
            body = body.Replace(" {State}", Statename);
            body = body.Replace(" {Mobile}", Mobileno);
            body = body.Replace("{DOJ}", Doj);
            body = body.Replace("{PlacementId}", PlacementId);
            body = body.Replace("{PlacementName}", PlacementName);
            body = body.Replace(" {KitName}", kitName);
            body = body.Replace(" {KitAmount}", Kitamount);
            body = body.Replace(" {Application}", ApplicationNo);
            body = body.Replace(" {Receipt}", ReceiptNo);
            body = body.Replace(" {Email}", Email);
            body = body.Replace(" {Panno}", Panno);
            body = body.Replace("{Passw}", Password);
            body = body.Replace("{EPassw}", EPassword);
            return body;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
            return null;
        }
    }
    protected void BtnNewJoin_ServerClick(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Registration.aspx");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}
