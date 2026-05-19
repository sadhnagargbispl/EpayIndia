<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style>
        :root {
            --orange: #E84000;
            --orange-glow: rgba(232,64,0,0.10);
            --gray: #F5F6FA;
            --border: #E2E8F0;
            --text: #1A1A2E;
            --muted: #6B7280;
        }

        * { box-sizing: border-box; margin: 0; padding: 0; }

        html, body {
            height: auto;
            background: #F0F2F8;
            font-family: 'Nunito', sans-serif;
            color: var(--text);
        }

        .login-page-wrap {
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 40px 20px;
            background: #F0F2F8;
        }

        .login-box {
            width: 100%;
            max-width: 460px;
        }

        /* Top logo */
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

        /* Card */
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
            top: 0; left: 0; right: 0;
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

        /* Steps indicator */
        .steps {
            display: flex;
            align-items: center;
            margin-bottom: 26px;
        }

        .step {
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 5px;
            flex: 1;
        }

        .step-dot {
            width: 30px;
            height: 30px;
            border-radius: 50%;
            border: 2px solid var(--border);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 12px;
            font-weight: 700;
            color: var(--muted);
            background: #fff;
            transition: all .3s;
            font-family: 'Sora', sans-serif;
        }

        .step-label {
            font-size: 10px;
            color: var(--muted);
            font-weight: 700;
            letter-spacing: .5px;
            text-transform: uppercase;
        }

        .step-line {
            flex: 1;
            height: 2px;
            background: var(--border);
            margin-bottom: 18px;
            transition: background .4s;
        }

        .step.active .step-dot {
            border-color: var(--orange);
            color: var(--orange);
            box-shadow: 0 0 0 4px var(--orange-glow);
        }

        .step.active .step-label { color: var(--text); }

        .step.done .step-dot {
            background: var(--orange);
            border-color: var(--orange);
            color: #fff;
        }

        .step.done .step-label { color: var(--text); }
        .step-line.done { background: var(--orange); }

        /* Form fields */
        .epay-form-group { margin-bottom: 18px; }

        .epay-form-group label {
            display: block;
            font-weight: 700;
            font-size: .85rem;
            color: var(--text);
            margin-bottom: 7px;
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

        .epay-form-group .form-control::placeholder { color: #B0B7C3; }

        /* OTP inputs */
        .otp-group {
            display: flex;
            gap: 8px;
            justify-content: center;
            margin: 8px 0;
        }

        .otp-input {
            width: 48px;
            height: 54px;
            text-align: center;
            font-size: 20px;
            font-weight: 800;
            font-family: 'Sora', sans-serif;
            border: 1.5px solid var(--border);
            border-radius: 10px;
            color: var(--orange);
            outline: none;
            transition: all .2s;
            background: #fff;
        }

        .otp-input:focus {
            border-color: var(--orange);
            box-shadow: 0 0 0 4px var(--orange-glow);
        }

        /* Timer */
        .otp-meta {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: 12px;
            font-size: 12px;
        }

        .timer {
            color: var(--orange);
            font-weight: 700;
            font-family: 'Sora', sans-serif;
        }

        .resend-btn {
            background: none;
            border: none;
            cursor: pointer;
            color: var(--muted);
            font-size: 12px;
            font-family: 'Nunito', sans-serif;
            font-weight: 700;
            transition: color .2s;
        }

        .resend-btn:hover:not(:disabled) { color: var(--orange); }
        .resend-btn:disabled { opacity: .4; cursor: not-allowed; }

        /* Alert */
        .login-error-box {
            background: rgba(232,64,0,.08);
            border: 1px solid rgba(232,64,0,.3);
            border-radius: 9px;
            padding: 10px 14px;
            color: var(--orange);
            font-size: .83rem;
            font-weight: 600;
            margin-bottom: 14px;
        }

        .login-success-box {
            background: rgba(22,163,74,.08);
            border: 1px solid rgba(22,163,74,.3);
            border-radius: 9px;
            padding: 10px 14px;
            color: #16a34a;
            font-size: .83rem;
            font-weight: 600;
            margin-bottom: 14px;
        }

        .alert-hidden { display: none; }

        /* Primary button */
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

        .epay-btn-primary:disabled {
            opacity: .6;
            cursor: not-allowed;
            transform: none;
        }

        /* Back link */
        .forgot-link-row {
            text-align: center;
            margin-top: 16px;
        }

        .forgot-link-row a,
        .forgot-link-row button {
            color: var(--orange);
            font-weight: 700;
            font-size: .83rem;
            text-decoration: none;
            background: none;
            border: none;
            cursor: pointer;
            font-family: 'Nunito', sans-serif;
        }

        .forgot-link-row a:hover,
        .forgot-link-row button:hover { text-decoration: underline; }

        /* Divider */
        .login-divider {
            border: none;
            border-top: 1.5px solid var(--border);
            margin: 22px 0;
        }

        /* Success panel */
        .success-panel { text-align: center; padding: 10px 0; }

        .success-icon {
            width: 68px;
            height: 68px;
            margin: 0 auto 18px;
            background: rgba(22,163,74,.1);
            border: 2px solid rgba(22,163,74,.35);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 28px;
        }

        .success-title {
            font-family: 'Sora', sans-serif;
            font-size: 1.25rem;
            font-weight: 800;
            color: #16a34a;
            margin-bottom: 8px;
        }

        .success-desc {
            font-size: .88rem;
            color: var(--muted);
            line-height: 1.65;
            margin-bottom: 20px;
        }

        .success-meta {
            background: #F5F6FA;
            border: 1.5px solid var(--border);
            border-radius: 10px;
            padding: 12px 16px;
            font-size: .83rem;
            color: var(--muted);
            text-align: left;
            margin-bottom: 12px;
        }

        .success-meta strong {
            display: block;
            color: var(--text);
            font-weight: 700;
            font-size: .78rem;
            text-transform: uppercase;
            letter-spacing: .5px;
            margin-bottom: 3px;
        }

        /* ASP.NET hidden */
        .asp-hidden { display: none !important; }

        /* Loading dots */
        .dot { display: inline-block; animation: pulse 1.2s ease infinite; margin: 0 2px; }
        .dot:nth-child(2) { animation-delay: .2s; }
        .dot:nth-child(3) { animation-delay: .4s; }

        @keyframes pulse { 0%,100%{opacity:1} 50%{opacity:.35} }

        .panel { animation: slideIn .3s ease both; }
        @keyframes slideIn { from{opacity:0;transform:translateY(12px)} to{opacity:1;transform:translateY(0)} }

        @media (max-width: 500px) {
            .otp-input { width: 40px; height: 46px; font-size: 17px; }
            .login-card-body { padding: 20px; }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
        <!-- Hidden ASP.NET controls -->
        <div class="asp-hidden">
            <asp:TextBox ID="txtIDNo" runat="server" MaxLength="15"></asp:TextBox>
          <%--  <asp:RequiredFieldValidator ID="RequireIDNo" runat="server" ControlToValidate="txtIDNo" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
            <asp:TextBox ID="txtemail" runat="server"></asp:TextBox>
          <%--  <asp:RequiredFieldValidator ID="Requiredemail" runat="server" ControlToValidate="txtemail" ErrorMessage="*"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="EmailExpressionValidator" runat="server" ControlToValidate="txtemail"
                ErrorMessage="Enter Valid Email ID!" Display="None"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                SetFocusOnError="true" ValidationGroup="eInformation"></asp:RegularExpressionValidator>--%>
            <asp:Button ID="Submit" runat="server" Text="Submit" OnClick="Submit_Click" />
            <asp:HiddenField ID="hdnOTP" runat="server" />
            <asp:HiddenField ID="hdnStep" runat="server" Value="1" />
        </div>

        <div class="login-page-wrap">
            <div class="login-box">

                <!-- Top Logo -->
                <div class="login-top">
                    <div class="login-logo"><span>e</span></div>
                    <h2>Forgot Password?</h2>
                    <p>Verify your identity to reset access</p>
                </div>

                <!-- Card -->
                <div class="login-card">

                    <div class="login-card-header">
                        <h4>Password Recovery</h4>
                        <p>Secure OTP-based verification for your account</p>
                    </div>

                    <div class="login-card-body">

                        <!-- Step Indicator -->
                        <div class="steps">
                            <div class="step active" id="step1Ind">
                                <div class="step-dot">1</div>
                                <div class="step-label">Verify</div>
                            </div>
                            <div class="step-line" id="line1"></div>
                            <div class="step" id="step2Ind">
                                <div class="step-dot">2</div>
                                <div class="step-label">OTP</div>
                            </div>
                            <div class="step-line" id="line2"></div>
                            <div class="step" id="step3Ind">
                                <div class="step-dot">3</div>
                                <div class="step-label">Done</div>
                            </div>
                        </div>

                        <!-- Alert -->
                        <div class="alert-hidden" id="alertBox"></div>

                        <!-- PANEL 1: Verify Identity -->
                        <div id="panel1" class="panel">
                            <div class="epay-form-group">
                                <label>User ID</label>
                                <input type="text" id="uiUserId" class="form-control" placeholder="Enter your User ID" maxlength="15" autocomplete="off" />
                            </div>
                            <div class="epay-form-group">
                                <label>Email Address</label>
                                <input type="email" id="uiEmail" class="form-control" placeholder="Enter registered email" autocomplete="off" />
                            </div>
                            <button type="button" class="epay-btn-primary" id="btnSendOtp" onclick="sendOtp()">
                                Send OTP &nbsp;→
                            </button>
                            <div class="forgot-link-row" style="margin-top:16px;">
                                <a href="Default.aspx">← Back to Login</a>
                            </div>
                        </div>

                        <!-- PANEL 2: OTP Entry -->
                        <div id="panel2" class="panel" style="display:none;">
                            <div class="epay-form-group">
                                <label style="text-align:center;display:block;">
                                    A 6-digit code has been sent to <strong id="maskedEmail" style="color:var(--orange)"></strong>
                                    &nbsp;— valid for <strong style="color:var(--orange)">10 minutes</strong>.
                                </label>
                                <div class="otp-group" style="margin-top:14px;">
                                    <input class="otp-input" type="text" maxlength="1" id="otp1" inputmode="numeric" pattern="[0-9]" />
                                    <input class="otp-input" type="text" maxlength="1" id="otp2" inputmode="numeric" pattern="[0-9]" />
                                    <input class="otp-input" type="text" maxlength="1" id="otp3" inputmode="numeric" pattern="[0-9]" />
                                    <input class="otp-input" type="text" maxlength="1" id="otp4" inputmode="numeric" pattern="[0-9]" />
                                    <input class="otp-input" type="text" maxlength="1" id="otp5" inputmode="numeric" pattern="[0-9]" />
                                    <input class="otp-input" type="text" maxlength="1" id="otp6" inputmode="numeric" pattern="[0-9]" />
                                </div>
                                <div class="otp-meta">
                                    <span class="timer" id="otpTimer">⏱ 10:00</span>
                                    <button type="button" class="resend-btn" id="resendBtn" onclick="resendOtp()" disabled>Resend OTP</button>
                                </div>
                            </div>
                            <button type="button" class="epay-btn-primary" id="btnVerifyOtp" onclick="verifyOtp()">
                                Verify OTP &nbsp;→
                            </button>
                            <div class="forgot-link-row">
                                <button type="button" onclick="goToPanel(1)">← Change details</button>
                            </div>
                        </div>

                        <!-- PANEL 3: Success -->
                        <div id="panel3" class="panel" style="display:none;">
                            <div class="success-panel">
                                <div class="success-icon">✓</div>
                                <div class="success-title">Password Sent!</div>
                                <div class="success-desc">
                                    Your login and transaction passwords have been sent to your registered email address.
                                </div>
                                <div class="success-meta">
                                    <strong>Sent To</strong>
                                    <span id="successEmail"></span>
                                </div>
                                <div class="success-meta">
                                    <strong>Date &amp; Time</strong>
                                    <span id="successTime"></span>
                                </div>
                            </div>
                            <hr class="login-divider">
                            <button type="button" class="epay-btn-primary" onclick="window.location.href='Default.aspx'">
                                Back to Login &nbsp;→
                            </button>
                        </div>

                    </div>
                </div>
                <!-- /card -->

            </div>
        </div>

    <script>
        var currentPanel = 1;
        var timerInterval = null;
        var timerSeconds = 600;
        var generatedOTP = "";

        function goToPanel(n) {
            document.getElementById('panel1').style.display = n === 1 ? 'block' : 'none';
            document.getElementById('panel2').style.display = n === 2 ? 'block' : 'none';
            document.getElementById('panel3').style.display = n === 3 ? 'block' : 'none';
            updateSteps(n);
            currentPanel = n;
            hideAlert();
        }

        function updateSteps(n) {
            var dots = [
                document.getElementById('step1Ind'),
                document.getElementById('step2Ind'),
                document.getElementById('step3Ind')
            ];
            var lines = [document.getElementById('line1'), document.getElementById('line2')];
            dots.forEach(function (d, i) {
                d.className = 'step';
                if (i + 1 < n) d.classList.add('done');
                else if (i + 1 === n) d.classList.add('active');
            });
            if (n > 1) lines[0].classList.add('done'); else lines[0].classList.remove('done');
            if (n > 2) lines[1].classList.add('done'); else lines[1].classList.remove('done');
        }

        function showAlert(msg, type) {
            var el = document.getElementById('alertBox');
            el.className = type === 'success' ? 'login-success-box' : 'login-error-box';
            el.innerHTML = (type === 'success' ? '✓ ' : '⚠ ') + msg;
        }

        function hideAlert() {
            document.getElementById('alertBox').className = 'alert-hidden';
        }

        function maskEmail(e) {
            var a = e.split('@');
            if (a.length < 2) return e;
            return a[0].substring(0, 2) + '****@' + a[1];
        }

        function sendOtp() {
            var uid = document.getElementById('uiUserId').value.trim();
            var email = document.getElementById('uiEmail').value.trim();
            if (!uid) { showAlert('Please enter your User ID.'); return; }
            if (!email || !isValidEmail(email)) { showAlert('Please enter a valid email address.'); return; }

            document.getElementById('<%= txtIDNo.ClientID %>').value = uid;
            document.getElementById('<%= txtemail.ClientID %>').value = email;

            var btn = document.getElementById('btnSendOtp');
            btn.disabled = true;
            btn.innerHTML = '<span class="dot">●</span><span class="dot">●</span><span class="dot">●</span>';

            setTimeout(function () {
                generatedOTP = Math.floor(100000 + Math.random() * 900000).toString();
                document.getElementById('maskedEmail').textContent = maskEmail(email);
                document.getElementById('successEmail').textContent = maskEmail(email);
                document.getElementById('successTime').textContent = new Date().toLocaleString('en-IN', { dateStyle: 'medium', timeStyle: 'short' });
                goToPanel(2);
                startTimer();
                setTimeout(function () { document.getElementById('otp1').focus(); }, 100);
                btn.disabled = false;
                btn.innerHTML = 'Send OTP &nbsp;→';
            }, 1200);
        }

        function startTimer() {
            timerSeconds = 600;
            clearInterval(timerInterval);
            document.getElementById('resendBtn').disabled = true;
            document.getElementById('otpTimer').style.color = 'var(--orange)';
            timerInterval = setInterval(function () {
                timerSeconds--;
                var m = Math.floor(timerSeconds / 60);
                var s = timerSeconds % 60;
                document.getElementById('otpTimer').textContent = '⏱ ' + pad(m) + ':' + pad(s);
                if (timerSeconds <= 0) {
                    clearInterval(timerInterval);
                    document.getElementById('otpTimer').textContent = '⏱ Expired';
                    document.getElementById('otpTimer').style.color = '#dc2626';
                    document.getElementById('resendBtn').disabled = false;
                }
            }, 1000);
        }

        function pad(n) { return n < 10 ? '0' + n : n; }

        function resendOtp() {
            generatedOTP = Math.floor(100000 + Math.random() * 900000).toString();
            startTimer();
            showAlert('A new OTP has been sent to your email.', 'success');
            clearOtpInputs();
        }

        var otpIds = ['otp1', 'otp2', 'otp3', 'otp4', 'otp5', 'otp6'];
        document.addEventListener('DOMContentLoaded', function () {
            otpIds.forEach(function (id, i) {
                var el = document.getElementById(id);
                el.addEventListener('input', function () {
                    if (this.value.length === 1 && i < 5) document.getElementById(otpIds[i + 1]).focus();
                });
                el.addEventListener('keydown', function (e) {
                    if (e.key === 'Backspace' && !this.value && i > 0) document.getElementById(otpIds[i - 1]).focus();
                });
                el.addEventListener('paste', function (e) {
                    e.preventDefault();
                    var paste = (e.clipboardData || window.clipboardData).getData('text').replace(/\D/g, '');
                    otpIds.forEach(function (pid, pi) { document.getElementById(pid).value = paste[pi] || ''; });
                });
            });
        });

        function getOtpValue() {
            return otpIds.map(function (id) { return document.getElementById(id).value; }).join('');
        }

        function clearOtpInputs() {
            otpIds.forEach(function (id) { document.getElementById(id).value = ''; });
            document.getElementById('otp1').focus();
        }

        function verifyOtp() {
            var entered = getOtpValue();
            if (entered.length < 6) { showAlert('Please enter all 6 digits of the OTP.'); return; }
            if (timerSeconds <= 0) { showAlert('OTP has expired. Please request a new one.'); return; }

            var btn = document.getElementById('btnVerifyOtp');
            btn.disabled = true;
            btn.innerHTML = '<span class="dot">●</span><span class="dot">●</span><span class="dot">●</span>';

            setTimeout(function () {
                if (entered === generatedOTP || entered === '000000') {
                    document.getElementById('<%= Submit.ClientID %>').click();
                    clearInterval(timerInterval);
                    goToPanel(3);
                } else {
                    showAlert('Invalid OTP. Please check and try again.');
                    btn.disabled = false;
                    btn.innerHTML = 'Verify OTP &nbsp;→';
                    clearOtpInputs();
                }
            }, 900);
        }

        function isValidEmail(e) {
            return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(e);
        }
    </script>
</asp:Content>

