using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FoodBookingRedirect : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string Str = "";
            Str = " select *,(MemFirstname+' '+MemLastname) as Name from M_memberMaster where Formno = '" + Convert.ToInt32(Session["formno"]) + "'";
            DataTable Dt = new DataTable();
            Dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, Str).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                string stri;
                //stri = Dt.Rows[0]["Idno"].ToString().Trim() + ";" + Dt.Rows[0]["Passw"].ToString() + ";" + Dt.Rows[0]["Name"].ToString().Trim() + ";" + Dt.Rows[0]["Email"].ToString().Trim() + ";" + Dt.Rows[0]["Mobl"].ToString().Trim() + ";" + Convert.ToDateTime(Dt.Rows[0]["doj"]).ToString("dd-MMM-yyyy")
                stri = Dt.Rows[0]["Idno"].ToString().Trim() + ";" + Dt.Rows[0]["Passw"].ToString() + "";
                string qr = EncryptData(stri);
                string Url = "https://food.epayindia.in/controller.aspx?user_info=" + qr + "&log_key=8173842B-3E77-4C5A-AAB8-C9F13562C4BD";
                Response.Redirect(Url);
            }
            else
            {
                Response.Redirect("https://food.epayindia.in/");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    private string EncryptData(string data)
    {
        string strmsg = string.Empty;
        byte[] encode = new byte[data.Length];
        encode = Encoding.UTF8.GetBytes(data);
        strmsg = Convert.ToBase64String(encode);
        return strmsg;
    }

}
