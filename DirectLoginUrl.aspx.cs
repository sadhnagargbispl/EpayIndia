using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DirectLoginUrl : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
         
            if (Session["IDNo"] == null)
            {
                Response.Redirect("https://partnerlogin.click4bima.com/");
            }

            else
            {
                string url = "https://partnerlogin.click4bima.com?p=ep" +
                             "&ak=aqDCIdQmpeUT20jILK3QpOkk%2F1qPqrdSVZM%2F0v5mmCQIQWNxS%2Fl%2F%2Bv%2FXrMOE9Dian51oXChI2OASLJ03OUwSfQ%3D%3D" +
                             "&pt=TzSfj9cF1NuI62FjXr2czywdtBCzoYCge7xGDBaKsIuc6gXqCWG2hnfdl4A1lGj0" +
                             "&un=" + Session["IDNo"] +
                             "&pw=" + Session["MemPassw"];

                Response.Redirect(url, false);
                Context.ApplicationInstance.CompleteRequest();
            }


        }
        catch (Exception)
        {
            // silently handled as in VB code
        }
    }
    private string Base64Encode(string plainText)
    {
        byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    
}