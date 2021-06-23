<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="DiscussionThread.aspx.cs" Inherits="TARCLearn.App_Pages.DiscussionThread" %>
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

    <script>
        function showContent() {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "progressBar": true,
                "preventDuplicates": false,
                "positionClass": "toast-top-right",
                "showDuration": "400",
                "hideDuration": "1000",
                "timeOut": "7000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            toastr["fail"]("The comment is empty.");

        }
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="label2"> 
        <div class=" rightButton "  role="group">
           <asp:LinkButton ID="btnEdit" CssClass="btn btn-outline-info" runat="server" CausesValidation="false" OnClick="btnEdit_Click">Edit</asp:LinkButton>
           <asp:LinkButton ID="btnSave"  CssClass="btn btn-outline-success " runat="server"  Visible="False" ValidationGroup="Edit" OnClick="btnSave_Click">Save</asp:LinkButton>
           <asp:LinkButton ID="btnCancel"  CssClass="btn btn-outline-danger " runat="server"  Visible="False" CausesValidation="false" OnClick="btnCancel_Click">Cancel</asp:LinkButton>
           <asp:LinkButton ID="btnDel"  CssClass="btn btn-danger" runat="server"  Visible="False" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");' OnClick="btnDel_Click">Delete</asp:LinkButton>
        </div>
        <asp:TextBox ID="txtTitle" runat="server" Width="70%" Enabled="false" BorderStyle="None" BackColor="Transparent"  Font-Bold="True" Font-Size="XX-Large"  ></asp:TextBox>
        <asp:TextBox ID="txtDesc" runat="server"  Width="100%" Enabled="false" BorderStyle="None" BackColor="Transparent" TextMode="MultiLine" Rows="5" ></asp:TextBox>
        
    </div>
    <div style="padding-left:15px; width:100%;">
        <asp:Label ID="lblComment"  runat="server"  Text="Comments" Font-Bold="True" Font-Size="X-Large"></asp:Label>
        <asp:Label ID="lblCount"  runat="server"   Font-Size="Large" CssClass="rightButton"></asp:Label>
    </div>
    <div>
        <asp:Repeater runat="server" ID="rptComment" OnItemCommand="rptComment_ItemCommand" >
            <ItemTemplate>
                <div style="margin:12.5px 0 12.5px 30px;">
                    
                    <asp:Label ID="lblUserType" style="width: 100%; justify-content:left center;" runat="server" Font-Bold="True" Text='<%# Eval("username") %>'></asp:Label>    
                   
                    <div class=" rightButton "  role="group">
                        <asp:LinkButton ID="btnEdit" CommandName="edit" CommandArgument='<%# Eval("messageId")%>' CssClass="btn btn-outline-info" runat="server" CausesValidation="false">Edit</asp:LinkButton>
                        <asp:LinkButton ID="btnSave" CommandName="save" CommandArgument='<%# Eval("messageId")%>' CssClass="btn btn-outline-success " runat="server"  Visible="False" ValidationGroup="Edit">Save</asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" CommandName="cancel"  CssClass="btn btn-outline-danger " runat="server"  Visible="False" CausesValidation="false">Cancel</asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" CommandName="delete" CommandArgument='<%# Eval("messageId")%>' CssClass="btn btn-danger" runat="server"  Visible="False" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'>Delete</asp:LinkButton>
                    </div>
                    
                    <div>                   
                        <asp:TextBox ID="txtDiscussionComment" runat="server" style="width: 700px;" Text='<%# Eval("message") %>' Enabled="false" BorderStyle="None" BackColor="Transparent"  ></asp:TextBox>                                                    
                        <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtDiscussionComment" runat="server" ErrorMessage=" Comment Cannot Be Blank" ControlToValidate="txtDiscussionComment" ></asp:RequiredFieldValidator>
                    
                    </div>
                 </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
     <div class="input-group input-group-sm chatMessageControls">
        <asp:TextBox ID="txtComment" runat="server"  Width="89%" placeholder="Type your comment here.." aria-describedby="sizing-addon3" TextMode="MultiLine"></asp:TextBox>
         
          
        <span class="input-group-btn">
            <asp:Button ID="clearMessageButton" runat="server" Text="Clear" class="btn btn-default" OnClick="clearMessageButton_Click"/>
            <asp:Button ID="sendMessageButton" runat="server" Text="Send" class="btn btn-primary" OnClick="sendMessageButton_Click"/>
            
        </span>
       
    </div>
</asp:Content>
