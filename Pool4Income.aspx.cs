using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pool4Income : System.Web.UI.Page
{
    DAL objDAL = new DAL();
    DataSet Ds = new DataSet();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["AStatus"] != null)
                {


                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            BindData(1);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindData(1);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public void BindData(int PageIndex)
    {
        lblError.Text = "";
        try
        {
            string FromSessid = "";
            string ToSessid = "";
            string Idno = "0";

            FromSessid = txtStartDate.Text != "" ? txtStartDate.Text : Session["CompDate"].ToString();
            ToSessid = txtEndDate.Text != "" ? txtEndDate.Text : DateTime.Now.ToString("dd-MMM-yyyy");
            Idno = txtMemId.Text != "" ? txtMemId.Text : "0";

            GvData1.DataSource = null;
            GvData1.DataBind();
            SqlParameter[] prms = new SqlParameter[7];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@FromSessid", FromSessid);
            prms[2] = new SqlParameter("@ToSessid", ToSessid);
            prms[3] = new SqlParameter("@PageIndex", PageIndex);
            prms[4] = new SqlParameter("@PageSize", 100000000);
            prms[5] = new SqlParameter("@IsExport", "N");
            prms[6] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetPool4IncomeReport", prms);
            GvData1.DataSource = Ds.Tables[0];
            GvData1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GvData1.DataBind();
            int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);
            Session["Pool4IncomeReport"] = Ds.Tables[0];
            ViewState["IdNo"] = "IdNo";
            ViewState["Sort_Order"] = "ASC";
            if (Ds.Tables[0].Rows.Count > 0)
            {
                lblCount.Text = "Total Record: " + Ds.Tables[1].Rows[0]["RecordCount"].ToString();
                lblinv.Text = "Total Investment: " + Ds.Tables[1].Rows[0]["Investment"].ToString();
                GvData1.Visible = true;
            }
            else
            {
                lblError.Text = "No Record Found!!";
                GvData1.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
  protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string FromSessid = "0";
            string ToSessid = "0";
            string Idno = "0";
            FromSessid = txtStartDate.Text != "" ? txtStartDate.Text : Session["CompDate"].ToString();
            ToSessid = txtEndDate.Text != "" ? txtEndDate.Text : DateTime.Now.ToString("dd-MMM-yyyy");
            Idno = txtMemId.Text != "" ? txtMemId.Text : "0";
            SqlParameter[] prms = new SqlParameter[7];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@FromSessid", FromSessid);
            prms[2] = new SqlParameter("@ToSessid", ToSessid);
            prms[3] = new SqlParameter("@PageIndex", 1);
            prms[4] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[5] = new SqlParameter("@IsExport", "Y");
            prms[6] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetPool4IncomeReport", prms);
            Session["Pool4IncomeReport"] = Ds.Tables[0];
            ExportExcel();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["Pool4IncomeReport"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Pool4IncomeReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Pool4IncomeReport.xlsx");
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
            throw new Exception(ex.Message);
        }
    }
    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData1.PageIndex = e.NewPageIndex;
            GvData1.DataSource = Session["Pool4IncomeReport"];
            GvData1.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}
