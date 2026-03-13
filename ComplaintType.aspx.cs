using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ComplaintType : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                this.btnShowRecord.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnShowRecord));
                this.btnShowRecord.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnShowRecord));
                this.btnShowRecord.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnShowRecord));
                txtSearch.Text = "";

                lblView.Visible = false;
                if (Session["AStatus"] != null)
                {
                    BindData();
                }
            }
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
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
            string status = "";
            string sql = "";
            string Condition = "";
            if (ddlComplain.SelectedValue.Trim().ToLower() == "showall")
            {
                Condition = "";
            }
            else
            {
                Condition = Condition + " And " + ddlComplain.SelectedValue + "  Like   '%" + ClearInject(txtSearch.Text) + "%' ";
            }
            if (String.Equals(ddlComplain.SelectedItem.Text.ToLower(), "status"))
            {
                if (!String.IsNullOrEmpty(txtSearch.Text))
                {
                    if (txtSearch.Text.ToLower().Contains("deactive"))
                    {
                        status = "DeActive";
                    }
                    else
                    {
                        status = "Active";
                    }
                    sql = objDal.IsoStart + " select * from  V#Complaint  Where 1=1  AND Status = '" + status.ToString() + "'" + objDal.IsoEnd;
                }
            }
            else
            {
                sql = objDal.IsoStart + " select * from V#Complaint Where 1=1  " + Condition + objDal.IsoEnd;
            }
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
    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["GData"];
            GvData.DataBind();
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    protected void DeleteGroup(object sender, EventArgs e)
    {
        try
        {
            string GrpID, scrname;
            String Msg = string.Empty;
            GridViewRow GVRw;
            GVRw = (GridViewRow)((Control)sender).NamingContainer;
            GrpID = ((Label)GVRw.FindControl("LblGrpID")).Text;
            string Sql = "Update M_ComplaintTypeMaster SET ActiveStatus='N',LastModified='De-Activated by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "' WHERE CTypeId='" + GrpID + "' AND RowStatus='Y'";
            int updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql));
            if (updateEffect > 0)
            {
                Msg = "Deleted Successfully.!";

            }
            else
            {
                Msg = "Not able to delete the selected Complaint Type.!";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Msg + "');", true);
            BindData();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataGrid dg = new DataGrid();
            string Condition = "";
            if (ddlComplain.SelectedValue.Trim().ToLower() == "showall")
            {
                Condition = "";
            }
            else
            {
                Condition = Condition + " And " + ddlComplain.SelectedValue + "  Like   '%" + ClearInject(txtSearch.Text) + "%' ";
            }
            string sql = objDal.IsoStart + " select * from  V#Complaint Where 1=1  " + Condition + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
                dg.DataSource = Dt;
            dg.DataBind();
            ExportToExcel("ComplaintType.xls", dg);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
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

    protected void btnShowRecord_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();

            ddlComplain.SelectedIndex = 0;
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
            Response.Redirect("AddCType.aspx", false);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }

    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
            StrObj = StrObj.Trim();
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return StrObj;
    }
}