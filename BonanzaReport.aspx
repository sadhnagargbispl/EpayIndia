<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BonanzaReport.aspx.cs" Inherits="BonanzaReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">BonanzaReportReport</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <label for="exampleInputEmail1">Member ID</label>
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="exampleInputEmail1">Bonanza</label>
                                        <asp:DropDownList ID="CmbKit" runat="server" class="form-control">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-md-3">
                                        <label for="exampleInputEmail1">PageSize</label>
                                        <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control">
                                            <asp:ListItem Text="10" Value="10" />
                                            <asp:ListItem Text="20" Value="20" />
                                            <asp:ListItem Text="50" Value="50" />
                                            <asp:ListItem Text="100" Value="100" />
                                            <asp:ListItem Text="200" Value="200" />
                                            <asp:ListItem Text="300" Value="300" />
                                            <asp:ListItem Text="400" Value="400" />
                                            <asp:ListItem Text="500" Value="500" />
                                            <asp:ListItem Text="600" Value="600" />
                                            <asp:ListItem Text="2000" Value="2000" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <br />
                                    <asp:Button ID="BtnShow" runat="server" Text="Search" TabIndex="3" ValidationGroup="Save"
                                        class="btn btn-primary" OnClick="BtnShow_Click" />
                                    <asp:Button ID="btnExport" runat="server" class="btn btn-primary"
                                        Text="Export To Excel" OnClick="btnExport_Click" />
                                </div>
                                <div id="doublescroll" class="col-md-12">
                                    <p>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div id="gvContainer" runat="server" class="table table-bordered" style="overflow: scroll">
                                                    <asp:Label ID="lblError" runat="server"></asp:Label>
                                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="true" AllowPaging="true" CssClass="table table-bordered"
                                                        HeaderStyle-CssClass="bg-primary" PageSize="10" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging">
                                                    </asp:GridView>

                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>


</asp:Content>


