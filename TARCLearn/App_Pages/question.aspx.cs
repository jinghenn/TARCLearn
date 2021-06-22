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
    public partial class quiz : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["editQuiz"] == null)
                {
                    Session["editQuiz"] = "false";
                }
                if (Session["updateQues"] == null)
                {
                    Session["updateQues"] = "false";
                }
                string quizId = Request.QueryString["quizId"];
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection quizCon = new SqlConnection(providerConStr);
                quizCon.Open();

                //select data to be bound
                String strSelectQuiz = "Select question AS question, questionId AS questionId from Question Where quizId=@quizId;";
                SqlCommand cmdSelectQuiz = new SqlCommand(strSelectQuiz, quizCon);
                cmdSelectQuiz.Parameters.AddWithValue("@quizId", quizId);

                quizRepeater.DataSource = cmdSelectQuiz.ExecuteReader();
                quizRepeater.DataBind();

                String strSelectTitle = "Select quizTitle FROM Quiz Where quizId=@quizId;";
                SqlCommand cmdSelectTitle = new SqlCommand(strSelectTitle, quizCon);
                cmdSelectTitle.Parameters.AddWithValue("@quizId", quizId);
                lblTittle.Text = Convert.ToString(cmdSelectTitle.ExecuteScalar());

                quizCon.Close();


            }




        }
        protected void quizRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            // determines which position in the outer layer repeater in the repeater (AlternatingItemTemplate, FooterTemplate,

            //HeaderTemplate，，ItemTemplate，SeparatorTemplate）
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                RadioButtonList rbl1 = (RadioButtonList)e.Item.FindControl("rbl1");
                ImageButton btnDeleteQues = (ImageButton)e.Item.FindControl("btnDeleteQues");
                ImageButton btnEditQues = (ImageButton)e.Item.FindControl("btnEditQues");

                if (Session["editQuiz"].ToString() == "true")
                {
                    btnDeleteQues.Visible = true;
                    btnEditQues.Visible = true;
                }
                string questionId = DataBinder.Eval(e.Item.DataItem, "questionId").ToString();

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection quizCon = new SqlConnection(providerConStr);
                quizCon.Open();


                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Choices] WHERE questionId = @questionId", quizCon);
                cmd.Parameters.AddWithValue("@questionId", questionId);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                rbl1.DataSource = dt;
                rbl1.DataTextField = "choice";
                rbl1.DataValueField = "isAnswer";
                rbl1.DataBind();

                quizCon.Close();






            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int correct = 0;
            int total = 0;
            foreach (RepeaterItem item in quizRepeater.Items)
            {
                total += 1;
                string value = (item.FindControl("rbl1") as RadioButtonList).SelectedValue;
                if (value == "True")
                {
                    correct += 1;
                }

            }
            Label1.Text = Convert.ToString(correct) + "/" + Convert.ToString(total);
        }

        protected void btnMore_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["editQuiz"].ToString() == "false")
            {
                Session["editQuiz"] = "true";
                String url = "question.aspx?quizId=1";
                Response.Redirect(url);

            }
            else
            {
                Session["editQuiz"] = "false";
                String url = "question.aspx?quizId=1";
                Response.Redirect(url);
            }
        }

        protected void closeFormClicked(object sender, EventArgs e)
        {
            Session["updateQues"] = "false";
        }

            protected void addQuesFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string question = formQues.Text;
                string choice1 = formChoice1.Text;
                string choice2 = formChoice2.Text;
                string choice3 = formChoice3.Text;
                string choice4 = formChoice4.Text;
                string ans = ddlAns.SelectedValue;
                string quizId = Request.QueryString["quizId"];
                Boolean[] isAnswer = null;
                if (ans == "Choice 1")
                {
                    isAnswer = new Boolean[] { true, false, false, false };
                }
                else if (ans == "Choice 2")
                {
                    isAnswer = new Boolean[] { false, true, false, false };
                }
                else if (ans == "Choice 3")
                {
                    isAnswer = new Boolean[] { false, false, true, false };
                }
                else if (ans == "Choice 4")
                {
                    isAnswer = new Boolean[] { false, false, false, true };
                }
                string[] allChoice = { choice1, choice2, choice3, choice4 };
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection quizCon = new SqlConnection(providerConStr);
                quizCon.Open();

                

                if (Session["updateQues"].ToString() == "false")
                {
                    string addQues = "INSERT INTO [dbo].[Question] VALUES(@question,@quizId);";
                    SqlCommand cmdAddQues = new SqlCommand(addQues, quizCon);

                    cmdAddQues.Parameters.AddWithValue("@question", question);
                    cmdAddQues.Parameters.AddWithValue("@quizId", quizId);
                    cmdAddQues.ExecuteNonQuery();

                    string strSelectId = "Select questionId FROM Question Where question=@question;";
                    SqlCommand cmdSelectId = new SqlCommand(strSelectId, quizCon);
                    cmdSelectId.Parameters.AddWithValue("@question", question);
                    string questionId = Convert.ToString(cmdSelectId.ExecuteScalar());

                   
                    for (int i = 0; i < allChoice.Length; i++)
                    {
                        string addChoice = "INSERT INTO [dbo].[Choices] VALUES(@choice,@isAnswer,@questionId);";
                        SqlCommand cmdAddChoice = new SqlCommand(addChoice, quizCon);
                        cmdAddChoice.Parameters.AddWithValue("@choice", allChoice[i]);
                        cmdAddChoice.Parameters.AddWithValue("@isAnswer", isAnswer[i]);
                        cmdAddChoice.Parameters.AddWithValue("@questionId", questionId);
                        cmdAddChoice.ExecuteNonQuery();

                    }
                }
                else if (Session["updateQues"].ToString() == "true")
                {
                    string updateQues = "UPDATE INTO [dbo].[Question] SET question=@question WHERE questionId=@questionId;";
                    SqlCommand cmdUpdateQues = new SqlCommand(updateQues, quizCon);

                    cmdUpdateQues.Parameters.AddWithValue("@question", question);
                    cmdUpdateQues.Parameters.AddWithValue("@questionId", Session["questionId"].ToString());
                    cmdUpdateQues.ExecuteNonQuery();

                    

                    string[] allChoiceId = (string[])Session["allChoiceId"];

                    for (int i = 0; i < allChoiceId.Length; i++)
                    {
                        string addChoice = "UPDATE INTO [dbo].[Choices] SET choice=@choice,isAnswer=@isAnswer WHERE choiceId=@choiceId);";
                        SqlCommand cmdAddChoice = new SqlCommand(addChoice, quizCon);
                        cmdAddChoice.Parameters.AddWithValue("@choice", allChoice[i]);
                        cmdAddChoice.Parameters.AddWithValue("@isAnswer", isAnswer[i]);
                        cmdAddChoice.Parameters.AddWithValue("@choiceId", allChoiceId[i]);
                        cmdAddChoice.ExecuteNonQuery();

                    }
                    Session["updateQues"] = "false";
                } 
                
                

                

              

                quizCon.Close();

                String url = "question.aspx?quizId=" + quizId;
                Response.Redirect(url);


            }



        }

        protected void quizRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string quizId = Request.QueryString["quizId"];
            String questionId = e.CommandArgument.ToString();
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection quizCon = new SqlConnection(providerConStr);
            quizCon.Open();

            if (e.CommandName == "deleteQues")
            {
                String strDelQues = "DELETE FROM Question WHERE questionId=@questionId ;";
                SqlCommand cmdDelQues = new SqlCommand(strDelQues, quizCon);
                cmdDelQues.Parameters.AddWithValue("@questionId", questionId);
                cmdDelQues.ExecuteNonQuery();

                quizCon.Close();

                String url = "question.aspx?quizId=" + quizId;
                Response.Redirect(url);
            }
            if (e.CommandName == "updateQues")
            {
                Session["updateQues"] = "true";
                Session["questionId"] = questionId;
                String strSelectQues = "Select question FROM Question Where questionId=@questionId;";
                SqlCommand cmdSelectQues = new SqlCommand(strSelectQues, quizCon);
                cmdSelectQues.Parameters.AddWithValue("@questionId", questionId);
                formQues.Text = Convert.ToString(cmdSelectQues.ExecuteScalar());

                String strSelectChoice = "Select choice FROM Choices Where questionId=@questionId;";
                SqlCommand cmdSelectChoice = new SqlCommand(strSelectChoice, quizCon);
                cmdSelectQues.Parameters.AddWithValue("@questionId", questionId);

                SqlDataReader sdr = cmdSelectQues.ExecuteReader();
                List<string> choice = new List<string>();
                while (sdr.Read())
                {
                    choice.Add(sdr[0].ToString());
                }

                formChoice1.Text = choice[0];
                formChoice2.Text = choice[1];
                formChoice3.Text = choice[2];
                formChoice4.Text = choice[3];

                String strSelectChoiceId = "Select choiceId FROM Choices Where questionId=@questionId;";
                SqlCommand cmdSelectChoiceId = new SqlCommand(strSelectChoiceId, quizCon);
                cmdSelectChoiceId.Parameters.AddWithValue("@questionId", questionId);

                SqlDataReader sdrChoiceId = cmdSelectQues.ExecuteReader();
                List<string> choiceId = new List<string>();
                while (sdrChoiceId.Read())
                {
                    choiceId.Add(sdr[0].ToString());
                }

                

                Session["allChoiceId"] = choiceId;

               

            }
        }
    }
}