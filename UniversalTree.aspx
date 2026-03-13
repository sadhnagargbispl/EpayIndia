<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UniversalTree.aspx.cs" Inherits="UniversalTree" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Pool Tree</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                        <div class="x_panel">

                                            <div class="panel-body">
                                                <div align="center">
                                                    <span id="lblt" class="text-danger"></span>
                                                </div>
                                                <div class="row">

                                                    <div class="col-md-2">
                                                        <label for="inputdefault">
                                                            Downline Member ID</label>
                                                        <asp:TextBox class="form-control" ID="txtDownLineFormNo" MaxLength="15" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtDownLineFormNo"
                                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                    </div>

                                                    <div class="col-md-2">
                                                        <label for="inputdefault">
                                                            Down Level</label>
                                                        <asp:TextBox class="form-control" ID="txtDeptlevel" MaxLength="4" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtDeptlevel"
                                                            runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                    </div>

                                                    <div class="col-md-2">
                                                        <label for="inputdefault">
                                                            Tree Name</label>
                                                        <asp:DropDownList ID="ddlTree" runat="server" CssClass="form-control"></asp:DropDownList>

                                                    </div>

                                                    <div class="col-md-2" style="padding-top: 5px !important;">
                                                        <br />
                                                        <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="BtnSearch_Click" />
                                                        <asp:Button ID="cmdBack" runat="server" Text="Back" class="btn btn-primary" OnClick="cmdBack_Click" />
                                                    </div>



                                                </div>
                                                <div class="col-sm-12 pull-none" style="position: inherit">
                                                    <div class="form-group ">
                                                        <center>
                                                          <%-- <iframe name="TreeFrame" frameborder="0" scrolling="auto" width="100%" height="1000"
                                                                id="TreeFrame" runat="server"></iframe>--%>
                                                        </center>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
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

