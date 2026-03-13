<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewPairIncentive.aspx.cs" Inherits="ViewPairIncentive" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <!-- bootstrap theme -->
    <link href="css/bootstrap-theme.css" rel="stylesheet">
    <!--external css-->
    <!-- font icon -->
    <link href="css/Grid.css" rel="Stylesheet" type="text/css" />
    <link href="css/elegant-icons-style.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <!-- Custom styles -->
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/style-responsive.css" rel="stylesheet" />
    <%--<link href="adminassets/main.css" rel="stylesheet" type="text/css" />--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="LblNo" runat="server" ForeColor="Black" Font-Size="6px"></asp:Label>
    </div>
    <div style="margin-bottom: 10px; padding-right: 20px; padding-left: 20px; padding-top: 20px">
        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true"
            GridLines="Both" AllowPaging="true" CssClass="table table-bordered" HeaderStyle-BackColor="#000000" HeaderStyle-ForeColor="White" HeaderStyle-HorizontalAlign ="Left" 
             ShowHeader="true" PageSize="20" EmptyDataText="No data to display." Width="100%"  OnPageIndexChanging="RptDirects_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText= "S.No.">
                    <ItemTemplate>
                        <%#Container.DataItemIndex + 1%>.
                    </ItemTemplate>
                </asp:TemplateField>
               
                
                
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
    </form>
</body>
</html>
