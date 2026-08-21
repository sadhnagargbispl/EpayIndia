<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="PetroCardFinalPurchase.aspx.cs" Inherits="PetroCardFinalPurchase" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="assets/jquery.min.js"></script>
    <script type="text/javascript" src="assets/jquery.validationEngine-en.js"></script>
    <script type="text/javascript" src="assets/jquery.validationEngine.js"></script>
    <link href="assets/validationEngine.jquery.min.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var jq = $.noConflict();

        jq(document).ready(function () {
            jq(document).bind("contextmenu", function (e) { e.preventDefault(); });

            jq(document).keydown(function (e) {
                if (e.which === 123) { return false; }               // F12
                if (e.which === 116) { return false; }               // F5
                if (e.ctrlKey && e.which === 82) { return false; }   // Ctrl + R
            });
        });

        function pageLoad(sender, args) {
            jq("#aspnetForm").validationEngine('attach', { promptPosition: "topRight" });
        }

        function ValidateAndSubmit(btn) {
            var valid = jq("#aspnetForm").validationEngine('validate');
            if (!valid) { return false; }

            if (!confirm('Are you sure to proceed with this purchase?')) { return false; }

            btn.value = 'Processing...';
            btn.disabled = true;
            setTimeout(function () { __doPostBack(btn.name, ''); }, 0);
            return false;
        }
    </script>

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
            padding: 55px 20px 70px;
            background: #f9fafb;
        }

        .map-wrap {
            max-width: 1180px;
            margin: auto;
        }

        /* ── Panels ── */
        .map-panel {
            background: #fff;
            border: 1.5px solid var(--border);
            border-radius: 20px;
            box-shadow: 0 4px 24px rgba(0,0,0,.06);
            overflow: hidden;
            margin-bottom: 24px;
        }

        .map-panel-head {
            background: linear-gradient(135deg, #fff7ed 0%, #ffedd5 100%);
            border-bottom: 1.5px dashed rgba(232,64,0,.25);
            padding: 16px 26px;
            display: flex;
            align-items: center;
            gap: 12px;
        }

        .map-panel-icon {
            width: 38px;
            height: 38px;
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

        .map-panel-body {
            padding: 26px;
        }

        /* ── Form fields ── */
        .map-field {
            margin-bottom: 20px;
        }

            .map-field > label,
            .map-field .map-label {
                display: block;
                font-size: .74rem;
                font-weight: 700;
                letter-spacing: .5px;
                text-transform: uppercase;
                color: var(--muted);
                margin-bottom: 7px;
            }

        .req {
            color: var(--orange);
            font-weight: 800;
        }

        .map-section .form-control {
            width: 100%;
            border: 1.5px solid var(--border);
            border-radius: 10px;
            padding: 11px 14px;
            font-size: .92rem;
            color: var(--text);
            background: #fff;
            transition: border-color .2s, box-shadow .2s;
        }

            .map-section .form-control:focus {
                outline: none;
                border-color: rgba(232,64,0,.55);
                box-shadow: 0 0 0 3px rgba(232,64,0,.10);
            }

            .map-section .form-control[readonly],
            .map-section .form-control[disabled] {
                background: #f6f7f9;
                color: #4B5563;
            }

        textarea.form-control {
            resize: vertical;
            min-height: 78px;
        }

        /* ── Order summary (sticky) ── */
        .map-summary {
            position: sticky;
            top: 20px;
        }

        .map-summary-head {
            background: linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%);
            border-bottom: 1.5px dashed rgba(22,163,74,.28);
        }

            .map-summary-head .map-panel-icon {
                background: var(--gg);
                border-color: rgba(22,163,74,.28);
            }

        .map-sum-row {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 10px;
            background: #f9fafb;
            border: 1px solid var(--border);
            border-radius: 10px;
            padding: 12px 15px;
            margin-bottom: 12px;
        }

        .map-sum-label {
            font-size: .74rem;
            font-weight: 700;
            color: var(--muted);
            text-transform: uppercase;
            letter-spacing: .5px;
        }

        .map-sum-value {
            font-family: 'Sora', sans-serif;
            font-size: .95rem;
            font-weight: 800;
            color: var(--text);
        }

        .map-sum-row.balance {
            background: linear-gradient(135deg, #f0fdf4, #dcfce7);
            border-color: rgba(22,163,74,.25);
        }

            .map-sum-row.balance .map-sum-label {
                color: var(--green);
            }

        .map-sum-total {
            background: linear-gradient(135deg, #fff7ed, #ffedd5);
            border: 1.5px dashed rgba(232,64,0,.35);
            border-radius: 12px;
            padding: 16px;
            text-align: center;
            margin-bottom: 18px;
        }

            .map-sum-total .map-sum-label {
                color: var(--orange);
                display: block;
                margin-bottom: 6px;
            }

        .map-sum-amount {
            font-family: 'Sora', sans-serif;
            font-size: 2rem;
            font-weight: 800;
            color: var(--orange);
            line-height: 1;
        }

        .map-notch {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin: 0 -1px 16px;
        }

        .map-notch-circle {
            width: 18px;
            height: 18px;
            border-radius: 50%;
            background: #f9fafb;
            border: 1.5px solid var(--border);
            flex-shrink: 0;
        }

        .map-notch-line {
            flex: 1;
            border-top: 2px dashed #e2e8f0;
            margin: 0 4px;
        }

        /* ── Buttons ── */
        .map-btn-p {
            display: block;
            width: 100%;
            text-align: center;
            background: var(--orange);
            color: #fff !important;
            font-family: 'Sora', sans-serif;
            font-weight: 700;
            font-size: .92rem;
            padding: 13px 0;
            border: none;
            border-radius: 50px;
            text-decoration: none;
            cursor: pointer;
            box-shadow: 0 6px 20px rgba(232,64,0,.30);
            transition: background .25s, transform .2s, box-shadow .25s;
        }

            .map-btn-p:hover:enabled,
            a.map-btn-p:hover {
                background: #c73600;
                transform: translateY(-2px);
                box-shadow: 0 9px 26px rgba(232,64,0,.40);
                text-decoration: none;
            }

            .map-btn-p:disabled {
                opacity: .6;
                cursor: not-allowed;
            }

        .map-btn-o {
            display: block;
            width: 100%;
            text-align: center;
            background: #fff;
            color: var(--green) !important;
            font-family: 'Sora', sans-serif;
            font-weight: 700;
            font-size: .88rem;
            padding: 11px 0;
            border-radius: 50px;
            text-decoration: none;
            border: 1.5px solid rgba(22,163,74,.35);
            margin-top: 12px;
            transition: background .25s, transform .2s;
        }

            .map-btn-o:hover {
                background: #f0fdf4;
                transform: translateY(-2px);
                text-decoration: none;
            }

        .map-secure-note {
            font-size: .76rem;
            color: var(--muted);
            text-align: center;
            margin-top: 14px;
            line-height: 1.6;
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

        /* ── Grid ── */
        .map-row {
            display: flex;
            flex-wrap: wrap;
            margin: 0 -10px;
        }

        .map-col-4,
        .map-col-6,
        .map-col-8,
        .map-col-12 {
            padding: 0 10px;
            width: 100%;
        }

        @media (min-width: 768px) {
            .map-col-4 {
                width: 33.3333%;
            }

            .map-col-6 {
                width: 50%;
            }

            .map-col-8 {
                width: 66.6667%;
            }
        }

        .map-layout {
            display: flex;
            flex-wrap: wrap;
            gap: 24px;
            align-items: flex-start;
        }

        .map-layout-main {
            flex: 1 1 620px;
            min-width: 0;
        }

        .map-layout-side {
            flex: 0 1 340px;
            min-width: 300px;
        }

        @media (max-width: 991px) {
            .map-summary {
                position: static;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- ─── HERO ─── -->
    <section class="map-hero">
        <div class="map-hero-inner">
            <div class="map-hero-badge">
                <div class="map-badge-dot"></div>
                Step 2 of 2 — Checkout
            </div>
            <h1>Complete Your <span>Petro Card</span> Purchase</h1>
            <p>Please verify your details below. Your card will be shipped to the address you provide here.</p>
        </div>
    </section>

    <!-- ─── FORM ─── -->
    <section class="map-section">
        <div class="map-wrap">

            <asp:Panel ID="pnlError" runat="server" CssClass="map-alert" Visible="false">
                <asp:Label ID="LblError" runat="server"></asp:Label>
            </asp:Panel>

            <div class="map-layout">

                <!-- ══ LEFT : FORM ══ -->
                <div class="map-layout-main">

                    <!-- Member Details -->
                    <div class="map-panel">
                        <div class="map-panel-head">
                            <div class="map-panel-icon">👤</div>
                            <div>
                                <h3 class="map-panel-title">Member Details</h3>
                                <p class="map-panel-sub">Your registered identity information</p>
                            </div>
                        </div>
                        <div class="map-panel-body">
                            <div class="map-row">

                                <div class="map-col-4">
                                    <div class="map-field" id="DiMemberId" runat="server">
                                        <label class="map-label">Member Id <span class="req">*</span></label>
                                        <asp:TextBox ID="txtMemberId" runat="server" CssClass="form-control validate[required]"
                                            AutoPostBack="true" Enabled="false"></asp:TextBox>

                                        <asp:Label ID="lblFormno" runat="server" Visible="false"></asp:Label>
                                        <asp:HiddenField ID="hdnMacadrs" runat="server" />
                                        <asp:HiddenField ID="HdnTopupSeq" runat="server" />
                                        <asp:HiddenField ID="HdnMemberMacAdrs" runat="server" />
                                        <asp:HiddenField ID="HdnMemberTopupseq" runat="server" />
                                        <asp:HiddenField ID="MemberStatus" runat="server" />
                                        <asp:HiddenField ID="hdnFormno" runat="server" />
                                        <asp:HiddenField ID="hdnemail" runat="server" />
                                        <asp:HiddenField ID="Hdnkitid" runat="server" />
                                        <asp:HiddenField ID="HdnWalletBalance" runat="server" />
                                    </div>
                                </div>

                                <div class="map-col-4">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <div class="map-field" id="DivMemberName" runat="server">
                                                <label class="map-label">Member Name <span class="req">*</span></label>
                                                <asp:Label ID="LblMobile" runat="server" Visible="false"></asp:Label>
                                                <asp:TextBox ID="TxtMemberName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtMemberId" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>

                                <div class="map-col-4">
                                    <div class="map-field">
                                        <label class="map-label">Gender <span class="req">*</span></label>
                                        <asp:DropDownList ID="DDlGender" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="map-col-4">
                                    <div class="map-field">
                                        <label class="map-label">Date Of Birth <span class="req">*</span></label>
                                        <asp:TextBox ID="TxtDOB" runat="server" CssClass="form-control" autocomplete="off"
                                            placeholder="dd-MMM-yyyy"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                                            TargetControlID="TxtDOB" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ControlToValidate="TxtDOB" ErrorMessage="Invalid Date" Display="Dynamic"
                                            Font-Names="arial" Font-Size="10px" ForeColor="Red" SetFocusOnError="True"
                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>

                                <div class="map-col-4">
                                    <div class="map-field">
                                        <label class="map-label">Pan No. <span class="req">*</span></label>
                                        <asp:TextBox ID="Txtpanno" runat="server" CssClass="form-control"
                                            MaxLength="10" placeholder="ABCDE1234F"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <!-- Contact Details -->
                    <div class="map-panel">
                        <div class="map-panel-head">
                            <div class="map-panel-icon">📞</div>
                            <div>
                                <h3 class="map-panel-title">Contact Details</h3>
                                <p class="map-panel-sub">We will send card updates on these</p>
                            </div>
                        </div>
                        <div class="map-panel-body">
                            <div class="map-row">

                                <div class="map-col-4">
                                    <div class="map-field">
                                        <label class="map-label">Email <span class="req">*</span></label>
                                        <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control"
                                            TextMode="Email" placeholder="name@example.com"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="map-col-4">
                                    <div class="map-field">
                                        <label class="map-label">Mobile No. <span class="req">*</span></label>
                                        <asp:TextBox ID="Txtmonileno" runat="server" CssClass="form-control"
                                            MaxLength="10" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="map-col-4">
                                    <div class="map-field">
                                        <label class="map-label">Whatsapp No. <span class="req">*</span></label>
                                        <asp:TextBox ID="TxtWhatsappNo" runat="server" CssClass="form-control"
                                            MaxLength="10" Text="0" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <!-- Shipping Address -->
                    <div class="map-panel">
                        <div class="map-panel-head">
                            <div class="map-panel-icon">📦</div>
                            <div>
                                <h3 class="map-panel-title">Shipping Address</h3>
                                <p class="map-panel-sub">Where should we deliver your Petro Card?</p>
                            </div>
                        </div>
                        <div class="map-panel-body">
                            <div class="map-row">

                                <div class="map-col-8">
                                    <div class="map-field">
                                        <label class="map-label">Shipping Address <span class="req">*</span></label>
                                        <asp:TextBox ID="TxtAddress" runat="server" CssClass="form-control"
                                            TextMode="MultiLine" Rows="2"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="map-col-4">
                                    <div class="map-field">
                                        <label class="map-label">PinCode <span class="req">*</span></label>
                                        <asp:TextBox ID="Txtpincode" runat="server" CssClass="form-control"
                                            MaxLength="6" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="map-col-4">
                                    <div class="map-field">
                                        <label class="map-label">State <span class="req">*</span></label>
                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="map-col-4">
                                    <div class="map-field">
                                        <label class="map-label">District <span class="req">*</span></label>
                                        <asp:TextBox ID="TxtDistrict" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="HDistrictCode" runat="server" />
                                    </div>
                                </div>

                                <div class="map-col-4">
                                    <div class="map-field">
                                        <label class="map-label">City <span class="req">*</span></label>
                                        <asp:TextBox ID="TxtCity" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </div>

                <!-- ══ RIGHT : ORDER SUMMARY ══ -->
                <div class="map-layout-side">
                    <div class="map-panel map-summary">
                        <div class="map-panel-head map-summary-head">
                            <div class="map-panel-icon">🧾</div>
                            <div>
                                <h3 class="map-panel-title">Order Summary</h3>
                                <p class="map-panel-sub">Review before you pay</p>
                            </div>
                        </div>

                        <div class="map-panel-body">

                            <div class="map-sum-total">
                                <span class="map-sum-label">Card Amount</span>
                                <div class="map-sum-amount">
                                    ₹
                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="map-sum-amount"
                                        BorderStyle="None" BackColor="Transparent" Width="150px" ReadOnly="true"
                                        Text="0" onkeypress="return isNumberKey(event);"></asp:TextBox>
                                </div>
                            </div>

                            <div class="map-notch">
                                <div class="map-notch-circle"></div>
                                <div class="map-notch-line"></div>
                                <div class="map-notch-circle"></div>
                            </div>
                           <%-- <asp:UpdatePanel ID="UpdPnlPayMode" runat="server">
                                <ContentTemplate>--%>

                                    <div class="map-field" id="DivPaymentMode" runat="server">
                                        <label class="map-label">Payment Mode <span class="req">*</span></label>
                                        <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="form-control"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentMode_SelectedIndexChanged">
                                            <asp:ListItem Text="--Select--" Value="Z" />
                                            <asp:ListItem Text="Wallet" Value="WALLET" />
                                            <asp:ListItem Text="Payment Gateway" Value="PG" />
                                        </asp:DropDownList>
                                    </div>

                                    <div class="map-field" id="DivWalletType" runat="server" visible="false" style="display: none;">
                                        <label class="map-label">Select Wallet <span class="req">*</span></label>
                                        <asp:DropDownList ID="ddlWalletType" runat="server" CssClass="form-control"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlWalletType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="map-field" id="Div1" runat="server" visible="false">
                                        <label class="map-label">Available Balance</label>
                                        <asp:TextBox ID="AvailableBal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>

                                    <%-- Payment Gateway wala UI yaha add kar lena, jab ddlPaymentMode == "PG" ho --%>
                                    <div class="map-field" id="DivPaymentGateway" runat="server" visible="false">
                                        <%-- aapka payment gateway UI / control yahan --%>
                                    </div>

                              <%--  </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlPaymentMode" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlWalletType" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>--%>
                            <%-- <div class="map-field" style="display: none;">
                                <label class="map-label">Select Wallet <span class="req">*</span></label>
                                <asp:DropDownList ID="ddlWalletType" runat="server" CssClass="form-control"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlWalletType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                            <div class="map-field" id="Div1" runat="server">
                                <label class="map-label">Available Balance</label>
                                <asp:TextBox ID="AvailableBal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>--%>

                            <%--   <div class="map-sum-row balance">
                                <div class="map-sum-label">⛽ Validity</div>
                                <div class="map-sum-value">15 Months</div>
                            </div>--%>

                            <asp:Button ID="BtnSubmit" runat="server" Text="Proceed To Pay"
                                CssClass="map-btn-p" ValidationGroup="Validation"
                                OnClientClick="return ValidateAndSubmit(this);" OnClick="BtnSubmit_Click" />

                            <a href="PETROCARDPurchase.aspx" class="map-btn-o">← Back to Packages</a>

                            <p class="map-secure-note">
                                🔒 Your transaction is secured. Amount will be deducted from the selected wallet.
                            </p>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>

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
