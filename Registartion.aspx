<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="Registartion.aspx.cs" Inherits="Registartion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>

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

        /* ── Page Breadcrumb ── */
        .reg-breadcrumb {
            background: #fff;
            border-bottom: 1px solid var(--border);
            padding: 14px 20px;
        }

        .reg-breadcrumb-inner {
            max-width: 900px;
            margin: auto;
            display: flex;
            align-items: center;
            gap: 8px;
            font-size: .82rem;
            color: var(--muted);
        }

            .reg-breadcrumb-inner a {
                color: var(--orange);
                text-decoration: none;
                font-weight: 600;
            }

                .reg-breadcrumb-inner a:hover {
                    text-decoration: underline;
                }

        .reg-breadcrumb-sep {
            color: #d1d5db;
        }

        /* ── Main Layout ── */
        .reg-section {
            padding: 40px 20px 60px;
            background: #f9fafb;
            min-height: 70vh;
        }

        .reg-wrap {
            max-width: 900px;
            margin: auto;
        }

        .reg-layout {
            display: block;
        }

        /* ══════════════════════════════
           FORM PANEL
        ══════════════════════════════ */
        .reg-left {
            display: flex;
            flex-direction: column;
            gap: 24px;
        }

        /* Title card */
        .reg-title-card {
            background: #fff;
            border-radius: 18px;
            border: 1px solid var(--border);
            padding: 28px 30px;
            box-shadow: 0 2px 16px rgba(0,0,0,.05);
        }

        .reg-pkg-badge {
            display: inline-flex;
            align-items: center;
            gap: 7px;
            background: var(--og);
            border: 1px solid rgba(232,64,0,.25);
            color: var(--orange);
            font-size: .72rem;
            font-weight: 700;
            letter-spacing: .8px;
            text-transform: uppercase;
            padding: 5px 14px;
            border-radius: 50px;
            margin-bottom: 14px;
        }

        .reg-pkg-dot {
            width: 7px;
            height: 7px;
            background: var(--orange);
            border-radius: 50%;
            animation: blink 1.4s infinite;
        }

        @keyframes blink {
            0%,100% { opacity: 1; }
            50% { opacity: .3; }
        }

        .reg-title-card h1 {
            font-family: 'Sora', sans-serif;
            font-size: clamp(1.3rem,2.5vw,1.8rem);
            font-weight: 800;
            color: var(--text);
            margin-bottom: 12px;
            line-height: 1.3;
        }

            .reg-title-card h1 span {
                color: var(--orange);
            }

        .reg-title-card p {
            color: var(--muted);
            font-size: .92rem;
            line-height: 1.8;
            margin: 0;
        }

        /* Form card */
        .reg-form-card {
            background: #fff;
            border-radius: 18px;
            border: 1px solid var(--border);
            padding: 28px 30px;
            box-shadow: 0 2px 16px rgba(0,0,0,.05);
        }

            .reg-form-card h3 {
                font-family: 'Sora', sans-serif;
                font-size: .88rem;
                font-weight: 700;
                color: var(--muted);
                text-transform: uppercase;
                letter-spacing: .6px;
                margin-bottom: 22px;
                padding-bottom: 12px;
                border-bottom: 1px solid var(--border);
                display: flex;
                align-items: center;
                gap: 8px;
            }

        /* Grid for form fields */
        .reg-grid {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 18px 20px;
        }

        .reg-field {
            display: flex;
            flex-direction: column;
        }

            .reg-field.full {
                grid-column: 1 / -1;
            }

            .reg-field label {
                font-family: 'Sora', sans-serif;
                font-size: .82rem;
                font-weight: 700;
                color: var(--text);
                margin-bottom: 8px;
                display: flex;
                align-items: center;
                gap: 4px;
            }

            .reg-field .req {
                color: var(--orange);
                font-weight: 800;
            }

            .reg-field .form-control,
            .reg-field input[type="text"],
            .reg-field input[type="password"],
            .reg-field input[type="email"],
            .reg-field select {
                width: 100%;
                padding: 12px 16px;
                font-size: .9rem;
                font-family: 'Sora', sans-serif;
                color: var(--text);
                background: #f9fafb;
                border: 1px solid var(--border);
                border-radius: 12px;
                outline: none;
                transition: border-color .2s, box-shadow .2s, background .2s;
                box-sizing: border-box;
            }

                .reg-field .form-control:focus,
                .reg-field input:focus,
                .reg-field select:focus {
                    border-color: var(--orange);
                    background: #fff;
                    box-shadow: 0 0 0 3px rgba(232,64,0,.12);
                }

                .reg-field .form-control[disabled],
                .reg-field input[disabled],
                .reg-field .form-control[readonly] {
                    background: #f1f3f5;
                    color: var(--muted);
                    cursor: not-allowed;
                }

        .reg-field-hint {
            font-size: .75rem;
            color: var(--muted);
            margin-top: 6px;
        }

        .reg-field-err {
            font-size: .78rem;
            color: #D11F7B;
            margin-top: 6px;
            font-weight: 600;
        }

        /* Mobile field group (code + number) */
        .reg-mobile-group {
            display: grid;
            grid-template-columns: 90px 1fr;
            gap: 10px;
        }

        /* Terms row */
        .reg-terms {
            margin-top: 22px;
            padding: 16px 18px;
            background: var(--og);
            border: 1px solid rgba(232,64,0,.18);
            border-radius: 12px;
            display: flex;
            align-items: center;
            gap: 10px;
            font-size: .88rem;
            color: var(--text);
        }

            .reg-terms input[type="checkbox"] {
                width: 18px;
                height: 18px;
                accent-color: var(--orange);
                cursor: pointer;
                flex-shrink: 0;
            }

            .reg-terms a {
                color: var(--orange);
                font-weight: 700;
                text-decoration: none;
            }

                .reg-terms a:hover {
                    text-decoration: underline;
                }

        /* Error message */
        .reg-err-msg {
            display: block;
            margin-top: 14px;
            padding: 10px 14px;
            background: rgba(220,38,38,.08);
            border: 1px solid rgba(220,38,38,.2);
            border-radius: 10px;
            color: #dc2626;
            font-size: .85rem;
            font-weight: 600;
        }

            .reg-err-msg:empty {
                display: none;
            }

        /* Action buttons */
        .reg-actions {
            margin-top: 22px;
            display: flex;
            gap: 12px;
            flex-wrap: wrap;
        }

        .reg-btn {
            flex: 1;
            min-width: 140px;
            text-align: center;
            font-family: 'Sora', sans-serif;
            font-size: 1rem;
            font-weight: 700;
            padding: 14px 24px;
            border-radius: 50px;
            text-decoration: none;
            border: none;
            cursor: pointer;
            transition: background .25s, transform .2s, box-shadow .25s, color .2s;
        }

        .reg-btn-primary {
            background: var(--orange);
            color: #fff;
            box-shadow: 0 8px 28px rgba(232,64,0,.32);
        }

            .reg-btn-primary:hover {
                background: #c73600;
                transform: translateY(-2px);
                box-shadow: 0 12px 36px rgba(232,64,0,.42);
                color: #fff;
            }

        .reg-btn-ghost {
            background: #fff;
            color: var(--text);
            border: 1.5px solid var(--border);
        }

            .reg-btn-ghost:hover {
                background: #f9fafb;
                border-color: var(--orange);
                color: var(--orange);
            }

        /* Back to login link */
        .reg-back-login {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            font-family: 'Sora', sans-serif;
            font-size: .85rem;
            font-weight: 600;
            color: var(--orange);
            text-decoration: none;
            padding: 8px 14px;
            border-radius: 50px;
            background: var(--og);
            border: 1px solid rgba(232,64,0,.18);
            transition: background .2s, transform .2s;
        }

            .reg-back-login:hover {
                background: rgba(232,64,0,.18);
                transform: translateX(-2px);
                color: var(--orange);
                text-decoration: none;
            }

        .reg-back-login .arrow {
            font-size: 1rem;
            line-height: 1;
        }

        .reg-form-card h3 {
            justify-content: space-between;
        }

        .reg-footer-login {
            margin-top: 18px;
            text-align: center;
            font-size: .88rem;
            color: var(--muted);
        }

            .reg-footer-login a {
                color: var(--orange);
                font-weight: 700;
                text-decoration: none;
                margin-left: 4px;
            }

                .reg-footer-login a:hover {
                    text-decoration: underline;
                }

        /* Hide native span color used in original markup */
        .reg-form-card label span[style*="color: Red"] {
            color: var(--orange) !important;
            font-size: 1rem !important;
            font-weight: 700 !important;
        }

        /* Ensure server controls in old "col-md-X" blocks still align nicely */
        .reg-form-card .mb-3 {
            margin-bottom: 0 !important;
        }

        /* Responsive */
        @media (max-width: 700px) {
            .reg-grid {
                grid-template-columns: 1fr;
            }
        }

        @media (max-width: 480px) {
            .reg-title-card,
            .reg-form-card {
                padding: 20px;
            }

            .reg-actions {
                flex-direction: column;
            }

            .reg-btn {
                width: 100%;
            }

            .reg-form-card h3 {
                flex-direction: column;
                align-items: flex-start;
                gap: 12px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- ─── BREADCRUMB ─── -->
    <div class="reg-breadcrumb">
        <div class="reg-breadcrumb-inner">
            <a href="index.aspx">Home</a>
            <span class="reg-breadcrumb-sep">›</span>
            <span>New Joining</span>
        </div>
    </div>

    <!-- ─── MAIN CONTENT ─── -->
    <section class="reg-section">
        <div class="reg-wrap">
            <div class="reg-layout">

                <div class="reg-left">

                    <!-- Title -->
                    <div class="reg-title-card">
                        <div class="reg-pkg-badge">
                            <div class="reg-pkg-dot"></div>
                            ePay India — New Registration
                        </div>
                        <h1>Join the ePay <span>Family Today</span></h1>
                        <p>Create your account to access shopping, utility payments, food booking, movie tickets, gift vouchers and much more — all in one place.</p>
                    </div>

                    <!-- Form -->
                    <div class="reg-form-card">
                        <h3>
                            <span>👤 Personal &amp; Contact Details</span>
                            <a href="login.aspx" class="reg-back-login">
                                <span class="arrow">←</span> Back to Login
                            </a>
                        </h3>

                        <div class="reg-grid">

                            <!-- Referral ID -->
                            <div class="reg-field" id="Div1" runat="server" visible="true">
                                <label>
                                    Referral ID <span class="req">*</span>
                                </label>
                                <asp:TextBox ID="txtRefralId" CssClass="form-control" TabIndex="1" runat="server"
                                    AutoPostBack="True" ValidationGroup="eInformation" autocomplete="off" OnTextChanged="txtRefralId_TextChanged"></asp:TextBox>
                                <asp:Label ID="lblRefralNm" runat="server" CssClass="reg-field-err"></asp:Label>
                                <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                            </div>

                            <!-- Placement ID -->
                            <div class="reg-field" id="rwSpnsr" runat="server" visible="false">
                                <label>
                                    Placement ID <span class="req">*</span>
                                </label>
                                <asp:TextBox ID="txtUplinerId" CssClass="form-control" TabIndex="2" runat="server" AutoPostBack="True"
                                    ValidationGroup="eSponsor" autocomplete="off" OnTextChanged="txtUplinerId_TextChanged"></asp:TextBox>
                                <asp:Label ID="lblUplnrNm" runat="server" CssClass="reg-field-err"></asp:Label>
                            </div>

                            <!-- Leg -->
                            <div class="reg-field" runat="server" id="DivLeg1" visible="false">
                                <label>
                                    Leg <span class="req">*</span>
                                </label>
                                <asp:RadioButtonList ID="RbtnLegNo" runat="server" TabIndex="3" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="RbtnLegNo_SelectedIndexChanged" />
                            </div>

                            <!-- Registration As -->
                            <div id="dvreg" runat="server" visible="false" class="reg-field full">
                                <div class="reg-grid">
                                    <div class="reg-field">
                                        <label>
                                            Registration As <span class="req">*</span>
                                        </label>
                                        <asp:RadioButtonList ID="RbCategory" runat="server" RepeatDirection="Horizontal"
                                            TabIndex="4" onchange="return GetRegistrationAs()">
                                            <asp:ListItem Text="Individual" Value="IN" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Company" Value="C"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="reg-field" id="RegType" style="display: none">
                                        <label>
                                            <asp:Label ID="LblRegType" Text="Registration Type" runat="server"></asp:Label>
                                            <span class="req">*</span>
                                        </label>
                                        <asp:RadioButtonList ID="CbSubCategory" runat="server" TabIndex="5" RepeatDirection="Horizontal"
                                            onchange="return GetRegistrationType()">
                                            <asp:ListItem Text="ProprietorShip" Value="SP" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Partnership Firm" Value="PF"></asp:ListItem>
                                            <asp:ListItem Text="Private Limited Company" Value="PL"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>

                            <!-- Name -->
                            <div class="reg-field">
                                <label>
                                    <asp:Label ID="LblName" runat="server"></asp:Label>Name <span class="req">*</span>
                                </label>
                                <asp:TextBox ID="txtFrstNm" runat="server" TabIndex="6" ValidationGroup="eInformation"
                                    autocomplete="off" CssClass="form-control validate[required,custom[onlyLetterNumberChar]]"></asp:TextBox>
                            </div>

                            <!-- Partner name (company) -->
                            <div class="reg-field" id="TrPrtnrCap" style="display: none">
                                <label>
                                    <asp:Label ID="LblPartnerName" runat="server" Text="Partner Name Seperated By Comma(,)"></asp:Label>
                                </label>
                            </div>

                            <!-- Father/Husband Name -->
                            <div class="reg-field" id="divFName" runat="server" visible="false">
                                <label>
                                    Father / Husband's Name <span class="req">*</span>
                                </label>
                                <div class="reg-mobile-group">
                                    <asp:DropDownList CssClass="form-control" ID="CmbType" runat="server" TabIndex="7">
                                        <asp:ListItem Value="S/O" Text="S/O"></asp:ListItem>
                                        <asp:ListItem Value="D/O" Text="D/O"></asp:ListItem>
                                        <asp:ListItem Value="W/O" Text="W/O"></asp:ListItem>
                                        <asp:ListItem Value="H/O" Text="H/O"></asp:ListItem>
                                        <asp:ListItem Value="C/O" Text="C/O"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtFNm" runat="server" TabIndex="8" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>

                            <!-- DOB -->
                            <div class="reg-field" id="Div2" visible="false" runat="server">
                                <label>
                                    <asp:Label ID="LblRegistDate" runat="server" Text="Date Of Birth"></asp:Label>
                                    <span class="req">*</span>
                                </label>
                                <div style="display: grid; grid-template-columns: 1fr 1fr 1fr; gap: 8px;">
                                    <asp:DropDownList ID="ddlDOBdt" runat="server" CssClass="form-control" TabIndex="9" autocomplete="off"></asp:DropDownList>
                                    <asp:DropDownList ID="ddlDOBmnth" runat="server" CssClass="form-control" TabIndex="10" autocomplete="off"></asp:DropDownList>
                                    <asp:DropDownList ID="ddlDOBYr" runat="server" CssClass="form-control" TabIndex="11" autocomplete="off"></asp:DropDownList>
                                </div>
                            </div>

                            <!-- Marital Status -->
                            <div class="reg-field" id="Div3" visible="false" runat="server">
                                <label>
                                    Marital Status <span class="req">*</span>
                                </label>
                                <asp:RadioButtonList ID="RbtMarried" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" TabIndex="12" onchange="return GetSelectedItem()" autocomplete="off">
                                    <asp:ListItem Text="Married" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="UnMarried" Value="N"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>

                            <!-- Marriage Date -->
                            <div class="reg-field" id="divMarriageDate" visible="false" style="display: none;">
                                <label>
                                    Marriage Date <span class="req">*</span>
                                </label>
                                <div style="display: grid; grid-template-columns: 1fr 1fr 1fr; gap: 8px;">
                                    <asp:DropDownList ID="DDlMDay" runat="server" CssClass="form-control" TabIndex="13"></asp:DropDownList>
                                    <asp:DropDownList ID="DDLMMonth" runat="server" CssClass="form-control" TabIndex="14"></asp:DropDownList>
                                    <asp:DropDownList ID="DDLMYear" runat="server" CssClass="form-control" TabIndex="15"></asp:DropDownList>
                                </div>
                            </div>

                            <!-- Company Name -->
                            <div class="reg-field" id="CompName" style="display: none">
                                <label>
                                    Company Name <span class="req">*</span>
                                </label>
                                <asp:TextBox ID="TxtCompanyName" runat="server" CssClass="form-control" TabIndex="16"></asp:TextBox>
                            </div>

                            <!-- Company Reg No -->
                            <div class="reg-field" id="CompRegistrationNo" style="display: none">
                                <label>
                                    Company Registration No <span class="req">*</span>
                                </label>
                                <asp:TextBox ID="TxtRegistrationNo" runat="server" CssClass="form-control" TabIndex="17"></asp:TextBox>
                            </div>

                            <!-- Contact / Address block -->
                            <div id="dvpin" runat="server" visible="false" class="reg-field full">
                                <div class="reg-grid">
                                    <div class="reg-field" id="Div4" visible="false" runat="server">
                                        <label>Address <span class="req">*</span></label>
                                        <asp:TextBox ID="txtAddLn1" CssClass="form-control" TabIndex="18" runat="server" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="reg-field">
                                        <label>Pin code <span class="req">*</span></label>
                                        <asp:TextBox ID="txtPinCode" CssClass="form-control" onkeypress="return isNumberKey(event);"
                                            TabIndex="19" runat="server" MaxLength="6" autocomplete="off" AutoPostBack="true"></asp:TextBox>
                                    </div>
                                    <div class="reg-field">
                                        <label>State <span class="req">*</span></label>
                                        <asp:TextBox ID="txtStateName" runat="server" CssClass="form-control" TabIndex="16" autocomplete="off" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="StateCode" runat="server" />
                                    </div>
                                    <div class="reg-field">
                                        <label>District <span class="req">*</span></label>
                                        <asp:TextBox ID="ddlDistrict" CssClass="form-control" TabIndex="17" runat="server" autocomplete="off" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="HDistrictCode" runat="server" />
                                    </div>
                                    <div class="reg-field">
                                        <label>City <span class="req">*</span></label>
                                        <asp:TextBox ID="ddlTehsil" CssClass="form-control" TabIndex="18" runat="server" ValidationGroup="eInformation" autocomplete="off" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="HCityCode" runat="server" />
                                    </div>
                                    <div class="reg-field">
                                        <label>Area <span class="req">*</span></label>
                                        <asp:DropDownList ID="DDlVillage" CssClass="form-control" TabIndex="19" runat="server" ValidationGroup="eInformation" autocomplete="off" onchange="FnVillageChange(this.value);"></asp:DropDownList>
                                    </div>
                                    <div class="reg-field" id="divVillage" style="display: none">
                                        <label>Area Name <span class="req">*</span></label>
                                        <asp:TextBox ID="TxtVillage" CssClass="form-control" TabIndex="20" runat="server" autocomplete="off"></asp:TextBox>
                                    </div>

                                    <!-- Postal -->
                                    <div id="Dvfld" runat="server" visible="false" class="reg-field full">
                                        <div class="reg-grid">
                                            <div class="reg-field full">
                                                <label>
                                                    <asp:CheckBox ID="ChkSame" runat="server" onclick="return GetSameAsPostal()" TabIndex="21" />
                                                    &nbsp;Same As Above
                                                </label>
                                            </div>
                                            <div class="reg-field">
                                                <label>Postal Address <span class="req">*</span></label>
                                                <asp:TextBox ID="TxtPostalAddress" CssClass="form-control" TabIndex="22" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="reg-field">
                                                <label>Pin code <span class="req">*</span></label>
                                                <asp:TextBox ID="TxtPostPincode" CssClass="form-control" onkeypress="return isNumberKey(event);" TabIndex="23" runat="server" MaxLength="6" autocomplete="off" AutoPostBack="true"></asp:TextBox>
                                            </div>
                                            <div class="reg-field">
                                                <label>State <span class="req">*</span></label>
                                                <asp:TextBox ID="TxtpostState" runat="server" CssClass="form-control" TabIndex="24" autocomplete="off" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="HPostStateCode" runat="server" />
                                            </div>
                                            <div class="reg-field">
                                                <label>District <span class="req">*</span></label>
                                                <asp:TextBox ID="TxtPostDistrict" CssClass="form-control" TabIndex="25" runat="server" autocomplete="off" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="HPostDistrict" runat="server" />
                                            </div>
                                            <div class="reg-field">
                                                <label>City <span class="req">*</span></label>
                                                <asp:TextBox ID="TxtPostCity" CssClass="form-control" TabIndex="26" runat="server" ValidationGroup="eInformation" autocomplete="off" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="HPostCity" runat="server" />
                                            </div>
                                            <div class="reg-field">
                                                <label>Area <span class="req">*</span></label>
                                                <asp:DropDownList ID="DDlPostVillage" CssClass="form-control" TabIndex="27" runat="server" ValidationGroup="eInformation" autocomplete="off" onchange="FnPostVillageChange(this.value);"></asp:DropDownList>
                                                <asp:HiddenField ID="HPostVillage" runat="server" />
                                            </div>
                                            <div class="reg-field" id="divPostVillage" style="display: none">
                                                <label>Area Name <span class="req">*</span></label>
                                                <asp:TextBox ID="TxtPostVillage" CssClass="form-control" TabIndex="28" runat="server" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Country Name -->
                            <div class="reg-field" id="Div5" runat="server">
                                <label>
                                    Country Name <span class="req">*</span>
                                </label>
                                <asp:DropDownList ID="ddlCountryNAme" runat="server" CssClass="form-control"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlCountryNAme_SelectedIndexChanged"></asp:DropDownList>
                            </div>

                            <!-- Mobile -->
                            <div class="reg-field">
                                <label>
                                    Mobile No. <span class="req">*</span>
                                </label>
                                <div class="reg-mobile-group">
                                    <asp:TextBox ID="ddlMobileNAme" CssClass="form-control" runat="server"
                                        ValidationGroup="eInformation" autocomplete="off" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtMobileNo" onkeypress="return isNumberKey(event);" CssClass="form-control validate[required]"
                                        runat="server" MaxLength="10" ValidationGroup="eInformation" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>

                            <!-- Phone -->
                            <div class="reg-field" id="Div6" visible="false" runat="server">
                                <label>
                                    Phone No. <span class="req">*</span>
                                </label>
                                <asp:TextBox ID="txtPhNo" onkeypress="return isNumberKey(event);" CssClass="form-control"
                                    TabIndex="30" runat="server" MaxLength="10" autocomplete="off"></asp:TextBox>
                            </div>

                            <!-- Email -->
                            <div class="reg-field" id="Div7" runat="server">
                                <label>
                                    E-Mail ID <span class="req">*</span>
                                </label>
                                <asp:TextBox ID="txtEMailId" CssClass="form-control validate[required]"
                                    TabIndex="31" runat="server" autocomplete="off"></asp:TextBox>
                                <asp:Label ID="LblEmainID" runat="server" CssClass="reg-field-err" Visible="false"></asp:Label>
                            </div>

                            <!-- Wallet Address -->
                            <div class="reg-field" id="Div966" runat="server" visible="false">
                                <label>Wallet Address</label>
                                <asp:TextBox ID="TxtWalletaddress" CssClass="form-control" TabIndex="31" runat="server" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="HdnWalletAddress" runat="server" />
                                <asp:HiddenField ID="HiddenField4" runat="server" />
                                <asp:HiddenField ID="Hdnhhhgg" runat="server" />
                            </div>

                            <!-- PAN Avail -->
                            <div class="reg-field" id="Div8" runat="server" visible="false">
                                <label>PAN No Available <span class="req">*</span></label>
                                <asp:RadioButtonList ID="RbtPan" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" TabIndex="41">
                                    <asp:ListItem Text="Yes" Value="Y" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:Label ID="LblPanNoAvail" runat="server" CssClass="reg-field-hint" Text="Payout will deduct 20%, If you not enter PAN NO."></asp:Label>
                            </div>

                            <!-- PAN -->
                            <div class="reg-field" id="Div9" runat="server" visible="false">
                                <label>PAN No. <span class="req">*</span></label>
                                <asp:TextBox ID="txtPanNo" CssClass="form-control validate[custom[panno]]" TabIndex="42" runat="server" autocomplete="off"></asp:TextBox>
                            </div>

                            <!-- Nominee -->
                            <div class="reg-field" runat="server" visible="false">
                                <label>Nominee Name <span class="req">*</span></label>
                                <asp:TextBox ID="txtNominee" CssClass="form-control" TabIndex="32" runat="server" autocomplete="off"></asp:TextBox>
                            </div>

                            <!-- Relation -->
                            <div class="reg-field" runat="server" visible="false">
                                <label>Relation <span class="req">*</span></label>
                                <asp:TextBox ID="txtRelation" CssClass="form-control" TabIndex="33" runat="server" autocomplete="off"></asp:TextBox>
                            </div>

                            <!-- Account No -->
                            <div class="reg-field" runat="server" visible="false">
                                <label>Account No. <span class="req">*</span></label>
                                <asp:TextBox ID="TxtAccountNo" onkeypress="return isNumberKey(event);" CssClass="form-control" TabIndex="34" runat="server" MaxLength="16" autocomplete="off"></asp:TextBox>
                            </div>

                            <!-- Account Type -->
                            <div class="reg-field" runat="server" visible="false">
                                <label>Account Type <span class="req">*</span></label>
                                <asp:DropDownList ID="DDLAccountType" runat="server" CssClass="form-control" TabIndex="21">
                                    <asp:ListItem Text="CHOOSE ACCOUNT TYPE" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="SAVING ACCOUNT" Value="SAVING ACCOUNT"></asp:ListItem>
                                    <asp:ListItem Text="CURRENT ACCOUNT" Value="CURRENT ACCOUNT"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <!-- Bank -->
                            <div class="reg-field" runat="server" visible="false">
                                <label>Bank <span class="req">*</span></label>
                                <asp:DropDownList ID="CmbBank" runat="server" CssClass="form-control" TabIndex="36"
                                    onchange="FnBankChange(this.value);" autocomplete="off"></asp:DropDownList>
                            </div>

                            <div class="reg-field" id="divBank" style="display: none">
                                <label>Bank Name <span class="req">*</span></label>
                                <asp:TextBox ID="TxtBank" CssClass="form-control" TabIndex="37" runat="server" autocomplete="off"></asp:TextBox>
                            </div>

                            <div class="reg-field" runat="server" visible="false">
                                <label>Branch Name <span class="req">*</span></label>
                                <asp:TextBox ID="TxtBranchName" CssClass="form-control" TabIndex="38" runat="server" autocomplete="off"></asp:TextBox>
                            </div>

                            <div class="reg-field" runat="server" visible="false">
                                <label>IFSC Code <span class="req">*</span></label>
                                <asp:TextBox ID="txtIfsCode" runat="server" CssClass="form-control" TabIndex="39" autocomplete="off"></asp:TextBox>
                            </div>

                            <div class="reg-field" runat="server" visible="false" style="display:none;">
                                <asp:TextBox ID="TxtMICR" CssClass="form-control" Visible="false" TabIndex="40" runat="server" autocomplete="off"></asp:TextBox>
                            </div>

                            <!-- Aadhaar -->
                            <div class="reg-field" id="Div10" runat="server" visible="false">
                                <label>AADHAR No. <span class="req">*</span></label>
                                <div style="display: grid; grid-template-columns: 1fr 1fr 1fr; gap: 8px;">
                                    <asp:TextBox ID="TxtAAdhar1" CssClass="form-control" TabIndex="43" runat="server" onkeypress="return isNumberKey(event);" MaxLength="4" autocomplete="off"></asp:TextBox>
                                    <asp:TextBox ID="TxtAadhar2" CssClass="form-control" TabIndex="44" runat="server" onkeypress="return isNumberKey(event);" MaxLength="4" autocomplete="off"></asp:TextBox>
                                    <asp:TextBox ID="TxtAadhar3" CssClass="form-control" TabIndex="45" runat="server" onkeypress="return isNumberKey(event);" MaxLength="4" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>

                            <!-- Paymode -->
                            <div class="reg-field" runat="server" visible="false">
                                <label>Select Paymode <span class="req">*</span></label>
                                <asp:DropDownList ID="DdlPaymode" runat="server" AutoPostBack="true" CssClass="form-control" TabIndex="46" autocomplete="off"></asp:DropDownList>
                            </div>

                            <div class="reg-field" runat="server" visible="false">
                                <label><asp:Label ID="LblDDNo" runat="server" Text="Draft/CHEQUE No. *"></asp:Label></label>
                                <asp:TextBox ID="TxtDDNo" CssClass="form-control" TabIndex="47" runat="server" MaxLength="15" autocomplete="off"></asp:TextBox>
                            </div>

                            <div class="reg-field" runat="server" visible="false">
                                <label><asp:Label ID="LblDDDate" runat="server" Text="Draft/CHEQUE Date *"></asp:Label></label>
                                <asp:TextBox ID="TxtDDDate" runat="server" TabIndex="48" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtDDDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                            </div>

                            <div class="reg-field" runat="server" visible="false">
                                <label>Issued Bank Name</label>
                                <asp:TextBox ID="TxtIssueBank" CssClass="form-control" TabIndex="49" runat="server" autocomplete="off"></asp:TextBox>
                            </div>

                            <div class="reg-field" runat="server" visible="false">
                                <label>Issued Bank Branch</label>
                                <asp:TextBox ID="TxtIssueBranch" CssClass="form-control" TabIndex="50" runat="server" autocomplete="off"></asp:TextBox>
                            </div>

                            <!-- Transaction Password -->
                            <div class="reg-field" id="Div11" visible="false" runat="server">
                                <label>Transaction Password <span class="req">*</span></label>
                                <asp:TextBox ID="TxtTransactionPassword" CssClass="form-control validate[required,minSize[5],maxSize[10]]"
                                    TabIndex="52" runat="server" TextMode="Password" ValidationGroup="eInformation" autocomplete="off"></asp:TextBox>
                            </div>

                            <!-- Password / Confirm -->
                            <div id="divOtp" runat="server" visible="false" class="reg-field full">
                                <div class="reg-grid">
                                    <div class="reg-field">
                                        <label>Password <span class="req">*</span></label>
                                        <asp:TextBox ID="TxtPasswd" CssClass="form-control" TabIndex="51"
                                            runat="server" name="password" TextMode="Password"></asp:TextBox>
                                    </div>
                                    <div class="reg-field">
                                        <label>Confirm Password <span class="req">*</span></label>
                                        <asp:TextBox ID="pass2" runat="server" name="password" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" Display="Dynamic"
                                            ControlToValidate="TxtPasswd" runat="server"
                                            CssClass="reg-field-err"
                                            ErrorMessage="RequiredFieldValidator">confirm New Password can't left blank</asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator1" ControlToValidate="TxtPasswd" ControlToCompare="Pass2"
                                            Type="String" Operator="Equal" Text="Passwords must match!" runat="Server"
                                            CssClass="reg-field-err" ForeColor="#972f36" />
                                    </div>
                                    <div class="reg-field full" style="display: none">
                                        <label>Enter OTP Sent on your E-mail Id. <span class="req">*</span></label>
                                        <asp:TextBox ID="TxtOtp" CssClass="form-control validate[required]" runat="server"
                                            autocomplete="off" placeholder="Enter OTP" ValidationGroup="eInformation"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" Display="Dynamic" ControlToValidate="TxtOtp"
                                            runat="server" CssClass="reg-field-err" ValidationGroup="eInformation">Opt Required</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- /reg-grid -->

                        <!-- E-pin Error -->
                        <asp:Label ID="lblErrEpin" runat="server" CssClass="reg-err-msg"></asp:Label>

                        <!-- Terms -->
                        <div class="reg-terms">
                            <asp:CheckBox ID="chkterms" runat="server" onclick="DivOnOff();" TabIndex="53" />
                            I Agree With&nbsp;<a href="#" data-toggle="modal" data-target="#myModalTerm">Terms And Condition</a>.
                        </div>

                        <!-- General Error -->
                        <asp:Label ID="errMsg" runat="server" CssClass="reg-err-msg"></asp:Label>

                        <!-- Actions -->
                        <div class="reg-actions" id="DivTerms" runat="server" visible="true">
                            <asp:Button ID="CmdSave" runat="server" Text="Submit" CssClass="reg-btn reg-btn-primary"
                                TabIndex="54" OnClick="CmdSave_Click" />
                            <asp:Button ID="CmdCancel" runat="server" Text="Cancel" CssClass="reg-btn reg-btn-ghost"
                                ValidationGroup="eCancel" TabIndex="55" Visible="true" />
                        </div>

                        <div class="reg-actions">
                            <asp:Button ID="BtnOtp" runat="server" Text="Submit OTP" CssClass="reg-btn reg-btn-primary"
                                Visible="false" ValidationGroup="eInformation" OnClick="BtnOtp_Click" />
                            <asp:Button ID="ResendOtp" runat="server" Text="Resend OTP" CssClass="reg-btn reg-btn-ghost"
                                Visible="false" ValidationGroup="eInformation" OnClick="ResendOtp_Click" />
                        </div>

                        <!-- Already have account -->
                        <div class="reg-footer-login">
                            Already have an account?<a href="login.aspx">Sign in here</a>
                        </div>

                    </div>
                    <!-- /reg-form-card -->
                </div>

            </div>
        </div>
    </section>

    <!-- Popper / Bootstrap (kept from original) -->
    <script src="http://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js" integrity="sha384-I7E8VVD/ismYTF4hNIPjVp/Zjvgyol6VFvRkX/vR+Vc4jQkC+hVqc2pM8ODewa9r" crossorigin="anonymous"></script>
    <script src="http://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.min.js" integrity="sha384-BBtl+eGJRgqQAUMxJ7pMwbEyER4l1g+O15P+16Ep7Q9Q+zqX6gSbd85u4mG4QzX+" crossorigin="anonymous"></script>

    <script>
        $(document).ready(function () {
            function togglePasswordVisibility(button, inputField) {
                let passwordInput = $(inputField);
                let icon = $(button);
                if (passwordInput.attr('type') === 'password') {
                    passwordInput.attr('type', 'text');
                    icon.removeClass('fa-eye-slash').addClass('fa-eye');
                } else {
                    passwordInput.attr('type', 'password');
                    icon.removeClass('fa-eye').addClass('fa-eye-slash');
                }
            }
            $('#TxtPasswd').on('click', function () {
                togglePasswordVisibility(this, '#TxtPasswd');
            });
            $('#pass2').on('click', function () {
                togglePasswordVisibility(this, '#pass2');
            });
        });
    </script>
        <script>
            function switchTab(btn, tabId) {
                document.querySelectorAll('.cpd-tab-btn').forEach(b => b.classList.remove('active'));
                document.querySelectorAll('.cpd-tab-content').forEach(t => t.classList.remove('active'));
                btn.classList.add('active');
                document.getElementById(tabId).classList.add('active');
            }

            //const hamburgerBtn = document.getElementById('hamburgerBtn');
            //const mobileNav = document.getElementById('mobileNav');
            //if (hamburgerBtn && mobileNav) {
            //    hamburgerBtn.addEventListener('click', function () {
            //        const isOpen = mobileNav.classList.toggle('open');
            //        hamburgerBtn.classList.toggle('open', isOpen);
            //        hamburgerBtn.setAttribute('aria-expanded', isOpen);
            //    });
            //    mobileNav.querySelectorAll('a').forEach(function (link) {
            //        link.addEventListener('click', function () {
            //            mobileNav.classList.remove('open');
            //            hamburgerBtn.classList.remove('open');
            //            hamburgerBtn.setAttribute('aria-expanded', 'false');
            //        });
            //    });
            //    document.addEventListener('click', function (e) {
            //        if (!hamburgerBtn.contains(e.target) && !mobileNav.contains(e.target)) {
            //            mobileNav.classList.remove('open');
            //            hamburgerBtn.classList.remove('open');
            //            hamburgerBtn.setAttribute('aria-expanded', 'false');
            //        }
            //    });
            //}
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
