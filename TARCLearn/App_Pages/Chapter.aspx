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
              height:15px;
              width:15px;
              
        }
    </style>
     
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="modal fade bd-example-modal-lg" id="modalForm">
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

        <div class="label1">
            <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">Chapter</asp:Label>
             <asp:ImageButton ID="btnDeleteChapter" CssClass="rightButton" runat="server" ImageUrl="~/images/delete_icon.png" OnClick="btnDeleteChapter_Click"  /> 
             <asp:ImageButton ID="btnAddChapter" CssClass="rightButton" runat="server" ImageUrl="~/images/add_icon.png"  data-toggle="modal" data-target="#modalForm" OnClientClick="return false;" />
        </div>

        <asp:Repeater ID="chpRepeater" runat="server" OnItemCommand="chapterRepeater_ItemCommand" >
            <ItemTemplate>            
                <asp:Button ID="btnChp" CommandName="selectChp"  Text= '<%# "Chapter " + (Eval("chpNo")) + " " + (Eval("chpTitle")) %>' CssClass="button1" runat="server"/>           
                <asp:Button ID="btnRM" CommandName="selectRM" CommandArgument='<%# Eval("chpId")%>' Text= "Reading Material"  CssClass="button2" runat="server" Visible="False"/>
                <asp:Button ID="btnVideo" CommandName="selectVideo" CommandArgument='<%# Eval("chpId")%>' Text= "Videos"  CssClass="button2" runat="server" Visible="False"/>
                <asp:Button ID="btnDis" CommandName="selectDis" CommandArgument='<%# Eval("chpId")%>' Text= "Discussion"  CssClass="button2" runat="server" Visible="False"/>
                <asp:Button ID="btnQuiz" CommandName="selectQuiz" CommandArgument='<%# Eval("chpId")%>' Text= "Quiz"  CssClass="button2" runat="server" Visible="False"/>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Repeater ID="rptDeleteChapter" runat="server"  Visible="False" OnItemCommand="rptDeleteChapter_ItemCommand" >
                <ItemTemplate>
                    <div class="label1">
                        <asp:Label ID="lblChapter" runat="server"  Text= '<%# "Chapter " + (Eval("chpNo")) + " " + (Eval("chpTitle")) %>' />                
                        <asp:ImageButton CommandName="deleteChapter" CommandArgument='<%# Eval("chpId")%>'
                            ID="ImageButton2" CssClass="rightButton" runat="server" ImageUrl="~/images/delete2_icon.png"  OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
                    </div>           
                </ItemTemplate>
        </asp:Repeater>

        </div>
</asp:Content>
