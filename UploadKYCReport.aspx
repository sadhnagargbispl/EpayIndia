<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UploadKYCReport.aspx.cs" Inherits="UploadKYCReport" %>

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
                                <h3 class="card-title">KYC Report</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        Member ID :
           <asp:TextBox ID="TxtMember" runat="server" Class="form-control"></asp:TextBox>
                                    </div>


                                    <div class="col-md-3">
                                        KYC Status Select :
            <asp:DropDownList ID="DDlSerchBy" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                RepeatLayout="Flow" class="form-control" OnSelectedIndexChanged="DDlSerchBy_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Text="Verify " Value="Y" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Rejected " Value="R"></asp:ListItem>
                <asp:ListItem Text="Pending " Value="P"></asp:ListItem>
                <asp:ListItem Text="Not Uploaded " Value="N"></asp:ListItem>
            </asp:DropDownList>
                                    </div>
                                   <%-- <div runat="server" id="DivDate" visible="true">--%>
                                        <div class="col-md-3" runat="server" id="DivStartDate">
                                            Start Date : 
           <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                                Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                                ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>
                                        <div class="col-md-3" runat="server" id="DivENDDate">
                                            End Date : 
           <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                                Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                                ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>
                                  <%--  </div>--%>

                                    <div class="col-md-3">
                                        KYC Summary Select :
                                        <asp:DropDownList ID="RbtSummary" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" class="form-control" OnSelectedIndexChanged="RbtSummary_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Summary" Value="S" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="History" Value="D"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <br />
                                        <asp:Button ID="btnSearch" runat="server" class="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" OnClick="btnExport_Click" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="table-responsive makeitresponsivegrid">
                                        <div align="center">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px; color: Red"></asp:Label>
                                            </div>
                                        </div>
                                        <div style="margin-top: 20px; margin-bottom: 20px;">
                                            <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Note : Contents getting displayed with red background are deactivated."
                                                Visible="false"></asp:Label>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 14px; color: Gray"></asp:Label>
                                        </div>

                                    </div>
                                </div>
                                <div id="gvContainer" runat="server" style="overflow: scroll; margin-top: 25px; margin-left: 25px; margin-bottom: 25px;">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" class="table table-bordered" PagerStyle-CssClass="PagerStyle"
                                        AlternatingRowStyle-CssClass="alt" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                        PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GvData_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNo.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                    </asp:GridView>
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" class="table table-bordered" PagerStyle-CssClass="PagerStyle"
                                        AlternatingRowStyle-CssClass="alt" HeaderStyle-CssClass="bg-primary" ShowHeader="true"
                                        PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GridView1_PageIndexChanging">
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNo.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Idno" HeaderText="Idno" />
                                            <asp:BoundField DataField="Name" HeaderText="Name" />
                                            <asp:BoundField DataField="Type" HeaderText="Type" />
                                            <asp:TemplateField HeaderText="Image">
                                                <ItemTemplate>
                                                    <img src='<%# Eval("Imgpath") %>' width="100" height="100" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Image">
                                                <ItemTemplate>
                                                    <img src='<%# Eval("Imgpath1") %>' width="100" height="100" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                            <asp:BoundField DataField="RejectReason" HeaderText="Reject Reason" />
                                            <asp:BoundField DataField="RejectRemark" HeaderText="Reject Remark" />
                                            <asp:BoundField DataField="Date" HeaderText="Date" />
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
