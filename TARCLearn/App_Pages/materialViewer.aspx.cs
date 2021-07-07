using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GroupDocs.Viewer;
using GroupDocs.Viewer.Options;

namespace TARCLearn.App_Pages
{
    public partial class materialViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string materialId = Request.QueryString["materialId"];
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection materialCon = new SqlConnection(providerConStr);
            materialCon.Open();

            //select data to be bound
            String strSelectMaterial = "Select materialName as materialName from Material Where materialId=@materialId;";
            SqlCommand cmdGetRmTitle = new SqlCommand(strSelectMaterial, materialCon);
            cmdGetRmTitle.Parameters.AddWithValue("@materialId", materialId);
            string materialFileName = Convert.ToString(cmdGetRmTitle.ExecuteScalar());


            docViewer.Document = "~/ReadingMaterials/" + materialFileName;
        }
    }
}