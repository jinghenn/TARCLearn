<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="pdfViewer.aspx.cs" Inherits="TARCLearn.App_Pages.ReadingMaterial" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .pdfView {
            width: 100%;
            height:680px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Repeater ID="viewPDFRepeater" runat="server" >
       <ItemTemplate>
          <embed  src="<%# "../ReadingMaterials/"+(Eval("materialName"))%>"  class="pdfView">
       </ItemTemplate>
    </asp:Repeater>
</asp:Content>
