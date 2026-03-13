<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CoinMaster.aspx.cs" Inherits="CoinMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <%--   <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">--%>
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
                                <h3 class="card-title">Stack Mode ROI Slab
                                </h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Start Date: "></asp:Label>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                            Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                            ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label ID="lblEndDate" runat="server" Text="End Date : "></asp:Label>
                                        <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                            Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                            ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                    </div>


                                    <div class="col-md-4">
                                        <br />
                                        <asp:Button ID="Btnsearch" runat="server" class="btn btn-primary" Text="Search" OnClick="Btnsearch_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary" Text="Export To Excel" OnClick="btnExport_Click" />
                                        <asp:Button ID="BtnAdd" runat="server" class="btn btn-primary" Text="Add ROI Slab" OnClick="BtnAdd_Click" />

                                        <asp:Button ID="btnShowRecord" runat="server" class="btn btn-primary" Text="View All"
                                            Visible="false" />
                                    </div>
                                </div> 
                                       <div align="center">
                                           <br />
                                    <div style="margin-bottom: 20px">


                                        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered"
                                            HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging">

                                            <Columns>
                                                <asp:TemplateField HeaderText="GrpID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("PID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CoinRate" HeaderText="Profit" ControlStyle-Width="50px"
                                                    SortExpression="Coinrate" />
                                                 <asp:BoundField DataField="Month" HeaderText="Month" ControlStyle-Width="50px" SortExpression="Month" />
                                                <asp:BoundField DataField="CoinDate" HeaderText="Date" ControlStyle-Width="50px"
                                                    SortExpression="Coindate" />
                                                  
                                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblStatus" runat="server" Text='<%# Eval("Status") %>' class='<%# Eval("StatusClass") %>'></asp:Label>
                                                    </ItemTemplate>
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

