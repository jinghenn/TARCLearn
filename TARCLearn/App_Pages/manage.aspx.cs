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

                

                manageCon.Close();
            }
        }               

        protected void manageStudentFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string courseId = formddlMCourse.SelectedValue;
                string email = formtxtMStudent.Text;
                List<string> failList = new List<string>();
                List<string> invalidEmailList = new List<string>();
                char[] delimiterChars = { ' ', ','};
                string[] emailList = email.Split(delimiterChars);

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection manageCon = new SqlConnection(providerConStr);
                manageCon.Open();


                for (int i = 0; i < emailList.Length; i++)
                {
                    //get StudentId 
                    SqlCommand cmdGetStudentId = new SqlCommand("Select userId from [dbo].[User] where email=@email", manageCon);
                    cmdGetStudentId.Parameters.AddWithValue("@email", emailList[i]);
                    string studentId = Convert.ToString(cmdGetStudentId.ExecuteScalar());

                    //check student enrol ard or not
                    SqlCommand cmdSelectCourse = new SqlCommand("Select * from [dbo].[Enrolment] where userId=@userId and courseId=@courseId", manageCon);
                    cmdSelectCourse.Parameters.AddWithValue("@userId", studentId);
                    cmdSelectCourse.Parameters.AddWithValue("@courseId", courseId);
                    SqlDataReader dtrCourse = cmdSelectCourse.ExecuteReader();

                    if (Session["manageStudent"].ToString() == "enrol")
                    {
                        if (!dtrCourse.HasRows && studentId != "")
                        {
                            String addEnrolment = "INSERT INTO [dbo].[Enrolment] VALUES(@userId,@courseId);";
                            SqlCommand cmdAddEnrolment = new SqlCommand(addEnrolment, manageCon);

                            cmdAddEnrolment.Parameters.AddWithValue("@userId", studentId);
                            cmdAddEnrolment.Parameters.AddWithValue("@courseId", courseId);
                            cmdAddEnrolment.ExecuteNonQuery();

                            //send email notify

                        } 
                        else if (studentId == "" && emailList[i] != "")
                        {
                            invalidEmailList.Add(emailList[i]);
                        }
                        else if(emailList[i] != "")
                        {
                            failList.Add(emailList[i]);
                        }
                    }
                    else if (Session["manageStudent"].ToString() == "drop")
                    {
                        if (dtrCourse.HasRows && studentId != null)
                        {
                            String strDrop = "DELETE FROM Enrolment WHERE courseId=@courseId AND userId=@userId";
                            SqlCommand cmdDrop = new SqlCommand(strDrop, manageCon);
                            cmdDrop.Parameters.AddWithValue("@userId", studentId);
                            cmdDrop.Parameters.AddWithValue("@courseId", courseId);
                            cmdDrop.ExecuteNonQuery();

                            //send email notify

                        }
                        else if (studentId == "" && emailList[i] != "")
                        {
                            invalidEmailList.Add(emailList[i]);
                        }
                        else if (emailList[i] != "")
                        {
                            failList.Add(emailList[i]);
                        }

                    }
                }
                manageCon.Close();
                if (!failList.Any() && !invalidEmailList.Any())
                {
                    if (Session["manageStudent"].ToString() == "enrol")
                    {
                        Response.Write("<script>alert('Succecful Enrol All Entered User.')</script>");
                    }
                    else if (Session["manageStudent"].ToString() == "drop")
                    {
                        Response.Write("<script>alert('Succecful Drop All Entered User.')</script>");
                    }                                      
                 
                }
                else
                {

                    if (Session["manageStudent"].ToString() == "enrol")
                    {
                        string respond = "<script>alert('Succecful Enrol All Entered User except " + failList.Count + " Already Enrolled User ";

                        for (int i = 0; i < failList.Count; i++)
                        {

                            if (i == failList.Count - 1)
                            {
                                respond += failList[i] + ". ";
                            }
                            else
                            {
                                respond += failList[i] + ", ";
                            }


                        }
                        

                        respond += " And " + invalidEmailList.Count + " Invalid Email ";

                        for (int i = 0; i < invalidEmailList.Count; i++)
                        {

                            if (i == invalidEmailList.Count - 1)
                            {
                                respond += invalidEmailList[i] + ". ";
                            }
                            else
                            {
                                respond += invalidEmailList[i] + ", ";
                            }


                        }
                        respond += "')</script>";

                        Response.Write(respond);

                    }
                    else if (Session["manageStudent"].ToString() == "drop")
                    {
                        string respond = "<script>alert('Succecful Drop All Entered User except " + failList.Count + " Does not Enrolled User ";

                        for (int i = 0; i < failList.Count; i++)
                        {

                            if (i == failList.Count - 1)
                            {
                                respond += failList[i] + ". ";
                            }
                            else
                            {
                                respond += failList[i] + ", ";
                            }


                        }


                        respond += " And " + invalidEmailList.Count + " Invalid Email ";

                        for (int i = 0; i < invalidEmailList.Count; i++)
                        {

                            if (i == invalidEmailList.Count - 1)
                            {
                                respond += invalidEmailList[i] + ". ";
                            }
                            else
                            {
                                respond += invalidEmailList[i] + ", ";
                            }


                        }
                        respond += "')</script>";

                        Response.Write(respond);
                        
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
    }
}