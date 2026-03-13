using ClosedXML.Excel;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class UserMaster : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DataTable tmpTable = new DataTable();
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.BtnShowAll.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnShowAll));
            this.BtnAdd.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnAdd));
            if (!Page.IsPostBack)
            {
                txtSearch.Text = "";
                Label1.Visible = false;
                string qry1 = objDal.IsoStart + "Select * from " + objDal.DBName + "..M_UserGroupMaster Where ActiveStatus='Y' AND RowStatus='Y'" + objDal.IsoEnd;
                tmpTable = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry1).Tables[0];
                ddlGroup.DataSource = tmpTable;
                ddlGroup.DataValueField = "GroupId";
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataBind();
                if (Session["GroupId"] == null)
                {
                    ddlGroup.SelectedValue = (string)Session["GroupId"];
                }

                if (Convert.ToInt32(Session["grdIndex"]) == 0)
                {
                }
                else
                {
                    GvData.PageIndex = Convert.ToInt32(Session["grdIndex"]);
                }
                // Fill grid with data
                BindData();
                Session["GroupId"] = ddlGroup.SelectedValue.ToString();
                Session["GroupName"] = ddlGroup.SelectedItem.Text;
                Session["grpID"] = ddlGroup.SelectedValue.ToString();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private string DisableTheButton(Control pge, Control btn)
    {

        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
            sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
            sb.Append("this.value = 'Please wait...';");
            sb.Append("this.disabled = true;");
            sb.Append(pge.Page.GetPostBackEventReference(btn));
            sb.Append(";");
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    public void BindData()
    {
        string Condition = "";
        try
        {
            if (ddlSearchFields.SelectedValue.Trim().ToLower() == "showall")
            {
                Condition = "";
            }
            else
            {
                Condition = Condition + " And " + ddlSearchFields.SelectedValue + "  Like   '%" + txtSearch.Text.Trim() + "%' ";
            }
             string sql = objDal.IsoStart + " select * from  V#UserBind Where 1=1  " + Condition + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                GvData.DataSource = Dt;
                GvData.DataBind();
                Session["GData"] = Dt;

            }
            else
            {
                btnExport.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnSubmit_Click1(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void DeleteGroup(object sender, System.EventArgs e)
    {
        try
        {
            string userID, scrname;
            GridViewRow GVRw;
            GVRw = ((Control)sender).NamingContainer as GridViewRow;
            userID = ((Label)GVRw.FindControl("LblUserID")).Text;
            string Sql = "Update M_UserMaster SET ActiveStatus='N',LastModified='De-Activated by " + ("UserName") + " at " + DateTime.Now.ToString() + "' WHERE UserId='" + userID + "' AND RowStatus='Y'";
            int updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql));
            if (Dt.Rows.Count > 0)
            {
                scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');" + "</SCRIPT>";
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Group! ');" + "</SCRIPT>";
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Group Deletion", scrname, false);
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["grpID"] = ddlGroup.SelectedValue.ToString();
            Session["GroupId"] = ddlGroup.SelectedValue.ToString();
            Session["GroupName"] = ddlGroup.SelectedItem.Text;
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }

    }
    protected void BtnShowAll_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();

            ddlSearchFields.SelectedIndex = 0;
            txtSearch.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
  
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("AddUser.aspx", false);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTemp = new DataTable();
            DataGrid dg = new DataGrid();
            string Condition = "";

            if (ddlSearchFields.SelectedValue.Trim().ToLower() == "showall")
            {
                Condition = "";
            }
            else
            {
                Condition = Condition + " And " + ddlSearchFields.SelectedValue + "  Like   '%" + txtSearch.Text.Trim() + "%' ";
            }
            string sql = objDal.IsoStart + " select * from  V#UserBind Where 1=1  " + Condition + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
                dg.DataSource = Dt;
            dg.DataBind();
            ExportToExcel("UserMaster.xls", dg);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "Error In Exporting File");
        }
    }

    private void ExportToExcel(string fileName, DataGrid dg)
    {
        try
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            dg.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["GData"];
            GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

}