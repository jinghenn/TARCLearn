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

                SqlCommand cmdEnrolCourse = new SqlCommand("SELECT * FROM Course c, Enrolment e WHERE c.courseId=e.courseId AND e.userId =@userId", manageCon);
                cmdEnrolCourse.Parameters.AddWithValue("@userId", Session["userId"].ToString());
                SqlDataAdapter sdaEnrolCourse = new SqlDataAdapter(cmdEnrolCourse);
                DataTable dtEnrolCourse = new DataTable();
                sdaEnrolCourse.Fill(dtEnrolCourse);
                formddlMCourse.DataSource = dtEnrolCourse;
                formddlMCourse.DataTextField = "courseTitle";
                formddlMCourse.DataValueField = "courseId";
                formddlMCourse.DataBind();

                SqlCommand cmdMStudent = new SqlCommand("SELECT * FROM [dbo].[User] ORDER BY username", manageCon);
                SqlDataAdapter sdaMStudent = new SqlDataAdapter(cmdMStudent);
                DataTable dtMStudent = new DataTable();
                sdaMStudent.Fill(dtMStudent);
                formddlMStudent.DataSource = dtMStudent;
                formddlMStudent.DataTextField = "username";
                formddlMStudent.DataValueField = "userId";
                formddlMStudent.DataBind();

                manageCon.Close();
            }
        }               

        protected void manageStudentFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string courseId = formddlMCourse.SelectedValue;
                string studentId = formddlMStudent.SelectedValue;

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection manageCon = new SqlConnection(providerConStr);
                manageCon.Open();

                SqlCommand cmdSelectCourse = new SqlCommand("Select * from [dbo].[Enrolment] where userId=@userId and courseId=@courseId", manageCon);
                cmdSelectCourse.Parameters.AddWithValue("@userId", studentId);
                cmdSelectCourse.Parameters.AddWithValue("@courseId", courseId);
                SqlDataReader dtrCourse = cmdSelectCourse.ExecuteReader();

                if (Session["manageStudent"].ToString() == "enrol")
                {                  
                    if (!dtrCourse.HasRows)
                    {
                        String addEnrolment = "INSERT INTO [dbo].[Enrolment] VALUES(@userId,@courseId);";
                        SqlCommand cmdAddEnrolment = new SqlCommand(addEnrolment, manageCon);

                        cmdAddEnrolment.Parameters.AddWithValue("@userId", studentId);
                        cmdAddEnrolment.Parameters.AddWithValue("@courseId", courseId);
                        cmdAddEnrolment.ExecuteNonQuery();
                        manageCon.Close();
                        //courseRepeater.DataBind();
                        Response.Write("<script>alert('Succecful Enrol Selected User .')</script>");
                        
                    }
                    else
                    {
                        manageCon.Close();
                        Response.Write("<script>alert('Selected Course Already Enrolled by this User .')</script>");
                    }
                }else if (Session["manageStudent"].ToString() == "drop")
                {
                    if (dtrCourse.HasRows)
                    {
                        String strDrop = "DELETE FROM Enrolment WHERE courseId=@courseId AND userId=@userId";
                        SqlCommand cmdDrop = new SqlCommand(strDrop, manageCon);
                        cmdDrop.Parameters.AddWithValue("@userId", studentId);
                        cmdDrop.Parameters.AddWithValue("@courseId", courseId);
                        cmdDrop.ExecuteNonQuery();
                        manageCon.Close();
                        Response.Write("<script>alert('Succecful Drop Selected User.')</script>");
                        
                    }
                    else
                    {
                        manageCon.Close();
                        Response.Write("<script>alert('Selected Course Does not Enrolled by this User .')</script>");
                    }
                    
                }

            }
        }

        protected void btnDrop_Click(object sender, EventArgs e)
        {
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection manageCon = new SqlConnection(providerConStr);
            manageCon.Open();

            SqlCommand cmdCheckCourse = new SqlCommand("SELECT * FROM Course c, Enrolment e WHERE c.courseId=e.courseId AND e.userId =@userId", manageCon);
            cmdCheckCourse.Parameters.AddWithValue("@userId", Session["userId"].ToString());
            SqlDataReader dtrCheckCourse = cmdCheckCourse.ExecuteReader();

            if (dtrCheckCourse.HasRows)
            {
                Session["manageStudent"] = "drop";
                lblMTitle.Text = "Enrol Student";
                manageCon.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                manageCon.Close();
                Response.Write("<script>alert('You does not enrol in any course. Please create or enrol yourself in a course to perform this action.')</script>");
            }
        }

        protected void btnEnrol_Click(object sender, EventArgs e)
        {
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection manageCon = new SqlConnection(providerConStr);
            manageCon.Open();

            SqlCommand cmdCheckCourse = new SqlCommand("SELECT * FROM Course c, Enrolment e WHERE c.courseId=e.courseId AND e.userId =@userId", manageCon);
            cmdCheckCourse.Parameters.AddWithValue("@userId", Session["userId"].ToString());
            SqlDataReader dtrCheckCourse = cmdCheckCourse.ExecuteReader();

            if (dtrCheckCourse.HasRows)
            {
                Session["manageStudent"] = "enrol";
                lblMTitle.Text = "Drop Student";
                manageCon.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                manageCon.Close();
                Response.Write("<script>alert('You does not enrol in any course. Please create or enrol yourself in a course to perform this action.')</script>");
            }
        }
    }
}