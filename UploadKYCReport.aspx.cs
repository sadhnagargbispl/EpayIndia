using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UploadKYCReport : System.Web.UI.Page
{
    DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        DAL objDAL = new DAL();
        string searchtext = Session["Search"] as string;
        if (DDlSerchBy.SelectedValue == "Y")
        {
            DivStartDate.Visible = true;
            DivENDDate.Visible = true;
        }
        else if (DDlSerchBy.SelectedValue == "R")
        {
            DivStartDate.Visible = true;
            DivENDDate.Visible = true;
        }
        else if (DDlSerchBy.SelectedValue == "P")
        {
            DivStartDate.Visible = true;
            DivENDDate.Visible = true;
        }
        else if (DDlSerchBy.SelectedValue == "N")
        {
            DivStartDate.Visible = false;
            DivENDDate.Visible = false;
        }
        else
        {
            DivStartDate.Visible = true;
            DivENDDate.Visible = true;
        }
        if (!Page.IsPostBack)
        {
            GvData.Visible = false;
            gvContainer.Visible = false;
            Session["PairData"] = null;
        }
    }
    private string GetFormNo()
    {
        objDAL = new DAL();
        string idNo;
        string formno = string.Empty;
        idNo = TxtMember.Text.Trim();
        string qry = "Select FormNo from " + objDAL.tblMemberMaster + " where IdNo='" + idNo + "'";
        DataTable dt = new DataTable();
        dt = objDAL.GetData(qry);

        if (dt.Rows.Count > 0)
        {
            formno = dt.Rows[0]["FormNo"].ToString();
        }
        else
        {
            lblErr.Text = "Member Id does not exist. Please check it once and then enter it again.";
            lblErr.Visible = true;
            TxtMember.Text = string.Empty;
        }
        return formno;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = Session["PairData"] as DataTable;
        using (var wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "KYCReport");
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=KYCReport.xlsx");

            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
    public void BindData()
    {
        lblErr.Text = "";
        lblCount.Text = "";
        string Condition = "";
        string Condition1 = "";
        string qry = "";
        string formno = "0";

        if (TxtMember.Text != "")
        {
            formno = TxtMember.Text;
        }
        qry = "Exec sp_GetKYCDetail '" + formno + "','" + txtStartDate.Text + "','" + txtEndDate.Text + "','" + DDlSerchBy.SelectedValue + "','" + RbtSummary.SelectedValue + "' ";
        DataTable dtData = new DataTable();
        dtData = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
        if (dtData.Rows.Count > 0)
        {
            if (RbtSummary.SelectedValue == "S")
            {
                GvData.DataSource = dtData;
                GvData.DataBind();
                GvData.Visible = true;
                GridView1.Visible = false;
            }
            else
            {
                GridView1.DataSource = dtData;
                GridView1.DataBind();
                GridView1.Visible = true;
                GvData.Visible = false;
            }

            Session["PairData"] = dtData;
            gvContainer.Visible = true;
            lblCount.Text = "Total : " + dtData.Rows.Count;
        }
        else
        {
            gvContainer.Visible = false;
            lblErr.Text = "No Record Found!!";
        }
    }
    protected void DDlSerchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
    protected void RbtSummary_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = Session["PairData"];
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvData.PageIndex = e.NewPageIndex;
        GvData.DataSource = Session["PairData"];
        GvData.DataBind();
    }
}