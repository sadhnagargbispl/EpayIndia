<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewdirectIncome.aspx.cs" Inherits="ViewdirectIncome" %>

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
        <div style="margin-bottom: 20px; padding-right: 20px">
            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                GridLines="Both" AllowPaging="true" CssClass="table table-striped table-advance table-hover"
                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" Width="100%" ShowHeader="true"
                PageSize="50" EmptyDataText="No data to display.">
                <Columns>
                    <asp:TemplateField HeaderText="S.No.">
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
