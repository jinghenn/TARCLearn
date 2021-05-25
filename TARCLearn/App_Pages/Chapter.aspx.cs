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
                String strSelectChp = "Select chapterTitle AS chpTitle, chapterId as chpId from Chapter Where courseId=@courseId;";
                SqlCommand cmdSelectCourse = new SqlCommand(strSelectChp, chpCon);
                cmdSelectCourse.Parameters.AddWithValue("@courseId", courseId);

                chpRepeater.DataSource = cmdSelectCourse.ExecuteReader();
                chpRepeater.DataBind();
            }
        }

        protected void chapterRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}