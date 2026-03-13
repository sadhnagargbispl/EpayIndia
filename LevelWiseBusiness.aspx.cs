using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class LevelWiseBusiness : System.Web.UI.Page
{
    private DataTable dtData = new DataTable();
    private DataTable Dt = new DataTable();
    DataSet  Ds = new DataSet();
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private void Page_Load(object sender, EventArgs e)
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
    protected void FillLevel()
    {

        string Formno = "";
        if (string.IsNullOrEmpty(txtMember.Text))
        {
            Formno = "0";
        }
        else
        {
            Formno = GetFormNo().ToString();
        }

        try
        {
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@FormNo", Formno);
            prms[1] = new SqlParameter("@type", "N");
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_GetLevelBusiness", prms);
            DdlLevel.DataSource = Ds.Tables[0];
            DdlLevel.DataTextField = "LevelName";
            DdlLevel.DataValueField = "MLevel";
            DdlLevel.DataBind();
            // Conn.Close();
            LevelDetail(1);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string GetFormNo()
    {
        DAL obj = new DAL();
        string idNo;
        string formno = "";
        idNo = txtMember.Text;
        idNo = idNo.Trim();
        string qry = objDal.IsoStart + "Select FormNo from " + objDal.DBName + ".. M_MemberMaster  where IdNo='" + idNo + "'" + objDal.IsoEnd;
        Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
        if (Dt.Rows.Count > 0)
        {
            formno = Dt.Rows[0]["FormNo"].ToString();
        }
        else
        {
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again.";
            lblErr.Visible = true;
            txtMember.Text = "";
        }
        return formno;

    }
        
        public void LevelDetail(int pageIndex)
    {
        try
        {
            string level = DdlLevel.SelectedValue;
            string Formno = "";

            Formno = txtMember.Text != "" ? txtMember.Text : "0";

            {
                Formno = GetFormNo();
            }
            SqlParameter[] prms = new SqlParameter[6];
            prms[0] = new SqlParameter("@FormNo", Formno);
            prms[1] = new SqlParameter("@MLevel", level);
            prms[2] = new SqlParameter("@PageIndex", pageIndex);
            prms[3] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[4] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            prms[5] = new SqlParameter("@IsExport", "N");
            Ds = SqlHelper.ExecuteDataset(constr, "sp_LevelBusinessReport", prms);
            GvData.DataSource = Ds.Tables[0];
            GvData.DataBind();
            Session["IssuedPinValue"] = Ds.Tables[0];

            int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);
            Session["GData"] = Ds.Tables[0];
            ViewState["IdNo"] = "IdNo";
            ViewState["Sort_Order"] = "ASC";

            if (Ds.Tables[0].Rows.Count > 0)
            {
                lbltotal.Text = "Total Record: " + Ds.Tables[1].Rows[0]["RecordCount"];
                GvData.Visible = true;
            }
            else
            {
                lbltotal.Text = "No Record Found!!";
                GvData.Visible = false;
            }
           
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
            LevelDetail(1);
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
            string level = DdlLevel.SelectedValue;
            string Formno = "";
            Formno = txtMember.Text != "" ? txtMember.Text : "0";
           
            {
                Formno = GetFormNo().ToString();
            }
            SqlParameter[] prms = new SqlParameter[6];
            prms[0] = new SqlParameter("@FormNo", Formno);
            prms[1] = new SqlParameter("@MLevel", level);
            prms[2] = new SqlParameter("@PageIndex", 1);
            prms[3] = new SqlParameter("@PageSize", int.Parse(ddlPageSize.SelectedValue));
            prms[4] = new SqlParameter("@RecordCount", SqlDbType.Int);
            prms[4].Direction = ParameterDirection.Output;
            prms[5] = new SqlParameter("@IsExport", "Y");

            Ds = SqlHelper.ExecuteDataset(constr1, "sp_LevelBusinessReport", prms);
            Session["GData1"] = Ds.Tables[0];
            ExportExcel();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "Error In Exporting File");
        }
    }
    private void ExportExcel()
    {
        try
        {
            DataTable dt = (DataTable)Session["GData1"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "LevelWiseBusiness");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=LevelWiseBusiness.xlsx");
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
    protected void txtMember_TextChanged(object sender, EventArgs e)
    {
        string Formno = "";
        Formno = GetFormNo();
        if (Formno == "0")
        {
        }
        else
        {
            FillLevel();

        }
       
    }
   
protected void BtnSubmit_Click1(object sender, EventArgs e)
    {
        try
        {
            LevelDetail(1);
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
