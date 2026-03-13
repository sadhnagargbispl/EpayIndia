<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FundTransfer.aspx.cs" Inherits="FundTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12">
                    <div class="card card-primary">
                        <div class="card-header">
                            <h3 class="card-title">Fund Transfer</h3>
                            <div class="clearfix">
                            </div>
                        </div>
                        <div class="card-body">
                            <div align="center">
                                <span id="lblt" class="text-danger"></span>
                            </div>

                            <div class="row" style="padding-top: 10px;">
                                <div class="col-md-3">
                                    <strong style="color: orangered">Available Balance:</strong>
                                    <asp:Label ID="LblAmount" runat="server" ForeColor="OrangeRed" CssClass="form-control" Text="0.00"></asp:Label>
                                </div>
                            </div>

                            <div class="row" style="padding-top: 10px;">
                                <div class="col-md-3">
                                    <strong>Member ID :</strong>
                                    <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="TxtIDNo_TextChanged"></asp:TextBox>
                                    <span style="color: orangered; font-weight:bold;">
                                        <asp:Label ID="LblMemName" runat="server"></asp:Label></span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Member ID.!"
                                        ControlToValidate="TxtIDNo" ValidationGroup="Save" ForeColor="OrangeRed"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <strong>Select Type:</strong>
                                    <asp:RadioButtonList ID="RbtAccount" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" CssClass="form-control">
                                        <asp:ListItem Text="Credit" Value="C" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Debit" Value="D"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="row" style="padding-top: 10px;">
                                <div class="col-md-3">
                                    <strong>Wallet Type:</strong>
                                    <asp:DropDownList ID="Rbtnwallet" runat="server" AutoPostBack="true" CssClass="form-control"
                                        OnSelectedIndexChanged="Rbtnwallet_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row" style="padding-top: 10px;">
                                <div class="col-md-3">
                                    <strong>Fund Amount:</strong>
                                    <asp:TextBox ID="TxtFund" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event);"
                                        AutoPostBack="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Fund Transfer Amount.!"
                                        ControlToValidate="TxtFund" ValidationGroup="Save" ForeColor="OrangeRed"></asp:RequiredFieldValidator>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <strong>Remarks</strong>
                                    <asp:TextBox ID="TxtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="padding-top: 10px;">
                                <div class="col-md-3">
                                    <asp:Button ID="BtnFundTransfer" runat="server" Text="Fund Transfer" class="btn btn-primary" ValidationGroup="Save" OnClick="BtnFundTransfer_Click" />
                                </div>
                            </div>
                            <div class="row" style="padding-top: 10px;">
                                <div class="col-md-12">
                                    <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                    <asp:Label ID="LblMobl" runat="server" Visible="false"></asp:Label>

                                    <asp:TextBox ID="TxtFormNo" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

