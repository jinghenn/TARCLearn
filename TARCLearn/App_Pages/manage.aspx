<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="manage.aspx.cs" Inherits="TARCLearn.App_Pages.manage" %>
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
    <div class="modal fade bd-example-modal-lg" id="createCourseForm">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Create New Course</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        
                       <div class="row mb-3">
                            <label for="formCourseCode" class="col-sm-3 col-form-label">Course Code</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formCourseCode" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Create Course Form" ForeColor="Red" ID="rfvFormCourseCode" ControlToValidate="formCourseCode" runat="server" Display="Dynamic" ErrorMessage="Course Code Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>   

                        <div class="row mb-3">
                            <label for="formCourseTitle" class="col-sm-3 col-form-label">Course Title</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formCourseTitle" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Create Course Form" ForeColor="Red" ID="rfvFormCourseTitle" ControlToValidate="formCourseTitle" runat="server" Display="Dynamic" ErrorMessage="Course Title Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div> 

                        <div class="row mb-3">
                            <label for="formCourseDesc" class="col-sm-3 col-form-label">Course Description</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formCourseDesc" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>  

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="createCourseFormSubmitClicked" ValidationGroup="Create Course Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>
    <div class="modal fade bd-example-modal-lg" id="deleteCourseForm">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Delete Course</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        
                        <div class="row mb-3">
                            <label for="formlblCourse" class="col-sm-3 col-form-label">Course</label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="formddlCourse" CssClass="form-select" runat="server" ></asp:DropDownList>
                                

                            </div>
                        </div>

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="deleteCourseFormSubmitClicked"  Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>

    <div class="modal fade bd-example-modal-lg" id="editCourseForm">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Edit Course</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        
                        <div class="row mb-3">
                            <label for="formddlEditCourse" class="col-sm-3 col-form-label">Course</label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="formddlEditCourse" CssClass="form-select" runat="server" ></asp:DropDownList>                                
                            </div>
                        </div>

                         <div class="row mb-3">
                            <label for="formEditCourseCode" class="col-sm-3 col-form-label">Course Code</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formEditCourseCode" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Edit Course Form" ForeColor="Red" ID="rfvFormEditCourseCode" ControlToValidate="formEditCourseCode" runat="server" Display="Dynamic" ErrorMessage="Course Code Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>   

                        <div class="row mb-3">
                            <label for="formEditCourseTitle" class="col-sm-3 col-form-label">Course Title</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formEditCourseTitle" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Edit Course Form" ForeColor="Red" ID="rfvFormEditCourseTitle" ControlToValidate="formEditCourseTitle" runat="server" Display="Dynamic" ErrorMessage="Course Title Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div> 

                        <div class="row mb-3">
                            <label for="formEditCourseDesc" class="col-sm-3 col-form-label">Course Description</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formEditCourseDesc" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>  


                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="editCourseFormSubmitClicked"  ValidationGroup="Edit Course Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>
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
                            <label for="formddlMStudent" class="col-sm-3 col-form-label">Student</label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="formddlMStudent" CssClass="form-select" runat="server" ></asp:DropDownList>

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

        <div class="label1">
            <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">Manage</asp:Label>
             
        </div>
        <asp:Button ID="btnManageCourses" runat="server" Text="Manage Courses" CssClass="button1" OnClick="btnManageCourses_Click"/>
        <asp:Button ID="btnCreateCourse"  Text= "Create Course"  CssClass="button2" runat="server" Visible="False" data-toggle="modal" data-target="#createCourseForm" OnClientClick="return false;"/>
       
        <asp:Button ID="btnEditCourse"  Text= "Edit Course"  CssClass="button2" runat="server" Visible="False" data-toggle="modal" data-target="#editCourseForm" OnClientClick="return false;"/>
        <asp:Button ID="btnDeleteCourse" Text= "Delete Course"  CssClass="button2" runat="server" Visible="False" data-toggle="modal" data-target="#deleteCourseForm" OnClientClick="return false;"/>
        
        <asp:Button ID="btnManageStudent"  runat="server" Text= "Manage Student" CssClass="button1" OnClick="btnManageStudent_Click" />           
        <asp:Button ID="btnEnrolStudent"  Text= "Enrol Student"  CssClass="button2" runat="server" Visible="False" OnClick="btnEnrolStudent_Click"/>
        <asp:Button ID="btnDropStudent"  Text= "Drop Student"  CssClass="button2" runat="server" Visible="False" OnClick="btnDropStudent_Click"/>
        
    </div>
</asp:Content>
