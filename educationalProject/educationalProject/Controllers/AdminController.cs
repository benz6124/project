using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class AdminController : ApiController
    {
        private oAdmin datacontext = new oAdmin();
        public IHttpActionResult Get()
        {
            return Ok(datacontext.Select());
        }
    }
}
