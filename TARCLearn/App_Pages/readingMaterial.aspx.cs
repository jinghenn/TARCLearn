using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.IO;

namespace TARCLearn.App_Pages
{
    public partial class readingMaterial : System.Web.UI.Page
    {
        String isDel;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblTittle.Text = Convert.ToString(isDel);
                string chapterId = Request.QueryString["chapterId"];
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection materialCon = new SqlConnection(providerConStr);
                materialCon.Open();

                //select data to be bound
                String strSelectLect = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = 'false' AND mode = 'LECTURE';";
                SqlCommand cmdSelectLect = new SqlCommand(strSelectLect, materialCon);
                cmdSelectLect.Parameters.AddWithValue("@chapterId", chapterId);

                rptLect.DataSource = cmdSelectLect.ExecuteReader();
                rptLect.DataBind();

                String strDelLect = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = 'false' AND mode = 'LECTURE';";
                SqlCommand cmdDelLect = new SqlCommand(strDelLect, materialCon);
                cmdDelLect.Parameters.AddWithValue("@chapterId", chapterId);

                rptDelLect.DataSource = cmdDelLect.ExecuteReader();
                rptDelLect.DataBind();

                String strSelectTut = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = 'false' AND mode = 'TUTORIAL';";
                SqlCommand cmdSelectTut = new SqlCommand(strSelectTut, materialCon);
                cmdSelectTut.Parameters.AddWithValue("@chapterId", chapterId);

                rptTut.DataSource = cmdSelectTut.ExecuteReader();
                rptTut.DataBind();

                String strDelTut = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = 'false' AND mode = 'TUTORIAL';";
                SqlCommand cmdDelTut = new SqlCommand(strDelTut, materialCon);
                cmdDelTut.Parameters.AddWithValue("@chapterId", chapterId);

                rptDelTut.DataSource = cmdDelTut.ExecuteReader();
                rptDelTut.DataBind();

                String strSelectPrac = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = 'false' AND mode = 'PRACTICAL';";
                SqlCommand cmdSelectPrac = new SqlCommand(strSelectPrac, materialCon);
                cmdSelectPrac.Parameters.AddWithValue("@chapterId", chapterId);

                rptPrac.DataSource = cmdSelectPrac.ExecuteReader();
                rptPrac.DataBind();

                String strDelPrac = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = 'false' AND mode = 'PRACTICAL';";
                SqlCommand cmdDelPrac = new SqlCommand(strDelPrac, materialCon);
                cmdDelPrac.Parameters.AddWithValue("@chapterId", chapterId);

                rptDelPrac.DataSource = cmdDelPrac.ExecuteReader();
                rptDelPrac.DataBind();

                String strSelectOth = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = 'false' AND mode = 'OTHER';";
                SqlCommand cmdSelectOth = new SqlCommand(strSelectOth, materialCon);
                cmdSelectOth.Parameters.AddWithValue("@chapterId", chapterId);

                rptOth.DataSource = cmdSelectOth.ExecuteReader();
                rptOth.DataBind();

                String strDelOth = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = 'false' AND mode = 'OTHER';";
                SqlCommand cmdDelOth = new SqlCommand(strDelOth, materialCon);
                cmdDelOth.Parameters.AddWithValue("@chapterId", chapterId);

                rptDelOth.DataSource = cmdDelOth.ExecuteReader();
                rptDelOth.DataBind();

                materialCon.Close();
            }
            
        }

        protected void btnLecture_Click(object sender, EventArgs e)
        {
            if ((isDel == null))
            {
                isDel = "false";
           
            }
            if (rptLect.Visible == true && isDel == "false")
            {
                lblTittle.Text = Convert.ToString(isDel);
                rptLect.Visible = false;
            }
            else if(rptLect.Visible == false && isDel == "false")
            {
                lblTittle.Text = Convert.ToString(isDel);
                rptLect.Visible = true;
            }else if (isDel == "true") 
            {
                rptDelLect.Visible = true;
            }
        }

        protected void btnPractical_Click(object sender, EventArgs e)
        {
            if (rptPrac.Visible == true && isDel == "false")
            {
                rptPrac.Visible = false;
            }
            else if (rptPrac.Visible == false && isDel == "false")
            {
                rptPrac.Visible = true;
            }
            else if (isDel == "true")
            {
                rptDelPrac.Visible = true;
            }
        }

        protected void btnTutorial_Click(object sender, EventArgs e)
        {
            if (rptTut.Visible == true && isDel == "false")
            {
                rptTut.Visible = false;
            }
            else if (rptTut.Visible == false && isDel == "false")
            {
                rptTut.Visible = true;
            }
            else if (isDel == "true")
            {
                rptDelTut.Visible = true;
            }
        }

        protected void btnOther_Click(object sender, EventArgs e)
        {
            if(rptOth.Visible == true && isDel == "false")
            {
                rptOth.Visible = false;
            }
            else if (rptOth.Visible == false && isDel == "false")
            {
                rptOth.Visible = true;
            }
            else if (isDel == "true")
            {
                rptDelOth.Visible = true;
            }
        }

        protected void rmRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "selectRM")
            {
                String materialId = e.CommandArgument.ToString();
                String url = "pdfViewer.aspx?materialId=" + materialId;
                Response.Redirect(url);

            }
        }

        protected void btnDeleteRM_Click(object sender, ImageClickEventArgs e)
        {
            lblTittle.Text = Convert.ToString(isDel);
            if ((isDel == null))
            {
                isDel = "false";

            }
            if (isDel == "false")
            {
                isDel = "true";
                lblTittle.Text = Convert.ToString(isDel);
                if (rptLect.Visible == true)
                {
                    rptLect.Visible = false;
                    rptDelLect.Visible = true;
                }
                if (rptPrac.Visible == true)
                {
                    rptPrac.Visible = false;
                    rptDelPrac.Visible = true;
                }
                if (rptTut.Visible == true)
                {
                    rptTut.Visible = false;
                    rptDelTut.Visible = true;
                }
                if (rptOth.Visible == true)
                {
                    rptOth.Visible = false;
                    rptDelOth.Visible = true;
                }

            }
            else
            {
                isDel = "false";
                lblTittle.Text = Convert.ToString(isDel);
                if (rptDelLect.Visible == true)
                {
                    rptDelLect.Visible = false;
                }
                if (rptDelPrac.Visible == true)
                {
                    rptDelPrac.Visible = false;
                }
                if (rptDelTut.Visible == true)
                {
                    rptDelTut.Visible = false;
                }
                if (rptDelOth.Visible == true)
                {
                    rptDelOth.Visible = false;
                }
                                    
            }
        }
                
            

        

        protected void rptDeleteRM_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String materialId = e.CommandArgument.ToString();
            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection rmCon = new SqlConnection(providerConStr);
            rmCon.Open();
            if (e.CommandName == "deleteRM")
            {
                String strDelRm = "DELETE FROM Material WHERE materialId=@materialId;";
                SqlCommand cmdDelRm = new SqlCommand(strDelRm, rmCon);
                cmdDelRm.Parameters.AddWithValue("@materialId", materialId);
                cmdDelRm.ExecuteNonQuery();

                SqlCommand cmdGetRmTitle = new SqlCommand("Select materialTitle from [dbo].[Material] where materialId=@materialId;", rmCon);
                cmdGetRmTitle.Parameters.AddWithValue("@materialId", materialId);
                String materialTitle = Convert.ToString(cmdGetRmTitle.ExecuteScalar());

                rmCon.Close();

                string file_name = materialTitle;
                string path = Server.MapPath("../ReadingMaterials/" + file_name);
                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                   
                }
                

                string chapterId = Request.QueryString["chapterId"];
                String url = "readingMaterial.aspx?chapterId=" + chapterId;
                Response.Redirect(url);

            }
        }
    }

}