//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TARCLearn
{
    using System;
    using System.Collections.Generic;
    
    public partial class Material
    {
        public int materialId { get; set; }
        public int index { get; set; }
        public string materialTitle { get; set; }
        public string materialDescription { get; set; }
        public string materialName { get; set; }
        public bool isVideo { get; set; }
        public string mode { get; set; }
        public int chapterId { get; set; }
    
        public virtual Chapter Chapter { get; set; }
    }
}
