using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARCLearn.Models
{
    public class ChoiceDto
    {
        public int choiceId { get; set; }
        public string choiceText { get; set; }
        public bool isAnswer { get; set; }
    }
}