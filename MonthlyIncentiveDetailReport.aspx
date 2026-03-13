<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MonthlyIncentiveDetailReport.aspx.cs" Inherits="MonthlyIncentiveDetailReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        function openPopup(element) {
            var url = element.href;
            hs.htmlExpand(element, {
                objectType: 'iframe',
                width: 720,
                height: 450,
                marginTop: 0
            });
            return false;
        }
    </script>


    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Monthly Payout Report</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-4">
                                        Member ID :
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        Session Wise :
                            
                                        <asp:DropDownList ID="DDlFromDate" runat="server" class="form-control" Style="text-indent: 1px;">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-md-2" style="display: none;">
                                        PageSize:
                                       <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged" AutoPostBack="true">
                                           <asp:ListItem Text="10" Value="10" />
                                           <asp:ListItem Text="20" Value="20" />
                                           <asp:ListItem Text="50" Value="50" />
                                           <asp:ListItem Text="100" Value="100" />
                                           <asp:ListItem Text="200" Value="200" />
                                           <asp:ListItem Text="300" Value="300" />
                                           <asp:ListItem Text="400" Value="400" />
                                           <asp:ListItem Text="500" Value="500" />
                                           <asp:ListItem Text="600" Value="600" />
                                           <asp:ListItem Text="2000" Value="2000" />
                                       </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <br />

                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" OnClick="BtnShow_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary"
                                            Text="Export To Excel" OnClick="btnExport_Click " />
                                    </div>
                                </div>

                                <div id="doublescroll" class="col-md-12">
                                    <p>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div id="gvContainer" runat="server" class="table table-bordered" style="overflow: scroll">
                                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lblinv" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>

                                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered"
                                                        HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GvData_PageIndexChanging">

                                                        <Columns>
                                                            <asp:BoundField DataField="SNo" HeaderText="SNo." SortExpression="SNo" />
                                                            <asp:TemplateField HeaderText="Payout Date" SortExpression="payoutdate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="StartDate" runat="server" Text='<%# Eval("payoutdate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Member ID" SortExpression="Member ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblMemberId" runat="server" Text='<%# Eval("IDno") %>'></asp:Label><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Member Name" SortExpression="Member Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblMemberName" runat="server" Text='<%# Eval("mem_Name") %>'></asp:Label><br />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="My Fund Profit Bonus" SortExpression="My Fund Profit Bonus">
                                                                <ItemTemplate>
                                                                    <%# Eval("SelfIncome") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Group Overriding Bonus" SortExpression="Group Overriding Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("LevelIncome")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="Self Stacking Bonus" SortExpression="Self Stacking Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("StackingIncome")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="My Rank Level Bonus" SortExpression="My Rank Level Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("RewardInc")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Salary Bonus" SortExpression="Salary Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("SalaryIncome")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Group Incentive Bonus" SortExpression="Group Incentive Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("PairIncome")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="Global Royalty Bonus" SortExpression="Global Royalty Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("RoyaltyIncome")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                           <%-- <asp:TemplateField HeaderText="Miracle Club Bonus" SortExpression="Miracle Club Bonus">
                                                                <ItemTemplate>
                                                                    <%#Eval("ClubIncome")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Total Bonus" SortExpression="Net Income">
                                                                <ItemTemplate>
                                                                    <%#Eval("netincome")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

</asp:Content>

