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
    public class MessagesController : ApiController
    {
        [HttpGet]
        [Route("api/messages/{messageId}")]
        [ResponseType(typeof(MessageDetailDto))]
        public async Task<IHttpActionResult> GetMessage(int messageId)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var message = await db.DiscussionMessages.SingleOrDefaultAsync(m => m.messageId == messageId);
                if (message == null)
                {
                    return Content(HttpStatusCode.NotFound, "Message: " + messageId + " does not exist");
                }
                var dto = new MessageDetailDto
                {
                    messageId = message.messageId,
                    message = message.message,
                    userId = message.userId,
                    userName = message.User.username
                };
                return Ok(dto);
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpPut]
        [Route("api/messages/{messageId}")]
        [ResponseType(typeof(MessageDetailDto))]
        public async Task<IHttpActionResult> PuttMessage(int messageId, [FromBody] MessageDto updatedMessage)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var message = await db.DiscussionMessages.SingleOrDefaultAsync(m => m.messageId == messageId);
                if (message == null)
                {
                    return Content(HttpStatusCode.NotFound, "Message: " + messageId + " does not exist");
                }
                message.message = updatedMessage.message;
                await db.SaveChangesAsync();

                var dto = new MessageDetailDto
                {
                    messageId = message.messageId,
                    message = message.message,
                    userId = message.userId,
                    userName = message.User.username
                };
                return Ok(dto);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpPost]
        [Route("api/messages", Name = "MessageRoute")]
        [ResponseType(typeof(MessageDetailDto))]
        public async Task<IHttpActionResult> PutMessage([FromBody] DiscussionMessage newMessage)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var msg = new DiscussionMessage()
                {
                    message = newMessage.message,
                    userId = newMessage.userId,
                    threadId = newMessage.threadId
                };
                db.DiscussionMessages.Add(msg);
                await db.SaveChangesAsync();
                var dto = await db.DiscussionMessages.Select(m => new MessageDetailDto {
                    messageId = m.messageId,
                    message = m.message,
                    userId = m.userId,
                    userName = m.User.username
                }).SingleOrDefaultAsync(m => m.messageId == msg.messageId);
                return CreatedAtRoute("MessageRoute", new { newMessage.messageId }, dto);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpDelete]
        [Route("api/messages/{messageId}")]
        [ResponseType(typeof(MessageDetailDto))]
        public async Task<IHttpActionResult> DeleteMessage(int messageId)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var message = await db.DiscussionMessages.SingleOrDefaultAsync(m => m.messageId == messageId);
                if (message == null)
                {
                    return Content(HttpStatusCode.NotFound, "Message: " + messageId + " does not exist");
                }
                var dto = new MessageDetailDto
                {
                    messageId = message.messageId,
                    message = message.message,
                    userId = message.userId,
                    userName = message.User.username
                };
                db.DiscussionMessages.Remove(message);
                await db.SaveChangesAsync();

                
                return Ok(dto);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }
    }

}