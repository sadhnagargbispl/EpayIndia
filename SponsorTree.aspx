<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SponsorTree.aspx.cs" Inherits="SponsorTree" %>

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
                                <h3 class="card-title">Sponsor Tree</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        <label for="exampleInputEmail1">Member ID</label>
                                        <asp:TextBox ID="DownLineFormNo" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <label for="exampleInputEmail1">Down Level</label>
                                        <asp:TextBox ID="deptlevel" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                    </div>


                                    <div class="col-md-3" style="padding-top:8px;">
                                        <br />
                                        <asp:Button ID="Button1" runat="server" Text="Search" TabIndex="3" ValidationGroup="Save"
                                            class="btn btn-primary" OnClick="Button1_Click" />
                                        <asp:Button ID="cmdBack" runat="server" class="btn btn-primary"
                                            Text="Back" OnClick="cmdBack_Click" />
                                    </div>

                                    <div class="col-md-12">
                                       <%-- <iframe name="TreeFrame" frameborder="0" scrolling="auto" src="Referaltree.aspx" width="100%" height="800"
                                            id="TreeFrame" runat="server"></iframe>--%>
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

