using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARCLearn.Models
{
    public class DiscussionThreadDto
    {
        public int threadId { get; set; }
        public string threadTitle { get; set; }
        public string userName { get; set; }
        
    }

    public class DiscussionThreadDetailDto
    {
        public int threadId { get; set; }
        public string threadTitle { get; set; }
        public string threadDescription { get; set; }
        public int chapterId { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
    }

    public class DiscussionAboutDto
    {
        public int threadId { get; set; }
        public string threadTitle { get; set; }
        public string threadDescription { get; set; }
    }

}