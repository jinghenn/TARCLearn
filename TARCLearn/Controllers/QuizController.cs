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
        [Route("api/quiz/{quizId}")]
        [ResponseType(typeof(QuizQuestionsDto))]
        public async Task<IHttpActionResult> GetQuizQuestions(int quizId)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var q = db.Quizs.FirstOrDefault(qz => qz.quizId == quizId);
                if (q == null)
                {
                    return Content(HttpStatusCode.NotFound, "Quiz: " + quizId + " not found");
                }
                var quiz = await db.Quizs.Select(qz => new QuizQuestionsDto
                {
                    quizId = qz.quizId,
                    quizTitle = qz.quizTitle,
                    questions = qz.Questions.Select(qn => new QuestionDto
                    {
                        questionId = qn.questionId,
                        questionText = qn.questionText,
                        choices = qn.Choices.Select(c => new ChoiceDto
                        {
                            choiceId = c.choiceId,
                            choiceText = c.choiceText,
                            isAnswer = c.isAnswer
                        })
                    })

                }).SingleOrDefaultAsync(qz => qz.quizId == quizId);
                return Ok(quiz);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpPost]
        [Route("api/quiz" , Name ="QuizRoute")]
        [ResponseType(typeof(QuizQuestionsDto))]
        public async Task<IHttpActionResult> PostQuizQuestions([FromBody] Quiz newQuiz)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                db.Quizs.Add(newQuiz);
                await db.SaveChangesAsync();
                var dto = new QuizQuestionsDto
                {
                    quizId = newQuiz.quizId,
                    quizTitle = newQuiz.quizTitle,
                    questions = newQuiz.Questions.Select(qz => new QuestionDto
                    {
                        questionId = qz.questionId,
                        questionText = qz.questionText,
                        choices = qz.Choices.Select(c => new ChoiceDto
                        {
                            choiceId = c.choiceId,
                            choiceText = c.choiceText,
                            isAnswer = c.isAnswer
                        })
                    })
                };
                return CreatedAtRoute("QuizRoute", new { newQuiz.quizId }, dto);
                
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpDelete]
        [Route("api/quiz/{quizId}")]
        [ResponseType(typeof(QuizQuestionsDto))]
        public async Task<IHttpActionResult> DeleteQuiz(int quizId)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var quiz = await db.Quizs.FirstOrDefaultAsync(q => q.quizId == quizId);
                var dto = new QuizQuestionsDto
                {
                    quizId = quiz.quizId,
                    quizTitle = quiz.quizTitle,
                    questions = quiz.Questions.Select(qz => new QuestionDto
                    {
                        questionId = qz.questionId,
                        questionText = qz.questionText,
                        choices = qz.Choices.Select(c => new ChoiceDto
                        {
                            choiceId = c.choiceId,
                            choiceText = c.choiceText,
                            isAnswer = c.isAnswer
                        })
                    })

                };
                if(quiz == null)
                {
                    return Content(HttpStatusCode.NotFound, "Quiz " + quizId + " not found");
                }
                db.Quizs.Remove(quiz);
                await db.SaveChangesAsync();
                return Ok(dto);
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpPut]
        [Route("api/quiz/{quizId}")]
        [ResponseType(typeof(QuizQuestionsDto))]
        public async Task<IHttpActionResult> PutQuiz(int quizId, [FromBody] Quiz updatedQuiz)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var quiz = await db.Quizs.FirstOrDefaultAsync(q => q.quizId == quizId);
                
                if(quiz == null)
                {
                    return Content(HttpStatusCode.NotFound, "Quiz " + quizId + " not found");
                }
                db.Quizs.Remove(quiz);
                db.Quizs.Add(updatedQuiz);
                await db.SaveChangesAsync();

                
                var dto = new QuizQuestionsDto
                {
                    quizId = updatedQuiz.quizId,
                    quizTitle = updatedQuiz.quizTitle,
                    questions = updatedQuiz.Questions.Select(qz => new QuestionDto
                    {
                        questionId = qz.questionId,
                        questionText = qz.questionText,
                        choices = qz.Choices.Select(c => new ChoiceDto
                        {
                            choiceId = c.choiceId,
                            choiceText = c.choiceText,
                            isAnswer = c.isAnswer
                        })
                    })
                };
                return Ok(dto);

            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }
    }
}