using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Drawing;

namespace TARCLearn.App_Pages
{
    public partial class Chapter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                string courseId = Request.QueryString["courseId"];
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection chpCon = new SqlConnection(providerConStr);
                chpCon.Open();

                //select data to be bound
                String strSelectChp = "Select chapterTitle AS chpTitle, chapterId AS chpId, chapterNo AS chpNo from Chapter Where courseId=@courseId;";
                SqlCommand cmdSelectChapter = new SqlCommand(strSelectChp, chpCon);
                cmdSelectChapter.Parameters.AddWithValue("@courseId", courseId);

                chpRepeater.DataSource = cmdSelectChapter.ExecuteReader();
                chpRepeater.DataBind();

                //select data to be bound
                String strSelectEditChp = "Select chapterTitle AS chpTitle, chapterId AS chpId, chapterNo AS chpNo from Chapter Where courseId=@courseId;";
                SqlCommand cmdSelectEditChapter = new SqlCommand(strSelectEditChp, chpCon);
                cmdSelectEditChapter.Parameters.AddWithValue("@courseId", courseId);

                rptDeleteChapter.DataSource = cmdSelectEditChapter.ExecuteReader();
                rptDeleteChapter.DataBind();

                chpCon.Close();

                
            }
            String userType = Session["userType"].ToString();
            if (userType == "Student")
            {
                btnDeleteChapter.Visible = false;
                btnAddChapter.Visible = false;
            }
        }

        protected void chapterRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String chapter = e.CommandArgument.ToString();
            Button btnRM = (Button)e.Item.FindControl("btnRM");
            Button btnVideo = (Button)e.Item.FindControl("btnVideo");
            Button btnDis = (Button)e.Item.FindControl("btnDis");
            Button btnQuiz = (Button)e.Item.FindControl("btnQuiz");
            if (e.CommandName == "selectChp")
            {
                if (btnRM.Visible)
                {
                    btnRM.Visible = false;
                    btnVideo.Visible = false;
                    btnDis.Visible = false;
                    btnQuiz.Visible = false;
                }
                else {
                    btnRM.Visible = true;
                    btnVideo.Visible = true;
                    btnDis.Visible = true;
                    btnQuiz.Visible = true;

                }
                               
            }if(e.CommandName == "selectRM")
            {
                String chapterId = e.CommandArgument.ToString();
                String url = "readingMaterial.aspx?chapterId=" + chapterId + "&materialType=readingMaterial";
                Response.Redirect(url);
               
            }
            if (e.CommandName == "selectVideo")
            {

                String chapterId = e.CommandArgument.ToString();
                String url = "readingMaterial.aspx?chapterId=" + chapterId + "&materialType=video";
                Response.Redirect(url);

            }
            if (e.CommandName == "selectDis")
            {

                String chapterId = e.CommandArgument.ToString();
                String url = "Discussion.aspx?chapterId=" + chapterId;
                Response.Redirect(url);

            }
            if (e.CommandName == "selectQuiz")
            {

                String chapterId = e.CommandArgument.ToString();
                String url = "quiz.aspx?chapterId=" + chapterId;
                Response.Redirect(url);
                

            }
        }

        protected void btnDeleteChapter_Click(object sender, ImageClickEventArgs e)
        {
            if (rptDeleteChapter.Visible == false)
            {
                chpRepeater.Visible = false;
                rptDeleteChapter.Visible = true;
            }
            else
            {
                string courseId = Request.QueryString["courseId"];
                String url = "Chapter.aspx?courseId=" + courseId;
                Response.Redirect(url);
            }
        }

        protected void rptDeleteChapter_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String chpId = e.CommandArgument.ToString();
            TextBox txtChapterNo = (TextBox)e.Item.FindControl("txtChapterNo");
            TextBox txtChapterTitle = (TextBox)e.Item.FindControl("txtChapterTitle");

            LinkButton btnEdit = (LinkButton)e.Item.FindControl("btnEdit");
            LinkButton btnSave = (LinkButton)e.Item.FindControl("btnSave");
            LinkButton btnCancel = (LinkButton)e.Item.FindControl("btnCancel");
            LinkButton btnDel = (LinkButton)e.Item.FindControl("btnDelete");

            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection chpCon = new SqlConnection(providerConStr);
            chpCon.Open();

            if (e.CommandName == "edit")
            {
                
                txtChapterNo.Enabled = true;
                txtChapterNo.BorderStyle = BorderStyle.Inset;
                txtChapterNo.BackColor = Color.White;

                txtChapterTitle.Enabled = true;
                txtChapterTitle.BorderStyle = BorderStyle.Inset;
                txtChapterTitle.BackColor = Color.White;

                btnEdit.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDel.Visible = true;

            }
            if (e.CommandName == "delete")
            {
               String strDelChapter = "DELETE FROM Chapter WHERE chapterId=@chapterId ;";
                SqlCommand cmdDelChapter = new SqlCommand(strDelChapter, chpCon);
                cmdDelChapter.Parameters.AddWithValue("@chapterId", chpId);
                cmdDelChapter.ExecuteNonQuery();

                chpCon.Close();

                string courseId = Request.QueryString["courseId"];
                String url = "Chapter.aspx?courseId=" + courseId;
                Response.Redirect(url);

            }
            if (e.CommandName == "save")
            {
                if (Page.IsValid)
                {

                    txtChapterNo.Enabled = true;
                    txtChapterNo.BorderStyle = BorderStyle.Inset;
                    txtChapterNo.BackColor = Color.White;

                    txtChapterTitle.Enabled = true;
                    txtChapterTitle.BorderStyle = BorderStyle.Inset;
                    txtChapterTitle.BackColor = Color.White;


                    btnEdit.Visible = true;
                    btnCancel.Visible = false;
                    btnDel.Visible = false;
                    btnSave.Visible = false;

                    SqlCommand cmdSelectChapterNo = new SqlCommand("Select * from [dbo].[Chapter] where chapterNo=@chapterNo", chpCon);
                    cmdSelectChapterNo.Parameters.AddWithValue("@chapterNo", txtChapterNo.Text);
                    SqlDataReader dtrChapterNo = cmdSelectChapterNo.ExecuteReader();

                    SqlCommand cmdSelectChapterTitle = new SqlCommand("Select * from [dbo].[Chapter] where chapterTitle=@chapterTitle", chpCon);
                    cmdSelectChapterTitle.Parameters.AddWithValue("@chapterTitle", txtChapterTitle.Text);
                    SqlDataReader dtrChapterTitle = cmdSelectChapterTitle.ExecuteReader();

                    if (!dtrChapterNo.HasRows && !dtrChapterTitle.HasRows)
                    {
                        String editCourse = "UPDATE [dbo].[Chapter] SET chapterNo = @newChapterNo, chapterTitle = @newChapterTitle WHERE chapterId = @chapterId";
                        SqlCommand cmdEditCourse = new SqlCommand(editCourse, chpCon);
                        cmdEditCourse.Parameters.AddWithValue("@newChapterNo", txtChapterNo.Text);
                        cmdEditCourse.Parameters.AddWithValue("@newChapterTitle", txtChapterTitle.Text);
                        cmdEditCourse.Parameters.AddWithValue("@chapterId", chpId);
                        cmdEditCourse.ExecuteNonQuery();
                         
                        chpCon.Close();
                        string courseId = Request.QueryString["courseId"];
                        String url = "Chapter.aspx?courseId=" + courseId;
                        Response.Redirect(url);
                    }
                    else if (dtrChapterNo.HasRows && dtrChapterTitle.HasRows)
                    {
                        Response.Write("<script>alert('Both Entered Chapter No. and Chapter Title Already Exists.')</script>");                      

                    }
                    else if (dtrChapterNo.HasRows && (txtChapterNo.Text == Session["currentChpNo"].ToString()) && !dtrChapterTitle.HasRows)
                    {
                        String editCourse = "UPDATE [dbo].[Chapter] SET chapterTitle = @newChapterTitle WHERE chapterId = @chapterId";
                        SqlCommand cmdEditCourse = new SqlCommand(editCourse, chpCon);
                        cmdEditCourse.Parameters.AddWithValue("@newChapterTitle", txtChapterTitle.Text);
                        cmdEditCourse.Parameters.AddWithValue("@chapterId", chpId);
                        cmdEditCourse.ExecuteNonQuery();

                        chpCon.Close();
                        string courseId = Request.QueryString["courseId"];
                        String url = "Chapter.aspx?courseId=" + courseId;
                        Response.Redirect(url);
                    }
                    else if (dtrChapterNo.HasRows && (txtChapterNo.Text != Session["currentChpNo"].ToString()) && !dtrChapterTitle.HasRows)
                    {
                        Response.Write("<script>alert('Entered Chapter No. Already Exists.')</script>");

                    }
                    else if (!dtrChapterNo.HasRows && (txtChapterTitle.Text == Session["currentChpTitle"].ToString()) && dtrChapterTitle.HasRows)
                    {
                        String editCourse = "UPDATE [dbo].[Chapter] SET chapterNo = @newChapterNo WHERE chapterId = @chapterId";
                        SqlCommand cmdEditCourse = new SqlCommand(editCourse, chpCon);
                        cmdEditCourse.Parameters.AddWithValue("@newChapterNo", txtChapterNo.Text);
                        cmdEditCourse.Parameters.AddWithValue("@chapterId", chpId);
                        cmdEditCourse.ExecuteNonQuery();

                        chpCon.Close();
                        string courseId = Request.QueryString["courseId"];
                        String url = "Chapter.aspx?courseId=" + courseId;
                        Response.Redirect(url);
                    }
                    else if (!dtrChapterNo.HasRows && (txtChapterTitle.Text != Session["currentChpTitle"].ToString()) && dtrChapterTitle.HasRows)
                    {
                        Response.Write("<script>alert('Entered Chapter Title Already Exists.')</script>");

                    }



                }

            }
            if (e.CommandName == "cancel")
            {
                string courseId = Request.QueryString["courseId"];
                txtChapterNo.Enabled = false;
                txtChapterNo.BorderStyle = BorderStyle.None;
                txtChapterNo.BackColor = Color.Transparent;

                txtChapterTitle.Enabled = false;
                txtChapterTitle.BorderStyle = BorderStyle.None;
                txtChapterTitle.BackColor = Color.Transparent;


                btnEdit.Visible = true;
                btnCancel.Visible = false;
                btnDel.Visible = false;
                btnSave.Visible = false;

                //select data to be bound
                String strSelectEditChp = "Select chapterTitle AS chpTitle, chapterId AS chpId, chapterNo AS chpNo from Chapter Where courseId=@courseId;";
                SqlCommand cmdSelectEditChapter = new SqlCommand(strSelectEditChp, chpCon);
                cmdSelectEditChapter.Parameters.AddWithValue("@courseId", courseId);

                rptDeleteChapter.DataSource = cmdSelectEditChapter.ExecuteReader();
                rptDeleteChapter.DataBind();

            }
        }
        protected void addChapterFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string chpNo = formChpNo.Text;
                string chpTitle = formChpTitle.Text;
                string courseId = Request.QueryString["courseId"];

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection courseCon = new SqlConnection(providerConStr);
                courseCon.Open();

                SqlCommand cmdSelectChpNo = new SqlCommand("Select * from [dbo].[Chapter] where chapterNo=@chapterNo AND courseId=@courseId;", courseCon);
                cmdSelectChpNo.Parameters.AddWithValue("@chapterNo", chpNo);
                cmdSelectChpNo.Parameters.AddWithValue("@courseId", courseId);             
                SqlDataReader dtrChpNo = cmdSelectChpNo.ExecuteReader();

                SqlCommand cmdSelectChpTitle = new SqlCommand("Select * from [dbo].[Chapter] where chapterTitle=@chapterTitle AND courseId=@courseId;", courseCon);                
                cmdSelectChpTitle.Parameters.AddWithValue("@courseId", courseId);
                cmdSelectChpTitle.Parameters.AddWithValue("@chapterTitle", chpTitle);
                SqlDataReader dtrChpTitle = cmdSelectChpTitle.ExecuteReader();

                if (!dtrChpNo.HasRows && !dtrChpTitle.HasRows)
                {
                    String addChapter = "INSERT INTO [dbo].[Chapter] VALUES(@chapterNo,@courseId,@chapterTitle);";
                    SqlCommand cmdAddChapter = new SqlCommand(addChapter, courseCon);

                    cmdAddChapter.Parameters.AddWithValue("@chapterNo", chpNo);
                    cmdAddChapter.Parameters.AddWithValue("@courseId", courseId);
                    cmdAddChapter.Parameters.AddWithValue("@chapterTitle", chpTitle);
                    cmdAddChapter.ExecuteNonQuery();
                    courseCon.Close();
                    chpRepeater.DataBind();
                    String url = "Chapter.aspx?courseId=" + courseId;
                    Response.Redirect(url);
                }
                else if (dtrChpNo.HasRows && dtrChpTitle.HasRows)
                {
                    Response.Write("<script>alert('Both Chapter No. and Chapter Title Arealdy Exist.')</script>");
                }
                else if (dtrChpNo.HasRows)
                {
                    Response.Write("<script>alert('Chapter No. Arealdy Exist.')</script>");
                }else if (dtrChpTitle.HasRows)
                {
                    Response.Write("<script>alert('Chapter Title Arealdy Exist.')</script>");
                }
                

            }



        }
    }
}