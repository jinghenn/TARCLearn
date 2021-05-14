using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TARCLearn.App_Pages
{
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
           
            bool isLect;
            if(rblIsLecturer.SelectedValue == "True")
            {
                isLect = true;
            }
            else
            {
                isLect = false;
            }

            using(var context = new TARCLearnEntities())
            {
                var newUser = new User
                {
                    userId = txtUserId.Text,
                    password = txtPassword.Text,
                    username = txtUsername.Text,
                    isLecturer = isLect
                };
                context.Users.Add(newUser);
                context.SaveChanges();
            }
            



        }
    }
}