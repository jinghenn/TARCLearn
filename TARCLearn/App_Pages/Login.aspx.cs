using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

namespace TARCLearn.App_Pages
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void loginFormBtn_Click(object sender, EventArgs e)
        {
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection loginCon = new SqlConnection(providerConStr);
            loginCon.Open();
            SqlCommand cmdSelectUser = new SqlCommand("Select * from [dbo].[User] where userId=@userId and password=@password", loginCon);
            cmdSelectUser.Parameters.AddWithValue("@userId", txtUserId.Text);
            cmdSelectUser.Parameters.AddWithValue("@password", txtPassword.Text);
            SqlDataReader dtrUser = cmdSelectUser.ExecuteReader();

            if (dtrUser.HasRows)
            {

                SqlCommand cmdGetUserId = new SqlCommand("Select userId from [dbo].[User] where userId=@userId and password=@password", loginCon);
                cmdGetUserId.Parameters.AddWithValue("@userId", txtUserId.Text);
                cmdGetUserId.Parameters.AddWithValue("@password", txtPassword.Text);
                String userId = Convert.ToString(cmdGetUserId.ExecuteScalar());

                SqlCommand cmdGetPass = new SqlCommand("Select password from [dbo].[User] where userId=@userId and password=@password", loginCon);
                cmdGetPass.Parameters.AddWithValue("@userId", txtUserId.Text);
                cmdGetPass.Parameters.AddWithValue("@password", txtPassword.Text);
                String password = Convert.ToString(cmdGetPass.ExecuteScalar());


                if (userId.CompareTo(txtUserId.Text) == 0 && password.CompareTo(txtPassword.Text) == 0)
                {
                    String strUserName = "Select username from [dbo].[User] where userId=@userId";
                    SqlCommand cmdGetUsername = new SqlCommand(strUserName, loginCon);
                    cmdGetUsername.Parameters.AddWithValue("@userId", txtUserId.Text);
                    Session["username"] = Convert.ToString(cmdGetUsername.ExecuteScalar());

                    String strUserType = "Select isLecturer from [dbo].[User] where userId=@userId";
                    SqlCommand cmdGetUserType = new SqlCommand(strUserType, loginCon);
                    cmdGetUserType.Parameters.AddWithValue("@userId", txtUserId.Text);
                    String userType;
                    if (Convert.ToBoolean(cmdGetUserType.ExecuteScalar()))
                    {
                        userType = "Lecturer";
                    }
                    else
                    {
                        userType = "Student";
                    }
                    Session["usertype"] = userType;
                    Session["userId"] = userId;

                    Response.Redirect("~/App_Pages/course.aspx");
                }
                else
                {
                    lblLoginFail.Text = "Invalid Password. Check Caps Lock.";
                }
            }
            else
            {
                lblLoginFail.Text = "Invalid User ID or Password";
            }           
            loginCon.Close();
    
        }
    }
}