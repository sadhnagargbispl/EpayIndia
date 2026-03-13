using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

public partial class UniTree : System.Web.UI.Page
{
    private string strQuery;
    private int minDeptLevel;
    string strDrawKit;
    DAL objDal = new DAL();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, System.EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (Session["AStatus"] == null)
                {
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    ValidateTree();
                }

            }
        }
        catch (Exception Ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + Ex.Message + "');", true);
        }
    }

    private void ValidateTree()
    {
        if (Request.QueryString["type"] == null)
        {
            
        }
        else {
            Session["Ttype"] = Request.QueryString["type"].ToString();

        }
             string strSelectedFormNo;

        if (!string.IsNullOrEmpty(Request["deptlevel"]))
            minDeptLevel = Convert.ToInt32(Request["deptlevel"]);
        else
            minDeptLevel = 3;

        if (string.IsNullOrEmpty(Request["DownLineFormNo"]))
        {
            lblError.Text = "Please Enter Member Id in respective field.";
            lblError.Visible = true;
        }
        else
        {
            strSelectedFormNo = Request["DownLineFormNo"];
            strQuery = getQuery(strSelectedFormNo, minDeptLevel);
            GenerateTree(strQuery);
        }
    }
    private string getQuery(string strSelectedFormNo, int minDeptLevel)
    {
        String Query = "";
        try
        {

            if (Session["Ttype"].ToString() == "A")
            {
                Query = "exec sp_Pool_Tree " + strSelectedFormNo + "," + minDeptLevel;
            }
            else if (Session["Ttype"].ToString() == "B")
            {
                Query = "exec sp_Pool_1_Tree " + strSelectedFormNo + "," + minDeptLevel;
            }
            else if (Session["Ttype"].ToString() == "C")
            {
                Query = "exec sp_Pool_2_Tree " + strSelectedFormNo + "," + minDeptLevel;
            }
            else if (Session["Ttype"].ToString() == "D")
            {
                Query = "exec sp_Pool_3_Tree " + strSelectedFormNo + "," + minDeptLevel;
            }
            else if (Session["Ttype"].ToString() == "E")
            {
                Query = "exec sp_Pool_4_Tree " + strSelectedFormNo + "," + minDeptLevel;
            }



        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Query;
    }
    private string ToolTipTable()
    {
        string strToolTip = "";

        return strToolTip;
    }

    private void GenerateTree(string strQuery)
    {
        DataTable dtData = new DataTable();
        strQuery = objDal.IsoStart + strQuery.ToString() + objDal.IsoEnd;
        dtData = SqlHelper.ExecuteDataset(constr1,CommandType.Text, strQuery).Tables[0];
        double ParentId;
        double FormNo;
        string MemberName;
        string LegNo;
        string Doj = "";
        string Category = "";
        double LeftBV, RightBV;
        double LeftJoining, RightJoining;
        string UpLiner, Sponsor, IMECode, CorpusName = "";
        int level;
        string NodeName;
        string myRunTimeString = "";
        string ExpandYesNo;
        string strImageFile;
        string strUrlPath = "";
        string UpDt;
        string tooltipstrig = "";
        string idno = "";

        myRunTimeString = myRunTimeString + "<Script Language=Javascript>" + Environment.NewLine;
        tooltipstrig = ToolTipTable();


        ParentId = -1;

        if (Request.QueryString["DownLineFormNo"] != "")
            FormNo = Convert.ToDouble(Request.QueryString["DownLineFormNo"]);
        else
        {
        }
        strImageFile = "images/base.jpg";
        int i = 0;

        int LoopValue;


        foreach (DataRow dr in dtData.Rows)
        {
            strImageFile = "images/" + dr["JoinColor"].ToString();
            if (i == 0)
            {
                myRunTimeString = myRunTimeString + "mytree = new dTree('mytree','" + strImageFile + "');" + Environment.NewLine;
                i = i + 1;
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
            LeftJoining = Convert.ToDouble(dr["Leftjoining"].ToString());
            RightJoining = Convert.ToDouble(dr["rightjoining"].ToString());
            level = Convert.ToInt32(dr["level"].ToString());
            IMECode = dr["IMECode"].ToString();
            // CorpusName = dr.Item("CorpusName").ToString
            strUrlPath = "Unitree.aspx?DownLineFormNo=" + FormNo;
            UpDt = dr["UpDt"].ToString();
            MemberName = "(" + dr["Formno"].ToString() + ")" + "(" + dr["memName"] + ")";
            NodeName = dr["memName"].ToString();
            LoopValue = Convert.ToInt32(dr["mlevel"]);
            idno = dr["Formno"].ToString();

            if (LoopValue < 4 & LoopValue > 0)
                ExpandYesNo = "true";
            else
                ExpandYesNo = "false";
            if (ParentId == -1)
                ExpandYesNo = "true";

            if (UpDt == "01 Jan 00")
                UpDt = "";

            if (FormNo <= 0)
            {
                strUrlPath = ""; // '"newjoining.aspx?UpLnFormNo=" & ParentId & "&legno=" & LegNo
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
                else if (Convert.ToUInt32(dr["Kitid"].ToString()) == 1)
                {
                    strImageFile = "images/Red.jpg";
                }
                else if (Convert.ToUInt32(dr["Kitid"].ToString()) == 2)
                {
                    strImageFile = "images/Blue.jpg";
                }
                else if (Convert.ToUInt32(dr["Kitid"].ToString()) == 3)
                {
                    strImageFile = "images/Green.jpg";
                }
                else if (Convert.ToUInt32(dr["Kitid"].ToString()) == 4)
                {
                    strImageFile = "images/Yellow.jpg";
                }
                else if (Convert.ToUInt32(dr["Kitid"].ToString()) == 5)
                {
                    strImageFile = "images/Orange.jpg";
                }
                else if (Convert.ToUInt32(dr["Kitid"].ToString()) == 6)
                {
                    strImageFile = "images/purpel.jpg";
                }
                else
                {
                    strImageFile = "images/empty.jpg";
                }

                strUrlPath = "Unitree.aspx?DownLineFormNo=" + FormNo;
            }

            strImageFile = "images/" + dr["JoinColor"].ToString();
            myRunTimeString = myRunTimeString + " mytree.add(" + FormNo + "," + ParentId + "," + "'" + Category + "'" + "," + "'" + Doj + "','" + MemberName + "','" + NodeName + "','" + UpLiner + "'" + ",'" + Sponsor + "'," + LeftBV + "," + RightBV + "," + "'" + strUrlPath + "'" + ",'" + MemberName + "'," + "'" + "" + "'" + "," + "'" + strImageFile + "'" + "," + "'" + strImageFile + "'" + "," + ExpandYesNo + ",'" + LeftJoining + "','" + RightJoining + "','" + IMECode + "','" + level + "' ,'" + UpDt + "','" + CorpusName.ToString() + "','" + idno + "');" + Environment.NewLine;
        }


        myRunTimeString = myRunTimeString + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + " document.write(mytree);" + Environment.NewLine;
        myRunTimeString = myRunTimeString + Environment.NewLine + "</script> " + "<br /> <br /> <br /> <br /> ";

        RegisterClientScriptBlock("clientScript", myRunTimeString);
    }

    private bool CheckDownLineMemberTree()
    {
        bool chk = new bool();
        chk = false;
        try
        {
            DataTable dtData = new DataTable();
            string TblName = "";
            if (Session["Ttype"].ToString() == "A")
            {
                TblName = "M_PoolTreeRelation";
            }
            else if (Session["Ttype"].ToString() == "B")
            {
                TblName = "M_Pool1TreeRelation";
            }
            else if (Session["Ttype"].ToString() == "C")
            {
                TblName = "M_Pool2TreeRelation";
            }
            else if (Session["Ttype"].ToString() == "D")
            {
                TblName = "M_Pool3TreeRelation";
            }
            else if (Session["Ttype"].ToString() == "E")
            {
                TblName = "M_Pool4TreeRelation";
            }
            strQuery = objDal.IsoStart + " Select FormnoDwn FROM " + objDal.DBName + ".." + TblName + " WHERE  FormNo=" + Request["DownLineFormNo"] + objDal.IsoEnd;

            dtData = objDal.GetData(strQuery);
            if (dtData.Rows.Count <= 0)
            {
                chk = false;
            }
            else
            {
                chk = true;
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return chk;
    }
    protected void cmdBack_Click(object sender, EventArgs e)
   {
     Response.Redirect("Home.aspx");
   }
}
