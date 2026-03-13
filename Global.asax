<%@ Application Language="C#" %>

<script RunAt="server">

            void Application_Start(object sender, EventArgs e)
            {
                Application["InvDB"] = "credInv";
                Application["Connect"] = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;


            }

            void Application_End(object sender, EventArgs e)
            {
                //  Code that runs on application shutdown

            }

            void Application_Error(object sender, EventArgs e)
            {
                // Code that runs when an unhandled error occurs

            }

            void Session_Start(object sender, EventArgs e)
            {
                // Code that runs when a new session is started
                getData();
            }

            void Session_End(object sender, EventArgs e)
            {
                // Code that runs when a session ends. 
                // Note: The Session_End event is raised only when the sessionstate mode
                // is set to InProc in the Web.config file. If session mode is set to StateServer 
                // or SQLServer, the event is not raised.

            }

            public void getData()
            {
                try
                {
                    DAL objdal = new DAL();
                    System.Data.DataTable dt_Company = new System.Data.DataTable();
                    string Constr1 = string.Empty;
                    Constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString.ToString();


                    string Str_Company = string.Empty;
                    Str_Company = objdal.IsoStart + "select * from " + objdal.DBName + "..M_CompanyMaster" + objdal.IsoEnd;
                    dt_Company = SqlHelper.ExecuteDataset(Constr1, System.Data.CommandType.Text, Str_Company).Tables[0];
                    if (dt_Company.Rows.Count > 0)
                    {
                        Session["CompName"] = dt_Company.Rows[0]["CompName"].ToString();
                        Session["CompAdd"] = dt_Company.Rows[0]["CompAdd"];
                        Session["CompWeb"] = dt_Company.Rows[0]["WebSite"].ToString() == "" ? "index.asp" : dt_Company.Rows[0]["WebSite"].ToString();
                        Session["Title"] = dt_Company.Rows[0]["CompTitle"].ToString();
                        Session["CompMail"] = dt_Company.Rows[0]["CompMail"].ToString();
                        Session["CompMobile"] = dt_Company.Rows[0]["MobileNo"].ToString();
                        Session["ClientId"] = dt_Company.Rows[0]["smsSenderId"].ToString();
                        Session["SmsId"] = dt_Company.Rows[0]["smsUserNm"].ToString();
                        Session["SmsPass"] = dt_Company.Rows[0]["smPass"].ToString();
                        Session["MailPass"] = dt_Company.Rows[0]["mailPass"].ToString();
                        Session["MailHost"] = dt_Company.Rows[0]["mailHost"].ToString();
                        Session["AdminWeb"] = dt_Company.Rows[0]["AdminWeb"].ToString();
                        Session["CompCST"] = dt_Company.Rows[0]["CompCSTNo"].ToString();
                        Session["CompDate"] = dt_Company.Rows[0]["RecTimeStamp"].ToString();
                        Session["WRPartyCode"] = "SI223344";
                        Session["CompWeb1"] = "www.starindia.online";
                        Session["SmsAPI"] = "http://alotsolutions.in/API/WebSMS/Http/v1.0a/index.php?";
                        Session["LogoUrl"] = dt_Company.Rows[0]["LogoUrl"].ToString();
                    }
                    else
                    {
                        Session["CompName"] = "";
                        Session["CompAdd"] = "";
                        Session["CompWeb"] = "";
                        Session["Title"] = "Welcome";
                    }



                    string Str_ConfigMaster = string.Empty;
                    System.Data.DataTable dt_ConfigMaster = new System.Data.DataTable();
                    Str_ConfigMaster = objdal.IsoStart + "select * from " + objdal.DBName + "..M_ConfigMaster" + objdal.IsoEnd;
                    dt_ConfigMaster = SqlHelper.ExecuteDataset(Constr1, System.Data.CommandType.Text, Str_ConfigMaster).Tables[0];
                    if (dt_ConfigMaster.Rows.Count > 0)
                    {
                        Session["IsGetExtreme"] = dt_ConfigMaster.Rows[0]["IsGetExtreme"].ToString();
                        Session["IsTopUp"] = dt_ConfigMaster.Rows[0]["IsTopUp"].ToString();
                        Session["IsSendSMS"] = dt_ConfigMaster.Rows[0]["IsSendSMS"].ToString();
                        Session["IdNoPrefix"] = dt_ConfigMaster.Rows[0]["IdNoPrefix"].ToString();
                        Session["IsFreeJoin"] = dt_ConfigMaster.Rows[0]["IsFreeJoin"].ToString();
                        Session["IsStartJoin"] = dt_ConfigMaster.Rows[0]["IsStartJoin"].ToString();
                        Session["JoinStartFrm"] = dt_ConfigMaster.Rows[0]["JoinStartFrm"].ToString();
                        Session["IsSubPlan"] = dt_ConfigMaster.Rows[0]["IsSubPlan"].ToString();
                        Session["Logout"] = dt_ConfigMaster.Rows[0]["LogoutPg"].ToString();
                    }
                    else
                    {
                        Session["IsGetExtreme"] = "N";
                        Session["IsTopUp"] = "N";
                        Session["IsSendSMS"] = "N";
                        Session["IdNoPrefix"] = "";
                        Session["IsFreeJoin"] = "N";
                        Session["IsStartJoin"] = "N";
                        Session["JoinStartFrm"] = "01-Sep-2011";
                        Session["IsSubPlan"] = "N";
                        Session["Logout"] = "Default.aspx";
                    }



                    string Str_DSessID = string.Empty;
                    System.Data.DataTable dt_DSessID = new System.Data.DataTable();
                    Str_DSessID = objdal.IsoStart + "select Max(SEssid) as SessID from " + objdal.DBName + "..D_Monthlypaydetail" + objdal.IsoEnd;
                    dt_DSessID = SqlHelper.ExecuteDataset(Constr1, System.Data.CommandType.Text, Str_DSessID).Tables[0];
                    if (dt_DSessID.Rows.Count > 0)
                    {
                        Session["MaxSessn"] = dt_DSessID.Rows[0]["SessID"].ToString();
                    }
                    else
                    {
                        Session["MaxSessn"] = "";
                    }

                    Str_DSessID = objdal.IsoStart + "select Max(SEssid) as SessID from " + objdal.DBName + "..m_SessnMaster" + objdal.IsoEnd;
                    dt_DSessID = SqlHelper.ExecuteDataset(Constr1, System.Data.CommandType.Text, Str_DSessID).Tables[0];
                    if (dt_DSessID.Rows.Count > 0)
                    {
                        Session["CurrentSessn"] = dt_DSessID.Rows[0]["SessID"].ToString();
                    }

                    else
                    {
                        Session["CurrentSessn"] = "";
                    }
                

        }
        catch
        {
            Session["CompName"] = "";
            Session["CompAdd"] = "";
            Session["CompWeb"] = "";
        }
    }

</script>
