<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddGroup.aspx.cs" Inherits="AddGroup" %>

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
                                <h3 class="card-title">Add Group Master</h3>
                            </div>
                            <div class="card-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Group Name :</label>
                                            <asp:TextBox ID="txtGrpName" class="form-control" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtGrpName"
                                                runat="server" ValidationGroup="VG1" ErrorMessage="Please Enter Group Name.!" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="form-group">
                                            <label>Remarks :</label>
                                            <asp:TextBox ID="txtRemarks" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label>Status :</label>
                                            <asp:RadioButtonList ID="rdblist" runat="server"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                                <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="BtnSave" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="VG1" OnClick="BtnSave_Click" />
                                            <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                            <asp:TextBox ID="txtGrpID" runat="server" Visible="false"></asp:TextBox>
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

