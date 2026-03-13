using ClosedXML.Excel;
using DocumentFormat.OpenXml.Presentation;
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

public partial class Reward : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DataSet Ds = new DataSet();
    DAL ObjDal = new DAL();
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
                    FillReward();
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
    protected void BtnShow_Click(object sender, EventArgs e)
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

    public void BindData()
    {
        try
        {
            string formno = "";

            string condition1 = "";
            string condition2 = "";
            string condition = "";
            if (Chkmemid.Checked)
            {
                formno = GetFormNo();
                condition += " And d.Formno='" + Convert.ToInt32(formno) + "'";
            }
            if (ddllist.SelectedValue != "0")
            {
                condition += " And b.Rankid='" + ddllist.SelectedValue + "'";
            }
            if (txtStartDate.Text != "")
            {
                condition1 = " And Cast(Convert(varchar,C.FrmDate,106) as DateTime)>='" + txtStartDate.Text + "'";
            }
            if (txtEndDate.Text != "")
            {
                condition2 = " And Cast(Convert(varchar,C.FrmDate,106) as DateTime)<='" + txtEndDate.Text + "'";
            }
            string qry1 = "";
            //qry1 =  " select * from  V#RewardNew Where 1=1  " + condition1 + " " + condition2 ;
        
            qry1 = "Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],";
            qry1 += "d.mobl as [Mobile No.],";
            qry1 += "b.Rank as [Rank Name],b.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as [Achieve Date] ";
            qry1 += "from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD ";
            qry1 += "left join D_SessnMaster as c on a.SessID = c.SessID ";
            qry1 += " Left join M_memberMaster as d on a.formno = d.formno ";
            qry1 += "Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 " + condition + " ";
            qry1 += " " + condition1 + " " + condition2 + " order by a.sessid desc";

            Dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, qry1).Tables[0];
            if(Dt.Rows .Count > 0)
            {
                
                GvData1.DataSource = Dt;
                GvData1.DataBind();
                Session["WalletData"] = Dt;
                ViewState["Sno"] = "Formno";
                ViewState["Sort_Order"] = "ASC";
                GvData1.Visible = true;
                lblError.Visible = false;
            }
            else
            {
                lblError.Text = "No Record Found!!";
                lblError.Visible = true;
                GvData1.Visible = false;
            }
            
            
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string GetFormNo()
    {
        DataTable dt = new DataTable();
        string idNo = "";
        string formno = "";
        idNo = txtMemId.Text;
        idNo = idNo.Trim();
        string qry = ObjDal.IsoStart + "Select FormNo from " + ObjDal.DBName + "..M_MemberMaster where IdNo='" + idNo + "'" + ObjDal.IsoEnd;
        dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
        if (dt.Rows.Count > 0)
        {
            formno = dt.Rows[0]["FormNo"].ToString();
        }
        else
        {
            lblError.Text = "Member Id does not exist. Please check it once and then enter it again.";
            lblError.Visible = true;
            txtMemId.Text = "";
        }
        return formno;
    }

    private void FillReward()
    {
        try
        {

            DAL objDAL = new DAL();
            DataTable Dt = new DataTable();
            string S = " select * from V#FillReward";
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, S).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                ddllist.DataSource = Dt;
                ddllist.DataTextField = "Rank";
                ddllist.DataValueField = "Rankid";
                ddllist.DataBind();
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
       
            string Condition = "";
            string formno = "";
            string scrName = "";
            string condition1 = "";
            string condition2 = "";

            if (Chkmemid.Checked)
            {
                formno = GetFormNo();
                Condition += " And d.Formno='" + Convert.ToInt32(formno) + "'";
            }
            if (ddllist.SelectedValue != "0")
            {
                Condition += " And b.Rankid='" + ddllist.SelectedValue + "'";
            }
            if (txtStartDate.Text != "")
            {
                condition1 = " And Cast(Convert(varchar,C.FrmDate,106) as DateTime)>='" + txtStartDate.Text + "'";
            }
            if (txtEndDate.Text != "")
            {
                condition2 = " And Cast(Convert(varchar,C.FrmDate,106) as DateTime)<='" + txtEndDate.Text + "'";
            }

            string qry1 = "";
            //qry1 = ObjDal.IsoStart + " Select * from " + ObjDal.DBName + "..V#RewardNew Where 1=1 "+ ObjDal.IsoEnd;
            qry1 = "Select d.Idno as [Member IDNo],d.MemFirstName [Member Name],";
            qry1 += "d.mobl as [Mobile No.],";
            qry1 += "b.Rank as [Rank Name],b.Reward,Replace(Convert(Varchar,Isnull(c.ToDate,Getdate()),106),' ','-') as [Achieve Date] ";
            qry1 += "from MstRankAchievers as A left join MstRanks As b on a.RankID = b.RankiD ";
            qry1 += "left join D_SessnMaster as c on a.SessID = c.SessID ";
            qry1 += " Left join M_memberMaster as d on a.formno = d.formno ";
            qry1 += "Left Join M_memberMaster as e on e.formno = d.refformno Where 1=1 " + Condition + " ";
            qry1 += " " + condition1 + " " + condition2 + " order by a.sessid desc";

            Dt = SqlHelper.ExecuteDataset (constr ,CommandType.Text, qry1).Tables[0];
          if(Dt.Rows.Count > 0 )
            {
                GvData1.DataSource = Dt;
                GvData1.DataBind();
                Session["GData"] = Dt;
                
            }

          
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
            DataTable dt = (DataTable)Session["GData"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "RankrewardReport");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=RankrewardReport.xlsx");
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
    protected void GrdTotal1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData1.PageIndex = e.NewPageIndex;
            GvData1.DataSource = Session["WalletData"];
            GvData1.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }

    }
}
