using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserPermission : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DataTable tmpTable = new DataTable();
    DAL objDal = new DAL();
    private ModuleFunction objModuleFun = new ModuleFunction();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.btnSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnSave));
            this.btnShow.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnShow));
            objModuleFun = new ModuleFunction();
            if (!Page.IsPostBack)
            {
                lblMsg.Visible = false;
                string qry1 = objDal.IsoStart + "Select * from " + objDal.DBName + "..M_UserGroupMaster  Where ActiveStatus='Y' AND " + objDal.activeCondition + objDal.IsoEnd;
                tmpTable = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry1).Tables[0];
                ddlGroup.DataSource = tmpTable;
                ddlGroup.DataValueField = "GroupId";
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataBind();
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
        try
        {

            string qry1 = objDal.IsoStart + "Select MenuId, MenuName, ParentID from " + objDal.DBName + "..M_WebMenuMaster where RowStatus='Y' AND ActiveStatus='Y' ";
            qry1 += "AND MenuName not like '-' order by Hierar, MenuId" + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry1).Tables[0];
            if (Dt.Rows.Count > 0)
                GvData.DataSource = Dt;
            GvData.DataBind();
            Session["GData"] = Dt;
            SetCheckBoxValue();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            lblMsg.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string qry1 = "Update M_UserPermissionMaster set RowStatus='N' where GroupId='" + ddlGroup.SelectedValue.ToString() + "';";
            CheckBox Chk;
            Label Lblmenu;
            if (GvData.Rows.Count > 0)
            {
                foreach (GridViewRow Gvr in GvData.Rows)
                {
                    Chk = (CheckBox)Gvr.FindControl("chkMenuPermission");
                    Lblmenu = (Label)Gvr.FindControl("lblMenuId");
                    if (Chk.Checked == true)
                    {
                        qry1 = qry1 + " insert into M_UserPermissionMaster(MenuId,GroupId) values('" + Lblmenu.Text + "','" + ddlGroup.SelectedValue.ToString() + "');";
                    }
                }
                int a = objDal.UpdateData(qry1);
                if (a != 0)
                {
                    lblMsg.Text = "Permission set for the selected group successfully.";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblMsg.Text = "Permission not set for the selected group";
                    lblMsg.Visible = true;
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }

            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["GData"];
            GvData.DataBind();
            Session["grdIndex"] = GvData.PageIndex;
            SetCheckBoxValue();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void SetCheckBoxValue()
    {
        try
        {
            List<string> strlist = new List<string>();

            // Fetch data from the database
            string qry2 = objDal.IsoStart + "Select a.MenuId as MenuId from " + objDal.DBName + ".. M_WebMenuMaster as a," + objDal.DBName + "..M_UserGroupMaster as b,";
            qry2 += "" + objDal.DBName + "..M_UserPermissionMaster as c where a.RowStatus='Y' AND b.RowStatus='Y' AND c.RowStatus='Y' AND a.MenuId=c.MenuId AND c.GroupId=b.GroupId ";
            qry2 += "AND c.GroupId='" + ddlGroup.SelectedValue.ToString() + "' order by a.MenuId" + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry2).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow row in Dt.Rows)
                {
                    strlist.Add(row["MenuId"].ToString());
                }
            }

            // Iterate over GridView rows and set checkbox status
            CheckBox Chk;
            Label Lbl;
            if (GvData.Rows.Count > 0)
            {
                foreach (GridViewRow Gvr in GvData.Rows)
                {
                    Chk = (CheckBox)Gvr.FindControl("chkMenuPermission");
                    Lbl = (Label)Gvr.FindControl("lblMenuId");
                    if (strlist.Contains(Lbl.Text))
                    {
                        Chk.Checked = true;
                    }
                    else
                    {
                        Chk.Checked = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void GvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int i;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (string.Equals(((Label)e.Row.FindControl("lblParentId")).Text.ToLower(), "0") == true)
                {
                    e.Row.BackColor = System.Drawing.Color.DarkGray;
                    for (i = 0; i < e.Row.Cells.Count; i++)
                    {
                        e.Row.Cells[i].Style["color"] = "Black";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

}