using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CouponReport : System.Web.UI.Page
{
    string strquery;
    DataTable dt;
    clsGeneral objGen = new clsGeneral();
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        {
            if (!IsPostBack)
            {
                PaymentDetails();
            }
        }
        else
        {
            Response.Redirect("Logout.aspx");
            Response.End();
        }
    }

    private void PaymentDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            DAL obj = new DAL();
            string condition = "";
            strquery = "Exec GetCouponDataCpanel '" + Convert.ToInt64(Session["Formno"]) + "'";
            dt = SqlHelper.ExecuteDataset(constr,CommandType.Text,strquery).Tables[0];
            RptDirects.DataSource = dt;
            RptDirects.DataBind();
            Session["ReceivedPin"] = dt;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "SideB");
        }
    }
}