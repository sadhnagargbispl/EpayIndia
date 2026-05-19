<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="thankyou.aspx.cs" Inherits="thankyou" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>
    * { box-sizing: border-box; margin: 0; padding: 0; }

    .ty-page {
        font-family: 'Segoe UI', Arial, sans-serif;
        background: #f0f2f7;
        min-height: 100vh;
    }

    /* ── Topbar ── */
    .ty-topbar {
        background: #1b2e6b;
        height: 48px;
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 0 24px;
    }
    .ty-logo {
        display: flex;
        align-items: center;
        gap: 8px;
        color: #fff;
        font-size: 16px;
        font-weight: 600;
        text-decoration: none;
    }
    .ty-logo-box {
        width: 26px;
        height: 26px;
        background: rgba(255,255,255,0.18);
        border-radius: 6px;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    .ty-logo-box svg { display: block; }
    .ty-topbar-nav {
        display: flex;
        gap: 20px;
        list-style: none;
    }
    .ty-topbar-nav a {
        color: rgba(255,255,255,0.80);
        font-size: 12.5px;
        text-decoration: none;
        display: flex;
        align-items: center;
        gap: 5px;
        transition: color 0.15s;
    }
    .ty-topbar-nav a:hover { color: #fff; }
    .ty-topbar-nav svg { width: 13px; height: 13px; }

    /* ── Wrapper ── */
    .ty-wrap {
        max-width: 620px;
        margin: 28px auto;
        padding: 0 14px 48px;
    }

    /* ── Hero card ── */
    .ty-hero-card {
        background: linear-gradient(135deg, #e8edf8 0%, #dff0f4 100%);
        border-radius: 12px;
        padding: 30px 28px;
        display: flex;
        align-items: center;
        gap: 22px;
        margin-bottom: 14px;
        border: 1px solid #d0d9ee;
    }
    .ty-hero-icon {
        width: 62px;
        height: 62px;
        border-radius: 50%;
        background: #1b2e6b;
        display: flex;
        align-items: center;
        justify-content: center;
        flex-shrink: 0;
    }
    .ty-hero-icon svg { width: 28px; height: 28px; }
    .ty-hero-card h1 {
        font-size: 20px;
        font-weight: 700;
        color: #1b2e6b;
        margin-bottom: 8px;
        line-height: 1.25;
    }
    .ty-hero-card p {
        font-size: 13.5px;
        color: #4a5578;
        line-height: 1.6;
    }
    .ty-hero-card p b { color: #1b2e6b; font-weight: 600; }

    /* ── Order detail card ── */
    .ty-order-card {
        background: #fff;
        border-radius: 10px;
        border: 1px solid #dde2ef;
        overflow: hidden;
        margin-bottom: 14px;
    }
    .ty-order-grid {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 1px;
        background: #dde2ef;
    }
    .ty-order-cell {
        background: #fff;
        padding: 16px 18px;
    }
    .ty-order-cell:hover { background: #fafbfe; }
    .ty-cell-label {
        font-size: 10px;
        font-weight: 700;
        color: #8b96b4;
        text-transform: uppercase;
        letter-spacing: 0.7px;
        margin-bottom: 6px;
    }
    .ty-cell-value {
        font-size: 14px;
        font-weight: 500;
        color: #1e2a45;
    }
    .ty-confirmed {
        color: #1a9e6e;
        font-weight: 600;
        font-size: 14px;
    }

    /* ── Buttons ── */
    .ty-btn-row {
        padding: 16px 18px;
        display: flex;
        gap: 10px;
        flex-wrap: wrap;
    }
    .ty-btn-outline {
        background: #fff;
        color: #1b2e6b;
        border: 1px solid #c4cce0;
        border-radius: 7px;
        padding: 9px 18px;
        font-size: 13px;
        font-weight: 500;
        font-family: inherit;
        cursor: pointer;
        display: inline-flex;
        align-items: center;
        gap: 7px;
        text-decoration: none;
        transition: background 0.15s, border-color 0.15s;
    }
    .ty-btn-outline:hover {
        background: #eef2ff;
        border-color: #1b2e6b;
        color: #1b2e6b;
    }
    .ty-btn-outline svg { width: 14px; height: 14px; flex-shrink: 0; }

    /* ── Progress card ── */
    .ty-progress-card {
        background: #fff;
        border-radius: 10px;
        border: 1px solid #dde2ef;
        padding: 20px 18px 22px;
    }
    .ty-progress-label {
        font-size: 10px;
        font-weight: 700;
        color: #8b96b4;
        text-transform: uppercase;
        letter-spacing: 0.7px;
        margin-bottom: 18px;
    }
    .ty-progress-row {
        display: flex;
        align-items: flex-start;
        position: relative;
    }
    .ty-progress-row::before {
        content: '';
        position: absolute;
        top: 16px;
        left: 16px;
        right: 16px;
        height: 2px;
        background: #e3e7f0;
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
    }
    .ty-p-dot {
        width: 32px;
        height: 32px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        margin-bottom: 8px;
        font-size: 13px;
        font-weight: 700;
    }
    .ty-p-dot svg { width: 15px; height: 15px; }
    .ty-p-dot.done   { background: #1b2e6b; }
    .ty-p-dot.active { background: #1a9e6e; }
    .ty-p-dot.idle   { background: #fff; border: 2px solid #d0d6e8; color: #9aa3be; }
    .ty-p-name {
        font-size: 11px;
        line-height: 1.4;
        max-width: 68px;
    }
    .ty-p-name.done   { color: #1e2a45; font-weight: 500; }
    .ty-p-name.active { color: #1a9e6e; font-weight: 600; }
    .ty-p-name.idle   { color: #9aa3be; }

    /* ── Footer trust ── */
    .ty-trust {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 18px;
        margin-top: 22px;
        font-size: 11.5px;
        color: #9aa3be;
    }
    .ty-trust span { display: flex; align-items: center; gap: 5px; }
    .ty-trust svg { width: 13px; height: 13px; }

    @media (max-width: 480px) {
        .ty-order-grid { grid-template-columns: 1fr; }
        .ty-hero-card { flex-direction: column; text-align: center; }
        .ty-hero-card h1 { font-size: 17px; }
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="ty-page">

    <!-- Topbar -->
    <div class="ty-topbar">
        <a href="/" class="ty-logo">
            <div class="ty-logo-box">
                <svg width="13" height="13" viewBox="0 0 14 14" fill="none">
                    <path d="M2 7L5.5 10.5L12 3.5" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
            </div>
            ePay
        </a>
        <ul class="ty-topbar-nav">
            <li>
                <a href="#">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="10"/><path d="M9.09 9a3 3 0 0 1 5.83 1c0 2-3 3-3 3"/><line x1="12" y1="17" x2="12.01" y2="17"/></svg>
                    Help Center
                </a>
            </li>
            <li>
                <a href="#">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="1" y="3" width="15" height="13" rx="2"/><path d="M16 8h4l3 5v3h-7V8z"/><circle cx="5.5" cy="18.5" r="2.5"/><circle cx="18.5" cy="18.5" r="2.5"/></svg>
                    Track Order
                </a>
            </li>
        </ul>
    </div>

    <!-- Content -->
    <div class="ty-wrap">

        <!-- Hero card -->
        <div class="ty-hero-card">
            <div class="ty-hero-icon">
                <svg viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
                    <polyline points="20 6 9 17 4 12"/>
                </svg>
            </div>
            <div>
                <h1>Thank you for your purchase!</h1>
                <p>Your order for <b><asp:Label ID="LblPackageNameHero" runat="server" Text=""></asp:Label></b> has been confirmed and is ready to redeem.</p>
            </div>
        </div>

        <!-- Order detail card -->
        <div class="ty-order-card">
            <div class="ty-order-grid">
                <div class="ty-order-cell">
                    <div class="ty-cell-label">Order Number</div>
                    <div class="ty-cell-value">#<asp:Label ID="LblOrderNumber" runat="server" Text=""></asp:Label></div>
                </div>
                <div class="ty-order-cell">
                    <div class="ty-cell-label">Order Date</div>
                    <div class="ty-cell-value"><asp:Label ID="LblOrderDate" runat="server" Text=""></asp:Label></div>
                </div>
                <div class="ty-order-cell">
                    <div class="ty-cell-label">Package</div>
                    <div class="ty-cell-value"><asp:Label ID="LblPackageName" runat="server" Text=""></asp:Label></div>
                </div>
                <div class="ty-order-cell">
                    <div class="ty-cell-label">Status</div>
                    <div class="ty-confirmed">Confirmed</div>
                </div>
            </div>
            <div class="ty-btn-row">
                <asp:Button ID="BtnRedeemNow" runat="server" CssClass="ty-btn-outline" OnClick="BtnProceedToPay_Click" Text="Redeem now" />
              <%--  <a href="#" class="ty-btn-outline">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><polyline points="7 10 12 15 17 10"/><line x1="12" y1="15" x2="12" y2="3"/></svg>
                    Download receipt
                </a>--%>
            </div>
        </div>

        <!-- Progress card -->
    <%--    <div class="ty-progress-card">
            <div class="ty-progress-label">Order Progress</div>
            <div class="ty-progress-row">
                <div class="ty-p-step">
                    <div class="ty-p-dot done">
                        <svg viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><polyline points="20 6 9 17 4 12"/></svg>
                    </div>
                    <div class="ty-p-name done">Order placed</div>
                </div>
                <div class="ty-p-step">
                    <div class="ty-p-dot done">
                        <svg viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"><polyline points="20 6 9 17 4 12"/></svg>
                    </div>
                    <div class="ty-p-name done">Payment confirmed</div>
                </div>
                <div class="ty-p-step">
                    <div class="ty-p-dot active">
                        <svg viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polyline points="20 12 20 22 4 22 4 12"/><polyline points="22 7 12 2 2 7"/><line x1="12" y1="22" x2="12" y2="7"/></svg>
                    </div>
                    <div class="ty-p-name active">Ready to redeem</div>
                </div>
                <div class="ty-p-step">
                    <div class="ty-p-dot idle">4</div>
                    <div class="ty-p-name idle">Used</div>
                </div>
            </div>
        </div>--%>

        <!-- Trust badges -->
   <%--     <div class="ty-trust">
            <span>
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="3" y="11" width="18" height="11" rx="2"/><path d="M7 11V7a5 5 0 0 1 10 0v4"/></svg>
                SSL Secured
            </span>
            <span>
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/><polyline points="9 12 11 14 15 10"/></svg>
                PCI DSS Certified
            </span>
            <span>ePay Digital India Pvt. Ltd.</span>
        </div>--%>

    </div>
</div>

<script>
    document.addEventListener('DOMContentLoaded', function () {
        var btn = document.getElementById('<%= BtnRedeemNow.ClientID %>');
        if (btn) {
            btn.innerHTML = '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" style="vertical-align:-2px;margin-right:5px"><polyline points="20 12 20 22 4 22 4 12"/><polyline points="22 7 12 2 2 7"/><line x1="12" y1="22" x2="12" y2="7"/></svg>Redeem now';
        }
    });
</script>
</asp:Content>
