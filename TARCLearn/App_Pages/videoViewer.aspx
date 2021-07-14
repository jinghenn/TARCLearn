<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="videoViewer.aspx.cs" Inherits="TARCLearn.App_Pages.videoViewer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <nav aria-label="breadcrumb" style="--bs-breadcrumb-divider: '>'; padding: 10px 0 0 30px; height:41px; background-color:#F5F5F5;">
       <ol class="breadcrumb">
         <li class="breadcrumb-item"><asp:Label ID="lblHome" runat="server"/></li>
         <li class="breadcrumb-item"><asp:Label ID="lblChp" runat="server"/></li>
         <li class="breadcrumb-item"><asp:Label ID="lblMaterial" runat="server"/></li>
         <li class="breadcrumb-item active" aria-current="page"><asp:Label ID="lblMaterialName" runat="server"/></li>
       </ol>
     </nav>

    <asp:Repeater ID="viewVideoRepeater" runat="server" >
       <ItemTemplate>
          <video autoplay loop controls="controls" style="height: 639px; width: 100%; position: relative; ">
            <source src="<%# "../videos/"+(Eval("materialName"))%>" type='video/mp4' />
              Your browser does not support HTML5 video.
          </video>
       </ItemTemplate>
    </asp:Repeater>
    
</asp:Content>
