<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="icon" type="image/x-icon" href="images/favicon.png">
<title>Purchase Coupon – ePay Digital India</title>
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
  .cp-hero {
    background: linear-gradient(135deg,#f0fdf4 0%,#fff7ed 55%,#fef9f0 100%);
    padding:70px 20px 60px; text-align:center;
    position:relative; overflow:hidden; border-bottom:1px solid var(--border);
  }
  .cp-hero::before {
    content:''; position:absolute; inset:0;
    background:radial-gradient(ellipse 70% 55% at 50% 0%,rgba(22,163,74,.07) 0%,transparent 65%);
  }
  .cp-hero-inner { position:relative; z-index:1; max-width:700px; margin:auto; }
  .cp-hero-badge {
    display:inline-flex; align-items:center; gap:8px;
    background:var(--og); border:1px solid rgba(232,64,0,.28); color:var(--orange);
    font-size:.78rem; font-weight:700; letter-spacing:.9px; text-transform:uppercase;
    padding:6px 18px; border-radius:50px; margin-bottom:20px;
  }
  .cp-badge-dot { width:8px; height:8px; background:var(--orange); border-radius:50%; animation:blink 1.4s infinite; }
  @keyframes blink { 0%,100%{opacity:1} 50%{opacity:.3} }
  .cp-hero h1 {
    font-family:'Sora',sans-serif; font-size:clamp(2rem,5vw,3rem);
    font-weight:800; color:var(--text); line-height:1.2; margin-bottom:16px;
  }
  .cp-hero h1 span { color:var(--orange); }
  .cp-hero p { color:var(--muted); font-size:1rem; line-height:1.8; max-width:540px; margin:0 auto; }

  .cp-stats { background:#fff; border-bottom:1px solid var(--border); padding:28px 20px; }
  .cp-stats-row { max-width:900px; margin:auto; display:flex; justify-content:center; align-items:center; flex-wrap:wrap; }
  .cp-stat { text-align:center; padding:8px 36px; }
  .cp-stat-num { font-family:'Sora',sans-serif; font-size:1.9rem; font-weight:800; color:var(--orange); line-height:1; }
  .cp-stat-label { font-size:.75rem; font-weight:600; color:var(--muted); text-transform:uppercase; letter-spacing:.5px; margin-top:4px; }
  .cp-stat-div { width:1px; height:46px; background:var(--border); }

  .cp-section { padding:60px 20px; }
  .cp-section.bg-light { background:#f9fafb; }
  .cp-wrap { max-width:1100px; margin:auto; }
  .cp-section-tag {
    display:inline-block; font-size:.73rem; font-weight:700; letter-spacing:.8px;
    text-transform:uppercase; padding:5px 14px; border-radius:50px; margin-bottom:10px;
  }
  .food-tag  { background:var(--og); color:var(--orange); }
  .movie-tag { background:var(--gg); color:var(--green); }
  .cp-section-title {
    font-family:'Sora',sans-serif; font-size:clamp(1.4rem,2.8vw,2rem);
    font-weight:800; color:var(--text); margin-bottom:8px;
  }
  .cp-section-title .ot { color:var(--orange); }
  .cp-section-title .gt { color:var(--green); }
  .cp-section-sub { color:var(--muted); font-size:.93rem; line-height:1.75; margin-bottom:36px; }

  /* COUPON GRID */
  .coupon-grid {
    display:grid;
    grid-template-columns:repeat(auto-fill,minmax(320px,1fr));
    gap:22px;
  }
  .coupon-card {
    background:#fff; border-radius:16px; overflow:hidden;
    display:flex; flex-direction:column;
    box-shadow:0 4px 24px rgba(0,0,0,.07); border:1px solid var(--border);
    transition:transform .28s, box-shadow .28s;
  }
  .coupon-card:hover { transform:translateY(-6px); box-shadow:0 14px 40px rgba(0,0,0,.12); }

  .coupon-stripe { height:6px; }
  .food-card  .coupon-stripe { background:linear-gradient(90deg,var(--orange),#ff8c42); }
  .movie-card .coupon-stripe { background:linear-gradient(90deg,var(--green),#4ade80); }

  .coupon-body { display:flex; align-items:stretch; flex:1; }

  .coupon-left {
    display:flex; flex-direction:column; align-items:center; justify-content:center;
    padding:24px 18px; min-width:105px;
  }
  .food-card  .coupon-left { background:linear-gradient(160deg,#fff7ed,#ffedd5); }
  .movie-card .coupon-left { background:linear-gradient(160deg,#f0fdf4,#dcfce7); }

  .coupon-icon-big { font-size:1.8rem; margin-bottom:8px; }
  .coupon-disc-pct { font-family:'Sora',sans-serif; font-weight:800; font-size:2.2rem; line-height:1; }
  .food-card  .coupon-disc-pct { color:var(--orange); }
  .movie-card .coupon-disc-pct { color:var(--green); }
  .coupon-disc-label { font-size:.65rem; font-weight:700; letter-spacing:.6px; text-transform:uppercase; margin-top:4px; color:var(--muted); }

  .coupon-divider { display:flex; flex-direction:column; align-items:center; width:22px; flex-shrink:0; position:relative; }
  .coupon-divider::before, .coupon-divider::after {
    content:''; display:block; width:20px; height:20px; border-radius:50%;
    background:#f3f4f6; border:1px solid var(--border); position:absolute;
  }
  .coupon-divider::before { top:-10px; }
  .coupon-divider::after  { bottom:-10px; }
  .coupon-divider-line { width:2px; height:100%; border-left:2px dashed #d1d5db; margin:0 auto; }

  .coupon-right {
    flex:1; padding:20px 20px 20px 16px;
    display:flex; flex-direction:column; justify-content:space-between;
  }
  .coupon-qty { font-family:'Sora',sans-serif; font-size:1rem; font-weight:800; color:var(--text); margin-bottom:4px; }
  .coupon-qty span { font-size:.78rem; font-weight:500; color:var(--muted); }
  .coupon-price-row { display:flex; align-items:baseline; gap:8px; margin-bottom:10px; }
  .coupon-price {
    font-family:'Sora',sans-serif; font-size:1.3rem; font-weight:800;
    color:var(--muted); text-decoration:line-through; text-decoration-color:#dc2626;
    text-decoration-thickness: 2px;
  }
  .coupon-price-small { font-size:.75rem; color:var(--muted); }
  .coupon-wallet-price {
    font-family:'Sora',sans-serif; font-size:1.25rem; font-weight:800;
    color:var(--text); margin-bottom:10px; margin-top:2px;
  }
  .coupon-tags { display:flex; flex-wrap:wrap; gap:6px; margin-bottom:14px; }
  .coupon-tag { font-size:.72rem; font-weight:700; padding:4px 10px; border-radius:50px; }
  .coupon-tag.save { background:rgba(220,38,38,.08); color:#dc2626; border:1px solid rgba(220,38,38,.2); }
  .food-card  .coupon-tag.wallet { background:var(--og); color:var(--orange); border:1px solid rgba(232,64,0,.22); }
  .movie-card .coupon-tag.wallet { background:var(--gg); color:var(--green);  border:1px solid rgba(22,163,74,.22); }

  .coupon-btn {
    display:block; text-align:center; padding:10px 0;
    border-radius:50px; font-family:'Sora',sans-serif;
    font-size:.84rem; font-weight:700; text-decoration:none;
    transition:transform .2s, box-shadow .2s;
  }
  .food-card  .coupon-btn { background:var(--orange); color:#fff; box-shadow:0 5px 16px rgba(232,64,0,.3); }
  .movie-card .coupon-btn { background:var(--green);  color:#fff; box-shadow:0 5px 16px rgba(22,163,74,.3); }
  .coupon-btn:hover { transform:translateY(-2px); box-shadow:0 8px 22px rgba(0,0,0,.18); }

  .cp-cta {
    background:linear-gradient(135deg,#f0fdf4 0%,#fff7ed 100%);
    padding:65px 20px; text-align:center;
    position:relative; overflow:hidden; border-top:1px solid var(--border);
  }
  .cp-cta::before {
    content:''; position:absolute; inset:0;
    background:radial-gradient(ellipse 60% 50% at 50% 0%,rgba(22,163,74,.07) 0%,transparent 65%);
  }
  .cp-cta-inner { position:relative; z-index:1; max-width:640px; margin:auto; }
  .cp-cta h2 { font-family:'Sora',sans-serif; font-size:clamp(1.5rem,3.5vw,2.2rem); font-weight:800; color:var(--text); margin-bottom:12px; }
  .cp-cta h2 span { color:var(--orange); }
  .cp-cta p { color:var(--muted); font-size:.95rem; line-height:1.8; margin-bottom:30px; }
  .cp-cta-btns { display:flex; gap:14px; justify-content:center; flex-wrap:wrap; }
  .cp-btn-p {
    background:var(--orange); color:#fff; font-family:'Sora',sans-serif; font-weight:700; font-size:.9rem;
    padding:13px 32px; border-radius:50px; text-decoration:none;
    box-shadow:0 8px 24px rgba(232,64,0,.28); transition:background .25s,transform .2s;
  }
  .cp-btn-p:hover { background:#c73600; transform:translateY(-2px); }
  .cp-btn-o {
    background:#fff; color:var(--green); font-family:'Sora',sans-serif; font-weight:700; font-size:.9rem;
    padding:13px 32px; border-radius:50px; text-decoration:none;
    border:1.5px solid rgba(22,163,74,.35); transition:background .25s,transform .2s;
  }
  .cp-btn-o:hover { background:#f0fdf4; transform:translateY(-2px); }

  @media(max-width:640px) {
    .coupon-grid { grid-template-columns:1fr; }
    .cp-stat { padding:8px 20px; }
    .cp-stat-div { height:34px; }
  }
</style>
</head>
<body>

<!-- #include file="inc_header.asp" -->

<div class="search-bar-wrap">
  <div class="search-inner">
    <div class="search-box">
      <div class="search-cat">
        <select>
          <option>All Categories</option><option>Mobiles</option><option>Fashion</option>
          <option>Electronics</option><option>Home &amp; Kitchen</option>
          <option>Beauty</option><option>Books</option><option>Baby &amp; Kids</option><option>Sports</option>
        </select><span>&#9662;</span>
      </div>
      <div class="search-input-wrap">
        <span class="search-icon">🔍</span>
        <input type="text" placeholder="Search for products, services and more...">
      </div>
    </div>
  </div>
</div>

<section class="cp-hero">
  <div class="cp-hero-inner">
    <div class="cp-hero-badge"><div class="cp-badge-dot"></div>ePay India — Purchase Coupon</div>
    <h1>Purchase <span>Coupons</span> &amp;<br>Save More Every Time</h1>
    <p>Buy Food &amp; Movie coupons in bulk and enjoy exclusive discounts with Ecommerce Wallet balance — powered by ePay Digital India.</p>
  </div>
</section>

 

<!-- ═══ MOVIES PORTAL ═══ -->
<section class="cp-section bg-light">
  <div class="cp-wrap">
    <div class="cp-section-tag movie-tag">🎬 Movies Portal</div>
    <h2 class="cp-section-title">ePay Digital India — <span class="gt">Movies Portal</span></h2>
    <p class="cp-section-sub">Purchase movie tickets in bulk and get attractive discounts with instant Ecommerce Wallet balance.</p>
    <div class="coupon-grid">

      <div class="coupon-card movie-card" data-anim>
        <div class="coupon-stripe"></div>
        <div class="coupon-body">
          <div class="coupon-left">
            <div class="coupon-icon-big">🎬</div>
            <div class="coupon-disc-pct">30%</div>
            <div class="coupon-disc-label">OFF</div>
          </div>
          <div class="coupon-divider"><div class="coupon-divider-line"></div></div>
          <div class="coupon-right">
            <div>
              <div class="coupon-qty">10 Movie Tickets <span>/ Pack</span></div>
              <div class="coupon-price-row"><div class="coupon-price">₹3,000</div></div>
              <div class="coupon-wallet-price">₹2,100</div>
              <div class="coupon-tags">
                <span class="coupon-tag save">Save ₹900</span>
              </div>
            </div>
            <a href="purchase-coupon-details.asp" class="coupon-btn">Buy Now →</a>
          </div>
        </div>
      </div>

      <div class="coupon-card movie-card" data-anim>
        <div class="coupon-stripe"></div>
        <div class="coupon-body">
          <div class="coupon-left">
            <div class="coupon-icon-big">🎬</div>
            <div class="coupon-disc-pct">40%</div>
            <div class="coupon-disc-label">OFF</div>
          </div>
          <div class="coupon-divider"><div class="coupon-divider-line"></div></div>
          <div class="coupon-right">
            <div>
              <div class="coupon-qty">20 Movie Tickets <span>/ Pack</span></div>
              <div class="coupon-price-row"><div class="coupon-price">₹6,000</div></div>
              <div class="coupon-wallet-price">₹3,600</div>
              <div class="coupon-tags">
                <span class="coupon-tag save">Save ₹2,400</span>
              </div>
            </div>
           <a href="purchase-coupon-details.asp" class="coupon-btn">Buy Now →</a>
          </div>
        </div>
      </div>

      <div class="coupon-card movie-card" data-anim>
        <div class="coupon-stripe"></div>
        <div class="coupon-body">
          <div class="coupon-left">
            <div class="coupon-icon-big">🎬</div>
            <div class="coupon-disc-pct">50%</div>
            <div class="coupon-disc-label">OFF</div>
          </div>
          <div class="coupon-divider"><div class="coupon-divider-line"></div></div>
          <div class="coupon-right">
            <div>
              <div class="coupon-qty">40 Movie Tickets <span>/ Pack</span></div>
              <div class="coupon-price-row"><div class="coupon-price">₹12,000</div></div>
              <div class="coupon-wallet-price">₹6,000</div>
              <div class="coupon-tags">
                <span class="coupon-tag save">Save ₹6,000</span>
              </div>
            </div>
            <a href="purchase-coupon-details.asp" class="coupon-btn">Buy Now →</a>
          </div>
        </div>
      </div>

    </div>
  </div>
</section>


<!-- ═══ FOOD PORTAL ═══ -->
<section class="cp-section">
  <div class="cp-wrap">
    <div class="cp-section-tag food-tag">🍔 Food Portal</div>
    <h2 class="cp-section-title">ePay Digital India — <span class="ot">Food Portal</span></h2>
    <p class="cp-section-sub">Purchase food coupons in bulk and enjoy up to 50% discount with instant Ecommerce Wallet balance.</p>
    <div class="coupon-grid">

      <div class="coupon-card food-card" data-anim>
        <div class="coupon-stripe"></div>
        <div class="coupon-body">
          <div class="coupon-left">
            <div class="coupon-icon-big">🍔</div>
            <div class="coupon-disc-pct">20%</div>
            <div class="coupon-disc-label">OFF</div>
          </div>
          <div class="coupon-divider"><div class="coupon-divider-line"></div></div>
          <div class="coupon-right">
            <div>
              <div class="coupon-qty">5 Food Coupons <span>/ Pack</span></div>
              <div class="coupon-price-row"><div class="coupon-price">₹2,500</div></div>
              <div class="coupon-wallet-price">₹2,000</div>
              <div class="coupon-tags">
                <span class="coupon-tag save">Save ₹500</span>
              </div>
            </div>
            <a href="purchase-coupon-details.asp" class="coupon-btn">Buy Now →</a>
          </div>
        </div>
      </div>

      <div class="coupon-card food-card" data-anim>
        <div class="coupon-stripe"></div>
        <div class="coupon-body">
          <div class="coupon-left">
            <div class="coupon-icon-big">🍔</div>
            <div class="coupon-disc-pct">30%</div>
            <div class="coupon-disc-label">OFF</div>
          </div>
          <div class="coupon-divider"><div class="coupon-divider-line"></div></div>
          <div class="coupon-right">
            <div>
              <div class="coupon-qty">10 Food Coupons <span>/ Pack</span></div>
              <div class="coupon-price-row"><div class="coupon-price">₹5,000</div></div>
              <div class="coupon-wallet-price">₹3,500</div>
              <div class="coupon-tags">
                <span class="coupon-tag save">Save ₹1,500</span>
              </div>
            </div>
            <a href="purchase-coupon-details.asp" class="coupon-btn">Buy Now →</a>
          </div>
        </div>
      </div>

      <div class="coupon-card food-card" data-anim>
        <div class="coupon-stripe"></div>
        <div class="coupon-body">
          <div class="coupon-left">
            <div class="coupon-icon-big">🍔</div>
            <div class="coupon-disc-pct">40%</div>
            <div class="coupon-disc-label">OFF</div>
          </div>
          <div class="coupon-divider"><div class="coupon-divider-line"></div></div>
          <div class="coupon-right">
            <div>
              <div class="coupon-qty">20 Food Coupons <span>/ Pack</span></div>
              <div class="coupon-price-row"><div class="coupon-price">₹10,000</div></div>
              <div class="coupon-wallet-price">₹6,000</div>
              <div class="coupon-tags">
                <span class="coupon-tag save">Save ₹4,000</span>
              </div>
            </div>
            <a href="purchase-coupon-details.asp" class="coupon-btn">Buy Now →</a>
          </div>
        </div>
      </div>

      <div class="coupon-card food-card" data-anim>
        <div class="coupon-stripe"></div>
        <div class="coupon-body">
          <div class="coupon-left">
            <div class="coupon-icon-big">🍔</div>
            <div class="coupon-disc-pct">50%</div>
            <div class="coupon-disc-label">OFF</div>
          </div>
          <div class="coupon-divider"><div class="coupon-divider-line"></div></div>
          <div class="coupon-right">
            <div>
              <div class="coupon-qty">50 Food Coupons <span>/ Pack</span></div>
              <div class="coupon-price-row"><div class="coupon-price">₹25,000</div></div>
              <div class="coupon-wallet-price">₹12,500</div>
              <div class="coupon-tags">
                <span class="coupon-tag save">Save ₹12,500</span>
              </div>
            </div>
            <a href="purchase-coupon-details.asp" class="coupon-btn">Buy Now →</a>
          </div>
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
      if (e.isIntersecting) { e.target.style.opacity='1'; e.target.style.transform='translateY(0)'; }
    });
  }, { threshold: 0.06 });
  document.querySelectorAll('[data-anim]').forEach((el, i) => {
    el.style.opacity='0'; el.style.transform='translateY(22px)';
    el.style.transition=`opacity .5s ease ${i*.07}s, transform .5s ease ${i*.07}s`;
    observer.observe(el);
  });
</script>
</body>
</html>
