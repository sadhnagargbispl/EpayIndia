<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LogReport.aspx.cs" Inherits="LogReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Log Report</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date"></asp:Label>
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date"></asp:Label>
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lbluserTypeCap" runat="server" Text="User Type"></asp:Label>
                                        <asp:RadioButtonList ID="RbtUser" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                            AutoPostBack="true" CssClass="form-control">
                                            <asp:ListItem Text="Member" Value="M" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Admin" Value="A"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblMemberIDCap" runat="server" Text="Member ID"></asp:Label>
                                        <asp:TextBox ID="txtMember" runat="server" class="form-control"></asp:TextBox>
                                        <asp:DropDownList ID="DDlMember" runat="server" class="form-control" Visible="false">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-lg-8">

                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" OnClick="btnExport_Click " />
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px; color: Red"></asp:Label>
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                                </div>

                                <div class="col-md-12">

                                    <div class="table-responsive">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                <asp:Label ID="gvContainer" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>

                                                <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" AllowPaging="true" CssClass="table table-bordered"
                                                    HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GvData_PageIndexChanging">
                                                </asp:GridView>

                                                </div>
                                            
                                            </ContentTemplate>

                                        </asp:UpdatePanel>
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
