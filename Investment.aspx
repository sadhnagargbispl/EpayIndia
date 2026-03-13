<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Investment.aspx.cs" Inherits="Investment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <link href="css/style-responsive.css" rel="stylesheet" type="text/css" />
 <link href="css/Grid.css" rel="stylesheet" type="text/css" /> --%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <link href="css/Coin.css" rel="stylesheet" />
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Virtual Investment </h3>
                            </div>
                            <div class="card-body">


                                <div class="row">
                                    <div class="col-md-2">
                                        Member ID :
                             <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Start Date: "></asp:Label>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblEndDate" runat="server" Text="End Date : "></asp:Label>
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>

                                    <div class="col-md-4">
                                        <br />
                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" OnClick="BtnShow_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" OnClick="btnExport_Click" />

                                        <asp:Button ID="BtnAddNew" runat="server" class="btn btn-primary" Text="Add Virtual Investment" OnClick="BtnAddNew_Click" />

                                        <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All" Visible="false" OnClick="btnShowRecord_Click" />
                                    </div>

                                </div>

                                <div style="margin-left: 25px; margin-top: 20px; margin-bottom: 20px;" id="divContent" runat="server" visible="false">
                                    <asp:Label ID="lbl" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblView" runat="server" Font-Bold="true" Visible="false" Text="Click on <span style='color: red;Font-Size:13px;'>View All</span> Button to see the complete detail again."></asp:Label>
                                </div>
                                <div class="table table-bordered" style="overflow: scroll">
                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                    <asp:Label ID="lblError" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                    <asp:Label ID="lblinv" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                    <asp:Label ID="lblreinvetment" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                    <asp:Label ID="LvlVartualinvestment" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                    <asp:GridView ID="GvData" runat="server"
                                        AutoGenerateColumns="False"
                                        AllowPaging="true" DataKeyNames="Sessid"
                                        CssClass="table table-bordered"
                                        HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display."
                                        OnPageIndexChanging="GvData_PageIndexChanging" OnRowDataBound="GvData_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="BId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("BID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Date" HeaderText="Date" />
                                            <asp:BoundField DataField="Idno" HeaderText="IdNo" />
                                            <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                            <asp:BoundField DataField="BVType" HeaderText="Investment Type" />
                                            <asp:BoundField DataField="LegNo" HeaderText="LegNo" />
                                            <asp:BoundField DataField="PV" HeaderText="Investment" />
                                            <asp:BoundField DataField="Remark" HeaderText="Remarks" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />

                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center" Visible="false">
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

