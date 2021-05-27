<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="Chapter.aspx.cs" Inherits="TARCLearn.App_Pages.Chapter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
      
        .button1 {
            background-color: white;
            text-align: left;
            border-radius: 0px;
            transition-duration: 0.4s;
            width:100%; 
            height:70px; 
            border-width:0px; 
            padding-left:20px;  
            margin-left:10px;
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
            width:100%; 
            height:70px; 
            border-width:0px; 
            padding-left:40px;  
            margin-left:10px;
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
    </style>
     <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-eOJMYsd53ii+scO/bJGFsiCZc+5NDVN2yr8+0RDqr0Ql0h+rP48ckxlpbzKgwra6" crossorigin="anonymous">   
    <link rel="icon" href="favicon.ico">  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="label1">
    <asp:Label ID="lblTittle" runat="server" Font-Bold="true" Font-Size="Large">Chapter</asp:Label>
    </div>
    <asp:Repeater ID="chpRepeater" runat="server" OnItemCommand="chapterRepeater_ItemCommand" >
        <ItemTemplate>
            <asp:Button ID="btnChp" CommandName="selectChp" CommandArgument='<%# Eval("chpId")%>' Text= '<%# Eval("chpTitle")%>'  CssClass="button1" runat="server"/>           
            <asp:Button ID="btnRM" CommandName="selectRM" Text= "Reading Material"  CssClass="button2" runat="server" Visible="False"/>
            <asp:Button ID="btnVideo" CommandName="selectVideo" Text= "Videos"  CssClass="button2" runat="server" Visible="False"/>
            <asp:Button ID="btnDis" CommandName="selectDis" Text= "Discussion"  CssClass="button2" runat="server" Visible="False"/>
            <asp:Button ID="btnQuiz" CommandName="selectQuiz" Text= "Quiz"  CssClass="button2" runat="server" Visible="False"/>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
