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
    public partial class ReadingMaterial : System.Web.UI.Page
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
            viewPDFRepeater.DataSource = cmdSelectCourse.ExecuteReader();
            viewPDFRepeater.DataBind();

            SqlCommand cmdGetChpId = new SqlCommand("Select chapterId from [dbo].[Material] where materialId=@materialId;", materialCon);
            cmdGetChpId.Parameters.AddWithValue("@materialId", materialId);
            string chapterId = Convert.ToString(cmdGetChpId.ExecuteScalar());

            SqlCommand cmdGetCourseId = new SqlCommand("Select courseId from [dbo].[Chapter] where chapterId=@chapterId;", materialCon);
            cmdGetCourseId.Parameters.AddWithValue("@chapterId", chapterId);
            string courseId = Convert.ToString(cmdGetCourseId.ExecuteScalar());

            lblHome.Text = "<a href = 'course.aspx'> Home </a>";
            lblChp.Text = "<a href = 'Chapter.aspx?courseId=" + courseId + "'> Chapter </a>";
            lblMaterial.Text = "<a href = 'material.aspx? chapterId = " + chapterId + " & materialType = readingMaterial'> Chapter </a>";

            materialCon.Close();
        }

       
    }
}