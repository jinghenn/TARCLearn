using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARCLearn.Models
{
    public class UserDto
    {
        public string userId { get; set; }
        public string username { get; set; }
    }
    public class UserDetailDto
    {
        public string userId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool isLecturer { get; set; }
    }
}
namespace TARCLearn.Models
{
    public class UserCoursesDto
    {
        public string userId { get; set; }
        public virtual IEnumerable<string> CourseIds { get; set; }
    }
}