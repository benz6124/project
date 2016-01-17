using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels.Wrappers;
namespace educationalProject.Controllers
{
    public class EvaluationResultController : ApiController
    {
        private oEvaluation_overall_result datacontext = new oEvaluation_overall_result();
        public IHttpActionResult PostByIndicatorAndCurriculum(JObject obj)
        {
            oIndicator data = new oIndicator
            {
                aca_year = Convert.ToInt32(obj["aca_year"]),
                indicator_num = Convert.ToInt32(obj["indicator_num"])
            };
            return Ok(datacontext.SelectByIndicatorAndCurriculum(data, obj["curri_id"].ToString()));
        }
       /* public IHttpActionResult Get()
        {
            oIndicator i = new oIndicator();
            i.indicator_num = 15;
            i.aca_year = 2558;
            return Ok(datacontext.SelectByIndicatorAndCurriculum(i, "21"));
           // return Ok(Convert.ToDateTime("2015-12-31",).GetDateTimeFormats()[3]);
        }*/
    }
}
