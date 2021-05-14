using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TARCLearn.Controllers
{
    public class UsersController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<User> Get()
        {
            using(TARCLearnEntities entities = new TARCLearnEntities())
            {
                return entities.Users.ToList();
            }
        }
        // GET api/<controller>/5
        public HttpResponseMessage Get(string id)
        {
            using (TARCLearnEntities entities = new TARCLearnEntities())
            {

                var entity = entities.Users.FirstOrDefault(e => e.userId == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User with id = " + id + "not found.");
                }
            }
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] User user)
        {
            try
            {
                using (TARCLearnEntities entities = new TARCLearnEntities())
                {
                    entities.Users.Add(user);
                    entities.SaveChanges();
                }
                var message = Request.CreateResponse(HttpStatusCode.Created, user);
                message.Headers.Location = new Uri(Request.RequestUri + user.userId);
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(string id, [FromBody] User user)
        {
            try
            {
                using (TARCLearnEntities entities = new TARCLearnEntities())
                {
                    var entity = entities.Users.FirstOrDefault(e => e.userId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with id = " + id + " not found.");
                    }
                    else
                    {
                        entity.password = user.password;
                        entity.username = user.username;
                        entity.isLecturer = user.isLecturer;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(string id)
        {
            try
            {
                using (TARCLearnEntities entities = new TARCLearnEntities())
                {
                    var entity = entities.Users.FirstOrDefault(e => e.userId == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "User with id = " + id + " not found.");
                    }
                    else
                    {
                        entities.Users.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}