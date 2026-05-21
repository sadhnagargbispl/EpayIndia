<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="DeleteAccount.aspx.cs" Inherits="DeleteAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="demoepay/css/mobile-app.css?v=1.5" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@tabler/icons-webfont@latest/tabler-icons.min.css" />
    <style>
        * { box-sizing: border-box; }

        .da-hero {
            background: #1a1a2e;
            padding: 18px 18px 32px;
            position: relative;
            overflow: hidden;
        }

        .da-hero::before {
            content: '';
            position: absolute;
            top: -30px;
            right: -30px;
            width: 130px;
            height: 130px;
            border-radius: 50%;
            background: rgba(232,64,0,0.13);
        }

        .da-topbar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 22px;
        }

        .da-icon-btn {
            width: 38px;
            height: 38px;
            border-radius: 10px;
            border: 1px solid rgba(255,255,255,0.15);
            background: rgba(255,255,255,0.08);
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            text-decoration: none;
        }

        .da-icon-btn i { color: #fff; font-size: 18px; }
        .da-icon-btn.round { border-radius: 50%; }

        .da-badge {
            display: inline-flex;
            align-items: center;
            gap: 7px;
            background: rgba(232,64,0,0.18);
            border: 1px solid rgba(232,64,0,0.35);
            border-radius: 50px;
            padding: 5px 14px;
            font-size: 11px;
            font-weight: 700;
            letter-spacing: 1px;
            text-transform: uppercase;
            color: #ff8a65;
        }

        .da-dot {
            width: 6px;
            height: 6px;
            border-radius: 50%;
            background: #ff5722;
            animation: blink 1.8s ease-in-out infinite;
        }

        @keyframes blink {
            0%, 100% { opacity: 1; transform: scale(1); }
            50% { opacity: 0.4; transform: scale(0.65); }
        }

        .da-hero-title {
            margin-top: 12px;
            font-size: 21px;
            font-weight: 700;
            color: #fff;
            line-height: 1.25;
        }

        .da-hero-sub {
            margin-top: 5px;
            font-size: 13px;
            color: rgba(255,255,255,0.45);
        }

        .da-body {
            padding: 16px;
            margin-top: -14px;
            display: flex;
            flex-direction: column;
            gap: 14px;
        }

        .da-card {
            background: #fff;
            border-radius: 20px;
            padding: 20px;
            border: 0.5px solid #e5e7eb;
        }

        .da-card-label {
            font-size: 11px;
            font-weight: 700;
            letter-spacing: 0.9px;
            text-transform: uppercase;
            color: #6b7280;
            margin-bottom: 16px;
            padding-bottom: 12px;
            border-bottom: 0.5px solid #e5e7eb;
            display: flex;
            align-items: center;
            gap: 7px;
        }

        .da-card-label i { color: #e84000; font-size: 15px; }

        .da-warn {
            display: flex;
            align-items: flex-start;
            gap: 10px;
            background: rgba(232,64,0,0.07);
            border: 1px solid rgba(232,64,0,0.2);
            border-radius: 12px;
            padding: 11px 13px;
            margin-bottom: 16px;
        }

        .da-warn i {
            color: #e84000;
            font-size: 16px;
            flex-shrink: 0;
            margin-top: 1px;
        }

        .da-warn-text {
            font-size: 12.5px;
            color: #6b7280;
            line-height: 1.55;
        }

        .da-warn-text strong { color: #e84000; font-weight: 700; }

        .da-field { margin-bottom: 14px; }

        .da-field-lbl {
            font-size: 12px;
            font-weight: 600;
            color: #6b7280;
            margin-bottom: 6px;
            display: flex;
            align-items: center;
            gap: 3px;
        }

        .da-req { color: #e84000; font-size: 15px; line-height: 1; }

        .da-inp-wrap { position: relative; }

        .da-inp-ico {
            position: absolute;
            left: 12px;
            top: 50%;
            transform: translateY(-50%);
            color: #9ca3af;
            font-size: 15px;
            pointer-events: none;
        }

        .da-inp,
        .da-inp.form-control {
            width: 100% !important;
            padding: 11px 12px 11px 36px !important;
            border-radius: 12px !important;
            border: 0.5px solid #d1d5db !important;
            background: #f9fafb !important;
            font-size: 14px !important;
            color: #111827 !important;
            outline: none;
            transition: border-color 0.2s;
            box-shadow: none !important;
        }

        .da-inp:focus { border-color: #e84000 !important; background: #fff !important; }
        .da-inp:disabled { opacity: 0.55; cursor: not-allowed; }

        .da-btn {
            width: 100%;
            padding: 14px;
            border-radius: 50px;
            border: none;
            background: #e84000;
            color: #fff;
            font-size: 14px;
            font-weight: 700;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            letter-spacing: 0.3px;
            transition: background 0.2s;
            margin-top: 18px;
        }

        .da-btn:hover { background: #c43600; }

        .da-secure {
            text-align: center;
            font-size: 11.5px;
            color: #9ca3af;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 5px;
            margin-top: 12px;
        }

        .da-secure i { font-size: 13px; color: #16a34a; }

        /* Bottom Nav */
        .da-nav {
            background: #fff;
            border-top: 0.5px solid #e5e7eb;
            display: flex;
            padding: 10px 0 14px;
            border-radius: 0 0 16px 16px;
        }

        .da-nav-item {
            flex: 1;
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 4px;
            font-size: 10.5px;
            color: #9ca3af;
            cursor: pointer;
            font-weight: 500;
            text-decoration: none;
        }

        .da-nav-item i { font-size: 20px; }
        .da-nav-item.active { color: #e84000; }

        /* Hide default label/controls styling from old layout */
        .control-group { margin-bottom: 0; }
        .controls { margin-left: 0; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
    <asp:Label ID="lblFormno" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="LblMobl" runat="server" Visible="false"></asp:Label>


        <!-- HERO -->
    <div class="da-hero">
        
        <div class="da-badge">
            <div class="da-dot"></div>
            Delete Account
        </div>
        <div class="da-hero-title">Remove Your Account</div>
        <div class="da-hero-sub">This action is permanent and cannot be undone</div>
    </div>

    <!-- BODY -->
    <div class="da-body">
        <div class="da-card">
            <div class="da-card-label">
                <i class="ti ti-clipboard-list"></i> Account Deletion
            </div>

            <!-- Warning -->
            <div class="da-warn">
                <i class="ti ti-alert-triangle"></i>
                <div class="da-warn-text">
                    <strong>Warning:</strong> Deleting your account will permanently remove all your data, balance, and transaction history.
                </div>
            </div>

            <!-- ID Field -->
            <div class="da-field">
                <div class="da-field-lbl">
                    ID <span class="da-req">*</span>
                </div>
                <div class="da-inp-wrap">
                    <i class="ti ti-id-badge da-inp-ico"></i>
                    <asp:TextBox ID="TxtSerialno" runat="server"
                        CssClass="da-inp"
                        placeholder="Enter account ID"
                        AutoPostBack="true"
                        OnTextChanged="TxtSerialno_TextChanged">
                    </asp:TextBox>
                </div>
            </div>

            <!-- Name Field -->
            <div class="da-field" id="DivMemberName" runat="server">
                <div class="da-field-lbl">
                    Name <span class="da-req">*</span>
                </div>
                <div class="da-inp-wrap">
                    <i class="ti ti-user da-inp-ico"></i>
                    <asp:TextBox ID="TxtSpName" runat="server"
                        CssClass="da-inp"
                        placeholder="Account holder name"
                        Enabled="false">
                    </asp:TextBox>
                </div>
            </div>

            <!-- Error Label -->
            <asp:Label ID="LblError" runat="server" Visible="false"
                Style="color:#e84000; font-size:13px; display:block; margin-top:8px;">
            </asp:Label>

            <!-- Delete Button -->
            <asp:Button ID="cmdSave1" runat="server"
                Text="Delete Account"
                CssClass="da-btn"
                ValidationGroup="Validation"
                OnClick="cmdSave1_Click"
                OnClientClick="return confirm('Are you sure you want to delete this account? This cannot be undone.');" />

            <!-- Secure text -->
            <div class="da-secure">
                <i class="ti ti-shield-check"></i> Secured &amp; encrypted
            </div>
        </div>

        <!-- Bottom Nav -->
        
    </div>
</asp:Content>
