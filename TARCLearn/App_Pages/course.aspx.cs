using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TARCLearn.App_Pages
{
    public partial class course : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userType = Session["userType"].ToString();
                if (userType == null)
                {
                    System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
                    string scriptKey = "ErrorMessage";

                    javaScript.Append("var userConfirmation = window.confirm('" + "Your Session has Expired, Please login again.');\n");
                    javaScript.Append("window.location='Login.aspx';");

                    ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
                }

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection courseCon = new SqlConnection(providerConStr);
                courseCon.Open();

                //select data to be bound
                String strSelectCourse = "Select c.courseDescription AS courseDesc, c.courseTitle AS courseTitle, c.courseId AS courseId, c.courseCode AS courseCode from Course c, Enrolment e Where c.courseId = e.courseId and e.userId =@userId;";
                SqlCommand cmdSelectCourse = new SqlCommand(strSelectCourse, courseCon);
                cmdSelectCourse.Parameters.AddWithValue("@userId", Session["userId"].ToString());

                rptCourse.DataSource = cmdSelectCourse.ExecuteReader();
                rptCourse.DataBind();
                
                if (userType == "Lecturer")
                {
                    //select data to be bound for delete repeater
                    String strDelCourse = "Select c.courseTitle AS courseTitle, c.courseId AS courseId, c.courseCode AS courseCode from Course c, Enrolment e Where c.courseId = e.courseId and e.userId =@userId;";
                    SqlCommand cmdDelCourse = new SqlCommand(strDelCourse, courseCon);
                    cmdDelCourse.Parameters.AddWithValue("@userId", Session["userId"].ToString());

                    rptManageCourse.DataSource = cmdDelCourse.ExecuteReader();
                    rptManageCourse.DataBind();
                   
                }
                else
                {
                    btnMore.Visible = false;
                    btnCreate.Visible = false;
                }
                courseCon.Close();
            }
        }

        public void successMsg(string msg)
        {
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
            string scriptKey = "SuccessMessage";
            string url = "course.aspx";

            javaScript.Append("var userConfirmation = window.confirm('" + "Successfully " + msg + "');\n");
            javaScript.Append("window.location='" + url + "';");

            ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
        }

        protected void btnMore_Click(object sender, ImageClickEventArgs e)
        {
            if (rptManageCourse.Visible == false)
            {
                rptCourse.Visible = false;
                rptManageCourse.Visible = true;
            }
            else
            {
                Response.Redirect("course.aspx");
            }
        }

        protected void rptCourse_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string courseId = e.CommandArgument.ToString();
            if (e.CommandName == "select")
            {
                string url = "Chapter.aspx?courseId=" + courseId;
                Response.Redirect(url);

            }
        }

        protected void rptManageCourse_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string courseId = e.CommandArgument.ToString();

            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection courseCon = new SqlConnection(providerConStr);
            courseCon.Open();

            if (e.CommandName == "delete")
            {
                SqlCommand cmdSelectCourse = new SqlCommand("Select * from [dbo].[Course] WHERE courseId=@courseId", courseCon);
                cmdSelectCourse.Parameters.AddWithValue("@courseId", courseId);
                SqlDataReader dtrCourse = cmdSelectCourse.ExecuteReader();

                if (dtrCourse.HasRows)
                {

                    String delCourse = "DELETE FROM Enrolment WHERE courseId = @courseId;";
                    SqlCommand cmdDelCourse = new SqlCommand(delCourse, courseCon);

                    cmdDelCourse.Parameters.AddWithValue("@courseId", courseId);
                    cmdDelCourse.ExecuteNonQuery();
                    courseCon.Close();

                    successMsg("deleted");
                                      
                }
                else
                {
                    courseCon.Close();
                    Response.Write("<script>alert('Selected Course Does Not Exists.')</script>");

                }
                
            }
            if (e.CommandName == "edit")
            {
                Session["courseId"] = courseId;
                SqlCommand cmdGetCourseCode = new SqlCommand("Select courseCode from [dbo].[Course] where courseId=@courseId", courseCon);
                cmdGetCourseCode.Parameters.AddWithValue("@courseId", courseId);
                formEditCourseCode.Text = Convert.ToString(cmdGetCourseCode.ExecuteScalar());

                SqlCommand cmdGetCourseTitle = new SqlCommand("Select courseTitle from [dbo].[Course] where courseId=@courseId", courseCon);
                cmdGetCourseTitle.Parameters.AddWithValue("@courseId", courseId);
                formEditCourseTitle.Text = Convert.ToString(cmdGetCourseTitle.ExecuteScalar());

                SqlCommand cmdGetCourseDesc = new SqlCommand("Select courseDescription from [dbo].[Course] where courseId=@courseId", courseCon);
                cmdGetCourseDesc.Parameters.AddWithValue("@courseId", courseId);
                formEditCourseDesc.Text = Convert.ToString(cmdGetCourseDesc.ExecuteScalar());
                courseCon.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "editCourse();", true);
            }
        }

        protected void editCourseFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string courseId = Session["courseId"].ToString();
                string newCourseCode = formEditCourseCode.Text;
                string newCourseTitle = formEditCourseTitle.Text;                
                string newCourseDesc;

                if (formCourseDesc.Text != null)
                {
                    newCourseDesc = formEditCourseDesc.Text;
                }
                else
                {
                    newCourseDesc = null;
                }

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection courseCon = new SqlConnection(providerConStr);
                courseCon.Open();

                SqlCommand cmdGetCourseCode = new SqlCommand("Select courseCode from [dbo].[Course] where courseId=@courseId", courseCon);
                cmdGetCourseCode.Parameters.AddWithValue("@courseId", courseId);
                string currentCourseCode = Convert.ToString(cmdGetCourseCode.ExecuteScalar());

                SqlCommand cmdGetCourseTitle = new SqlCommand("Select courseTitle from [dbo].[Course] where courseId=@courseId", courseCon);
                cmdGetCourseTitle.Parameters.AddWithValue("@courseId", courseId);
                string currentCourseTitle = Convert.ToString(cmdGetCourseTitle.ExecuteScalar());

                SqlCommand cmdSelectCourseCode = new SqlCommand("Select * from [dbo].[Course] where courseCode=@courseCode", courseCon);
                cmdSelectCourseCode.Parameters.AddWithValue("@courseCode", newCourseCode);
                SqlDataReader dtrCourseCode = cmdSelectCourseCode.ExecuteReader();

                SqlCommand cmdSelectCourseTitle = new SqlCommand("Select * from [dbo].[Course] where courseTitle=@courseTitle", courseCon);
                cmdSelectCourseTitle.Parameters.AddWithValue("@courseTitle", newCourseTitle);
                SqlDataReader dtrCourseTitle = cmdSelectCourseTitle.ExecuteReader();

                SqlCommand cmdSelectCourseDesc = new SqlCommand("Select * from [dbo].[Course] where courseDescription=@courseDescription AND courseId=@courseId", courseCon);
                cmdSelectCourseDesc.Parameters.AddWithValue("@courseDescription", newCourseDesc);
                cmdSelectCourseDesc.Parameters.AddWithValue("@courseId", courseId);
                SqlDataReader dtrCourseDesc = cmdSelectCourseDesc.ExecuteReader();

                if (!dtrCourseCode.HasRows && !dtrCourseTitle.HasRows)
                {
                    String editCourse = "UPDATE [dbo].[Course] SET courseCode = @newCourseCode, courseTitle = @newCourseTitle, courseDescription = @newCourseDescription WHERE courseId = @courseId";
                    SqlCommand cmdEditCourse = new SqlCommand(editCourse, courseCon);
                    cmdEditCourse.Parameters.AddWithValue("@newCourseCode", newCourseCode);
                    cmdEditCourse.Parameters.AddWithValue("@newCourseTitle", newCourseTitle);
                    cmdEditCourse.Parameters.AddWithValue("@newCourseDescription", newCourseDesc);
                    cmdEditCourse.Parameters.AddWithValue("@courseId", courseId);
                    cmdEditCourse.ExecuteNonQuery();
                    courseCon.Close();
                    successMsg("updated");
                }
                else if (dtrCourseCode.HasRows && dtrCourseTitle.HasRows)
                {
                    if (!dtrCourseDesc.HasRows)
                    {
                        String editCourse = "UPDATE [dbo].[Course] SET courseDescription = @newCourseDescription WHERE courseId = @courseId";
                        SqlCommand cmdEditCourse = new SqlCommand(editCourse, courseCon);
                        cmdEditCourse.Parameters.AddWithValue("@newCourseDescription", newCourseDesc);
                        cmdEditCourse.Parameters.AddWithValue("@courseId", courseId);
                        cmdEditCourse.ExecuteNonQuery();
                        courseCon.Close();
                        successMsg("updated");
                    }
                    else
                    {
                        courseCon.Close();
                        Response.Write("<script>alert('Both Entered Course Code and Course Title Already Exists.')</script>");
                    }
                    
                }
                else if (dtrCourseCode.HasRows && (newCourseCode == currentCourseCode) && !dtrCourseTitle.HasRows)
                {
                    String editCourse = "UPDATE [dbo].[Course] SET courseTitle = @newCourseTitle, courseDescription = @newCourseDescription WHERE courseId = @courseId";
                    SqlCommand cmdEditCourse = new SqlCommand(editCourse, courseCon);
                    cmdEditCourse.Parameters.AddWithValue("@newCourseTitle", newCourseTitle);
                    cmdEditCourse.Parameters.AddWithValue("@newCourseDescription", newCourseDesc);
                    cmdEditCourse.Parameters.AddWithValue("@courseId", courseId);
                    cmdEditCourse.ExecuteNonQuery();
                    courseCon.Close();
                    successMsg("updated");
                }
                else if (dtrCourseCode.HasRows && (newCourseCode != currentCourseCode) && !dtrCourseTitle.HasRows)
                {
                    courseCon.Close();
                    Response.Write("<script>alert('Entered Course Code Already Exists.')</script>");

                }
                else if (!dtrCourseCode.HasRows && (newCourseTitle == currentCourseTitle) && dtrCourseTitle.HasRows)
                {
                    String editCourse = "UPDATE [dbo].[Course] SET courseCode = @newCourseCode, courseDescription = @newCourseDescription WHERE courseId = @courseId";
                    SqlCommand cmdEditCourse = new SqlCommand(editCourse, courseCon);
                    cmdEditCourse.Parameters.AddWithValue("@newCourseCode", newCourseCode);
                    cmdEditCourse.Parameters.AddWithValue("@newCourseDescription", newCourseDesc);
                    cmdEditCourse.Parameters.AddWithValue("@courseId", courseId);
                    cmdEditCourse.ExecuteNonQuery();
                    courseCon.Close();
                    successMsg("updated");
                }
                else if (!dtrCourseCode.HasRows && (newCourseTitle != currentCourseTitle) && dtrCourseTitle.HasRows)
                {
                    courseCon.Close();
                    Response.Write("<script>alert('Entered Course Title Already Exists.')</script>");

                }

            }
        }

        protected void createCourseFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                string courseCode = formCourseCode.Text;
                string courseTitle = formCourseTitle.Text;

                string courseDesc;
                if (formCourseDesc.Text != null)
                {
                    courseDesc = formCourseDesc.Text;
                }
                else
                {
                    courseDesc = null;
                }


                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection courseCon = new SqlConnection(providerConStr);
                courseCon.Open();

                SqlCommand cmdSelectCourseCode = new SqlCommand("Select * from [dbo].[Course] where courseCode=@courseCode", courseCon);
                cmdSelectCourseCode.Parameters.AddWithValue("@courseCode", courseCode);
                SqlDataReader dtrCourseCode = cmdSelectCourseCode.ExecuteReader();

                SqlCommand cmdSelectCourseTitle = new SqlCommand("Select * from [dbo].[Course] where courseTitle=@courseTitle", courseCon);
                cmdSelectCourseTitle.Parameters.AddWithValue("@courseTitle", courseTitle);
                SqlDataReader dtrCourseTitle = cmdSelectCourseTitle.ExecuteReader();

                if (!dtrCourseCode.HasRows && !dtrCourseTitle.HasRows)
                {
                    String createCourse = "INSERT INTO [dbo].[Course] VALUES(@courseCode, @courseTitle, @courseDescription); ";
                    SqlCommand cmdCreateCourse = new SqlCommand(createCourse, courseCon);
                    cmdCreateCourse.Parameters.AddWithValue("@courseCode", courseCode);
                    cmdCreateCourse.Parameters.AddWithValue("@courseTitle", courseTitle);
                    cmdCreateCourse.Parameters.AddWithValue("@courseDescription", courseDesc);
                    cmdCreateCourse.ExecuteNonQuery();

                    SqlCommand cmdGetCourseId = new SqlCommand("Select courseId from [dbo].[Course] where courseCode=@courseCode", courseCon);
                    cmdGetCourseId.Parameters.AddWithValue("@courseCode", courseCode);
                    string courseId = Convert.ToString(cmdGetCourseId.ExecuteScalar());

                    String enrolCourse = "INSERT INTO [dbo].[Enrolment] VALUES(@userId, @courseId); ";
                    SqlCommand cmdEnrolCourse = new SqlCommand(enrolCourse, courseCon);
                    cmdEnrolCourse.Parameters.AddWithValue("@userId", Session["userId"].ToString());
                    cmdEnrolCourse.Parameters.AddWithValue("@courseId", courseId);
                    cmdEnrolCourse.ExecuteNonQuery();

                    courseCon.Close();
                    successMsg("added");
                }
                else if (dtrCourseCode.HasRows && dtrCourseTitle.HasRows)
                {
                    courseCon.Close();
                    Response.Write("<script>alert('Both Entered Course Code and Course Title Already Exists.')</script>");

                }
                else if (dtrCourseCode.HasRows)
                {
                    courseCon.Close();
                    Response.Write("<script>alert('Entered Course Code Already Exists.')</script>");

                }
                else if (dtrCourseTitle.HasRows)
                {
                    courseCon.Close();
                    Response.Write("<script>alert('Entered Course Title Already Exists.')</script>");

                }

            }
        }
    }
}