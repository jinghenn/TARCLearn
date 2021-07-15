using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TARCLearn.App_Pages
{
    public partial class quiz : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string chapterId = Request.QueryString["chapterId"];

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection quizCon = new SqlConnection(providerConStr);
                quizCon.Open();

                //select data to be bound
                String strSelectCourseQuiz = "Select quizTitle AS quizTitle, quizId AS quizId from Quiz Where chapterId =@chapterId;";
                SqlCommand cmdSelectQuiz = new SqlCommand(strSelectCourseQuiz, quizCon);
                cmdSelectQuiz.Parameters.AddWithValue("@chapterId", chapterId);

                quizRepeater.DataSource = cmdSelectQuiz.ExecuteReader();
                quizRepeater.DataBind();

                SqlCommand cmdGetCourseId = new SqlCommand("Select courseId from [dbo].[Chapter] where chapterId=@chapterId;", quizCon);
                cmdGetCourseId.Parameters.AddWithValue("@chapterId", chapterId);
                string courseId = Convert.ToString(cmdGetCourseId.ExecuteScalar());

                lblHome.Text = "<a href = 'course.aspx'> Home </a>";
                lblChp.Text = "<a href = 'Chapter.aspx?courseId=" + courseId + "'> Chapter </a>";

                String userType = Session["userType"].ToString();
                if (userType == "Lecturer")
                {
                    //select data to be bound for delete repeater
                    String strDelQuiz = "Select quizTitle AS quizTitle, quizId AS quizId from Quiz Where chapterId =@chapterId;";
                    SqlCommand cmdDelQuiz = new SqlCommand(strDelQuiz, quizCon);
                    cmdDelQuiz.Parameters.AddWithValue("@chapterId", chapterId);

                    rptDeleteQuiz.DataSource = cmdDelQuiz.ExecuteReader();
                    rptDeleteQuiz.DataBind();

                    
                }
                else
                {
                    btnMore.Visible = false;
                    btnAdd.Visible = false;
                }
                quizCon.Close();
            }
        }

        protected void rptDeleteQuiz_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string chapterId = Request.QueryString["chapterId"];
            
            String quizId = e.CommandArgument.ToString();

            TextBox txtQuizTitle = (TextBox)e.Item.FindControl("txtQuizTitle");

            ImageButton btnEdit = (ImageButton)e.Item.FindControl("btnEdit");
            ImageButton btnSave = (ImageButton)e.Item.FindControl("btnSave");
            ImageButton btnCancel = (ImageButton)e.Item.FindControl("btnCancel");
            ImageButton btnDelete = (ImageButton)e.Item.FindControl("btnDelete");

            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection quizCon = new SqlConnection(providerConStr);
            quizCon.Open();

            if (e.CommandName == "delete")
            {
                String strDelQues = "DELETE FROM Quiz WHERE quizId=@quizId ;";
                SqlCommand cmdDelQues = new SqlCommand(strDelQues, quizCon);
                cmdDelQues.Parameters.AddWithValue("@quizId", quizId);
                cmdDelQues.ExecuteNonQuery();

                quizCon.Close();

                String url = "quiz.aspx?chapterId=" + chapterId;
                Response.Redirect(url);
            }
            if (e.CommandName == "edit")
            {
                txtQuizTitle.Enabled = true;
                txtQuizTitle.BorderStyle = BorderStyle.Inset;
                txtQuizTitle.BackColor = Color.White;

                btnEdit.Visible = false;               
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDelete.Visible = false;

            }
            if (e.CommandName == "save")
            {
                if (Page.IsValid)
                {

                    txtQuizTitle.Enabled = true;
                    txtQuizTitle.BorderStyle = BorderStyle.Inset;
                    txtQuizTitle.BackColor = Color.White;

                    btnEdit.Visible = true;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnDelete.Visible = true;

                    SqlCommand cmdSelect = new SqlCommand("Select * from [dbo].[Quiz] where quizTitle=@quizTitle", quizCon);
                    cmdSelect.Parameters.AddWithValue("@quizTitle", txtQuizTitle.Text);
                    SqlDataReader dtr = cmdSelect.ExecuteReader();



                    if (!dtr.HasRows)
                    {
                        String edit = "UPDATE [dbo].[Quiz] SET quizTitle = @quizTitle WHERE quizId = @quizId";
                        SqlCommand cmdEdit = new SqlCommand(edit, quizCon);
                        cmdEdit.Parameters.AddWithValue("@quizTitle", txtQuizTitle.Text);
                        cmdEdit.Parameters.AddWithValue("@quizId", quizId);

                        cmdEdit.ExecuteNonQuery();

                        quizCon.Close();
                        String url = "quiz.aspx?chapterId=" + chapterId;
                        Response.Redirect(url);
                    }
                    else if (dtr.HasRows)
                    {
                        Response.Write("<script>alert('Entered Quiz Title Already Exists.')</script>");

                    }




                }

            }
            if (e.CommandName == "cancel")
            {

                String url = "quiz.aspx?chapterId=" + chapterId;
                Response.Redirect(url);

            }
          
        }

        protected void quizRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String quizId = e.CommandArgument.ToString();
            if (e.CommandName == "select")
            {
                String url = "QuestionList.aspx?quizId=" + quizId;
                Response.Redirect(url);

            }
        }

        protected void addQuizFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string quizTitle = formQuizTitle.Text;              
                string chapterId = Request.QueryString["chapterId"];

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection quizCon = new SqlConnection(providerConStr);
                quizCon.Open();

                SqlCommand cmdSelect = new SqlCommand("Select * from [dbo].[Quiz] where quizTitle=@quizTitle", quizCon);
                cmdSelect.Parameters.AddWithValue("@quizTitle", quizTitle);
                SqlDataReader dtr = cmdSelect.ExecuteReader();



                if (!dtr.HasRows)                
                {
                    String add = "INSERT INTO [dbo].[Quiz] VALUES(@quizTitle,@chapterId);";
                    SqlCommand cmdAdd = new SqlCommand(add, quizCon);

                    cmdAdd.Parameters.AddWithValue("@quizTitle", quizTitle);
                    cmdAdd.Parameters.AddWithValue("@chapterId", chapterId);
                    
                    cmdAdd.ExecuteNonQuery();
                    quizCon.Close();

                    String url = "quiz.aspx?chapterId=" + chapterId;
                    Response.Redirect(url);
                }               
                else if (dtr.HasRows)
                {
                    Response.Write("<script>alert('Quiz Title Arealdy Exist.')</script>");
                }
                


            }



        }

        protected void btnMore_Click(object sender, ImageClickEventArgs e)
        {
            if (rptDeleteQuiz.Visible == false)
            {
                quizRepeater.Visible = false;
                rptDeleteQuiz.Visible = true;
            }
            else
            {
                string chapterId = Request.QueryString["chapterId"];
                String url = "quiz.aspx?chapterId=" + chapterId;
                Response.Redirect(url);
            }
        }
    }
}