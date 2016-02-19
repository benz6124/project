using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels.Wrappers;
namespace educationalProject.Controllers
{
    public class EvaluationResultController : ApiController
    {
        private oEvaluation_overall_result datacontext = new oEvaluation_overall_result();
        /*
        [ActionName("individual")]
        public IHttpActionResult PostByIndicatorAndCurriculum(JObject obj)
        {
            oIndicator data = new oIndicator
            {
                aca_year = Convert.ToInt32(obj["aca_year"]),
                indicator_num = Convert.ToInt32(obj["indicator_num"])
            };
            return Ok(datacontext.SelectByIndicatorAndCurriculum(data, obj["curri_id"].ToString()));
        }*/

        [ActionName("overall")]
        public async Task<IHttpActionResult> PostForQueryOverallResult(oCurriculum_academic data)
        {
            return Ok(await datacontext.Select(data));
        }
    }
}
