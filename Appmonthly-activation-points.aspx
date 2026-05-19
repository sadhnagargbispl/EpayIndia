<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="Appmonthly-activation-points.aspx.cs" Inherits="Appmonthly_activation_points" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="demoepay/css/mobile-app.css?v=1.5" rel="stylesheet" />
    <style>
        /* ═══ ACTIVATION POINTS SPECIFIC ═══ */

        .map-list {
            padding: 8px 16px 16px;
            display: flex;
            flex-direction: column;
            gap: 14px;
        }

        .map-card {
            background: #fff;
            border-radius: var(--radius);
            overflow: hidden;
            box-shadow: var(--shadow);
            transition: transform .2s, box-shadow .2s;
            cursor: pointer;
        }

            .map-card:active {
                transform: scale(.99);
            }

        .map-card-body {
            display: flex;
            align-items: stretch;
        }

        .map-left {
            background: linear-gradient(160deg, #fff7ed, #ffedd5);
            padding: 18px 14px;
            min-width: 110px;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            text-align: center;
            flex-shrink: 0;
        }

        .map-icon {
            font-size: 1.7rem;
            margin-bottom: 6px;
        }

        .map-pkg-label {
            font-size: .58rem;
            font-weight: 700;
            letter-spacing: .7px;
            text-transform: uppercase;
            color: var(--muted);
            margin-bottom: 4px;
        }

        .map-pkg-amount {
            font-size: 1.4rem;
            font-weight: 800;
            color: var(--primary);
            line-height: 1;
        }

            .map-pkg-amount sup {
                font-size: .8rem;
                vertical-align: top;
                margin-top: 3px;
                display: inline-block;
                color: rgba(232,64,0,.7);
            }

        .map-divider {
            width: 16px;
            position: relative;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
        }

            .map-divider::before, .map-divider::after {
                content: '';
                position: absolute;
                width: 14px;
                height: 14px;
                background: var(--bg);
                border-radius: 50%;
            }

            .map-divider::before {
                top: -7px;
            }

            .map-divider::after {
                bottom: -7px;
            }

        .map-divider-line {
            width: 1.5px;
            height: 70%;
            border-left: 1.5px dashed #d1d5db;
        }

        .map-right {
            flex: 1;
            padding: 14px 14px 14px 10px;
            display: flex;
            flex-direction: column;
            gap: 8px;
            justify-content: center;
            min-width: 0;
        }

        .map-info {
            display: flex;
            align-items: center;
            gap: 8px;
            font-size: .8rem;
        }

        .map-info-label {
            color: var(--muted);
            font-weight: 600;
        }

        .map-info-value {
            font-weight: 800;
            color: var(--text);
            margin-left: auto;
        }

        .map-info.cashback .map-info-value {
            color: var(--green);
        }

        .map-info i {
            width: 22px;
            height: 22px;
            border-radius: 6px;
            background: rgba(232,64,0,.1);
            color: var(--primary);
            font-size: .7rem;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .map-info.cashback i {
            background: rgba(22,163,74,.1);
            color: var(--green);
        }

        .map-btn {
            display: block;
            background: var(--primary);
            color: #fff;
            border: none;
            padding: 9px 0;
            border-radius: 50px;
            font-size: .78rem;
            font-weight: 800;
            cursor: pointer;
            text-decoration: none;
            text-align: center;
            margin-top: 4px;
            box-shadow: 0 4px 12px rgba(232,64,0,.25);
            transition: all .2s;
        }

            .map-btn:hover {
                background: var(--primary-d);
                transform: translateY(-1px);
            }

        /* Stats banner */
        .stats-banner {
            margin: 16px 16px 4px;
            background: #fff;
            border-radius: var(--radius);
            padding: 14px;
            box-shadow: var(--shadow);
            display: flex;
            align-items: center;
            justify-content: space-around;
        }

        .stat-item {
            text-align: center;
        }

        .stat-num {
            font-size: 1.2rem;
            font-weight: 800;
            color: var(--primary);
            line-height: 1;
        }

        .stat-label {
            font-size: .62rem;
            font-weight: 700;
            color: var(--muted);
            text-transform: uppercase;
            letter-spacing: .4px;
            margin-top: 4px;
        }

        .stat-div {
            width: 1px;
            height: 30px;
            background: var(--border);
        }

        /* Info banner */
        .info-banner {
            margin: 14px 16px;
            background: linear-gradient(135deg, #f0fdf4, #dcfce7);
            border: 1px solid rgba(22,163,74,.25);
            border-radius: var(--radius);
            padding: 14px 16px;
            display: flex;
            align-items: center;
            gap: 12px;
        }

        .info-banner-icon {
            width: 36px;
            height: 36px;
            border-radius: 10px;
            background: var(--green);
            color: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1rem;
            flex-shrink: 0;
        }

        .info-banner-text {
            font-size: .8rem;
            color: var(--text);
            line-height: 1.5;
            font-weight: 600;
        }

            .info-banner-text strong {
                color: var(--green);
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- HERO -->
    <div class="hero">
        <div class="hero-badge">
            <div class="hero-badge-dot"></div>
            Activation Plans
     
        </div>
        <div class="hero-title">Monthly <span>Activation Points</span></div>
        <div class="hero-sub">Purchase any package &amp; earn up to 200% cashback points</div>
    </div>

    <!-- STATS BANNER -->
    <div class="stats-banner">
        <div class="stat-item">
            <div class="stat-num">4</div>
            <div class="stat-label">Plans</div>
        </div>
        <div class="stat-div"></div>
        <div class="stat-item">
            <div class="stat-num">200%</div>
            <div class="stat-label">Cashback</div>
        </div>
        <div class="stat-div"></div>
        <div class="stat-item">
            <div class="stat-num">360</div>
            <div class="stat-label">Max Days</div>
        </div>
    </div>

    <!-- INFO BANNER -->
    <div class="info-banner">
        <div class="info-banner-icon"><i class="fa fa-gift"></i></div>
        <div class="info-banner-text">
            Get <strong>up to 200% cashback</strong> on any activation plan you purchase!
     
        </div>
    </div>

    <!-- SECTION LABEL -->
    <div class="sec-label">Choose Your Activation Plan</div>

    <!-- ACTIVATION CARDS -->
    <div class="map-list">

        <!-- Plan 1 -->
        <asp:Repeater ID="rptPackages"
            runat="server"
            OnItemCommand="rptPackages_ItemCommand">

            <ItemTemplate>
                <div class="map-card">
                    <div class="map-card-body">
                        <div class="map-left">
                            <asp:HiddenField ID="hdnKitId"
                                runat="server"
                                Value='<%# Eval("KitId") %>' />

                            <asp:HiddenField ID="hdnAmount"
                                runat="server"
                                Value='<%# Eval("kitamount") %>' />
                            <div class="map-icon">
                                <%# 
    Convert.ToInt32(Eval("KitId")) == 5 ? "⚡" :
    Convert.ToInt32(Eval("KitId")) == 8 ? "🚀" :
    Convert.ToInt32(Eval("KitId")) == 9 ? "💎" :
    "👑"
%>
                            </div>
                            <div class="map-pkg-label">Package</div>
                            <div class="map-pkg-amount"><sup>₹</sup><%# Eval("kitamount") %></div>
                        </div>
                        <div class="map-divider">
                            <div class="map-divider-line"></div>
                        </div>
                        <div class="map-right">
                            <div class="map-info">
                                <i class="fa fa-clock"></i>
                                <span class="map-info-label">Valid Days</span>
                                <span class="map-info-value"><%# Eval("MonthlyDay") %> Days</span>
                            </div>
                            <div class="map-info cashback">
                                <i class="fa fa-coins"></i>
                                <span class="map-info-label">Cashback</span>
                                <span class="map-info-value"><%# Eval("CashbackPoints") %> Pts</span>
                            </div>
                            <asp:LinkButton ID="btnActivate"
                                runat="server"
                                CssClass="map-btn"
                                Text="Activate Plan →"
                                CommandName="Activate"
                                OnClientClick="
if(this.getAttribute('data-clicked')=='1'){return false;}
this.setAttribute('data-clicked','1');
this.innerText='Processing...';
this.style.pointerEvents='none';
this.style.opacity='0.6';
">
                            </asp:LinkButton>
                            <%-- <a href="#" class="map-btn" onclick="showToast('Activating ₹300 plan...')">Activate Plan →</a>--%>
                        </div>
                    </div>
                </div>
                <%--   <div class="map-card" data-anim>

                    

                    <div class="map-card-header">
                        <div class="map-card-icon">
                            <%# 
    Convert.ToInt32(Eval("KitId")) == 5 ? "⚡" :
    Convert.ToInt32(Eval("KitId")) == 8 ? "🚀" :
    Convert.ToInt32(Eval("KitId")) == 9 ? "💎" :
    "👑"
%>
                        </div>

                        <div class="map-card-pkg-label">
                            Package Amount
                        </div>

                        <div class="map-card-pkg-amount">
                            <sup>₹ </sup><%# Eval("kitamount") %>
                        </div>
                    </div>

                    <div class="map-card-notch">
                        <div class="map-card-notch-circle"></div>
                        <div class="map-card-notch-line"></div>
                        <div class="map-card-notch-circle"></div>
                    </div>

                    <div class="map-card-body">

                        <div class="map-info-row">
                            <div class="map-info-label">⏱ Valid Days</div>
                            <div class="map-info-value">
                                <%# Eval("MonthlyDay") %>
                            </div>
                        </div>

                        <div class="map-info-row cashback">
                            <div class="map-info-label">
                                🎯 Cashback Points
                            </div>

                            <div class="map-info-value">
                                <%# Eval("CashbackPoints") %>
                            </div>
                        </div>

                       
                    </div>
                </div>--%>
            </ItemTemplate>

        </asp:Repeater>
        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
        <%--    <div class="map-card">
            <div class="map-card-body">
                <div class="map-left">
                    <div class="map-icon">⚡</div>
                    <div class="map-pkg-label">Package</div>
                    <div class="map-pkg-amount"><sup>₹</sup>300</div>
                </div>
                <div class="map-divider">
                    <div class="map-divider-line"></div>
                </div>
                <div class="map-right">
                    <div class="map-info">
                        <i class="fa fa-clock"></i>
                        <span class="map-info-label">Valid Days</span>
                        <span class="map-info-value">30 Days</span>
                    </div>
                    <div class="map-info cashback">
                        <i class="fa fa-coins"></i>
                        <span class="map-info-label">Cashback</span>
                        <span class="map-info-value">600 Pts</span>
                    </div>
                    <a href="#" class="map-btn" onclick="showToast('Activating ₹300 plan...')">Activate Plan →</a>
                </div>
            </div>
        </div>--%>

        <!-- Plan 2 -->
        <%-- <div class="map-card">
            <div class="map-card-body">
                <div class="map-left">
                    <div class="map-icon">🚀</div>
                    <div class="map-pkg-label">Package</div>
                    <div class="map-pkg-amount"><sup>₹</sup>900</div>
                </div>
                <div class="map-divider">
                    <div class="map-divider-line"></div>
                </div>
                <div class="map-right">
                    <div class="map-info">
                        <i class="fa fa-clock"></i>
                        <span class="map-info-label">Valid Days</span>
                        <span class="map-info-value">90 Days</span>
                    </div>
                    <div class="map-info cashback">
                        <i class="fa fa-coins"></i>
                        <span class="map-info-label">Cashback</span>
                        <span class="map-info-value">2,100 Pts</span>
                    </div>
                    <a href="#" class="map-btn" onclick="showToast('Activating ₹900 plan...')">Activate Plan →</a>
                </div>
            </div>
        </div>--%>

        <!-- Plan 3 -->
        <%--    <div class="map-card">
            <div class="map-card-body">
                <div class="map-left">
                    <div class="map-icon">💎</div>
                    <div class="map-pkg-label">Package</div>
                    <div class="map-pkg-amount"><sup>₹</sup>1,800</div>
                </div>
                <div class="map-divider">
                    <div class="map-divider-line"></div>
                </div>
                <div class="map-right">
                    <div class="map-info">
                        <i class="fa fa-clock"></i>
                        <span class="map-info-label">Valid Days</span>
                        <span class="map-info-value">180 Days</span>
                    </div>
                    <div class="map-info cashback">
                        <i class="fa fa-coins"></i>
                        <span class="map-info-label">Cashback</span>
                        <span class="map-info-value">4,200 Pts</span>
                    </div>
                    <a href="#" class="map-btn" onclick="showToast('Activating ₹1,800 plan...')">Activate Plan →</a>
                </div>
            </div>
        </div>--%>

        <!-- Plan 4 -->
       <%-- <div class="map-card">
            <div class="map-card-body">
                <div class="map-left">
                    <div class="map-icon">👑</div>
                    <div class="map-pkg-label">Package</div>
                    <div class="map-pkg-amount"><sup>₹</sup>3,600</div>
                </div>
                <div class="map-divider">
                    <div class="map-divider-line"></div>
                </div>
                <div class="map-right">
                    <div class="map-info">
                        <i class="fa fa-clock"></i>
                        <span class="map-info-label">Valid Days</span>
                        <span class="map-info-value">360 Days</span>
                    </div>
                    <div class="map-info cashback">
                        <i class="fa fa-coins"></i>
                        <span class="map-info-label">Cashback</span>
                        <span class="map-info-value">8,400 Pts</span>
                    </div>
                    <a href="#" class="map-btn" onclick="showToast('Activating ₹3,600 plan...')">Activate Plan →</a>
                </div>
            </div>
        </div>--%>

    </div>
</asp:Content>

