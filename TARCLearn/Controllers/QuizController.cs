using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using TARCLearn.Models;
using System.Threading.Tasks;

namespace TARCLearn.Controllers
{
    public class QuizController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(IEnumerable<QuestionDto>))]
        public async Task<IHttpActionResult> GetQuizQuestions(int quizId)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var q = db.Quizs.FirstOrDefault(qz => qz.quizId == quizId);
                if(q == null)
                {
                    return Content(HttpStatusCode.NotFound, "Quiz: " + quizId + " not found");
                }
                var quiz = await db.Quizs.Select(qz => new QuizQuestionsDto
                {
                    quizId = qz.quizId,
                    questions = qz.Questions.Select(qn => new QuestionDto
                    {
                        questionId = qn.questionId,
                        questionText = qn.questionText,
                        choices = qn.Choices.Select(c => new ChoiceDto 
                        { 
                            choiceId = c.choiceId,
                            choiceText = c.choiceText
                        })
                    })

                }).SingleOrDefaultAsync(qz => qz.quizId == quizId);
                return Ok(quiz.questions);
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

    }
}