<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="icon" type="image/x-icon" href="images/favicon.png">
<title>About Us – ePay Digital India Pvt. Ltd.</title>
<link href="https://fonts.googleapis.com/css2?family=Sora:wght@300;400;500;600;700;800&family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet">
<link rel="stylesheet" href="css/custom_stylesheet.css">
<link rel="stylesheet" href="css/style.css">
<style>
  /* ── About Page Styles ── */
  :root {
    --orange: #E84000;
    --orange-light: #FF5722;
    --orange-glow: rgba(232,64,0,0.12);
    --dark: #0D1117;
    --muted: #6B7280;
    --border: #E2E8F0;
    --text: #1A1A2E;
  }

  /* ── Hero Banner ── */
  .abt-hero {
    background: linear-gradient(135deg, #f8fafc 0%, #ffffff 60%, #f0f4ff 100%);
    padding: 80px 20px 70px;
    text-align: center;
    position: relative;
    overflow: hidden;
    border-bottom: 1px solid var(--border);
  }
  .abt-hero::before {
    content: '';
    position: absolute;
    inset: 0;
    background: radial-gradient(ellipse 70% 60% at 50% 0%, rgba(232,64,0,0.07) 0%, transparent 70%);
  }
  .abt-hero-inner { position: relative; z-index: 1; max-width: 760px; margin: auto; }
  .abt-hero-badge {
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
  .abt-hero h1 {
    font-family: 'Sora', sans-serif;
    font-size: clamp(1.9rem, 4.5vw, 3rem);
    font-weight: 800;
    color: var(--text);
    line-height: 1.25;
    margin-bottom: 18px;
  }
  .abt-hero h1 span { color: var(--orange); }
  .abt-hero p {
    color: var(--muted);
    font-size: 1.05rem;
    line-height: 1.8;
    max-width: 620px;
    margin: 0 auto;
  }

  /* ── Stats Band ── */
  .abt-stats {
    background: #fff;
    border-bottom: 1px solid var(--border);
    padding: 40px 20px;
  }
  .abt-stats-grid {
    max-width: 960px;
    margin: auto;
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 20px;
    text-align: center;
  }
  .abt-stat-item { padding: 10px; }
  .abt-stat-num {
    font-family: 'Sora', sans-serif;
    font-size: 2rem;
    font-weight: 800;
    color: var(--orange);
    line-height: 1;
    margin-bottom: 6px;
  }
  .abt-stat-label { font-size: .82rem; color: var(--muted); font-weight: 600; text-transform: uppercase; letter-spacing: .5px; }
  .abt-stat-divider {
    width: 1px;
    background: var(--border);
    align-self: stretch;
    margin: 8px 0;
    display: none;
  }

  /* ── Section wrapper ── */
  .abt-section { padding: 70px 20px; }
  .abt-section.bg-light { background: #f9fafb; }
  .abt-wrap { max-width: 1100px; margin: auto; }
  .abt-section-tag {
    display: inline-block;
    background: var(--orange-glow);
    color: var(--orange);
    font-size: .75rem;
    font-weight: 700;
    letter-spacing: .8px;
    text-transform: uppercase;
    padding: 5px 14px;
    border-radius: 50px;
    margin-bottom: 14px;
  }
  .abt-section-title {
    font-family: 'Sora', sans-serif;
    font-size: clamp(1.4rem, 2.8vw, 2rem);
    font-weight: 800;
    color: var(--text);
    line-height: 1.3;
    margin-bottom: 14px;
  }
  .abt-section-title span { color: var(--orange); }
  .abt-section-sub { color: var(--muted); font-size: .95rem; line-height: 1.8; }

  /* ── Vision / Mission two-column ── */
  .abt-vm-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 28px;
    margin-top: 44px;
  }
  .abt-vm-card {
    background: #fff;
    border: 1px solid var(--border);
    border-radius: 20px;
    padding: 36px 32px;
    position: relative;
    overflow: hidden;
    transition: box-shadow .3s, transform .3s;
  }
  .abt-vm-card:hover { box-shadow: 0 12px 40px rgba(0,0,0,.09); transform: translateY(-4px); }
  .abt-vm-card::before {
    content: '';
    position: absolute;
    top: 0; left: 0; right: 0;
    height: 4px;
    background: linear-gradient(90deg, var(--orange), #FF8C42);
    border-radius: 20px 20px 0 0;
  }
  .abt-vm-icon {
    width: 54px; height: 54px;
    background: var(--orange-glow);
    border-radius: 14px;
    display: flex; align-items: center; justify-content: center;
    font-size: 1.5rem;
    margin-bottom: 20px;
    border: 1px solid rgba(232,64,0,.2);
  }
  .abt-vm-card h3 {
    font-family: 'Sora', sans-serif;
    font-size: 1.15rem;
    font-weight: 800;
    color: var(--text);
    margin-bottom: 12px;
  }
  .abt-vm-card p { color: var(--muted); font-size: .9rem; line-height: 1.75; margin: 0; }

  /* ── Mission bullet list ── */
  .abt-mission-list { list-style: none; padding: 0; margin: 0; display: flex; flex-direction: column; gap: 10px; }
  .abt-mission-list li {
    display: flex; align-items: flex-start; gap: 10px;
    color: var(--muted); font-size: .88rem; line-height: 1.6;
  }
  .abt-mission-list li::before {
    content: '✓';
    flex-shrink: 0;
    width: 22px; height: 22px;
    background: var(--orange);
    color: #fff;
    border-radius: 50%;
    font-size: .7rem;
    font-weight: 800;
    display: flex; align-items: center; justify-content: center;
    margin-top: 1px;
  }

  /* ── Services Cards ── */
  .abt-services-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 22px;
    margin-top: 44px;
  }
  .abt-svc-card {
    background: #fff;
    border: 1px solid var(--border);
    border-radius: 18px;
    padding: 30px 28px;
    display: flex;
    gap: 20px;
    align-items: flex-start;
    transition: box-shadow .3s, border-color .3s, transform .3s;
  }
  .abt-svc-card:hover {
    box-shadow: 0 10px 32px rgba(232,64,0,.1);
    border-color: rgba(232,64,0,.35);
    transform: translateY(-3px);
  }
  .abt-svc-icon {
    width: 56px; height: 56px;
    background: linear-gradient(135deg, var(--orange), #FF8C42);
    border-radius: 14px;
    display: flex; align-items: center; justify-content: center;
    font-size: 1.6rem;
    flex-shrink: 0;
    box-shadow: 0 6px 18px rgba(232,64,0,.28);
  }
  .abt-svc-body h4 {
    font-family: 'Sora', sans-serif;
    font-size: .98rem;
    font-weight: 700;
    color: var(--text);
    margin-bottom: 8px;
  }
  .abt-svc-body p { color: var(--muted); font-size: .84rem; line-height: 1.65; margin: 0; }

  /* ── Why Choose ── */
  .abt-why-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 22px;
    margin-top: 44px;
  }
  .abt-why-card {
    background: #fff;
    border: 1px solid var(--border);
    border-radius: 16px;
    padding: 28px 24px;
    text-align: center;
    transition: box-shadow .3s, transform .3s, border-color .3s;
  }
  .abt-why-card:hover {
    box-shadow: 0 10px 30px rgba(0,0,0,.08);
    transform: translateY(-4px);
    border-color: rgba(232,64,0,.25);
  }
  .abt-why-emoji {
    font-size: 2.4rem;
    margin-bottom: 14px;
    display: block;
  }
  .abt-why-card h4 {
    font-family: 'Sora', sans-serif;
    font-weight: 700;
    font-size: .92rem;
    color: var(--text);
    margin-bottom: 8px;
  }
  .abt-why-card p { color: var(--muted); font-size: .82rem; line-height: 1.6; margin: 0; }

  /* ── Commitment Banner ── */
  .abt-commitment {
    background: linear-gradient(135deg, #f8fafc 0%, #ffffff 60%, #f0f4ff 100%);
    padding: 70px 20px;
    text-align: center;
    position: relative;
    overflow: hidden;
    border-top: 1px solid var(--border);
    border-bottom: 1px solid var(--border);
  }
  .abt-commitment::before {
    content: '';
    position: absolute;
    inset: 0;
    background: radial-gradient(ellipse 80% 50% at 50% 0%, rgba(232,64,0,.07) 0%, transparent 70%);
  }
  .abt-commitment-inner { position: relative; z-index: 1; max-width: 700px; margin: auto; }
  .abt-commitment h2 {
    font-family: 'Sora', sans-serif;
    font-size: clamp(1.5rem, 3vw, 2.1rem);
    font-weight: 800;
    color: var(--text);
    margin-bottom: 16px;
  }
  .abt-commitment h2 span { color: var(--orange); }
  .abt-commitment p { color: var(--muted); font-size: .95rem; line-height: 1.8; margin-bottom: 36px; }
  .abt-pillars {
    display: flex;
    flex-wrap: wrap;
    gap: 14px;
    justify-content: center;
    margin-bottom: 40px;
  }
  .abt-pillar {
    background: var(--orange-glow);
    border: 1px solid rgba(232,64,0,.25);
    color: var(--orange);
    font-size: .82rem;
    font-weight: 700;
    letter-spacing: .5px;
    padding: 9px 22px;
    border-radius: 50px;
    text-transform: uppercase;
  }
  .abt-tagline {
    font-family: 'Sora', sans-serif;
    font-size: 1rem;
    font-style: italic;
    color: var(--muted);
    border-top: 1px solid var(--border);
    padding-top: 24px;
  }

  /* ── Who We Are – Info Row ── */
  .abt-info-row {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 20px;
    margin-top: 36px;
  }
  .abt-info-card {
    display: flex;
    align-items: flex-start;
    gap: 16px;
    background: #f9fafb;
    border: 1px solid var(--border);
    border-radius: 14px;
    padding: 20px 20px;
    transition: border-color .25s, box-shadow .25s, transform .25s;
  }
  .abt-info-card:hover {
    border-color: rgba(232,64,0,.3);
    box-shadow: 0 6px 20px rgba(232,64,0,.08);
    transform: translateY(-3px);
  }
  .abt-info-icon {
    width: 44px; height: 44px;
    background: linear-gradient(135deg, var(--orange), #FF8C42);
    border-radius: 12px;
    display: flex; align-items: center; justify-content: center;
    font-size: 1.3rem;
    flex-shrink: 0;
    box-shadow: 0 4px 12px rgba(232,64,0,.25);
  }
  .abt-info-card strong {
    display: block;
    font-family: 'Sora', sans-serif;
    font-size: .9rem;
    font-weight: 700;
    color: var(--text);
    margin-bottom: 5px;
  }
  .abt-info-card p {
    color: var(--muted);
    font-size: .82rem;
    line-height: 1.55;
    margin: 0;
  }

  /* ── Responsive ── */
  @media (max-width: 768px) {
    .abt-stats-grid { grid-template-columns: repeat(2, 1fr); }
    .abt-vm-grid { grid-template-columns: 1fr; }
    .abt-services-grid { grid-template-columns: 1fr; }
    .abt-why-grid { grid-template-columns: repeat(2, 1fr); }
    .abt-svc-card { flex-direction: column; gap: 14px; }
    .abt-info-row { grid-template-columns: 1fr; }
  }
  @media (max-width: 480px) {
    .abt-why-grid { grid-template-columns: 1fr; }
    .abt-stats-grid { grid-template-columns: 1fr 1fr; }
    .abt-hero { padding: 60px 16px 50px; }
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


<!-- ─── HERO BANNER ─── -->
<section class="abt-hero">
  <div class="abt-hero-inner">
    <div class="abt-hero-badge">&#9733; About Us</div>
    <h1>About <span>ePay Digital India</span><br>Pvt. Ltd.</h1>
    <p>A technology-driven company building a smart digital ecosystem that combines E-commerce, Utility Services, Digital Solutions, and Business Growth Opportunities on a single platform.</p>
  </div>
</section>


<!-- ─── STATS BAND ─── -->
<div class="abt-stats">
  <div class="abt-stats-grid">
    <div class="abt-stat-item">
      <div class="abt-stat-num">1M+</div>
      <div class="abt-stat-label">Happy Users</div>
    </div>
    <div class="abt-stat-item">
      <div class="abt-stat-num">50K+</div>
      <div class="abt-stat-label">Merchants</div>
    </div>
    <div class="abt-stat-item">
      <div class="abt-stat-num">28+</div>
      <div class="abt-stat-label">States Covered</div>
    </div>
    <div class="abt-stat-item">
      <div class="abt-stat-num">99.9%</div>
      <div class="abt-stat-label">Uptime</div>
    </div>
  </div>
</div>


<!-- ─── WHO WE ARE ─── -->
<section class="abt-section">
  <div class="abt-wrap">
    <div class="abt-section-tag">Who We Are</div>
    <h2 class="abt-section-title">Empowering Digital Growth <span>Across India</span></h2>
    <p class="abt-section-sub">Founded with the vision of empowering individuals through digital innovation, ePay Digital India Pvt. Ltd. simplifies everyday services while creating opportunities for growth in the digital economy. Our platform provides convenient access to essential digital services, online shopping solutions, and value-added benefits through a user-friendly and scalable system.</p>
    <div class="abt-info-row">
      <div class="abt-info-card">
        <span class="abt-info-icon">&#9889;</span>
        <div>
          <strong>Innovation First</strong>
          <p>Cutting-edge technology at the core of every solution we build.</p>
        </div>
      </div>
      <div class="abt-info-card">
        <span class="abt-info-icon">&#127968;</span>
        <div>
          <strong>Pan-India Reach</strong>
          <p>Serving users and merchants across 28+ states nationwide.</p>
        </div>
        
      </div>
      <div class="abt-info-card">
        <span class="abt-info-icon">&#128274;</span>
        <div>
          <strong>Secure &amp; Reliable</strong>
          <p>Enterprise-grade security with 99.9% platform uptime guarantee.</p>
        </div>
      </div>
    </div>
  </div>
</section>


<!-- ─── VISION & MISSION ─── -->
<section class="abt-section bg-light">
  <div class="abt-wrap">
    <div class="abt-section-tag">Our Purpose</div>
    <h2 class="abt-section-title">Vision &amp; <span>Mission</span></h2>
    <div class="abt-vm-grid">

      <!-- Vision -->
      <div class="abt-vm-card">
        <div class="abt-vm-icon">&#127919;</div>
        <h3>Our Vision</h3>
        <p>To become a trusted and innovative digital platform that empowers people across India through technology, convenience, and growth opportunities — making digital services accessible to every corner of the country.</p>
      </div>

      <!-- Mission -->
      <div class="abt-vm-card">
        <div class="abt-vm-icon">&#128640;</div>
        <h3>Our Mission</h3>
        <ul class="abt-mission-list">
          <li>To deliver reliable and accessible digital services</li>
          <li>To create a seamless user experience through technology</li>
          <li>To support digital empowerment and business growth</li>
          <li>To build a strong nationwide network through innovation and service excellence</li>
        </ul>
      </div>

    </div>
  </div>
</section>


<!-- ─── OUR SERVICES ─── -->
<section class="abt-section">
  <div class="abt-wrap">
    <div class="abt-section-tag">What We Offer</div>
    <h2 class="abt-section-title">Our <span>Services</span></h2>
    <div class="abt-services-grid">

      <div class="abt-svc-card">
        <div class="abt-svc-icon">&#128717;</div>
        <div class="abt-svc-body">
          <h4>E-commerce Solutions</h4>
          <p>A growing online shopping ecosystem with access to a wide range of products and seamless delivery support across India.</p>
        </div>
      </div>

      <div class="abt-svc-card">
        <div class="abt-svc-icon">&#128242;</div>
        <div class="abt-svc-body">
          <h4>ePay Digital Services</h4>
          <p>Digital utility solutions including recharge, bill payments, and other essential services designed for daily convenience with cashback on every transaction.</p>
        </div>
      </div>

      <div class="abt-svc-card">
        <div class="abt-svc-icon">&#127873;</div>
        <div class="abt-svc-body">
          <h4>Gift Voucher &amp; Reward Solutions</h4>
          <p>Smart savings and promotional benefits through digital vouchers and reward systems that add real value to every interaction.</p>
        </div>
      </div>

      <div class="abt-svc-card">
        <div class="abt-svc-icon">&#127760;</div>
        <div class="abt-svc-body">
          <h4>Digital Business Platform</h4>
          <p>A scalable platform designed to support digital business expansion and user engagement across India with robust tools for growth.</p>
        </div>
      </div>

    </div>
  </div>
</section>


<!-- ─── WHY CHOOSE US ─── -->
<section class="abt-section bg-light">
  <div class="abt-wrap">
    <div class="abt-section-tag">Why ePay?</div>
    <h2 class="abt-section-title">Why <span>Choose Us</span></h2>
    <div class="abt-why-grid">

      <div class="abt-why-card">
        <span class="abt-why-emoji">&#128187;</span>
        <h4>Technology-Driven Platform</h4>
        <p>Built on modern, secure, and scalable infrastructure to handle millions of transactions reliably.</p>
      </div>

      <div class="abt-why-card">
        <span class="abt-why-emoji">&#128100;</span>
        <h4>User-Friendly Experience</h4>
        <p>Intuitive interfaces designed for all users, ensuring a smooth and enjoyable digital journey every time.</p>
      </div>

      <div class="abt-why-card">
        <span class="abt-why-emoji">&#128101;</span>
        <h4>Multiple Services, One Ecosystem</h4>
        <p>Everything from shopping to utility payments and business tools — all available under a single platform.</p>
      </div>

      <div class="abt-why-card">
        <span class="abt-why-emoji">&#128200;</span>
        <h4>Growth-Oriented Approach</h4>
        <p>Programs and tools designed to help users, merchants, and partners grow their digital presence and income.</p>
      </div>

      <div class="abt-why-card">
        <span class="abt-why-emoji">&#128587;</span>
        <h4>Dedicated Support</h4>
        <p>Our team is always available to assist users and resolve queries quickly so you never feel left behind.</p>
      </div>

      <div class="abt-why-card">
        <span class="abt-why-emoji">&#128161;</span>
        <h4>Continuous Innovation</h4>
        <p>We constantly evolve our platform with new features and improvements to stay ahead of digital trends.</p>
      </div>

    </div>
  </div>
</section>


<!-- ─── COMMITMENT BANNER ─── -->
<section class="abt-commitment">
  <div class="abt-commitment-inner">
    <h2>Our <span>Commitment</span> to You</h2>
    <p>At ePay Digital India Pvt. Ltd., we are committed to delivering value through every service we offer. We continuously work towards building reliable digital solutions that support individuals, businesses, and communities in the evolving digital world.</p>
    <div class="abt-pillars">
      <span class="abt-pillar">&#10003; Service</span>
      <span class="abt-pillar">&#128161; Innovation</span>
      <span class="abt-pillar">&#128200; Growth</span>
      <span class="abt-pillar">&#128274; Trust</span>
    </div>
    <div class="abt-tagline">"Empowering Digital Growth Across India." — ePay Digital India Pvt. Ltd.</div>
  </div>
</section>


<!-- ─── FOOTER ─── -->
<!-- #include file="inc_footer.asp" -->


<script>
  // Mobile nav toggle
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

  // Intersection Observer — entrance animations
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(e => {
      if (e.isIntersecting) {
        e.target.style.opacity = '1';
        e.target.style.transform = 'translateY(0)';
      }
    });
  }, { threshold: 0.1 });

  document.querySelectorAll('.abt-vm-card, .abt-svc-card, .abt-why-card, .abt-stat-item').forEach((el, i) => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(22px)';
    el.style.transition = `opacity .5s ease ${i * 0.07}s, transform .5s ease ${i * 0.07}s`;
    observer.observe(el);
  });
</script>

</body>
</html>
