using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.ViewModels.Wrappers;
using educationalProject.Models;
namespace educationalProject.Controllers
{
    public class IndicatorSubIndicatorController : ApiController
    {
        private oIndicator_sub_indicator_list datacontext = new oIndicator_sub_indicator_list();
        public IHttpActionResult PostToQueryIndicatorSubIndicator([FromBody]int year)
        {
            return Ok(datacontext.SelectByAcademicYear(year));
        }

        [ActionName("saveindicator")]
        public IHttpActionResult PutForUpdateIndicatorSubIndicator(List<oIndicator_sub_indicator_list> list)
        {
            object result = datacontext.UpdateEntireList(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("savesubindicator")]
        public IHttpActionResult PutForUpdateSubIndicator(List<Sub_indicator> list)
        {
            object result = datacontext.UpdateOnlySubIndicatorList(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
