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
    }
}