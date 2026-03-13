using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class AddPackage : System.Web.UI.Page
{
    string GroupIdQS;
    ModuleFunction objModuleFun = new ModuleFunction();
    DataTable Dt = new DataTable();
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    private int CurrentPageIndex;
    private readonly object pagerDataList;
    string KitIdQS;
    protected void Page_Load(object sender, EventArgs e)

    {
        try

        {
            BtnSave.Attributes.Add("onclick", DisableTheButton(Page, BtnSave));
            objModuleFun = new ModuleFunction();
            if (!string.IsNullOrEmpty(Request["KitId"]))
            {
                KitIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["KitId"]));
            }
            if (!Page.IsPostBack)
            {
                ClearAll();
                if (Session["AStatus"] != null)
                {
                    if (!string.IsNullOrEmpty(Request["KitId"]))
                    {
                        BtnSave.Text = "Modify";
                        BindData();
                    }
                    else
                    {
                        Fill_SeriesStart();
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
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void Fill_SeriesStart()
    {
        try
        {
            string Sql = objDal.IsoStart + " Select Case When Max(KitId) Is Null Then '100001' Else (Max(KitId)+1)* 100000 +1 END as SrStart FROM " + objDal.DBName + "..MM_KitMaster " + objDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql).Tables[0];
            if (Dt.Rows.Count > 0)

            {
                txtSerialStart.Text = Dt.Rows[0]["SrStart"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void BindData()
    {
        string Dat1;
        try
        {
            string sql = objDal.IsoStart + "Select * From " + objDal.DBName + "..MM_KitMaster  Where KitId='" + KitIdQS + "' AND " + objDal.activeCondition + objDal.IsoEnd;
            Dt = new DataTable();
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                LblKitDate.Text = Convert.ToDateTime(Dt.Rows[0]["Rectimestamp"]).ToString("dd-MMM-yyyy");
                Dat1 = DateTime.Now.ToString("dd-MMM-yyyy");
                if (LblKitDate.Text == Dat1)
                {
                    txtBV.ReadOnly = false;
                    txtPV.ReadOnly = false;
                    txtRP.ReadOnly = false;

                }
                else
                {
                    txtBV.ReadOnly = true;
                    txtPV.ReadOnly = true;
                    txtRP.ReadOnly = true;
                }
                txtKitId.Text = Dt.Rows[0]["KitId"].ToString();
                txtkitName.Text = Dt.Rows[0]["KitName"].ToString();
                txtJoinAmt.Text = Dt.Rows[0]["JoinAmount"].ToString();
                txtKitAmt.Text = Dt.Rows[0]["KitAmount"].ToString();
                txtKitUnit.Text = Dt.Rows[0]["KitUnit"].ToString();
                txtSerialStart.Text = Dt.Rows[0]["SerialStart"].ToString();
                txtRefIn.Text = Dt.Rows[0]["RefIncome"].ToString();
                txtPoolIn.Text = Dt.Rows[0]["PoolIncome"].ToString();
                txtSpillIn.Text = Dt.Rows[0]["SpillIncome"].ToString();
                txtBinaryIn.Text = Dt.Rows[0]["BinaryIncome"].ToString();
                txtBV.Text = Dt.Rows[0]["BV"].ToString();
                txtPV.Text = Dt.Rows[0]["PV"].ToString();
                txtRP.Text = Dt.Rows[0]["RP"].ToString();
                TxtTopUp.Text = Dt.Rows[0]["TopUpSeq"].ToString();
                txtCapping.Text = Dt.Rows[0]["Capping"].ToString();
                txtRemarks.Text = Dt.Rows[0]["Remarks"].ToString();
                txtActiveStatus.Text = Dt.Rows[0]["ActiveStatus"].ToString();
                RbtColor.SelectedValue = Dt.Rows[0]["JoinColor"].ToString();
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

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string Sql;
            string Str;
            string KitId = "";
            string JoinColr = "";
            if (rdblist.SelectedIndex == 0)
            {
                txtActiveStatus.Text = "Y";
            }
            else
            {
                txtActiveStatus.Text = "N";
            }
            if (Convert.ToDouble(txtKitAmt.Text) > 0)
            {
                JoinColr = "Green.jpg";
            }
            else
            {
                JoinColr = "red.jpg";
            }
            if (!string.IsNullOrEmpty(Request["KitId"]))
            {
                Str = "Insert into TempMMKitMaster([KId],[KitId],[KitName],[JoinAmount],[KitAmount],[KitUnit],[SerialStart],[RefIncome],[PoolIncome],[SpillIncome],[BinaryIncome],[BV],[PV],[RP],[Capping],[Remarks],";
                Str += "[ActiveStatus],[RecTimeStamp],[LastModified],[UserCode],[UserId],[JoinStatus],[JoinColor],[AllowTopUp],[Statement],[IsBill],[SP],[OnWebSite],[TopUpSeq],[MRecTimeStamp],[MUserID])Select [KId], [KitId],[KitName],[JoinAmount],[KitAmount],";
                Str += "[KitUnit],[SerialStart],[RefIncome],[PoolIncome],[SpillIncome],[BinaryIncome],[BV],[PV],[RP],[Capping],[Remarks],[ActiveStatus],[RecTimeStamp],[LastModified],[UserCode],[UserId],[JoinStatus],";
                Str += "[JoinColor],[AllowTopUp],[Statement],[IsBill],[SP],[OnWebSite],[TopUpSeq],GetDate(),'" + Session["UserID"] + "' from MM_KitMaster as a where a.KitId='" + txtKitId.Text + "';";

                Sql = Str + ";Update MM_KitMaster set KitId='" + txtKitId.Text + "',KitName='" + ClearInject(txtkitName.Text) + "',JoinAmount='" + txtJoinAmt.Text + "',";
                Sql += "KitAmount='" + ClearInject(txtKitAmt.Text) + "',KitUnit='" + ClearInject(txtKitUnit.Text) + "',SerialStart='" + ClearInject(txtSerialStart.Text) + "',";
                Sql += "RefIncome='" + ClearInject(txtRefIn.Text) + "',PoolIncome='" + ClearInject(txtPoolIn.Text) + "',";
                Sql += "SpillIncome='" + ClearInject(txtSpillIn.Text) + "',BinaryIncome='" + ClearInject(txtBinaryIn.Text) + "',BV='" + ClearInject(txtBV.Text) + "',PV='" + ClearInject(txtPV.Text) + "',RP='" + ClearInject(txtRP.Text) + "',";
                Sql += "Capping='" + ClearInject(txtCapping.Text) + "',Remarks='" + ClearInject(txtRemarks.Text) + "',ActiveStatus='" + ClearInject(txtActiveStatus.Text) + "',";
                Sql += "LastModified='Modified by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "',";
                Sql += "JoinColor='" + RbtColor.SelectedValue + "',UserCode='" + Session["UserName"] + "',UserId='" + Session["UserID"] + "',TopUpSeq='" + ClearInject(TxtTopUp.Text) + "' where KitId='" + ClearInject(txtKitId.Text) + "'";
            }
            else
            {
                Str = "select Case When Max(KitId) Is Null Then '1' Else Max(KitId)+1 END as KitId from MM_KitMaster ";
                DataTable Dt = new DataTable();
                Dt = objDal.GetData(Str);
                if (Dt.Rows.Count > 0)
                {
                    KitId = Dt.Rows[0]["KitId"].ToString();
                }
                Sql = " insert into MM_KitMaster (KitId,KitName,JoinAmount,KitAmount,KitUnit,SerialStart," +
                    "RefIncome,PoolIncome,SpillIncome,BinaryIncome,BV,PV,RP,Capping,Remarks,ActiveStatus," +
                    "LastModified,UserCode,UserId,IPAdrs,RowStatus,JoinColor,TopUpSeq) " +
                    "Select Case When Max(KitId) Is Null Then '1' Else Max(KitId)+1 END as KitId," +
                    "'" + ClearInject(txtkitName.Text) + "','" + Convert.ToDouble(txtJoinAmt.Text) + "'," +
                    "'" + Convert.ToDouble(txtKitAmt.Text) + "','" + Convert.ToDouble(txtKitUnit.Text) + "',";
                Sql += "Case When Max(KitId) Is Null Then '100001' Else (Max(KitId)+1)* 100000 +1 END,'" + Convert.ToDouble(txtRefIn.Text) + "',";
                Sql += "'" + Convert.ToDouble(txtPoolIn.Text) + "','" + Convert.ToDouble(txtSpillIn.Text) + "','" + Convert.ToDouble(txtBinaryIn.Text) + "',";
                Sql += "'" + Convert.ToDouble(txtBV.Text) + "','" + Convert.ToDouble(txtPV.Text) + "','" + Convert.ToDouble(txtRP.Text) + "', ";
                Sql += "'" + Convert.ToDouble(txtCapping.Text) + "','" + ClearInject(txtRemarks.Text) + "','" + txtActiveStatus.Text + "',";
                Sql += "'Modified by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "','" + Session["UserName"] + "',";
                Sql += "'" + Convert.ToDouble(Session["UserID"]) + "','" + ClearInject(txtIPAdrs.Text) + "','Y','" + RbtColor.SelectedValue + "',";
                Sql += "'" + Convert.ToDouble(TxtTopUp.Text) + "' From MM_KitMaster ";
            }

            string Str_Sql = string.Empty;
            Str_Sql = "Begin Try   Begin Transaction " + Sql + "  Commit Transaction  End Try  BEGIN CATCH  ROLLBACK Transaction END CATCH";
            int updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql));
            if (updateEffect > 0)
            {
                if (!string.IsNullOrEmpty(Request["KitId"]) && updateEffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Successfully Updated.!');location.replace('PackageMaster.aspx');", true);

                }
                else if (updateEffect > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Save Successfully.!');location.replace('PackageMaster.aspx');", true);
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
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("PackageMaster.aspx", false);
        }
        catch (Exception Ex)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
        }
    }
    private void ClearAll()
    {
        try
        {

            txtKitId.Text = "";
            txtkitName.Text = "";
            txtJoinAmt.Text = "0";
            txtKitAmt.Text = "0";
            txtKitUnit.Text = "0";
            txtSerialStart.Text = "";
            txtRefIn.Text = "0";
            txtPoolIn.Text = "0";
            txtSpillIn.Text = "0";
            txtBinaryIn.Text = "0";
            txtBV.Text = "0";
            txtPV.Text = "0";
            txtRP.Text = "0";
            txtCapping.Text = "0";
            txtRemarks.Text = "";
            txtActiveStatus.Text = "";
            txtIPAdrs.Text = "";
            TxtTopUp.Text = "0";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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