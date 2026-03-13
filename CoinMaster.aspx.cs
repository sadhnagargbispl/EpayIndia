using ClosedXML.Excel;
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
public partial class CoinMaster : System.Web.UI.Page
{
    DAL objDAL = new DAL();
    string IsoStart;
    string IsoEnd;
    ModuleFunction objModuleFun = new ModuleFunction();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!Page.IsPostBack)
            {
                btnShowRecord.Visible = false;
                if (Session["AStatus"] != null && Session["AStatus"].ToString() == "OK")
                {
                    BindData();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }

    public void BindData()
    {
        try
        {
            string FromSessid = "";
            string ToSessid = "";

            FromSessid = txtStartDate.Text != "" ? txtStartDate.Text : Session["CompDate"].ToString();
            ToSessid = txtEndDate.Text != "" ? txtEndDate.Text : DateTime.Now.ToString("dd-MMM-yyyy");

            DataTable Dt = new DataTable();
            string sql = "Select PId,Cast(PId as varchar) as VCTypeId,cast(Profit as Numeric(18,2)) as Coinrate,Month,REPLACE(CONVERT(VARCHAR, rectimestamp , 106),' ', '-') +' '+CONVERT(varchar(15), CAST(rectimestamp AS TIME),100) as CoinDate," +
                         " CASE WHEN statusapi='Y' Then 'Active' ELSE 'DeActive' END AS Status," +
                         " Case when statusapi='N' then 'label label-danger' else 'label label-success' end as StatusClass From "+ objDAL.DBName +"..trnprofit " +
                         " Where 1=1 and cast(RectimeStamp as date)>='" + FromSessid  + "' and cast(RectimeStamp as date)<='" + ToSessid + "' Order by PId Desc";
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            GvData.DataSource = Dt;
            GvData.DataBind();
            Session["GData"] = Dt;
            ViewState["WithDrawDate"] = "PID";
            ViewState["Sort_Order"] = "ASC";

            Dt = new DataTable();
            Dt = objDAL.GetData(sql);
            GvData.DataSource = Dt;
            GvData.DataBind();
            Session["GData"] = Dt;

            if (Dt.Rows.Count > 0)
            {
                btnExport.Enabled = true;
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
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("AddCoin.aspx", false);
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
            GridViewRow GVRw;

            GVRw = (GridViewRow)((Control)sender).Parent.Parent;
            GrpID = ((Label)GVRw.FindControl("LblGrpID")).Text;
            string sql = "Update trnprofit SET statusapi='N',LastModified='De-Activated by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "' WHERE PId='" + GrpID + "' ";
            int updateEffect = objDAL.UpdateData(sql);
            if (updateEffect != 0)
            {
                scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');</SCRIPT>";
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Not able to delete the Coin rate!');</SCRIPT>";
            }
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Complaint Type Deletion", scrname, false);
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        {
            try
            {
                string FromSessid = "";
                string ToSessid = "";

                FromSessid = txtStartDate.Text != "" ? txtStartDate.Text : Session["CompDate"].ToString();
                ToSessid = txtEndDate.Text != "" ? txtEndDate.Text : DateTime.Now.ToString("dd-MMM-yyyy");
                DataTable dt = new DataTable();
                //dt = (DataTable)Session["GData"];
                string sql = "Select cast(Profit as Numeric(18,2)) as Profit,Month,REPLACE(CONVERT(VARCHAR, rectimestamp , 106),' ', '-') +' '+CONVERT(varchar(15), CAST(rectimestamp AS TIME),100) as Date," +
                         " CASE WHEN statusapi='Y' Then 'Active' ELSE 'DeActive' END AS Status" +
                         " From " + objDAL.DBName + "..trnprofit " +
                         " Where 1=1 and cast(RectimeStamp as date)>='" + FromSessid + "' and cast(RectimeStamp as date)<='" + ToSessid + "' Order by PId Desc";
                dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Users");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=CoinMaster-" + DateTime.Now.ToShortDateString() + ".xlsx");
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
    }


    protected void Btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
}
