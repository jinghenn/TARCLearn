using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TARCLearn.Models;
using System.Data.Entity;

namespace TARCLearn.Controllers
{
    public class UsersController : ApiController
    {
        // GET api/<controller>
        public IQueryable<UserDto> GetUsers()
        {
            TARCLearnEntities entities = new TARCLearnEntities();
            var users = from u in entities.Users
                        select new UserDto()
                        {
                            userId = u.userId,
                            username = u.username,
                        };
            return users;
        }
        [ResponseType(typeof(UserDetailDto))]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            TARCLearnEntities entities = new TARCLearnEntities();
            var user = await entities.Users.Select(u =>
            new UserDetailDto()
            {
                userId = u.userId,
                username = u.username,
                password = u.password,
                email = u.email,
                isLecturer = u.isLecturer
            }).SingleOrDefaultAsync(c => c.userId == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Route("api/users/{id}/courses")]
        [ResponseType(typeof(IEnumerable<CourseDto>))]
        public async Task<IHttpActionResult> GetUserCourses(string id)
        {
            TARCLearnEntities entities = new TARCLearnEntities();
            var user = await entities.Users.Include(c => c.Courses).Select(u =>
            new UserCoursesDto()
            {
                userId = u.userId,
                Courses = u.Courses.Select(cid => new CourseDto()
                {
                    courseId = cid.courseId,
                    courseCode = cid.courseCode,
                    courseTitle = cid.courseTitle
                })
            }).SingleOrDefaultAsync(c => c.userId == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Courses);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] User user)
        {
            try
            {
                using (TARCLearnEntities entities = new TARCLearnEntities())
                {
                    entities.Users.Add(user);
                    entities.SaveChanges();
                }
                var message = Request.CreateResponse(HttpStatusCode.Created, user);
                message.Headers.Location = new Uri(Request.RequestUri + user.userId);
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(string id, [FromBody] User user)
        {
            try
            {
                using (TARCLearnEntities entities = new TARCLearnEntities())
                {
                    var entity = entities.Users.FirstOrDefault(e => e.userId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with id = " + id + " not found.");
                    }
                    else
                    {
                        entity.password = user.password;
                        entity.username = user.username;
                        entity.isLecturer = user.isLecturer;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(string id)
        {
            try
            {
                using (TARCLearnEntities entities = new TARCLearnEntities())
                {
                    var entity = entities.Users.FirstOrDefault(e => e.userId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with id = " + id + " not found.");
                    }
                    else
                    {
                        entities.Users.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/users/{id}/discussions")]
        [ResponseType(typeof(IEnumerable<DiscussionThreadDetailDto>))]
        public async Task<IHttpActionResult> GetUserDiscussions(string id)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var user = await db.Users.FirstOrDefaultAsync(u => u.userId == id);
                if (user == null)
                {
                    return Content(HttpStatusCode.NotFound, $"User {id} not found");

                }
                
                var discussions = db.Courses
                    .Where(c => c.Users.Any(u => u.userId == id))
                    .SelectMany(c => c.Chapters.SelectMany(chap => chap.DiscussionThreads
                    .Where(dt => dt.userId == id || dt.DiscussionMessages.Any(dm => dm.userId == id))
                    .Select(
                       t => new DiscussionThreadDetailDto
                       {
                           threadId = t.threadId,
                           threadTitle = t.threadTitle,
                           threadDescription = t.threadDescription,
                           userName = t.User.username,
                           userId = t.userId,
                           chapterId = t.chapterId
                       }))).ToHashSet();

                
                return Ok(discussions.Distinct(new DiscussionEqualityComparer()));
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }
    }
    class DiscussionEqualityComparer : IEqualityComparer<DiscussionThreadDetailDto>
    {

        public bool Equals(DiscussionThreadDetailDto x, DiscussionThreadDetailDto y)
        {
            return x.threadId == y.threadId;
        }

        public int GetHashCode(DiscussionThreadDetailDto obj)
        {
            return obj.threadId.GetHashCode();
        }
    }
}