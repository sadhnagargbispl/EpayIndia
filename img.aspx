<%@ Page Language="C#" AutoEventWireup="true" CodeFile="img.aspx.cs" Inherits="img" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <table style="height: 320px; width:403px">
    <tr>
    <td>
  <asp:Label ID="LblPic" runat="server" Text="Previous Image"></asp:Label>
  <asp:Label ID="LblUpdatePic" runat="server" Text="Updated Image" Visible="false" ></asp:Label>
    <%--Previous Profile--%>
    <br />
    <asp:Image runat="server" Width="100%" ID="Image1" />
    </td>
    
   <td>
   </td>
    
    
    
   
    
    <%--<table id="tblImage" runat="server" visible="false"  >--%>
  
                <td>
                <asp:Label ID="LblNewPic" runat="server" Text="Upload New Image"></asp:Label>
                <br />
              
                    <%--<asp:Image ID="DistImage" runat="server" Height="100px" Width="111px" 
                        Visible="true" />--%>
                <%--<<%--/td>--%>
            <%--</tr>            
            <tr>
                <td>--%>
                   
                 <asp:FileUpload ID="ImageUpload" CssClass="Btn" runat="server" Visible="True" />
                 <br />
               <%-- </td>
            </tr>
            <tr>
                <td>--%>
                    <asp:Button CssClass="Btn" ID="Upload" runat="server" Visible="True" Text="Upload" />
                  <asp:Button CssClass="Btn" ID="Cancel" runat="server" Visible="True" Text="Cancel" />
                  </td> 
                  </tr>
                  </table>
    </form>
</body>
</html>
