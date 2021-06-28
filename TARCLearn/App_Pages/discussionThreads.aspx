<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="discussionThreads.aspx.cs" Inherits="TARCLearn.App_Pages.discussionThreads" %>
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
        .label2{
            width: 100%;            

            padding:25px 0 0 30px;


        }

        .rightButton{
              float: right;
              margin-right:30px;
             
              
        }
        .chatMessageControls {
    
            margin-top: 6px;
            padding: 10px;
            width: 100%;
            background-color: #fff;
            border-top: 1px solid #e1e1e1;
            border-left: 1px solid #d4d4d4;
            border-right: 1px solid #d4d4d4;
            border-bottom: 1px solid #c3c3c3;
            border-bottom-left-radius: 6px;
            border-bottom-right-radius: 6px;
    
        }
        

        #clearMessageButton {
    
            border-radius: 0;
            border-left: none;
            border-right: none;
    
        }

        #sendMessageButton {
    
            border-top-left-radius: 0;
            border-bottom-left-radius: 0;
    
        }
        
        </style>
    <script type="text/javascript">

        function openModal() {
            $('#EditForm').modal('show');
        }

      


        
        
        
    </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="modal fade bd-example-modal-lg" id="EditForm">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Edit Discussion</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        
                       <div class="row mb-3">
                            <label for="formDisTitle" class="col-sm-3 col-form-label">Discussion Title</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formDisTitle" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Edit Form" ForeColor="Red" ID="rfvDisTitle" ControlToValidate="formDisTitle" runat="server" Display="Dynamic" ErrorMessage="Discussion Title Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>   

                        <div class="row mb-3">
                            <label for="formDisDesc" class="col-sm-3 col-form-label">Discussion Description</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formDisDesc" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Edit Form" ForeColor="Red" ID="rfvformDisDesc" ControlToValidate="formDisDesc" runat="server" Display="Dynamic" ErrorMessage="Discussion Description Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>  

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="editDiscussionFormSubmitClicked" ValidationGroup="Edit Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>

    <div class="main main-raised" >
        <div class="label2"> 
            <div>
                <asp:ImageButton ID="btnDeleteDT" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png" OnClientClick='return confirm("Are you sure you want to delete this item?");' OnClick="btnDeleteDT_Click"/> 
                <asp:ImageButton ID="btnEditDT" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png" OnClick="btnEditDT_Click" />      
                <asp:Label ID="txtTitle" runat="server"  Font-Bold="True" Font-Size="XX-Large"  ></asp:Label>
           </div>
                <asp:TextBox ID="txtDesc" runat="server"  Width="100%" Enabled="false" BorderStyle="None" BackColor="Transparent" TextMode="MultiLine" Rows="5" ></asp:TextBox>
        </div>
        <div style="padding-left:15px;">
            <asp:Label ID="lblComment"  runat="server"  Text="Comments" Font-Bold="True" Font-Size="X-Large"></asp:Label>
            <asp:Label ID="lblCount"  runat="server"   Font-Size="Large" CssClass="rightButton"></asp:Label>
        </div>
        <div>
            <asp:Repeater runat="server" ID="rptComment"  OnItemDataBound="rptComment_ItemDataBound" OnItemCommand="rptComment_ItemCommand" >
                <ItemTemplate>
                    <div style="margin:12.5px 0 12.5px 30px;">                    
                   
                        <div >
                            <asp:Label ID="lblUserType" style=" justify-content:left center;" runat="server" Font-Bold="True" Text='<%# Eval("username") %>'></asp:Label>    
                            <asp:ImageButton ID="btnDeleteC" CssClass="rightButton" CommandName="delete" CommandArgument='<%# Eval("messageId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png" Visible="True" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
                            <asp:ImageButton ID="btnEditC" CssClass="rightButton" CommandName="edit" CommandArgument='<%# Eval("messageId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png" CausesValidation="false"/>                
                            <asp:ImageButton ID="btnCancelC" CssClass="rightButton" CommandName="cancel" Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete2_icon.png" Visible="False" CausesValidation="false"/>                
                            <asp:ImageButton ID="btnSaveC" CssClass="rightButton" CommandName="save" CommandArgument='<%# Eval("messageId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/save_icon.png"  Visible="False" ValidationGroup="Edit"/>                


                        </div>
                    
                                  
                        <asp:TextBox ID="txtDiscussionComment" runat="server" Width="89%" Text='<%# Eval("message") %>' Enabled="false" BorderStyle="None" BackColor="Transparent" TextMode="MultiLine" ></asp:TextBox>                                                    
                        <div>      
                            <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtDiscussionComment" runat="server" ErrorMessage=" Comment Cannot Be Blank" ControlToValidate="txtDiscussionComment" ></asp:RequiredFieldValidator>
                        </div>  
                    
                     </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
         <div class="input-group input-group-sm chatMessageControls ">
            <asp:TextBox ID="txtComment" runat="server"  Width="89%" placeholder="Type your comment here.." aria-describedby="sizing-addon3" TextMode="MultiLine"></asp:TextBox>

          
            <span class="input-group-btn">
                <asp:Button ID="clearMessageButton" runat="server" Text="Clear" class="btn btn-default" OnClick="clearMessageButton_Click"/>
                <asp:Button ID="sendMessageButton" runat="server" Text="Send" class="btn btn-primary" OnClick="sendMessageButton_Click" ValidationGroup="Comment" />
            
            </span>
             <div>
                 <asp:RequiredFieldValidator ValidationGroup="Comment" ForeColor="Red" Display="Dynamic" ID="rfvtxtComment" runat="server" ErrorMessage=" Comment Cannot Be Blank" ControlToValidate="txtComment" ></asp:RequiredFieldValidator>
             </div>
       
        </div>
    </div>
</asp:Content>
