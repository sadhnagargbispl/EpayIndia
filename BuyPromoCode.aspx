<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="BuyPromoCode.aspx.cs" Inherits="BuyPromoCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<link href="https://fonts.googleapis.com/css2?family=Sora:wght@400;600;700;800&family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet">
<style>
  :root {
    --orange: #E84000;
    --orange-glow: rgba(232,64,0,0.10);
    --gray: #F5F6FA;
    --border: #E2E8F0;
    --text: #1A1A2E;
    --muted: #6B7280;
  }

  .ep-page-wrap {
    background: #F0F2F8;
    min-height: 80vh;
    padding: 36px 20px;
    font-family: 'Nunito', sans-serif;
  }

  /* ── Page Header Banner ── */
  .ep-page-header {
    max-width: 640px; margin: 0 auto 28px;
    background: linear-gradient(135deg, #0D1117 0%, #1a2744 60%, #0f1923 100%);
    border-radius: 18px; padding: 28px 32px;
    display: flex; align-items: center; justify-content: space-between;
    position: relative; overflow: hidden;
  }
  .ep-page-header::after {
    content: '';
    position: absolute; right: 0; top: 0; bottom: 0; width: 40%;
    background: radial-gradient(ellipse at right, rgba(232,64,0,.2), transparent);
    pointer-events: none;
  }
  .ep-page-header::before {
    content: '';
    position: absolute; top: 0; left: 0; right: 0; height: 3px;
    background: linear-gradient(90deg, var(--orange), #FF8C42);
  }
  .ep-ph-text .ep-badge {
    display: inline-flex; align-items: center; gap: 6px;
    background: rgba(232,64,0,.22); border: 1px solid rgba(232,64,0,.4);
    color: #FF8C42; font-size: .72rem; font-weight: 700; letter-spacing: .4px;
    padding: 4px 12px; border-radius: 50px; margin-bottom: 10px;
  }
  .ep-ph-text .ep-badge span {
    width: 7px; height: 7px; background: #FF8C42;
    border-radius: 50%; display: inline-block;
    animation: epPulse 1.5s infinite;
  }
  @keyframes epPulse {
    0%,100%{transform:scale(1);opacity:1;}
    50%{transform:scale(1.35);opacity:.7;}
  }
  .ep-ph-text h1 {
    font-family: 'Sora', sans-serif; color: #fff;
    font-size: 1.35rem; font-weight: 800; margin-bottom: 4px;
  }
  .ep-ph-text p { color: rgba(255,255,255,.55); font-size: .83rem; margin: 0; }
  .ep-ph-icon { font-size: 2.8rem; position: relative; z-index: 2; }

  /* ── Balance Display ── */
  .ep-balance-card {
    max-width: 640px; margin: 0 auto 22px;
    background: linear-gradient(135deg, #0D1117 0%, #1a2744 100%);
    border-radius: 14px; padding: 20px 24px;
    display: flex; align-items: center; justify-content: space-between;
    border: 1.5px solid rgba(255,255,255,.07);
  }
  .ep-balance-card .bal-label {
    color: rgba(255,255,255,.5); font-size: .8rem; font-weight: 600; margin-bottom: 5px;
    font-family: 'Nunito', sans-serif;
  }
  .ep-balance-card .bal-val {
    font-family: 'Sora', sans-serif; color: #fff;
    font-size: 1.7rem; font-weight: 800; letter-spacing: -1px;
  }
  .ep-balance-card .bal-icon { font-size: 2.2rem; opacity: .45; }

  /* ── Main Card ── */
  .ep-card {
    max-width: 640px; margin: 0 auto;
    background: #fff; border-radius: 18px;
    border: 1.5px solid var(--border);
    box-shadow: 0 4px 24px rgba(0,0,0,.06);
    overflow: hidden;
  }
  .ep-card-header {
    background: linear-gradient(135deg, #0D1117 0%, #1a2744 100%);
    padding: 22px 28px; position: relative; overflow: hidden;
  }
  .ep-card-header::before {
    content: '';
    position: absolute; top: 0; left: 0; right: 0; height: 3px;
    background: linear-gradient(90deg, var(--orange), #FF8C42);
  }
  .ep-card-header h4 {
    font-family: 'Sora', sans-serif; color: #fff;
    font-size: 1.05rem; font-weight: 700; margin: 0;
  }
  .ep-card-body { padding: 28px; }

  /* ── Info Notice ── */
  .ep-notice {
    display: flex; align-items: center; gap: 10px;
    background: var(--orange-glow); border: 1px solid rgba(232,64,0,.2);
    border-radius: 10px; padding: 12px 16px; margin-bottom: 22px;
    font-size: .84rem; font-weight: 700; color: var(--orange);
    font-family: 'Nunito', sans-serif;
  }

  /* ── Form Elements ── */
  .ep-form-group { margin-bottom: 18px; }
  .ep-form-group label {
    display: block; font-weight: 700; font-size: .85rem;
    color: var(--text); margin-bottom: 7px;
    font-family: 'Nunito', sans-serif;
  }
  .ep-form-group .form-control {
    width: 100%; padding: 11px 15px;
    border: 1.5px solid var(--border); border-radius: 10px;
    font-family: 'Nunito', sans-serif; font-size: .9rem;
    color: var(--text); background: #fff; outline: none;
    transition: border-color .2s, box-shadow .2s;
  }
  .ep-form-group .form-control:focus {
    border-color: var(--orange);
    box-shadow: 0 0 0 4px var(--orange-glow);
  }
  .ep-form-group .form-control[disabled],
  .ep-form-group .form-control[readonly] {
    background: var(--gray); color: var(--muted); cursor: not-allowed;
  }
  .ep-form-group .form-control::placeholder { color: #B0B7C3; }

  /* ── Error label ── */
  .ep-err {
    display: block; margin-top: 6px;
    font-size: .8rem; font-weight: 700;
    color: var(--orange); font-family: 'Nunito', sans-serif;
  }

  /* ── Buttons ── */
  .ep-btn-primary {
    width: 100%; background: var(--orange); color: #fff; border: none;
    padding: 13px 26px; border-radius: 11px;
    font-weight: 800; font-size: .92rem; cursor: pointer;
    font-family: 'Nunito', sans-serif;
    transition: background .2s, transform .15s, box-shadow .2s;
    letter-spacing: .2px;
  }
  .ep-btn-primary:hover {
    background: #c93500; transform: translateY(-1px);
    box-shadow: 0 8px 22px rgba(232,64,0,.3);
  }
  .ep-btn-secondary {
    width: 100%; background: transparent; color: var(--text);
    border: 1.5px solid var(--border); padding: 12px 26px;
    border-radius: 11px; font-weight: 700; font-size: .9rem;
    cursor: pointer; font-family: 'Nunito', sans-serif;
    transition: all .2s;
  }
  .ep-btn-secondary:hover { border-color: var(--orange); color: var(--orange); }

  /* ── OTP Section ── */
  .ep-otp-box {
    background: var(--gray); border: 1.5px solid var(--border);
    border-radius: 14px; padding: 22px; margin-top: 20px;
  }
  .ep-otp-box .otp-label {
    font-weight: 700; font-size: .85rem; color: var(--text);
    margin-bottom: 10px; display: block;
    font-family: 'Nunito', sans-serif;
  }
  .ep-otp-box .form-control {
    text-align: center; letter-spacing: 6px;
    font-size: 1.1rem; font-weight: 700;
  }
  .ep-btn-row { display: grid; grid-template-columns: 1fr 1fr; gap: 12px; margin-top: 14px; }

  .ep-divider { border: none; border-top: 1.5px solid var(--border); margin: 22px 0; }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<div class="ep-page-wrap">

  <!-- Page Header -->
  <div class="ep-page-header">
    <div class="ep-ph-text">
      <div class="ep-badge"><span></span> Digital Vouchers</div>
      <h1>Buy PromoCode</h1>
      <p>Purchase promo codes using your wallet balance</p>
    </div>
    <div class="ep-ph-icon">🎟️</div>
  </div>

  <!-- Balance Display -->
  <div class="ep-balance-card">
    <div>
      <div class="bal-label">Available Balance</div>
      <asp:TextBox ID="TxtCredit" runat="server"
        CssClass="bal-val"
        ReadOnly="True"
        style="background:transparent;border:none;color:#fff;font-family:'Sora',sans-serif;font-size:1.7rem;font-weight:800;letter-spacing:-1px;padding:0;width:auto;">
      </asp:TextBox>
    </div>
    <div class="bal-icon">💰</div>
  </div>

  <!-- Main Card -->
  <div class="ep-card">
    <div class="ep-card-header">
      <h4>Buy PromoCode</h4>
    </div>
    <div class="ep-card-body">

      <!-- Minimum notice -->
      <div class="ep-notice">
        ⚠️ &nbsp;Minimum purchase requirement: 5 coupons
      </div>

      <!-- Error label -->
      <asp:Label ID="Label2" runat="server" CssClass="ep-err"
        ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>

      <!-- Hidden fields -->
      <asp:HiddenField ID="hdnSessn" runat="server" />
      <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
      <asp:DropDownList CssClass="form-control" ID="DDlCode" runat="server"
        style="display:none;"></asp:DropDownList>

      <!-- Amount -->
      <div class="ep-form-group">
        <label>Amount per Coupon</label>
        <asp:TextBox ID="TxtAmount" runat="server"
          CssClass="form-control" Enabled="False"
          placeholder="₹ 0.00">
        </asp:TextBox>
      </div>

      <!-- Quantity -->
      <div class="ep-form-group">
        <label>Quantity <span style="color:var(--orange);">*</span></label>
        <asp:TextBox ID="Txtqty" runat="server"
          CssClass="form-control"
          TabIndex="3"
          ValidationGroup="eInformation"
          OnTextChanged="Txtqty_TextChanged"
          AutoPostBack="true"
          placeholder="Enter quantity (min 5)">
        </asp:TextBox>
        <asp:Label ID="errMsg" runat="server"
          CssClass="ep-err"
          ForeColor="Red" Font-Bold="true" Font-Size="Small">
        </asp:Label>
      </div>

      <!-- Final Amount -->
      <div class="ep-form-group">
        <label>Final Amount</label>
        <asp:TextBox ID="TxtFinalAmount" runat="server"
          CssClass="form-control" Enabled="False" TabIndex="8"
          placeholder="₹ 0.00">
        </asp:TextBox>
      </div>

      <!-- Send OTP Button -->
      <div class="ep-form-group">
        <asp:Button ID="CmdSave" runat="server"
          Text="Send OTP  →"
          CssClass="ep-btn-primary"
          TabIndex="27"
          ValidationGroup="eInformation"
          OnClick="CmdSave_Click" />
        <asp:Label ID="Label1" runat="server"
          CssClass="ep-err"
          ForeColor="Red" Font-Bold="true" Font-Size="Small">
          Minimum purchase requirement: 5 coupons.
        </asp:Label>
      </div>

      <!-- OTP Section (hidden until Send OTP clicked) -->
      <div runat="server" id="divotp" visible="false">
        <div class="ep-otp-box">
          <span class="otp-label">Enter OTP sent to your registered mobile</span>
          <asp:TextBox ID="TxtOtp" runat="server"
            CssClass="form-control"
            autocomplete="off"
            placeholder="• • • • • •"
            MaxLength="6">
          </asp:TextBox>
          <div class="ep-btn-row">
            <asp:Button ID="BtnOtp" runat="server"
              Text="Submit"
              CssClass="ep-btn-primary"
              Visible="false"
              OnClick="BtnOtp_Click" />
            <asp:Button ID="ResendOtp" runat="server"
              Text="Resend OTP"
              CssClass="ep-btn-secondary"
              Visible="false"
              OnClick="ResendOtp_Click" />
          </div>
        </div>
      </div>

      <hr class="ep-divider">

    </div>
  </div>

</div>

</asp:Content>
