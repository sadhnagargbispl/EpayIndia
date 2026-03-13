<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="profile.aspx.cs" Inherits="profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function Validation() {
            var a = document.getElementById('<%= txtPanNo.ClientID %>').value;
            if (a == "") {
                return true;
            }
            else {
                var regex1 = /^[A-Z]{5}\d{4}[A-Z]{1}$/;  //this is the pattern of regular expersion
                if (regex1.test(a) == false) {
                    alert('Please enter valid pan number');
                    return false;
                }
            }
        }
    </script>

    <script type="text/javascript">
        function Validation1() {
            var a = document.getElementById('<%= TxtIfsCode.ClientID %>').value;
            if (a == "") {
                return true;
            }
            else {
                var regex1 = / ^[A-Za-z]{4}[0][a-zA-Z0-9]{6}$/;  //this is the pattern of regular expersion
                if (regex1.test(a) == false) {
                    alert('Please enter valid IFSC Code');
                    return false;
                }
            }
        }
    </script>
    <script type="text/javascript" src="assets/jquery.min.js">
    </script>

    <script type="text/javascript" src="assets/jquery.validationEngine-en.js"></script>

    <script type="text/javascript" src="assets/jquery.validationEngine.js"></script>

    <link href="assets/validationEngine.jquery.min.css" rel="stylesheet" type="text/css" />


    <script type="text/javascript">
        function FnVillageChange(val) {

            if (val == "381264") {

                document.getElementById("divVillage").style.display = "block";

            }
            else {
                document.getElementById("divVillage").style.display = "none";
            }

        }
    </script>
    <style type="text/css">
        body {
            margin: 0;
            padding: 0;
            font-family: Arial;
        }

        .modal1 {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .center1 {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

            .center1 img {
                height: 128px;
                width: 128px;
            }
    </style>
    <style>
        .textbox {
            width: 200px; /* Width ko set karein */
            height: 30px; /* Height ko set karein */
        }
    </style>
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
                                <h3 class="card-title">profile  </h3>
                            </div>
                            <div class="card-body">
                                <div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            Enter Member ID :
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtMemberId" runat="server" class="form-control"></asp:TextBox><asp:RequiredFieldValidator
                                                        ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtMemberId"
                                                        runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnShowMemDetail" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>

                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="col-md-4">
                                                    <br />

                                                    <asp:Button ID="btnShowMemDetail" runat="server"
                                                        Text="Show Detail" class="btn btn-primary" OnClick="btnShowMemDetail_Click" />
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="txtMemberId" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                        <asp:UpdatePanel ID="updatepanel3" runat="server">
                                            <ContentTemplate>
                                                <div class="col-md-4">
                                                    <br />

                                                    <asp:button id="btnSubmit" runat="server" text="Update" class="btn btn-primary" visible="false"
                                                        validationgroup="Save" xmlns:asp="#unknown" onclick="btnSubmit_Click" />
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnShowMemDetail" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>

                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px">
                                            </asp:Label>
                                        </div>
                                        <div id="divDetailSection" runat="server" style="margin-bottom: 30px; margin-right: 20px;">

                                            <div class="col-md-12">
                                                <div class="col-md-3">
                                                </div>
                                                <div class="col-md-6">
                                                    <h5 style="color: Black">
                                                        <strong>Sponsor Detail </strong>
                                                    </h5>
                                                </div>
                                                <div class="col-md-3">
                                                </div>
                                            </div>
                                            <div class="row" id="tblSpnsr" runat="server" visible="false">
                                                <div class="col-md-1">
                                                </div>

                                                <div class="col-md-4">
                                                    Sponsor ID:
                                                    <div class="form-group">
                                                        <asp:TextBox ID="lblUplinerId" class="form-control" runat="server" ReadOnly="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    Sponsor Name:
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="lblUplnrNm" class="form-control" runat="server" ReadOnly="false"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    Sponsor ID:
                                                    <asp:TextBox ID="lblRefralId" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    Sponsor Name:
                                                    <asp:TextBox ID="lblRefralNm" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    Member Name :
                                                    <asp:TextBox ID="MemfirstName" CssClass="form-control validate[required,custom[onlyLetterNumberChar]]"
                                                        runat="server" ForeColor="black" Font-Bold="true" Font-Size="12px" TabIndex="8"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    Country 
   
                                                    <asp:DropDownList ID="ddlCountryName" runat="server" TabIndex="35" CssClass="form-control" OnSelectedIndexChanged="ddlCountryName_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-2" style="padding-top: 23px;">
                                                    <asp:TextBox ID="ddlCountry" CssClass="form-control "
                                                        TabIndex="23" runat="server" ReadOnly="true"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    Mobile No.:
                                                    <asp:TextBox ID="txtMobileNo" CssClass="form-control validate[required,custom[mobile]]"
                                                        TabIndex="24" runat="server" MaxLength="10"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Display="Dynamic" ControlToValidate="txtMobileNo"
                                                        runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-4">
                                                    E-Mail ID:
                                                    <asp:TextBox ID="TxtEmailID" CssClass="form-control validate[custom[email]]" TabIndex="25" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    Password:
                                                        <asp:TextBox ID="TxtPassword" runat="server" CssClass="form-control validate[required]" TabIndex="34"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" Display="Dynamic" ControlToValidate="TxtPassword"
                                                        runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-4">
                                                    Transaction Password:
    <asp:TextBox ID="TxtTransactionPassword" CssClass="form-control validate[required]" TabIndex="35" runat="server"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="row">


                                                <div class="col-md-4" style="display: none">
                                                    Proposer Name:
     <asp:TextBox ID="TxtProposerName" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                                                </div>

                                            </div>


                                            <div class="col-md-12" id="trpinpoin" runat="server" visible="false">
                                                <div class="col-md-2">
                                                    Pin Point:
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RadioButtonList ID="RbtPinPoint" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Selected="True" Text="Yes" Value="Y"></asp:ListItem>
                                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                                <div class="col-md-3">
                                                </div>
                                                <div class="col-md-3">
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row" style="display: none">
                                                <div class="col-md-5">
                                                    Wallet Address USDT(BEP20) :
                                                    <asp:TextBox ID="TxtCoinAddress" runat="server" CssClass="form-control"
                                                        TabIndex="36"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-12">
                                                <br />
                                                <div class="col-md-2" runat="server" visible="false">
                                                    Date of Birth:
                                                </div>
                                                <div class="col-md-4" runat="server" visible="false">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlDOBdt" runat="server" class="form-control" AutoPostBack="true"
                                                                    TabIndex="11">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlDOBmnth" runat="server" class="form-control" AutoPostBack="true"
                                                                    TabIndex="12">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlDOBYr" runat="server" class="form-control" AutoPostBack="true"
                                                                    TabIndex="13">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div class="col-md-6">
                                                    </div>
                                                </div>
                                                <div class="col-md-12" runat="server" visible="false">
                                                    <div class="col-md-3">
                                                    </div>
                                                    <div class="col-md-6">
                                                        <h5 style="color: Black">
                                                            <strong>Address Detail </strong>
                                                        </h5>
                                                    </div>
                                                    <div class="col-md-3">
                                                    </div>
                                                </div>
                                                <br />
                                                <div class="col-md-2" runat="server" visible="false">
                                                    Address :
                                                </div>
                                                <div class="col-md-4" runat="server" visible="false">
                                                    <asp:TextBox ID="txtAddLn1" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                        TabIndex="14" runat="server">
                                                    </asp:TextBox>
                                                </div>
                                                <div class="col-md-2" runat="server" visible="false">
                                                    Pincode:
                                                </div>
                                                <div class="col-md-4" runat="server" visible="false">
                                                    <asp:TextBox ID="txtPinCode" CssClass="form-control validate[custom[pincode]]" AutoPostBack="true"
                                                        TabIndex="15" runat="server"></asp:TextBox>
                                                </div>
                                                <asp:UpdatePanel ID="Update1" runat="server">
                                                    <ContentTemplate>

                                                        <div class="col-md-12" runat="server" visible="false">
                                                            <br />
                                                            <div class="col-md-2">
                                                                State:
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtStateName" runat="server" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                                    TabIndex="16" autocomplete="off" Enabled="false"></asp:TextBox>
                                                                <asp:HiddenField ID="StateCode" runat="server" />
                                                            </div>
                                                            <div class="col-md-2">
                                                                District:
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:HiddenField ID="HDistrictCode" runat="server" />
                                                                <asp:TextBox ID="ddlDistrict" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                                    TabIndex="17" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12" runat="server" visible="false">
                                                            <br />
                                                            <div class="col-md-2">
                                                                City:
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:HiddenField ID="HCityCode" runat="server" />
                                                                <asp:TextBox ID="ddlTehsil" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                                    TabIndex="18" runat="server"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                                Area:
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="DDlArea" runat="server" class="form-control" TabIndex="19"
                                                                    onchange="FnVillageChange(this.value);">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="TxtPincode" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnShowMemDetail" EventName="Click" />

                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <div class="col-md-12" id="divVillage" style="display: none">
                                                    <div class="col-md-2">
                                                        Area Name
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtVillage" CssClass="form-control" TabIndex="20" runat="server"
                                                            autocomplete="off"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                    </div>
                                                </div>
                                                <div class="col-md-12" runat="server" visible="false">
                                                    <br />
                                                    <div class="col-md-2">
                                                        Address Proof:
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="DDLAddressProof" runat="server" CssClass="form-control" TabIndex="21">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        Address Proof No:
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtIdProofNo" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                            TabIndex="22" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="col-md-3">
                                                    </div>
                                                    <div class="col-md-6" runat="server" visible="false">
                                                        <h5 style="color: Black">
                                                            <strong>Contact Detail </strong>
                                                        </h5>
                                                    </div>
                                                    <div class="col-md-3">
                                                    </div>
                                                </div>
                                                <div class="col-md-12" runat="server" visible="false">
                                                    <br />
                                                    <div class="col-md-3" runat="server" visible="false">
                                                        <asp:TextBox ID="txtFNm" runat="server" TabIndex="10" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                            ForeColor="black" Font-Bold="true" Font-Size="12px"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2">
                                                        Phone No.:
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtPhNo" CssClass="form-control " TabIndex="24" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="col-md-12">
                                                    <div class="col-md-3">
                                                    </div>
                                                    <div class="col-md-6 " runat="server" visible="false">
                                                        <h5 style="color: Black">
                                                            <strong>Nominee Detail</strong></h5>
                                                    </div>
                                                    <div class="col-md-3">
                                                    </div>
                                                </div>
                                                <div class="col-md-12" runat="server" visible="false">
                                                    <br />
                                                    <div class="col-md-2">
                                                        Nominee Name:
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="lblNominee" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                            TabIndex="26" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2">
                                                        Relation:
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="lblRelation" CssClass="form-control validate[custom[onlyLetterNumberChar]]"
                                                            TabIndex="27" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="col-md-3">
                                                    </div>
                                                    <div class="col-md-6" runat="server" visible="false">
                                                        <h5 style="color: Black">
                                                            <strong>Bank Detail</strong></h5>
                                                    </div>
                                                    <div class="col-md-3">
                                                    </div>
                                                </div>
                                                <div class="col-md-12" runat="server" visible="false">
                                                    <div class="col-md-2">
                                                        Account Type.:
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="DDLAccountType" runat="server" CssClass="form-control" TabIndex="28">
                                                            <asp:ListItem Text="CHOOSE ACCOUNT TYPE" Value="0" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="SAVING ACCOUNT" Value="SAVING ACCOUNT"></asp:ListItem>
                                                            <asp:ListItem Text="CURRENT ACCOUNT" Value="CURRENT ACCOUNT"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        Account No.:
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtAccountNo" class="form-control" TabIndex="29" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-12" runat="server" visible="false">
                                                    <br />
                                                    <div class="col-md-2">
                                                        Bank:
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="CmbBank" class="form-control" runat="server" TabIndex="30">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        Branch Name:
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtBranchName" CssClass="form-control" TabIndex="31" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-12" runat="server" visible="false">
                                                    <br />
                                                    <div class="col-md-2">
                                                        IFSC Code:
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="TxtIfsCode" runat="server" CssClass="form-control validate[custom[ifsccode]]"
                                                            TabIndex="32"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Enter Valid IFSC Code"
                                                            ControlToValidate="TxtIfsCode" ValidationGroup="Save" ValidationExpression="[A-Za-z]{4}[0][a-zA-Z0-9]{6}$"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="col-md-2">
                                                        PAN No.:
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtPanNo" CssClass="form-control validate[custom[panno]]" TabIndex="33"
                                                            runat="server"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Enter Valid Pan No"
                                                            ControlToValidate="txtPanNo" ValidationExpression="[A-Za-z]{5}\d{4}[A-Za-z]{1}"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>


                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                                    ShowSummary="False" ValidationGroup="Save" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnShowMemDetail" EventName="Click" />

                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </section>
    </div>
</asp:Content>

