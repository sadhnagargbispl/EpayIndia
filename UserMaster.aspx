<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserMaster.aspx.cs" Inherits="UserMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <style>
        /* Define a class for adding space between buttons */
        .button-space {
            margin-right: 10px; /* Adjust this value to set the desired space */
        }

        /* Define media query for mobile devices */
        @media only screen and (max-width: 500px) {
            /* Adjust button style for mobile view */
            .btn {
                margin-bottom: 5px; /* Add some space between buttons vertically */
                width: 40%; /* Make buttons full-width on mobile devices */
            }

            .button-space {
                margin-right: 0; /* Remove right margin for buttons on mobile devices */
            }
        }
    </style>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">New User Master </h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlSearchFields" runat="server" class="form-control">
                                            <asp:ListItem Selected="True" Value="ShowAll">--Search By--</asp:ListItem>
                                            <asp:ListItem Value="UserName">User Name</asp:ListItem>
                                            <asp:ListItem>Remarks</asp:ListItem>
                                            <asp:ListItem>Status</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtSearch" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlGroup" runat="server" class="form-control" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>

                                </div>
                                <div class="row">
                                    &nbsp;
                                </div>
                                <div class="row">
                                    <div class=" col-md-12">
                                        <asp:Button ID="BtnShowAll" runat="server" class="btn btn-primary" Text="Show" OnClick="BtnShowAll_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary"
                                            Text="Export To Excel" OnClick="btnExport_Click " />
                                        <asp:Button ID="BtnAdd" runat="server" class="btn btn-primary" Text="Add User" OnClick="BtnAdd_Click" />
                                        <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All"
                                            Visible="false" />
                                    </div>
                                </div>
                                <br />
                                <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;" id="DivView"
                                    runat="server" visible="false">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                                    <br />

                                </div>
                                <div style="margin-bottom: 20px; overflow: scroll;">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="GrpID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUserID" runat="server" Text='<%# Eval("UserId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                            <asp:BoundField DataField="Passw" HeaderText="Password" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                            <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%-- <a href='<%# "AddUser.aspx?UserId=" + HttpUtility.UrlEncode(Crypto.Encrypt(Eval("UserId").ToString())) %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 330,marginTop : 0 } )">
                                                        <i class="fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px"></i>
                                                        <asp:Label ID="Label1" runat="server" /></i>
                                                                <asp:Label ID="LBModify" runat="server" />
                                                    </a>--%>

                                                    <a href='<%# "AddUser.aspx?UserId=" + HttpUtility.UrlEncode(Crypto.Encrypt(Eval("UserId").ToString())) %>'>
                                                        <i class="fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px"></i>
                                                        <asp:Label ID="Label1" runat="server" /></i>
                                                        <asp:Label ID="LBModify" runat="server" />
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>

                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup">
                                                    <i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle Width="85px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

