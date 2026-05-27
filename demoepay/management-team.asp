<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="icon" type="image/x-icon" href="demoepay/images/favicon.png">
<title>Management Team – ePay Digital India Pvt. Ltd.</title>
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

  /* ── Page Hero ── */
  .pg-hero {
    background: linear-gradient(135deg, #f8fafc 0%, #ffffff 60%, #f0f4ff 100%);
    padding: 70px 20px 60px;
    text-align: center;
    position: relative;
    overflow: hidden;
    border-bottom: 1px solid var(--border);
  }
  .pg-hero::before {
    content: '';
    position: absolute;
    inset: 0;
    background: radial-gradient(ellipse 70% 55% at 50% 0%, rgba(232,64,0,.07) 0%, transparent 70%);
  }
  .pg-hero-inner { position: relative; z-index: 1; max-width: 680px; margin: auto; }
  .pg-hero-badge {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    background: rgba(232,64,0,.10);
    border: 1px solid rgba(232,64,0,.25);
    color: var(--orange);
    font-size: .76rem;
    font-weight: 700;
    letter-spacing: .9px;
    text-transform: uppercase;
    padding: 6px 16px;
    border-radius: 50px;
    margin-bottom: 20px;
  }
  .pg-hero h1 {
    font-family: 'Sora', sans-serif;
    font-size: clamp(1.7rem, 4vw, 2.7rem);
    font-weight: 800;
    color: var(--text);
    line-height: 1.25;
    margin-bottom: 14px;
  }
  .pg-hero h1 span { color: var(--orange); }
  .pg-hero p { color: var(--muted); font-size: .97rem; line-height: 1.75; }

  /* ── Breadcrumb ── */
  .pg-breadcrumb {
    background: #f9fafb;
    border-bottom: 1px solid var(--border);
    padding: 12px 30px;
    font-size: .82rem;
    color: var(--muted);
  }
  .pg-breadcrumb a { color: var(--orange); text-decoration: none; font-weight: 600; }
  .pg-breadcrumb a:hover { text-decoration: underline; }
  .pg-breadcrumb span { margin: 0 6px; }

  /* ── Team Section ── */
  .mt-section {
    padding: 64px 20px 80px;
    background: #f9fafb;
  }
  .mt-wrap { max-width: 1120px; margin: auto; }

  .mt-section-label {
    display: flex;
    align-items: center;
    gap: 14px;
    margin-bottom: 48px;
  }
  .mt-section-label .line {
    flex: 1;
    height: 1px;
    background: linear-gradient(90deg, var(--border), transparent);
  }
  .mt-section-label .line.left { background: linear-gradient(270deg, var(--border), transparent); }
  .mt-section-label-text {
    font-family: 'Sora', sans-serif;
    font-size: .8rem;
    font-weight: 700;
    letter-spacing: 1.2px;
    text-transform: uppercase;
    color: var(--orange);
    white-space: nowrap;
  }

  /* ── Team Grid ── */
  .mt-grid {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 24px;
  }

  /* ── Member Card ── */
  .mt-card {
    background: #fff;
    border: 1px solid var(--border);
    border-radius: 20px;
    overflow: hidden;
    text-align: center;
    transition: box-shadow .3s, transform .3s, border-color .3s;
    position: relative;
  }
  .mt-card:hover {
    box-shadow: 0 14px 40px rgba(0,0,0,.1);
    transform: translateY(-5px);
    border-color: rgba(232,64,0,.25);
  }
  .mt-card::after {
    content: '';
    position: absolute;
    top: 0; left: 0; right: 0;
    height: 3px;
    background: linear-gradient(90deg, var(--orange), #FF8C42);
    opacity: 0;
    transition: opacity .3s;
  }
  .mt-card:hover::after { opacity: 1; }

  /* Photo */
  .mt-photo-wrap {
    width: 100%;
    aspect-ratio: 1 / 1.1;
    overflow: hidden;
    position: relative;
    background: linear-gradient(160deg, #f0f2f5, #e8eaf0);
  }
  .mt-photo-wrap img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    object-position: top center;
    display: block;
    transition: transform .4s ease;
  }
  .mt-card:hover .mt-photo-wrap img { transform: scale(1.06); }

  /* Info */
  .mt-info {
    padding: 18px 16px 20px;
  }
  .mt-info h3 {
    font-family: 'Sora', sans-serif;
    font-size: .92rem;
    font-weight: 700;
    color: var(--text);
    margin: 0 0 6px;
    line-height: 1.3;
  }
  .mt-info .mt-role-badge {
    display: inline-flex;
    align-items: center;
    gap: 5px;
    background: var(--orange-glow);
    color: var(--orange);
    font-size: .7rem;
    font-weight: 700;
    letter-spacing: .5px;
    padding: 4px 12px;
    border-radius: 50px;
    text-transform: uppercase;
  }

  /* ── Responsive ── */
  @media (max-width: 1024px) {
    .mt-grid { grid-template-columns: repeat(3, 1fr); }
  }
  @media (max-width: 680px) {
    .mt-grid { grid-template-columns: repeat(2, 1fr); gap: 16px; }
    .mt-section { padding: 44px 14px 60px; }
  }
  @media (max-width: 420px) {
    .mt-grid { grid-template-columns: repeat(2, 1fr); gap: 12px; }
    .pg-hero { padding: 50px 16px 44px; }
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

<!-- ─── HERO ─── -->
<section class="pg-hero">
  <div class="pg-hero-inner">
    <div class="pg-hero-badge">&#128101; Our Team</div>
    <h1>Management <span>Team</span></h1>
    <p>Meet the dedicated professionals driving ePay Digital India's mission — a team of passionate leaders committed to innovation, growth, and digital empowerment across the nation.</p>
  </div>
</section>

<!-- ─── BREADCRUMB ─── -->
<div class="pg-breadcrumb">
  <a href="index.asp">Home</a><span>&#8250;</span>
  <a href="aboutus.asp">About Us</a><span>&#8250;</span>
  Management Team
</div>

<!-- ─── TEAM GRID ─── -->
<section class="mt-section">
  <div class="mt-wrap">

    <div class="mt-section-label">
      <div class="line left"></div>
      <div class="mt-section-label-text">Our Leadership</div>
      <div class="line"></div>
    </div>

    <div class="mt-grid">

      <!-- 1 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/1.%20Dr%20.Dhanpal%20Wathare.jpeg" alt="Dr. Dhanpal Wathare">
        </div>
        <div class="mt-info">
          <h3>Dr. Dhanpal Wathare</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 2 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/2.%20Mr.%20Suhas%20Kadam.png" alt="Mr. Suhas Kadam">
        </div>
        <div class="mt-info">
          <h3>Mr. Suhas Kadam</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 3 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/3.%20Mr.%20Krishna%20Gavali.png" alt="Mr. Krishna Gavali">
        </div>
        <div class="mt-info">
          <h3>Mr. Krishna Gavali</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 4 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/4.%20Mr.%20Sandip%20Rane.jpeg" alt="Mr. Sandip Rane">
        </div>
        <div class="mt-info">
          <h3>Mr. Sandip Rane</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 5 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/5.%20Mr.%20Veekkas%20Purii.jpeg" alt="Mr. Veekkas Purii">
        </div>
        <div class="mt-info">
          <h3>Mr. Veekkas Purii</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 6 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/6.%20Mr.Shashikant%20Patil.jpeg" alt="Mr. Shashikant Patil">
        </div>
        <div class="mt-info">
          <h3>Mr. Shashikant Patil</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 7 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/7.%20Mr.%20Santosh%20Patil..jpeg" alt="Mr. Santosh Patil">
        </div>
        <div class="mt-info">
          <h3>Mr. Santosh Patil</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 8 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
           <img src="demoepay/images/management-team/8. Mr.Vijaykumar Sawant.jpg" alt="Mr. Vijaykumar Sawant">
        </div>
        <div class="mt-info">
          <h3>Mr. Vijaykumar Sawant</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 9 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
           <img src="demoepay/images/management-team/9.%20Mr.%20Maruti%20Kore.jpeg" alt="Mr. Maruti Kore">
        </div>
        <div class="mt-info">
          <h3>Mr. Maruti Kore</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 10 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
           <img src="demoepay/images/management-team/10.%20Mr.%20Chandrakant%20Vatigave%20..png" alt="Mr. Chandrakant Vatigave">
        </div>
        <div class="mt-info">
          <h3>Mr. Chandrakant Vatigave</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 11 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/11.%20Mr.%20Pradeep%20Dalavi.png" alt="Mr. Pradeep Dalavi">
        </div>
        <div class="mt-info">
          <h3>Mr. Pradeep Dalavi</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 12 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/12.%20Mr.%20Dinkar%20khot.png" alt="Mr. Dinkar Khot">
        </div>
        <div class="mt-info">
          <h3>Mr. Dinkar Khot</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 13 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/13.%20Mr.%20Prashant%20Kadam.png" alt="Mr. Prashant Kadam">
        </div>
        <div class="mt-info">
          <h3>Mr. Prashant Kadam</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 14 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/14.%20Mr.%20Sandip%20Patil.jpeg" alt="Mr. Sandip Patil">
        </div>
        <div class="mt-info">
          <h3>Mr. Sandip Patil</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

      <!-- 15 -->
      <div class="mt-card">
        <div class="mt-photo-wrap">
          <img src="demoepay/images/management-team/15.%20Mr.%20Jayprakash%20Dhanavde.png" alt="Mr. Jayprakash Dhanavde">
        </div>
        <div class="mt-info">
          <h3>Mr. Jayprakash Dhanavde</h3>
          <span class="mt-role-badge">&#9733; Management</span>
        </div>
      </div>

    </div>
  </div>
</section>

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

  // Staggered entrance animations
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(e => {
      if (e.isIntersecting) {
        e.target.style.opacity = '1';
        e.target.style.transform = 'translateY(0) scale(1)';
      }
    });
  }, { threshold: 0.08 });

  document.querySelectorAll('.mt-card').forEach((el, i) => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(24px) scale(0.97)';
    el.style.transition = `opacity .5s ease ${i * 0.05}s, transform .5s ease ${i * 0.05}s`;
    observer.observe(el);
  });
</script>

</body>
</html>
