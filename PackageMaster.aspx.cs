using ClosedXML.Excel;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class PackageMaster : System.Web.UI.Page
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
                AddKit.Attributes.Add("onclick", DisableTheButton(Page, AddKit));
                txtSearch.Text = "";
                lblView.Visible = false;
                if (Session["AStatus"] != null)
                {
                    BindData();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public void BindData()
    {
        string status = "";
        string sql = "";
        try
        {
            string Condition = "";
            if (ddlSearchFields.SelectedValue.Trim().ToLower() == "showall")
            {
                Condition = "";
            }
            else
            {
                Condition = Condition + " And " + ddlSearchFields.SelectedValue + "  Like   '%" + ClearInject(txtSearch.Text) + "%' ";
            }
            if (String.Equals(ddlSearchFields.SelectedItem.Text.ToLower(), "status"))
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
                    sql = objDal.IsoStart + " select * from  V#MMkITMASTER Where 1=1  AND Status = '" + status.ToString() + "'" + objDal.IsoEnd;
                }
            }
            else
            {
                sql = objDal.IsoStart + " select * from  V#MMkITMASTER Where 1=1  " + Condition + objDal.IsoEnd;
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
    protected void DeleteGroup(object sender, EventArgs e)
    {
        try
        {
            string KitId;
            GridViewRow GVRw;
            GVRw = ((GridViewRow)((Control)sender).NamingContainer);
            KitId = ((Label)GVRw.FindControl("LblNewsID")).Text;
            string Sql = "Update MM_KitMaster SET ActiveStatus='N',LastModified='De-Activated by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "' WHERE KitId='" + KitId.ToString() + "' AND RowStatus='Y'";
            int updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql));
            if (updateEffect > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Done Successfully.!')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Not able to delete the selected Group.! ')", true);

            }
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void btnShowRecord_Click(object sender, EventArgs e)
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
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("AddPackage.aspx", false);
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
            DataTable dt = new DataTable();
            dt = (DataTable)Session["GData"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Users");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=KitMaster-" + DateTime.Now.ToShortDateString() + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
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