using ClosedXML.Excel;
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

public partial class BonanzaReport : System.Web.UI.Page
{
    private DataTable dtData = new DataTable();
    private DataTable Dt = new DataTable();
    DataSet Ds;
    private DAL objDAL = new DAL();
    DAL Obj = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            objDAL = new DAL();
            if (!Page.IsPostBack)
            {
                txtMemId.Text = "";
                GvData.Visible = false;
                FillBonanza();
                if (Session["AStatus"] != null)
                   
                {
                    // Your C# code here
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
        BindData(1);
    }

    private string Get_IDNo(string MyFormNo)
    {
        try
        {
            string IdNo = "";
            DataTable dt = new DataTable();
            DataSet Ds = new DataSet();
            string strSql = objDAL.IsoStart + "select Formno  from " + objDAL.DBName + "..M_MemberMaster WHERE Idno='" + MyFormNo + "' " + objDAL.IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strSql);
            dt = Ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                IdNo = dt.Rows[0]["Formno"].ToString();
            }
            return IdNo;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "SideB");
            return "";
        }
    }

    private void FillBonanza()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetBonanza");
            CmbKit.DataSource = Ds.Tables[0];
            CmbKit.DataValueField = "offerid";
            CmbKit.DataTextField = "offername";
            CmbKit.DataBind();
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
            string FromSessid = "0";
            string ToSessid = "0";
            string Idno = "0";

            string cmdkit = "";
            string status = "";
            if (!string.IsNullOrEmpty(txtMemId.Text))
            {
                Idno = Get_IDNo(txtMemId.Text);
            }
            else
            {
                Idno = "0";
            }
            if (!string.IsNullOrEmpty(CmbKit.SelectedItem.Text))
            {
                cmdkit = CmbKit.SelectedValue;
            }
            GvData.DataSource = null;
            GvData.DataBind();
            SqlParameter[] prms = new SqlParameter[6];
            prms[0] = new SqlParameter("@IDNo", Idno.ToLower());
            prms[1] = new SqlParameter("@Bonanza", int.Parse(CmbKit.SelectedValue));
            prms[2] = new SqlParameter("@PageIndex", PageIndex);
            prms[3] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[4] = new SqlParameter("@IsExport", "N");
            prms[5] = new SqlParameter("@RecordCount", ParameterDirection.Output);

            Ds = SqlHelper.ExecuteDataset(constr1, "sp_NepalBonanzaNew", prms);
            GvData.DataSource = Ds.Tables[0];
            GvData.DataBind();
            int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);
            Session["GData"] = Ds.Tables[0];
            ViewState["IdNo"] = "IdNo";
            ViewState["Sort_Order"] = "ASC";

            if (Ds.Tables[0].Rows.Count > 0)
            {
                Label1.Text = "Total Record: " + Ds.Tables[1].Rows[0]["RecordCount"];
                GvData.Visible = true;
            }
            else
            {
                lblError.Text = "No Record Found!!";
                GvData.Visible = false;
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
            string FromSessid = "0";
            string ToSessid = "0";
            string Idno = "0";

            string cmdkit = "";
            string status = "";
            if (!string.IsNullOrEmpty(txtMemId.Text))
            {
                Idno = Get_IDNo(txtMemId.Text);
            }
            else
            {
                Idno = "0";
            }
            if (!string.IsNullOrEmpty(CmbKit.SelectedItem.Text))
            {
                cmdkit = CmbKit.SelectedValue;
            }
            GvData.DataSource = null;
            GvData.DataBind();
            SqlParameter[] prms = new SqlParameter[6];
            prms[0] = new SqlParameter("@IDNo", Convert.ToString(Idno).ToLower());
            prms[1] = new SqlParameter("@Bonanza", int.Parse(CmbKit.SelectedValue));
            prms[2] = new SqlParameter("@PageIndex", 1);
            prms[3] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[4] = new SqlParameter("@IsExport", "N");
            prms[5] = new SqlParameter("@RecordCount", ParameterDirection.Output);

            if (cmdkit == "1002")
            {
                Ds = SqlHelper.ExecuteDataset(constr1, "sp_DubaiBonanzaNew", prms);
            }
            else
            {
                Ds = SqlHelper.ExecuteDataset(constr1, "sp_NepalBonanzaNew", prms);
            }

            Session["GData1"] = Ds.Tables[0];
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
            DataTable dt = (DataTable)Session["GData1"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "BonanzaReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=BonanzaReport.xlsx");
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