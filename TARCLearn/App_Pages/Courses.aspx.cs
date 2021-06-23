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
using System.Drawing;

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
            TextBox txtCourseCode = (TextBox)e.Item.FindControl("txtCourseCode");
            TextBox txtCourseTitle = (TextBox)e.Item.FindControl("txtCourseTitle");

            LinkButton btnEdit = (LinkButton)e.Item.FindControl("btnEdit");
            LinkButton btnSave = (LinkButton)e.Item.FindControl("btnSave");
            LinkButton btnCancel = (LinkButton)e.Item.FindControl("btnCancel");
            LinkButton btnDel = (LinkButton)e.Item.FindControl("btnDelete");

           
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection courseCon = new SqlConnection(providerConStr);
            courseCon.Open();

            SqlCommand cmdGetCourseCode = new SqlCommand("Select courseCode from [dbo].[Course] where courseId=@courseId", courseCon);
            cmdGetCourseCode.Parameters.AddWithValue("@courseId", courseId);
            String currentCourseCode = Convert.ToString(cmdGetCourseCode.ExecuteScalar());

            SqlCommand cmdGetCourseTitle = new SqlCommand("Select courseTitle from [dbo].[Course] where courseId=@courseId", courseCon);
            cmdGetCourseTitle.Parameters.AddWithValue("@courseId", courseId);
            String currentCourseTitle = Convert.ToString(cmdGetCourseTitle.ExecuteScalar());

            if (e.CommandName == "edit")
            {        
                txtCourseCode.Enabled = true;
                txtCourseCode.BorderStyle = BorderStyle.Inset;
                txtCourseCode.BackColor = Color.White;

                txtCourseTitle.Enabled = true;
                txtCourseTitle.BorderStyle = BorderStyle.Inset;
                txtCourseTitle.BackColor = Color.White;

                btnEdit.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDel.Visible = true;
                

            }
            if(e.CommandName == "delete")
            {
                String strDelCourse = "DELETE FROM Enrolment WHERE courseId=@courseId ";
                SqlCommand cmdDelCourse = new SqlCommand(strDelCourse, courseCon);
                cmdDelCourse.Parameters.AddWithValue("@courseId", courseId);
                cmdDelCourse.ExecuteNonQuery();
                courseCon.Close();
                Response.Redirect("Courses.aspx");
            }
            if (e.CommandName == "save")
            {
                if (Page.IsValid)
                {

                    txtCourseCode.Enabled = true;
                    txtCourseCode.BorderStyle = BorderStyle.Inset;
                    txtCourseCode.BackColor = Color.White;

                    txtCourseTitle.Enabled = true;
                    txtCourseTitle.BorderStyle = BorderStyle.Inset;
                    txtCourseTitle.BackColor = Color.White;


                    btnEdit.Visible = true;
                    btnCancel.Visible = false;
                    btnDel.Visible = false;
                    btnSave.Visible = false;

                    SqlCommand cmdSelectCourseCode = new SqlCommand("Select * from [dbo].[Course] where courseCode=@courseCode", courseCon);
                    cmdSelectCourseCode.Parameters.AddWithValue("@courseCode", txtCourseCode.Text);
                    SqlDataReader dtrCourseCode = cmdSelectCourseCode.ExecuteReader();

                    SqlCommand cmdSelectCourseTitle = new SqlCommand("Select * from [dbo].[Course] where courseTitle=@courseTitle", courseCon);                   
                    cmdSelectCourseTitle.Parameters.AddWithValue("@courseTitle", txtCourseTitle.Text);
                    SqlDataReader dtrCourseTitle = cmdSelectCourseTitle.ExecuteReader();

                    if (!dtrCourseCode.HasRows && !dtrCourseTitle.HasRows)
                    {
                        String editCourse = "UPDATE [dbo].[Course] SET courseCode = @newCourseCode, courseTitle = @newCourseTitle WHERE courseId = @courseId";
                        SqlCommand cmdEditCourse = new SqlCommand(editCourse, courseCon);
                        cmdEditCourse.Parameters.AddWithValue("@newCourseCode", txtCourseCode.Text);
                        cmdEditCourse.Parameters.AddWithValue("@newCourseTitle", txtCourseTitle.Text);
                        cmdEditCourse.Parameters.AddWithValue("@courseId", courseId);
                        cmdEditCourse.ExecuteNonQuery();
                        courseCon.Close();
                        Response.Redirect("Courses.aspx");
                    }
                    else if (dtrCourseCode.HasRows && dtrCourseTitle.HasRows)
                    {
                        Response.Write("<script>alert('Both Entered Course Code and Course Title Already Exists.')</script>");

                    }
                    else if(dtrCourseCode.HasRows && (txtCourseCode.Text == currentCourseCode) && !dtrCourseTitle.HasRows)
                    {
                        String editCourse = "UPDATE [dbo].[Course] SET courseTitle = @newCourseTitle WHERE courseId = @courseId";
                        SqlCommand cmdEditCourse = new SqlCommand(editCourse, courseCon);                        
                        cmdEditCourse.Parameters.AddWithValue("@newCourseTitle", txtCourseTitle.Text);
                        cmdEditCourse.Parameters.AddWithValue("@courseId", courseId);
                        cmdEditCourse.ExecuteNonQuery();
                        courseCon.Close();
                        Response.Redirect("Courses.aspx");
                    }
                    else if (dtrCourseCode.HasRows && (txtCourseCode.Text != currentCourseCode) && !dtrCourseTitle.HasRows)
                    {
                        Response.Write("<script>alert('Entered Course Code Already Exists.')</script>");

                    }
                    else if (!dtrCourseCode.HasRows && (txtCourseTitle.Text == currentCourseTitle) && dtrCourseTitle.HasRows)
                    {
                        String editCourse = "UPDATE [dbo].[Course] SET courseCode = @newCourseCode WHERE courseId = @courseId";
                        SqlCommand cmdEditCourse = new SqlCommand(editCourse, courseCon);
                        cmdEditCourse.Parameters.AddWithValue("@newCourseCode", txtCourseCode.Text);
                        cmdEditCourse.Parameters.AddWithValue("@courseId", courseId);
                        cmdEditCourse.ExecuteNonQuery();
                        courseCon.Close();
                        Response.Redirect("Courses.aspx");
                    }
                    else if (!dtrCourseCode.HasRows && (txtCourseTitle.Text != currentCourseTitle) && dtrCourseTitle.HasRows)
                    {
                        Response.Write("<script>alert('Entered Course Title Already Exists.')</script>");

                    }



                }
               
            }
            if (e.CommandName == "cancel")
            {           

                txtCourseCode.Enabled = false;
                txtCourseCode.BorderStyle = BorderStyle.None;
                txtCourseCode.BackColor = Color.Transparent;

                txtCourseTitle.Enabled = false;
                txtCourseTitle.BorderStyle = BorderStyle.None;
                txtCourseTitle.BackColor = Color.Transparent;


                btnEdit.Visible = true;
                btnCancel.Visible = false;
                btnDel.Visible = false;
                btnSave.Visible = false;

                String strDelCourse = "Select c.courseTitle AS courseTitle, c.courseId AS courseId, c.courseCode AS courseCode from Course c, Enrolment e Where c.courseId = e.courseId and e.userId =@userId;";
                SqlCommand cmdDelCourse = new SqlCommand(strDelCourse, courseCon);
                cmdDelCourse.Parameters.AddWithValue("@userId", Session["userId"].ToString());

                rptDeleteCourse.DataSource = cmdDelCourse.ExecuteReader();
                rptDeleteCourse.DataBind();

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

                SqlCommand cmdSelectCourse = new SqlCommand("Select * from [dbo].[Enrolment] where userId=@userId and courseId=@courseId", courseCon);
                cmdSelectCourse.Parameters.AddWithValue("@userId", Session["userId"].ToString());
                cmdSelectCourse.Parameters.AddWithValue("@courseId", courseEnrol);
                SqlDataReader dtrCourse = cmdSelectCourse.ExecuteReader();

                if (!dtrCourse.HasRows)
                {
                    String addEnrolment = "INSERT INTO [dbo].[Enrolment] VALUES(@userId,@courseId);";
                    SqlCommand cmdAddEnrolment = new SqlCommand(addEnrolment, courseCon);

                    cmdAddEnrolment.Parameters.AddWithValue("@userId", Session["userId"].ToString());
                    cmdAddEnrolment.Parameters.AddWithValue("@courseId", courseEnrol);
                    cmdAddEnrolment.ExecuteNonQuery();
                    courseCon.Close();
                    //courseRepeater.DataBind();
                    Response.Redirect("Courses.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Selected Course Already Enrolled.')</script>");
                    
                }

                    


                    

            }
            
                
            
        }

      

      

    }
    
}