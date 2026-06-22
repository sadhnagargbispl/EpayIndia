<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="terms_and_conditions_web.aspx.cs" Inherits="terms_and_conditions_web" %>

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

        .tc-section h6 {
            font-family: 'Sora', sans-serif;
            font-size: .85rem;
            font-weight: 800;
            letter-spacing: 1px;
            text-transform: uppercase;
            color: var(--orange);
            border-left: 4px solid var(--orange);
            padding-left: 12px;
            margin-top: 48px;
            margin-bottom: 14px;
        }

        .tc-section ul {
            padding-left: 20px;
            margin: 0;
        }

            .tc-section ul li {
                color: #374151;
                font-size: .97rem;
                line-height: 1.85;
                margin-bottom: 12px;
                text-align: justify;
            }

        .tc-section b {
            display: block;
            margin-top: 12px;
            color: var(--text);
            font-size: .95rem;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <section class="page-hero">
        <div class="page-hero-inner">
            <div class="page-hero-badge">&#10003; Legal</div>
            <h1>Terms & <span>Conditions</span></h1>
            <p>Please read our terms and conditions carefully. By using our platform, you agree to the following terms.</p>
        </div>
    </section>

    <div class="page-content">
        <div class="tc-section col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">

            <h6>Overview</h6>
            <ul>
                <li>This website is operated by EPay Digital India Pvt Ltd. Throughout the site, the terms "we", "us" and "our" refer to EPay Digital India Pvt Ltd. EPay Digital India Pvt Ltd offers this website, including all information, tools and services available from this site to you, the user, conditioned upon your acceptance of all terms, conditions, policies and notices stated here.</li>
                <li>By visiting our site and/ or purchasing something from us, you engage in our "Service" and agree to be bound by the following terms and conditions ("Terms of Service", "Terms"), including those additional terms and conditions and policies referenced herein and/or available by hyperlink. These Terms of Service apply to all users of the site, including without limitation users who are browsers, vendors, customers, merchants, and/ or contributors of content.</li>
                <li>Please read these Terms of Service carefully before accessing or using our website. By accessing or using any part of the site, you agree to be bound by these Terms of Service. If you do not agree to all the terms and conditions of this agreement, then you may not access the website or use any services. If these Terms of Service are considered an offer, acceptance is expressly limited to these Terms of Service.</li>
                <li>Any new features or tools which are added to the current store shall also be subject to the Terms of Service. You can review the most current version of the Terms of Service at any time on this page. We reserve the right to update, change or replace any part of these Terms of Service by posting updates and/or changes to our website. It is your responsibility to check this page periodically for changes. Your continued use of or access to the website following the posting of any changes constitutes acceptance of those changes.</li>
            </ul>

            <h6>Section 1 – Online Store Terms</h6>
            <ul>
                <li>By agreeing to these Terms of Service, you represent that you are at least the age of majority in your state or province of residence, or that you are the age of majority in your state or province of residence and you have given us your consent to allow any of your minor dependents to use this site.</li>
                <li>You may not use our products for any illegal or unauthorized purpose nor may you, in the use of the Service, violate any laws in your jurisdiction (including but not limited to copyright laws).</li>
                <li>A breach or violation of any of the Terms will result in an immediate termination of your Services.</li>
            </ul>

            <h6>Section 2 – General Conditions</h6>
            <ul>
                <li>We reserve the right to refuse service to anyone for any reason at any time.</li>
                <li>You understand that your content (not including credit card information), may be transferred unencrypted and involve (a) transmissions over various networks; and (b) changes to conform and adapt to technical requirements of connecting networks or devices. Credit card information is always encrypted during transfer over networks.</li>
                <li>You agree not to reproduce, duplicate, copy, sell, resell or exploit any portion of the Service, use of the Service, or access to the Service or any contact on the website through which the service is provided, without express written permission by us.</li>
                <li>The headings used in this agreement are included for convenience only and will not limit or otherwise affect these Terms.</li>
            </ul>

            <h6>Section 3 – Accuracy, Completeness and Timeliness of Information</h6>
            <ul>
                <li>We are not responsible if information made available on this site is not accurate, complete or current. The material on this site is provided for general information only and should not be relied upon or used as the sole basis for making decisions without consulting primary, more accurate, more complete or more timely sources of information. Any reliance on the material on this site is at your own risk.</li>
                <li>This site may contain certain historical information. Historical information, necessarily, is not current and is provided for your reference only. We reserve the right to modify the contents of this site at any time, but we have no obligation to update any information on our site. You agree that it is your responsibility to monitor changes to our site.</li>
            </ul>

            <h6>Section 4 – Modifications to the Service and Prices</h6>
            <ul>
                <li>Prices for our products are subject to change without notice.</li>
                <li>We reserve the right at any time to modify or discontinue the Service (or any part or content thereof) without notice at any time.</li>
                <li>We shall not be liable to you or to any third-party for any modification, price change, suspension or discontinuance of the Service.</li>
            </ul>

            <h6>Section 5 – Products or Services</h6>
            <ul>
                <li>Certain products or services may be available exclusively online through the website. These products or services may have limited quantities and are subject to return or exchange only according to our Return Policy.</li>
                <li>We have made every effort to display as accurately as possible the colors and images of our products that appear at the store. We cannot guarantee that your computer monitor's display of any color will be accurate.</li>
                <li>We reserve the right, but are not obligated, to limit the sales of our products or Services to any person, geographic region or jurisdiction. We may exercise this right on a case-by-case basis. We reserve the right to limit the quantities of any products or services that we offer. All descriptions of products or product pricing are subject to change at anytime without notice, at the sole discretion of us. We reserve the right to discontinue any product at any time. Any offer for any product or service made on this site is void where prohibited.</li>
                <li>We do not warrant that the quality of any products, services, information, or other material purchased or obtained by you will meet your expectations, or that any errors in the Service will be corrected.</li>
                <li>Some products which are currently selling on the portal may belong to the stock lot or stock excess. It may belong to the prior manufacturing year (2019, 2018, or older); some listed products may be out of warranty and packing box and packaging might be damaged.</li>
                <li>It is advisable to read all terms and conditions or refund policies while buying any product for more clarity.</li>
            </ul>

            <h6>Section 6 – Accuracy of Billing and Account Information</h6>
            <ul>
                <li>We reserve the right to refuse any order you place with us. We may, in our sole discretion, limit or cancel quantities purchased per person, per household or per order. These restrictions may include orders placed by or under the same customer account, the same credit card, and/or orders that use the same billing and/or shipping address. In the event that we make a change to or cancel an order, we may attempt to notify you by contacting the e-mail and/or billing address/phone number provided at the time the order was made. We reserve the right to limit or prohibit orders that, in our sole judgment, appear to be placed by dealers, resellers or distributors.</li>
                <li>You agree to provide current, complete and accurate purchase and account information for all purchases made at our store. You agree to promptly update your account and other information, including your email address and credit card numbers and expiration dates, so that we can complete your transactions and contact you as needed.</li>
            </ul>
            <b>For more detail, please review our Returns Policy.</b>

            <h6>Section 7 – Optional Tools</h6>
            <ul>
                <li>We may provide you with access to third-party tools over which we neither monitor nor have any control nor input.</li>
                <li>You acknowledge and agree that we provide access to such tools "as is" and "as available" without any warranties, representations or conditions of any kind and without any endorsement. We shall have no liability whatsoever arising from or relating to your use of optional third-party tools.</li>
                <li>Any use by you of optional tools offered through the site is entirely at your own risk and discretion and you should ensure that you are familiar with and approve of the terms on which tools are provided by the relevant third-party provider(s).</li>
                <li>We may also, in the future, offer new services and/or features through the website (including, the release of new tools and resources). Such new features and/or services shall also be subject to these Terms of Service.</li>
            </ul>

            <h6>Section 8 – Third-Party Links</h6>
            <ul>
                <li>Certain content, products and services available via our Service may include materials from third-parties.</li>
                <li>Third-party links on this site may direct you to third-party websites that are not affiliated with us. We are not responsible for examining or evaluating the content or accuracy and we do not warrant and will not have any liability or responsibility for any third-party materials or websites, or for any other materials, products, or services of third-parties.</li>
                <li>We are not liable for any harm or damages related to the purchase or use of goods, services, resources, content, or any other transactions made in connection with any third-party websites. Please review carefully the third-party's policies and practices and make sure you understand them before you engage in any transaction. Complaints, claims, concerns, or questions regarding third-party products should be directed to the third-party.</li>
            </ul>

            <h6>Section 9 – User Comments, Feedback and Other Submissions</h6>
            <ul>
                <li>If, at our request, you send certain specific submissions (for example contest entries) or without a request from us you send creative ideas, suggestions, proposals, plans, or other materials, whether online, by email, by postal mail, or otherwise (collectively, 'comments'), you agree that we may, at any time, without restriction, edit, copy, publish, distribute, translate and otherwise use in any medium any comments that you forward to us. We are and shall be under no obligation (1) to maintain any comments in confidence; (2) to pay compensation for any comments; or (3) to respond to any comments.</li>
                <li>We may, but have no obligation to, monitor, edit or remove content that we determine in our sole discretion are unlawful, offensive, threatening, libellous, defamatory, pornographic, obscene or otherwise objectionable or violates any party's intellectual property or these Terms of Service.</li>
                <li>You agree that your comments will not violate any right of any third-party, including copyright, trademark, privacy, personality or other personal or proprietary right. You further agree that your comments will not contain libellous or otherwise unlawful, abusive or obscene material, or contain any computer virus or other malware that could in any way affect the operation of the Service or any related website. You may not use a false e-mail address, pretend to be someone other than yourself, or otherwise mislead us or third-parties as to the origin of any comments. You are solely responsible for any comments you make and their accuracy. We take no responsibility and assume no liability for any comments posted by you or any third-party.</li>
            </ul>

            <h6>Section 10 – Personal Information</h6>
            <ul>
                <li>Your submission of personal information through the store is governed by our Privacy Policy. To view our Privacy Policy, please visit the Privacy Policy page on this website.</li>
            </ul>

            <h6>Section 11 – Errors, Inaccuracies and Omissions</h6>
            <ul>
                <li>Occasionally there may be information on our site or in the Service that contains typographical errors, inaccuracies or omissions that may relate to product descriptions, pricing, promotions, offers, product shipping charges, transit times and availability. We reserve the right to correct any errors, inaccuracies or omissions, and to change or update information or cancel orders if any information in the Service or on any related website is inaccurate at any time without prior notice (including after you have submitted your order).</li>
                <li>We undertake no obligation to update, amend or clarify information in the Service or on any related website, including without limitation, pricing information, except as required by law. No specified update or refresh date applied in the Service or on any related website, should be taken to indicate that all information in the Service or on any related website has been modified or updated.</li>
            </ul>

            <h6>Section 12 – Prohibited Uses</h6>
            <ul>
                <li>In addition to other prohibitions as set forth in the Terms of Service, you are prohibited from using the site or its content: (a) for any unlawful purpose; (b) to solicit others to perform or participate in any unlawful acts; (c) to violate any international, federal, provincial or state regulations, rules, laws, or local ordinances; (d) to infringe upon or violate our intellectual property rights or the intellectual property rights of others; (e) to harass, abuse, insult, harm, defame, slander, disparage, intimidate, or discriminate based on gender, sexual orientation, religion, ethnicity, race, age, national origin, or disability; (f) to submit false or misleading information; (g) to upload or transmit viruses or any other type of malicious code that will or may be used in any way that will affect the functionality or operation of the Service or of any related website, other websites, or the Internet; (h) to collect or track the personal information of others; (i) to spam, phish, pharm, pretext, spider, crawl, or scrape; (j) for any obscene or immoral purpose; or (k) to interfere with or circumvent the security features of the Service or any related website, other websites, or the Internet. We reserve the right to terminate your use of the Service or any related website for violating any of the prohibited uses.</li>
            </ul>

            <h6>Section 13 – Disclaimer of Warranties; Limitation of Liability</h6>
            <ul>
                <li>We do not guarantee, represent or warrant that your use of our service will be uninterrupted, timely, secure or error-free.</li>
                <li>We do not warrant that the results that may be obtained from the use of the service will be accurate or reliable.</li>
                <li>You agree that from time to time we may remove the service/product for indefinite periods of time or cancel the service at any time, without notice to you.</li>
                <li>You expressly agree that your use of, or inability to use, the service is at your sole risk. The service and all products and services delivered to you through the service are (except as expressly stated by us) provided 'as is' and 'as available' for your use, without any representation, warranties or conditions of any kind, either express or implied, including all implied warranties or conditions of merchantability, merchantable quality, fitness for a particular purpose, durability, title, and non-infringement.</li>
                <li>In no case shall EPay Digital India Pvt Ltd, our directors, officers, employees, affiliates, agents, contractors, interns, suppliers, service providers or licensors be liable for any injury, loss, claim, or any direct, indirect, incidental, punitive, special, or consequential damages of any kind, including, without limitation lost profits, lost revenue, lost savings, loss of data, replacement costs, or any similar damages, whether based in contract, tort (including negligence), strict liability or otherwise, arising from your use of any of the service or any products procured using the service, or for any other claim related in any way to your use of the service or any product, including, but not limited to, any errors or omissions in any content, or any loss or damage of any kind incurred as a result of the use of the service or any content (or product) posted, transmitted, or otherwise made available via the service, even if advised of their possibility. Because some states or jurisdictions do not allow the exclusion or the limitation of liability for consequential or incidental damages, in such states or jurisdictions, our liability shall be limited to the maximum extent permitted by law.</li>
            </ul>

            <h6>Section 14 – Indemnification</h6>
            <ul>
                <li>You agree to indemnify, defend and hold harmless EPay Digital India Pvt Ltd and our parent, subsidiaries, affiliates, partners, officers, directors, agents, contractors, licensors, service providers, subcontractors, suppliers, interns and employees, harmless from any claim or demand, including reasonable attorneys' fees, made by any third-party due to or arising out of your breach of these Terms of Service or the documents they incorporate by reference, or your violation of any law or the rights of a third-party.</li>
            </ul>

            <h6>Section 15 – Severability</h6>
            <ul>
                <li>In the event that any provision of these Terms of Service is determined to be unlawful, void or unenforceable, such provision shall nonetheless be enforceable to the fullest extent permitted by applicable law, and the unenforceable portion shall be deemed to be severed from these Terms of Service, such determination shall not affect the validity and enforceability of any other remaining provisions.</li>
            </ul>

            <h6>Section 16 – Termination</h6>
            <ul>
                <li>The obligations and liabilities of the parties incurred prior to the termination date shall survive the termination of this agreement for all purposes.</li>
                <li>These Terms of Service are effective unless and until terminated by either you or us. You may terminate these Terms of Service at any time by notifying us that you no longer wish to use our Services, or when you cease using our site.</li>
                <li>If in our sole judgment you fail, or we suspect that you have failed, to comply with any term or provision of these Terms of Service, we also may terminate this agreement at any time without notice and you will remain liable for all amounts due up to and including the date of termination; and/or accordingly may deny you access to our Services (or any part thereof).</li>
            </ul>

            <h6>Section 17 – Entire Agreement</h6>
            <ul>
                <li>The failure of us to exercise or enforce any right or provision of these Terms of Service shall not constitute a waiver of such right or provision.</li>
                <li>These Terms of Service and any policies or operating rules posted by us on this site or in respect to The Service constitutes the entire agreement and understanding between you and us and govern your use of the Service, superseding any prior or contemporaneous agreements, communications and proposals, whether oral or written, between you and us (including, but not limited to, any prior versions of the Terms of Service).</li>
                <li>Any ambiguities in the interpretation of these Terms of Service shall not be construed against the drafting party.</li>
            </ul>

            <h6>Section 18 – Governing Law</h6>
            <ul>
                <li>These Terms of Service and any separate agreements whereby we provide you Services shall be governed by and construed in accordance with the laws of India.</li>
            </ul>

            <h6>Section 19 – Changes to Terms of Service</h6>
            <ul>
                <li>You can review the most current version of the Terms of Service at any time at this page.</li>
                <li>We reserve the right, at our sole discretion, to update, change or replace any part of these Terms of Service by posting updates and changes to our website. It is your responsibility to check our website periodically for changes. Your continued use of or access to our website or the Service following the posting of any changes to these Terms of Service constitutes acceptance of those changes.</li>
            </ul>

            <h6>Section 20 – Contact Information</h6>
            <ul>
                <li>Questions about the Terms of Service should be sent to us at <a href="/cdn-cgi/l/email-protection#d7beb9b1b897b2a7b6aebeb9b3beb6f9beb9" style="color: var(--orange); text-decoration: none; font-weight: 600;"><span class="__cf_email__" data-cfemail="056c6b636a456075647c6c6b616c642b6c6b">[email&#160;protected]</span></a></li>
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

