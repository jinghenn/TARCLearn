using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TARCLearn.App_Pages
{
    public partial class videoViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["Userid"] == null)
            {
                System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
                string scriptKey = "ErrorMessage";

                javaScript.Append("var userConfirmation = window.confirm('" + "Your Session has Expired, Please login again.');\n");
                javaScript.Append("window.location='Login.aspx';");

                ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
            }

            string materialId = Request.QueryString["materialId"];
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection materialCon = new SqlConnection(providerConStr);
            materialCon.Open();

            //select data to be bound
            String strSelectMaterial = "Select materialName as materialName from Material Where materialId=@materialId;";
            SqlCommand cmdSelectCourse = new SqlCommand(strSelectMaterial, materialCon);
            cmdSelectCourse.Parameters.AddWithValue("@materialId", materialId);
            viewVideoRepeater.DataSource = cmdSelectCourse.ExecuteReader();
            viewVideoRepeater.DataBind();

            SqlCommand cmdGetChpId = new SqlCommand("Select chapterId from [dbo].[Material] where materialId=@materialId;", materialCon);
            cmdGetChpId.Parameters.AddWithValue("@materialId", materialId);
            string chapterId = Convert.ToString(cmdGetChpId.ExecuteScalar());

            SqlCommand cmdGetCourseId = new SqlCommand("Select courseId from [dbo].[Chapter] where chapterId=@chapterId;", materialCon);
            cmdGetCourseId.Parameters.AddWithValue("@chapterId", chapterId);
            string courseId = Convert.ToString(cmdGetCourseId.ExecuteScalar());

            SqlCommand cmdGetMtitle = new SqlCommand("Select materialTitle from [dbo].[Material] where materialId=@materialId;", materialCon);
            cmdGetMtitle.Parameters.AddWithValue("@materialId", materialId);
            lblMaterialName.Text = Convert.ToString(cmdGetMtitle.ExecuteScalar());

            lblHome.Text = "<a href = 'course.aspx'> Home </a>";
            lblChp.Text = "<a href = 'Chapter.aspx?courseId=" + courseId + "'> Chapter </a>";
            lblMaterial.Text = "<a href = 'material.aspx?chapterId=" + chapterId + "&materialType=video'> Video </a>";


            materialCon.Close();
        }
    }
}