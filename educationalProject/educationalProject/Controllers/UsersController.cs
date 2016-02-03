using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace educationalProject.Controllers
{
    public class UsersController : ApiController
    {
        [ActionName("createnewusers")]
        public IHttpActionResult Post()
        {
            return Ok();
        }
    }
}
