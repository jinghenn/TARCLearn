using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARCLearn.Models
{
    public class ChapterDto
    {
        public string chapterNo { get; set; }
        public string chapterTitle { get; set; }
        
    }
    public class ChapterDetailDto
    {
        public int chapterId { get; set; }
        public string chapterNo { get; set; }
        public string chapterTitle { get; set; }
    }
    public class ChapterMaterialsDto
    {
        public int chapterId { get; set; }
        
        public IEnumerable<MaterialDetailDto> materials{ get; set; }
    }

    public class ChapterDiscussionsDto
    {
        public int chapterId { get; set; }
        public IEnumerable<DiscussionThreadDto> discussionThreads { get; set; }
    }

    //internal class ChapterComparer : IComparer<ChapterDto>
    //{
    //    public string chapterNo { get; set; }
    //    public string chapterTitle { get; set; }

    //    public int Compare(ChapterDto x, ChapterDto y)
    //    {
    //        var a = Convert.ToInt32(x.chapterNo);
    //        var b = Convert.ToInt32(y.chapterNo);
    //        if (a > b) return 1;
    //        if (a < b) return -1;
    //        return 0;
    //    }
    //}
}