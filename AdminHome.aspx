<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AdminHome.aspx.cs" Inherits="AdminHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">

        window.addEventListener('mouseup', function (event) {
            var box = document.getElementById('contextMenu');
            var Btn = document.getElementById('DivOptions');
            if (event.target != Btn && event.target.parentnode != box) {
                box.style.display = 'none';
            }
        });

        function ShowMenu(Dv, control, e) {
            var posx = e.clientX + window.pageXOffset + 'px'; //Left Position of Mouse Pointer
            var posy = e.clientY + window.pageYOffset + 'px'; //Top Position of Mouse Pointer
            var StrArr = Dv.id.split(';');
            var el = StrArr[0];
            var IsActive = StrArr[1];
            var IsBlock = StrArr[2];
            var Site = StrArr[3];
            var Passw = StrArr[4];
            document.getElementById(control).style.position = 'absolute';
            document.getElementById(control).style.display = 'inline';
            document.getElementById(control).style.left = posx;
            document.getElementById(control).style.top = posy;
            var currentdate = new Date();
            var TmID = currentdate.getDate().toString() + currentdate.getHours().toString() + currentdate.getFullYear().toString() + currentdate.getMonth().toString();
            //alert(Site);
            var inHtm = ' <li><a href="' + Site + '/Default.aspx?lgnT=' + Passw + '&ID=' + TmID + '" target="_blank">View Account</a></li>';
            inHtm = inHtm + ' <li class="separator"><a href="Profile.aspx?key=' + el + '">Update Profile</a></li>';
            //            inHtm = inHtm + '<li class="separator"><a href="binarytree.aspx?key=' + el + '">View Tree</a></li>';

            if (IsBlock == 'Y')
                inHtm = inHtm + '<li class="separator"><a href="UnBlock.aspx?Tp=S&key=' + el + '">Unblock</a></li>';
            else
                inHtm = inHtm + '<li class="separator"><a href="Block.aspx?Tp=S&key=' + el + '">Block Now</a></li>';
            document.getElementById(control).innerHTML = inHtm
        }
    </script>
    <style type="text/css">
        .ContextItem {
            width: 150px;
            background: #337ab7;
            color: Black;
            font-weight: normal;
            text-align: left;
        }

            .ContextItem LI {
                width: 150px;
                list-style: none;
                padding: 0px;
                margin: 0px;
            }

                .ContextItem LI:hover {
                    font-weight: bold;
                    background: #C2CEEB;
                    width: 110px;
                }

            .ContextItem A {
                width: 150px;
                color: #fff;
                text-decoration: none;
                line-height: 20px;
                height: 20px;
            }

            .ContextItem LI.separator {
                border-top: solid 0px #CCC;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <div class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1 class="m-0">Home</h1>
                    </div>
                    <!-- /.col -->

                    <!-- /.col -->
                </div>
                <!-- /.row -->
            </div>
            <!-- /.container-fluid -->
        </div>
        <!-- /.content-header -->

        <!-- Main content -->
        <section class="content">


            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Home</h3>
                            </div>
                            <!-- /.card-header -->
                            <!-- form start -->
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlSearch" runat="server" class="form-control">
                                            <asp:ListItem Value="0" Selected="True">Search Type</asp:ListItem>
                                            <asp:ListItem Value="IDNo">Distributor ID</asp:ListItem>
                                            <asp:ListItem Value="MemName">Name</asp:ListItem>
                                            <asp:ListItem Value="StateName">State</asp:ListItem>
                                            <asp:ListItem Value="EMail">Email</asp:ListItem>
                                            <asp:ListItem Value="DOJ">Joining Date</asp:ListItem>
                                            <asp:ListItem Value="KitName">Package Name</asp:ListItem>
                                           <%-- <asp:ListItem Value="walletaddress">Wallet Address</asp:ListItem>--%>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp; &nbsp; &nbsp;
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtSrchText" runat="server" class="form-control"></asp:TextBox>
                                    </div>

                                </div>
                                <div class="col-3">
                                    <div class="form-group">
                                        <br />
                                        <asp:Button ID="BtnSearch" runat="server" class="btn btn-primary" Text="Search" OnClick="BtnSearch_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary"
                                            Text="Export To Excel" OnClick="btnExport_Click " Visible ="false"  />
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Visible="false" Font-Size="13px"></asp:Label>
                                </div>
                                <div style="overflow: scroll" class="table table-bordered">
                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                        GridLines="Both" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                        ShowHeader="true" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNo.">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Member ID" SortExpression="IdNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="MemberID" runat="server" Text='<%# Eval("IDNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" SortExpression="Qstr">
                                                <ItemTemplate>
                                                    <%#Eval("Qstr")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        <%--    <asp:TemplateField HeaderText="Proposer Name" SortExpression="ProposerName">
                                                <ItemTemplate>
                                                    <%#Eval("ProposerName")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Sponsor ID" SortExpression="SponsorId">
                                                <ItemTemplate>
                                                    <asp:Label ID="SponsorId" runat="server" Text='<%# Eval("SponsorId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sponsor Name" SortExpression="SponsorName">
                                                <ItemTemplate>
                                                    <asp:Label ID="SponsorName" runat="server" Text='<%# Eval("SponsorName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Joining Date" ControlStyle-Width="150px" ItemStyle-Width="150px" SortExpression="Doj">
                                                <ItemTemplate>
                                                    <asp:Label ID="Doj" runat="server" Text='<%# Eval("Doj") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Click Here!">
                                                <ItemTemplate>

                                                    <div id="DivOptions">
                                                        <div id='<%# Eval("IDNo") +";"+ Eval("ActiveStatus") +";"+ Eval("IsBlock") +";"+ Eval("Site") +";"+ Eval("LgnID")  %>'
                                                            style="background-color: #99CCCC; color: Black; font-weight: bold; font-size: 12px"
                                                            onclick="ShowMenu(this,'contextMenu',event);">
                                                            <asp:Image runat="server" ID="Image1" ImageUrl="~/img/moreopt.jpg" AlternateText="More Option.." />
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mobile No." SortExpression="MobileNo" >
                                                <ItemTemplate>
                                                    <asp:Label ID="Mobile" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email" SortExpression="Email">
                                                <ItemTemplate>
                                                    <asp:Label ID="Email" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Package" SortExpression="KitName">
                                                <ItemTemplate>
                                                    <asp:Label ID="Package" runat="server" Text='<%# Eval("KitName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Package Amount" SortExpression="KitAmount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="PackageAmount" runat="server" Text='<%# Eval("KitAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Package Bv" SortExpression="BV" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="PackageBv" runat="server" Text='<%# Eval("Bv") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="City" Visible="false" SortExpression="City">
                                                <ItemTemplate>
                                                    <asp:Label ID="City" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="State" SortExpression="StateName" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="State" runat="server" Text='<%# Eval("StateName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Password" Visible="false" SortExpression="Passw">
                                                <ItemTemplate>
                                                    <asp:Label ID="Password" runat="server" Text='<%# Eval("Passw") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Activ.Date" SortExpression="UpgrdDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="UpgrdDate" runat="server" Text='<%# Eval("UpgrdDate") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Wallet Address" SortExpression="walletaddress" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="Walletaddress" runat="server" Text='<%# Eval("walletaddress") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="Status" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="E-Wallet" Visible="false" SortExpression="Balance">
                                                <ItemTemplate>
                                                    <asp:Label ID="Wallet" runat="server" Text='<%# Eval("Balance") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            
                                        </Columns>
                                    </asp:GridView>

                                </div>
                            </div>

                            <div class="row">
                            </div>
                        </div>
                        <!-- /.card-body -->


                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>
        <!-- /.content -->
    </div>
    <ul id="contextMenu" class="ContextItem">
    </ul>
</asp:Content>

