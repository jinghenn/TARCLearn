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
    public partial class readingMaterial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string chapterId = Request.QueryString["chapterId"];
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection materialCon = new SqlConnection(providerConStr);
                materialCon.Open();

                //select data to be bound
                String strSelectMaterial = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId;";
                SqlCommand cmdSelectMaterial = new SqlCommand(strSelectMaterial, materialCon);
                cmdSelectMaterial.Parameters.AddWithValue("@chapterId", chapterId);

                rmRepeater.DataSource = cmdSelectMaterial.ExecuteReader();
                rmRepeater.DataBind();
            }
        }

        protected void rmRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "selectRM")
            {
                String materialId = e.CommandArgument.ToString();
                String url = "pdfViewer.aspx?materialId=" + materialId;
                Response.Redirect(url);

            }
        
        }
    }
}