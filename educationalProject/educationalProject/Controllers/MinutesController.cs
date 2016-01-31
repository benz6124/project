using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class MinutesController : ApiController
    {
        private oMinutes datacontext = new oMinutes();

        [ActionName("getminutes")]
        public IHttpActionResult PostForQueryMinutes(oCurriculum_academic data)
        {
            datacontext.curri_id = data.curri_id;
            datacontext.aca_year = data.aca_year;
            return Ok(datacontext.SelectByCurriculumAcademic());
        }

        [ActionName("add")]
        public IHttpActionResult PostForAddNewMinutes()
        {
            return Ok();
        }

        [ActionName("delete")]
        public IHttpActionResult PutForDeleteMinutes()
        {
            return Ok();
        }
    }
}
