using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SponsorTree : System.Web.UI.Page
{

    DataTable Dt = new DataTable();
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string strScript;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (Session["AStatus"] != null)
                {
                }
                else
                {
                    Session["PageName"] = "Member / Sponsor Tree";
                }
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    protected void cmdBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Home.aspx");
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }

    }

    private string get_FormNo(string IDNo)
    {

        string FormNo = "";
        try
        {
            DataTable dt = new DataTable();
            string StrSql = objDal.IsoStart + "Select FormNo From " + objDal.DBName + "..M_MemberMaster Where IDNo='" + IDNo + "'" + objDal.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, StrSql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                FormNo = dt.Rows[0]["FormNo"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return FormNo;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["AStatus"] != null)
            {
                string DownFormNo = get_FormNo(DownLineFormNo.Text); // Assuming DownLineFormNo is a TextBox
                //TreeFrame.Attributes["src"] = "Referaltree.aspx?DownLineFormNo=" + DownFormNo;
                if (DownFormNo == "")
                    {

                    strScript = "<script language='javascript'>alert('Member ID Not Exist.!!');</script>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", strScript, false);
                }
                else
                { 
                Response.Redirect("Referaltree.aspx?DownLineFormNo=" + DownFormNo);
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }


}
