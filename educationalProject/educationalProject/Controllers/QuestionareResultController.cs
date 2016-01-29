using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class QuestionareResultController : ApiController
    {
        private oQuestionare_set datacontext = new oQuestionare_set();
        public IHttpActionResult PostForQueryQuestionareResult([FromBody]int questionare_set_id)
        {
            return Ok(datacontext.SelectWithResult(questionare_set_id));
        }
    }

}
