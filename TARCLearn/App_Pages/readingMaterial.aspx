<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="readingMaterial.aspx.cs" Inherits="TARCLearn.App_Pages.readingMaterial" %>
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
            padding:23px 0 0 40px;  
            margin-left:15px;
        }
        .label2{            
            text-align: left center;                    
        }
        .rightButton{
              float: right;
              margin-right:30px;
              height:15px;
              width:15px;
              background-color: #f5f3f0;
              
        }

   </style>

    <script>
        function UploadPUploadFileCheckDFCheck(source, arguments) { //client validation
            var sFile = arguments.Value;
            if (formMaterialType.SelectedValue == "False") {
                arguments.IsValid =
                    (sFile.endsWith('.pdf'));
            } else if (formMaterialType.SelectedValue == "True") {
                arguments.IsValid =
                    (sFile.endsWith('.mp4'));
            }
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
                            <label for="formTitle" class="col-sm-3 col-form-label">Material Title</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formTitle" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="Add Form" ForeColor="Red" ID="rfvTitle" ControlToValidate="formTitle" runat="server" Display="Dynamic" ErrorMessage="Material Title Cannot Be Blank"></asp:RequiredFieldValidator>
                            </div>
                        </div>  

                        <div class="row mb-3">
                            <label for="formDescription" class="col-sm-3 col-form-label">Material Title</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="formDescription" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>  
                        
                        <div class="row mb-3">
                            <label for="formMaterialType" class="col-sm-3 col-form-label">Art Category</label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="formMaterialType" CssClass="form-select" runat="server">                                
                                    <asp:ListItem Value="True">Video</asp:ListItem>
                                    <asp:ListItem Value="False">Reading Material</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <label for="formMaterialMode" class="col-sm-3 col-form-label">Art Category</label>
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
                            <asp:CustomValidator ValidationGroup="Add Form" ForeColor="Red" ID="CustomValidator1" ControlToValidate="file" runat="server" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Invalid: File Type " ClientValidationFunction="UploadFileCheck"></asp:CustomValidator>
                            <div id="uploadHelp" class="form-text">
                                Supported file extensions : .pdf for Reading Material and .mp4 for video
                            </div>
                        </div>

                        <div class="row mb-3">
                            <asp:Button CssClass="btn btn-success" runat="server" OnClick="addNewMaterialFormSubmitClicked" ValidationGroup="Add Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>

    <div class="main main-raised" >
<%-- title   --%>
      <div class="label1">
         <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">Reading Material</asp:Label>   
         <asp:ImageButton ID="btnDeleteRM" CssClass="rightButton" runat="server" ImageUrl="~/images/delete_icon.png" OnClick="btnDeleteRM_Click"   /> 
         <asp:ImageButton ID="btnAddRM" CssClass="rightButton" runat="server" ImageUrl="~/images/add_icon.png"  data-toggle="modal" data-target="#modalForm" OnClientClick="return false;" />
      </div>
       

<%-- lecture section  --%>     
      <asp:Button Text='Lecture'  runat="server" ID="btnLecture" CssClass="button1" OnClick="btnLecture_Click"  />

      <%-- normal repeater  --%>   
      <asp:Repeater ID="rptLect" runat="server" OnItemCommand="rmRepeater_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# Eval("materialTitle")%>'  runat="server" ID="btnCourse" CssClass="button2"  />
            
        </ItemTemplate>
      </asp:Repeater>

      <%-- delete repeater  --%>
      <asp:Repeater ID="rptDelLect" runat="server"  Visible="False" OnItemCommand="rptDeleteRM_ItemCommand" >
        <ItemTemplate>
           <div class="deleteRpt">
               <asp:Label ID="lblLec" runat="server"  Text= '<%# Eval("materialTitle")%>' CssClass="label2" />                
               <asp:ImageButton CommandName="deleteRM" CommandArgument='<%# Eval("materialId")%>'
                            ID="ImageButton2" CssClass="rightButton " runat="server" ImageUrl="~/images/delete2_icon.png"  OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
           </div>           
        </ItemTemplate>
      </asp:Repeater>

<%-- practical section  --%>  
    <asp:Button Text='Practical'  runat="server" ID="btnPractical" CssClass="button1" OnClick="btnPractical_Click"  />

    <%-- normal repeater  --%>
    <asp:Repeater ID="rptPrac" runat="server" OnItemCommand="rmRepeater_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# Eval("materialTitle")%>'  runat="server" ID="btnCourse" CssClass="button2"  />
            
        </ItemTemplate>
    </asp:Repeater>
    
    <%-- delete repeater  --%>
    <asp:Repeater ID="rptDelPrac" runat="server"  Visible="False" OnItemCommand="rptDeleteRM_ItemCommand" >
        <ItemTemplate>
           <div class="deleteRpt">
               <asp:Label ID="lblPrac" runat="server"  Text= '<%# Eval("materialTitle")%>' CssClass="label2" />                
               <asp:ImageButton CommandName="deleteRM" CommandArgument='<%# Eval("materialId")%>'
                            ID="ImageButton2" CssClass="rightButton" runat="server" ImageUrl="~/images/delete2_icon.png"  OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
           </div>           
        </ItemTemplate>
      </asp:Repeater>

<%-- tutorial section  --%>  
    <asp:Button Text='Tutorial'  runat="server" ID="btnTutorial" CssClass="button1" OnClick="btnTutorial_Click"  />
    
    <%-- normal repeater  --%>
    <asp:Repeater ID="rptTut" runat="server" OnItemCommand="rmRepeater_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# Eval("materialTitle")%>'  runat="server" ID="btnCourse" CssClass="button2"  />
            
        </ItemTemplate>
    </asp:Repeater>

    <%-- delete repeater  --%>
    <asp:Repeater ID="rptDelTut" runat="server"  Visible="False" OnItemCommand="rptDeleteRM_ItemCommand" >
        <ItemTemplate>
           <div class="deleteRpt">
               <asp:Label ID="lblTut" runat="server"  Text= '<%# Eval("materialTitle")%>' CssClass="label2"/>                
               <asp:ImageButton CommandName="deleteRM" CommandArgument='<%# Eval("materialId")%>'
                            ID="ImageButton2" CssClass="rightButton" runat="server" ImageUrl="~/images/delete2_icon.png"  OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
           </div>           
        </ItemTemplate>
      </asp:Repeater>

<%-- other section  --%>
    <asp:Button Text='Other'  runat="server" ID="btnOther" CssClass="button1" OnClick="btnOther_Click"  />
 
<%-- normal repeater  --%>
    <asp:Repeater ID="rptOth" runat="server" OnItemCommand="rmRepeater_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# Eval("materialTitle")%>'  runat="server" ID="btnCourse" CssClass="button2"  />
            
        </ItemTemplate>
    </asp:Repeater> 

<%-- delete repeater  --%>
    <asp:Repeater ID="rptDelOth" runat="server"  Visible="False" OnItemCommand="rptDeleteRM_ItemCommand" >
        <ItemTemplate>
           <div class="label2">
               <asp:Label ID="lblOth" runat="server"  Text= '<%# Eval("materialTitle")%>' />                
               <asp:ImageButton CommandName="deleteRM" CommandArgument='<%# Eval("materialId")%>'
                            ID="ImageButton2" CssClass="rightButton" runat="server" ImageUrl="~/images/delete2_icon.png"  OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
           </div>           
        </ItemTemplate>
      </asp:Repeater>
   </div>
</asp:Content>
