using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OAuthAuthentication.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            if (this.User.IsInRole("Manager"))
            {
                return new string[] { "value1", "value2", "value3", "value4" };
            }
            else
            {
                return new string[] { "value1", "value2" };
            }
        }

        // GET api/values/5
        [Authorize(Roles = "Administrators")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
