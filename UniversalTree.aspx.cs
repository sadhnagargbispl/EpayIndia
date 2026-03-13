using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;


public partial class UniversalTree : System.Web.UI.Page
{
    DAL ObjDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string strScript;
    protected void Page_Load(object sender, System.EventArgs e)
    {
        try
        {
            if (Session["AStatus"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            else
            {
                Session["PageName"] = "Member / Member Tree";
            }
            Fill_PoolType();
            if (Request.QueryString != null && Request.QueryString.HasKeys())
            {
               
                if (!string.IsNullOrEmpty(Request.QueryString["key"]))
                {
                    if (!Page.IsPostBack)
                    {

                        txtDownLineFormNo.Text = Request.QueryString["key"].ToString();
                        string formno;
                        formno = GetFormNo();
                        if (formno != "")
                        {
                            string level;
                            level = "4";
                            //TreeFrame.Attributes["src"] = "UniTree.aspx?DownLineFormNo=" + formno.ToString() + "&deptlevel=" + level.ToString() + "&type=" + ddlTree.SelectedValue.ToString();
                            Response.Redirect("UniTree.aspx?DownLineFormNo=" + formno.ToString() + "&deptlevel=" + level.ToString() + "&type=" + ddlTree.SelectedValue.ToString());
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }

    private string GetFormNo()
    {
        string formno = "";
        try
        {
            string idNo;

            idNo = txtDownLineFormNo.Text;
            string qry = ObjDal.IsoStart + " Select FormNo from " + ObjDal.DBName + "..M_MemberMaster where IdNo='" + idNo + "' " + ObjDal.IsoEnd;
            DataTable dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
            if ((dt.Rows.Count > 0))
            {
                formno = dt.Rows[0]["FormNo"].ToString();

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return formno;
    }

   

   
    private void Fill_PoolType()
    {
        try
        {
            DataTable dt = new DataTable();
            string str = ObjDal.IsoStart + " Select Tree_Code As PreFix,Tree_Name As Name From " + ObjDal.DBName + "..M_PoolTreeMaster   order By ID " + ObjDal.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            if ((dt.Rows.Count > 0))
            {
                ddlTree.DataSource = dt;
                ddlTree.DataTextField = "Name";
                ddlTree.DataValueField = "PreFix";
                ddlTree.DataBind();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }


    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string formno;
            formno = GetFormNo();
            string depthlevel = txtDeptlevel.Text;
            if (formno == "")
            {
                strScript = "<script language='javascript'>alert('Member ID Not Exist.!!');</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", strScript, false);

            }
            else
            { 
            //TreeFrame.Attributes["src"] = "UniTree.aspx?DownLineFormNo=" + formno.ToString() + "&deptlevel=" + depthlevel.ToString() + "&type=" + ddlTree.SelectedValue.ToString();
            Response.Redirect("UniTree.aspx?DownLineFormNo=" + formno.ToString() + "&deptlevel=" + depthlevel.ToString() + "&type=" + ddlTree.SelectedValue.ToString());
            }
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }

    protected void cmdBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Home.aspx");
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
}
