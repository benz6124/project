using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class IndicatorController : ApiController
    {
        private oIndicator datacontext = new oIndicator();
        public IHttpActionResult PostByCurriculumAcademic(oCurriculum_academic data)
        {
            object result = datacontext.SelectWhere(String.Format("aca_year='{0}'",data.aca_year));
            return Ok();
        }
    }
}
