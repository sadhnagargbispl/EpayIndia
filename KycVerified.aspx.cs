using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class KycVerified : System.Web.UI.Page
{
    DataTable dtData = new DataTable();
    DAL objDAL;
    //clsGeneral objGen = new clsGeneral();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            objDAL = new DAL();

            if (!Page.IsPostBack)
            {
                if (Session["AStatus"] != null && Session["AStatus"].ToString() == "OK")
                {
                    if (Request.QueryString.HasKeys())
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["key"]))
                        {
                            txtMemId.Text = Request.QueryString["key"];
                            //ChkMem.Checked = true;
                            BindData(" AND IDNo='" + Request.QueryString["key"] + "'");
                        }
                    }
                    else
                    {
                        BindData();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception here
        }

    }
    protected void BtnUnVerify_Click(object sender, EventArgs e)
    {
        try
        {
            if (TxtARemark.Text == "")
            {
                LblARemark.Visible = true;
                LblARemark.Text = "Please Enter Remark";
                return;
            }
            LblARemark.Visible = false;

            int Rejectcnt = 0;
            string str = "";
            string scrname;
            string Condition = "";
            Label lbl;
            CheckBox Chk;
            string Remark = "";
            Label LblIdno;

            if (TxtARemark.Text == "")
            {
                scrname = "<SCRIPT language='javascript'>alert('Please Enter Remarks ');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }

            foreach (GridViewRow Gvr in GvData.Rows)
            {
                Chk = (CheckBox)Gvr.FindControl("chkSelect");
                lbl = (Label)Gvr.FindControl("LblGrpID");
                LblIdno = (Label)Gvr.FindControl("LblIdno");

                if (Chk.Checked)
                {
                    string strsql = "";
                    DataTable dtcheck = new DataTable();
                    strsql = "select Count(formno) as cnt from KycVerify where IsBankVerified='R' and Formno='" + Convert.ToInt32(lbl.Text) + "'";
                    dtcheck = SqlHelper.ExecuteDataset(constr, CommandType.Text, strsql).Tables[0];

                    if (Convert.ToInt32(dtcheck.Rows[0]["cnt"]) == 0)
                    {
                        Remark = "KYC UnVerify of IdNo:" + LblIdno.Text;
                        str += "; Update KycVerify Set IsAddrssverified='R', AddrssVerifyDate=GETDATE(),AddrssUserid='" + Convert.ToInt32(Session["Userid"]) +
                                "', AddrssRemark='" + TxtARemark.Text + "',AddressRejectId='" + DDlREason.SelectedValue +
                                "', IsBankVerified='R' ,BankverifyDate = GETDATE(),BankUserid='" + Convert.ToInt32(Session["Userid"]) +
                                "',BankProofRemark='" + TxtARemark.Text + "',BankRejectId='" + DDlREason.SelectedValue +
                                "', IsPanVerified='R',PanVerifyDate=GETDATE(),PanUserid='" + Convert.ToInt32(Session["Userid"]) +
                                "',PanRemarks='" + TxtARemark.Text + "',PanRejectId='" + DDlREason.SelectedValue +
                                "' where IsAddrssverified<>'R'  and formno='" + lbl.Text + "';";

                        str += "insert into KycHistory(formno,idno,Memname,Type,ImgPath,ImgPath1,Userid,RectimeStamp,Status,Remark,RejectId) " +
                                "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'A',b.AddrProof,b.BackAddressProof,'" + Convert.ToInt32(Session["Userid"]) +
                                "',GETDATE(),2,'" + TxtARemark.Text.Trim() + "','" + DDlREason.SelectedValue + "' from M_MemberMaster as a,KycVerify as b " +
                                "where a.Formno=b.Formno and a.Formno='" + Convert.ToInt32(lbl.Text) + "';";

                        str += "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status,Remark,RejectId) " +
                                "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'P',b.PanImg,'" + Convert.ToInt32(Session["Userid"]) +
                                "',GETDATE(),2,'" + TxtARemark.Text.Trim() + "','" + DDlREason.SelectedValue + "' from M_MemberMaster as a,KycVerify as b " +
                                "where a.Formno=b.Formno and a.Formno='" + Convert.ToInt32(lbl.Text) + "';";

                        str += "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status,Remark,RejectId) " +
                                "select a.formno,a.Idno,a.MemfirstName+''+a.MemlastName,'B',b.BankProof,'" + Convert.ToInt32(Session["Userid"]) +
                                "',GETDATE(),2,'" + TxtARemark.Text.Trim() + "','" + DDlREason.SelectedValue + "' from M_MemberMaster as a,KycVerify as b " +
                                "where a.Formno=b.Formno and a.Formno='" + Convert.ToInt32(lbl.Text) + "';";

                        str += "Insert Into TempKycVerify Select *,GETDATE(),'" + Convert.ToInt32(Session["Userid"]) + "' From KycVerify Where FormNo='" + Convert.ToInt32(lbl.Text) + "'";

                        str += "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values " +
                                "('" + Convert.ToInt32(Session["UserID"]) + "','" + Session["UserName"] + "','KYC Verify ','KYC UnVerify','" + Remark +
                                "',GETDATE(),'" + lbl.Text + "')";

                        Rejectcnt++;
                    }
                }
            }

            int i = 0;
            if (!string.IsNullOrEmpty(str))
            {
                i = objDAL.UpdateData(str);
            }

            if (i != 0)
            {
                scrname = "<SCRIPT language='javascript'>alert('" + Rejectcnt + " Rejected successfully. ');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Image", scrname, false);
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('" + Rejectcnt + " UnVerified unsuccessfully. ');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Image", scrname, false);
            }

            DivRemark.Visible = false;
            TxtARemark.Text = "";
            BindData();
        }
        catch (Exception ex)
        {
            // Handle exception
        }

    }
    protected void BtnVerifiy_Click(object sender, EventArgs e)
    {
        try
        {
            int approovecnt = 0;
            string str = "";
            string scrname;
            string condition = "";
            Label lbl;
            Label lblIdNo;
            CheckBox chk;
            string remark = "";

            foreach (GridViewRow gvr in GvData.Rows)
            {
                chk = (CheckBox)gvr.FindControl("chkSelect");
                lbl = (Label)gvr.FindControl("LblGrpID");
                lblIdNo = (Label)gvr.FindControl("LblIdno");

                if (chk.Checked)
                {
                    string strsql = "";
                    DataTable dtcheck = new DataTable();
                    strsql = "select Count(formno) as cnt from KycVerify where IsBankVerified in ('Y','R') and Formno='" + Convert.ToInt32(lbl.Text) + "'";
                    dtcheck = SqlHelper.ExecuteDataset(constr, CommandType.Text, strsql).Tables[0];
                    if (Convert.ToInt32(dtcheck.Rows[0]["cnt"]) == 0)
                    {
                        remark = "Address Proof Verify of IdNo:" + lblIdNo.Text;
                        str += "Exec Sp_ApproveKYCDetails '" + Convert.ToInt32(lbl.Text) + "','" + Convert.ToInt32(Session["Userid"]) + "',";
                        str += "'" + Session["UserName"] + "','" + remark + "'";

                        //str += "; Update KycVerify Set IsAddrssverified='Y',AddrssVerifyDate=getdate(),AddrssUserId='" + Convert.ToInt32(Session["Userid"]) + "',IsPanVerified='Y',PanVerifyDate=getdate(),PanUserid='" + Convert.ToInt32(Session["Userid"]) + "' " +
                        //    ",IsBankVerified='Y',BankverifyDate = getdate(),BankUserId='" + Convert.ToInt32(Session["UserId"]) + "' where IsidVerified<>'Y' AND IsPanVerified<>'Y' AND formno='" + lbl.Text + "';";
                        //str += "Insert Into TempKycVerify Select *,GetDate(),'" + Convert.ToInt32(Session["Userid"]) + "' From KycVerify Where FormNo='" + Convert.ToInt32(lbl.Text) + "';";
                        //str += "insert into KycHistory(formno,idno,Memname,Type,ImgPath,ImgPath1,Userid,RectimeStamp,Status)" +
                        //    "select a.formno,a.Idno,a.MemfirstName + ' ' + a.MemlastName,'A',b.AddrProof,b.BackAddressProof,'" + Convert.ToInt32(Session["Userid"]) + "',Getdate(),1 from M_MemberMaster as a inner join KycVerify as b" +
                        //    " on a.Formno=b.Formno where a.Formno='" + Convert.ToInt32(lbl.Text) + "';";
                        //str += "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status)" +
                        //    "select a.formno,a.Idno,a.MemfirstName + ' ' + a.MemlastName,'P',b.PanImg,'" + Convert.ToInt32(Session["Userid"]) + "',Getdate(),1 from M_MemberMaster as a inner join KycVerify as b" +
                        //    " on a.Formno=b.Formno where a.Formno='" + Convert.ToInt32(lbl.Text) + "';";
                        //str += "insert into KycHistory(formno,idno,Memname,Type,ImgPath,Userid,RectimeStamp,Status)" +
                        //    "select a.formno,a.Idno,a.MemfirstName + ' ' + a.MemlastName,'B',b.BankProof,'" + Convert.ToInt32(Session["Userid"]) + "',Getdate(),1 from M_MemberMaster as a inner join KycVerify as b" +
                        //    " on a.Formno=b.Formno where a.Formno='" + Convert.ToInt32(lbl.Text) + "';";
                        //str += "; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId) Values" +
                        //    "('" + Convert.ToInt32(Session["UserID"]) + "','" + Session["UserName"] + "','KYC Verify ','Address Proof Verify','" + remark + "',Getdate(),'" + lbl.Text + "')";

                        approovecnt++;
                    }
                }
            }

            int i = 0;
            if (!string.IsNullOrEmpty(str))
            {
                i = objDAL.UpdateData(str);
            }
            if (i != 0)
            {
                scrname = "<script language='javascript'>alert('" + approovecnt + " Verified successfully. ');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Image", scrname, false);
            }
            else
            {
                scrname = "<script language='javascript'>alert('" + approovecnt + " Verified successfully. ');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Image", scrname, false);
            }

            BindData();
        }
        catch (Exception ex)
        {
            // Handle the exception or log it
        }

    }
    protected void BtnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtTemp = new DataTable();
            DataGrid dg = new DataGrid();
            string condition = "";

            // Build the condition string based on checkboxes and dropdowns

            if (!string.IsNullOrEmpty(txtMemId.Text))
            {
                condition += " AND IDNo='" + txtMemId.Text.Trim() + "'";
            }

            if (DDlVerify.SelectedValue != "S")
            {
                condition += " And c.IsAddrssverified='" + DDlVerify.SelectedValue + "' ";
            }

            if (RbtSearch.SelectedValue != "A")
            {
                condition += " And a.ActiveStatus='" + RbtSearch.SelectedValue + "' ";
            }

            string sql = "";

            // SQL query construction based on the conditions
            sql = "select a.IDNo, RTRIM(a.MemFirstName + ' ' + a.MemLastName) as MemberName, " +
                  "Replace(CONVERT(varchar, a.Doj, 106), ' ', '-') as Doj, " +
                  "Case when a.Activestatus='Y' then Replace(CONVERT(varchar, a.UpgradeDate, 106), ' ', '-') else '' End as ActivationDate, " +
                  "a.Address1, a.City, a.Tehsil, a.District, b.Statename, " +
                  "a.Pincode, d.IdType, '''' + Convert(Varchar, c.IdProofNo) As IdProofNo, g.Bankname, '''' + Convert(Varchar, a.AcNo) As Acno, " +
                  "a.Branchname, a.Ifscode, a.Fax as AccountType, a.panno, " +
                  "CASE WHEN c.IsAddrssverified='Y' THEN 'Verified' when c.IsAddrssverified='R' then 'Rejected' Else 'Verification Due' END AS Verification, " +
                  "Case when c.IsAddrssVerified='Y' then Replace(CONVERT(varchar, c.AddrssVerifyDate, 106), ' ', '-') else '' end as VerifyDate, " +
                  "Isnull(e.Username, '') VerifyBy, " +
                  "Case when c.isAddrssVerified='R' then c.AddrssRemark else '' end as RejectRemark, " +
                  "Isnull(f.Reason, '') As RejectReason " +
                  "From M_MemberMaster as a, M_StatedivMaster as b, M_BAnkMaster as g, KycVerify as c " +
                  "Left Join M_KycReject as f On c.AddressRejectId=f.Kid " +
                  "Left Join M_UserMaster as e On c.AddrssUserId=e.Userid and e.Rowstatus='Y', M_IdTypeMaster as d " +
                  "where c.IdType=d.Id and d.ActiveStatus='Y' and a.Formno=c.Formno and " +
                  "a.statecode=b.statecode and b.Rowstatus='Y' and a.bankid=g.bankcode and g.rowstatus='Y' " +
                  condition + " and (c.BackAddressProof<>'' Or c.AddrProof<>'') Order by ActivationDate DESC";

            // Fetch data using objDAL
            dtTemp = objDAL.GetData(sql);

            // Bind data to DataGrid
            dg.DataSource = dtTemp;
            dg.DataBind();

            // Export data to Excel
            ExportToExcel("KYCDetail.xls", dg);
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Response.Write(ex.Message + " Error In Exporting File");
        }
    }
    private void ExportToExcel(string fileName, DataGrid dg)
    {
        using (var sw = new System.IO.StringWriter())
        {
            using (var htw = new HtmlTextWriter(sw))
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel"; // Correct MIME type for Excel
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Charset = "";

                dg.EnableViewState = false;
                dg.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
            }
        }
    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
    public void BindData(string Condition = "")
    {
        try
        {
            // If ChkMem is checked, add condition based on txtMemId

            if (!string.IsNullOrEmpty(txtMemId.Text))
            {
                Condition += " AND IDNo='" + txtMemId.Text.Trim() + "'";
            }


            // Check the value of DDlVerify and add the appropriate condition
            if (DDlVerify.SelectedValue != "S")
            {
                Condition += " And c.IsAddrssverified='" + DDlVerify.SelectedValue + "' ";
            }

            // Check the value of RbtSearch and add the appropriate condition
            if (RbtSearch.SelectedValue != "A")
            {
                Condition += " And a.ActiveStatus='" + RbtSearch.SelectedValue + "' ";
            }

            string sql = "";
            sql = "select Cast(a.FormNo as varchar) as FormNo, Replace(CONVERT(varchar, a.Doj, 106), ' ', '-') as Doj, " +
                  "Case when a.Activestatus='Y' then Replace(CONVERT(varchar, a.UpgradeDate, 106), ' ', '-') else '' End as ActivationDate, " +
                  "a.IDNo, RTRIM(a.MemFirstName + ' ' + a.MemLastName) as MemName, a.Address1, a.City, a.Tehsil, a.District, " +
                  "b.Statename, g.Bankname, a.Acno, a.Branchname, a.Ifscode, a.Pincode, a.Fax, a.panno, " +
                  "CASE WHEN c.BackAddressProof<>'' then c.BackAddressProof else '" + Session["CompWeb"] + "/Images/no_photo.jpg' end as BackAdressProof, " +
                  "Case when c.BackAddressProof<>'' then Replace(convert(varchar, BackAddressDate, 106), ' ', '-') else '' end as BackAddressDate, " +
                  "CASE WHEN c.IsAddrssverified='Y' THEN 'Verified' when c.IsAddrssverified='R' then 'Rejected' Else 'Verification Due' END AS AddrssVerf, " +
                  "case when c.AddrProof<>'' then Replace(CONVERT(varchar, c.AddrProofDate, 106), ' ', '-') else '' end as AddressProofDate, d.IdType, c.IdProofNo, " +
                  "CASE WHEN c.AddrProof <>'' Then c.AddrProof ELSE '" + Session["CompWeb"] + "/Images/no_photo.jpg' END AddressproofStatus, " +
                  "Case when c.IsAddrssverified='Y' then 'False' else 'True' end as EnableStatus, " +
                  "Case when c.IsAddrssVerified='Y' then Replace(CONVERT(varchar, c.AddrssVerifyDate, 106), ' ', '-') else '' end as AddressVerifyDate, " +
                  "Isnull(e.Username, '') VerifyBy, " +
                  "Case when c.isAddrssVerified='R' then c.AddrssRemark else '' end as RejectRemark, " +
                  "Isnull(f.Reason, '') As RejectReason, " +
                  "case when c.BankProof<>'' then Replace(CONVERT(varchar, c.BankProofDate, 106), ' ', '-') else '' end as BankProofDate, " +
                  "CASE WHEN c.BankProof <>'' Then c.BankProof ELSE '" + Session["CompWeb"] + "/Images/no_photo.jpg' END AS BankProofStatus, " +
                  "case when c.PanImg<>'' then Replace(CONVERT(varchar, c.PANImgDate, 106), ' ', '-') else '' end as PanProofDate, " +
                  "CASE WHEN c.PanImg <>'' Then c.PanImg ELSE '' END PanproofStatus, " +
                  "Case when c.IsPanverified='N' then '' else Replace(CONVERT(varchar, c.PanVerifyDate, 106), ' ', '-') end as PanVerifyDate " +
                  "From M_MemberMaster as a, M_StatedivMaster as b, M_BAnkMaster as g, KycVerify as c " +
                  "Left Join M_KycReject as f On c.AddressRejectId=f.Kid " +
                  "Left Join M_UserMaster as e On c.AddrssUserId=e.Userid and e.Rowstatus='Y', M_IdTypeMaster as d " +
                  "where c.IdType=d.Id and d.ActiveStatus='Y' and a.bankid=g.bankcode and g.rowstatus='Y' " +
                  "and a.Formno=c.Formno and a.statecode=b.statecode and b.Rowstatus='Y' " +
                  "and (c.BackAddressProof<>'' Or c.AddrProof <>'') " +
                  Condition + " Order by AddressProofdate DESC";

            // Execute the SQL query and bind the data to the grid view
            dtData = new DataTable();
            dtData = objDAL.GetData(sql);
            GvData.DataSource = dtData;
            GvData.DataBind();
            Session["GData"] = dtData;

            // Enable or disable buttons based on data availability
            if (dtData.Rows.Count > 0)
            {
                BtnVerifiy.Enabled = true;
                BTnUnVerification.Enabled = true;
                BtnExport.Enabled = true;
            }
            else
            {
                BtnVerifiy.Enabled = false;
                BTnUnVerification.Enabled = false;
                BtnExport.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (optional: add logging or other error handling here)
        }
    }
    protected void GvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GvData.PageIndex = e.NewPageIndex;
            GvData.DataSource = Session["GData"];
            GvData.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void GvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkSelectAll = (CheckBox)e.Row.FindControl("ChkSelectAll");
            if (chkSelectAll != null)
            {
                chkSelectAll.Attributes.Add("onclick", "javascript:SelectAll('" + chkSelectAll.ClientID + "')");
            }
        }
    }

    protected void BTnUnVerification_Click(object sender, EventArgs e)
    {
        string scrname = "";
        DivRemark.Visible = true;
        BtnVerifiy.Enabled = false;
        BTnUnVerification.Enabled = false;
        FillDetail();

    }
    protected void FillDetail()
    {
        try
        {
            string s = "";
            s = "Select * from M_KycReject where activeStatus='Y'";
            objDAL = new DAL();

            DataTable Dt = new DataTable();
            Dt = objDAL.GetData(s);

            if (Dt.Rows.Count > 0)
            {
                DDlREason.DataValueField = "kId";
                DDlREason.DataTextField = "reason";
                DDlREason.DataSource = Dt;
                DDlREason.DataBind();
            }
        }
        catch (Exception ex)
        {
            // Handle exception
        }
    }

}
