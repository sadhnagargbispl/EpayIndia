<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Downline.aspx.cs" Inherits="Downline" %>
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
                                <h3 class="card-title">Member Downline Report
                                </h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <label for="exampleInputEmail1">Enter Member ID :</label>
                                        <asp:TextBox class="form-control" ID="txtMemberId" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId" runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:CheckBox ID="ChkKit" runat="server" Text="Choose Package :" TextAlign="Left" />
                                        <asp:DropDownList ID="CmbKit" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>


                                    <div class="col-md-3">
                                        <label for="exampleInputEmail1">Choose Start Date:</label>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>


                                    </div>
                                    <div class="col-md-3">
                                        <label for="exampleInputEmail1">Choose End Date:</label>
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>


                                    <div class="col-md-3" style="display: none">
                                        <asp:DropDownList ID="DDLSession" runat="server" class="form-control" Visible="false"></asp:DropDownList>

                                        <asp:Label ID="LblSDate" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LblTDate" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LblSTDate" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="LblToDate" runat="server" Visible="false"></asp:Label>

                                    </div>
                                    <br />
                                    <div class="col-md-3">

                                        <asp:RadioButtonList ID="rbleg" runat="server" RepeatDirection="Horizontal" CssClass="form-control" RepeatLayout="Flow">
                                            <asp:ListItem Selected="True" Value="0" Text="Both"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Left"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Right"></asp:ListItem>
                                        </asp:RadioButtonList>

                                    </div>
                                    <div class="col-md-3">

                                        <asp:RadioButtonList ID="RbtSearch" runat="server" RepeatDirection="Horizontal" CssClass="form-control" RepeatLayout="Flow">
                                            <asp:ListItem Text="Date wise" Value="D" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Session Wise" Value="S"></asp:ListItem>
                                        </asp:RadioButtonList>

                                    </div>
                                    

                                    <div class="col-md-3">

                                        <asp:DropDownList ID="DDlDate" runat="server" class="form-control" >
                                            <asp:ListItem Text="Joining" Value="J"></asp:ListItem>
                                            <asp:ListItem Text="Activation" Value="A"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Button ID="btnShowDownline" class="btn btn-primary" runat="server" Text="Show Downline Report" ValidationGroup="Save" OnClick="btnShowDownline_Click" />
                                    </div>
                                    <div class="col-md-9">
                                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label></div>


                                </div>
                                <div id="divMemDownline" runat="server" visible="false" style="overflow: auto; width: 95%; margin-left: 15px; height: 500px; margin-bottom: 20px;">

                                    <br />
                                    <br />
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tbody>
                                            <tr style="width: 700px" id="TrLeftHeading" runat="server">
                                                <td class="form-heading" colspan="4" style="width: 700px">
                                                    <h4>Downline Left</h4>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Leftactive" Font-Bold="true" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LeftDeactive" Font-Bold="true" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="tr-1">
                                                <td align="right" valign="middle" colspan="4">
                                                    <div id="DivSideA" runat="server">
                                                        <table border="0" width="100%" cellspacing="2" cellpadding="0">

                                                            <tr>
                                                                <td align="center" valign="top">

                                                                    <div id="gvContainer" runat="server" style="overflow: auto; width: 100%; margin-top: 10px; margin-bottom: 10px;">
                                                                        <asp:GridView ID="GvData" runat="server" class="table table-bordered" RowStyle-Height="25px"
                                                                            CellPadding="3" HorizontalAlign="Center" AutoGenerateColumns="False" AllowPaging="True"
                                                                            Width="100%" ShowHeader="true" PageSize="5" EmptyDataText="No data to display." GridLines="Both"
                                                                            HeaderStyle-CssClass="bg-primary" OnPageIndexChanging="GvData_PageIndexChanging" >
                                                                            <%--OnPageIndexChanging="GrdDirects1_PageIndexChanging"--%>
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %>. </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="IDNO" HeaderText="ID No">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="MemName" HeaderText="Member Name">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="RefFormno" HeaderText="Sponsor ID">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="referalName" HeaderText="Sponsor Name">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="DateofJoining" HeaderText="Date Of Joining">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Topup" HeaderText="Topup Date">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="KitName" HeaderText="Package">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="KitAmount" HeaderText="Pkg. MRP"></asp:BoundField>
                                                                                <asp:BoundField DataField="BV" HeaderText="PV"></asp:BoundField>
                                                                            </Columns>
                                                                            <%--  <PagerStyle Mode="NumericPages" CssClass="PagerStyle"></PagerStyle>--%>
                                                                        </asp:GridView>

                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top">
                                                                    <asp:Button ID="BtnExportA" runat="server" Text="Export to Excel" class="btn btn-primary" Style="margin-left: 400px;" OnClick="BtnExportA_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 2px;" colspan="4"></td>
                                            </tr>
                                            <tr id="trRightHeading" runat="server">
                                                <td class="form-heading" colspan="4" style="width: 700px">
                                                    <h4>Downline Right</h4>
                                                </td>

                                            </tr>
                                            <tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="RActive" Font-Bold="true" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="RDeactive" Font-Bold="true" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </tr>
                                            <tr class="tr-1">
                                                <td align="right" valign="middle" colspan="4">
                                                    <div id="DivSideB" runat="server">
                                                        <table border="0" width="100%" cellspacing="2" cellpadding="0">

                                                            <tr>
                                                                <td align="center" valign="top">

                                                                    <div id="Div1" runat="server" style="overflow: auto; width: 100%; margin-top: 10px; margin-bottom: 10px;">
                                                                        <asp:GridView ID="GrdDirects2" runat="server" class="table table-bordered" RowStyle-Height="25px"
                                                                            CellPadding="3" HorizontalAlign="Center" AutoGenerateColumns="False" AllowPaging="True"
                                                                            Width="100%" ShowHeader="true" PageSize="5" EmptyDataText="No data to display." GridLines="Both"
                                                                            HeaderStyle-CssClass="bg-primary" OnPageIndexChanging="GrdDirects2_PageIndexChanging">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %>. </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="IDNO" HeaderText="ID No">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="MemName" HeaderText="Member Name">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="RefFormno" HeaderText="Sponsor ID">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="referalName" HeaderText="Sponsor Name">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="DateofJoining" HeaderText="Date Of Joining">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Topup" HeaderText="Topup Date">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="KitName" HeaderText="Package">
                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="KitAmount" HeaderText="Pkg. MRP"></asp:BoundField>
                                                                                <asp:BoundField DataField="BV" HeaderText="PV"></asp:BoundField>
                                                                            </Columns>
                                                                            <%--<PagerStyle Mode="NumericPages" CssClass="PagerStyle"></PagerStyle>--%>
                                                                        </asp:GridView>

                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top">
                                                                    <asp:Button ID="BtnExportB" runat="server" Text="Export to Excel" class="btn btn-primary" Style="margin-left: 400px;" OnClick="BtnExportB_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
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

