using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.IO;
using System.Drawing;
using System.Linq;

namespace TARCLearn.App_Pages
{
    public partial class readingMaterial : System.Web.UI.Page
    {
        Boolean isVideo;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string chapterId = Request.QueryString["chapterId"];
                string materialType = Request.QueryString["materialType"];
                if (Session["isDel"] == null)
                {
                    Session["isDel"] = "false";
                }


                if (materialType == "video")
                {
                    isVideo = true;
                    lblTittle.Text = "Video";
                    lblSupport.Text = "Supported file extensions : .flv, .mov, .wmv, .avi, .avchd, .f4v, .swf, .mkv, .webm, .html5', .mpeg-2 and .mp4";
                }
                else
                {
                    isVideo = false;
                    lblSupport.Text = "Supported file extensions : .pdf, .pptx, .docx, .xlsx, .jpg, .jpeg and .png";
                }


                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection materialCon = new SqlConnection(providerConStr);
                materialCon.Open();

                //select data to be bound
                
                

                TARCLearnEntities db = new TARCLearnEntities();
                int chapterIdINT = Convert.ToInt32(chapterId);
                
                var materialLec = db.Materials.Where(m => m.chapterId == chapterIdINT).Where(m => m.mode == "LECTURE").Where(m => m.isVideo == isVideo).OrderBy(m => m.index);
                rptLect.DataSource = materialLec.ToList(); ;
                rptLect.DataBind();

                var materialTut = db.Materials.Where(m => m.chapterId == chapterIdINT).Where(m => m.mode == "TUTORIAL").Where(m => m.isVideo == isVideo).OrderBy(m => m.index);
                rptTut.DataSource = materialTut.ToList(); ;
                rptTut.DataBind();

                var materialPrac = db.Materials.Where(m => m.chapterId == chapterIdINT).Where(m => m.mode == "PRACTICAL").Where(m => m.isVideo == isVideo).OrderBy(m => m.index);
                rptPrac.DataSource = materialPrac.ToList(); ;
                rptPrac.DataBind();

                var materialOth = db.Materials.Where(m => m.chapterId == chapterIdINT).Where(m => m.mode == "OTHER").Where(m => m.isVideo == isVideo).OrderBy(m => m.index);
                rptOth.DataSource = materialOth.ToList(); ;
                rptOth.DataBind();

                string userType = Session["userType"].ToString();
                if (userType == "Lecturer")
                {
                    var materialDelLec = db.Materials.Where(m => m.chapterId == chapterIdINT).Where(m => m.mode == "LECTURE").Where(m => m.isVideo == isVideo).OrderBy(m => m.index);
                    rptDelLect.DataSource = materialDelLec.ToList(); ;
                    rptDelLect.DataBind();

                    var materialDelTut = db.Materials.Where(m => m.chapterId == chapterIdINT).Where(m => m.mode == "TUTORIAL").Where(m => m.isVideo == isVideo).OrderBy(m => m.index);
                    rptDelTut.DataSource = materialDelTut.ToList(); ;
                    rptDelTut.DataBind();

                    var materialDelPrac = db.Materials.Where(m => m.chapterId == chapterIdINT).Where(m => m.mode == "PRACTICAL").Where(m => m.isVideo == isVideo).OrderBy(m => m.index);
                    rptDelPrac.DataSource = materialDelPrac.ToList(); ;
                    rptDelPrac.DataBind();

                    var materialDelOth = db.Materials.Where(m => m.chapterId == chapterIdINT).Where(m => m.mode == "OTHER").Where(m => m.isVideo == isVideo).OrderBy(m => m.index);
                    rptDelOth.DataSource = materialDelOth.ToList(); ;
                    rptDelOth.DataBind();

                    
                }
                else
                {
                    btnMore.Visible = false;
                    btnAdd.Visible = false;
                }

                materialCon.Close();
            }

        }

        protected void btnLecture_Click(object sender, EventArgs e)
        {
            String isDel = Session["isDel"].ToString();

            if (rptLect.Visible == true && isDel == "false")
            {
                rptLect.Visible = false;
            }
            else if (rptLect.Visible == false && isDel == "false")
            {
                rptLect.Visible = true;
            }
            else if (rptDelLect.Visible == false && isDel == "true")
            {
                rptDelLect.Visible = true;
            }
            else if (rptDelLect.Visible == true && isDel == "true")
            {
                rptDelLect.Visible = false;
            }
        }

        protected void btnPractical_Click(object sender, EventArgs e)
        {
            String isDel = Session["isDel"].ToString();

            if (rptPrac.Visible == true && isDel == "false")
            {
                rptPrac.Visible = false;
            }
            else if (rptPrac.Visible == false && isDel == "false")
            {
                rptPrac.Visible = true;
            }
            else if (rptDelPrac.Visible == false && isDel == "true")
            {
                rptDelPrac.Visible = true;
            }
            else if (rptDelPrac.Visible == true && isDel == "true")
            {
                rptDelPrac.Visible = false;
            }

        }

        protected void btnTutorial_Click(object sender, EventArgs e)
        {
            String isDel = Session["isDel"].ToString();

            if (rptTut.Visible == true && isDel == "false")
            {
                rptTut.Visible = false;
            }
            else if (rptTut.Visible == false && isDel == "false")
            {
                rptTut.Visible = true;
            }
            else if (rptDelTut.Visible == false && isDel == "true")
            {
                rptDelTut.Visible = true;
            }
            else if (rptDelTut.Visible == true && isDel == "true")
            {
                rptDelTut.Visible = false;
            }
        }

        protected void btnOther_Click(object sender, EventArgs e)
        {
            String isDel = Session["isDel"].ToString();

            if (rptOth.Visible == true && isDel == "false")
            {
                rptOth.Visible = false;
            }
            else if (rptOth.Visible == false && isDel == "false")
            {
                rptOth.Visible = true;
            }
            else if (rptDelOth.Visible == false && isDel == "true")
            {
                rptDelOth.Visible = true;
            }
            else if (rptDelOth.Visible == true && isDel == "true")
            {
                rptDelOth.Visible = false;
            }
        }

        protected void rptMaterial_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string materialType = Request.QueryString["materialType"];

            if (materialType == "video")
            {
                isVideo = true;

            }
            else
            {
                isVideo = false;
            }
            if (e.CommandName == "selectRM")
            {
                String materialId = e.CommandArgument.ToString();
                String url;
                if (isVideo)
                {
                    url = "videoViewer.aspx?materialId=" + materialId;
                }
                else
                {
                    url = "materialViewer.aspx?materialId=" + materialId;
                }

                Response.Redirect(url);

            }
        }

        protected void btnMore_Click(object sender, ImageClickEventArgs e)
        {
            String isDel = Session["isDel"].ToString();

            if (isDel == "false")
            {
                Session["isDel"] = "true";
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
                Session["isDel"] = "false";
                if (rptDelLect.Visible == true)
                {
                    rptDelLect.Visible = false;
                    rptLect.Visible = true;
                }
                if (rptDelPrac.Visible == true)
                {
                    rptDelPrac.Visible = false;
                    rptPrac.Visible = true;
                }
                if (rptDelTut.Visible == true)
                {
                    rptDelTut.Visible = false;
                    rptDelTut.Visible = true;
                }
                if (rptDelOth.Visible == true)
                {
                    rptDelOth.Visible = false;
                    rptOth.Visible = true;
                }



            }
        }

        protected void rptEdit_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //get materialId 
            string materialId = e.CommandArgument.ToString();
            string chapterId = Request.QueryString["chapterId"];
            string materialType = Request.QueryString["materialType"];

            if (materialType == "video")
            {
                isVideo = true;

            }
            else
            {
                isVideo = false;
            }

            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection materialCon = new SqlConnection(providerConStr);
            materialCon.Open();

            //get materialFileName
            SqlCommand cmdGetFileName = new SqlCommand("Select materialName from [dbo].[Material] where materialId=@materialId;", materialCon);
            cmdGetFileName.Parameters.AddWithValue("@materialId", materialId);
            String materialFileName = Convert.ToString(cmdGetFileName.ExecuteScalar());

            if (e.CommandName == "edit")
            {
                Session["materialId"] = materialId;
                int materialIdINT = Convert.ToInt32(materialId);
                TARCLearnEntities db = new TARCLearnEntities();
                var material = db.Materials.FirstOrDefault(m => m.materialId == materialIdINT);
                var index = material.index;
                formEditIndex.Text = Convert.ToString(index);

                SqlCommand cmdGetTitle = new SqlCommand("Select materialTitle from [dbo].[Material] where materialId=@materialId;", materialCon);
                cmdGetTitle.Parameters.AddWithValue("@materialId", materialId);
                formEditTitle.Text = Convert.ToString(cmdGetTitle.ExecuteScalar());

                SqlCommand cmdGetDesc = new SqlCommand("Select materialDescription from [dbo].[Material] where materialId=@materialId;", materialCon);
                cmdGetDesc.Parameters.AddWithValue("@materialId", materialId);
                formEditDescription.Text = Convert.ToString(cmdGetDesc.ExecuteScalar());

                SqlCommand cmdGetMode = new SqlCommand("Select mode from [dbo].[Material] where materialId=@materialId;", materialCon);
                cmdGetMode.Parameters.AddWithValue("@materialId", materialId);
                ddlFormEditMaterialMode.SelectedValue = Convert.ToString(cmdGetMode.ExecuteScalar());
                materialCon.Close();

                ClientScript.RegisterStartupScript(this.GetType(), "Pop", "editMaterial();", true);

            }           
            if (e.CommandName == "delete")
            {
                string file_name;

                if (isVideo)
                {
                    file_name = "~/videos/" + materialFileName;

                }
                else
                {
                    file_name = "~/ReadingMaterials/" + materialFileName;

                }

                string strPath = Server.MapPath(file_name);
                FileInfo file = new FileInfo(strPath);
                if (file.Exists)//check file exsit or not  
                {
                    System.IO.File.Delete(strPath);
                    String strDelRm = "DELETE FROM Material WHERE materialId=@materialId;";
                    SqlCommand cmdDelRm = new SqlCommand(strDelRm, materialCon);
                    cmdDelRm.Parameters.AddWithValue("@materialId", materialId);
                    cmdDelRm.ExecuteNonQuery();

                    materialCon.Close();

                    System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
                    string scriptKey = "SuccessMessage";
                    string url = "material.aspx?chapterId=" + chapterId + "&materialType=" + materialType;

                    javaScript.Append("var userConfirmation = window.confirm('" + "Successfully deleted!" + "');\n");
                    javaScript.Append("window.location='" + url + "';");

                    ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);

                }
                else
                {
                    Response.Write("<script>alert('File Does Not Exist.')</script>");


                }
            }
            
            
           
        }

        protected void addNewMaterialFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string materialType = Request.QueryString["materialType"];

                if (materialType == "video")
                {
                    isVideo = true;

                }
                else
                {
                    isVideo = false;
                }
                string description;
                string chapterId = Request.QueryString["chapterId"];
                string filepath;
                if (isVideo)
                {
                    filepath = "../videos/" + file.FileName;

                }
                else
                {
                    filepath = "../ReadingMaterials/" + file.FileName;

                }

                string strPath = Server.MapPath(filepath);
                FileInfo fileInfo = new FileInfo(strPath);



                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection materialCon = new SqlConnection(providerConStr);
                materialCon.Open();

                SqlCommand cmdSelectMaterialTitle = new SqlCommand("Select * from [dbo].[Material] where materialTitle=@materialTitle AND isVideo = @isVideo", materialCon);
                cmdSelectMaterialTitle.Parameters.AddWithValue("@materialTitle", formTitle.Text);
                cmdSelectMaterialTitle.Parameters.AddWithValue("@isVideo", isVideo);
                SqlDataReader dtrMaterialTitle = cmdSelectMaterialTitle.ExecuteReader();

                TARCLearnEntities db = new TARCLearnEntities();
                int chapterIdINT = Convert.ToInt32(chapterId);
                int newIndex = Convert.ToInt32(formIndex.Text);
                var dtrMaterialIndex = db.Materials.Where(m => m.chapterId == chapterIdINT).Where(m => m.mode == ddlFormEditMaterialMode.SelectedValue).Where(m => m.index == newIndex);


                if (!dtrMaterialTitle.HasRows && !fileInfo.Exists && !dtrMaterialIndex.Any())
                {
                    if (file.HasFile && file.PostedFile != null)
                    {
                        // Get the name of the file to upload.
                        string fileName = Server.HtmlEncode(file.FileName);


                        if (!isVideo)
                        {
                            string path = Server.MapPath("~/ReadingMaterials/" + file.FileName);
                            file.PostedFile.SaveAs(path);

                        }
                        else
                        {
                            string path = Server.MapPath("~/videos/" + file.FileName);
                            file.PostedFile.SaveAs(path);
                        }
                        if (formDescription.Text != null)
                        {
                            description = formDescription.Text;
                        }
                        else
                        {
                            description = null;
                        }

                       

                        String addMaterial = "INSERT INTO [dbo].[Material] VALUES(@index,@materialTitle,@materialDescription,@materialName,@isVideo,@mode,@chapterId);";
                        SqlCommand cmdAddMaterial = new SqlCommand(addMaterial, materialCon);

                        cmdAddMaterial.Parameters.AddWithValue("@index", formIndex.Text);
                        cmdAddMaterial.Parameters.AddWithValue("@materialTitle", formTitle.Text);
                        cmdAddMaterial.Parameters.AddWithValue("@materialDescription", description);
                        cmdAddMaterial.Parameters.AddWithValue("@materialName", file.FileName);
                        cmdAddMaterial.Parameters.AddWithValue("@isVideo", isVideo);
                        cmdAddMaterial.Parameters.AddWithValue("@mode", formMaterialMode.SelectedValue);
                        cmdAddMaterial.Parameters.AddWithValue("@chapterId", chapterId);

                        cmdAddMaterial.ExecuteNonQuery();
                        materialCon.Close();

                        string url = "material.aspx?chapterId=" + chapterId + "&materialType=" + materialType;
                        Response.Redirect(url);
                    }
                }
                else if(dtrMaterialTitle.HasRows && fileInfo.Exists && dtrMaterialIndex.Any())
                {
                    Response.Write("<script>alert('Entered Index, Title And Uploaded File Already Exists.')</script>");
                }
                else if (dtrMaterialTitle.HasRows && fileInfo.Exists)
                {
                    Response.Write("<script>alert('Both Entered Title And Uploaded File Already Exists.')</script>");
                }
                else if (dtrMaterialIndex.Any() && fileInfo.Exists)
                {
                    Response.Write("<script>alert('Both Entered Index And Uploaded File Already Exists.')</script>");
                }
                else if (dtrMaterialTitle.HasRows && dtrMaterialIndex.Any())
                {
                    Response.Write("<script>alert('Entered Index AND Title Already Exists.')</script>");
                }
                else if (dtrMaterialTitle.HasRows)
                {
                    Response.Write("<script>alert('Entered Title Already Exists.')</script>");
                }
                else if (dtrMaterialIndex.Any())
                {
                    Response.Write("<script>alert('Entered Index Already Exists.')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Uploaded File Already Exists.')</script>");
                }

            }




        }

        protected void editMaterialFormSubmitClicked(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {                 
                string materialId = Session["materialId"].ToString();
                string chapterId = Request.QueryString["chapterId"];
                string materialType = Request.QueryString["materialType"];

                if (materialType == "video")
                {
                    isVideo = true;

                }
                else
                {
                    isVideo = false;
                }
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection materialCon = new SqlConnection(providerConStr);
                materialCon.Open();

                //get materialFileName
                SqlCommand cmdGetFileName = new SqlCommand("Select materialName from [dbo].[Material] where materialId=@materialId;", materialCon);
                cmdGetFileName.Parameters.AddWithValue("@materialId", materialId);
                String materialFileName = Convert.ToString(cmdGetFileName.ExecuteScalar());

                SqlCommand cmdGetTitle = new SqlCommand("Select materialTitle from [dbo].[Material] where materialId=@materialId;", materialCon);
                cmdGetTitle.Parameters.AddWithValue("@materialId", materialId);
                String currentMaterialTitle = Convert.ToString(cmdGetTitle.ExecuteScalar());

                SqlCommand cmdGetMode = new SqlCommand("Select mode from [dbo].[Material] where materialId=@materialId;", materialCon);
                cmdGetMode.Parameters.AddWithValue("@materialId", materialId);
                String currentMaterialMode = Convert.ToString(cmdGetMode.ExecuteScalar());

                SqlCommand cmdGetDesc = new SqlCommand("Select materialDescription from [dbo].[Material] where materialId=@materialId;", materialCon);
                cmdGetDesc.Parameters.AddWithValue("@materialId", materialId);
                String currentMaterialDesc = Convert.ToString(cmdGetDesc.ExecuteScalar());

                int materialIdINT = Convert.ToInt32(materialId);
                TARCLearnEntities db = new TARCLearnEntities();
                var material = db.Materials.FirstOrDefault(m => m.materialId == materialIdINT);
                var index = material.index;
                String currentIndex = Convert.ToString(index);

                SqlCommand cmdSelectMaterialTitle = new SqlCommand("Select * from [dbo].[Material] where materialTitle=@materialTitle", materialCon);
                cmdSelectMaterialTitle.Parameters.AddWithValue("@materialTitle", formEditTitle.Text);
                SqlDataReader dtrMaterialTitle = cmdSelectMaterialTitle.ExecuteReader();

                int chapterIdINT = Convert.ToInt32(chapterId);
                int newIndex = Convert.ToInt32(formEditIndex.Text);
                var dtrMaterialIndex = db.Materials.Where(m => m.chapterId == chapterIdINT).Where(m => m.mode == ddlFormEditMaterialMode.SelectedValue).Where(m => m.index == newIndex);
               
                //When both title and index does not exist
                if (!dtrMaterialTitle.HasRows && !dtrMaterialIndex.Any())
                {
                    string newFileName;
                    string oldFileName;
                    string extension = System.IO.Path.GetExtension(materialFileName);

                    if (isVideo)
                    {
                        newFileName = "~/videos/" + formEditTitle.Text + extension;
                        oldFileName = "~/videos/" + materialFileName;
                    }
                    else
                    {
                        newFileName = "~/ReadingMaterials/" + formEditTitle.Text + extension;
                        oldFileName = "~/ReadingMaterials/" + materialFileName;
                    }

                    string newMaterialTitle = Server.MapPath(newFileName);
                    string oldMaterialTitle = Server.MapPath(oldFileName);

                    System.IO.File.Move(oldMaterialTitle, newMaterialTitle);                    

                    materialCon.Close();
                    var materialIndex = db.Materials.SingleOrDefault(m => m.materialId == materialIdINT);
                    
                    if (materialIndex != null)
                    {
                        materialIndex.materialTitle = formEditTitle.Text;
                        materialIndex.index = Convert.ToInt32(formEditIndex.Text);
                        materialIndex.materialName = formEditTitle.Text + extension;
                        materialIndex.mode = ddlFormEditMaterialMode.SelectedValue;
                        materialIndex.materialDescription = formEditDescription.Text;
                        db.SaveChanges();
                    }
                    System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
                    string scriptKey = "SuccessMessage";
                    string url = "material.aspx?chapterId=" + chapterId + "&materialType=" + materialType;

                    javaScript.Append("var userConfirmation = window.confirm('" + "Successfully updated!" + "');\n");
                    javaScript.Append("window.location='" + url + "';");

                    ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
                }
                //when both title and index exist
                else if (dtrMaterialTitle.HasRows && dtrMaterialIndex.Any())
                {
                    //check if the mode changed
                    if(ddlFormEditMaterialMode.SelectedValue != currentMaterialMode || formEditDescription.Text != currentMaterialDesc)
                    {
                        String editMaterial = "UPDATE [dbo].[Material] SET mode=@mode, materialDescription=@materialDescription WHERE materialId = @materialId";
                        SqlCommand cmdEditMaterial = new SqlCommand(editMaterial, materialCon);                        
                        cmdEditMaterial.Parameters.AddWithValue("@materialId", materialId);
                        cmdEditMaterial.Parameters.AddWithValue("@mode", ddlFormEditMaterialMode.SelectedValue);
                        cmdEditMaterial.Parameters.AddWithValue("@materialDescription", formEditDescription.Text);
                        cmdEditMaterial.ExecuteNonQuery();

                        materialCon.Close();

                        
                        System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
                        string scriptKey = "SuccessMessage";
                        string url = "material.aspx?chapterId=" + chapterId + "&materialType=" + materialType;                       

                        javaScript.Append("var userConfirmation = window.confirm('" + "Successfully updated!" + "');\n");
                        javaScript.Append("window.location='" + url + "';");

                        ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
                    }
                    else
                    {
                        materialCon.Close();
                        Response.Write("<script>alert('Both Entered Material Title and Index Already Exists.')</script>");
                    }

                }
                //when only index is change
                else if (dtrMaterialTitle.HasRows && (formEditTitle.Text == currentMaterialTitle) && !dtrMaterialIndex.Any())
                {
                    var materialIndex = db.Materials.SingleOrDefault(m => m.materialId == materialIdINT);

                    if (materialIndex != null)
                    {
                        materialIndex.index = Convert.ToInt32(formEditIndex.Text);
                        materialIndex.mode = ddlFormEditMaterialMode.SelectedValue;
                        materialIndex.materialDescription = formEditDescription.Text;
                        db.SaveChanges();
                    }                  

                    materialCon.Close();
                    System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
                    string scriptKey = "SuccessMessage";
                    string url = "material.aspx?chapterId=" + chapterId + "&materialType=" + materialType;

                    javaScript.Append("var userConfirmation = window.confirm('" + "Successfully updated!" + "');\n");
                    javaScript.Append("window.location='" + url + "';");

                    ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
                }
                //when title and index is change but title ard exist
                else if (dtrMaterialTitle.HasRows && (formEditTitle.Text != currentMaterialTitle) && !dtrMaterialIndex.Any())
                {
                    materialCon.Close();
                    Response.Write("<script>alert('Entered Title Already Exists.')</script>");

                }
                else if (!dtrMaterialTitle.HasRows && (formEditIndex.Text == currentIndex) && dtrMaterialIndex.Any())
                {
                    string newFileName;
                    string oldFileName;
                    string extension = System.IO.Path.GetExtension(materialFileName);

                    if (isVideo)
                    {
                        newFileName = "~/videos/" + formEditTitle.Text + extension;
                        oldFileName = "~/videos/" + materialFileName;
                    }
                    else
                    {
                        newFileName = "~/ReadingMaterials/" + formEditTitle.Text + extension;
                        oldFileName = "~/ReadingMaterials/" + materialFileName;
                    }

                    string newMaterialTitle = Server.MapPath(newFileName);
                    string oldMaterialTitle = Server.MapPath(oldFileName);

                    System.IO.File.Move(oldMaterialTitle, newMaterialTitle);

                    String editMaterial = "UPDATE [dbo].[Material] SET materialTitle=@materialTitle , mode=@mode, materialName=@materialName, materialDescription=@materialDescription WHERE materialId = @materialId";
                    SqlCommand cmdEditMaterial = new SqlCommand(editMaterial, materialCon);
                    cmdEditMaterial.Parameters.AddWithValue("@materialTitle", formEditTitle.Text);
                    cmdEditMaterial.Parameters.AddWithValue("@materialName", formEditTitle.Text + extension);
                    cmdEditMaterial.Parameters.AddWithValue("@mode", ddlFormEditMaterialMode.SelectedValue);
                    cmdEditMaterial.Parameters.AddWithValue("@materialDescription", formEditDescription.Text);
                    cmdEditMaterial.Parameters.AddWithValue("@materialId", materialId);
                    cmdEditMaterial.ExecuteNonQuery();

                    materialCon.Close();

                    System.Text.StringBuilder javaScript = new System.Text.StringBuilder();
                    string scriptKey = "SuccessMessage";
                    string url = "material.aspx?chapterId=" + chapterId + "&materialType=" + materialType;

                    javaScript.Append("var userConfirmation = window.confirm('" + "Successfully updated!" + "');\n");
                    javaScript.Append("window.location='" + url + "';");

                    ClientScript.RegisterStartupScript(this.GetType(), scriptKey, javaScript.ToString(), true);
                }
                else if (!dtrMaterialTitle.HasRows && (formEditIndex.Text != currentIndex) && dtrMaterialIndex.Any())
                {
                    materialCon.Close();
                    Response.Write("<script>alert('Entered Material Index Already Exists For This Chapter.')</script>");

                }
                
            }
        }
    }
}