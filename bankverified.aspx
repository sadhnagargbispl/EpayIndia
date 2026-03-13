<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="bankverified.aspx.cs" Inherits="bankverified" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <style>
        /* Define a class for adding space between buttons */
        .button-space {
            margin-right: 10px; /* Adjust this value to set the desired space */
        }
        /* Define media query for mobile devices */
        @media only screen and (max-width: 500px) {
            /* Adjust button style for mobile view */
            .btn {
                margin-bottom: 5px; /* Add some space between buttons vertically */
                width: 40%; /* Make buttons full-width on mobile devices */
            }

            .button-space {
                margin-right: 0; /* Remove right margin for buttons on mobile devices */
            }
        }
    </style>
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">KYC Master</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-2">
                                        <label for="exampleInputEmail1">Member ID</label>
                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2" style="display: none">
                                        <label for="FromDate">Status </label>
                                        <asp:DropDownList ID="RbtSearch" runat="server" class="form-control">
                                            <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <label for="FromDate">Verify Status </label>
                                        <asp:DropDownList ID="DDlVerify" runat="server" class="form-control">
                                            <asp:ListItem Text="All" Value="S" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="VERIFY" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="VERIFICATION DUE" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="REJECTED" Value="R"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class=" col-md-12">
                                        <br />
                                        <asp:Button runat="server" ID="BtnSearch" class="btn btn-primary" Text="Search" OnClick="BtnSearch_Click" />
                                        &nbsp; &nbsp; 
                                        <asp:Button runat="server" ID="BtnExport" class="btn btn-primary" Text="Export To Excel"
                                            Enabled="false" OnClick="BtnExport_Click" />
                                        &nbsp; &nbsp; 
                                     <asp:Button ID="BtnVerifiy" runat="server" Text="Verification" class="btn btn-primary"
                                         Enabled="false" OnClick="BtnVerifiy_Click" />
                                        &nbsp; &nbsp; 
                                        <asp:Button ID="BTnUnVerification" runat="server" Text="Reject" class="btn btn-primary" OnClick="BTnUnVerification_Click" />
                                        &nbsp; &nbsp; 
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div id="DivRemark" runat="server" visible="false">
                                        <table id="TblRemark" runat="server" align="center">
                                            <tr>
                                                <td align="left">
                                                    <br />
                                                    <strong>Reason</strong>*
                                                </td>
                                                <td align="left">
                                                    <br />
                                                    <asp:DropDownList ID="DDlREason" runat="server" Style="width: 200px;">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <br />
                                                    <strong>Remark</strong>*
                                                </td>
                                                <td align="left">
                                                    <br />
                                                    <asp:TextBox ID="TxtARemark" runat="server" TextMode="MultiLine" Style="width: 200px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <br />
                                                    <asp:Button ID="BtnUnVerify" runat="server" class="btn btn-primary" Text="Reject"
                                                        OnClientClick="return confirmation();" OnClick="BtnUnVerify_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <br />
                                    <div id="DivSideA" runat="server">
                                        <div class="table-responsive">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                                                <ContentTemplate>
                                                    <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                                        GridLines="None" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                                                        PageSize="20" EmptyDataText="No data to display.">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="CheckAll">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="IDNo" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblGrpID" runat="server" Text='<%# Eval("FormNo") %>'></asp:Label>
                                                                    <asp:Label ID="LblIdno" runat="server" Text='<%# Eval("IdNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex +1 %>.
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="IDNo" HeaderText="ID No." />
                                                            <asp:BoundField DataField="MemName" HeaderText="Member Name" />
                                                            <asp:BoundField DataField="Doj" HeaderText="Date Of Joining" />
                                                            <asp:BoundField DataField="Bankname" HeaderText="Bank Name" />
                                                            <asp:BoundField DataField="Acno" HeaderText="Account No" />
                                                            <asp:BoundField DataField="Branchname" HeaderText="Branch Name" />
                                                            <asp:BoundField DataField="Ifscode" HeaderText="IFSC Code" />
                                                            <asp:BoundField DataField="Panno" HeaderText="Pan No" />
                                                            <asp:BoundField DataField="ActivationDate" HeaderText=" Date Of Activation" />
                                                            <asp:TemplateField HeaderText="Bank Proof" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <a href='<%# "Img.aspx?ID=" + Eval("FormNo") + "&Type=BankProof" %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                                        <asp:Image ID="Image3" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("BankProofStatus")  %>' />
                                                                    </a>
                                                                    <br />
                                                                    <asp:Label ID="Lbldate" runat="server" Text='<%#Eval("BankProofDate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Bank Proof Verify Date" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <strong>Verify Status: </strong>
                                                                    <asp:Label ID="LblIdVerify" runat="server" Text='<%#Eval("BankVerf") %>'></asp:Label>
                                                                    <br />
                                                                    <strong>Verify Date:</strong>
                                                                    <asp:Label ID="LblVerifyDate" runat="server" Text='<%# Eval("BankVerifyDate") %>'></asp:Label>

                                                                    <br />
                                                                    <strong>Processed By:</strong>
                                                                    <asp:Label ID="LblProcessby" runat="server" Text='<%# Eval("VerifyBy") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Bank Reject">
                                                                <ItemTemplate>
                                                                    <strong>Reject Remark:</strong>
                                                                    <asp:Label ID="LblRejectRemark" runat="server" Text='<%#Eval("RejectRemark") %>'></asp:Label>
                                                                    <br />
                                                                    <strong>Reject Reason:</strong>
                                                                    <asp:Label ID="LblRejectReason" runat="server" Text='<%#Eval("RejectReason") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Pan Proof" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <a href='<%# "Img.aspx?ID=" + Eval("FormNo") + "&Type=Pancard" %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                                        <asp:Image ID="Image4" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("PanProofStatus")  %>' />
                                                                    </a>
                                                                    <br />
                                                                    <asp:Label ID="Lblpandate" runat="server" Text='<%#Eval("PanProofDate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Pan Proof Verify Date" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <strong>Verify Status: </strong>
                                                                    <asp:Label ID="LblpanVerify" runat="server" Text='<%#Eval("PanVerf") %>'></asp:Label>
                                                                    <br />
                                                                    <strong>Verify Date:</strong>
                                                                    <asp:Label ID="LblpanVerifyDate" runat="server" Text='<%# Eval("PanVerifyDate") %>'></asp:Label>
                                                                    <%--  Reject Remark:
                                                <asp:Label ID="LblRejectRemark" runat="server" Text='<%#Eval("RejectRemark") %>'></asp:Label>
                                                                    --%>
                                                                    <br />
                                                                    <strong>Processed By:</strong>
                                                                    <asp:Label ID="LblpanProcessby" runat="server" Text='<%# Eval("VerifyBy") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Pan Reject">
                                                                <ItemTemplate>
                                                                    <strong>Reject Remark:</strong>
                                                                    <asp:Label ID="LblpanRejectRemark" runat="server" Text='<%#Eval("PanRejectRemark") %>'></asp:Label>
                                                                    <br />
                                                                    <strong>Reject Reason:</strong>
                                                                    <asp:Label ID="LblpanRejectReason" runat="server" Text='<%#Eval("RejectReason") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="PagerStyle " />
                                                        <PagerSettings Mode="NumericFirstLast" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
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
