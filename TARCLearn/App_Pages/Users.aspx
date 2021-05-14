<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="TARCLearn.App_Pages.Users" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            User id:
            <asp:TextBox ID="txtUserId" runat="server"></asp:TextBox>
            <br />
            Password:
            <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
            <br />
            Username:
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
            <br />
            isLecturer<asp:RadioButtonList ID="rblIsLecturer" runat="server">
                <asp:ListItem>True</asp:ListItem>
                <asp:ListItem Selected="True">False</asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Button" />
        </div>
    </form>
</body>
</html>
