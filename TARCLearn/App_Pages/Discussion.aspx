<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="Discussion.aspx.cs" Inherits="TARCLearn.App_Pages.Discussionaspx" %>
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
                                <asp:TextBox ID="formDisDesc" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Edit Form" ForeColor="Red" ID="rfvformDisDesc" ControlToValidate="formDisDesc" runat="server" Display="Dynamic" ErrorMessage="Discussion Description Cannot Be Blank"></asp:RequiredFieldValidator>
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
     <nav aria-label="breadcrumb" style="--bs-breadcrumb-divider: '>'; padding: 10px 0 0 30px; height:41px; background-color:#F5F5F5;">
       <ol class="breadcrumb">
         <li class="breadcrumb-item"><asp:Label ID="lblHome" runat="server"/></li>
         <li class="breadcrumb-item"><asp:Label ID="lblChp" runat="server"/></li>        
         <li class="breadcrumb-item active" aria-current="page">Discussion Thread</li>
       </ol>
     </nav>
        <div class="label1">
            <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">DiscussionThread</asp:Label> 
            <asp:ImageButton ID="btnMore" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/more_icon.png" OnClick="btnMore_Click" /> 
            <asp:ImageButton ID="btnAdd" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/add_icon.png"  data-toggle="modal" data-target="#modalForm" OnClientClick="return false;" />        
        </div>
    
        <asp:Repeater ID="disRepeater" runat="server" OnItemCommand="courseRepeater_ItemCommand" >
            <ItemTemplate>
                <asp:Button CommandName="selectDiscussion" CommandArgument='<%# Eval("threadId")%>' 
                    Text= '<%# Eval("threadTitle")%>' 
                    runat="server" ID="btnCourse" CssClass="button1"  />
            
            </ItemTemplate>
        </asp:Repeater>

        <asp:Repeater ID="rptEditDiscussion" runat="server"  Visible="False" OnItemCommand="rptEditDiscussion_ItemCommand" OnItemDataBound="rptEditDiscussion_ItemDataBound">
            <ItemTemplate>
                <div style="padding:12.5px 0 12.5px 30px; height: 70px;">
                    <div>

                        <asp:TextBox ID="txtDiscussionTitle" runat="server" style="width: 700px;" Text='<%# Eval("threadTitle") %>' Enabled="false" BorderStyle="None" BackColor="Transparent"  ></asp:TextBox>
                        <asp:ImageButton ID="btnDelete" CssClass="rightButton" CommandName="delete" CommandArgument='<%# Eval("threadId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png" Visible="True" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
                        <asp:ImageButton ID="btnEdit" CssClass="rightButton" CommandName="edit" CommandArgument='<%# Eval("threadId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png" CausesValidation="false"/>                
                        <asp:ImageButton ID="btnCancel" CssClass="rightButton" CommandName="cancel" Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete2_icon.png" Visible="False" CausesValidation="false"/>                
                        <asp:ImageButton ID="btnSave" CssClass="rightButton" CommandName="save" CommandArgument='<%# Eval("threadId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/save_icon.png"  Visible="False" ValidationGroup="Edit"/>                

                       
                    </div>
                    
                    <div>
                        <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtDiscussionTitle" runat="server" ErrorMessage=" - Course Title Cannot Be Blank" ControlToValidate="txtDiscussionTitle" ></asp:RequiredFieldValidator>
                    </div>
                </div>
            
            </ItemTemplate>
        </asp:Repeater>
      
    </div>
</asp:Content>
