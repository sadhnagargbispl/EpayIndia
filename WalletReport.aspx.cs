using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WalletReport : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    DataSet Ds = new DataSet();
    string IsoStart;
    string IsoEnd;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.BtnShow.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnShow));
            string searchtext = Session["Search"] as string;
            if (!IsPostBack)
            {
                if (Session["AStatus"] != null)
                {
                    GvData.Visible = false;
                    gvContainer.Visible = false;
                    Session["WalletData"] = null;
                    CreateOrAlter_sp_GetWalletBalanceDetail();
                    Filldate();
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public void CreateOrAlter_sp_GetWalletBalanceDetail()
    {
        string sql =
            "CREATE OR ALTER PROCEDURE dbo.sp_GetWalletBalanceDetail " +
            "( " +
            " @IDNo NVARCHAR(200), " +
            " @PageIndex INT = 1, " +
            " @PageSize INT = 10, " +
            " @IsExport CHAR(1), " +
            " @FromDate DATETIME, " +
            " @ToDate DATETIME, " +
            " @RecordCount INT OUTPUT " +
            ") AS BEGIN SET NOCOUNT ON; " +
            "DECLARE @SQL NVARCHAR(MAX)='', @Cols NVARCHAR(MAX)=''; " +

            "SELECT @Cols=@Cols+" +
            "' ,SUM(CASE WHEN vt.actype='''+CAST(actype AS VARCHAR)+''' AND t.VType=''C'' THEN cast((t.Amount) as decimal(16,4)) ELSE 0 END) AS ['+WalletName+'-Credit]'" +
            "+' ,SUM(CASE WHEN vt.actype='''+CAST(actype AS VARCHAR)+''' AND t.VType=''D'' THEN cast((t.Amount) as decimal(16,4)) ELSE 0 END) AS ['+WalletName+'-Debit]'" +
            "+' ,SUM(CASE WHEN vt.actype='''+CAST(actype AS VARCHAR)+''' THEN CASE WHEN t.VType=''C'' THEN cast((t.Amount) as decimal(16,4)) ELSE -cast((t.Amount) as decimal(16,4)) END ELSE 0 END) AS ['+WalletName+'-Balance]' " +
            "FROM " + objDAL.DBName + "..VoucherType WHERE ActiveStatus='Y'; " +

            "IF OBJECT_ID('tempdb..##Results') IS NOT NULL DROP TABLE ##Results; " +

            "SET @SQL='SELECT ROW_NUMBER() OVER(ORDER BY b.IdNo) AS SNo, " +
            "b.IdNo AS [Member ID], " +
            "b.MemFirstname+'' ''+b.MemLastName AS [Member Name] '+@Cols+' INTO ##Results " +
            "FROM " + objDAL.DBName + "..TrnVoucher t " +
            "INNER JOIN " + objDAL.DBName + "..VoucherType vt ON vt.Actype=t.Actype " +
            "INNER JOIN " + objDAL.DBName + "..M_MemberMaster b ON b.Formno=CASE WHEN t.CrTo<>0 THEN t.CrTo ELSE t.DrTo END " +
            "WHERE CAST(t.VoucherDate AS DATE) BETWEEN '''+CONVERT(VARCHAR,@FromDate,23)+''' AND '''+CONVERT(VARCHAR,@ToDate,23)+''' " +
            "AND ('''+@IDNo+'''=''0'' OR b.IdNo='''+@IDNo+''') " +
            "GROUP BY b.IdNo,b.MemFirstname,b.MemLastName'; " +

            "EXEC(@SQL); " +

            "IF @IsExport='N' BEGIN " +
            "SELECT * FROM ##Results WHERE SNo BETWEEN (@PageIndex-1)*@PageSize+1 AND (@PageIndex*@PageSize); " +
            "SELECT @RecordCount=COUNT(*) FROM ##Results; END " +
            "ELSE BEGIN SELECT * FROM ##Results; END " +
            "SELECT @RecordCount as RecordCount; " +
            "DROP TABLE ##Results; END";

        using (SqlConnection con = new SqlConnection(constr1))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
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
    private string GetFormNo()
    {
        string formno = "";
        try
        {

            string idNo = txtMemberId.Text.Trim();
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
        catch (Exception ex)
        {
            return "";
            throw new Exception(ex.Message);

        }
        return formno;
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["WalletData1"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "WalletBalance");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=WalletBalanceReport.xlsx");
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
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            string Idno = "0";
            if (!string.IsNullOrEmpty(txtMemberId.Text))
            {
                Idno = txtMemberId.Text;
            }
            else
            {
                Idno = "0";
            }
            string str = objDAL.IsoStart + " exec sp_GetWalletBalanceDetail '" + Idno.ToLower() + "','1','100000','Y','" + txtStartDate.Text + "', '" + txtEndDate.Text + "',0" + objDAL.IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str);
            Session["WalletData1"] = Ds.Tables[0];
            ExportExcel();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public void BindData(int PageIndex)
    {
        try
        {
            string Idno = "0";

            lblErr.Text = "";
            lblCount.Text = "";

            GvData.DataSource = null;
            GvData.DataBind();

            if (!string.IsNullOrEmpty(txtMemberId.Text))
            {
                Idno = txtMemberId.Text;
            }
            // Start Date
            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                txtStartDate.Text = Session["CompDate"].ToString();
            }

            // End Date
            if (string.IsNullOrEmpty(txtEndDate.Text))
            {
                txtEndDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }

            SqlParameter[] prms = new SqlParameter[7];

            prms[0] = new SqlParameter("@IdNo", Idno.ToLower());
            prms[1] = new SqlParameter("@PageIndex", PageIndex);
            prms[2] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[3] = new SqlParameter("@IsExport", "N");
            prms[4] = new SqlParameter("@FromDate", txtStartDate.Text);
            prms[5] = new SqlParameter("@ToDate", txtEndDate.Text);

            prms[6] = new SqlParameter("@RecordCount", SqlDbType.Int);
            prms[6].Direction = ParameterDirection.Output;

            Ds = SqlHelper.ExecuteDataset(constr1,"sp_GetWalletBalanceDetail",prms);

            GvData.DataSource = Ds.Tables[0];
            GvData.DataBind();

            int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);

            Session["WalletData"] = Ds.Tables[0];

            ViewState["Sno"] = "Formno";
            ViewState["Sort_Order"] = "ASC";

            if (recordCount > 0)
            {
                for (int i = 0; i < GvData.Columns.Count; i++)
                {
                    TableCell tableCell = GvData.HeaderRow.Cells[i];

                    Image img = new Image();
                    img.ImageUrl = "~/Images/Uparrow.png";

                    tableCell.Controls.Add(new LiteralControl("&nbsp;"));
                    tableCell.Controls.Add(img);
                }

                GvData.Visible = true;
                gvContainer.Visible = true;
                lblCount.Text = "Total : " + recordCount;
            }
            else
            {
                GvData.Visible = false;
                gvContainer.Visible = false;
                lblErr.Text = "No Record Found!!";
            }

            //PopulatePager(recordCount, PageIndex);
        }
        catch (Exception ex)
        {
            // optionally log error
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
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            BindData(1);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
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
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

}
