<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="icon" type="image/x-icon" href="images/favicon.png">
<title>Contact Us – ePay Digital India Pvt. Ltd.</title>
<link href="https://fonts.googleapis.com/css2?family=Sora:wght@300;400;500;600;700;800&family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet">
<link rel="stylesheet" href="css/custom_stylesheet.css">
<link rel="stylesheet" href="css/style.css">
<style>
  /* ── Contact Page Styles ── */
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
  .cnt-hero {
    background: linear-gradient(135deg, #f8fafc 0%, #ffffff 60%, #f0f4ff 100%);
    padding: 80px 20px 70px;
    text-align: center;
    position: relative;
    overflow: hidden;
    border-bottom: 1px solid var(--border);
  }
  .cnt-hero::before {
    content: '';
    position: absolute;
    inset: 0;
    background: radial-gradient(ellipse 70% 60% at 50% 0%, rgba(232,64,0,0.07) 0%, transparent 70%);
  }
  .cnt-hero-inner { position: relative; z-index: 1; max-width: 760px; margin: auto; }
  .cnt-hero-badge {
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
  .cnt-hero h1 {
    font-family: 'Sora', sans-serif;
    font-size: clamp(1.9rem, 4.5vw, 3rem);
    font-weight: 800;
    color: var(--text);
    line-height: 1.25;
    margin-bottom: 18px;
  }
  .cnt-hero h1 span { color: var(--orange); }
  .cnt-hero p {
    color: var(--muted);
    font-size: 1.05rem;
    line-height: 1.8;
    max-width: 620px;
    margin: 0 auto;
  }

  /* ── Section wrapper ── */
  .cnt-section { padding: 70px 20px; }
  .cnt-section.bg-light { background: #f9fafb; }
  .cnt-wrap { max-width: 1100px; margin: auto; }
  .cnt-section-tag {
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
  .cnt-section-title {
    font-family: 'Sora', sans-serif;
    font-size: clamp(1.4rem, 2.8vw, 2rem);
    font-weight: 800;
    color: var(--text);
    line-height: 1.3;
    margin-bottom: 14px;
  }
  .cnt-section-title span { color: var(--orange); }
  .cnt-section-sub { color: var(--muted); font-size: .95rem; line-height: 1.8; }

  /* ── Info + Map Split ── */
  .cnt-split {
    display: grid;
    grid-template-columns: .9fr 1.1fr;
    gap: 36px;
    margin-top: 44px;
    align-items: stretch;
  }

  /* ── Info Side Panel ── */
  .cnt-info-panel {
    background: linear-gradient(135deg, #1A1A2E 0%, #0D1117 100%);
    border-radius: 20px;
    padding: 40px 34px;
    color: #fff;
  }
  .cnt-info-panel h3 {
    font-family: 'Sora', sans-serif;
    font-size: 1.2rem;
    font-weight: 800;
    margin-bottom: 8px;
  }
  .cnt-info-panel > p { color: #B8BCC8; font-size: .85rem; line-height: 1.7; margin-bottom: 28px; }
  .cnt-info-list { display: flex; flex-direction: column; gap: 22px; }
  .cnt-info-row { display: flex; gap: 16px; align-items: flex-start; }
  .cnt-info-icon {
    width: 42px; height: 42px;
    flex-shrink: 0;
    background: rgba(232,64,0,.18);
    border: 1px solid rgba(232,64,0,.35);
    border-radius: 12px;
    display: flex; align-items: center; justify-content: center;
    font-size: 1.2rem;
  }
  .cnt-info-row strong { display: block; font-size: .88rem; margin-bottom: 4px; color: #fff; }
  .cnt-info-row span, .cnt-info-row a {
    color: #B8BCC8;
    font-size: .85rem;
    line-height: 1.6;
    text-decoration: none;
  }
  .cnt-info-row a:hover { color: #FF8C42; }
  .cnt-info-divider { border-top: 1px solid rgba(255,255,255,.1); margin: 8px 0; }

  /* ── Enquiry Form ── */
  .cnt-form-wrap {
    background: #fff;
    border-radius: 20px;
    border: 1px solid var(--border);
    box-shadow: 0 10px 30px rgba(0,0,0,.06);
    padding: 40px 34px;
  }
  .cnt-form-wrap h3 {
    font-family: 'Sora', sans-serif;
    font-size: 1.2rem;
    font-weight: 800;
    color: var(--text);
    margin-bottom: 8px;
  }
  .cnt-form-wrap > p { color: var(--muted); font-size: .85rem; line-height: 1.7; margin-bottom: 26px; }
  .cnt-form-grid {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 18px 16px;
  }
  .cnt-form-group { display: flex; flex-direction: column; gap: 6px; }
  .cnt-form-group.full { grid-column: 1 / -1; }
  .cnt-form-group label {
    font-size: .82rem;
    font-weight: 600;
    color: var(--text);
  }
  .cnt-form-group input,
  .cnt-form-group textarea {
    font-family: 'Nunito', sans-serif;
    font-size: .92rem;
    color: var(--text);
    background: #f9fafb;
    border: 1px solid var(--border);
    border-radius: 10px;
    padding: 12px 14px;
    outline: none;
    transition: border-color .2s ease, box-shadow .2s ease;
  }
  .cnt-form-group textarea { resize: vertical; min-height: 110px; }
  .cnt-form-group input:focus,
  .cnt-form-group textarea:focus {
    border-color: var(--orange);
    box-shadow: 0 0 0 3px var(--orange-glow);
    background: #fff;
  }
  .cnt-form-submit {
    margin-top: 6px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    background: var(--orange);
    color: #fff;
    font-family: 'Sora', sans-serif;
    font-size: .92rem;
    font-weight: 700;
    border: none;
    border-radius: 10px;
    padding: 13px 28px;
    cursor: pointer;
    transition: background .2s ease, transform .2s ease;
  }
  .cnt-form-submit:hover { background: var(--orange-light); transform: translateY(-1px); }
  .cnt-form-msg {
    margin-top: 14px;
    font-size: .85rem;
    font-weight: 600;
    display: none;
  }
  .cnt-form-msg.show { display: block; }
  .cnt-form-msg.success { color: #18794e; }
  .cnt-form-msg.error { color: #c0392b; }

  /* ── Responsive ── */
  @media (max-width: 900px) {
    .cnt-split { grid-template-columns: 1fr; }
  }
  @media (max-width: 600px) {
    .cnt-info-panel { padding: 28px 22px; }
    .cnt-form-wrap { padding: 28px 22px; }
    .cnt-form-grid { grid-template-columns: 1fr; }
    .cnt-hero { padding: 60px 16px 50px; }
  }
</style>
</head>
<body>

<!-- #include file="inc_header.asp" -->

<!-- ─── HERO BANNER ─── -->
<section class="cnt-hero">
  <div class="cnt-hero-inner">
    <div class="cnt-hero-badge">&#9742; Contact Us</div>
    <h1>Get in Touch with <span>ePay Digital India</span></h1>
    <p>Have a question, need support, or just want to say hello? Our team is here to help you every step of the way.</p>
  </div>
</section>


<!-- ─── CONTACT INFO + MAP ─── -->
<section class="cnt-section">
  <div class="cnt-wrap">
    <div class="cnt-section-tag">Reach Us</div>
    <h2 class="cnt-section-title">Get in <span>Touch</span></h2>
    <p class="cnt-section-sub">Have a question, need support, or just want to say hello? Our team is available Monday to Saturday, 10:00 AM – 6:00 PM.</p>

    <div class="cnt-split">

      <!-- Info Panel -->
      <div class="cnt-info-panel">
        <h3>Contact Information</h3>
        <p>Reach out to us directly using the details below. Our support team is happy to assist you.</p>

        <div class="cnt-info-list">
          <div class="cnt-info-row">
            <span class="cnt-info-icon">&#127970;</span>
            <div>
              <strong>Corporate Address</strong>
              <span>3rd Floor, Amar Heights, Sawkar Colony, Ishwarpur, Sangli, Maharashtra – 415409</span>
            </div>
          </div>

          <div class="cnt-info-divider"></div>

          <div class="cnt-info-row">
            <span class="cnt-info-icon">&#128340;</span>
            <div>
              <strong>Working Hours</strong>
              <span>Monday to Saturday | 10:00 AM – 6:00 PM</span>
            </div>
          </div>

          <div class="cnt-info-divider"></div>

          <div class="cnt-info-row">
            <span class="cnt-info-icon">&#128722;</span>
            <div>
              <strong>eCommerce Support</strong>
              <a href="tel:+919684029323">+91 96840 29323</a>
            </div>
          </div>

          <div class="cnt-info-divider"></div>

          <div class="cnt-info-row">
            <span class="cnt-info-icon">&#128295;</span>
            <div>
              <strong>Technical Support</strong>
              <a href="tel:+919684029313">+91 96840 29313</a>
            </div>
          </div>

          <div class="cnt-info-divider"></div>

          <div class="cnt-info-row">
            <span class="cnt-info-icon">&#9993;</span>
            <div>
              <strong>Email</strong>
              <a href="mailto:support.epayindia@gmail.com">support.epayindia@gmail.com</a>
            </div>
          </div>

          <div class="cnt-info-divider"></div>

          <div class="cnt-info-row">
            <span class="cnt-info-icon">&#127760;</span>
            <div>
              <strong>Website</strong>
              <a href="https://www.epayindia.in" target="_blank">www.epayindia.in</a>
            </div>
          </div>
        </div>
      </div>

      <!-- Enquiry Form -->
      <div class="cnt-form-wrap">
        <h3>Send an Enquiry</h3>
        <p>Fill out the form below and our team will get back to you shortly.</p>

        <form id="enquiryForm" action="contact-us-submit.asp" method="post" novalidate>
          <div class="cnt-form-grid">
            <div class="cnt-form-group">
              <label for="firstName">First Name</label>
              <input type="text" id="firstName" name="firstName" placeholder="Enter your first name" required>
            </div>

            <div class="cnt-form-group">
              <label for="lastName">Last Name</label>
              <input type="text" id="lastName" name="lastName" placeholder="Enter your last name" required>
            </div>

            <div class="cnt-form-group">
              <label for="email">Email</label>
              <input type="email" id="email" name="email" placeholder="Enter your email" required>
            </div>

            <div class="cnt-form-group">
              <label for="phone">Phone</label>
              <input type="tel" id="phone" name="phone" placeholder="Enter your phone number" pattern="[0-9+\-\s]{7,15}" required>
            </div>

            <div class="cnt-form-group full">
              <label for="address">Address</label>
              <input type="text" id="address" name="address" placeholder="Enter your address">
            </div>

            <div class="cnt-form-group full">
              <label for="message">Message</label>
              <textarea id="message" name="message" placeholder="Type your message here" required></textarea>
            </div>
          </div>

          <button type="submit" class="cnt-form-submit">Submit Enquiry</button>
          <div class="cnt-form-msg" id="enquiryMsg"></div>
        </form>
      </div>

    </div>
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

  document.querySelectorAll('.cnt-info-panel, .cnt-form-wrap').forEach((el, i) => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(22px)';
    el.style.transition = `opacity .5s ease ${i * 0.07}s, transform .5s ease ${i * 0.07}s`;
    observer.observe(el);
  });

  // Enquiry form validation + submit feedback
  const enquiryForm = document.getElementById('enquiryForm');
  const enquiryMsg   = document.getElementById('enquiryMsg');

  if (enquiryForm) {
    enquiryForm.addEventListener('submit', function (e) {
      if (!enquiryForm.checkValidity()) {
        e.preventDefault();
        enquiryForm.reportValidity();
        return;
      }
      enquiryMsg.textContent = 'Submitting your enquiry...';
      enquiryMsg.className = 'cnt-form-msg show success';
    });
  }
</script>

</body>
</html>
