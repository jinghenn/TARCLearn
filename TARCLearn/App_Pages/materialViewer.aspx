<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="materialViewer.aspx.cs" Inherits="TARCLearn.App_Pages.materialViewer" %>
<%@ Register TagPrefix="GleamTech" Namespace="GleamTech.DocumentUltimate" Assembly="GleamTech.DocumentUltimate" %>
<%@ Register TagPrefix="GleamTech" Namespace="GleamTech.DocumentUltimate.AspNet.WebForms" Assembly="GleamTech.DocumentUltimate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >  
     
          <GleamTech:DocumentViewerControl runat="server" Width="100%"   Height="680"   ID="docViewer" />      

 
  
    
</asp:Content>
