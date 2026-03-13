<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddBvpoint.aspx.cs" Inherits="AddBvpoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        p {
            font-weight: bold;
            color: #666666;
            margin: 0px;
            line-height: 25px;
            width: 400px;
            padding-bottom: 8px;
            text-align: left;
        }
    </style>
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
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <!-- left column -->
                    <div class="col-md-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Add Virtual Power</h3>
                            </div>
                            <div class="card-body">
                                <div align="center">
                                    <span id="lblt" class="text-danger"></span>
                                </div>
                                <div class="row">
                                    <%--<div class="col-md-4">--%>
                                    <div class="form-group">
                                        <asp:Label ID="LblAmount" runat="server" CssClass="label-text"></asp:Label>
                                        <span id="lblStock" runat="server" style="color: #000; font-weight: bold; font-size: 14px;"></span>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group">
                                        <label>Member ID : </label>
                                        <asp:Label ID="LblMobl" runat="server" Visible="false"></asp:Label>
                                        <asp:TextBox ID="TxtIDNo" runat="server" CssClass="TxtBox" OnTextChanged="TxtIDNo_TextChanged" AutoPostBack="true"></asp:TextBox><br />
                                        <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                        <asp:TextBox ID="TxtFormNo" runat="server" CssClass="TxtBox" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                            ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group">
                                        <label>PV Type:</label>
                                        <asp:RadioButtonList ID="RbtType" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" ValidationGroup="Save" AutoPostBack="true" OnSelectedIndexChanged="RbtType_SelectedIndexChanged">
                                            <asp:ListItem Text="Self" Value="S" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Tree" Value="T"></asp:ListItem>
                                        </asp:RadioButtonList>

                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group">
                                        <label>Leg No:</label>
                                        <asp:RadioButtonList ID="RbtLeg" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ValidationGroup="Save">
                                            <asp:ListItem Text="Left" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group">
                                        <label>PV Point:</label>
                                        <asp:TextBox ID="TxtFund" runat="server" CssClass="TxtBox" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter BV Point."
                                            ControlToValidate="TxtFund" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group">
                                        <label>Remarks :</label>
                                        <asp:TextBox ID="TxtRemarks" runat="server" CssClass="TxtBox"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Remrks"
                                            ControlToValidate="TxtRemarks" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group">
                                        <asp:Button ID="BtnFundTransfer" runat="server" Text="Add BV" OnClientClick="return confirmation();"
                                            class="btn btn-primary" ValidationGroup="Save" OnClick="BtnFundTransfer_Click" />
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
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

