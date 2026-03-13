using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IdActivate : System.Web.UI.Page
{
    DAL Objdal = new DAL();
    string Sql = "";
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string scrName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (Session["AStatus"].ToString() == "OK")
            if (Session["AStatus"] != null && (string)Session["AStatus"] == "OK")
            {
                Session["PageName"] = "Member / ID Activate ";
                if (!IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringAdmin(6);
                    FillKit();
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    public void FillKit(string condition = "")
    {
        try
        {
            Sql = Objdal.IsoStart + " Select kitId,KitName From " + Objdal.DBName + "..M_KitMaster" +
                  " Where  RowStatus='Y' and KitId<>1 " + condition + "  Order By Kitid " + Objdal.IsoEnd;

            DataTable Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql).Tables[0];

            DDlKit.DataSource = Dt;
            DDlKit.DataTextField = "KitName";
            DDlKit.DataValueField = "KitId";
            DDlKit.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public string GenerateRandomStringAdmin( int iLength)
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
    private bool Check_IdNo()
    {
        try
        {
            DataTable dt1 = new DataTable();

            Sql = Objdal.IsoStart + " Select a.Formno,a.Idno,a.MemFirstName + ' ' + a.MemLastName as MemName,IsNull(c.Idno,'') as SponsorId," +
                  " isnull((c.MemFirstName+' '+c.MemLastname),' ') as SponsorName,a.activeStatus,a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq " +
                  " ,a.LegNo,B.KitName,Case when a.ActiveStatus='Y' then Replace(Convert(Varchar,a.UpgradeDate,106),' ','-') else '' end as UpgradeDate," +
                  " a.FLD1,a.Planid " +
                  " from " + Objdal.DBName + "..M_KitMaster as b," + Objdal.DBName + "..M_MemberMaster as a Left Join " + Objdal.DBName + "..M_MemberMaster as c on a.RefFormno=c.Formno  " +
                  " where a.KitId=b.KitId and  (b.RowStatus='Y')  and a.idno='" + TxtIDNo.Text + "' and a.IsBlock='N'" + Objdal.IsoEnd;

            DataTable Dt_ = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql).Tables[0];

            if (Dt_.Rows.Count == 0)
            {
                lblError.Text = " Please enter correct Member ID.";
                lblError.ForeColor = System.Drawing.Color.Red;
                TxtIDNo.Text = "";
                LblMemName.Text = "";
                LblKitName.Text = "";
                LblNewKitid.Text = "";
                GrdDirects1.Visible = false;
                BtnUpgrade.Enabled = false;
                return false;
            }
            else
            {
                LblKitId.Text = Dt_.Rows[0]["KitId"].ToString();
                lblError.Text = "";
                LblMemName.Text = Dt_.Rows[0]["MemName"].ToString();
                LblFormno.Text = Dt_.Rows[0]["Formno"].ToString();
                LblKitName.Text = Dt_.Rows[0]["KitName"].ToString();

                if (Dt_.Rows[0]["ActiveStatus"].ToString() == "Y")
                {
                    LblCondition.Text = " and TopupSeq>='" + Dt_.Rows[0]["TopupSeq"].ToString() + "'";

                    scrName = "<SCRIPT language='javascript'>alert('This Id already activate.');location.replace('IdActivate.aspx');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
                    return false;
                }
                else
                {
                    LblCondition.Text = "";
                }

                string s = Objdal.IsoStart + " select c.idno,(c.memfirstName+C.MemlastName) as MemName,b.KitName,Replace(Convert(Varchar,BillDate,106),' ','-')as UpgradeDate " +
                           " from " + Objdal.DBName + "..Repurchincome as a," + Objdal.DBName + "..M_kitMaster as B ," + Objdal.DBName + "..m_membermaster as c  " +
                           " where a.formno=c.formno and a.kitid=b.kitid and b.RowsTatus='Y' and a.Formno='" + Dt_.Rows[0]["Formno"].ToString() + "' " +
                           " Union all " +
                           " select c.idno,(c.memfirstName+C.MemlastName) as MemName,b.KitName,''as UpgradeDate " +
                           " from " + Objdal.DBName + "..M_kitMaster as B ," + Objdal.DBName + "..m_membermaster as c " +
                           " where c.kitid=b.kitid and b.RowsTatus='Y' and c.Formno='" + Dt_.Rows[0]["Formno"].ToString() + "' and c.ActiveStatus='N' " + Objdal.IsoEnd;

                dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, s).Tables[0];

                if (dt1.Rows.Count > 0)
                {
                    GrdDirects1.DataSource = dt1;
                    GrdDirects1.DataBind();
                    GrdDirects1.Visible = true;
                }
                else
                {
                    GrdDirects1.Visible = false;
                }

                LblMemName.ForeColor = System.Drawing.Color.Black;
                BtnUpgrade.Enabled = true;

                return true;
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
            return false;
        }
    }
    protected void BtnUpgrade_Click(object sender, EventArgs e)
    {
        try
        {
            int updateeffect;
            string StrSql = "Insert into Trnactivecadmin (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
            updateeffect = Objdal.SaveData(StrSql);

            if (updateeffect > 0)
            {
                string scrname;
                string Remark = " Package Upgrade of Idno:" + TxtIDNo.Text + "";

                try
                {
                    lblError.Text = "";

                    if (string.IsNullOrWhiteSpace(TxtIDNo.Text))
                    {
                        lblError.Text = "Enter Member ID.";
                        return;
                    }
                    else
                    {
                        if (!Check_IdNo())
                        {
                            lblError.Text = "Invalid Member ID.";
                            return;
                        }

                        if (DDlKit.SelectedValue == "0")
                        {
                            scrname = "<SCRIPT language='javascript'>alert('Choose Package Name');</SCRIPT>";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Error", scrname, false);
                            return;
                        }

                        Sql = "Exec Sp_ActivateidNew '" + TxtIDNo.Text.Trim() + "'," + Convert.ToInt32(DDlKit.SelectedValue) + ";";
                        if (Objdal.SaveData(Sql) != 0)
                        {
                            Objdal = new DAL();
                            Sql = "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,memberId)Values" +
                                "('" + Convert.ToInt32(Session["UserID"]) + "','" + Session["UserName"] + "','Upgrade Package ','Upgrade Package','" + Remark + "',Getdate(),'" + LblFormno.Text + "')";

                            Objdal.SaveData(Sql);

                            scrname = "<SCRIPT language='javascript'>alert('ID Activated Successfully !! ');</SCRIPT>";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Upgraded", scrname, false);
                            lblError.Text = " ID Activated successfully.";

                            TxtIDNo.Text = "";
                            LblMemName.Text = "";
                            lblError.Text = "";
                            BtnUpgrade.Enabled = false;
                            GrdDirects1.Visible = false;
                            LblFormno.Text = "";
                            LblKitId.Text = "";
                            LblKitName.Text = "";
                            LblNewKitid.Text = "";
                            FillKit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    scrname = "<SCRIPT language='javascript'>alert('" + ex.Message + "');</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Error", scrname, false);
                }
            }
            else
            {
                Response.Redirect("IdActivate.aspx");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            TxtIDNo.Text = "";
            LblMemName.Text = "";
            lblError.Text = "";
            BtnUpgrade.Enabled = false;
            GrdDirects1.Visible = false;
            LblFormno.Text = "";
            LblKitId.Text = "";
            LblKitName.Text = "";
            LblNewKitid.Text = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }


    protected void TxtIDNo_TextChanged(object sender, EventArgs e)
    {
        Check_IdNo();
    }
}
