<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddCoin.aspx.cs" Inherits="AddCoin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <!-- left column -->
                    <div class="col-md-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title"> Stack Mode ROI Slab </h3>
                            </div>
                            <div class="card-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Stack Month:</label>
                                            <asp:DropDownList ID="DDLStackType" runat="server" class="form-control"
                                                AutoPostBack="True" OnSelectedIndexChanged="DDLStackType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group">
                                            <label>Profit :</label>
                                            <asp:TextBox CssClass="form-control" ID="txtCType" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtCType"
                                                runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                        </div>

                                        <div class="form-group">
                                            <label>Status :</label>
                                            <asp:RadioButtonList ID="rdblist" runat="server" CellPadding="0" CellSpacing="10"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                                <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="BtnSave" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" OnClick="BtnSave_Click" />
                                            <asp:TextBox ID="txtCTypeID" runat="server" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
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

