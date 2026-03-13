using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PackageActivate : System.Web.UI.Page
{
    DAL Objdal = new DAL();
    string Sql = "";
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string scrName;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["AStatus"].ToString() == "OK")
            {
                Session["PageName"] = "Member / ID Activate ";
                if (!IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringAdmin(6);
                    //FillKit();
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
    //public void FillKit(string condition = "")
    //{
    //    try
    //    {
    //        Sql = Objdal.IsoStart + " Select kitId,KitName From " + Objdal.DBName + "..M_KitMaster" +
    //              " Where  RowStatus='Y' and KitId<>4 " + condition + "  Order By kitName" + Objdal.IsoEnd;

    //        DataTable Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql).Tables[0];

    //        DDlKit.DataSource = Dt;
    //        DDlKit.DataTextField = "KitName";
    //        DDlKit.DataValueField = "KitId";
    //        DDlKit.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}

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
            //Sql = "EXEC sP_GetRepurchdataNew  '" + TxtIDNo.Text + "'";
            Sql = Objdal.IsoStart + " Select a.Formno,a.Idno,a.MemFirstName + ' ' + a.MemLastName as MemName,IsNull(c.Idno,'') as SponsorId,convert (int,D.Repurchincome ) as Repurchincome," +
                  " isnull((c.MemFirstName+' '+c.MemLastname),' ') as SponsorName,a.activeStatus,a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq " +
                  " ,a.LegNo,B.KitName,Case when a.ActiveStatus='Y' then Replace(Convert(Varchar,a.UpgradeDate,106),' ','-') else '' end as UpgradeDate," +
                  " a.FLD1,a.Planid " +
                  " from " + Objdal.DBName + "..M_KitMaster as b, "+ Objdal.DBName + "..Repurchincome AS D," + Objdal.DBName + "..M_MemberMaster as a Left Join " + Objdal.DBName + "..M_MemberMaster as c on a.RefFormno=c.Formno  " +
                  " where  A.KitId=D.KitId AND a.KitId=b.KitId and  (b.RowStatus='Y')  and a.idno='" + TxtIDNo.Text + "' and a.IsBlock='N'" + Objdal.IsoEnd;

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

                if (Dt_.Rows[0]["ActiveStatus"].ToString() == "Y" && Dt_.Rows[0]["Kitid"].ToString()=="4")
                {
                    LblCondition.Text = " and TopupSeq>='" + Dt_.Rows[0]["TopupSeq"].ToString() + "'";

                    scrName = "<SCRIPT language='javascript'>alert('This Id already activate 0 Pin Activation.');location.replace('PackageActivate.aspx');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrName, false);
                    return false;
                }
                else
                {
                    LblCondition.Text = "";
                }
                // string s = " exec sP_GetRepurchdata '" + Dt_.Rows[0]["Formno"].ToString() + "'"

                string s = Objdal.IsoStart + " select c.idno,(c.memfirstName+C.MemlastName) as MemName,b.KitName,Replace(Convert(Varchar,BillDate,106),' ','-')as UpgradeDate,convert (int,a.Repurchincome ) as Repurchincome " +
                           " from " + Objdal.DBName + "..Repurchincome as a," + Objdal.DBName + "..M_kitMaster as B ," + Objdal.DBName + "..m_membermaster as c  " +
                           " where a.formno=c.formno and a.kitid=b.kitid and b.RowsTatus='Y' and a.Formno='" + Dt_.Rows[0]["Formno"].ToString() + "' " +
                           " Union all " +
                           " select c.idno,(c.memfirstName+C.MemlastName) as MemName,b.KitName,''as UpgradeDate ,convert (int,D.Repurchincome ) as Repurchincome" +
                           " from " + Objdal.DBName + "..M_kitMaster as B ," + Objdal.DBName + "..Repurchincome as D," + Objdal.DBName + "..m_membermaster as c " +
                           " where D.kitid=b.kitid AND  D.kitid=b.kitid  AND  c.kitid=b.kitid and b.RowsTatus='Y' and c.Formno='" + Dt_.Rows[0]["Formno"].ToString() + "' and c.ActiveStatus='N' " + Objdal.IsoEnd;

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
            int updateEffect;
            string strSql = "Insert into Trnactivecadmin (Transid, Rectimestamp) values(" + HdnCheckTrnns.Value + ", getdate())";
            int updateEffect1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, strSql));
              if (updateEffect1 > 0)
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
                        if (Convert.ToInt32(txtAmount.Text) >= 49)
                        {
                            if (Convert.ToInt32(txtAmount.Text) >= 50 && Convert.ToInt32(txtAmount.Text) <= 50000000.00)
                            {
                                Sql = "Exec Sp_packageActivateid '" + TxtIDNo.Text.Trim() + "',2," + Convert.ToInt32(txtAmount.Text) + ";";
                            }

                            if (Objdal.SaveData(Sql) != 0)
                            {
                                Objdal = new DAL();
                                Sql = "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,memberId)Values" +
                                    "('" + Convert.ToInt32(Session["UserID"]) + "','" + Session["UserName"] + "','Upgrade Package ','Upgrade Package','" + Remark + "',Getdate(),'" + LblFormno.Text + "')";
                               
                                int updateEffect2 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Sql));
                            
                                    //Objdal.SaveData(Sql);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('ID Activated Successfully.!');location.replace('PackageActivate.aspx');", true);
                               
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
                                txtAmount.Text = "";
                        
                            }
                        }
                        else
                        {
                            scrname = "<SCRIPT language='javascript'>alert('The investment should be more than 50 !!');</SCRIPT>";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Error", scrname, false);
                            return;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Something Want Wrong');location.replace('PackageActivate.aspx');", true);
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
            txtAmount.Text = "";
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

    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //string str = "exec sp_checkinv '" + TxtIDNo.Text + "' ";
            //DataTable DT = new DataTable();
            //DT = SqlHelper.ExecuteDataset(constr, CommandType.Text, str).Tables[0];
            //if (DT.Rows.Count > 0)
            //{
            //    if ((Convert.ToInt32 (txtAmount.Text) < Convert.ToInt32(DT.Rows[0]["repurch"])))
            //    {
            //        scrName = "<SCRIPT language='javascript'>alert('The investment should be more than last investment!!');" + "</SCRIPT>";
            //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
            //        txtAmount.Text = "";
            //        return;
            //    }
            //}
            if (Convert.ToInt32(txtAmount.Text) <= 49)
            {
                scrName = "<SCRIPT language='javascript'>alert('The investment should be more than 50 !!');" + "</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                txtAmount.Text = "";
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}
