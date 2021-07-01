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
using System.Drawing;
using GroupDocs.Viewer;
using GroupDocs.Viewer.Options;

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
                    lblSupport.Text = "Supported file extensions : .mp4";
                }
                else
                {
                    isVideo = false;
                    lblSupport.Text = "Supported file extensions : .pdf";
                }
                
                
                string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
                string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
                SqlConnection materialCon = new SqlConnection(providerConStr);
                materialCon.Open();

                //select data to be bound
                String strSelectLect = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = @isVideo AND mode = 'LECTURE';";
                SqlCommand cmdSelectLect = new SqlCommand(strSelectLect, materialCon);
                cmdSelectLect.Parameters.AddWithValue("@chapterId", chapterId);
                cmdSelectLect.Parameters.AddWithValue("@isVideo", isVideo);
                rptLect.DataSource = cmdSelectLect.ExecuteReader();
                rptLect.DataBind();

                

                String strSelectTut = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = @isVideo AND mode = 'TUTORIAL';";
                SqlCommand cmdSelectTut = new SqlCommand(strSelectTut, materialCon);
                cmdSelectTut.Parameters.AddWithValue("@chapterId", chapterId);
                cmdSelectTut.Parameters.AddWithValue("@isVideo", isVideo);
                rptTut.DataSource = cmdSelectTut.ExecuteReader();
                rptTut.DataBind();

                

                String strSelectPrac = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = @isVideo AND mode = 'PRACTICAL';";
                SqlCommand cmdSelectPrac = new SqlCommand(strSelectPrac, materialCon);
                cmdSelectPrac.Parameters.AddWithValue("@chapterId", chapterId);
                cmdSelectPrac.Parameters.AddWithValue("@isVideo", isVideo);
                rptPrac.DataSource = cmdSelectPrac.ExecuteReader();
                rptPrac.DataBind();

                

                String strSelectOth = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = @isVideo AND mode = 'OTHER';";
                SqlCommand cmdSelectOth = new SqlCommand(strSelectOth, materialCon);
                cmdSelectOth.Parameters.AddWithValue("@chapterId", chapterId);
                cmdSelectOth.Parameters.AddWithValue("@isVideo", isVideo);
                rptOth.DataSource = cmdSelectOth.ExecuteReader();
                rptOth.DataBind();

                string userType = Session["userType"].ToString();
                if (userType == "Lecturer")
                {

                    String strDelLect = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = @isVideo AND mode = 'LECTURE';";
                    SqlCommand cmdDelLect = new SqlCommand(strDelLect, materialCon);
                    cmdDelLect.Parameters.AddWithValue("@chapterId", chapterId);
                    cmdDelLect.Parameters.AddWithValue("@isVideo", isVideo);
                    rptDelLect.DataSource = cmdDelLect.ExecuteReader();
                    rptDelLect.DataBind();

                    String strDelTut = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = @isVideo AND mode = 'TUTORIAL';";
                    SqlCommand cmdDelTut = new SqlCommand(strDelTut, materialCon);
                    cmdDelTut.Parameters.AddWithValue("@chapterId", chapterId);
                    cmdDelTut.Parameters.AddWithValue("@isVideo", isVideo);
                    rptDelTut.DataSource = cmdDelTut.ExecuteReader();
                    rptDelTut.DataBind();

                    String strDelPrac = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = @isVideo AND mode = 'PRACTICAL';";
                    SqlCommand cmdDelPrac = new SqlCommand(strDelPrac, materialCon);
                    cmdDelPrac.Parameters.AddWithValue("@chapterId", chapterId);
                    cmdDelPrac.Parameters.AddWithValue("@isVideo", isVideo);
                    rptDelPrac.DataSource = cmdDelPrac.ExecuteReader();
                    rptDelPrac.DataBind();

                    String strDelOth = "Select materialTitle AS materialTitle, materialId AS materialId from Material Where chapterId = @chapterId AND isVideo = @isVideo AND mode = 'OTHER';";
                    SqlCommand cmdDelOth = new SqlCommand(strDelOth, materialCon);
                    cmdDelOth.Parameters.AddWithValue("@chapterId", chapterId);
                    cmdDelOth.Parameters.AddWithValue("@isVideo", isVideo);
                    rptDelOth.DataSource = cmdDelOth.ExecuteReader();
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

        protected void rmRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
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
                    url = "pdfViewer.aspx?materialId=" + materialId;
                }
                 
                Response.Redirect(url);

            }
        }

        protected void btnDeleteRM_Click(object sender, ImageClickEventArgs e)
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

        public void saveFuction(SqlConnection materialCon, string materialTitle, string materialId, string materialFileName)
        {
           

            SqlCommand cmdSelectMaterialTitle = new SqlCommand("Select * from [dbo].[Material] where materialTitle=@materialTitle", materialCon);
            cmdSelectMaterialTitle.Parameters.AddWithValue("@materialTitle", materialTitle);
            SqlDataReader dtrMaterialTitle = cmdSelectMaterialTitle.ExecuteReader();

          

            if (!dtrMaterialTitle.HasRows )
            {
                string newFileName;
                string oldFileName;
                string extension = System.IO.Path.GetExtension(materialFileName);
               
                if (isVideo)
                {
                     newFileName = "~/videos/" + materialTitle + extension;
                     oldFileName = "~/videos/" + materialFileName;
                }
                else
                {
                    newFileName = "~/ReadingMaterials/" + materialTitle + extension;
                    oldFileName = "~/ReadingMaterials/" + materialFileName;
                }
                 
                string newMaterialTitle = Server.MapPath(newFileName);              
                string oldMaterialTitle = Server.MapPath(oldFileName);

                System.IO.File.Move(oldMaterialTitle, newMaterialTitle);

                String editMaterial = "UPDATE [dbo].[Material] SET materialTitle=@materialTitle, materialName=@materialName WHERE materialId = @materialId";
                SqlCommand cmdEditMaterial = new SqlCommand(editMaterial, materialCon);
                cmdEditMaterial.Parameters.AddWithValue("@materialTitle", materialTitle);
                cmdEditMaterial.Parameters.AddWithValue("@materialName", materialTitle + ".pdf");
                cmdEditMaterial.Parameters.AddWithValue("@materialId", materialId);               
                cmdEditMaterial.ExecuteNonQuery();

                materialCon.Close();
               
                string chapterId = Request.QueryString["chapterId"];
                string materialType = Request.QueryString["materialType"];
                string url = "material.aspx?chapterId=" + chapterId + "&materialType=" + materialType;
                Response.Redirect(url);
            }            
            else
            {
                Response.Write("<script>alert('Entered Material Title Already Exists.')</script>");

            }
        }

        public void cancelFunction(TextBox txt, LinkButton btnEdit, LinkButton btnCancel, LinkButton btnDel, LinkButton btnSave)
        {           
            txt.Enabled = false;
            txt.BorderStyle = BorderStyle.None;
            txt.BackColor = Color.Transparent;

            btnEdit.Visible = true;
            btnCancel.Visible = false;
            btnDel.Visible = false;
            btnSave.Visible = false;

            
        }


        protected void rptEditRM_ItemCommand(object source, RepeaterCommandEventArgs e)
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
            ImageButton btnEdit = (ImageButton)e.Item.FindControl("btnEdit");
            ImageButton btnSave = (ImageButton)e.Item.FindControl("btnSave");
            ImageButton btnCancel = (ImageButton)e.Item.FindControl("btnCancel");
            ImageButton btnDel = (ImageButton)e.Item.FindControl("btnDelete");

            string conStr = ConfigurationManager.ConnectionStrings["TARCLearnEntities"].ConnectionString;
            string providerConStr = new EntityConnectionStringBuilder(conStr).ProviderConnectionString;
            SqlConnection rmCon = new SqlConnection(providerConStr);
            rmCon.Open();

            //get materialFileName
            SqlCommand cmdGetRmTitle = new SqlCommand("Select materialName from [dbo].[Material] where materialId=@materialId;", rmCon);
            cmdGetRmTitle.Parameters.AddWithValue("@materialId", materialId);
            String materialFileName = Convert.ToString(cmdGetRmTitle.ExecuteScalar());

            if (e.CommandName == "editLec")
            {
                TextBox txtLec = (TextBox)e.Item.FindControl("txtLec");
                txtLec.Enabled = true;
                txtLec.BorderStyle = BorderStyle.Inset;
                txtLec.BackColor = Color.White;

                btnEdit.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDel.Visible = false;

            }
            if (e.CommandName == "editPrac")
            {
                TextBox txtPrac = (TextBox)e.Item.FindControl("txtPrac");
                txtPrac.Enabled = true;
                txtPrac.BorderStyle = BorderStyle.Inset;
                txtPrac.BackColor = Color.White;

                btnEdit.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDel.Visible = false;

            }
            if (e.CommandName == "editTut")
            {
                TextBox txtTut = (TextBox)e.Item.FindControl("txtTut");
                txtTut.Enabled = true;
                txtTut.BorderStyle = BorderStyle.Inset;
                txtTut.BackColor = Color.White;

                btnEdit.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDel.Visible = false;

            }
            if (e.CommandName == "editOth")
            {
                TextBox txtOth = (TextBox)e.Item.FindControl("txtOth");
                txtOth.Enabled = true;
                txtOth.BorderStyle = BorderStyle.Inset;
                txtOth.BackColor = Color.White;

                btnEdit.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                btnDel.Visible = false;

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
                    SqlCommand cmdDelRm = new SqlCommand(strDelRm, rmCon);
                    cmdDelRm.Parameters.AddWithValue("@materialId", materialId);
                    cmdDelRm.ExecuteNonQuery();

                    rmCon.Close();

                    Response.Write("<script>alert('Successfully deleted.')</script>");
                                        
                    string url = "material.aspx?chapterId=" + chapterId + "&materialType=" + materialType;
                    Response.Redirect(url);

                }
                else
                {
                    Response.Write("<script>alert('File Does Not Exist.')</script>");


                }
            }
            if (e.CommandName == "saveLec")
            {
                if (Page.IsValid)
                {
                    TextBox txtLec = (TextBox)e.Item.FindControl("txtLec");
                    txtLec.Enabled = true;
                    txtLec.BorderStyle = BorderStyle.Inset;
                    txtLec.BackColor = Color.White;

                    btnEdit.Visible = true;
                    btnCancel.Visible = false;
                    btnDel.Visible = true;
                    btnSave.Visible = false;

                    saveFuction(rmCon, txtLec.Text, materialId, materialFileName);
                }
            }
            if (e.CommandName == "savePrac")
            {
                if (Page.IsValid)
                {
                    TextBox txtPrac = (TextBox)e.Item.FindControl("txtPrac");
                    txtPrac.Enabled = true;
                    txtPrac.BorderStyle = BorderStyle.Inset;
                    txtPrac.BackColor = Color.White;

                    btnEdit.Visible = true;
                    btnCancel.Visible = false;
                    btnDel.Visible = true;
                    btnSave.Visible = false;

                    saveFuction(rmCon, txtPrac.Text, materialId, materialFileName);
                }
            }
            if (e.CommandName == "saveTut")
            {
                if (Page.IsValid)
                {
                    TextBox txtTut = (TextBox)e.Item.FindControl("txtTut");
                    txtTut.Enabled = true;
                    txtTut.BorderStyle = BorderStyle.Inset;
                    txtTut.BackColor = Color.White;

                    btnEdit.Visible = true;
                    btnCancel.Visible = false;
                    btnDel.Visible = true;
                    btnSave.Visible = false;

                    saveFuction(rmCon, txtTut.Text, materialId, materialFileName);
                }
            }
            if (e.CommandName == "saveOth")
            {
                if (Page.IsValid)
                {
                    TextBox txtOth = (TextBox)e.Item.FindControl("txtOth");
                    txtOth.Enabled = true;
                    txtOth.BorderStyle = BorderStyle.Inset;
                    txtOth.BackColor = Color.White;

                    btnEdit.Visible = true;
                    btnCancel.Visible = false;
                    btnDel.Visible = true;
                    btnSave.Visible = false;

                    saveFuction(rmCon, txtOth.Text, materialId, materialFileName);
                }
            }
            if (e.CommandName == "cancel")
            {
                string url = "material.aspx?chapterId=" + chapterId + "&materialType=" + materialType;
                Response.Redirect(url);

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

                if (!dtrMaterialTitle.HasRows && !fileInfo.Exists)                   
                {
                    if (file.HasFile && file.PostedFile != null)
                    {
                        // Get the name of the file to upload.
                        string fileName = Server.HtmlEncode(file.FileName);

                        // Get the extension of the uploaded file.
                        string extension = System.IO.Path.GetExtension(fileName);

                        if (extension == ".pdf" || extension == ".pptx")
                        {
                            string path = Server.MapPath("~/ReadingMaterials/" + file.FileName);
                            file.PostedFile.SaveAs(path);

                        }else
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
                        String getIndex = "SELECT COUNT(materialId) FROM [dbo].[Material] WHERE @chapterId = chapterId AND @isVideo = isVideo; ";
                        SqlCommand cmdGetIndex = new SqlCommand(getIndex, materialCon);
                        cmdGetIndex.Parameters.AddWithValue("@chapterId", chapterId);
                        cmdGetIndex.Parameters.AddWithValue("@isVideo", isVideo);
                        int newIndex = Convert.ToInt32(cmdGetIndex.ExecuteScalar()) + 1;

                        String addMaterial = "INSERT INTO [dbo].[Material] VALUES(@index,@materialTitle,@materialDescription,@materialName,@isVideo,@mode,@chapterId);";
                        SqlCommand cmdAddMaterial = new SqlCommand(addMaterial, materialCon);

                        cmdAddMaterial.Parameters.AddWithValue("@index", newIndex);
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
                else if (dtrMaterialTitle.HasRows && fileInfo.Exists)
                {
                    Response.Write("<script>alert('Both Entered Title And Uploaded File Already Exists.')</script>");
                }
                else if (dtrMaterialTitle.HasRows)
                {
                    Response.Write("<script>alert('Entered Title Already Exists.')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Uploaded File Already Exists.')</script>");
                }

            }




        }
    }
}