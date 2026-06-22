<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="privacy-policy_web.aspx.cs" Inherits="privacy_policy_web" %>

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

        .privacy-body ul {
            padding-left: 1.4rem;
            color: var(--text);
            font-size: .97rem;
            line-height: 1.9;
        }

            .privacy-body ul li {
                margin-bottom: 10px;
            }

        .privacy-body h6 {
            font-family: 'Sora', sans-serif;
            font-size: 1.05rem;
            font-weight: 700;
            color: var(--orange);
            margin-top: 2.5rem;
            margin-bottom: .5rem;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


   

    <section class="page-hero">
        <div class="page-hero-inner">
            <div class="page-hero-badge">&#10003; Legal</div>
            <h1>Privacy <span>Policy</span></h1>
            <p>We value your privacy. Learn how we collect, use, and protect your personal data at ePay Digital India.</p>
        </div>
    </section>

    <div class="page-content">
        <div class="privacy-body" align="justify">

            <ul class="mt-2">
                <li>We value the trust you placed on us, and we appreciate it. That's why we secure your information upon the highest standards for secure transactions and customer information privacy.</li>
                <li>Please read the following explanation to know about how collect and process your information gathering and dissemination practices.</li>
                <li>By using EPay Digital India Pvt Ltd Services, you are allowing to the practices described in this Privacy Declaration.</li>
            </ul>

            <h6>Note :</h6>
            <ul class="mt-2">
                <li>We may change our privacy policy at any time. To make sure you are aware of any changes, please review this policy often.</li>
                <li>By visiting you agree to be bound by the terms and conditions of our Privacy Policy. If you're not, so please do not use or access our Website.</li>
            </ul>

            <h6>What personal and other information does EPay Digital India Pvt Ltd collects from you?</h6>
            <ul class="mt-2">
                <li>We collect your personal and other information to provide and continually improve our products and services. Here is the detailed description of the information we collect</li>
                <li>Information you give us we receive and store any information you provide to EPay Digital India Pvt Ltd on time to time. Our initial goal in doing this is to provide you with a safe, efficient, quick and customized experience.</li>
                <li>We show which fields are required and which fields are optional. You always have the option not to provide information which is not required. We may automatically track certain information about you based upon your behaviour on EPay Digital India Pvt Ltd website. We use this information to do in-house research on our users' through analytics i.e demographics, interests, and behaviour to understand, protect and serve our users in a better way.</li>
                <li>This information is compiled and analysed on an aggregated basis. This information may include the URL that you just came from, which URL you next go to, your computer browser information, and your IP address.</li>
                <li>We use data collection devices such as "cookies" on certain pages of the Website to help analyse our web page flow, measure promotional effectiveness, and promote trust and safety. "Cookies" are small files placed on your hard drive that assist us in providing our services. We also use cookies to allow you to enter your password less frequently during a session. Cookies can also help us provide information that is targeted to your interests. Most cookies are "session cookies," meaning that they are automatically deleted from your hard drive at the end of a session. You are always free to decline our cookies if your browser permits, although in that case, you may not be able to use certain features on the Website and you may be required to re-enter your password more frequently during a session.</li>
                <li>If you wish to buy anything from the , we collect information about your buying product, preferences, and other such information that you choose to provide.</li>
                <li>If you transact with us, we collect some additional information, such as a billing address, a credit/debit card number and a credit/debit card expiration date and/ or other payment instrument details and tracking information.</li>
                <li>We retain this information as necessary to resolve conflicts, provide customer support and troubleshoot problems as permitted by law.</li>
                <li>If you send us personal correspondence, such as emails or letters, or if other users or third parties send us correspondence about your activities or postings on the Website, we may collect such information.</li>
                <li>We collect personally identifiable information (email address, name, phone number, credit card / debit card / other payment instrument details, etc.) from you when you set up a account with us. We do use your contact information to send you offers based on your previous orders and your interests.</li>
            </ul>

            <h6>Use of Demographic / Profile Data / Your Information</h6>
            <ul class="mt-2">
                <li>We use personal information to provide the services or products you request. To the extent, we use your personal information for re-marketing to you. We use your personal information to support sellers in handling and fulfilling orders, enhancing customer experience, resolve disputes, troubleshoot problems, help promote a safe service, collect money, measure consumer interest in our products and services, inform you about online and offline offers, products, services, and latest updates, customize and enhance your experience with EPay Digital India Pvt Ltd, detect and protect us against error, fraud and other criminal activity, enforce our terms and conditions, and as otherwise described to you at the time of collection.</li>
                <li>With your permission, we will have access to your SMS, contacts in your direct, location and device information and we may request you to provide your PAN and Know-Your-Customer (KYC) details to check your eligibility for certain products/services including to credit and payment products etc. to enhance your experience on the platform and provide you access to the services being offered by us, our members or lending partners. Access, storage and use of this data will be in consonance with applicable laws.</li>
                <li>In our efforts to continually improve our product and service offerings, we and our members collect and analyse demographic and profile data about our user's activity on EPay Digital India Pvt Ltd.</li>
                <li>We identify and use your IP address to help diagnose problems with our server and to administer our Website. Your IP address is also used to help identify you and to gather demographic information.</li>
            </ul>

            <h6>How Secured is your information with us?</h6>
            <ul class="mt-2">
                <li>We design our policies with your security and privacy.</li>
                <li>We work to protect the security of your personal information during transmission by using encryption protocols and software.</li>
                <li>We follow the Payment Card Industry Data Security Standard when handling credit card data.</li>
                <li>We maintain physical, electronic, and procedural safeguards in connection with the collection, storage, and disclosure of personal customer information. Our security procedures mean that we may occasionally request proof of identity before we disclose personal information to you.</li>
                <li>Our devices offer security features to protect them against unauthorized access and loss of data. You can control these features and configure them based on your needs.</li>
                <li>When you use our Services, we and some third parties may use cookies and similar technologies to provide you with a safer, more durable and better user experience. Cookies are short text files that are automatically created by your browser and stored on your device when you use the Services. Cookies are useful for enabling the browser to remember information specific to a given user. The cookies do not contain any of your personally identifiable information.</li>
            </ul>

            <h6>Links to another site.</h6>
            <ul class="mt-2">
                <li>Our Website links to other websites that may collect personally identifiable information about you. EPay Digital India Pvt Ltd is not responsible for the privacy practices or the content of those linked websites.</li>
            </ul>

            <h6>Security Precautions</h6>
            <ul class="mt-2">
                <li>By Using The EPay Digital India Pvt Ltd has powerful security measures to protect the loss, misuse, and modification of the information under our control. Whenever you change or access your account information, we offer the use of a secure server. Once your information is in our possession we adhere to strict security guidelines, protecting it against unauthorized access.</li>
            </ul>

            <h6>Your acquiescence</h6>
            <ul class="mt-2">
                <li>By using the EPay Digital India Pvt Ltd website or by providing your data, you permit to the collection and use of the information you disclose on the Website by this Privacy Policy, including your permission for sharing your information as per this privacy policy. If you disclose any personal information related to other people to us, you represent that you have the authority to do so and to permit us to use the information by this Privacy Policy.</li>
                <li>If we decide to change our privacy policy, we will post those changes on this page so that you are always aware of what information we collect, how we use it, and under what circumstances we disclose it.</li>
            </ul>

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

