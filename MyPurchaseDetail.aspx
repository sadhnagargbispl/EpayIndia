<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="MyPurchaseDetail.aspx.cs" Inherits="MyPurchaseDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>
    * { box-sizing: border-box; }

    .pd-section {
        font-family: 'Segoe UI', Arial, sans-serif;
        padding: 28px 0 48px;
        background: #f4f6fb;
        min-height: 60vh;
    }

    .pd-container {
        max-width: 960px;
        margin: 0 auto;
        padding: 0 18px;
    }

    /* ── Page header ── */
    .pd-page-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-bottom: 20px;
        flex-wrap: wrap;
        gap: 10px;
    }
    .pd-page-title {
        display: flex;
        align-items: center;
        gap: 10px;
    }
    .pd-page-title-icon {
        width: 38px;
        height: 38px;
        background: #1b2e6b;
        border-radius: 9px;
        display: flex;
        align-items: center;
        justify-content: center;
        flex-shrink: 0;
    }
    .pd-page-title-icon svg { width: 18px; height: 18px; }
    .pd-page-title h2 {
        font-size: 17px;
        font-weight: 600;
        color: #1b2e6b;
        margin: 0;
    }
    .pd-page-title p {
        font-size: 12px;
        color: #8b96b4;
        margin: 2px 0 0;
    }

    .pd-back-btn {
        display: inline-flex;
        align-items: center;
        gap: 6px;
        background: #fff;
        color: #1b2e6b;
        border: 1px solid #d0d9ee;
        border-radius: 7px;
        padding: 8px 14px;
        font-size: 12.5px;
        font-weight: 500;
        font-family: inherit;
        cursor: pointer;
        text-decoration: none;
        transition: background 0.15s;
    }
    .pd-back-btn:hover { background: #eef2ff; }
    .pd-back-btn svg { width: 14px; height: 14px; }

    /* ── Card ── */
    .pd-card {
        background: #fff;
        border-radius: 12px;
        border: 1px solid #dde2ef;
        overflow: hidden;
    }

    .pd-card-head {
        padding: 14px 20px;
        border-bottom: 1px solid #eef0f6;
        display: flex;
        align-items: center;
        justify-content: space-between;
        flex-wrap: wrap;
        gap: 10px;
    }
    .pd-card-head-left {
        display: flex;
        align-items: center;
        gap: 8px;
    }
    .pd-card-head-left svg { width: 15px; height: 15px; color: #8b96b4; }
    .pd-card-head-left span {
        font-size: 12px;
        font-weight: 700;
        color: #3a4560;
        text-transform: uppercase;
        letter-spacing: 0.5px;
    }
    .pd-total-badge {
        background: #eef2ff;
        color: #1b2e6b;
        font-size: 11.5px;
        font-weight: 600;
        padding: 4px 12px;
        border-radius: 20px;
    }

    /* ── Error label ── */
    .pd-error {
        display: block;
        text-align: center;
        padding: 10px 20px;
        font-size: 13px;
        color: #c0392b;
    }

    /* ── Table ── */
    .pd-table-wrap {
        overflow-x: auto;
        -webkit-overflow-scrolling: touch;
    }

    .pd-table {
        width: 100%;
        border-collapse: collapse;
        font-size: 13.5px;
        min-width: 520px;
    }

    .pd-table thead tr {
        background: #f7f8fc;
        border-bottom: 2px solid #e3e7f0;
    }

    .pd-table thead th {
        padding: 12px 16px;
        text-align: left;
        font-size: 10.5px;
        font-weight: 700;
        color: #8b96b4;
        text-transform: uppercase;
        letter-spacing: 0.6px;
        white-space: nowrap;
    }

    .pd-table tbody tr {
        border-bottom: 1px solid #eef0f6;
        transition: background 0.12s;
    }
    .pd-table tbody tr:last-child { border-bottom: none; }
    .pd-table tbody tr:hover { background: #fafbfe; }

    .pd-table tbody td {
        padding: 13px 16px;
        color: #1e2a45;
        vertical-align: middle;
    }

    .pd-sno {
        font-size: 12px;
        font-weight: 600;
        color: #9aa3be;
        width: 48px;
    }

    .pd-order-no {
        font-weight: 600;
        color: #1b2e6b;
    }

    .pd-amount {
        font-weight: 600;
        color: #1e2a45;
    }

    .pd-package-badge {
        display: inline-flex;
        align-items: center;
        gap: 5px;
        background: #eef2ff;
        color: #1b2e6b;
        font-size: 12px;
        font-weight: 500;
        padding: 4px 10px;
        border-radius: 6px;
        white-space: nowrap;
    }
    .pd-package-badge svg { width: 12px; height: 12px; }

    .pd-date {
        font-size: 13px;
        color: #5a6480;
    }

    /* ── Empty state ── */
    .pd-empty {
        text-align: center;
        padding: 48px 20px;
        color: #9aa3be;
    }
    .pd-empty svg {
        width: 40px;
        height: 40px;
        margin-bottom: 12px;
        opacity: 0.5;
    }
    .pd-empty p { font-size: 14px; }

    /* ── Pagination override ── */
    .pd-card .pagination {
        margin: 12px 16px;
        justify-content: center;
        display: flex;
        gap: 4px;
        list-style: none;
        padding: 0;
        flex-wrap: wrap;
    }
    .pd-card .pagination li a,
    .pd-card .pagination li span {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        min-width: 32px;
        height: 32px;
        padding: 0 8px;
        border-radius: 6px;
        border: 1px solid #dde2ef;
        background: #fff;
        color: #1b2e6b;
        font-size: 13px;
        font-weight: 500;
        text-decoration: none;
        cursor: pointer;
        transition: background 0.13s;
    }
    .pd-card .pagination li a:hover { background: #eef2ff; }
    .pd-card .pagination li.active span,
    .pd-card .pagination li.active a {
        background: #1b2e6b;
        color: #fff;
        border-color: #1b2e6b;
    }
    .pd-card .pagination li.disabled span {
        opacity: 0.4;
        cursor: default;
    }

    /* GridView pagination row */
    .pd-table tr.pagination-row td {
        padding: 12px 16px;
        border-bottom: none;
        border-top: 1px solid #eef0f6;
        background: #fafbfe;
    }
    .pd-table tr.pagination-row td table {
        margin: 0 auto;
    }
    .pd-table tr.pagination-row td table td a,
    .pd-table tr.pagination-row td table td span {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        min-width: 30px;
        height: 30px;
        padding: 0 8px;
        border-radius: 6px;
        border: 1px solid #dde2ef;
        background: #fff;
        color: #1b2e6b;
        font-size: 12.5px;
        font-weight: 500;
        text-decoration: none;
        margin: 0 2px;
        transition: background 0.13s;
    }
    .pd-table tr.pagination-row td table td a:hover { background: #eef2ff; }
    .pd-table tr.pagination-row td table td span {
        background: #1b2e6b;
        color: #fff;
        border-color: #1b2e6b;
    }

    /* GridView default table fix */
    .pd-table-gv {
        width: 100%;
        border-collapse: collapse;
    }
    .pd-table-gv th {
        background: #f7f8fc;
        padding: 12px 16px;
        font-size: 10.5px;
        font-weight: 700;
        color: #8b96b4;
        text-transform: uppercase;
        letter-spacing: 0.6px;
        border-bottom: 2px solid #e3e7f0;
        text-align: left;
        white-space: nowrap;
    }
    .pd-table-gv td {
        padding: 13px 16px;
        font-size: 13.5px;
        color: #1e2a45;
        border-bottom: 1px solid #eef0f6;
        vertical-align: middle;
    }
    .pd-table-gv tr:last-child td { border-bottom: none; }
    .pd-table-gv tr:hover td { background: #fafbfe; }
    .pd-table-gv tr.GridHeader th { background: #f7f8fc; }
    .pd-table-gv tr.GridAltRow td { background: #fafbfe; }
    .pd-table-gv tr.GridAltRow:hover td { background: #f4f6fb; }

    @media (max-width: 600px) {
        .pd-page-header { flex-direction: column; align-items: flex-start; }
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<link rel="stylesheet" href="plugins/datatables/dataTables.bootstrap.css">

<div class="pd-section">
    <div class="pd-container">

        <!-- Page header -->
        <div class="pd-page-header">
            <div class="pd-page-title">
                <div class="pd-page-title-icon">
                    <svg viewBox="0 0 24 24" fill="none" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                        <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/>
                        <polyline points="14 2 14 8 20 8"/>
                        <line x1="16" y1="13" x2="8" y2="13"/>
                        <line x1="16" y1="17" x2="8" y2="17"/>
                    </svg>
                </div>
                <div>
                    <h2>Coupon Purchase Detail</h2>
                    <p>All your purchased packages in one place</p>
                </div>
            </div>
            <a href="javascript:history.back();" class="pd-back-btn">
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                    <polyline points="15 18 9 12 15 6"/>
                </svg>
                Go Back
            </a>
        </div>

        <!-- Main card -->
        <div class="pd-card">
            <div class="pd-card-head">
                <div class="pd-card-head-left">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                        <line x1="8" y1="6" x2="21" y2="6"/><line x1="8" y1="12" x2="21" y2="12"/>
                        <line x1="8" y1="18" x2="21" y2="18"/>
                        <line x1="3" y1="6" x2="3.01" y2="6"/><line x1="3" y1="12" x2="3.01" y2="12"/>
                        <line x1="3" y1="18" x2="3.01" y2="18"/>
                    </svg>
                    <span>Coupon Purchase Records</span>
                </div>
                <span class="pd-total-badge" id="spanRecordCount"></span>
            </div>

            <asp:Label ID="Label2" runat="server" CssClass="pd-error"></asp:Label>

            <div class="pd-table-wrap">
                <asp:GridView ID="gv" runat="server"
                    CssClass="pd-table-gv"
                    AutoGenerateColumns="true"
                    ShowHeaderWhenEmpty="true"
                    AllowPaging="true"
                    PageSize="15"
                    OnPageIndexChanging="gv_PageIndexChanging"
                    GridLines="None"
                    HeaderStyle-CssClass="GridHeader"
                    AlternatingRowStyle-CssClass="GridAltRow"
                    EmptyDataText="No purchase records found.">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No.">
                            <ItemTemplate>
                                <span style="font-size:12px;font-weight:600;color:#9aa3be;"><%# Container.DataItemIndex + 1 %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="pagination-row" HorizontalAlign="Center" />
                    <EmptyDataRowStyle CssClass="pd-empty-row" />
                </asp:GridView>
            </div>
        </div>

    </div>
</div>

<script>
    // Row count badge
    (function () {
        var gv = document.querySelector('.pd-table-gv');
        var badge = document.getElementById('spanRecordCount');
        if (gv && badge) {
            var rows = gv.querySelectorAll('tbody tr');
            var count = 0;
            rows.forEach(function (r) {
                if (!r.querySelector('td[colspan]')) count++;
            });
            if (count > 0) badge.textContent = count + ' record' + (count !== 1 ? 's' : '');
        }
    })();

    // ── User Dropdown ──
    var userDropdownBtn = document.getElementById('userDropdownBtn');
    var userDropdown = document.getElementById('userDropdown');
    var userDropdownWrap = document.getElementById('userDropdownWrap');
    if (userDropdownBtn && userDropdown && userDropdownWrap) {
        userDropdownBtn.addEventListener('click', function (e) {
            e.stopPropagation();
            userDropdown.classList.toggle('open');
        });
        document.addEventListener('click', function (e) {
            if (!userDropdownWrap.contains(e.target)) userDropdown.classList.remove('open');
        });
    }

    // ── Mobile Nav ──
    var hamburgerBtn = document.getElementById('hamburgerBtn');
    var mobileNav = document.getElementById('mobileNav');
    if (hamburgerBtn && mobileNav) {
        hamburgerBtn.addEventListener('click', function (e) {
            e.stopPropagation();
            var isOpen = mobileNav.classList.toggle('open');
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
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') {
                mobileNav.classList.remove('open');
                hamburgerBtn.classList.remove('open');
                hamburgerBtn.setAttribute('aria-expanded', 'false');
            }
        });
    }

    // ── Category Scroll ──
    function scrollCats(dir) {
        var el = document.getElementById('catsScroll');
        if (el) el.scrollBy({ left: dir * 240, behavior: 'smooth' });
    }

    // ── Scroll Animation ──
    var observer = new IntersectionObserver(function (entries, obs) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.classList.add('show');
                obs.unobserve(entry.target);
            }
        });
    }, { threshold: 0.08 });
    document.querySelectorAll('.service-card,.cat-card,.about-feat,.why-feat,[data-anim]').forEach(function (el, i) {
        el.style.transitionDelay = (i * 0.08) + 's';
        observer.observe(el);
    });
</script>
</asp:Content>
