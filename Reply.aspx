<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reply.aspx.cs" Inherits="Reply" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <title></title>
    <link href="../Resources/CSS/Main.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <!-- left column -->
                    <div class="col-md-12">
                        <div class="card card-primary">
                            <%--  <div class="card-header">
                             <h3 class="card-title">Reply Master</h3>
                         </div>--%>
                            <div class="card-body">
                                <div class="col-md-12" style="padding-top: 1%; display: none;">
                                    <div class="col-md-4">
                                        Reply :
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label CssClass="TxtBox" ID="Label1" Width="350px" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        To :
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label CssClass="TxtBox" ID="LblMemName" Width="350px" runat="server"></asp:Label>
                                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        Complaint type :
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Label CssClass="TxtComplaint" ID="LblCType" Width="278px" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        Complaint:
                                    </div>
                                    <div class="col-md-2">

                                        <asp:TextBox CssClass="TxtBox" ID="TxtComplaint" ReadOnly="true" TextMode="MultiLine"
                                            Height="50px" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        Reply :
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox CssClass="TxtReply" ID="TxtReply" MaxLength="5000" TextMode="MultiLine"
                                            Height="70px" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Style="vertical-align: top"
                                            ControlToValidate="TxtReply" ValidationGroup="Send" ErrorMessage="*"></asp:RequiredFieldValidator>

                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <asp:Button ID="BtnSave" CssClass="btn btn-primary" runat="server" Text="Save"
                                        ValidationGroup="Save" OnClick="BtnSave_Click" />
                                    <asp:TextBox ID="TextBox3" runat="server" Visible="false"></asp:TextBox><br />

                                </div>
                                <div class="col-md-12">
                                    <div class="col-md-4">
                                        Previous Reply:
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox CssClass="TxtReply" ID="TxtPreReply" MaxLength="5000" TextMode="MultiLine"
                                            Height="70px" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Style="vertical-align: top"
                                            ControlToValidate="TxtReply" ValidationGroup="Send" ErrorMessage="*"></asp:RequiredFieldValidator>

                                    </div>

                                </div>



                            </div>



                        </div>
                    </div>
                </div>
        </section>
    </div>


</asp:Content>

