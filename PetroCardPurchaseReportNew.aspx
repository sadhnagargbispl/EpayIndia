<%@ Page Title="" Language="C#" MasterPageFile="~/SiteMaster.master" AutoEventWireup="true" CodeFile="PetroCardPurchaseReportNew.aspx.cs" Inherits="PetroCardPurchaseReportNew" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <link rel="stylesheet" href="plugins/datatables/dataTables.bootstrap.css">
 <div class="spacedivider3">
 </div>
 <div class="col-md-12">
     <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
         <div class="profile-bar-simple red-border clearfix">
             <h6>Petro Card Purchase Detail
             </h6>
         </div>
         <div class="clearfix gen-profile-box" style="min-height: auto;">
             <div class="profile-bar clearfix" style="background: #000;">
                 <div class="clearfix">
                     <div class="clearfix">
                         <br>
                         <div class="centered">
                             <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
                         </div>
                         <div class="clr">
                         </div>
                         <div class="col-md-12">
                             <div class="col-md-12">
                                 <div class="table-responsive">
                                     <div class="table mb-0" cellspacing="0" cellpadding="4" rules="all" border="1" id="ctl00_ContentPlaceHolder2_DGVReferral"
                                         style="width: 100%; border-collapse: collapse;">
                                         <asp:GridView ID="RptDirects" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered"
                                             HeaderStyle-BackColor="#c6c8ca" HeaderStyle-ForeColor="Red" AllowPaging="true"
                                             PageSize="10" OnPageIndexChanging="RptDirects_PageIndexChanging">
                                             <Columns>
                                             </Columns>
                                         </asp:GridView>
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
 <%-- </div> </div> </div> </div> </div> --%>
</asp:Content>