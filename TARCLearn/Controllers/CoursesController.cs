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
using System.Net.Mail;
using System.Web;
//using System.Linq.Dynamic.Core;
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
                    username = u.username,
                    email = u.email
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
                var course = entities.Courses.Where(c => c.courseCode == newCourse.courseCode).FirstOrDefault();
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
                if(course.courseCode != updatedCourse.courseCode)
                {
                    var courseWithCourseCode = await entities.Courses.Where(c => c.courseCode == updatedCourse.courseCode).FirstOrDefaultAsync();
                    if (courseWithCourseCode != null)
                    {
                        return Content(HttpStatusCode.Conflict, "Course with Course Code: " + updatedCourse.courseCode + " already exist");
                    }
                }
                
                course.courseCode = updatedCourse.courseCode;
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
        [Route("api/courses/{courseId}/enrol")]
        [ResponseType(typeof(IEnumerable<string>))]
        public async Task<IHttpActionResult> PostEnrolment(int courseId, [FromBody] List<string> emailList)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var course = db.Courses.Include(c => c.Users).FirstOrDefault(c => c.courseId == courseId);
                if (course == null)
                {
                    return Content(HttpStatusCode.NotFound, "course" + courseId + " not found.");
                }

                var failedEmail = new List<string>();
                var successedEmail = new List<string>();
                bool failFlag = false;
                foreach (string email in emailList)
                {
                    var currentUser = db.Users.Where(u => u.email == email).FirstOrDefault();
                    if (currentUser == null || course.Users.Any(u => u.Equals(currentUser)))
                    {
                        failFlag = true;
                        failedEmail.Add(email);
                        continue;
                    }
                    course.Users.Add(currentUser);
                    successedEmail.Add(email);
                }
                await db.SaveChangesAsync();
                SendEnrolmentEmail(successedEmail, $"{course.courseCode} {course.courseTitle}");
                if (failFlag)
                {
                    IEnumerable<string> mList = failedEmail;
                    return Content(HttpStatusCode.NotFound, mList);
                }
                
                return Ok(emailList);
                
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }


        }

        [HttpPut]
        [Route("api/courses/{courseId}/unenrol")]
        [ResponseType(typeof(IEnumerable<string>))]
        public async Task<IHttpActionResult> DeleteEnrolment(int courseId, [FromBody] List<string> emailList)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var course = db.Courses.Include(c => c.Users).FirstOrDefault(c => c.courseId == courseId);
                if (course == null)
                {
                    return Content(HttpStatusCode.NotFound, "course" + courseId + " not found.");
                }
                
                var failedEmail = new List<string>();
                var successedEmail = new List<string>();
                bool failFlag = false;
                foreach (string email in emailList)
                {
                    var currentUser = db.Users.Where(u => u.email == email).FirstOrDefault();
                    var lecturerList = course.Users.Where(u => u.isLecturer).ToList();
                    if (currentUser == null || !(course.Users.Any(u => u.Equals(currentUser))))
                    {
                        failFlag = true;
                        failedEmail.Add(email);
                        continue;
                    }
                    if(lecturerList.Contains(currentUser) && lecturerList.Count == 1)
                    {
                        failFlag = true;
                        failedEmail.Add(email);
                        continue;
                    }
                    course.Users.Remove(currentUser);
                    successedEmail.Add(email);
                }
                await db.SaveChangesAsync();
                if (failFlag)
                {
                    IEnumerable<string> mList = failedEmail;
                    return Content(HttpStatusCode.NotFound, mList);
                }

                return Ok(emailList);

            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }


        }

        //[HttpDelete]
        //[Route("api/courses/unenrol")]
        //public async Task<IHttpActionResult> DeleteEnrolment(int courseId, string userId)
        //{
        //    TARCLearnEntities entities = new TARCLearnEntities();
        //    try
        //    {
        //        var course = await entities.Courses.Include(c => c.Users)
        //            .FirstOrDefaultAsync(c => c.courseId == courseId);
        //        var user = await entities.Users.FirstOrDefaultAsync(u => u.userId == userId);
        //        if (course == null || user == null)
        //        {
        //            return Content(HttpStatusCode.NotFound, "Course: " + courseId + " does not exist.");
        //        }
        //        if (user == null)
        //        {
        //            return Content(HttpStatusCode.NotFound, "User: " + userId + " does not exist.");
        //        }
        //        if (!(course.Users.Contains(user)))
        //        {
        //            return Content(HttpStatusCode.NotFound, "User: " + userId + " not in course: " + courseId);
        //        }

        //        course.Users.Remove(user);
        //        await entities.SaveChangesAsync();

        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return Content(HttpStatusCode.BadRequest, e.ToString());
        //    }

        //}

        [HttpDelete]
        [Route("api/courses/{courseId}")]
        [ResponseType(typeof(CourseDetailDto))]
        public async Task<IHttpActionResult> DeleteCourse(int courseId)
        {
            try
            {
                TARCLearnEntities entities = new TARCLearnEntities();
                var course = await entities.Courses.FirstOrDefaultAsync(c => c.courseId == courseId);
                if (course == null)
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
            catch (Exception e)
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
                Chapters = c.Chapters.Select(ch => new ChapterDetailDto()
                {
                    chapterId = ch.chapterId,
                    chapterNo = ch.chapterNo,
                    chapterTitle = ch.chapterTitle

                })

            }).SingleOrDefaultAsync(c => c.courseId == id);
            var sorted = new List<ChapterDetailDto>(course.Chapters);
            sorted.Sort(new ChapterComparer());
            course.Chapters = sorted;
            if (course == null)
            {
                return Content(HttpStatusCode.NotFound, "Course: " + course.courseId + " not found");
            }
            return Ok(course.Chapters);
        }

        private bool IsNoLecturer(int courseId)
        {
            var db = new TARCLearnEntities();
            var course = db.Courses.FirstOrDefault(c => c.courseId == courseId);
            var lecturerList = course.Users.Where(u => u.isLecturer).ToList();

            return lecturerList.Count < 1;

        }
        private void SendEnrolmentEmail(List<string> address, string course)
        {
            try
            {
                var enrolmentEmail = new MailMessage();
                address.ForEach(a =>
                {
                    enrolmentEmail.To.Add(a);
                });
                enrolmentEmail.Subject = $"TARCLearn enrolment: \"{course}\"";
                enrolmentEmail.From = new MailAddress("tarclearn@gmail.com");
                enrolmentEmail.Body = $"You have been added to the course: {course}";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("tarclearn@gmail.com", "tarclearn1122"),
                    EnableSsl = true
                };
                smtp.Send(enrolmentEmail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            
        }
    }
    
    internal class ChapterComparer : IComparer<ChapterDetailDto>
    {
        public int Compare(ChapterDetailDto x, ChapterDetailDto y)
        {
            var a = double.Parse(x.chapterNo);
            var b = double.Parse(y.chapterNo);
            if (a > b) return 1;
            if (a < b) return -1;
            return 0;
        }
    }

    
}