<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="icon" type="image/x-icon" href="demoepay/images/favicon.png">
<title>Privacy Policy – ePay Digital India Pvt. Ltd.</title>
<link href="https://fonts.googleapis.com/css2?family=Sora:wght@300;400;500;600;700;800&family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet">
<link rel="stylesheet" href="css/custom_stylesheet.css">
<link rel="stylesheet" href="css/style.css">
<style>
  :root {
    --orange: #E84000;
    --orange-light: #FF5722;
    --orange-glow: rgba(232,64,0,0.12);
    --dark: #0D1117;
    --muted: #6B7280;
    --border: #E2E8F0;
    --text: #1A1A2E;
  }
  .page-hero {
    background: linear-gradient(135deg, #f8fafc 0%, #ffffff 60%, #f0f4ff 100%);
    padding: 80px 20px 70px;
    text-align: center;
    position: relative;
    overflow: hidden;
    border-bottom: 1px solid var(--border);
  }
  .page-hero::before {
    content: '';
    position: absolute;
    inset: 0;
    background: radial-gradient(ellipse 70% 60% at 50% 0%, rgba(232,64,0,0.07) 0%, transparent 70%);
  }
  .page-hero-inner { position: relative; z-index: 1; max-width: 760px; margin: auto; }
  .page-hero-badge {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    background: rgba(232,64,0,0.10);
    border: 1px solid rgba(232,64,0,0.25);
    color: var(--orange);
    font-size: .78rem;
    font-weight: 700;
    letter-spacing: .8px;
    text-transform: uppercase;
    padding: 6px 16px;
    border-radius: 50px;
    margin-bottom: 22px;
  }
  .page-hero h1 {
    font-family: 'Sora', sans-serif;
    font-size: clamp(1.9rem, 4.5vw, 3rem);
    font-weight: 800;
    color: var(--text);
    line-height: 1.25;
    margin-bottom: 18px;
  }
  .page-hero h1 span { color: var(--orange); }
  .page-hero p {
    color: var(--muted);
    font-size: 1.05rem;
    line-height: 1.8;
    max-width: 620px;
    margin: 0 auto;
  }
  .page-content {
    max-width: 900px;
    margin: 60px auto;
    padding: 0 20px 80px;
  }
  .coming-soon-box {
    text-align: center;
    padding: 80px 40px;
    background: #f9fafb;
    border: 2px dashed var(--border);
    border-radius: 20px;
  }
  .coming-soon-box .cs-icon { font-size: 4rem; margin-bottom: 20px; }
  .coming-soon-box h2 {
    font-family: 'Sora', sans-serif;
    font-size: 1.6rem;
    font-weight: 800;
    color: var(--text);
    margin-bottom: 12px;
  }
  .coming-soon-box p { color: var(--muted); font-size: .95rem; line-height: 1.8; }
</style>
</head>
<body>

<!-- #include file="inc_header.asp" -->

<div class="search-bar-wrap">
  <div class="search-inner">
    <div class="search-box">
      <div class="search-cat">
        <select>
          <option>All Categories</option>
          <option>Mobiles</option>
          <option>Fashion</option>
          <option>Electronics</option>
          <option>Home & Kitchen</option>
          <option>Beauty</option>
          <option>Books</option>
          <option>Baby & Kids</option>
          <option>Sports</option>
        </select>
        <span>▾</span>
      </div>
      <div class="search-input-wrap">
        <span class="search-icon">🔍</span>
        <input type="text" placeholder="Search for products, services and more...">
      </div>
    </div>
  </div>
</div>

<section class="page-hero">
  <div class="page-hero-inner">
    <div class="page-hero-badge">&#10003; Legal</div>
    <h1>Privacy <span>Policy</span></h1>
    <p>We value your privacy. Learn how we collect, use, and protect your personal data at ePay Digital India.</p>
  </div>
</section>

<div class="page-content">
  <div class="coming-soon-box">
    <div class="cs-icon">🔒</div>
    <h2>Content Coming Soon</h2>
    <p>We are currently working on this page. Our complete Privacy Policy will be published here shortly.</p>
  </div>
</div>

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
</script>

</body>
</html>
