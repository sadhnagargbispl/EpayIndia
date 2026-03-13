<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ApprovePaymentReqs.aspx.cs" Inherits="ApprovePaymentReqs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/Pagging.css" rel="stylesheet" />
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
                width: 45%; /* Make buttons full-width on mobile devices */
            }

            .button-space {
                margin-right: 0; /* Remove right margin for buttons on mobile devices */
            }
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Approve Payment Request </h3>
                            </div>
                             <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                            <div class="card-body">
                                <div class="row">
                                    <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px; color: Red"></asp:Label>
                                    <div class="col-md-2">
                                        Member ID :
                                        <asp:TextBox ID="TxtMemID" runat="server" class="form-control" Style="display: inline"> 
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        Payment Mode:
                                        <asp:DropDownList ID="ddlPayment" runat="server" class="form-control" Style="text-indent: 1px;">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        From Date :
                                   <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        From Date :
                                          <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>

                                    <div class="col-md-4">
                                        Status :
                                        <asp:RadioButtonList ID="RbtStatus" runat="server" RepeatDirection="Horizontal" CssClass="form-control" RepeatLayout="Flow">
                                            <asp:ListItem Text="All" Value="Z" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Approve" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="N"></asp:ListItem>
                                        </asp:RadioButtonList>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Button ID="BtnSearch" runat="server" class="btn btn-primary" Text="Search" OnClick="BtnSearch_Click" />
                                        <asp:Button ID="btnApproove" runat="server" class="btn btn-primary" Text="Approve" Visible="false" OnClick="btnApproove_Click" />
                                        <asp:Button ID="BtnRejects" runat="server" class="btn btn-primary" Text="Reject" Visible="false" OnClick="BtnRejects_Click" />
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                                </div>
                                <div id="DivRemark" runat="server" visible="false">
                                    <table id="TblRemark" runat="server" align="center" style="background-color: #5056bc; color: #ffffff; border-color: Black; border-width: 1px; margin-top: -10px;">
                                        <tr>
                                            <td align="left">
                                                <strong>Remark</strong>*
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtARemark" runat="server" TextMode="MultiLine" Style="color: Black"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="btnApprove" runat="server" class="btn btn-primary" Text="Approve"
                                                    Visible="false" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" OnClick="btnApprove_Click" />
                                                <asp:Button ID="BtnReject" runat="server" Text="Reject " class="btn btn-primary"
                                                    Visible="false" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" OnClick="btnReject_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Label ID="lblMsg" runat="server" Font-Size="13px" Visible="False"></asp:Label>
                                <div class="col-md-12">
                                    <div id="gvContainers" runat="server">
                                        <div class="table-responsive">
                                            <style>
                                                a {
                                                    color: white;
                                                    text-decoration: none;
                                                    background-color: transparent;
                                                }
                                            </style>
                                            <div style="margin-top: 20px; margin-bottom: 20px;">

                                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                                <br />
                                                <asp:Label ID="LblCredit" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                                <asp:Label ID="lblDebit" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                                <asp:Label ID="LblBalance" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray; margin-right: 10px"></asp:Label>
                                            </div>
                                            <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px; margin-bottom: 25px;">
                                                <asp:GridView ID="GvData" Width="100%" runat="server" AllowPaging="false"
                                                    GridLines="Both" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                    EmptyDataText="No data to display." AutoGenerateColumns="false"
                                                    AllowSorting="true">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="CheckAll">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# Convert.ToBoolean(Eval("EnableStatus")) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                                                                <asp:Label ID="LblReqNo" runat="server" Text='<%# Eval("ReqNo") %>'></asp:Label>
                                                                <asp:Label ID="LblIdNo" runat="server" Text='<%# Eval("IdNo") %>'></asp:Label>
                                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                                <asp:Label ID="LblPaymode" runat="server" Text='<%# Eval("PayMode") %>'></asp:Label>
                                                                <asp:Label ID="LblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="IDNo" HeaderText="Member No." />
                                                        <asp:BoundField DataField="MemberName" HeaderText="Member Name" />
                                                <%--        <asp:BoundField DataField="MobileNo" HeaderText="Mobile No" />--%>
                                                        <asp:BoundField DataField="ReqDate" HeaderText="Request Date"></asp:BoundField>
                                                        <asp:BoundField DataField="PayMode" HeaderText="Payment Mode"></asp:BoundField>
                                                        <asp:BoundField DataField="ChqNo" HeaderText="Transaction No"></asp:BoundField>
                                                        <asp:BoundField DataField="ChequeDate" HeaderText="Transaction Date"></asp:BoundField>
                                                        <asp:BoundField DataField="Amount" HeaderText="Amount"></asp:BoundField>
                                                        <asp:BoundField DataField="Remarks" HeaderText="Member Remark"></asp:BoundField>
                                                        <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                                                        <asp:BoundField DataField="ApproveRemark" HeaderText="User  Remark" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <a href='<%# "Img.aspx?&type=Payment&ID=" + Eval("Reqno") %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe', width: 785, height: 580, marginTop: 0 })">
                                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ScannedFile") %>' Height="80px" Width="80px" Visible='<%# Convert.ToBoolean(Eval("ScannedFileStatus")) %>' />
                                                                </a>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
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
