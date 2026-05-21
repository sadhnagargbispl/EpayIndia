<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Welcome.aspx.cs" Inherits="Welcome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
    <style>
        :root {
            --orange: #E84000;
            --og: rgba(232,64,0,0.10);
            --green: #16a34a;
            --gg: rgba(22,163,74,0.10);
            --muted: #6B7280;
            --border: #E2E8F0;
            --text: #1A1A2E;
        }

        /* ── Page Breadcrumb ── */
        .wel-breadcrumb {
            background: #fff;
            border-bottom: 1px solid var(--border);
            padding: 14px 20px;
        }

        .wel-breadcrumb-inner {
            max-width: 640px;
            margin: auto;
            display: flex;
            align-items: center;
            gap: 8px;
            font-size: .78rem;
            color: var(--muted);
        }

            .wel-breadcrumb-inner a {
                color: var(--orange);
                text-decoration: none;
                font-weight: 600;
            }

                .wel-breadcrumb-inner a:hover {
                    text-decoration: underline;
                }

        .wel-breadcrumb-sep {
            color: #d1d5db;
        }

        /* ── Main Layout ── */
        .wel-section {
            padding: 28px 16px 40px;
            background: #f9fafb;
            min-height: 70vh;
        }

        .wel-wrap {
            max-width: 640px;
            margin: auto;
        }

        /* ── Action bar ── */
        .wel-actionbar {
            display: flex;
            justify-content: flex-end;
            gap: 8px;
            flex-wrap: wrap;
            margin-bottom: 16px;
        }

        .wel-act-btn {
            font-family: 'Sora', sans-serif;
            font-size: .78rem;
            font-weight: 700;
            padding: 8px 16px;
            border-radius: 50px;
            border: none;
            cursor: pointer;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 5px;
            transition: background .25s, transform .2s, box-shadow .25s, color .2s;
        }

        .wel-act-primary {
            background: var(--orange);
            color: #fff;
            box-shadow: 0 6px 20px rgba(232,64,0,.30);
        }

            .wel-act-primary:hover {
                background: #c73600;
                transform: translateY(-2px);
                box-shadow: 0 10px 28px rgba(232,64,0,.40);
                color: #fff;
            }

        .wel-act-ghost {
            background: #fff;
            color: var(--text);
            border: 1.5px solid var(--border);
        }

            .wel-act-ghost:hover {
                background: #f9fafb;
                border-color: var(--orange);
                color: var(--orange);
            }

        .wel-act-success {
            background: var(--green);
            color: #fff;
            box-shadow: 0 6px 20px rgba(22,163,74,.30);
        }

            .wel-act-success:hover {
                background: #15803d;
                transform: translateY(-2px);
                color: #fff;
            }

        /* ── Hero card ── */
        .wel-hero {
            background: #fff;
            border-radius: 20px;
            border: 1px solid var(--border);
            overflow: hidden;
            box-shadow: 0 4px 32px rgba(0,0,0,.06);
        }

        .wel-hero-header {
            background: linear-gradient(135deg, var(--orange) 0%, #ff8c42 100%);
            color: #fff;
            padding: 24px 24px 50px;
            text-align: center;
            position: relative;
            overflow: hidden;
        }

            .wel-hero-header::before,
            .wel-hero-header::after {
                content: '';
                position: absolute;
                border-radius: 50%;
                background: rgba(255,255,255,.08);
            }

            .wel-hero-header::before {
                width: 130px;
                height: 130px;
                top: -40px;
                left: -30px;
            }

            .wel-hero-header::after {
                width: 160px;
                height: 160px;
                bottom: -70px;
                right: -40px;
            }

        .wel-hero-badge {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background: rgba(255,255,255,.18);
            border: 1px solid rgba(255,255,255,.28);
            color: #fff;
            font-size: .65rem;
            font-weight: 700;
            letter-spacing: .7px;
            text-transform: uppercase;
            padding: 4px 12px;
            border-radius: 50px;
            margin-bottom: 10px;
            position: relative;
            z-index: 1;
        }

        .wel-hero-dot {
            width: 6px;
            height: 6px;
            background: #fff;
            border-radius: 50%;
            animation: blink 1.4s infinite;
        }

        @keyframes blink {
            0%,100% { opacity: 1; }
            50% { opacity: .35; }
        }

        .wel-hero-header h1 {
            font-family: 'Sora', sans-serif;
            font-size: clamp(1.2rem,2.4vw,1.5rem);
            font-weight: 800;
            margin: 0 0 6px;
            position: relative;
            z-index: 1;
            color: #fff;
        }

        .wel-hero-header .wel-celebrate {
            font-size: 1.8rem;
            margin-bottom: 4px;
            display: block;
            position: relative;
            z-index: 1;
        }

        .wel-hero-header p {
            font-size: .82rem;
            opacity: .95;
            margin: 0;
            position: relative;
            z-index: 1;
        }

        /* Greeting card overlapping hero */
        .wel-greeting {
            background: #fff;
            margin: -32px 22px 0;
            padding: 16px 20px;
            border-radius: 14px;
            border: 1px solid var(--border);
            box-shadow: 0 6px 22px rgba(0,0,0,.07);
            text-align: center;
            position: relative;
            z-index: 2;
        }

        .wel-greeting-name {
            font-family: 'Sora', sans-serif;
            font-size: 1rem;
            font-weight: 800;
            color: var(--text);
            margin-bottom: 3px;
        }

            .wel-greeting-name .wel-id {
                color: var(--orange);
                font-weight: 700;
            }

        .wel-greeting-date {
            font-size: .76rem;
            color: var(--muted);
        }

            .wel-greeting-date strong {
                color: var(--text);
                font-weight: 700;
            }

        /* Body */
        .wel-body {
            padding: 22px 22px 24px;
        }

        .wel-section-title {
            font-family: 'Sora', sans-serif;
            font-size: .78rem;
            font-weight: 700;
            color: var(--muted);
            text-transform: uppercase;
            letter-spacing: .6px;
            margin-bottom: 14px;
            padding-bottom: 10px;
            border-bottom: 1px solid var(--border);
            display: flex;
            align-items: center;
            gap: 8px;
        }

        /* Info rows */
        .wel-info-rows {
            display: flex;
            flex-direction: column;
            gap: 10px;
            margin-bottom: 18px;
        }

        .wel-info-row {
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 10px 14px;
            border-radius: 10px;
            background: #f9fafb;
            border: 1px solid var(--border);
        }

        .wel-info-row-left {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .wel-info-icon {
            width: 34px;
            height: 34px;
            border-radius: 9px;
            background: var(--og);
            border: 1px solid rgba(232,64,0,.2);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: .95rem;
            flex-shrink: 0;
        }

        .wel-info-key {
            font-size: .68rem;
            font-weight: 700;
            color: var(--muted);
            text-transform: uppercase;
            letter-spacing: .5px;
            margin-bottom: 1px;
        }

        .wel-info-val {
            font-family: 'Sora', sans-serif;
            font-size: .88rem;
            font-weight: 700;
            color: var(--text);
            word-break: break-all;
        }

        /* Highlight password rows */
        .wel-info-row.highlight {
            background: linear-gradient(135deg,#fff7ed,#ffedd5);
            border-color: rgba(232,64,0,.25);
        }

            .wel-info-row.highlight .wel-info-val {
                color: var(--orange);
            }

        /* Footer thank-you */
        .wel-footer {
            text-align: center;
            padding: 16px 16px;
            background: #f9fafb;
            border-radius: 12px;
            border: 1px dashed var(--border);
        }

            .wel-footer p {
                font-size: .82rem;
                color: var(--muted);
                margin: 0 0 4px;
            }

            .wel-footer .wel-company {
                font-family: 'Sora', sans-serif;
                font-size: .95rem;
                font-weight: 800;
                color: var(--orange);
            }

        /* Hide the original raw <table> when used inside our card —
           we render values via labels styled above, but keep tag for server access */
        #welcomeletter {
            display: none;
        }

        /* PRINT */
        @media print {
            .wel-actionbar,
            .wel-breadcrumb {
                display: none !important;
            }

            .wel-section {
                background: #fff;
                padding: 0;
            }

            .wel-hero {
                box-shadow: none;
                border: 1px solid #ddd;
            }

            .wel-hero-header {
                -webkit-print-color-adjust: exact;
                print-color-adjust: exact;
            }
        }

        /* Responsive — Tablet */
        @media (max-width: 768px) {
            .wel-section {
                padding: 20px 12px 32px;
            }

            .wel-wrap {
                max-width: 100%;
            }

            .wel-hero-header {
                padding: 22px 18px 46px;
            }

            .wel-hero-header h1 {
                font-size: 1.25rem;
            }

            .wel-hero-header .wel-celebrate {
                font-size: 1.6rem;
            }

            .wel-greeting {
                margin: -30px 16px 0;
                padding: 14px 16px;
            }

            .wel-greeting-name {
                font-size: .92rem;
                line-height: 1.4;
            }

            .wel-body {
                padding: 20px 16px 20px;
            }
        }

        /* Responsive — Mobile */
        @media (max-width: 480px) {
            .wel-breadcrumb {
                padding: 10px 14px;
            }

            .wel-breadcrumb-inner {
                font-size: .72rem;
            }

            .wel-section {
                padding: 16px 10px 28px;
            }

            .wel-actionbar {
                justify-content: stretch;
                gap: 6px;
                margin-bottom: 12px;
            }

            .wel-act-btn {
                flex: 1 1 auto;
                justify-content: center;
                padding: 9px 10px;
                font-size: .74rem;
            }

            .wel-hero-header {
                padding: 20px 14px 44px;
            }

            .wel-hero-badge {
                font-size: .6rem;
                padding: 3px 10px;
            }

            .wel-hero-header .wel-celebrate {
                font-size: 1.5rem;
            }

            .wel-hero-header h1 {
                font-size: 1.1rem;
                margin: 0 0 4px;
            }

            .wel-hero-header p {
                font-size: .76rem;
            }

            .wel-greeting {
                margin: -28px 12px 0;
                padding: 12px 14px;
                border-radius: 12px;
            }

            .wel-greeting-name {
                font-size: .88rem;
                line-height: 1.4;
                word-break: break-word;
            }

                .wel-greeting-name .wel-id {
                    display: inline-block;
                    margin-top: 2px;
                }

            .wel-greeting-date {
                font-size: .72rem;
            }

            .wel-body {
                padding: 18px 14px 18px;
            }

            .wel-section-title {
                font-size: .72rem;
                margin-bottom: 12px;
                padding-bottom: 8px;
            }

            .wel-info-rows {
                gap: 8px;
                margin-bottom: 14px;
            }

            .wel-info-row {
                padding: 9px 12px;
                border-radius: 10px;
            }

            .wel-info-row-left {
                gap: 9px;
                width: 100%;
            }

            .wel-info-icon {
                width: 30px;
                height: 30px;
                font-size: .85rem;
                border-radius: 8px;
            }

            .wel-info-key {
                font-size: .62rem;
            }

            .wel-info-val {
                font-size: .82rem;
            }

            .wel-footer {
                padding: 14px 12px;
            }

                .wel-footer p {
                    font-size: .76rem;
                }

                .wel-footer .wel-company {
                    font-size: .88rem;
                }
        }

        /* Very small phones */
        @media (max-width: 360px) {
            .wel-hero-header h1 {
                font-size: 1rem;
            }

            .wel-greeting-name {
                font-size: .82rem;
            }

            .wel-info-val {
                font-size: .78rem;
            }

            .wel-act-btn {
                font-size: .7rem;
                padding: 8px 8px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- ─── BREADCRUMB ─── -->
    <div class="wel-breadcrumb">
        <div class="wel-breadcrumb-inner">
            <a href="index.aspx">Home</a>
            <span class="wel-breadcrumb-sep">›</span>
            <span>Welcome</span>
        </div>
    </div>

    <!-- ─── MAIN CONTENT ─── -->
    <section class="wel-section">
        <div class="wel-wrap">

            <!-- ACTION BUTTONS -->
            <div class="wel-actionbar">
                <button type="button" class="wel-act-btn wel-act-primary"
                    id="BtnHome" runat="server"
                    onclick="BtnHome_ServerClick" visible="false">
                    🏠 Home
                </button>

                <button type="button" class="wel-act-btn wel-act-ghost"
                    id="BtnPrint" runat="server"
                    onclick="javascript:PrintDiv();" visible="false">
                    🖨️ Print
                </button>

                <button type="button" class="wel-act-btn wel-act-success"
                    id="BtnNewJoin" runat="server"
                    onclick="BtnNewJoin_ServerClick" visible="false">
                    ➕ New Joining
                </button>
            </div>

            <!-- MAIN CONTENT -->
            <div id="dvContents" runat="server">

                <div class="wel-hero">

                    <!-- HERO HEADER -->
                    <div class="wel-hero-header">
                   <%--     <div class="wel-hero-badge">
                            <div class="wel-hero-dot"></div>
                            Registration Successful
                        </div>--%>
                        <span class="wel-celebrate">🎉</span>
                        <h1>Welcome to the Family!</h1>
                        <p>Your account has been created successfully</p>
                    </div>

                    <!-- GREETING (overlapping) -->
                    <div class="wel-greeting">
                        <div class="wel-greeting-name">
                            Dear <asp:Label ID="LblName" runat="server"></asp:Label>
                            <span class="wel-id">(<asp:Label ID="LblIdno" runat="server"></asp:Label>)</span>
                        </div>
                        <div class="wel-greeting-date">
                            Date : <strong><asp:Label ID="lblDoj" runat="server"></asp:Label></strong>
                        </div>
                    </div>

                    <!-- BODY -->
                    <div class="wel-body">

                        <div class="wel-section-title">📋 Account Details</div>

                        <div class="wel-info-rows">

                            <!-- Member Name -->
                            <div class="wel-info-row">
                                <div class="wel-info-row-left">
                                    <div class="wel-info-icon">👤</div>
                                    <div>
                                        <div class="wel-info-key">Member Name</div>
                                        <div class="wel-info-val">
                                            <asp:Label ID="LblName1" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Member ID -->
                            <div class="wel-info-row">
                                <div class="wel-info-row-left">
                                    <div class="wel-info-icon">🪪</div>
                                    <div>
                                        <div class="wel-info-key">Member ID</div>
                                        <div class="wel-info-val">
                                            <asp:Label ID="LblIdno1" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Joining Date -->
                            <div class="wel-info-row">
                                <div class="wel-info-row-left">
                                    <div class="wel-info-icon">📅</div>
                                    <div>
                                        <div class="wel-info-key">Joining Date</div>
                                        <div class="wel-info-val">
                                            <asp:Label ID="lblDoj1" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Password -->
                            <div class="wel-info-row highlight">
                                <div class="wel-info-row-left">
                                    <div class="wel-info-icon">🔑</div>
                                    <div>
                                        <div class="wel-info-key">Password</div>
                                        <div class="wel-info-val">
                                            <asp:Label ID="lblPassw" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Transaction Password -->
                            <div class="wel-info-row highlight">
                                <div class="wel-info-row-left">
                                    <div class="wel-info-icon">🔐</div>
                                    <div>
                                        <div class="wel-info-key">Transaction Password</div>
                                        <div class="wel-info-val">
                                            <asp:Label ID="lblTransPassw" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <!-- Hidden table for server-side reference (do not remove) -->
                     <%--   <table id="welcomeletter" runat="server">
                            <tbody>
                                <tr><td>Member Name</td><td></td></tr>
                                <tr><td>Member ID</td><td></td></tr>
                                <tr><td>Joining Date</td><td></td></tr>
                                <tr><td>Password</td><td></td></tr>
                                <tr><td>Transaction Password</td><td></td></tr>
                            </tbody>
                        </table>--%>

                        <!-- FOOTER -->
                        <div class="wel-footer">
                            <p>Thank you for joining ConnectDots family.</p>
                            <div class="wel-company">
                                <%=Session["CompName"]%>
                            </div>
                        </div>

                    </div>
                </div>

            </div>

        </div>
    </section>
        <script>
        function switchTab(btn, tabId) {
            document.querySelectorAll('.cpd-tab-btn').forEach(b => b.classList.remove('active'));
            document.querySelectorAll('.cpd-tab-content').forEach(t => t.classList.remove('active'));
            btn.classList.add('active');
            document.getElementById(tabId).classList.add('active');
        }

        //const hamburgerBtn = document.getElementById('hamburgerBtn');
        //const mobileNav = document.getElementById('mobileNav');
        //if (hamburgerBtn && mobileNav) {
        //    hamburgerBtn.addEventListener('click', function () {
        //        const isOpen = mobileNav.classList.toggle('open');
        //        hamburgerBtn.classList.toggle('open', isOpen);
        //        hamburgerBtn.setAttribute('aria-expanded', isOpen);
        //    });
        //    mobileNav.querySelectorAll('a').forEach(function (link) {
        //        link.addEventListener('click', function () {
        //            mobileNav.classList.remove('open');
        //            hamburgerBtn.classList.remove('open');
        //            hamburgerBtn.setAttribute('aria-expanded', 'false');
        //        });
        //    });
        //    document.addEventListener('click', function (e) {
        //        if (!hamburgerBtn.contains(e.target) && !mobileNav.contains(e.target)) {
        //            mobileNav.classList.remove('open');
        //            hamburgerBtn.classList.remove('open');
        //            hamburgerBtn.setAttribute('aria-expanded', 'false');
        //        }
        //    });
        //}
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
