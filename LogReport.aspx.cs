using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class LogReport : System.Web.UI.Page
{

    DataTable Dt = new DataTable();
    DAL objDal = new DAL();
    ModuleFunction objModuleFun = new ModuleFunction();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string searchtext = Session["Search"] as string;
            this.btnSearch.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnSearch));
            if (!Page.IsPostBack)
            {
                string qry1 = objDal.IsoStart + "Select * From(Select 0 As UserId,'-- ALL --' As UserName Union ALL select UserID,";
                qry1 += "UserName As UserName from " + objDal.DBName + "..M_UserMaster Where ActiveStatus='Y' and RowStatus='Y') As Temp order by UserID " + objDal.IsoEnd;
                Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry1).Tables[0];
                DDlMember.DataSource = Dt;
                DDlMember.DataValueField = "UserId";
                DDlMember.DataTextField = "UserName";
                DDlMember.DataBind();
                if (RbtUser.SelectedValue == "A")
                {
                    DDlMember.Visible = true;
                    txtMember.Visible = false;
                }
                else
                {
                    DDlMember.Visible = false;
                    txtMember.Visible = true;
                }

                GvData.Visible = false;
                gvContainer.Visible = false;
                Session["LogReport"] = null;
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
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            lblErr.Text = "";
            lblCount.Text = "";
            string Condition = "";

            {
                if (RbtUser.SelectedValue == "M")
                {
                    if (!string.IsNullOrEmpty(txtMember.Text))
                    {
                        Condition += " AND Idno='" + ClearInject(txtMember.Text) + "'";
                    }
                }
                else
                {
                    if (DDlMember.SelectedValue != "0")
                    {
                        Condition += " AND UserId='" + DDlMember.SelectedValue + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(txtStartDate.Text))
            {
                Condition += " AND CAST(CONVERT(Varchar, Changedate, 106) AS DateTime) >= '" + ClearInject(txtStartDate.Text) + "'";
            }
            if (!string.IsNullOrEmpty(txtEndDate.Text))
            {
                Condition += " AND CAST(CONVERT(Varchar, Changedate, 106) AS DateTime) <= '" + ClearInject(txtEndDate.Text) + "'";
            }
            string sql = objDal.IsoStart + " select * from  V#LogReport Where 1=1  " + Condition + objDal.IsoEnd;

            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                GvData.DataSource = Dt;
                GvData.DataBind();
                Session["LogReport"] = Dt;
                GvData.Visible = true;
                gvContainer.Visible = true;
                lblCount.Text = "Total : " + Dt.Rows.Count;
            }
            else
            {
                GvData.Visible = false;
                gvContainer.Visible = false;
                lblErr.Text = "No Record Found!!";
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }

    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataGrid dg = new DataGrid();
            lblErr.Text = "";
            lblCount.Text = "";
            string Condition = "";

            {
                if (RbtUser.SelectedValue == "M")
                {
                    if (!string.IsNullOrEmpty(txtMember.Text))
                    {
                        Condition += " AND Idno='" + ClearInject(txtMember.Text) + "'";
                    }
                }
                else
                {
                    if (DDlMember.SelectedValue != "0")
                    {
                        Condition += " AND UserId='" + DDlMember.SelectedValue + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(txtStartDate.Text))
            {
                Condition += " AND CAST(CONVERT(Varchar, Changedate, 106) AS DateTime) >= '" + ClearInject(txtStartDate.Text) + "'";
            }
            if (!string.IsNullOrEmpty(txtEndDate.Text))
            {
                Condition += " AND CAST(CONVERT(Varchar, Changedate, 106) AS DateTime) <= '" + ClearInject(txtEndDate.Text) + "'";
            }
            string sql = objDal.IsoStart + " select * from  V#LogReport Where 1=1  " + Condition + objDal.IsoEnd;

            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
                dg.DataSource = Dt;
            dg.DataBind();
            ExportToExcel("LogReport.xls", dg);
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
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        {
            try
            {
                GvData.PageIndex = e.NewPageIndex;
                GvData.DataSource = Session["LogReport"];
                GvData.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
            }
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