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
    public partial class discussionThreads : System.Web.UI.Page
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

                string userId = Session["userId"].ToString();
                string userType = Session["userType"].ToString();

                String strGetUser = "SELECT userId FROM DiscussionThread WHERE threadId = @threadId;";
                SqlCommand cmdGetUser = new SqlCommand(strGetUser, threadCon);
                cmdGetUser.Parameters.AddWithValue("@threadId", threadId);
                string currentThreadOwner = Convert.ToString(cmdGetUser.ExecuteScalar());

                if (currentThreadOwner != userId)
                {
                    if (userType == "Lecturer")
                    {
                        btnEditDT.Visible = false;
                    }
                    else
                    {
                        btnEditDT.Visible = false;
                        btnDeleteDT.Visible = false;
                    }
                }

                SqlCommand cmdGetChpId = new SqlCommand("Select chapterId from [dbo].[DiscussionThread] where threadId=@threadId;", threadCon);
                cmdGetChpId.Parameters.AddWithValue("@threadId", threadId);
                string chapterId = Convert.ToString(cmdGetChpId.ExecuteScalar());

                SqlCommand cmdGetCourseId = new SqlCommand("Select courseId from [dbo].[Chapter] where chapterId=@chapterId;", threadCon);
                cmdGetCourseId.Parameters.AddWithValue("@chapterId", chapterId);
                string courseId = Convert.ToString(cmdGetCourseId.ExecuteScalar());                
               
                lblHome.Text = "<a href = 'course.aspx'> Home </a>";
                lblChp.Text = "<a href = 'Chapter.aspx?courseId=" + courseId + "'> Chapter </a>";
                lblDis.Text = "<a href = 'Discussion.aspx?chapterId=" + chapterId + "'> Discussion Thread </a>";
                lblDisTitle.Text = title;

                threadCon.Close();
            }
        }

        public void successMsg(string msg, string threadId, string chapterId)
        {
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
            string scriptKey = "SuccessMessage";
            string url = "discussionThreads.aspx?threadId=" + threadId + "&chapterId=" + chapterId;

            javaScript.Append("var userConfirmation = window.confirm('" + "Successfully " + msg + "');\n");
            javaScript.Append("window.location='" + url + "';");

            ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
        }

        protected void rptComment_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            // determines which position in the outer layer repeater in the repeater (AlternatingItemTemplate, FooterTemplate,

            //HeaderTemplate，，ItemTemplate，SeparatorTemplate）
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImageButton btnDeleteC = (ImageButton)e.Item.FindControl("btnDeleteC");
                ImageButton btnEditC = (ImageButton)e.Item.FindControl("btnEditC");

                string userId = Session["userId"].ToString();
                string userType = Session["userType"].ToString();

                string threadId = Request.QueryString["threadId"];
                string messageId = DataBinder.Eval(e.Item.DataItem, "messageId").ToString();

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection threadCon = new SqlConnection(providerConStr);
                threadCon.Open();

                String strGetUser = "SELECT userId FROM DiscussionMessage WHERE messageId = @messageId;";
                SqlCommand cmdGetUser = new SqlCommand(strGetUser, threadCon);
                cmdGetUser.Parameters.AddWithValue("@messageId", messageId);
                string currentThreadOwner = Convert.ToString(cmdGetUser.ExecuteScalar());

                if (currentThreadOwner != userId)
                {
                    if (userType == "Lecturer")
                    {
                        btnEditC.Visible = false;
                    }
                    else
                    {
                        btnEditC.Visible = false;
                        btnDeleteC.Visible = false;
                    }
                }
                
            }
        }
        protected void rptComment_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string chapterId = Request.QueryString["chapterId"];
            string userId = Session["userId"].ToString();
            string threadId = Request.QueryString["threadId"];
            string messageId = e.CommandArgument.ToString();
            TextBox txtDiscussionComment = (TextBox)e.Item.FindControl("txtDiscussionComment");


            ImageButton btnEdit = (ImageButton)e.Item.FindControl("btnEditC");
            ImageButton btnSave = (ImageButton)e.Item.FindControl("btnSaveC");
            ImageButton btnCancel = (ImageButton)e.Item.FindControl("btnCancelC");
            ImageButton btnDel = (ImageButton)e.Item.FindControl("btnDeleteC");


            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection threadCon = new SqlConnection(providerConStr);
            threadCon.Open();


            if (e.CommandName == "edit")
            {
                
                txtDiscussionComment.Enabled = true;
                txtDiscussionComment.BorderStyle = BorderStyle.Inset;
                txtDiscussionComment.BackColor = Color.White;

                btnEdit.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDel.Visible = false;
               
                threadCon.Close();

            }
            if (e.CommandName == "delete")
            {
                String strDel = "DELETE FROM DiscussionMessage WHERE messageId=@messageId ";
                SqlCommand cmdDel = new SqlCommand(strDel, threadCon);
                cmdDel.Parameters.AddWithValue("@messageId", messageId);
                cmdDel.ExecuteNonQuery();
                threadCon.Close();
                
                successMsg( "deleted", threadId, chapterId);
                
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
                    btnDel.Visible = true;
                    btnSave.Visible = false;

                    String editMsg = "UPDATE [dbo].[DiscussionMessage] SET message = @newMessage WHERE messageId = @messageId;";
                    SqlCommand cmdEditMsg = new SqlCommand(editMsg, threadCon);
                    cmdEditMsg.Parameters.AddWithValue("@newMessage", txtDiscussionComment.Text);
                    cmdEditMsg.Parameters.AddWithValue("@messageId", messageId);
                    cmdEditMsg.ExecuteNonQuery();
                    
                    successMsg("updated", threadId, chapterId);                    

                }

            }
            if (e.CommandName == "cancel")
            {              
                String url = "discussionThreads.aspx?threadId=" + threadId + "&chapterId=" + chapterId;
                Response.Redirect(url);
            }
        }

        protected void clearMessageButton_Click(object sender, EventArgs e)
        {
            txtComment.Text = "";
        }

        protected void sendMessageButton_Click(object sender, EventArgs e)
        {
            if (txtComment.Text != "")
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
                String url = "discussionThreads.aspx?threadId=" + Request.QueryString["threadId"] + "&chapterId=" + chapterId;
                Response.Redirect(url);
            }
            
        }

        protected void btnDeleteDT_Click(object sender, ImageClickEventArgs e)
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
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
            string scriptKey = "SuccessMessage";
            string url = "Discussion.aspx?chapterId=" + chapterId;

            javaScript.Append("var userConfirmation = window.confirm('" + "Successfully deleted!');\n");
            javaScript.Append("window.location='" + url + "';");

            ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
           
        }

        protected void btnEditDT_Click(object sender, ImageClickEventArgs e)
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
            formDisTitle.Text = title;

            String strGetDesc = "Select threadDescription FROM DiscussionThread WHERE threadId = @threadId;";
            SqlCommand cmdGetDesc = new SqlCommand(strGetDesc, threadCon);
            cmdGetDesc.Parameters.AddWithValue("@threadId", threadId);
            string message = Convert.ToString(cmdGetDesc.ExecuteScalar());
            formDisDesc.Text = message;

            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
        }

        protected void editDiscussionFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string chapterId = Request.QueryString["chapterId"];
                string threadId = Request.QueryString["threadId"];
                string threadTitle = formDisTitle.Text;
                string userId = Session["userId"].ToString();
                string threadDescription = formDisDesc.Text;
                

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection disCon = new SqlConnection(providerConStr);
                disCon.Open();

                SqlCommand cmdSelectCTitle = new SqlCommand("Select threadTitle from [dbo].[DiscussionThread] where threadId=@threadId;", disCon);
                cmdSelectCTitle.Parameters.AddWithValue("@threadId", threadId);              
                string currentTitle = Convert.ToString(cmdSelectCTitle.ExecuteScalar());            

                SqlCommand cmdSelectCDecs = new SqlCommand("Select threadDescription from [dbo].[DiscussionThread] where threadId=@threadId;", disCon);
                cmdSelectCDecs.Parameters.AddWithValue("@threadId", threadId);
                string currentDesc = Convert.ToString(cmdSelectCDecs.ExecuteScalar());

                SqlCommand cmdSelectDisTitle = new SqlCommand("Select * from [dbo].[DiscussionThread] where threadTitle=@threadTitle AND chapterId=@chapterId;", disCon);
                cmdSelectDisTitle.Parameters.AddWithValue("@threadTitle", threadTitle);
                cmdSelectDisTitle.Parameters.AddWithValue("@chapterId", chapterId);
                SqlDataReader dtrDisTitle = cmdSelectDisTitle.ExecuteReader();

                SqlCommand cmdSelectDisDesc = new SqlCommand("Select * from [dbo].[DiscussionThread] where threadDescription=@threadDescription AND chapterId=@chapterId;", disCon);
                cmdSelectDisDesc.Parameters.AddWithValue("@threadDescription", threadDescription);
                cmdSelectDisDesc.Parameters.AddWithValue("@chapterId", chapterId);
                SqlDataReader dtrDisDesc = cmdSelectDisDesc.ExecuteReader();

                if (!dtrDisTitle.HasRows && !dtrDisDesc.HasRows)
                {
                    String addDis = "UPDATE [dbo].[DiscussionThread] SET threadTitle=@threadTitle, threadDescription=@threadDescription WHERE threadId=@threadId;";
                    SqlCommand cmdAddDis = new SqlCommand(addDis, disCon);

                    cmdAddDis.Parameters.AddWithValue("@threadTitle", threadTitle);
                    cmdAddDis.Parameters.AddWithValue("@threadDescription", threadDescription);
                    cmdAddDis.Parameters.AddWithValue("@chapterId", chapterId);
                    cmdAddDis.Parameters.AddWithValue("@userId", userId);
                    cmdAddDis.ExecuteNonQuery();
                    disCon.Close();

                    successMsg("updated", threadId, chapterId);
                    
                }
                else if (dtrDisTitle.HasRows && dtrDisDesc.HasRows)
                {

                    disCon.Close();
                    Response.Write("<script>alert('Both Entered Discussion Title and Discussion Description Already Exists.')</script>");
                    

                }
                else if (dtrDisTitle.HasRows && (threadTitle == currentTitle) && !dtrDisDesc.HasRows)
                {
                    String editDis = "UPDATE [dbo].[DiscussionThread] SET threadDescription=@threadDescription WHERE threadId=@threadId;";
                    SqlCommand cmdEditDis = new SqlCommand(editDis, disCon);
                    cmdEditDis.Parameters.AddWithValue("@threadDescription", threadDescription);
                    cmdEditDis.Parameters.AddWithValue("@threadId", threadId);

                    cmdEditDis.ExecuteNonQuery();
                    disCon.Close();
                    successMsg("updated", threadId, chapterId);
                }
                else if (dtrDisTitle.HasRows && (threadTitle != currentTitle) && !dtrDisDesc.HasRows)
                {
                    disCon.Close();                   
                    Response.Write("<script>alert('Entered Discussion Title Already Exists.')</script>");

                }
                else if (!dtrDisTitle.HasRows && (threadDescription == currentDesc) && dtrDisDesc.HasRows)
                {
                    String editDis = "UPDATE [dbo].[DiscussionThread] SET threadTitle=@threadTitle WHERE threadId=@threadId;";
                    SqlCommand cmdEditDis = new SqlCommand(editDis, disCon);
                    cmdEditDis.Parameters.AddWithValue("@threadTitle", threadTitle);
                    cmdEditDis.Parameters.AddWithValue("@threadId", threadId);

                    cmdEditDis.ExecuteNonQuery();
                    disCon.Close();
                    successMsg("updated", threadId, chapterId);
                }
                else if (!dtrDisTitle.HasRows && (threadDescription != currentDesc) && dtrDisDesc.HasRows)
                {
                    disCon.Close();
                    Response.Write("<script>alert('Entered Discussion Description Already Exists.')</script>");

                }

            }
        }
            
                



            



        
    }
}