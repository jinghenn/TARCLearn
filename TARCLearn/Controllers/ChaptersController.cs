﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Description;
using System.Threading.Tasks;
using TARCLearn.Models;
namespace TARCLearn.Controllers
{
    public class ChaptersController : ApiController
    {
        [HttpPost]
        [Route("api/chapters", Name ="CreateChapter")]
        [ResponseType(typeof(ChapterDto))]
        public async Task<IHttpActionResult> PostChapter(string courseId, [FromBody] ChapterDto newChapter)
        {
            try
            {
                TARCLearnEntities db = new TARCLearnEntities();
                var course = await db.Courses.Include(ch => ch.Chapters)
                    .FirstOrDefaultAsync(c => c.courseId == courseId);
                if(course == null)
                {
                    return Content(HttpStatusCode.NotFound, course);
                }
                var chap = new Chapter()
                {
                    chapterNo = newChapter.chapterNo,
                    chapterTitle = newChapter.chapterTitle
                };
                course.Chapters.Add(chap);
                await db.SaveChangesAsync();
                var dto = await db.Chapters.Select(z => new ChapterDetailDto()
                {
                    chapterId = z.chapterId,
                    chapterNo = z.chapterNo,
                    chapterTitle = z.chapterTitle
                }).SingleOrDefaultAsync(z => z.chapterId == chap.chapterId);
                return CreatedAtRoute("CreateChapter", new { chapterId = chap.chapterId }, dto);
            }catch(Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpDelete]
        [Route("api/chapters")]
        [ResponseType(typeof(ChapterDetailDto))]
        public async Task<IHttpActionResult> DeleteChapter(int chapterId)
        {
            try
            {
                TARCLearnEntities entities = new TARCLearnEntities();
                var chapter = await entities.Chapters.FirstOrDefaultAsync(c => c.chapterId == chapterId);
                if (chapter == null)
                {
                    return Content(HttpStatusCode.NotFound, "Chapter: " + chapterId + " not found");
                }

                var dto = new ChapterDetailDto()
                {
                    chapterId = chapter.chapterId,
                    chapterNo = chapter.chapterNo,
                    chapterTitle = chapter.chapterTitle
                };
                entities.Chapters.Remove(chapter);
                await entities.SaveChangesAsync();
                return Ok(dto);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e);
            }
        }
        
    }
}