using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //BindServices();
            //string sql = "SELECT * FROM ServiceMaster WHERE IsActive = 1";
            //DataTable dt = new DataTable();
            //dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
            //rptServices.DataSource = dt;
            //rptServices.DataBind();
        }
    }
    //private void BindServices()
    //{
    //    DataTable dt = GetServicesFromDB();
    //    //dt.Columns.Add("EncPid", typeof(string));
    //    //foreach (DataRow dr in dt.Rows)
    //    //{
    //    //    string portal = dr["PortalName"].ToString();
    //    //    dr["EncPid"] = EncryptText(portal);
    //    //}

    //    rptServices.DataSource = dt;
    //    rptServices.DataBind();
    //}

    private DataTable GetServicesFromDB()
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM services WHERE status = 1", con))
            {
                con.Open();
                dt.Load(cmd.ExecuteReader());
            }
        }
        return dt;
    }
    public static string EncryptText(string input)
    {
        byte[] key = Encoding.UTF8.GetBytes("A1B2C3D4E5F6G7H8");  // 16 char key
        byte[] iv = Encoding.UTF8.GetBytes("1H2G3F4E5D6C7B8A");   // 16 char IV

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(input);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}