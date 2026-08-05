<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="PetroCardFinalPurchaseNew.aspx.cs" Inherits="PetroCardFinalPurchaseNew" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="assets/jquery.min.js">
    </script>

    <%--   <script type="text/javascript" src="js/plugins/jquery/jquery.min.js"></script>--%>

    <script type="text/javascript" src="assets/jquery.validationEngine-en.js"></script>

    <script type="text/javascript" src="assets/jquery.validationEngine.js"></script>

    <link href="assets/validationEngine.jquery.min.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script> --%>


    <script type="text/javascript">
        var jq = $.noConflict();
        jq(document).ready(function () {
            jq(document).bind("contextmenu", function (e) {
                e.preventDefault();
            });
            jq(document).keydown(function (e) {

                if (e.which === 123) {
                    return false;
                }
                if (e.which === 116) {
                    return false;
                }
                if (e.ctrlKey && e.which === 82) {

                    return false;

                }


            });




        });
        function pageLoad(sender, args) {

            jq(document).ready(function () {

                jq("#aspnetForm").validationEngine('attach', { promptPosition: "topRight" });
            });

            jq(document).ready(function () {
                jq("#CmdSave1").click(function () {
                    // Your JavaScript code here
                });
            });


            var valid = jq("#aspnetForm").validationEngine('validate');
            var vars = jq("#aspnetForm").serialize();
            if (valid == true) {
                return true;

            } else {
                return false;
            }
        });
        }


    </script>

    <style type="text/css">
        .feedbackform {
            width: auto;
            height: auto;
            position: absolute;
            top: 100px;
            left: 40%;
            z-index: 9999;
            display: block;
            padding: 15px;
            background: #fff;
            border-radius: 5px;
            border: 1px solid;
        }

            .feedbackform img {
                max-width: 150px;
                display: block;
            }

        .feedbackpop {
            background: url(images/blackbg2.png) repeat;
            position: fixed;
            width: 100%;
            height: 100%;
            display: block;
            z-index: 9999;
        }

        #closeicon a {
            background: url(images/close2.png) no-repeat;
            width: 55px;
            height: 55px;
            display: block;
            margin: -22px -30px 0 0;
            float: right;
            position: absolute;
            right: 0px;
        }

            #closeicon a:hover {
                background: url(images/close2_hover.png) no-repeat;
            }

        #feedbackwrap {
            width: 1000px;
            margin: 0 auto;
            position: relative;
        }

        @media ( max-width: 1000px ) {
            #feedbackwrap {
                width: 100%;
            }

            .feedbackform {
                left: 2%;
                right: 2%;
            }
        }
    </style>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Petro Card Purchase
                </h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <div class="centered">
                        <div class="col-md-6">

                            <div class="form-group" id="DiMemberId" runat="server">
                                <label for="inputdefault">
                                    Member Id <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:TextBox ID="txtMemberId" runat="server" CssClass="form-control validate[required]"
                                    AutoPostBack="true" Enabled="false"></asp:TextBox>
                                <asp:Label ID="lblFormno" runat="server" Visible="false"></asp:Label>
                                <asp:HiddenField ID="hdnMacadrs" runat="server" />
                                <asp:HiddenField ID="HdnTopupSeq" runat="server" />
                                <asp:HiddenField ID="HdnMemberMacAdrs" runat="server" />
                                <asp:HiddenField ID="HdnMemberTopupseq" runat="server" />
                                <asp:HiddenField ID="MemberStatus" runat="server" />
                                <asp:HiddenField ID="hdnFormno" runat="server" />
                                <asp:HiddenField ID="hdnemail" runat="server" />
                                <asp:HiddenField ID="Hdnkitid" runat="server" />
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <div class="form-group" id="DivMemberName" runat="server">
                                        <label for="inputdefault">
                                            Member Name <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                        <asp:Label ID="LblMobile" runat="server" Visible="false"></asp:Label>
                                        <asp:TextBox ID="TxtMemberName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtMemberId" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <div class="form-group ">
                                <label>
                                    Gender <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:DropDownList ID="DDlGender" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="inputdefault">
                                    Card Amount <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:TextBox ID="txtAmount" runat="server" class="form-control" aria-describedby="emailHelp"
                                    placeholder="Enter Amount" AutoPostBack="true" ReadOnly="true" Text="0" onkeypress="return isNumberKey(event);"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label for="inputdefault">Email <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label for="inputdefault">Mobile No. <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:TextBox ID="Txtmonileno" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label for="inputdefault">Whatsapp No. <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:TextBox ID="TxtWhatsappNo" runat="server" CssClass="form-control" MaxLength="10" Text="0"></asp:TextBox>
                            </div>
                            <asp:HiddenField ID="HdnWalletBalance" runat="server" />
                            <div class="form-group ">
                                <label for="inputdefault">Pan No. <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:TextBox ID="Txtpanno" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label for="inputdefault">Date Of Birth <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:TextBox ID="TxtDOB" runat="server" CssClass="form-control"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtDOB"
                                    Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtDOB" ErrorMessage="Invalid Date"
                                    Font-Names="arial" Font-Size="10px" ForeColor="Red" SetFocusOnError="True"
                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"></asp:RegularExpressionValidator>
                            </div>
                            <div class="form-group ">
                                <label for="inputdefault">Shipping Address <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:TextBox ID="TxtAddress" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label for="inputdefault">PinCode <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:TextBox ID="Txtpincode" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group ">
                                <label>
                                    State <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group ">
                                <label for="inputdefault">City <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:TextBox ID="TxtCity" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group ">
                                <label class="control-label col-sm-2">
                                    District <span style="color: Red; font-weight: bold; font-size: 1.4em">*</span></label>
                                <asp:TextBox ID="TxtDistrict" CssClass="form-control" runat="server" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="HDistrictCode" runat="server" />
                            </div>

                            <div class="form-group">
                                <asp:Button ID="BtnSubmit" runat="server" Text="Proceed To Pay" class="btn btn-primary" ValidationGroup="Validation" OnClick="BtnSubmit_Click" />
                            </div>
                            <div class="form-group ">
                                <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
