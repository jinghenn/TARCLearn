using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARCLearn.Models
{
    public class CourseDto
    {
        public string courseId { get; set; }
        public string courseName { get; set; }
    }
    public class CourseUsersDto
    {
        public string courseId { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
}