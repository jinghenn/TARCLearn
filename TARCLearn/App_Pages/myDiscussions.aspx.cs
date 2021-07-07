using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TARCLearn.App_Pages
{
    public partial class myDiscussions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                string userId = Session["userId"].ToString();
                
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection disCon = new SqlConnection(providerConStr);
                disCon.Open();

                //select data to be bound
                String strSelectDis = "Select d.threadTitle AS threadTitle, d.threadId AS threadId FROM DiscussionThread d FULL OUTER JOIN DiscussionMessage m ON d.threadId = m.threadId AND d.userId = m.userId WHERE d.userId=@userId AND m.userId=@userId;";
                SqlCommand cmdSelectDis = new SqlCommand(strSelectDis, disCon);
                cmdSelectDis.Parameters.AddWithValue("@userId", userId);

                rptDis.DataSource = cmdSelectDis.ExecuteReader();
                rptDis.DataBind();

                String strSelectEditDis = "SELECT d.threadTitle AS threadTitle,  d.threadId AS threadId FROM (SELECT * FROM [dbo].[DiscussionThread] WHERE userId=@userId) d JOIN (SELECT * FROM [dbo].[DiscussionMessage] WHERE userId=@userId) m ON d.userId = m.userId AND d.threadId = m.threadId ;";
                SqlCommand cmdSelectEditDis = new SqlCommand(strSelectEditDis, disCon);
                cmdSelectEditDis.Parameters.AddWithValue("@userId", userId);

                rptEditDiscussion.DataSource = cmdSelectEditDis.ExecuteReader();
                rptEditDiscussion.DataBind();

                disCon.Close();
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

        protected void btnMore_Click(object sender, ImageClickEventArgs e)
        {
            if (rptEditDiscussion.Visible == false)
            {
                rptDis.Visible = false;
                rptEditDiscussion.Visible = true;
            }
            else
            {
                Response.Redirect("myDiscussions.aspx");
            }
        }

        protected void rptEditDiscussion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string userType = Session["userType"].ToString();
            string userId = Session["userId"].ToString();
            string chapterId = Request.QueryString["chapterId"];
            String threadId = e.CommandArgument.ToString();
            TextBox txtDiscussionTitle = (TextBox)e.Item.FindControl("txtDiscussionTitle");


            ImageButton btnEdit = (ImageButton)e.Item.FindControl("btnEdit");
            ImageButton btnSave = (ImageButton)e.Item.FindControl("btnSave");
            ImageButton btnCancel = (ImageButton)e.Item.FindControl("btnCancel");
            ImageButton btnDel = (ImageButton)e.Item.FindControl("btnDelete");


            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection disCon = new SqlConnection(providerConStr);
            disCon.Open();


            if (e.CommandName == "edit")
            {
                txtDiscussionTitle.Enabled = true;
                txtDiscussionTitle.BorderStyle = BorderStyle.Inset;
                txtDiscussionTitle.BackColor = Color.White;

                btnEdit.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDel.Visible = false;


            }
            if (e.CommandName == "delete")
            {
                String strDel = "DELETE FROM DiscussionThread WHERE threadId=@threadId ";
                SqlCommand cmdDel = new SqlCommand(strDel, disCon);
                cmdDel.Parameters.AddWithValue("@threadId", threadId);
                cmdDel.ExecuteNonQuery();
                disCon.Close();
                Response.Redirect("myDiscussions.aspx");
            }
            if (e.CommandName == "save")
            {
                if (Page.IsValid)
                {

                    txtDiscussionTitle.Enabled = true;
                    txtDiscussionTitle.BorderStyle = BorderStyle.Inset;
                    txtDiscussionTitle.BackColor = Color.White;

                    btnEdit.Visible = true;
                    btnCancel.Visible = false;
                    btnDel.Visible = true;
                    btnSave.Visible = false;

                    SqlCommand cmdSelect = new SqlCommand("Select * from [dbo].[DiscussionThread] where threadTitle=@threadTitle", disCon);
                    cmdSelect.Parameters.AddWithValue("@threadTitle", txtDiscussionTitle.Text);
                    SqlDataReader dtrThreadTitle = cmdSelect.ExecuteReader();



                    if (!dtrThreadTitle.HasRows)
                    {
                        String edit = "UPDATE [dbo].[DiscussionThread] SET threadTitle = @threadTitle WHERE threadId = @threadId";
                        SqlCommand cmdEdit = new SqlCommand(edit, disCon);
                        cmdEdit.Parameters.AddWithValue("@threadTitle", txtDiscussionTitle.Text);
                        cmdEdit.Parameters.AddWithValue("@threadId", threadId);
                        cmdEdit.ExecuteNonQuery();
                        Response.Redirect("myDiscussions.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('Entered Thread Title Already Exists.')</script>");

                    }




                }

            }
            if (e.CommandName == "cancel")
            {
                Response.Redirect("myDiscussions.aspx");

            }
        }
    }
}