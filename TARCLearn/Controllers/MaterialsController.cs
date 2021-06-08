using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TARCLearn.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net.Http.Headers;

namespace TARCLearn.Controllers
{
    public class MaterialsController : ApiController
    {
        [HttpGet]
        [Route("api/materials/{materialId}")]
        [ResponseType(typeof(MaterialDetailDto))]
        public async Task<IHttpActionResult> GetMaterial(int materialId)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var material = await db.Materials.Select(m => new MaterialDetailDto()
                {
                    materialId = m.materialId,
                    materialDescription = m.materialDescription,
                    materialTitle = m.materialTitle,
                    mode = m.mode,
                    materialName = m.materialName,
                    index = m.index,
                    isVideo = m.isVideo
                }).SingleOrDefaultAsync(m => m.materialId == materialId);
                if (material == null)
                {
                    return Content(HttpStatusCode.NotFound, "Material: " + materialId + " not found");
                }
                return Ok(material);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }


        }

        [HttpPut]
        [Route("api/materials/{materialId}")]
        [ResponseType(typeof(MaterialDetailDto))]
        public async Task<IHttpActionResult> PutMaterial(int materialId, [FromBody] MaterialDetailDto newMaterial)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var material = await db.Materials.SingleOrDefaultAsync(c => c.materialId == materialId);
                if (material == null)
                {
                    return Content(HttpStatusCode.NotFound, "Material " + materialId + " not found");
                }
                material.index = newMaterial.index;
                material.materialTitle = newMaterial.materialTitle;
                material.materialDescription = newMaterial.materialDescription;
                material.mode = newMaterial.mode;
                await db.SaveChangesAsync();
                var dto = new MaterialDetailDto()
                {
                    index = newMaterial.index,
                    materialTitle = newMaterial.materialTitle,
                    materialDescription = newMaterial.materialDescription,
                    materialName = material.materialName,
                    mode = newMaterial.mode,
                };
                return Ok(dto);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpDelete]
        [Route("api/materials/{materialId}")]
        [ResponseType(typeof(MaterialDetailDto))]
        public async Task<IHttpActionResult> DeleteChapter(int materialId)
        {
            try
            {
                TARCLearnEntities entities = new TARCLearnEntities();
                var material = await entities.Materials.FirstOrDefaultAsync(c => c.materialId == materialId);
                if (material == null)
                {
                    return Content(HttpStatusCode.NotFound, "Material: " + materialId + " not found");
                }

                var dto = new MaterialDetailDto()
                {
                    materialId = material.materialId,
                    index = material.index,
                    materialTitle = material.materialTitle,
                    materialDescription = material.materialDescription,
                    isVideo = material.isVideo,
                    materialName = material.materialName,
                    mode = material.mode,
                };
                
                entities.Materials.Remove(material);
                await entities.SaveChangesAsync();
                try
                {
                    var path = dto.isVideo ? HttpContext.Current.Server.MapPath("~/videos") :
                        HttpContext.Current.Server.MapPath("~/ReadingMaterials");
                    File.Delete(path + "\\" + dto.materialName);
                }catch(Exception e)
                {
                    return Content(HttpStatusCode.InternalServerError, e);
                }
                return Ok(dto);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpGet]
        [Route("api/materials/download")]
        public HttpResponseMessage GetFile(int materialId)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var material = db.Materials.FirstOrDefault(m => m.materialId == materialId);
                if(material == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Material " + materialId + " not found");
                }

                string filePath = HttpContext.Current.Server.MapPath("~/ReadingMaterials/" + material.materialName);
                if (!File.Exists(filePath))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Gone, material.materialName);
                }
                var dataBytes = File.ReadAllBytes(filePath);
                var dataStream = new MemoryStream(dataBytes);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(dataStream);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = material.materialName;
                string type = MimeMapping.GetMimeMapping(material.materialName);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(type);
                return response;

            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

        }
    }
}