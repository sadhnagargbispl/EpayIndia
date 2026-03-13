using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NewTree : System.Web.UI.Page
{
    private string strQuery;
    private int minDeptLevel;
    private SqlConnection conn = new SqlConnection();
    private SqlCommand Comm = new SqlCommand();
    private SqlDataAdapter Adp1;
    private DataTable  dtData = new DataTable();
    private string strDrawKit;
    private DAL objDal;
    double ParentId;
    double FormNo;
    string MemberName;
    string LegNo, Id;
    string Doj = "";
    string Category = "";
    double LeftBV, RightBV, LeftPV, RightPV;
    double LeftJoining, RightJoining, LeftCarryForward, RightCarryForward, ActiveDirect, ActiveIndirect;
    string UpLiner, Sponsor;
    int level;
    string NodeName;
    string myRunTimeString = "";
    string ExpandYesNo;
    string strImageFile;
    string strUrlPath = "";
    string UpDt;
    string tooltipstrig;
    string IsBlock = "";
    string BlockStatus = "";
    string UplinerFormno = "";
    string status = "";

    protected void Page_Load(object sender, EventArgs e)
    
    {
        objDal = new DAL();

        if (!Page.IsPostBack)
        {
            if (Session["AStatus"] != null && Session["AStatus"].ToString() == "OK")
            {
                // Call getKits();
            }
            else
            {
                Response.Redirect("Default.aspx");
            }

            ValidateTree();
        }
    }
    private void ValidateTree()
    {
        string strSelectedFormNo;
        if (!string.IsNullOrEmpty(Request["deptlevel"]))
        {
            minDeptLevel = Convert.ToInt32(Request["deptlevel"]);
        }
        else
        {
            minDeptLevel = 5;
        }

        if (string.IsNullOrEmpty(Request["DownLineFormNo"]))
        {
       
        }
        else
        {
            if (CheckDownLineMemberTree() == false)
            {
              
                lblError.Text = "Please Check DownLine Member ID.";
                lblError.Visible = true;
            }

           
            strSelectedFormNo = Request["DownLineFormNo"];

            strQuery = getQuery(strSelectedFormNo, minDeptLevel);
            GenerateTree(strQuery);
        }
      
    }
    private bool CheckDownLineMemberTree()
    {
        bool chk = false;
        strQuery = "SELECT FormnoDwn FROM M_MemTreeRelation WHERE FormNo=" + Request["DownLineFormNo"];

        // Assuming objDal is an instance of DAL class handling database operations
        dtData = new DataTable();
        dtData = objDal.GetData(strQuery);

        if (dtData.Rows.Count <= 0)
        {
            chk = false;
        }
        else
        {
            chk = true;
        }

        return chk;
    }
    private void GenerateTree(string strQuery)
    {
        // Assuming objDal is an instance of DAL class handling database operations
        dtData = new DataTable();
        dtData = objDal.GenerateTreeProc(strQuery);

        DataTable dt1 = new DataTable();
      
        myRunTimeString = myRunTimeString + "<Script Language=Javascript>" + Environment.NewLine;
        tooltipstrig = ToolTipTable();

        // Define Parent Setting
        ParentId = -1;

        if (!string.IsNullOrEmpty(Request["DownLineFormNo"]))
        {
            FormNo = Convert.ToDouble(Request["DownLineFormNo"]);
        }
        else
        {
            // FormNo = Convert.ToDouble(Session["FormNo"].ToString());
        }

        strImageFile = "images/base.jpg";
        string UplineFormno = "";
        int i = 0;
        foreach (DataRow dr in dtData.Rows)
        {
            strImageFile = Session["compWeb"] + "/" + dr["JoinColor"].ToString();

            if (i == 0)
            {
                myRunTimeString = myRunTimeString + "mytree = new dTree('mytree','" + strImageFile + "');" + Environment.NewLine;
                i++;
            }

            ParentId = Convert.ToDouble(dr["UPLNFORMNO"].ToString());
            FormNo = Convert.ToDouble(dr["FormNoDwn"].ToString());
            LegNo = dr["legno"].ToString();
            UpLiner = dr["UpLiner"].ToString();
            Sponsor = dr["Sponsor"].ToString();
            Doj = dr["doj"].ToString();
            Category = dr["Category"].ToString();
            LeftBV = Convert.ToDouble(dr["LeftBV"].ToString());
            RightBV = Convert.ToDouble(dr["rightBV"].ToString());
            status = dr["idstatus"].ToString();
            LeftJoining = Convert.ToDouble(dr["Leftjoining"].ToString());
            RightJoining = Convert.ToDouble(dr["rightjoining"].ToString());
            ActiveDirect = Convert.ToDouble(dr["LeftActive"].ToString());
            ActiveIndirect = Convert.ToDouble(dr["RightActive"].ToString());
            level = Convert.ToInt32(dr["level"].ToString());
            Session["Upliner"] = dtData.Rows[0]["uplinerformno"].ToString();
            strUrlPath = "newtree.aspx?DownLineFormNo=" + FormNo;
            UpDt = dr["UpDt"].ToString();
            IsBlock = dr["IsBlock"].ToString();
            BlockStatus = dr["BlockedStatus"].ToString();
            MemberName = "(" + dr["Formno"].ToString() + ")" + "(" + dr["memName"] + ")";
            NodeName = dr["memName"].ToString();
            int LoopValue = Convert.ToInt32(dr["mlevel"]);
            LeftCarryForward = Convert.ToDouble(dr["LeftCarryForwardBv"].ToString());
            RightCarryForward = Convert.ToDouble(dr["RightCarryForwardBv"].ToString());

            if (LoopValue < 6 && LoopValue > 0)
            {
                ExpandYesNo = "true";
            }
            else
            {
                ExpandYesNo = "false";
            }

            if (ParentId == -1)
            {
                ExpandYesNo = "true";
            }

            if (UpDt == "01 Jan 00")
            {
                UpDt = "";
            }

            if (FormNo <= 0)
            {
                strUrlPath = "";

                if (LegNo == "1")
                {
                    MemberName = "Left";
                }
                else
                {
                    MemberName = "Right";
                }
            }
            else
            {
                if (dr["ActiveStatus"].ToString() == "N")
                {
                    strImageFile = "images/deact.jpg";
                }
                else if (dr["Kitid"].ToString() == "1")
                {
                    strImageFile = "images/Red.jpg";
                }
                else if (dr["Kitid"].ToString() == "2")
                {
                    strImageFile = "images/Blue.jpg";
                }
                else if (dr["Kitid"].ToString() == "3")
                {
                    strImageFile = "images/Green.jpg";
                }
                else if (dr["Kitid"].ToString() == "4")
                {
                    strImageFile = "images/Yellow.jpg";
                }
                else if (dr["Kitid"].ToString() == "5")
                {
                    strImageFile = "images/Orange.jpg";
                }
                else if (dr["Kitid"].ToString() == "6")
                {
                    strImageFile = "images/purpel.jpg";
                }
                else
                {
                    strImageFile = "images/empty.jpg";
                }

                strUrlPath = "newtree.aspx?DownLineFormNo=" + FormNo;
            }

            strImageFile = Session["compWeb"] + "/" + dr["JoinColor"].ToString();
            myRunTimeString = myRunTimeString + " mytree.add(" + FormNo + "," + ParentId + ",'" + Category + "'," +
                "'" + Doj + "','" + MemberName + "','" + NodeName + "','" + UpLiner + "','" + Sponsor + "'," + LeftBV + "," +
                "" + RightBV + ",'" + strUrlPath + "','" + MemberName + "','','" + strImageFile + "','" + strImageFile + "'," +
                "" + ExpandYesNo + ",'" + LeftJoining + "','" + RightJoining + "'," + level + ",'" + UpDt + "','" + LeftCarryForward + "'," +
                "'" + RightCarryForward + "','" + Id + "','" + ActiveDirect + "','" + ActiveIndirect + "','" + IsBlock + "'," +
                "'" + BlockStatus + "','" + LeftPV + "','" + RightPV + "','" + status + "');" + Environment.NewLine;
        }

        myRunTimeString = myRunTimeString + Environment.NewLine + Environment.NewLine + Environment.NewLine + " document.write(mytree);" + Environment.NewLine;
        myRunTimeString = myRunTimeString + Environment.NewLine + "</script> " + "<br /> <br /> <br /> <br /> ";

        RegisterClientScriptBlock("clientScript", myRunTimeString);
    }
    private string ToolTipTable()
    {
        string strToolTip;
        strToolTip = "onMouseOver=\"ddrivetip('<table width=100% border=0 cellpadding=5 cellspacing=1 bgcolor=#CCCCCC class=containtd>  <tr>     <td width=50% bgcolor=#999999><font color=#FFFFFF><strong>Member ID</strong></font></td>  </tr>  <tr>     <td>430</td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Name</strong></font></td>  </tr>  <tr>     <td>Mr-MAHESH BHARDWAJ </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Date of Joining</strong></font></td>  </tr>  <tr>     <td>2008-08-07 16:14:54 </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Total Status</strong></font></td>  </tr>  <tr>     <td>LEFT:123 , RIGHT:2198 </td>  </tr>  <tr>     <td bgcolor=#999999><font color=#FFFFFF><strong>Product</strong></font></td>  </tr>  <tr>     <td>CODE NO. 01-S.L. &nbsp;</td>  </tr></table>')\" onMouseOut=\"hideddrivetip()\"";
        return strToolTip;
    }
    private string getQuery(string strSelectedFormNo, int minDeptLevel)
    {
        // Check if user pass downline member, then make according to downline member, otherwise show his tree
        return "exec ShowBinaryTree " + strSelectedFormNo + "," + minDeptLevel;
    }
    protected void cmdBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }



}
