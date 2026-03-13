<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PackageMaster.aspx.cs" Inherits="PackageMaster" %>

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
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Package Master</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2" style="padding: 10px;">
                                        <asp:DropDownList ID="ddlSearchFields" runat="server" class="form-control">
                                            <asp:ListItem Selected="True" Value="ShowAll">--Search By--</asp:ListItem>
                                            <asp:ListItem Value="KitName">Kit Name</asp:ListItem>
                                            <asp:ListItem>Remarks</asp:ListItem>
                                            <asp:ListItem>Status</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2" style="padding: 10px;">
                                        <asp:TextBox ID="txtSearch" class="form-control" runat="server"></asp:TextBox>
                                    </div>

                                    <div class=" col-md-12" style="padding: 10px;">
                                        <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="Show"
                                            OnClick="btnShowRecord_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary"
                                            Text="Export To Excel" OnClick="btnExport_Click " />
                                        <asp:Button ID="AddKit" runat="server" class="btn btn-primary" Text="Create New Kit" OnClick="BtnAdd_Click" />

                                    </div>
                                </div>

                                <br />
                                <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;" id="DivView"
                                    runat="server" visible="false">
                                    <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                                </div>
                                <div class="table-responsive" style="margin-bottom: 20px">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered"
                                        HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging">

                                        <Columns>
                                            <asp:TemplateField HeaderText="KitId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblNewsID" runat="server" Text='<%# Eval("KitId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S.No.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>.
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="KitName" HeaderText="KitName" />
                                            <asp:BoundField DataField="JoinAmount" HeaderText="JoinAmount" />
                                            <asp:BoundField DataField="KitAmount" HeaderText="KitAmount" />
                                            <asp:BoundField DataField="KitUnit" HeaderText="KitUnit" />
                                            <asp:BoundField DataField="SerialStart" HeaderText="SerialStart" />
                                            <asp:BoundField DataField="RefIncome" HeaderText="RefIncome" />
                                            <asp:BoundField DataField="PoolIncome" HeaderText="PoolIncome" />
                                            <asp:BoundField DataField="SpillIncome" HeaderText="SpillIncome" />
                                            <asp:BoundField DataField="BinaryIncome" HeaderText="BinaryIncome" />
                                            <asp:BoundField DataField="BV" HeaderText="BV" />
                                            <asp:BoundField DataField="PV" HeaderText="PV" />
                                            <asp:BoundField DataField="RP" HeaderText="RP" />
                                            <asp:BoundField DataField="Capping" HeaderText="Capping" />
                                            <asp:BoundField DataField="TopUpSeq" HeaderText="Top Up Sequence" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:TemplateField HeaderText="Modify" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btn-group">
                                                <ItemTemplate>
                                                    <a href='<%# "AddPackage.aspx?KitId=" + HttpUtility.UrlEncode(Crypto.Encrypt(Eval("VKitId").ToString())) %>'>
                                                        <i class="fa fa-edit" style="color: #337ab7; font-size: 20px; margin-right: 20px"></i>
                                                        <asp:Label ID="Label1" runat="server" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup"><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle Width="55px"></HeaderStyle>
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

