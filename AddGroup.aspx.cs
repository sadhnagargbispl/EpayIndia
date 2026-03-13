using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class AddGroup : System.Web.UI.Page
{
    string GroupIdQS;
    ModuleFunction objModuleFun = new ModuleFunction();
    DataTable dtData = new DataTable();
    DataTable Dt = new DataTable();
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Request["GroupId"]) == false)
            {
                GroupIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["GroupId"]));
            }
            if (!Page.IsPostBack)
            {
                ClearAll();
                if (Session["AStatus"] != null)
                {
                    this.BtnSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnSave));
                    if (string.IsNullOrEmpty(Request["GroupId"]) == false)
                    {
                        BtnSave.Text = "Modify";
                        BindData();
                    }
                }
                else

                {

                    string scrname = "<SCRIPT language='javascript'> window.top.location.reload();" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close", scrname, false);
                }
            }
            txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress();
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
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    private void BindData()
    {
        try
        {
            string sql = objDal.IsoStart + "Select * From " + objDal.DBName + "..M_UserGroupMaster ";
            sql += " Where GroupId='" + GroupIdQS + "' AND rowStatus='Y'" + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                txtGrpName.Text = Dt.Rows[0]["GroupName"].ToString();
                txtRemarks.Text = Dt.Rows[0]["Remarks"].ToString();
                txtGrpID.Text = Dt.Rows[0]["GroupId"].ToString();
                txtActiveStatus.Text = Dt.Rows[0]["ActiveStatus"].ToString();
                if (txtActiveStatus.Text.ToUpper().Equals("Y"))
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
    private bool CheckUser()
    {
        bool Result = false;
        try
        {
            string sql = "";
            if (Request["GroupId"] == "")
            {
                sql = objDal.IsoStart + "select 1 from " + objDal.DBName + "..M_UserGroupMaster ";
                sql += " Where GroupName='" + ClearInject(txtGrpName.Text) + "' and activeStatus='Y' and RowStatus='Y'" + objDal.IsoEnd;
            }
            else
            {
                sql = objDal.IsoStart + "select 1 from " + objDal.DBName + "..M_UserGroupMaster ";
                sql += " where GroupName='" + ClearInject(txtGrpName.Text) + "' and activeStatus='Y' and RowStatus='Y'"; 
                sql += " and GroupId <>'" + Convert.ToInt32(GroupIdQS) + "'" + objDal.IsoEnd;
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
            throw new Exception(Ex.Message);
        }
        return Result;

    }


    protected void BtnSave_Click(object sender, EventArgs e)
    {
        string Sql = "";
        if (rdblist.SelectedIndex == 0)
        {
            txtActiveStatus.Text = "Y";
        }
        else
        {
            txtActiveStatus.Text = "N";
        }
        if (CheckUser() == true)
        {
            if (!string.IsNullOrEmpty(Request["GroupId"]))
            {
                Sql = "Update M_UserGroupMaster SET RowStatus='N' Where GroupId='" + GroupIdQS + "';";
                Sql += " Insert into M_UserGroupMaster (GroupId,GroupName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) ";
                Sql += "Values('" + Convert.ToInt32(txtGrpID.Text.Trim()) + "','" + ClearInject(txtGrpName.Text.Trim()) + "','" + ClearInject(txtRemarks.Text.Trim()) + "',";
                Sql += "'" + ClearInject(txtActiveStatus.Text.Trim()) + "','Modified by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "','" + Session["UserName"] + "',";
                Sql += "'" + Convert.ToInt32(Session["UserID"]) + "','" + ClearInject(txtIPAdrs.Text.Trim()) + "','Y')";
            }
            else
            {
                Sql = "Insert into M_UserGroupMaster(GroupId,GroupName,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus)";
                Sql += "  Select Case When Max(GroupId) Is Null Then '1' Else Max(GroupId)+1 END as GroupId,'" + ClearInject(txtGrpName.Text.Trim()) + "','" + ClearInject(txtRemarks.Text.Trim()) + "','" + ClearInject(txtActiveStatus.Text.Trim()) + "',";
                Sql += " 'New by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "','" + Session["UserName"] + "',";
                Sql += "'" + Convert.ToInt32(Session["UserID"]) + "','" + ClearInject(txtIPAdrs.Text.Trim ()) + "','Y' From M_UserGroupMaster";
            }
            string Str_Sql = string.Empty;
            Str_Sql = "Begin Try   Begin Transaction " + Sql + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
            int updateEffect = 0;
            updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Str_Sql));
            if (!string.IsNullOrEmpty(Request["GroupId"]) && updateEffect != 0)

            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Successfully Updated.!');location.replace('GroupMaster.aspx');", true);
            }
            else if (updateEffect != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Save Successfully.!');location.replace('GroupMaster.aspx');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Data not saved Successfully.!');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Group Already Exist.!');", true);
        }
    }
    private void ClearAll()
    {
        try
        {
            txtGrpName.Text = "";
            txtGrpID.Text = "";
            txtRemarks.Text = "";
            txtActiveStatus.Text = "";
            txtIPAdrs.Text = "";
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
            Response.Redirect("GroupMaster.aspx", false);
        }
        catch (Exception Ex)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
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