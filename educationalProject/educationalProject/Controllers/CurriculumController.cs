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
        public IHttpActionResult Get1(string test)
        {
            String test1 = "test";
            String ss = test1.GetType().ToString();
            return Ok();
        }

        public IHttpActionResult PostNewCurriculum(oCu_curriculum data)
        {
            data.year = (DateTime.Now.Year+543).ToString();
            object result = data.Insert();
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

    }
}
