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
    
    public partial class Choice
    {
        public string choiceId { get; set; }
        public string choice1 { get; set; }
        public bool isAnswer { get; set; }
        public string questionId { get; set; }
    
        public virtual Question Question { get; set; }
    }
}