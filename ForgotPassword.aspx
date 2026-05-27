<%@ Page Title="Forgot Password" Language="C#" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Forgot Password</title>
    <link href="https://fonts.googleapis.com/css2?family=Sora:wght@400;600;700;800&family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
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
            --border: #E2E8F0;
            --text: #1A1A2E;
            --muted: #6B7280;
        }

        *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

        body {
            min-height: 100vh;
            background: #F0F2F8;
            font-family: 'Nunito', sans-serif;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 24px 16px;
        }

        /* ── MODAL CARD ── */
        .forgot-modal {
            width: 100%;
            max-width: 460px;
            background: #fff;
            border-radius: 20px;
            overflow: hidden;
            box-shadow: 0 24px 60px rgba(0,0,0,0.13);
            border: 1.5px solid var(--border);
            animation: popIn .35s cubic-bezier(.34,1.56,.64,1);
        }

        @keyframes popIn {
            from { opacity:0; transform: scale(0.92) translateY(18px); }
            to   { opacity:1; transform: scale(1)    translateY(0); }
        }

        /* ── HEADER ── */
        .forgot-modal-header {
            background: linear-gradient(135deg, #f97316 0%, #e84000 100%);
            padding: 26px 30px 22px;
            position: relative;
        }

        .forgot-modal-header::before {
            content: '';
            position: absolute;
            top: 0; left: 0; right: 0;
            height: 3px;
            background: linear-gradient(90deg, var(--orange), #FF8C42);
        }

        .header-top {
            display: flex;
            align-items: center;
            gap: 12px;
            margin-bottom: 4px;
        }

        .header-icon {
            width: 42px; height: 42px;
            background: rgba(255,255,255,0.18);
            border-radius: 11px;
            display: flex; align-items: center; justify-content: center;
            font-size: 18px; color: #fff;
            flex-shrink: 0;
        }

        .forgot-modal-header h5 {
            font-family: 'Sora', sans-serif;
            font-size: 1.15rem;
            font-weight: 700;
            color: #fff;
            margin: 0;
        }

        .forgot-modal-header p {
            font-size: .82rem;
            color: rgba(255,255,255,.6);
            margin: 0;
            padding-left: 54px;
        }

        /* ── BODY ── */
        .forgot-modal-body {
            padding: 28px 30px 30px;
        }

        /* ── STEP BAR ── */
        .forgot-step-bar {
            display: flex;
            align-items: center;
            margin-bottom: 26px;
        }

        .fstep {
            display: flex;
            flex-direction: column;
            align-items: center;
            flex: 1;
        }

        .fstep-dot {
            width: 32px; height: 32px;
            border-radius: 50%;
            background: #f1f5f9;
            color: #94a3b8;
            font-size: 12px;
            font-weight: 700;
            display: flex; align-items: center; justify-content: center;
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
            font-weight: 700;
            letter-spacing: .5px;
            text-transform: uppercase;
        }

        .fstep-lbl.active { color: var(--orange); }
        .fstep-lbl.done   { color: var(--green); }

        .fstep-line {
            flex: 1;
            height: 2px;
            background: #e2e8f0;
            margin-bottom: 17px;
            transition: background .3s;
        }

        .fstep-line.done { background: var(--green); }

        /* ── ALERT ── */
        .forgot-alert {
            display: none;
            padding: 10px 14px;
            border-radius: 9px;
            font-size: .82rem;
            font-weight: 600;
            margin-bottom: 16px;
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

        /* ── PANELS ── */
        .forgot-panel { display: none; animation: slideIn .25s ease; }
        .forgot-panel.active { display: block; }

        @keyframes slideIn {
            from { opacity:0; transform: translateX(14px); }
            to   { opacity:1; transform: translateX(0); }
        }

        /* ── FORM ELEMENTS ── */
        .f-group { margin-bottom: 16px; }

        .f-group label {
            display: block;
            font-size: .8rem;
            font-weight: 700;
            color: var(--text);
            margin-bottom: 6px;
            letter-spacing: .3px;
        }

        .f-input {
            width: 100%;
            padding: 11px 15px;
            border: 1.5px solid var(--border);
            border-radius: 10px;
            font-family: 'Nunito', sans-serif;
            font-size: .92rem;
            color: var(--text);
            outline: none;
            transition: border-color .2s, box-shadow .2s;
            background: #fff;
        }

        .f-input:focus {
            border-color: var(--orange);
            box-shadow: 0 0 0 4px var(--orange-glow);
        }

        .f-input::placeholder { color: #B0B7C3; }

        /* ── BUTTON ── */
        .f-btn {
            width: 100%;
            background: var(--orange);
            color: #fff;
            border: none;
            padding: 13px 24px;
            border-radius: 11px;
            font-weight: 800;
            font-size: .92rem;
            cursor: pointer;
            font-family: 'Nunito', sans-serif;
            transition: background .2s, transform .15s, box-shadow .2s;
            margin-top: 18px;
            letter-spacing: .2px;
        }

        .f-btn:hover {
            background: #c93500;
            transform: translateY(-1px);
            box-shadow: 0 8px 22px rgba(232,64,0,.28);
        }

        .f-btn:disabled {
            background: #d1d5db;
            cursor: not-allowed;
            transform: none;
            box-shadow: none;
        }

        /* ── OTP ── */
        .fotp-sent {
            font-size: .83rem;
            color: var(--muted);
            text-align: center;
            margin-bottom: 6px;
        }

        .fotp-sent b { color: var(--text); font-weight: 700; }

        .fotp-inputs {
            display: flex;
            gap: 8px;
            justify-content: center;
            margin: 14px 0 6px;
        }

        .fotp-inputs input {
            width: 46px; height: 52px;
            text-align: center;
            font-size: 22px;
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

        .fotp-resend:hover { text-decoration: underline; }

        /* ── SUCCESS ── */
        .forgot-success {
            text-align: center;
            padding: 8px 0 4px;
        }

        .success-circle {
            width: 70px; height: 70px;
            background: var(--green-light);
            border: 2px solid var(--green-border);
            border-radius: 50%;
            margin: 0 auto 18px;
            display: flex; align-items: center; justify-content: center;
            font-size: 30px;
            animation: popCircle .5s cubic-bezier(.34,1.56,.64,1);
        }

        @keyframes popCircle {
            from { transform: scale(0); }
            to   { transform: scale(1); }
        }

        .forgot-success h5 {
            font-family: 'Sora', sans-serif;
            font-size: 1.1rem;
            color: var(--green);
            font-weight: 700;
            margin-bottom: 10px;
        }

        .forgot-success p {
            font-size: .87rem;
            color: var(--muted);
            line-height: 1.7;
        }

        .forgot-success p b { color: var(--text); }

        /* ── BACK LINK ── */
        .back-link {
            display: block;
            text-align: center;
            margin-top: 20px;
            font-size: .83rem;
            color: var(--muted);
        }

        .back-link a {
            color: var(--orange);
            font-weight: 700;
            text-decoration: none;
        }

        .back-link a:hover { text-decoration: underline; }
    </style>
</head>
<body>

    <form id="form1" runat="server">

        <div class="forgot-modal">

            <!-- Header -->
            <div class="forgot-modal-header">
                <div class="header-top">
                    <div class="header-icon"><i class="fa fa-key"></i></div>
                    <h5>Forgot Password</h5>
                </div>
                <p>Verify your identity to recover your account</p>
            </div>

            <!-- Body -->
            <div class="forgot-modal-body">

                <!-- Step Bar -->
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

                <!-- Alert -->
                <div class="forgot-alert" id="fAlert"></div>

                <!-- Panel 1: Verify -->
                <div class="forgot-panel active" id="fpanel1">
                    <div class="f-group">
                        <label>User ID</label>
                        <input type="text" id="fUserId" class="f-input" placeholder="Enter your User ID" maxlength="15" autocomplete="off" />
                    </div>
                    <div class="f-group" style="margin-bottom:0;">
                        <label>Registered Email</label>
                        <input type="email" id="fEmail" class="f-input" placeholder="Enter registered email" autocomplete="off" />
                    </div>
                    <button type="button" class="f-btn" id="fBtnSend" onclick="fSendOtp()">
                        <i class="fa fa-paper-plane"></i> &nbsp;Send OTP
                    </button>
                </div>

                <!-- Panel 2: OTP -->
                <div class="forgot-panel" id="fpanel2">
                    <p class="fotp-sent">OTP sent to <b id="fMaskedEmail"></b></p>
                    <div class="fotp-inputs">
                        <input type="text" maxlength="1" id="f1" oninput="fotpMove(this,'f2')" onkeydown="fotpBack(event,this,null)" inputmode="numeric" />
                        <input type="text" maxlength="1" id="f2" oninput="fotpMove(this,'f3')" onkeydown="fotpBack(event,this,'f1')"  inputmode="numeric" />
                        <input type="text" maxlength="1" id="f3" oninput="fotpMove(this,'f4')" onkeydown="fotpBack(event,this,'f2')"  inputmode="numeric" />
                        <input type="text" maxlength="1" id="f4" oninput="fotpMove(this,'f5')" onkeydown="fotpBack(event,this,'f3')"  inputmode="numeric" />
                        <input type="text" maxlength="1" id="f5" oninput="fotpMove(this,'f6')" onkeydown="fotpBack(event,this,'f4')"  inputmode="numeric" />
                        <input type="text" maxlength="1" id="f6" oninput="fotpMove(this,null)" onkeydown="fotpBack(event,this,'f5')"  inputmode="numeric" />
                    </div>
                    <div class="fotp-timer">
                        Expires in <span id="fTimer">10:00</span>
                        <button class="fotp-resend" id="fResendBtn" type="button" onclick="fResend()">Resend OTP</button>
                    </div>
                    <button type="button" class="f-btn" id="fBtnVerify" onclick="fVerifyOtp()">
                        <i class="fa fa-check-circle"></i> &nbsp;Verify OTP
                    </button>
                </div>

                <!-- Panel 3: Success -->
                <div class="forgot-panel" id="fpanel3">
                    <div class="forgot-success">
                        <div class="success-circle">
                            <i class="fa fa-check" style="color:#16a34a;"></i>
                        </div>
                        <h5>Password Sent!</h5>
                        <p>
                            Your login and transaction passwords have been sent to <b id="fSuccessEmail"></b>. Please check your inbox.
                        </p>
                    </div>
                   <%-- <button type="button" class="f-btn" onclick="window.location='ForgotPassword.aspx'" style="margin-top:20px;">
                        <i class="fa fa-sign-in"></i> &nbsp;Go to Login
                    </button>--%>
                </div>

                <!-- Back to Login -->
                <%--<div class="back-link">
                    <a href="ForgotPassword.aspx"><i class="fa fa-arrow-left"></i> Back to Login</a>
                </div>--%>

            </div>
            <!-- /body -->
        </div>
        <!-- /modal card -->

        <!-- Hidden ASP.NET Controls -->
        <div style="display:none;">
            <asp:TextBox ID="txtForgotID"       runat="server"></asp:TextBox>
            <asp:TextBox ID="txtForgotEmail"    runat="server"></asp:TextBox>
            <asp:HiddenField ID="HdnForgotStep"   runat="server" Value="0" />
            <asp:HiddenField ID="HdnForgotOTP"    runat="server" />
            <asp:Button ID="BtnForgotSubmit"    runat="server" Text="ForgotGo"
                OnClick="BtnForgotSubmit_Click"     UseSubmitBehavior="false" />
            <asp:Button ID="BtnForgotVerifyOTP" runat="server" Text="ForgotVerify"
                OnClick="BtnForgotVerifyOTP_Click"  UseSubmitBehavior="false" />
            <asp:HiddenField ID="HdnForgotResult" runat="server" Value="" />
        </div>

    </form>

    <script>
        var fTimerInterval = null;
        var fTimerSecs = 600;

        // ── Step indicator ──
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
            [1,2,3].forEach(function(i){ document.getElementById('fpanel'+i).classList.remove('active'); });
            document.getElementById('fpanel' + n).classList.add('active');
            fUpdateSteps(n);
            fHideAlert();
        }

        // ── Alert ──
        function fShowAlert(msg, type) {
            var el = document.getElementById('fAlert');
            el.className = 'forgot-alert ' + (type === 'ok' ? 'ok' : 'err');
            el.innerHTML = (type === 'ok'
                ? '<i class="fa fa-check-circle"></i> '
                : '<i class="fa fa-exclamation-triangle"></i> ') + msg;
        }
        function fHideAlert() {
            var el = document.getElementById('fAlert');
            el.className = 'forgot-alert';
            el.innerHTML = '';
        }

        // ── OTP boxes ──
        function fotpMove(cur, nextId) {
            cur.classList.toggle('filled', cur.value !== '');
            if (cur.value && nextId) document.getElementById(nextId).focus();
        }
        function fotpBack(e, cur, prevId) {
            if (e.key === 'Backspace' && !cur.value && prevId) document.getElementById(prevId).focus();
        }
        function fClearOtp() {
            ['f1','f2','f3','f4','f5','f6'].forEach(function(id){
                var el = document.getElementById(id);
                el.value = '';
                el.classList.remove('filled');
            });
        }
        function fGetOtp() {
            return ['f1','f2','f3','f4','f5','f6'].map(function(id){ return document.getElementById(id).value; }).join('');
        }

        // ── Step 1: Send OTP ──
        function fSendOtp() {
            var uid   = document.getElementById('fUserId').value.trim();
            var email = document.getElementById('fEmail').value.trim();
            if (!uid)   { fShowAlert('Please enter your User ID.'); return; }
            if (!email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email))
                         { fShowAlert('Please enter a valid email address.'); return; }

            var btn = document.getElementById('fBtnSend');
            btn.disabled = true;
            btn.innerHTML = '<i class="fa fa-spinner fa-spin"></i> &nbsp;Sending...';

            document.getElementById('<%= txtForgotID.ClientID %>').value       = uid;
            document.getElementById('<%= txtForgotEmail.ClientID %>').value    = email;
            document.getElementById('<%= HdnForgotStep.ClientID %>').value     = '1';
            document.getElementById('<%= BtnForgotSubmit.ClientID %>').click();
        }

        function fAfterSendResult() {
            var result = document.getElementById('<%= HdnForgotResult.ClientID %>').value;
            if (!result) return;

            var btn = document.getElementById('fBtnSend');
            btn.disabled = false;
            btn.innerHTML = '<i class="fa fa-paper-plane"></i> &nbsp;Send OTP';

            if (result === 'OK') {
                var email = document.getElementById('<%= txtForgotEmail.ClientID %>').value.trim();
                document.getElementById('fMaskedEmail').textContent  = fMaskEmail(email);
                document.getElementById('fSuccessEmail').textContent = fMaskEmail(email);
                fGoPanel(2);
                fStartTimer();
                setTimeout(function(){ document.getElementById('f1').focus(); }, 100);
            } else {
                fShowAlert(result);
            }
            document.getElementById('<%= HdnForgotResult.ClientID %>').value = '';
        }

        // ── Step 2: Verify OTP ──
        function fVerifyOtp() {
            var entered = fGetOtp();
            if (entered.length < 6) { fShowAlert('Please enter all 6 digits.'); return; }
            if (fTimerSecs <= 0)    { fShowAlert('OTP expired. Click Resend.'); return; }

            var btn = document.getElementById('fBtnVerify');
            btn.disabled = true;
            btn.innerHTML = '<i class="fa fa-spinner fa-spin"></i> &nbsp;Verifying...';

            document.getElementById('<%= HdnForgotOTP.ClientID %>').value  = entered;
            document.getElementById('<%= HdnForgotStep.ClientID %>').value = '2';
            document.getElementById('<%= BtnForgotVerifyOTP.ClientID %>').click();
        }

        function fAfterVerifyResult() {
            var result = document.getElementById('<%= HdnForgotResult.ClientID %>').value;
            if (!result) return;

            var btn = document.getElementById('fBtnVerify');
            btn.disabled = false;
            btn.innerHTML = '<i class="fa fa-check-circle"></i> &nbsp;Verify OTP';

            if (result === 'OK') {
                clearInterval(fTimerInterval);
                // Set email in success panel from the hidden textbox (survives postback)
                var email = document.getElementById('<%= txtForgotEmail.ClientID %>').value.trim();
                document.getElementById('fSuccessEmail').textContent = fMaskEmail(email);
                fGoPanel(3);
            } else {
                fShowAlert(result);
                fClearOtp();
                setTimeout(function(){ document.getElementById('f1').focus(); }, 100);
            }
            document.getElementById('<%= HdnForgotResult.ClientID %>').value = '';
        }

        // ── Timer ──
        function fStartTimer() {
            fTimerSecs = 600;
            clearInterval(fTimerInterval);
            document.getElementById('fResendBtn').style.display = 'none';
            var el = document.getElementById('fTimer');
            fTimerInterval = setInterval(function(){
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

        // ── Email mask ──
        function fMaskEmail(e) {
            var a = e.split('@');
            if (a.length < 2) return e;
            return a[0].substring(0, 2) + '****@' + a[1];
        }

        // ── On page load: restore state after postback ──
        window.addEventListener('load', function () {
            var step   = document.getElementById('<%= HdnForgotStep.ClientID %>').value;
            var result = document.getElementById('<%= HdnForgotResult.ClientID %>').value;

            if (step === '1' && result) { fAfterSendResult(); }
            if (step === '2' && result) {
                fGoPanel(2);
                fStartTimer();
                fAfterVerifyResult();
            }

            // Auto-focus first field
            var u = document.getElementById('fUserId');
            if (u) u.focus();
        });
    </script>

</body>
</html>
