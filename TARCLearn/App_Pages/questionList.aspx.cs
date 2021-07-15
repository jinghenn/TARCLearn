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
                TextBox txtQuesText = (TextBox)e.Item.FindControl("txtQuesText");
                string questionId = DataBinder.Eval(e.Item.DataItem, "questionId").ToString();
                string questionText = DataBinder.Eval(e.Item.DataItem, "questionText").ToString();

                int questionNo = Convert.ToInt32(Session["quesNo"].ToString()) + 1;
                Session["quesNo"] = questionNo;
                txtQuesText.Text = Convert.ToString(questionNo) + ".) " + questionText;

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

                string add = "INSERT INTO [dbo].[Choices] VALUES(@choiceText,@isAnswer,@questionId);";
                SqlCommand cmdAdd = new SqlCommand(add, quizCon);

                cmdAdd.Parameters.AddWithValue("@choiceText", formChoice.Text);
                cmdAdd.Parameters.AddWithValue("@isAnswer", Convert.ToBoolean(rblAnswer.SelectedItem.Text));
                cmdAdd.Parameters.AddWithValue("@questionId", Session["questionId"].ToString());
                cmdAdd.ExecuteNonQuery();

                quizCon.Close();

                successMsg("added", quizId);


            }
        }

        protected void rptEditChoice_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string choiceId = e.CommandArgument.ToString();
            string quizId = Request.QueryString["quizId"];
            TextBox txtChoice = (TextBox)e.Item.FindControl("txtChoice");

            LinkButton btnEdit = (LinkButton)e.Item.FindControl("btnEdit");
            LinkButton btnSave = (LinkButton)e.Item.FindControl("btnSave");
            LinkButton btnCancel = (LinkButton)e.Item.FindControl("btnCancel");
            LinkButton btnDel = (LinkButton)e.Item.FindControl("btnDelete");

            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection quizCon = new SqlConnection(providerConStr);
            quizCon.Open();

            if (e.CommandName == "edit")
            {               

                txtChoice.Enabled = true;
                txtChoice.BorderStyle = BorderStyle.Inset;
                txtChoice.BackColor = Color.White;

                btnEdit.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDel.Visible = true;


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
            if (e.CommandName == "save")
            {
                if (Page.IsValid)
                {
                    txtChoice.Enabled = true;
                    txtChoice.BorderStyle = BorderStyle.Inset;
                    txtChoice.BackColor = Color.White;

                    btnEdit.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    btnDel.Visible = true;

                    String edit = "UPDATE [dbo].[Choices] SET choiceText = @choiceText WHERE choiceId = @choiceId";
                    SqlCommand cmdEdit = new SqlCommand(edit, quizCon);
                    cmdEdit.Parameters.AddWithValue("@choiceText", txtChoice.Text);
                    cmdEdit.Parameters.AddWithValue("@choiceId", choiceId);
                    cmdEdit.ExecuteNonQuery();
                    quizCon.Close();
                    successMsg("updated", quizId);

                }

            }
            if (e.CommandName == "cancel")
            {

                txtChoice.Enabled = false;
                txtChoice.BorderStyle = BorderStyle.None;
                txtChoice.BackColor = Color.Transparent;

                btnEdit.Visible = true;
                btnCancel.Visible = false;
                btnDel.Visible = false;
                btnSave.Visible = false;

                String url = "questionList.aspx?quizId=" + quizId;
                Response.Redirect(url);

            }
        }
    }
}
