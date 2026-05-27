<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="icon" type="image/x-icon" href="demoepay/images/favicon.png">
<title>Coupon Details – ePay Digital India</title>
<link href="https://fonts.googleapis.com/css2?family=Sora:wght@300;400;500;600;700;800&family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet">
<link rel="stylesheet" href="css/custom_stylesheet.css">
<link rel="stylesheet" href="css/style.css">
<style>
  :root {
    --orange: #E84000;
    --og:     rgba(232,64,0,0.10);
    --green:  #16a34a;
    --gg:     rgba(22,163,74,0.10);
    --muted:  #6B7280;
    --border: #E2E8F0;
    --text:   #1A1A2E;
  }

  /* ── Page Breadcrumb ── */
  .cpd-breadcrumb {
    background: #fff; border-bottom: 1px solid var(--border); padding: 14px 20px;
  }
  .cpd-breadcrumb-inner {
    max-width: 1200px; margin: auto;
    display: flex; align-items: center; gap: 8px;
    font-size: .82rem; color: var(--muted);
  }
  .cpd-breadcrumb-inner a { color: var(--orange); text-decoration: none; font-weight: 600; }
  .cpd-breadcrumb-inner a:hover { text-decoration: underline; }
  .cpd-breadcrumb-sep { color: #d1d5db; }

  /* ── Main Layout ── */
  .cpd-section { padding: 40px 20px 60px; background: #f9fafb; min-height: 70vh; }
  .cpd-wrap { max-width: 1200px; margin: auto; }
  .cpd-layout {
    display: grid;
    grid-template-columns: 1fr 360px;
    gap: 32px;
    align-items: start;
  }

  /* ══════════════════════════════
     LEFT — DETAILS PANEL
  ══════════════════════════════ */
  .cpd-left { display: flex; flex-direction: column; gap: 24px; }

  /* Package title card */
  .cpd-title-card {
    background: #fff; border-radius: 18px; border: 1px solid var(--border);
    padding: 28px 30px; box-shadow: 0 2px 16px rgba(0,0,0,.05);
  }
  .cpd-pkg-badge {
    display: inline-flex; align-items: center; gap: 7px;
    background: var(--og); border: 1px solid rgba(232,64,0,.25); color: var(--orange);
    font-size: .72rem; font-weight: 700; letter-spacing: .8px; text-transform: uppercase;
    padding: 5px 14px; border-radius: 50px; margin-bottom: 14px;
  }
  .cpd-pkg-dot { width: 7px; height: 7px; background: var(--orange); border-radius: 50%; animation: blink 1.4s infinite; }
  @keyframes blink { 0%,100%{opacity:1} 50%{opacity:.3} }
  .cpd-title-card h1 {
    font-family: 'Sora', sans-serif; font-size: clamp(1.3rem,2.5vw,1.8rem);
    font-weight: 800; color: var(--text); margin-bottom: 12px; line-height: 1.3;
  }
  .cpd-title-card h1 span { color: var(--orange); }
  .cpd-title-card p { color: var(--muted); font-size: .92rem; line-height: 1.8; }

  /* User info card */
  .cpd-info-card {
    background: #fff; border-radius: 18px; border: 1px solid var(--border);
    padding: 24px 30px; box-shadow: 0 2px 16px rgba(0,0,0,.05);
  }
  .cpd-info-card h3 {
    font-family: 'Sora', sans-serif; font-size: .88rem; font-weight: 700;
    color: var(--muted); text-transform: uppercase; letter-spacing: .6px;
    margin-bottom: 18px; padding-bottom: 12px; border-bottom: 1px solid var(--border);
  }
  .cpd-info-rows { display: flex; flex-direction: column; gap: 14px; }
  .cpd-info-row {
    display: flex; align-items: center; justify-content: space-between;
    padding: 14px 18px; border-radius: 12px; background: #f9fafb; border: 1px solid var(--border);
  }
  .cpd-info-row-left { display: flex; align-items: center; gap: 12px; }
  .cpd-info-icon {
    width: 38px; height: 38px; border-radius: 10px;
    background: var(--og); border: 1px solid rgba(232,64,0,.2);
    display: flex; align-items: center; justify-content: center; font-size: 1rem; flex-shrink: 0;
  }
  .cpd-info-key { font-size: .76rem; font-weight: 700; color: var(--muted); text-transform: uppercase; letter-spacing: .5px; margin-bottom: 2px; }
  .cpd-info-val { font-family: 'Sora', sans-serif; font-size: .95rem; font-weight: 700; color: var(--text); }
  .cpd-info-row.amount-row { background: linear-gradient(135deg,#fff7ed,#ffedd5); border-color: rgba(232,64,0,.25); }
  .cpd-info-row.amount-row .cpd-info-val { color: var(--orange); font-size: 1.15rem; }

  /* Pay button */
  .cpd-pay-btn {
    display: block; text-align: center; width: 100%;
    background: var(--orange); color: #fff;
    font-family: 'Sora', sans-serif; font-size: 1rem; font-weight: 700;
    padding: 16px 0; border-radius: 50px; text-decoration: none; border: none; cursor: pointer;
    box-shadow: 0 8px 28px rgba(232,64,0,.32);
    transition: background .25s, transform .2s, box-shadow .25s;
    margin-top: 4px;
  }
  .cpd-pay-btn:hover { background: #c73600; transform: translateY(-2px); box-shadow: 0 12px 36px rgba(232,64,0,.42); }

  /* ── Tab Menu ── */
  .cpd-tabs-card {
    background: #fff; border-radius: 18px; border: 1px solid var(--border);
    overflow: hidden; box-shadow: 0 2px 16px rgba(0,0,0,.05);
  }
  .cpd-tabs-nav {
    display: flex; border-bottom: 1px solid var(--border); overflow-x: auto;
  }
  .cpd-tab-btn {
    flex: 1; min-width: 110px;
    padding: 16px 20px; text-align: center;
    font-family: 'Sora', sans-serif; font-size: .84rem; font-weight: 700;
    color: var(--muted); background: #fff; border: none; cursor: pointer;
    border-bottom: 3px solid transparent; transition: color .2s, border-color .2s, background .2s;
    white-space: nowrap;
  }
  .cpd-tab-btn:hover { color: var(--orange); background: #fef9f7; }
  .cpd-tab-btn.active { color: var(--orange); border-bottom-color: var(--orange); background: #fff; }

  .cpd-tab-content { display: none; padding: 28px 30px; }
  .cpd-tab-content.active { display: block; }
  .cpd-tab-content h4 {
    font-family: 'Sora', sans-serif; font-size: 1rem; font-weight: 800;
    color: var(--text); margin-bottom: 14px;
  }
  .cpd-tab-content p {
    color: var(--muted); font-size: .9rem; line-height: 1.85; margin-bottom: 14px;
  }
  .cpd-tab-content ul {
    list-style: none; padding: 0; margin: 0; display: flex; flex-direction: column; gap: 10px;
  }
  .cpd-tab-content ul li {
    display: flex; align-items: flex-start; gap: 10px;
    color: var(--muted); font-size: .88rem; line-height: 1.6;
  }
  .cpd-tab-content ul li::before {
    content: '✓'; flex-shrink: 0;
    width: 20px; height: 20px; border-radius: 50%;
    background: var(--orange); color: #fff;
    font-size: .65rem; font-weight: 800;
    display: flex; align-items: center; justify-content: center; margin-top: 1px;
  }
  .cpd-tab-content ul li.tc::before { content: '•'; background: var(--muted); font-size: .8rem; }

  /* ══════════════════════════════
     RIGHT — COUPON CARD PREVIEW
  ══════════════════════════════ */
  .cpd-right { position: sticky; top: 24px; }

  .cpd-coupon-wrap {
    background: #fff; border-radius: 20px; border: 1px solid var(--border);
    overflow: hidden; box-shadow: 0 4px 32px rgba(0,0,0,.08);
  }

  .cpd-coupon-header {
    background: linear-gradient(135deg,#fff7ed,#ffedd5);
    padding: 24px; text-align: center;
    border-bottom: 1px solid rgba(232,64,0,.18);
  }
  .cpd-coupon-header h3 {
    font-family: 'Sora', sans-serif; font-size: .9rem; font-weight: 800;
    color: var(--text); margin-bottom: 4px;
  }
  .cpd-coupon-header p { font-size: .78rem; color: var(--muted); }

  /* The Coupon card inside */
  .cpd-coupon-card {
    background: #fff; border-radius: 16px; overflow: hidden;
    display: flex; flex-direction: column; margin: 20px;
    box-shadow: 0 4px 24px rgba(0,0,0,.09); border: 1.5px solid var(--border);
  }
  .cpd-coupon-stripe { height: 6px; background: linear-gradient(90deg,var(--orange),#ff8c42); }
  .cpd-coupon-body   { display: flex; align-items: stretch; }

  .cpd-c-left {
    display: flex; flex-direction: column; align-items: center; justify-content: center;
    padding: 24px 18px; min-width: 100px;
    background: linear-gradient(160deg,#fff7ed,#ffedd5);
  }
  .cpd-c-icon { font-size: 1.8rem; margin-bottom: 8px; }
  .cpd-c-pct { font-family: 'Sora',sans-serif; font-weight: 800; font-size: 2rem; color: var(--orange); line-height: 1; }
  .cpd-c-off { font-size: .62rem; font-weight: 700; letter-spacing: .6px; text-transform: uppercase; color: var(--muted); margin-top: 4px; }

  .cpd-c-divider { display: flex; flex-direction: column; align-items: center; width: 20px; flex-shrink: 0; position: relative; }
  .cpd-c-divider::before, .cpd-c-divider::after {
    content: ''; width: 18px; height: 18px; border-radius: 50%;
    background: #f9fafb; border: 1px solid var(--border); position: absolute;
  }
  .cpd-c-divider::before { top: -9px; }
  .cpd-c-divider::after  { bottom: -9px; }
  .cpd-c-dline { width: 2px; height: 100%; border-left: 2px dashed #d1d5db; margin: 0 auto; }

  .cpd-c-right { flex: 1; padding: 18px 18px 18px 14px; display: flex; flex-direction: column; justify-content: center; gap: 8px; }
  .cpd-c-qty { font-family:'Sora',sans-serif; font-size:.95rem; font-weight:800; color:var(--text); }
  .cpd-c-qty span { font-size:.76rem; font-weight:500; color:var(--muted); }
  .cpd-c-orig { font-family:'Sora',sans-serif; font-size:1.1rem; font-weight:800; color:var(--muted); text-decoration:line-through; text-decoration-color:#dc2626; text-decoration-thickness:2px; }
  .cpd-c-wallet { font-family:'Sora',sans-serif; font-size:1.2rem; font-weight:800; color:var(--text); }
  .cpd-c-save { display:inline-flex; align-items:center; font-size:.72rem; font-weight:700; padding:4px 10px; border-radius:50px; background:rgba(220,38,38,.08); color:#dc2626; border:1px solid rgba(220,38,38,.2); width:fit-content; }

  /* Summary box */
  .cpd-summary {
    margin: 0 20px 20px;
    background: #f9fafb; border: 1px solid var(--border);
    border-radius: 14px; padding: 18px;
  }
  .cpd-summary-row {
    display: flex; justify-content: space-between; align-items: center;
    font-size: .85rem; padding: 6px 0; border-bottom: 1px dashed #e9ecef;
  }
  .cpd-summary-row:last-child { border-bottom: none; font-weight: 800; font-size: .92rem; }
  .cpd-summary-row span:first-child { color: var(--muted); }
  .cpd-summary-row span:last-child  { font-family: 'Sora',sans-serif; font-weight: 700; color: var(--text); }
  .cpd-summary-row.total span:last-child { color: var(--orange); font-size: 1rem; }

  /* Secure badge */
  .cpd-secure {
    margin: 0 20px 20px; text-align: center;
    font-size: .76rem; color: var(--muted); display: flex; align-items: center; justify-content: center; gap: 5px;
  }

  /* ── Responsive ── */
  @media (max-width: 900px) {
    .cpd-layout { grid-template-columns: 1fr; }
    .cpd-right { position: static; }
  }
  @media (max-width: 480px) {
    .cpd-title-card, .cpd-info-card, .cpd-tabs-card { padding: 20px; }
    .cpd-tab-content { padding: 20px; }
  }
</style>
</head>
<body>

<!-- #include file="inc_header.asp" -->

<!-- ─── SEARCH BAR ─── -->
<div class="search-bar-wrap">
  <div class="search-inner">
    <div class="search-box">
      <div class="search-cat">
        <select>
          <option>All Categories</option><option>Mobiles</option><option>Fashion</option>
          <option>Electronics</option><option>Home &amp; Kitchen</option>
          <option>Beauty</option><option>Books</option><option>Baby &amp; Kids</option><option>Sports</option>
        </select>
        <span>&#9662;</span>
      </div>
      <div class="search-input-wrap">
        <span class="search-icon">🔍</span>
        <input type="text" placeholder="Search for products, services and more...">
      </div>
    </div>
  </div>
</div>


<!-- ─── BREADCRUMB ─── -->
<div class="cpd-breadcrumb">
  <div class="cpd-breadcrumb-inner">
    <a href="index.asp">Home</a>
    <span class="cpd-breadcrumb-sep">›</span>
    <a href="purchase-coupon.asp">Purchase Coupon</a>
    <span class="cpd-breadcrumb-sep">›</span>
    <span>Coupon Details</span>
  </div>
</div>


<!-- ─── MAIN CONTENT ─── -->
<section class="cpd-section">
  <div class="cpd-wrap">
    <div class="cpd-layout">

      <!-- ════ LEFT PANEL ════ -->
      <div class="cpd-left">

        <!-- Title -->
        <div class="cpd-title-card">
          <div class="cpd-pkg-badge">
            <div class="cpd-pkg-dot"></div>
            ePay India — Coupon Package
          </div>
          <h1>Food Booking / <span>Movie Package</span></h1>
          <p>Discover the ultimate convenience with our all-in-one platform: shop online, manage utilities, book holidays and movies, order food, and find perfect gift vouchers effortlessly. Stay tuned for our upcoming Petro Card to top-up your tank and your life with ease.</p>
        </div>

        <!-- User Info -->
        <div class="cpd-info-card">
          <h3>📋 Booking Details</h3>
          <div class="cpd-info-rows">

            <div class="cpd-info-row">
              <div class="cpd-info-row-left">
                <div class="cpd-info-icon">👤</div>
                <div>
                  <div class="cpd-info-key">User Name</div>
                  <div class="cpd-info-val">ePay India</div>
                </div>
              </div>
            </div>

            <div class="cpd-info-row">
              <div class="cpd-info-row-left">
                <div class="cpd-info-icon">🪪</div>
                <div>
                  <div class="cpd-info-key">ID Number</div>
                  <div class="cpd-info-val">MEGA6549870</div>
                </div>
              </div>
            </div>

            <div class="cpd-info-row amount-row">
              <div class="cpd-info-row-left">
                <div class="cpd-info-icon">💰</div>
                <div>
                  <div class="cpd-info-key">Package Amount</div>
                  <div class="cpd-info-val">Rs. 50,000.00</div>
                </div>
              </div>
            </div>

          </div>

          <br>
          <a href="#" class="cpd-pay-btn">💳 Process to Pay →</a>
        </div>

        <!-- Tabs -->
        <div class="cpd-tabs-card">
          <div class="cpd-tabs-nav">
            <button class="cpd-tab-btn active" onclick="switchTab(this,'tab-desc')">📄 Description</button>
            <button class="cpd-tab-btn" onclick="switchTab(this,'tab-how')">📖 How to Use</button>
            <button class="cpd-tab-btn" onclick="switchTab(this,'tab-tc')">📜 Terms &amp; Conditions</button>
          </div>

          <!-- Description -->
          <div class="cpd-tab-content active" id="tab-desc">
            <h4>About This Coupon Package</h4>
            <p>This coupon package gives you access to exclusive food and movie bookings at the best prices. Purchase in bulk and enjoy maximum savings with ePay's Ecommerce Wallet balance credited directly to your account.</p>
            <p>Each coupon can be used at participating restaurants and movie theatres across India. Valid for use within the specified validity period from the date of purchase.</p>
            <ul>
              <li>Instant digital delivery after purchase</li>
              <li>Usable at 1000+ partner restaurants &amp; cinemas</li>
              <li>Ecommerce Wallet balance credited immediately</li>
              <li>Savings up to 50% on total coupon value</li>
            </ul>
          </div>

          <!-- How to Use -->
          <div class="cpd-tab-content" id="tab-how">
            <h4>How to Use Your Coupon</h4>
            <ul>
              <li>Login to your ePay account and go to "My Coupons" section.</li>
              <li>Select the coupon you wish to use at checkout.</li>
              <li>At the restaurant or cinema, show your digital coupon code at the counter.</li>
              <li>The discount will be applied automatically on the total bill amount.</li>
              <li>Remaining wallet balance stays in your ePay account for future use.</li>
              <li>Contact ePay support for any assistance during redemption.</li>
            </ul>
          </div>

          <!-- Terms & Conditions -->
          <div class="cpd-tab-content" id="tab-tc">
            <h4>Terms &amp; Conditions</h4>
            <ul>
              <li class="tc">Coupons are non-transferable and cannot be exchanged for cash.</li>
              <li class="tc">Each coupon is valid for single use only unless stated otherwise.</li>
              <li class="tc">ePay reserves the right to cancel coupons in case of fraudulent activity.</li>
              <li class="tc">Validity period starts from the date of activation, not purchase.</li>
              <li class="tc">Coupons cannot be clubbed with any other ongoing offer or discount.</li>
              <li class="tc">Refunds will only be processed as per ePay's refund policy.</li>
              <li class="tc">ePay Digital India Pvt. Ltd. is the final authority on all disputes.</li>
            </ul>
          </div>

        </div>
      </div>
      <!-- /LEFT -->


      <!-- ════ RIGHT PANEL — COUPON PREVIEW ════ -->
      <div class="cpd-right">

        <div class="cpd-coupon-wrap">
          <div class="cpd-coupon-header">
            <h3>🎟️ Selected Coupon</h3>
            <p>Review your coupon before payment</p>
          </div>

          <!-- Coupon Card -->
          <div class="cpd-coupon-card">
            <div class="cpd-coupon-stripe"></div>
            <div class="cpd-coupon-body">
              <div class="cpd-c-left">
                <div class="cpd-c-icon">🍔</div>
                <div class="cpd-c-pct">40%</div>
                <div class="cpd-c-off">OFF</div>
              </div>
              <div class="cpd-c-divider"><div class="cpd-c-dline"></div></div>
              <div class="cpd-c-right">
                <div class="cpd-c-qty">20 Food Coupons <span>/ Pack</span></div>
                <div class="cpd-c-orig">₹10,000</div>
                <div class="cpd-c-wallet">₹6,000</div>
                <div class="cpd-c-save">Save ₹4,000</div>
              </div>
            </div>
          </div>

          <!-- Order Summary -->
          <div class="cpd-summary">
            <div class="cpd-summary-row">
              <span>Original Price</span>
              <span>₹10,000</span>
            </div>
            <div class="cpd-summary-row">
              <span>Discount (40%)</span>
              <span style="color:#dc2626;">− ₹4,000</span>
            </div>
            <div class="cpd-summary-row">
              <span>Wallet Credit</span>
              <span style="color:var(--green);">₹6,000</span>
            </div>
            <div class="cpd-summary-row total">
              <span>You Pay</span>
              <span>₹10,000</span>
            </div>
          </div>

          <div class="cpd-secure">
            🔒 &nbsp;100% Secure &amp; Encrypted Payment
          </div>
        </div>

      </div>
      <!-- /RIGHT -->

    </div>
  </div>
</section>


<!-- #include file="inc_footer.asp" -->


<script>
  function switchTab(btn, tabId) {
    document.querySelectorAll('.cpd-tab-btn').forEach(b => b.classList.remove('active'));
    document.querySelectorAll('.cpd-tab-content').forEach(t => t.classList.remove('active'));
    btn.classList.add('active');
    document.getElementById(tabId).classList.add('active');
  }

  const hamburgerBtn = document.getElementById('hamburgerBtn');
  const mobileNav    = document.getElementById('mobileNav');
  if (hamburgerBtn && mobileNav) {
    hamburgerBtn.addEventListener('click', function () {
      const isOpen = mobileNav.classList.toggle('open');
      hamburgerBtn.classList.toggle('open', isOpen);
      hamburgerBtn.setAttribute('aria-expanded', isOpen);
    });
    mobileNav.querySelectorAll('a').forEach(function (link) {
      link.addEventListener('click', function () {
        mobileNav.classList.remove('open');
        hamburgerBtn.classList.remove('open');
        hamburgerBtn.setAttribute('aria-expanded', 'false');
      });
    });
    document.addEventListener('click', function (e) {
      if (!hamburgerBtn.contains(e.target) && !mobileNav.contains(e.target)) {
        mobileNav.classList.remove('open');
        hamburgerBtn.classList.remove('open');
        hamburgerBtn.setAttribute('aria-expanded', 'false');
      }
    });
  }
</script>

</body>
</html>
