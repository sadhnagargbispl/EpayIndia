using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddCoin : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    string scrname;
    DAL objDAL;
    DataSet Ds = new DataSet();
    ModuleFunction objModuleFun = new ModuleFunction();
    string CTypeIdQS;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["AStatus"] != null && Session["AStatus"].ToString() == "OK")
        {
            // Do nothing, user is authenticated
        }
        else
        {
            Response.Redirect("Default.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!string.IsNullOrEmpty(Request["Type"]))
        {
            CTypeIdQS = Crypto.Decrypt(objModuleFun.EncodeBase64(Request["Type"]));
        }

        if (!IsPostBack)
        {
            ClearAll();
            if (Session["AStatus"] != null && Session["AStatus"].ToString() == "OK")
            {
                if (!string.IsNullOrEmpty(Request["Type"]))
                {
                    BtnSave.Text = "Modify";
                    BindData();
                }
                FillStackList();
            }
            else
            {
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();</SCRIPT>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close", scrname, false);
            }
        }
        txtIPAdrs.Text = objModuleFun.GetVisitorIPAddress();
    }

    private void FillStackList()
    {
        try
        {
            Ds = SqlHelper.ExecuteDataset(constr1, "sp_ddlStackList");
            DDLStackType.DataSource = Ds.Tables[0];
            DDLStackType.DataValueField = "Value";
            DDLStackType.DataTextField = "Name";
            DDLStackType.DataBind();
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

            DataTable Dt = new DataTable();
            string sql = "Select * From "+ objDAL.DBName +"..trnprofit Where Pid='" + CTypeIdQS + "' ";
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                txtCType.Text = Dt.Rows[0]["Profit"].ToString();
                txtCTypeID.Text = Dt.Rows[0]["Pid"].ToString();
                txtActiveStatus.Text = Dt.Rows[0]["statusapi"].ToString();
                DDLStackType.SelectedValue = Dt.Rows[0]["Month"].ToString();
                if (string.Equals(txtActiveStatus.Text.ToUpper(), "Y", StringComparison.OrdinalIgnoreCase))
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
        string sql;
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
           
                sql = "UPDATE trnprofit SET Profit='" + txtCType.Text.Trim().ToUpper() + "',statusapi='" + txtActiveStatus.Text + "',Month='" + DDLStackType .SelectedValue + "', " +
                  "RectimeStamp=GETDATE(),UserId='" + Convert.ToInt32(Session["UserID"]) + "', " +
                  "LastModified='Modified by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "' " +
                  "WHERE Pid='" + Convert.ToInt32(txtCTypeID.Text) + "'";
           
        }
        else
        {
           DataTable Dt1 = new DataTable();
          string sql1 = "Select * From trnprofit Where Month=" + DDLStackType.SelectedValue + " and StatusApi='Y' ";
           Dt1 = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql1).Tables[0];
            if (Dt1.Rows.Count > 0)
            {
                sql = "UPDATE trnprofit SET statusapi='N' WHERE Month='" + DDLStackType.SelectedValue + "' and statusapi='Y'; " +
                 "INSERT INTO trnprofit(Pid, Profit, statusapi, RectimeStamp, LastModified, UserId,Month,sessid,Msessid) " +
                 "SELECT CASE WHEN MAX(PId) IS NULL THEN '1' ELSE MAX(PId) + 1 END AS PId, '" + txtCType.Text + "', " +
                 "'" + txtActiveStatus.Text + "', GETDATE(), 'New by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "', " +
                 "'" + Convert.ToInt32(Session["UserID"]) + "','" + DDLStackType.SelectedValue + "',convert(VARCHAR, cast(cast(getdate() AS VARCHAR) AS DATETIME), 112),(SELECT IsNULL(Max(SessID), 1)FROM M_MonthSessnMaster) FROM trnprofit";
            }
            else {

                sql = "INSERT INTO trnprofit(Pid, Profit, statusapi, RectimeStamp, LastModified, UserId,Month,sessid,Msessid) " +
                      "SELECT CASE WHEN MAX(PId) IS NULL THEN '1' ELSE MAX(PId) + 1 END AS PId, '" + txtCType.Text + "', " +
                      "'" + txtActiveStatus.Text + "', GETDATE(), 'New by " + Session["UserName"] + " at " + DateTime.Now.ToString() + "', " +
                      "'" + Convert.ToInt32(Session["UserID"]) + "','" + DDLStackType.SelectedValue + "',convert(VARCHAR, cast(cast(getdate() AS VARCHAR) AS DATETIME), 112),(SELECT IsNULL(Max(SessID), 1)FROM M_MonthSessnMaster) FROM trnprofit";
            }
        }
        int updateEffect = 0;
        updateEffect = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql));

        if (!string.IsNullOrEmpty(Request["Type"]) && updateEffect != 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Save Successfully .!');location.replace('CoinMaster.aspx');", true);
     
        }
        else if (updateEffect != 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Save Successfully .!');location.replace('CoinMaster.aspx');", true);
        }
        else
        {
            scrname = "<SCRIPT language='javascript'>alert('Data not saved Successfully!!');</SCRIPT>";
        }

       
    }

    private void ClearAll()
    {
        try
        {
            txtCType.Text = "";
            txtCTypeID.Text = "";
            txtActiveStatus.Text = "";
            txtIPAdrs.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }


    protected void DDLStackType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}
