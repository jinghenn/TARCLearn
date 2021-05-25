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
    public partial class MainPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection courseCon = new SqlConnection(providerConStr);
                courseCon.Open();
             
                //select data to be bound
                String strSelectCourse = "Select c.courseTitle AS courseTitle, c.courseId AS courseId from Course c, Enrolment e Where c.courseId = e.courseId and e.userId =@userId;";
                SqlCommand cmdSelectCourse = new SqlCommand(strSelectCourse, courseCon);
                cmdSelectCourse.Parameters.AddWithValue("@userId", Session["userId"].ToString());

                courseRepeater.DataSource = cmdSelectCourse.ExecuteReader();
                courseRepeater.DataBind();
            }

        }

        protected void courseRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String courseId = e.CommandArgument.ToString();
            if (e.CommandName == "selectCourse")
            {
                String url = "Chapter.aspx?courseId=" + courseId;
                Response.Redirect(url);
                
            }
        }
    }
}