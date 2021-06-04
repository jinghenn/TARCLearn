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
        
        public IEnumerable<MaterialDetailDto> materials{get;set;}
    }
}