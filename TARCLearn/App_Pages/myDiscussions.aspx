<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="myDiscussions.aspx.cs" Inherits="TARCLearn.App_Pages.myDiscussions" %>
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
    <div class="label1">
      <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">DiscussionThread</asp:Label> 
    </div>
    
    <asp:Repeater ID="rptDis" runat="server" OnItemCommand="courseRepeater_ItemCommand" >
       <ItemTemplate>
          <asp:Button CommandName="selectDiscussion" CommandArgument='<%# Eval("threadId")%>' 
                Text= '<%# Eval("threadTitle")%>' 
                runat="server" ID="btnCourse" CssClass="button1"  />
            
       </ItemTemplate>
   </asp:Repeater>    

   

        
      
   
</asp:Content>
