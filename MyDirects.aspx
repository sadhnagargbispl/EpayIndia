<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MyDirects.aspx.cs" Inherits="MyDirects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Level Wise Business</h3>
                            </div>
                            <!-- /.card-header -->
                            <!-- form start -->
                            <div class="card-body">

                                <div class="row">
                                    <div class="col-2">
                                        <div class="form-group">
                                            <div class="form-group">
                                                Member ID :
                                                <asp:TextBox ID="txtMember" runat="server" AutoPostBack="true" class="form-control" OnTextChanged="txtMember_TextChanged"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMember"
                                                    runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                <asp:Label ID="lblErr" runat="server" Style="font-weight: bold; font-size: 12px; color: Red"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-2">
                                        <div class="form-group">
                                            Search By :
                                         <asp:DropDownList ID="rbtnsearch" runat="server" class="form-control">
                                             <asp:ListItem Text="Level Wise" Selected="True" Value="L"></asp:ListItem>
                                         </asp:DropDownList>
                                        </div>

                                    </div>
                                    <div class="col-2">
                                        <div class="form-group">
                                            Level :
                                            <asp:DropDownList ID="DdlLevel" CssClass="form-control" TabIndex="1" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-2">
                                        <div class="form-group">
                                            Search :
                                        <asp:DropDownList ID="DDlSearchby" CssClass="form-control" TabIndex="2" runat="server">
                                            <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Inactive" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-3">
                                        <div class="form-group">
                                            <br />
                                            <asp:Button ID="BtnSubmit" runat="server" Text="Search" TabIndex="3" ValidationGroup="Save"
                                                class="btn btn-primary" OnClick="BtnSubmit_Click" />
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <br>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                    <div class="col-md-3">
                                    </div>
                                    <div class="sda-content-3">
                                        <table id="table" class="table table-bordered">
                                            <tbody>
                                                <tr>
                                                    <td></td>
                                                    <th style="text-align: center">Direct
                                                    </th>
                                                    <th style="text-align: center">Indirect
                                                    </th>
                                                    <th style="text-align: center">Total
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Joining
                                                    </th>
                                                    <td id="tdDirectleft" runat="server" style="text-align: center">0
                                                    </td>
                                                    <td id="tdDirectright" runat="server" style="text-align: center">0
                                                    </td>
                                                    <td id="TotalDirect" runat="server" style="text-align: center">0
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>Active
                                                    </th>
                                                    <td id="tddirectActive" runat="server" style="text-align: center">0
                                                    </td>
                                                    <td id="tdindirectActive" runat="server" style="text-align: center">0
                                                    </td>
                                                    <td id="TotalActive" runat="server" style="text-align: center">0
                                                    </td>
                                                </tr>
                                              <tr>
                                                    <th>BV
                                                    </th>
                                                    <td id="Directunit" runat="server" style="text-align: center">0
                                                    </td>
                                                    <td id="indirectunit" runat="server" style="text-align: center">0
                                                    </td>
                                                    <td id="totalunit" runat="server" style="text-align: center">0
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="col-md-3">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div id="DivSideA" runat="server">
                                        <div class="table-responsive">
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text="Total Records"></asp:Label>
                                                    <asp:Label ID="lbltotal" runat="server"></asp:Label>

                                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered"
                                                        HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging">

                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex + 1%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Level" HeaderText=" Level" />
                                                            <asp:BoundField DataField="Sponsor Id" HeaderText="Sponsor ID" />
                                                            <asp:BoundField DataField="Sponsor Name" HeaderText="Sponsor Name" />
                                                            <asp:BoundField DataField="Member ID" HeaderText=" ID No" />
                                                            <asp:BoundField DataField="Member Name" HeaderText="Member Name" />
                                                            <asp:BoundField DataField="Mobile No" HeaderText="Mobile No." />
                                                            <asp:BoundField DataField="Date of Joining" HeaderText="Date Of Joining" />
                                                           <%-- <asp:BoundField DataField="BV" HeaderText="BV" />--%>
                                                            <%--<asp:BoundField DataField="PackageName" HeaderText="Package Name" />--%>
                                                            <asp:BoundField DataField="Active Status" HeaderText="Active Status" />
                                                            <asp:BoundField DataField="Activation Date" HeaderText="Activation Date" />
                                                        </Columns>
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
            </div>
        </section>
    </div>
</asp:Content>

