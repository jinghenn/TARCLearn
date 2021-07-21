using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TARCLearn.App_Pages
{
    public partial class questionList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            

            if (!IsPostBack)
            {
                string userId = Session["userId"].ToString();
                if (userId == null)
                {
                    System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
                    string scriptKey = "ErrorMessage";

                    javaScript.Append("var userConfirmation = window.confirm('" + "Your Session has Expired, Please login again.');\n");
                    javaScript.Append("window.location='Login.aspx';");

                    ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
                }
                Session["quesNo"] = "0";
                if (Session["editQuiz"] == null)
                {
                    Session["editQuiz"] = "false";
                }
                string quizId = Request.QueryString["quizId"];
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection quizCon = new SqlConnection(providerConStr);
                quizCon.Open();

                //select data to be bound
                String strSelectQuiz = "Select questionText AS questionText, questionId AS questionId from Question Where quizId=@quizId;";
                SqlCommand cmdSelectQuiz = new SqlCommand(strSelectQuiz, quizCon);
                cmdSelectQuiz.Parameters.AddWithValue("@quizId", quizId);

                quizRepeater.DataSource = cmdSelectQuiz.ExecuteReader();
                quizRepeater.DataBind();



                String strSelectTitle = "Select quizTitle FROM Quiz Where quizId=@quizId;";
                SqlCommand cmdSelectTitle = new SqlCommand(strSelectTitle, quizCon);
                cmdSelectTitle.Parameters.AddWithValue("@quizId", quizId);
                string title = Convert.ToString(cmdSelectTitle.ExecuteScalar());
                lblTittle.Text = title;

                SqlCommand cmdGetChpId = new SqlCommand("Select chapterId from [dbo].[Quiz] where quizId=@quizId;", quizCon);
                cmdGetChpId.Parameters.AddWithValue("@quizId", quizId);
                string chapterId = Convert.ToString(cmdGetChpId.ExecuteScalar());

                SqlCommand cmdGetCourseId = new SqlCommand("Select courseId from [dbo].[Chapter] where chapterId=@chapterId;", quizCon);
                cmdGetCourseId.Parameters.AddWithValue("@chapterId", chapterId);
                string courseId = Convert.ToString(cmdGetCourseId.ExecuteScalar());

                lblHome.Text = "<a href = 'course.aspx'> Home </a>";
                lblChp.Text = "<a href = 'Chapter.aspx?courseId=" + courseId + "'> Chapter </a>";
                lblQuiz.Text = "<a href = 'quiz.aspx?chapterId=" + chapterId + "'> Quizs </a>";
                lblQues.Text = title;

                quizCon.Close();

                string userType = Session["userType"].ToString();
                if (userType == "Student")
                {
                    btnMore.Visible = false;
                    btnAddQues.Visible = false;
                }

            }




        }

        public void successMsg(string msg, string id)
        {
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
            string scriptKey = "SuccessMessage";
            string url = "questionList.aspx?quizId=" + id;

            javaScript.Append("var userConfirmation = window.confirm('" + "Successfully " + msg + "');\n");
            javaScript.Append("window.location='" + url + "';");

            ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
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
                Repeater rptEditChoice = (Repeater)e.Item.FindControl("rptEditChoice");
                LinkButton btnEditQuesText = (LinkButton)e.Item.FindControl("btnEditQuesText");
                LinkButton btnAddChoice = (LinkButton)e.Item.FindControl("btnAddChoice");
                TextBox txtQuesNo = (TextBox)e.Item.FindControl("txtQuesNo");
                string questionId = DataBinder.Eval(e.Item.DataItem, "questionId").ToString();
                

                int questionNo = Convert.ToInt32(Session["quesNo"].ToString()) + 1;
                Session["quesNo"] = questionNo;
                txtQuesNo.Text = Convert.ToString(questionNo) + ".) " ;

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
                rbl1.DataTextField = "choiceText";
                rbl1.DataValueField = "isAnswer";
                rbl1.DataBind();

                if (Session["editQuiz"].ToString() == "true")
                {
                    String strSelectEditChoice = "SELECT choiceText AS choiceText, choiceId AS choiceId FROM [dbo].[Choices] WHERE questionId = @questionId";
                    SqlCommand cmdSelectEditChoice = new SqlCommand(strSelectEditChoice, quizCon);
                    cmdSelectEditChoice.Parameters.AddWithValue("@questionId", questionId);

                    rptEditChoice.DataSource = cmdSelectEditChoice.ExecuteReader();
                    rptEditChoice.DataBind();
                    rptEditChoice.Visible = true;
                    btnEditQuesText.Visible = true;
                    btnAddChoice.Visible = true;
                    rbl1.Visible = false;
                }
                else
                {
                    rptEditChoice.Visible = false;
                    btnEditQuesText.Visible = false;
                    btnAddChoice.Visible = false;
                    rbl1.Visible = true;
                }

                quizCon.Close();

            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
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
            lblResult.Text = Convert.ToString(correct) + "/" + Convert.ToString(total);
            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "showResult();", true);
        }

        protected void btnMore_Click(object sender, ImageClickEventArgs e)
        {

            if (Session["editQuiz"].ToString() == "false")
            {
                Session["editQuiz"] = "true";
            }
            else
            {
                Session["editQuiz"] = "false";

            }
            string quizId = Request.QueryString["quizId"];
            String url = "questionList.aspx?quizId=" + quizId;
            Response.Redirect(url);
        }



        protected void addQuesFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string questionText = formQues.Text;

                string quizId = Request.QueryString["quizId"];


                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection quizCon = new SqlConnection(providerConStr);
                quizCon.Open();

                string addQues = "INSERT INTO [dbo].[Question] VALUES(@questionText,@quizId);";
                SqlCommand cmdAddQues = new SqlCommand(addQues, quizCon);

                cmdAddQues.Parameters.AddWithValue("@questionText", questionText);
                cmdAddQues.Parameters.AddWithValue("@quizId", quizId);
                cmdAddQues.ExecuteNonQuery();

                quizCon.Close();

                successMsg("added", quizId);


            }



        }
        protected void quizResultFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                string quizId = Request.QueryString["quizId"];
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection quizCon = new SqlConnection(providerConStr);
                quizCon.Open();          

                String strGetChp = "Select chapterId FROM Quiz Where quizId=@quizId;";
                SqlCommand cmdGetChp = new SqlCommand(strGetChp, quizCon);
                cmdGetChp.Parameters.AddWithValue("@quizId", quizId);
                string chapterId = Convert.ToString(cmdGetChp.ExecuteScalar());

                quizCon.Close();
                String url = "quiz.aspx?chapterId=" + chapterId;
                Response.Redirect(url);


            }



        }

        protected void quizRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string quizId = Request.QueryString["quizId"];
            String questionId = e.CommandArgument.ToString();

            TextBox txtQuesText = (TextBox)e.Item.FindControl("txtQuesText");

            LinkButton btnEditQuesText = (LinkButton)e.Item.FindControl("btnEditQuesText");
            LinkButton btnAddChoice = (LinkButton)e.Item.FindControl("btnAddChoice");
            LinkButton btnSaveQuesText = (LinkButton)e.Item.FindControl("btnSaveQuesText");
            LinkButton btnCancelQuesText = (LinkButton)e.Item.FindControl("btnCancelQuesText");
            LinkButton btnDeleteQuesText = (LinkButton)e.Item.FindControl("btnDeleteQuesText");

            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection quizCon = new SqlConnection(providerConStr);
            quizCon.Open();

            if (e.CommandName == "delete")
            {
                String strDelQues = "DELETE FROM Question WHERE questionId=@questionId ;";
                SqlCommand cmdDelQues = new SqlCommand(strDelQues, quizCon);
                cmdDelQues.Parameters.AddWithValue("@questionId", questionId);
                cmdDelQues.ExecuteNonQuery();

                quizCon.Close();

                successMsg("deleted", quizId);
            }
            if (e.CommandName == "edit")
            {
                txtQuesText.Enabled = true;
                txtQuesText.BorderStyle = BorderStyle.Inset;
                txtQuesText.BackColor = Color.White;

                btnEditQuesText.Visible = false;
                btnAddChoice.Visible = false;
                btnSaveQuesText.Visible = true;
                btnCancelQuesText.Visible = true;
                btnDeleteQuesText.Visible = true;

            }
            if (e.CommandName == "save")
            {
                if (Page.IsValid)
                {

                    txtQuesText.Enabled = true;
                    txtQuesText.BorderStyle = BorderStyle.Inset;
                    txtQuesText.BackColor = Color.White;

                    btnEditQuesText.Visible = true;
                    btnAddChoice.Visible = true;
                    btnSaveQuesText.Visible = false;
                    btnCancelQuesText.Visible = false;
                    btnDeleteQuesText.Visible = false;

                    SqlCommand cmdSelect = new SqlCommand("Select * from [dbo].[Question] where questionText=@questionText", quizCon);
                    cmdSelect.Parameters.AddWithValue("@questionText", txtQuesText.Text);
                    SqlDataReader dtr = cmdSelect.ExecuteReader();



                    if (!dtr.HasRows)
                    {
                        String edit = "UPDATE [dbo].[Question] SET questionText = @questionText WHERE questionId = @questionId";
                        SqlCommand cmdEdit = new SqlCommand(edit, quizCon);
                        cmdEdit.Parameters.AddWithValue("@questionText", txtQuesText.Text);
                        cmdEdit.Parameters.AddWithValue("@questionId", questionId);

                        cmdEdit.ExecuteNonQuery();

                        quizCon.Close();
                        successMsg("updated", quizId);
                    }
                    else if (dtr.HasRows)
                    {
                        Response.Write("<script>alert('Entered Question Already Exists.')</script>");

                    }




                }

            }
            if (e.CommandName == "cancel")
            {

                txtQuesText.Enabled = false;
                txtQuesText.BorderStyle = BorderStyle.None;
                txtQuesText.BackColor = Color.Transparent;

                btnEditQuesText.Visible = true;
                btnAddChoice.Visible = true;
                btnSaveQuesText.Visible = false;
                btnCancelQuesText.Visible = false;
                btnDeleteQuesText.Visible = false;

                String url = "questionList.aspx?quizId=" + quizId;
                Response.Redirect(url);

            }
            if (e.CommandName == "add")
            {
                Session["questionId"] = questionId;
                Session["manageChoice"] = "add";
                formChoice.Text = "";               

                SqlCommand cmdSelectChoice = new SqlCommand("Select * from [dbo].[Choices] c, [dbo].[Question] q where c.questionId = q.questionId AND c.questionId=@questionId AND c.isAnswer=@isAnswer;", quizCon);
                cmdSelectChoice.Parameters.AddWithValue("@questionId", questionId);
                cmdSelectChoice.Parameters.AddWithValue("@isAnswer", true);
                SqlDataReader dtrChoice = cmdSelectChoice.ExecuteReader();

                if (dtrChoice.HasRows)
                {
                    foreach (ListItem item in rblAnswer.Items)
                    {
                        if (item.Text == "True")
                        {
                            item.Enabled = false;
                            item.Attributes.Add("style", "color:#999;");
                            break;
                        }
                    }
                    lblAnswer.Text = "Is Answer (Already have an answer.)";
                }
                else
                {
                    foreach (ListItem item in rblAnswer.Items)
                    {
                        if (item.Text == "True")
                        {
                            item.Enabled = true;                           
                            break;
                        }
                    }
                    lblAnswer.Text = "Is Answer";
                }
                lblMTitle.Text = "Add New Choice";
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
                
                             
            }
        }

        protected void addChoiceFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string quizId = Request.QueryString["quizId"];
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection quizCon = new SqlConnection(providerConStr);
                quizCon.Open();

                if(Session["manageChoice"].ToString() == "add") {
                    string add = "INSERT INTO [dbo].[Choices] VALUES(@choiceText,@isAnswer,@questionId);";
                    SqlCommand cmdAdd = new SqlCommand(add, quizCon);

                    cmdAdd.Parameters.AddWithValue("@choiceText", formChoice.Text);
                    cmdAdd.Parameters.AddWithValue("@isAnswer", Convert.ToBoolean(rblAnswer.SelectedItem.Text));
                    cmdAdd.Parameters.AddWithValue("@questionId", Session["questionId"].ToString());
                    cmdAdd.ExecuteNonQuery();
                    quizCon.Close();
                    successMsg("added", quizId);
                }
                else if (Session["manageChoice"].ToString() == "update")
                {
                    String edit = "UPDATE [dbo].[Choices] SET choiceText = @choiceText, isAnswer=@isAnswer WHERE choiceId = @choiceId";
                    SqlCommand cmdEdit = new SqlCommand(edit, quizCon);
                    cmdEdit.Parameters.AddWithValue("@choiceText", formChoice.Text);
                    cmdEdit.Parameters.AddWithValue("@choiceId", Session["choiceId"].ToString());
                    cmdEdit.Parameters.AddWithValue("@isAnswer", Convert.ToBoolean(rblAnswer.SelectedItem.Text));
                    cmdEdit.ExecuteNonQuery();
                    quizCon.Close();
                    successMsg("updated", quizId);
                }
                
            }
        }

        protected void rptEditChoice_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string choiceId = e.CommandArgument.ToString();
            string quizId = Request.QueryString["quizId"];          

            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection quizCon = new SqlConnection(providerConStr);
            quizCon.Open();

            if (e.CommandName == "edit")
            {
                SqlCommand cmdGetQuestionId = new SqlCommand("Select questionId from [dbo].[Choices] where choiceId=@choiceId", quizCon);
                cmdGetQuestionId.Parameters.AddWithValue("@choiceId", choiceId);
                String questionId = Convert.ToString(cmdGetQuestionId.ExecuteScalar());

                SqlCommand cmdGetChoiceText = new SqlCommand("Select choiceText from [dbo].[Choices] where choiceId=@choiceId", quizCon);
                cmdGetChoiceText.Parameters.AddWithValue("@choiceId", choiceId);
                formChoice.Text = Convert.ToString(cmdGetChoiceText.ExecuteScalar());

                SqlCommand cmdGetAnswer = new SqlCommand("Select isAnswer from [dbo].[Choices] where choiceId=@choiceId", quizCon);
                cmdGetAnswer.Parameters.AddWithValue("@choiceId", choiceId);
                rblAnswer.SelectedValue = Convert.ToString(cmdGetAnswer.ExecuteScalar());

                SqlCommand cmdSelectChoice = new SqlCommand("Select * from [dbo].[Choices] c, [dbo].[Question] q where c.questionId = q.questionId AND c.questionId=@questionId AND c.isAnswer=@isAnswer;", quizCon);
                cmdSelectChoice.Parameters.AddWithValue("@questionId", questionId);
                cmdSelectChoice.Parameters.AddWithValue("@isAnswer", true);
                SqlDataReader dtrChoice = cmdSelectChoice.ExecuteReader();

                if (dtrChoice.HasRows)
                {
                    SqlCommand cmdGetChoice = new SqlCommand("Select isAnswer from [dbo].[Choices] where choiceId=@choiceId", quizCon);
                    cmdGetChoice.Parameters.AddWithValue("@choiceId", choiceId);
                    Boolean currentChoice = Convert.ToBoolean(cmdGetChoice.ExecuteScalar());

                    if (!currentChoice)
                    {
                        foreach (ListItem item in rblAnswer.Items)
                        {
                            if (item.Text == "True")
                            {
                                item.Enabled = false;
                                item.Attributes.Add("style", "color:#999;");
                                break;
                            }
                        }
                        lblAnswer.Text = "Is Answer (Already have an answer.)";
                    }
                    else
                    {
                        foreach (ListItem item in rblAnswer.Items)
                        {
                            if (item.Text == "True")
                            {
                                item.Enabled = true;
                                break;
                            }
                        }
                        lblAnswer.Text = "Is Answer";
                    }


                }
                else
                {
                    foreach (ListItem item in rblAnswer.Items)
                    {
                        if (item.Text == "True")
                        {
                            item.Enabled = true;
                            break;
                        }
                    }
                    lblAnswer.Text = "Is Answer";
                }
            

                Session["choiceId"] = choiceId;
                Session["manageChoice"] = "update";
                lblMTitle.Text = "Edit Choice";
                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);

            }
            if (e.CommandName == "delete")
            {
                String strDel = "DELETE FROM Choices WHERE choiceId=@choiceId ";
                SqlCommand cmdDel = new SqlCommand(strDel, quizCon);
                cmdDel.Parameters.AddWithValue("@choiceId", choiceId);
                cmdDel.ExecuteNonQuery();
                quizCon.Close();
                successMsg("deleted", quizId);
            }
            
        }
    }
}
