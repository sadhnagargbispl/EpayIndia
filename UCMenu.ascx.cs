using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Claims;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UCMenu : System.Web.UI.UserControl
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    DAL obj = new DAL();
    DataTable dt = new DataTable();
    DataSet ds = new DataSet();
    int TotalVerify, Addressverify, BankVerify, PanVerify;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Load_Menu();
            }
        }
        catch (Exception Ex)
        {
            Response.Write(Ex.Message + "SideB");
        }
    }
    private void Load_Menu()
    {
        try
        {
            string icon = "";
            string notification = "";
            string html = "";
            string userid = Session["UserID"].ToString();
            DataTable dt_Menu = new DataTable();
            DataTable dt_Compcount = new DataTable();

            string sql = obj.IsoStart + " Exec Sp_GetParentMenu " + userid + " " + obj.IsoEnd;
            dt_Menu = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];

            string s = obj.IsoStart + " select count(a.FormNo)as BankVerify from " + obj.DBName + "..KycVerify as a," +
          " " + obj.DBName + "..M_MemberMaster as b where a.Formno=b.formno  and a.IsBankverified='N'  " +
          "and Ltrim(a.BankProof)<>''" + obj.IsoEnd;

            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, s).Tables[0];

            if (dt.Rows.Count > 0)
            {
                BankVerify = Convert.ToInt32(dt.Rows[0]["BankVerify"]);
            }
            if (dt_Menu.Rows.Count > 0)
            {


                foreach (DataRow dr in dt_Menu.Rows)
                {
                    string parentId = dr["ParentId"].ToString();
                    if (string.Equals(dr["MenuName"], "-") == false)
                    {
                        if (Convert.ToInt32(parentId) == 0)
                        {
                            // html = html & "<li><a href=""#"">" & dr("MenuName") & "</a></li>"
                            string MainMenu = dr["MenuName"].ToString();
                            string SubMenu = Load_SubMenu(dr["MenuId"].ToString(), dt_Menu);
                            if (dr["OnSelect"].ToString() == "")
                            {
                                if (dr["MenuName"].ToString() == "User")
                                {
                                    icon = "fa fa-user";
                                    notification = "";
                                }
                                else if (dr["MenuName"].ToString() == "Wallet")
                                {
                                    icon = "fa  fa-folder-open-o";
                                    notification = "";
                                }
                                else if (dr["MenuName"].ToString() == "Master")
                                {
                                    icon = "fa fa-edit";
                                    notification = "";
                                }
                                else if (dr["MenuName"].ToString() == "Report")
                                {
                                    icon = "fa fa-list-alt";
                                    notification = "";
                                }
                                else if (dr["Menuname"].ToString() == "Member")
                                {
                                    icon = "fa fa-sitemap";
                                    notification = "";
                                }
                                else if (dr["MenuName"].ToString() == "Home")
                                {
                                    icon = "fa fa-table";
                                    notification = "";
                                }
                                else if (dr["menuName"].ToString() == "LogOut")
                                {
                                    icon = "fa fa-signout";
                                    notification = "";
                                }
                                else if (dr["MenuName"].ToString() == "KYC Verify")
                                {
                                    // If TotalVerify > 0 Then
                                    notification = "<span class=\"badge badge-info right\" style=\"margin-left: 64px; padding: 3px !important;\">" + BankVerify + "</span>";
                                }
                                else if (dr["MenuName"].ToString() == "Complaint")
                                {
                                    string s1 = obj.IsoStart + " Exec Sp_GetComplainCountMenu " + obj.IsoEnd;
                                    dt_Compcount = SqlHelper.ExecuteDataset(constr1, CommandType.Text, s1).Tables[0];
                                    notification = "<span class=\"badge badge-info right\" style=\"margin-left: 64px; padding: 3px !important;\">" + dt_Compcount.Rows[0]["Complaint"] + "</span>";
                                }

                                MainMenu = " <li class=\"nav-item\">";
                                MainMenu += " <a href=\"#\" class=\"nav-link\">";
                                MainMenu += "  <i class=\"nav-icon fas fa-tree\"></i>";
                                MainMenu += " <p>";
                                MainMenu += " " + dr["MenuName"].ToString() + "";
                                MainMenu += " <i class=\"right fas fa-angle-left\"></i>";
                                MainMenu += "" + notification + "";
                                MainMenu += " </p>";
                                MainMenu += " </a>";
                                MainMenu += " <ul class=\"nav nav-treeview\">";
                                MainMenu += "" + SubMenu + "";
                                MainMenu += "</ul>";
                                MainMenu += "</li>";
                            }
                            else
                            {
                                if (dr["MenuName"].ToString() == "User")
                                {
                                    icon = "fa fa-user";
                                    MainMenu = "<li><a href=\"" + dr["OnSelect"].ToString() + "\"><i class=\"" + icon + "\"></i>" + dr["MenuName"].ToString() + "</a> </li> ";
                                }
                                else if (dr["MenuName"].ToString() == "Wallet")
                                {
                                    icon = "fa  fa-folder-open-o";
                                    MainMenu = "<li><a href=\"" + dr["OnSelect"].ToString() + "\"><i class=\"" + icon + "\"></i>" + dr["MenuName"].ToString() + "</a> </li> ";
                                }
                                else if (dr["MenuName"].ToString() == "Master")
                                {
                                    icon = "fa fa-edit";
                                    MainMenu = "<li><a href=\"" + dr["OnSelect"].ToString() + "\"><i class=\"" + icon + "\"></i>" + dr["MenuName"].ToString() + "</a> </li> ";
                                }
                                else if (dr["MenuName"].ToString() == "Report")
                                {
                                    icon = "fa fa-list-alt";
                                    MainMenu = "<li><a href=\"" + dr["OnSelect"].ToString() + "\"><i class=\"" + icon + "\"></i>" + dr["MenuName"].ToString() + "</a> </li> ";
                                }
                                else if (dr["Menuname"].ToString() == "Member")
                                {
                                    icon = "fa fa-sitemap";
                                    MainMenu = "<li><a href=\"" + dr["OnSelect"].ToString() + "\"><i class=\"" + icon + "\"></i>" + dr["MenuName"].ToString() + "</a> </li> ";
                                }
                                else if (dr["MenuName"].ToString() == "Home")
                                {

                                   
                                    
                                    MainMenu = "  <li class=\"nav-item menu-open\">";
                                    MainMenu += " <a href=\""+ dr["OnSelect"].ToString() + "\" class=\"nav-link active\">";
                                    MainMenu += "  <i class=\"nav-icon fas fa-tachometer-alt\"></i>";
                                    MainMenu += " <p>";
                                    MainMenu += " "+ dr["MenuName"].ToString() + "";
                                    MainMenu += "  </p>";
                                    MainMenu += "  </a>";
                                    MainMenu += "  </li>";
                                    ;
                                }

                                else if (dr["menuName"].ToString() == "Logout")
                                {
                                    MainMenu = "  <li class=\"nav-item\">";
                                    MainMenu += " <a href=\"" + dr["OnSelect"].ToString() + "\" class=\"nav-link\">";
                                    MainMenu += "  <i class=\"nav-icon far fa-circle text-info\"></i>";
                                    MainMenu += " <p>";
                                    MainMenu += " " + dr["MenuName"].ToString() + "";
                                    MainMenu += "  </p>";
                                    MainMenu += "  </a>";
                                    MainMenu += "  </li>";
                                }

                            }
                            html = html + MainMenu;
                        }
                    }
                }
            }
            menu.InnerHtml = html;

        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    private string Load_SubMenu(string MenuId, DataTable dt)
    {
        string html = "";
        try
        {
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    string parentId = dr["ParentId"].ToString();
                    if (Convert.ToInt32(parentId) != 0 & Convert.ToInt32(parentId) == Convert.ToInt32(MenuId) & string.Equals(dr["MenuName"].ToString(), "-") == false)
                    {
                        string onselect = "'" + dr["OnSelect"].ToString() + "'";
                        string menuName = dr["MenuName"].ToString();
                        if (menuName == "Address Verify")
                        {
                            //html = html + "<li class=\"nav-item\"><a  href=\"" + dr["OnSelect"].ToString() + "\">" + dr["MenuName"].ToString() + "<span class=\"label label-rouded label-custom pull-right\" style=\"background-color: Green;\">" + Addressverify + "</span></a></li>";
                            html += "  <li class=\"nav-item\">";
                            html += " <a href=\"" + dr["OnSelect"].ToString() + "\" class=\"nav-link\">";
                            html += "   <i class=\"far fa-circle nav-icon\"></i>";
                            html += "  <p>" + dr["MenuName"].ToString() + "</p>";
                            html += " </a>";
                            html += " </li>";
                        }

                        else if (menuName == "Bank Verify")
                        {
                            html += "  <li class=\"nav-item\">";
                            html += " <a href=\"" + dr["OnSelect"].ToString() + "\" class=\"nav-link\">";
                            html += "   <i class=\"far fa-circle nav-icon\"></i>";
                            html += "  <p>" + dr["MenuName"].ToString() + "</p>";
                            html += " </a>";
                            html += " </li>";
                        }
                        else if (menuName == "PanCard Verify")
                        {
                            html += "  <li class=\"nav-item\">";
                            html += " <a href=\"" + dr["OnSelect"].ToString() + "\" class=\"nav-link\">";
                            html += "   <i class=\"far fa-circle nav-icon\"></i>";
                            html += "  <p>" + dr["MenuName"].ToString() + "</p>";
                            html += " </a>";
                            html += " </li>";
                        }
                        else
                        {
                            html += "  <li class=\"nav-item\">";
                            html += " <a href=\"" + dr["OnSelect"].ToString() + "\" class=\"nav-link\">";
                            html += "   <i class=\"far fa-circle nav-icon\"></i>";
                            html += "  <p>" + dr["MenuName"].ToString() + "</p>";
                            html += " </a>";
                            html += " </li>";
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
        return html;
    }

}