<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FundTransferNew.aspx.cs" Inherits="FundTransferNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <script type="text/javascript" language="javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrapper">
        <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">
                            Fund Transfer</h3>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="panel-body">
                        <div align="center">
                            <span id="lblt" class="text-danger"></span>
                        </div>
                        <table cellspacing="10px" cellpadding="0%">
                            <tbody>
                                <tr>
                                    <td colspan="4" style="width: 100%">
                                        <%--    <span id="lblStock" runat="server" style="color: #000; font-weight:bold;font-size:14px;"></span> --%>
                                        <%-- <asp:Label runat="server" Style="margin-bottom: 11px" ID="lblMessage"
                        Text="Transfer Amount" Font-Bold="true" Font-Size="15px"></asp:Label>--%>
                                        <br />
                                    </td>
                                </tr>
                                <%--<p  style="color: #666666; line-height: 25px;" >    
                                 
                </p>
                 <p style="color: #666666; line-height: 25px;">--%>
                                <tr style="margin-top: 20px; padding-top: 20px">
                                    <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                        <strong>Member ID :</strong>
                                         <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    </td>
                                    <td>
                                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>--%>
                                                <br />
                                                <asp:Label ID="LblAmount" runat="server" ForeColor="Red"></asp:Label>
                                                <asp:Label ID="LblMobl" runat="server" Visible="false"></asp:Label>
                                                <asp:TextBox ID="TxtIDNo" runat="server" class="form-control" AutoPostBack="true"
                                                    Width="200px" OnTextChanged="TxtIDNo_TextChanged"></asp:TextBox>
                                                <asp:Label ID="LblMemName" runat="server" CssClass="label-text"></asp:Label>
                                                <asp:TextBox ID="TxtFormNo" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                           <%-- </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="TxtIDNo" EventName="TextChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Enter Member ID."
                                            ControlToValidate="TxtIDNo" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr style="margin-top: 20px; padding-top: 20px">
                                    <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                        <strong>Select Type:</strong>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="RbtAccount" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow">
                                            <asp:ListItem Text="Credit" Value="C" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Debit" Value="D"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr style="margin-top: 20px; padding-top: 20px;">
                                    <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                        <strong>Wallet Type:</strong>
                                    </td>
                                    <td>
                                       <asp:DropDownList  ID="Rbtnwallet" runat="server" AutoPostBack="true" CssClass="form-control" style="width :200px" OnSelectedIndexChanged="Rbtnwallet_SelectedIndexChanged">
                                           
                                  
                                      </asp:DropDownList> 
                                        <br /> 
                                    </td>
                                </tr>
                              
                                <tr style="margin-top: 20px; padding-top: 20px">
                                    <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                        <strong>Fund Amount:</strong>
                                    </td>
                                    <td>
                                        <%-- <br />--%>
                                        <asp:TextBox ID="TxtFund" Width="200px" runat="server" class="form-control" onkeypress="return isNumberKey(event);"
                                            AutoPostBack="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Enter Fund Transfer Amount."
                                            ControlToValidate="TxtFund" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr style="margin-top: 20px; padding-top: 20px">
                                    <td width="20%" align="left" valign="middle" style="padding-left: 20px;">
                                        <strong>Remarks</strong>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtRemarks" Width="200px" runat="server" class="form-control"></asp:TextBox>
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="* Enter Remrks"
                                            ControlToValidate="TxtRemarks" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                    </td>
                                </tr>
                                <tr style="margin-top: 20px; padding-top: 20px">
                                    <td width="14%" align="left" valign="middle" style="padding-left: 20px;">
                                    </td>
                                    <td>
                                       <%-- <asp:Button ID="BtnFundTransfer" runat="server" Text="Fund Transfer" OnClientClick="return confirmation();"
                                            class="btn btn-primary" ValidationGroup="Save" />--%>
                                             <asp:Button ID="BtnFundTransfer" runat="server" Text="Fund Transfer" OnClientClick ="this.disabled=true; this.value='Sending…';" UseSubmitBehavior="false"
                                            class="btn btn-primary" ValidationGroup="Save" OnClick="BtnFundTransfer_Click" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14px" ForeColor="Maroon"></asp:Label>
                                        <br />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <!-- end of weather widget -->
    </div>
    </div> 
</asp:Content>

