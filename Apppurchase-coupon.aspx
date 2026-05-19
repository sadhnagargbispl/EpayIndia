<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="Apppurchase-coupon.aspx.cs" Inherits="Apppurchase_coupon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="demoepay/css/mobile-app.css?v=1.5" rel="stylesheet" />
    <style>
        /* ═══ PURCHASE COUPON SPECIFIC ═══ */

        /* Category Tabs */
        .cat-tabs {
            display: flex;
            gap: 8px;
            padding: 14px 16px 4px;
            overflow-x: auto;
            scrollbar-width: none;
        }

            .cat-tabs::-webkit-scrollbar {
                display: none;
            }

        .cat-tab {
            flex-shrink: 0;
            background: #fff;
            border: 1.5px solid var(--border);
            border-radius: 50px;
            padding: 9px 18px;
            font-size: .78rem;
            font-weight: 700;
            color: var(--text);
            cursor: pointer;
            white-space: nowrap;
            transition: all .2s;
            display: flex;
            align-items: center;
            gap: 6px;
        }

            .cat-tab.active {
                background: var(--primary);
                color: #fff;
                border-color: var(--primary);
                box-shadow: 0 4px 12px rgba(232,64,0,.25);
            }

        /* Coupon List */
        .coupon-list {
            padding: 8px 16px 16px;
            display: flex;
            flex-direction: column;
            gap: 14px;
        }

        .coupon-card {
            background: #fff;
            border-radius: var(--radius);
            overflow: hidden;
            box-shadow: var(--shadow);
            transition: transform .2s, box-shadow .2s;
            cursor: pointer;
            text-decoration: none;
            color: var(--text);
            display: block;
        }

            .coupon-card:active {
                transform: scale(.98);
            }

            .coupon-card:hover {
                box-shadow: 0 8px 24px rgba(0,0,0,.12);
            }

        .coupon-stripe {
            height: 5px;
        }

        .coupon-card.food .coupon-stripe {
            background: linear-gradient(90deg, var(--primary), var(--accent));
        }

        .coupon-card.movie .coupon-stripe {
            background: linear-gradient(90deg, var(--green), #4ade80);
        }

        .coupon-body {
            display: flex;
            align-items: stretch;
        }

        .coupon-left {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            padding: 18px 14px;
            min-width: 100px;
            flex-shrink: 0;
        }

        .coupon-card.food .coupon-left {
            background: linear-gradient(160deg, #fff7ed, #ffedd5);
        }

        .coupon-card.movie .coupon-left {
            background: linear-gradient(160deg, #f0fdf4, #dcfce7);
        }

        .coupon-icon-lg {
            font-size: 1.6rem;
            margin-bottom: 6px;
        }

        .coupon-pct {
            font-size: 1.8rem;
            font-weight: 800;
            line-height: 1;
        }

        .coupon-card.food .coupon-pct {
            color: var(--primary);
        }

        .coupon-card.movie .coupon-pct {
            color: var(--green);
        }

        .coupon-off {
            font-size: .6rem;
            font-weight: 700;
            letter-spacing: .8px;
            text-transform: uppercase;
            color: var(--muted);
            margin-top: 3px;
        }

        .coupon-divider {
            width: 16px;
            flex-shrink: 0;
            position: relative;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            .coupon-divider::before, .coupon-divider::after {
                content: '';
                position: absolute;
                width: 14px;
                height: 14px;
                background: var(--bg);
                border-radius: 50%;
            }

            .coupon-divider::before {
                top: -7px;
            }

            .coupon-divider::after {
                bottom: -7px;
            }

        .coupon-divider-line {
            width: 1.5px;
            height: 70%;
            border-left: 1.5px dashed #d1d5db;
        }

        .coupon-right {
            flex: 1;
            padding: 14px 14px 14px 10px;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            gap: 8px;
            min-width: 0;
        }

        .coupon-qty {
            font-size: .92rem;
            font-weight: 800;
            color: var(--text);
            line-height: 1.3;
        }

            .coupon-qty span {
                font-size: .7rem;
                font-weight: 500;
                color: var(--muted);
            }

        .coupon-price-row {
            display: flex;
            align-items: baseline;
            gap: 8px;
            flex-wrap: wrap;
        }

        .coupon-orig {
            font-size: .82rem;
            font-weight: 700;
            color: var(--muted);
            text-decoration: line-through;
            text-decoration-color: #dc2626;
            text-decoration-thickness: 1.5px;
        }

        .coupon-final {
            font-size: 1.15rem;
            font-weight: 800;
            color: var(--text);
        }

        .coupon-meta {
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .coupon-save {
            font-size: .65rem;
            font-weight: 700;
            padding: 3px 9px;
            border-radius: 50px;
            background: rgba(220,38,38,.1);
            color: #dc2626;
            border: 1px solid rgba(220,38,38,.2);
        }

        .coupon-arrow {
            margin-left: auto;
            color: var(--primary);
            font-size: .9rem;
        }

        /* Empty state */
        .empty-state {
            padding: 40px 20px;
            text-align: center;
            color: var(--muted);
        }

            .empty-state i {
                font-size: 2.5rem;
                color: #d1d5db;
                margin-bottom: 12px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- HERO -->
    <div class="hero">
        <div class="hero-badge">
            <div class="hero-badge-dot"></div>
            Purchase Coupon
   
        </div>
        <div class="hero-title">Buy <span>Coupons</span> &amp; Save More</div>
        <div class="hero-sub">Bulk food &amp; movie coupons with exclusive Ecommerce Wallet discounts</div>
    </div>

    <!-- CATEGORY TABS -->
    <div class="cat-tabs">
        <button type="button" class="cat-tab active" onclick="filterCoupons(this, 'all')">
            <i class="fa fa-th-large"></i>All Coupons
   
        </button>
        <button type="button" class="cat-tab" onclick="filterCoupons(this, 'movie')">
            <i class="fa fa-film"></i>Movies
   
        </button>
        <button type="button" class="cat-tab" onclick="filterCoupons(this, 'food')">
            <i class="fa fa-utensils"></i>Food
   
        </button>
    </div>

    <!-- MOVIE COUPONS -->
    <div class="sec-label" data-section="movie">🎬 Movies Portal</div>
    <div class="coupon-list" data-section="movie">
        <asp:Repeater ID="RepMovies" runat="server">
            <ItemTemplate>
                <a href='<%#String.Format("Apppurchase-coupon-details.aspx?KitID={0}", HttpUtility.UrlEncode(Crypto.Encrypt(Eval("kitid").ToString()))) %>' class="coupon-card movie">
                    <div class="coupon-stripe"></div>
                    <div class="coupon-body">
                        <div class="coupon-left">
                            <div class="coupon-icon-lg">🎬</div>
                            <div class="coupon-pct"><%#Eval("discount")%>%</div>
                            <div class="coupon-off">Off</div>
                        </div>
                        <div class="coupon-divider">
                            <div class="coupon-divider-line"></div>
                        </div>
                        <div class="coupon-right">
                            <div class="coupon-qty"><%#Eval("Tickets")%> Movie Tickets <span>/ Pack</span></div>
                            <div class="coupon-price-row">
                                <span class="coupon-orig">₹<%#Eval("couponprice")%></span>
                                <span class="coupon-final">₹<%#Eval("kitamount")%></span>
                            </div>
                            <div class="coupon-meta">
                                <span class="coupon-save">Save ₹<%#Eval("saveamount")%></span>
                                <span class="coupon-arrow"><i class="fa fa-arrow-right"></i></span>
                            </div>
                        </div>
                    </div>
                </a>
             
            </ItemTemplate>
        </asp:Repeater>
        <%--  <a href="Apppurchase-coupon-details.aspx" class="coupon-card movie">
            <div class="coupon-stripe"></div>
            <div class="coupon-body">
                <div class="coupon-left">
                    <div class="coupon-icon-lg">🎬</div>
                    <div class="coupon-pct">30%</div>
                    <div class="coupon-off">Off</div>
                </div>
                <div class="coupon-divider">
                    <div class="coupon-divider-line"></div>
                </div>
                <div class="coupon-right">
                    <div class="coupon-qty">10 Movie Tickets <span>/ Pack</span></div>
                    <div class="coupon-price-row">
                        <span class="coupon-orig">₹3,000</span>
                        <span class="coupon-final">₹2,100</span>
                    </div>
                    <div class="coupon-meta">
                        <span class="coupon-save">Save ₹900</span>
                        <span class="coupon-arrow"><i class="fa fa-arrow-right"></i></span>
                    </div>
                </div>
            </div>
        </a>--%>

        <%--        <a href="Apppurchase-coupon-details.aspx" class="coupon-card movie">
            <div class="coupon-stripe"></div>
            <div class="coupon-body">
                <div class="coupon-left">
                    <div class="coupon-icon-lg">🎬</div>
                    <div class="coupon-pct">40%</div>
                    <div class="coupon-off">Off</div>
                </div>
                <div class="coupon-divider">
                    <div class="coupon-divider-line"></div>
                </div>
                <div class="coupon-right">
                    <div class="coupon-qty">20 Movie Tickets <span>/ Pack</span></div>
                    <div class="coupon-price-row">
                        <span class="coupon-orig">₹6,000</span>
                        <span class="coupon-final">₹3,600</span>
                    </div>
                    <div class="coupon-meta">
                        <span class="coupon-save">Save ₹2,400</span>
                        <span class="coupon-arrow"><i class="fa fa-arrow-right"></i></span>
                    </div>
                </div>
            </div>
        </a>

        <a href="Apppurchase-coupon-details.aspx" class="coupon-card movie">
            <div class="coupon-stripe"></div>
            <div class="coupon-body">
                <div class="coupon-left">
                    <div class="coupon-icon-lg">🎬</div>
                    <div class="coupon-pct">50%</div>
                    <div class="coupon-off">Off</div>
                </div>
                <div class="coupon-divider">
                    <div class="coupon-divider-line"></div>
                </div>
                <div class="coupon-right">
                    <div class="coupon-qty">40 Movie Tickets <span>/ Pack</span></div>
                    <div class="coupon-price-row">
                        <span class="coupon-orig">₹12,000</span>
                        <span class="coupon-final">₹6,000</span>
                    </div>
                    <div class="coupon-meta">
                        <span class="coupon-save">Save ₹6,000</span>
                        <span class="coupon-arrow"><i class="fa fa-arrow-right"></i></span>
                    </div>
                </div>
            </div>
        </a>--%>
    </div>

    <!-- FOOD COUPONS -->
    <div class="sec-label" data-section="food">🍔 Food Portal</div>
    <div class="coupon-list" data-section="food">
        <asp:Repeater ID="RepFood" runat="server">
            <ItemTemplate>
                <a href='<%#String.Format("Apppurchase-coupon-details.aspx?KitID={0}", HttpUtility.UrlEncode(Crypto.Encrypt(Eval("kitid").ToString()))) %>' class="coupon-card food">
            <div class="coupon-stripe"></div>
            <div class="coupon-body">
                <div class="coupon-left">
                    <div class="coupon-icon-lg">🍔</div>
                    <div class="coupon-pct"><%#Eval("discount")%>%</div>
                    <div class="coupon-off">Off</div>
                </div>
                <div class="coupon-divider">
                    <div class="coupon-divider-line"></div>
                </div>
                <div class="coupon-right">
                    <div class="coupon-qty"><%#Eval("Tickets")%> Food Coupons <span>/ Pack</span></div>
                    <div class="coupon-price-row">
                        <span class="coupon-orig">₹<%#Eval("couponprice")%></span>
                        <span class="coupon-final">₹<%#Eval("kitamount")%></span>
                    </div>
                    <div class="coupon-meta">
                        <span class="coupon-save">Save ₹<%#Eval("saveamount")%></span>
                        <span class="coupon-arrow"><i class="fa fa-arrow-right"></i></span>
                    </div>
                </div>
            </div>
        </a>
                <%--<div class="coupon-card food-card" data-anim>
                    <div class="coupon-stripe"></div>
                    <div class="coupon-body">
                        <div class="coupon-left">
                            <div class="coupon-icon-big">🍔</div>
                            <div class="coupon-disc-pct"><%#Eval("discount")%>%</div>
                            <div class="coupon-disc-label">OFF</div>
                        </div>
                        <div class="coupon-divider">
                            <div class="coupon-divider-line"></div>
                        </div>
                        <div class="coupon-right">
                            <div>
                                <div class="coupon-qty"><%#Eval("Tickets")%> Food Coupons <span>/ Pack</span></div>
                                <div class="coupon-price-row">
                                    <div class="coupon-price">₹<%#Eval("couponprice")%></div>
                                </div>
                                <div class="coupon-wallet-price">₹<%#Eval("kitamount")%></div>
                                <div class="coupon-tags">
                                    <span class="coupon-tag save">Save ₹<%#Eval("saveamount")%></span>
                                </div>
                            </div>
                            <a href='<%#String.Format("purchase-coupon-details.aspx?KitID={0}", HttpUtility.UrlEncode(Crypto.Encrypt(Eval("kitid").ToString()))) %>' class="coupon-btn">Buy Now →</a>
                        </div>
                    </div>
                </div>--%>
            </ItemTemplate>
        </asp:Repeater>
        <%--  <a href="Apppurchase-coupon-details.aspx" class="coupon-card food">
            <div class="coupon-stripe"></div>
            <div class="coupon-body">
                <div class="coupon-left">
                    <div class="coupon-icon-lg">🍔</div>
                    <div class="coupon-pct">20%</div>
                    <div class="coupon-off">Off</div>
                </div>
                <div class="coupon-divider">
                    <div class="coupon-divider-line"></div>
                </div>
                <div class="coupon-right">
                    <div class="coupon-qty">5 Food Coupons <span>/ Pack</span></div>
                    <div class="coupon-price-row">
                        <span class="coupon-orig">₹2,500</span>
                        <span class="coupon-final">₹2,000</span>
                    </div>
                    <div class="coupon-meta">
                        <span class="coupon-save">Save ₹500</span>
                        <span class="coupon-arrow"><i class="fa fa-arrow-right"></i></span>
                    </div>
                </div>
            </div>
        </a>

        <a href="Apppurchase-coupon-details.aspx" class="coupon-card food">
            <div class="coupon-stripe"></div>
            <div class="coupon-body">
                <div class="coupon-left">
                    <div class="coupon-icon-lg">🍔</div>
                    <div class="coupon-pct">30%</div>
                    <div class="coupon-off">Off</div>
                </div>
                <div class="coupon-divider">
                    <div class="coupon-divider-line"></div>
                </div>
                <div class="coupon-right">
                    <div class="coupon-qty">10 Food Coupons <span>/ Pack</span></div>
                    <div class="coupon-price-row">
                        <span class="coupon-orig">₹5,000</span>
                        <span class="coupon-final">₹3,500</span>
                    </div>
                    <div class="coupon-meta">
                        <span class="coupon-save">Save ₹1,500</span>
                        <span class="coupon-arrow"><i class="fa fa-arrow-right"></i></span>
                    </div>
                </div>
            </div>
        </a>

        <a href="Apppurchase-coupon-details.aspx" class="coupon-card food">
            <div class="coupon-stripe"></div>
            <div class="coupon-body">
                <div class="coupon-left">
                    <div class="coupon-icon-lg">🍔</div>
                    <div class="coupon-pct">40%</div>
                    <div class="coupon-off">Off</div>
                </div>
                <div class="coupon-divider">
                    <div class="coupon-divider-line"></div>
                </div>
                <div class="coupon-right">
                    <div class="coupon-qty">20 Food Coupons <span>/ Pack</span></div>
                    <div class="coupon-price-row">
                        <span class="coupon-orig">₹10,000</span>
                        <span class="coupon-final">₹6,000</span>
                    </div>
                    <div class="coupon-meta">
                        <span class="coupon-save">Save ₹4,000</span>
                        <span class="coupon-arrow"><i class="fa fa-arrow-right"></i></span>
                    </div>
                </div>
            </div>
        </a>

        <a href="Apppurchase-coupon-details.aspx" class="coupon-card food">
            <div class="coupon-stripe"></div>
            <div class="coupon-body">
                <div class="coupon-left">
                    <div class="coupon-icon-lg">🍔</div>
                    <div class="coupon-pct">50%</div>
                    <div class="coupon-off">Off</div>
                </div>
                <div class="coupon-divider">
                    <div class="coupon-divider-line"></div>
                </div>
                <div class="coupon-right">
                    <div class="coupon-qty">50 Food Coupons <span>/ Pack</span></div>
                    <div class="coupon-price-row">
                        <span class="coupon-orig">₹25,000</span>
                        <span class="coupon-final">₹12,500</span>
                    </div>
                    <div class="coupon-meta">
                        <span class="coupon-save">Save ₹12,500</span>
                        <span class="coupon-arrow"><i class="fa fa-arrow-right"></i></span>
                    </div>
                </div>
            </div>
        </a>--%>
    </div>
</asp:Content>

