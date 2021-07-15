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
    public partial class Discussionaspx : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                string userId = Session["userId"].ToString();
                string chapterId = Request.QueryString["chapterId"];
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection disCon = new SqlConnection(providerConStr);
                disCon.Open();

                //select data to be bound
                String strSelectDis = "Select threadTitle AS threadTitle,  threadId AS threadId FROM DiscussionThread WHERE chapterId=@chapterId;";
                SqlCommand cmdSelectDis = new SqlCommand(strSelectDis, disCon);
                cmdSelectDis.Parameters.AddWithValue("@chapterId", chapterId);

                disRepeater.DataSource = cmdSelectDis.ExecuteReader();
                disRepeater.DataBind();

                String strSelectEditDis = "Select threadTitle AS threadTitle,  threadId AS threadId FROM DiscussionThread WHERE chapterId=@chapterId;";
                SqlCommand cmdSelectEditDis = new SqlCommand(strSelectEditDis, disCon);
                cmdSelectEditDis.Parameters.AddWithValue("@chapterId", chapterId);

                rptEditDiscussion.DataSource = cmdSelectEditDis.ExecuteReader();
                rptEditDiscussion.DataBind();

                SqlCommand cmdGetCourseId = new SqlCommand("Select courseId from [dbo].[Chapter] where chapterId=@chapterId;", disCon);
                cmdGetCourseId.Parameters.AddWithValue("@chapterId", chapterId);
                string courseId = Convert.ToString(cmdGetCourseId.ExecuteScalar());

                lblHome.Text = "<a href = 'course.aspx'> Home </a>";
                lblChp.Text = "<a href = 'Chapter.aspx?courseId=" + courseId + "'> Chapter </a>";

                disCon.Close();
            }
        }

        public void successMsg(string msg, string id)
        {
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
            string scriptKey = "SuccessMessage";
            string url = "Discussion.aspx?chapterId=" + id;

            javaScript.Append("var userConfirmation = window.confirm('" + "Successfully "+ msg + "');\n");
            javaScript.Append("window.location='" + url + "';");

            ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
        }

        protected void addDiscussionFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string chapterId = Request.QueryString["chapterId"];
                string threadTitle = formDisTitle.Text;
                string userId = Session["userId"].ToString();
                string threadDescription = formDisDesc.Text;


                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection disCon = new SqlConnection(providerConStr);
                disCon.Open();

                SqlCommand cmdSelectDisTitle = new SqlCommand("Select * from [dbo].[DiscussionThread] where threadTitle=@threadTitle AND chapterId=@chapterId;", disCon);
                cmdSelectDisTitle.Parameters.AddWithValue("@threadTitle", threadTitle);
                cmdSelectDisTitle.Parameters.AddWithValue("@chapterId", chapterId);
                SqlDataReader dtrDisTitle = cmdSelectDisTitle.ExecuteReader();

                if (!dtrDisTitle.HasRows)
                {
                    String addDis = "INSERT INTO [dbo].[DiscussionThread] VALUES(@threadTitle,@threadDescription,@chapterId,@userId);";
                    SqlCommand cmdAddDis = new SqlCommand(addDis, disCon);

                    cmdAddDis.Parameters.AddWithValue("@threadTitle", threadTitle);
                    cmdAddDis.Parameters.AddWithValue("@threadDescription", threadDescription);
                    cmdAddDis.Parameters.AddWithValue("@chapterId", chapterId);
                    cmdAddDis.Parameters.AddWithValue("@userId", userId);
                    cmdAddDis.ExecuteNonQuery();
                    disCon.Close();
                    disRepeater.DataBind();
                    successMsg("added", chapterId);
                }
                else
                {
                    Response.Write("<script>alert('Discussion Title Arealdy Exist.')</script>");
                }



            }



        }

        protected void courseRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String threadId = e.CommandArgument.ToString();
            string chapterId = Request.QueryString["chapterId"];
            if (e.CommandName == "selectDiscussion")
            {

                String url = "discussionThreads.aspx?threadId=" + threadId + "&chapterId=" + chapterId;
                Response.Redirect(url);

            }
        }
        protected void rptEditDiscussion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImageButton btnDelete = (ImageButton)e.Item.FindControl("btnDelete");
                ImageButton btnEdit = (ImageButton)e.Item.FindControl("btnEdit");

                string userId = Session["userId"].ToString();
                string userType = Session["userType"].ToString();
               
                string threadId = DataBinder.Eval(e.Item.DataItem, "threadId").ToString();

                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection threadCon = new SqlConnection(providerConStr);
                threadCon.Open();

                String strGetUser = "SELECT userId FROM DiscussionThread WHERE threadId = @threadId;";
                SqlCommand cmdGetUser = new SqlCommand(strGetUser, threadCon);
                cmdGetUser.Parameters.AddWithValue("@threadId", threadId);
                string currentThreadOwner = Convert.ToString(cmdGetUser.ExecuteScalar());

                if (currentThreadOwner != userId)
                {
                    if (userType == "Lecturer")
                    {
                        btnEdit.Visible = false;
                    }
                    else
                    {
                        btnEdit.Visible = false;
                        btnDelete.Visible = false;
                    }
                }

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
                successMsg("deleted", chapterId);
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
                        successMsg("updated", chapterId);
                    }
                    else
                    {
                        Response.Write("<script>alert('Entered Thread Title Already Exists.')</script>");

                    }
                    



                }

            }
            if (e.CommandName == "cancel")
            {
                String url = "Discussion.aspx?chapterId=" + chapterId;
                Response.Redirect(url);

            }
        }

        protected void btnMore_Click(object sender, ImageClickEventArgs e)
        {
            string userType = Session["userType"].ToString();
            string userId = Session["userId"].ToString();
            string chapterId = Request.QueryString["chapterId"];

            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection disCon = new SqlConnection(providerConStr);
            disCon.Open();
            
            if (userType == "Lecturer")
            {             
                SqlCommand cmdSelectDis = new SqlCommand("Select * from [dbo].[DiscussionThread] where chapterId=@chapterId", disCon);
                cmdSelectDis.Parameters.AddWithValue("@chapterId", chapterId);
                SqlDataReader dtrUser = cmdSelectDis.ExecuteReader();
                if (dtrUser.HasRows) {
                    if (rptEditDiscussion.Visible == false)
                    {
                        disRepeater.Visible = false;
                        rptEditDiscussion.Visible = true;
                    }
                    else
                    {                        
                        String url = "Discussion.aspx?chapterId=" + chapterId;
                        Response.Redirect(url);
                    }
                }
                else
                {
                    Response.Write("<script>alert('There are No Discussion Thread in this chapter for Edit.')</script>");
                }
            }
            else
            {
                SqlCommand cmdSelectDis = new SqlCommand("Select * from [dbo].[DiscussionThread] where chapterId=@chapterId AND userId = @userId", disCon);
                cmdSelectDis.Parameters.AddWithValue("@chapterId", chapterId);
                cmdSelectDis.Parameters.AddWithValue("@userId", userId);
                SqlDataReader dtrUser = cmdSelectDis.ExecuteReader();
                if (dtrUser.HasRows)
                {
                    if (rptEditDiscussion.Visible == false)
                    {
                        disRepeater.Visible = false;
                        rptEditDiscussion.Visible = true;
                    }
                    else
                    {
                        String url = "Discussion.aspx?chapterId=" + chapterId;
                        Response.Redirect(url);
                    }
                }
                else
                {
                    Response.Write("<script>alert('You Do Not Have Any Discussion Thread for Edit.')</script>");
                }

            
            }
        }
    }
}