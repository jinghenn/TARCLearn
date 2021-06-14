using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARCLearn.Models
{
    public class QuestionDto
    {
        public int questionId { get; set; }
        public string questionText { get; set; }
        public IEnumerable<ChoiceDto> choices { get; set; }
    }
}