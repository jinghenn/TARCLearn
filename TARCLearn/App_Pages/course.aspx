<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="course.aspx.cs" Inherits="TARCLearn.App_Pages.course" %>
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
    <script type="text/javascript">

        function createCourse() {
            $('#createCourseForm').modal('show');
        }

        function editCourse() {
            $('#editCourseForm').modal('show');
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
                                <asp:RegularExpressionValidator ValidationGroup="Create Course Form" ForeColor="Red" id="revFormCourseCode" runat="server" Display="Dynamic" ErrorMessage="Wrong Format." ValidationExpression="([A-Z]{4}\d{4})|(MPU-[a-zA-Z0-9]{4})|([A-Z]{4}\d{3}[A-Z])" ControlToValidate="formCourseCode" />                            
                        
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
    <div class="modal fade bd-example-modal-lg" id="editCourseForm">       
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Edit Course</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">                                              

                         <div class="row mb-3">
                            <label for="formEditCourseCode" class="col-sm-3 col-form-label">Course Code</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formEditCourseCode" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Edit Course Form" ForeColor="Red" ID="rfvFormEditCourseCode" ControlToValidate="formEditCourseCode" runat="server" Display="Dynamic" ErrorMessage="Course Code Cannot Be Blank"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ValidationGroup="Edit Course Form" ForeColor="Red" id="revFormEditCourseCode" runat="server" Display="Dynamic" ErrorMessage="Wrong Format." ValidationExpression="([A-Z]{4}\d{4})|(MPU-[a-zA-Z0-9]{4})|([A-Z]{4}\d{3}[A-Z])" ControlToValidate="formEditCourseCode" />                            

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
                                <asp:TextBox ID="formEditCourseDesc" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>
                        </div>  


                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="editCourseFormSubmitClicked"  ValidationGroup="Edit Course Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>
    <div class="main main-raised" >
        <nav style="--bs-breadcrumb-divider: '>';  padding: 10px 0 0 30px; height:41px; background-color:#F5F5F5; width:100%;" aria-label="breadcrumb" >
              <ol class="breadcrumb">       
                <li class="breadcrumb-item active"  aria-current="page">Home</li>
              </ol>
         </nav>
        <div class="label1">
            <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">Courses</asp:Label> 
            <asp:ImageButton ID="btnMore" CssClass="rightButton"  Height="15px" Width="15px" runat="server" ImageUrl="~/images/more_icon.png" OnClick="btnMore_Click" /> 
            <asp:ImageButton ID="btnCreate" CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/add_icon.png"  data-toggle="modal" data-target="#createCourseForm" OnClientClick="return false;" />        
        </div>
    
        <asp:Repeater ID="rptCourse" runat="server" OnItemCommand="rptCourse_ItemCommand" >
            <ItemTemplate>
                <asp:Button CommandName="select" CommandArgument='<%# Eval("courseId")%>' 
                    Text= '<%# (Eval("courseCode")) + " " + (Eval("courseTitle")) %>' 
                    runat="server" ID="btnCourse" CssClass="button1"  ToolTip='<%# Eval("courseDesc")%>' />
            
            </ItemTemplate>
        </asp:Repeater>

        <asp:Repeater ID="rptManageCourse" runat="server"  Visible="False" OnItemCommand="rptManageCourse_ItemCommand">
            <ItemTemplate>
                <div class="label1">
                    <asp:Label ID="lblCourse" runat="server" Text= '<%# (Eval("courseCode")) + " " + (Eval("courseTitle")) %>'/> 
                    <asp:ImageButton ID="btnDel" CommandName="delete" CommandArgument='<%# Eval("courseId")%>' CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png"  OnClientClick='return confirm("Are you sure you want to delete this course?");'/> 
                    <asp:ImageButton ID="btnEdit" CommandName="edit" CommandArgument='<%# Eval("courseId")%>' CssClass="rightButton" Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png"   />        
                </div>
            
            </ItemTemplate>
        </asp:Repeater>
      
    </div>
</asp:Content>
