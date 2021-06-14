using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARCLearn.Models
{
    public class QuizDto
    {
        public int quizId { get; set; }
        public string quizTitle { get; set; }
    }
    public class QuizQuestionsDto
    {
        public int quizId { get; set; }
        public IEnumerable<QuestionDto> questions { get; set; }
    }
}