<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="shipping-delivery-policy_web.aspx.cs" Inherits="shipping_delivery_policy_web" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

        .page-hero-inner {
            position: relative;
            z-index: 1;
            max-width: 760px;
            margin: auto;
        }

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

            .page-hero h1 span {
                color: var(--orange);
            }

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

        .policy-section {
            margin-bottom: 48px;
        }

            .policy-section h2 {
                font-family: 'Sora', sans-serif;
                font-size: 1.35rem;
                font-weight: 800;
                color: var(--text);
                margin-bottom: 14px;
                padding-bottom: 10px;
                border-bottom: 2px solid var(--orange);
                display: inline-block;
            }

            .policy-section p {
                color: #374151;
                font-size: .97rem;
                line-height: 1.85;
                margin-bottom: 12px;
            }

        .policy-card {
            background: #f9fafb;
            border: 1px solid var(--border);
            border-radius: 14px;
            padding: 28px 32px;
            margin-bottom: 20px;
        }

            .policy-card h3 {
                font-family: 'Sora', sans-serif;
                font-size: 1rem;
                font-weight: 700;
                color: var(--orange);
                margin-bottom: 8px;
            }

            .policy-card p {
                color: #374151;
                font-size: .95rem;
                line-height: 1.8;
                margin: 0;
            }

        .checklist {
            list-style: none;
            padding: 0;
            margin: 12px 0 0;
        }

            .checklist li {
                padding: 8px 0 8px 28px;
                position: relative;
                color: #374151;
                font-size: .95rem;
                line-height: 1.7;
                border-bottom: 1px solid var(--border);
            }

                .checklist li:last-child {
                    border-bottom: none;
                }

                .checklist li::before {
                    content: '✓';
                    position: absolute;
                    left: 0;
                    color: var(--orange);
                    font-weight: 700;
                }

        .contact-box {
            background: linear-gradient(135deg, rgba(232,64,0,0.06), rgba(232,64,0,0.02));
            border: 1px solid rgba(232,64,0,0.2);
            border-radius: 14px;
            padding: 28px 32px;
            margin-top: 40px;
        }

            .contact-box h3 {
                font-family: 'Sora', sans-serif;
                font-size: 1.05rem;
                font-weight: 700;
                color: var(--text);
                margin-bottom: 12px;
            }

            .contact-box p {
                color: #374151;
                font-size: .95rem;
                line-height: 1.8;
                margin: 0 0 6px;
            }

            .contact-box a {
                color: var(--orange);
                text-decoration: none;
                font-weight: 600;
            }

        .jurisdiction-note {
            background: #fffbeb;
            border-left: 4px solid #f59e0b;
            padding: 14px 20px;
            border-radius: 0 8px 8px 0;
            margin-top: 16px;
            font-size: .9rem;
            color: #92400e;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="page-hero">
        <div class="page-hero-inner">
            <div class="page-hero-badge">&#10003; Policy</div>
            <h1>Shipping / <span>Delivery Policy</span></h1>
            <p>Learn about our shipping timelines, delivery partners, and everything related to how your orders reach you safely.</p>
        </div>
    </section>

    <div class="page-content">

        <!-- How delivery works -->
        <div class="policy-section">
            <h2>How does the delivery process work?</h2>
            <p>Once our system processes your order, your products are inspected thoroughly to ensure they are in a perfect condition.</p>
            <p>After they pass through the final round of quality check, they are packed and handed over to our delivery partner i.e. <strong>Blue Dart</strong>.</p>
            <p>Our delivery partners then bring the package to you at the earliest possibility. In case they are unable to reach your provided address or at a suitable time, they will contact you to resolve the issue.</p>
            <p>Guarantee for delivery made by the Company is subject to the terms and conditions of the courier company. Any inconsistency/errors in name or address will result in non-delivery of the product.</p>
        </div>

        <!-- FAQ -->
        <div class="policy-section">
            <h2>FAQ</h2>

            <div class="policy-card">
                <h3>What is the estimated delivery time?</h3>
                <p>Our orders are delivered normally within <strong>7–10 business days</strong> (excluding Sundays and public holidays).</p>
            </div>

            <div class="policy-card">
                <h3>Are there any shipping charges applicable on my order?</h3>
                <p>We have standard shipping charges which may vary according to the volumetric weight of the order.</p>
            </div>
        </div>

        <!-- Return Policy -->
        <div class="policy-section">
            <h2>EPay Digital India Pvt. Ltd. — Return Policy</h2>
            <p>EPay Digital India Pvt. Ltd. offers its customers an <strong>'Easy return policy'</strong>, wherein you can raise a return request of a product within <strong>1–3 business days</strong> of its delivery.</p>
            <p>Returns are accepted if the product you receive has a mismatch in colour or size, or arrives in a damaged condition.</p>
            <p><strong>Please note:</strong> We do not entertain returns if you simply dislike the product or wish to exchange it for personal preference.</p>

            <h2 style="margin-top: 32px;">Checklist to Return the Product</h2>
            <p>Please ensure the following conditions are met before initiating a return:</p>
            <ul class="checklist">
                <li>Products must be returned with all original tags intact.</li>
                <li>Original packaging must be intact and undamaged.</li>
                <li>Items must be in an unwashed, undamaged, and unused condition.</li>
                <li>Refund / replacement will be issued after a thorough inspection of the product(s).</li>
                <li>The process will be completed within <strong>72 working hours</strong> once the product is received at our office.</li>
            </ul>

            <div class="jurisdiction-note">
                All arising disputes will be subject to <strong>Bhilwara Jurisdiction</strong> only.
            </div>
        </div>

        <!-- Contact -->
        <div class="contact-box">
            <h3>Grievance Officer — Contact Us</h3>
            <p>&#9993; Email: <a href="/cdn-cgi/l/email-protection#1c75727a735c796c7d65757278757d327572"><span class="__cf_email__" data-cfemail="eb82858d84ab8e9b8a9282858f828ac58285">[email&#160;protected]</span></a></p>
            <p>&#128222; Phone: <a href="#">+91 XXX-XXX-XXXX</a></p>
        </div>

    </div>
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

