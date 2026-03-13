using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UnBlock : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DataSet Ds = new DataSet();
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.btnShowSingleDetail.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnShowSingleDetail));
            this.BtnBlock.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnBlock));
            lblrecordcount.Text = "";
            if (!Page.IsPostBack)
            {
                txtMemberId.Text = "";
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["key"]) && Request.QueryString["key"] != null)
                    {
                        if (Request.QueryString["Tp"] == "S")
                        {
                            rdblistChoice.SelectedValue = "single";
                        }
                        else
                        {
                            rdblistChoice.SelectedValue = "multiple";
                        }
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
    private void ShowDetail()
    {
        try
        {
            string idNo;
            if (!string.IsNullOrEmpty(txtMemberId.Text))
            {
                idNo = objDal.ClearInject(txtMemberId.Text);
                lblError.Text = "";
                string qry = objDal.IsoStart + "Select FormNo FROM " + objDal.DBName + ".. M_MemberMaster WHERE IDNO='" + idNo + "'" + objDal.IsoEnd;
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
                if (rdblistChoice.SelectedValue == "single")
                {
                    qry = " exec sp_GetMemberDetails " + idNo + " ";
                }
                else
                {
                    qry = "exec sp_NewGetMemberDetails " + TxtFormNo.Text.Trim() + "";
                }
                Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
                if (Dt.Rows.Count == 0)

                {
                    lblError.Text = "Detail Not Found.";
                    lblError.Visible = true;
                }
                else
                {
                    GvData.DataSource = Dt;
                    Session["UnBlockIDs"] = Dt;
                    GvData.DataBind();
                    GvData.Visible = true;
                    lblrecordcount.Text = "Record Count : " + Dt.Rows.Count;
                    BtnBlock.Visible = true;
                }
            }
            else
            {
                lblError.Text = "Member Id can not be blank. Please provide member ID to proceed.";
                lblError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnUnblock_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql, scrname;
            string Remark = "";
            if (rdblistChoice.SelectedValue == "single")
            {
                Remark = " UnBlock Id " + ClearInject(txtMemberId.Text) + " By " + Session["UserName"] + "";
                Sql = " exec sp_GatBtnUnblock '" + ClearInject(TxtFormNo.Text) + "' ,'" + Convert.ToInt32(Session["UserID"]) + "', ";
                Sql += "'" + Session["UserName"] + "', '" + ClearInject(Remark) + "'";
                scrname = "ID";
            }
            else
            {
                Remark = " UnBlock Tree " + ClearInject(txtMemberId.Text) + " By " + Session["UserName"] + "";
                Sql = " Exec sp_GatUnblockNew '" + ClearInject(TxtFormNo.Text) + "','" + Convert.ToInt32(Session["UserID"]) + "'";
                Sql += ", '" + Session["UserName"] + "', '" + ClearInject(Remark) + "' ";
                scrname = "Tree";
            }
            string Str_Sql = string.Empty;
            Str_Sql = "Begin Try   Begin Transaction " + Sql + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
            int updateEffect = 0;
            updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_Sql));
            if (updateEffect != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + scrname + " unblocked Successfully.!')", true);
                TxtFormNo.Text = "";
                txtMemberId.Text = "";
                BtnBlock.Visible = false;
                GvData.Visible = false;
                lblrecordcount.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Not unblocked.!')", true);
                
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

    protected void btnShowSingleDetail_Click(object sender, EventArgs e)
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

}
