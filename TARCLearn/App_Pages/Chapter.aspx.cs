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
                SqlCommand cmdSelectCourse = new SqlCommand(strSelectChp, chpCon);
                cmdSelectCourse.Parameters.AddWithValue("@courseId", courseId);

                chpRepeater.DataSource = cmdSelectCourse.ExecuteReader();
                chpRepeater.DataBind();              
                chpCon.Close();
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
    }
}