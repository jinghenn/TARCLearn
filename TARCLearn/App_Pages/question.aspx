<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="question.aspx.cs" Inherits="TARCLearn.App_Pages.quiz" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="modal fade bd-example-modal-lg" id="modalForm">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Add New Question</h4>
                        <button type="button" class="close" data-dismiss="modal" OnClick="closeFormClicked" >&times;</button>
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
                            <label for="formChoice1" class="col-sm-3 col-form-label">Choice 1</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formChoice1" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvFormChoice1" ControlToValidate="formChoice1" runat="server" Display="Dynamic" ErrorMessage="Choice 1 Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div> 

                        <div class="row mb-3">
                            <label for="formChoice2" class="col-sm-3 col-form-label">Choice 2</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formChoice2" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvFormChoice2" ControlToValidate="formChoice2" runat="server" Display="Dynamic" ErrorMessage="Choice 2 Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div> 

                        <div class="row mb-3">
                            <label for="formChoice3" class="col-sm-3 col-form-label">Choice 3</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formChoice3" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvFormChoice3" ControlToValidate="formChoice3" runat="server" Display="Dynamic" ErrorMessage="Choice 3 Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div> 

                        <div class="row mb-3">
                            <label for="formChoice4" class="col-sm-3 col-form-label">Choice 4</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formChoice4" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvFormChoice4" ControlToValidate="formChoice4" runat="server" Display="Dynamic" ErrorMessage="Choice 4 Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div> 

                        <div class="row mb-3">
                            <label for="ddlAns" class="col-sm-3 col-form-label">Answer</label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlAns" runat="server" >                                     
                                    <asp:ListItem>Choice 1</asp:ListItem>  
                                    <asp:ListItem>Choice 2</asp:ListItem>  
                                    <asp:ListItem>Choice 3</asp:ListItem>  
                                    <asp:ListItem>Choice 4</asp:ListItem>                                    
                                </asp:DropDownList>  
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvDdlAns" ControlToValidate="ddlAns" runat="server" Display="Dynamic" ErrorMessage="Answer Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div> 

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="addQuesFormSubmitClicked" ValidationGroup="Add Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>
    <div class="main main-raised" >
        <div class="label1">
                <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large"/> 
                <asp:ImageButton ID="btnMore" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/more_icon.png" OnClick="btnMore_Click" /> 
                <asp:ImageButton ID="btnAddQues" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/add_icon.png" data-toggle="modal" data-target="#modalForm" OnClientClick="return false;" />        
        </div>

        <asp:Repeater ID="quizRepeater" runat="server" OnItemDataBound="quizRepeater_ItemDataBound" OnItemCommand="quizRepeater_ItemCommand">
            <ItemTemplate>
                <div class="questionBox">
                    <div style="height:45px; ">
                            <asp:Label runat="server" Text='<%# Eval("question")%>'></asp:Label>  
                            <asp:ImageButton ID="btnDeleteQues" CssClass="rightButton" CommandName="deleteQues" CommandArgument='<%# Eval("questionId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png" Visible="False" OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
                            <asp:ImageButton ID="btnEditQues" CssClass="rightButton" CommandName="updateQues" CommandArgument='<%# Eval("questionId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png" Visible="False" data-toggle="modal" data-target="#modalForm" OnClientClick="return false;"/>        
                    </div>
                                        
                    <asp:RadioButtonList ID="rbl1" runat="server"></asp:RadioButtonList>
                </div>    

            </ItemTemplate>
        </asp:Repeater>

        <asp:Button ID="Button1" runat="server" Text="Submit" class="btn btn-primary rightButton" OnClick="Button1_Click" />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
      </div>
</asp:Content>
