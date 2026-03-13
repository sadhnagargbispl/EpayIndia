using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Downline : System.Web.UI.Page
{
    DataTable dt;
    
    string strquery;
    string FrmCondition = "";
    int ACnt = 0;
    int BCnt = 0;
    DAL objDAL = new DAL();
    ModuleFunction objModuleFun = new ModuleFunction();
    public string formNo;
    protected void Page_Load(object sender, EventArgs e)
    
    
    {
        objDAL = new DAL();
        objModuleFun = new ModuleFunction();

        if (Session["AStatus"] != null && Session["AStatus"].ToString() == "OK")
        {
            Session["PageName"] = "Member / Member Downline  Report";

            if (!IsPostBack)
            {
                FillKit();
                BindSession();
                txtMemberId.Text = "";
            }
        }
        else
        {
            Response.Redirect("Logout.aspx");
        }
    }

    private void FillDownlineSumm(string FormNo)
    {
        try
        {
            DataTable Dt = new DataTable();
            string Condition2 = "";
            string Condition3 = "";

            if (RbtSearch.SelectedValue == "D")
            {
                if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
                {
                    Condition2 = "and Cast(Replace(CONVERT(Varchar,a.Billdate,106),' ','-')as date)>=Cast('" + txtStartDate.Text + "' as date) " +
                                 "and Cast(Replace(CONVERT(varchar,a.BillDate,106),' ','-') as Date)<=Cast(Replace(Convert(Varchar,'" + txtEndDate.Text + "',106),' ','-') as Date)";

                    Condition3 = "and Sessid= (Select Max(a.Sessid) from M_SessWiseBvCF as a,M_SessnMaster as b where a.Sessid=b.Sessid" +
                                 " and  Cast(Replace(CONVERT(Varchar,b.Frmdate,106),' ','-')as date)>=Cast('" + txtStartDate.Text + "' as Date) " +
                                 "and  Cast(Replace(CONVERT(Varchar,b.Todate,106),' ','-')as date)<=Cast('" + txtEndDate.Text + "' as Date) " +
                                 "and Formno='" + FormNo + "')";
                }
            }
            else
            {
                Condition2 += "and Cast(Replace(CONVERT(Varchar,a.BillDate,106),' ','-') as Date)>='" + LblSDate.Text + "' " +
                              "and cast(Replace(CONVERT(varchar,a.BillDate,106),' ','-') as Date)<='" + LblTDate.Text + "'";

                Condition3 += " and Sessid='" + DDLSession.SelectedValue + "'";
            }

            if (ChkKit.Checked)
            {
                Condition2 += " And KitId='" + CmbKit.SelectedValue + "' ";
            }

            string strquery = " select IsNull(Sum(temp.LeftBV),0) as LeftBV,Isnull(Sum(Temp.RightBV),0)as RightBV ,IsnUll(sum(Temp.LegXBVCf),0) as LegXBVCf,IsNull(sum(Temp.LegYBVCF),0) as LegYBVCf," +
                              " Isnull(sum(Temp.LegXBVPaid),0) as LegXBVPaid,IsnUll(sum(Temp.LegYBvPaid),0) as LegYBVPaid From(" +
                              " Select Isnull(sum(Repurchincome),0) as LeftBV,0 as RightBV,0 as LegXBVCF,0 as LegYBvCF,0 as LegXBVPaid,0 as LegYBVPaid FROM RepurchIncome as a,M_MemTreeRelation as b  WHERE " +
                              " a.Formno=b.FormnoDwn and a.BillType<>'R' and b.LegNo='1' and " +
                              " b.FormNo=" + FormNo + " " + Condition2 + " " +
                              " Union ALL" +
                              " Select 0 as LeftBV,Isnull(sum(Repurchincome),0) as RightBV,0 as LegXBVCF,0 as LegYBvCF,0 as LegXBVPaid,0 as LegYBVPaid FROM RepurchIncome as a,M_MemTreeRelation as b  WHERE " +
                              " a.Formno=b.FormnoDwn and a.BillType<>'R' and b.LegNo='2' and " +
                              " b.FormNo=" + FormNo + " " + Condition2 + " " +
                              " Union All " +
                              " select 0 as LeftBv,0 as RightBV,Isnull(LegXBvCF,0) as LegXBvCF,Isnull(LegYBVCF,0) as LegYBVCF,Isnull(LegXBVPaid,0) as LegXBVPaid,IsnUll(LegYBVPaid,0) as LegYBVPaid from M_SesswiseBvCF  " +
                              " where Formno='" + FormNo + "' " + Condition3 + "" +
                              ") as Temp";

            Dt = objDAL.GetData(strquery);

            // Use Dt DataTable as needed

            RadioButton();
        }
        catch (Exception ex)
        {
            // Handle exceptions here
        }
    }
    public void BindSession()
    {
        string sql = "Select * From( select SessID,Cast(SessID as varchar) + ' [' + Replace(Convert(varchar,FrmDate,106),' ','-') + ' to ' " +
                     " + ISNULL(Replace(Convert(varchar,ToDate,106),' ','-'),'') + ']' As SessnName,FrmDate " +
                     " as FromDate,ToDate as Todate from M_SessnMaster Where ToDate Is Not Null) As Temp order by SessID";

        dt = new DataTable();
        dt = objDAL.GetData(sql);

        DDLSession.DataSource = dt;
        DDLSession.DataTextField = "SessnName";
        DDLSession.DataValueField = "SessId";
        DDLSession.DataBind();

        if (dt.Rows.Count > 0)
        {
            txtEndDate.Text = ((DateTime)dt.Rows[0]["FromDate"]).ToString("dd-MMM-yyyy");
            LblTDate.Text = ((DateTime)dt.Rows[0]["ToDate"]).ToString("dd-MMM-yyyy");
        }
        else
        {
            LblSDate.Text = "";
            LblTDate.Text = "";
        }
    }
    public void FillKit()
    {
        string query = "Select kitId,KitName From M_KitMaster where RowStatus='Y'  Order By kitId";

        dt = new DataTable();
        dt = objDAL.GetData(query);

        CmbKit.DataSource = dt;
        CmbKit.DataTextField = "KitName";
        CmbKit.DataValueField = "KitId";
        CmbKit.DataBind();
    }
    public void FillDownline(string Condition = "", bool IsSideA = false)
    {
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        string Condition2 = "";
        string condition3 = "";
        try
        {
            formNo = GetFormNo();
            if (RbtSearch.SelectedValue == "D")
            {
                if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
                {
                    if (DDlDate.SelectedValue == "J")
                    {
                        Condition2 = "and Cast(Replace(CONVERT(Varchar,JoinDate,106),' ','-')as date)>=Cast('" + txtStartDate.Text + "' as Date) and Cast(Replace(CONVERT(varchar,JoinDate,106),' ','-') as Date)<=Cast('" + txtEndDate.Text + "' as date)";
                    }
                    else
                    {
                        Condition2 = "and Cast(Replace(CONVERT(Varchar,JoinDate,106),' ','-') as Date)>=Cast('" + txtStartDate.Text + "' as Date) and cast(Replace(CONVERT(varchar,JoinDate,106),' ','-') as Date)<=Cast('" + txtEndDate.Text + "' as date) And ActiveStatus='Y'";
                    }
                }
            }
            else
            {
                Condition2 += "and Cast(Replace(CONVERT(Varchar,JoinDate,106),' ','-') as Date)>='" + LblSDate.Text + "' and cast(Replace(CONVERT(varchar,JoinDate,106),' ','-') as Date)<='" + LblTDate.Text + "'";
            }
            if (ChkKit.Checked)
            {
                Condition2 += " And KitId='" + CmbKit.SelectedValue + "' ";
            }
            if (DDlDate.SelectedValue == "A")
            {
                Condition2 += " And Activestatus='Y'";
            }

            string Str = " Select Sum(lActive) as LeftActive,Sum(lDeactive) as LeftDeactive," +
                         " sUM(Ractive) AS RightActive,Sum(RDeactive) as RightDeactive From " +
                         " (select Count(Formno) as lActive,0 as lDeactive,0 as Ractive,0 as RDeactive from V#Downline" +
                         " where formno=" + Convert.ToInt32(formNo.ToString()) + "  and LegNo='1' and ActiveStatus='Y' " + Condition2 + "" +
                         " Union All" +
                         " select 0 as lActive,Count(Formno) as lDeactive,0 as Ractive,0 as RDeactive from V#Downline " +
                         " where formno=" + Convert.ToInt32(formNo.ToString()) + "   and LegNo='1' and ActiveStatus='N' " + Condition2 + "" +
                         " Union All" +
                         " select 0 as lActive,0 as lDeactive,Count(Formno) as Ractive,0 as RDeactive from V#Downline" +
                         " where formno=" + Convert.ToInt32(formNo.ToString()) + "  and LegNo='2' and ActiveStatus='Y' " + Condition2 + "" +
                         " union All" +
                         " select 0 as lActive,0 as lDeactive,0 as Ractive,Count(Formno) as RDeactive from V#Downline " +
                         " where formno=" + Convert.ToInt32(formNo.ToString()) + "  and LegNo='2' and ActiveStatus='N' " + Condition2 + ") as Temp";

            dt = new DataTable();
            dt = objDAL.GetData(Str);

            if (dt.Rows.Count > 0)
            {
                Leftactive.Text = "Left Active: " + dt.Rows[0]["LeftActive"].ToString();
                LeftDeactive.Text = "Left Deactive: " + dt.Rows[0]["LeftDeactive"].ToString();
                RActive.Text = "Right Active: " + dt.Rows[0]["RightActive"].ToString();
                RDeactive.Text = "Right Deactive: " + dt.Rows[0]["RightDeactive"].ToString();
                Leftactive.Visible = true;
                LeftDeactive.Visible = true;
                RActive.Visible = true;
                RDeactive.Visible = true;
            }
            if (rbleg.SelectedValue == "0")
            {
                strquery = "select top 100 *,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" + Convert.ToInt32(formNo.ToString()) + " and LegNo='1' " + Condition2 + "  Order by JoinDate";


                dt1 = new DataTable();
                dt1 = objDAL.GenerateTreeProc(strquery);
                Session["DirectData1"] = dt1;
                GvData.DataSource = dt1;
                GvData.DataBind();
                DivSideA.Style["display"] = "block";
                if (dt1.Rows.Count > 0)
                {
                    //Leftactive.Text = "Left Active: " + dt1.Rows[0]["LeftActive"].ToString();
                    //LeftDeactive.Text = "Left Deactive: " + dt1.Rows[0]["LeftDeactive"].ToString();
                    trRightHeading.Visible = false;
                    TrLeftHeading.Visible = true;
                    Leftactive.Visible = true;
                    LeftDeactive.Visible = true;
                }

            }
            if (rbleg.SelectedValue == "0")
            {
                strquery = "select top 100 *,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" + Convert.ToInt32(formNo.ToString()) + " and LegNo='2' " + Condition2 + " order by JoinDate";
                //strquery = "select top 100 *,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#DownlineUpdate where formno=" + Convert.ToInt32(formNo.ToString()) + " and LegNo='2' " + Condition2 + " order by JoinDate";

                dt2 = new DataTable();
                dt2 = objDAL.GenerateTreeProc(strquery);
                Session["DirectData2"] = dt2;
                GrdDirects2.DataSource = dt2;
                GrdDirects2.DataBind();
                DivSideB.Style["display"] = "block";
                if (dt2.Rows.Count > 0)
                {
                    //RActive.Text = "Right Active: " + dt.Rows[0]["RightActive"].ToString();
                    //RDeactive.Text = "Right Deactive: " + dt.Rows[0]["RightDeactive"].ToString();
                    RActive.Visible = true;
                    RDeactive.Visible = true;
                    trRightHeading.Visible = true;
                    TrLeftHeading.Visible = false;
                }
            }
                if (rbleg.SelectedValue == "1")
            {
                strquery = "select top 100 *,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" + Convert.ToInt32(formNo.ToString()) + " and LegNo='1' " + Condition2 + "  Order by JoinDate";
               

                dt1 = new DataTable();
                dt1 = objDAL.GenerateTreeProc(strquery);
                Session["DirectData1"] = dt1;
                GvData.DataSource = dt1;
                GvData.DataBind();
                DivSideA.Style["display"] = "block";
                if (dt1.Rows.Count > 0)
                {
                    //Leftactive.Text = "Left Active: " + dt1.Rows[0]["LeftActive"].ToString();
                    //LeftDeactive.Text = "Left Deactive: " + dt1.Rows[0]["LeftDeactive"].ToString();
                    trRightHeading.Visible = false;
                    TrLeftHeading.Visible = true;
                    Leftactive.Visible = true;
                    LeftDeactive.Visible = true;
                    RActive.Visible = false;
                    RDeactive.Visible = false;
                }

            }
            if (rbleg.SelectedValue == "2")
            {
                strquery = "select top 100 *,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#Downline where formno=" + Convert.ToInt32(formNo.ToString()) + " and LegNo='2' " + Condition2 + " order by JoinDate";
                //strquery = "select top 100 *,doj +' '+JoiningTime  as DateofJoining,TopupDate+ ' '+TopUpTime as TopUp  from V#DownlineUpdate where formno=" + Convert.ToInt32(formNo.ToString()) + " and LegNo='2' " + Condition2 + " order by JoinDate";

                dt2 = new DataTable();
                dt2 = objDAL.GenerateTreeProc(strquery);
                Session["DirectData2"] = dt2;
                GrdDirects2.DataSource = dt2;
                GrdDirects2.DataBind();
                DivSideB.Style["display"] = "block";
                if (dt2.Rows.Count > 0)
                {
                    //RActive.Text = "Right Active: " + dt.Rows[0]["RightActive"].ToString();
                    //RDeactive.Text = "Right Deactive: " + dt.Rows[0]["RightDeactive"].ToString();
                    Leftactive.Visible =false;
                    LeftDeactive.Visible = false;
                    RActive.Visible = true;
                    RDeactive.Visible = true;
                    trRightHeading.Visible = true;
                    TrLeftHeading.Visible = false;
                }
            }

            RadioButton();
        }
        catch (Exception ex)
        {
            if (IsSideA)
            {
                Response.Write(ex.Message + "SideA");
            }
            else
            {
                Response.Write(ex.Message + "SideB");
            }
        }
    }

  

    public void ExportDownline(string Str, string Condition = "")
    {
        try
        {
            DataTable dtTemp = new DataTable();
            DataGrid dg = new DataGrid();

            dtTemp = new DataTable();
            dtTemp = objDAL.GetData(Str);

            dg.DataSource = dtTemp;
            dg.DataBind();

            ExportToExcel("Downline.xls", dg);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "Error In Exporting File");
        }
    }
    protected void rbleg_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButton();
    }
    private void ExportToExcel(string strFileName, DataGrid dg)
    {
        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);

        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
        Response.Charset = "";
        dg.EnableViewState = false;
        dg.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }
    public static void ExportToSpreadsheet(DataTable table, string name)
    {
        HttpContext context = HttpContext.Current;
        context.Response.Clear();

        foreach (DataColumn column in table.Columns)
        {
            context.Response.Write(column.ColumnName + ";");
        }

        context.Response.Write(Environment.NewLine);

        foreach (DataRow row in table.Rows)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                context.Response.Write(row[i].ToString().Replace(";", string.Empty) + ";");
            }
            context.Response.Write(Environment.NewLine);
        }

        context.Response.ContentType = "text/csv";
        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + ".csv");
        context.Response.End();
    }
    private void RadioButton()
    {
        if (rbleg.SelectedIndex == 1)
        {
            DivSideA.Style["display"] = "block";
            DivSideB.Style["display"] = "none";
            trRightHeading.Visible = false;
            TrLeftHeading.Visible = true;
            BtnExportA.Visible = true;
            BtnExportB.Visible = false;
        }
        else if (rbleg.SelectedIndex == 2)
        {
            DivSideA.Style["display"] = "none";
            DivSideB.Style["display"] = "block";
            trRightHeading.Visible = true;
            TrLeftHeading.Visible = false;
            BtnExportA.Visible = false;
            BtnExportB.Visible = true;
        }
        else
        {
            DivSideA.Style["display"] = "block";
            DivSideB.Style["display"] = "block";
            trRightHeading.Visible = true;
            TrLeftHeading.Visible = true;
            BtnExportA.Visible = true;
            BtnExportB.Visible = true;
        }
    }
    protected void BtnExportB_Click(object sender, EventArgs e)
    {
        string cond = "";
        formNo = GetFormNo();
        if (RbtSearch.SelectedValue == "D")
        {
            if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
            {
                if (DDlDate.SelectedValue == "J")
                {
                    cond = "and Cast(Replace(CONVERT(Varchar,UpgradeDate1,106),' ','-') as date)>=Cast('" + txtStartDate.Text + "' as Date) and Cast(Replace(CONVERT(varchar,UpgradeDate1,106),' ','-') as Date)<=Cast('" + txtEndDate.Text + "' as Date)";
                }
                else
                {
                    cond = "and Cast(Replace(CONVERT(Varchar,UpgradeDate1,106),' ','-') as Date)>=Cast('" + txtStartDate.Text + "' as Date) and cast(CONVERT(varchar,UpgradeDate1,106) as Date)<=cast('" + txtEndDate.Text + "' as date) And ActiveStatus='Y'";
                }
            }
        }
        else
        {
            cond = "and Cast(Replace(CONVERT(Varchar,UpgradeDate1,106),' ','-') as Date)>='" + LblSDate.Text + "'  and cast(Replace(CONVERT(varchar,UpgradeDate1,106),' ','-') as Date)<='" + LblTDate.Text + "'";
        }
        if (ChkKit.Checked)
        {
            cond += " And KitId='" + CmbKit.SelectedValue + "' ";
        }
        if (DDlDate.SelectedValue == "A")
        {
            cond += " And Activestatus='Y' ";
        }

        string str = "select top 100 IDNO,MemName as MemberName,RefFormno as SponsorId,ReferalName as SponsorName,doj as Doj,JoiningTime,TopUpDate ,TopUpTime, KitName as Package," +
            "KitAmount as PackageAmount,Bv as PV  from V#Downline where formno=" + Convert.ToInt32(formNo.ToString()) + " and LegNo='2' " + cond + " order by JoinDate";

        ExportDownline(str, "");
    }
    protected void BtnExportA_Click(object sender, EventArgs e)
    {
        string cond = "";
        formNo = GetFormNo();
        if (RbtSearch.SelectedValue == "D")
        {
            if (!string.IsNullOrEmpty(txtStartDate.Text) && !string.IsNullOrEmpty(txtEndDate.Text))
            {
                if (DDlDate.SelectedValue == "J")
                {
                    cond = "and Cast(Replace(CONVERT(Varchar,UpgradeDate1,106),' ','-')as date)>=Cast('" + txtStartDate.Text + "' as date) and Cast(Replace(CONVERT(varchar,UpgradeDate1,106),' ','-') as Date)<=Cast('" + txtEndDate.Text + "' as Date)";
                }
                else
                {
                    cond = "and Cast(Replace(CONVERT(Varchar,UpgradeDate1,106),' ','-') as Date)>=Cast('" + txtStartDate.Text + "' as Date) and cast(Replace(CONVERT(varchar,UpgradeDate1,106),' ','-') as Date)<=Cast('" + txtEndDate.Text + "' as Date) And ActiveStatus='Y'";
                }
            }
        }
        else
        {
            cond = "and Cast(Replace(CONVERT(Varchar,UpgradeDate1,106),' ','-') as Date)>='" + LblSDate.Text + "' and cast(Replace(CONVERT(varchar,UpgradeDate1,106),' ','-') as Date)<='" + LblTDate.Text + "' ";
        }
        if (ChkKit.Checked)
        {
            cond += " And KitId='" + CmbKit.SelectedValue + "' ";
        }
        if (DDlDate.SelectedValue == "A")
        {
            cond += " And Activestatus='Y' ";
        }

        string str = "select top 100 IDNO,MemName as MemberName,RefFormno as SponsorId,ReferalName as SponsorName,doj as Doj,JoiningTime,TopUpDate ,TopUpTime," +
            " KitName as Package,KitAmount as PackageAmount,Bv as PV  from V#Downline where formno=" + Convert.ToInt32(formNo.ToString()) + " and LegNo='1' " + cond + " order by JoinDate";

        ExportDownline(str, "");
    }
    protected void GrdDirects1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["DirectData1"];
            GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void GrdDirects2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GrdDirects2.PageIndex = e.NewPageIndex;
            GrdDirects2.DataSource = Session["DirectData2"];
            GrdDirects2.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    
    protected void btnShowDownline_Click(object sender, EventArgs e)
    {
        string formNo = GetFormNo();
        if (!string.IsNullOrEmpty(formNo))
        {
            lblError.Text = "";
            FillDownlineSumm(formNo);
            divMemDownline.Visible = true;
            // filldetail();
            FillDownline();
            FillDownline("", false);
        }
    }
    private string GetFormNo()
    {
        objDAL = new DAL();
        string formNo = "";
        string idNo = txtMemberId.Text;
        string qry = "Select FormNo from " + objDAL.tblMemberMaster + " where IdNo='" + idNo + "'";
        DataTable dt = objDAL.GetData(qry);
        if (dt.Rows.Count > 0)
        {
            formNo = dt.Rows[0]["FormNo"].ToString();
        }
        else
        {
            lblError.Text = "Member Id does not exist. Please check it once and then enter it again.";
            lblError.Visible = true;
            txtMemberId.Text = "";
        }
        return formNo;
    }
    protected void RbtSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RbtSearch.SelectedValue == "S")
        {
            DDLSession.Visible = true;
            txtEndDate.Visible = false;
            txtStartDate.Visible = false;
            txtEndDate.Visible = false;
            //lblStartDate.Visible = false;
            DDlDate.Visible = false;
        }
        else
        {
            DDLSession.Visible = false;
            txtEndDate.Visible = true;
            txtStartDate.Visible = true;
            txtEndDate.Visible = true;
            //lblStartDate.Visible = true;
            DDlDate.Visible = true;
        }
    }
    protected void DDLSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql = "select Cast(Replace(Convert(Varchar,FrmDate,106),' ','-') as DateTime) as FromDate, " +
                     "Cast(Replace(Convert(Varchar,ToDate,106),' ','-') as DateTime) as ToDate, " +
                     "FrmDate as FDate, ToDate as TDate " +
                     "from M_SessnMaster where Sessid='" + DDLSession.SelectedValue + "'";

        dt = objDAL.GetData(sql);
        if (dt.Rows.Count > 0)
        {
            txtStartDate.Text = ((DateTime)dt.Rows[0]["FDate"]).ToString("dd-MMM-yyyy");
            LblTDate.Text = ((DateTime)dt.Rows[0]["TDate"]).ToString("dd-MMM-yyyy");
        }
        else
        {
            LblTDate.Text = "";
            LblSDate.Text = "";
            LblSTDate.Text = "";
            LblToDate.Text = "";
        }
    }


    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["DirectData1"];
            GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}
