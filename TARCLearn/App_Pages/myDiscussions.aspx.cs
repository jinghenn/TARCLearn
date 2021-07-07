using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TARCLearn.Models;

namespace TARCLearn.App_Pages
{
    public partial class myDiscussions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {          

                TARCLearnEntities db = new TARCLearnEntities();

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:44348/api/"); //change the base address to match your current address

                var userId = Session["userId"].ToString(); //change to any user id
                var consumeApi = client.GetAsync($"users/{userId}/discussions");
                consumeApi.Wait();
                var result = consumeApi.Result;
                if (result.IsSuccessStatusCode)
                {
                    var resultRecords = result.Content.ReadAsAsync<IList<DiscussionThreadDetailDto>>();
                    resultRecords.Wait();
                    var discussionThreads = resultRecords.Result;
                    rptDis.DataSource = discussionThreads;
                    rptDis.DataBind();

               

                }



               
            }
        }

        protected void courseRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String threadId = e.CommandArgument.ToString();

            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection disCon = new SqlConnection(providerConStr);
            disCon.Open();

            SqlCommand cmdGetchpId = new SqlCommand("Select chapterId from [dbo].[DiscussionThread] where threadId=@threadId;", disCon);
            cmdGetchpId.Parameters.AddWithValue("@threadId", threadId);
            String chapterId = Convert.ToString(cmdGetchpId.ExecuteScalar());

            disCon.Close();
            if (e.CommandName == "selectDiscussion")
            {

                String url = "discussionThreads.aspx?threadId=" + threadId + "&chapterId=" + chapterId;
                Response.Redirect(url);

            }
        }

        




              
    }
}