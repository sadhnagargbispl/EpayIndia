using ClosedXML.Excel;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Complaints : System.Web.UI.Page
{
    DAL ObjDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!Page.IsPostBack)
            {
                txtSearch.Text = "";
                btnShowRecord.Visible = false;
                FillUser();
                BindData();
                if (Session["AStatus"] != null)
                {
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void btnShowRecord_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            btnShowRecord.Visible = false;
            ddlGroupFields.SelectedIndex = 0;
            txtSearch.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["GData"];
                GvData.PageIndex = e.NewPageIndex;
                GvData.DataSource = dt;
                GvData.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
            }
        }
    }

    public void BindData(string Condition = "")
    {
        try
        {
            string sql = "";
            DataTable Dt = new DataTable();
            sql = ObjDal.IsoStart + "Select M.IDNo,M.CID,Cast(M.CID as varchar) as VCId,M.MemName,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate,M.Status FROM";
            sql += "  (Select b.MemFirstName +' '+ b.MemLastName as MemName,a.*,Case when a.ComplaintStatus='O' then 'Open' else 'Close' end as Status ";
            sql += "  FROM " + ObjDal.DBName + "..M_ComplaintMaster as a," + ObjDal.DBName + "..M_MemberMaster as b WHERE a.IDNo=b.IDNo) as M LEFT JOIN " + ObjDal.DBName + "..M_SolutionMaster as S";
            sql += "  ON M.CID=S.CID," + ObjDal.DBName + "..M_UserMaster as U," + ObjDal.DBName + "..m_ComplaintTypeMaster as c WHERE c.CtypeId=m.CtypeId  ";
            sql += " and c.RowStatus='Y' and c.ToUserId=U.Userid and  u.RowStatus='Y'    " + Condition + " ORDER BY M.RecTimeStamp DESC" + ObjDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
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


    private void FillUser()
    {
        try
        {
            DataTable Dt = new DataTable();
            string sql = ObjDal.IsoStart + "SELECT * FROM V#FillUser " + ObjDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                DDlGroup.DataSource = Dt;
                DDlGroup.DataTextField = "GroupName";
                DDlGroup.DataValueField = "GroupID";
                DDlGroup.DataBind();
            }
            if (Session["GroupId"].ToString() != "1")
            {
                DDlGroup.SelectedValue = Session["GroupId"].ToString();
                DDlGroup.Enabled = false;
                DDlGroup.Visible = false;
                LblGroup.Visible = false;
            }
            else
            {
                DDlGroup.Enabled = true;
                DDlGroup.Visible = true;
                LblGroup.Visible = true;
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
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string Condition_ = "";
            if (RbReplied.SelectedValue != "K")
            {
                Condition_ = " AND M.IsReplied='" + RbReplied.SelectedValue.Trim() + "' ";
            }

            string scrname = "";
            if (ChkDate.Checked)
            {
                if (string.IsNullOrEmpty(lblStartDate.Text))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Enter From Date  ');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                }
                else
                {
                    Condition_ += " AND CAST (M.RecTimeStamp As DAte)>= Cast('" + lblStartDate.Text.Trim() + "' As Date)";
                }

                if (string.IsNullOrEmpty(lblEndDate.Text))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Enter To Date  ');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                }
                else
                {
                    Condition_ += "AND CAST (M.RecTimeStamp As DAte) <= Cast('" + lblEndDate.Text.Trim() + "' As Date)";
                }


            }
            if (DDlGroup.SelectedValue != "0")
            {
                Condition_ += "and U.GroupId='" + DDlGroup.SelectedValue + "'";
            }

            if (!string.IsNullOrEmpty(txtSearch.Text) && ddlGroupFields.SelectedValue != "None")
            {
                if (string.Equals(ddlGroupFields.SelectedItem.Text.ToLower(), "showall"))
                {
                    // No condition to add for 'showall'
                }
                else if (string.Equals(ddlGroupFields.SelectedItem.Text.ToLower(), "solution"))
                {
                    Condition_ += " AND s." + ddlGroupFields.SelectedValue.ToString() + " like '%" + txtSearch.Text + "%'";
                }
                else
                {
                    Condition_ += " AND M." + ddlGroupFields.SelectedValue.ToString() + " like '%" + txtSearch.Text + "%'";
                }
            }

            BindData(Condition_);
        }
        catch (Exception ex)
        {
            // Handle the exception (log it, show a message, etc.)
        }
    }

 
    protected void btnExport_Click(object sender, EventArgs e)
    {
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["GData"];
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Users");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=ComplaintsMaster-" + DateTime.Now.ToShortDateString() + ".xlsx");
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
}