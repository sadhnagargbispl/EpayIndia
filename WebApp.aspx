<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="WebApp.aspx.cs" Inherits="WebApp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="demoepay/css/AppWeb.css" rel="stylesheet" />
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="hero">
        <div class="hero-greeting" id="greeting"></div>
        <div class="hero-name" id="ddUserName" runat="server"></div>
        <div class="hero-sub">India's Smart Digital &amp; eCommerce Platform</div>
    </div>

    <!-- SERVICES -->
    <div class="sec-label">Our Services</div>
    <div class="svc-grid">

        <a class="svc-card" href="AppStoreredirect.aspx">
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

        <a class="svc-card" href="https://food.epayindia.in/" target="_blank">
            <div class="svc-ico ic-re"><i class="fa fa-utensils"></i></div>
            <div class="svc-name">Food Order</div>
            <div class="svc-desc">Order food online</div>
        </a>

        <a class="svc-card" href="https://movie.epayindia.in/" target="_blank">
            <div class="svc-ico ic-ye"><i class="fa fa-film"></i></div>
            <div class="svc-name">Movie Booking</div>
            <div class="svc-desc">Book your tickets</div>
        </a>

        <a class="svc-card" href="#" onclick="showToast('My Account')">
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

        <a class="svc-card" href="index.aspx">
            <div class="svc-ico ic-li"><i class="fa fa-globe"></i></div>
            <div class="svc-name">View Website</div>
            <div class="svc-desc">Main website</div>
        </a>

    </div>



    <!-- spacing before footer -->
        <script>
            function setGreeting() {
                const hour = new Date().getHours();
                let message = "Good Evening";

                if (hour < 12) {
                    message = "Good Morning";
                }
                else if (hour < 17) {
                    message = "Good Afternoon";
                }

                document.getElementById("greeting").innerText = message + ",";
            }

            setGreeting();
</script>
</asp:Content>

