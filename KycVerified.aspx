<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="KycVerified.aspx.cs" Inherits="KycVerified" %>

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
                                <h3 class="card-title">Kyc Verify</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-4">
                                        <strong>Member ID Wise : </strong>

                                        <asp:TextBox ID="txtMemId" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <strong>Status: </strong>
                                        <asp:DropDownList ID="RbtSearch" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" class="form-control">
                                            <asp:ListItem Text="All" Value="A" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <strong>Verify Status:</strong>
                                        <asp:DropDownList ID="DDlVerify" runat="server" class="form-control">
                                            <asp:ListItem Text="VERIFICATION DUE" Value="N" Selected="True"></asp:ListItem>

                                            <asp:ListItem Text="VERIFY" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="REJECTED" Value="R"></asp:ListItem>
                                            <asp:ListItem Text="All" Value="S"></asp:ListItem>

                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-md-1">
                                        <asp:Button runat="server" ID="BtnSearch" class="btn btn-primary" Text="Search" OnClick="BtnSearch_Click" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button runat="server" ID="BtnExport" class="btn btn-primary" Text="Export To Excel"
                                            Enabled="false" OnClick="BtnExport_Click" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="BtnVerifiy" runat="server" Text="Verification" class="btn btn-primary"
                                            Enabled="false" OnClick="BtnVerifiy_Click" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:Button ID="BTnUnVerification" runat="server" Text="Reject" class="btn btn-primary" OnClick="BTnUnVerification_Click" />
                                    </div>
                                </div>

                                <br />
                                <center>
                                    <div id="DivRemark" runat="server" visible="False">
                                        <table id="TblRemark" runat="server" align="center" style="background-color: #5056bc; color: #ffffff; border-color: Black; border-width: 1px; margin-top: -10px;">
                                            <tr>
                                                <td align="left">
                                                    <br />
                                                    <strong>Reason</strong>*
                                                </td>
                                                <td align="left">
                                                    <br />
                                                    <asp:DropDownList ID="DDlREason" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <strong>Remark</strong>*
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="TxtARemark" runat="server" TextMode="MultiLine" Style="color: Black" class="form-control"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <br />
                                                    <asp:Button ID="BtnUnVerify" runat="server" class="btn btn-primary" Text="Reject"
                                                        OnClick="BtnUnVerify_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                     <asp:Label ID="LblARemark" runat="server" ForeColor="red" Visible="False"></asp:Label>
                                </center>
                                <div class="row">













                                   
                                    <div class="col-md-12" style="overflow: scroll">
                                        <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                            GridLines="None" AllowPaging="true" class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" ShowHeader="true"
                                            PageSize="20" EmptyDataText="No data to display." OnPageIndexChanging="GvData_PageIndexChanging" OnRowDataBound="GvData_RowDataBound">
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
                                                <asp:BoundField DataField="ActivationDate" HeaderText=" Date Of Activation" />
                                                <asp:BoundField DataField="IdType" HeaderText="IdType" />
                                                <asp:BoundField DataField="IdProofNo" HeaderText="Address Proof No" />
                                                <asp:BoundField DataField="City" HeaderText="City" />
                                                <asp:BoundField DataField="District" HeaderText="District" />
                                                <asp:BoundField DataField="statename" HeaderText="State" />
                                                <asp:BoundField DataField="Address1" HeaderText="Address" />
                                                <asp:BoundField DataField="Pincode" HeaderText="Pincode" />
                                                <asp:BoundField DataField="Bankname" HeaderText="Bank Name" />
                                                <asp:BoundField DataField="Acno" HeaderText="Account No" />
                                                <asp:BoundField DataField="Branchname" HeaderText="Branch Name" />
                                                <asp:BoundField DataField="Ifscode" HeaderText="IFSC Code" />
                                                <asp:BoundField DataField="Panno" HeaderText="Pan No." />

                                                <asp:TemplateField HeaderText="Front Address Proof" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <a href='<%# "Img.aspx?ID=" + Eval("FormNo") + "&Type=FrontAddress" %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                            <asp:Image ID="Image3" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("AddressproofStatus")  %>' />
                                                        </a>
                                                        <br />
                                                        <%--   Upload Date<br />
                                                    <asp:Label ID="Lbldate" runat="server" Text='<%#Eval("AddressProofDate") %>'></asp:Label>
                                                        --%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Back Address Proof" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <a href='<%# "Img.aspx?ID=" + Eval("FormNo") + "&Type=BackAddress" %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                            <asp:Image ID="Image4" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("BackAdressProof")  %>' />
                                                        </a>
                                                        <br />
                                                        <%--    Upload Date:<br />
                                                    <asp:Label ID="LblBackDate" runat="server" Text='<%#Eval("BackAddressDate") %>'></asp:Label>
                                                        --%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bank Proof" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <a href='<%# "Img.aspx?ID=" + Eval("FormNo") + "&Type=BankProof" %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                            <asp:Image ID="ImageBank" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("BankProofStatus")  %>' />
                                                        </a>
                                                        <br />
                                                        <%--       <asp:Label ID="LblBankdate" runat="server" Text='<%#Eval("BankProofDate") %>'></asp:Label>
                                                        --%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pancard" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%-- <asp:LinkButton ID="LBApprove" runat="server" Text="Show Image" OnClick="ApproveData" style="color:Black" data-toggle="modal" data-target="#modal_open"  href="javascript:void(0);"></asp:LinkButton>--%>
                                                        <a href='<%# "Img.aspx?ID=" + Eval("FormNo") + "&Type=Pancard" %>' onclick="return hs.htmlExpand(this, { objectType: 'iframe',width:500,height:500,marginTop : 50 } )">
                                                            <asp:Image ID="Imagepan" Width="50px" Height="50px" runat="server" ImageUrl='<%#  Eval("PanproofStatus")  %>' />
                                                        </a>
                                                        <br />
                                                        <asp:Label ID="Lbldate1" runat="server" Text='<%#Eval("PanProofDate") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Uploaded Address Proof Date" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <strong>Verify Detail:</strong>
                                                        <asp:Label ID="LblIdVerify" runat="server" Text='<%#Eval("AddrssVerf") %>'></asp:Label>
                                                        <br />
                                                        <strong>Verify Date</strong>
                                                        <asp:Label ID="LblVerifyDate" runat="server" Text='<%# Eval("AddressVerifyDate") %>'></asp:Label>
                                                        <strong>Processed By:</strong>
                                                        <asp:Label ID="LblProcessby" runat="server" Text='<%# Eval("VerifyBy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reject">
                                                    <ItemTemplate>
                                                        <strong>Reject Remark:</strong>
                                                        <asp:Label ID="LblRejectRemark" runat="server" Text='<%#Eval("RejectRemark") %>'></asp:Label>
                                                        <br />
                                                        <strong>Reject Reason:</strong>
                                                        <asp:Label ID="LblRejectReason" runat="server" Text='<%#Eval("RejectReason") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="PagerStyle " />
                                            <PagerSettings Mode="NumericFirstLast" />
                                        </asp:GridView>
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
