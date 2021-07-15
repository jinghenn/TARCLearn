<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="materialViewer.aspx.cs" Inherits="TARCLearn.App_Pages.materialViewer" %>
<%@ Register TagPrefix="GleamTech" Namespace="GleamTech.DocumentUltimate" Assembly="GleamTech.DocumentUltimate" %>
<%@ Register TagPrefix="GleamTech" Namespace="GleamTech.DocumentUltimate.AspNet.WebForms" Assembly="GleamTech.DocumentUltimate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
     <nav aria-label="breadcrumb" style="--bs-breadcrumb-divider: '>'; padding: 10px 0 0 30px; height:41px; background-color:#F5F5F5;">
       <ol class="breadcrumb">
         <li class="breadcrumb-item"><asp:Label ID="lblHome" runat="server"/></li>
         <li class="breadcrumb-item"><asp:Label ID="lblChp" runat="server"/></li>
         <li class="breadcrumb-item"><asp:Label ID="lblMaterial" runat="server"/></li>
         <li class="breadcrumb-item active" aria-current="page"><asp:Label ID="lblMaterialName" runat="server"/></li>
       </ol>
     </nav>
     
     <GleamTech:DocumentViewerControl runat="server" Width="100%"   Height="639"   ID="docViewer" />      

 
  
    
</asp:Content>
