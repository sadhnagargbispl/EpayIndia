using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using ClosedXML.Excel;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reply : System.Web.UI.Page
{

    DataTable Dt = new DataTable();
    string scrname;
    DAL objDAL = new DAL();
    ModuleFunction objModuleFun;
    string CIdQS;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BtnSave.Attributes.Add("onclick", DisableTheButton(Page, BtnSave));
            if (Session["AStatus"] != null)

                if (Request["CId"] != null && !Page.IsPostBack)
                {
                    CIdQS = Request["CId"];

                    if (Session["AStatus"] == "OK")
                    {
                        if (Request["CId"] != null)
                        {
                            BindData();
                            HdnCheckTrnns.Value = GenerateRandomString(6);
                        }
                    }
                    else
                    {
                        string scrname = "<SCRIPT language='javascript'> window.top.location.reload();" + "</SCRIPT>";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Close", scrname, false);
                    }
                }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public string GenerateRandomString(int iLength)
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

    private void BindData()
    {
        try
        {
            CIdQS = Request["CId"];
            string sql = objDAL.IsoStart + " Select M.IDNo,M.MemName,";
            sql += "ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate,M.CType,M.Complaint ,";
            sql += "ISNULL(S.Solution,'') as Solution,ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate FROM";
            sql += " (Select b.MemFirstName +' '+ b.MemLastName as MemName,a.*";
            sql += "  FROM " + objDAL.DBName + "..M_ComplaintMaster as a," + objDAL.DBName + "..M_MemberMaster as b WHERE a.IDNo=b.IDNo AND a.CID='" + CIdQS + "') as M LEFT JOIN " + objDAL.DBName + "..M_SolutionMaster as S";
            sql += "  ON M.CID=S.CID " + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                LblMemName.Text = Dt.Rows[0]["MemName"] + "[" + Dt.Rows[0]["IDNo"] + "]";
                LblCType.Text = Dt.Rows[0]["CType"].ToString();

                TxtComplaint.Text = Dt.Rows[0]["Complaint"].ToString();
                TxtPreReply.Text = "";
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    if (TxtPreReply.Text != "")
                    {
                        TxtPreReply.Text = TxtPreReply.Text + Environment.NewLine + "-----------------------------------------" + Environment.NewLine;
                    }
                    if (Dt.Rows[i]["Solution"].ToString().Trim() != "")
                    {
                        TxtPreReply.Text = TxtPreReply.Text + Dt.Rows[i]["SDate"] + ": " + Environment.NewLine + Dt.Rows[i]["Solution"];
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string DisableTheButton(Control pge, Control btn)
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

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int updateeffect;
            string StrSql = "Insert into Trnreply (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
            updateeffect = objDAL.SaveData(StrSql);

            if (updateeffect > 0)
            {
                string Sql;
                objDAL = new DAL();
                string CIdQS = Request["CId"];
                Sql = "UPDATE M_ComplaintMaster SET IsReplied='Y' WHERE CID='" + CIdQS + "'; " +
                      "Insert into M_SolutionMaster(CId,Solution) VALUES (" + CIdQS + ", N'" + objDAL.ClearInject(TxtReply.Text.Trim()) + "')";

                int UpdtEffect = objDAL.SaveData(Sql);

                if (UpdtEffect == 0)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Reply not sent.');</SCRIPT>";
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('Reply has been sent successfully.');</SCRIPT>";
                    TxtComplaint.Text = "";
                    TxtReply.Text = "";
                    // SendMail();
                }

                scrname = "<SCRIPT language='javascript'> window.top.location.reload();</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Try later.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrname, false);
                return;
            }
        }
        catch (Exception ex)
        {
            // Handle or log the exception as needed
        }
    }

    private void ClearAll()
    {
        try
        {
            LblMemName.Text = "";
            LblCType.Text = "";
            TxtComplaint.Text = "";
            TxtPreReply.Text = "";
            TxtReply.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}