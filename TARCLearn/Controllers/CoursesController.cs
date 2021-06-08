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
    public class CoursesController : ApiController
    {
        [Route("api/courses/{id}/users")]
        [ResponseType(typeof(IEnumerable<UserDto>))]
        public async Task<IHttpActionResult> GetCourseUsers(int id)
        {
            TARCLearnEntities entities = new TARCLearnEntities();
            var course = await entities.Courses.Include(c => c.Users).Select(c =>
            new CourseUsersDto()
            {
                courseId = c.courseId,
                Users = c.Users.OrderBy(u => u.userId).Select(u => new UserDto()
                {
                    userId = u.userId,
                    username = u.username
                })
            }).SingleOrDefaultAsync(c => c.courseId == id);
            if (course == null)
            {
                return Content(HttpStatusCode.NotFound, "Course: " + course.courseId + " not found");
            }
            return Ok(course.Users);
        }
        [HttpGet]
        [Route("api/courses/{CourseId}")]
        [ResponseType(typeof(CourseDetailDto))]
        public async Task<IHttpActionResult> GetCourseDetail(int courseId)
        {
            TARCLearnEntities entities = new TARCLearnEntities();
            var course = await entities.Courses.Select(c =>
            new CourseDetailDto()
            {
                courseId = c.courseId,
                courseCode = c.courseCode,
                courseDescription = c.courseDescription,
                courseTitle = c.courseTitle
            }).SingleOrDefaultAsync(c => c.courseId == courseId);
            if (course == null)
            {
                return Content(HttpStatusCode.NotFound, "Course: " + courseId + " not found");
            }
            return Ok(course);
        }

        [HttpPost]
        [ResponseType(typeof(CourseDetailDto))]
        public async Task<IHttpActionResult> PostCourse([FromBody] Course newCourse)
        {
            try
            {
                TARCLearnEntities entities = new TARCLearnEntities();
                var course = await entities.Courses.FirstOrDefaultAsync(c => c.courseId == newCourse.courseId);
                if (course == null)
                {
                    entities.Courses.Add(newCourse);
                    await entities.SaveChangesAsync();

                    var dto = new CourseDetailDto()
                    {
                        courseId = newCourse.courseId,
                        courseCode = newCourse.courseCode,
                        courseTitle = newCourse.courseTitle,
                        courseDescription = newCourse.courseDescription
                    };
                    return CreatedAtRoute("DefaultApi", new { newCourse.courseId }, dto);
                }
                else
                {
                    return Content(HttpStatusCode.Conflict, "Course: " + newCourse.courseCode + " already exist");
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.ToString());
            }
        }

        [HttpPut]
        [Route("api/courses/{courseId}")]
        [ResponseType(typeof(CourseDetailDto))]
        public async Task<IHttpActionResult> PutCourse(int courseId, [FromBody] Course updatedCourse)
        {
            try
            {
                TARCLearnEntities entities = new TARCLearnEntities();
                var course = await entities.Courses.SingleOrDefaultAsync(c => c.courseId == courseId);
                if (course == null)
                {
                    return Content(HttpStatusCode.NotFound, "Course: " + courseId + " not found");
                }
                course.courseTitle = updatedCourse.courseTitle;
                course.courseDescription = updatedCourse.courseDescription;
                await entities.SaveChangesAsync();
                var dto = new CourseDetailDto()
                {
                    courseId = course.courseId,
                    courseCode = course.courseCode,
                    courseDescription = course.courseDescription,
                    courseTitle = course.courseTitle
                };
                return Ok(dto);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }
        [HttpPost]
        [Route("api/courses/enrol", Name = "EnrolUser")]
        [ResponseType(typeof(IEnumerable<UserDto>))]
        public async Task<IHttpActionResult> PostEnrolment(int courseId, string userId)
        {
            try
            {
                TARCLearnEntities entities = new TARCLearnEntities();
                var course = await entities.Courses.Include(c => c.Users)
                    .FirstOrDefaultAsync(c => c.courseId == courseId);
                var user = await entities.Users.FirstOrDefaultAsync(u => u.userId == userId);
                if (course == null || user == null)
                {
                    return Content(HttpStatusCode.NotFound, "Course: " + courseId + " does not exist.");
                }
                if (user == null)
                {
                    return Content(HttpStatusCode.NotFound, "User: " + userId + " does not exist.");
                }
                if (course.Users.Contains(user))
                {
                    return Content(HttpStatusCode.Conflict, userId + " already in course " + courseId);
                }

                course.Users.Add(user);
                await entities.SaveChangesAsync();

                var dto = new CourseUsersDto()
                {
                    courseId = course.courseId,
                    Users = course.Users.Select(u => new UserDto()
                    {
                        userId = u.userId,
                        username = u.username
                    })
                };

                return CreatedAtRoute("EnrolUser", new { courseId = course.courseId }, dto.Users);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.ToString());
            }


        }

        [HttpDelete]
        [Route("api/courses/unenrol")]
        public async Task<IHttpActionResult> DeleteEnrolment(int courseId, string userId)
        {
            TARCLearnEntities entities = new TARCLearnEntities();
            try
            {
                var course = await entities.Courses.Include(c => c.Users)
                    .FirstOrDefaultAsync(c => c.courseId == courseId);
                var user = await entities.Users.FirstOrDefaultAsync(u => u.userId == userId);
                if (course == null || user == null)
                {
                    return Content(HttpStatusCode.NotFound, "Course: " + courseId + " does not exist.");
                }
                if (user == null)
                {
                    return Content(HttpStatusCode.NotFound, "User: " + userId + " does not exist.");
                }
                if (!(course.Users.Contains(user)))
                {
                    return Content(HttpStatusCode.NotFound, "User: " + userId + " not in course: " + courseId);
                }

                course.Users.Remove(user);
                await entities.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.ToString());
            }

        }

        [HttpDelete]
        [Route("api/courses/{courseId}")]
        [ResponseType(typeof(CourseDetailDto))]
        public async Task<IHttpActionResult> DeleteCourse(int courseId)
        {
            try
            {
                TARCLearnEntities entities = new TARCLearnEntities();
                var course = await entities.Courses.FirstOrDefaultAsync(c => c.courseId == courseId);
                if(course == null)
                {
                    return Content(HttpStatusCode.NotFound, "Course: " + courseId + " not found");
                }
                
                var dto = new CourseDetailDto()
                {
                    courseId = course.courseId,
                    courseCode = course.courseCode,
                    courseDescription = course.courseDescription,
                    courseTitle = course.courseTitle
                };
                entities.Courses.Remove(course);
                await entities.SaveChangesAsync();
                return Ok(dto);
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }
        [HttpGet]
        [Route("api/courses/{id}/chapters")]
        [ResponseType(typeof(IEnumerable<ChapterDetailDto>))]
        public async Task<IHttpActionResult> GetCourseChapters(int id)
        {
            TARCLearnEntities entities = new TARCLearnEntities();
            var course = await entities.Courses.Include(c => c.Chapters).Select(c =>
            new CourseChaptersDto()
            {
                courseId = c.courseId,
                Chapters = c.Chapters.OrderBy(ch => ch.chapterNo).Select(ch => new ChapterDetailDto()
                {
                    chapterId = ch.chapterId,
                    chapterNo = ch.chapterNo,
                    chapterTitle = ch.chapterTitle
                })
               
            }).SingleOrDefaultAsync(c => c.courseId == id);
            if (course == null)
            {
                return Content(HttpStatusCode.NotFound, "Course: " + course.courseId + " not found");
            }
            return Ok(course.Chapters);
        }

    }
}