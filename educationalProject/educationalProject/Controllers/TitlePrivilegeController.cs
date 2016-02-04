using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class TitlePrivilegeController : ApiController
    {
        private oTitle_privilege datacontext = new oTitle_privilege();
        public IHttpActionResult Get()
        {
            return Ok(datacontext.SelectDistinctTitle());
        }
    }
}
