using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Description;
using TARCLearn.Models;
using System.Threading.Tasks;

namespace TARCLearn.Controllers
{
    public class DiscussionsController : ApiController
    {
        [HttpGet]
        [Route("api/discussions/{threadId}")]
        [ResponseType(typeof(DiscussionThreadDetailDto))]
        public async Task<IHttpActionResult> GetDiscussionThread(int threadId)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var thread = await db.DiscussionThreads.FirstOrDefaultAsync(d => d.threadId == threadId);
                if (thread == null)
                {
                    return Content(HttpStatusCode.NotFound, "Discussion thread not found");
                }
                var dto = new DiscussionThreadDetailDto
                {
                    threadId = thread.threadId,
                    threadTitle = thread.threadTitle,
                    threadDescription = thread.threadDescription,
                    chapterId = thread.chapterId,
                    userId = thread.userId
                };
                return Ok(dto);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpPut]
        [Route("api/discussions/{threadId}")]
        [ResponseType(typeof(DiscussionThreadDetailDto))]
        public async Task<IHttpActionResult> PutDiscussionThread(int threadId, [FromBody]DiscussionThreadDetailDto updatedThread)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var thread = await db.DiscussionThreads.FirstOrDefaultAsync(d => d.threadId == threadId);
                if (thread == null)
                {
                    return Content(HttpStatusCode.NotFound, "Discussion thread not found");
                }
                thread.threadTitle = updatedThread.threadTitle;
                thread.threadDescription = updatedThread.threadDescription;
                await db.SaveChangesAsync();

                var dto = new DiscussionThreadDetailDto
                {
                    threadId = thread.threadId,
                    threadTitle = thread.threadTitle,
                    threadDescription = thread.threadDescription,
                    chapterId = thread.chapterId,
                    userId = thread.userId
                };
                return Ok(dto);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpPost]
        [Route("api/discussions", Name = "DiscussionRoute")]
        [ResponseType(typeof(DiscussionThreadDetailDto))]
        public async Task<IHttpActionResult> PostDiscussionThread([FromBody] DiscussionThread newThread)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                db.DiscussionThreads.Add(newThread);
                await db.SaveChangesAsync();
                var dto = new DiscussionThreadDetailDto()
                {
                    threadId = newThread.threadId,
                    threadTitle = newThread.threadTitle,
                    threadDescription = newThread.threadDescription,
                    userId = newThread.userId,
                    chapterId = newThread.chapterId
                };
                return CreatedAtRoute("DiscussionRoute", new { newThread.threadId }, dto);
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpDelete]
        [Route("api/discussions/{threadId}")]
        [ResponseType(typeof(DiscussionThreadDetailDto))]
        public async Task<IHttpActionResult> DeleteDiscussionThread(int threadId)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var thread = await db.DiscussionThreads.FirstOrDefaultAsync(d => d.threadId == threadId);
                if (thread == null)
                {
                    return Content(HttpStatusCode.NotFound, "Discussion thread not found");
                }
                db.DiscussionThreads.Remove(thread);
                await db.SaveChangesAsync();
                var dto = new DiscussionThreadDetailDto()
                {
                    threadId = thread.threadId,
                    threadTitle = thread.threadTitle,
                    threadDescription = thread.threadDescription,
                    userId = thread.userId,
                    chapterId = thread.chapterId
                };
                return Ok(dto);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }
    }
}