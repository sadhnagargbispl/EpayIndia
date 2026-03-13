using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
public partial class profile : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    DAL objDAL = new DAL();
    string strQuery;
    private string scrname;
    public string formNo;
    string strDOB;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

        btnSubmit.Attributes.Add("onclick", DisableTheButton(Page, btnSubmit));
        btnShowMemDetail.Attributes.Add("onclick", DisableTheButton(Page, btnShowMemDetail));
        try
        {
            if (Session["AStatus"] != null)
            {
                Session["PageName"] = "Member / Update Member Profile";

            }
            else
            {
                Response.Redirect("logout.aspx");
            }

            if (!Page.IsPostBack)
            {
                divDetailSection.Visible = false;
                FillCountryName();
                FillDate();
                FillBankMaster();
                FindSession();
                FillIdtypeMaster();

                if (Request["key"] != null)
                {
                    FillDetail();
                    txtMemberId.Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
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
    private void FillDetail()
    {
        string CmbType;
        try
        {
            divDetailSection.Visible = true;
            string strqry = "";
            strqry = "exec sp_MemDtl ' and mMst.IDno=''" + txtMemberId.Text + "'''";
            if (Request["key"] != null)
            {
                strqry = "exec sp_MemDtl ' and mMst.IDno=''" + Request["key"] + "'''";
                txtMemberId.Text = Request["key"];
                txtMemberId.Enabled = false;
            }
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strqry).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                lblUplinerId.Text = Dt.Rows[0]["UpLnIdNo"].ToString();
                lblUplnrNm.Text = Dt.Rows[0]["UpLnName"].ToString();
                lblRefralId.Text = Dt.Rows[0]["RefIdNo"].ToString();
                lblRefralNm.Text = Dt.Rows[0]["RefName"].ToString();

                if (Dt.Rows[0]["MemRelation"].ToString() == "")
                {
                    CmbType = "S/O";
                }
                else
                {
                    CmbType = Dt.Rows[0]["MemRelation"].ToString();
                }
                ddlCountryName.SelectedValue = Dt.Rows[0]["CountryId"].ToString();
                FillCountryMasterCode();
                ddlCountry.Text = Dt.Rows[0]["usercode"].ToString();
                MemfirstName.Text = Dt.Rows[0]["MemName"].ToString();
                txtFNm.Text = Dt.Rows[0]["MemFname"].ToString();
                txtAddLn1.Text = Dt.Rows[0]["Address1"].ToString();
                ddlDOBdt.SelectedValue = Convert.ToDateTime(Dt.Rows[0]["MemDob"]).Day.ToString();
                ddlDOBmnth.SelectedValue = Convert.ToDateTime(Dt.Rows[0]["MemDob"]).Month.ToString();
                FillCityPinDetail();

                DDLAddressProof.SelectedValue = Dt.Rows[0]["Idtype"].ToString();
                if (Dt.Rows[0]["AreaCode"] != DBNull.Value && Convert.ToInt32(Dt.Rows[0]["AreaCode"]) != 0)
                {
                    DDlArea.SelectedValue = Dt.Rows[0]["areacode"].ToString();
                }

                TxtIdProofNo.Text = Dt.Rows[0]["IdProofNo"].ToString();
                txtPhNo.Text = Dt.Rows[0]["PhN1"].ToString();
                txtMobileNo.Text = Dt.Rows[0]["Mobl"].ToString();
                TxtEmailID.Text = Dt.Rows[0]["EMail"].ToString();
                txtPanNo.Text = Dt.Rows[0]["Panno"].ToString();

                if (Dt.Rows[0]["BankId"] == DBNull.Value)
                {
                    CmbBank.SelectedValue = "0";
                }
                else
                {
                    CmbBank.SelectedValue = Dt.Rows[0]["BankId"].ToString();
                }
                if (Dt.Rows[0]["BranchName"] == DBNull.Value)
                {
                    TxtBranchName.Text = "";
                }
                else
                {
                    TxtBranchName.Text = Dt.Rows[0]["BranchName"].ToString();
                }
                if (Dt.Rows[0]["Fax"].ToString() == "CHOOSE ACCOUNT TYPE")
                {
                    DDLAccountType.SelectedValue = "0";
                }
                else
                {
                    DDLAccountType.SelectedValue = Dt.Rows[0]["Fax"].ToString();
                }
                TxtAccountNo.Text = Dt.Rows[0]["Acno"].ToString();
                TxtIfsCode.Text = Dt.Rows[0]["IFSCode"].ToString();
                lblNominee.Text = Dt.Rows[0]["NomineeName"].ToString();
                lblRelation.Text = Dt.Rows[0]["Relation"].ToString();
                TxtTransactionPassword.Text = Dt.Rows[0]["EPassw"].ToString();
                TxtPassword.Text = Dt.Rows[0]["Passw"].ToString();
                TxtCoinAddress.Text = Dt.Rows[0]["WalletAddress"].ToString();
                TxtProposerName.Text = Dt.Rows[0]["Remarks"].ToString();
                btnShowMemDetail.Visible = false;
                divDetailSection.Visible = true;
                btnSubmit.Enabled = true;
                btnSubmit.Visible = true;
                //Btncancel.Enabled = true;
            }
            else
            {
                btnShowMemDetail.Visible = true;
                divDetailSection.Visible = false;
                btnSubmit.Enabled = false;
                //Btncancel.Enabled = false;
                btnSubmit.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    private void FillCityPinDetail()
    {
        try
        {
            string sql = "";
            if (!string.IsNullOrEmpty(txtPinCode.Text) && int.Parse(txtPinCode.Text) != 0)
            {
                sql = objDAL.IsoStart + "select a.Statename,b.DistrictName,c.CityName,d.VillageName,d.Pincode,a.StateCode,b.DistrictCode";
                sql += ",c.CityCode,d.VillageCode from " + objDAL.DBName + "..M_STateDivMaster as a with( NoLock) Inner Join " + objDAL.DBName + "..M_DistrictMaster as b ";
                sql += "with( NoLock) On a.StateCode=b.StateCode and a.ActivEstatus='Y' and b.ActiveStatus='Y' ";
                sql += "Inner Join " + objDAL.DBName + ".. M_CityStatemaster as c with( NoLock)  On b.DistrictCode=c.DistrictCode and c.ActivEstatus='Y' ";
                sql += "Inner Join " + objDAL.DBName + "..M_VillageMaster as d with( NoLock) On c.CityCode=d.CityCode and d.ActiveStatus='Y'";
                sql += " where  d.Pincode='" + txtPinCode.Text + "'" + objDAL.IsoEnd;
                Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                if (Dt.Rows.Count > 0)

                {
                    ddlTehsil.Text = Dt.Rows[0]["CityName"].ToString();
                    ddlDistrict.Text = Dt.Rows[0]["DistrictName"].ToString();
                    StateCode.Value = Dt.Rows[0]["Statecode"].ToString();
                    HDistrictCode.Value = Dt.Rows[0]["Districtcode"].ToString();
                    HCityCode.Value = Dt.Rows[0]["Citycode"].ToString();
                    txtStateName.Text = Dt.Rows[0]["StateName"].ToString();

                    DataRow dr = Dt.NewRow();
                    dr["Villagecode"] = 381264;
                    dr["Villagename"] = "Others";
                    Dt.Rows.Add(dr);

                    DDlArea.DataSource = Dt;
                    DDlArea.DataValueField = "VillageCode";
                    DDlArea.DataTextField = "VillageName";
                    DDlArea.DataBind();
                    DDlArea.SelectedIndex = 0;
                }
                else
                {
                    StateCode.Value = "0";
                    HDistrictCode.Value = "0";
                    HCityCode.Value = "0";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    protected void txtPinCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string scrname = "";
            string sql = "";
            if (Convert.ToInt32(txtPinCode.Text) != 0)
            {
                sql = objDAL.IsoStart + "select a.Statename,b.DistrictName,c.CityName,d.VillageName,d.Pincode,a.StateCode,b.DistrictCode" +
                    " ,c.CityCode,d.VillageCode from " + objDAL.DBName + "..M_STateDivMaster as a with( NoLock) Inner Join " + objDAL.DBName + "..M_DistrictMaster as b " +
                    " with( NoLock) On a.StateCode=b.StateCode and a.ActivEstatus='Y' and b.ActiveStatus='Y' " +
                    " Inner Join " + objDAL.DBName + ".. M_CityStatemaster as c with( NoLock)  On b.DistrictCode=c.DistrictCode and c.ActivEstatus='Y' " +
                    " Inner Join " + objDAL.DBName + "..M_VillageMaster as d with( NoLock) On c.CityCode=d.CityCode and d.ActiveStatus='Y'" +
                    " where  d.Pincode='" + Convert.ToInt32(txtPinCode.Text) + "'" + objDAL.IsoEnd;

                Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    txtStateName.Text = Dt.Rows[0]["StateName"].ToString();
                    StateCode.Value = Dt.Rows[0]["StateCode"].ToString();
                    ddlDistrict.Text = Dt.Rows[0]["DistrictName"].ToString();
                    HDistrictCode.Value = Dt.Rows[0]["DistrictCode"].ToString();
                    ddlTehsil.Text = Dt.Rows[0]["CityName"].ToString();
                    HCityCode.Value = Dt.Rows[0]["CityCode"].ToString();
                    DataRow dr;
                    dr = Dt.NewRow();
                    dr["Villagecode"] = 381264;
                    dr["Villagename"] = "Others";
                    Dt.Rows.Add(dr);
                    DDlArea.DataSource = Dt;
                    DDlArea.DataValueField = "VillageCode";
                    DDlArea.DataTextField = "VillageName";
                    DDlArea.DataBind();
                    DDlArea.SelectedIndex = 0;
                    DDlArea.Focus();
                    if (DDlArea.SelectedValue == "381264")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Javascript", "javascript:FnVillageChange(381264); ", true);
                    }
                }
                else
                {
                    txtPinCode.Focus();
                    txtStateName.Text = "";
                    StateCode.Value = "0";
                    ddlDistrict.Text = "";
                    HCityCode.Value = "0";
                    ddlTehsil.Text = "";
                    HDistrictCode.Value = "0";
                    DDlArea.Items.Clear();

                    scrname = "<SCRIPT language='javascript'>alert(' Pincode Not exist.');" + "</SCRIPT>";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Pincode Not exist.');", true);
                }
            }
        }
        catch (Exception Ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            objDAL.WriteToFile(text + Ex.Message);
            throw new Exception(Ex.Message);
        }
    }
    private void FillDate()
    {
        try
        {
            for (int i = 1; i <= 31; i++)
            {
                ddlDOBdt.Items.Add(i.ToString());

            }
            for (int i = 1; i <= 12; i++)
            {
                ddlDOBmnth.Items.Add(i.ToString());

            }
            for (int i = 1920; i <= 2031; i++)
            {
                ddlDOBYr.Items.Add(i.ToString());

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }

    }
    private void FillBankMaster()
    {
        try
        {
            strQuery = objDAL.IsoStart + "SELECT BankCode as BankCode,BANKNAME as Bank FROM " + objDAL.DBName + "..M_BankMaster WHERE ";
            strQuery += " RowStatus='Y'  ORDER BY BANKNAME" + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                CmbBank.DataSource = Dt;
                CmbBank.DataValueField = "BankCode";
                CmbBank.DataTextField = "Bank";
                CmbBank.DataBind();
                CmbBank.SelectedIndex = 0;
            }
            else
            {
                CmbBank.DataSource = Dt;
                CmbBank.DataValueField = "BankCode";
                CmbBank.DataTextField = "Bank";
                CmbBank.DataBind();
                CmbBank.SelectedIndex = 0;
            }

        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    private void FindSession()
    {
        try
        {
            strQuery = objDAL.IsoStart + "Select Top 1 SessID,ToDate,FrmDate ";
            strQuery += "from " + objDAL.DBName + "..M_SessnMaster order by SessID desc" + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                Session["SessID"] = Dt.Rows[0]["SessID"].ToString();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    private void UpdateDb()
    {
        try
        {
            bool value = true;
            formNo = GetFormNo();
            string remark = "";
            try
            {
                DataTable Dt1 = new DataTable();
                string Name = "";
                string Password = "";
                string TransPassword = "";
                string str = objDAL.IsoStart + "select a.*,b.IdproofNo,b.Idtype from " + objDAL.DBName + "..M_MemberMaster as a,";
                str += " " + objDAL.DBName + "..KycVerify as b where a.Formno=b.Formno and a.Formno='" + formNo + "'" + objDAL.IsoEnd;
                Dt1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
                if (Dt1.Rows.Count > 0)
                {
                    strDOB = ddlDOBdt.Text + "-" + ddlDOBmnth.Text + "-" + ddlDOBYr.Text;
                    Name = Dt1.Rows[0]["MemFirstName"].ToString();
                    TransPassword = Dt1.Rows[0]["EPassw"].ToString();
                    Password = Dt1.Rows[0]["Passw"].ToString();
                    string MemName = MemfirstName.Text.Trim();
                    MemName = MemName.Trim();
                    Name = Name.Trim();
                    if (ClearInject(Name.ToUpper()) != ClearInject(MemName.ToUpper()))
                    {
                        remark = remark + "MemberName Changed From " + ClearInject(Name) + " to " + ClearInject(MemName) + ",";
                    }
                    string MemFName = txtFNm.Text.Trim();
                    MemFName = MemFName.Trim();
                    if (!string.Equals(ClearInject(Dt1.Rows[0]["MemFName"].ToString().ToUpper()), ClearInject(MemFName.ToUpper())))
                    {
                        remark = remark + " FatherName Changed From " + ClearInject(Dt1.Rows[0]["MemFName"].ToString()) + " to " + ClearInject(MemFName) + ",";
                    }
                    if (Dt1.Rows[0]["MemDob"].ToString() != strDOB)
                    {
                        remark = remark + " Dob Changed From " + ClearInject(Dt1.Rows[0]["MemDob"].ToString()) + " to " + ClearInject(strDOB) + ",";
                    }

                    if (ClearInject(Dt1.Rows[0]["Address1"].ToString().ToUpper()) != ClearInject(txtAddLn1.Text.ToUpper()))
                    {
                        remark = remark + " Address Changed From " + ClearInject(Dt1.Rows[0]["Address1"].ToString()) + " to " + ClearInject(txtAddLn1.Text) + ",";
                    }

                    if (ClearInject(Dt1.Rows[0]["PinCode"].ToString().ToUpper()) != ClearInject(txtPinCode.Text.ToUpper()))
                    {
                        remark = remark + " PinCode Changed From " + ClearInject(Dt1.Rows[0]["PinCode"].ToString()) + " to " + ClearInject(txtPinCode.Text) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["Countryid"].ToString().ToUpper()) != ClearInject(ddlCountryName.SelectedValue.ToUpper()))
                    {
                        remark = remark + " CountryName Changed From " + ClearInject(Dt1.Rows[0]["Countryid"].ToString()) + " to " + ClearInject(ddlCountryName.SelectedValue) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["usercode"].ToString()) != ClearInject(ddlCountry.Text))
                    {
                        remark = remark + " STD CODE " + ClearInject(Dt1.Rows[0]["usercode"].ToString()) + " to " + ClearInject(ddlCountry.Text) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["Statecode"].ToString().ToUpper()) != ClearInject(StateCode.Value.ToUpper()))
                    {
                        remark = remark + " Statecode Changed From " + ClearInject(Dt1.Rows[0]["Statecode"].ToString()) + " to " + ClearInject(StateCode.Value) + ",";
                    }

                    if (ClearInject(Dt1.Rows[0]["District"].ToString().ToUpper()) != ClearInject(ddlDistrict.Text.ToUpper()))
                    {
                        remark = remark + " District Changed From " + ClearInject(Dt1.Rows[0]["District"].ToString()) + " to " + ClearInject(ddlDistrict.Text) + ",";
                    }

                    if (ClearInject(Dt1.Rows[0]["City"].ToString()) != ClearInject(ddlTehsil.Text.ToUpper()))
                    {
                        remark = remark + " City Changed From " + ClearInject(Dt1.Rows[0]["City"].ToString()) + " to " + ClearInject(ddlTehsil.Text) + ",";
                    }

                    if (ClearInject(Dt1.Rows[0]["Areacode"].ToString().ToUpper()) != ClearInject(DDlArea.SelectedValue.ToUpper()))
                    {
                        remark = remark + " Areacode Changed From " + ClearInject(Dt1.Rows[0]["Areacode"].ToString()) + " to " + ClearInject(DDlArea.SelectedValue) + ",";
                    }
                    if (Convert.ToInt32(Dt1.Rows[0]["Idtype"].ToString()) != Convert.ToInt32(DDLAddressProof.SelectedValue))
                    {
                        remark = remark + " AddressProof Changed From " + ClearInject(Dt1.Rows[0]["Idtype"].ToString()) + " to " + ClearInject(DDLAddressProof.SelectedValue) + ",";
                    }

                    if (Dt1.Rows[0]["IdproofNo"].ToString().ToUpper() != TxtIdProofNo.Text.ToUpper())
                    {
                        remark = remark + " AddressProofno Changed From " + ClearInject(Dt1.Rows[0]["IdproofNo"].ToString()) + " to " + ClearInject(TxtIdProofNo.Text) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["Mobl"].ToString()) != ClearInject(txtMobileNo.Text))

                        remark = remark + " MobileNo Changed From " + ClearInject(Dt1.Rows[0]["Mobl"].ToString()) + " to " + ClearInject(txtMobileNo.Text) + ",";

                    if (ClearInject(Dt1.Rows[0]["PhN1"].ToString()) != ClearInject(txtPhNo.Text))
                    {
                        remark = remark + " PhoneNo Changed From " + ClearInject(Dt1.Rows[0]["PhN1"].ToString()) + " to " + ClearInject(txtPhNo.Text) + ",";
                    }

                    if (ClearInject(Dt1.Rows[0]["Email"].ToString()) != ClearInject(TxtEmailID.Text))
                    {
                        remark = remark + " Email Changed From " + ClearInject(Dt1.Rows[0]["Email"].ToString()) + " to " + ClearInject(TxtEmailID.Text) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["NomineeName"].ToString()) != ClearInject(lblNominee.Text))
                    {
                        remark = remark + " NomineeName Changed From " + ClearInject(Dt1.Rows[0]["NomineeName"].ToString()) + " to " + ClearInject(lblNominee.Text) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["Relation"].ToString()) != ClearInject(lblRelation.Text))
                    {
                        remark = remark + " Relation Changed From " + ClearInject(Dt1.Rows[0]["Relation"].ToString()) + " to " + ClearInject(lblRelation.Text) + ",";
                    }
                    if (Dt1.Rows[0]["Fax"].ToString().ToUpper() != ClearInject(DDLAccountType.SelectedItem.Text))
                    {
                        remark = remark + " AccountType Changed From " + ClearInject(Dt1.Rows[0]["Fax"].ToString()) + " to " + ClearInject(DDLAccountType.SelectedItem.Text) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["AcNo"].ToString()) != ClearInject(TxtAccountNo.Text))
                    {
                        remark = remark + " AccountNo Changed From " + ClearInject(Dt1.Rows[0]["AcNo"].ToString()) + " to " + ClearInject(TxtAccountNo.Text) + ",";
                    }
                    if (Convert.ToInt32(Dt1.Rows[0]["BankId"].ToString()) != Convert.ToInt32(CmbBank.SelectedValue))
                    {
                        remark = remark + " Bank Changed From " + ClearInject(Dt1.Rows[0]["BankId"].ToString()) + " to " + ClearInject(CmbBank.SelectedValue) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["BranchName"].ToString().ToUpper()) != ClearInject(TxtBranchName.Text.ToUpper()))
                    {

                        remark = remark + " BranchName Changed From " + ClearInject(Dt1.Rows[0]["BranchName"].ToString()) + " to " + ClearInject(TxtBranchName.Text) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["IFSCode"].ToString().ToUpper()) != ClearInject(TxtIfsCode.Text.ToUpper()))
                    {
                        remark = remark + " IFSCode Changed From " + ClearInject(Dt1.Rows[0]["IFSCode"].ToString()) + " to " + ClearInject(TxtIfsCode.Text) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["Passw"].ToString()) != ClearInject(TxtPassword.Text))
                    {
                        remark = remark + " Password Changed From " + ClearInject(Dt1.Rows[0]["Passw"].ToString()) + " to " + ClearInject(TxtPassword.Text) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["Panno"].ToString().ToUpper()) != ClearInject(txtPanNo.Text.ToUpper()))
                    {
                        remark = remark + " Panno Changed From " + ClearInject(Dt1.Rows[0]["Panno"].ToString()) + " to " + ClearInject(txtPanNo.Text) + ",";
                    }
                    if (ClearInject(Dt1.Rows[0]["Remarks"].ToString().ToUpper()) != ClearInject(TxtProposerName.Text.ToUpper()))
                    {
                        remark = remark + " Proposer Name Changed From " + ClearInject(Dt1.Rows[0]["Remarks"].ToString()) + " to " + ClearInject(TxtProposerName.Text) + ",";
                    }

                    if (ClearInject(Dt1.Rows[0]["EPassw"].ToString()) != ClearInject(TxtTransactionPassword.Text))
                    {
                        remark = remark + " Transaction Password  Changed From " + ClearInject(Dt1.Rows[0]["EPassw"].ToString()) + " to " + ClearInject(TxtTransactionPassword.Text) + ",";
                    }
                    DateTime formattedDateTime = DateTime.Now;
                    string userHostAddress = "YourUserHostAddress";
                    string strQryS = "exec Sp_UpdateMemberProfile_AdminNew '" + formNo + "','" + ClearInject(MemfirstName.Text.ToUpper()) + "','" + ClearInject(txtFNm.Text.ToUpper()) + "',";
                    strQryS += " '" + formattedDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "','" + ClearInject(txtPhNo.Text) + "','" + ClearInject(txtMobileNo.Text) + "','" + ClearInject(TxtEmailID.Text.Trim()) + "',";
                    strQryS += " '" + ClearInject(lblNominee.Text.ToUpper()) + "','" + ClearInject(lblRelation.Text.ToUpper()) + "','" + TxtCoinAddress.Text.Trim() + "','" + ddlCountryName.SelectedValue + "','" + ddlCountry.Text.Trim() + "','" + TxtPassword.Text.Trim() + "',";
                    strQryS += "'" + TxtTransactionPassword.Text.Trim() + "','" + TxtProposerName.Text.Trim() + "','Update Profile - " + userHostAddress + "','U';";
                    string Qry = " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values";
                    Qry += "('" + Session["UserID"] + "','" + Session["UserName"] + "','Profile','Profile Update','" + remark + "',Getdate(),'" + (formNo) + "');";
                    Qry += strQryS;
                    int a = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Qry));
                    if (a > 0)
                    {
                        if (Request["key"] != null)
                        {
                            string alertMessage = "alert('Profile Successfully Updated.!');location.replace('Profile.aspx');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", alertMessage, true);
                            txtMemberId.Enabled = true;
                        }
                        else
                        {
                            value = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Profile Successfully Updated.!');location.replace('Profile.aspx');", true);
                            FillDetail();
                            btnSubmit.Visible = false;
                            btnShowMemDetail.Visible = true;
                            return;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                value = false;
                scrname = "<SCRIPT language='javascript'>alert('" + Ex.Message + "');" + "</SCRIPT>";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", scrname, true);

                return;
            }
        }
        catch (Exception EX)
        {
            throw new Exception(EX.Message);
        }
    }
    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
            return StrObj;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (TxtAccountNo.Text.Trim() != "" || TxtIfsCode.Text.Trim() != "" || TxtBranchName.Text.Trim() != "")
            {
                // Your validation code goes here
            }

            txtMemberId.Enabled = true;

            UpdateDb();
            divDetailSection.Visible = false;
            txtMemberId.Text = "";
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
        }
    }

    protected void btnShowMemDetail_Click(object sender, EventArgs e)
    {
        try
        {
            formNo = GetFormNo();
            if (formNo == "")
            {

                divDetailSection.Visible = false;
                btnSubmit.Enabled = false;
                //Btncancel.Enabled = false;
                txtMemberId.Enabled = true;
            }
            else
            {

                txtMemberId.Enabled = false;
                divDetailSection.Visible = true;
                btnSubmit.Enabled = true;
                //Btncancel.Enabled = true;

                FillDetail();
                if (Session["IsGetExtreme"].ToString() == "N")
                {
                    tblSpnsr.Visible = false;
                }
                else
                {
                    tblSpnsr.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + Ex.Message + "')", true);
        }
    }

    private string GetFormNo()
    {
        try
        {
            objDAL = new DAL();
            string idNo;
            idNo = txtMemberId.Text;
            string qry = objDAL.IsoStart + "Select FormNo from " + objDAL.DBName + "..M_MemberMaster where IdNo='" + idNo + "'" + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, qry).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                formNo = Dt.Rows[0]["FormNo"].ToString();
                lblError.Visible = false;
            }
            else
            {
                lblError.Text = "Member Id does not exist. Please check it once and then enter it again.";
                lblError.Visible = true;
                txtMemberId.Text = "";
                formNo = "";
            }
            return formNo;
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    private void FillIdtypeMaster()
    {
        try
        {
            string strQuery = objDAL.IsoStart + "SELECT Id,IdType  FROM " + objDAL.DBName + "..M_IdTypeMaster WHERE ACTIVESTATUS='Y' " + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
            DDLAddressProof.DataSource = Dt;
            DDLAddressProof.DataValueField = "Id";
            DDLAddressProof.DataTextField = "IdType";
            DDLAddressProof.DataBind();
            DDLAddressProof.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            objDAL.WriteToFile(text + Ex.Message);
            throw new Exception(Ex.Message);
        }
    }
    private void FillCountryName()
    {
        try
        {
            string strQuery = objDAL.IsoStart + "SELECT CId,CountryName FROM " + objDAL.DBName + "..M_CountryMaster WHERE ACTIVESTATUS='Y' ORDER BY CountryNAme " + objDAL.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
            ddlCountryName.DataSource = Dt;
            ddlCountryName.DataValueField = "CId";
            ddlCountryName.DataTextField = "CountryName";
            ddlCountryName.DataBind();
            ddlCountryName.SelectedIndex = 198;
        }
        catch (Exception Ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            objDAL.WriteToFile(text + Ex.Message);
            throw new Exception(Ex.Message);
        }
    }
   
    protected void ddlCountryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillCountryMasterCode();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FillCountryMasterCode()
    {
        try
        {
            //DataTable dt = new DataTable();
            DataTable dt = new DataTable();
            strQuery = "SELECT StdCode FROM " + objDAL.DBName + "..M_CountryMaster WHERE ACTIVESTATUS='Y' AND Cid = '" + ddlCountryName.SelectedValue + "'";
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ddlCountry.Text = dt.Rows[0]["StdCode"].ToString();
            }
        }
        // ddlMobileNAme.DataSource = dt
        // ddlMobileNAme.DataValueField = "StdCode"
        // ddlMobileNAme.DataBind()

        catch (Exception ex)
        {
        }
    }
}
