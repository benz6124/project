using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class EvidenceController : ApiController
    {
        private oEvidence datacontext = new oEvidence();
        [ActionName("getnormal")]
        public IHttpActionResult PostByIndicatorAndCurriculum(JObject obj)
        {
            oIndicator data = new oIndicator
            {
                aca_year = Convert.ToInt32(obj["aca_year"]),
                indicator_num = Convert.ToInt32(obj["indicator_num"])
            };
            return Ok(datacontext.SelectByIndicatorAndCurriculum(data, obj["curri_id"].ToString()));
        }

        [ActionName("getwithtname")]
        public IHttpActionResult PostByIndicatorAndCurriculumWithGetName(JObject obj)
        {
            oIndicator data = new oIndicator
            {
                aca_year = Convert.ToInt32(obj["aca_year"]),
                indicator_num = Convert.ToInt32(obj["indicator_num"])
            };
            return Ok(datacontext.SelectByIndicatorAndCurriculumWithTName(data, obj["curri_id"].ToString()));
        }
    }
}
