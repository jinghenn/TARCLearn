<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TARCLearn.App_Pages.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<link rel="stylesheet" type="text/css" href="../Css/cssLogin.css" />
    <title>TARC Learn</title>
</head>
<body>
	
    <div class="whiteboxPosition">
		<div>
			<asp:Image ID="Image1" class="image" runat="server" ImageUrl="~/image/TARC.png"/>
		
			<span class= "test">
			TARC LEARN
			</span>

			<div class="loginWhiteboxSize">
				<form id="loginform" runat="server" class="form">
					<span class="text1 ">
						Login
					</span>
					<asp:Label ID="lblLoginFail" class="text2" style="color: red;" runat="server"></asp:Label>
					<span class="text2">
						<br />
						User ID
						<asp:RequiredFieldValidator ID="rsfLUserId" runat="server" ControlToValidate="txtUserId" Display="Dynamic" ErrorMessage="User ID is required." Font-Size="8px" ForeColor="Red">*User ID is required.</asp:RequiredFieldValidator>
					</span>
					<div class="wrapInput1">
                        <asp:TextBox ID="txtUserId" runat="server" class="input" ></asp:TextBox>
                     </div>
					
					<span class="text2">
						Password
						</span>
                    <asp:RequiredFieldValidator ID="rsfLPassword" runat="server" ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="Password is required." Font-Size="8px" ForeColor="Red">*Password is required.</asp:RequiredFieldValidator>
                	<div class="wrapInput2">
						<asp:TextBox ID="txtPassword" runat="server" class="input" TextMode="Password"></asp:TextBox>
						
					</div>						

				       <asp:Button ID="loginFormBtn" runat="server" Text="Login" class="formBtn" OnClick="loginFormBtn_Click" />  						
				</form>
			</div>
		</div>
	</div>
	
</body>
</html>


		
        
	


