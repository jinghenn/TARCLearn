<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="quiz.aspx.cs" Inherits="TARCLearn.App_Pages.quiz" %>
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
                        <h4 class="modal-title">Add New Quiz</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        
                        <div class="row mb-3">
                            <label for="formCourseCode" class="col-sm-3 col-form-label">Quiz Title</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formQuizTitle" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvFormQuizTitle" ControlToValidate="formQuizTitle" runat="server" Display="Dynamic" ErrorMessage="Quiz Title Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>   

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="addQuizFormSubmitClicked" ValidationGroup="Add Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>

    <div class="main main-raised" >
        <div class="label1">
            <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">Quizs</asp:Label> 
            <asp:ImageButton ID="btnMore" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/more_icon.png" OnClick="btnMore_Click" /> 
            <asp:ImageButton ID="btnAdd" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/add_icon.png"  data-toggle="modal" data-target="#modalForm" OnClientClick="return false;" />        
        </div>
    
        <asp:Repeater ID="quizRepeater" runat="server" OnItemCommand="quizRepeater_ItemCommand" >
            <ItemTemplate>
                <asp:Button CommandName="select" CommandArgument='<%# Eval("quizId")%>' 
                    Text= '<%# Eval("quizTitle")%>' 
                    runat="server" ID="btnQuiz" CssClass="button1"  />
            
            </ItemTemplate>
        </asp:Repeater>

        <asp:Repeater ID="rptDeleteQuiz" runat="server"  Visible="False" OnItemCommand="rptDeleteQuiz_ItemCommand">
            <ItemTemplate>
                <div style="padding:12.5px 0 12.5px 30px; height: 70px;">
                    <div>

                        <asp:TextBox ID="txtQuizTitle" runat="server" style="width: 700px;" Text='<%# Eval("quizTitle") %>' Enabled="false" BorderStyle="None" BackColor="Transparent"  ></asp:TextBox>
                    
                        <div class=" rightButton"  role="group">
                            <asp:LinkButton ID="btnEdit" CommandName="edit"  CssClass="btn btn-outline-info" runat="server" CausesValidation="false">Edit</asp:LinkButton>
                            <asp:LinkButton ID="btnSave" CommandName="save" CommandArgument='<%# Eval("quizId")%>' CssClass="btn btn-outline-success " runat="server"  Visible="False" ValidationGroup="Edit">Save</asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" CommandName="cancel"  CssClass="btn btn-outline-danger " runat="server"  Visible="False" CausesValidation="false">Cancel</asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" CommandName="delete" CommandArgument='<%# Eval("quizId")%>' CssClass="btn btn-danger" runat="server"  Visible="False" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'>Delete</asp:LinkButton>
                        </div> 
                    </div>
                    
                    <div>
                        <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtQuizTitle" runat="server" ErrorMessage="Quiz Title Cannot Be Blank" ControlToValidate="txtQuizTitle" ></asp:RequiredFieldValidator>
                    </div>
                </div>
            
            </ItemTemplate>
        </asp:Repeater>
      
    </div>
</asp:Content>
