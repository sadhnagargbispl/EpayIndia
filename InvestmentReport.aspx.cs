using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InvestmentReport : System.Web.UI.Page
{
    DAL objDAL = new DAL();
    DataTable dtData = new DataTable();
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
                    //Filldate();
                    Fillkit();
                    FillStackList();
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
    private void Fillkit()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr, "sp_ddlPageSize");
            ddlPageSize.DataSource = Ds.Tables[0];
            ddlPageSize.DataValueField = "ddlPageSize";
            ddlPageSize.DataTextField = "ddlPageSize";
            ddlPageSize.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FillStackList()
    {
        try
        {
            //Ds = SqlHelper.ExecuteDataset(constr1, "sp_ddlStacktype");
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_FillKit");
            DDLStackType.DataSource = Ds.Tables[0];
            DDLStackType.DataValueField = "kitid";
            DDLStackType.DataTextField = "Name";
            DDLStackType.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
    public void BindData(int PageIndex)
    {
        lblError.Text = "";
        try
        {
            string FromSessid = "";
            string ToSessid = "";
            string Idno = "0";
            //if (txtStartDate.Text == "")
            //{
            //    Session["CompDate"] = "12-Feb-2018";
            //}

            FromSessid = txtStartDate.Text != "" ? txtStartDate.Text : Session["CompDate"].ToString();
            ToSessid = txtEndDate.Text != "" ? txtEndDate.Text : DateTime.Now.ToString("dd-MMM-yyyy");
            Idno = txtMemId.Text != "" ? txtMemId.Text : "0";

            GvData1.DataSource = null;
            GvData1.DataBind();
            SqlParameter[] prms = new SqlParameter[8];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@FromSessid", FromSessid);
            prms[2] = new SqlParameter("@ToSessid", ToSessid);
            prms[3] = new SqlParameter("@PageIndex", PageIndex);
            prms[4] = new SqlParameter("@PageSize", 100000000);
            prms[5] = new SqlParameter("@IsExport", "N");
            prms[6] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            prms[7] = new SqlParameter("@StackType", DDLStackType.SelectedValue);
            //Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetInvestmentDetail_", prms);
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetInvestmentDetailNew", prms);

            GvData1.DataSource = Ds.Tables[0];
            GvData1.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GvData1.DataBind();
            int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);
            Session["InvestmentReport"] = Ds.Tables[0];
            ViewState["IdNo"] = "IdNo";
            ViewState["Sort_Order"] = "ASC";

            if (Ds.Tables[0].Rows.Count > 0)
            {
                lblCount.Text = "Total Record: " + Ds.Tables[1].Rows[0]["RecordCount"].ToString();
                if (Ds.Tables[1].Rows[0]["Investment"].ToString() == "")
                {
                    lblinv.Text = "Total Subscription: " + "0.00";
                }
                else
                {
                    lblinv.Text = "Total Subscription: " + Ds.Tables[1].Rows[0]["Investment"].ToString();
                }
                if (Ds.Tables[1].Rows[0]["Reinvestment"].ToString() == "")
                {
                    lblreinvetment.Text = "Total Monthly Activation: " + "0.00";
                }
                else
                {
                    lblreinvetment.Text = "Total Monthly Activation: " + Ds.Tables[1].Rows[0]["Reinvestment"].ToString();
                }
                //if (Ds.Tables[1].Rows[0]["Vartualinvestment"].ToString() == "")
                //{
                //    LvlVartualinvestment.Text = "Total Vartual Investment: " + "0.00";
                //}
                //else
                //{
                //    LvlVartualinvestment.Text = "Total Vartual Investment: " + Ds.Tables[1].Rows[0]["Vartualinvestment"].ToString();
                //}
                GvData1.Visible = true;
                lblCount.Visible = true;
                lblinv.Visible = true;
                lblreinvetment.Visible = true;
                LvlVartualinvestment.Visible = true;
            }
            else
            {
                lblError.Text = "No Record Found!!";
                GvData1.Visible = false;
                lblCount.Visible = false;
                lblinv.Visible = false;
                lblreinvetment.Visible = false;
                LvlVartualinvestment.Visible = false;
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
            SqlParameter[] prms = new SqlParameter[8];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@FromSessid", FromSessid);
            prms[2] = new SqlParameter("@ToSessid", ToSessid);
            prms[3] = new SqlParameter("@PageIndex", 1);
            prms[4] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[5] = new SqlParameter("@IsExport", "Y");
            prms[6] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            prms[7] = new SqlParameter("@StackType", DDLStackType.SelectedValue);
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetInvestmentDetailNew", prms);
            Session["InvestmentReportExcel"] = Ds.Tables[0];
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
            DataTable dt = (DataTable)Session["InvestmentReportExcel"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "InvestmentReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=InvestmentReport.xlsx");
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
    private void Filldate()
    {
        try
        {

            string Str = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate ";
            dtData = new DataTable();
            dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Str).Tables[0];
            if (dtData.Rows.Count > 0)
            {
                txtStartDate.Text = dtData.Rows[0]["CurrentDate"].ToString();
                txtEndDate.Text = dtData.Rows[0]["CurrentDate"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData1.PageIndex = e.NewPageIndex;
            GvData1.DataSource = Session["InvestmentReport"];
            GvData1.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void DDLStackType_SelectedIndexChanged(object sender, EventArgs e)
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
}
