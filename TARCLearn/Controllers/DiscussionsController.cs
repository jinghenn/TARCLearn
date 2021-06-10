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
                    userId = thread.userId,
                    userName = db.Users.FirstOrDefault(u => u.userId == thread.userId).username
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
        [ResponseType(typeof(DiscussionAboutDto))]
        public async Task<IHttpActionResult> PutDiscussionThread(int threadId, [FromBody]DiscussionAboutDto updatedThread)
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

                return Ok(updatedThread);
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
                    userName = db.Users.FirstOrDefault(u => u.userId == newThread.userId).username,
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
                var dto = new DiscussionAboutDto()
                {
                    threadId = thread.threadId,
                    threadTitle = thread.threadTitle,
                    threadDescription = thread.threadDescription
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