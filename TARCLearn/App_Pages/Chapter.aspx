<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="Chapter.aspx.cs" Inherits="TARCLearn.App_Pages.Chapter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Css/cssTARCLearn.css" /> 
     <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-eOJMYsd53ii+scO/bJGFsiCZc+5NDVN2yr8+0RDqr0Ql0h+rP48ckxlpbzKgwra6" crossorigin="anonymous">   
    <link rel="icon" href="favicon.ico">  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblTittle" runat="server" Width="100%" Font-Bold="True" Font-Size="Large" Height="70px" Justify-Content="center" >Chapter</asp:Label>
    <asp:Repeater ID="chpRepeater" runat="server" OnItemCommand="chapterRepeater_ItemCommand" >
        <ItemTemplate>
            <asp:Button ID="Button1" CommandName="selectChp" CommandArgument='<%# Eval("chpId")%>' Text= '<%# Eval("chpTitle")%>'  class=" button" runat="server"/>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
