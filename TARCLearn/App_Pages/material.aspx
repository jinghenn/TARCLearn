<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="material.aspx.cs" Inherits="TARCLearn.App_Pages.readingMaterial" %>
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
        .deleteRpt{
            background-color: #f5f3f0;
            width:calc(100% - 15px); 
            height:70px; 
            border-width:0px; 
            padding:25px 0 12.5px 30px;  
            margin-left:15px;
        }
        .label2{            
            text-align: left center;                    
        }
        .rightButton{
              float: right;
              margin-right:30px;
             
              
              
        }

   </style>

    <script>
        function UploadFileCheck(source, arguments) { //client validation
            var sFile = arguments.Value;
            const params = new URLSearchParams(window.location.search);
            var materialType = params.get('materialType').toString();
            if (materialType == "video") {
                
                 arguments.IsValid =
                     ((sFile.endsWith('.flv')) ||
                         (sFile.endsWith('.mov')) ||
                         (sFile.endsWith('.wmv')) ||
                         (sFile.endsWith('.avi')) ||
                         (sFile.endsWith('.avchd')) ||
                         (sFile.endsWith('.f4v')) ||
                         (sFile.endsWith('.swf')) ||
                         (sFile.endsWith('.mkv')) ||
                         (sFile.endsWith('.webm')) ||
                         (sFile.endsWith('.html5')) ||
                         (sFile.endsWith('.mpeg-2')) ||
                         (sFile.endsWith('.mp4')));
              

            } else {
                
                 arguments.IsValid =
                     ((sFile.endsWith('.pdf')) ||
                         (sFile.endsWith('.pptx')) ||
                         (sFile.endsWith('.ppt')) ||
                         (sFile.endsWith('.doc')) ||
                         (sFile.endsWith('.docx')) ||
                         (sFile.endsWith('.xlsx')) ||
                         (sFile.endsWith('.jpg')) ||
                         (sFile.endsWith('.png')) ||
                         (sFile.endsWith('.jpeg')));
                
            }
        }

        function editMaterial() {
                $('#editForm').modal('show');
        }

        


    
        
    </script>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="modal fade bd-example-modal-lg" id="modalForm">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Add New Material</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        <div class="row mb-3">
                            <label for="formIndex" class="col-sm-3 col-form-label">Material Index</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formIndex" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvFormIndex" ControlToValidate="formIndex" runat="server" Display="Dynamic" ErrorMessage="Material Title Cannot Be Blank"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ValidationGroup="Add Form" ForeColor="Red" id="revFormIndex" runat="server" Display="Dynamic" ErrorMessage="Please Enter Only Numbers." ValidationExpression="^\d+$" ControlToValidate="formIndex" />                            
                            </div>
                        </div>  

                        <div class="row mb-3">
                            <label for="formTitle" class="col-sm-3 col-form-label">Material Title</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formTitle" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvTitle" ControlToValidate="formTitle" runat="server" Display="Dynamic" ErrorMessage="Material Title Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>  

                        <div class="row mb-3">
                            <label for="formDescription" class="col-sm-3 col-form-label">Material Description</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formDescription" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>                                                                  

                        <div class="row mb-3">
                            <label for="formMaterialMode" class="col-sm-3 col-form-label">Material Category</label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="formMaterialMode" CssClass="form-select" runat="server">                                
                                    <asp:ListItem Value="Lecture">Lecture</asp:ListItem>
                                    <asp:ListItem Value="Practical">Practical</asp:ListItem>
                                    <asp:ListItem Value="Tutorial">Tutorial</asp:ListItem>
                                    <asp:ListItem Value="Other">Other</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                      
                         <div class="row mb-3">
                            <label for="file" class="form-label">Upload your file here</label>
                            <asp:FileUpload ID="file" runat="server" CssClass="form-control" />
                            <asp:CustomValidator ValidationGroup="Add Form" ForeColor="Red" ID="CustomValidator1" ControlToValidate="file" runat="server" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Invalid: File Type." ClientValidationFunction="UploadFileCheck"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvFile" ControlToValidate="file" runat="server" Display="Dynamic" ErrorMessage="File Cannot Be Blank"></asp:RequiredFieldValidator>

                             <div id="uploadHelp" class="form-text">
                                <asp:Label ID="lblSupport" runat="server" ></asp:Label>                                
                            </div>
                        </div>

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="addNewMaterialFormSubmitClicked" ValidationGroup="Add Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>
    <div class="modal fade bd-example-modal-lg" id="editForm">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Edit Material</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">

                        <div class="row mb-3">
                            <label for="formEditIndex" class="col-sm-3 col-form-label">Material Index</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formEditIndex" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Edit Form" ForeColor="Red" ID="rfvFormEditIndex" ControlToValidate="formEditIndex" runat="server" Display="Dynamic" ErrorMessage="Material Title Cannot Be Blank"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ValidationGroup="Edit Form" ForeColor="Red" id="revFormEditIndex" runat="server" Display="Dynamic" ErrorMessage="Please Enter Only Numbers." ValidationExpression="^\d+$" ControlToValidate="formEditIndex" />                            
                            </div>
                        </div>  

                        <div class="row mb-3">
                            <label for="formEditTitle" class="col-sm-3 col-form-label">Material Title</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formEditTitle" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Edit Form" ForeColor="Red" ID="rfvFormEditTitle" ControlToValidate="formEditTitle" runat="server" Display="Dynamic" ErrorMessage="Material Title Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>  

                        <div class="row mb-3">
                            <label for="formEditDescription" class="col-sm-3 col-form-label">Material Description</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formEditDescription" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>                                                                  

                        <div class="row mb-3">
                            <label for="formEditMaterialMode" class="col-sm-3 col-form-label">Material Category</label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlFormEditMaterialMode" CssClass="form-select" runat="server">                                
                                    <asp:ListItem Value="Lecture">Lecture</asp:ListItem>
                                    <asp:ListItem Value="Practical">Practical</asp:ListItem>
                                    <asp:ListItem Value="Tutorial">Tutorial</asp:ListItem>
                                    <asp:ListItem Value="Other">Other</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                      
                         

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="editMaterialFormSubmitClicked" ValidationGroup="Edit Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>

    <div class="main main-raised" >
        <nav aria-label="breadcrumb" style="--bs-breadcrumb-divider: '>'; padding: 10px 0 0 30px; height:41px; background-color:#F5F5F5;width:100%;">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><asp:Label ID="lblHome" runat="server"/></li>
             <li class="breadcrumb-item"><asp:Label ID="lblChp" runat="server"/></li>
            <li class="breadcrumb-item active" aria-current="page"><asp:Label ID="lblMaterial" runat="server"/></li>
          </ol>
        </nav>
<%-- title   --%>
      <div class="label1">
         <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">Material</asp:Label>   
         <asp:ImageButton ID="btnMore" CssClass="rightButton" runat="server" ImageUrl="~/images/more_icon.png"  Height="15px" Width="15px" OnClick="btnMore_Click"   /> 
         <asp:ImageButton ID="btnAdd" CssClass="rightButton" runat="server" ImageUrl="~/images/add_icon.png"   Height="15px" Width="15px" data-toggle="modal" data-target="#modalForm" OnClientClick="return false;" />
      </div>
       

<%-- lecture section  --%>     
      <asp:Button Text='Lecture'  runat="server" ID="btnLecture" CssClass="button1" OnClick="btnLecture_Click"  /> 

      <%-- normal repeater  --%>   
      <asp:Repeater ID="rptLect" runat="server" OnItemCommand="rptMaterial_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# (Eval("materialTitle")) %>'   runat="server" ID="btnCourse" CssClass="button2" ToolTip='<%# Eval("materialDescription")%>' />
            
        </ItemTemplate>
      </asp:Repeater>

      <%-- Edit repeater  --%>
      <asp:Repeater ID="rptDelLect" runat="server"  Visible="False" OnItemCommand="rptEdit_ItemCommand" >
        <ItemTemplate>           
            <div class="deleteRpt">                                                              
                    <div>
                        <asp:TextBox ID="txtLec" runat="server" style="width: 700px;" Text='<%# Eval("materialTitle") %>' Enabled="false" BorderStyle="None" BackColor="Transparent"  ></asp:TextBox>
                    
                        <asp:ImageButton ID="btnDelete" CssClass="rightButton" CommandName="delete" CommandArgument='<%# Eval("materialId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png" Visible="True" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
                        <asp:ImageButton ID="btnEdit" CssClass="rightButton" CommandName="edit" CommandArgument='<%# Eval("materialId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png" CausesValidation="false"/>                                 
                        
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtLec" runat="server" ErrorMessage=" - Material Title Cannot Be Blank" ControlToValidate="txtLec" ></asp:RequiredFieldValidator>
                    </div>
                   
            </div>

        </ItemTemplate>
      </asp:Repeater>

<%-- practical section  --%>  
    <asp:Button Text='Practical'  runat="server" ID="btnPractical" CssClass="button1" OnClick="btnPractical_Click"  />

    <%-- normal repeater  --%>
    <asp:Repeater ID="rptPrac" runat="server" OnItemCommand="rptMaterial_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# (Eval("materialTitle")) %>'  runat="server" ID="btnCourse" CssClass="button2"  ToolTip='<%# Eval("materialDescription")%>'/>
            
        </ItemTemplate>
    </asp:Repeater>
    
    <%-- Edit repeater  --%>
    <asp:Repeater ID="rptDelPrac" runat="server"  Visible="False" OnItemCommand="rptEdit_ItemCommand" >
        <ItemTemplate>
           <div class="deleteRpt">
               <div>
                        <asp:TextBox ID="txtPrac" runat="server" style="width: 700px;" Text='<%# Eval("materialTitle") %>' Enabled="false" BorderStyle="None" BackColor="Transparent"  ></asp:TextBox>
                        <asp:ImageButton ID="btnDelete" CssClass="rightButton" CommandName="delete" CommandArgument='<%# Eval("materialId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png" Visible="True" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
                        <asp:ImageButton ID="btnEdit" CssClass="rightButton" CommandName="edit" CommandArgument='<%# Eval("materialId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png" CausesValidation="false"/>                
   
                 
                </div>
                <div>
                    <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtPrac" runat="server" ErrorMessage=" - Material Title Cannot Be Blank" ControlToValidate="txtPrac" ></asp:RequiredFieldValidator>
                </div>
           </div>           
        </ItemTemplate>
      </asp:Repeater>

<%-- tutorial section  --%>  
    <asp:Button Text='Tutorial'  runat="server" ID="btnTutorial" CssClass="button1" OnClick="btnTutorial_Click"  />
    
    <%-- normal repeater  --%>
    <asp:Repeater ID="rptTut" runat="server" OnItemCommand="rptMaterial_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# (Eval("materialTitle")) %>'  runat="server" ID="btnCourse" CssClass="button2"  ToolTip='<%# Eval("materialDescription")%>'/>
            
        </ItemTemplate>
    </asp:Repeater>

    <%-- Edit repeater  --%>
    <asp:Repeater ID="rptDelTut" runat="server"  Visible="False" OnItemCommand="rptEdit_ItemCommand" >
        <ItemTemplate>
           <div class="deleteRpt">
               <div>
                 <asp:TextBox ID="txtTut" runat="server" style="width: 700px;" Text='<%# Eval("materialTitle") %>' Enabled="false" BorderStyle="None" BackColor="Transparent"  ></asp:TextBox>
                 <asp:ImageButton ID="btnDelete" CssClass="rightButton" CommandName="delete" CommandArgument='<%# Eval("materialId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png" Visible="True" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
                 <asp:ImageButton ID="btnEdit" CssClass="rightButton" CommandName="edit" CommandArgument='<%# Eval("materialId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png" CausesValidation="false"/>                

                </div>
                <div>
                    <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtTut" runat="server" ErrorMessage=" - Material Title Cannot Be Blank" ControlToValidate="txtTut" ></asp:RequiredFieldValidator>
                </div>                                           
           </div>           
        </ItemTemplate>
      </asp:Repeater>

<%-- other section  --%>
    <asp:Button Text='Other'  runat="server" ID="btnOther" CssClass="button1" OnClick="btnOther_Click"  />
 
<%-- normal repeater  --%>
    <asp:Repeater ID="rptOth" runat="server" OnItemCommand="rptMaterial_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# (Eval("materialTitle")) %>'  runat="server" ID="btnCourse" CssClass="button2" ToolTip='<%# Eval("materialDescription")%>' />
            
        </ItemTemplate>
    </asp:Repeater> 

<%-- Edit repeater  --%>
    <asp:Repeater ID="rptDelOth" runat="server"  Visible="False" OnItemCommand="rptEdit_ItemCommand" >
        <ItemTemplate>
           <div class="deleteRpt">
               <div>
                 <asp:TextBox ID="txtOth" runat="server" style="width: 700px;" Text='<%# Eval("materialTitle") %>' Enabled="false" BorderStyle="None" BackColor="Transparent"  ></asp:TextBox>
                 <asp:ImageButton ID="btnDelete" CssClass="rightButton" CommandName="delete" CommandArgument='<%# Eval("materialId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/delete_icon.png" Visible="True" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
                 <asp:ImageButton ID="btnEdit" CssClass="rightButton" CommandName="edit" CommandArgument='<%# Eval("materialId")%>' Height="15px" Width="15px" runat="server" ImageUrl="~/images/edit_icon.png" CausesValidation="false"/>                
                </div>
                <div>
                    <asp:RequiredFieldValidator ValidationGroup="Edit" ForeColor="Red" Display="Dynamic" ID="rfvtxtOth" runat="server" ErrorMessage=" - Material Title Cannot Be Blank" ControlToValidate="txtOth" ></asp:RequiredFieldValidator>
                </div>                                           
           </div>                   
        </ItemTemplate>
      </asp:Repeater>
   </div>
</asp:Content>
