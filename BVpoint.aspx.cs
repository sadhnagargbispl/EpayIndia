using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BVpoint : System.Web.UI.Page
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
        string sql = "SELECT CAST(A.BID AS VARCHAR) AS BID, a.Sessid,Replace(Convert(Varchar,a.RectimeStamp,106),' ','-')as Date, b.Idno, " +
                     "(b.MemFirstName + ' ' + b.MemLastName) AS MemberName, " +
                     "CASE WHEN a.LegNo = 1 THEN 'Left' ELSE 'Right' END AS LegNo, " +
                     "a.PV,Case when a.BVType='S' then 'Self' else 'Tree' end as BvType, a.Remark, " +
                     "CASE WHEN a.ActiveStatus = 'Y' THEN 'Active' ELSE 'Deactive' END AS Status " +
                     "FROM TrnBV AS a, M_memberMaster AS b " +
                     "WHERE a.Formno = b.Formno " +
                     "ORDER BY  a.RectimeStamp DESC";

        DataTable dtData = new DataTable();
        dtData = objDAL.GetData(sql);
        GvData.DataSource = dtData;
        GvData.DataBind();
        Session["GData"] = dtData;

        if (dtData.Rows.Count > 0)
        {
            btnExport.Enabled = true;
            btnPrintAll.Enabled = true;
            btnPrintCurrent.Enabled = true;
        }
        else
        {
            btnExport.Enabled = false;
            btnPrintAll.Enabled = false;
            btnPrintCurrent.Enabled = false;
        }
    }
    protected void btnShowRecord_Click(object sender, EventArgs e)
    {
        BindData();
        btnShowRecord.Visible = false;
        lblView.Visible = false;
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvData.PageIndex = e.NewPageIndex;
        GvData.DataSource = Session["GData"];
        GvData.DataBind();
    }
    protected void GvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string Sessid = GvData.DataKeys[e.Row.RowIndex].Value.ToString();
            LinkButton GrpId;
            Label LblDeacTive;

            if (Convert.ToInt32(Sessid) == Convert.ToInt32(Session["CurrentSessn"]))
            {
                GrpId = (LinkButton)e.Row.Cells[8].FindControl("LBDelete");
                if (e.Row.Cells[7].Text.ToLower() == "deactive")
                {
                    GrpId.Visible = false;
                }
                else
                {
                    GrpId.Visible = true;
                }
            }
            else
            {
                GrpId = (LinkButton)e.Row.Cells[8].FindControl("LBDelete");
                GrpId.Visible = false;
            }
        }
    }
    protected void DeleteGroup(object sender, EventArgs e)
    {
        string BankCode, scrname;
        DataTable dt;
        GridViewRow GVRw;

        GVRw = (GridViewRow)((Control)sender).Parent.Parent;
        BankCode = ((Label)GVRw.FindControl("LblGrpID")).Text;

        string str = "SELECT * FROM TrnBV WHERE BId='" + Convert.ToInt32(BankCode) +
                     "' AND Sessid='" + Convert.ToInt32(Session["CurrentSessn"]) + "'";

        dt = new DataTable();
        dt = objDAL.GetData(str);

        if (dt.Rows.Count > 0)
        {
            string Sql = "UPDATE TrnBV SET ActiveStatus='N' WHERE BId='" + Convert.ToInt32(BankCode) + "'";
            int updateEffect = objDAL.UpdateData(Sql);

            if (updateEffect != 0)
            {
                scrname = "<SCRIPT language='javascript'>alert('Deleted Successfully!');</SCRIPT>";
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Not able to delete the selected Group!');</SCRIPT>";
            }

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Group Deletion", scrname, false);
            BindData();
        }
        else
        {
            scrname = "<SCRIPT language='javascript'>alert('This Record Cannot Be Deleted!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Group Deletion", scrname, false);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment; filename=BVDetails.xls");
        Response.ContentType = "application/ms-excel";

        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);

        GvData.AllowPaging = false;
        GvData.GridLines = GridLines.Both;

        DataTable dtData = new DataTable();
        dtData = (DataTable)Session["GData"];
        GvData.DataSource = dtData;
        GvData.DataBind();

        // Change the header row style
        GvData.HeaderRow.Style.Add("background-color", "#FFFFFF");

        // Apply styles to the header cells
        for (int i = 0; i <GvData.HeaderRow.Cells.Count; i++)
        {
            GvData.HeaderRow.Cells[i].Style.Add("background-color", "#D8A38D");
        }

        // Remove modify and delete columns from the grid
        GvData.HeaderRow.Cells[GvData.HeaderRow.Cells.Count - 1].Visible = false;
        GvData.HeaderRow.Cells[GvData.HeaderRow.Cells.Count - 2].Visible = false;

        for (int i = 0; i <GvData.Rows.Count; i++)
        {
        //GvData.Rows[i].Cells[GvData.HeaderRow.Cells.Count - 1].Visible = false;
        //GvData.Rows[i].Cells[GvData.HeaderRow.Cells.Count - 2].Visible = false;
        }

        // Apply styles to the data rows
        for (int i = 0; i < GvData.Rows.Count; i++)
        {
            for (int j = 0; j < GvData.Rows[i].Cells.Count; j++)
            {
                GvData.Rows[i].Cells[j].Style.Add("background-color", "#FFFFFF");
                GvData.Rows[i].Cells[j].Style.Add("color", "#000000");
            }
        }

        GvData.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
    }
    protected void btnPrintCurrent_Click(object sender, EventArgs e)
    {
        GvData.AllowPaging = false;
        GvData.GridLines = GridLines.Both;

        DataTable dtData = new DataTable();
        dtData = (DataTable)Session["GData"];
        GvData.DataSource = dtData;
        GvData.DataBind();
        GvData.PagerSettings.Visible = false;

        GvData.BorderStyle = BorderStyle.Solid;
        GvData.BorderColor = System.Drawing.Color.Black;

        // Remove modify and delete columns from the grid
        GvData.HeaderRow.Cells[GvData.HeaderRow.Cells.Count - 1].Visible = false;
        GvData.HeaderRow.Cells[GvData.HeaderRow.Cells.Count - 2].Visible = false;

        for (int i = 0; i < GvData.Rows.Count; i++)
        {
            GvData.Rows[i].Cells[GvData.HeaderRow.Cells.Count - 1].Visible = false;
            GvData.Rows[i].Cells[GvData.HeaderRow.Cells.Count - 2].Visible = false;
        }

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        GvData.RenderControl(hw);

        string gridHTML = sw.ToString().Replace("\"", "'").Replace(Environment.NewLine, "");

        StringBuilder sb = new StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("window.onload = new function(){");
        sb.Append("var printWin = window.open('', '', 'left=0,top=0,width=1000,height=600,status=0');");
        sb.Append("printWin.document.write(\"");
        sb.Append(gridHTML);
        sb.Append("\");");
        sb.Append("printWin.document.close();");
        sb.Append("printWin.focus();");
        sb.Append("printWin.print();");
        sb.Append("printWin.close();};");
        sb.Append("</script>");

        ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());

        GvData.AllowPaging = true;
        GvData.PagerSettings.Visible = true;

        dtData = new DataTable();
        dtData = (DataTable)Session["GData"];
        GvData.DataSource = dtData;
        GvData.DataBind();
    }
    protected void btnPrintAll_Click(object sender, EventArgs e)
    {
        GvData.AllowPaging = false;
        GvData.GridLines = GridLines.Both;

        DataTable dtData = new DataTable();
        dtData = (DataTable)Session["GData"];
        GvData.DataSource = dtData;
        GvData.DataBind();
        GvData.PagerSettings.Visible = false;

        GvData.BorderStyle = BorderStyle.Solid;
        GvData.BorderColor = System.Drawing.Color.Black;

        // Remove modify and delete columns from the grid
        GvData.HeaderRow.Cells[GvData.HeaderRow.Cells.Count - 1].Visible = false;
        GvData.HeaderRow.Cells[GvData.HeaderRow.Cells.Count - 2].Visible = false;

        for (int i = 0; i < GvData.Rows.Count; i++)
        {
            GvData.Rows[i].Cells[GvData.HeaderRow.Cells.Count - 1].Visible = false;
            GvData.Rows[i].Cells[GvData.HeaderRow.Cells.Count - 2].Visible = false;
        }

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        GvData.RenderControl(hw);

        string gridHTML = sw.ToString().Replace("\"", "'").Replace(Environment.NewLine, "");

        StringBuilder sb = new StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("window.onload = new function(){");
        sb.Append("var printWin = window.open('', '', 'left=0,top=0,width=1000,height=1000,status=0');");
        sb.Append("printWin.document.write(\"");
        sb.Append(gridHTML);
        sb.Append("\");");
        sb.Append("printWin.document.close();");
        sb.Append("printWin.focus();");
        sb.Append("printWin.print();");
        sb.Append("printWin.close();};");
        sb.Append("</script>");

        ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());

        GvData.AllowPaging = true;
        GvData.PagerSettings.Visible = true;

        dtData = new DataTable();
        dtData = (DataTable)Session["GData"];
        GvData.DataSource = dtData;
        GvData.DataBind();
    }

    protected void BtnAddNew_Click(object sender, EventArgs e)
    {
        
       try
        {
            Response.Redirect("AddBVPoint.aspx", false);
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
}
