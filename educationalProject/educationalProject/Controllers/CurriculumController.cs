using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using educationalProject.Models;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class CurriculumController : ApiController
    {
        private oCu_curriculum datacontext = new oCu_curriculum();
        public IHttpActionResult Get()
        {
            object result = datacontext.Select();
		    return Ok(result);
        }

        public IHttpActionResult PostNewCurriculum(oCu_curriculum data)
        {
            data.year = (DateTime.Now.Year+543).ToString();
            data.period += (char)0x30;
            object result = data.Insert();
            if (result == null)
                return Ok(datacontext.Select());
            else
                return InternalServerError(new Exception(result.ToString()));
        }

    }
}
