using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BinaryTree : System.Web.UI.Page
{
    DAL Objdal = new DAL();
    protected void cmdBack_Click(object sender, EventArgs e)
    {
        // Response.Redirect("cpindex.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // string formno = GetFormNo();
        // string depthlevel = txtDeptlevel.Text;
        // TreeFrame.Attributes["src"] = "NewTree.aspx?DownLineFormNo=" + Convert.ToInt32(formno).ToString() + "&deptlevel=" + Convert.ToInt32(depthlevel).ToString();
        ShowTree(Convert.ToInt32(txtDeptlevel.Text));
    }

    protected void BtnStepAbove_Click(object sender, EventArgs e)
    {
        string scrname = "";
        if (Session["Upliner"] != null)
        {
            string uplnformno = Session["Upliner"].ToString();
            if (Convert.ToInt32(uplnformno) == 0)
            {
                scrname = "<SCRIPT language='javascript'>alert('No Upliner Id !! ');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Incorrect", scrname, false);
                return;
            }
            else
            {
                // TreeFrame.Attributes["src"] = "Newtree.aspx?DownLineFormNo=" + uplnformno + "&deptlevel=" + Convert.ToInt32(4);
                Response.Redirect("Newtree.aspx?DownLineFormNo=" + uplnformno + "&deptlevel=" + Convert.ToInt32(4));
            }
        }
    }
    private string GetFormNo()
    {
        string idNo = txtDownLineFormNo.Text;
        string formno = "";
        string UplnFormno = "";
        DataTable Dt = new DataTable();

        string qry = "Select FormNo, UplnFormno from " + Objdal.tblMemberMaster + " where IdNo='" + idNo + "'";
        DataTable dt = Objdal.GetData(qry);

        if (dt.Rows.Count > 0)
        {
            formno = dt.Rows[0]["FormNo"].ToString();
            UplnFormno = dt.Rows[0]["UplnFormNo"].ToString();
            if (UplnFormno != "1")
            {
                Session["Upliner"] = UplnFormno;
            }
        }

        return formno;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["AStatus"].ToString() != "OK")
        if (Session["AStatus"] != null && (string)Session["AStatus"] != "OK")
        {
            Response.Redirect("Default.aspx");
        }
        else
        {
            Session["PageName"] = "Member / Member Tree";
        }

        if (Request.QueryString.HasKeys())
        {
            if (!string.IsNullOrEmpty(Request.QueryString["key"]))
            {
                if (!Page.IsPostBack)
                {
                    txtDownLineFormNo.Text = Request.QueryString["key"].ToString();
                    ShowTree(6);
                }
            }
        }
    }
    private void ShowTree(int Level)
    {
        string formno = GetFormNo();
        string scrname = "";
        if (!string.IsNullOrEmpty(formno))
        {
            // TreeFrame.Attributes["src"] = "NewTree.aspx?DownLineFormNo=" + Convert.ToInt32(formno).ToString() + "&deptlevel=" + Convert.ToInt32(Level).ToString();
            Response.Redirect("NewTree.aspx?DownLineFormNo=" + Convert.ToInt32(formno).ToString() + "&deptlevel=" + Convert.ToInt32(Level).ToString());
        }
        else
        {
            scrname = "<SCRIPT language='javascript'>alert('Enter correct Member Id !! ');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Incorrect", scrname, false);
        }
    }

}
