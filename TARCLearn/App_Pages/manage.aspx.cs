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
    public partial class manage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection manageCon = new SqlConnection(providerConStr);
                manageCon.Open();

                SqlCommand cmdDel = new SqlCommand("SELECT * FROM [dbo].[Course]", manageCon);
                SqlDataAdapter sdaDel = new SqlDataAdapter(cmdDel);
                DataTable dtDel = new DataTable();
                sdaDel.Fill(dtDel);
                formddlCourse.DataSource = dtDel;
                formddlCourse.DataTextField = "courseTitle";
                formddlCourse.DataValueField = "courseId";
                formddlCourse.DataBind();

                SqlCommand cmdEdit = new SqlCommand("SELECT * FROM [dbo].[Course]", manageCon);
                SqlDataAdapter sdaEdit = new SqlDataAdapter(cmdEdit);
                DataTable dtEdit = new DataTable();
                sdaEdit.Fill(dtEdit);
                formddlEditCourse.DataSource = dtEdit;
                formddlEditCourse.DataTextField = "courseTitle";
                formddlEditCourse.DataValueField = "courseId";
                formddlEditCourse.DataBind();

                SqlCommand cmdEnrolCourse = new SqlCommand("SELECT * FROM [dbo].[Course]", manageCon);
                SqlDataAdapter sdaEnrolCourse = new SqlDataAdapter(cmdEnrolCourse);
                DataTable dtEnrolCourse = new DataTable();
                sdaEnrolCourse.Fill(dtEnrolCourse);
                formddlMCourse.DataSource = dtEnrolCourse;
                formddlMCourse.DataTextField = "courseTitle";
                formddlMCourse.DataValueField = "courseId";
                formddlMCourse.DataBind();

                SqlCommand cmdMStudent = new SqlCommand("SELECT * FROM [dbo].[User] WHERE isLecturer=@isLecturer ORDER BY username", manageCon);
                cmdMStudent.Parameters.AddWithValue("@isLecturer", "False");
                SqlDataAdapter sdaMStudent = new SqlDataAdapter(cmdMStudent);
                DataTable dtMStudent = new DataTable();
                sdaMStudent.Fill(dtMStudent);
                formddlMStudent.DataSource = dtMStudent;
                formddlMStudent.DataTextField = "username";
                formddlMStudent.DataValueField = "userId";
                formddlMStudent.DataBind();
            }
        }

        protected void btnManageCourses_Click(object sender, EventArgs e)
        {
            if (btnCreateCourse.Visible == false)
            {
                btnCreateCourse.Visible = true;
                btnDeleteCourse.Visible = true;
                btnEditCourse.Visible = true;
            }
            else
            {
                btnCreateCourse.Visible = false;
                btnDeleteCourse.Visible = false;
                btnEditCourse.Visible = false;
            }
        }

        protected void btnManageStudent_Click(object sender, EventArgs e)
        {
            if (btnEnrolStudent.Visible == false)
            {
                btnEnrolStudent.Visible = true;
                btnDropStudent.Visible = true;

            }
            else
            {
                btnEnrolStudent.Visible = false;
                btnDropStudent.Visible = false;

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
                SqlConnection manageCon = new SqlConnection(providerConStr);
                manageCon.Open();

                SqlCommand cmdSelectCourseCode = new SqlCommand("Select * from [dbo].[Course] where courseCode=@courseCode", manageCon);
                cmdSelectCourseCode.Parameters.AddWithValue("@courseCode", courseCode);
                SqlDataReader dtrCourseCode = cmdSelectCourseCode.ExecuteReader();

                SqlCommand cmdSelectCourseTitle = new SqlCommand("Select * from [dbo].[Course] where courseTitle=@courseTitle", manageCon);
                cmdSelectCourseTitle.Parameters.AddWithValue("@courseTitle", courseTitle);
                SqlDataReader dtrCourseTitle = cmdSelectCourseTitle.ExecuteReader();

                if (!dtrCourseCode.HasRows && !dtrCourseTitle.HasRows)
                {
                    String createCourse = "INSERT INTO [dbo].[Course] VALUES(@courseCode, @courseTitle, @courseDescription); ";
                    SqlCommand cmdCreateCourse = new SqlCommand(createCourse, manageCon);
                    cmdCreateCourse.Parameters.AddWithValue("@courseCode", courseCode);
                    cmdCreateCourse.Parameters.AddWithValue("@courseTitle", courseTitle);
                    cmdCreateCourse.Parameters.AddWithValue("@courseDescription", courseDesc);
                    cmdCreateCourse.ExecuteNonQuery();
                    manageCon.Close();
                    Response.Redirect("manage.aspx");
                }
                else if (dtrCourseCode.HasRows && dtrCourseTitle.HasRows)
                {
                    Response.Write("<script>alert('Both Entered Course Code and Course Title Already Exists.')</script>");

                }
                else if (dtrCourseCode.HasRows)
                {
                    Response.Write("<script>alert('Entered Course Code Already Exists.')</script>");

                }
                else if (dtrCourseTitle.HasRows)
                {
                    Response.Write("<script>alert('Entered Course Title Already Exists.')</script>");

                }



            }
        }
        protected void deleteCourseFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string courseDeleteId = formddlCourse.SelectedValue;

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection manageCon = new SqlConnection(providerConStr);
                manageCon.Open();

                SqlCommand cmdSelectCourse = new SqlCommand("Select * from [dbo].[Course] WHERE courseId=@courseId", manageCon);
                cmdSelectCourse.Parameters.AddWithValue("@courseId", courseDeleteId);
                SqlDataReader dtrCourse = cmdSelectCourse.ExecuteReader();

                if (dtrCourse.HasRows)
                {

                    String delCourse = "DELETE FROM Enrolment WHERE courseId = @courseId;";
                    SqlCommand cmdDelCourse = new SqlCommand(delCourse, manageCon);

                    cmdDelCourse.Parameters.AddWithValue("@courseId", courseDeleteId);
                    cmdDelCourse.ExecuteNonQuery();
                    manageCon.Close();

                    Response.Redirect("Manage.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Selected Course Does Not Exists.')</script>");

                }
            }
        }

        protected void editCourseFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                string courseCode = formEditCourseCode.Text;
                string courseTitle = formEditCourseTitle.Text;
                string courseEditId = formddlEditCourse.SelectedValue;
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
                SqlConnection manageCon = new SqlConnection(providerConStr);
                manageCon.Open();

                SqlCommand cmdSelectCourseCode = new SqlCommand("Select * from [dbo].[Course] where courseCode=@courseCode", manageCon);
                cmdSelectCourseCode.Parameters.AddWithValue("@courseCode", courseCode);
                SqlDataReader dtrCourseCode = cmdSelectCourseCode.ExecuteReader();

                SqlCommand cmdSelectCourseTitle = new SqlCommand("Select * from [dbo].[Course] where courseTitle=@courseTitle", manageCon);
                cmdSelectCourseTitle.Parameters.AddWithValue("@courseTitle", courseTitle);
                SqlDataReader dtrCourseTitle = cmdSelectCourseTitle.ExecuteReader();

                if (!dtrCourseCode.HasRows && !dtrCourseTitle.HasRows)
                {
                    String editCourse = "UPDATE [dbo].[Course] SET courseCode = @newCourseCode, courseTitle = @newCourseTitle, courseDescription=@newCourseDescription WHERE courseId = @courseId";
                    SqlCommand cmdEditCourse = new SqlCommand(editCourse, manageCon);
                    cmdEditCourse.Parameters.AddWithValue("@newCourseCode", courseCode);
                    cmdEditCourse.Parameters.AddWithValue("@newCourseTitle", courseTitle);
                    cmdEditCourse.Parameters.AddWithValue("@newCourseDescription", courseDesc);
                    cmdEditCourse.Parameters.AddWithValue("@courseId", courseEditId);
                    cmdEditCourse.ExecuteNonQuery();
                    manageCon.Close();
                    Response.Redirect("manage.aspx");
                }
                else if (dtrCourseCode.HasRows && dtrCourseTitle.HasRows)
                {
                    Response.Write("<script>alert('Both Entered Course Code and Course Title Already Exists.')</script>");

                }
                else if (dtrCourseCode.HasRows)
                {
                    Response.Write("<script>alert('Entered Course Code Already Exists.')</script>");

                }
                else if (dtrCourseTitle.HasRows)
                {
                    Response.Write("<script>alert('Entered Course Title Already Exists.')</script>");

                }



            }
        }

        protected void manageStudentFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string courseESId = formddlMCourse.SelectedValue;
                string studentESId = formddlMStudent.SelectedValue;

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection manageCon = new SqlConnection(providerConStr);
                manageCon.Open();

                if (Session["manageStudent"].ToString() == "enrol")
                {
                    SqlCommand cmdSelectCourse = new SqlCommand("Select * from [dbo].[Enrolment] where userId=@userId and courseId=@courseId", manageCon);
                    cmdSelectCourse.Parameters.AddWithValue("@userId", studentESId);
                    cmdSelectCourse.Parameters.AddWithValue("@courseId", courseESId);
                    SqlDataReader dtrCourse = cmdSelectCourse.ExecuteReader();

                    if (!dtrCourse.HasRows)
                    {
                        String addEnrolment = "INSERT INTO [dbo].[Enrolment] VALUES(@userId,@courseId);";
                        SqlCommand cmdAddEnrolment = new SqlCommand(addEnrolment, manageCon);

                        cmdAddEnrolment.Parameters.AddWithValue("@userId", studentESId);
                        cmdAddEnrolment.Parameters.AddWithValue("@courseId", courseESId);
                        cmdAddEnrolment.ExecuteNonQuery();
                        manageCon.Close();
                        //courseRepeater.DataBind();
                        Response.Redirect("manage.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('Selected Course Already Enrolled by this User .')</script>");

                    }
                }else if (Session["manageStudent"].ToString() == "drop")
                {
                    String strDrop = "DELETE FROM Enrolment WHERE courseId=@courseId AND userId=@userId";
                    SqlCommand cmdDrop = new SqlCommand(strDrop, manageCon);
                    cmdDrop.Parameters.AddWithValue("@userId", studentESId);
                    cmdDrop.Parameters.AddWithValue("@courseId", courseESId);
                    cmdDrop.ExecuteNonQuery();
                    manageCon.Close();
                    Response.Redirect("manage.aspx");

                }

            }
        }

        protected void btnDropStudent_Click(object sender, EventArgs e)
        {
            Session["manageStudent"] = "drop";
            lblMTitle.Text = "Enrol Student";
            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
        }

        protected void btnEnrolStudent_Click(object sender, EventArgs e)
        {
            Session["manageStudent"] = "enrol";
            lblMTitle.Text = "Drop Student";
            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
        }
    }
}