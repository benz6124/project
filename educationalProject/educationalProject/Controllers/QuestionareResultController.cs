using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class QuestionareResultController : ApiController
    {
        private oQuestionare_set datacontext = new oQuestionare_set();
        public async Task<IHttpActionResult> PostForQueryQuestionareResult([FromBody]int questionare_set_id)
        {
            object result = await datacontext.SelectWithResult(questionare_set_id);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return BadRequest(result.ToString());
        }
    }

}
