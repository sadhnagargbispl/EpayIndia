<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="Apppurchase-coupon-details.aspx.cs" Inherits="Apppurchase_coupon_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="demoepay/css/mobile-app.css?v=1.5" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <style>
        /* ═══ COUPON DETAILS SPECIFIC ═══ */

        .cd-preview {
            margin: 16px;
            background: #fff;
            border-radius: var(--radius);
            overflow: hidden;
            box-shadow: var(--shadow);
        }

        .cd-preview-stripe {
            height: 6px;
            background: linear-gradient(90deg, var(--primary), var(--accent));
        }

        .cd-preview-body {
            display: flex;
            align-items: stretch;
        }

        .cd-preview-left {
            background: linear-gradient(160deg, #fff7ed, #ffedd5);
            padding: 22px 18px;
            min-width: 120px;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
        }

        .cd-preview-icon {
            font-size: 2rem;
            margin-bottom: 8px;
            color: var(--primary);
        }

        .cd-preview-pct {
            font-size: 2.2rem;
            font-weight: 800;
            color: var(--primary);
            line-height: 1;
        }

        .cd-preview-off {
            font-size: .65rem;
            font-weight: 700;
            letter-spacing: .8px;
            text-transform: uppercase;
            color: var(--muted);
            margin-top: 4px;
        }

        .cd-preview-div {
            width: 18px;
            position: relative;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            .cd-preview-div::before, .cd-preview-div::after {
                content: '';
                position: absolute;
                width: 16px;
                height: 16px;
                background: var(--bg);
                border-radius: 50%;
            }

            .cd-preview-div::before {
                top: -8px;
            }

            .cd-preview-div::after {
                bottom: -8px;
            }

        .cd-preview-div-line {
            width: 1.5px;
            height: 75%;
            border-left: 1.5px dashed #d1d5db;
        }

        .cd-preview-right {
            flex: 1;
            padding: 18px 18px 18px 12px;
            display: flex;
            flex-direction: column;
            justify-content: center;
            gap: 8px;
        }

        .cd-preview-qty {
            font-size: 1rem;
            font-weight: 800;
            color: var(--text);
        }

            .cd-preview-qty span {
                font-size: .75rem;
                font-weight: 500;
                color: var(--muted);
            }

        .cd-preview-orig {
            font-size: .9rem;
            font-weight: 700;
            color: var(--muted);
            text-decoration: line-through;
            text-decoration-color: #dc2626;
            text-decoration-thickness: 1.5px;
        }

        .cd-preview-final {
            font-size: 1.4rem;
            font-weight: 800;
            color: var(--text);
        }

        .cd-preview-save {
            display: inline-flex;
            align-items: center;
            font-size: .7rem;
            font-weight: 700;
            padding: 3px 10px;
            border-radius: 50px;
            background: rgba(220,38,38,.1);
            color: #dc2626;
            border: 1px solid rgba(220,38,38,.2);
            width: fit-content;
        }

        /* Card */
        .cd-card {
            margin: 0 16px 14px;
            background: #fff;
            border-radius: var(--radius);
            padding: 18px;
            box-shadow: var(--shadow);
        }

        .cd-card-title {
            font-size: .72rem;
            font-weight: 800;
            letter-spacing: .8px;
            text-transform: uppercase;
            color: var(--muted);
            margin-bottom: 14px;
            padding-bottom: 12px;
            border-bottom: 1px solid var(--border);
            display: flex;
            align-items: center;
            gap: 8px;
        }

            .cd-card-title i {
                color: var(--primary);
            }

        .cd-coupon-title {
            font-size: 1.15rem;
            font-weight: 800;
            color: var(--text);
            margin-bottom: 6px;
            line-height: 1.3;
        }

            .cd-coupon-title span {
                color: var(--primary);
            }

        .cd-coupon-desc {
            color: var(--muted);
            font-size: .82rem;
            line-height: 1.6;
        }

        /* Info rows */
        .cd-info-rows {
            display: flex;
            flex-direction: column;
            gap: 10px;
        }

        .cd-info-row {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 12px 14px;
            background: #f9fafb;
            border: 1px solid var(--border);
            border-radius: 12px;
        }

        .cd-info-icon {
            width: 36px;
            height: 36px;
            border-radius: 10px;
            background: rgba(232,64,0,.1);
            border: 1px solid rgba(232,64,0,.2);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: .95rem;
            color: var(--primary);
            flex-shrink: 0;
        }

        .cd-info-key {
            font-size: .68rem;
            font-weight: 700;
            color: var(--muted);
            text-transform: uppercase;
            letter-spacing: .5px;
            margin-bottom: 2px;
        }

        .cd-info-val {
            font-size: .92rem;
            font-weight: 800;
            color: var(--text);
        }

        .cd-info-row.amount {
            background: linear-gradient(135deg, #fff7ed, #ffedd5);
            border-color: rgba(232,64,0,.25);
        }

            .cd-info-row.amount .cd-info-val {
                color: var(--primary);
                font-size: 1.05rem;
            }

        /* Summary */
        .cd-summary-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            font-size: .85rem;
            padding: 7px 0;
            border-bottom: 1px dashed var(--border);
        }

            .cd-summary-row:last-child {
                border-bottom: none;
                font-weight: 800;
                font-size: .95rem;
                padding-top: 10px;
            }

            .cd-summary-row span:first-child {
                color: var(--muted);
            }

            .cd-summary-row span:last-child {
                font-weight: 700;
                color: var(--text);
            }

            .cd-summary-row.total span:last-child {
                color: var(--primary);
                font-size: 1.05rem;
            }

            .cd-summary-row.discount span:last-child {
                color: #dc2626;
            }

            .cd-summary-row.wallet span:last-child {
                color: var(--green);
            }

        /* ── Wallet Balance Row (inside summary card, after You Pay) ── */
        .cd-wallet-inline {
            margin-top: 14px;
            padding: 12px 14px;
            border-radius: 12px;
            background: linear-gradient(135deg, #f0fdf4, #dcfce7);
            border: 1px solid rgba(22,163,74,.25);
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 10px;
        }

        .cd-wallet-inline-left {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .cd-wallet-inline-icon {
            width: 34px;
            height: 34px;
            border-radius: 9px;
            background: rgba(22,163,74,.15);
            border: 1px solid rgba(22,163,74,.3);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: .88rem;
            color: #16a34a;
            flex-shrink: 0;
        }

        .cd-wallet-inline-key {
            font-size: .66rem;
            font-weight: 700;
            color: #15803d;
            text-transform: uppercase;
            letter-spacing: .5px;
            margin-bottom: 2px;
        }

        .cd-wallet-inline-val {
            font-size: .9rem;
            font-weight: 800;
            color: #15803d;
        }

        .cd-wallet-toggle-wrap {
            display: flex;
            align-items: center;
            gap: 7px;
            flex-shrink: 0;
        }

        .cd-wallet-toggle-label {
            font-size: .72rem;
            font-weight: 700;
            color: #15803d;
        }

        /* Toggle switch */
        .cd-toggle {
            position: relative;
            width: 42px;
            height: 24px;
            flex-shrink: 0;
        }

            .cd-toggle input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .cd-toggle-slider {
            position: absolute;
            inset: 0;
            background: #d1d5db;
            border-radius: 24px;
            cursor: pointer;
            transition: background .25s;
        }

            .cd-toggle-slider::before {
                content: '';
                position: absolute;
                width: 18px;
                height: 18px;
                border-radius: 50%;
                background: #fff;
                top: 3px;
                left: 3px;
                transition: transform .25s;
                box-shadow: 0 1px 4px rgba(0,0,0,.18);
            }

        .cd-toggle input:checked + .cd-toggle-slider {
            background: #16a34a;
        }

            .cd-toggle input:checked + .cd-toggle-slider::before {
                transform: translateX(18px);
            }

        /* Pay Button (inside summary card, below wallet row) */
        .cd-pay-btn-wrap {
            margin-top: 14px;
        }

        .cd-pay-btn-full {
            width: 100%;
            background: var(--primary);
            color: #fff;
            border: none;
            padding: 14px 0;
            border-radius: 50px;
            font-size: .92rem;
            font-weight: 800;
            cursor: pointer;
            box-shadow: 0 4px 14px rgba(232,64,0,.35);
            transition: all .2s;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
        }

            .cd-pay-btn-full:hover {
                background: var(--primary-d);
                transform: translateY(-1px);
            }

        .cd-pay-btn-sub {
            text-align: center;
            font-size: .7rem;
            color: var(--muted);
            margin-top: 8px;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 5px;
        }

        /* Tabs */
        .cd-tabs-nav {
            display: flex;
            margin: 0 16px;
            background: #fff;
            border-radius: var(--radius);
            overflow: hidden;
            box-shadow: var(--shadow);
            margin-bottom: 14px;
        }

        .cd-tab-btn {
            flex: 1;
            padding: 13px 8px;
            text-align: center;
            font-size: .78rem;
            font-weight: 700;
            color: var(--muted);
            background: #fff;
            border: none;
            cursor: pointer;
            border-bottom: 3px solid transparent;
            transition: all .2s;
        }

            .cd-tab-btn.active {
                color: var(--primary);
                border-bottom-color: var(--primary);
                background: #fff7ed;
            }

        .cd-tab-content {
            display: none;
        }

            .cd-tab-content.active {
                display: block;
            }

            .cd-tab-content p {
                color: var(--muted);
                font-size: .85rem;
                line-height: 1.7;
                margin-bottom: 10px;
            }

            .cd-tab-content ul {
                list-style: none;
                display: flex;
                flex-direction: column;
                gap: 9px;
            }

                .cd-tab-content ul li {
                    display: flex;
                    align-items: flex-start;
                    gap: 9px;
                    color: var(--muted);
                    font-size: .82rem;
                    line-height: 1.55;
                }

                    .cd-tab-content ul li::before {
                        content: '✓';
                        flex-shrink: 0;
                        width: 18px;
                        height: 18px;
                        border-radius: 50%;
                        background: var(--primary);
                        color: #fff;
                        font-size: .6rem;
                        font-weight: 800;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        margin-top: 1px;
                    }

                    .cd-tab-content ul li.tc::before {
                        content: '•';
                        background: var(--muted);
                        font-size: .75rem;
                    }

        .cd-secure {
            padding: 12px 16px 16px;
            text-align: center;
            font-size: .72rem;
            color: var(--muted);
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- HERO -->
    <div class="hero">
        <div class="hero-badge">
            <div class="hero-badge-dot"></div>
            Coupon Package
       
            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
        </div>
        <div class="hero-title">Food / <span>Movie Package</span></div>
        <asp:Label ID="LblPackageName" runat="server" Text=""></asp:Label>
        <div class="hero-sub">Review your coupon and complete payment securely</div>
    </div>

    <!-- COUPON PREVIEW -->
    <div class="cd-preview">
        <div class="cd-preview-stripe"></div>
        <div class="cd-preview-body">
            <div class="cd-preview-left">
                <div class="cd-preview-icon">
                    <i class="fa-solid fa-burger"></i>
                </div>
                <div class="cd-preview-pct">
                    <asp:Label ID="LblDiscount" runat="server" Text=""></asp:Label>%</div>
                <div class="cd-preview-off">Off</div>
            </div>
            <div class="cd-preview-div">
                <div class="cd-preview-div-line"></div>
            </div>
            <div class="cd-preview-right">
                <div class="cd-preview-qty">
                    <asp:Label ID="Lbltickets" runat="server" Text="" Style="font-family: 'Sora', sans-serif; font-size: .95rem; font-weight: 800; color: var(--text);"></asp:Label>
                    Coupons <span>/ Pack</span></div>
                <div class="cd-preview-orig">&#8377;<asp:Label ID="LblAMount" runat="server" Text=""></asp:Label></div>
                <div class="cd-preview-final">&#8377;<asp:Label ID="Lblwallet" runat="server" Text=""></asp:Label></div>
                <div class="cd-preview-save">Save &#8377;<asp:Label ID="LblSaveamount" runat="server" Text=""></asp:Label></div>
            </div>
        </div>
    </div>

    <!-- DESCRIPTION CARD -->
    <div class="cd-card">
        <div class="cd-coupon-title">Food Booking / <span>Movie Package</span></div>
        <div class="cd-coupon-desc">
            Discover the ultimate convenience with our all-in-one platform — shop online, manage utilities,
            book holidays and movies, order food, and find perfect gift vouchers effortlessly.
       
        </div>
    </div>

    <!-- BOOKING DETAILS -->
    <div class="cd-card">
        <div class="cd-card-title">
            <i class="fa-solid fa-clipboard-list"></i>Booking Details
       
        </div>
        <div class="cd-info-rows">

            <div class="cd-info-row">
                <div class="cd-info-icon"><i class="fa-solid fa-user"></i></div>
                <div>
                    <div class="cd-info-key">User Name</div>
                    <div class="cd-info-val">
                        <asp:Label ID="LblUserName" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>

            <div class="cd-info-row">
                <div class="cd-info-icon"><i class="fa-solid fa-id-card"></i></div>
                <div>
                    <div class="cd-info-key">ID Number</div>
                    <div class="cd-info-val">
                        <asp:Label ID="LblUserID" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>

            <div class="cd-info-row amount">
                <div class="cd-info-icon" style="background: rgba(232,64,0,.15); border-color: rgba(232,64,0,.3);">
                    <i class="fa-solid fa-wallet"></i>
                </div>
                <div>
                    <div class="cd-info-key">Package Amount</div>
                    <div class="cd-info-val">
                        &#8377;
                        <asp:Label ID="LblPakageAmount" runat="server" Text="0.00"></asp:Label>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <!-- ORDER SUMMARY — with wallet + button inside -->
    <div class="cd-card">
        <div class="cd-card-title">
            <i class="fa-solid fa-receipt"></i>Order Summary
       
        </div>

        <div class="cd-summary-row">
            <span>Original Price</span>
            <span>&#8377;<asp:Label ID="OriginalPrice" runat="server" Text=""></asp:Label></span>
        </div>
        <div class="cd-summary-row discount">
            <span>Discount ((<asp:Label ID="Discount" runat="server" Text="" Style="color: var(--muted);"></asp:Label>%)</span>
            <span>&minus; &#8377;<asp:Label ID="DiscountLess" runat="server" Text="" Style="color: #dc2626;"></asp:Label></span>
        </div>
        <div class="cd-summary-row wallet" style="display: none;">
            <span>Wallet Credit</span>
            <span id="summaryWalletCredit">&#8377;<asp:Label ID="LblWalletCredit" runat="server" Text="" Style="color: var(--green);"></asp:Label></span>
        </div>
        <div class="cd-summary-row total">
            <span>You Pay</span>
            <span id="summaryYouPay">&#8377;<asp:Label ID="LblYouPay" runat="server" Text=""></asp:Label></span>
        </div>

        <!-- ── Available Wallet Balance (right after You Pay) ── -->
        <div class="cd-wallet-inline">
            <div class="cd-wallet-inline-left">
                <div class="cd-wallet-inline-icon"><i class="fa-solid fa-wallet"></i></div>
                <div>
                    <div class="cd-wallet-inline-key">ePay Wallet Balance</div>
                    <div class="cd-wallet-inline-val">
                        &#8377;<asp:Label ID="Lblwalletbalance" runat="server" Text="0.00"></asp:Label>
                        available
                    </div>
                </div>
            </div>
            <div class="cd-wallet-toggle-wrap">
                <span class="cd-wallet-toggle-label" id="toggleLabel">Apply</span>
                <label class="cd-toggle">
                    <input type="checkbox" id="walletToggle" checked />
                    <span class="cd-toggle-slider"></span>
                </label>
            </div>
        </div>

        <!-- ── Pay Now Button (right below wallet row) ── -->
        <div class="cd-pay-btn-wrap">
            <asp:TextBox ID="TxtAmount" runat="server" class="form-control" aria-describedby="emailHelp"
                placeholder="Enter Amount" AutoPostBack="true" Style="width: 80%; margin-right: 10px;"
                ReadOnly="true" Visible="false"></asp:TextBox>
            <asp:Button ID="BtnProceedToPay" runat="server" Text="Process to Pay" class="cd-pay-btn-full" OnClick="BtnProceedToPay_Click" />
            <%--      <button class="cd-pay-btn-full" onclick="processPay()">
                <i class="fa-solid fa-credit-card"></i>
                Pay Now &mdash; <span id="payBtnAmount">&#8377;10,000</span>
            </button>--%>
            <div class="cd-pay-btn-sub">
                <i class="fa-solid fa-lock"></i>100% Secure &amp; Encrypted Payment
           
            </div>
        </div>

    </div>

    <!-- TABS -->
    <div class="cd-tabs-nav">
        <button type="button" class="cd-tab-btn active" onclick="switchTab(this,'tab-desc')">
            <i class="fa-solid fa-file-lines"></i>Description
       
        </button>
        <button type="button" class="cd-tab-btn" onclick="switchTab(this,'tab-how')">
            <i class="fa-solid fa-book-open"></i>How to Use
       
        </button>
        <button type="button" class="cd-tab-btn" onclick="switchTab(this,'tab-tc')">
            <i class="fa-solid fa-scroll"></i>T&amp;C
       
        </button>
    </div>

    <div class="cd-card">
        <div class="cd-tab-content active" id="tab-desc">
            <p>
                This coupon package gives you access to exclusive food and movie bookings at the best prices.
                Purchase in bulk and enjoy maximum savings with ePay's Ecommerce Wallet balance credited directly to your account.
           
            </p>
            <ul>
                <li>Instant digital delivery after purchase</li>
                <li>Usable at 1000+ partner restaurants &amp; cinemas</li>
                <li>Ecommerce Wallet balance credited immediately</li>
                <li>Savings up to 50% on total coupon value</li>
            </ul>
        </div>

        <div class="cd-tab-content" id="tab-how">
            <ul>
                <li>Login to your ePay account and go to "My Coupons" section.</li>
                <li>Select the coupon you wish to use at checkout.</li>
                <li>At the restaurant or cinema, show your digital coupon code at the counter.</li>
                <li>The discount will be applied automatically on the total bill amount.</li>
                <li>Remaining wallet balance stays in your ePay account for future use.</li>
                <li>Contact ePay support for any assistance during redemption.</li>
            </ul>
        </div>

        <div class="cd-tab-content" id="tab-tc">
            <ul>
                <li class="tc">Coupons are non-transferable and cannot be exchanged for cash.</li>
                <li class="tc">Each coupon is valid for single use only unless stated otherwise.</li>
                <li class="tc">ePay reserves the right to cancel coupons in case of fraudulent activity.</li>
                <li class="tc">Validity period starts from the date of activation, not purchase.</li>
                <li class="tc">Coupons cannot be clubbed with any other ongoing offer or discount.</li>
                <li class="tc">Refunds will only be processed as per ePay's refund policy.</li>
                <li class="tc">ePay Digital India Pvt. Ltd. is the final authority on all disputes.</li>
            </ul>
        </div>
    </div>

    <script>
        function switchTab(btn, tabId) {
            document.querySelectorAll('.cd-tab-btn').forEach(function (b) { b.classList.remove('active'); });
            document.querySelectorAll('.cd-tab-content').forEach(function (t) { t.classList.remove('active'); });
            btn.classList.add('active');
            document.getElementById(tabId).classList.add('active');
        }

        function applyWalletBalance() {
            var isChecked = document.getElementById('walletToggle').checked;
            var label = document.getElementById('toggleLabel');
            var sumCredit = document.getElementById('summaryWalletCredit');
            var sumYouPay = document.getElementById('summaryYouPay');
            var btnAmount = document.getElementById('payBtnAmount');

            if (isChecked) {
                label.textContent = 'Applied';
                sumCredit.textContent = '\u20B96,000';
                sumYouPay.textContent = '\u20B94,000';
                btnAmount.textContent = '\u20B94,000';
            } else {
                label.textContent = 'Apply';
                sumCredit.textContent = '\u20B90';
                sumYouPay.textContent = '\u20B910,000';
                btnAmount.textContent = '\u20B910,000';
            }
        }

        function processPay() {
            var useWallet = document.getElementById('walletToggle').checked;
            window.location.href = 'Apppayment.aspx?useWallet=' + (useWallet ? '1' : '0');
        }
    </script>

</asp:Content>
