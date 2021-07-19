﻿<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="manage.aspx.cs" Inherits="TARCLearn.App_Pages.manage" %>
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
    <script type="text/javascript">

        function openModal() {
            $('#manageStudentForm').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="modal fade bd-example-modal-lg" id="manageStudentForm">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Label ID="lblMTitle" runat="server"  class="modal-title"></asp:Label>                       
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        
                        <div class="row mb-3">
                            <label for="formddlMCourse" class="col-sm-3 col-form-label">Course</label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="formddlMCourse" CssClass="form-select" runat="server" ></asp:DropDownList>

                            </div>
                        </div>

                        <div class="row mb-3">
                            <label for="formtxtMStudent" class="col-sm-3 col-form-label">Emails</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formtxtMStudent" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Edit Form" ForeColor="Red" ID="rfvformtxtDesc" ControlToValidate="formtxtMStudent" runat="server" Display="Dynamic" ErrorMessage="Discussion Description Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>  

                        

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="manageStudentFormSubmitClicked"  Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>
    <div class="main main-raised" >
     <nav aria-label="breadcrumb" style="--bs-breadcrumb-divider: '>'; padding: 10px 0 0 30px; height:41px; background-color:#F5F5F5;width:100%;">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="course.aspx">Home</a></li>
            <li class="breadcrumb-item active" aria-current="page">Manage User</li>
          </ol>
      </nav>

        <div class="label1">
            <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">Manage Enrolment</asp:Label>
             
        </div>
        <div class=" rightButton "  role="group">         
            <asp:LinkButton ID="lbEnrol"  CssClass="btn btn-outline-success " runat="server" OnClick="lbEnrol_Click" >Enrol</asp:LinkButton>
            <asp:LinkButton ID="lbDrop"  CssClass="btn btn-outline-danger " runat="server" OnClick="lbDrop_Click">Drop</asp:LinkButton>
        </div>  
                                
       
        
        
    </div>
</asp:Content>
