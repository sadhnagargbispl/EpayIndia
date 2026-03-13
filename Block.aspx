<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Block.aspx.cs" Inherits="Block" %>

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
                                <h3 class="card-title">Block</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                          <%--  <asp:Panel ID="pnlChoice" runat="server" BackColor="Transparent">
                                                <asp:RadioButtonList ID="rdblistChoice" runat="server" AutoPostBack="True" CellPadding="2"
                                                    CellSpacing="5" RepeatColumns="2" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="single">Block Single Member</asp:ListItem>
                                                    <asp:ListItem Value="multiple">Block Tree</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </asp:Panel>--%>
                                        </div>
                                        <div class="form-group" id="divSingle" runat="server">
                                            ID No.
                                          <asp:TextBox class="form-control" ID="txtMemberId" runat="server" OnTextChanged="txtMemberId_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:TextBox ID="TxtFormNo" runat="server" Visible="false" ></asp:TextBox>
                                          <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId"
                                                runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>--%>
                                        </div>
                                        <div class="form-group" id="divreason" runat="server" visible="false" >
                                            Reason :
                                            <asp:TextBox class="form-control" ID="TxtReason" runat="server"></asp:TextBox>
                                            <asp:TextBox ID="TextBox2" runat="server" Visible="false"></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="TxtReason"
                                                runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                        </div>

                                        <div class="form-group">
                                            <asp:Button ID="btnShowSingleDetail" class="btn btn-primary" runat="server" Text="View Detail"
                                                ValidationGroup="Save" OnClick="BtnShow_Click" />
                                            <asp:Button ID="BtnBlock" class="btn btn-primary" runat="server" Text="Block" Width="70px"
                                                Visible="false" ValidationGroup="Save" OnClick="BtnBlock_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-2">
                                            <asp:Label ID="lblrecordcount" runat="server" Text="" Font-Bold="True"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label Style="padding-left: 10px; padding-top: 5px" ForeColor="Red" ID="lblError"
                                                runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </div>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div style="margin-bottom: 20px; overflow: scroll;">
                                            <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" AllowPaging="true" CssClass="table table-bordered"
                                                HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging">
                                            </asp:GridView>

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

