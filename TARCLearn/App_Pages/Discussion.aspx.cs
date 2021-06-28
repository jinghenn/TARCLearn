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
                string userType = Session["userType"].ToString();
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

                if(userType == "Lecturer")
                {
                    String strSelectEditDis = "Select threadTitle AS threadTitle,  threadId AS threadId FROM DiscussionThread WHERE chapterId=@chapterId;";
                    SqlCommand cmdSelectEditDis = new SqlCommand(strSelectEditDis, disCon);
                    cmdSelectEditDis.Parameters.AddWithValue("@chapterId", chapterId);

                    rptEditDiscussion.DataSource = cmdSelectEditDis.ExecuteReader();
                    rptEditDiscussion.DataBind();
                }
                else
                {
                    String strSelectEditDis = "Select threadTitle AS threadTitle,  threadId AS threadId FROM DiscussionThread WHERE chapterId=@chapterId AND userId = @userId;";
                    SqlCommand cmdSelectEditDis = new SqlCommand(strSelectEditDis, disCon);
                    cmdSelectEditDis.Parameters.AddWithValue("@chapterId", chapterId);
                    cmdSelectEditDis.Parameters.AddWithValue("@userId", userId);
                    rptEditDiscussion.DataSource = cmdSelectEditDis.ExecuteReader();
                    rptEditDiscussion.DataBind();

                    btnMore.Visible = false;
                    btnAdd.Visible = false;

                }



                disCon.Close();
            }
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
                    String url = "Discussion.aspx?chapterId=" + chapterId;
                    Response.Redirect(url);
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
                
                String url = "discussionThreads.aspx?threadId=" + threadId + "&chapterId="+ chapterId;
                Response.Redirect(url);

            }
        }

        protected void rptEditDiscussion_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string userType = Session["userType"].ToString();
            string userId = Session["userId"].ToString();
            string chapterId = Request.QueryString["chapterId"];
            String threadId = e.CommandArgument.ToString();
            TextBox txtDiscussionTitle = (TextBox)e.Item.FindControl("txtDiscussionTitle");
           

            LinkButton btnEdit = (LinkButton)e.Item.FindControl("btnEdit");
            LinkButton btnSave = (LinkButton)e.Item.FindControl("btnSave");
            LinkButton btnCancel = (LinkButton)e.Item.FindControl("btnCancel");
            LinkButton btnDel = (LinkButton)e.Item.FindControl("btnDelete");


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
                btnDel.Visible = true;


            }
            if (e.CommandName == "delete")
            {
                String strDel = "DELETE FROM DiscussionThread WHERE threadId=@threadId ";
                SqlCommand cmdDel = new SqlCommand(strDel, disCon);
                cmdDel.Parameters.AddWithValue("@threadId", threadId);
                cmdDel.ExecuteNonQuery();
                disCon.Close();
                String url = "Discussion.aspx?chapterId=" + chapterId;
                Response.Redirect(url);
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
                    btnDel.Visible = false;
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
                        String url = "Discussion.aspx?courseId=" + chapterId;
                        Response.Redirect(url);
                    }
                    else
                    {
                        Response.Write("<script>alert('Entered Thread Title Already Exists.')</script>");

                    }
                    



                }

            }
            if (e.CommandName == "cancel")
            {

                txtDiscussionTitle.Enabled = false;
                txtDiscussionTitle.BorderStyle = BorderStyle.None;
                txtDiscussionTitle.BackColor = Color.Transparent;
             
                btnEdit.Visible = true;
                btnCancel.Visible = false;
                btnDel.Visible = false;
                btnSave.Visible = false;

                if (userType == "Lecturer")
                {
                    String strSelectEditDis = "Select threadTitle AS threadTitle,  threadId AS threadId FROM DiscussionThread WHERE chapterId=@chapterId;";
                    SqlCommand cmdSelectEditDis = new SqlCommand(strSelectEditDis, disCon);
                    cmdSelectEditDis.Parameters.AddWithValue("@chapterId", chapterId);

                    rptEditDiscussion.DataSource = cmdSelectEditDis.ExecuteReader();
                    rptEditDiscussion.DataBind();
                }
                else
                {
                    String strSelectEditDis = "Select threadTitle AS threadTitle,  threadId AS threadId FROM DiscussionThread WHERE chapterId=@chapterId AND userId = @userId;";
                    SqlCommand cmdSelectEditDis = new SqlCommand(strSelectEditDis, disCon);
                    cmdSelectEditDis.Parameters.AddWithValue("@chapterId", chapterId);
                    cmdSelectEditDis.Parameters.AddWithValue("@userId", userId);
                    rptEditDiscussion.DataSource = cmdSelectEditDis.ExecuteReader();
                    rptEditDiscussion.DataBind();
                }

            }
        }

        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
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