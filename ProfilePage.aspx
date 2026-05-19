<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="ProfilePage.aspx.cs" Inherits="ProfilePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<link href="https://fonts.googleapis.com/css2?family=Sora:wght@400;600;700;800&family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet">
<style>
  :root {
    --orange: #E84000;
    --orange-glow: rgba(232,64,0,0.10);
    --gray: #F5F6FA;
    --border: #E2E8F0;
    --text: #1A1A2E;
    --muted: #6B7280;
    --green: #16A34A;
  }

  .ep-page-wrap {
    background: #F0F2F8;
    min-height: 80vh;
    padding: 36px 20px;
    font-family: 'Nunito', sans-serif;
  }

  /* ── Page Header ── */
  .ep-page-header {
    max-width: 720px; margin: 0 auto 28px;
    background: linear-gradient(135deg, #0D1117 0%, #1a2744 60%, #0f1923 100%);
    border-radius: 18px; padding: 28px 32px;
    display: flex; align-items: center; justify-content: space-between;
    position: relative; overflow: hidden;
  }
  .ep-page-header::after {
    content: '';
    position: absolute; right: 0; top: 0; bottom: 0; width: 40%;
    background: radial-gradient(ellipse at right, rgba(232,64,0,.2), transparent);
    pointer-events: none;
  }
  .ep-page-header::before {
    content: '';
    position: absolute; top: 0; left: 0; right: 0; height: 3px;
    background: linear-gradient(90deg, var(--orange), #FF8C42);
  }
  .ep-ph-text .ep-badge {
    display: inline-flex; align-items: center; gap: 6px;
    background: rgba(232,64,0,.22); border: 1px solid rgba(232,64,0,.4);
    color: #FF8C42; font-size: .72rem; font-weight: 700; letter-spacing: .4px;
    padding: 4px 12px; border-radius: 50px; margin-bottom: 10px;
  }
  .ep-ph-text .ep-badge span {
    width: 7px; height: 7px; background: #FF8C42;
    border-radius: 50%; display: inline-block;
    animation: epPulse 1.5s infinite;
  }
  @keyframes epPulse {
    0%,100%{transform:scale(1);opacity:1;}
    50%{transform:scale(1.35);opacity:.7;}
  }
  .ep-ph-text h1 {
    font-family: 'Sora', sans-serif; color: #fff;
    font-size: 1.35rem; font-weight: 800; margin-bottom: 4px;
  }
  .ep-ph-text p { color: rgba(255,255,255,.55); font-size: .83rem; margin: 0; }
  .ep-ph-icon { font-size: 2.8rem; position: relative; z-index: 2; }

  /* ── Avatar Row ── */
  .ep-avatar-row {
    display: flex; align-items: center; gap: 16px;
    background: var(--gray); border: 1.5px solid var(--border);
    border-radius: 14px; padding: 16px 20px; margin-bottom: 24px;
  }
  .ep-avatar {
    width: 58px; height: 58px; border-radius: 50%;
    background: var(--orange); color: #fff;
    display: flex; align-items: center; justify-content: center;
    font-family: 'Sora', sans-serif; font-size: 1.25rem; font-weight: 800;
    flex-shrink: 0;
  }
  .ep-avatar-info h4 {
    font-family: 'Sora', sans-serif; font-size: 1rem; font-weight: 700;
    color: var(--text); margin-bottom: 2px;
  }
  .ep-avatar-info p { color: var(--muted); font-size: .82rem; margin: 0; }
  .ep-member-tag {
    display: inline-flex; align-items: center; gap: 5px;
    background: var(--orange-glow); color: var(--orange);
    border: 1px solid rgba(232,64,0,.25); border-radius: 50px;
    font-size: .7rem; font-weight: 700; padding: 3px 9px; margin-top: 5px;
  }

  /* ── Main Card ── */
  .ep-card {
    max-width: 720px; margin: 0 auto;
    background: #fff; border-radius: 18px;
    border: 1.5px solid var(--border);
    box-shadow: 0 4px 24px rgba(0,0,0,.06);
    overflow: hidden;
  }
  .ep-card-header {
    background: linear-gradient(135deg, #0D1117 0%, #1a2744 100%);
    padding: 22px 28px; position: relative; overflow: hidden;
  }
  .ep-card-header::before {
    content: '';
    position: absolute; top: 0; left: 0; right: 0; height: 3px;
    background: linear-gradient(90deg, var(--orange), #FF8C42);
  }
  .ep-card-header h4 {
    font-family: 'Sora', sans-serif; color: #fff;
    font-size: 1.05rem; font-weight: 700; margin-bottom: 3px;
  }
  .ep-card-header p { color: rgba(255,255,255,.55); font-size: .83rem; margin: 0; }
  .ep-card-body { padding: 28px; }

  /* ── Form Grid ── */
  .ep-form-grid {
    display: grid; grid-template-columns: 1fr 1fr; gap: 0 20px;
  }
  .ep-form-grid .ep-form-group.full { grid-column: 1 / -1; }

  /* ── Form Elements ── */
  .ep-form-group { margin-bottom: 18px; }
  .ep-form-group label {
    display: block; font-weight: 700; font-size: .85rem;
    color: var(--text); margin-bottom: 7px;
    font-family: 'Nunito', sans-serif;
  }
  .ep-form-group label .req { color: var(--orange); }
  .ep-form-group .form-control {
    width: 100%; padding: 11px 15px;
    border: 1.5px solid var(--border); border-radius: 10px;
    font-family: 'Nunito', sans-serif; font-size: .9rem;
    color: var(--text); background: #fff; outline: none;
    transition: border-color .2s, box-shadow .2s;
  }
  .ep-form-group .form-control:focus {
    border-color: var(--orange);
    box-shadow: 0 0 0 4px var(--orange-glow);
  }
  .ep-form-group .form-control[disabled],
  .ep-form-group .form-control[readonly] {
    background: var(--gray); color: var(--muted); cursor: not-allowed;
  }
  .ep-form-group .form-control::placeholder { color: #B0B7C3; }

  /* ── Disabled section label ── */
  .ep-section-label {
    font-family: 'Sora', sans-serif; font-size: .78rem; font-weight: 700;
    color: var(--muted); letter-spacing: .5px; text-transform: uppercase;
    margin: 6px 0 14px; padding-bottom: 8px;
    border-bottom: 1.5px solid var(--border);
    grid-column: 1 / -1;
  }

  /* ── Error label ── */
  .ep-err {
    display: block; margin-top: 5px;
    font-size: .8rem; font-weight: 700;
    color: var(--orange); font-family: 'Nunito', sans-serif;
  }

  /* ── Buttons ── */
  .ep-btn-primary {
    width: 100%; background: var(--orange); color: #fff; border: none;
    padding: 13px 26px; border-radius: 11px;
    font-weight: 800; font-size: .92rem; cursor: pointer;
    font-family: 'Nunito', sans-serif;
    transition: background .2s, transform .15s, box-shadow .2s;
  }
  .ep-btn-primary:hover {
    background: #c93500; transform: translateY(-1px);
    box-shadow: 0 8px 22px rgba(232,64,0,.3);
  }

  .ep-divider { border: none; border-top: 1.5px solid var(--border); margin: 22px 0; }

  /* ── Success message ── */
  .ep-success {
    display: none; margin-top: 14px;
    background: rgba(22,163,74,.1); border: 1px solid rgba(22,163,74,.3);
    border-radius: 10px; padding: 12px 16px;
    color: #15803D; font-size: .85rem; font-weight: 700;
    text-align: center; font-family: 'Nunito', sans-serif;
  }

  @media (max-width: 576px) {
    .ep-form-grid { grid-template-columns: 1fr; }
    .ep-form-grid .ep-form-group.full { grid-column: 1; }
    .ep-page-header { padding: 20px 18px; }
    .ep-ph-icon { display: none; }
  }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<div class="ep-page-wrap">

  <!-- Page Header -->
  <div class="ep-page-header">
    <div class="ep-ph-text">
      <div class="ep-badge"><span></span> Member Account</div>
      <h1>Profile Update</h1>
      <p>Manage your personal information &amp; account details</p>
    </div>
    <div class="ep-ph-icon">👤</div>
  </div>

  <!-- Main Card -->
  <div class="ep-card">
    <div class="ep-card-header">
      <h4>Profile Update</h4>
      <p>Discover the ultimate convenience with our all-in-one platform</p>
    </div>
    <div class="ep-card-body">

      <!-- Hidden + Error fields -->
      <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
      <asp:HiddenField ID="hdnSessn" runat="server" />
      <asp:Label ID="errMsg" runat="server" CssClass="ep-err"></asp:Label>

      <!-- Hidden sponsor/placement divs (unchanged logic) -->
      <div id="sponsordetail" runat="server" visible="false" style="display:none;">
        <div class="ep-form-group">
          <label>Sponsor ID</label>
          <asp:TextBox ID="txtReferalId" CssClass="form-control" TabIndex="1"
            runat="server" AutoPostBack="True" Enabled="False"></asp:TextBox>
        </div>
      </div>
      <div id="DivSponsorName" runat="server" visible="false" style="display:none;">
        <div class="ep-form-group">
          <label>Sponsor Name <span class="req">*</span></label>
          <asp:TextBox ID="TxtReferalNm" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
        </div>
      </div>
      <div id="DivUplinerId" runat="server" visible="false" style="display:none;">
        <div class="ep-form-group">
          <label>Placement ID <span class="req">*</span></label>
          <asp:TextBox ID="TxtUplinerid" CssClass="form-control" TabIndex="1"
            runat="server" AutoPostBack="True" Enabled="False"></asp:TextBox>
        </div>
      </div>
      <div id="DivUplinerName" runat="server" visible="false" style="display:none;">
        <div class="ep-form-group">
          <label>Placement Name <span class="req">*</span></label>
          <asp:TextBox ID="TxtUplinerName" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
        </div>
      </div>

      <!-- Hidden type dropdown (unchanged) -->
      <asp:DropDownList CssClass="form-control" ID="CmbType" runat="server"
        TabIndex="7" style="display:none;">
        <asp:ListItem Value="S/O" Text="S/O"></asp:ListItem>
        <asp:ListItem Value="D/O" Text="D/O"></asp:ListItem>
        <asp:ListItem Value="W/O" Text="W/O"></asp:ListItem>
        <asp:ListItem Value="C/O" Text="C/O"></asp:ListItem>
      </asp:DropDownList>
      <asp:TextBox ID="lblPosition" CssClass="form-control" runat="server"
        Enabled="false" style="display:none;"></asp:TextBox>
      <asp:TextBox ID="TxtDobDate" CssClass="form-control" runat="server"
        TabIndex="9" style="display:none;"></asp:TextBox>
      <asp:TextBox ID="txtPhNo" CssClass="form-control" runat="server"
        MaxLength="10" TabIndex="16" style="display:none;"></asp:TextBox>
      <div id="Divcardno" runat="server" visible="false" style="display:none;">
        <asp:TextBox ID="txtCardNo" CssClass="form-control" TabIndex="16" runat="server"></asp:TextBox>
      </div>

      <!-- ── Visible Form Grid ── -->
      <div class="ep-form-grid">

        <!-- Your Name (read-only) -->
        <div class="ep-form-group">
          <label>Your Name</label>
          <asp:TextBox ID="txtFrstNm" runat="server"
            CssClass="form-control"
            TabIndex="3"
            ValidationGroup="eInformation"
            ReadOnly="true">
          </asp:TextBox>
        </div>

        <!-- Mobile No. (disabled) -->
        <div class="ep-form-group">
          <label>Mobile No.</label>
          <asp:TextBox ID="txtMobileNo" runat="server"
            CssClass="form-control"
            onkeypress="return isNumberKey(event);"
            TabIndex="15" MaxLength="10"
            ValidationGroup="eInformation"
            Enabled="False">
          </asp:TextBox>
        </div>

        <!-- Father's Name -->
        <div class="ep-form-group">
          <label>Father's Name</label>
          <asp:TextBox ID="txtFNm" runat="server"
            CssClass="form-control"
            TabIndex="8"
            ValidationGroup="eInformation"
            placeholder="Enter father's name">
          </asp:TextBox>
        </div>

        <!-- E-Mail (disabled) -->
        <div class="ep-form-group">
          <label>E-Mail ID</label>
          <asp:TextBox ID="txtEMailId" runat="server"
            CssClass="form-control"
            TabIndex="17"
            Enabled="False">
          </asp:TextBox>
        </div>

        <!-- Nominee Name -->
        <div class="ep-form-group">
          <label>Nominee Name</label>
          <asp:TextBox ID="txtNominee" runat="server"
            CssClass="form-control"
            TabIndex="18"
            Enabled="true"
            placeholder="Enter nominee name">
          </asp:TextBox>
        </div>

        <!-- Relation (disabled) -->
        <div class="ep-form-group">
          <label>Relation</label>
          <asp:TextBox ID="txtRelation" runat="server"
            CssClass="form-control"
            TabIndex="19"
            Enabled="False"
            placeholder="e.g. Spouse, Child">
          </asp:TextBox>
        </div>

      </div>
      <!-- end grid -->

      <hr class="ep-divider">

      <!-- Update Button -->
      <asp:Button ID="CmdSave" runat="server"
        Text="Update Profile  ✓"
        CssClass="ep-btn-primary"
        TabIndex="27"
        ValidationGroup="eInformation"
        OnClick="CmdSave_Click" />

      <asp:Button ID="CmdCancel" runat="server"
        Text="Cancel"
        CssClass="ep-btn-primary"
        TabIndex="28"
        ValidationGroup="Form-Reset"
        Visible="false"
        style="background:var(--gray);color:var(--text);border:1.5px solid var(--border);margin-top:10px;" />

    </div>
  </div>

</div>

</asp:Content>
