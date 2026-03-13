<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BlockIdReport.aspx.cs" Inherits="BlockIdReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
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
                                <h3 class="card-title">Block ID Report</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        Member ID :
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>


                                    <div class="col-md-2">
                                        PageSize:
                                        <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <br />
                                        <asp:Button ID="BtnShow" runat="server" class="btn btn-primary" Text="Show Detail" OnClick="BtnShow_Click" />
                                        <asp:Button ID="btnExport" runat="server" class="btn btn-primary"
                                            Text="Export To Excel" OnClick="btnExport_Click  " />
                                    </div>
                                </div>
                                <div id="doublescroll" class="col-md-12">
                                    <p>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div id="gvContainer" runat="server" class="table table-bordered" style="overflow: scroll">
                                                    <asp:Label ID="lblCount" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lblError" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>
                                                    <asp:Label ID="lblinv" runat="server" Style="font-weight: bold; font-size: 12px; color: Gray"></asp:Label>

                                                    <asp:GridView ID="GvData1" runat="server" AutoGenerateColumns="true" AllowPaging="true" CssClass="table table-bordered"
                                                        HeaderStyle-CssClass="bg-primary" PageSize="20" EmptyDataText="No data to display." OnPageIndexChanging="GrdTotal1_PageIndexChanging">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="SNo.">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
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
