<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserPermission.aspx.cs" Inherits="UserPermission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="card card-primary">
                            <div class="card-header">
                                <h3 class="card-title">Set User Permission</h3>
                            </div>
                               <div class="card-body">
                               <div class="col-md-6">
                                <asp:Label ID="lblText" runat="server" Text="Select Group : " Font-Bold="True"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlGroup" runat="server" class="form-control" AutoPostBack="false" style="display :inline; width :55%"> 
                                </asp:DropDownList>
                                     <asp:Button ID="btnShow" runat="server" class="btn btn-primary" Text="Show" OnClick="btnShow_Click"/> 
                                  <asp:Button ID="btnSave" runat="server" class="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                         </div>
                         
                                       <div align="center">
                                <asp:Label ID="lblMsg" runat="server" Font-Size="13px" Visible="False"></asp:Label>
                            </div>
                                
                           <div style="margin-bottom: 20px; overflow: scroll;">
                               <br />
                                       <asp:GridView ID="GvData" runat="server" AutoGenerateColumns="False" RowStyle-Height="25px"
                                    GridLines="Both"  class="table table-bordered" HeaderStyle-CssClass="bg-primary"
                                    ShowHeader="true"  EmptyDataText="No data to display.">
                                    <Columns>
                                        <asp:TemplateField HeaderText="MenuId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMenuId" runat="server" Text='<%# Eval("MenuId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="70px">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkMenuPermission" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="MenuName" HeaderText="Menu" />
                                        <asp:TemplateField HeaderText="ParentId" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParentId" runat="server" Text='<%# Eval("ParentId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" />
                                    <PagerStyle CssClass ="PagerStyle" />
                                </asp:GridView>
                           						                    </div>
                </div>
           </div>
            </div>
                    </div>
                </div>
    </section>
</div>
</asp:Content>

