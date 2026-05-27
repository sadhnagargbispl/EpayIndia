<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="icon" type="image/x-icon" href="demoepay/images/favicon.png">
<title> ePay - India's Smart Digital & Ecommerce Platform</title>
<link href="https://fonts.googleapis.com/css2?family=Sora:wght@300;400;500;600;700;800&family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet">
<link rel="stylesheet" href="css/custom_stylesheet.css">
<link rel="stylesheet" href="css/style.css">
<style>
  /* ════════════════════════════════════════
     HERO SLIDER – LIGHT THEME OVERRIDES
  ════════════════════════════════════════ */

  /* ── Slide 1 – Light Green ── */
  .hero-slide-1 {
    background: linear-gradient(135deg, #e8f5e9 0%, #f1f8e9 45%, #e0f2f1 100%) !important;
    background-image: none !important;
  }
  .hero-slide-1::before {
    background:
      radial-gradient(ellipse 55% 80% at 90% 50%, rgba(16,185,129,.14) 0%, transparent 60%),
      radial-gradient(circle 320px at 10% 80%,   rgba(52,211,153,.10) 0%, transparent 55%) !important;
  }
  /* Decorative blob */
  .hero-slide-1::after {
    content: '';
    position: absolute;
    right: -60px; top: -60px;
    width: 420px; height: 420px;
    background: radial-gradient(circle, rgba(16,185,129,.13) 0%, transparent 65%);
    border-radius: 50%;
    z-index: 0;
    pointer-events: none;
  }
  .hero-slide-1 h1            { color: #1A1A2E !important; }
  .hero-slide-1 h1 .accent    { color: #E84000 !important; }
  .hero-slide-1 p             { color: #4B5563 !important; }
  .hero-slide-1 .hero-badge   {
    background: rgba(16,185,129,.15) !important;
    border: 1px solid rgba(16,185,129,.35) !important;
    color: #065f46 !important;
  }
  .hero-slide-1 .hero-badge-dot { background: #10b981 !important; }
  .hero-slide-1 .btn-outline {
    background: rgba(26,26,46,.07) !important;
    color: #1A1A2E !important;
    border-color: rgba(26,26,46,.18) !important;
    backdrop-filter: none !important;
  }
  .hero-slide-1 .btn-outline:hover { background: rgba(26,26,46,.13) !important; }

  /* ── Slide 2 – Light Orange ── */
  .hero-slide-2 {
    background: linear-gradient(135deg, #fff7ed 0%, #ffedd5 45%, #fff3e0 100%) !important;
  }
  .hero-slide-2::before {
    background:
      radial-gradient(ellipse 55% 80% at 90% 50%, rgba(232,64,0,.13) 0%, transparent 60%),
      radial-gradient(circle 320px at 10% 80%,   rgba(251,146,60,.10) 0%, transparent 55%) !important;
  }
  .hero-slide-2::after {
    content: '';
    position: absolute;
    right: -60px; top: -60px;
    width: 420px; height: 420px;
    background: radial-gradient(circle, rgba(232,64,0,.12) 0%, transparent 65%);
    border-radius: 50%;
    z-index: 0;
    pointer-events: none;
  }
  .hero-slide-2 h1            { color: #1A1A2E !important; }
  .hero-slide-2 h1 .accent    { color: #E84000 !important; }
  .hero-slide-2 p             { color: #4B5563 !important; }
  .hero-slide-2 .hero-badge   {
    background: rgba(232,64,0,.12) !important;
    border: 1px solid rgba(232,64,0,.3) !important;
    color: #9a3412 !important;
  }
  .hero-slide-2 .hero-badge-dot { background: #E84000 !important; }
  .hero-slide-2 .btn-outline {
    background: rgba(232,64,0,.08) !important;
    color: #9a3412 !important;
    border-color: rgba(232,64,0,.28) !important;
    backdrop-filter: none !important;
  }
  .hero-slide-2 .btn-outline:hover { background: rgba(232,64,0,.15) !important; }

  /* ── Slide 3 – White / Pearl ── */
  .hero-slide-3 {
    background: linear-gradient(135deg, #ffffff 0%, #f8fafc 45%, #ffffff 100%) !important;
  }
  .hero-slide-3::before {
    background:
      radial-gradient(ellipse 55% 80% at 90% 50%, rgba(99,102,241,.10) 0%, transparent 60%),
      radial-gradient(circle 320px at 10% 80%,   rgba(139,92,246,.07) 0%, transparent 55%) !important;
  }
  .hero-slide-3::after {
    content: '';
    position: absolute;
    right: -60px; top: -60px;
    width: 420px; height: 420px;
    background: radial-gradient(circle, rgba(99,102,241,.10) 0%, transparent 65%);
    border-radius: 50%;
    z-index: 0;
    pointer-events: none;
  }
  .hero-slide-3 h1            { color: #1A1A2E !important; }
  .hero-slide-3 h1 .accent    { color: #E84000 !important; }
  .hero-slide-3 p             { color: #4B5563 !important; }
  .hero-slide-3 .hero-badge   {
    background: rgba(99,102,241,.1) !important;
    border: 1px solid rgba(99,102,241,.25) !important;
    color: #4338ca !important;
  }
  .hero-slide-3 .hero-badge-dot { background: #6366f1 !important; }
  .hero-slide-3 .btn-outline {
    background: rgba(26,26,46,.06) !important;
    color: #1A1A2E !important;
    border-color: rgba(26,26,46,.18) !important;
    backdrop-filter: none !important;
  }
  .hero-slide-3 .btn-outline:hover { background: rgba(26,26,46,.12) !important; }

  /* ── Slider nav arrows – visible on light bg ── */
  .slider-btn {
    background: rgba(26,26,46,.1) !important;
    border: 1.5px solid rgba(26,26,46,.18) !important;
    color: #1A1A2E !important;
  }
  .slider-btn:hover {
    background: #E84000 !important;
    border-color: #E84000 !important;
    color: #fff !important;
  }

  /* ── Dots – visible on light bg ── */
  .slider-dot {
    background: rgba(26,26,46,.25) !important;
  }
  .slider-dot.active {
    background: #E84000 !important;
  }

  /* ── Bottom decorative ring ── */
  .hero-slide-1 .hero-inner,
  .hero-slide-2 .hero-inner,
  .hero-slide-3 .hero-inner { position: relative; z-index: 2; }
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
     </div>
      <div class="search-input-wrap">
        <input type="text" placeholder="Search for products, services and more...">
        <a href="#" class="search-icon" aria-label="Search">🔍</a>
      </div>
    </div>
  </div>
</div>

<!-- ─── HERO SLIDER ─── -->
<section class="hero-slider" id="heroSlider">

  <div class="hero-slides" id="heroSlides">

    <!-- Slide 1 — Main Brand -->
    <div class="hero-slide hero-slide-1">
      <div class="hero-inner">
        <div class="hero-content">
          <div class="hero-badge">
            <div class="hero-badge-dot"></div>
            Smart Digital Platform — India's #1 Choice
          </div>
          <h1>
            India's Smart<br>
            <span class="accent">ePay</span> Digital &amp;<br>
            Ecommerce Platform
          </h1>
          <p>One app for all your shopping, gift vouchers, digital services &amp; more. Simplify your financial life today.</p>
          <div class="hero-btns">
            <a href="#" class="btn-primary">Explore Services →</a>
            <a href="#" class="btn-outline">⬇ Download App</a>
          </div>
        </div>
        <div class="hero-phone">
          <img src="demoepay/images/slider_img.png" alt="ePay Digital App">
        </div>
      </div>
    </div>

    <!-- Slide 2 — Store Shopping -->
    <div class="hero-slide hero-slide-2">
      <div class="hero-inner">
        <div class="hero-content">
          <div class="hero-badge">
            <div class="hero-badge-dot"></div>
            ePay Store — Shop Everything Online
          </div>
          <h1>
            Shop Smarter,<br>
            Save <span class="accent">Bigger</span> Every<br>
            Single Day
          </h1>
          <p>Thousands of products at your fingertips. Exclusive offers, fast delivery &amp; easy returns — all in one place.</p>
          <div class="hero-btns">
            <a href="https://store.epayindia.in/" target="_blank" class="btn-primary">Visit Store →</a>
            <a href="#" class="btn-outline">🏷️ View Offers</a>
          </div>
        </div>
        <div class="hero-phone">
          <img src="demoepay/images/slider_img_2.png" alt="ePay Digital App">
        </div>
      </div>
    </div>

    <!-- Slide 3 — Gift Vouchers & Digital Services -->
    <div class="hero-slide hero-slide-3">
      <div class="hero-inner">
        <div class="hero-content">
          <div class="hero-badge">
            <div class="hero-badge-dot"></div>
            Gift Vouchers &amp; Digital Services
          </div>
          <h1>
            Send <span class="accent">Gift Vouchers</span><br>
            &amp; Access Digital<br>
            Services Instantly
          </h1>
          <p>Instant e-gift cards for every occasion &amp; powerful digital utility tools — all under one ePay membership.</p>
          <div class="hero-btns">
            <a href="https://gv.epayindia.in/" target="_blank" class="btn-primary">Buy Vouchers →</a>
            <a href="https://utility.epayindia.in/" target="_blank" class="btn-outline">⚙️ Digital Services</a>
          </div>
        </div>
         <div class="hero-phone">
          <img src="demoepay/images/slider_img_3.png" alt="ePay Digital App">
        </div>
      </div>
    </div>

  </div><!-- /hero-slides -->

  <!-- Prev / Next buttons -->
  <button class="slider-btn prev" id="heroPrev" aria-label="Previous slide">&#8249;</button>
  <button class="slider-btn next" id="heroNext" aria-label="Next slide">&#8250;</button>

  <!-- Dot indicators -->
  <div class="slider-dots" id="sliderDots">
    <button class="slider-dot active" data-index="0"></button>
    <button class="slider-dot" data-index="1"></button>
    <button class="slider-dot" data-index="2"></button>
  </div>

</section>

<!-- ─── OUR SERVICES ─── -->
<section class="section our-services-section">
  <div class="section-header">
    <h2>Our <span class="accent">Services</span></h2>
    <div class="section-divider">
      <div class="divider-line rev"></div>
      <span class="divider-icon">⚡</span>
      <div class="divider-line"></div>
    </div>
    <p>Fast &amp; Reliable Solutions for You</p>
  </div>

  <div class="services-grid">

    <div class="service-card-live">
      <div class="live-badge"><span class="live-dot"></span> LIVE</div>
      <div class="service-icon">🛍️</div>
      <h3>Store Shopping</h3>
      <p>Your one-stop shop for everything you need.</p>
      <a href="https://store.epayindia.in/" target="_blank" class="service-link">Shop Now →</a>
    </div>

    <div class="service-card-live">
      <div class="live-badge"><span class="live-dot"></span> LIVE</div>
      <div class="service-icon">🛒</div>
      <h3>E-Commerce Solutions</h3>
      <p>Secure online selling with easy checkout.</p>
      <a href="https://shop.epayindia.in/" target="_blank" class="service-link">Shop Now →</a>
    </div>

    <div class="service-card-live">
      <div class="live-badge"><span class="live-dot"></span> LIVE</div>
      <div class="service-icon">💻</div>
      <h3>ePay Digital Services</h3>
      <p>Digital tools to simplify business operations.</p>
      <a href="https://utility.epayindia.in/" target="_blank" class="service-link">Explore Now →</a>
    </div>

    <div class="service-card-live">
      <div class="live-badge"><span class="live-dot"></span> LIVE</div>
      <div class="service-icon">🎁</div>
      <h3>Gift Vouchers</h3>
      <p>Instant digital vouchers for easy gifting.</p>
      <a href="https://gv.epayindia.in/" target="_blank" class="service-link">Buy Vouchers →</a>
    </div>

    <div class="service-card-live">
      <div class="live-badge"><span class="live-dot"></span> LIVE</div>
      <div class="service-icon">🍔</div>
      <h3>Food Tadka</h3>
      <p>Order your favourite food with fast delivery and exclusive offers.</p>
      <a href="https://food.epayindia.in/" target="_blank" class="service-link">Order Food →</a>
    </div>

    <div class="service-card-live">
      <div class="live-badge"><span class="live-dot"></span> LIVE</div>
      <div class="service-icon">🎬</div>
      <h3>Movie Tadka</h3>
      <p>Book your favourite movies with exclusive deals and offers.</p>
      <a href="https://movie.epayindia.in/" target="_blank" class="service-link">Book Ticket →</a>
    </div>

    <div class="service-card-live">
      <div class="live-badge"><span class="live-dot"></span> LIVE</div>
      <div class="service-icon">🏷️</div>
      <h3>Purchase Coupon</h3>
      <p>Get exclusive coupons and save big on every purchase.</p>
      <a href="purchase-coupon.asp" class="service-link">Get Coupon →</a>
    </div>

    <div class="service-card-live">
      <div class="live-badge"><span class="live-dot"></span> LIVE</div>
      <div class="service-icon">👑</div>
      <h3>Subscription Now</h3>
      <p>Subscribe once and enjoy unlimited benefits and rewards forever.</p>
      <a href="subscription-now.asp" class="service-link">Subscribe Now →</a>
    </div>

    <div class="service-card-live">
      <div class="live-badge"><span class="live-dot"></span> LIVE</div>
      <div class="service-icon">⭐</div>
      <h3>Monthly Activation Points</h3>
      <p>Earn and redeem monthly activation points on every activity.</p>
      <a href="monthly-activation-points.asp" class="service-link">View Points →</a>
    </div>

  </div>
</section>



<section class="section services-section">
  <div class="section-header">
    <h2>ePay  <span class="accent">Upcoming Services </span></h2>
    <div class="section-divider">
      <div class="divider-line rev"></div>
      <span class="divider-icon">⚡</span>
      <div class="divider-line"></div>
    </div>
    <p>Discover exciting new features and services coming soon to enhance your digital experience </p>
  </div>


  <div class="services-grid" style="justify-content: center;">

    <div class="service-card-upcoming upc-blue">
      <div class="coming-badge">⏳ Coming Soon</div>
      <div class="service-icon">💳</div>
      <h3>Fintech Services</h3>
      <p>Secure digital payment and fintech solutions.</p>
      <a href="#" class="service-link">Explore Now →</a>
    </div>

    <div class="service-card-upcoming upc-purple">
      <div class="coming-badge">⏳ Coming Soon</div>
      <div class="service-icon">📅</div>
      <h3>Booking Solutions</h3>
      <p>Fast and easy booking systems for travel, events & more.</p>
      <a href="#" class="service-link">Explore Now →</a>
    </div>

    <div class="service-card-upcoming upc-amber">
      <div class="coming-badge">⏳ Coming Soon</div>
      <div class="service-icon">🌐</div>
      <h3>ePay International</h3>
      <p>Seamless cross-border payments and international money transfer solutions.</p>
      <a href="#" class="service-link">Explore Now →</a>
    </div>

    <div class="service-card-upcoming upc-rose">
      <div class="coming-badge">⏳ Coming Soon</div>
      <div class="service-icon">💼</div>
      <h3>ePay Job Portal</h3>
      <p>Find your dream job or hire top talent — India's smart recruitment platform.</p>
      <a href="#" class="service-link">Explore Now →</a>
    </div>

  </div>
</section>

<!-- ─── PREMIUM CTA BANNER ─── -->

<div class="upcoming-section">
  <br>
  <div class="upcoming-banner">
    <div class="upcoming-content">
      <div class="premium-badge">
        <span class="premium-dot"></span>
        <span>ePay India </span>
      </div>
      <h2>Join <span class="premium-accent">ePay Premium</span> —<br>Shop, Save &amp; Earn Every Day</h2>
      <p>One-time subscription. Unlimited rewards, exclusive deals, priority support &amp; cashback on every order — forever.</p>
      <div class="premium-btns">
        <a href="#" class="btn-primary">Subscription →</a>
        <a href="#" class="btn-outline">Learn More</a>
      </div>
    </div>
    <div class="upcoming-features">
      <div class="upcoming-feat">
        <div class="upcoming-feat-icon">🎯</div>
        <span>Exclusive Deals</span>
      </div>
      <div class="upcoming-feat">
        <div class="upcoming-feat-icon">💰</div>
        <span>Cashback Rewards</span>
      </div>
      <div class="upcoming-feat">
        <div class="upcoming-feat-icon">🚀</div>
        <span>Priority Support</span>
      </div>
      <div class="upcoming-feat">
        <div class="upcoming-feat-icon">♾️</div>
        <span>Lifetime Access</span>
      </div>
    </div>
  </div>
</div>

<!-- ─── OFFERS ─── -->
<section class="offers-section">
  <div class="section-header">
    <h2><span class="accent">Offers</span> for All Shopping Products</h2>
    <p>Exclusive deals across every category - Shop more, save more!</p>
  </div>
  <div class="categories-row">
    <button class="scroll-btn left" onclick="scrollCats(-1)">‹</button>

    <div class="categories-scroll" id="catsScroll">

    <div class="product-card">
      <a href="https://store.epayindia.in/Home/ProductInfoContainer/1679183?pName=AKAI%20100%20Watts%20Bluetooth%20Speaker%20with%202%20Wireless%20Mic%20Karaoke%20System%20%7C%20Bluetooth%205.0%20Home%20Theatre%20Music%20System%20with%20Deep%20Bass%20Sound%20for%20TV,%20PC,%20Smartphone%20Music%20%26%20Movies%20%7C%20AUX%2FUSB%2FFM%20(Ultraboom-100)" target="_blank">
        <div class="product-img">
          <img src="https://m.media-amazon.com/images/I/31J2O2gd14L.jpg" alt="AKAI 100 Watts Bluetooth Speaker">
          <span class="badge">Offer</span>
        </div>
      </a>
      <div class="product-info">
        <h3 class="product-title"><a href="https://store.epayindia.in/Home/ProductInfoContainer/1679183?pName=AKAI%20100%20Watts%20Bluetooth%20Speaker%20with%202%20Wireless%20Mic%20Karaoke%20System%20%7C%20Bluetooth%205.0%20Home%20Theatre%20Music%20System%20with%20Deep%20Bass%20Sound%20for%20TV,%20PC,%20Smartphone%20Music%20%26%20Movies%20%7C%20AUX%2FUSB%2FFM%20(Ultraboom-100)" target="_blank" style="text-decoration:none;color:inherit;">AKAI 100 Watts Bluetooth Speaker with 2 Wireless Mic Karaoke System</a></h3>
        <div class="price">₹8,264</div>
        <div class="rating">⭐⭐⭐⭐☆ (4.2)</div>
        <a href="https://store.epayindia.in/Home/ProductInfoContainer/1679183?pName=AKAI%20100%20Watts%20Bluetooth%20Speaker%20with%202%20Wireless%20Mic%20Karaoke%20System%20%7C%20Bluetooth%205.0%20Home%20Theatre%20Music%20System%20with%20Deep%20Bass%20Sound%20for%20TV,%20PC,%20Smartphone%20Music%20%26%20Movies%20%7C%20AUX%2FUSB%2FFM%20(Ultraboom-100)" target="_blank"><button class="btn-cart">Add to Cart</button></a>
      </div>
    </div>

    <div class="product-card">
      <a href="https://store.epayindia.in/Home/ProductInfoContainer/2407665?pName=Bollyclues%20Women%27s%20Floral%20Tiered%20Mini%20Dress%20%7C%20Puff%20Sleeve%20Button-Down%20Casual%20Dress%20%7C%20Lightweight%20Summer%20Babydoll%20Dress%20%7C%20Short%20Sleeve%20Collared%20Western%20Dress%20for%20Women" target="_blank">
        <div class="product-img">
          <img src="https://m.media-amazon.com/images/I/41NEoxfZEtL.jpg" alt="Bollyclues Women's Floral Tiered Mini Dress">
          <span class="badge">Offer</span>
        </div>
      </a>
      <div class="product-info">
        <h3 class="product-title"><a href="https://store.epayindia.in/Home/ProductInfoContainer/2407665?pName=Bollyclues%20Women%27s%20Floral%20Tiered%20Mini%20Dress%20%7C%20Puff%20Sleeve%20Button-Down%20Casual%20Dress%20%7C%20Lightweight%20Summer%20Babydoll%20Dress%20%7C%20Short%20Sleeve%20Collared%20Western%20Dress%20for%20Women" target="_blank" style="text-decoration:none;color:inherit;">Bollyclues Women's Floral Tiered Mini Dress | Puff Sleeve Button-Down Casual Dress</a></h3>
        <div class="price">₹589</div>
        <div class="rating">⭐⭐⭐⭐☆ (4.2)</div>
        <a href="https://store.epayindia.in/Home/ProductInfoContainer/2407665?pName=Bollyclues%20Women%27s%20Floral%20Tiered%20Mini%20Dress%20%7C%20Puff%20Sleeve%20Button-Down%20Casual%20Dress%20%7C%20Lightweight%20Summer%20Babydoll%20Dress%20%7C%20Short%20Sleeve%20Collared%20Western%20Dress%20for%20Women" target="_blank"><button class="btn-cart">Add to Cart</button></a>
      </div>
    </div>

    <div class="product-card">
      <a href="https://store.epayindia.in/Home/ProductInfoContainer/2480474?pName=Generic%20fine%20Fashion%20mart%20Mens%20Formal%20Shirt%20Business" target="_blank">
        <div class="product-img">
          <img src="https://m.media-amazon.com/images/I/41AYbhe-nCL.jpg" alt="Generic Mens Formal Shirt">
          <span class="badge">Offer</span>
        </div>
      </a>
      <div class="product-info">
        <h3 class="product-title"><a href="https://store.epayindia.in/Home/ProductInfoContainer/2480474?pName=Generic%20fine%20Fashion%20mart%20Mens%20Formal%20Shirt%20Business" target="_blank" style="text-decoration:none;color:inherit;">Generic fine Fashion mart Mens Formal Shirt Business</a></h3>
        <div class="price">₹1,399</div>
        <div class="rating">⭐⭐⭐⭐☆ (4.2)</div>
        <a href="https://store.epayindia.in/Home/ProductInfoContainer/2480474?pName=Generic%20fine%20Fashion%20mart%20Mens%20Formal%20Shirt%20Business" target="_blank"><button class="btn-cart">Add to Cart</button></a>
      </div>
    </div>

    <div class="product-card">
      <a href="https://store.epayindia.in/Home/ProductInfoContainer/2378594?pName=HP%2015%20255R%20G10,%20AMD%20Ryzen%205%207535U%20Hexa%20Core%20(6%20Core,%2012%20Threads,%202.9%20to%20Max%204.3%20Ghz)%20(8%20GB%2F512%20GB%20SSD%2FAMD%20Radeon%20Graphics%2FWindows%2011)%20Thin%20and%20Light%20Laptop(15.6%20inch%20HD%20Display,%20Matt%20Silver,%201.45%20kg" target="_blank">
        <div class="product-img">
          <img src="https://m.media-amazon.com/images/I/413PmklYq+L.jpg" alt="HP 15 255R G10 Laptop">
          <span class="badge">Offer</span>
        </div>
      </a>
      <div class="product-info">
        <h3 class="product-title"><a href="https://store.epayindia.in/Home/ProductInfoContainer/2378594?pName=HP%2015%20255R%20G10,%20AMD%20Ryzen%205%207535U%20Hexa%20Core%20(6%20Core,%2012%20Threads,%202.9%20to%20Max%204.3%20Ghz)%20(8%20GB%2F512%20GB%20SSD%2FAMD%20Radeon%20Graphics%2FWindows%2011)%20Thin%20and%20Light%20Laptop(15.6%20inch%20HD%20Display,%20Matt%20Silver,%201.45%20kg" target="_blank" style="text-decoration:none;color:inherit;">HP 15 255R G10, AMD Ryzen 5 7535U Hexa Core (6 Core, 12 Threads, 2.9 to Max 4.3 GHz)</a></h3>
        <div class="price">₹47,950</div>
        <div class="rating">⭐⭐⭐⭐☆ (4.2)</div>
        <a href="https://store.epayindia.in/Home/ProductInfoContainer/2378594?pName=HP%2015%20255R%20G10,%20AMD%20Ryzen%205%207535U%20Hexa%20Core%20(6%20Core,%2012%20Threads,%202.9%20to%20Max%204.3%20Ghz)%20(8%20GB%2F512%20GB%20SSD%2FAMD%20Radeon%20Graphics%2FWindows%2011)%20Thin%20and%20Light%20Laptop(15.6%20inch%20HD%20Display,%20Matt%20Silver,%201.45%20kg" target="_blank"><button class="btn-cart">Add to Cart</button></a>
      </div>
    </div>

    <div class="product-card">
      <a href="https://store.epayindia.in/Home/ProductInfoContainer/1942375?pName=Sony%20108%20cm%20(43%20inches)%20BRAVIA%202M2%20Series%204K%20Ultra%20HD%20Smart%20LED%20Google%20TV%20K-43S22BM2" target="_blank">
        <div class="product-img">
          <img src="https://m.media-amazon.com/images/I/51Tw5W9fg9L.jpg" alt="Sony 43 inch BRAVIA 4K TV">
          <span class="badge">Offer</span>
        </div>
      </a>
      <div class="product-info">
        <h3 class="product-title"><a href="https://store.epayindia.in/Home/ProductInfoContainer/1942375?pName=Sony%20108%20cm%20(43%20inches)%20BRAVIA%202M2%20Series%204K%20Ultra%20HD%20Smart%20LED%20Google%20TV%20K-43S22BM2" target="_blank" style="text-decoration:none;color:inherit;">Sony 108 cm (43 inches) BRAVIA 2M2 Series 4K Ultra HD Smart LED Google TV</a></h3>
        <div class="price">₹47,990</div>
        <div class="rating">⭐⭐⭐⭐☆ (4.2)</div>
        <a href="https://store.epayindia.in/Home/ProductInfoContainer/1942375?pName=Sony%20108%20cm%20(43%20inches)%20BRAVIA%202M2%20Series%204K%20Ultra%20HD%20Smart%20LED%20Google%20TV%20K-43S22BM2" target="_blank"><button class="btn-cart">Add to Cart</button></a>
      </div>
    </div>

    <div class="product-card">
      <a href="https://store.epayindia.in/Home/ProductInfoContainer/2288277?pName=Oppo%20A6%20Pro%205G%20(Brown,%208GB%20RAM,%20128GB%20Storage)%20with%20No%20Cost%20EMI%2FAdditional%20Exchange%20Offers" target="_blank">
        <div class="product-img">
          <img src="https://m.media-amazon.com/images/I/41U-CWp5PkL.jpg" alt="Oppo A6 Pro 5G">
          <span class="badge">Offer</span>
        </div>
      </a>
      <div class="product-info">
        <h3 class="product-title"><a href="https://store.epayindia.in/Home/ProductInfoContainer/2288277?pName=Oppo%20A6%20Pro%205G%20(Brown,%208GB%20RAM,%20128GB%20Storage)%20with%20No%20Cost%20EMI%2FAdditional%20Exchange%20Offers" target="_blank" style="text-decoration:none;color:inherit;">Oppo A6 Pro 5G (Brown, 8GB RAM, 128GB Storage) with No Cost EMI/Additional Exchange Offers</a></h3>
        <div class="price">₹26,999</div>
        <div class="rating">⭐⭐⭐⭐☆ (4.2)</div>
        <a href="https://store.epayindia.in/Home/ProductInfoContainer/2288277?pName=Oppo%20A6%20Pro%205G%20(Brown,%208GB%20RAM,%20128GB%20Storage)%20with%20No%20Cost%20EMI%2FAdditional%20Exchange%20Offers" target="_blank"><button class="btn-cart">Add to Cart</button></a>
      </div>
    </div>

    <div class="product-card">
      <a href="https://store.epayindia.in/Home/ProductInfoContainer/2465388?pName=REDMI%20Note%2015%205G%20(Glacier%20Blue,%208GB%20RAM%20128GB%20Storage)%20%7C%20108MP%20OIS%20Camera%20%7C%20Snapdragon%206%20Gen%203%20%7C%2017.2cm%20Tough%20Curved%20AMOLED%20Screen%20%7C%20Segment%E2%80%99s%20Slimmest%20Phone%20%7C%205520mAh%20Si%2FC%20Battery%20%7C%2045W%20Fast%20Charging" target="_blank">
        <div class="product-img">
          <img src="https://m.media-amazon.com/images/I/41gggAvZQ1L.jpg" alt="REDMI Note 15 5G">
          <span class="badge">Offer</span>
        </div>
      </a>
      <div class="product-info">
        <h3 class="product-title"><a href="https://store.epayindia.in/Home/ProductInfoContainer/2465388?pName=REDMI%20Note%2015%205G%20(Glacier%20Blue,%208GB%20RAM%20128GB%20Storage)%20%7C%20108MP%20OIS%20Camera%20%7C%20Snapdragon%206%20Gen%203%20%7C%2017.2cm%20Tough%20Curved%20AMOLED%20Screen%20%7C%20Segment%E2%80%99s%20Slimmest%20Phone%20%7C%205520mAh%20Si%2FC%20Battery%20%7C%2045W%20Fast%20Charging" target="_blank" style="text-decoration:none;color:inherit;">REDMI Note 15 5G (Glacier Blue, 8GB RAM 128GB Storage) | 108MP OIS Camera</a></h3>
        <div class="price">₹24,999</div>
        <div class="rating">⭐⭐⭐⭐☆ (4.2)</div>
        <a href="https://store.epayindia.in/Home/ProductInfoContainer/2465388?pName=REDMI%20Note%2015%205G%20(Glacier%20Blue,%208GB%20RAM%20128GB%20Storage)%20%7C%20108MP%20OIS%20Camera%20%7C%20Snapdragon%206%20Gen%203%20%7C%2017.2cm%20Tough%20Curved%20AMOLED%20Screen%20%7C%20Segment%E2%80%99s%20Slimmest%20Phone%20%7C%205520mAh%20Si%2FC%20Battery%20%7C%2045W%20Fast%20Charging" target="_blank"><button class="btn-cart">Add to Cart</button></a>
      </div>
    </div>



    </div>
    <button class="scroll-btn right" onclick="scrollCats(1)">›</button>
  </div>
</section>

<!-- ─── WHY CHOOSE ─── -->
<section class="why-section">
  <div class="why-inner">
    <div class="section-header">
      <h2>Why Choose <span class="accent">ePay?</span></h2>
    </div>
    <div class="why-features-row">
      <div class="why-feat">
        <div class="why-feat-icon">🎧</div>
        <div class="why-feat-text">
          <h4>24/7 Support</h4>
          <p>We are here for you always.</p>
        </div>
      </div>
      <div class="why-feat">
        <div class="why-feat-icon">🏷️</div>
        <div class="why-feat-text">
          <h4>Best Offers</h4>
          <p>Enjoy exclusive deals and discounts.</p>
        </div>
      </div>
      <div class="why-feat">
        <div class="why-feat-icon">🔒</div>
        <div class="why-feat-text">
          <h4>Secure Payments</h4>
          <p>PCI DSS certified transactions.</p>
        </div>
      </div>
      <div class="why-feat">
        <div class="why-feat-icon">⚡</div>
        <div class="why-feat-text">
          <h4>Fast Delivery</h4>
          <p>Express shipping nationwide.</p>
        </div>
      </div>
    </div>

    <!-- Stats Band -->
   <div class="stats-band">

  <div class="stat-item">
    <div class="stat-icon">👥</div>
    <div>
      <div class="stat-num">2 Lakh+</div>
      <div class="stat-label">Happy Users</div>
    </div>
  </div>

  <div class="divider"></div>

  <div class="stat-item">
    <div class="stat-icon">🔧</div>
    <div>
      <div class="stat-num">50+</div>
      <div class="stat-label">Services</div>
    </div>
  </div>

  <div class="divider"></div>

  <div class="stat-item">
    <div class="stat-icon">🤝</div>
    <div>
      <div class="stat-num">100+</div>
      <div class="stat-label">Partners</div>
    </div>
  </div>

  <div class="divider"></div>

  <div class="stat-item">
    <div class="stat-icon">⏰</div>
    <div>
      <div class="stat-num">99.9%</div>
      <div class="stat-label">Uptime</div>
    </div>
  </div>

</div>



  </div>
</section>

<!-- ─── ABOUT ─── -->
<section class="about-section">
  <div class="about-inner">
    <div class="about-content">
      <h2>India's Smart <span class="accent">ePay</span> Digital &amp; Ecommerce Platform</h2>
      <p>Welcome to ePay Digital India Pvt. Ltd. is a technology-driven platform offering eCommerce through a unified ecosystem.

Our platform is designed to provide users with seamless access to online shopping, digital services & Business Solutions.
ePay focuses on delivering a secure, user-friendly, and scalable digital experience while enabling merchants and users to benefit from value-added services, rewards programs, and business growth opportunities in compliance with applicable guidelines and policies. </p>
     
    </div>
    <div class="about-visual">
      <div class="about-features">
        <div class="about-feat">
          <div class="about-feat-icon">♾️</div>
          <div class="about-feat-text">
            <h5>ePay India </h5>
            <p>One time Subscription, unlimited benefits and access forever.</p>
          </div>
        </div>
        <div class="about-feat">
          <div class="about-feat-icon">💰</div>
          <div class="about-feat-text">
            <h5>Shop More </h5>
            <p>Shop and get rewards on every purchase.</p>
          </div>
        </div>
        <div class="about-feat">
          <div class="about-feat-icon">✅</div>
          <div class="about-feat-text">
            <h5>Secure & Trusted</h5>
            <p>Bank-grade encrypted and PCI DSS certified opportunity</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</section>

<!-- ─── FOOTER ─── -->

<!-- #include file="inc_footer.asp" --> 


<script>
  // ─── Hero Slider ───
  (function () {
    var slides  = document.getElementById('heroSlides');
    var dots    = document.querySelectorAll('.slider-dot');
    var total   = 3;
    var current = 0;
    var timer;

    function goTo(index) {
      current = (index + total) % total;
      slides.style.transform = 'translateX(-' + current * 100 + '%)';
      dots.forEach(function (d, i) {
        d.classList.toggle('active', i === current);
      });
    }

    function next() { goTo(current + 1); }
    function prev() { goTo(current - 1); }

    function startAuto() { timer = setInterval(next, 6500); }
    function stopAuto()  { clearInterval(timer); }

    document.getElementById('heroNext').addEventListener('click', function () { stopAuto(); next(); startAuto(); });
    document.getElementById('heroPrev').addEventListener('click', function () { stopAuto(); prev(); startAuto(); });

    dots.forEach(function (d) {
      d.addEventListener('click', function () {
        stopAuto();
        goTo(parseInt(this.dataset.index));
        startAuto();
      });
    });

    // Pause on hover
    document.getElementById('heroSlider').addEventListener('mouseenter', stopAuto);
    document.getElementById('heroSlider').addEventListener('mouseleave', startAuto);

    startAuto();
  })();

  // ─── User dropdown toggle ───
  const userDropdownBtn  = document.getElementById('userDropdownBtn');
  const userDropdown     = document.getElementById('userDropdown');
  const userDropdownWrap = document.getElementById('userDropdownWrap');

  if (userDropdownBtn && userDropdown) {
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

  // ─── Mobile nav toggle ───
  const hamburgerBtn = document.getElementById('hamburgerBtn');
  const mobileNav    = document.getElementById('mobileNav');

  hamburgerBtn.addEventListener('click', function () {
    const isOpen = mobileNav.classList.toggle('open');
    hamburgerBtn.classList.toggle('open', isOpen);
    hamburgerBtn.setAttribute('aria-expanded', isOpen);
  });

  // Close menu when a mobile nav link is clicked
  mobileNav.querySelectorAll('a').forEach(function (link) {
    link.addEventListener('click', function () {
      mobileNav.classList.remove('open');
      hamburgerBtn.classList.remove('open');
      hamburgerBtn.setAttribute('aria-expanded', 'false');
    });
  });

  // Close menu on outside click
  document.addEventListener('click', function (e) {
    if (!hamburgerBtn.contains(e.target) && !mobileNav.contains(e.target)) {
      mobileNav.classList.remove('open');
      hamburgerBtn.classList.remove('open');
      hamburgerBtn.setAttribute('aria-expanded', 'false');
    }
  });

  // Smooth scroll for categories on mobile
  function scrollCats(dir) {
    const el = document.getElementById('catsScroll');
    el.scrollBy({ left: dir * 240, behavior: 'smooth' });
  }

  // Intersection observer for subtle entrance animations
  const observer = new IntersectionObserver((entries) => {
    entries.forEach(e => {
      if (e.isIntersecting) {
        e.target.style.opacity = '1';
        e.target.style.transform = 'translateY(0)';
      }
    });
  }, { threshold: 0.1 });

  document.querySelectorAll('.service-card, .cat-card, .about-feat, .why-feat').forEach(el => {
    el.style.opacity = '0';
    el.style.transform = 'translateY(20px)';
    el.style.transition = 'opacity .5s ease, transform .5s ease';
    observer.observe(el);
  });

  // Stagger service cards
  document.querySelectorAll('.service-card').forEach((el, i) => {
    el.style.transitionDelay = `${i * 0.08}s`;
  });
  document.querySelectorAll('.cat-card').forEach((el, i) => {
    el.style.transitionDelay = `${i * 0.05}s`;
  });
</script>

</body>
</html>
