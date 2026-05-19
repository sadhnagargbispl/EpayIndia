<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppLogin.aspx.cs" Inherits="AppLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <title>Login – ePay India</title>
    <link rel="icon" type="image/x-icon" href="images/favicon.png" />
    <link href="https://fonts.googleapis.com/css2?family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <link href="demoepay/css/AppLogin.css?v=1.5" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="app-shell">

                <!-- HEADER -->
                <header class="app-header">
                    <a href="webapp.aspx" class="h-back" title="Back" style="display: none;">
                        <i class="fa fa-arrow-left"></i>
                    </a>
                    <img src="images/logo_light.png" alt="ePay" class="h-logo" onerror="this.style.display='none'" />
                    <div class="h-title">e<span>Pay</span> India</div>
                </header>

                <!-- HERO STRIP -->
                <div class="hero-strip">
                    <div class="logo-circle">
                        <img src="images/logo.png" alt="ePay India" onerror="this.style.display='none'; this.nextElementSibling.style.display='block';" />
                        <i class="fa fa-wallet logo-icon-fallback" style="display: none;"></i>
                    </div>
                    <div class="hero-h">Welcome Back!</div>
                    <div class="hero-sub">Sign in to your ePay India account</div>
                </div>

                <!-- LOGIN CARD -->
                <div class="login-card">

                    <div class="card-title">
                        <i class="fa fa-sign-in-alt"></i>
                        Member Login
   
                    </div>

                    <!-- error box -->
                    <div class="err-msg" id="errMsg">
                        <i class="fa fa-exclamation-circle"></i>
                        <span id="errText">
                            <asp:Label ID="lblError" runat="server" CssClass="login-error-box"
                                Style="display: none;" EnableViewState="false"></asp:Label></span>
                    </div>


                    <!-- User ID -->
                    <div class="field">
                        <label for="userId">User ID</label>
                        <div class="input-wrap">
                            <i class="fa fa-user f-icon"></i>
                            <asp:TextBox ID="TxtUserID" runat="server"
                                placeholder="Enter your User ID"
                                MaxLength="50">
                            </asp:TextBox>
                            <%--  <input type="text" id="userId" name="userId"
                            placeholder="Enter your User ID" autocomplete="username" required />--%>
                        </div>
                    </div>

                    <!-- Password -->
                    <div class="field">
                        <label for="password">Password</label>
                        <div class="input-wrap">
                            <i class="fa fa-lock f-icon"></i>
                            <%--  <input type="password" id="password" name="password"
                                placeholder="Enter your password" autocomplete="current-password" required />--%>
                            <asp:TextBox ID="TxtPassword" runat="server"
                                TextMode="Password"
                                placeholder="Enter your password"
                                MaxLength="50">
                            </asp:TextBox>
                            <button type="button" class="toggle-pw" id="eyeBtn"
                                onclick="togglePassword()" tabindex="-1"
                                aria-label="Show/hide password">
                                <i id="eyeIcon" class="fa fa-eye"></i>
                            </button>
                            <%--       <a href="webapp.html">
                                <button type="button" class="toggle-pw" onclick="togglePw()" id="pwToggle" title="Show / Hide">
                                    <i class="fa fa-eye" id="pwIcon"></i>
                                </button>
                            </a>--%>
                        </div>
                    </div>

                    <!-- Remember / Forgot -->
                    <div class="row-extra">
                        <label class="remember">
                            <input type="checkbox" id="rememberMe" />
                            Remember me
       
                        </label>
                        <a href="#" class="forgot-link" onclick="showToast('Password reset link will be sent to your registered email.')">Forgot Password?
</a>
                    </div>

                    <!-- Submit -->
                    <asp:Button ID="BtnLogin" runat="server"
                        Text="Login  →"
                        CssClass="btn-login"
                        OnClick="BtnLogin_Click" />

                    <%-- <button type="submit" class="btn-login">
                        <i class="fa fa-sign-in-alt"></i>
                        Login to Account
     
                    </button>--%>

                    <!-- Divider -->
                    <%--   <div class="divider">OR LOGIN WITH</div>--%>

                    <!-- Social -->
                    <%--         <div class="social-row">
                        <button class="btn-social" onclick="showToast('Google login coming soon')">
                            <i class="fab fa-google"></i>Google
     
                        </button>
                        <button class="btn-social" onclick="showToast('OTP login coming soon')">
                            <i class="fa fa-mobile-alt"></i>OTP Login
     
                        </button>
                    </div>--%>

                    <!-- Register link -->
                    <%--     <div class="register-row">
                        Don't have an account?
     
                        <a href="register.html">Register Now</a>
                    </div>--%>
                </div>
                <!-- /login-card -->

                <!-- FOOTER NOTE -->
                <div class="page-footer">
                    By logging in you agree to our
   
                    <a href="#">Terms of Service</a> &amp;
   
                    <a href="#">Privacy Policy</a><br />
                    © 2025 ePay India. All rights reserved.
 
                </div>

            </div>
            <!-- /app-shell -->

            <!-- TOAST -->
            <div class="toast" id="toast"></div>

        </div>
        <script>
            function togglePassword() {
                var pwBox = document.getElementById('<%= TxtPassword.ClientID %>');
                var icon = document.getElementById('eyeIcon');
                if (pwBox.type === 'password') {
                    pwBox.type = 'text';
                    icon.classList.remove('fa-eye');
                    icon.classList.add('fa-eye-slash');
                } else {
                    pwBox.type = 'password';
                    icon.classList.remove('fa-eye-slash');
                    icon.classList.add('fa-eye');
                }
            }

            // Show server-side error label if it has text
            window.addEventListener('DOMContentLoaded', function () {
                var lbl = document.getElementById('<%= lblError.ClientID %>');
                if (lbl && lbl.innerText.trim() !== '') {
                    lbl.style.display = 'block';
                }
            });
        </script>

    </form>
</body>
</html>
