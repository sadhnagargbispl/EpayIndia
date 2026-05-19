using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class MyPurchaseDetail : System.Web.UI.Page
{
    DAL ObjDAL = new DAL();
    string IsoStart;
    string IsoEnd;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!IsPostBack)
                {
                    FillData();
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            // Optionally log the exception or handle it
        }

    }
    public void FillData()
    {
        string str = "";
        try
        {
            str = "SELECT billno AS [Order No.], Repurchincome AS [Order Amount], kitname AS [Pakage Name], " +
                  "REPLACE(CONVERT(VARCHAR, billdate, 106), ' ', '-') AS [Activation Date] " +
                  "FROM MM_kitmaster AS A, Repurchincome_MM AS C " +
                  "WHERE A.kitid = C.kitid AND C.formno = '" + HttpContext.Current.Session["formno"] + "' " +
                  "ORDER BY rid DESC";

            DataTable Dt_ = new DataTable();
            Dt_ = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["constr"].ConnectionString, CommandType.Text, str).Tables[0];

            if (Dt_.Rows.Count > 0)
            {
                gv.DataSource = Dt_;
                gv.DataBind();
                HttpContext.Current.Session["DirectData1"] = Dt_;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gv.PageIndex = e.NewPageIndex;
            FillData();
        }
        catch (Exception ex)
        {
            // Optionally log the exception or handle it here
        }
    }
}
