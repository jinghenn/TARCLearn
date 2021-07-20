using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TARCLearn.App_Pages
{
    public partial class TARCLearn : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = Session["userId"].ToString();
            if (userId == null)
            {
                System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
                string scriptKey = "ErrorMessage";

                javaScript.Append("var userConfirmation = window.confirm('" + "Your Session has Expired, Please login again.');\n");
                javaScript.Append("window.location='Login.aspx';");

                ScriptManager.RegisterStartupScript(this, this.GetType(), scriptKey, javaScript.ToString(), true);
                
            }

            if (Session["username"] != null)
            {
                lblUserName.Text = Session["username"].ToString();
                lblUserType.Text = Session["usertype"].ToString();

            }
            
            if (Session["usertype"].ToString() == "Student")
            {
                btnManage.Visible = false;
            }
        }

        protected void btnCourses_Click(object sender, EventArgs e)
        {
            Response.Redirect("course.aspx");
        }

        protected void btnManage_Click(object sender, EventArgs e)
        {
            Response.Redirect("manage.aspx");
        }

        protected void btnDN_Click(object sender, EventArgs e)
        {
            Response.Redirect("myDiscussions.aspx");
        }
    }
}