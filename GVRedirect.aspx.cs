using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GVRedirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string formPostText = "";
            if (Session["IDNo"] == null)
            {
                Response.Redirect("https://gv.epayindia.in/");
            }
            else
            {
                formPostText =
            "<form method=\"POST\" action=\"https://gv.epayindia.in/members/index.php\" name=\"frm2Post\">" +
            "<input type=\"hidden\" name=\"token\" value=\"ec162e6458c12872a46c7c6c12e6bc49\" />" +
            "<input type=\"hidden\" name=\"mod\" value=\"interLogin\" />" +
            "<input type=\"hidden\" name=\"userid\" value=\"" + Session["IDNo"] + "\" />" +
            "<input type=\"hidden\" name=\"password\" value=\"" + Session["MemPassw"] + "\" />" +
            "<script type=\"text/javascript\">document.frm2Post.submit();</script>" +
            "</form>";
                Response.Write(formPostText);
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

    private string Base64Decode(string base64EncodedData)
    {
        byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}