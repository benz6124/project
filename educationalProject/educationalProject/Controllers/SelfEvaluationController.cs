using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels.Wrappers;
using Newtonsoft.Json.Linq;
namespace educationalProject.Controllers
{
    public class SelfEvaluationController : ApiController
    {
        public IHttpActionResult PostToQuerySelfEvaluationData(JObject obj)
        {
            oSelf_evaluation_sub_indicator_name datacontext = new oSelf_evaluation_sub_indicator_name();
            oIndicator data = new oIndicator
            {
                aca_year = Convert.ToInt32(obj["aca_year"]),
                indicator_num = Convert.ToInt32(obj["indicator_num"])
            };
            object result = datacontext.SelectByIndicatorAndCurriculum(data, obj["curri_id"].ToString());
            if (result != null)
                return Ok(result);
            else return Ok("");
        }
    }
}
