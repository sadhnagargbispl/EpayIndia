
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

public partial class AppStoreredirect : Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DirectLogin();
        }
    }

    private void DirectLogin()
    {
        try
        {
            // Session Check
            if (Session["IDNO"] == null || Session["MemPassw"] == null)
            {
                Response.Redirect("https://store.epayindia.in/");
                return;
            }

            // Encrypt Values
            string userName = Encrypt(Session["IDNO"].ToString());
            string password = Encrypt(Session["MemPassw"].ToString());

            // HTML Form Generate
            StringBuilder sb = new StringBuilder();

            sb.Append("<html>");
            sb.Append("<body onload='document.forms[0].submit()'>");

            sb.Append("<form method='POST' action='https://store.epayindia.in/Account/DirectLogin'>");

            sb.AppendFormat("<input type='hidden' name='LoginId' value='{0}' />", userName);
            sb.AppendFormat("<input type='hidden' name='Password' value='{0}' />", password);

            sb.Append("<input type='hidden' name='Token' value='RXVwL2lp7z7iKcIA0KP+3Br1se9o5vLCDeJAAZ6SCxwdybJEEdfqxfGZH2M10yxa' />");

            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");

            Response.Clear();
            Response.Write(sb.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            Response.Write("Error : " + ex.Message);
        }
    }

    public string Encrypt(string plainText)
    {
        try
        {
            string completeUrl =
                "https://store.epayindia.in/Account/Encrypt?plainText=" +
                Uri.EscapeDataString(plainText);

            ServicePointManager.SecurityProtocol =
                (SecurityProtocolType)3072; // TLS 1.2

            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(completeUrl);

            request.Method = "GET";
            request.ContentType = "application/json";

            using (HttpWebResponse response =
                (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader =
                    new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Encryption Error : " + ex.Message);
        }
    }

    public string Decrypt(string cipherText)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes("Y$8tM9d#k4KqpV^rLw2zXN&yS7uPqU@3");
            aes.IV = iv;

            using (MemoryStream ms = new MemoryStream(buffer))
            using (CryptoStream cs = new CryptoStream(ms,
                aes.CreateDecryptor(),
                CryptoStreamMode.Read))
            using (StreamReader reader = new StreamReader(cs))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//public partial class AppStoreredirect : System.Web.UI.Page
//{
//    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
//    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
//    protected void Page_Load(object sender, EventArgs e)
//    {
//        try
//        {
//            if (Session["IDNO"] == null)
//            {
//                Response.Redirect("https://store.epayindia.in/");
//            }
//            else
//            {
//                try
//                {
//                    string formPostText = "";
//                    string UserName = Encrypt(Session["IDNO"].ToString());
//                    string Password = Encrypt(Session["MemPassw"].ToString());
//                    formPostText = @"<form method=""POST"" action=""https://store.epayindia.in/Account/DirectLogin"" name=""frm2Post"">" +
//                                   " <input type=\"hidden\" name=\"LoginId\" value=\"" + UserName + "\" />" +
//                                   " <input type=\"hidden\" name=\"Password\" value=\"" + Password + "\" />" +
//                                   " <input type=\"hidden\" name=\"Token\" value=\"RXVwL2lp7z7iKcIA0KP+3Br1se9o5vLCDeJAAZ6SCxwdybJEEdfqxfGZH2M10yxa\" />" +
//                                   " <script type=\"text/javascript\">document.frm2Post.submit();</script></form>";

//                    Response.Write(formPostText);
//                }
//                catch (Exception ex)
//                {
//                    // Handle exception (if needed, log it or show a message)
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            throw new Exception(ex.Message);
//        }

//    }
//    public string Encrypt(string plainText)
//    {
//        string completeUrl = "https://store.epayindia.in/Account/Encrypt?plainText=" + plainText;
//        System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072; // TLS 1.2

//        HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(completeUrl);
//        request1.ContentType = "application/json";
//        request1.Method = "GET";

//        using (HttpWebResponse httpWebResponse = (HttpWebResponse)request1.GetResponse())
//        using (StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream()))
//        {
//            string responseString = reader.ReadToEnd();
//            return responseString;
//        }
//    }
//    public string Decrypt(string cipherText)
//    {
//        byte[] iv = new byte[16]; // 16-byte IV for AES
//        byte[] buffer = Convert.FromBase64String(cipherText);

//        using (Aes aes = Aes.Create())
//        {
//            aes.Key = Encoding.UTF8.GetBytes("Y$8tM9d#k4KqpV^rLw2zXN&yS7uPqU@3"); // 32-byte key for AES-256
//            aes.IV = iv;

//            using (MemoryStream ms = new MemoryStream(buffer))
//            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
//            using (StreamReader reader = new StreamReader(cs))
//            {
//                return reader.ReadToEnd();
//            }
//        }
//    }

//}

