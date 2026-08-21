<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="PETROCARDPurchase.aspx.cs" Inherits="PETROCARDPurchase" %>

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

        /* ══ Hero ══ */
        .map-hero {
            background: linear-gradient(135deg, #f0fdf4 0%, #fff7ed 55%, #fef9f0 100%);
            padding: 60px 20px 52px;
            text-align: center;
            position: relative;
            overflow: hidden;
            border-bottom: 1px solid var(--border);
        }

            .map-hero::before {
                content: '';
                position: absolute;
                inset: 0;
                background: radial-gradient(ellipse 70% 55% at 50% 0%, rgba(232,64,0,.08) 0%, transparent 65%);
            }

        .map-hero-inner {
            position: relative;
            z-index: 1;
            max-width: 700px;
            margin: auto;
        }

        .map-hero-badge {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            background: var(--og);
            border: 1px solid rgba(232,64,0,.28);
            color: var(--orange);
            font-size: .78rem;
            font-weight: 700;
            letter-spacing: .9px;
            text-transform: uppercase;
            padding: 6px 18px;
            border-radius: 50px;
            margin-bottom: 18px;
        }

        .map-badge-dot {
            width: 8px;
            height: 8px;
            background: var(--orange);
            border-radius: 50%;
            animation: blink 1.4s infinite;
        }

        @keyframes blink {
            0%,100% {
                opacity: 1
            }

            50% {
                opacity: .3
            }
        }

        .map-hero h1 {
            font-family: 'Sora', sans-serif;
            font-size: clamp(1.9rem,4.6vw,2.8rem);
            font-weight: 800;
            color: var(--text);
            line-height: 1.2;
            margin-bottom: 14px;
        }

            .map-hero h1 span {
                color: var(--orange);
            }

        .map-hero p {
            color: var(--muted);
            font-size: .98rem;
            line-height: 1.8;
            max-width: 560px;
            margin: 0 auto;
        }

        /* ══ Section ══ */
        .map-section {
            padding: 55px 20px 70px;
            background: #f9fafb;
        }

        .map-wrap {
            max-width: 1240px;
            margin: auto;
        }

        .map-section-tag {
            display: inline-block;
            background: var(--og);
            color: var(--orange);
            font-size: .73rem;
            font-weight: 700;
            letter-spacing: .8px;
            text-transform: uppercase;
            padding: 5px 14px;
            border-radius: 50px;
            margin-bottom: 10px;
        }

        .map-section-title {
            font-family: 'Sora', sans-serif;
            font-size: clamp(1.4rem,2.8vw,2rem);
            font-weight: 800;
            color: var(--text);
            margin-bottom: 8px;
        }

            .map-section-title span {
                color: var(--orange);
            }

        .map-section-sub {
            color: var(--muted);
            font-size: .93rem;
            line-height: 1.75;
            margin-bottom: 0;
        }

        /* ══ Head row + balance chip ══ */
        .map-head-row {
            display: flex;
            align-items: flex-end;
            justify-content: space-between;
            gap: 20px;
            flex-wrap: wrap;
            margin-bottom: 34px;
        }

        .map-head-left {
            flex: 1 1 420px;
            min-width: 0;
        }

        .map-bal-chip {
            flex: 0 0 auto;
            background: linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%);
            border: 1.5px solid rgba(22,163,74,.28);
            border-radius: 16px;
            padding: 14px 24px;
            text-align: right;
            box-shadow: 0 4px 18px rgba(22,163,74,.10);
        }

        .map-bal-label {
            display: block;
            font-size: .68rem;
            font-weight: 700;
            letter-spacing: .9px;
            text-transform: uppercase;
            color: var(--green);
            margin-bottom: 4px;
        }

        .map-bal-value {
            font-family: 'Sora', sans-serif;
            font-size: 1.55rem;
            font-weight: 800;
            color: var(--green);
            line-height: 1;
        }

        /* ══ KYC Gate ══ */
        .map-kyc {
            background: #fff;
            border: 1.5px solid rgba(232,64,0,.35);
            border-radius: 20px;
            box-shadow: 0 4px 24px rgba(0,0,0,.06);
            overflow: hidden;
            margin-bottom: 30px;
        }

            .map-kyc.rejected {
                border-color: rgba(220,38,38,.40);
            }

            .map-kyc.verified {
                border-color: rgba(22,163,74,.40);
            }

        .map-kyc-body {
            display: flex;
            align-items: center;
            gap: 18px;
            padding: 22px 26px;
            flex-wrap: wrap;
            background: linear-gradient(135deg, #fff7ed 0%, #ffedd5 100%);
        }

        .map-kyc.rejected .map-kyc-body {
            background: linear-gradient(135deg, #fef2f2 0%, #fee2e2 100%);
        }

        .map-kyc.verified .map-kyc-body {
            background: linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%);
        }

        .map-kyc-icon {
            width: 52px;
            height: 52px;
            border-radius: 50%;
            background: rgba(255,255,255,.75);
            border: 1.5px solid rgba(232,64,0,.28);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.4rem;
            flex-shrink: 0;
        }

        .map-kyc.rejected .map-kyc-icon {
            border-color: rgba(220,38,38,.30);
        }

        .map-kyc.verified .map-kyc-icon {
            border-color: rgba(22,163,74,.30);
        }

        .map-kyc-text {
            flex: 1 1 340px;
            min-width: 0;
        }

            .map-kyc-text h3 {
                font-family: 'Sora', sans-serif;
                font-size: 1.05rem;
                font-weight: 800;
                color: var(--text);
                margin: 0 0 5px;
            }

            .map-kyc-text p {
                font-size: .87rem;
                color: #4B5563;
                margin: 0;
                line-height: 1.65;
            }

        .map-kyc-action {
            margin-left: auto;
        }

        .map-kyc-btn {
            display: inline-block;
            background: var(--orange);
            color: #fff !important;
            font-family: 'Sora', sans-serif;
            font-weight: 700;
            font-size: .88rem;
            padding: 12px 30px;
            border-radius: 50px;
            text-decoration: none;
            white-space: nowrap;
            box-shadow: 0 6px 20px rgba(232,64,0,.28);
            transition: background .25s, transform .2s;
        }

            .map-kyc-btn:hover {
                background: #c73600;
                color: #fff !important;
                transform: translateY(-2px);
                text-decoration: none;
            }

        .map-kyc-chip {
            display: inline-block;
            font-family: 'Sora', sans-serif;
            font-size: .68rem;
            font-weight: 800;
            letter-spacing: .8px;
            text-transform: uppercase;
            padding: 7px 18px;
            border-radius: 50px;
            color: #fff;
            background: var(--green);
            white-space: nowrap;
        }

        /* ══ Cards grid ══ */
        .map-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
            gap: 26px;
        }

        .map-card {
            background: #fff;
            border-radius: 20px;
            border: 1.5px solid var(--border);
            overflow: hidden;
            display: flex;
            flex-direction: column;
            box-shadow: 0 4px 24px rgba(0,0,0,.06);
            transition: transform .28s, box-shadow .28s, border-color .28s;
            position: relative;
            opacity: 0;
            transform: translateY(24px);
        }

            .map-card.show {
                opacity: 1;
                transform: translateY(0);
                transition: opacity .5s ease, transform .5s ease, box-shadow .28s, border-color .28s;
            }

            .map-card:hover {
                transform: translateY(-7px);
                box-shadow: 0 16px 48px rgba(232,64,0,.13);
                border-color: rgba(232,64,0,.35);
            }

            .map-card.locked {
                opacity: .74;
            }

                .map-card.locked .map-card-img {
                    filter: grayscale(65%);
                }

            .map-card.purchased {
                border-color: rgba(22,163,74,.45);
                box-shadow: 0 8px 30px rgba(22,163,74,.14);
            }

                .map-card.purchased .map-card-header {
                    background: linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%);
                    border-bottom-color: rgba(22,163,74,.30);
                }

        .map-card-header {
            background: linear-gradient(135deg, #fff7ed 0%, #ffedd5 100%);
            border-bottom: 1.5px dashed rgba(232,64,0,.25);
            padding: 20px 24px;
            text-align: center;
            position: relative;
        }

        .map-card-img {
            width: 100%;
            max-width: 280px;
            height: auto;
            border-radius: 12px;
            display: block;
            margin: 0 auto 16px;
            box-shadow: 0 6px 18px rgba(0,0,0,.10);
        }

        .map-card-pkg-label {
            font-size: .72rem;
            font-weight: 700;
            letter-spacing: .8px;
            text-transform: uppercase;
            color: var(--muted);
            margin-bottom: 6px;
        }

        .map-card-pkg-amount {
            font-family: 'Sora', sans-serif;
            font-size: 2.2rem;
            font-weight: 800;
            color: var(--orange);
            line-height: 1;
        }

            .map-card-pkg-amount sup {
                font-size: 1rem;
                vertical-align: top;
                margin-top: 6px;
                display: inline-block;
                color: rgba(232,64,0,.7);
            }

        /* Notch */
        .map-card-notch {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin: 0 -1px;
        }

        .map-card-notch-circle {
            width: 18px;
            height: 18px;
            border-radius: 50%;
            background: #f9fafb;
            border: 1.5px solid var(--border);
            flex-shrink: 0;
        }

        .map-card-notch-line {
            flex: 1;
            border-top: 2px dashed #e2e8f0;
            margin: 0 4px;
        }

        /* Body */
        .map-card-body {
            padding: 20px 24px 24px;
            display: flex;
            flex-direction: column;
            flex: 1;
            gap: 12px;
        }

        .map-benefit-title {
            font-size: .72rem;
            font-weight: 700;
            letter-spacing: .8px;
            text-transform: uppercase;
            color: var(--muted);
            margin: 0;
        }

        .map-benefit-list {
            list-style: none;
            padding: 0;
            margin: 0;
            font-size: .85rem;
            color: #4B5563;
            line-height: 1.6;
        }

            .map-benefit-list li {
                position: relative;
                padding-left: 24px;
                margin-bottom: 9px;
            }

                .map-benefit-list li:before {
                    content: '\2713';
                    position: absolute;
                    left: 0;
                    top: 1px;
                    width: 16px;
                    height: 16px;
                    border-radius: 50%;
                    background: var(--gg);
                    color: var(--green);
                    font-size: 10px;
                    font-weight: 700;
                    line-height: 16px;
                    text-align: center;
                }

        /* Button */
        .map-card-btn {
            display: block;
            text-align: center;
            background: var(--orange);
            color: #fff !important;
            font-family: 'Sora', sans-serif;
            font-size: .88rem;
            font-weight: 700;
            padding: 13px 0;
            border-radius: 50px;
            text-decoration: none;
            box-shadow: 0 5px 18px rgba(232,64,0,.3);
            transition: background .25s, transform .2s, box-shadow .25s;
            margin-top: auto;
        }

            .map-card-btn:hover {
                background: #c73600;
                color: #fff !important;
                transform: translateY(-2px);
                box-shadow: 0 8px 24px rgba(232,64,0,.4);
                text-decoration: none;
            }

            .map-card-btn.done {
                background: var(--green);
                box-shadow: 0 5px 18px rgba(22,163,74,.30);
                cursor: default;
                pointer-events: none;
            }

        /* Tier / status badge */
        .map-tier-badge,
        .map-status-ribbon {
            position: absolute;
            top: 14px;
            right: 14px;
            color: #fff;
            font-family: 'Sora', sans-serif;
            font-size: .62rem;
            font-weight: 800;
            letter-spacing: .8px;
            text-transform: uppercase;
            padding: 5px 12px;
            border-radius: 50px;
            z-index: 3;
        }

        .map-card.silver .map-tier-badge {
            background: #7C8794;
        }

        .map-card.gold .map-tier-badge {
            background: #B37E1C;
        }

        .map-card.platinum .map-tier-badge {
            background: #3F3490;
        }

        .map-status-ribbon {
            background: var(--green);
        }

            .map-status-ribbon.grey {
                background: #94A3B8;
            }

        .map-tier-icon {
            position: absolute;
            top: 14px;
            left: 14px;
            width: 34px;
            height: 34px;
            border-radius: 50%;
            background: var(--og);
            border: 1.5px solid rgba(232,64,0,.25);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1rem;
            z-index: 3;
        }

        .map-state-note {
            font-size: .78rem;
            text-align: center;
            line-height: 1.55;
            margin: 0;
            color: var(--muted);
        }

            .map-state-note.ok {
                color: var(--green);
                font-weight: 600;
            }

        .map-alert {
            background: #fef2f2;
            border: 1px solid #fecaca;
            color: #b91c1c;
            border-radius: 12px;
            padding: 12px 18px;
            font-size: .88rem;
            font-weight: 600;
            margin-bottom: 22px;
        }

        /* ══ Responsive ══ */
        @media (max-width: 768px) {
            .map-grid {
                grid-template-columns: 1fr;
                max-width: 400px;
                margin: 0 auto;
            }
        }

        @media (max-width: 640px) {
            .map-head-row {
                flex-direction: column;
                align-items: stretch;
            }

            .map-bal-chip {
                text-align: center;
            }

            .map-kyc-action {
                margin-left: 0;
                width: 100%;
            }

            .map-kyc-btn {
                display: block;
                text-align: center;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- ═══ HERO ═══ -->
    <section class="map-hero">
        <div class="map-hero-inner">
            <div class="map-hero-badge">
                <div class="map-badge-dot"></div>
                ePay India &mdash; Petro Card
            </div>
            <h1>Petro <span>Card</span> Purchase</h1>
            <p>Buy any Petro Card Package and enjoy up to 200% benefits on your fuel spends. More refills, more savings &mdash; every single month.</p>
        </div>
    </section>

    <!-- ═══ KIT CARDS ═══ -->
    <section class="map-section">
        <div class="map-wrap">

            <asp:Panel ID="pnlError" runat="server" CssClass="map-alert" Visible="false">
                <asp:Label ID="Label2" runat="server"></asp:Label>
            </asp:Panel>

            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
            <asp:HiddenField ID="HdnWalletBalance" runat="server" />

            <!-- ── PAN KYC Gate ── -->
            <asp:Panel ID="pnlKyc" runat="server" CssClass="map-kyc" Visible="false">
                <div class="map-kyc-body">
                    <div class="map-kyc-icon">
                        <asp:Literal ID="litKycIcon" runat="server"></asp:Literal>
                    </div>

                    <div class="map-kyc-text">
                        <h3>
                            <asp:Literal ID="litKycTitle" runat="server"></asp:Literal></h3>
                        <p>
                            <asp:Literal ID="litKycMsg" runat="server"></asp:Literal></p>
                    </div>

                    <div class="map-kyc-action">
                        <asp:HyperLink ID="lnkKyc" runat="server" CssClass="map-kyc-btn"></asp:HyperLink>
                        <asp:Literal ID="litKycChip" runat="server" Visible="false"></asp:Literal>
                    </div>
                </div>
            </asp:Panel>

            <!-- ── Heading + Balance ── -->
            <div class="map-head-row">
                <div class="map-head-left">
                    <div class="map-section-tag">&#9981; Petro Card Package</div>
                    <h2 class="map-section-title">Choose Your <span>Petro Card Package</span></h2>
                    <p class="map-section-sub">Purchase any Package mentioned below and get up to 200% of the purchased amount as fuel benefits over 15 months.</p>
                </div>

                <div class="map-bal-chip">
                    <span class="map-bal-label">Available Balance</span>
                    <div class="map-bal-value">
                        <asp:Label ID="lblWalletBalance" runat="server" Text="Rs. 0.00"></asp:Label>
                    </div>
                </div>
            </div>

            <!-- ── Cards ── -->
            <div class="map-grid">
                <asp:Repeater ID="rptKitDetails" runat="server" OnItemDataBound="rptKitDetails_ItemDataBound">
                    <ItemTemplate>

                        <div id="divCard" runat="server" class="map-card" data-anim>

                            <div class="map-tier-icon"><%# Eval("Icon") %></div>

                            <asp:Panel ID="pnlTier" runat="server" CssClass="map-tier-badge">
                                <%# Eval("ThemeLabel") %>
                            </asp:Panel>

                            <asp:Panel ID="pnlStatus" runat="server" CssClass="map-status-ribbon" Visible="false">
                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                            </asp:Panel>

                            <asp:Label ID="LblKitid" runat="server" Text='<%# Eval("kitid") %>' Visible="false"></asp:Label>
                            <asp:TextBox ID="TxtAmount" runat="server" Visible="false" Text='<%# Eval("kitamount") %>'></asp:TextBox>

                            <div class="map-card-header">
                                <img src='<%# Eval("img") %>' alt='<%# Eval("kitdisplayname") %>' class="map-card-img" />
                                <div class="map-card-pkg-label">Package Amount</div>
                                <div class="map-card-pkg-amount">
                                    <sup>&#8377; </sup><%# Eval("kitamountdisp") %>
                                </div>
                            </div>

                            <div class="map-card-notch">
                                <div class="map-card-notch-circle"></div>
                                <div class="map-card-notch-line"></div>
                                <div class="map-card-notch-circle"></div>
                            </div>

                            <div class="map-card-body">

                                <div class="map-benefit-title">Key Benefits</div>
                                <ul class="map-benefit-list">
                                    <%# Eval("Benf") %>
                                </ul>

                                <asp:HyperLink ID="lnkBuy" runat="server" CssClass="map-card-btn"
                                    Text="Buy Now &#8594;"></asp:HyperLink>

                                <asp:Label ID="lblNote" runat="server" CssClass="map-state-note" Visible="false"></asp:Label>
                            </div>
                        </div>

                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </div>
    </section>

    <script type="text/javascript">

        // Buy Now — confirm + double click guard
        function PetroBuyNow(lnk) {
            if (lnk.getAttribute('data-clicked') === '1') { return false; }

            if (!confirm('Are you sure to proceed with this purchase?')) { return false; }

            lnk.setAttribute('data-clicked', '1');
            lnk.innerHTML = 'Processing...';
            lnk.style.opacity = '0.6';
            lnk.style.pointerEvents = 'none';
            return true;
        }

        // PAN verify nahi hai — KYC page par bhejo
        function PetroKycRequired(url) {
            if (confirm('PAN Verification Required\n\n'
                + 'PAN verification is mandatory before purchasing a Petro Card Package.\n\n'
                + 'Click OK to complete your PAN verification now.')) {
                window.location.href = url;
            }
            return false;
        }

        // Balance kam hai
        // function PetroLowBalance(kitAmt, bal, shortfall) {
        //     alert('Insufficient Balance!\n\n'
        //         + 'Kit Amount        : Rs. ' + kitAmt + '\n'
        //         + 'Available Balance : Rs. ' + bal + '\n'
        //         + 'Shortfall         : Rs. ' + shortfall + '\n\n'
        //         + 'Please add funds to your wallet and try again.');
        //     return false;
        // }

        // Doosra kit pehle se liya hua hai
        function PetroKitLocked() {
            alert('You have already purchased a Petro Card Package.\n\n'
                + 'Only one Petro Card Package can be purchased per member.');
            return false;
        }

        // Scroll reveal
        (function () {
            var observer = new IntersectionObserver(function (entries, obs) {
                entries.forEach(function (entry) {
                    if (entry.isIntersecting) {
                        entry.target.classList.add('show');
                        obs.unobserve(entry.target);
                    }
                });
            }, { threshold: 0.08 });

            document.querySelectorAll('[data-anim]').forEach(function (el, i) {
                el.style.transitionDelay = (i * 0.08) + 's';
                observer.observe(el);
            });
        })();

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
