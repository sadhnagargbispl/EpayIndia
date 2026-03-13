<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BVpoint.aspx.cs" Inherits="BVpoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
 <link href="css/Grid.css" rel="stylesheet" type="text/css" /> --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="css/Coin.css" rel="stylesheet" />
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Virtual Pawer </h3>
                            </div>
                            <div class="card-body">
                                <div align="center">
                                    <div class="col-md-12">
                                        <br />
                                        <asp:Button ID="btnPrintCurrent" runat="server" Text="Print Current Page" class="btn btn-primary" OnClick="btnPrintCurrent_Click" />
                                        <asp:Button ID="btnPrintAll" runat="server" Text="Print All Pages" class="btn btn-primary" OnClick="btnPrintAll_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" OnClick="btnExport_Click" />
                                        
                                            <asp:Button ID="BtnAddNew" runat="server" class="btn btn-primary" Text="Add Virtual PV" OnClick="BtnAddNew_Click" />
   
                                         <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary"  Text="View All" Visible="false" OnClick="btnShowRecord_Click" />
                                        <%--<asp:Button ID="BtnAdvSearch" runat="server" CssClass="Btn" Text="Advanced Search" />--%>
                                    </div>
                                    <br />
                                    <br />

                                    <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;" id="divContent" runat="server" visible="false">
                                        <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                                        <br />
                                        <br />
                                        <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                                    </div>
                                    <div style="margin-bottom: 20px">
                                        <asp:GridView ID="GvData" runat="server"
                                            AutoGenerateColumns="False" 
                                            AllowPaging="true" DataKeyNames="Sessid"
                                            CssClass="table table-bordered"
                                            HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display."
                                            OnPageIndexChanging="GvData_PageIndexChanging" OnRowDataBound="GvData_RowDataBound" >
                                            <Columns>
                                                <asp:TemplateField HeaderText="BId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("BID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="BankCode" HeaderText="Bank Code" Visible="false"/>--%>
                                                <asp:BoundField DataField="Date" HeaderText="Date" />
                                                <asp:BoundField DataField="Idno" HeaderText="IdNo" />
                                                <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                                  <asp:BoundField DataField="BVType" HeaderText="PVType" />
                                                <asp:BoundField DataField="LegNo" HeaderText="LegNo" />
                                                <asp:BoundField DataField="PV" HeaderText="PV" />
                                                <asp:BoundField DataField="Remark" HeaderText="Remarks" />
                                                <asp:BoundField DataField="Status" HeaderText="Status" />

                                                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" Visible="false" >
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LBDelete" runat="server" Text="Delete" OnClick="DeleteGroup" ><i class="fa fa-close" style=" color:#d9534f; font-size :20px"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="55px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <br />
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

