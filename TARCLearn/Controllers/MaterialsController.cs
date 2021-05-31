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
                if(material == null)
                {
                    return Content(HttpStatusCode.NotFound, "Material: " + materialId +" not found");
                }
                return Ok(material);
            }catch(Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
            

        }
    }
}