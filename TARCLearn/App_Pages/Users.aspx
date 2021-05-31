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
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="materialId" DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:BoundField DataField="materialId" HeaderText="materialId" ReadOnly="True" SortExpression="materialId" InsertVisible="False" />
                    <asp:BoundField DataField="index" HeaderText="index" SortExpression="index" />
                    <asp:BoundField DataField="materialTitle" HeaderText="materialTitle" SortExpression="materialTitle" />
                    <asp:BoundField DataField="materialDescription" HeaderText="materialDescription" SortExpression="materialDescription" />
                    <asp:BoundField DataField="materialUrl" HeaderText="materialUrl" SortExpression="materialUrl" />
                    <asp:BoundField DataField="materialName" HeaderText="materialName" SortExpression="materialName" />
                    <asp:CheckBoxField DataField="isVideo" HeaderText="isVideo" SortExpression="isVideo" />
                    <asp:BoundField DataField="mode" HeaderText="mode" SortExpression="mode" />
                    <asp:BoundField DataField="chapterId" HeaderText="chapterId" SortExpression="chapterId" />
                </Columns>
            </asp:GridView>
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="chapterId" DataSourceID="SqlDataSource2">
                <Columns>
                    <asp:BoundField DataField="chapterId" HeaderText="chapterId" ReadOnly="True" SortExpression="chapterId" InsertVisible="False" />
                    <asp:BoundField DataField="chapterNo" HeaderText="chapterNo" SortExpression="chapterNo" />
                    <asp:BoundField DataField="courseId" HeaderText="courseId" SortExpression="courseId" />
                    <asp:BoundField DataField="chapterTitle" HeaderText="chapterTitle" SortExpression="chapterTitle" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TARCLearn.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework" ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM [Chapter]"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TARCLearn.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework" ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM [Material]"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
