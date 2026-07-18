<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="WebApp.aspx.cs" Inherits="WebApp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="demoepay/css/AppWeb.css?v=1.7" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- ===== BANNER SLIDER (dynamic) ===== -->
    <div class="banner-section">
        <div class="banner-slider" id="bannerSlider">
            <div class="banner-track" id="bannerTrack">
                <asp:Repeater ID="rptBanner" runat="server">
                    <ItemTemplate>
                        <div class="banner-slide">
                            <div class="banner-inner <%# Eval("bg_class") %>">
                                <div class="banner-circle bc1"></div>
                                <div class="banner-circle bc2"></div>
                                <div class="banner-deco"><%# Eval("deco") %></div>
                                <div class="banner-text">
                                    <span class="bn-tag"><%# Eval("tag") %></span>
                                    <div class="bn-title"><%# Eval("title") %></div>
                                    <div class="bn-sub"><%# Eval("subtitle") %></div>
                                    <a href='<%# Eval("url") %>' target='<%# Target(Eval("target")) %>' class="bn-cta <%# Dark(Eval("cta_dark")) %>">
                                        <%# Eval("cta") %> <i class='<%# Eval("cta_icon") %>'></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="banner-nav" id="bannerNav">
                <asp:Repeater ID="rptBannerDots" runat="server">
                    <ItemTemplate>
                        <div class='bn-dot <%# Container.ItemIndex == 0 ? "active" : "" %>' onclick='goToSlide(<%# Container.ItemIndex %>)'></div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <button type="button" class="banner-arr bn-prev" onclick="prevSlide()"><i class="fa fa-chevron-left"></i></button>
            <button type="button" class="banner-arr bn-next" onclick="nextSlide()"><i class="fa fa-chevron-right"></i></button>
        </div>
    </div>

    <!-- ===== QUICK SHORTCUTS (dynamic) ===== -->
    <div class="sec-head">
        <h2><span class="sh-icon" style="background: linear-gradient(135deg,#E84000,#ff8c42); color: #fff;"><i class="fa fa-bolt"></i></span>Quick Access</h2>
        <a href="#services-section">See All <i class="fa fa-angle-right"></i></a>
    </div>
    <div class="quick-scroll">
        <asp:Repeater ID="rptQuick" runat="server">
            <ItemTemplate>
                <a class="quick-card" href='<%# Eval("url") %>' target='<%# Target(Eval("target")) %>'>
                    <div class="qc-ico <%# Eval("color") %>"><i class='<%# Eval("icon") %>'></i></div>
                    <div class="qc-label"><%# Eval("label") %></div>
                </a>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- ===== CATEGORY CHIPS (dynamic) ===== -->
    <div class="sec-head">
        <h2><span class="sh-icon" style="background: linear-gradient(135deg,#7c3aed,#a78bfa); color: #fff;"><i class="fa fa-th-large"></i></span>Categories</h2>
    </div>
    <div class="chips-row">
        <asp:Repeater ID="rptCategories" runat="server">
            <ItemTemplate>
                <a class="chip <%# ActiveChip(Eval("is_default")) %>" href='<%# Eval("url") %>' target='<%# Target(Eval("target")) %>'><i class='<%# Eval("icon") %>'></i><%# Eval("label") %></a>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- ===== FEATURED SERVICES (dynamic) ===== -->
    <div class="sec-head">
        <h2><span class="sh-icon" style="background: linear-gradient(135deg,#d97706,#fcd34d); color: #fff;"><i class="fa fa-star"></i></span>Featured Services</h2>
        <a href="StoreRedirect.aspx" target="_blank">View All <i class="fa fa-angle-right"></i></a>
    </div>
    <div class="feat-scroll">
        <asp:Repeater ID="rptFeatured" runat="server">
            <ItemTemplate>
                <a class="feat-card" href='<%# Eval("url") %>' target='<%# Target(Eval("target")) %>'>
                    <div class="feat-img" style='background: <%# Eval("img_bg") %>;'><span><%# Eval("emoji") %></span></div>
                    <div class="feat-body">
                        <div class="feat-name"><%# Eval("name") %></div>
                        <div class="feat-desc"><%# Eval("description") %></div>
                        <div class="feat-badge"><i class='<%# Eval("badge_icon") %>'></i><%# Eval("badge") %></div>
                    </div>
                </a>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- ===== SPECIAL OFFERS (dynamic) ===== -->
    <div class="sec-head">
        <h2><span class="sh-icon" style="background: linear-gradient(135deg,#dc2626,#f87171); color: #fff;"><i class="fa fa-percent"></i></span>Special Offers</h2>
    </div>
    <div class="offer-grid">
        <asp:Repeater ID="rptOffers" runat="server">
            <ItemTemplate>
                <a class="offer-card <%# Eval("bg_class") %>" href='<%# Eval("url") %>' target='<%# Target(Eval("target")) %>'>
                    <div class="offer-deco"><%# Eval("deco") %></div>
                    <div class="offer-tag"><%# Eval("tag") %></div>
                    <div class="offer-title"><%# Eval("title") %></div>
                    <div class="offer-sub"><%# Eval("subtitle") %></div>
                </a>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- ===== WIDE PROMO (TOP – dynamic) ===== -->
    <div class="sec-head">
        <div></div>
    </div>
    <asp:Repeater ID="rptPromoTop" runat="server">
        <ItemTemplate>
            <div class="wide-promo">
                <div class="wp-inner <%# Eval("bg_class") %>">
                    <div class="wp-circle wc1"></div>
                    <div class="wp-circle wc2"></div>
                    <div class="wp-deco"><%# Eval("deco") %></div>
                    <div class="wp-text">
                        <div class="wp-tag"><%# Eval("tag") %></div>
                        <div class="wp-title"><%# Eval("title") %></div>
                        <div class="wp-sub"><%# Eval("subtitle") %></div>
                    </div>
                    <a href='<%# Eval("url") %>' target='<%# Target(Eval("target")) %>' class="wp-btn <%# Inv(Eval("btn_inv")) %>"><%# Eval("cta") %></a>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <!-- ===== ALL SERVICES GRID (dynamic) ===== -->
    <div class="sec-head" id="services-section">
        <h2><span class="sh-icon" style="background: linear-gradient(135deg,#0891b2,#22d3ee); color: #fff;"><i class="fa fa-th-large"></i></span>All Services</h2>
        <a href="#" onclick="showToast('All services listed below')">See All <i class="fa fa-angle-right"></i></a>
    </div>
    <div class="svc-grid">
        <asp:Repeater ID="rptServices" runat="server">
            <ItemTemplate>
                <a class="svc-card" href='<%# Eval("url") %>' target='<%# Target(Eval("target")) %>'>
                    <div class="svc-ico <%# Eval("color") %>"><i class='<%# Eval("icon") %>'></i></div>
                    <div class="svc-name"><%# Eval("name") %></div>
                </a>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- ===== BEST SELLING (dynamic) ===== -->
    <div class="sec-head">
        <h2><span class="sh-icon" style="background: linear-gradient(135deg,#d97706,#f59e0b); color: #fff;"><i class="fa fa-trophy"></i></span>Best Selling</h2>
    </div>
    <div class="best-grid">
        <asp:Repeater ID="rptBest" runat="server">
            <ItemTemplate>
                <a class="best-card" href='<%# Eval("url") %>' target='<%# Target(Eval("target")) %>'>
                    <div class="bc-rank" <%# RankStyle(Eval("rank_bg")) %>><%# Eval("rank") %></div>
                    <div class="bc-ico" style='background: <%# Eval("icon_bg") %>;'><i class='<%# Eval("icon") %>' style="color: #fff; font-size: 1.4rem;"></i></div>
                    <div class="bc-name"><%# Eval("name") %></div>
                    <div class="bc-stat"><%# Eval("users") %></div>
                </a>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- ===== WIDE PROMO (BOTTOM – dynamic) ===== -->
    <div class="sec-head">
        <div></div>
    </div>
    <asp:Repeater ID="rptPromoBottom" runat="server">
        <ItemTemplate>
            <div class="wide-promo">
                <div class="wp-inner <%# Eval("bg_class") %>">
                    <div class="wp-circle wc1"></div>
                    <div class="wp-circle wc2"></div>
                    <div class="wp-deco"><%# Eval("deco") %></div>
                    <div class="wp-text">
                        <div class="wp-tag"><%# Eval("tag") %></div>
                        <div class="wp-title"><%# Eval("title") %></div>
                        <div class="wp-sub"><%# Eval("subtitle") %></div>
                    </div>
                    <a href='<%# Eval("url") %>' target='<%# Target(Eval("target")) %>' class="wp-btn <%# Inv(Eval("btn_inv")) %>"><%# Eval("cta") %></a>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <!-- ===== TRUST / STATS (dynamic) ===== -->
    <style>
        
        .trust-grid {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            gap: 10px;
        }

        .trust-card {
            flex: 0 1 calc(33.333% - 10px);
            min-width: 90px; /* chhota kiya taaki 320px width pe bhi 3 fit ho */
            box-sizing: border-box;
            padding: 14px 8px; /* mobile ke hisaab se padding bhi kam kar do */
        }

        .tc-icon {
            width: 36px;
            height: 36px;
        }

        .tc-val {
            font-size: 1rem;
        }

        .tc-lab {
            font-size: 0.72rem;
        }
    </style>
    <div class="sec-head">
        <h2><span class="sh-icon" style="background: linear-gradient(135deg,#059669,#34d399); color: #fff;"><i class="fa fa-shield-alt"></i></span>Why Choose ePay</h2>
    </div>
    <div class="trust-grid">
        <asp:Repeater ID="rptTrust" runat="server">
            <ItemTemplate>
                <div class="trust-card">
                    <div class="tc-icon" style='background: <%# Eval("icon_bg") %>;'>
                        <i class='<%# Eval("icon") %>' style='color: <%# Eval("icon_color") %>; font-size: 1.1rem;'></i>
                    </div>
                    <div class="tc-val"><%# Eval("value") %></div>
                    <div class="tc-lab"><%# Eval("label") %></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <%--   <div class="hero">
        <div class="hero-greeting" id="greeting"></div>
        <div class="hero-name" id="ddUserName" runat="server"></div>
        <div class="hero-sub">Digital Commerce &amp; Utility Platform</div>
    </div>

    <!-- SERVICES -->
     <div class="sec-label">Our Services</div>
 <div class="svc-grid">

   <a class="svc-card" href="AppStoreredirect.aspx" target="_blank">
     <div class="svc-ico ic-or"><i class="fa fa-store"></i></div>
     <div class="svc-name">Store Shopping</div>
     <div class="svc-desc">Shop top brands</div>
   </a>

   <a class="svc-card" href="BrandRedirect.aspx" target="_blank">
     <div class="svc-ico ic-bl"><i class="fa fa-shopping-cart"></i></div>
     <div class="svc-name">E-Commerce Solutions</div>
     <div class="svc-desc">Buy &amp; sell online</div>
   </a>

   <a class="svc-card" href="UtilityServicesRedirect.aspx" target="_blank">
     <div class="svc-ico ic-pu"><i class="fa fa-bolt"></i></div>
     <div class="svc-name">ePay Digital Services</div>
     <div class="svc-desc">Bills &amp; utilities</div>
   </a>

   <a class="svc-card" href="GVRedirect.aspx" target="_blank">
     <div class="svc-ico ic-gr"><i class="fa fa-gift"></i></div>
     <div class="svc-name">Gift Vouchers</div>
     <div class="svc-desc">Send &amp; receive gifts</div>
   </a>

   <a class="svc-card" href="FoodBookingRedirect.aspx" target="_blank">
     <div class="svc-ico ic-re"><i class="fa fa-utensils"></i></div>
     <div class="svc-name">ePay Food</div>
     <div class="svc-desc">Order food online</div>
   </a>

   <a class="svc-card" href="MovieBookingRedirect.aspx" target="_blank">
     <div class="svc-ico ic-ye"><i class="fa fa-film"></i></div>
     <div class="svc-name">ePay Movie</div>
     <div class="svc-desc">Book your tickets</div>
   </a>

   <a class="svc-card" href="MainAccountRedirect.ASPX" target="_blank">
     <div class="svc-ico ic-te"><i class="fa fa-user-circle"></i></div>
     <div class="svc-name">My Account</div>
     <div class="svc-desc">Profile &amp; settings</div>
   </a>

   <a class="svc-card" href="Apppurchase-coupon.aspx">
     <div class="svc-ico ic-pi"><i class="fa fa-ticket-alt"></i></div>
     <div class="svc-name">Purchase Coupon</div>
     <div class="svc-desc">Discount coupons</div>
   </a>

   <a class="svc-card" href="Appsubscription-now.aspx">
     <div class="svc-ico ic-in"><i class="fa fa-crown"></i></div>
     <div class="svc-name">Subscription Now</div>
     <div class="svc-desc">Premium plans</div>
   </a>

   <a class="svc-card" href="Appmonthly-activation-points.aspx">
     <div class="svc-ico ic-cy"><i class="fa fa-chart-bar"></i></div>
     <div class="svc-name">Monthly Activation Points</div>
     <div class="svc-desc">Earn rewards</div>
   </a>

   <a class="svc-card" href="MainWebsiteRedirect.aspx">
     <div class="svc-ico ic-li"><i class="fa fa-globe"></i></div>
     <div class="svc-name">View Website</div>
     <div class="svc-desc">Main website</div>
   </a>

 </div>--%>
</asp:Content>

