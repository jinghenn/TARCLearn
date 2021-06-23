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
    public partial class DiscussionThread : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string threadId = Request.QueryString["threadId"];

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection threadCon = new SqlConnection(providerConStr);
                threadCon.Open();

                String strGetTitle = "Select threadTitle FROM DiscussionThread WHERE threadId = @threadId;";
                SqlCommand cmdGetTitle = new SqlCommand(strGetTitle, threadCon);
                cmdGetTitle.Parameters.AddWithValue("@threadId", threadId);
                string title = Convert.ToString(cmdGetTitle.ExecuteScalar());
                txtTitle.Text = title;

                String strGetDesc = "Select threadDescription FROM DiscussionThread WHERE threadId = @threadId;";
                SqlCommand cmdGetDesc = new SqlCommand(strGetDesc, threadCon);
                cmdGetDesc.Parameters.AddWithValue("@threadId", threadId);
                string message = Convert.ToString(cmdGetDesc.ExecuteScalar());
                txtDesc.Text = message;

                String strGetCount = "SELECT COUNT(messageId) FROM DiscussionMessage WHERE threadId = @threadId;";
                SqlCommand cmdGetCount = new SqlCommand(strGetCount, threadCon);
                cmdGetCount.Parameters.AddWithValue("@threadId", threadId);
                string count = Convert.ToString(cmdGetCount.ExecuteScalar());
                lblCount.Text = count + " comments";



                //select data to be bound
                String strSelect = "Select d.message AS message, d.messageId AS messageId, u.username AS username FROM [dbo].[User] u, [dbo].[DiscussionMessage] d WHERE u.userId = d.userId AND threadId = @threadId;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, threadCon);
                cmdSelect.Parameters.AddWithValue("@threadId", threadId);
                rptComment.DataSource = cmdSelect.ExecuteReader();
                rptComment.DataBind();

                threadCon.Close();

            }
        }

        protected void rptComment_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string userId = Session["userId"].ToString();
            string threadId = Request.QueryString["threadId"];
            string messageId = e.CommandArgument.ToString();
            TextBox txtDiscussionComment = (TextBox)e.Item.FindControl("txtDiscussionComment");
            

            LinkButton btnEdit = (LinkButton)e.Item.FindControl("btnEdit");
            LinkButton btnSave = (LinkButton)e.Item.FindControl("btnSave");
            LinkButton btnCancel = (LinkButton)e.Item.FindControl("btnCancel");
            LinkButton btnDel = (LinkButton)e.Item.FindControl("btnDelete");


            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection threadCon = new SqlConnection(providerConStr);
            threadCon.Open();


            if (e.CommandName == "edit")
            {
                String strGetAll = "Select * FROM DiscussionMessage WHERE messageId = @messageId AND userId = @userId;";
                SqlCommand cmdGetAll = new SqlCommand(strGetAll, threadCon);
                cmdGetAll.Parameters.AddWithValue("@messageId", messageId);
                cmdGetAll.Parameters.AddWithValue("@userId", userId);
                SqlDataReader dtrMessage = cmdGetAll.ExecuteReader();

                if (dtrMessage.HasRows)
                {

                    txtDiscussionComment.Enabled = true;
                    txtDiscussionComment.BorderStyle = BorderStyle.Inset;
                    txtDiscussionComment.BackColor = Color.White;

                    btnEdit.Visible = false;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    btnDel.Visible = true;
                }
                else if(!dtrMessage.HasRows)
                {
                    
                    btnEdit.Visible = false;                   
                    btnCancel.Visible = true;
                }
                threadCon.Close();

            }
            if (e.CommandName == "delete")
            {
                String strDel = "DELETE FROM DiscussionMessage WHERE messageId=@messageId ";
                SqlCommand cmdDel = new SqlCommand(strDel, threadCon);
                cmdDel.Parameters.AddWithValue("@messageId", messageId);
                cmdDel.ExecuteNonQuery();
                threadCon.Close();
                string chapterId = Request.QueryString["chapterId"];
                String url = "DiscussionThread.aspx?threadId=" + threadId + "&chapterId=" + chapterId;
                Response.Redirect(url);
            }
            if (e.CommandName == "save")
            {
                if (Page.IsValid)
                {

                    txtDiscussionComment.Enabled = true;
                    txtDiscussionComment.BorderStyle = BorderStyle.Inset;
                    txtDiscussionComment.BackColor = Color.White;                  

                    btnEdit.Visible = true;
                    btnCancel.Visible = false;
                    btnDel.Visible = false;
                    btnSave.Visible = false;

                    
                    String editMsg = "UPDATE [dbo].[DiscussionMessage] SET message = @newMessage WHERE messageId = @messageId;";
                    SqlCommand cmdEditMsg = new SqlCommand(editMsg, threadCon);
                    cmdEditMsg.Parameters.AddWithValue("@newMessage", txtDiscussionComment.Text);                   
                    cmdEditMsg.Parameters.AddWithValue("@messageId", messageId);
                    cmdEditMsg.ExecuteNonQuery();

                    string chapterId = Request.QueryString["chapterId"];
                    String url = "DiscussionThread.aspx?threadId=" + threadId + "&chapterId=" + chapterId;
                    Response.Redirect(url);




                }

            }
            if (e.CommandName == "cancel")
            {

                txtDiscussionComment.Enabled = false;
                txtDiscussionComment.BorderStyle = BorderStyle.None;
                txtDiscussionComment.BackColor = Color.Transparent;                

                btnEdit.Visible = true;
                btnCancel.Visible = false;
                btnDel.Visible = false;
                btnSave.Visible = false;

                String strSelect = "Select d.message AS message, d.messageId AS messageId, u.username AS username FROM [dbo].[User] u, [dbo].[DiscussionMessage] d WHERE u.userId = d.userId AND threadId = @threadId;";
                SqlCommand cmdSelect = new SqlCommand(strSelect, threadCon);
                cmdSelect.Parameters.AddWithValue("@threadId", threadId);
                rptComment.DataSource = cmdSelect.ExecuteReader();
                rptComment.DataBind();
                threadCon.Close();
            }
        }

        protected void clearMessageButton_Click(object sender, EventArgs e)
        {
            txtComment.Text = "";
        }

        protected void sendMessageButton_Click(object sender, EventArgs e)
        {
            if(txtComment.Text != "")
            {
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection threadCon = new SqlConnection(providerConStr);
                threadCon.Open();

                String insert = "INSERT INTO [dbo].[DiscussionMessage] VALUES(@message,@userId,@threadId);";
                SqlCommand cmdInsert = new SqlCommand(insert, threadCon);
                cmdInsert.Parameters.AddWithValue("@message", txtComment.Text);
                cmdInsert.Parameters.AddWithValue("@userId", Session["userId"].ToString());
                cmdInsert.Parameters.AddWithValue("@threadId", Request.QueryString["threadId"]);
                cmdInsert.ExecuteNonQuery();
                threadCon.Close();

                string chapterId = Request.QueryString["chapterId"];
                String url = "DiscussionThread.aspx?threadId=" + Request.QueryString["threadId"] + "&chapterId=" + chapterId;
                Response.Redirect(url);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showContent();", true);

            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            txtTitle.Enabled = true;
            txtTitle.BorderStyle = BorderStyle.Inset;
            txtTitle.BackColor = Color.White;

            txtDesc.Enabled = true;
            txtDesc.BorderStyle = BorderStyle.Inset;
            txtDesc.BackColor = Color.White;

            btnEdit.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            btnDel.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["threadId"];
            txtTitle.Enabled = false;
            txtTitle.BorderStyle = BorderStyle.None;
            txtTitle.BackColor = Color.Transparent;

            txtDesc.Enabled = false;
            txtDesc.BorderStyle = BorderStyle.None;
            txtDesc.BackColor = Color.Transparent;

            btnEdit.Visible = true;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnDel.Visible = false;

            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection threadCon = new SqlConnection(providerConStr);
            threadCon.Open();

            String edit = "UPDATE [dbo].[DiscussionThread] SET threadTitle = @threadTitle, threadDescription = @threadDescription WHERE threadId=@threadId";
            SqlCommand cmdEdit = new SqlCommand(edit, threadCon);
            cmdEdit.Parameters.AddWithValue("@threadTitle", txtTitle.Text);
            cmdEdit.Parameters.AddWithValue("@threadDescription", txtDesc.Text);
            cmdEdit.Parameters.AddWithValue("@threadId", threadId);
            cmdEdit.ExecuteNonQuery();
            threadCon.Close();


            string chapterId = Request.QueryString["chapterId"];
            String url = "DiscussionThread.aspx?threadId=" + threadId + "&chapterId=" + chapterId;
            Response.Redirect(url);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["threadId"];
            txtTitle.Enabled = false;
            txtTitle.BorderStyle = BorderStyle.None;
            txtTitle.BackColor = Color.Transparent;

            txtDesc.Enabled = false;
            txtDesc.BorderStyle = BorderStyle.None;
            txtDesc.BackColor = Color.Transparent;

            btnEdit.Visible = true;
            btnSave.Visible = false;
            btnCancel.Visible = false;
            btnDel.Visible = false;

            string chapterId = Request.QueryString["chapterId"];
            String url = "DiscussionThread.aspx?threadId=" + threadId + "&chapterId=" + chapterId;
            Response.Redirect(url);
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            string threadId = Request.QueryString["threadId"];
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection threadCon = new SqlConnection(providerConStr);
            threadCon.Open();

           

            String strDel = "DELETE FROM DiscussionThread WHERE threadId=@threadId ";
            SqlCommand cmdDel = new SqlCommand(strDel, threadCon);
            cmdDel.Parameters.AddWithValue("@threadId", threadId);
            cmdDel.ExecuteNonQuery();
            threadCon.Close();

            string chapterId = Request.QueryString["chapterId"];         
            string url = "Discussion.aspx?chapterId=" + chapterId;
            Response.Redirect(url);
        }
    }
}