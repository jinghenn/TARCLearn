using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARCLearn.Models
{
    public class MaterialDto
    {
        public int materialId { get; set; }
        public int index { get; set; }
        public bool isVideo{ get; set; }
        public string materialTitle { get; set; }
        
    }
    public class MaterialDetailDto
    {
        public int materialId { get; set; }
        public int index { get; set; }
        public string materialTitle { get; set; }
        public string materialDescription { get; set; }
        //public string materialUrl { get; set; }
        public string materialName { get; set; }
        public bool isVideo{ get; set; }
        public string mode { get; set; }

    }
}