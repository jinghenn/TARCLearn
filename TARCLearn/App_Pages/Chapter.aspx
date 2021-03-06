<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="Chapter.aspx.cs" Inherits="TARCLearn.App_Pages.Chapter" %>

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
        

        .button2 {
            background-color: #f5f3f0;
            text-align: left;
            border-radius: 0px;
            transition-duration: 0.4s;
            width:calc(100% - 15px); 
            height:70px; 
            border-width:0px; 
            padding-left:40px;  
            margin-left:15px;
        }

            .button2:hover {
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
    <div class="modal fade bd-example-modal-lg" id="modalForm" >
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Add New Chapter</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        <div class="row mb-3">
                            <label for="formChpNo" class="col-sm-3 col-form-label">Chapter No.</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formChpNo" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvChpNo" ControlToValidate="formChpNo" runat="server" Display="Dynamic" ErrorMessage="Chapter No. Cannot Be Blank"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ValidationGroup="Add Form" ForeColor="Red" id="revFormChpNo" runat="server" Display="Dynamic" ErrorMessage="Only two level digit is allowed" ValidationExpression="^\d+([.]\d)?$" ControlToValidate="formChpNo" />                            
                            </div>
                        </div>  
                        
                        <div class="row mb-3">
                            <label for="formChpTitle" class="col-sm-3 col-form-label">Chapter Title</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formChpTitle" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvChpTitle" ControlToValidate="formChpTitle" runat="server" Display="Dynamic" ErrorMessage="Chapter Title Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>   
                      

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="addChapterFormSubmitClicked" ValidationGroup="Add Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>

    <div class="main main-raised" >
        <nav aria-label="breadcrumb" style="--bs-breadcrumb-divider: '>'; padding: 10px 0 0 30px; height:41px; background-color:#F5F5F5; width:100%;">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="course.aspx">Home</a></li>
            <li class="breadcrumb-item active" aria-current="page">Chapter</li>
          </ol>
        </nav>
        <div class="label1">
            <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">Chapter</asp:Label>
             <asp:ImageButton ID="btnMore" CssClass="rightButton" runat="server"  ImageUrl="~/images/more_icon.png" Height="15px" Width="15px" OnClick="btnDeleteChapter_Click"  /> 
             <asp:ImageButton ID="btnAdd" CssClass="rightButton" runat="server"  ImageUrl="~/images/add_icon.png"  data-toggle="modal" data-target="#modalForm" Height="15px" Width="15px" OnClientClick="return false;" />
        </div>

        <asp:Repeater ID="chpRepeater" runat="server" OnItemCommand="chapterRepeater_ItemCommand" >
            <ItemTemplate>            
                <asp:Button ID="btnChp" CommandName="selectChp"  Text= '<%# "Chapter " + (Eval("chpNo")) + " " + (Eval("chpTitle")) %>' CssClass="button1" runat="server"/>           
                <asp:Button ID="btnRM" CommandName="selectRM" CommandArgument='<%# Eval("chpId")%>' Text= "Material"  CssClass="button2" runat="server" Visible="False"/>
                <asp:Button ID="btnVideo" CommandName="selectVideo" CommandArgument='<%# Eval("chpId")%>' Text= "Videos"  CssClass="button2" runat="server" Visible="False"/>
                <asp:Button ID="btnDis" CommandName="selectDis" CommandArgument='<%# Eval("chpId")%>' Text= "Discussion"  CssClass="button2" runat="server" Visible="False"/>
                <asp:Button ID="btnQuiz" CommandName="selectQuiz" CommandArgument='<%# Eval("chpId")%>' Text= "Quiz"  CssClass="button2" runat="server" Visible="False"/>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Repeater ID="rptDeleteChapter" runat="server"  Visible="False" OnItemCommand="rptDeleteChapter_ItemCommand" >
                <ItemTemplate>

                    <div class="label1">
                                       
                        
                    <div>
                        <asp:Label ID="lblChapter" runat="server"  Text= "Chapter "/> 
                        <asp:TextBox ID="txtChapterNo" runat="server" Text='<%#Eval("chpNo") %>' Enabled="false" BorderStyle="None" BackColor="Transparent" AutoPostBack="False"></asp:TextBox>

                        <asp:TextBox ID="txtChapterTitle" runat="server" style="width: 700px;" Text='<%# Eval("chpTitle") %>' Enabled="false" BorderStyle="None" BackColor="Transparent"  ></asp:TextBox>
                    
                        <asp:ImageButton ID="btnDelete" CssClass="rightButton" CommandName="delete" CommandArgument='<%# Eval("chpId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png" Visible="True" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
                        <asp:ImageButton ID="btnEdit" CssClass="rightButton" CommandName="edit" CommandArgument='<%# Eval("chpId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png" CausesValidation="false"/>                
                        <asp:ImageButton ID="btnCancel" CssClass="rightButton" CommandName="cancel" Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete2_icon.png" Visible="False" CausesValidation="false"/>                
                        <asp:ImageButton ID="btnSave" CssClass="rightButton" CommandName="save" CommandArgument='<%# Eval("chpId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/save_icon.png"  Visible="False" ValidationGroup="Edit"/>                


                    </div>
                    <div>
                        <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtChapterNo" runat="server" ErrorMessage=" - Chapter No Cannot Be Blank" ControlToValidate="txtChapterNo" ></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ValidationGroup="Edit" ForeColor="Red" id="revtxtChapterNo" runat="server" Display="Dynamic" ErrorMessage=" - Only two level digit is allowed" ValidationExpression="^\d+([.]\d)?$" ControlToValidate="txtChapterNo" />                            

                    </div>
                    <div>
                        <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtChapterTitle" runat="server" ErrorMessage=" - Chapter Title Cannot Be Blank" ControlToValidate="txtChapterTitle" ></asp:RequiredFieldValidator>
                    </div>
                    </div>
                </ItemTemplate>
        </asp:Repeater>

        </div>
</asp:Content>
