using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
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
                Session["userNo"] = "0";
                string userId = Session["userId"].ToString();
                if (userId == null)
                {
                    System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
                    string scriptKey = "ErrorMessage";

                    javaScript.Append("var userConfirmation = window.confirm('" + "Your Session has Expired, Please login again.');\n");
                    javaScript.Append("window.location='Login.aspx';");

                    ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
                }

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

                ddlCourse.DataSource = dtEnrolCourse;
                ddlCourse.DataTextField = "courseTitle";
                ddlCourse.DataValueField = "courseId";
                ddlCourse.DataBind();

                //select data to be bound
                String strGetUser = "Select u.userId AS userId, u.username AS username, u.email AS email from [dbo].[User] u, [dbo].[Enrolment] e Where u.userId = e.userId AND courseId=@courseId;";
                SqlCommand cmdSelectQuiz = new SqlCommand(strGetUser, manageCon);
                cmdSelectQuiz.Parameters.AddWithValue("@courseId", ddlCourse.SelectedValue);

                rptUserList.DataSource = cmdSelectQuiz.ExecuteReader();
                rptUserList.DataBind();

                manageCon.Close();

                
                
            }
        }

        protected void rptUserList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            // determines which position in the outer layer repeater in the repeater (AlternatingItemTemplate, FooterTemplate,

            //HeaderTemplate，，ItemTemplate，SeparatorTemplate）
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblNo = (Label)e.Item.FindControl("lblNo");

                int userNo = Convert.ToInt32(Session["userNo"].ToString()) + 1;
                Session["userNo"] = userNo;
                lblNo.Text = Convert.ToString(userNo);
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
                List<string> lecturerFailList = new List<string>();
                char[] delimiterChars = { ' ', ','};
                string[] emailList = email.Split(delimiterChars);

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection manageCon = new SqlConnection(providerConStr);
                manageCon.Open();


                for (int i = 0; i < emailList.Length; i++)
                {
                    //get userId 
                    SqlCommand cmdGetuserId = new SqlCommand("Select userId from [dbo].[User] where email=@email", manageCon);
                    cmdGetuserId.Parameters.AddWithValue("@email", emailList[i]);
                    string userId = Convert.ToString(cmdGetuserId.ExecuteScalar());

                    //check student enrol ard or not
                    SqlCommand cmdSelectCourse = new SqlCommand("Select * from [dbo].[Enrolment] where userId=@userId and courseId=@courseId", manageCon);
                    cmdSelectCourse.Parameters.AddWithValue("@userId", userId);
                    cmdSelectCourse.Parameters.AddWithValue("@courseId", courseId);
                    SqlDataReader dtrCourse = cmdSelectCourse.ExecuteReader();

                    //get lecturer count
                    SqlCommand cmdGetLecCount = new SqlCommand("Select COUNT(e.userId) from [dbo].[Enrolment] e, [dbo].[User] u where u.userId=e.userId AND e.courseId=@courseId AND u.isLecturer=@isLecturer;", manageCon);
                    cmdGetLecCount.Parameters.AddWithValue("@isLecturer", true);
                    cmdGetLecCount.Parameters.AddWithValue("@courseId", courseId);
                    int availableLecture = Convert.ToInt32(cmdGetLecCount.ExecuteScalar());

                    //get user type
                    SqlCommand cmdGetUserType = new SqlCommand("Select isLecturer from [dbo].[User] where email=@email", manageCon);
                    cmdGetUserType.Parameters.AddWithValue("@email", emailList[i]);
                    Boolean dropUserType = Convert.ToBoolean(cmdGetUserType.ExecuteScalar());
                    

                    if (Session["manageStudent"].ToString() == "enrol")
                    {
                        if (!dtrCourse.HasRows && userId != "")
                        {
                            String addEnrolment = "INSERT INTO [dbo].[Enrolment] VALUES(@userId,@courseId);";
                            SqlCommand cmdAddEnrolment = new SqlCommand(addEnrolment, manageCon);

                            cmdAddEnrolment.Parameters.AddWithValue("@userId", userId);
                            cmdAddEnrolment.Parameters.AddWithValue("@courseId", courseId);
                            cmdAddEnrolment.ExecuteNonQuery();

                            //get course code                            
                            SqlCommand cmdGetCC = new SqlCommand("Select courseCode from [dbo].[Course] where courseId=@courseId", manageCon);
                            cmdGetCC.Parameters.AddWithValue("@courseId", courseId);
                            string courseCode = Convert.ToString(cmdGetCC.ExecuteScalar());

                            //get course Title                            
                            SqlCommand cmdGetCT = new SqlCommand("Select courseTitle from [dbo].[Course] where courseId=@courseId", manageCon);
                            cmdGetCT.Parameters.AddWithValue("@courseId", courseId);
                            string courseTitle = Convert.ToString(cmdGetCT.ExecuteScalar());

                            //send email notify
                            string to = emailList[i]; //To address    
                            string from = "tarclearn@gmail.com"; //From address    
                            MailMessage message = new MailMessage(from, to);                           

                            string mailbody = "You have been invite to new course " + courseCode + " " + courseTitle;
                            message.Subject = "Invited to New Course";
                            message.Body = mailbody;
                            message.BodyEncoding = Encoding.UTF8;
                            message.IsBodyHtml = true;
                            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                            System.Net.NetworkCredential basicCredential1 = new
                            System.Net.NetworkCredential("tarclearn@gmail.com", "tarclearn1122");
                            client.EnableSsl = true;
                            client.UseDefaultCredentials = false;
                            client.Credentials = basicCredential1;

                            try
                            {
                                client.Send(message);                               
                            }

                            catch (Exception ex)
                            {
                                throw ex;
                            }

                        } 
                        else if (userId == "" && emailList[i] != "")
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
                        if (dtrCourse.HasRows && userId != null)
                        {
                            if(dropUserType && availableLecture < 2)
                            {
                                lecturerFailList.Add(emailList[i]);
                            }
                            else
                            {
                                String strDrop = "DELETE FROM Enrolment WHERE courseId=@courseId AND userId=@userId";
                                SqlCommand cmdDrop = new SqlCommand(strDrop, manageCon);
                                cmdDrop.Parameters.AddWithValue("@userId", userId);
                                cmdDrop.Parameters.AddWithValue("@courseId", courseId);
                                cmdDrop.ExecuteNonQuery();

                                //send email notify
                                //get course code                            
                                SqlCommand cmdGetCC = new SqlCommand("Select courseCode from [dbo].[Course] where courseId=@courseId", manageCon);
                                cmdGetCC.Parameters.AddWithValue("@courseId", courseId);
                                string courseCode = Convert.ToString(cmdGetCC.ExecuteScalar());

                                //get course Title                            
                                SqlCommand cmdGetCT = new SqlCommand("Select courseTitle from [dbo].[Course] where courseId=@courseId", manageCon);
                                cmdGetCT.Parameters.AddWithValue("@courseId", courseId);
                                string courseTitle = Convert.ToString(cmdGetCT.ExecuteScalar());

                                //send email notify
                                string to = emailList[i]; //To address    
                                string from = "tarclearn@gmail.com"; //From address    
                                MailMessage message = new MailMessage(from, to);

                                string mailbody = "You have been drop from the course " + courseCode + " " + courseTitle;
                                message.Subject = "Dropped from a Course";
                                message.Body = mailbody;
                                message.BodyEncoding = Encoding.UTF8;
                                message.IsBodyHtml = true;
                                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                                System.Net.NetworkCredential basicCredential1 = new
                                System.Net.NetworkCredential("tarclearn@gmail.com", "tarclearn1122");
                                client.EnableSsl = true;
                                client.UseDefaultCredentials = false;
                                client.Credentials = basicCredential1;

                                try
                                {
                                    client.Send(message);
                                }

                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }                            
                        
                        }
                        else if (userId == "" && emailList[i] != "")
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
                if (!failList.Any() && !invalidEmailList.Any() && !lecturerFailList.Any())
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
                        string respond = "<script>alert('Succecful Drop All Entered User except";

                        if (lecturerFailList.Any())
                        {
                            respond += " " + lecturerFailList.Count + " lecturer (At least 1 lecturer must be in a course) ";
                            for (int i = 0; i < lecturerFailList.Count; i++)
                            {

                                if (i == lecturerFailList.Count - 1)
                                {
                                    respond += lecturerFailList[i] + ". ";
                                }
                                else
                                {
                                    respond += lecturerFailList[i] + ", ";
                                }


                            }
                        }
                        if (failList.Any())
                        {
                            respond += " " + failList.Count + " Does not Enrolled User ";

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
                        }
                        if (failList.Any())
                        {
                            respond += " " + invalidEmailList.Count + " Invalid Email ";

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
                        }                       
                                       
                        respond += "')</script>";

                        Response.Write(respond);
                        
                    }
                }
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
                formddlMCourse.SelectedValue = ddlCourse.SelectedValue;
                manageCon.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                manageCon.Close();
                Response.Write("<script>alert('You does not enrol in any course. Please create or enrol yourself in a course to perform this action.')</script>");
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
                formddlMCourse.SelectedValue = ddlCourse.SelectedValue;
                manageCon.Close();
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                manageCon.Close();
                Response.Write("<script>alert('You does not enrol in any course. Please create or enrol yourself in a course to perform this action.')</script>");
            }
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["userNo"] = "0";
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection manageCon = new SqlConnection(providerConStr);
            manageCon.Open();

            //select data to be bound
            String strGetUser = "Select u.userId AS userId, u.username AS username, u.email AS email from [dbo].[User] u, [dbo].[Enrolment] e Where u.userId = e.userId AND courseId=@courseId;";
            SqlCommand cmdSelectQuiz = new SqlCommand(strGetUser, manageCon);
            cmdSelectQuiz.Parameters.AddWithValue("@courseId", ddlCourse.SelectedValue);

            rptUserList.DataSource = cmdSelectQuiz.ExecuteReader();
            rptUserList.DataBind();

            manageCon.Close();
        }
    }
}