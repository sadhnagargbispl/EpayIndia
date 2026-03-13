using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WalletTransactionReport : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    DataSet Ds = new DataSet();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string searchtext = Session["Search"] as string;
            if (!IsPostBack)
            {
                if (Session["AStatus"] != null)
                {
                    GvData.Visible = false;
                    gvContainer.Visible = false;
                    Session["AccountData"] = null;
                    FillWallet();
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
        }

    }
    private void FillWallet()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr, "sp_GetWallettype");
            RbtWalletType.DataSource = Ds.Tables[0];
            RbtWalletType.DataValueField = "Actype";
            RbtWalletType.DataTextField = "Walletname";
            RbtWalletType.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    private void Filldate()
    {
        try
        {
            objDAL = new DAL();
            string Str = objDAL .IsoStart +"Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate " + objDAL.IsoEnd  ;
            dtData = new DataTable();
            dtData = objDAL.GetData(Str);
            if (dtData.Rows.Count > 0)
            {
                txtStartDate.Text = dtData.Rows[0]["CurrentDate"].ToString();
                txtEndDate.Text = dtData.Rows[0]["CurrentDate"].ToString();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    private string GetFormNo()
    {
        string formno = "";
        try
        {  
            string idNo  = txtMemberId.Text.Trim(); 
            
           
            string qry = objDAL.IsoStart + "Select FormNo from " + objDAL.DBName + "..M_MemberMaster where IdNo='" + idNo + "'" + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                formno = Dt.Rows[0]["FormNo"].ToString();
            }
            else
            {
                lblErr.Text = "Member Id does not exist. Please check it once and then enter it again.";
                lblErr.Visible = true;
                txtMemberId.Text = "";
            }
            
        }
        catch (Exception Ex)
        {
           
            throw new Exception(Ex.Message);
            
        }
        return formno;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        lblErr.Text = "";
        lblCount.Text = "";
        try
        {
            string AcType = RbtWalletType.SelectedValue;
            string Idno = "0";
            string VoucherNo = "0";

            if (!string.IsNullOrEmpty(txtMemberId.Text))
            {
                Idno = GetFormNo();
            }
            else
            {
                Idno = "0";
            }

            DateTime startDate;
            DateTime endDate;
            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                startDate = Convert.ToDateTime(Session["CompDate"]);
            }
            else
            {
                startDate = DateTime.Parse(txtStartDate.Text);
            }

            if (string.IsNullOrEmpty(txtEndDate.Text))
            {
                endDate = DateTime.Now;
            }
            else
            {
                endDate = DateTime.Parse(txtEndDate.Text);
            }

            if (!string.IsNullOrEmpty(txtVoucherNo.Text))
            {
                VoucherNo = txtVoucherNo.Text.Trim();
            }
            else
            {
                VoucherNo = "0";
            }

            GvData.DataSource = null;
            GvData.DataBind();
            SqlParameter[] prms = new SqlParameter[9];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@WalletType", AcType);
            prms[2] = new SqlParameter("@StartDate", startDate);
            prms[3] = new SqlParameter("@EndDate", endDate);
            prms[4] = new SqlParameter("@PageIndex", 1);
            prms[5] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[6] = new SqlParameter("@IsExport", "Y");
            prms[7] = new SqlParameter("@VoucherNo", VoucherNo);
            prms[8] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetWalletTransactionDetail", prms);
            Session["GData1"] = Ds.Tables[0];
            ExportExcel();
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
        }
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["GData1"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "WalletTransactionReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=WalletTransactionReport.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
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
            BindData(1);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
        }
    }
    public void BindData(int PageIndex)
    {
        
        try
        {
            lblErr.Text = "";
            lblCount.Text = "";
            string AcType = RbtWalletType.SelectedValue;
            string Idno = "0";
            string VoucherNo = "0";

            if (!string.IsNullOrEmpty(txtMemberId.Text))
            {
                Idno = GetFormNo();
            }
            else
            {
                Idno = "0";
            }

            DateTime startDate;
            DateTime endDate;
            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                startDate = Convert.ToDateTime(Session["CompDate"]);
            }
            else
            {
                startDate = DateTime.Parse(txtStartDate.Text);
            }

            if (string.IsNullOrEmpty(txtEndDate.Text))
            {
                endDate = DateTime.Now;
            }
            else
            {
                endDate = DateTime.Parse(txtEndDate.Text);
            }

            if (!string.IsNullOrEmpty(txtVoucherNo.Text))
            {
                VoucherNo = txtVoucherNo.Text.Trim();
            }
            else
            {
                VoucherNo = "0";
            }

            GvData.DataSource = null;
            GvData.DataBind();
            SqlParameter[] prms = new SqlParameter[9];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@WalletType", AcType);
            prms[2] = new SqlParameter("@StartDate", startDate);
            prms[3] = new SqlParameter("@EndDate", endDate);
            prms[4] = new SqlParameter("@PageIndex", PageIndex);
            prms[5] = new SqlParameter("@PageSize", 100000000);
            prms[6] = new SqlParameter("@IsExport", "N");
            prms[7] = new SqlParameter("@VoucherNo", VoucherNo);
            prms[8] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetWalletTransactionDetail", prms);
   
            GvData.DataSource = Ds.Tables[0];
            GvData.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GvData.DataBind();
            int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);
            Session["AccountData"] = Ds.Tables[0];
            dtData= Ds.Tables[0];

            ViewState["IdNo"] = "IdNo";
            ViewState["Sort_Order"] = "ASC";
            if (Ds.Tables[0].Rows.Count > 0)
            {
                lblCount.Text = "Total Record: " + Ds.Tables[1].Rows[0]["RecordCount"];
                LblCredit.Text = "Credit: " + Ds.Tables[1].Rows[0]["Credit"];
                lblDebit.Text = "Debit: " + Ds.Tables[1].Rows[0]["Debit"];
                LblBalance.Text = "Balance: " + Ds.Tables[1].Rows[0]["Balance"];
                //dtData.Compute("Sum(Amount)", "")
                GvData.Visible = true;
                gvContainer.Visible = true;
            }
            else
            {
                lblErr.Text = "No Record Found!!";
                GvData.Visible = false;
                gvContainer.Visible = false;
            }

        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            BindData(1);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
        }
    }

    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindData(1);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
        }
    }

}
