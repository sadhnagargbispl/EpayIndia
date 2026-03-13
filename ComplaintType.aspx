<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ComplaintType.aspx.cs" Inherits="ComplaintType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Complain Type Master</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2" style="padding: 10px;">
                                        <asp:DropDownList ID="ddlComplain" runat="server" class="form-control">
                                            <asp:ListItem Selected="True" Value="ShowAll">--Search By--</asp:ListItem>
                                            <asp:ListItem Value="CType">Complaint Name</asp:ListItem>
                                            <asp:ListItem>Remarks</asp:ListItem>
                                            <asp:ListItem>Status</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2" style="padding: 10px;">
                                        <asp:TextBox ID="txtSearch" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class=" col-md-4" style="padding: 10px;">

                                        <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="Show"
                                            OnClick="btnShowRecord_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary"
                                            Text="Export To Excel" OnClick="btnExport_Click " />
                                        <asp:Button ID="BtnAdd" runat="server" class="btn btn-primary" Text="Add Complaint Type" OnClick="BtnAdd_Click" />
                                    </div>
                                    <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;" id="DivView"
                                        runat="server" visible="false">
                                        <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                                        <br />
                                        <br />
                                        <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class=" col-md-12">
                                        &nbsp;
                                    </div>
                                    <div class="table table-responsive">
                                        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" RowStyle-Height="25px"
                                            GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" PageSize="20" EmptyDataText="No data to display.">
                                            <Columns>
                                                <asp:TemplateField HeaderText="GrpID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("CTypeID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CType" HeaderText="Complaint Name" ControlStyle-Width="50px" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                                <asp:BoundField DataField="Status" HeaderText="Status" />

                                                <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <a href='<%# "AddCType.aspx?Type=" + HttpUtility.UrlEncode(Crypto.Encrypt(Eval("VCTypeID").ToString())) %>'>
                                                            <i class="fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px"></i>
                                                            <asp:Label ID="Label1" runat="server" />
                                                            </i>
                                                         <asp:Label ID="LBModify" runat="server" />
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="85px" ItemStyle-HorizontalAlign="Center">
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
            </div>
        </section>
    </div>


</asp:Content>

