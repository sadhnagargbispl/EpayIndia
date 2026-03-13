using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

public partial class Referaltree : System.Web.UI.Page
{

    DataTable Dt = new DataTable();
    DataSet Ds = new DataSet();
    DAL objDAL = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    string strQuery;
    int minDeptLevel;
    int ParentId;
    double FormNo;
    string MemberName;
    string LegNo;
    string Doj;
    string Category = "";
    double LeftBV;
    double RightBV;
    string UpLiner;
    string Sponsor;
    string NodeName;
    string myRunTimeString = "";
    string ExpandYesNo;
    string strImageFile = "";
    string strUrlPath = "";
    string idstatus = "";
    string tooltipstrig;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (Session["AStatus"] != null && Session["AStatus"].ToString() == "OK")
                {
                    ValidateTree();
                }
                else
                {
                    Response.Redirect("logout.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }

    private void ValidateTree()
    {
        try
        {
            string strSelectedFormNo;
            // ---- If User Not Set MinDept level than set 10 dept level
            if (string.IsNullOrEmpty(Request["DeptLevel"]))
            {
                minDeptLevel = 3;
            }
            else
            {
                minDeptLevel = Convert.ToInt32(Request.QueryString["DeptLevel"].ToString());
            }
            if (!string.IsNullOrEmpty(Request["DownLineFormNo"]))
            {
                strSelectedFormNo = Request["DownLineFormNo"];
                strQuery = getQuery(strSelectedFormNo, minDeptLevel);
                GenerateTree(strQuery);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private bool CheckDownLineMemberTree()
    {
        bool checkDownLineMemberTree = false;
        try
        {
            DataTable dt = new DataTable();
            string strQuery = objDAL.IsoStart + "SELECT FormnoDwn ";
            strQuery += "FROM " + objDAL.DBName + "..R_MemTreeRelation WHERE FormNoDwn='" + Request["DownLineFormNo"] + "' AND FormNo='" + Session["FORMNO"] + "'" + objDAL.IsoEnd;
            dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery).Tables[0];
            if (dt.Rows.Count <= 0)
            {
                checkDownLineMemberTree = false;
            }
            else
            {
                checkDownLineMemberTree = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return checkDownLineMemberTree;
    }
    public void GenerateTree(string strQuery)
    {
        try
        {
            DataSet ds1 = new DataSet();
            ds1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery);
            myRunTimeString = myRunTimeString + "<Script Language=Javascript>" + Environment.NewLine;
            tooltipstrig = ToolTipTable();
            ParentId = -1;
            if (Request["DownLineFormNo"] != null && Request["DownLineFormNo"] != "")
            {
                FormNo = Convert.ToDouble(Request["DownLineFormNo"]);
                DataSet tmpDS = new DataSet();
                strQuery = objDAL.IsoStart + "SELECT a.*,(Case When a.Isblock = 'Y' Then 'black.jpg' Else  b.Joincolor  End)  As Joincolor ,B.kitname as Category,c.Direct,C.Indirect,";
                strQuery += " Case when a.ActiveStatus='Y' then 'Active' else 'Deactive' end as idstatus ";
                strQuery += "FROM " + objDAL.DBName + "..m_MemberMaster as a, " + objDAL.DBName + "..M_KitMaster as b,V#DI as c ";
                strQuery += " WHERE a.KitID=b.KitID AND a.FORMNO=" + FormNo + " and a.Formno=c.Formno" + objDAL.IsoEnd;
                tmpDS = SqlHelper.ExecuteDataset(constr1, CommandType.Text, strQuery);
                if (tmpDS.Tables[0].Rows.Count > 0)
                {
                    MemberName = tmpDS.Tables[0].Rows[0]["IdNo"].ToString();
                    NodeName = tmpDS.Tables[0].Rows[0]["MemFirstName"].ToString();
                    strImageFile = Session["CompWeb"] + "/Img/" + tmpDS.Tables[0].Rows[0]["JoinColor"].ToString();
                    Category = tmpDS.Tables[0].Rows[0]["Category"].ToString();
                    LeftBV = Convert.ToDouble(tmpDS.Tables[0].Rows[0]["Direct"]);
                    RightBV = Convert.ToDouble(tmpDS.Tables[0].Rows[0]["Indirect"]);
                    idstatus = tmpDS.Tables[0].Rows[0]["Idstatus"].ToString();
                }
            }
            else
            {
                FormNo = Convert.ToDouble(Session["FormNo"]);
                MemberName = Session["FormNo"].ToString();
                NodeName = Session["MemName"].ToString();
            }

            myRunTimeString = myRunTimeString + "mytree = new dTree('mytree','" + strImageFile + "');" + Environment.NewLine;
            myRunTimeString = myRunTimeString + "mytree.add(" + FormNo + "," + ParentId + "," + "'" + Category + "'," + "'" + Doj + "','" + MemberName + "','" + NodeName + "','" + UpLiner + "'" + ",'" + Sponsor + "'," + LeftBV + "," + RightBV + "," + "''" + ",'" + MemberName + "'," + "''" + "," + "'" + strImageFile + "'" + "," + "'" + strImageFile + "'" + "," + "'true'" + ",'" + idstatus + "');" + Environment.NewLine;

            int LoopValue = 0;
            string FolderFile = "";

            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                ParentId = Convert.ToInt32(dr["RefFormno"]);
                FormNo = Convert.ToDouble(dr["FormNoDwn"]);
                LegNo = dr["Reflegno"].ToString();
                UpLiner = "0";
                Sponsor = "0";
                Doj = dr["doj"].ToString();
                Category = dr["Category"].ToString();
                LeftBV = Convert.ToDouble(dr["Direct"]);
                RightBV = Convert.ToDouble(dr["Indirect"]);
                idstatus = dr["IdStatus"].ToString();
                strUrlPath = "Referaltree.aspx?DownLineFormNo=" + FormNo;
                strImageFile = "img/" + dr["JoinColor"].ToString();

                MemberName = dr["IdNO"].ToString();
                NodeName = dr["MemFirstName"].ToString();

                LoopValue = LoopValue + 1;
                if (LoopValue <= 5)
                {
                    ExpandYesNo = "true";
                }
                else
                {
                    ExpandYesNo = "false";
                }

                if (FormNo <= 0)
                {
                    strImageFile = "images/empty.jpg";
                    MemberName = "Direct";
                    strUrlPath = "";
                }
                else
                {
                    strImageFile = Session["CompWeb"] + "/img/" + dr["JoinColor"].ToString();
                    strUrlPath = "ReferalTree.aspx?DownLineFormNo=" + FormNo;
                }

                myRunTimeString = myRunTimeString + " mytree.add(" + FormNo + "," + ParentId + "," + "'" + Category + "'" + "," + "'" + Doj + "','" + MemberName + "','" + NodeName + "','" + UpLiner + "'" + ",'" + Sponsor + "'," + LeftBV + "," + RightBV + "," + "'" + strUrlPath + "'" + ",'" + MemberName + "'," + "'" + "" + "'" + "," + "'" + strImageFile + "'" + "," + "'" + strImageFile + "'" + "," + ExpandYesNo + ",'" + idstatus + "');" + Environment.NewLine;
            }

            myRunTimeString = myRunTimeString + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + " document.write(mytree);" + Environment.NewLine;
            myRunTimeString = myRunTimeString + "</script> " + "<br> <br> <br> <br> ";

            RegisterClientScriptBlock("clientScript", myRunTimeString);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private string ToolTipTable()
    {
        try
        {
            string strToolTip = "";
            // Example tooltip string
            //strToolTip = "onMouseOver=\"ddrivetip('<table width=100% border=0 cellpadding=5 cellspacing=1 bgcolor=#CCCCCC class=containtd>  <tr>     <td width=50% bgcolor=#999999><font color=#FFFFFF><strong>Member ID</strong></font></td>  </tr>  <tr>     <td>430</td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Name</strong></font></td>  </tr>  <tr>     <td>Mr-MAHESH BHARDWAJ </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Date of Joining</strong></font></td>  </tr>  <tr>     <td>2008-08-07 16:14:54 </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Total Status</strong></font></td>  </tr>  <tr>     <td>LEFT:123 , RIGHT:2198 </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Product</strong></font></td>  </tr>  <tr>     <td>CODE NO. 01-S.L. &nbsp;</td>  </tr></table>')\" onMouseOut=\"hideddrivetip()\"";
            return strToolTip;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private string getQuery(string strSelectedFormNo, int minDeptLevel)
    {
        String  strQ = string.Empty;
        try
        {
            strQ =  "exec sp_ShowRefTree " + strSelectedFormNo + " , " + minDeptLevel;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return strQ;
    }

    protected void cmdBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }
}
