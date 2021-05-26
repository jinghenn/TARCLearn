using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARCLearn.Models
{
    public class CourseDto
    {
        public string courseId { get; set; }
        public string courseTitle { get; set; }
    }
    public class CourseDetailDto
    {
        public string courseId { get; set; }
        public string courseTitle { get; set; }
        public string courseDescription { get; set; }
    }
    public class CourseUsersDto
    {
        public string courseId { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
    
}