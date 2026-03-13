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
public partial class bankverified : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DAL obj = new DAL();
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BtnVerifiy.Attributes.Add("onclick", DisableTheButton(Page, BtnVerifiy));
            BTnUnVerification.Attributes.Add("onclick", DisableTheButton(Page, BTnUnVerification));
           
            if (!Page.IsPostBack)
            { 
                if (Session["AStatus"] != null)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["key"]) && Request.QueryString["key"] != null)
                    {
                        if (Request.QueryString["key"] != "" && Request.QueryString["key"] != null)
                        {
                            txtMemId.Text = Request.QueryString["key"];
                            BindData(" AND IDNo='" + Request.QueryString["key"] + "'");
                        }
                    }
                    else
                    {
                        BindData();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
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
            sb.Append("this.value = 'Please Wait...';");
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
  
    public void BindData(string Condition = "")
    {
        try
        {
            {
                if (!string.IsNullOrEmpty(txtMemId.Text))
                {
                    Condition += " AND IDNo='" + txtMemId.Text.Trim() + "'";
                }
            }
            if (DDlVerify.SelectedValue != "S")
            {
                Condition += " And c.IsBankVerified='" + DDlVerify.SelectedValue + "'  ";
            }
            if (RbtSearch.SelectedValue != "A")
            {
                Condition += " And a.ActiveStatus='" + RbtSearch.SelectedValue + "'  ";
            }
      
            string sql = "Select Cast(a.FormNo as varchar) as FormNo,Replace(CONVERT(varchar,Doj,106),' ','-')as Doj," +
                "Case when a.Activestatus='Y' then Replace(CONVERT(varchar,UpgradeDate,106),' ','-') Else '' End as ActivationDate, " +
                " a.IDNo,RTRIM(a.MemFirstName +' ' + a.MemLastName) as MemName,b.Bankname,a.Acno,a.Branchname," +
                "a.Ifscode,a.panno,   CASE WHEN c.IsBankVerified='Y' THEN 'Verified' when c.IsBankVerified='R' then 'Rejected' Else 'Verification Due' END AS BankVerf, " +
                " case when c.BankProof<>'' then Replace(CONVERT(varchar,c.BankProofDate ,106),' ','-') else '' end  as BankProofDate," +
                " CASE WHEN c.BankProof<> '' Then c.BankProof ELSE '" + Session["CompWeb"].ToString() + "/Images/no_photo.jpg' END AS BankProofStatus," +
                "   Case when c.IsBankVerified='N' then '' else Replace(convert(Varchar,c.BankVerifyDate,106),' ','-') end as BankVerifyDate, " +
                "  Case when c.IsBankVerified='Y' then 'False' else 'True' end as EnableStatus,   Case when c.IsBankVerified<>'R' then '' else c.BankProofRemark end as RejectRemark , " +
                " CASE WHEN c.IsPanVerified='Y' THEN 'Verified' when c.IsPanVerified='R' then 'Rejected' Else 'Verification Due' END AS PanVerf, " +
                "  case when c.PanImg<>'' then Replace(CONVERT(varchar,c.PanVerifyDate ,106),' ','-') else '' end  as PanProofDate,  " +
                " CASE WHEN c.PanImg <>'' Then c.PanImg  ELSE 'https://knowledgecupl.com//Images/no_photo.jpg' END AS PanProofStatus,  " +
                " Case when c.IsPanVerified='N' then '' else Replace(convert(Varchar,c.PanVerifyDate,106),' ','-') end as PanVerifyDate, " +
                " Case when c.IsPanVerified='Y' then 'False' else 'True' end as PanEnableStatus,  Case when c.IsPanVerified<>'R' then '' else c.PanRemarks end as PanRejectRemark ," +
                "  Isnull(e.UserName,' ')as VerifyBy,Isnull(f.Reason, '')As RejectReason   From "+ objDal.DBName + "..M_MemberMaster as a  " +
                "  Inner Join "+ objDal.DBName + "..M_BAnkMaster as b On a.Bankid=b.Bankcode and b.Rowstatus='Y'  " +
                "  Inner Join "+ objDal.DBName + "..KYCVerify as c   Left Join  "+ objDal.DBName + "..M_KycReject as f On c.BankRejectId=f.Kid  On a.Formno=c.Formno  " +
                "  Left Join "+ objDal.DBName + "..M_Usermaster as e On c.BankUserid=e.Userid and e.RowStatus='Y' Where 1=1  and c.BankProof <>''" +
                " " + Condition + " ";
            //string sql = obj.IsoStart + " select * from V#newkycverify where 1=1 " + Condition + "  " + obj.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                GvData.Visible = true;
                GvData.DataSource = Dt;
                GvData.DataBind();
                Session["GData"] = Dt;
                BTnUnVerification.Enabled = true;
                BtnVerifiy.Enabled = true;
                BtnExport.Enabled = true;
            }
            else
            {
                GvData.Visible = false;
                BtnVerifiy.Enabled = false;
                BTnUnVerification.Enabled = false;
                BtnExport.Enabled = false;
 
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
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
    protected void BtnVerifiy_Click(object sender, EventArgs e)
    {
        try
        {
            string str = "";
            string scrname;
            Label lbl;
            Label LblIdNo;
            CheckBox Chk;
            string Remark = "";
            foreach (GridViewRow Gvr in GvData.Rows)
            {
                Chk = (CheckBox)Gvr.FindControl("chkSelect");
                lbl = (Label)Gvr.FindControl("LblGrpID");
                LblIdNo = (Label)Gvr.FindControl("LblIdno");
                if (Chk.Checked)
                {
                    Remark = "Bank Proof Verify of IdNo:" + LblIdNo.Text;
                    str = "exec sp_BtnVerifiyNew '" + Session["UserID"] + "','" + Session["UserName"] + "','" + Remark + "','" + lbl.Text + "'";
                }
            }
            string Str_Sql =  " Begin Try   Begin Transaction " + str + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
            int i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_Sql));
            if (i > 0)
            {
                scrname = "<SCRIPT language='javascript'>alert(' Verified successfully. ');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Image", scrname, false);
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert(' Verified unsuccessfully. ');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Image", scrname, false);
            }

            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BtnUnVerify_Click(object sender, EventArgs e)
    {
        try
        {
            string str = "";
            string scrname;
            Label lbl;
            CheckBox Chk;
            string Remark = "";
            Label LblIdno;
            if ((TxtARemark.Text).Trim() == "")
            {

                scrname = "<SCRIPT language='javascript'>alert('Enter Remark.');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Enter Remark.');", true);
                return;
            }
            foreach (GridViewRow Gvr in GvData.Rows)
            {
                Chk = (CheckBox)Gvr.FindControl("chkSelect");
                lbl = (Label)Gvr.FindControl("LblGrpID");
                LblIdno = (Label)Gvr.FindControl("LblIdno");
                if (Chk.Checked)
                {
                    Remark = "Kyc UnVerify of IdNo:" + LblIdno.Text;
                    str = "exec SP_BtnUnVerify '" + Session["UserID"] + "','" + Session["UserName"] + "','" + Remark + "','" + lbl.Text + "','" + DDlREason.SelectedValue + "'";
                }
            }
            string Str_Sql = "Begin Try   Begin Transaction " + str + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
            int X = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_Sql));
            if (X > 0)
                scrname = "<SCRIPT language='javascript'>alert(' UnVerified successfuly. ');" + "</SCRIPT>";
            else
                scrname = "<SCRIPT language='javascript'>alert(' UnVerified unsuccessfuly. ');" + "</SCRIPT>";

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Image", scrname, false);

            DivRemark.Visible = false;
            TxtARemark.Text = "";
            BindData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BTnUnVerification_Click(object sender, EventArgs e)
    {
        try
        {
            DivRemark.Visible = true;
            BtnVerifiy.Enabled = false;
            BTnUnVerification.Enabled = false;
            FillDetail();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void FillDetail()
    {
        try
        {
            string s = obj.IsoStart +"Select * from "+ obj.DBName +"..M_KycReject where activeStatus='Y'" + obj.IsoEnd ;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, s).Tables[0];
            if (Dt.Rows.Count > 0)
           
            {
                DDlREason.DataValueField = "kId";
                DDlREason.DataTextField = "reason";
                DDlREason.DataSource = Dt;
                DDlREason.DataBind();
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
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTemp = new DataTable();
            DataGrid dg = new DataGrid();
            string Condition = "";
            if (!string.IsNullOrEmpty(txtMemId.Text))
            {
                Condition = Condition + " AND IDNo='" + txtMemId.Text.Trim() + "'";
            }

            if (DDlVerify.SelectedValue != "S")
            {
                Condition = Condition + " And c.IsBankVerified='" + DDlVerify.SelectedValue + "'  ";
            }
            if (RbtSearch.SelectedValue != "A")
            {
                Condition = Condition + " And a.ActiveStatus='" + RbtSearch.SelectedValue + "'  ";
            }
            string sql =" select * from V#newkycverifyExport where 1=1 " + Condition + "  " ;
            dtTemp = SqlHelper.ExecuteDataset(constr1,CommandType.Text ,sql).Tables[0];
            dg.DataSource = dtTemp;
            dg.DataBind();
            ExportToExcel("BankProofKYC.xls", dg);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "Error In Exporting File");
        }
    }
    private void ExportToExcel(string fileName, DataGrid dg)
    {
        try
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            dg.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
}