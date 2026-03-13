using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
public partial class AddNews : System.Web.UI.Page
{
    ModuleFunction objModuleFun = new ModuleFunction();
    DataTable Dt = new DataTable();
    DAL objDal = new DAL();
    string UserIdQS;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BtnSave.Attributes.Add("onclick", DisableTheButton(Page, BtnSave));
            if (!string.IsNullOrEmpty(Request["NewsId"]))
            {
                UserIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["NewsId"]));
            }

            if (!Page.IsPostBack)
            {
                ClearAll();
                FillCategory();

                txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress();

                if (!string.IsNullOrEmpty(Request["NewsId"]))
                {
                    BtnSave.Text = "Modify";
                    BindData();
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
    private void FillCategory()
    {
        try
        {
            string str = objDal.IsoStart + "  Exec Sp_GetAllRank " + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                DDlCategory.DataSource = Dt;
                DDlCategory.DataTextField = "Rank";
                DDlCategory.DataValueField = "ID";
                DDlCategory.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("NewsNSeminarMaster.aspx", false);
        }
        catch (Exception Ex)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
        }
    }
    private void BindData()
    {
        try
        {

            string sql = objDal.IsoStart + "Select * From " + objDal.DBName + "..M_NewsSeminarMaster Where NewsId='" + UserIdQS + "' AND " + objDal.activeCondition + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                txtNewsID.Text = Dt.Rows[0]["NewsId"].ToString();
                txtHeading.Text = Dt.Rows[0]["NewsHdr"].ToString();
                txtDetail.Text = Dt.Rows[0]["NewsDtl"].ToString();
                txtFrmDate.Text = Convert.ToDateTime(Dt.Rows[0]["FrmDate"]).ToString("dd-MMM-yyyy");
                txtToDate.Text = Convert.ToDateTime(Dt.Rows[0]["ToDate"]).ToString("dd-MMM-yyyy");
                txtRemarks.Text = Dt.Rows[0]["Remarks"].ToString();
                txtActiveStatus.Text = Dt.Rows[0]["ActiveStatus"].ToString();
                DDlCategory.SelectedValue = Dt.Rows[0]["Rankid"].ToString();

                if (txtActiveStatus.Text.ToUpper() == "Y")
                {
                    rdblist.SelectedIndex = 0;
                }
                else
                {
                    rdblist.SelectedIndex = 1;
                }


            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void ClearAll()
    {
        try
        {
            txtHeading.Text = "";
            txtDetail.Text = "";
            txtRemarks.Text = "";
            txtActiveStatus.Text = "";
            txtIPAdrs.Text = "";
            txtNewsID.Text = "";
            txtFrmDate.Text = "";
            txtToDate.Text = "";
            FillCategory();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    private string ClearInject(string StrObj)
    {
        string Result = "";
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
            Result = StrObj.Trim();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return Result;

    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {

            string FrmDate = txtFrmDate.Text;
            string ToDate = txtToDate.Text;
            string Sql;
            if (rdblist.SelectedIndex == 0)
            {
                txtActiveStatus.Text = "Y";
            }
            else
            {
                txtActiveStatus.Text = "N";
            }
            try
            {
                DateTime Dt = Convert.ToDateTime(FrmDate);
            }
            catch (Exception)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Check Start Date.!')", true);
                return;
            }

            try
            {
                DateTime Dt = Convert.ToDateTime(ToDate);
            }
            catch (Exception)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Check End Date.!')", true);
                return;
            }

            if (!string.IsNullOrEmpty(Request["NewsId"]))
            {
                Sql = "Update  M_NewsSeminarMaster SET RowStatus='N' Where NewsId='" + UserIdQS + "';";
                Sql += " Insert into M_NewsSeminarMaster(NewsId,NewsHdr,NewsDtl,FrmDate,ToDate,NType,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,Rankid) ";
                Sql += " Values('" + ClearInject(txtNewsID.Text.Trim ()) + "','" + ClearInject(txtHeading.Text.Trim()) + "','" + ClearInject(txtDetail.Text.Trim()) + "',";
                Sql += "'" + FrmDate + "','" + ToDate + "','N','" + ClearInject(txtRemarks.Text.Trim()) + "','" + ClearInject(txtActiveStatus.Text.Trim()) + "',";
                Sql += "'Modified by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "',";
                Sql += "'" + Session["UserName"] + "','" + Session["UserID"] + "','" + ClearInject(txtIPAdrs.Text.Trim()) + "',";
                Sql += "'Y','" + DDlCategory.SelectedValue + "') ";
            }
            else
            {

                Sql = " Insert into M_NewsSeminarMaster(NewsId,NewsHdr,NewsDtl,FrmDate,ToDate,NType,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus,Rankid)";
                Sql += " Select Case When Max(NewsId) Is Null Then '1' Else Max(NewsId)+1 END as NewsId,'" + ClearInject(txtHeading.Text.Trim()) + "','" + ClearInject(txtDetail.Text.Trim()) + "','" + FrmDate + "','" + ToDate + "','N','" + ClearInject(txtRemarks.Text.Trim()) + "',";
                Sql += " '" + ClearInject(txtActiveStatus.Text.Trim()) + "','New by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "','" + Session["UserName"] + "',";
                Sql += "'" + Session["UserID"] + "','" + ClearInject(txtIPAdrs.Text.Trim ()) + "','Y','" + DDlCategory.SelectedValue + "' From  M_NewsSeminarMaster ";
            }
            string Str_Sql = string.Empty;
            Str_Sql = "Begin Try   Begin Transaction " + Sql + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
            int updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql));
            if (updateEffect > 0)
                if (!string.IsNullOrEmpty(Request["NewsId"]) && updateEffect != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Successfully Updated.!');location.replace('NewsNSeminarMaster.aspx');", true);
                }
                else if (updateEffect != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Save Successfully .!');location.replace('NewsNSeminarMaster.aspx');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Data not saved Successfully.!');", true);
                }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Group Already Exist.!');location.replace('NewsNSeminarMaster.aspx');", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}