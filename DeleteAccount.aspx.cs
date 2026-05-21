using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Text;
//using System.Windows.Interop;
using System.Web.UI.WebControls;
//using DocumentFormat.OpenXml.Wordprocessing;
public partial class DeleteAccount : System.Web.UI.Page
{
    string scrname;
    string TransferId;
    DataTable dt1;
    DataTable dt2;
    string MobileNo1 = "";
    DAL ObjDAL = new DAL();
    DataTable Dt = new DataTable();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string IsoStart;
    string IsoEnd;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdSave1.Attributes.Add("onclick", DisableTheButton(this.Page, this.cmdSave1));
            if (!Page.IsPostBack)
            {
                HdnCheckTrnns.Value = GenerateRandomStringActive(6);
                // FillDetail();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void FillDetail()
    {
        try
        {
            string idverified = "";

            string sql = IsoStart + "exec sp_FetchID_ByIDNo '" + Session["Formno"] + "' " + IsoEnd;
            DataTable dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                TxtSerialno.Text = dt.Rows[0]["RefIDNo"] == DBNull.Value ? "" : dt.Rows[0]["RefIDNo"].ToString();
                TxtSpName.Text = dt.Rows[0]["MemName"].ToString();
                SetReadOnly(TxtSerialno);
                SetReadOnly(TxtSpName);
                // ================= UPDATE BUTTON LOGIC =================
                bool allFilled =
                    !string.IsNullOrWhiteSpace(TxtSerialno.Text) &&
                    !string.IsNullOrWhiteSpace(TxtSpName.Text);

                cmdSave1.Visible = !allFilled;
            }
        }
        catch (Exception ex)
        {

        }

    }
    private void SetReadOnly(TextBox txt)
    {
        txt.ReadOnly = !string.IsNullOrWhiteSpace(txt.Text);
    }
    public string GenerateRandomStringActive(int iLength)
    {
        try
        {
            Random rdm = new Random();
            char[] allowChrs = "123456789".ToCharArray();
            string sResult = "";

            for (int i = 0; i < iLength; i++)
            {
                sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
            }
            return sResult;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
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
    private string GetName()
    {
        try
        {
            DataTable dt = new DataTable();
            string str = IsoStart + "Exec CheckDeleteAccount '" + TxtSerialno.Text.Trim() + "'" + IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string msg = dt.Rows[0]["Msg"].ToString();
                if (msg.ToUpper() == "OK")
                {
                    TxtSpName.Text = dt.Rows[0]["MemName"].ToString();
                    return "OK";
                }
                else
                {
                    scrname = "<SCRIPT>alert('" + msg + "');</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    TxtSerialno.Text = "";
                    return "";
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
        return "";
    }
    protected void TxtSerialno_TextChanged(object sender, EventArgs e)
    {
        GetName();
    }
    protected void cmdSave1_Click(object sender, EventArgs e)
    {
        try
        {
            if (TxtSerialno.Text == "")
            {
                scrname = "<SCRIPT>alert('Please Enter ID.!');</SCRIPT>";
                this.RegisterStartupScript("MyAlert", scrname);
                return;
            }
            else
            {

                UpdateParentID();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void UpdateParentID()
    {
        try
        {
            string str = "";
            try
            {
                string StrSql1 = "Insert into Trnactivecadmin (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
                int i = 0;
                try
                {
                    i = ObjDAL.SaveData(StrSql1);
                }
                catch
                {

                }
                if (i > 0)
                {
                    double I = 0;
                    string query = "Exec SA_UpdateDeleteAccount '" + TxtSerialno.Text + "'";
                    string Remark = "Delete Account Update " + TxtSerialno.Text;
                    query += " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId) Values " +
                             "(0,'','Delete Account Update','Delete Account Update','" + Remark + "',Getdate(),'" + TxtSerialno.Text + "')";
                    I = ObjDAL.SaveData(query);
                    if (I > 0)
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Your account deletion request has been received. Our team will contact you if any further information is required.!');location.replace('DeleteAccount.aspx');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrname, false);
                        return;
                    }
                    else
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Your account deletion request has been not received.!');location.replace('DeleteAccount.aspx');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrname, false);
                        return;
                    }
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('Try Later.!');location.replace('DeleteAccount.aspx');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrname, false);
                    return;
                }

            }
            catch (Exception ex)
            {
                string path = HttpContext.Current.Request.Url.AbsoluteUri;
                string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
                ObjDAL.WriteToFile(text + ex.Message);
                Response.Write("Try later.");
                Response.Write(ex.Message);
                Response.End();
            }
        }
        catch (Exception)
        {
            // Top-level catch intentionally empty like original VB code
        }
    }
}
