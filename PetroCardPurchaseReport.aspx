<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="PetroCardPurchaseReport.aspx.cs" Inherits="PetroCardPurchaseReport" %>

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

        /* ── Hero ── */
        .map-hero {
            background: linear-gradient(135deg, #f0fdf4 0%, #fff7ed 55%, #fef9f0 100%);
            padding: 55px 20px 48px;
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
            font-size: clamp(1.7rem,4.2vw,2.6rem);
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
            font-size: .97rem;
            line-height: 1.8;
            max-width: 560px;
            margin: 0 auto;
        }

        /* ── Section ── */
        .map-section {
            padding: 50px 20px 70px;
            background: #f9fafb;
        }

        .map-wrap {
            max-width: 100%;
            margin: auto;
        }

        /* ── Panel ── */
        .map-panel {
            background: #fff;
            border: 1.5px solid var(--border);
            border-radius: 20px;
            box-shadow: 0 4px 24px rgba(0,0,0,.06);
            overflow: hidden;
        }

        .map-panel-head {
            background: linear-gradient(135deg, #fff7ed 0%, #ffedd5 100%);
            border-bottom: 1.5px dashed rgba(232,64,0,.25);
            padding: 16px 26px;
            display: flex;
            align-items: center;
            gap: 14px;
            flex-wrap: wrap;
        }

        .map-panel-icon {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            background: var(--og);
            border: 1.5px solid rgba(232,64,0,.25);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.05rem;
            flex-shrink: 0;
        }

        .map-panel-title {
            font-family: 'Sora', sans-serif;
            font-size: 1rem;
            font-weight: 800;
            color: var(--text);
            margin: 0;
        }

        .map-panel-sub {
            font-size: .76rem;
            color: var(--muted);
            margin: 2px 0 0;
        }

        .map-panel-tools {
            margin-left: auto;
            display: flex;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
        }

        .map-search {
            border: 1.5px solid var(--border);
            border-radius: 50px;
            padding: 9px 18px;
            font-size: .86rem;
            color: var(--text);
            background: #fff;
            min-width: 220px;
            transition: border-color .2s, box-shadow .2s;
        }

            .map-search:focus {
                outline: none;
                border-color: rgba(232,64,0,.55);
                box-shadow: 0 0 0 3px rgba(232,64,0,.10);
            }

        .map-btn-sm {
            background: #fff;
            color: var(--green);
            font-family: 'Sora', sans-serif;
            font-weight: 700;
            font-size: .8rem;
            padding: 9px 20px;
            border-radius: 50px;
            border: 1.5px solid rgba(22,163,74,.35);
            cursor: pointer;
            transition: background .25s, transform .2s;
        }

            .map-btn-sm:hover {
                background: #f0fdf4;
                transform: translateY(-2px);
            }

        .map-panel-body {
            padding: 22px 26px 26px;
        }

        /* ── Table ── */
        .map-table-wrap {
            overflow-x: auto;
            border: 1px solid var(--border);
            border-radius: 14px;
        }

        .map-table-wrap table {
            width: 100%;
            border-collapse: collapse;
            margin: 0;
            font-size: .86rem;
        }

            .map-table-wrap table th {
                background: linear-gradient(135deg, #fff7ed 0%, #ffedd5 100%);
                color: var(--text) !important;
                font-family: 'Sora', sans-serif;
                font-size: .72rem;
                font-weight: 800;
                letter-spacing: .6px;
                text-transform: uppercase;
                padding: 14px 16px;
                text-align: left;
                white-space: nowrap;
                border: none;
                border-bottom: 2px solid rgba(232,64,0,.20);
            }

            .map-table-wrap table td {
                padding: 13px 16px;
                color: #4B5563;
                border: none;
                border-bottom: 1px solid var(--border);
                white-space: nowrap;
                vertical-align: middle;
            }

            .map-table-wrap table tr:last-child td {
                border-bottom: none;
            }

            .map-table-wrap table tbody tr:nth-child(even) {
                background: #fbfcfd;
            }

            .map-table-wrap table tbody tr:hover {
                background: #fff7ed;
            }

        /* Pager */
        .map-table-wrap table tr.map-pager td,
        .map-table-wrap table tr td table td {
            border: none;
            padding: 6px;
            white-space: nowrap;
        }

        .map-table-wrap table tr.map-pager {
            background: #f9fafb !important;
        }

            .map-table-wrap table tr.map-pager a,
            .map-table-wrap table tr.map-pager span {
                display: inline-block;
                min-width: 32px;
                text-align: center;
                padding: 6px 10px;
                margin: 0 3px;
                border-radius: 8px;
                font-family: 'Sora', sans-serif;
                font-size: .8rem;
                font-weight: 700;
                text-decoration: none;
                border: 1px solid var(--border);
                background: #fff;
                color: var(--muted);
                transition: background .2s, color .2s, border-color .2s;
            }

                .map-table-wrap table tr.map-pager a:hover {
                    background: var(--og);
                    color: var(--orange);
                    border-color: rgba(232,64,0,.35);
                    text-decoration: none;
                }

            .map-table-wrap table tr.map-pager span {
                background: var(--orange);
                color: #fff;
                border-color: var(--orange);
            }

        /* ── Empty / message ── */
        .map-empty {
            text-align: center;
            padding: 55px 20px;
            color: var(--muted);
        }

            .map-empty .icon {
                font-size: 2.4rem;
                display: block;
                margin-bottom: 12px;
                opacity: .55;
            }

            .map-empty h4 {
                font-family: 'Sora', sans-serif;
                font-size: 1.05rem;
                font-weight: 800;
                color: var(--text);
                margin: 0 0 6px;
            }

            .map-empty p {
                font-size: .88rem;
                margin: 0 0 22px;
                line-height: 1.7;
            }

        .map-btn-p {
            display: inline-block;
            background: var(--orange);
            color: #fff !important;
            font-family: 'Sora', sans-serif;
            font-weight: 700;
            font-size: .88rem;
            padding: 12px 30px;
            border-radius: 50px;
            text-decoration: none;
            box-shadow: 0 6px 20px rgba(232,64,0,.28);
            transition: background .25s, transform .2s;
        }

            .map-btn-p:hover {
                background: #c73600;
                transform: translateY(-2px);
                text-decoration: none;
            }

        .map-alert {
            background: #fef2f2;
            border: 1px solid #fecaca;
            color: #b91c1c;
            border-radius: 12px;
            padding: 12px 18px;
            font-size: .88rem;
            font-weight: 600;
            margin-bottom: 20px;
        }

        @media (max-width: 640px) {
            .map-panel-tools {
                margin-left: 0;
                width: 100%;
            }

            .map-search {
                flex: 1;
                min-width: 0;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- ─── HERO ─── -->
<%--    <section class="map-hero">
        <div class="map-hero-inner">
            <div class="map-hero-badge">
                <div class="map-badge-dot"></div>
                ePay India &mdash; Reports
            </div>
            <h1>Petro Card <span>Purchase</span> Report</h1>
            <p>A complete record of your Petro Card Kit purchases with transaction details and status.</p>
        </div>
    </section>--%>

    <!-- ─── REPORT ─── -->
    <section class="map-section">
        <div class="map-wrap">

            <asp:Panel ID="pnlError" runat="server" CssClass="map-alert" Visible="false">
                <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
            </asp:Panel>

            <div class="map-panel">

                <div class="map-panel-head">
                    <div class="map-panel-icon">&#128203;</div>
                    <div>
                        <h3 class="map-panel-title">Purchase History</h3>
                        <p class="map-panel-sub">All your Petro Card Kit transactions</p>
                    </div>

                    <div class="map-panel-tools">
                        <input type="text" id="txtSearch" class="map-search"
                            placeholder="Search in report..." onkeyup="FilterReport(this.value);" />
                        <button type="button" class="map-btn-sm" onclick="ExportReport();">
                            &#8681; Export CSV
                        </button>
                    </div>
                </div>

                <div class="map-panel-body">
                    <div class="map-table-wrap" id="reportWrap">
                        <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="true"
                            AllowPaging="true" PageSize="10"
                            OnPageIndexChanging="RptDirects_PageIndexChanging"
                            GridLines="None" BorderStyle="None" CellPadding="0" CellSpacing="0"
                            Width="100%" PagerStyle-CssClass="map-pager"
                            EmptyDataText="">
                            <Columns>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="map-empty">
                                    <span class="icon">&#128663;</span>
                                    <h4>No Purchases Yet</h4>
                                    <p>You have not purchased any Petro Card Kit so far.</p>
                                    <a href="PETROCARDPurchase.aspx" class="map-btn-p">Browse Petro Card Kits</a>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>

            </div>
        </div>
    </section>

    <script type="text/javascript">

        // ── Client side search filter ──
        function FilterReport(term) {
            term = (term || '').toLowerCase();

            var wrap = document.getElementById('reportWrap');
            if (!wrap) { return; }

            var table = wrap.querySelector('table');
            if (!table) { return; }

            var rows = table.tBodies.length
                ? table.tBodies[0].rows
                : table.rows;

            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];

                // header aur pager row skip
                if (row.getElementsByTagName('th').length > 0) { continue; }
                if (row.className.indexOf('map-pager') > -1) { continue; }

                var text = (row.innerText || row.textContent || '').toLowerCase();
                row.style.display = (term === '' || text.indexOf(term) > -1) ? '' : 'none';
            }
        }

        // ── CSV export ──
        function ExportReport() {
            var wrap = document.getElementById('reportWrap');
            if (!wrap) { return; }

            var table = wrap.querySelector('table');
            if (!table) {
                alert('There is no data to export.');
                return;
            }

            var csv = [];

            for (var i = 0; i < table.rows.length; i++) {
                var row = table.rows[i];

                if (row.className.indexOf('map-pager') > -1) { continue; }
                if (row.style.display === 'none') { continue; }
                if (row.cells.length === 1 && row.cells[0].querySelector('table')) { continue; }

                var line = [];
                for (var j = 0; j < row.cells.length; j++) {
                    var val = (row.cells[j].innerText || row.cells[j].textContent || '')
                        .replace(/"/g, '""')
                        .replace(/\s+/g, ' ')
                        .trim();
                    line.push('"' + val + '"');
                }
                if (line.length > 0) { csv.push(line.join(',')); }
            }

            if (csv.length <= 1) {
                alert('There is no data to export.');
                return;
            }

            var blob = new Blob(['\uFEFF' + csv.join('\n')], { type: 'text/csv;charset=utf-8;' });
            var link = document.createElement('a');
            link.href = URL.createObjectURL(blob);
            link.download = 'PetroCardPurchaseReport_'
                + new Date().toISOString().slice(0, 10) + '.csv';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }

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