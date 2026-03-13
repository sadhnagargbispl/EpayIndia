using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminHome : System.Web.UI.Page
{
    //DataTable dtData = new DataTable();
    DAL objDAL = new DAL();
    string IsoStart;
    string IsoEnd;
    ModuleFunction objModuleFun = new ModuleFunction();
    DataTable Dt = new DataTable();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private int CurrentPageIndex;
    private readonly object pagerDataList;
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["AStatus"] != null)
        {
            Session["PageName"] = "Home";
        }
        else
        {
            Response.Redirect("Default.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {

                BindData();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    public void BindData(string Condition = "")
    {
        try
        {

            if (ddlSearch.SelectedValue == "0")
            {
                Condition = "";
            }
            else if (ddlSearch.SelectedValue == "StateName")
            {
                Condition = " Where b.StateName like '%" + txtSrchText.Text + "%'";
            }
            else if (ddlSearch.SelectedValue == "DOJ")
            {
                Condition = " Where Replace(Convert(varchar,a.DOJ,106),' ','-') like '%" + txtSrchText.Text + "%'";
            }
            else if (ddlSearch.SelectedValue == "MemName")
            {
                Condition = " Where (a.MemFirstName like '%" + txtSrchText.Text + "%' OR a.MemFirstName like '%" + txtSrchText.Text + "%') ";
            }
            else if (ddlSearch.SelectedValue == "RMemName")
            {
                Condition = " Where (c.MemFirstName like '%" + txtSrchText.Text + "%' OR c.MemFirstName like '%" + txtSrchText.Text + "%') ";
            }
            else if (ddlSearch.SelectedValue == "KitName")
            {
                Condition = " Where d.KitName like '%" + txtSrchText.Text + "%'";
            }
            else if (ddlSearch.SelectedValue == "walletaddress")
            {
                Condition = " Where a.walletaddress like '%" + txtSrchText.Text + "%'";
            }
            else if (ddlSearch.SelectedValue == "RMemID")
            {
                Condition = " Where e.IDNO = '" + txtSrchText.Text + "' ";
            }
            else if (ddlSearch.SelectedValue == "IDNo")
            {
                Condition = " Where a.IDNO = '" + txtSrchText.Text + "' ";
            }
            else
            {
                Condition = " Where a." + ddlSearch.SelectedValue + " like '%" + txtSrchText.Text + "%'";
            }

            string sql = objDAL.IsoStart + "Select top 25  A.IDNo,A.MemFirstName As MemName,Case when A.LegNo=1 then 'Left' else 'Right' end as Leg,'' as LgnID,'" + Session["CompWeb"] + "' as Site,";
            sql += " IsNull(REPLACE(CONVERT(VARCHAR(11), a.Doj , 106), ' ', '-')+' '+STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' '),'') As Doj,A.City,B.StateName,A.Email,A.Mobl As MobileNo,a.WalletAddress,";
            sql += "A.Passw,C.IDNo As SponsorID,(C.MemFirstName +' '+C.MemLastName) as SponsorName, e.IdNo as UplinerIdNo, (e.MemFirstName+ ''+ e.MemLastName) as UplinerName, ";
            sql += " CASE WHEN DataLength(A.MemPic)>0 THEN '<a href=\"Img.aspx?ID='+ a.IDNO + '\" onclick=\"return hs.htmlExpand(this, { objectType: iframe,width: 470,height: 470,marginTop : 0 } )\" >'+ A.MemFirstName +'</a>'";
            sql += " ELSE  A.MemFirstName  END AS Qstr, Case When A.ActiveStatus='Y' and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status,";
            sql += " CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR(11), a.UpgradeDate , 106), ' ', '-') +' '+STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' '),'') ELSE '' END as UpgrdDate,";
            sql += " D.KitName,D.KitAmount,D.Bv,'Rs 0.00/-' As Balance,a.ActiveStatus,a.IsBlock,a.Remarks as ProposerName From " + objDAL.DBName + "..M_MemberMaster As A Inner Join " + objDAL.DBName + "..M_StateDivMaster As B on A.StateCode=B.StateCode and  b.RowStatus='Y' ";
            sql += " Left Join " + objDAL.DBName + "..M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join " + objDAL.DBName + "..M_MemberMaster As e On A.UpLnFormNo=e.Formno Inner join " + objDAL.DBName + "..M_kitMaster As D On A.KitID=D.KitID " + Condition + " Order by A.Doj Desc" + objDAL.IsoEnd;


            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            foreach (DataRow Dr in Dt.Rows)
            {
                Dr["LgnID"] = Crypto.Encrypt("uid=" + Dr["IDNo"] + "&pwd=" + Dr["Passw"] + "&mobile=" + Session["Mobile"]);
            }
            GvData.DataSource = Dt;
            GvData.DataBind();
            Session["GData"] = Dt;
            Session["MemberData"] = Dt;
            ViewState["Idno"] = "Idno";
            ViewState["Sort_Order"] = "ASC";
            if (Dt.Rows.Count > 0)
            {
                btnExport.Enabled = true;
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Enabled = false;
                btnExport.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
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
            GvData.DataSource = Session["MemberData"];
            GvData.DataBind();
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
            Exportdata();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public void Exportdata()
    {
        try
        {
            string Condition = "";
            if (ddlSearch.SelectedValue == "0")
            {
                Condition = "";
            }
            else if (ddlSearch.SelectedValue == "StateName")
            {
                Condition = " Where b.StateName like '%" + txtSrchText.Text + "%'";
            }
            else if (ddlSearch.SelectedValue == "DOJ")
            {
                Condition = " Where Replace(Convert(varchar,a.DOJ,106),' ','-') like '%" + txtSrchText.Text + "%'";
            }
            else if (ddlSearch.SelectedValue == "MemName")
            {
                Condition = " Where (a.MemFirstName like '%" + txtSrchText.Text + "%' OR a.MemFirstName like '%" + txtSrchText.Text + "%') ";
            }
            else if (ddlSearch.SelectedValue == "RMemName")
            {
                Condition = " Where (c.MemFirstName like '%" + txtSrchText.Text + "%' OR c.MemFirstName like '%" + txtSrchText.Text + "%') ";
            }
            else if (ddlSearch.SelectedValue == "KitName")
            {
                Condition = " Where d.KitName like '%" + txtSrchText.Text + "%'";
            }
            else if (ddlSearch.SelectedValue == "walletaddress")
            {
                Condition = " Where a.walletaddress like '%" + txtSrchText.Text + "%'";
            }
            else if (ddlSearch.SelectedValue == "RMemID")
            {
                Condition = " Where e.IDNO = '" + txtSrchText.Text + "' ";
            }
            else if (ddlSearch.SelectedValue == "IDNo")
            {
                Condition = " Where a.IDNO = '" + txtSrchText.Text + "' ";
            }
            else
            {
                Condition = " Where a." + ddlSearch.SelectedValue + " like '%" + txtSrchText.Text + "%'";
            }

            DataTable dtTemp = new DataTable();
            DataGrid dg = new DataGrid();
            try
            {
                string sql = IsoStart + "Select Top 500  A.IDNo,A.MemFirstName As MemName,C.IDNo As SponsorId,(C.MemFirstName +' '+C.MemLastName) as SponsorName," +
                             " A.Email,A.walletAddress,D.KitName as packageName,IsNull(REPLACE(CONVERT(VARCHAR, a.Doj , 106), ' ', '-'),' ') as DojDate," +
                             " STUFF(RIGHT( CONVERT(VARCHAR,a.Doj ,100 ) ,7), 6, 0, ' ') As DojTime, " +
                             " CASE WHEN a.ActiveStatus='Y' THEN ISNULL( REPLACE(CONVERT(VARCHAR, a.UpgradeDate , 106), ' ', '-'),'') ELSE '' END as UpgrdDate," +
                             " CASE WHEN a.ActiveStatus='Y' THEN STUFF(RIGHT( CONVERT(VARCHAR,a.UpgradeDate ,100 ) ,7), 6, 0, ' ') ELSE '' END as Upgrdtime,Case When A.ActiveStatus='Y' " +
                             " and a.IsBlock='N' then 'Active' when a.IsBlock='Y' then 'Blocked' Else 'Pending' End As Status,a.ActiveStatus From " + objDAL.DBName + "..M_MemberMaster As A " +
                             " Inner Join " + objDAL.DBName + "..M_StateDivMaster As B on A.StateCode=B.StateCode and b.RowStatus='Y' Left Join " + objDAL.DBName + "..M_MemberMaster As C On A.RefFormNo=C.FormNo Left Join " + objDAL.DBName + "..M_MemberMaster As e " +
                             " On A.UpLnFormNo=e.Formno Inner join " + objDAL.DBName + "..M_kitMaster As D On A.KitID=D.KitID " + Condition + " Order by A.Doj Desc" + IsoEnd;

                dtTemp = new DataTable();
                dtTemp = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                dg.DataSource = dtTemp;
                dg.DataBind();

                ExportToExcel("MemberDetail.xls", ref dg);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message + "Error In Exporting File");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    private void ExportToExcel(string strFileName, ref DataGrid dg)
    {
        try
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw;
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.Charset = "";
            dg.EnableViewState = false;
            htw = new HtmlTextWriter(sw);
            dg.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

}
