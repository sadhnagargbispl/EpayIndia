<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="icon" type="image/x-icon" href="images/favicon.png">
<title>Subscription Now – ePay Digital India</title>
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
    --orange-glow: rgba(232,64,0,0.10);
  }

  /* ── Hero ── */
  .sub-hero {
    background: linear-gradient(135deg, #fff7ed 0%, #ffedd5 50%, #fff3e0 100%);
    padding: 70px 20px 60px;
    text-align: center;
    position: relative;
    overflow: hidden;
    border-bottom: 1px solid var(--border);
  }
  .sub-hero::before {
    content: '';
    position: absolute;
    inset: 0;
    background: radial-gradient(ellipse 70% 60% at 50% 0%, rgba(232,64,0,0.10) 0%, transparent 70%);
  }
  .sub-hero-inner { position: relative; z-index: 1; max-width: 700px; margin: auto; }
  .sub-hero-badge {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    background: rgba(232,64,0,0.12);
    border: 1px solid rgba(232,64,0,0.28);
    color: var(--orange);
    font-size: .78rem;
    font-weight: 700;
    letter-spacing: .9px;
    text-transform: uppercase;
    padding: 6px 18px;
    border-radius: 50px;
    margin-bottom: 20px;
  }
  .sub-hero-badge-dot {
    width: 8px; height: 8px;
    background: var(--orange);
    border-radius: 50%;
    animation: blink 1.4s infinite;
  }
  @keyframes blink { 0%,100%{opacity:1} 50%{opacity:.3} }
  .sub-hero h1 {
    font-family: 'Sora', sans-serif;
    font-size: clamp(2rem, 5vw, 3.2rem);
    font-weight: 800;
    color: var(--text);
    line-height: 1.2;
    margin-bottom: 16px;
  }
  .sub-hero h1 span { color: var(--orange); }
  .sub-hero p {
    color: var(--muted);
    font-size: 1rem;
    line-height: 1.8;
    max-width: 560px;
    margin: 0 auto;
  }

  /* ── Section wrapper ── */
  .sub-section { padding: 70px 20px; }
  .sub-section.bg-light { background: #f9fafb; }
  .sub-wrap { max-width: 1100px; margin: auto; }
  .sub-section-tag {
    display: inline-block;
    background: var(--orange-glow);
    color: var(--orange);
    font-size: .75rem;
    font-weight: 700;
    letter-spacing: .8px;
    text-transform: uppercase;
    padding: 5px 14px;
    border-radius: 50px;
    margin-bottom: 12px;
  }
  .sub-section-title {
    font-family: 'Sora', sans-serif;
    font-size: clamp(1.5rem, 3vw, 2.1rem);
    font-weight: 800;
    color: var(--text);
    margin-bottom: 10px;
  }
  .sub-section-title span { color: var(--orange); }
  .sub-section-sub { color: var(--muted); font-size: .95rem; line-height: 1.8; margin-bottom: 44px; }

  /* ── Packages Grid ── */
  .sub-packages-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 26px;
    margin-top: 44px;
  }

  .sub-pkg-card {
    background: #fff;
    border-radius: 20px;
    text-align: center;
    position: relative;
    overflow: hidden;
    transition: transform .3s, box-shadow .3s, border-color .3s;
    border: 1.5px solid var(--border);
    box-shadow: 0 4px 24px rgba(0,0,0,.06);
    display: flex; flex-direction: column;
  }
  .sub-pkg-card:hover { transform: translateY(-8px); box-shadow: 0 20px 50px rgba(232,64,0,.13); border-color: rgba(232,64,0,.35); }

  /* Top stripe */
  .sub-pkg-card::before {
    content: '';
    display: block;
    height: 6px;
    background: linear-gradient(90deg, var(--orange), #ff8c42);
    position: absolute; top: 0; left: 0; right: 0;
  }

  /* Popular badge */
  .sub-pkg-card.popular { border-color: var(--orange); }
  .sub-pkg-card.popular::before { background: linear-gradient(90deg, var(--orange), #ff5722); }
  .sub-popular-badge {
    position: absolute;
    top: 14px; right: 14px;
    background: var(--orange);
    color: #fff;
    font-size: .65rem;
    font-weight: 800;
    letter-spacing: .6px;
    text-transform: uppercase;
    padding: 4px 12px;
    border-radius: 50px;
  }

  /* Card header area */
  .sub-pkg-header {
    background: linear-gradient(160deg, #fff7ed, #ffedd5);
    padding: 36px 30px 24px;
    border-bottom: 1.5px dashed rgba(232,64,0,.2);
  }
  .sub-pkg-icon {
    font-size: 2.4rem;
    margin-bottom: 12px;
  }
  .sub-pkg-name {
    font-family: 'Sora', sans-serif;
    font-size: .85rem;
    font-weight: 800;
    color: var(--orange);
    letter-spacing: 1px;
    text-transform: uppercase;
    margin-bottom: 12px;
  }
  .sub-pkg-price {
    font-family: 'Sora', sans-serif;
    font-size: 2.8rem;
    font-weight: 800;
    color: var(--text);
    line-height: 1;
  }
  .sub-pkg-price span { font-size: 1.2rem; vertical-align: top; margin-top: 8px; display: inline-block; color: var(--muted); }

  /* Card body */
  .sub-pkg-body { padding: 20px 24px 24px; display: flex; flex-direction: column; flex: 1; gap: 12px; }
  .sub-pkg-divider { display: none; }
  .sub-pkg-capping-label {
    font-size: .72rem; font-weight: 700; letter-spacing: .7px;
    text-transform: uppercase; color: var(--muted);
  }
  .sub-pkg-capping-row {
    display: flex; align-items: center; justify-content: space-between;
    background: #f9fafb; border: 1px solid var(--border); border-radius: 10px; padding: 12px 16px;
  }
  .sub-pkg-capping-row .lbl { font-size: .74rem; font-weight: 700; color: var(--muted); text-transform: uppercase; letter-spacing: .5px; }
  .sub-pkg-capping {
    font-family: 'Sora', sans-serif;
    font-size: 1rem;
    font-weight: 800;
    color: var(--text);
  }
  .sub-pkg-btn {
    display: block;
    width: 100%;
    padding: 13px 0;
    background: var(--orange);
    color: #fff;
    font-family: 'Sora', sans-serif;
    font-size: .88rem;
    font-weight: 700;
    letter-spacing: .5px;
    border: none;
    border-radius: 50px;
    cursor: pointer;
    text-decoration: none;
    transition: background .25s, transform .2s, box-shadow .25s;
    box-shadow: 0 6px 20px rgba(232,64,0,.3);
    margin-top: auto;
  }
  .sub-pkg-btn:hover { background: #c73600; transform: translateY(-2px); box-shadow: 0 10px 28px rgba(232,64,0,.42); }
  .sub-pkg-card.popular .sub-pkg-btn { box-shadow: 0 6px 20px rgba(232,64,0,.35); }

  /* ── Benefits Section ── */
  .sub-benefits-wrap {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 50px;
    align-items: start;
  }
  .sub-benefits-text p {
    color: var(--muted);
    font-size: .95rem;
    line-height: 1.85;
    margin-bottom: 30px;
  }
  .sub-perks { display: flex; flex-direction: column; gap: 14px; }
  .sub-perk {
    display: flex;
    align-items: flex-start;
    gap: 14px;
    background: #fff;
    border: 1px solid var(--border);
    border-radius: 14px;
    padding: 16px 20px;
    transition: border-color .25s, box-shadow .25s, transform .25s;
  }
  .sub-perk:hover { border-color: rgba(232,64,0,.3); box-shadow: 0 6px 20px rgba(232,64,0,.07); transform: translateY(-2px); }
  .sub-perk-icon {
    width: 42px; height: 42px;
    background: var(--orange-glow);
    border: 1px solid rgba(232,64,0,.2);
    border-radius: 12px;
    display: flex; align-items: center; justify-content: center;
    font-size: 1.2rem;
    flex-shrink: 0;
  }
  .sub-perk strong {
    display: block;
    font-family: 'Sora', sans-serif;
    font-size: .88rem;
    font-weight: 700;
    color: var(--text);
    margin-bottom: 3px;
  }
  .sub-perk p { color: var(--muted); font-size: .8rem; line-height: 1.5; margin: 0; }

  /* ── Wallet Points Table ── */
  .sub-table-wrap {
    background: #fff;
    border: 1px solid var(--border);
    border-radius: 20px;
    overflow: hidden;
    box-shadow: 0 4px 24px rgba(0,0,0,.06);
  }
  .sub-table-head {
    background: linear-gradient(135deg, var(--orange), #ff8c42);
    padding: 20px 28px;
    display: flex;
    align-items: center;
    gap: 12px;
  }
  .sub-table-head-icon { font-size: 1.4rem; }
  .sub-table-head h4 {
    font-family: 'Sora', sans-serif;
    font-size: 1rem;
    font-weight: 700;
    color: #fff;
    margin: 0;
  }
  .sub-table-head p { color: rgba(255,255,255,.7); font-size: .78rem; margin: 2px 0 0; }
  table.sub-table {
    width: 100%;
    border-collapse: collapse;
  }
  table.sub-table thead th {
    background: rgba(232,64,0,.08);
    color: var(--orange);
    font-family: 'Sora', sans-serif;
    font-size: .78rem;
    font-weight: 700;
    letter-spacing: .7px;
    text-transform: uppercase;
    padding: 14px 28px;
    text-align: left;
    border-bottom: 1px solid var(--border);
  }
  table.sub-table tbody tr {
    border-bottom: 1px solid var(--border);
    transition: background .2s;
  }
  table.sub-table tbody tr:last-child { border-bottom: none; }
  table.sub-table tbody tr:hover { background: #fafafa; }
  table.sub-table tbody td {
    padding: 16px 28px;
    font-size: .9rem;
    color: var(--text);
    vertical-align: middle;
  }
  .sub-table-pkg-name { font-family: 'Sora', sans-serif; font-weight: 700; }
  .sub-table-pkg-price { color: var(--muted); font-size: .82rem; font-weight: 500; }
  .sub-table-pts {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    background: rgba(232,64,0,.09);
    border: 1px solid rgba(232,64,0,.22);
    color: var(--orange);
    font-family: 'Sora', sans-serif;
    font-size: .9rem;
    font-weight: 800;
    padding: 4px 14px;
    border-radius: 50px;
  }

  /* ── CTA Banner ── */
  .sub-cta {
    background: linear-gradient(135deg, var(--dark) 0%, #2a1f3d 100%);
    padding: 70px 20px;
    text-align: center;
    position: relative;
    overflow: hidden;
  }
  .sub-cta::before {
    content: '';
    position: absolute;
    inset: 0;
    background: radial-gradient(ellipse 60% 50% at 50% 0%, rgba(232,64,0,.15) 0%, transparent 65%);
  }
  .sub-cta-inner { position: relative; z-index: 1; max-width: 680px; margin: auto; }
  .sub-cta h2 {
    font-family: 'Sora', sans-serif;
    font-size: clamp(1.6rem, 3.5vw, 2.4rem);
    font-weight: 800;
    color: #fff;
    margin-bottom: 14px;
  }
  .sub-cta h2 span { color: var(--orange); }
  .sub-cta p { color: rgba(255,255,255,.6); font-size: .96rem; line-height: 1.8; margin-bottom: 36px; }
  .sub-cta-btns { display: flex; gap: 14px; justify-content: center; flex-wrap: wrap; }
  .sub-btn-primary {
    background: var(--orange);
    color: #fff;
    font-family: 'Sora', sans-serif;
    font-weight: 700;
    font-size: .92rem;
    padding: 14px 34px;
    border-radius: 50px;
    text-decoration: none;
    box-shadow: 0 8px 24px rgba(232,64,0,.4);
    transition: background .25s, transform .2s;
  }
  .sub-btn-primary:hover { background: #c73600; transform: translateY(-2px); }
  .sub-btn-outline {
    background: rgba(255,255,255,.08);
    color: #fff;
    font-family: 'Sora', sans-serif;
    font-weight: 600;
    font-size: .92rem;
    padding: 14px 34px;
    border-radius: 50px;
    text-decoration: none;
    border: 1.5px solid rgba(255,255,255,.2);
    transition: background .25s, transform .2s;
  }
  .sub-btn-outline:hover { background: rgba(255,255,255,.15); transform: translateY(-2px); }

  /* ── Responsive ── */
  @media (max-width: 900px) {
    .sub-packages-grid { grid-template-columns: 1fr 1fr; }
    .sub-benefits-wrap { grid-template-columns: 1fr; gap: 34px; }
  }
  @media (max-width: 580px) {
    .sub-packages-grid { grid-template-columns: 1fr; max-width: 360px; margin: 44px auto 0; }
    table.sub-table thead th,
    table.sub-table tbody td { padding: 12px 16px; font-size: .82rem; }
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
          <option>All Categories</option>
          <option>Mobiles</option>
          <option>Fashion</option>
          <option>Electronics</option>
          <option>Home &amp; Kitchen</option>
          <option>Beauty</option>
          <option>Books</option>
          <option>Baby &amp; Kids</option>
          <option>Sports</option>
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


<!-- ─── HERO BANNER ─── -->
<section class="sub-hero">
  <div class="sub-hero-inner">
    <div class="sub-hero-badge">
     <div class="sub-hero-badge-dot"></div>
     ePay India — Subscription Plans
     </div>
    <h1>Choose Your <span>ePay</span><br>Subscription Plan</h1>
    <p>Subscribe once and unlock unlimited income benefits, wallet points, daily capping &amp; exclusive rewards — forever.</p>
  </div>
</section>


<!-- ─── SUBSCRIPTION PACKAGES ─── -->
<section class="sub-section">
  <div class="sub-wrap">
    <div class="sub-section-tag">Subscription Package</div>
    <h2 class="sub-section-title">Pick the <span>Right Plan</span> for You</h2>
    <p class="sub-section-sub">All plans include access to ePay services, wallet points &amp; daily earning capping limits.</p>

    <div class="sub-packages-grid">

      <!-- Prime -->
      <div class="sub-pkg-card" data-anim>
        <div class="sub-pkg-header">
          <div class="sub-pkg-icon">🥈</div>
          <div class="sub-pkg-name">Prime Package</div>
          <div class="sub-pkg-price"><span>₹</span>999</div>
        </div>
        <div class="sub-pkg-body">
          <a href="#" class="sub-pkg-btn">Subscribe Now →</a>
        </div>
      </div>

      <!-- Pro — Popular -->
      <div class="sub-pkg-card popular" data-anim>
         <div class="sub-pkg-header">
          <div class="sub-pkg-icon">🥇</div>
          <div class="sub-pkg-name">Pro Package</div>
          <div class="sub-pkg-price"><span>₹</span>4,999</div>
        </div>
        <div class="sub-pkg-body">
          <a href="#" class="sub-pkg-btn">Subscribe Now →</a>
        </div>
      </div>

      <!-- Premium -->
      <div class="sub-pkg-card" data-anim>
        <div class="sub-pkg-header">
          <div class="sub-pkg-icon">👑</div>
          <div class="sub-pkg-name">Premium Package</div>
          <div class="sub-pkg-price"><span>₹</span>9,999</div>
        </div>
        <div class="sub-pkg-body">
          <a href="#" class="sub-pkg-btn">Subscribe Now →</a>
        </div>
      </div>

    </div>
  </div>
</section>


 <!-- <section class="sub-section bg-light">
  <div class="sub-wrap">
    <div class="sub-section-tag">Subscription Benefits</div>
    <h2 class="sub-section-title">What You <span>Get</span></h2>

    <div class="sub-benefits-wrap">

       <div class="sub-benefits-text">
        <p>Purchase any subscription to get access to unlimited income benefits. Get the wallet points as per the mentioned table and enjoy exclusive rewards on every transaction.</p>
        <div class="sub-perks">
          <div class="sub-perk" data-anim>
            <div class="sub-perk-icon">💰</div>
            <div>
              <strong>Wallet Points on Activation</strong>
              <p>Instant wallet points credited as per your selected package on day one.</p>
            </div>
          </div>
          <div class="sub-perk" data-anim>
            <div class="sub-perk-icon">📈</div>
            <div>
              <strong>Daily Earning Capping</strong>
              <p>Higher packages unlock higher daily earning limits, maximising your income potential.</p>
            </div>
          </div>
          <div class="sub-perk" data-anim>
            <div class="sub-perk-icon">♾️</div>
            <div>
              <strong>Lifetime Access</strong>
              <p>One-time subscription, unlimited benefits and access to all ePay services forever.</p>
            </div>
          </div>
          <div class="sub-perk" data-anim>
            <div class="sub-perk-icon">🎁</div>
            <div>
              <strong>Exclusive Rewards &amp; Offers</strong>
              <p>Get special deals, cashback and priority access to new features before anyone else.</p>
            </div>
          </div>
        </div>
      </div>

       <div class="sub-table-wrap" data-anim>
        <div class="sub-table-head">
          <div class="sub-table-head-icon">⭐</div>
          <div>
            <h4>Wallet Points Table</h4>
            <p>Points credited on activation</p>
          </div>
        </div>
        <table class="sub-table">
          <thead>
            <tr>
              <th>Package Name</th>
              <th>Wallet Points</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>
                <div class="sub-table-pkg-name">Prime Package</div>
                <div class="sub-table-pkg-price">@ Rs. 999</div>
              </td>
              <td><span class="sub-table-pts">⭐ 1,200</span></td>
            </tr>
            <tr>
              <td>
                <div class="sub-table-pkg-name">Pro Package</div>
                <div class="sub-table-pkg-price">@ Rs. 4,999</div>
              </td>
              <td><span class="sub-table-pts">⭐ 6,000</span></td>
            </tr>
            <tr>
              <td>
                <div class="sub-table-pkg-name">Premium Package</div>
                <div class="sub-table-pkg-price">@ Rs. 9,999</div>
              </td>
              <td><span class="sub-table-pts">⭐ 12,000</span></td>
            </tr>
          </tbody>
        </table>
      </div>

    </div>
  </div>
</section> -->


 <!-- <section class="sub-cta">
  <div class="sub-cta-inner">
    <h2>Ready to <span>Join ePay?</span></h2>
    <p>Subscribe today and start earning wallet points, unlocking daily capping rewards &amp; accessing all ePay premium services.</p>
    <div class="sub-cta-btns">
      <a href="#" class="sub-btn-primary">Subscribe Now →</a>
      <a href="index.asp" class="sub-btn-outline">Explore Services</a>
    </div>
  </div>
</section> -->


<!-- ─── FOOTER ─── -->
<!-- #include file="inc_footer.asp" -->


<script>
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

  const observer = new IntersectionObserver((entries) => {
    entries.forEach(e => {
      if (e.isIntersecting) {
        e.target.style.opacity = '1';
        e.target.style.transform = 'translateY(0)';
      }
    });
  }, { threshold: 0.08 });

  document.querySelectorAll('[data-anim]').forEach((el, i) => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(24px)';
    el.style.transition = `opacity .5s ease ${i * 0.08}s, transform .5s ease ${i * 0.08}s`;
    observer.observe(el);
  });
</script>

</body>
</html>
