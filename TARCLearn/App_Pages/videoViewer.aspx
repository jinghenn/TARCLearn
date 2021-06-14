<%@ Page Title="" Language="C#" MasterPageFile="~/App_Pages/TARCLearn.Master" AutoEventWireup="true" CodeBehind="videoViewer.aspx.cs" Inherits="TARCLearn.App_Pages.videoViewer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Repeater ID="viewVideoRepeater" runat="server" >
       <ItemTemplate>
          <video autoplay loop controls="controls" style="height: 680px; width: 100%; position: relative; ">
      <source src="<%# "../videos/"+(Eval("materialName"))%>" type="video/mp4" /></video>
       </ItemTemplate>
    </asp:Repeater>
    
</asp:Content>
