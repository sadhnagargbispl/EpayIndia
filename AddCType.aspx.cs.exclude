using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
public partial class AddCType : System.Web.UI.Page
{

    string CTypeIdQS;
    ModuleFunction objModuleFun = new ModuleFunction();
    DataTable Dt = new DataTable();
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.BtnSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.BtnSave));
        objModuleFun = new ModuleFunction();
        if (string.IsNullOrEmpty(Request["Type"]) == false)
        {
            CTypeIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["Type"]));
        }

        if (!Page.IsPostBack)
        {
            ClearAll();

            if (Session["AStatus"] != null)
            {
                if (string.IsNullOrEmpty(Request["Type"]) == false)
                {
                    BtnSave.Text = "Modify";
                    BindData();
                }
                FillUser();
            }
            else
            {
                string scrname = "<SCRIPT language='javascript'> window.top.location.reload();" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close", scrname, false);
            }
        }
        txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress();


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
            string sql = objDal.IsoStart + "Select TID,CTypeID,CType,Activestatus,Remarks,RecTimeStamp,Rowstatus,LastModified,UserID,ToUserid,ToUserEmail ";
            sql += " From " + objDal.DBName + "..M_ComplaintTypeMaster Where CTypeId='" + CTypeIdQS + "' AND rowStatus='Y'" + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                txtCType.Text = Dt.Rows[0]["CType"].ToString();
                txtRemarks.Text = Dt.Rows[0]["Remarks"].ToString();
                txtCTypeID.Text = Dt.Rows[0]["CTypeId"].ToString();
                txtActiveStatus.Text = Dt.Rows[0]["ActiveStatus"].ToString();
                TxtEmail.Text = Dt.Rows[0]["ToUserEmail"].ToString();

                DDlUser.SelectedValue = Dt.Rows[0]["ToUserId"].ToString();
                if (string.Equals(txtActiveStatus.Text.ToUpper(), "Y") == true)
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
    private void FillUser()
    {
        DataTable Dt = new DataTable();
        string strquery = string.Empty;
        strquery = objDal.IsoStart + "SELECT UserId, Username FROM " + objDal.DBName + "..M_Usermaster ";
        strquery += "WHERE ACTIVESTATUS='Y' AND RowStatus='Y' ORDER BY UserId" + objDal.IsoEnd;
        Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strquery).Tables[0];
        DDlUser.DataSource = Dt;
        DDlUser.DataValueField = "UserId";
        DDlUser.DataTextField = "UserName";
        DDlUser.DataBind();
        DDlUser.SelectedIndex = 1;

    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {

        try
        {
            string sql = "";
            if (rdblist.SelectedIndex == 0)
            {
                txtActiveStatus.Text = "Y";
            }
            else
            {
                txtActiveStatus.Text = "N";
            }

            if (!string.IsNullOrEmpty(Request["Type"]))
            {
                sql = "Update M_ComplaintTypeMaster SET RowStatus='N' Where CTypeId='" + CTypeIdQS + "';";
                sql += " Insert into M_ComplaintTypeMaster(CTypeId,CType,Remarks,ActiveStatus,LastModified,UserId,ToUserId,ToUserEmail)";
                sql += " Values('" + Convert.ToInt32(ClearInject(txtCTypeID.Text)) + "',@CType,@Remarks,'" + ClearInject(txtActiveStatus.Text) + "',";
                sql += "'Modified by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "','" + Convert.ToInt32(Session["UserID"]) + "',@ToUserId,@ToUserEmail)";
            }
            else
            {
                sql = "Insert into M_ComplaintTypeMaster(CTypeId,CType,Remarks,ActiveStatus,LastModified,UserId,RowStatus,ToUserId,ToUserEmail)";
                sql += " Select Case When Max(CTypeId) Is Null Then '1' Else Max(CTypeId)+1 END as CTypeId,@CType,@Remarks,'" + ClearInject(txtActiveStatus.Text) + "',";
                sql += "'New by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "','" + Convert.ToInt32(Session["UserID"]) + "',";
                sql += "'Y',@ToUserId,@ToUserEmail From M_ComplaintTypeMaster";
            }
            string parameters = "@CType;@Remarks;@ToUserId;@ToUserEmail";
            string parameterValues = $"{ClearInject(txtCType.Text)};{ClearInject(txtRemarks.Text)};{Convert.ToInt32(DDlUser.SelectedValue)};{ClearInject(TxtEmail.Text)}";
            int updateEffect = 0;
            updateEffect = objDal.UpdateData(sql, parameters, parameterValues);


            if (!string.IsNullOrEmpty(Request["Type"]) && updateEffect > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Successfully Updated.!');location.replace('ComplaintType.aspx');", true);

            }
            else if (updateEffect != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Save Successfully .!');location.replace('ComplaintType.aspx');", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Data not saved Successfully.!');", true);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('ADDCtype Already Exist.!');", true);

        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }




    private void ClearAll()
    {
        try
        {
            txtCType.Text = "";
            txtCTypeID.Text = "";
            txtRemarks.Text = "";
            txtActiveStatus.Text = "";
            txtIPAdrs.Text = "";
            TxtEmail.Text = "";
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
            Response.Redirect("ComplaintType.aspx", false);
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
