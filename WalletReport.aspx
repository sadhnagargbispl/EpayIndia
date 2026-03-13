<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WalletReport.aspx.cs" Inherits="WalletReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/Pagging.css" rel="stylesheet" />
    <style>
        a {
            color: white;
            text-decoration: none;
            background-color: transparent;
        }
    </style>
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
                                <h3 class="card-title">Wallet Report </h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        Member ID :
                                        <asp:TextBox ID="txtMemberId" runat="server" class="form-control" Style="display: inline">
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        StartDate :
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        EndDate:
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        PageSize:
                     
                                        <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                            <asp:ListItem Text="10" Value="10" />
                                            <asp:ListItem Text="20" Value="20" />
                                            <asp:ListItem Text="50" Value="50" />
                                            <asp:ListItem Text="100" Value="100" />
                                            <asp:ListItem Text="200" Value="200" />
                                            <asp:ListItem Text="300" Value="300" />
                                            <asp:ListItem Text="400" Value="400" />
                                            <asp:ListItem Text="500" Value="500" />
                                            <asp:ListItem Text="1000" Value="1000" />
                                            <asp:ListItem Text="2000" Value="2000" />
                                        </asp:DropDownList>

                                    </div>
                                    <div class="col-3">
                                        <br />
                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary"
                                            Text="Export To Excel" OnClick="btnExport_Click" />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px; color: Red"></asp:Label>
                                </div>

                                <div class="col-md-12">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                                </div>
                                <div class="col-md-12">
                                    <div id="gvContainers" runat="server">
                                        <div class="table-responsive">

                                            <div style="margin-top: 20px; margin-bottom: 20px;">

                                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                                <br />
                                                <asp:Label ID="LblCredit" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                                <asp:Label ID="lblDebit" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                                <asp:Label ID="LblBalance" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                            </div>
                                            <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px; margin-bottom: 25px;">
                                                <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="false"
                                                    GridLines="Both" class="table table-bordered" HeaderStyle-ForeColor="#5056bc" HeaderStyle-CssClass="bg-primary"
                                                    EmptyDataText="No data to display." AutoGenerateColumns="true"
                                                    AllowSorting="true" OnPageIndexChanging="GvData_PageIndexChanging">

                                                    <Columns>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" />
                                                    <PagerStyle CssClass="pagination-ys" />
                                                </asp:GridView>

                                            </div>

                                        </div>
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

