using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Description;
using System.Threading.Tasks;
using TARCLearn.Models;
namespace TARCLearn.Controllers
{
    public class ChaptersController : ApiController
    {
        [HttpPost]
        [Route("api/chapters")]
        [ResponseType(typeof(ChapterDto))]
        public async Task<IHttpActionResult> PostChapter(string cid, [FromBody] ChapterDto newChapter)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var course = await db.Courses.Include(ch => ch.Chapters)
                    .FirstOrDefaultAsync(c => c.courseId == cid);
                if(course == null)
                {
                    return Content(HttpStatusCode.NotFound, course);
                }
                Chapter chap = new Chapter()
                {
                    courseId = cid,
                    chapterId = newChapter.chapterId,
                    chapterTitle = newChapter.chapterTitle
                };
                course.Chapters.Add(chap);
                await db.SaveChangesAsync();
                var dto = new ChapterDto()
                {
                    chapterId = chap.chapterId,
                    chapterTitle = chap.chapterTitle
                };
                return CreatedAtRoute("DefaultApi", new { chapterId = newChapter.chapterId }, dto);
            }catch(Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }
    }
}