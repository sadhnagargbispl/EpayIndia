<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="CouponReport.aspx.cs" Inherits="CouponReport" %>

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

  /* ── Page Header ── */
  .ep-page-header {
    max-width: 1100px; margin: 0 auto 28px;
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

  /* ── Stats Row ── */
  .ep-stats-row {
    max-width: 1100px; margin: 0 auto 24px;
    display: grid; grid-template-columns: repeat(3, 1fr); gap: 16px;
  }
  .ep-stat-card {
    background: #fff; border-radius: 14px;
    border: 1.5px solid var(--border); padding: 18px 20px;
    text-align: center;
    box-shadow: 0 2px 12px rgba(0,0,0,.04);
  }
  .ep-stat-card .s-icon { font-size: 1.5rem; margin-bottom: 7px; }
  .ep-stat-card .s-num {
    font-family: 'Sora', sans-serif; font-size: 1.5rem; font-weight: 800;
    color: var(--orange); display: block; margin-bottom: 3px;
  }
  .ep-stat-card .s-label {
    font-size: .79rem; color: var(--muted); font-weight: 600;
  }

  /* ── Main Card ── */
  .ep-card {
    max-width: 1100px; margin: 0 auto;
    background: #fff; border-radius: 18px;
    border: 1.5px solid var(--border);
    box-shadow: 0 4px 24px rgba(0,0,0,.06);
    overflow: hidden;
  }
  .ep-card-header {
    background: linear-gradient(135deg, #0D1117 0%, #1a2744 100%);
    padding: 18px 28px; position: relative; overflow: hidden;
    display: flex; align-items: center; justify-content: space-between;
    flex-wrap: wrap; gap: 12px;
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
  .ep-search-wrap {
    position: relative; display: flex; align-items: center;
  }
  .ep-search-wrap .search-icon {
    position: absolute; left: 11px; color: var(--muted); font-size: .9rem;
  }
  .ep-search-input {
    padding: 8px 13px 8px 32px; border: 1.5px solid var(--border);
    border-radius: 9px; font-family: 'Nunito', sans-serif;
    font-size: .83rem; color: var(--text); background: #fff;
    outline: none; width: 200px; transition: border-color .2s;
  }
  .ep-search-input:focus { border-color: var(--orange); }

  .ep-card-body { padding: 0; }

  /* ── GridView Table Styles ── */
  .ep-table-wrap { overflow-x: auto; }

  .ep-grid-table {
    width: 100%; border-collapse: collapse;
    font-family: 'Nunito', sans-serif; font-size: .87rem;
    min-width: 600px;
  }
  .ep-grid-table thead tr {
    background: linear-gradient(90deg, #0D1117, #1a2744);
  }
  .ep-grid-table thead th {
    color: rgba(255,255,255,.82); font-family: 'Sora', sans-serif;
    font-weight: 600; font-size: .79rem; letter-spacing: .35px;
    padding: 13px 16px; text-align: left; white-space: nowrap;
    border: none;
  }
  .ep-grid-table tbody tr { transition: background .15s; }
  .ep-grid-table tbody tr:nth-child(even) { background: var(--gray); }
  .ep-grid-table tbody tr:hover { background: var(--orange-glow); }
  .ep-grid-table tbody td {
    padding: 12px 16px; color: var(--text);
    border-bottom: 1px solid var(--border); vertical-align: middle;
  }
  .ep-grid-table tbody tr:last-child td { border-bottom: none; }

  /* Empty row */
  .ep-grid-table tbody td[colspan] {
    text-align: center; color: var(--muted);
    font-style: italic; padding: 28px;
  }

  /* Pager row */
  .ep-grid-table tr.pager td {
    background: var(--gray); padding: 10px 16px; text-align: center;
    border-top: 1.5px solid var(--border);
  }
  .ep-grid-table tr.pager a, .ep-grid-table tr.pager span {
    display: inline-flex; align-items: center; justify-content: center;
    width: 32px; height: 32px; border-radius: 8px; margin: 0 2px;
    font-size: .83rem; font-weight: 700; text-decoration: none;
    border: 1.5px solid var(--border); color: var(--text); background: #fff;
    transition: all .2s;
  }
  .ep-grid-table tr.pager span {
    background: var(--orange); color: #fff; border-color: var(--orange);
  }
  .ep-grid-table tr.pager a:hover { border-color: var(--orange); color: var(--orange); }

  .ep-divider { border: none; border-top: 1.5px solid var(--border); margin: 0; }
  .ep-err {
    display: block; padding: 10px 16px;
    font-size: .83rem; font-weight: 700;
    color: var(--orange); font-family: 'Nunito', sans-serif;
  }

  @media (max-width: 768px) {
    .ep-stats-row { grid-template-columns: repeat(3,1fr); gap: 10px; }
    .ep-page-header { padding: 20px 18px; }
    .ep-ph-icon { display: none; }
    .ep-card-header { flex-direction: column; align-items: flex-start; }
    .ep-search-input { width: 100%; }
  }
  @media (max-width: 480px) {
    .ep-stats-row { grid-template-columns: 1fr; }
  }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<div class="ep-page-wrap">

  <!-- Page Header -->
  <div class="ep-page-header">
    <div class="ep-ph-text">
      <div class="ep-badge"><span></span> Reports</div>
      <h1>Coupon Report</h1>
      <p>View all your purchased and redeemed promo codes</p>
    </div>
    <div class="ep-ph-icon">📊</div>
  </div>

  <!-- Stats Row -->
  <div class="ep-stats-row">
    <div class="ep-stat-card">
      <div class="s-icon">🎟️</div>
      <asp:Label ID="lblTotalCoupons" runat="server" CssClass="s-num">0</asp:Label>
      <span class="s-label">Total Coupons</span>
    </div>
    <div class="ep-stat-card">
      <div class="s-icon">✅</div>
      <asp:Label ID="lblActiveCoupons" runat="server" CssClass="s-num">0</asp:Label>
      <span class="s-label">Active</span>
    </div>
    <div class="ep-stat-card">
      <div class="s-icon">🔄</div>
      <asp:Label ID="lblUsedCoupons" runat="server" CssClass="s-num">0</asp:Label>
      <span class="s-label">Used</span>
    </div>
  </div>

  <!-- Main Card -->
  <div class="ep-card">
    <div class="ep-card-header">
      <h4>Coupon Report</h4>
      <div class="ep-search-wrap">
        <span class="search-icon">🔍</span>
        <input type="text" class="ep-search-input"
          placeholder="Search coupons…"
          onkeyup="filterGrid(this.value)"
          id="gridSearchBox" />
      </div>
    </div>

    <div class="ep-card-body">

      <!-- Error label -->
      <asp:Label ID="errMsg" runat="server" CssClass="ep-err"></asp:Label>
      <asp:HiddenField ID="HdnCheckTrnns" runat="server" />

      <!-- GridView -->
      <div class="ep-table-wrap">
        <asp:GridView ID="RptDirects" runat="server"
          CssClass="ep-grid-table"
          AutoGenerateColumns="true"
          ShowHeaderWhenEmpty="true"
          GridLines="None"
          BorderWidth="0"
          PagerStyle-CssClass="pager"
          HeaderStyle-CssClass=""
          RowStyle-CssClass=""
          AlternatingRowStyle-CssClass="">
        </asp:GridView>
      </div>

      <hr class="ep-divider">
    </div>
  </div>

</div>

<script>
  function filterGrid(val) {
    var rows = document.querySelectorAll('.ep-grid-table tbody tr');
    var v = val.toLowerCase();
    rows.forEach(function(row) {
      if (row.classList.contains('pager')) return;
      row.style.display = row.textContent.toLowerCase().includes(v) ? '' : 'none';
    });
  }
</script>

</asp:Content>
