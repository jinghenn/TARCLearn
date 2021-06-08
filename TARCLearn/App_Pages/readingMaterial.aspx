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
                            <asp:Button CssClass="btn btn-success" runat="server"  ValidationGroup="Add Form" Text="Save" />
                        </div>

                    </div>

                </div>
            </div>
        </div>

    <div class="main main-raised" >

      <div class="label1">
         <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">Reading Material</asp:Label>   
         <asp:ImageButton ID="btnDeleteRM" CssClass="rightButton" runat="server" ImageUrl="~/images/delete_icon.png" OnClick="btnDeleteRM_Click"   /> 
         <asp:ImageButton ID="btnAddRM" CssClass="rightButton" runat="server" ImageUrl="~/images/add_icon.png"  data-toggle="modal" data-target="#modalForm" OnClientClick="return false;" />
      </div>
     
      <asp:Button Text='Lecture'  runat="server" ID="btnLecture" CssClass="button1" OnClick="btnLecture_Click"  />
   
      <asp:Repeater ID="rptLect" runat="server" OnItemCommand="rmRepeater_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# Eval("materialTitle")%>'  runat="server" ID="btnCourse" CssClass="button2"  />
            
        </ItemTemplate>
      </asp:Repeater>

      <asp:Repeater ID="rptDelLect" runat="server"  Visible="False" OnItemCommand="rptDeleteRM_ItemCommand" >
        <ItemTemplate>
           <div class="label1">
               <asp:Label ID="lblLec" runat="server"  Text= '<%# Eval("materialTitle")%>' />                
               <asp:ImageButton CommandName="deleteRM" CommandArgument='<%# Eval("materialId")%>'
                            ID="ImageButton2" CssClass="rightButton" runat="server" ImageUrl="~/images/delete2_icon.png"  OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
           </div>           
        </ItemTemplate>
      </asp:Repeater>


  
    <asp:Button Text='Practical'  runat="server" ID="btnPractical" CssClass="button1" OnClick="btnPractical_Click"  />
   
    <asp:Repeater ID="rptPrac" runat="server" OnItemCommand="rmRepeater_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# Eval("materialTitle")%>'  runat="server" ID="btnCourse" CssClass="button2"  />
            
        </ItemTemplate>
    </asp:Repeater>

    <asp:Repeater ID="rptDelPrac" runat="server"  Visible="False" OnItemCommand="rptDeleteRM_ItemCommand" >
        <ItemTemplate>
           <div class="label1">
               <asp:Label ID="lblPrac" runat="server"  Text= '<%# Eval("materialTitle")%>' />                
               <asp:ImageButton CommandName="deleteRM" CommandArgument='<%# Eval("materialId")%>'
                            ID="ImageButton2" CssClass="rightButton" runat="server" ImageUrl="~/images/delete2_icon.png"  OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
           </div>           
        </ItemTemplate>
      </asp:Repeater>

    <asp:Button Text='Tutorial'  runat="server" ID="btnTutorial" CssClass="button1" OnClick="btnTutorial_Click"  />
   
    <asp:Repeater ID="rptTut" runat="server" OnItemCommand="rmRepeater_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# Eval("materialTitle")%>'  runat="server" ID="btnCourse" CssClass="button2"  />
            
        </ItemTemplate>
    </asp:Repeater> 
    <asp:Repeater ID="rptDelTut" runat="server"  Visible="False" OnItemCommand="rptDeleteRM_ItemCommand" >
        <ItemTemplate>
           <div class="label1">
               <asp:Label ID="lblTut" runat="server"  Text= '<%# Eval("materialTitle")%>' />                
               <asp:ImageButton CommandName="deleteRM" CommandArgument='<%# Eval("materialId")%>'
                            ID="ImageButton2" CssClass="rightButton" runat="server" ImageUrl="~/images/delete2_icon.png"  OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
           </div>           
        </ItemTemplate>
      </asp:Repeater>

    <asp:Button Text='Other'  runat="server" ID="btnOther" CssClass="button1" OnClick="btnOther_Click"  />
   
    <asp:Repeater ID="rptOth" runat="server" OnItemCommand="rmRepeater_ItemCommand" Visible="False" >
        <ItemTemplate>
            <asp:Button CommandName="selectRM" CommandArgument='<%# Eval("materialId")%>' 
                Text='<%# Eval("materialTitle")%>'  runat="server" ID="btnCourse" CssClass="button2"  />
            
        </ItemTemplate>
    </asp:Repeater> 
    <asp:Repeater ID="rptDelOth" runat="server"  Visible="False" OnItemCommand="rptDeleteRM_ItemCommand" >
        <ItemTemplate>
           <div class="label1">
               <asp:Label ID="lblOth" runat="server"  Text= '<%# Eval("materialTitle")%>' />                
               <asp:ImageButton CommandName="deleteRM" CommandArgument='<%# Eval("materialId")%>'
                            ID="ImageButton2" CssClass="rightButton" runat="server" ImageUrl="~/images/delete2_icon.png"  OnClientClick='return confirm("Are you sure you want to delete this item?");'/> 
           </div>           
        </ItemTemplate>
      </asp:Repeater>
        </div>
</asp:Content>
