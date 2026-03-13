using Irony;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddUser : System.Web.UI.Page
{
    string UserIdQS;
    DataTable Dt = new DataTable();
    DAL objDal = new DAL();
    ModuleFunction objModuleFun = new ModuleFunction();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request["UserId"]))
            {
                UserIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["UserId"]));
            }
            this.BtnSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnSave));
            this.BtnSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnSave));
            if (!Page.IsPostBack)
            {
                ClearAll();
                FillGroup();
                txtGrpID.Text = Session["GroupId"].ToString();
                ddlGroup.SelectedValue = Session["GroupId"].ToString();
                txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress();
                if (!string.IsNullOrEmpty(Request["UserId"]))
                {
                    BtnSave.Text = "Modify";
                    BindData();
                }
            }
        }
        catch (Exception Ex)
        {
            string scrname = "<SCRIPT language='javascript'>alert('" + Ex.Message + "');</SCRIPT>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scrname, true);
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
    private void BindData()

    {
        try
        {
            string sql = objDal.IsoStart + "Select a.*, b.GroupName From " + objDal.DBName + "..M_UserMaster as a,";
            sql += " " + objDal.DBName + "..M_UserGroupMaster as b";
            sql += " Where a.GroupId = b.GroupId and a.UserId='" + UserIdQS + "' AND a.RowStatus='Y' ";
            sql += "and b.activeStatus='Y' and b.RowStatus='Y'" + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                txtGrpID.Text = Dt.Rows[0]["GroupId"].ToString();
                ddlGroup.SelectedValue = Dt.Rows[0]["GroupId"].ToString();
                txtUsrName.Text = Dt.Rows[0]["UserName"].ToString();
                txtPswd.Text = Dt.Rows[0]["Passw"].ToString();
                txtPswd.Attributes.Add("value", Dt.Rows[0]["Passw"].ToString());
                txtRemarks.Text = Dt.Rows[0]["Remarks"].ToString();
                TxtMobileNo.Text = Dt.Rows[0]["MobileNo"].ToString();
                txtUserID.Text = Dt.Rows[0]["UserId"].ToString();
                txtActiveStatus.Text = Dt.Rows[0]["ActiveStatus"].ToString();

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
    private void FillGroup()
    {
        try
        {
            string sql = objDal.IsoStart + "select * from " + objDal.DBName + "..M_UserGroupMaster where rowstatus ='Y' AND activestatus ='Y'" + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                ddlGroup.DataSource = Dt;
                ddlGroup.DataValueField = "GroupId";
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataBind();
                Session["Groupmaster"] = Dt;
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
            txtUsrName.Text = "";
            txtUserID.Text = "";
            txtRemarks.Text = "";
            txtActiveStatus.Text = "";
            txtIPAdrs.Text = "";
            txtGrpID.Text = "";
            TxtMobileNo.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql;
            if (checkuser())
            {
                if (rdblist.SelectedIndex == 0)
                {
                    txtActiveStatus.Text = "Y";
                }
                else
                {
                    txtActiveStatus.Text = "N";
                }
                if (string.IsNullOrEmpty(Request["UserId"]) == false)
                {
                    Sql = "Update M_UserMaster SET RowStatus='N' Where UserId='" + UserIdQS + "';";
                    Sql += " Insert into M_UserMaster(GroupId,UserId,UserName,Passw,Remarks,ActiveStatus,LastModified,UserCode,UsrId,IPAdrs,RowStatus,MobileNo)" +
                        " Values('" + ddlGroup.SelectedValue  + "','" + ClearInject(txtUserID.Text) + "','" + ClearInject(txtUsrName.Text) + "','" + ClearInject(txtPswd.Text) + "','" + ClearInject(txtRemarks.Text) + "','" + ClearInject(txtActiveStatus.Text) + "'," +
                        " 'Modified by " + ClearInject(Session["UserName"].ToString())   + " at " + DateTime.Now.ToString() + "','" + ClearInject(Session["UserName"].ToString()) + "','" + Session["UserID"] + "','" + txtIPAdrs.Text + "','Y','" + ClearInject(TxtMobileNo.Text) + "')";
                }
                else

                {
                    Sql = " Insert into M_UserMaster(GroupId,UserId,UserName,Passw,Remarks,ActiveStatus,LastModified,UserCode,UsrId,IPAdrs,RowStatus,MobileNo) ";
                    Sql += " Select '" + ddlGroup.SelectedValue + "',Case When Max(UserId) Is Null Then '1' Else Max(UserId)+1 END as UserId,";
                    Sql += "'" + ClearInject(txtUsrName.Text) + "','" + ClearInject(txtPswd.Text) + "','" + ClearInject(txtRemarks.Text) + "','" + ClearInject(txtActiveStatus.Text) + "','New by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "',";
                    Sql += "'" + ClearInject(Session["UserName"].ToString()) + "','" + ClearInject(Session["UserID"].ToString()) + "','" + txtIPAdrs.Text + "','Y','" + ClearInject(TxtMobileNo.Text) + "' From " + objDal.tblUserMaster;
                }
                string Str_Sql = string.Empty;
                Str_Sql = "Begin Try   Begin Transaction " + Sql + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
                int updateEffect = 0;
                updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_Sql));
                if (!string.IsNullOrEmpty(Request["UserId"]) && updateEffect != 0)

                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Successfully Updated.!');location.replace('UserMaster.aspx');", true);
                }
                else if (updateEffect != 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Save Successfully.!');location.replace('UserMaster.aspx');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Data not saved Successfully.!');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('User Already Exist.!');", true);
            }
        }
        catch (Exception ex)
        {
            string scrname = "<SCRIPT language='javascript'>alert('" + ex.Message + "');</SCRIPT>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scrname, true);
        }
    }
    private bool checkuser()
    {
        bool Result = false;
        try
        {
            string sql = "";
            if (Request.QueryString["UserId"] == null || Request.QueryString["UserId"] == "")
            {
                sql = objDal.IsoStart + "select * from " + objDal.DBName + ".. M_UserMaster where UserName='" + ClearInject(txtUsrName.Text) + "' and activeStatus='Y' and RowStatus='Y'" + objDal.IsoEnd;
            }
            else
            {
                sql = objDal.IsoStart + "select * from " + objDal.DBName + ".. M_UserMaster where UserName='" + ClearInject(txtUsrName.Text) + "' and activeStatus='Y' and RowStatus='Y' and UserId <> '" + int.Parse(UserIdQS) + "'" + objDal.IsoEnd;
            }
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count == 0)
            {
                Result = true;
            }
            else
            {
                Result = false;
            }
        }
        catch (Exception Ex)
        {
            Result = false;
            throw new Exception(Ex.Message);

        }
        return Result;

    }
    protected void txtUsrName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (checkuser())
            {
                string scrname = "<SCRIPT language='javascript'>alert('User Name Already Exist!!');</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close", scrname, false);
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
            Response.Redirect("UserMaster.aspx", false);
        }
        catch (Exception Ex)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
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

}