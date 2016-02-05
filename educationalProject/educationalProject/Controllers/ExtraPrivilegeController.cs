using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class ExtraPrivilegeController : ApiController
    {
        private oExtra_privilege datacontext = new oExtra_privilege();
        public IHttpActionResult Post(oExtra_privilege data)
        {
            return Ok(data.SelectByCurriculumAndTitle());
        }
    }
}
