using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Configuration;
using TARCLearn.Models;
using Newtonsoft.Json;

namespace TARCLearn.Controllers
{
    public class UploadController : ApiController
    {
        [HttpPost]
        [Route("api/upload")]
        public async Task<HttpResponseMessage> Upload(int chapterId, string type)
        {
            var fileuploadPath = "";
            if (type == "video")
            {
                fileuploadPath = HttpContext.Current.Server.MapPath("~/videos");
            }
            else if(type == "material")
            {
                fileuploadPath = HttpContext.Current.Server.MapPath("~/ReadingMaterials");
            }
            try
            {
                var provider = new MultipartFormDataStreamProvider(fileuploadPath);
                var content = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
                foreach (var header in Request.Content.Headers)
                { 
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                await content.ReadAsMultipartAsync(provider);
                var filePath = "";
                foreach (MultipartFileData file in provider.FileData)
                {
                    Console.WriteLine(file.Headers.ContentDisposition.Name.Trim('"'));
                    if (file.Headers.ContentDisposition.Name.Trim('"') == "file")
                    {
                        var name = file.Headers.ContentDisposition.FileName;
                        name = name.Trim('"');
                        var localFileName = file.LocalFileName;
                        filePath = Path.Combine(fileuploadPath, name);


                        int i = 0;
                        while (File.Exists(filePath))
                        {
                            i++;
                            var lastInd = name.LastIndexOf(".");
                            var front = name.Substring(0, lastInd);
                            var end = name.Substring(lastInd + 1);

                            filePath = Path.Combine(fileuploadPath, front + "(" + i.ToString() + ")." + end);
                        }
                        File.Move(localFileName, filePath);
                    }
                    
                }
                var lastIndex = filePath.LastIndexOf("\\");
                filePath = filePath.Substring(lastIndex + 1);
                TARCLearnEntities db = new TARCLearnEntities();
                foreach(var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        if (key == "material")
                        {
                            
                            try
                            {

                                var newMat = JsonConvert.DeserializeObject<MaterialDetailDto>(val);
                                var myMat = new Material()
                                {
                                    materialName = filePath,
                                    materialDescription = newMat.materialDescription,
                                    materialTitle = newMat.materialTitle,
                                    chapterId = chapterId,
                                    isVideo = newMat.isVideo,
                                    mode = newMat.mode,
                                    index = newMat.index
                                    
                                };
                                db.Materials.Add(myMat);
                                db.SaveChanges();
                                return Request.CreateResponse(HttpStatusCode.OK, newMat);
                            }
                            catch (Exception e)
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
                            }
                        }
                    }
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, filePath);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

        }
    }
}