using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class OthersEvaluationController : ApiController
    {
        private oOthers_evaluation datacontext = new oOthers_evaluation();
        public IHttpActionResult Post(JObject data)
        {
            datacontext.aca_year = Convert.ToInt32(data["aca_year"]);
            datacontext.indicator_num = Convert.ToInt32(data["indicator_num"]);
            datacontext.curri_id = data["curri_id"].ToString();
            return Ok(datacontext.SelectByIndicator());
        }

        public IHttpActionResult Put()
        {
            return Ok();
        }
    }
}
