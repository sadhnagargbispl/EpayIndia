<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Registartion.aspx.cs" Inherits="Registartion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <title>Wave World</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/css/bootstrap.min.css">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css">

    <style>
        body {
            background: #f7f9fc;
            font-family: Arial, sans-serif;
        }

        .register-box {
            max-width: 450px;
            margin: 60px auto;
            padding: 30px;
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 0 15px rgba(0,0,0,0.1);
        }

            .register-box .form-control {
                border-radius: 8px;
            }

        .register-btn {
            border-radius: 8px;
        }
        /* Circle (radio button) ka size badhao */
        .radio-list input[type=radio] {
            transform: scale(1.3); /* 1.3 = 130% size */
            margin-right: 8px; /* circle aur text ke beech gap */
            margin-left: 12px; /* circle ke pehle gap */
            cursor: pointer;
        }

        /* Label ke baad spacing */
        .radio-list label {
            margin-right: 25px;
            cursor: pointer;
        }
    </style>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="register-box">
                <div class="text-center mb-4">
                    <img src="img/logo.png" alt="One Wave Fintech" width="160">
                    <h4 class="mt-3">Create Account</h4>
                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                </div>

                <div>
                    <!-- Yes/No Radio -->
                    <div class="mb-3" style="display: none">
                        <label class="form-label d-block">Select User Type</label>
                        <asp:RadioButtonList ID="RadioButtonUserType" runat="server"
                            AutoPostBack="true"
                            CssClass="radio-list"
                            OnSelectedIndexChanged="RadioButtonUserType_SelectedIndexChanged"
                            RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <%--<asp:ListItem Text="New User" Value="New User"  />--%>
                            <asp:ListItem Text="One Wave User" Value="One Wave User" Selected="True" />
                        </asp:RadioButtonList>
                    </div>

                    <%--<div class="mb-3">
                        <label class="form-label d-block"></label>
                        <div class="form-check form-check-inline">
                            <input class="form-check-input" type="radio" name="agree" id="yesOption" value="Yes" required>
                            <label class="form-check-label" for="yesOption">
                                One Wave User
                            </label>
                        </div>
                        <div class="form-check form-check-inline">
                            <input class="form-check-input" type="radio" name="agree" id="noOption" value="No" required>
                            <label class="form-check-label" for="noOption">
                                New User
                            </label>
                        </div>
                    </div>--%>
                    <div>
                        <!-- Name -->
                        <div class="form-check" runat="server" id="DivLeg1" style="display: none;">
                            <label class="form-check-input">
                                Leg<span class="red">*</span></label>
                            <div class="col-sm-10">
                                <asp:RadioButtonList ID="RbtnLegNo" runat="server" TabIndex="2" RepeatDirection="Horizontal"
                                    Style="width: 150px" CssClass="form-check-label" />
                            </div>
                        </div>
                        <div class="form-group" style="display: none">
                            <label for="exampleInputEmail1 text-black">Last Name<span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                            <asp:TextBox ID="txtlastname" ClientIDMode="Static" class="form-control validate[required,custom[onlyLetterNumberChar]]"
                                placeholder="Enter Last Name" runat="server" TabIndex="4"></asp:TextBox>
                        </div>
                        <div class="form-group" style="display: none;">
                            <label for="exampleInputEmail1 text-black">Father / Husband's Name<span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                            <asp:TextBox ID="txtfather" ClientIDMode="Static" class="form-control validate[required,custom[onlyLetterNumberChar]]"
                                placeholder="Enter Father / Husband's Name" runat="server" TabIndex="4"></asp:TextBox>
                        </div>
                        <div class="form-group" style="display: none;">
                            <label for="exampleInputEmail1 text-black">Address </label>
                            <asp:TextBox ID="TxtAddress" ClientIDMode="Static" class="form-control " placeholder="Enter Address"
                                runat="server" TabIndex="5"></asp:TextBox>
                            <asp:HiddenField ID="Address" runat="server" />
                        </div>
                        <div class="form-group" style="display: none;">
                            <label for="exampleInputEmail1 text-black">Pin code<span style="color: Red; font-size: large; font-weight: bold;">*</span> </label>
                            <asp:TextBox ID="txtPinCode" CssClass="form-control" onkeypress="return isNumberKey(event);"
                                TabIndex="6" runat="server" MaxLength="6" autocomplete="off" placeholder="Enter Pin code"></asp:TextBox>
                            <asp:HiddenField ID="Pincode" runat="server" />
                        </div>
                        <div class="form-group select-control" style="display: none;">
                            <label for="exampleInputEmail1 text-black">State<span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                            <asp:DropDownList ID="ddlStatename" runat="server" CssClass="form-control" TabIndex="7">
                            </asp:DropDownList>
                            <asp:HiddenField ID="StateCode" runat="server" />

                        </div>
                        <div class="form-group" style="display: none;">
                            <label for="exampleInputEmail1 text-black">District <span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                            <asp:TextBox ID="txtDistrict" CssClass="form-control  " TabIndex="8" runat="server"
                                placeholder="Enter District"></asp:TextBox>
                            <asp:HiddenField ID="HDistrictCode" runat="server" />
                        </div>
                        <div class="form-group" style="display: none;">
                            <label for="exampleInputEmail1 text-black">City <span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                            <asp:TextBox ID="txtTehsil" CssClass="form-control" TabIndex="9" runat="server"
                                ValidationGroup="eInformation" autocomplete="off" placeholder="Enter City"></asp:TextBox>
                            <asp:HiddenField ID="HCityCode" runat="server" />
                        </div>

                        <div class="mb-3" runat="server" id="DivUserSelectTypeName" visible="true">
                            <label class="form-label">Full Name <span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="fa-solid fa-user"></i></span>
                                <asp:TextBox ID="Txtname" ClientIDMode="Static" class="form-control validate[required,custom[onlyLetterNumberChar]]"
                                    placeholder="Enter Name" runat="server" TabIndex="3"></asp:TextBox>
                                <%--<input type="text" class="form-control" placeholder="Enter full name" required>--%>
                            </div>
                        </div>

                        <!-- Mobile No -->
                        <div class="mb-3" runat="server" id="DivUserSelectTypeMobile" visible="true">
                            <label class="form-label">Mobile No. <span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="fa-solid fa-phone"></i></span>
                                <asp:TextBox ID="txtmobl"
                                    CssClass="form-control validate[required,custom[mobile]]" onkeypress="return isNumberKey(event);"
                                    runat="server" MaxLength="10" ValidationGroup="eInformation" autocomplete="off" placeholder="Enter Mobile No." OnTextChanged="txtmobl_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <%--<input type="text" class="form-control" placeholder="Enter mobile number" required>--%>
                            </div>
                        </div>
                        <!-- Email -->
                        <div class="mb-3" runat="server" id="DivUserSelectTypeEmail" visible="true">
                            <label class="form-label">Email <span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="fa-solid fa-envelope"></i></span>
                                <asp:TextBox ID="txtemail" CssClass="form-control validate[required,custom[email]]"
                                    placeholder="Enter Email ID" runat="server" TabIndex="11" OnTextChanged="txtemail_TextChanged" AutoPostBack="true"></asp:TextBox>

                                <%--<input type="email" class="form-control" placeholder="Enter email" required>--%>
                            </div>
                            <asp:Label ID="LblNameEmail" runat="server" Style="color: #198754; font-weight: bold"></asp:Label>
                        </div>
                        <!-- Password -->
                        <div class="mb-3" runat="server" id="DivUserSelectTypePassword" visible="true">
                            <label class="form-label">Password <span style="color: Red; font-size: large; font-weight: bold;">*</span></label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="fa-solid fa-lock"></i></span>
                                <asp:TextBox ID="TxtPasswd" class="form-control validate[required,minSize[8],maxSize[10]] "
                                    placeholder="Enter Password" TabIndex="12" runat="server" name="password" TextMode="Password"></asp:TextBox>
                                <%-- <input type="password" class="form-control" placeholder="Create password" required>--%>
                            </div>
                        </div>
                        <!-- Button -->
                        <%--  <button type="submit" class="btn btn-success w-100 register-btn">
                            <i class="fa-solid fa-user-plus"></i>Register
                        </button>--%>
                        <asp:Button ID="BtnProceedToPay" runat="server" Text="Register" class="btn btn-success w-100 register-btn" TabIndex="14" OnClick="BtnProceedToPay_Click" />
                        <asp:Button ID="BtnVerify" runat="server" Text="Verify" class="btn btn-success w-100 register-btn" TabIndex="14" OnClick="BtnVerify_Click" Visible="false" />
                    </div>
                </div>
                <!-- Already account -->
                <div class="text-center mt-4">
                    <p>Already have an account? <a href="login.aspx" class="text-decoration-none">Login here</a></p>
                </div>
            </div>
        </div>
    </form>
    <!-- Bootstrap JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.3/js/bootstrap.bundle.min.js"></script>
</body>
</html>
