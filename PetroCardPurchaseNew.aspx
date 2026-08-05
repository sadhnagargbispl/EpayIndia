<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="PetroCardPurchaseNew.aspx.cs" Inherits="PetroCardPurchaseNew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-md-12">
        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
            <div class="profile-bar-simple red-border clearfix">
                <h6>Petro Card Purchase</h6>
            </div>
            <div class="clearfix gen-profile-box" style="min-height: auto;">
                <div class="profile-bar clearfix" style="background: #fff;">
                    <div class="centered">
                        <div class="clr">
                            <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
                        </div>
                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        <asp:HiddenField ID="HdnWalletBalance" runat="server" />
                        <asp:Repeater ID="rptKitDetails" runat="server">
                            <ItemTemplate>
                                <div class="row profile-bar p-2 align-items-center">

                                    <div class="col-lg-3 col-sm-4 col-12 order-lg-2 order-sm-2 order-1">
                                        <div class="clearfix rounded-3 mb-1">
                                            <img src='<%#Eval("img")%>' alt="voucher image" class=" img-fluid img-responsive rounded-3">
                                        </div>
                                    </div>

                                    <div class="col-lg-9 col-sm-8 col-12 order-lg-1 order-sm-1 order-2">
                                        <div class="clearfix rounded-3 mb-1">
                                            <h4 style="color: red;"><%#Eval("kitname")%></h4>
                                            <h3 style="color: green;"><strong><%#Eval("kitamount")%> </strong></h3>
                                            <hr>
                                            <asp:Label ID="LblKitid" runat="server" Text='<%#Eval("kitid")%>' Visible="false"></asp:Label>
                                            <p style="margin-top: 10px;"><b>You can enjoy significant benefits on your Petro Card </b></p>
                                            <ul>
                                                <%#Eval("Benf")%>
                                            </ul>
                                            <hr>

                                            <p class="mt-3"><%#Eval("BB")%></p>

                                            <%-- <p class="mt-3">
                                                <b>Available Balance : <span class="red" id="AvailableBal" style="color: Red" runat="server"><%#Eval("Balance")%></span></b>
                                            </p>--%>
                                            <div class="form-group d-flex">
                                                <%-- <input type="text" id="TxtAmount" class="form-control"
                                                    placeholder="Enter Amount" style="width: 80%; margin-right: 10px;"
                                                    readonly="readonly" value='<%#Eval("kitamount")%>' runat="server">
                                                --%>
                                                <asp:TextBox ID="TxtAmount" CssClass="form-control"
                                                    Placeholder="Enter Amount" Style="width: 80%; margin-right: 10px;" ReadOnly="true"
                                                    runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "kitamount") %>'></asp:TextBox>

                                                <a name="" id="" class="btn btn-primary border-0 rounded-3 btn-sm fs-5 shadow-0"
                                                    href='<%# string.Format("PetroCardFinalPurchaseNew.aspx?KitID={0}",HttpUtility.UrlEncode(Crypto.Encrypt(Eval("kitid").ToString()))) %>'
                                                    role="button" onclientclick="return DisableTheButton(this);">Buy Now</a>
                                                <%--<a href='<%# string.Format("PetroCardFinalPurchase.aspx?KitID={0}",Eval("kitid").ToString()) %>'>--%>
                                                <%--<asp:Button ID="BtnProceedToPay" runat="server" Text="Buy Now" class="btn btn-primary border-0 rounded-3 btn-sm fs-5 shadow-0"
     ValidationGroup="Validation" OnClientClick="return DisableTheButton(this);" OnClick="BtnProceedToPay_Click" />--%>
                                                </a>
                                               
                                                <%--<a name="" id="" class="btn btn-primary border-0 rounded-3 btn-sm fs-5 shadow-0" href="#" role="button">Proceed To Pay</a>--%>
                                            </div>
                                        </div>

                                        <br>
                                    </div>

                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <hr>










                        <div class="clr">
                        </div>
                        <div class="form-horizontal">
                            <div class="col-md-12">
                                <div class="table-responsive">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="assets/jquery.min.js"></script>

    <%--    <script type="text/javascript" src="assets/datatable.css"></script>--%>

    <script src="plugins/datatables/jquery.dataTables.min.js"></script>

    <script src="plugins/datatables/dataTables.bootstrap.min.js"></script>

    <%--  <script type="text/javascript" src="js/plugins/datatables/jquery.dataTables.min.js"></script>--%>

    <script type="text/javascript" src="assets/tableExport.js"></script>

    <%--  <script type="text/javascript">
    var jq = $.noConflict();
    function pageLoad(sender, args) {
       

        jq(document).ready(function() {
            jq('#customers2').DataTable();

        });
    }


</script>
    --%>
    <script type="text/javascript">
        function DisableTheButton(btn) {
            // Ensure validation works before proceeding
            if (typeof (Page_ClientValidate) === 'function' && !Page_ClientValidate()) {
                return false;
            }

            // Show confirmation before proceeding
            if (!confirm('Are you sure to proceed?')) {
                return false;
            }

            // Disable the button and change text
            btn.value = 'Processing...';
            btn.disabled = true;

            // Allow postback to continue
            setTimeout(function () {
                __doPostBack(btn.name, '');
            }, 0);

            return false; // Prevents duplicate clicks but allows postback
        }
    </script>


</asp:Content>

