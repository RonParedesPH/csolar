using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace zachatelhno.Controllers
{
    using zachatelhno.Models;

    public class userController : ApiController
    {
        private zacahtelhnoContext db = new zacahtelhnoContext();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            var user = db.users.Where(c => c.user_id == id).SingleOrDefault();
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        [Route("appTokenRequest/{appName}")]
        [HttpGet]
        public IHttpActionResult appTokenRequest(string appName)
        {
            var app = db.viewUserRoles.Where(c => c.user_name == appName && c.role_name == "app_instance").SingleOrDefault();
            if (app != null)
            {
                var session = new userSession();
                session.user_idf = app.user_idf;
                db.userSessions.Add(session);

                db.SaveChanges();

                return Ok(session.token);
            }
            else
            {
                return NotFound();
            }
        }
   }
}