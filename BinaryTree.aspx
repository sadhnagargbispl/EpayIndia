<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BinaryTree.aspx.cs" Inherits="BinaryTree" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  <div class="content-wrapper">
      <!-- Main content -->
      <section class="content">
          <div class="container-fluid">
              <div class="row">
                  <div class="col-12">
                      <div class="card card-primary">
                          <div class="card-header">
                              <h3 class="card-title">Group Tree</h3>
                          </div>
                          <div class="card-body">
                              <div class="row">
                                  <div class="col-md-2">
                                      <label for="exampleInputEmail1">Member ID</label>
                                      <asp:TextBox ID="txtDownLineFormNo" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtDownLineFormNo"
                                        runat="server" ValidationGroup="Save">*
                                    </asp:RequiredFieldValidator>
                                  </div>
                                  <div class="col-md-2">
                                      <label for="exampleInputEmail1">Down Level</label>
                                      <asp:TextBox ID="txtDeptlevel" runat="server" class="form-control" Style="display: inline"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="txtDeptlevel"
                                        runat="server" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                  </div>


                                  <div class="col-md-3" style="padding-top:8px;">
                                      <br />
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-primary" CausesValidation="true"
                                        ValidationGroup="Save" OnClick="btnSearch_Click" />
                                   <asp:Button ID="cmdBack" runat="server" Text="Back" class="btn btn-primary" CausesValidation="false" OnClick="cmdBack_Click"  Visible="false"/>
                                    <asp:Button ID="BtnStepAbove" runat="server" Text="1 Step Above" class="btn btn-primary"
                                        CausesValidation="false" Onclick="BtnStepAbove_Click" Visible="false" />
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

