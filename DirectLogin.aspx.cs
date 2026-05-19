using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DirectLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string encPortal = Request.QueryString["pid"];

            if (!string.IsNullOrEmpty(encPortal))
            {
                try
                {
                    string portal = DecryptText(encPortal);
                    ViewState["Portal"] = portal;
                    string redirectUrl = "";
                    switch (portal)
                    {
                        case "shop":
                            redirectUrl = "StoreRedirect.aspx";
                            break;

                        case "gv":
                            redirectUrl = "GVRedirect.aspx";
                            break;

                        case "utility":
                            redirectUrl = "https://utility.waveworld.co/";
                            break;

                        case "fd":
                            redirectUrl = "https://food.waveworld.co/";
                            break;

                        case "movie":
                            redirectUrl = "https://movie.waveworld.co/";
                            break;
                    }

                    Response.Redirect(redirectUrl);
                }
                catch
                {
                    Response.Write("Invalid Portal Value");
                }
            }
        }
    }
    public static string DecryptText(string cipherText)
    {
        byte[] key = Encoding.UTF8.GetBytes("A1B2C3D4E5F6G7H8");  // 16 char key
        byte[] iv = Encoding.UTF8.GetBytes("1H2G3F4E5D6C7B8A");   // 16 char IV

        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream(buffer))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}