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
            var course = await entities.Courses.Include(u => u.Users).Select(c =>
            new CourseUsersDto()
            {
                courseId = c.courseId,
                Users = c.Users.Select(u => new UserDto()
                {
                    userId = u.userId,
                    username= u.username
                })
            }).SingleOrDefaultAsync(c => c.courseId == id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course.Users);
        }

    }
}