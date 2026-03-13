<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IdActivate.aspx.cs" Inherits="IdActivate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function confirmation() {
            if (confirm('Are you sure about this action ?')) {
                return true;
            } else {
                return false;
            }
        }
    </script>

    <style type="text/css">
        .col-md-12
        {
            margin-bottom: 10px;
        }
    </style>
    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font-family: Arial;
        }
        .modal1
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        .center1
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .center1 img
        {
            height: 128px;
            width: 128px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      <div class="content-wrapper">
     <!-- Main content -->
     <section class="content">
         <div class="container-fluid">
             <div class="row">
                 <div class="col-12">
                     <div class="card card-primary">
                         <div class="card-header">
                             <h3 class="card-title">ID Activation
                                </h3>
                         </div>
                         <div class="card-body">
                              <div align="center">
                            <span id="lblt" class="text-danger"></span>
                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        </div>
                          <div class="table-responsive makeitresponsivegrid">
                            <div align="center">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label ID="LblCondition" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                            </div>
                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Member ID :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="TxtIDNo_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                                    ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                <asp:Label ID="LblKitId" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblKitId1" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblNewKitid" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="LblFormno" runat="server" Visible="false"></asp:Label>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="DDlKit" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Member Name :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="LblMemName" runat="server" class="form-control"></asp:Label></div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Current Package :
                                            </div>
                                            <div class="col-md-4">
                                                <asp:Label ID="LblKitName" runat="server" class="form-control"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                Package Name :</div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DDlKit" runat="server" class="form-control">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <div class="col-md-12">
                                                    <div class="col-md-4">
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:Button ID="BtnUpgrade" runat="server" Text="Upgrade" class="btn btn-primary"
                                                            ValidationGroup="Save" Enabled="false" OnClientClick="return confirmation();"  OnClick="BtnUpgrade_Click"/>
                                                        <asp:Button ID="BtnCancel" class="btn btn-primary" runat="server" Text="Cancel"  OnClick="BtnCancel_Click"/>
                                                    </div>
                                                    <div class="col-md-4">
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="DDlKit" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <div class="col-md-12">
                                            <asp:DataGrid ID="GrdDirects1" runat="server" CssClass="table table-striped table-advance table-hover"
                                                RowStyle-Height="25px" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                CellPadding="3" HorizontalAlign="Center" AutoGenerateColumns="False" AllowPaging="True"
                                                Width="100%" ShowHeader="true" PageSize="5" EmptyDataText="No data to display."
                                                Visible="false">
                                                <Columns>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <%#Container.DataSetIndex + 1%>.
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="IDNO" HeaderText="ID No">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="MemName" HeaderText="Member Name">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="KitName" HeaderText="Package Name">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="UpgradeDate" HeaderText="Top Up Date">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                                    </asp:BoundColumn>
                                                </Columns>
                                                <PagerStyle Mode="NumericPages" CssClass="PagerStyle"></PagerStyle>
                                             
                                            </asp:DataGrid>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="BtnUpgrade" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="DDlKit" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                         </div>
                     </div>
                 </div>
             </div>
         </div>
     </section>
 </div>

</asp:Content>

