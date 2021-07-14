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
      <nav aria-label="breadcrumb" style="--bs-breadcrumb-divider: '>'; padding: 10px 0 0 30px; height:41px; background-color:#F5F5F5;">
       <ol class="breadcrumb">
         <li class="breadcrumb-item"><asp:Label ID="lblHome" runat="server"/></li>
         <li class="breadcrumb-item"><asp:Label ID="lblChp" runat="server"/></li>
         <li class="breadcrumb-item active" aria-current="page">Quizs</li>
       </ol>
     </nav>
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
                        <asp:ImageButton ID="btnDelete" CssClass="rightButton" CommandName="delete" CommandArgument='<%# Eval("quizId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png" Visible="True" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
                        <asp:ImageButton ID="btnEdit" CssClass="rightButton" CommandName="edit" CommandArgument='<%# Eval("quizId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png" CausesValidation="false"/>                
                        <asp:ImageButton ID="btnCancel" CssClass="rightButton" CommandName="cancel" Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete2_icon.png" Visible="False" CausesValidation="false"/>                
                        <asp:ImageButton ID="btnSave" CssClass="rightButton" CommandName="save" CommandArgument='<%# Eval("quizId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/save_icon.png"  Visible="False" ValidationGroup="Edit"/>                


                        
                    </div>
                    
                    <div>
                        <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtQuizTitle" runat="server" ErrorMessage="Quiz Title Cannot Be Blank" ControlToValidate="txtQuizTitle" ></asp:RequiredFieldValidator>
                    </div>
                </div>
            
            </ItemTemplate>
        </asp:Repeater>
      
    </div>
</asp:Content>
