<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Certificate.aspx.cs" Inherits="Certificate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style>
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

        .pg-hero-inner {
            position: relative;
            z-index: 1;
            max-width: 680px;
            margin: auto;
        }

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

            .pg-hero h1 span {
                color: var(--orange);
            }

        .pg-hero p {
            color: var(--muted);
            font-size: .97rem;
            line-height: 1.75;
        }

        /* ── Breadcrumb ── */
        .pg-breadcrumb {
            background: #f9fafb;
            border-bottom: 1px solid var(--border);
            padding: 12px 30px;
            font-size: .82rem;
            color: var(--muted);
        }

            .pg-breadcrumb a {
                color: var(--orange);
                text-decoration: none;
                font-weight: 600;
            }

                .pg-breadcrumb a:hover {
                    text-decoration: underline;
                }

            .pg-breadcrumb span {
                margin: 0 6px;
            }

        /* ── Certificate Section ── */
        .cert-section {
            padding: 64px 20px 80px;
            background: #f9fafb;
        }

        .cert-wrap {
            max-width: 1100px;
            margin: auto;
        }

        .cert-section-label {
            display: flex;
            align-items: center;
            gap: 14px;
            margin-bottom: 48px;
        }

            .cert-section-label .line {
                flex: 1;
                height: 1px;
                background: linear-gradient(90deg, var(--border), transparent);
            }

                .cert-section-label .line.left {
                    background: linear-gradient(270deg, var(--border), transparent);
                }

        .cert-section-label-text {
            font-family: 'Sora', sans-serif;
            font-size: .8rem;
            font-weight: 700;
            letter-spacing: 1.2px;
            text-transform: uppercase;
            color: var(--orange);
            white-space: nowrap;
        }

        /* ── Grid ── */
        .cert-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 26px;
        }

        /* ── Certificate Card ── */
        .cert-card {
            background: #fff;
            border: 1px solid var(--border);
            border-radius: 20px;
            overflow: hidden;
            cursor: zoom-in;
            position: relative;
            transition: box-shadow .3s, transform .3s, border-color .3s;
            box-shadow: 0 2px 12px rgba(0,0,0,.05);
        }

            .cert-card:hover {
                box-shadow: 0 16px 44px rgba(0,0,0,.12);
                transform: translateY(-5px);
                border-color: rgba(232,64,0,.3);
            }

            .cert-card::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                height: 3px;
                background: linear-gradient(90deg, var(--orange), #FF8C42);
                z-index: 1;
            }

        /* Thumbnail */
        .cert-thumb {
            width: 100%;
            aspect-ratio: 3 / 4;
            overflow: hidden;
            background: #f0f2f5;
            position: relative;
        }

            .cert-thumb img {
                width: 100%;
                height: 100%;
                object-fit: cover;
                object-position: top center;
                display: block;
                transition: transform .45s ease;
            }

        .cert-card:hover .cert-thumb img {
            transform: scale(1.04);
        }

        /* Zoom overlay on hover */
        .cert-zoom-hint {
            position: absolute;
            inset: 0;
            background: rgba(13,17,23,.45);
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            gap: 8px;
            opacity: 0;
            transition: opacity .3s;
        }

        .cert-card:hover .cert-zoom-hint {
            opacity: 1;
        }

        .cert-zoom-hint .zoom-icon {
            width: 52px;
            height: 52px;
            background: var(--orange);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.4rem;
            box-shadow: 0 6px 20px rgba(232,64,0,.5);
        }

        .cert-zoom-hint span {
            color: #fff;
            font-size: .78rem;
            font-weight: 700;
            letter-spacing: .6px;
            text-transform: uppercase;
        }

        /* Card footer */
        .cert-info {
            padding: 16px 18px 18px;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .cert-info-left {
        }

        .cert-num {
            font-family: 'Sora', sans-serif;
            font-size: .72rem;
            font-weight: 700;
            color: var(--orange);
            letter-spacing: .5px;
            text-transform: uppercase;
            margin-bottom: 2px;
        }

        .cert-title {
            font-family: 'Sora', sans-serif;
            font-size: .9rem;
            font-weight: 700;
            color: var(--text);
        }

        .cert-view-btn {
            width: 34px;
            height: 34px;
            background: var(--orange-glow);
            border: 1px solid rgba(232,64,0,.2);
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: var(--orange);
            font-size: .85rem;
            flex-shrink: 0;
            transition: background .2s, border-color .2s;
        }

        .cert-card:hover .cert-view-btn {
            background: var(--orange);
            color: #fff;
            border-color: var(--orange);
        }

        /* ── LIGHTBOX ── */
        .lb-overlay {
            position: fixed;
            inset: 0;
            background: rgba(5,7,12,.92);
            z-index: 9000;
            display: flex;
            align-items: center;
            justify-content: center;
            opacity: 0;
            pointer-events: none;
            transition: opacity .3s;
            backdrop-filter: blur(6px);
            -webkit-backdrop-filter: blur(6px);
        }

            .lb-overlay.lb-open {
                opacity: 1;
                pointer-events: all;
            }

        .lb-container {
            position: relative;
            max-width: 88vw;
            max-height: 90vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .lb-img-wrap {
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 30px 80px rgba(0,0,0,.7);
            transform: scale(0.92);
            transition: transform .35s cubic-bezier(.34,1.56,.64,1);
        }

        .lb-overlay.lb-open .lb-img-wrap {
            transform: scale(1);
        }

        .lb-img-wrap img {
            display: block;
            max-width: 80vw;
            max-height: 82vh;
            width: auto;
            height: auto;
            object-fit: contain;
        }

        /* Close */
        .lb-close {
            position: fixed;
            top: 18px;
            right: 22px;
            width: 44px;
            height: 44px;
            background: rgba(255,255,255,.1);
            border: 1px solid rgba(255,255,255,.18);
            border-radius: 50%;
            color: #fff;
            font-size: 1.3rem;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            transition: background .2s, transform .2s;
            z-index: 9001;
            line-height: 1;
        }

            .lb-close:hover {
                background: var(--orange);
                transform: rotate(90deg);
                border-color: var(--orange);
            }

        /* Nav arrows */
        .lb-nav {
            position: fixed;
            top: 50%;
            transform: translateY(-50%);
            width: 48px;
            height: 48px;
            background: rgba(255,255,255,.1);
            border: 1px solid rgba(255,255,255,.18);
            border-radius: 50%;
            color: #fff;
            font-size: 1.2rem;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            transition: background .2s, transform .2s;
            z-index: 9001;
            user-select: none;
        }

            .lb-nav:hover {
                background: var(--orange);
                border-color: var(--orange);
            }

        .lb-prev {
            left: 16px;
        }

        .lb-next {
            right: 16px;
        }

        .lb-prev:hover {
            transform: translateY(-50%) scale(1.1);
        }

        .lb-next:hover {
            transform: translateY(-50%) scale(1.1);
        }

        /* Counter + label */
        .lb-footer {
            position: fixed;
            bottom: 24px;
            left: 50%;
            transform: translateX(-50%);
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 6px;
            z-index: 9001;
        }

        .lb-counter {
            background: rgba(255,255,255,.1);
            border: 1px solid rgba(255,255,255,.15);
            color: #fff;
            font-size: .78rem;
            font-weight: 700;
            letter-spacing: .6px;
            padding: 5px 16px;
            border-radius: 50px;
            font-family: 'Sora', sans-serif;
        }

        .lb-dots {
            display: flex;
            gap: 6px;
        }

        .lb-dot {
            width: 7px;
            height: 7px;
            border-radius: 50%;
            background: rgba(255,255,255,.3);
            cursor: pointer;
            transition: background .2s, transform .2s;
        }

            .lb-dot.active {
                background: var(--orange);
                transform: scale(1.3);
            }

        /* ── Responsive ── */
        @media (max-width: 900px) {
            .cert-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 520px) {
            .cert-grid {
                grid-template-columns: repeat(2, 1fr);
                gap: 14px;
            }

            .cert-section {
                padding: 40px 14px 60px;
            }

            .pg-hero {
                padding: 50px 16px 44px;
            }

            .lb-nav {
                width: 38px;
                height: 38px;
                font-size: 1rem;
            }

            .lb-prev {
                left: 8px;
            }

            .lb-next {
                right: 8px;
            }

            .lb-img-wrap img {
                max-width: 92vw;
                max-height: 75vh;
            }
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- ─── HERO ─── -->
    <section class="pg-hero">
        <div class="pg-hero-inner">
            <div class="pg-hero-badge">&#127942; Trust &amp; Compliance</div>
            <h1>Our <span>Certificates</span></h1>
            <p>ePay Digital India Pvt. Ltd. is backed by verified certifications that demonstrate our commitment to legal compliance, security standards, and operational excellence.</p>
        </div>
    </section>

    <!-- ─── BREADCRUMB ─── -->
    <div class="pg-breadcrumb">
        <a href="index.aspx">Home</a><span>&#8250;</span>
        <a href="aboutus.aspx">About Us</a><span>&#8250;</span>
        Certificates
    </div>

    <!-- ─── CERTIFICATES GRID ─── -->
    <section class="cert-section">
        <div class="cert-wrap">

            <div class="cert-section-label">
                <div class="line left"></div>
                <div class="cert-section-label-text">Verified &amp; Certified</div>
                <div class="line"></div>
            </div>

            <div class="cert-grid" id="certGrid">

                <div class="cert-card" onclick="openLightbox(0)">
                    <div class="cert-thumb">
                        <img src="demoepay/images/certificate/certificate_1.jpg" alt="Certificate 1" loading="lazy">
                        <div class="cert-zoom-hint">
                            <div class="zoom-icon">&#128269;</div>
                            <span>Click to Zoom</span>
                        </div>
                    </div>
                    <div class="cert-info">
                        <div class="cert-info-left">
                            <div class="cert-num">Certificate #01</div>
                            <div class="cert-title">Official Certificate</div>
                        </div>
                        <div class="cert-view-btn">&#8599;</div>
                    </div>
                </div>

                <div class="cert-card" onclick="openLightbox(1)">
                    <div class="cert-thumb">
                        <img src="demoepay/images/certificate/certificate_2.jpg" alt="Certificate 2" loading="lazy">
                        <div class="cert-zoom-hint">
                            <div class="zoom-icon">&#128269;</div>
                            <span>Click to Zoom</span>
                        </div>
                    </div>
                    <div class="cert-info">
                        <div class="cert-info-left">
                            <div class="cert-num">Certificate #02</div>
                            <div class="cert-title">Official Certificate</div>
                        </div>
                        <div class="cert-view-btn">&#8599;</div>
                    </div>
                </div>

                <div class="cert-card" onclick="openLightbox(2)">
                    <div class="cert-thumb">
                        <img src="demoepay/images/certificate/certificate_3.jpg" alt="Certificate 3" loading="lazy">
                        <div class="cert-zoom-hint">
                            <div class="zoom-icon">&#128269;</div>
                            <span>Click to Zoom</span>
                        </div>
                    </div>
                    <div class="cert-info">
                        <div class="cert-info-left">
                            <div class="cert-num">Certificate #03</div>
                            <div class="cert-title">Official Certificate</div>
                        </div>
                        <div class="cert-view-btn">&#8599;</div>
                    </div>
                </div>

                <div class="cert-card" onclick="openLightbox(3)">
                    <div class="cert-thumb">
                        <img src="demoepay/images/certificate/certificate_4.jpg" alt="Certificate 4" loading="lazy">
                        <div class="cert-zoom-hint">
                            <div class="zoom-icon">&#128269;</div>
                            <span>Click to Zoom</span>
                        </div>
                    </div>
                    <div class="cert-info">
                        <div class="cert-info-left">
                            <div class="cert-num">Certificate #04</div>
                            <div class="cert-title">Official Certificate</div>
                        </div>
                        <div class="cert-view-btn">&#8599;</div>
                    </div>
                </div>

                <div class="cert-card" onclick="openLightbox(4)">
                    <div class="cert-thumb">
                        <img src="demoepay/images/certificate/certificate_5.jpg" alt="Certificate 5" loading="lazy">
                        <div class="cert-zoom-hint">
                            <div class="zoom-icon">&#128269;</div>
                            <span>Click to Zoom</span>
                        </div>
                    </div>
                    <div class="cert-info">
                        <div class="cert-info-left">
                            <div class="cert-num">Certificate #05</div>
                            <div class="cert-title">Official Certificate</div>
                        </div>
                        <div class="cert-view-btn">&#8599;</div>
                    </div>
                </div>

            </div>
        </div>
    </section>

    <!-- ─── LIGHTBOX ─── -->
    <div class="lb-overlay" id="lbOverlay" role="dialog" aria-modal="true" aria-label="Certificate Viewer">

        <!-- Close -->
        <button type="button"  class="lb-close" id="lbClose" title="Close (Esc)" aria-label="Close">&#10005;</button>

        <!-- Image -->
        <div class="lb-container">
            <div class="lb-img-wrap">
                <img id="lbImage" src="" alt="Certificate">
            </div>
        </div>

        <!-- Prev / Next -->
        <button type="button" class="lb-nav lb-prev" id="lbPrev" title="Previous" aria-label="Previous">&#8249;</button>
        <button type="button"  class="lb-nav lb-next" id="lbNext" title="Next" aria-label="Next">&#8250;</button>

        <!-- Footer: counter + dots -->
        <div class="lb-footer">
            <div class="lb-counter" id="lbCounter">1 / 5</div>
            <div class="lb-dots" id="lbDots"></div>
        </div>

    </div>
    
<script>
  /* ── Lightbox Logic ── */
  var images = [
    'demoepay/images/certificate/certificate_1.jpg',
    'demoepay/images/certificate/certificate_2.jpg',
    'demoepay/images/certificate/certificate_3.jpg',
    'demoepay/images/certificate/certificate_4.jpg',
    'demoepay/images/certificate/certificate_5.jpg'
  ];
  var total   = images.length;
  var current = 0;

  var overlay  = document.getElementById('lbOverlay');
  var lbImage  = document.getElementById('lbImage');
  var lbCounter = document.getElementById('lbCounter');
  var dotsWrap = document.getElementById('lbDots');

  // Build dots
  for (var d = 0; d < total; d++) {
    var dot = document.createElement('div');
    dot.className = 'lb-dot';
    dot.setAttribute('data-i', d);
    dot.addEventListener('click', (function(i){ return function(){ goTo(i); }; })(d));
    dotsWrap.appendChild(dot);
  }

  function updateUI() {
    lbImage.src = images[current];
    lbImage.alt = 'Certificate ' + (current + 1);
    lbCounter.textContent = (current + 1) + ' / ' + total;
    var dots = dotsWrap.querySelectorAll('.lb-dot');
    dots.forEach(function(d, i) { d.classList.toggle('active', i === current); });
  }

  function goTo(index) {
    current = (index + total) % total;
    lbImage.style.opacity = '0';
    lbImage.style.transform = 'scale(0.96)';
    setTimeout(function() {
      updateUI();
      lbImage.style.opacity = '1';
      lbImage.style.transform = 'scale(1)';
    }, 160);
  }

  function openLightbox(index) {
    current = index;
    lbImage.style.transition = 'none';
    lbImage.style.opacity = '1';
    lbImage.style.transform = 'scale(1)';
    updateUI();
    overlay.classList.add('lb-open');
    document.body.style.overflow = 'hidden';
    setTimeout(function() {
      lbImage.style.transition = 'opacity .16s ease, transform .16s ease';
    }, 50);
  }

  function closeLightbox() {
    overlay.classList.remove('lb-open');
    document.body.style.overflow = '';
    setTimeout(function() { lbImage.src = ''; }, 320);
  }

  document.getElementById('lbClose').addEventListener('click', closeLightbox);
  document.getElementById('lbPrev').addEventListener('click', function() { goTo(current - 1); });
  document.getElementById('lbNext').addEventListener('click', function() { goTo(current + 1); });

  // Click backdrop to close
  overlay.addEventListener('click', function(e) {
    if (e.target === overlay) closeLightbox();
  });

  // Keyboard nav
  document.addEventListener('keydown', function(e) {
    if (!overlay.classList.contains('lb-open')) return;
    if (e.key === 'Escape')      closeLightbox();
    if (e.key === 'ArrowRight')  goTo(current + 1);
    if (e.key === 'ArrowLeft')   goTo(current - 1);
  });

  // Touch swipe support
  var touchStartX = 0;
  overlay.addEventListener('touchstart', function(e) { touchStartX = e.changedTouches[0].clientX; }, { passive: true });
  overlay.addEventListener('touchend', function(e) {
    var diff = touchStartX - e.changedTouches[0].clientX;
    if (Math.abs(diff) > 50) goTo(diff > 0 ? current + 1 : current - 1);
  });

  ///* ── Mobile Nav ── */
  //var hamburgerBtn = document.getElementById('hamburgerBtn');
  //var mobileNav    = document.getElementById('mobileNav');
  //if (hamburgerBtn && mobileNav) {
  //  hamburgerBtn.addEventListener('click', function () {
  //    var isOpen = mobileNav.classList.toggle('open');
  //    hamburgerBtn.classList.toggle('open', isOpen);
  //    hamburgerBtn.setAttribute('aria-expanded', isOpen);
  //  });
  //  mobileNav.querySelectorAll('a').forEach(function (link) {
  //    link.addEventListener('click', function () {
  //      mobileNav.classList.remove('open');
  //      hamburgerBtn.classList.remove('open');
  //      hamburgerBtn.setAttribute('aria-expanded', 'false');
  //    });
  //  });
  //  document.addEventListener('click', function (e) {
  //    if (!hamburgerBtn.contains(e.target) && !mobileNav.contains(e.target)) {
  //      mobileNav.classList.remove('open');
  //      hamburgerBtn.classList.remove('open');
  //      hamburgerBtn.setAttribute('aria-expanded', 'false');
  //    }
  //  });
  //}

  ///* ── Entrance Animations ── */
  //var obsv = new IntersectionObserver(function(entries) {
  //  entries.forEach(function(e) {
  //    if (e.isIntersecting) {
  //      e.target.style.opacity = '1';
  //      e.target.style.transform = 'translateY(0) scale(1)';
  //    }
  //  });
  //}, { threshold: 0.08 });

  //document.querySelectorAll('.cert-card').forEach(function(el, i) {
  //  el.style.opacity = '0';
  //  el.style.transform = 'translateY(24px) scale(0.97)';
  //  el.style.transition = 'opacity .5s ease ' + (i * 0.08) + 's, transform .5s ease ' + (i * 0.08) + 's';
  //  obsv.observe(el);
  //});
</script>
    <script>

        // ─────────────────────────────
        // User Dropdown Toggle
        // ─────────────────────────────
        const userDropdownBtn = document.getElementById('userDropdownBtn');
        const userDropdown = document.getElementById('userDropdown');
        const userDropdownWrap = document.getElementById('userDropdownWrap');

        if (userDropdownBtn && userDropdown && userDropdownWrap) {

            userDropdownBtn.addEventListener('click', function (e) {
                e.stopPropagation();
                userDropdown.classList.toggle('open');
            });

            document.addEventListener('click', function (e) {

                if (!userDropdownWrap.contains(e.target)) {
                    userDropdown.classList.remove('open');
                }

            });
        }


        // ─────────────────────────────
        // Mobile Navigation Toggle
        // ─────────────────────────────
        const hamburgerBtn = document.getElementById('hamburgerBtn');
        const mobileNav = document.getElementById('mobileNav');

        if (hamburgerBtn && mobileNav) {

            // Open / Close Menu
            hamburgerBtn.addEventListener('click', function (e) {

                e.stopPropagation();

                const isOpen = mobileNav.classList.toggle('open');

                hamburgerBtn.classList.toggle('open', isOpen);

                hamburgerBtn.setAttribute('aria-expanded', isOpen);
            });


            // Close Menu On Link Click
            mobileNav.querySelectorAll('a').forEach(function (link) {

                link.addEventListener('click', function () {

                    mobileNav.classList.remove('open');

                    hamburgerBtn.classList.remove('open');

                    hamburgerBtn.setAttribute('aria-expanded', 'false');

                });

            });


            // Close Menu On Outside Click
            document.addEventListener('click', function (e) {

                if (
                    !hamburgerBtn.contains(e.target) &&
                    !mobileNav.contains(e.target)
                ) {

                    mobileNav.classList.remove('open');

                    hamburgerBtn.classList.remove('open');

                    hamburgerBtn.setAttribute('aria-expanded', 'false');
                }

            });


            // ESC Key Support
            document.addEventListener('keydown', function (e) {

                if (e.key === 'Escape') {

                    mobileNav.classList.remove('open');

                    hamburgerBtn.classList.remove('open');

                    hamburgerBtn.setAttribute('aria-expanded', 'false');
                }

            });

        }


        // ─────────────────────────────
        // Category Scroll
        // ─────────────────────────────
        function scrollCats(dir) {

            const el = document.getElementById('catsScroll');

            if (el) {

                el.scrollBy({
                    left: dir * 240,
                    behavior: 'smooth'
                });

            }
        }


        // ─────────────────────────────
        // Scroll Animation
        // ─────────────────────────────
        const observer = new IntersectionObserver((entries, observer) => {

            entries.forEach(entry => {

                if (entry.isIntersecting) {

                    entry.target.classList.add('show');

                    observer.unobserve(entry.target);
                }

            });

        }, {
            threshold: 0.08
        });


        document.querySelectorAll(
            '.service-card, .cat-card, .about-feat, .why-feat, [data-anim]'
        ).forEach((el, i) => {

            el.style.transitionDelay = `${i * 0.08}s`;

            observer.observe(el);

        });

</script>
</asp:Content>

