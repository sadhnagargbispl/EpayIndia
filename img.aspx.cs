using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


partial class img : System.Web.UI.Page
{
    public string FormNo;
    private DataTable dt;
    private DAL obj=new DAL();
    DataSet Ds;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, System.EventArgs e)
    {
        FormNo = Request["ID"];
        string sql;
        obj = new DAL();
        if (Request["Type"] != null)
        {
            if (Request["Type"] == "Blog")
            {
                sql = "select case When ImgPath='' then '' else ImgPath" + " end as ImageLnk1 from M_Testimonials  where AId='" + Request["AId"] + "'";
                dt = new DataTable();
                //obj = new DAL();
                //dt = obj.GetData(Sql);
                dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImageLnk1"].ToString ();
                    LblNewPic.Visible = false;
                    LblPic.Visible = false;
                    LblUpdatePic.Visible = false;
                    ImageUpload.Visible = false;
                    Upload.Visible = false;
                    Cancel.Visible = false;
                }
            }
            else if (Request["Type"] == "Payment")
            {
                sql = "select case When ScannedFile='' then '" + Session["CompWeb"] + "Images/no_photo.jpg' else '" + Session["CompWeb"] + "/images/UploadImage/'+ ScannedFile" + " end as ImageLnk1 from WalletReq  where Reqno='" + Request["ID"] + "'";
                dt = new DataTable();
                //obj = new DAL();
                //dt = obj.GetData(sql);
                dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImageLnk1"].ToString();
                    LblNewPic.Visible = false;
                    LblPic.Visible = false;
                    LblUpdatePic.Visible = false;
                    ImageUpload.Visible = false;
                    Upload.Visible = false;
                    Cancel.Visible = false;
                }
            }
            else if (Request["Type"] == "PinRequest")
            {
                sql = "select case When Imgpath='' then '" + Session["CompWeb"] + "Images/no_photo.jpg' else '" + Session["CompWeb"] + "/images/UploadImage/'+ ImgPath" + " end as ImageLnk1 from TrnPinReqMain  where Reqno='" + Request["ID"] + "'";
                dt = new DataTable();
                //obj = new DAL();
                //dt = obj.GetData(sql);
                dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImageLnk1"].ToString();
                    LblNewPic.Visible = false;
                    LblPic.Visible = false;
                    LblUpdatePic.Visible = false;
                    ImageUpload.Visible = false;
                    Upload.Visible = false;
                    Cancel.Visible = false;
                }
            }
            else if (Request["Type"] == "BackAddress")
            {
                sql = "select case When BackAddressProof='' then '" + Session["CompWeb"] + "Images/no_photo.jpg' else BackAddressProof" + " end as ImageLnk1 from KycVerify  where Formno='" + Request["ID"] + "'";
                dt = new DataTable();
                //obj = new DAL();
                //dt = obj.GetData(sql);
                dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImageLnk1"].ToString();
                    LblNewPic.Visible = false;
                    LblPic.Visible = false;
                    LblUpdatePic.Visible = false;
                    ImageUpload.Visible = false;
                    Upload.Visible = false;
                    Cancel.Visible = false;
                }
            }
            else if (Request["Type"] == "FrontAddress")
            {
                sql = "select case When AddrProof='' then '" + Session["CompWeb"] + "Images/no_photo.jpg' else AddrProof" + " end as ImageLnk1 from KycVerify  where Formno='" + Request["ID"] + "'";
                dt = new DataTable();
                //obj = new DAL();
                //dt = obj.GetData(sql);
                dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImageLnk1"].ToString();
                    LblNewPic.Visible = false;
                    LblPic.Visible = false;
                    LblUpdatePic.Visible = false;
                    ImageUpload.Visible = false;
                    Upload.Visible = false;
                    Cancel.Visible = false;
                }
            }
            else if (Request["Type"] == "BankProof")
            {
                sql = "select case When BankProof='' then '" + Session["CompWeb"] + "/Images/no_photo.jpg' else BankProof" + " end as ImageLnk1 from KycVerify  where Formno='" + Request["ID"] + "'";
                dt = new DataTable();
                //obj = new DAL();
                //dt = obj.GetData(sql);
                dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImageLnk1"].ToString();
                    LblNewPic.Visible = false;
                    LblPic.Visible = false;
                    LblUpdatePic.Visible = false;
                    ImageUpload.Visible = false;
                    Upload.Visible = false;
                    Cancel.Visible = false;
                }
            }
            else if (Request["Type"] == "Pancard")
            {
                sql = "select case When PanImg='' then '" + Session["CompWeb"] + "/Images/no_photo.jpg' else PanImg" + " end as ImageLnk1 from KycVerify  where Formno='" + Request["ID"] + "'";
                dt = new DataTable();
                //obj = new DAL();
                //dt = obj.GetData(sql);
                dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Image1.ImageUrl = dt.Rows[0]["ImageLnk1"].ToString();
                    LblNewPic.Visible = false;
                    LblPic.Visible = false;
                    LblUpdatePic.Visible = false;
                    ImageUpload.Visible = false;
                    Upload.Visible = false;
                    Cancel.Visible = false;
                }
            }
            else
            {
                Image1.ImageUrl = "ImgHandler.ashx?id=" + Request["ID"] + "&Type=" + Request["Type"];
                LblNewPic.Visible = true;
                LblPic.Visible = true;
                LblUpdatePic.Visible = true;
                ImageUpload.Visible = true;

                Upload.Visible = true;
                Cancel.Visible = true;
            }
        }
        else
        {
            Image1.ImageUrl = "ImgHandler.ashx?id=" + Request["ID"];
            LblNewPic.Visible = true;
            LblPic.Visible = true;
            LblUpdatePic.Visible = true;
            ImageUpload.Visible = true;
            Upload.Visible = true;
            Cancel.Visible = true;
        }
    }
    protected void Upload_Click(object sender, System.EventArgs e)
    {
        string type;
        type = Request["Type"];
        try
        {
            if (ImageUpload.PostedFile != null && ImageUpload.PostedFile.FileName != "")
            {
                string strExtension = System.IO.Path.GetExtension(ImageUpload.FileName);
                if ((strExtension.ToUpper() == ".JPG") | (strExtension.ToUpper() == ".GIF"))
                {
                    // Resize Image Before Uploading to DataBase
                    System.Drawing.Image imageToBeResized = System.Drawing.Image.FromStream(ImageUpload.PostedFile.InputStream);
                    int imageHeight = imageToBeResized.Height;
                    int imageWidth = imageToBeResized.Width;
                    int maxHeight = 200;
                    int maxWidth = 200;
                    imageHeight = (imageHeight * maxWidth) / Convert.ToInt32((double)imageWidth);
                    imageWidth = maxWidth;
                    if (imageHeight > maxHeight)
                    {
                        imageWidth = (imageWidth * maxHeight) / Convert.ToInt32((double)imageHeight);
                        imageHeight = maxHeight;
                    }
                    //Drawing.Bitmap bitmap = new DocumentFormat.OpenXml.Office.Drawing.Drawing.Bitmap(imageToBeResized, imageWidth, imageHeight);
                    System.IO.MemoryStream stream = new MemoryStream();
                    //bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    stream.Position = 0;
                    byte[] image = new byte[stream.Length + 1];
                    stream.Read(image, 0, image.Length);
                    // Create SQL Connection 
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = (Application("Connect"));
                    con.Open();
                    // Create SQL Command 
                    SqlCommand cmd = new SqlCommand();
                    string sql;
                    if (type == "Address")
                        sql = "Update M_MemberMaster set AddrssProof = @Image,DtAddrssProof=GetDate() where FormNo= '" + FormNo + "'";
                    else if (type == "Identity")
                        sql = "Update M_MemberMaster set IdentityProof = @Image where FormNo= '" + FormNo + "'";
                    else
                        sql = "Update M_MemberMaster set memPic = @Image where FormNo= '" + FormNo + "'";

                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    SqlParameter UploadedImage = new SqlParameter("@Image", SqlDbType.Image, image.Length);
                    UploadedImage.Value = image;
                    cmd.Parameters.Add(UploadedImage);
                    // con.Open()
                    int result = cmd.ExecuteNonQuery();
                    con.Close();
                    if (result > 0)
                    {
                    }
                }
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            //MsgBox(ex.Message);
        }
        LblPic.Visible = false;
        ImageUpload.Visible = false;
        LblNewPic.Visible = false;
        Upload.Visible = false;
        LblUpdatePic.Visible = true;
        // 'LnkPhoto.Visible = True
        Cancel.Visible = false;
    }

    private string Application(string v)
    {
        throw new NotImplementedException();
    }

    // Protected Sub LnkPhoto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkPhoto.Click
    // tblImage.Visible = True
    // Image1.Visible = False
    // 'LnkPhoto.Visible = False
    // ImageUpload.Visible = True
    // Upload.Visible = True
    // Cancel.Visible = True
    // End Sub

    protected void Cancel_Click(object sender, System.EventArgs e)
    {
        ImageUpload.Visible = false;
        Upload.Visible = false;
        // LnkPhoto.Visible = True
        Cancel.Visible = false;
    }
}
