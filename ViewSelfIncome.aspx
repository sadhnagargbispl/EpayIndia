<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewSelfIncome.aspx.cs" Inherits="ViewSelfIncome" %>

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
</head>
<body>
    <form id="form1" runat="server">

        <div>
            <asp:Label ID="LblNo" runat="server" ForeColor="Black" Font-Size="14px"></asp:Label>
        </div>

        <div style="margin-bottom: 20px; padding-right: 20px;">
            <%-- <div style="margin-bottom: 20px; padding-right: 20px">--%>
            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="True" RowStyle-Height="25px"
                GridLines="Both" AllowPaging="true" CssClass="table table-striped table-advance table-hover"
                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" Width="100%" ShowHeader="true"
                PageSize="50" EmptyDataText="No data to display.">
            </asp:GridView>
        </div>

        <div></div>
        <br />
        <br />
    </form>
</body>
</html>
