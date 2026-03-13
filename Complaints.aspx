<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Complaints.aspx.cs" Inherits="Complaints" %>

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
                                <h3 class="card-title">Complaint  </h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label2" runat="server" Text="Search By: "></asp:Label>
                                        <asp:DropDownList ID="ddlGroupFields" runat="server" class="form-control">
                                            <asp:ListItem Value="None" Selected="True">--Search By--</asp:ListItem>
                                            <asp:ListItem Value="IDNo">ID No.</asp:ListItem>
                                            <asp:ListItem Value="CType">Complaint Type</asp:ListItem>
                                            <asp:ListItem Value="Complaint">Complaint</asp:ListItem>
                                            <asp:ListItem Value="Solution">Solution</asp:ListItem>
                                            <asp:ListItem>ShowAll</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        Name:
                                        <asp:TextBox ID="txtSearch" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2" style="display: none">
                                        <asp:CheckBox ID="ChkDate" runat="server" Text="Complaint Date :" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        Group :
                                        <asp:DropDownList ID="DDlGroup" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        Status :
                                        <asp:RadioButtonList ID="RbReplied" runat="server" CssClass="form-control" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                            <asp:ListItem Value="K" Text="All" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Y" Text="Replied"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="Not yet Replied"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2" style="display: none">
                                        <asp:Label ID="LblGroup" runat="server" Text="Search Group Wise">
                                        </asp:Label>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:Button ID="BtnSubmit" runat="server" class="btn btn-primary" Text="Search" OnClick="btnShowRecord_Click" />
                                        <%-- <asp:Button ID="btnshowall" runat="server" class="btn btn-primary" Text="Show All" />--%>
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" OnClick="btnExport_Click" />
                                        <asp:Button ID="btnShowRecord" runat="server" CssClass="btn-btn primary " Text="View All"
                                            Visible="false" />

                                    </div>
                                    <div class="table table-responsive">
                                        <br />
                                        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" RowStyle-Height="25px"
                                            GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            ShowHeader="true" PageSize="20" EmptyDataText="No data to display.">
                                            <Columns>
                                                <asp:TemplateField HeaderText="ComplaintID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblID" runat="server" Text='<%# Eval("CID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CID" HeaderText="Compl.ID" />
                                                <asp:BoundField DataField="IDNo" HeaderText="ID No." ControlStyle-Width="50px" />
                                                <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                                                <asp:BoundField DataField="CDate" HeaderText="Complaint Date" />
                                                <asp:BoundField DataField="CType" HeaderText="Complaint Type" />
                                                <asp:BoundField DataField="Complaint" HeaderText="Complaint" />
                                                <asp:BoundField DataField="SDate" HeaderText="Reply Date" />
                                                <asp:BoundField DataField="Solution" HeaderText="Previous Reply" />
                                                <asp:TemplateField HeaderText="Reply" HeaderStyle-Width="55px" ItemStyle-HorizontalAlign="Center"
                                                    ControlStyle-CssClass="btn-group ">
                                                    <ItemTemplate>
                                                        <a class="btn btn-primary" href='<%# "Reply.aspx?CId=" + Eval("VCId")  %>'
                                                            onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 470,height: 450,marginTop : 0 } )">
                                                            <i class="icon-check-alt2"></i>
                                                            <asp:Label ID="LBModify" runat="server" Text="Reply" />
                                                        </a>
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
            </div>
        </section>
    </div>


</asp:Content>

