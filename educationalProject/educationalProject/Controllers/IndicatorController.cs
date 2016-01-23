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

        [ActionName("querybycurriculumacademic")]
        public IHttpActionResult PostByCurriculumAcademic(oCurriculum_academic data)
        {
            object result = datacontext.SelectWhereOrderByWithKeepYearSource(string.Format("aca_year=(select max(j.aca_year) from indicator as j where j.aca_year <= {0})", data.aca_year), "indicator_num", null,data.aca_year);
            return Ok(result);
        }

        [ActionName("querybyacademicyear")]
        public IHttpActionResult PostByAcademicYear([FromBody]int year)
        {
            object result = datacontext.SelectWhereOrderByWithKeepYearSource(string.Format("aca_year=(select max(j.aca_year) from indicator as j where j.aca_year <= {0})", year), "indicator_num", null, year);
            return Ok(result);
        }

        public IHttpActionResult GetIndicatorYear()
        {
            return Ok(datacontext.SelectDistinctIndicatorYear());
        }
    }
}
