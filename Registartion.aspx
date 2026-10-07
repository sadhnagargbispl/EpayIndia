<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Registartion.aspx.cs" Inherits="Registration" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

<style>
    /* Registration wizard - coupon theme (merged). Link from head: <link rel="stylesheet" href="css/registration.css" /> */

/* ---------- Tokens ---------- */
.reg-wizard {
    --orange: #E84000;
    --og: rgba(232,64,0,.10);
    --green: #16a34a;
    --gg: rgba(22,163,74,.10);

    --rw-primary: #E84000;
    --rw-primary-dark: #c73600;
    --rw-ink: #1A1A2E;
    --rw-muted: #6B7280;
    --rw-line: #E2E8F0;
    --rw-surface: #fff;
    --rw-subtle: #f9fafb;
    --rw-field: #fff;
    --rw-ok: #16a34a;
    --rw-ok-soft: #f0fdf4;
    --rw-warn: #b45309;
    --rw-warn-soft: #fffbeb;
    --rw-danger: #dc2626;
    --rw-danger-soft: #fef2f2;
    --rw-radius: 16px;
}

[data-theme-version="dark"] .reg-wizard {
    --rw-ink: #e8eaf0;
    --rw-muted: #9aa3b2;
    --rw-line: rgba(255,255,255,.12);
    --rw-surface: #2D2D37;
    --rw-subtle: #262630;
    --rw-field: #1E1E25;
    --rw-ok-soft: rgba(22,163,74,.14);
    --rw-warn-soft: rgba(180,83,9,.16);
    --rw-danger-soft: rgba(220,38,38,.14);
}

/* ---------- Base / box model ---------- */
.reg-wizard,
.reg-wizard *,
.reg-wizard *::before,
.reg-wizard *::after {
    box-sizing: border-box;
}

.reg-wizard {
    font-family: 'Sora', 'Segoe UI', sans-serif;
}

    .reg-wizard small {
        display: block;
    }

/* ---------- Hero ---------- */
.rw-hero {
    background: linear-gradient(135deg, #f0fdf4 0%, #fff7ed 55%, #fef9f0 100%);
    padding: 44px 20px 40px;
    text-align: center;
    position: relative;
    overflow: hidden;
    border: 1px solid var(--rw-line);
    border-radius: var(--rw-radius);
    margin-bottom: 24px;
}

    .rw-hero::before {
        content: '';
        position: absolute;
        inset: 0;
        background: radial-gradient(ellipse 70% 55% at 50% 0%, rgba(22,163,74,.07) 0%, transparent 65%);
    }

.rw-hero-inner {
    position: relative;
    z-index: 1;
    max-width: 640px;
    margin: auto;
}

.rw-hero-badge {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    background: var(--og);
    border: 1px solid rgba(232,64,0,.28);
    color: var(--orange);
    font-size: .75rem;
    font-weight: 700;
    letter-spacing: .9px;
    text-transform: uppercase;
    padding: 6px 18px;
    border-radius: 50px;
    margin-bottom: 16px;
}

.rw-badge-dot {
    width: 8px;
    height: 8px;
    background: var(--orange);
    border-radius: 50%;
    animation: rwblink 1.4s infinite;
}

@keyframes rwblink {
    0%, 100% { opacity: 1 }
    50% { opacity: .3 }
}

.rw-hero h1 {
    font-family: 'Sora', sans-serif;
    font-size: clamp(1.6rem, 4vw, 2.4rem);
    font-weight: 800;
    color: var(--rw-ink);
    line-height: 1.25;
    margin: 0 0 10px;
}

    .rw-hero h1 span {
        color: var(--orange);
    }

.rw-hero p {
    color: var(--rw-muted);
    font-size: .95rem;
    line-height: 1.75;
    max-width: 500px;
    margin: 0 auto;
}

/* ---------- Shell ---------- */
.rw-shell {
    display: grid;
    grid-template-columns: 340px minmax(0, 1fr);
    background: var(--rw-surface);
    border: 1px solid var(--rw-line);
    border-radius: var(--rw-radius);
    overflow: hidden;
    box-shadow: 0 4px 24px rgba(0,0,0,.07);
}

/* ---------- Brand panel (light, coupon-left jaisa) ---------- */
.rw-brand {
    background: linear-gradient(160deg, #fff7ed 0%, #ffedd5 55%, #f0fdf4 130%);
    color: var(--rw-ink);
    padding: 32px 30px;
    display: flex;
    flex-direction: column;
    border-right: 1px solid var(--rw-line);
}

.rw-mark {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-bottom: 30px;
}

.rw-mark-i {
    width: 32px;
    height: 32px;
    border-radius: 9px;
    background: var(--orange);
    box-shadow: 0 5px 16px rgba(232,64,0,.3);
    display: flex;
    align-items: center;
    justify-content: center;
    flex: 0 0 32px;
}

.rw-mark b {
    font-family: 'Sora', sans-serif;
    font-size: 14.5px;
    font-weight: 800;
    letter-spacing: -.01em;
    color: var(--rw-ink);
}

.rw-brand-h {
    font-family: 'Sora', sans-serif;
    font-size: 21px;
    font-weight: 800;
    margin: 0 0 9px;
    letter-spacing: -.015em;
    line-height: 1.3;
    color: var(--rw-ink);
}

.rw-brand-p {
    font-size: 12.5px;
    color: var(--rw-muted);
    margin: 0 0 28px;
    line-height: 1.65;
}

.rw-brand-foot {
    margin-top: auto;
    padding-top: 28px;
    font-size: 11.5px;
    color: var(--rw-muted);
    line-height: 1.65;
}

/* ---------- Steps ---------- */
.rw-steps {
    list-style: none;
    margin: 0;
    padding: 0;
}

.reg-step {
    position: relative;
    display: flex;
    gap: 13px;
    align-items: flex-start;
    padding-bottom: 22px;
}

    .reg-step:last-child {
        padding-bottom: 0;
    }

    /* Connector: is step se agle dot tak */
    .reg-step::before {
        content: "";
        position: absolute;
        left: 13px;
        top: 30px;
        bottom: 2px;
        width: 2px;
        background: #d1d5db;
    }

    .reg-step:last-child::before {
        display: none;
    }

    .reg-step.done::before {
        background: var(--green);
    }

    .reg-step.skip::before {
        background: #f59e0b;
    }

.rw-dot {
    width: 28px;
    height: 28px;
    flex: 0 0 28px;
    border-radius: 50%;
    border: 1.5px solid #d1d5db;
    background: #fff;
    color: var(--rw-muted);
    font-size: 12px;
    font-weight: 700;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all .2s ease;
}

    .rw-dot .tick,
    .rw-dot .mns {
        display: none;
        font-size: 11px;
    }

.rw-st {
    font-size: 13.5px;
    font-weight: 600;
    color: var(--rw-muted);
    line-height: 1.35;
    padding-top: 4px;
}

.rw-sn {
    display: block;
    font-size: 11px;
    color: #9ca3af;
    margin-top: 2px;
    font-weight: 400;
}

/* current = orange */
.reg-step.now .rw-dot {
    background: var(--orange);
    border-color: var(--orange);
    color: #fff;
    box-shadow: 0 0 0 4px var(--og);
}

.reg-step.now .rw-st {
    color: var(--rw-ink);
    font-weight: 700;
}

.reg-step.now .rw-sn {
    color: var(--orange);
}

/* completed = green */
.reg-step.done .rw-dot {
    background: var(--green);
    border-color: var(--green);
    color: #fff;
}

    .reg-step.done .rw-dot .num {
        display: none;
    }

    .reg-step.done .rw-dot .tick {
        display: block;
    }

.reg-step.done .rw-st {
    color: var(--green);
}

/* skipped = amber */
.reg-step.skip .rw-dot {
    background: var(--rw-warn-soft);
    border-color: #f59e0b;
    color: var(--rw-warn);
}

    .reg-step.skip .rw-dot .num {
        display: none;
    }

    .reg-step.skip .rw-dot .mns {
        display: block;
    }

.reg-step.skip .rw-st {
    color: var(--rw-warn);
}

/* Tablet / phone: panel upar ki patli strip ban jaata hai */
@media (max-width: 991.98px) {
    .rw-shell {
        grid-template-columns: 1fr;
    }

    .rw-brand {
        padding: 20px 22px 22px;
        border-right: 0;
        border-bottom: 1px solid var(--rw-line);
    }

    .rw-brand-h,
    .rw-brand-p,
    .rw-brand-foot {
        display: none;
    }

    .rw-mark {
        margin-bottom: 20px;
    }

    .rw-steps {
        display: flex;
    }

    .reg-step {
        flex: 1 1 0;
        display: block;
        text-align: center;
        padding: 0 3px;
        min-width: 0;
    }

        .reg-step::before {
            left: calc(50% + 14px);
            right: calc(-50% + 14px);
            top: 13px;
            bottom: auto;
            width: auto;
            height: 2px;
        }

    .rw-dot {
        margin: 0 auto 7px;
    }

    .rw-st {
        font-size: 10.5px;
        padding-top: 0;
        line-height: 1.3;
    }

    .rw-sn {
        display: none;
    }

    .rw-hero {
        padding: 30px 16px 26px;
    }
}

/* ---------- Form column ---------- */
.rw-main {
    position: relative;
    display: flex;
    flex-direction: column;
    min-width: 0;
}

    /* Top stripe (coupon-stripe) */
    .rw-main::before {
        content: '';
        display: block;
        height: 6px;
        background: linear-gradient(90deg, var(--orange), #ff8c42);
    }

.rw-head {
    padding: 28px 34px 0;
}

.rw-eyebrow {
    display: inline-block;
    font-size: 10.5px;
    font-weight: 700;
    letter-spacing: .8px;
    text-transform: uppercase;
    background: var(--og);
    color: var(--orange);
    padding: 5px 14px;
    border-radius: 50px;
    margin: 0 0 10px;
}

.rw-title {
    font-family: 'Sora', sans-serif;
    font-size: 1.5rem;
    font-weight: 800;
    color: var(--rw-ink);
    margin: 0;
    line-height: 1.3;
    letter-spacing: -.012em;
}

.rw-sub {
    font-size: 13.5px;
    color: var(--rw-muted);
    margin: 7px 0 0;
    line-height: 1.6;
    max-width: 62ch;
}

.rw-body {
    padding: 26px 34px 30px;
    flex: 1 1 auto;
}

.rw-foot {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 12px;
    flex-wrap: wrap;
    padding: 18px 34px 26px;
    border-top: 1px solid var(--rw-line);
    background: var(--rw-subtle);
}

.rw-foot-right {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-left: auto;
    flex-wrap: wrap;
}

.rw-foot-note {
    flex: 1 1 100%;
    order: 3;
    font-size: 12px;
    color: var(--rw-muted);
    line-height: 1.55;
    margin: 2px 0 0;
}

@media (max-width: 575.98px) {
    .rw-head {
        padding: 24px 20px 0;
    }

    .rw-body {
        padding: 20px 20px 24px;
    }

    .rw-foot {
        padding: 16px 20px 22px;
    }

    .rw-foot-right {
        width: 100%;
    }

        .rw-foot-right .btn {
            flex: 1 1 auto;
        }
}

/* ---------- Grid (Bootstrap-free) ---------- */
.reg-wizard .row {
    display: grid;
    grid-template-columns: repeat(12, minmax(0, 1fr));
    gap: 16px;
    margin: 0;
}

    .reg-wizard .row.g-2 {
        gap: 8px;
    }

    .reg-wizard .row.g-3 {
        gap: 18px 16px;
    }

.reg-wizard .col-12 { grid-column: span 12; }
.reg-wizard .col-8 { grid-column: span 8; }
.reg-wizard .col-6 { grid-column: span 6; }
.reg-wizard .col-4 { grid-column: span 4; }
.reg-wizard .col-md-7 { grid-column: span 7; }
.reg-wizard .col-md-6 { grid-column: span 6; }
.reg-wizard .col-md-5 { grid-column: span 5; }

@media (max-width: 767.98px) {
    .reg-wizard .col-md-7,
    .reg-wizard .col-md-6,
    .reg-wizard .col-md-5 {
        grid-column: span 12;
    }
}

.reg-wizard .d-flex { display: flex; }
.reg-wizard .align-items-end { align-items: flex-end; }
.reg-wizard .align-items-center { align-items: center; }

/* ---------- Buttons (coupon-btn jaise pill) ---------- */
.reg-wizard .btn {
    display: inline-block;
    cursor: pointer;
    text-align: center;
    text-decoration: none;
    font-family: 'Sora', sans-serif;
    font-weight: 700;
    font-size: .84rem;
    line-height: 1.4;
    padding: 11px 30px;
    border-radius: 50px;
    border: 1.5px solid transparent;
    transition: transform .2s, box-shadow .2s, background .2s;
}

.reg-wizard .btn-rw-primary {
    background: var(--orange);
    border-color: var(--orange);
    color: #fff;
    box-shadow: 0 5px 16px rgba(232,64,0,.3);
}

    .reg-wizard .btn-rw-primary:hover {
        background: var(--rw-primary-dark);
        border-color: var(--rw-primary-dark);
        color: #fff;
        transform: translateY(-2px);
        box-shadow: 0 8px 22px rgba(232,64,0,.35);
    }

.reg-wizard .btn-rw-ghost {
    background: #fff;
    border-color: var(--rw-line);
    color: var(--rw-muted);
}

    .reg-wizard .btn-rw-ghost:hover {
        border-color: var(--green);
        color: var(--green);
        background: #f0fdf4;
    }

.reg-wizard .btn-rw-skip {
    background: #fff;
    border-color: rgba(245,158,11,.5);
    color: var(--rw-warn);
}

    .reg-wizard .btn-rw-skip:hover {
        background: var(--rw-warn-soft);
        border-color: var(--rw-warn);
        color: var(--rw-warn);
    }

.reg-wizard .btn-rw-ok {
    background: var(--green);
    border-color: var(--green);
    color: #fff;
    box-shadow: 0 5px 16px rgba(22,163,74,.3);
}

    .reg-wizard .btn-rw-ok:hover {
        background: #15803d;
        border-color: #15803d;
        color: #fff;
        transform: translateY(-2px);
        box-shadow: 0 8px 22px rgba(22,163,74,.35);
    }

.reg-wizard .btn[disabled] {
    opacity: .55;
    cursor: not-allowed;
    transform: none;
    box-shadow: none;
}

/* ---------- Fields ---------- */
.reg-wizard .form-label {
    display: block;
    font-size: 12.5px;
    font-weight: 600;
    color: var(--rw-ink);
    margin-bottom: 6px;
}

.reg-wizard .rw-main .form-control {
    display: block;
    width: 100%;
    height: 44px;
    padding: 10px 14px;
    font-size: 14px;
    font-family: inherit;
    line-height: 1.4;
    background-color: var(--rw-field);
    border: 1.5px solid var(--rw-line);
    border-radius: 12px;
    color: var(--rw-ink);
    outline: none;
    transition: border-color .2s, box-shadow .2s;
}

    .reg-wizard .rw-main .form-control::placeholder {
        color: #a3acb9;
    }

    .reg-wizard .rw-main .form-control:focus {
        border-color: var(--orange);
        box-shadow: 0 0 0 3px var(--og);
    }

    .reg-wizard .rw-main .form-control:disabled {
        opacity: .75;
        cursor: not-allowed;
    }

.reg-wizard .rw-main textarea.form-control {
    height: auto;
}

.rw-req {
    color: var(--rw-danger);
    margin-left: 2px;
}

.rw-hint {
    display: block;
    font-size: 11.5px;
    color: var(--rw-muted);
    margin-top: 6px;
    line-height: 1.55;
}

.rw-name-hint {
    display: block;
    font-size: 12.5px;
    font-weight: 600;
    color: var(--green);
    margin-top: 6px;
}

.rw-section-title {
    font-size: 11px;
    font-weight: 700;
    letter-spacing: .09em;
    text-transform: uppercase;
    color: var(--orange);
    margin: 0 0 15px;
}

.rw-otp input {
    font-size: 19px;
    letter-spacing: .4em;
    text-align: center;
    font-weight: 600;
}

/* Radio list (Leg etc.): table ko input-box jaisa nahi, clean pills */
.reg-wizard table.form-control {
    height: auto;
    border: 0 !important;
    background: transparent !important;
    padding: 0 !important;
    box-shadow: none !important;
}

.reg-wizard input[type=radio] {
    margin-right: 4px;
    accent-color: var(--orange);
}

.reg-wizard td label {
    margin: 0 10px 0 0;
    padding: 8px 16px;
    cursor: pointer;
    border: 1.5px solid var(--rw-line);
    border-radius: 50px;
    background: #fff;
    font-size: 13px;
    font-weight: 600;
}

.reg-wizard input[type=radio]:checked + label {
    border-color: var(--orange);
    background: var(--og);
    color: var(--orange);
}

/* Mobile: +91 prefix + number */
.reg-wizard .input-group {
    display: flex;
    width: 100%;
}

    .reg-wizard .input-group .form-control {
        flex: 1 1 auto;
        width: 1%;
    }

.reg-wizard .rw-main .form-control.rw-code {
    flex: 0 0 76px;
    width: 76px;
    max-width: 76px;
    background: #fff7ed;
    color: var(--orange);
    font-weight: 700;
    text-align: center;
    border-right: 0;
    border-radius: 12px 0 0 12px;
}

.reg-wizard .input-group .form-control:last-child {
    border-radius: 0 12px 12px 0;
}

/* ---------- Consent / terms ---------- */
.rw-consent {
    display: flex;
    align-items: flex-start;
    gap: 11px;
    padding: 13px 15px;
    border: 1px solid rgba(232,64,0,.2);
    border-radius: 12px;
    background: #fff7ed;
}

    .rw-consent input[type=checkbox] {
        width: 17px;
        height: 17px;
        margin: 1px 0 0;
        flex: 0 0 auto;
        accent-color: var(--orange);
        cursor: pointer;
    }

    .rw-consent label {
        font-size: 13px;
        font-weight: 400;
        color: var(--rw-ink);
        line-height: 1.55;
        margin: 0;
        cursor: pointer;
    }

.rw-consent-lg {
    padding: 16px 18px;
}

    .rw-consent-lg label {
        font-size: 14px;
    }

/* ---------- Alerts ---------- */
.rw-alert {
    display: block;
    padding: 11px 14px;
    border-radius: 12px;
    font-size: 13px;
    line-height: 1.55;
    border: 1px solid transparent;
    margin-top: 16px;
}

    /* Khaali ASP.NET Label ka span hide */
    .rw-alert:empty {
        display: none;
    }

.rw-alert-ok {
    background: var(--rw-ok-soft);
    border-color: rgba(22,163,74,.28);
    color: var(--rw-ok);
}

.rw-alert-error {
    background: var(--rw-danger-soft);
    border-color: rgba(220,38,38,.25);
    color: var(--rw-danger);
}

.rw-alert-info {
    background: var(--rw-subtle);
    border-color: var(--rw-line);
    color: var(--rw-muted);
}

.rw-alert a {
    color: inherit;
    text-decoration: underline;
}

.rw-inline-note {
    display: block;
    font-size: 12px;
    font-weight: 600;
    margin-top: 6px;
}

/* ---------- Verified result ---------- */
.rw-result {
    border: 1px solid rgba(22,163,74,.28);
    background: linear-gradient(160deg, #f0fdf4, #dcfce7);
    border-radius: 16px;
    padding: 16px 18px;
    margin-top: 22px;
}

.rw-result-head {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 13px;
    font-weight: 700;
    color: var(--rw-ok);
    margin-bottom: 12px;
}

.rw-kv {
    width: 100%;
    font-size: 13px;
    border-collapse: collapse;
}

    .rw-kv td {
        padding: 8px 0;
        border-bottom: 1px solid var(--rw-line);
        vertical-align: top;
        color: var(--rw-ink);
        word-break: break-word;
    }

        .rw-kv td:first-child {
            width: 36%;
            color: var(--rw-muted);
            padding-right: 14px;
        }

    .rw-kv tr:last-child td {
        border-bottom: none;
    }

.rw-result .rw-kv td {
    border-bottom-color: rgba(22,163,74,.2);
}

.rw-review .rw-kv td {
    padding: 12px 0;
}

/* ---------- Status pill ---------- */
.rw-pill {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    padding: 3px 11px;
    border-radius: 999px;
    font-size: 12px;
    font-weight: 700;
    line-height: 1.7;
}

.rw-pill-ok {
    background: var(--rw-ok-soft);
    color: var(--rw-ok);
}

.rw-pill-skip {
    background: var(--rw-warn-soft);
    color: var(--rw-warn);
}


/* =====================================================================
   COMPACT SIZE - sabse neeche rehna chahiye (upar ke rules ko override karta hai)
   ===================================================================== */

/* Card ab poori width nahi, beech mein 1040px tak */
.reg-wizard {
    max-width: 1040px;
    margin: 0 auto;
}

.rw-shell {
    grid-template-columns: 270px minmax(0, 1fr);
    border-radius: 14px;
}

.rw-main::before {
    height: 4px;
}

/* Left panel */
.rw-brand {
    padding: 24px 22px;
}

.rw-mark {
    margin-bottom: 22px;
}

.rw-mark-i {
    width: 28px;
    height: 28px;
    flex: 0 0 28px;
}

.rw-mark b {
    font-size: 13.5px;
}

.rw-brand-h {
    font-size: 17px;
    margin-bottom: 7px;
}

.rw-brand-p {
    font-size: 11.5px;
    margin-bottom: 22px;
    line-height: 1.55;
}

.reg-step {
    padding-bottom: 16px;
    gap: 11px;
}

    .reg-step::before {
        left: 11px;
        top: 26px;
    }

.rw-dot {
    width: 24px;
    height: 24px;
    flex: 0 0 24px;
    font-size: 11px;
}

.rw-st {
    font-size: 12.5px;
    padding-top: 3px;
}

.rw-sn {
    font-size: 10.5px;
}

.rw-brand-foot {
    font-size: 11px;
    padding-top: 18px;
}

/* Heading area */
.rw-head {
    padding: 20px 26px 0;
}

.rw-eyebrow {
    font-size: 9.5px;
    padding: 4px 12px;
    margin-bottom: 8px;
}

.rw-title {
    font-size: 1.15rem;
}

.rw-sub {
    font-size: 12.5px;
    margin-top: 4px;
}

.rw-body {
    padding: 18px 26px 20px;
}

.reg-wizard .row.g-3 {
    gap: 14px 14px;
}

/* Fields */
.reg-wizard .form-label {
    font-size: 12px;
    margin-bottom: 4px;
}

.reg-wizard .rw-main .form-control {
    height: 38px;
    padding: 7px 12px;
    font-size: 13px;
    border-radius: 10px;
}

.reg-wizard .rw-main textarea.form-control {
    height: auto;
}

.reg-wizard .rw-main .form-control.rw-code {
    flex: 0 0 62px;
    width: 62px;
    max-width: 62px;
    border-radius: 10px 0 0 10px;
}

.reg-wizard .input-group .form-control:last-child {
    border-radius: 0 10px 10px 0;
}

.rw-hint {
    font-size: 11px;
    margin-top: 4px;
}

.reg-wizard td label {
    padding: 6px 14px;
    font-size: 12.5px;
}

.rw-consent {
    padding: 10px 13px;
}

/* Buttons */
.reg-wizard .btn {
    padding: 8px 24px;
    font-size: .8rem;
}

/* Footer bar */
.rw-foot {
    padding: 14px 26px 16px;
}

/* Hero */
.rw-hero {
    padding: 28px 16px 24px;
    margin-bottom: 18px;
}

    .rw-hero h1 {
        font-size: clamp(1.3rem, 3vw, 1.8rem);
    }

    .rw-hero p {
        font-size: .85rem;
    }

/* Mobile */
@media (max-width: 991.98px) {
    .rw-shell {
        grid-template-columns: 1fr;
    }

    .rw-brand {
        padding: 16px 16px 18px;
    }
}

@media (max-width: 575.98px) {
    .rw-head {
        padding: 18px 16px 0;
    }

    .rw-body {
        padding: 16px 16px 18px;
    }

    .rw-foot {
        padding: 12px 16px 16px;
    }
}
</style>
    <link rel="stylesheet" href='<%= ResolveUrl("~/css/registration.css") %>' />

    <script type="text/javascript">
        var PAN_REGEX = /^[A-Z]{5}[0-9]{4}[A-Z]$/;
        var AADHAAR_REGEX = /^[2-9][0-9]{11}$/;

        // Verify & Continue unlocks only when both hold. The server re-checks both;
        // these flags only keep the member from making a paid call that would fail.
        var panDupOk = false;   // PAN passed (or could not run) the duplicate check
        var linkedPan = '';     // PAN the server confirmed as linked with Aadhaar

        // ---------------------------------------------------------------
        // jQuery-free helpers (jQuery is not guaranteed on this page)
        // ---------------------------------------------------------------

        // Calls an ASP.NET [WebMethod] on THIS page. Resolves with the parsed JSON
        // ({ d: ... }); rejects on network error, HTTP error, bad JSON or timeout.
        function postPageMethod(name, payload, timeoutMs) {
            var ctrl = (typeof AbortController !== 'undefined') ? new AbortController() : null;
            var timer = ctrl ? setTimeout(function () { ctrl.abort(); }, timeoutMs) : null;

            return fetch(window.location.pathname + '/' + name, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json; charset=utf-8' },
                body: JSON.stringify(payload),
                credentials: 'same-origin',
                signal: ctrl ? ctrl.signal : undefined
            }).then(function (res) {
                if (timer) clearTimeout(timer);
                if (!res.ok) {
                    return res.text().then(function (t) {
                        throw new Error('HTTP ' + res.status + ': ' + t.substring(0, 300));
                    });
                }
                return res.json();
            }).catch(function (err) {
                if (timer) clearTimeout(timer);
                throw err;
            });
        }

        // Server messages can carry API error text, so they are never inserted raw.
        function esc(s) {
            var d = document.createElement('div');
            d.textContent = (s == null ? '' : String(s));
            return d.innerHTML;
        }

        function onlyDigits(el) {
            el.value = el.value.replace(/[^0-9]/g, '');
        }

        function setPanMsg(html) {
            var el = document.getElementById('lblPanCheckMsg');
            if (el) el.innerHTML = html;
        }

        function setLinkMsg(html) {
            var el = document.getElementById('lblLinkMsg');
            if (el) el.innerHTML = html;
        }

        function currentPan() {
            var box = document.getElementById('<%=txtPanCard.ClientID%>');
            return box ? box.value.trim().toUpperCase() : '';
        }

        function currentLinkAadhaar() {
            var box = document.getElementById('<%=txtPanAadhaar.ClientID%>');
            return box ? box.value.replace(/[^0-9]/g, '') : '';
        }

        function refreshPanNext() {
            var btn = document.getElementById('<%=btnPanNext.ClientID%>');
            if (btn) btn.disabled = !(panDupOk && linkedPan !== '' && linkedPan === currentPan());
        }

        // Kept under its old name: the duplicate check reports through it.
        function setPanNextEnabled(flag) {
            panDupOk = flag;
            refreshPanNext();
        }

        // Any edit to the PAN or Aadhaar makes the earlier link result stale.
        function resetLink() {
            linkedPan = '';
            setLinkMsg('');
            refreshPanNext();
        }

        var COULD_NOT_VERIFY = "<span class='rw-inline-note' style='color:#b45309'>Could not verify right now. It will be re-checked when you continue.</span>";

        // Free DB check before the paid verification call, so a PAN that is already
        // taken never burns an API credit.
        function checkPanDuplicate() {
            var box = document.getElementById('<%=txtPanCard.ClientID%>');
            if (!box) return;

            var pan = box.value.trim().toUpperCase();
            box.value = pan;

            if (pan.length === 0) { setPanMsg(""); setPanNextEnabled(false); return; }

            if (!PAN_REGEX.test(pan)) {
                setPanMsg("<span class='rw-inline-note' style='color:#dc2626'>Invalid PAN format. Example: ABCDE1234F</span>");
                setPanNextEnabled(false);
                return;
            }

            setPanMsg("<span class='rw-inline-note' style='color:#6b7a8d'>Checking...</span>");

            postPageMethod('CheckPanUniqueReg', { pan: pan }, 10000)
                .then(function (response) {
                    // Ignore a stale answer if the PAN was edited meanwhile.
                    if (pan !== currentPan()) return;

                    var r = response.d;
                    if (r === "OK") {
                        // &#10003; rather than a literal tick, to avoid encoding mojibake.
                        setPanMsg("<span class='rw-inline-note' style='color:#16a34a'>&#10003; PAN available.</span>");
                        setPanNextEnabled(true);
                    } else if (r === "DUPLICATE") {
                        setPanMsg("<span class='rw-inline-note' style='color:#dc2626'>This PAN is already registered with another ID.</span>");
                        setPanNextEnabled(false);
                    } else {
                        setPanMsg(COULD_NOT_VERIFY);
                        setPanNextEnabled(true);
                    }
                })
                .catch(function (err) {
                    console.error('CheckPanUniqueReg failed:', err);
                    if (pan !== currentPan()) return;
                    setPanMsg(COULD_NOT_VERIFY);
                    setPanNextEnabled(true);
                });
        }

        // Every PAN submit costs a paid API call, so everything is validated first.
        function onPanNext(btn) {
            var pan = document.getElementById('<%=txtPanCard.ClientID%>').value.trim().toUpperCase();
            var name = document.getElementById('<%=txtPanFullName.ClientID%>').value.trim();
            var dob = document.getElementById('<%=txtPanDob.ClientID%>').value.trim();
            var consent = document.getElementById('<%=chkPanConsent.ClientID%>');

            if (!PAN_REGEX.test(pan)) {
                alert('Invalid PAN format. Example: ABCDE1234F');
                return false;
            }
            if (name.length < 3) {
                alert('Please enter your full name exactly as printed on the PAN card.');
                return false;
            }
            if (dob.length === 0) {
                alert('Please enter your Date of Birth as printed on the PAN card.');
                return false;
            }
            if (consent && !consent.checked) {
                alert('Please give consent to verify your PAN details.');
                return false;
            }
            if (linkedPan !== pan) {
                alert('Please check your PAN-Aadhaar link first.');
                return false;
            }

            btn.value = 'Verifying...';
            setTimeout(function () { btn.disabled = true; }, 10);
            return true;
        }

        function checkPanAadhaarLink(btn) {
            var pan = currentPan();
            var aadhaar = currentLinkAadhaar();
            var consent = document.getElementById('<%=chkPanConsent.ClientID%>');

            if (!PAN_REGEX.test(pan)) {
                alert('Invalid PAN format. Example: ABCDE1234F');
                return;
            }
            if (!AADHAAR_REGEX.test(aadhaar)) {
                alert('Please enter a valid 12-digit Aadhaar number.');
                return;
            }
            if (consent && !consent.checked) {
                alert('Please give consent to verify your PAN details.');
                return;
            }

            linkedPan = '';
            refreshPanNext();

            var oldText = btn.value;
            btn.disabled = true;
            btn.value = 'Checking...';
            setLinkMsg("<span class='rw-inline-note' style='color:#6b7a8d'>Checking...</span>");

            var failMsg = 'Could not check PAN-Aadhaar link. Please try again.';

            postPageMethod('CheckPanAadhaarLinkReg', { pan: pan, aadhaar: aadhaar, consent: true }, 15000)
                .then(function (response) {
                    var r = response.d || {};
                    // The member may have edited either box while the check ran;
                    // the result then belongs to values no longer on screen.
                    if (pan !== currentPan() || aadhaar !== currentLinkAadhaar()) {
                        setLinkMsg('');
                    } else if (r.linked) {
                        linkedPan = pan;
                        setLinkMsg("<span class='rw-inline-note' style='color:#16a34a'>&#10003; " + esc(r.msg) + "</span>");
                    } else {
                        var msg = r.msg || failMsg;
                        setLinkMsg("<span class='rw-inline-note' style='color:#dc2626'>" + esc(msg) + "</span>");
                        alert(msg);
                    }
                })
                .catch(function (err) {
                    console.error('CheckPanAadhaarLinkReg failed:', err);
                    setLinkMsg("<span class='rw-inline-note' style='color:#dc2626'>" + esc(failMsg) + "</span>");
                    alert(failMsg);
                })
                .then(function () {
                    // runs on both success and failure (replaces jQuery "complete")
                    btn.disabled = false;
                    btn.value = oldText;
                    refreshPanNext();
                });
        }

        document.addEventListener('DOMContentLoaded', function () {
            var panBox = document.getElementById('<%=txtPanCard.ClientID%>');
            var aadhaarBox = document.getElementById('<%=txtPanAadhaar.ClientID%>');
            var hf = document.getElementById('<%=hfPanLinkedPan.ClientID%>');

            // Step 2 is not on screen: nothing to wire up.
            if (!panBox) return;

            panBox.addEventListener('input', resetLink);
            if (aadhaarBox) aadhaarBox.addEventListener('input', function () {
                var digits = this.value.replace(/[^0-9]/g, '');
                if (digits !== this.value) this.value = digits;
                resetLink();
            });

            // Restores the "linked" state after a postback, e.g. a failed verify.
            var saved = hf ? hf.value : '';
            if (saved !== '' && saved === currentPan()) {
                linkedPan = saved;
                setLinkMsg("<span class='rw-inline-note' style='color:#16a34a'>&#10003; PAN is linked with Aadhaar. You can now verify your PAN.</span>");
            } else {
                linkedPan = '';
                setLinkMsg('');
            }

            refreshPanNext();
            if (!panBox.disabled && currentPan() !== '') checkPanDuplicate();
        });

        function onPanSkip() {
            return confirm('Skip PAN verification?\n\nPayout will deduct 20% TDS if PAN is not provided. You can complete PAN KYC later after login.');
        }

        function onSendOtp(btn) {
            var a = document.getElementById('<%=txtAadhaarNo.ClientID%>').value.trim();
            var consent = document.getElementById('<%=chkAadhaarConsent.ClientID%>');

            if (a.length !== 12) {
                alert('Please enter a valid 12-digit Aadhaar number.');
                return false;
            }
            if (consent && !consent.checked) {
                alert('Please give consent before verifying your Aadhaar.');
                return false;
            }

            btn.value = 'Sending...';
            setTimeout(function () { btn.disabled = true; }, 10);
            return true;
        }

        function onVerifyOtp(btn) {
            var otp = document.getElementById('<%=txtAadhaarOtp.ClientID%>').value.trim();
            if (otp.length < 4) {
                alert('Please enter the OTP received on your Aadhaar-linked mobile.');
                return false;
            }
            btn.value = 'Verifying...';
            setTimeout(function () { btn.disabled = true; }, 10);
            return true;
        }

        function onAadhaarSkip() {
            return confirm('Skip Aadhaar / Address KYC?\n\nYou can complete it later after login.');
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-body">
        <div class="container-fluid">
            <div class="reg-wizard">
                <div class="rw-shell">

                    <%-- ==================== BRAND PANEL ====================
                         Rendered once and shared by all four steps. The step
                         classes and the footnote are set by ShowStep(). --%>
                    <aside class="rw-brand">
                        <div class="rw-mark">
                            <span class="rw-mark-i">
                                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="#fff"
                                    stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round">
                                    <path d="M4 16 L11 6 L13 12 L20 6" />
                                </svg>
                            </span>
                            <b>ePay India</b>
                        </div>

                        <h2 class="rw-brand-h">Join in four steps</h2>
                        <p class="rw-brand-p">
                            Basic details first. PAN and Aadhaar are optional &mdash; skip either
                            one and finish it after login.
                        </p>

                        <ol class="rw-steps">
                            <li class="reg-step now" id="regStep1" runat="server">
                                <span class="rw-dot"><span class="num">1</span><i class="fas fa-check tick"></i><i class="fas fa-minus mns"></i></span>
                                <span class="rw-st">Basic Details<span class="rw-sn">Sponsor &amp; contact</span></span>
                            </li>
                            <li class="reg-step wait" id="regStep2" runat="server">
                                <span class="rw-dot"><span class="num">2</span><i class="fas fa-check tick"></i><i class="fas fa-minus mns"></i></span>
                                <span class="rw-st">PAN KYC<span class="rw-sn">Optional</span></span>
                            </li>
                            <li class="reg-step wait" id="regStep3" runat="server">
                                <span class="rw-dot"><span class="num">3</span><i class="fas fa-check tick"></i><i class="fas fa-minus mns"></i></span>
                                <span class="rw-st">Aadhaar KYC<span class="rw-sn">Optional</span></span>
                            </li>
                            <li class="reg-step wait" id="regStep4" runat="server">
                                <span class="rw-dot"><span class="num">4</span><i class="fas fa-check tick"></i><i class="fas fa-minus mns"></i></span>
                                <span class="rw-st">Confirm<span class="rw-sn">Terms &amp; submit</span></span>
                            </li>
                        </ol>

                        <p class="rw-brand-foot">
                            <asp:Literal ID="litBrandNote" runat="server"></asp:Literal>
                        </p>
                    </aside>

                    <%-- ==================== FORM COLUMN ==================== --%>
                    <div class="rw-main">

                        <%-- -------------------- STEP 1 : BASIC DETAILS -------------------- --%>
                        <div id="pnlStep1" runat="server">
                            <div class="rw-head">
                                <p class="rw-eyebrow">Step 1 of 4</p>
                                <h3 class="rw-title">Basic Details</h3>
                                <p class="rw-sub">Tell us who referred you and how we can reach you.</p>
                            </div>

                            <div class="rw-body">
                                <div class="row g-3">

                                    <div id="Div1" class="col-md-6" runat="server" visible="true">
                                        <label class="form-label">Referral ID<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtRefralId" CssClass="form-control" TabIndex="1" runat="server"
                                            AutoPostBack="True" ValidationGroup="eInformation" autocomplete="off"
                                            placeholder="e.g. EPI123456" OnTextChanged="txtRefralId_TextChanged"></asp:TextBox>
                                        <asp:Label ID="lblRefralNm" runat="server" CssClass="rw-name-hint"></asp:Label>
                                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    </div>

                                    <div class="col-md-6" id="rwSpnsr" runat="server" visible="false">
                                        <label class="form-label">Placement ID<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtUplinerId" CssClass="form-control" TabIndex="2" runat="server" AutoPostBack="True"
                                            ValidationGroup="eSponsor" autocomplete="off" OnTextChanged="txtUplinerId_TextChanged"></asp:TextBox>
                                        <asp:Label ID="lblUplnrNm" runat="server" CssClass="rw-name-hint"></asp:Label>
                                    </div>

                                    <div class="col-md-6" runat="server" id="DivLeg1" visible="true">
                                        <label class="form-label">Leg<span class="rw-req">*</span></label>
                                        <asp:RadioButtonList ID="RbtnLegNo" runat="server" TabIndex="3" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="RbtnLegNo_SelectedIndexChanged" CssClass="form-control" />
                                    </div>

                                    <div id="dvreg" class="col-12" runat="server" visible="false">
                                        <div class="row g-3">
                                            <div class="col-md-6">
                                                <label class="form-label">Registration As<span class="rw-req">*</span></label>
                                                <asp:RadioButtonList ID="RbCategory" runat="server" RepeatDirection="Horizontal"
                                                    TabIndex="4" onchange="return GetRegistrationAs()">
                                                    <asp:ListItem Text="Individual" Value="IN" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Company" Value="C"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                            <div class="col-md-6" id="RegType" style="display: none">
                                                <label class="form-label">
                                                    <asp:Label ID="LblRegType" Text="Registration Type" runat="server"></asp:Label><span class="rw-req">*</span></label>
                                                <asp:RadioButtonList ID="CbSubCategory" runat="server" TabIndex="5" RepeatDirection="Horizontal"
                                                    onchange="return GetRegistrationType()">
                                                    <asp:ListItem Text="ProprietorShip" Value="SP" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Partnership Firm" Value="PF"></asp:ListItem>
                                                    <asp:ListItem Text="Private Limited Company" Value="PL"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>

                                    <%-- Full Name is intentionally NOT collected here any more. The name comes
                                         from the verified PAN (step 2) or the verified Aadhaar (step 3); the
                                         txtFrstNm box now lives in step 4 and shows only when both were skipped. --%>

                                    <div class="col-md-6" id="TrPrtnrCap" style="display: none">
                                        <label class="form-label">
                                            <asp:Label ID="LblPartnerName" runat="server" Text="Partner Name Seperated By Comma(,)"></asp:Label></label>
                                    </div>

                                    <div class="col-md-6" id="divFName" runat="server" visible="false">
                                        <label class="form-label">Father / Husband's Name<span class="rw-req">*</span></label>
                                        <div class="row g-2">
                                            <div class="col-4">
                                                <asp:DropDownList CssClass="form-control" ID="CmbType" runat="server" TabIndex="7">
                                                    <asp:ListItem Value="S/O" Text="S/O"></asp:ListItem>
                                                    <asp:ListItem Value="D/O" Text="D/O"></asp:ListItem>
                                                    <asp:ListItem Value="W/O" Text="W/O"></asp:ListItem>
                                                    <asp:ListItem Value="H/O" Text="H/O"></asp:ListItem>
                                                    <asp:ListItem Value="C/O" Text="C/O"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-8">
                                                <asp:TextBox ID="txtFNm" runat="server" TabIndex="8" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="Div2" class="col-md-6" visible="false" runat="server">
                                        <label class="form-label">
                                            <asp:Label ID="LblRegistDate" runat="server" Text="Date Of Birth"></asp:Label><span class="rw-req">*</span></label>
                                        <div class="row g-2">
                                            <div class="col-4">
                                                <asp:DropDownList ID="ddlDOBdt" runat="server" CssClass="form-control" TabIndex="9" autocomplete="off"></asp:DropDownList>
                                            </div>
                                            <div class="col-4">
                                                <asp:DropDownList ID="ddlDOBmnth" runat="server" CssClass="form-control" TabIndex="10" autocomplete="off"></asp:DropDownList>
                                            </div>
                                            <div class="col-4">
                                                <asp:DropDownList ID="ddlDOBYr" runat="server" CssClass="form-control" TabIndex="11" autocomplete="off"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6" id="Div3" visible="false" runat="server">
                                        <label class="form-label">Marital Status<span class="rw-req">*</span></label>
                                        <asp:RadioButtonList ID="RbtMarried" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" TabIndex="12" onchange="return GetSelectedItem()" autocomplete="off">
                                            <asp:ListItem Text="Married" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="UnMarried" Value="N"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>

                                    <div class="col-md-6" id="divMarriageDate" visible="false" style="display: none;">
                                        <label class="form-label">Marriage Date<span class="rw-req">*</span></label>
                                        <div class="row g-2">
                                            <div class="col-4">
                                                <asp:DropDownList ID="DDlMDay" runat="server" CssClass="form-control" TabIndex="13"></asp:DropDownList>
                                            </div>
                                            <div class="col-4">
                                                <asp:DropDownList ID="DDLMMonth" runat="server" CssClass="form-control" TabIndex="14"></asp:DropDownList>
                                            </div>
                                            <div class="col-4">
                                                <asp:DropDownList ID="DDLMYear" runat="server" CssClass="form-control" TabIndex="15"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6" id="CompName" style="display: none">
                                        <label class="form-label">Company Name<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="TxtCompanyName" runat="server" CssClass="form-control" TabIndex="16"></asp:TextBox>
                                    </div>

                                    <div class="col-md-6" id="CompRegistrationNo" style="display: none">
                                        <label class="form-label">Company Registration No<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="TxtRegistrationNo" runat="server" CssClass="form-control" TabIndex="17"></asp:TextBox>
                                    </div>

                                    <%-- Address block. Hidden here: it is filled from the Aadhaar step. --%>
                                    <div id="dvpin" class="col-12" runat="server" visible="false">
                                        <p class="rw-section-title">Contact Detail</p>
                                        <div class="row g-3">
                                            <div id="Div4" class="col-md-6" visible="false" runat="server">
                                                <label class="form-label">Address<span class="rw-req">*</span></label>
                                                <asp:TextBox ID="txtAddLn1" CssClass="form-control" TabIndex="18" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <label class="form-label">Pin code<span class="rw-req">*</span></label>
                                                <asp:TextBox ID="txtPinCode" CssClass="form-control" onkeypress="return isNumberKey(event);"
                                                    TabIndex="19" runat="server" MaxLength="6" autocomplete="off" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <label class="form-label">State<span class="rw-req">*</span></label>
                                                <asp:TextBox ID="txtStateName" runat="server" CssClass="form-control" TabIndex="16"
                                                    autocomplete="off" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="StateCode" runat="server" />
                                            </div>
                                            <div class="col-md-6">
                                                <label class="form-label">District<span class="rw-req">*</span></label>
                                                <asp:TextBox ID="ddlDistrict" CssClass="form-control" TabIndex="17" runat="server"
                                                    autocomplete="off" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="HDistrictCode" runat="server" />
                                            </div>
                                            <div class="col-md-6">
                                                <label class="form-label">City<span class="rw-req">*</span></label>
                                                <asp:TextBox ID="ddlTehsil" CssClass="form-control" TabIndex="18" runat="server"
                                                    ValidationGroup="eInformation" autocomplete="off" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="HCityCode" runat="server" />
                                            </div>
                                            <div class="col-md-6">
                                                <label class="form-label">Area<span class="rw-req">*</span></label>
                                                <asp:DropDownList ID="DDlVillage" CssClass="form-control" TabIndex="19" runat="server"
                                                    ValidationGroup="eInformation" autocomplete="off" onchange="FnVillageChange(this.value);">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-6" id="divVillage" style="display: none">
                                                <label class="form-label">Area Name<span class="rw-req">*</span></label>
                                                <asp:TextBox ID="TxtVillage" CssClass="form-control" TabIndex="20" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>

                                            <div id="Dvfld" class="col-12" runat="server" visible="false">
                                                <div class="row g-3">
                                                    <div class="col-12">
                                                        <div class="rw-consent">
                                                            <asp:CheckBox ID="ChkSame" runat="server" onclick="return GetSameAsPostal()" TabIndex="21" />
                                                            <label>Postal address is same as above</label>
                                                        </div>
                                                    </div>
                                                    <div class="col-12">
                                                        <p class="rw-section-title">Postal Address</p>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="form-label">Address<span class="rw-req">*</span></label>
                                                        <asp:TextBox ID="TxtPostalAddress" CssClass="form-control" TabIndex="22" runat="server" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="form-label">Pin code<span class="rw-req">*</span></label>
                                                        <asp:TextBox ID="TxtPostPincode" CssClass="form-control" onkeypress="return isNumberKey(event);"
                                                            TabIndex="23" runat="server" MaxLength="6" autocomplete="off" AutoPostBack="true"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="form-label">State<span class="rw-req">*</span></label>
                                                        <asp:TextBox ID="TxtpostState" runat="server" CssClass="form-control" TabIndex="24"
                                                            autocomplete="off" Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="HPostStateCode" runat="server" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="form-label">District<span class="rw-req">*</span></label>
                                                        <asp:TextBox ID="TxtPostDistrict" CssClass="form-control" TabIndex="25" runat="server"
                                                            autocomplete="off" Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="HPostDistrict" runat="server" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="form-label">City<span class="rw-req">*</span></label>
                                                        <asp:TextBox ID="TxtPostCity" CssClass="form-control" TabIndex="26" runat="server"
                                                            ValidationGroup="eInformation" autocomplete="off" Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="HPostCity" runat="server" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <label class="form-label">Area<span class="rw-req">*</span></label>
                                                        <asp:DropDownList ID="DDlPostVillage" CssClass="form-control" TabIndex="27" runat="server"
                                                            ValidationGroup="eInformation" autocomplete="off" onchange="FnPostVillageChange(this.value);">
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="HPostVillage" runat="server" />
                                                    </div>
                                                    <div class="col-md-6" id="divPostVillage" style="display: none">
                                                        <label class="form-label">Area Name<span class="rw-req">*</span></label>
                                                        <asp:TextBox ID="TxtPostVillage" CssClass="form-control" TabIndex="28" runat="server" autocomplete="off"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="Div5" class="col-md-6" runat="server">
                                        <label class="form-label">Country<span class="rw-req">*</span></label>
                                        <asp:DropDownList ID="ddlCountryNAme" runat="server" CssClass="form-control" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCountryNAme_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-md-6">
                                        <label class="form-label">Mobile No.<span class="rw-req">*</span></label>
                                        <div class="input-group">
                                            <asp:TextBox ID="ddlMobileNAme" CssClass="form-control rw-code" runat="server" ValidationGroup="eInformation"
                                                autocomplete="off" Enabled="false" placeholder="+91"></asp:TextBox>
                                            <asp:TextBox ID="txtMobileNo" onkeypress="return isNumberKey(event);" CssClass="form-control validate[required]"
                                                runat="server" MaxLength="10" ValidationGroup="eInformation" autocomplete="off"
                                                placeholder="10-digit mobile number"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div id="Div6" class="col-md-6" visible="false" runat="server">
                                        <label class="form-label">Phone No.<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtPhNo" onkeypress="return isNumberKey(event);" CssClass="form-control"
                                            TabIndex="30" runat="server" MaxLength="10" autocomplete="off"></asp:TextBox>
                                    </div>

                                    <div id="Div7" class="col-md-6" runat="server">
                                        <label class="form-label">E-Mail ID<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtEMailId" CssClass="form-control validate[required]" TextMode="Email"
                                            TabIndex="31" runat="server" autocomplete="off" placeholder="you@example.com"></asp:TextBox>
                                        <small class="rw-hint">Your login details will be sent to this address.</small>
                                        <asp:Label ID="LblEmainID" runat="server" CssClass="rw-alert rw-alert-error"></asp:Label>
                                    </div>

                                    <div id="Div966" class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">Wallet Address</label>
                                        <asp:TextBox ID="TxtWalletaddress" CssClass="form-control" TabIndex="31" runat="server" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="HdnWalletAddress" runat="server" />
                                        <asp:HiddenField ID="HiddenField4" runat="server" />
                                        <asp:HiddenField ID="Hdnhhhgg" runat="server" />
                                    </div>

                                    <%-- ---------------------------------------------------------------
                                         Legacy fields. PAN and Aadhaar are collected in step 2 and 3;
                                         these stay hidden and only carry the verified values into the
                                         member record, so none of them may be removed.
                                         --------------------------------------------------------------- --%>
                                    <div id="Div8" class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">PAN No Available<span class="rw-req">*</span></label>
                                        <asp:RadioButtonList ID="RbtPan" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                            RepeatLayout="Table" TabIndex="41">
                                            <asp:ListItem Text="Yes" Value="Y" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:Label ID="LblPanNoAvail" runat="server" CssClass="rw-hint"
                                            Text="Payout will deduct 20%, If you not enter PAN NO."></asp:Label>
                                    </div>

                                    <div id="Div9" class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">PAN No.<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtPanNo" CssClass="form-control validate[custom[panno]]"
                                            TabIndex="42" runat="server" autocomplete="off"></asp:TextBox>
                                    </div>

                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">Nominee Name<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtNominee" CssClass="form-control" TabIndex="32" runat="server" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">Relation<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtRelation" CssClass="form-control" TabIndex="33" runat="server" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">Account No.<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="TxtAccountNo" onkeypress="return isNumberKey(event);" CssClass="form-control"
                                            TabIndex="34" runat="server" MaxLength="16" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">Account Type<span class="rw-req">*</span></label>
                                        <asp:DropDownList ID="DDLAccountType" runat="server" CssClass="form-control" TabIndex="21">
                                            <asp:ListItem Text="CHOOSE ACCOUNT TYPE" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="SAVING ACCOUNT" Value="SAVING ACCOUNT"></asp:ListItem>
                                            <asp:ListItem Text="CURRENT ACCOUNT" Value="CURRENT ACCOUNT"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">Bank<span class="rw-req">*</span></label>
                                        <asp:DropDownList ID="CmbBank" runat="server" CssClass="form-control" TabIndex="36"
                                            onchange="FnBankChange(this.value);" autocomplete="off">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6" id="divBank" style="display: none">
                                        <label class="form-label">Bank Name<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="TxtBank" CssClass="form-control" TabIndex="37" runat="server" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">Branch Name<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="TxtBranchName" CssClass="form-control" TabIndex="38" runat="server" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">IFSC Code<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtIfsCode" runat="server" CssClass="form-control" TabIndex="39" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <asp:TextBox ID="TxtMICR" CssClass="form-control" Visible="false" TabIndex="40" runat="server" autocomplete="off"></asp:TextBox>
                                    </div>

                                    <div id="Div10" class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">AADHAR No.<span class="rw-req">*</span></label>
                                        <div class="row g-2">
                                            <div class="col-4">
                                                <asp:TextBox ID="TxtAAdhar1" CssClass="form-control" TabIndex="43" runat="server"
                                                    onkeypress="return isNumberKey(event);" MaxLength="4" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="col-4">
                                                <asp:TextBox ID="TxtAadhar2" CssClass="form-control" TabIndex="44" runat="server"
                                                    onkeypress="return isNumberKey(event);" MaxLength="4" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="col-4">
                                                <asp:TextBox ID="TxtAadhar3" CssClass="form-control" TabIndex="45" runat="server"
                                                    onkeypress="return isNumberKey(event);" MaxLength="4" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">Select Paymode<span class="rw-req">*</span></label>
                                        <asp:DropDownList ID="DdlPaymode" runat="server" AutoPostBack="true" CssClass="form-control"
                                            TabIndex="46" autocomplete="off">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">
                                            <asp:Label ID="LblDDNo" runat="server" Text="Draft/CHEQUE No. *"></asp:Label></label>
                                        <asp:TextBox ID="TxtDDNo" CssClass="form-control" TabIndex="47" runat="server" MaxLength="15" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">
                                            <asp:Label ID="LblDDDate" runat="server" Text="Draft/CHEQUE Date *"></asp:Label></label>
                                        <asp:TextBox ID="TxtDDDate" runat="server" TabIndex="48" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                        <AjaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtDDDate"
                                            Format="dd-MMM-yyyy"></AjaxToolkit:CalendarExtender>
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">Issued Bank Name</label>
                                        <asp:TextBox ID="TxtIssueBank" CssClass="form-control" TabIndex="49" runat="server" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6" runat="server" visible="false">
                                        <label class="form-label">Issued Bank Branch</label>
                                        <asp:TextBox ID="TxtIssueBranch" CssClass="form-control" TabIndex="50" runat="server" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div id="Div11" class="col-md-6" visible="false" runat="server">
                                        <label class="form-label">Transaction Password<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="TxtTransactionPassword" CssClass="validate[required,minSize[5],maxSize[10]] form-control"
                                            TabIndex="52" runat="server" TextMode="Password" ValidationGroup="eInformation" autocomplete="off"></asp:TextBox>
                                    </div>

                                    <div id="divOtp" class="col-12" runat="server" visible="false">
                                        <div class="row g-3">
                                            <div class="col-md-6">
                                                <label class="form-label">Password<span class="rw-req">*</span></label>
                                                <asp:TextBox ID="TxtPasswd" CssClass="form-control" TabIndex="51" runat="server"
                                                    name="password" TextMode="Password"></asp:TextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <label class="form-label">Confirm Password<span class="rw-req">*</span></label>
                                                <asp:TextBox ID="pass2" runat="server" name="password" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" Display="Dynamic" ControlToValidate="TxtPasswd"
                                                    runat="server" ErrorMessage="RequiredFieldValidator">confirm New Password can't left blank</asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="CompareValidator1" ControlToValidate="TxtPasswd" ControlToCompare="Pass2"
                                                    Type="String" Operator="Equal" Text="Passwords must match!" runat="Server" ForeColor="#972f36" />
                                            </div>
                                            <div class="col-md-6" style="display: none">
                                                <label class="form-label">Enter OTP Sent on your E-mail Id.<span class="rw-req">*</span></label>
                                                <asp:TextBox ID="TxtOtp" CssClass="form-control validate[required]" runat="server"
                                                    autocomplete="off" placeholder="Enter OTP" ValidationGroup="eInformation"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" Display="Dynamic" ControlToValidate="TxtOtp"
                                                    runat="server" ValidationGroup="eInformation">Opt Required
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <asp:Label ID="lblErrEpin" runat="server" CssClass="rw-alert rw-alert-error"></asp:Label>
                            </div>

                            <div class="rw-foot">
                                <asp:Button ID="CmdCancel" runat="server" Text="Cancel" CssClass="btn btn-rw-ghost"
                                    ValidationGroup="eCancel" CausesValidation="false" OnClick="CmdCancel_Click" />
                                <div class="rw-foot-right">
                                    <asp:Button ID="btnStep1Next" runat="server" Text="Continue" CssClass="btn btn-rw-primary"
                                        CausesValidation="false" OnClick="btnStep1Next_Click" />
                                </div>
                            </div>
                        </div>

                        <%-- -------------------- STEP 2 : PAN KYC -------------------- --%>
                        <div id="pnlStep2" runat="server" visible="false">
                            <div class="rw-head">
                                <p class="rw-eyebrow">Step 2 of 4 &middot; Optional</p>
                                <h3 class="rw-title">PAN Card KYC</h3>
                                <p class="rw-sub">
                                    Enter your PAN exactly as printed on the card. We verify it online with the
                                    Income Tax Department &mdash; no document upload needed.
                                </p>
                            </div>

                            <div class="rw-body">
                                <div class="row g-3">
                                    <div class="col-md-6">
                                        <label class="form-label">PAN Card No.<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtPanCard" runat="server" CssClass="form-control"
                                            onblur="checkPanDuplicate()" Style="text-transform: uppercase; letter-spacing: .06em;"
                                            MaxLength="10" placeholder="ABCDE1234F"></asp:TextBox>
                                        <span id="lblPanCheckMsg"></span>
                                    </div>

                                    <div class="col-md-6">
                                        <label class="form-label">Date of Birth (as per PAN)<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtPanDob" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-12">
                                        <label class="form-label">Name (exactly as printed on PAN)<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtPanFullName" runat="server" CssClass="form-control"
                                            MaxLength="100" placeholder="e.g. RAKESH KUMAR"></asp:TextBox>
                                        <small class="rw-hint">Spelling must match the card exactly, including the order of names.
                                            This becomes your registered name, and the name on your Aadhaar
                                            must be the same as this name.
                                        </small>
                                    </div>

                                    <div class="col-12">
                                        <div class="rw-consent">
                                            <asp:CheckBox ID="chkPanConsent" runat="server" />
                                            <label for="<%=chkPanConsent.ClientID%>">
                                                I give my consent to verify the above PAN details with the Income Tax Department / NSDL.
                                            </label>
                                        </div>
                                    </div>

                                    <div class="col-md-7">
                                        <label class="form-label">Aadhaar No. (linked with your PAN)<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtPanAadhaar" runat="server" CssClass="form-control"
                                            MaxLength="12" inputmode="numeric" autocomplete="off" Style="letter-spacing: .08em;"
                                            placeholder="12-digit Aadhaar number"></asp:TextBox>
                                        <small class="rw-hint">Your PAN can be verified only after it is confirmed as linked with this Aadhaar.</small>
                                    </div>

                                    <div class="col-md-5 d-flex align-items-end">
                                        <asp:Button ID="btnPanCheckLink" runat="server" Text="Check PAN-Aadhaar Link"
                                            CssClass="btn btn-rw-ghost" UseSubmitBehavior="false" CausesValidation="false"
                                            OnClientClick="checkPanAadhaarLink(this); return false;" />
                                    </div>

                                    <div class="col-12">
                                        <span id="lblLinkMsg"></span>
                                        <asp:HiddenField ID="hfPanLinkedPan" runat="server" />
                                    </div>
                                </div>

                                <asp:Label ID="lblPanMsg" runat="server" CssClass="rw-alert rw-alert-error"></asp:Label>

                                <div id="divPanFetched" runat="server" visible="false">
                                    <div class="rw-result">
                                        <div class="rw-result-head">
                                            <i class="fas fa-check-circle"></i>PAN Verified
                                        </div>
                                        <table class="rw-kv">
                                            <tr>
                                                <td>PAN</td>
                                                <td>
                                                    <asp:Label ID="lblPanFetchedNo" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>Name</td>
                                                <td>
                                                    <asp:Label ID="lblPanFetchedName" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>Category</td>
                                                <td>
                                                    <asp:Label ID="lblPanFetchedCategory" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>

                            <div class="rw-foot">
                                <asp:Button ID="btnPanBack" runat="server" Text="Back" CssClass="btn btn-rw-ghost"
                                    CausesValidation="false" OnClick="btnPanBack_Click" />
                                <div class="rw-foot-right">
                                    <%-- Shown only once the PAN is verified. Re-running the paid check or
                                         skipping past a verification already paid for are both wrong here. --%>
                                    <asp:Button ID="btnPanEdit" runat="server" Text="Change PAN" CssClass="btn btn-rw-ghost"
                                        Visible="false" CausesValidation="false" OnClick="btnPanEdit_Click" />
                                    <asp:Button ID="btnPanSkip" runat="server" Text="Skip for now" CssClass="btn btn-rw-skip"
                                        CausesValidation="false" OnClick="btnPanSkip_Click" OnClientClick="return onPanSkip();" />
                                    <asp:Button ID="btnPanNext" runat="server" Text="Verify &amp; Continue" CssClass="btn btn-rw-primary"
                                        CausesValidation="false" OnClick="btnPanNext_Click" OnClientClick="return onPanNext(this);" />
                                    <asp:Button ID="btnPanContinue" runat="server" Text="Next" CssClass="btn btn-rw-primary"
                                        Visible="false" CausesValidation="false" OnClick="btnPanContinue_Click" />
                                </div>
                                <p class="rw-foot-note">
                                    <asp:Literal ID="litPanFootNote" runat="server"></asp:Literal>
                                </p>
                            </div>
                        </div>

                        <%-- -------------------- STEP 3 : AADHAAR / ADDRESS KYC -------------------- --%>
                        <div id="pnlStep3" runat="server" visible="false">
                            <div class="rw-head">
                                <p class="rw-eyebrow">Step 3 of 4 &middot; Optional</p>
                                <h3 class="rw-title">Aadhaar &amp; Address KYC</h3>
                                <p class="rw-sub">
                                    Verify your Aadhaar with an OTP. Your address is fetched straight from UIDAI,
                                    so there is nothing to type or upload. If you verified your PAN, the name on
                                    your Aadhaar must match the name on your PAN.
                                </p>
                            </div>

                            <div class="rw-body">
                                <%-- Aadhaar entry + OTP --%>
                                <div id="divAadhaarEntry" runat="server">
                                    <div class="row g-3">
                                        <div class="col-md-7">
                                            <label class="form-label">Aadhaar No.<span class="rw-req">*</span></label>
                                            <asp:TextBox ID="txtAadhaarNo" runat="server" CssClass="form-control"
                                                MaxLength="12" onkeyup="onlyDigits(this)" Style="letter-spacing: .08em;"
                                                placeholder="12-digit Aadhaar number"></asp:TextBox>
                                            <small class="rw-hint">An OTP will be sent to your Aadhaar-linked mobile number.</small>
                                            <asp:Label ID="lblAadhaarLinkedNote" runat="server" Visible="false" CssClass="rw-hint"
                                                Style="display: block; color: #15803d;"
                                                Text="This is the Aadhaar linked with your PAN."></asp:Label>
                                        </div>

                                        <div class="col-12">
                                            <div class="rw-consent">
                                                <asp:CheckBox ID="chkAadhaarConsent" runat="server" />
                                                <label for="<%=chkAadhaarConsent.ClientID%>">
                                                    I consent to verify my Aadhaar and fetch my address through UIDAI Offline e-KYC.
                                                </label>
                                            </div>
                                        </div>

                                        <div class="col-12">
                                            <asp:Button ID="btnSendOtpReg" runat="server" Text="Send OTP"
                                                CssClass="btn btn-rw-primary" CausesValidation="false"
                                                OnClick="btnSendOtpReg_Click" OnClientClick="return onSendOtp(this);" />
                                        </div>

                                        <div id="divAadhaarOtp" runat="server" visible="false" class="col-12">
                                            <div class="row g-3 align-items-end" style="margin-top: 4px;">
                                                <div class="col-md-5 rw-otp">
                                                    <label class="form-label">Enter OTP<span class="rw-req">*</span></label>
                                                    <asp:TextBox ID="txtAadhaarOtp" runat="server" CssClass="form-control"
                                                        MaxLength="6" onkeyup="onlyDigits(this)" placeholder="000000"></asp:TextBox>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:Button ID="btnVerifyOtpReg" runat="server" Text="Verify OTP"
                                                        CssClass="btn btn-rw-ok" CausesValidation="false"
                                                        OnClick="btnVerifyOtpReg_Click" OnClientClick="return onVerifyOtp(this);" />
                                                    &nbsp;<asp:Button ID="btnResendOtpReg" runat="server" Text="Resend OTP"
                                                        CssClass="btn btn-rw-ghost" CausesValidation="false"
                                                        OnClick="btnSendOtpReg_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%-- Confirm the fetched address --%>
                                <div id="divAadhaarConfirm" runat="server" visible="false">
                                    <p class="rw-section-title">Confirm your address</p>
                                    <div class="row g-3">
                                        <div class="col-12">
                                            <label class="form-label">Address</label>
                                            <%-- Read-only on purpose: the save uses the UIDAI record, not this box. --%>
                                            <asp:TextBox ID="txtRegAddress" runat="server" CssClass="form-control"
                                                TextMode="MultiLine" Rows="3" ReadOnly="true"></asp:TextBox>
                                            <small class="rw-hint">Fetched from UIDAI and saved exactly as returned.</small>
                                        </div>
                                        <div class="col-md-6">
                                            <label class="form-label">Pincode</label>
                                            <asp:TextBox ID="txtRegPincode" runat="server" CssClass="form-control"
                                                MaxLength="6" ReadOnly="true"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <label class="form-label">State</label>
                                            <asp:DropDownList ID="ddlRegState" runat="server" CssClass="form-control"
                                                AutoPostBack="true" CausesValidation="false"
                                                OnSelectedIndexChanged="ddlRegState_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-6">
                                            <label class="form-label">District<span class="rw-req">*</span></label>
                                            <asp:TextBox ID="txtRegDistrict" runat="server" CssClass="form-control" MaxLength="180"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <label class="form-label">City<span class="rw-req">*</span></label>
                                            <asp:TextBox ID="txtRegCity" runat="server" CssClass="form-control" MaxLength="180"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <asp:Label ID="lblAadhaarMsg" runat="server" CssClass="rw-alert rw-alert-info"></asp:Label>

                                <div id="divUidFetched" runat="server" visible="false">
                                    <div class="rw-result">
                                        <div class="rw-result-head">
                                            <i class="fas fa-check-circle"></i>UIDAI Verified Record
                                        </div>
                                        <table class="rw-kv">
                                            <tr id="trUidName" runat="server">
                                                <td>Name</td>
                                                <td>
                                                    <asp:Label ID="lblUidName" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr id="trUidDob" runat="server">
                                                <td>Date of Birth</td>
                                                <td>
                                                    <asp:Label ID="lblUidDob" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr id="trUidGender" runat="server">
                                                <td>Gender</td>
                                                <td>
                                                    <asp:Label ID="lblUidGender" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr id="trUidCareOf" runat="server">
                                                <td>Care of</td>
                                                <td>
                                                    <asp:Label ID="lblUidCareOf" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>Aadhaar</td>
                                                <td>
                                                    <asp:Label ID="lblUidAadhaar" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>Address</td>
                                                <td>
                                                    <asp:Label ID="lblUidAddress" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>Pincode</td>
                                                <td>
                                                    <asp:Label ID="lblUidPincode" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr id="trUidTxnId" runat="server" visible="false">
                                                <td>Reference</td>
                                                <td>
                                                    <asp:Label ID="lblUidTxnId" runat="server" Style="font-size: 11px; opacity: .8;"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>

                            <div class="rw-foot">
                                <asp:Button ID="btnAadhaarBack" runat="server" Text="Back" CssClass="btn btn-rw-ghost"
                                    CausesValidation="false" OnClick="btnAadhaarBack_Click" />
                                <div class="rw-foot-right">
                                    <%-- Shown only once the Aadhaar is verified, for the same reason as on the
                                         PAN step: skipping would throw away a verification already paid for. --%>
                                    <asp:Button ID="btnAadhaarEdit" runat="server" Text="Change Aadhaar" CssClass="btn btn-rw-ghost"
                                        Visible="false" CausesValidation="false" OnClick="btnAadhaarEdit_Click" />
                                    <asp:Button ID="btnAadhaarSkip" runat="server" Text="Skip for now" CssClass="btn btn-rw-skip"
                                        CausesValidation="false" OnClick="btnAadhaarSkip_Click" OnClientClick="return onAadhaarSkip();" />
                                    <asp:Button ID="btnAadhaarNext" runat="server" Text="Continue" CssClass="btn btn-rw-primary"
                                        CausesValidation="false" OnClick="btnAadhaarNext_Click" />
                                </div>
                                <p class="rw-foot-note">
                                    <asp:Literal ID="litAadhaarFootNote" runat="server"></asp:Literal>
                                </p>
                            </div>
                        </div>

                        <%-- -------------------- STEP 4 : TERMS & SUBMIT -------------------- --%>
                        <div id="pnlStep4" runat="server" visible="false" class="rw-review">
                            <div class="rw-head">
                                <p class="rw-eyebrow">Step 4 of 4</p>
                                <h3 class="rw-title">Review &amp; Confirm</h3>
                                <p class="rw-sub">Check everything below, accept the terms and complete your joining.</p>
                            </div>

                            <div class="rw-body">
                                <%-- Full Name. Shown only when both PAN and Aadhaar were skipped; otherwise
                                     the name comes from the verified KYC and is just listed in the table. --%>
                                <div id="divNameEntry" runat="server" visible="false" class="row g-3" style="margin-bottom: 18px;">
                                    <div class="col-12">
                                        <label class="form-label">Full Name<span class="rw-req">*</span></label>
                                        <asp:TextBox ID="txtFrstNm" runat="server" CssClass="form-control" MaxLength="100"
                                            autocomplete="off" TabIndex="6" placeholder="As per your official documents"></asp:TextBox>
                                        <small class="rw-hint">You skipped PAN and Aadhaar, so please enter your name here.</small>
                                    </div>
                                </div>

                                <table class="rw-kv">
                                    <tr>
                                        <td>Referral ID</td>
                                        <td>
                                            <asp:Label ID="lblSumRefral" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Name</td>
                                        <td>
                                            <asp:Label ID="lblSumName" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Mobile No.</td>
                                        <td>
                                            <asp:Label ID="lblSumMobile" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>E-Mail ID</td>
                                        <td>
                                            <asp:Label ID="lblSumEmail" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>PAN KYC</td>
                                        <td>
                                            <asp:Label ID="lblSumPan" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Aadhaar KYC</td>
                                        <td>
                                            <asp:Label ID="lblSumAadhaar" runat="server"></asp:Label></td>
                                    </tr>
                                </table>

                                <div class="rw-consent rw-consent-lg" style="margin-top: 22px;">
                                    <asp:CheckBox ID="chkterms" runat="server" TabIndex="53" />
                                    <label for="<%=chkterms.ClientID%>">
                                        I have read and agree to the
                                        <a href="#" data-toggle="modal" data-target="#myModalTerm">Terms and Conditions</a>.
                                    </label>
                                </div>

                                <asp:Label ID="errMsg" runat="server" CssClass="rw-alert rw-alert-error"></asp:Label>
                            </div>

                            <div class="rw-foot" id="DivTerms" runat="server" visible="true">
                                <asp:Button ID="btnStep4Back" runat="server" Text="Back" CssClass="btn btn-rw-ghost"
                                    CausesValidation="false" OnClick="btnStep4Back_Click" />
                                <div class="rw-foot-right">
                                    <asp:Button ID="CmdSave" runat="server" Text="Complete Joining" CssClass="btn btn-rw-ok"
                                        TabIndex="54" OnClick="CmdSave_Click" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <%-- Legacy e-mail OTP path. Hidden, kept for the existing handlers. --%>
                <asp:Button ID="BtnOtp" runat="server" Text="Submit" CssClass="btn btn-rw-primary" Visible="false"
                    ValidationGroup="eInformation" OnClick="BtnOtp_Click" />
                <asp:Button ID="ResendOtp" runat="server" Text="Resend Otp" CssClass="btn btn-rw-ghost"
                    Visible="false" ValidationGroup="eInformation" OnClick="ResendOtp_Click" />

            </div>
        </div>
    </div>
</asp:Content>
