using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARCLearn.Models
{
    public class CourseDto
    {
        public int courseId { get; set; }
        public string courseCode { get; set; }
        public string courseTitle { get; set; }
    }
    public class CourseDetailDto
    {
        public int courseId { get; set; }
        public string courseCode { get; set; }
        public string courseTitle { get; set; }
        public string courseDescription { get; set; }
    }
    public class CourseUsersDto
    {
        public int courseId { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
    public class CourseChaptersDto
    {
        public int courseId { get; set; }
        public IEnumerable<ChapterDetailDto> Chapters { get; set; }
    }
    
}