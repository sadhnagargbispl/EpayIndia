using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Block : System.Web.UI.Page
{
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.btnShowSingleDetail.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnShowSingleDetail));
            lblrecordcount.Text = "";

            if (!IsPostBack)
            {
                txtMemberId.Text = "";

                //if (Request.QueryString.HasKeys)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["key"]) && Request.QueryString["key"] != null)
                    {
                        txtMemberId.Text = Request.QueryString["key"].ToString();
                        ShowDetail();

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void ShowDetail()
    {
        try
        {
            string idNo;
            lblError.Text = "";
            DataTable Dt = new DataTable();
            if (!string.IsNullOrEmpty(txtMemberId.Text))
            {
                idNo = ClearInject(txtMemberId.Text);
                string qry = objDal.IsoStart + "Select FormNo from " + objDal.DBName + "..M_MemberMaster WHERE IDNO='" + idNo + "'" + objDal.IsoEnd;
                Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    TxtFormNo.Text = Dt.Rows[0][0].ToString();
                }
                else
                {
                    lblError.Text = "Member ID not exist. Please provide correct member ID.";
                    lblError.Visible = true;
                    return;
                }
               
               qry = objDal.IsoStart + " exec sp_GetMemberDetailsNew " + idNo + " " + objDal.IsoEnd; 
                
                Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
                if (Dt.Rows.Count == 0)
                {
                    lblError.Text = "Detail not Found.";
                    lblError.Visible = true;
                    divreason.Visible = false;
                }
                else
                {
                    GvData.DataSource = Dt;
                    Session["BlockIDs"] = Dt;
                    GvData.DataBind();
                    GvData.Visible = true;
                    lblrecordcount.Text = "Record Count : " + Dt.Rows.Count;
                    BtnBlock.Visible = true;
                    divreason.Visible = true;
                    btnShowSingleDetail.Enabled = false;
                }
            }
            else
            {
                lblError.Text = "Member Id can not be blank. Please provide member ID to proceed.";
                lblError.Visible = true;
            }
        }
        catch (Exception EX)
        {
            throw new Exception(EX.Message);
        }
    }
    protected void BtnBlock_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql, scrname;
            string Remark = "";
            Remark = " Block Id " +  ClearInject(txtMemberId.Text) + " By " + Session["UserName"];
            Sql = "exec sp_BlockMember '" + ClearInject(TxtReason.Text) + "','" + ClearInject(TxtFormNo.Text) + "','" + Convert.ToInt32(Session["UserID"]) + "',";
            Sql += "'" + Session["UserName"] + "','" + ClearInject(Remark) + "'";
            scrname = "ID";
            string Str_Sql = string.Empty;
            Str_Sql = "Begin Try   Begin Transaction " + Sql + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
            int updateEffect = 0;
            updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_Sql));
            if (updateEffect > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + scrname + " blocked Successfully.!')", true);

                TxtFormNo.Text = ""; 
                txtMemberId.Text = ""; 
                BtnBlock.Visible = false;
                GvData.Visible = false; 
                lblrecordcount.Text = "";
                TxtReason.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Not blocked!! ')", true);
               
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
    private string DisableTheButton(Control pge, Control btn)
    {
        try
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
            sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
            sb.Append("this.value = 'Please wait...';");
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
    protected void BtnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ShowDetail();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
            StrObj = StrObj.Trim();
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return StrObj;
    }

   
    protected void txtMemberId_TextChanged(object sender, EventArgs e)
    {
        string idNo;
        lblError.Text = "";
        DataTable Dt = new DataTable();
        idNo = ClearInject(txtMemberId.Text);
        string qry = objDal.IsoStart + "Select FormNo,isblock from " + objDal.DBName + "..M_MemberMaster WHERE IDNO='" + idNo + "'" + objDal.IsoEnd;
        Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
        if (Dt.Rows[0]["isblock"].ToString() == "Y")
        {
            lblError.Text = "This Member ID already block.";
            lblError.Visible = true;
            btnShowSingleDetail.Enabled = false;
            txtMemberId.Text = "";
            return;
        }
        else {
            btnShowSingleDetail.Enabled = true;
        }
    }
}
