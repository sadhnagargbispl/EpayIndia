<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="AppThankYou.aspx.cs" Inherits="AppThankYou" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="demoepay/css/mobile-app.css?v=1.5" rel="stylesheet" />
    <style>
        /* ═══ THANK YOU PAGE SPECIFIC ═══ */

        /* Success Hero */
        .ty-success-hero {
            background: linear-gradient(135deg, #1A1A2E 0%, #2d1b4e 55%, #1a3050 100%);
            padding: 30px 16px 36px;
            position: relative;
            overflow: hidden;
            text-align: center;
        }

        .ty-success-hero::before {
            content: '';
            position: absolute;
            top: -80px;
            right: -60px;
            width: 220px;
            height: 220px;
            border-radius: 50%;
            background: radial-gradient(circle, rgba(22,163,74,.25) 0%, transparent 70%);
        }

        .ty-success-hero::after {
            content: '';
            position: absolute;
            bottom: -50px;
            left: -40px;
            width: 160px;
            height: 160px;
            border-radius: 50%;
            background: radial-gradient(circle, rgba(232,64,0,.18) 0%, transparent 70%);
        }

        /* Animated check circle */
        .ty-check-wrap {
            position: relative;
            z-index: 1;
            display: inline-flex;
            margin-bottom: 16px;
        }

        .ty-check-circle {
            width: 84px;
            height: 84px;
            border-radius: 50%;
            background: linear-gradient(135deg, #16a34a, #22c55e);
            display: flex;
            align-items: center;
            justify-content: center;
            box-shadow: 0 8px 30px rgba(22,163,74,.45),
                        0 0 0 8px rgba(22,163,74,.18),
                        0 0 0 16px rgba(22,163,74,.08);
            animation: ty-pop 0.5s cubic-bezier(.34,1.56,.64,1) both;
        }

        .ty-check-circle i {
            color: #fff;
            font-size: 2.4rem;
            animation: ty-check 0.6s ease 0.3s both;
        }

        @keyframes ty-pop {
            0% { transform: scale(0); }
            100% { transform: scale(1); }
        }

        @keyframes ty-check {
            0% { transform: scale(0); opacity: 0; }
            100% { transform: scale(1); opacity: 1; }
        }

        /* Ripple rings */
        .ty-ripple {
            position: absolute;
            inset: 0;
            border-radius: 50%;
            border: 2px solid rgba(22,163,74,.4);
            animation: ty-ripple 2s ease-out infinite;
        }

        .ty-ripple.r2 { animation-delay: 0.7s; }

        @keyframes ty-ripple {
            0% { transform: scale(1); opacity: 1; }
            100% { transform: scale(1.8); opacity: 0; }
        }

        .ty-hero-title {
            color: #fff;
            font-size: 1.5rem;
            font-weight: 800;
            position: relative;
            z-index: 1;
            line-height: 1.2;
            margin-bottom: 8px;
        }

        .ty-hero-title span {
            color: var(--accent);
        }

        .ty-hero-sub {
            color: rgba(255,255,255,.7);
            font-size: .85rem;
            position: relative;
            z-index: 1;
            line-height: 1.5;
            max-width: 320px;
            margin: 0 auto;
        }

        .ty-hero-sub b {
            color: #fff;
            font-weight: 700;
        }

        /* Status badge */
        .ty-status-badge {
            display: inline-flex;
            align-items: center;
            gap: 7px;
            background: rgba(22,163,74,.18);
            border: 1px solid rgba(34,197,94,.4);
            color: #4ade80;
            font-size: .72rem;
            font-weight: 700;
            letter-spacing: .8px;
            text-transform: uppercase;
            padding: 6px 14px;
            border-radius: 50px;
            margin-top: 14px;
            position: relative;
            z-index: 1;
        }

        .ty-status-badge i {
            font-size: .65rem;
            animation: blink 1.4s infinite;
        }

        /* Order Card */
        .ty-order-card {
            margin: -18px 16px 14px;
            background: #fff;
            border-radius: var(--radius);
            box-shadow: 0 6px 24px rgba(0,0,0,.10);
            overflow: hidden;
            position: relative;
            z-index: 2;
        }

        .ty-order-header {
            padding: 16px 18px;
            background: linear-gradient(135deg, #fff7ed, #ffedd5);
            border-bottom: 1px dashed rgba(232,64,0,.25);
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 12px;
        }

        .ty-order-header-left {
            display: flex;
            align-items: center;
            gap: 10px;
            min-width: 0;
        }

        .ty-order-header-icon {
            width: 38px;
            height: 38px;
            border-radius: 10px;
            background: var(--primary);
            color: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1rem;
            flex-shrink: 0;
        }

        .ty-order-header-title {
            font-size: .68rem;
            font-weight: 700;
            color: var(--muted);
            text-transform: uppercase;
            letter-spacing: .5px;
        }

        .ty-order-header-num {
            font-size: .95rem;
            font-weight: 800;
            color: var(--text);
            margin-top: 2px;
        }

        .ty-confirmed-pill {
            background: rgba(22,163,74,.12);
            border: 1px solid rgba(22,163,74,.3);
            color: var(--green);
            font-size: .68rem;
            font-weight: 800;
            padding: 5px 11px;
            border-radius: 50px;
            display: inline-flex;
            align-items: center;
            gap: 5px;
            flex-shrink: 0;
        }

        /* Order Details Grid */
        .ty-order-body {
            padding: 16px 18px;
        }

        .ty-detail-row {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 11px 0;
            border-bottom: 1px dashed var(--border);
        }

        .ty-detail-row:last-child {
            border-bottom: none;
            padding-bottom: 0;
        }

        .ty-detail-row:first-child {
            padding-top: 0;
        }

        .ty-detail-icon {
            width: 34px;
            height: 34px;
            border-radius: 10px;
            background: rgba(232,64,0,.1);
            color: var(--primary);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: .85rem;
            flex-shrink: 0;
        }

        .ty-detail-body {
            flex: 1;
            min-width: 0;
        }

        .ty-detail-label {
            font-size: .65rem;
            font-weight: 700;
            color: var(--muted);
            text-transform: uppercase;
            letter-spacing: .5px;
            margin-bottom: 2px;
        }

        .ty-detail-value {
            font-size: .88rem;
            font-weight: 700;
            color: var(--text);
            word-break: break-word;
        }

        /* Action Buttons */
        .ty-actions {
            padding: 0 16px;
            display: flex;
            flex-direction: column;
            gap: 10px;
            margin-bottom: 14px;
        }

        .ty-btn-primary {
            background: var(--primary);
            color: #fff;
            border: none;
            padding: 14px 0;
            border-radius: 50px;
            font-size: .92rem;
            font-weight: 800;
            cursor: pointer;
            text-decoration: none;
            text-align: center;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            box-shadow: 0 6px 18px rgba(232,64,0,.32);
            transition: all .2s;
            width: 100%;
            font-family: inherit;
        }

        .ty-btn-primary:hover {
            background: var(--primary-d);
            transform: translateY(-1px);
            color: #fff;
            box-shadow: 0 10px 24px rgba(232,64,0,.42);
        }

        .ty-btn-outline {
            background: #fff;
            color: var(--text);
            border: 1.5px solid var(--border);
            padding: 12px 0;
            border-radius: 50px;
            font-size: .88rem;
            font-weight: 700;
            cursor: pointer;
            text-decoration: none;
            text-align: center;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            transition: all .2s;
            width: 100%;
            font-family: inherit;
        }

        .ty-btn-outline:hover {
            background: #f9fafb;
            border-color: var(--primary);
            color: var(--primary);
        }

        /* Progress Tracker */
        .ty-progress-card {
            margin: 0 16px 14px;
            background: #fff;
            border-radius: var(--radius);
            padding: 18px;
            box-shadow: var(--shadow);
        }

        .ty-progress-title {
            font-size: .72rem;
            font-weight: 800;
            letter-spacing: .8px;
            text-transform: uppercase;
            color: var(--muted);
            margin-bottom: 18px;
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .ty-progress-title i {
            color: var(--primary);
        }

        .ty-progress-row {
            display: flex;
            position: relative;
            justify-content: space-between;
        }

        .ty-progress-row::before {
            content: '';
            position: absolute;
            top: 14px;
            left: 14px;
            right: 14px;
            height: 2px;
            background: var(--border);
            z-index: 0;
        }

        .ty-progress-row::after {
            content: '';
            position: absolute;
            top: 14px;
            left: 14px;
            width: calc(66.66% - 14px);
            height: 2px;
            background: linear-gradient(90deg, var(--green), var(--green) 50%, var(--primary));
            z-index: 0;
        }

        .ty-p-step {
            flex: 1;
            display: flex;
            flex-direction: column;
            align-items: center;
            text-align: center;
            position: relative;
            z-index: 1;
            gap: 8px;
        }

        .ty-p-dot {
            width: 30px;
            height: 30px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: .75rem;
            font-weight: 800;
            flex-shrink: 0;
        }

        .ty-p-dot.done {
            background: var(--green);
            color: #fff;
            box-shadow: 0 0 0 4px rgba(22,163,74,.15);
        }

        .ty-p-dot.active {
            background: var(--primary);
            color: #fff;
            box-shadow: 0 0 0 4px rgba(232,64,0,.18);
            animation: ty-pulse 1.6s infinite;
        }

        @keyframes ty-pulse {
            0%, 100% { box-shadow: 0 0 0 4px rgba(232,64,0,.18); }
            50% { box-shadow: 0 0 0 8px rgba(232,64,0,.08); }
        }

        .ty-p-dot.idle {
            background: #fff;
            border: 2px solid var(--border);
            color: #9ca3af;
        }

        .ty-p-name {
            font-size: .65rem;
            font-weight: 700;
            line-height: 1.3;
            max-width: 75px;
        }

        .ty-p-name.done { color: var(--text); }
        .ty-p-name.active { color: var(--primary); }
        .ty-p-name.idle { color: #9ca3af; }

        /* Trust badges */
        .ty-trust {
            margin: 4px 16px 14px;
            padding: 14px;
            background: #fff;
            border-radius: var(--radius);
            box-shadow: var(--shadow);
            display: flex;
            align-items: center;
            justify-content: space-around;
            gap: 8px;
        }

        .ty-trust-item {
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 6px;
            text-align: center;
            flex: 1;
        }

        .ty-trust-icon {
            width: 34px;
            height: 34px;
            border-radius: 10px;
            background: rgba(22,163,74,.1);
            color: var(--green);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: .85rem;
        }

        .ty-trust-text {
            font-size: .62rem;
            font-weight: 700;
            color: var(--muted);
            text-transform: uppercase;
            letter-spacing: .4px;
            line-height: 1.2;
        }

        .ty-trust-div {
            width: 1px;
            height: 36px;
            background: var(--border);
        }

        /* Need help banner */
        .ty-help {
            margin: 0 16px 14px;
            background: linear-gradient(135deg, #1A1A2E, #2d1b4e);
            border-radius: var(--radius);
            padding: 16px;
            display: flex;
            align-items: center;
            gap: 12px;
            color: #fff;
        }

        .ty-help-icon {
            width: 40px;
            height: 40px;
            border-radius: 10px;
            background: rgba(255,255,255,.12);
            color: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1rem;
            flex-shrink: 0;
        }

        .ty-help-text {
            flex: 1;
            font-size: .8rem;
            line-height: 1.4;
            font-weight: 600;
        }

        .ty-help-text small {
            display: block;
            color: rgba(255,255,255,.6);
            font-size: .68rem;
            font-weight: 500;
            margin-top: 2px;
        }

        .ty-help-btn {
            background: var(--primary);
            color: #fff;
            border: none;
            padding: 8px 14px;
            border-radius: 50px;
            font-size: .72rem;
            font-weight: 800;
            cursor: pointer;
            text-decoration: none;
            flex-shrink: 0;
            transition: background .2s;
        }

        .ty-help-btn:hover {
            background: var(--primary-d);
            color: #fff;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- SUCCESS HERO -->
    <div class="ty-success-hero">
        <div class="ty-check-wrap">
            <div class="ty-check-circle">
                <i class="fa fa-check"></i>
            </div>
            <div class="ty-ripple"></div>
            <div class="ty-ripple r2"></div>
        </div>
        <div class="ty-hero-title">Payment <span>Successful!</span></div>
        <div class="ty-hero-sub">
            Your order for <b><asp:Label ID="LblPackageNameHero" runat="server" Text="Food / Movie Package"></asp:Label></b> has been confirmed and is ready to redeem.
        </div>
        <div class="ty-status-badge">
            <i class="fa fa-circle"></i> Order Confirmed
        </div>
    </div>

    <!-- ORDER CARD -->
    <div class="ty-order-card">
        <div class="ty-order-header">
            <div class="ty-order-header-left">
                <div class="ty-order-header-icon"><i class="fa fa-receipt"></i></div>
                <div>
                    <div class="ty-order-header-title">Order Number</div>
                    <div class="ty-order-header-num">#<asp:Label ID="LblOrderNumber" runat="server" Text="EP123456789"></asp:Label></div>
                </div>
            </div>
            <div class="ty-confirmed-pill">
                <i class="fa fa-check-circle"></i> Confirmed
            </div>
        </div>

        <div class="ty-order-body">
            <div class="ty-detail-row">
                <div class="ty-detail-icon"><i class="fa fa-calendar-alt"></i></div>
                <div class="ty-detail-body">
                    <div class="ty-detail-label">Order Date</div>
                    <div class="ty-detail-value"><asp:Label ID="LblOrderDate" runat="server" Text=""></asp:Label></div>
                </div>
            </div>

            <div class="ty-detail-row">
                <div class="ty-detail-icon"><i class="fa fa-ticket-alt"></i></div>
                <div class="ty-detail-body">
                    <div class="ty-detail-label">Package</div>
                    <div class="ty-detail-value"><asp:Label ID="LblPackageName" runat="server" Text=""></asp:Label></div>
                </div>
            </div>

            <div class="ty-detail-row">
                <div class="ty-detail-icon"><i class="fa fa-wallet"></i></div>
                <div class="ty-detail-body">
                    <div class="ty-detail-label">Amount Paid</div>
                    <div class="ty-detail-value" style="color:var(--primary);">₹<asp:Label ID="Label1" runat="server" Text=""></asp:Label></div>
                </div>
            </div>
        </div>
    </div>

    <!-- ACTION BUTTONS -->
    <div class="ty-actions">
        <asp:LinkButton ID="BtnRedeemNow" runat="server" CssClass="ty-btn-primary" OnClick="BtnProceedToPay_Click">
            <i class="fa fa-bolt"></i> Redeem Now
        </asp:LinkButton>
     <%--   <a href="#" class="ty-btn-outline">
            <i class="fa fa-download"></i> Download Receipt
        </a>--%>
    </div>

    <!-- PROGRESS TRACKER -->
<%--    <div class="ty-progress-card">
        <div class="ty-progress-title">
            <i class="fa fa-route"></i> Order Progress
        </div>
        <div class="ty-progress-row">
            <div class="ty-p-step">
                <div class="ty-p-dot done"><i class="fa fa-check"></i></div>
                <div class="ty-p-name done">Order Placed</div>
            </div>
            <div class="ty-p-step">
                <div class="ty-p-dot done"><i class="fa fa-check"></i></div>
                <div class="ty-p-name done">Payment Done</div>
            </div>
            <div class="ty-p-step">
                <div class="ty-p-dot active"><i class="fa fa-bolt"></i></div>
                <div class="ty-p-name active">Ready to Redeem</div>
            </div>
            <div class="ty-p-step">
                <div class="ty-p-dot idle">4</div>
                <div class="ty-p-name idle">Used</div>
            </div>
        </div>
    </div>--%>

    <!-- TRUST BADGES -->
  <%--  <div class="ty-trust">
        <div class="ty-trust-item">
            <div class="ty-trust-icon"><i class="fa fa-lock"></i></div>
            <div class="ty-trust-text">SSL<br/>Secured</div>
        </div>
        <div class="ty-trust-div"></div>
        <div class="ty-trust-item">
            <div class="ty-trust-icon"><i class="fa fa-shield-alt"></i></div>
            <div class="ty-trust-text">PCI DSS<br/>Certified</div>
        </div>
        <div class="ty-trust-div"></div>
        <div class="ty-trust-item">
            <div class="ty-trust-icon"><i class="fa fa-award"></i></div>
            <div class="ty-trust-text">Verified<br/>Partner</div>
        </div>
    </div>--%>

    <!-- NEED HELP -->
<%--    <div class="ty-help">
        <div class="ty-help-icon"><i class="fa fa-headset"></i></div>
        <div class="ty-help-text">
            Need help with your order?
            <small>Our team is here 24/7</small>
        </div>
        <a href="#" class="ty-help-btn">Contact</a>
    </div>--%>

    <div style="height:16px"></div>

</asp:Content>
