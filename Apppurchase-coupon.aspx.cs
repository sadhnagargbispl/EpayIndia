using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
public partial class Apppurchase_coupon : System.Web.UI.Page
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
            IsoStart = ObjDAL.Isostart;
            IsoEnd = ObjDAL.IsoEnd;

            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    DataTable tmpTable = new DataTable();
                    FUN_SP_GETLIST();
                }
            }
            else
            {
                Response.Redirect("Applogin.aspx", false);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FUN_SP_GETLIST()
    {
        try
        {
            DataSet Ds = new DataSet();
            string Sql = IsoStart + "Exec Sp_GetAllPageckeDetail" + IsoEnd;
            Ds = SqlHelper.ExecuteDataset(constr1, CommandType.Text, Sql);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                RepMovies.DataSource = Ds.Tables[0];
                RepMovies.DataBind();
            }
            if (Ds.Tables[1].Rows.Count > 0)
            {
                RepFood.DataSource = Ds.Tables[1];
                RepFood.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
