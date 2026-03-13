using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if ( Session["AStatus"] != null )
            {
                lblName.Text = Session["UserName"].ToString();
                //lblCompanyName.Text = Session["CompName"].ToString();
            }
            else
            {
                Response.Redirect("Default.aspx");
            }


        }
    }
}
