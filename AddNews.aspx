<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddNews.aspx.cs" Inherits="AddNews" %>

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
                    <!-- left column -->
                    <div class="col-md-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Add News Master</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div align="center">
                                            <span id="lblt" class="text-danger"></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group" style="padding-top: 1%; display: none;">
                                        Type :
                                            <asp:DropDownList ID="DDLType" CssClass="DDl " runat="server" AutoPostBack="true"
                                                Width="350px">
                                                <asp:ListItem Text="News" Value="N" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                    </div>


                                    <div class="form-group">
                                        Designation :
                                        <asp:DropDownList ID="DDlCategory" CssClass="form-control " runat="server">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="form-group">
                                        Heading :
                                     <asp:TextBox CssClass="form-control" ID="txtHeading" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtHeading"
                                            runat="server" ValidationGroup="Save" ForeColor="OrangeRed" ErrorMessage="Please Enter Heading.!"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group">
                                        Detail :
                                     <asp:TextBox CssClass="form-control" ID="txtDetail" runat="server" TextMode="MultiLine"
                                         Height="300px"></asp:TextBox>
                                        <AjaxToolkit:HtmlEditorExtender ID="HtmlEditorExtender1" runat="server" TargetControlID="txtDetail"
                                            EnableSanitization="false">
                                        </AjaxToolkit:HtmlEditorExtender>
                                    </div>
                                    <div class="form-group">
                                        From Date :
                                    <asp:TextBox CssClass="form-control" ID="txtFrmDate" runat="server"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFrmDate" Format="dd-MMM-yyyy"
                                            runat="server"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtFrmDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="form-group">
                                        To Date :
                                    <asp:TextBox CssClass="form-control" ID="txtToDate" runat="server"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="dd-MMM-yyyy"
                                            runat="server"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtToDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit" Display="Dynamic"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="form-group">
                                        Remarks :
                                    <asp:TextBox CssClass="form-control" ID="txtRemarks" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        Status :
                                     <asp:RadioButtonList ID="rdblist" runat="server" CellPadding="0" CellSpacing="10"
                                         RepeatDirection="Horizontal">
                                         <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                         <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                     </asp:RadioButtonList>
                                    </div>

                                    <div class="form-group">
                                        <asp:Button ID="BtnSave" CssClass="btn btn-primary" runat="server" Text="Save"
                                            ValidationGroup="Save" OnClick="BtnSave_Click" />
                                        <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                        <asp:TextBox ID="txtNewsID" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtAIdID" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group" id="pVenue" visible="false" runat="server">
                                    Venue :
                                    <asp:TextBox ID="txtVenue" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group" id="PState" visible="false" runat="server">
                                    State :
                                    <asp:DropDownList ID="DlState" runat="server" CssClass="DDl " Width="350px">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group" id="pcity" visible="false" runat="server">
                                    City :
                                    <asp:TextBox ID="TxtCity" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="form-group" id="pLeader" visible="false" runat="server">
                                    Leader Name :
                                     <asp:TextBox ID="txtLeader" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="form-group" id="pPhoneNo" visible="false" runat="server">
                                    PhoneNo:
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>





</asp:Content>

