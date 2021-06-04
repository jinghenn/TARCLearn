using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

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
                String url = "readingMaterial.aspx?chapterId=" + chapterId;
                Response.Redirect(url);
               
            }
            if (e.CommandName == "selectVideo")
            {
               
                Response.Redirect("videoViewer.aspx");

            }
        }

        protected void btnDeleteChapter_Click(object sender, ImageClickEventArgs e)
        {
            if (rptDeleteChapter.Visible == false)
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

                rptDeleteChapter.DataSource = cmdSelectChapter.ExecuteReader();
                rptDeleteChapter.DataBind();
                chpCon.Close();

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
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection courseCon = new SqlConnection(providerConStr);
            courseCon.Open();
            if (e.CommandName == "deleteChapter")
            {
               String strDelChapter = "DELETE FROM Chapter WHERE chapterId=@chapterId ;";
                SqlCommand cmdDelChapter = new SqlCommand(strDelChapter, courseCon);
                cmdDelChapter.Parameters.AddWithValue("@chapterId", chpId);
                cmdDelChapter.ExecuteNonQuery();

                 
                
                courseCon.Close();
                string courseId = Request.QueryString["courseId"];
                String url = "Chapter.aspx?courseId=" + courseId;
                Response.Redirect(url);

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

                SqlCommand cmdSelectChp = new SqlCommand("Select * from [dbo].[Chapter] where (chapterNo=@chapterNo AND courseId=@courseId) OR (chapterTitle=@chapterTitle AND courseId=@courseId)", courseCon);
                cmdSelectChp.Parameters.AddWithValue("@chapterNo", chpNo);
                cmdSelectChp.Parameters.AddWithValue("@courseId", courseId);
                cmdSelectChp.Parameters.AddWithValue("@chapterTitle", chpTitle);
                SqlDataReader dtrChp = cmdSelectChp.ExecuteReader();

                if (dtrChp.HasRows)
                {
                    Response.Write("<script>alert('Chapter No. or Chapter Title Arealdy Exist.')</script>");
                }
                else
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

            }



        }
    }
}