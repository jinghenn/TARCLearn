using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARCLearn.Models
{
    public class MessageDetailDto
    {
        public int messageId { get; set; }
        public string message { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
    }
    public class MessageDto
    {
        public int messageId { get; set; }
        public string message { get; set; }
    }
}