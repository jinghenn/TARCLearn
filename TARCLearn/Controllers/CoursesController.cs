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
        public async Task<IHttpActionResult> GetCourseUsers(string id)
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

        [HttpPost]
        [ResponseType(typeof(CourseDto))]
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

                    var dto = new CourseDto()
                    {
                        courseId = newCourse.courseId,
                        courseName = newCourse.courseTitle,
                    };
                    return CreatedAtRoute("DefaultApi", new { courseId = newCourse.courseId }, dto);
                }
                else{
                    return Content(HttpStatusCode.Conflict, "Course: " + newCourse.courseId + " already exist");
                }
            }catch(Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.ToString());
            }
        }
        [HttpPost]
        [Route("api/courses/enrol", Name="EnrolUser")]
        [ResponseType(typeof(IEnumerable<UserDto>))]
        public async Task<IHttpActionResult> PostEnrolment(string courseId, string userId)
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

                return CreatedAtRoute("EnrolUser", new {courseId = course.courseId}, dto.Users);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.ToString());
            }

            
        }

        [HttpDelete]
        [Route("api/courses/unenrol")]
        public async Task<IHttpActionResult> DeleteEnrolment(string courseId, string userId)
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

    }
}