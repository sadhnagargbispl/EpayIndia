<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddUser.aspx.cs" Inherits="AddUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                                <h3 class="card-title">Add User Master</h3>
                            </div>
                            <div class="card-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        User Name :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ddlGroup" runat="server" class="form-control" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">User Name :</div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtUsrName" class="form-control" runat="server" AutoPostBack="true" MaxLength="40"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtUsrName" runat="server"
                                            ValidationGroup="Save" ForeColor="OrangeRed" ErrorMessage="Please Enter User Name.!"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">Password :  </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtPswd" class="form-control" runat="server" MaxLength="20"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="txtPswd" runat="server"
                                            ValidationGroup="Save" ForeColor="OrangeRed" ErrorMessage="Please Enter Password.!"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">Mobile No :  </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtMobileNo" class="form-control" runat="server" MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" ControlToValidate="TxtMobileNo" runat="server"
                                            ValidationGroup="Save" ForeColor="OrangeRed" ErrorMessage="Please Enter Mobile No.!">
                                        </asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                            TargetControlID="TxtMobileNo" />
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="col-md-4">Remarks : </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtRemarks" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        Status :
                                    </div>
                                    <div class="col-md-6">
                                        <asp:RadioButtonList ID="rdblist" runat="server" CellPadding="0" CellSpacing="10"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                            <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-2"></div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        <asp:Button ID="BtnSave" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" OnClick="BtnSave_Click" />
                                        <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                        <asp:TextBox ID="txtGrpID" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtUserID" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-8"></div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </section>
    </div>


</asp:Content>

