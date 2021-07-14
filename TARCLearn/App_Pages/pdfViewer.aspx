<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="pdfViewer.aspx.cs" Inherits="TARCLearn.App_Pages.ReadingMaterial" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .pdfView {
            width: 100%;
            height:639px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <nav aria-label="breadcrumb" style="--bs-breadcrumb-divider: '>'; padding: 10px 0 0 30px; height:41px; background-color:#F5F5F5;">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><asp:Label ID="lblHome" runat="server"/></li>
             <li class="breadcrumb-item"><asp:Label ID="lblChp" runat="server"/></li>
            <li class="breadcrumb-item active" aria-current="page"><asp:Label ID="lblMaterial" runat="server"/></li>
          </ol>
        </nav>
    <asp:Repeater ID="viewPDFRepeater" runat="server" >
       <ItemTemplate>
          <embed  src="<%# "../ReadingMaterials/"+(Eval("materialName"))%>"  class="pdfView" id="pdfViewer">
       </ItemTemplate>
    </asp:Repeater>
</asp:Content>
