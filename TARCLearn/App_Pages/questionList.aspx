<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="questionList.aspx.cs" Inherits="TARCLearn.App_Pages.questionList" %>
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

        .questionBox{
            
            width:calc(100% - 30px);  
            height:auto;
            padding:20px;  
            margin:0 15px 0 15px;
            border-bottom:solid;
            border-width: 2px;
        }
      
      
   </style>

    <script type="text/javascript">
        
        function openModal() {
            $('#addForm').modal('show');
        }  
        function showResult() {
            $('#quizResultForm').modal('show');
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="modal fade bd-example-modal-lg" id="modalForm">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Add New Question</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        
                        <div class="row mb-3">
                            <label for="formQues" class="col-sm-3 col-form-label">Question</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formQues" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvQues" ControlToValidate="formQues" runat="server" Display="Dynamic" ErrorMessage="Question Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>                                                                    

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="addQuesFormSubmitClicked" ValidationGroup="Add Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>

    <div class="modal fade bd-example-modal-lg" id="addForm">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Label ID="lblMTitle" runat="server"  class="modal-title"></asp:Label>  
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        
                        <div class="row mb-3">
                            <label for="formChoice" class="col-sm-3 col-form-label">Choice</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formChoice" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Choice Form" ForeColor="Red" ID="rfvFormChoice" ControlToValidate="formChoice" runat="server" Display="Dynamic" ErrorMessage="Choice Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        
                        <div class="row mb-3">
                            <asp:Label ID="lblAnswer" class="col-sm-3 col-form-label" runat="server" Text="Is Answer"></asp:Label>                            
                            <div class="col-sm-9">
                                 <asp:RadioButtonList ID="rblAnswer" runat="server">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>                                  
                                </asp:RadioButtonList>
                                 <asp:RequiredFieldValidator ValidationGroup="Add Choice Form" ForeColor="Red" ID="rfvrblAnswer" ControlToValidate="rblAnswer" runat="server" Display="Dynamic" ErrorMessage="Is Answer Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div> 

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="addChoiceFormSubmitClicked" ValidationGroup="Add Choice Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>
    <div class="modal" tabindex="-1" role="dialog" id="quizResultForm">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Quiz Results</h5>                                   
          </div>
          <div class="modal-body">
            <p>Here is your quiz results.</p>
            <asp:Label ID="lblResult" runat="server" Font-Size="XX-Large" Font-Bold="True"></asp:Label>
          </div>
          <div class="modal-footer">
            <asp:Button CssClass="btn btn-success" runat="server" OnClick="quizResultFormSubmitClicked" Text="Return back to Quiz List." />
            
          </div>
        </div>
      </div>
    </div>

    <div class="main main-raised" >
     <nav aria-label="breadcrumb" style="--bs-breadcrumb-divider: '>'; padding: 10px 0 0 30px; height:41px; background-color:#F5F5F5;width:100%;">
       <ol class="breadcrumb">
         <li class="breadcrumb-item"><asp:Label ID="lblHome" runat="server"/></li>
         <li class="breadcrumb-item"><asp:Label ID="lblChp" runat="server"/></li>
         <li class="breadcrumb-item"><asp:Label ID="lblQuiz" runat="server"/></li>
         <li class="breadcrumb-item active" aria-current="page"><asp:Label ID="lblQues" runat="server"/></li>
       </ol>
     </nav>
        <div class="label1">
                <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large"/> 
                <asp:ImageButton ID="btnMore" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/more_icon.png" OnClick="btnMore_Click" /> 
                <asp:ImageButton ID="btnAddQues" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/add_icon.png" data-toggle="modal" data-target="#modalForm" OnClientClick="return false;" />        
        </div>

        <asp:Repeater ID="quizRepeater" runat="server" OnItemDataBound="quizRepeater_ItemDataBound" OnItemCommand="quizRepeater_ItemCommand">
            <ItemTemplate>
                <div class="questionBox">
                    <div>
                                                    
                            <asp:TextBox ID="txtQuesNo" runat="server" Width="5%"  Enabled="false" BorderStyle="None" BackColor="Transparent" TextMode="MultiLine" Rows="2" ></asp:TextBox>
                            <asp:TextBox ID="txtQuesText"  Text='<%# Eval("questionText")%>' runat="server" Width="65%"  Enabled="false" BorderStyle="None" BackColor="Transparent" TextMode="MultiLine" Rows="2" ></asp:TextBox>
                          
                            <div class=" rightButton "  role="group">
                                <asp:LinkButton ID="btnEditQuesText" CommandName="edit" CommandArgument='<%# Eval("questionId")%>' CssClass="btn btn-outline-info" runat="server" CausesValidation="false"  Visible="False">Edit</asp:LinkButton>
                                <asp:LinkButton ID="btnAddChoice" CommandName="add" CommandArgument='<%# Eval("questionId")%>' CssClass="btn btn-outline-secondary " runat="server"  Visible="False" ValidationGroup="Edit" >Add Choice</asp:LinkButton>
                                <asp:LinkButton ID="btnSaveQuesText" CommandName="save" CommandArgument='<%# Eval("questionId")%>' CssClass="btn btn-outline-success " runat="server"  Visible="False" ValidationGroup="Edit">Save</asp:LinkButton>
                                <asp:LinkButton ID="btnCancelQuesText" CommandName="cancel"  CssClass="btn btn-outline-danger " runat="server"  Visible="False" CausesValidation="false">Cancel</asp:LinkButton>
                                <asp:LinkButton ID="btnDeleteQuesText" CommandName="delete" CommandArgument='<%# Eval("questionId")%>' CssClass="btn btn-danger" runat="server"  Visible="False" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'>Delete</asp:LinkButton>
                            </div>      

                    </div>
                                        
                    <asp:RadioButtonList ID="rbl1" runat="server"></asp:RadioButtonList>

                    <asp:Repeater ID="rptEditChoice" runat="server"  OnItemCommand="rptEditChoice_ItemCommand" Visible="False">
                        <ItemTemplate>
                           <div class="label1">
                                <asp:Label ID="lblChoice" runat="server" Text= '<%# Eval("choiceText") %>'/>
                                <asp:ImageButton ID="btnDelete" CommandName="delete" CommandArgument='<%# Eval("choiceId")%>' CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png"  OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
                                <asp:ImageButton ID="btnEdit" CommandName="edit" CommandArgument='<%# Eval("choiceId")%>' CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png"   />                                                      
                            
                           </div> 
                        </ItemTemplate>
                    </asp:Repeater>
                </div>    

            </ItemTemplate>
        </asp:Repeater>


        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary btn-lg rightButton" style="margin:30px;" OnClick="btnSubmit_Click" />
        
      </div>
</asp:Content>
