<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="icon" type="image/x-icon" href="demoepay/images/favicon.png">
<title>Monthly Activation Points – ePay Digital India</title>
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

  /* ── Hero ── */
  .map-hero {
    background: linear-gradient(135deg, #f0fdf4 0%, #fff7ed 55%, #fef9f0 100%);
    padding: 70px 20px 60px; text-align: center;
    position: relative; overflow: hidden; border-bottom: 1px solid var(--border);
  }
  .map-hero::before {
    content: ''; position: absolute; inset: 0;
    background: radial-gradient(ellipse 70% 55% at 50% 0%, rgba(232,64,0,.08) 0%, transparent 65%);
  }
  .map-hero-inner { position: relative; z-index: 1; max-width: 700px; margin: auto; }
  .map-hero-badge {
    display: inline-flex; align-items: center; gap: 8px;
    background: var(--og); border: 1px solid rgba(232,64,0,.28); color: var(--orange);
    font-size: .78rem; font-weight: 700; letter-spacing: .9px; text-transform: uppercase;
    padding: 6px 18px; border-radius: 50px; margin-bottom: 20px;
  }
  .map-badge-dot { width: 8px; height: 8px; background: var(--orange); border-radius: 50%; animation: blink 1.4s infinite; }
  @keyframes blink { 0%,100%{opacity:1} 50%{opacity:.3} }
  .map-hero h1 {
    font-family: 'Sora', sans-serif; font-size: clamp(2rem,5vw,3rem);
    font-weight: 800; color: var(--text); line-height: 1.2; margin-bottom: 16px;
  }
  .map-hero h1 span { color: var(--orange); }
  .map-hero p { color: var(--muted); font-size: 1rem; line-height: 1.8; max-width: 560px; margin: 0 auto; }

  /* ── Stats ── */
  .map-stats { background: #fff; border-bottom: 1px solid var(--border); padding: 28px 20px; }
  .map-stats-row {
    max-width: 900px; margin: auto;
    display: flex; justify-content: center; align-items: center; flex-wrap: wrap;
  }
  .map-stat { text-align: center; padding: 8px 40px; }
  .map-stat-num { font-family: 'Sora', sans-serif; font-size: 1.9rem; font-weight: 800; color: var(--orange); line-height: 1; }
  .map-stat-label { font-size: .75rem; font-weight: 600; color: var(--muted); text-transform: uppercase; letter-spacing: .5px; margin-top: 4px; }
  .map-stat-div { width: 1px; height: 46px; background: var(--border); }

  /* ── Section ── */
  .map-section { padding: 65px 20px; background: #f9fafb; }
  .map-wrap { max-width: 1100px; margin: auto; }
  .map-section-tag {
    display: inline-block; background: var(--og); color: var(--orange);
    font-size: .73rem; font-weight: 700; letter-spacing: .8px; text-transform: uppercase;
    padding: 5px 14px; border-radius: 50px; margin-bottom: 10px;
  }
  .map-section-title {
    font-family: 'Sora', sans-serif; font-size: clamp(1.4rem,2.8vw,2rem);
    font-weight: 800; color: var(--text); margin-bottom: 8px;
  }
  .map-section-title span { color: var(--orange); }
  .map-section-sub { color: var(--muted); font-size: .93rem; line-height: 1.75; margin-bottom: 40px; }

  /* ══════════════════════════════
     ACTIVATION PLAN CARDS
  ══════════════════════════════ */
  .map-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
    gap: 24px;
  }

  .map-card {
    background: #fff;
    border-radius: 20px;
    border: 1.5px solid var(--border);
    overflow: hidden;
    display: flex;
    flex-direction: column;
    box-shadow: 0 4px 24px rgba(0,0,0,.06);
    transition: transform .28s, box-shadow .28s, border-color .28s;
    position: relative;
  }
  .map-card:hover {
    transform: translateY(-7px);
    box-shadow: 0 16px 48px rgba(232,64,0,.13);
    border-color: rgba(232,64,0,.35);
  }

  /* Top colored header */
  .map-card-header {
    background: linear-gradient(135deg, #fff7ed 0%, #ffedd5 100%);
    border-bottom: 1.5px dashed rgba(232,64,0,.25);
    padding: 28px 24px 22px;
    text-align: center;
    position: relative;
  }
  .map-card-icon {
    width: 52px; height: 52px; border-radius: 50%;
    background: var(--og); border: 2px solid rgba(232,64,0,.25);
    display: flex; align-items: center; justify-content: center;
    font-size: 1.5rem; margin: 0 auto 14px;
  }
  .map-card-pkg-label {
    font-size: .72rem; font-weight: 700; letter-spacing: .8px;
    text-transform: uppercase; color: var(--muted); margin-bottom: 6px;
  }
  .map-card-pkg-amount {
    font-family: 'Sora', sans-serif; font-size: 2.2rem; font-weight: 800; color: var(--orange); line-height: 1;
  }
  .map-card-pkg-amount sup {
    font-size: 1rem; vertical-align: top; margin-top: 6px; display: inline-block;
    color: rgba(232,64,0,.7);
  }

  /* Notch cut-outs */
  .map-card-notch {
    display: flex; align-items: center; justify-content: space-between;
    margin: 0 -1px; position: relative;
  }
  .map-card-notch-circle {
    width: 18px; height: 18px; border-radius: 50%;
    background: #f9fafb; border: 1.5px solid var(--border);
    flex-shrink: 0;
  }
  .map-card-notch-line {
    flex: 1; border-top: 2px dashed #e2e8f0; margin: 0 4px;
  }

  /* Body */
  .map-card-body { padding: 20px 24px 24px; display: flex; flex-direction: column; flex: 1; gap: 14px; }

  .map-info-row {
    display: flex; align-items: center; justify-content: space-between;
    background: #f9fafb; border: 1px solid var(--border);
    border-radius: 10px; padding: 12px 16px;
  }
  .map-info-label {
    font-size: .76rem; font-weight: 700; color: var(--muted);
    text-transform: uppercase; letter-spacing: .5px;
  }
  .map-info-value {
    font-family: 'Sora', sans-serif; font-size: .95rem; font-weight: 800; color: var(--text);
  }
  .map-info-row.cashback {
    background: linear-gradient(135deg, #f0fdf4, #dcfce7);
    border-color: rgba(22,163,74,.25);
  }
  .map-info-row.cashback .map-info-label { color: var(--green); }
  .map-info-row.cashback .map-info-value { color: var(--green); font-size: 1.05rem; }

  /* Button */
  .map-card-btn {
    display: block; text-align: center;
    background: var(--orange); color: #fff;
    font-family: 'Sora', sans-serif; font-size: .86rem; font-weight: 700;
    padding: 12px 0; border-radius: 50px; text-decoration: none;
    box-shadow: 0 5px 18px rgba(232,64,0,.3);
    transition: background .25s, transform .2s, box-shadow .25s;
    margin-top: auto;
  }
  .map-card-btn:hover { background: #c73600; transform: translateY(-2px); box-shadow: 0 8px 24px rgba(232,64,0,.4); }

  /* Popular badge */
  .map-popular-badge {
    position: absolute; top: 14px; right: 14px;
    background: var(--orange); color: #fff;
    font-size: .65rem; font-weight: 800; letter-spacing: .6px; text-transform: uppercase;
    padding: 4px 10px; border-radius: 50px;
  }

  /* ── CTA ── */
  .map-cta {
    background: linear-gradient(135deg, #f0fdf4 0%, #fff7ed 100%);
    padding: 65px 20px; text-align: center;
    position: relative; overflow: hidden; border-top: 1px solid var(--border);
  }
  .map-cta::before {
    content: ''; position: absolute; inset: 0;
    background: radial-gradient(ellipse 60% 50% at 50% 0%, rgba(232,64,0,.07) 0%, transparent 65%);
  }
  .map-cta-inner { position: relative; z-index: 1; max-width: 640px; margin: auto; }
  .map-cta h2 {
    font-family: 'Sora', sans-serif; font-size: clamp(1.5rem,3.5vw,2.2rem);
    font-weight: 800; color: var(--text); margin-bottom: 12px;
  }
  .map-cta h2 span { color: var(--orange); }
  .map-cta p { color: var(--muted); font-size: .95rem; line-height: 1.8; margin-bottom: 30px; }
  .map-cta-btns { display: flex; gap: 14px; justify-content: center; flex-wrap: wrap; }
  .map-btn-p {
    background: var(--orange); color: #fff;
    font-family: 'Sora', sans-serif; font-weight: 700; font-size: .9rem;
    padding: 13px 32px; border-radius: 50px; text-decoration: none;
    box-shadow: 0 8px 24px rgba(232,64,0,.28); transition: background .25s, transform .2s;
  }
  .map-btn-p:hover { background: #c73600; transform: translateY(-2px); }
  .map-btn-o {
    background: #fff; color: var(--green);
    font-family: 'Sora', sans-serif; font-weight: 700; font-size: .9rem;
    padding: 13px 32px; border-radius: 50px; text-decoration: none;
    border: 1.5px solid rgba(22,163,74,.35); transition: background .25s, transform .2s;
  }
  .map-btn-o:hover { background: #f0fdf4; transform: translateY(-2px); }

  /* ── Responsive ── */
  @media (max-width: 768px) {
    .map-grid { grid-template-columns: repeat(2, 1fr); }
    .map-stat { padding: 8px 20px; }
    .map-stat-div { height: 36px; }
  }
  @media (max-width: 480px) {
    .map-grid { grid-template-columns: 1fr; max-width: 340px; margin: 0 auto; }
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


<!-- ─── HERO ─── -->
<section class="map-hero">
  <div class="map-hero-inner">
    <div class="map-hero-badge">
      <div class="map-badge-dot"></div>
      ePay India — Activation Plans
    </div>
    <h1>Monthly <span>Activation</span> Points</h1>
    <p>Purchase any activation package and earn up to 200% cashback points. More days, more rewards — activate and grow with ePay.</p>
  </div>
</section>


 


<!-- ─── ACTIVATION PLAN CARDS ─── -->
<section class="map-section">
  <div class="map-wrap">
    <div class="map-section-tag">⭐ Activation Plans</div>
    <h2 class="map-section-title">Choose Your <span>Activation Plan</span></h2>
    <p class="map-section-sub">Purchase any package mentioned below and get up to 200% of the purchased amount as cashback points.</p>

    <div class="map-grid">

      <!-- Plan 1 — ₹300 -->
      <div class="map-card" data-anim>
        <div class="map-card-header">
          <div class="map-card-icon">⚡</div>
          <div class="map-card-pkg-label">Package Amount</div>
          <div class="map-card-pkg-amount"><sup>₹</sup>300</div>
        </div>
        <div class="map-card-notch">
          <div class="map-card-notch-circle"></div>
          <div class="map-card-notch-line"></div>
          <div class="map-card-notch-circle"></div>
        </div>
        <div class="map-card-body">
          <div class="map-info-row">
            <div class="map-info-label">⏱ Valid Days</div>
            <div class="map-info-value">30 Days</div>
          </div>
          <div class="map-info-row cashback">
            <div class="map-info-label">🎯 Cashback Points</div>
            <div class="map-info-value">600 Pts</div>
          </div>
          <a href="#" class="map-card-btn">Activate Plan →</a>
        </div>
      </div>

      <!-- Plan 2 — ₹900 -->
      <div class="map-card" data-anim>
        <div class="map-card-header">
          <div class="map-card-icon">🚀</div>
          <div class="map-card-pkg-label">Package Amount</div>
          <div class="map-card-pkg-amount"><sup>₹</sup>900</div>
        </div>
        <div class="map-card-notch">
          <div class="map-card-notch-circle"></div>
          <div class="map-card-notch-line"></div>
          <div class="map-card-notch-circle"></div>
        </div>
        <div class="map-card-body">
          <div class="map-info-row">
            <div class="map-info-label">⏱ Valid Days</div>
            <div class="map-info-value">90 Days</div>
          </div>
          <div class="map-info-row cashback">
            <div class="map-info-label">🎯 Cashback Points</div>
            <div class="map-info-value">2,100 Pts</div>
          </div>
          <a href="#" class="map-card-btn">Activate Plan →</a>
        </div>
      </div>

      <!-- Plan 3 — ₹1800 -->
      <div class="map-card" data-anim>
        <div class="map-card-header">
           <div class="map-card-icon">💎</div>
          <div class="map-card-pkg-label">Package Amount</div>
          <div class="map-card-pkg-amount"><sup>₹</sup>1,800</div>
        </div>
        <div class="map-card-notch">
          <div class="map-card-notch-circle"></div>
          <div class="map-card-notch-line"></div>
          <div class="map-card-notch-circle"></div>
        </div>
        <div class="map-card-body">
          <div class="map-info-row">
            <div class="map-info-label">⏱ Valid Days</div>
            <div class="map-info-value">180 Days</div>
          </div>
          <div class="map-info-row cashback">
            <div class="map-info-label">🎯 Cashback Points</div>
            <div class="map-info-value">4,200 Pts</div>
          </div>
          <a href="#" class="map-card-btn">Activate Plan →</a>
        </div>
      </div>

      <!-- Plan 4 — ₹3600 -->
      <div class="map-card" data-anim>
        <div class="map-card-header">
          <div class="map-card-icon">👑</div>
          <div class="map-card-pkg-label">Package Amount</div>
          <div class="map-card-pkg-amount"><sup>₹</sup>3,600</div>
        </div>
        <div class="map-card-notch">
          <div class="map-card-notch-circle"></div>
          <div class="map-card-notch-line"></div>
          <div class="map-card-notch-circle"></div>
        </div>
        <div class="map-card-body">
          <div class="map-info-row">
            <div class="map-info-label">⏱ Valid Days</div>
            <div class="map-info-value">360 Days</div>
          </div>
          <div class="map-info-row cashback">
            <div class="map-info-label">🎯 Cashback Points</div>
            <div class="map-info-value">8,400 Pts</div>
          </div>
          <a href="#" class="map-card-btn">Activate Plan →</a>
        </div>
      </div>

    </div>
  </div>
</section>


 


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
      if (e.isIntersecting) { e.target.style.opacity = '1'; e.target.style.transform = 'translateY(0)'; }
    });
  }, { threshold: 0.08 });

  document.querySelectorAll('[data-anim]').forEach((el, i) => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(24px)';
    el.style.transition = `opacity .5s ease ${i * 0.1}s, transform .5s ease ${i * 0.1}s`;
    observer.observe(el);
  });
</script>

</body>
</html>
