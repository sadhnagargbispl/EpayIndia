<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="icon" type="image/x-icon" href="demoepay/images/favicon.png">
<title>Board of Directors – ePay Digital India Pvt. Ltd.</title>
<link href="https://fonts.googleapis.com/css2?family=Sora:wght@300;400;500;600;700;800&family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet">
<link rel="stylesheet" href="css/custom_stylesheet.css">
<link rel="stylesheet" href="css/style.css">
<style>
  :root {
    --orange: #E84000;
    --orange-light: #FF5722;
    --orange-glow: rgba(232,64,0,0.12);
    --dark: #0D1117;
    --muted: #0D1117;
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

  /* ── Directors Section ── */
  .bod-section {
    padding: 64px 20px 80px;
    background: #fff;
  }
  .bod-wrap { max-width: 1050px; margin: auto; }

  .bod-section-label {
    display: flex;
    align-items: center;
    gap: 14px;
    margin-bottom: 48px;
  }
  .bod-section-label .line {
    flex: 1;
    height: 1px;
    background: linear-gradient(90deg, var(--border), transparent);
  }
  .bod-section-label .line.left { background: linear-gradient(270deg, var(--border), transparent); }
  .bod-section-label-text {
    font-family: 'Sora', sans-serif;
    font-size: .8rem;
    font-weight: 700;
    letter-spacing: 1.2px;
    text-transform: uppercase;
    color: var(--orange);
    white-space: nowrap;
  }

  /* ── Director Card ── */
  .bod-card {
    display: grid;
    grid-template-columns: 300px 1fr;
    gap: 0;
    background: #fff;
    border: 1px solid var(--border);
    border-radius: 24px;
    overflow: hidden;
    margin-bottom: 40px;
    transition: box-shadow .35s, transform .35s;
    box-shadow: 0 4px 20px rgba(0,0,0,.05);
  }
  .bod-card:hover {
    box-shadow: 0 16px 48px rgba(0,0,0,.1);
    transform: translateY(-4px);
  }
  .bod-card:last-child { margin-bottom: 0; }

  /* Photo side */
  .bod-photo-side {
    position: relative;
    background: linear-gradient(160deg, #0D1117, #1a2744);
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: flex-end;
    overflow: hidden;
    min-height: 380px;
  }
  .bod-photo-side::before {
    content: '';
    position: absolute;
    inset: 0;
    background: radial-gradient(ellipse 80% 60% at 50% 100%, rgba(232,64,0,.22) 0%, transparent 65%);
  }
  .bod-photo-side img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    object-position: top center;
    display: block;
    position: absolute;
    top: 0; left: 0;
  }
  .bod-photo-overlay {
    position: relative;
    z-index: 2;
    width: 100%;
    padding: 20px 22px;
    background: linear-gradient(0deg, rgba(13,17,23,.95) 0%, rgba(13,17,23,.5) 70%, transparent 100%);
  }
  .bod-number {
    font-family: 'Sora', sans-serif;
    font-size: 2.5rem;
    font-weight: 800;
    color: rgba(255,140,66,.18);
    line-height: 1;
    margin-bottom: 4px;
  }
  .bod-photo-name {
    font-family: 'Sora', sans-serif;
    font-size: 1rem;
    font-weight: 700;
    color: #fff;
    margin-bottom: 4px;
  }
.bod-photo-role {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    background: rgb(255 255 255 / 20%);
    border: 1px solid rgb(255 255 255 / 40%);
    color: #ffffff;
    font-size: .72rem;
    font-weight: 700;
    letter-spacing: .6px;
    text-transform: uppercase;
    padding: 4px 12px;
    border-radius: 50px;
}

  /* Content side */
  .bod-content-side {
    padding: 42px 44px;
    display: flex;
    flex-direction: column;
    justify-content: center;
  }
  .bod-content-tag {
    display: inline-block;
    background: var(--orange-glow);
    color: var(--orange);
    font-size: .73rem;
    font-weight: 700;
    letter-spacing: .8px;
    text-transform: uppercase;
    padding: 5px 14px;
    border-radius: 50px;
    margin-bottom: 16px;
    width: 180px;
  }
  .bod-content-side h2 {
    font-family: 'Sora', sans-serif;
    font-size: 1.55rem;
    font-weight: 800;
    color: var(--text);
    margin-bottom: 6px;
    line-height: 1.2;
  }
  .bod-content-side .bod-title-line {
    width: 100%;
    height: 3px;
    background: linear-gradient(90deg, var(--orange), #FF8C42);
    border-radius: 2px;
    margin-bottom: 20px;
  }
  .bod-content-side p {
    color: var(--muted);
    font-size: .93rem;
    line-height: 1.85;
    margin: 0;
  }

  /* ── Responsive ── */
  @media (max-width: 820px) {
    .bod-card { grid-template-columns: 1fr; }
    .bod-photo-side { min-height: 320px; }
    .bod-photo-side img { object-position: center top; }
    .bod-content-side { padding: 28px 26px; }
  }
  @media (max-width: 480px) {
    .pg-hero { padding: 50px 16px 44px; }
    .bod-section { padding: 44px 14px 60px; }
    .bod-photo-side { min-height: 260px; }
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
    <div class="pg-hero-badge">&#11088; Leadership</div>
    <h1>Board of <span>Directors</span></h1>
    <p>Our board brings together visionary leaders with deep expertise in technology, business growth, and digital innovation — steering ePay Digital India towards sustainable success.</p>
  </div>
</section>

<!-- ─── BREADCRUMB ─── -->
<div class="pg-breadcrumb">
  <a href="index.asp">Home</a><span>&#8250;</span>
  <a href="aboutus.asp">About Us</a><span>&#8250;</span>
  Board of Directors
</div>

<!-- ─── DIRECTORS ─── -->
<section class="bod-section">
  <div class="bod-wrap">

    <div class="bod-section-label">
      <div class="line left"></div>
      <div class="bod-section-label-text">Meet Our Board</div>
      <div class="line"></div>
    </div>

    <!-- Director 1 – Mr. Yash Navale -->
    <div class="bod-card">
      <div class="bod-photo-side">
        <img src="demoepay/images/board-of-directors/1.%20Mr.%20Yash%20Navale%20%20(Board%20of%20Director).jpeg" alt="Mr. Yash Navale">
        <div class="bod-photo-overlay">
           <div class="bod-photo-role">&#9733; Board of Director</div>
        </div>

      </div>
      <div class="bod-content-side">
        
        <h2>Mr. Yash Navale.</h2>
        <div class="bod-title-line"></div>
 
        <div class="bod-content-tag">Strategic Vision</div>
        <p>Mr. Yash Navale plays a key role in driving the company's strategic vision through strong leadership, innovation, and long-term planning. He focuses on identifying new business opportunities and expanding the company's presence across markets to achieve sustainable growth.</p>
        <br>
        <p>With a forward-thinking approach, he ensures that all decisions align with the organization's future goals and scalability. He actively promotes modern business models and technology-driven solutions to stay competitive. His expertise in structured planning and governance supports effective decision-making and overall business success.</p>
      </div>
    </div>

    <!-- Director 2 – Mr. Prakash Pawar -->
    <div class="bod-card">
      <div class="bod-photo-side">
        <img src="demoepay/images/board-of-directors/2.%20Mr.%20Prakash%20Pawar%20(Board%20of%20Director).jpeg" alt="Mr. Prakash Pawar">
        <div class="bod-photo-overlay">
           <div class="bod-photo-role">&#9733; Board of Director</div>
        </div>
      </div>
      <div class="bod-content-side">
        
        <h2>Mr. Prakash Pawar. </h2>
        <div class="bod-title-line"></div>
        <div class="bod-content-tag">Business Expansion</div>
        <p>Mr. Prakash Pawar plays a vital role in driving the company's strategic vision and nationwide expansion. He focuses on leading PAN India growth by identifying new markets, building strong regional presence, and strengthening the franchise network.</p>
        <br>
        <p>With a result-oriented approach, he ensures scalable business development and consistent performance across locations. His leadership supports long-term planning, market penetration, and sustainable growth while maintaining strong operational alignment across the organization.</p>
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

  // Entrance animations
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(e => {
      if (e.isIntersecting) {
        e.target.style.opacity = '1';
        e.target.style.transform = 'translateY(0)';
      }
    });
  }, { threshold: 0.08 });

  document.querySelectorAll('.bod-card').forEach((el, i) => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(28px)';
    el.style.transition = `opacity .6s ease ${i * 0.15}s, transform .6s ease ${i * 0.15}s`;
    observer.observe(el);
  });
</script>

</body>
</html>
