using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PetroCardPurchaseIncomeReport : System.Web.UI.Page
{
    DAL ObjDal = new DAL();
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                //UserStatus.InnerText = "Welcome " + Session["MemName"] + "(" + Session["Formno"] + ")" + Session["Company"] + "";
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
            if (!Page.IsPostBack)
            {
                Fill_Grid();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    private void Fill_Grid()
    {
        try
        {
            DataTable Dt = new DataTable();
            string str = "";
            str = ObjDal.Isostart + " Exec sp_GetPetroLevelIncome '" + Session["Formno"] + "' " + ObjDal.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, str).Tables[0];
            DibDateCondition.Visible = true;
            Rptdatecondition.DataSource = Dt;
            Rptdatecondition.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            Session["DailyPayout"] = null;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    protected void Rptdatecondition_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Rptdatecondition.PageIndex = e.NewPageIndex;
            Fill_Grid();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('" + ex.Message + "');", true);
        }
    }
}
