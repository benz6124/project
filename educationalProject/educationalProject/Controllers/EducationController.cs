using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class EducationController : ApiController
    {
        public IHttpActionResult Post(oEducation data)
        {
            object result = data.Insert();
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }
        public IHttpActionResult Put(oEducation data)
        {
            object result = data.Update();
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
