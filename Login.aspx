<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="https://fonts.googleapis.com/css2?family=Sora:wght@400;600;700;800&family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet">
    <style>
        :root {
            --orange: #E84000;
            --orange-glow: rgba(232,64,0,0.10);
            --orange-light: #FFF4EF;
            --orange-border: #FFD4C2;
            --green: #16a34a;
            --green-light: #f0fdf4;
            --green-border: #bbf7d0;
            --red: #b91c1c;
            --red-light: #fef2f2;
            --red-border: #fecaca;
            --gray: #F5F6FA;
            --border: #E2E8F0;
            --text: #1A1A2E;
            --muted: #6B7280;
        }

        .login-page-wrap {
            min-height: 80vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 40px 20px;
            background: #F0F2F8;
            font-family: 'Nunito', sans-serif;
        }


        .login-box {
            width: 100%;
            max-width: 440px;
        }

        .login-top {
            text-align: center;
            margin-bottom: 28px;
        }

        .login-logo {
            width: 64px;
            height: 64px;
            background: var(--orange);
            border-radius: 16px;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 14px;
        }

            .login-logo span {
                font-family: 'Sora', sans-serif;
                color: #fff;
                font-weight: 800;
                font-size: 1.5rem;
            }

        .login-top h2 {
            font-family: 'Sora', sans-serif;
            font-size: 1.55rem;
            font-weight: 800;
            color: var(--text);
            margin-bottom: 5px;
        }

        .login-top p {
            color: var(--muted);
            font-size: .9rem;
        }

        .login-card {
            background: #fff;
            border-radius: 18px;
            border: 1.5px solid var(--border);
            overflow: hidden;
            box-shadow: 0 4px 24px rgba(0,0,0,.07);
        }

        .login-card-header {
            background: linear-gradient(135deg, #f97316 0%, #e84000 100%);
            padding: 22px 28px;
            position: relative;
            overflow: hidden;
        }

            .login-card-header::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                height: 3px;
                background: linear-gradient(90deg, var(--orange), #FF8C42);
            }

            .login-card-header h4 {
                font-family: 'Sora', sans-serif;
                color: #fff;
                font-size: 1.1rem;
                font-weight: 700;
                margin-bottom: 3px;
            }

            .login-card-header p {
                color: rgba(255,255,255,.55);
                font-size: .83rem;
                margin: 0;
            }

        .login-card-body {
            padding: 28px;
        }

        .epay-form-group {
            margin-bottom: 18px;
        }

            .epay-form-group label {
                display: block;
                font-weight: 700;
                font-size: .85rem;
                color: var(--text);
                margin-bottom: 7px;
                font-family: 'Nunito', sans-serif;
            }

            .epay-form-group .form-control {
                width: 100%;
                padding: 11px 15px;
                border: 1.5px solid var(--border);
                border-radius: 10px;
                font-family: 'Nunito', sans-serif;
                font-size: .92rem;
                color: var(--text);
                background: #fff;
                transition: border-color .2s, box-shadow .2s;
                outline: none;
            }

                .epay-form-group .form-control:focus {
                    border-color: var(--orange);
                    box-shadow: 0 0 0 4px var(--orange-glow);
                }

                .epay-form-group .form-control::placeholder {
                    color: #B0B7C3;
                }

        .pw-field-wrap {
            position: relative;
        }

            .pw-field-wrap .form-control {
                padding-right: 44px;
            }

        .eye-toggle {
            position: absolute;
            right: 13px;
            top: 50%;
            transform: translateY(-50%);
            background: none;
            border: none;
            cursor: pointer;
            color: var(--muted);
            font-size: 1rem;
            padding: 0;
            transition: color .2s;
            line-height: 1;
        }

            .eye-toggle:hover {
                color: var(--orange);
            }

        .forgot-link-row {
            text-align: right;
            margin-bottom: 18px;
        }

            .forgot-link-row a {
                color: var(--orange);
                font-weight: 700;
                font-size: .83rem;
                text-decoration: none;
                cursor: pointer;
            }

                .forgot-link-row a:hover {
                    text-decoration: underline;
                }

        .login-error-box {
            display: none;
            background: rgba(232,64,0,.08);
            border: 1px solid rgba(232,64,0,.3);
            border-radius: 9px;
            padding: 10px 14px;
            color: var(--orange);
            font-size: .83rem;
            font-weight: 600;
            margin-bottom: 14px;
        }

        .epay-btn-primary {
            width: 100%;
            background: var(--orange);
            color: #fff;
            border: none;
            padding: 13px 26px;
            border-radius: 11px;
            font-weight: 800;
            font-size: .92rem;
            cursor: pointer;
            font-family: 'Nunito', sans-serif;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            transition: background .2s, transform .15s, box-shadow .2s;
            letter-spacing: .2px;
        }

            .epay-btn-primary:hover {
                background: #c93500;
                transform: translateY(-1px);
                box-shadow: 0 8px 22px rgba(232,64,0,.3);
            }

        .login-divider {
            border: none;
            border-top: 1.5px solid var(--border);
            margin: 22px 0;
        }

        .signup-row {
            text-align: center;
            color: var(--muted);
            font-size: .88rem;
        }

            .signup-row a {
                color: var(--orange);
                font-weight: 700;
                text-decoration: none;
            }

                .signup-row a:hover {
                    text-decoration: underline;
                }

        /* =============================================
           FORGOT PASSWORD MODAL - ORANGE THEME
        ============================================= */
        #forgotOverlay {
            display: none;
            position: fixed;
            inset: 0;
            z-index: 9999;
            background: rgba(20, 14, 10, 0.75);
            backdrop-filter: blur(6px);
            -webkit-backdrop-filter: blur(6px);
            align-items: center;
            justify-content: center;
            padding: 16px;
            font-family: 'Nunito', sans-serif;
        }

            #forgotOverlay.show {
                display: flex;
                animation: fadeOverlay .25s ease;
            }

        @keyframes fadeOverlay {
            from {
                opacity: 0;
            }
            to {
                opacity: 1;
            }
        }

        .forgot-modal {
            width: 100%;
            max-width: 440px;
            background: #fff;
            border-radius: 18px;
            overflow: hidden;
            box-shadow: 0 32px 80px rgba(0,0,0,0.35);
            animation: popModal .3s cubic-bezier(.34,1.56,.64,1);
            position: relative;
            border: 1.5px solid var(--border);
        }

        @keyframes popModal {
            from {
                opacity: 0;
                transform: scale(0.88) translateY(20px);
            }
            to {
                opacity: 1;
                transform: scale(1) translateY(0);
            }
        }

        .forgot-modal-header {
            background: linear-gradient(135deg, #f97316 0%, #e84000 100%);
            padding: 24px 28px 20px;
            position: relative;
        }

            .forgot-modal-header::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                height: 3px;
                background: linear-gradient(90deg, var(--orange), #FF8C42);
            }

            .forgot-modal-header h5 {
                margin: 0 0 4px;
                font-family: 'Sora', sans-serif;
                font-size: 1.1rem;
                font-weight: 700;
                color: #fff;
            }

            .forgot-modal-header p {
                margin: 0;
                font-size: .83rem;
                color: rgba(255,255,255,.65);
            }

        .forgot-close {
            position: absolute;
            top: 14px;
            right: 16px;
            background: rgba(255,255,255,0.15);
            border: none;
            width: 30px;
            height: 30px;
            border-radius: 50%;
            color: #fff;
            font-size: 14px;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: background .2s;
        }

            .forgot-close:hover {
                background: rgba(255,255,255,0.3);
            }

        .forgot-modal-body {
            padding: 26px 28px 28px;
        }

        /* Step Bar */
        .forgot-step-bar {
            display: flex;
            align-items: center;
            margin-bottom: 24px;
        }

        .fstep {
            display: flex;
            flex-direction: column;
            align-items: center;
            flex: 1;
        }

        .fstep-dot {
            width: 30px;
            height: 30px;
            border-radius: 50%;
            background: #f1f5f9;
            color: #94a3b8;
            font-size: 12px;
            font-weight: 700;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: all .3s;
            font-family: 'Sora', sans-serif;
        }

            .fstep-dot.active {
                background: var(--orange);
                color: #fff;
                box-shadow: 0 0 0 4px var(--orange-glow);
            }

            .fstep-dot.done {
                background: var(--green);
                color: #fff;
            }

        .fstep-lbl {
            font-size: 10px;
            color: #94a3b8;
            margin-top: 5px;
            white-space: nowrap;
            font-weight: 600;
            letter-spacing: .3px;
            text-transform: uppercase;
        }

            .fstep-lbl.active {
                color: var(--orange);
                font-weight: 700;
            }

            .fstep-lbl.done {
                color: var(--green);
            }

        .fstep-line {
            flex: 1;
            height: 2px;
            background: #e2e8f0;
            margin-bottom: 15px;
            transition: background .3s;
        }

            .fstep-line.done {
                background: var(--green);
            }

        /* Panels */
        .forgot-panel {
            display: none;
            animation: slideInPanel .25s ease;
        }

            .forgot-panel.active {
                display: block;
            }

        @keyframes slideInPanel {
            from {
                opacity: 0;
                transform: translateX(16px);
            }
            to {
                opacity: 1;
                transform: translateX(0);
            }
        }

        /* Labels & Inputs */
        .forgot-label {
            font-size: .78rem;
            font-weight: 700;
            color: var(--text);
            letter-spacing: .3px;
            display: block;
            margin-bottom: 6px;
            font-family: 'Nunito', sans-serif;
        }

        .forgot-input {
            width: 100%;
            padding: 11px 15px;
            border: 1.5px solid var(--border);
            border-radius: 10px;
            font-family: 'Nunito', sans-serif;
            font-size: .92rem;
            color: var(--text);
            outline: none;
            transition: border-color .2s, box-shadow .2s;
        }

            .forgot-input:focus {
                border-color: var(--orange);
                box-shadow: 0 0 0 4px var(--orange-glow);
            }

            .forgot-input::placeholder {
                color: #B0B7C3;
            }

        /* OTP Inputs */
        .fotp-sent {
            font-size: .82rem;
            color: var(--muted);
            text-align: center;
            margin-bottom: 14px;
        }

            .fotp-sent b {
                color: var(--text);
                font-weight: 700;
            }

        .fotp-inputs {
            display: flex;
            gap: 8px;
            justify-content: center;
            margin: 14px 0 6px;
        }

            .fotp-inputs input {
                width: 44px;
                height: 50px;
                text-align: center;
                font-size: 20px;
                font-weight: 700;
                border: 1.5px solid var(--border);
                border-radius: 10px;
                outline: none;
                transition: all .2s;
                color: var(--text);
                font-family: 'Sora', sans-serif;
            }

                .fotp-inputs input:focus {
                    border-color: var(--orange);
                    box-shadow: 0 0 0 4px var(--orange-glow);
                }

                .fotp-inputs input.filled {
                    border-color: var(--green);
                    background: var(--green-light);
                    color: var(--green);
                }

        .fotp-timer {
            font-size: .8rem;
            color: var(--muted);
            text-align: center;
            margin-top: 10px;
            margin-bottom: 4px;
        }

            .fotp-timer span {
                font-weight: 700;
                color: var(--orange);
                font-family: 'Sora', sans-serif;
            }

        .fotp-resend {
            background: none;
            border: none;
            color: var(--orange);
            cursor: pointer;
            font-size: .8rem;
            display: none;
            font-weight: 700;
            padding: 0;
            margin-left: 6px;
            font-family: 'Nunito', sans-serif;
        }

            .fotp-resend:hover {
                text-decoration: underline;
            }

        /* Buttons */
        .forgot-btn-primary {
            width: 100%;
            background: var(--orange);
            color: #fff;
            border: none;
            padding: 12px 24px;
            border-radius: 11px;
            font-weight: 800;
            font-size: .92rem;
            cursor: pointer;
            font-family: 'Nunito', sans-serif;
            transition: background .2s, transform .15s, box-shadow .2s;
            margin-top: 18px;
            letter-spacing: .2px;
        }

            .forgot-btn-primary:hover {
                background: #c93500;
                transform: translateY(-1px);
                box-shadow: 0 8px 22px rgba(232,64,0,.3);
            }

            .forgot-btn-primary:disabled {
                background: #d1d5db;
                cursor: not-allowed;
                transform: none;
                box-shadow: none;
            }

        /* Alert */
        .forgot-alert {
            display: none;
            padding: 10px 14px;
            border-radius: 9px;
            font-size: .82rem;
            font-weight: 600;
            margin-bottom: 14px;
            align-items: center;
            gap: 8px;
        }

            .forgot-alert.err {
                display: flex;
                background: var(--red-light);
                border: 1px solid var(--red-border);
                color: var(--red);
            }

            .forgot-alert.ok {
                display: flex;
                background: var(--green-light);
                border: 1px solid var(--green-border);
                color: #15803d;
            }

        /* Success Panel */
        .forgot-success {
            text-align: center;
            padding: 10px 0 6px;
        }

        .success-circle {
            width: 68px;
            height: 68px;
            background: var(--green-light);
            border: 2px solid var(--green-border);
            border-radius: 50%;
            margin: 0 auto 16px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 28px;
            animation: popIn .5s cubic-bezier(.34,1.56,.64,1);
        }

        @keyframes popIn {
            from {
                transform: scale(0);
            }
            to {
                transform: scale(1);
            }
        }

        .forgot-success h5 {
            font-family: 'Sora', sans-serif;
            font-size: 1.05rem;
            color: var(--green);
            font-weight: 700;
            margin-bottom: 8px;
        }

        .forgot-success p {
            font-size: .85rem;
            color: var(--muted);
            line-height: 1.65;
            margin-bottom: 0;
        }

            .forgot-success p b {
                color: var(--text);
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="login-page-wrap">
        <div class="login-box">

            <!-- Top Logo & Title -->
            <div class="login-top">
                <div class="login-logo"><span>e</span></div>
                <h2>Welcome back</h2>
                <p>Sign in to your ePay member account</p>
            </div>

            <!-- Card -->
            <div class="login-card">

                <div class="login-card-header">
                    <h4>Login Your Account</h4>
                    <p>Discover the ultimate convenience with our all-in-one platform</p>
                </div>

                <div class="login-card-body">

                    <!-- Server-side error label -->
                    <asp:Label ID="lblError" runat="server" CssClass="login-error-box"
                        Style="display: none;" EnableViewState="false"></asp:Label>

                    <!-- User ID -->
                    <div class="epay-form-group">
                        <label for="<%= TxtUserID.ClientID %>">User ID</label>
                        <asp:TextBox ID="TxtUserID" runat="server"
                            CssClass="form-control"
                            placeholder="Enter your User ID"
                            MaxLength="50">
                        </asp:TextBox>
                    </div>

                    <!-- Password -->
                    <div class="epay-form-group">
                        <label for="<%= TxtPassword.ClientID %>">Password</label>
                        <div class="pw-field-wrap">
                            <asp:TextBox ID="TxtPassword" runat="server"
                                CssClass="form-control"
                                TextMode="Password"
                                placeholder="Enter your password"
                                MaxLength="50">
                            </asp:TextBox>
                            <button type="button" class="eye-toggle" id="eyeBtn"
                                onclick="togglePassword()" tabindex="-1"
                                aria-label="Show/hide password">
                                <i id="eyeIcon" class="fa fa-eye"></i>
                            </button>
                        </div>
                    </div>

                    <!-- Forgot password -->
                    <div class="forgot-link-row">
                        <a href="javascript:void(0);" onclick="openForgot();">Forgot Password?</a>
                    </div>

                    <!-- Login Button -->
                    <asp:Button ID="BtnLogin" runat="server"
                        Text="Login  →"
                        CssClass="epay-btn-primary"
                        OnClick="BtnLogin_Click" />

                    <hr class="login-divider">

                    <p class="signup-row">
                        Not a Member? &nbsp;<a href="Registartion.aspx">Signup Now</a>
                    </p>

                </div>
            </div>

        </div>
    </div>


    <!-- =============================================
         FORGOT PASSWORD MODAL
    ============================================= -->
    <div id="forgotOverlay" onclick="overlayClose(event)">
        <div class="forgot-modal" id="forgotModal" onclick="event.stopPropagation();">

            <!-- Modal Header -->
            <div class="forgot-modal-header">
                <h5><i class="fa fa-key"></i> &nbsp;Forgot Password</h5>
                <p>Verify your identity to recover account</p>
                <button class="forgot-close" onclick="closeForgot()" type="button">
                    <i class="fa fa-times"></i>
                </button>
            </div>

            <!-- Modal Body -->
            <div class="forgot-modal-body">

                <!-- Forgot Step Bar -->
                <div class="forgot-step-bar">
                    <div class="fstep">
                        <div class="fstep-dot active" id="fsc1">1</div>
                        <div class="fstep-lbl active" id="fsl1">Verify</div>
                    </div>
                    <div class="fstep-line" id="fline1"></div>
                    <div class="fstep">
                        <div class="fstep-dot" id="fsc2">2</div>
                        <div class="fstep-lbl" id="fsl2">OTP</div>
                    </div>
                    <div class="fstep-line" id="fline2"></div>
                    <div class="fstep">
                        <div class="fstep-dot" id="fsc3">3</div>
                        <div class="fstep-lbl" id="fsl3">Done</div>
                    </div>
                </div>

                <!-- Alert Box -->
                <div class="forgot-alert" id="fAlert"></div>

                <!-- Forgot Panel 1: Verify -->
                <div class="forgot-panel active" id="fpanel1">
                    <div class="epay-form-group">
                        <label class="forgot-label">User ID</label>
                        <input type="text" id="fUserId" class="forgot-input" placeholder="Enter your User ID" maxlength="15" autocomplete="off" />
                    </div>
                    <div class="epay-form-group" style="margin-bottom:0;">
                        <label class="forgot-label">Registered Email</label>
                        <input type="email" id="fEmail" class="forgot-input" placeholder="Enter registered email" autocomplete="off" />
                    </div>
                    <button type="button" class="forgot-btn-primary" id="fBtnSend" onclick="fSendOtp()">
                        Send OTP
                    </button>
                </div>

                <!-- Forgot Panel 2: OTP -->
                <div class="forgot-panel" id="fpanel2">
                    <p class="fotp-sent">
                        OTP sent to <b id="fMaskedEmail"></b>
                    </p>
                    <div class="fotp-inputs">
                        <input type="text" maxlength="1" id="f1" oninput="fotpMove(this,'f2')" onkeydown="fotpBack(event,this,null)" inputmode="numeric" />
                        <input type="text" maxlength="1" id="f2" oninput="fotpMove(this,'f3')" onkeydown="fotpBack(event,this,'f1')" inputmode="numeric" />
                        <input type="text" maxlength="1" id="f3" oninput="fotpMove(this,'f4')" onkeydown="fotpBack(event,this,'f2')" inputmode="numeric" />
                        <input type="text" maxlength="1" id="f4" oninput="fotpMove(this,'f5')" onkeydown="fotpBack(event,this,'f3')" inputmode="numeric" />
                        <input type="text" maxlength="1" id="f5" oninput="fotpMove(this,'f6')" onkeydown="fotpBack(event,this,'f4')" inputmode="numeric" />
                        <input type="text" maxlength="1" id="f6" oninput="fotpMove(this,null)" onkeydown="fotpBack(event,this,'f5')" inputmode="numeric" />
                    </div>
                    <div class="fotp-timer">
                        Expires in <span id="fTimer">10:00</span>
                        <button class="fotp-resend" id="fResendBtn" type="button" onclick="fResend()">Resend OTP</button>
                    </div>
                    <button type="button" class="forgot-btn-primary" id="fBtnVerify" onclick="fVerifyOtp()">
                        Verify OTP
                    </button>
                </div>

                <!-- Forgot Panel 3: Success -->
                <div class="forgot-panel" id="fpanel3">
                    <div class="forgot-success">
                        <div class="success-circle">
                            <i class="fa fa-check" style="color:#16a34a;"></i>
                        </div>
                        <h5>Password Sent!</h5>
                        <p>
                            Your login and transaction passwords have been sent to<br />
                            <b id="fSuccessEmail"></b>
                        </p>
                    </div>
                    <button type="button" class="forgot-btn-primary" onclick="closeForgot()" style="margin-top:20px;">
                        Close &amp; Login
                    </button>
                </div>

            </div>
            <!-- /modal-body -->
        </div>
        <!-- /forgot-modal -->
    </div>
    <!-- /forgotOverlay -->


    <!-- Hidden ASP.NET controls for Forgot postback -->
    <div style="display: none;">
        <asp:TextBox ID="txtForgotID" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtForgotEmail" runat="server"></asp:TextBox>
        <asp:HiddenField ID="HdnForgotStep" runat="server" Value="0" />
        <asp:HiddenField ID="HdnForgotOTP" runat="server" />
        <asp:Button ID="BtnForgotSubmit" runat="server" Text="ForgotGo"
            OnClick="BtnForgotSubmit_Click" UseSubmitBehavior="false" />
        <asp:Button ID="BtnForgotVerifyOTP" runat="server" Text="ForgotVerify"
            OnClick="BtnForgotVerifyOTP_Click" UseSubmitBehavior="false" />
        <asp:HiddenField ID="HdnForgotResult" runat="server" Value="" />
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

        // ============================================================
        //  FORGOT PASSWORD MODAL SCRIPTS
        // ============================================================
        var fTimerInterval = null;
        var fTimerSecs = 600;
        var fCurrentPanel = 1;

        function openForgot() {
            fResetModal();
            document.getElementById('forgotOverlay').classList.add('show');
            setTimeout(function () { document.getElementById('fUserId').focus(); }, 200);
        }
        function closeForgot() {
            document.getElementById('forgotOverlay').classList.remove('show');
            clearInterval(fTimerInterval);
        }
        function overlayClose(e) {
            if (e.target === document.getElementById('forgotOverlay')) closeForgot();
        }
        function fResetModal() {
            fGoPanel(1);
            document.getElementById('fUserId').value = '';
            document.getElementById('fEmail').value = '';
            fClearOtp();
            fHideAlert();
        }

        function fUpdateSteps(n) {
            for (var i = 1; i <= 3; i++) {
                var d = document.getElementById('fsc' + i), l = document.getElementById('fsl' + i);
                var isDone = (i < n) || (n === 3 && i === 3);  // ← Step 3 bhi done hoga
                var isActive = (i === n) && (n !== 3);
                d.className = 'fstep-dot' + (isDone ? ' done' : isActive ? ' active' : '');
                l.className = 'fstep-lbl' + (isDone ? ' done' : isActive ? ' active' : '');
                d.innerHTML = isDone ? '<i class="fa fa-check"></i>' : String(i);  // ← fa-check icon
                if (i < 3) document.getElementById('fline' + i).className = 'fstep-line' + (i < n ? ' done' : '');
            }
        }
        function fGoPanel(n) {
            ['fpanel1', 'fpanel2', 'fpanel3'].forEach(function (id) { document.getElementById(id).classList.remove('active'); });
            document.getElementById('fpanel' + n).classList.add('active');
            fUpdateSteps(n);
            fCurrentPanel = n;
            fHideAlert();
        }

        function fShowAlert(msg, type) {
            var el = document.getElementById('fAlert');
            el.className = 'forgot-alert ' + (type === 'ok' ? 'ok' : 'err');
            el.innerHTML = (type === 'ok' ? '<i class="fa fa-check-circle"></i> ' : '<i class="fa fa-exclamation-triangle"></i> ') + msg;
        }
        function fHideAlert() {
            var el = document.getElementById('fAlert');
            el.className = 'forgot-alert';
            el.innerHTML = '';
        }

        function fotpMove(cur, nextId) {
            cur.classList.toggle('filled', cur.value !== '');
            if (cur.value && nextId) document.getElementById(nextId).focus();
        }
        function fotpBack(e, cur, prevId) {
            if (e.key === 'Backspace' && !cur.value && prevId) document.getElementById(prevId).focus();
        }
        function fClearOtp() {
            ['f1', 'f2', 'f3', 'f4', 'f5', 'f6'].forEach(function (id) {
                document.getElementById(id).value = '';
                document.getElementById(id).classList.remove('filled');
            });
        }
        function fGetOtp() {
            return ['f1', 'f2', 'f3', 'f4', 'f5', 'f6'].map(function (id) { return document.getElementById(id).value; }).join('');
        }

        // Step 1 - Send OTP
        function fSendOtp() {
            var uid = document.getElementById('fUserId').value.trim();
            var email = document.getElementById('fEmail').value.trim();
            if (!uid) { fShowAlert('Please enter your User ID.'); return; }
            if (!email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) { fShowAlert('Please enter a valid email address.'); return; }

            var btn = document.getElementById('fBtnSend');
            btn.disabled = true;
            btn.textContent = 'Sending...';

            document.getElementById('<%= txtForgotID.ClientID %>').value = uid;
            document.getElementById('<%= txtForgotEmail.ClientID %>').value = email;
            document.getElementById('<%= HdnForgotStep.ClientID %>').value = '1';
            document.getElementById('<%= BtnForgotSubmit.ClientID %>').click();
        }

        function fAfterSendResult() {
            var result = document.getElementById('<%= HdnForgotResult.ClientID %>').value;
            if (!result) return;

            var btn = document.getElementById('fBtnSend');
            btn.disabled = false;
            btn.textContent = 'Send OTP';

            if (result === 'OK') {
                var email = document.getElementById('<%= txtForgotEmail.ClientID %>').value.trim();
                document.getElementById('fMaskedEmail').textContent = fMaskEmail(email);
                document.getElementById('fSuccessEmail').textContent = fMaskEmail(email);
                fGoPanel(2);
                fStartTimer();
                setTimeout(function () { document.getElementById('f1').focus(); }, 100);
            } else {
                fShowAlert(result);
            }
            document.getElementById('<%= HdnForgotResult.ClientID %>').value = '';
        }

        // Step 2 - Verify OTP
        function fVerifyOtp() {
            var entered = fGetOtp();
            if (entered.length < 6) { fShowAlert('Please enter all 6 digits.'); return; }
            if (fTimerSecs <= 0) { fShowAlert('OTP expired. Click Resend.'); return; }

            var btn = document.getElementById('fBtnVerify');
            btn.disabled = true;
            btn.textContent = 'Verifying...';

            document.getElementById('<%= HdnForgotOTP.ClientID %>').value = entered;
            document.getElementById('<%= HdnForgotStep.ClientID %>').value = '2';
            document.getElementById('<%= BtnForgotVerifyOTP.ClientID %>').click();
        }

        function fAfterVerifyResult() {
            var result = document.getElementById('<%= HdnForgotResult.ClientID %>').value;
            if (!result) return;

            var btn = document.getElementById('fBtnVerify');
            btn.disabled = false;
            btn.textContent = 'Verify OTP';

            if (result === 'OK') {
                clearInterval(fTimerInterval);
                fGoPanel(3);
            } else {
                fShowAlert(result);
                fClearOtp();
                setTimeout(function () { document.getElementById('f1').focus(); }, 100);
            }
            document.getElementById('<%= HdnForgotResult.ClientID %>').value = '';
        }

        // Timer
        function fStartTimer() {
            fTimerSecs = 600;
            clearInterval(fTimerInterval);
            document.getElementById('fResendBtn').style.display = 'none';
            var el = document.getElementById('fTimer');
            fTimerInterval = setInterval(function () {
                var m = Math.floor(fTimerSecs / 60), s = fTimerSecs % 60;
                el.textContent = (m < 10 ? '0' : '') + m + ':' + (s < 10 ? '0' : '') + s;
                if (--fTimerSecs < 0) {
                    clearInterval(fTimerInterval);
                    el.textContent = '00:00';
                    document.getElementById('fResendBtn').style.display = 'inline-block';
                }
            }, 1000);
        }
        function fResend() {
            fClearOtp();
            document.getElementById('<%= HdnForgotStep.ClientID %>').value = '1';
            document.getElementById('<%= BtnForgotSubmit.ClientID %>').click();
        }

        function fMaskEmail(e) {
            var a = e.split('@');
            if (a.length < 2) return e;
            return a[0].substring(0, 2) + '****@' + a[1];
        }

        // On page load - check forgot postback result
        window.addEventListener('load', function () {
            var step = document.getElementById('<%= HdnForgotStep.ClientID %>').value;
            var result = document.getElementById('<%= HdnForgotResult.ClientID %>').value;

            if (step === '1' && result) {
                openForgot();
                fAfterSendResult();
            }
            if (step === '2' && result) {
                openForgot();
                fGoPanel(2);
                fStartTimer();
                fAfterVerifyResult();
            }
        });

        // ESC key closes modal
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') closeForgot();
        });
    </script>

</asp:Content>
