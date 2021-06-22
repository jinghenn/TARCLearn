﻿<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="Discussion.aspx.cs" Inherits="TARCLearn.App_Pages.Discussionaspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style>
       .button1 {
            background-color: white;
            text-align: left;
            border-radius: 0px;
            transition-duration: 0.4s;
            width:calc(100% - 15px);  
            height:70px; 
            border-width:0px; 
            padding-left:20px;  
            margin-left:15px;
        }

            .button1:hover {
                background-color: #D6E4F1; /* light blue */
                color: #0275d8;
            }
        .label1{
            width: 100%;            
            height: 70px; 
            padding:25px 0 0 30px;
        }

        .rightButton{
              float: right;
              margin-right:30px;
             
              
        }
      
      
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="modal fade bd-example-modal-lg" id="modalForm">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Add New Discussion</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        
                       <div class="row mb-3">
                            <label for="formDisTitle" class="col-sm-3 col-form-label">Discussion Title</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formDisTitle" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvDisTitle" ControlToValidate="formDisTitle" runat="server" Display="Dynamic" ErrorMessage="Discussion Title Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>   

                        <div class="row mb-3">
                            <label for="formDisDesc" class="col-sm-3 col-form-label">Discussion Description</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formDisDesc" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>  

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="addDiscussionFormSubmitClicked" ValidationGroup="Add Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>

    <div class="main main-raised" >
        <div class="label1">
            <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">DiscussionThread</asp:Label> 
            <asp:ImageButton ID="btnEdit" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/more_icon.png" OnClick="btnEdit_Click" /> 
            <asp:ImageButton ID="btnAdd" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/add_icon.png"  data-toggle="modal" data-target="#modalForm" OnClientClick="return false;" />        
        </div>
    
        <asp:Repeater ID="disRepeater" runat="server" OnItemCommand="courseRepeater_ItemCommand" >
            <ItemTemplate>
                <asp:Button CommandName="selectDiscussion" CommandArgument='<%# Eval("threadId")%>' 
                    Text= '<%# Eval("threadTitle")%>' 
                    runat="server" ID="btnCourse" CssClass="button1"  />
            
            </ItemTemplate>
        </asp:Repeater>

        <asp:Repeater ID="rptEditDiscussion" runat="server"  Visible="False" OnItemCommand="rptEditDiscussion_ItemCommand">
            <ItemTemplate>
                <div style="padding:12.5px 0 12.5px 30px; height: 70px;">
                    <div>

                        <asp:TextBox ID="txtDiscussionTitle" runat="server" style="width: 700px;" Text='<%# Eval("threadTitle") %>' Enabled="false" BorderStyle="None" BackColor="Transparent"  ></asp:TextBox>
                    
                        <div class=" rightButton " style="padding-bottom:20px;" role="group">
                            <asp:LinkButton ID="btnEdit" CommandName="edit"  CssClass="btn btn-outline-info" runat="server" CausesValidation="false">Edit</asp:LinkButton>
                            <asp:LinkButton ID="btnSave" CommandName="save" CommandArgument='<%# Eval("threadId")%>' CssClass="btn btn-outline-success " runat="server"  Visible="False" ValidationGroup="Edit">Save</asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" CommandName="cancel"  CssClass="btn btn-outline-danger " runat="server"  Visible="False" CausesValidation="false">Cancel</asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" CommandName="delete" CommandArgument='<%# Eval("threadId")%>' CssClass="btn btn-danger" runat="server"  Visible="False" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'>Delete</asp:LinkButton>
                        </div> 
                    </div>
                    
                    <div>
                        <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtDiscussionTitle" runat="server" ErrorMessage=" - Course Title Cannot Be Blank" ControlToValidate="txtDiscussionTitle" ></asp:RequiredFieldValidator>
                    </div>
                </div>
            
            </ItemTemplate>
        </asp:Repeater>
      
    </div>
</asp:Content>