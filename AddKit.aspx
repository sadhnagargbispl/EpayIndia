<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddKit.aspx.cs" Inherits="AddKit" %>

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
                                <h3 class="card-title">Add Kit Master</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div align="center">
                                            <span id="lblt" class="text-danger"></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            Kit Name :
                                       <asp:TextBox ID="txtkitName" class="form-control" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtkitName"
                                                runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                        </div>
                                        <div class="form-group">
                                            Kit Amount :  
                                    <asp:TextBox class="form-control" ID="txtKitAmt" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            Join Amount :
                                    <asp:TextBox class="form-control" ID="txtJoinAmt" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Kit Unit :
                                     <asp:TextBox class="form-control" ID="txtKitUnit" runat="server"></asp:TextBox>
                                            <asp:Label ID="LblKitDate" runat="server" Visible="false"></asp:Label>
                                        </div>

                                        <div class="form-group">
                                            Serial Start :
                                    <asp:TextBox class="form-control" ID="txtSerialStart" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            Ref. Income :
                                    <asp:TextBox class="form-control" ID="txtRefIn" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            Pool Income :
                                    <asp:TextBox class="form-control" ID="txtPoolIn" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Spill Income :
                                    <asp:TextBox class="form-control" ID="txtSpillIn" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Binary Income :
                                     <asp:TextBox class="form-control" ID="txtBinaryIn" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            B.V. :
                                    <asp:TextBox class="form-control" ID="txtBV" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            P.V :
                                    <asp:TextBox class="form-control" ID="txtPV" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            R.P. :
                                    <asp:TextBox class="form-control" ID="txtRP" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Capping :
                                    <asp:TextBox class="form-control" ID="txtCapping" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Top Up Sequence :
                                    <asp:TextBox class="form-control" ID="TxtTopUp" runat="server" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Remarks :
                                    <asp:TextBox class="form-control" ID="txtRemarks" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            Join Color :
                                    <asp:RadioButtonList ID="RbtColor" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Green" Selected="True" Value="Green.jpg">
                                          <img src="images/Green.jpg" />
                                        </asp:ListItem>
                                        <asp:ListItem Text="Red" Selected="True" Value="Red.jpg">
                                            <img src="images/red.jpg" />
                                        </asp:ListItem>

                                    </asp:RadioButtonList>
                                        </div>

                                        <div class="form-group">
                                            Status :
                                    <asp:RadioButtonList ID="rdblist" runat="server" CellPadding="0" CellSpacing="10"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="true" Text="Active">Active</asp:ListItem>
                                        <asp:ListItem Text="DeActive">DeActive</asp:ListItem>
                                    </asp:RadioButtonList>
                                        </div>
                                        <div class="form-group">
                                            <asp:TextBox ID="txtActiveStatus" runat="server" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="txtKitId" runat="server" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="txtIPAdrs" runat="server" Visible="false"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="BtnSave" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="Save" OnClick="BtnSave_Click" />
                                            <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
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

