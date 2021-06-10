using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data;

namespace TARCLearn.App_Pages
{
    public partial class MainPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection courseCon = new SqlConnection(providerConStr);
                courseCon.Open();
             
                //select data to be bound
                String strSelectCourse = "Select c.courseTitle AS courseTitle, c.courseId AS courseId, c.courseCode AS courseCode from Course c, Enrolment e Where c.courseId = e.courseId and e.userId =@userId;";
                SqlCommand cmdSelectCourse = new SqlCommand(strSelectCourse, courseCon);
                cmdSelectCourse.Parameters.AddWithValue("@userId", Session["userId"].ToString());

                courseRepeater.DataSource = cmdSelectCourse.ExecuteReader();
                courseRepeater.DataBind();

                

                String userType = Session["userType"].ToString();
                if (userType == "Lecturer")
                {
                    //select data to be bound for delete repeater
                    String strDelCourse = "Select c.courseTitle AS courseTitle, c.courseId AS courseId, c.courseCode AS courseCode from Course c, Enrolment e Where c.courseId = e.courseId and e.userId =@userId;";
                    SqlCommand cmdDelCourse = new SqlCommand(strDelCourse, courseCon);
                    cmdDelCourse.Parameters.AddWithValue("@userId", Session["userId"].ToString());

                    rptDeleteCourse.DataSource = cmdDelCourse.ExecuteReader();
                    rptDeleteCourse.DataBind(); 

                    SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Course]", courseCon);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    formddlCourse.DataSource = dt;
                    formddlCourse.DataTextField = "courseTitle";
                    formddlCourse.DataValueField = "courseId";
                    formddlCourse.DataBind();
                }
                else
                {
                    btnDeleteEnrolCourse.Visible = false;
                    btnEnrolCourse.Visible = false;
                }
                courseCon.Close();
            }

        }

        protected void courseRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String courseId = e.CommandArgument.ToString();
            if (e.CommandName == "selectCourse")
            {
                String url = "Chapter.aspx?courseId=" + courseId;
                Response.Redirect(url);
                
            }
        }

     

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {

            if (rptDeleteCourse.Visible == false)
            {
                courseRepeater.Visible = false;
                rptDeleteCourse.Visible = true;
            }
            else
            {
                Response.Redirect("Courses.aspx");
            }
        }

        protected void rptDeleteCourse_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String courseId = e.CommandArgument.ToString();
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection courseCon = new SqlConnection(providerConStr);
            courseCon.Open();
            if (e.CommandName == "deleteCourse")
            {
                String strDelCourse = "DELETE FROM Enrolment WHERE courseId=@courseId ";
                SqlCommand cmdDelCourse = new SqlCommand(strDelCourse, courseCon);
                cmdDelCourse.Parameters.AddWithValue("@courseId", courseId);
                cmdDelCourse.ExecuteNonQuery();
                courseCon.Close();
                Response.Redirect("Courses.aspx");

            }
        }

        protected void enrolCourseFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string courseEnrol = formddlCourse.SelectedValue;

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection courseCon = new SqlConnection(providerConStr);
                courseCon.Open();
                String addEnrolment = "INSERT INTO [dbo].[Enrolment] VALUES(@userId,@courseId);";
                SqlCommand cmdAddEnrolment = new SqlCommand(addEnrolment, courseCon);

                cmdAddEnrolment.Parameters.AddWithValue("@userId", Session["userId"].ToString());
                cmdAddEnrolment.Parameters.AddWithValue("@courseId", courseEnrol);
                cmdAddEnrolment.ExecuteNonQuery();
                courseCon.Close();
                //courseRepeater.DataBind();
                Response.Redirect("Courses.aspx");

            }
            
                
            
        }


    }
    
}