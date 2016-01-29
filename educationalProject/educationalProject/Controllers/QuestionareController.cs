using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class QuestionareController : ApiController
    {
        private oQuestionare_set datacontext = new oQuestionare_set();
        [ActionName("getquestionareset")]
        public IHttpActionResult PostForQueryQuestionareSet(oCurriculum_academic data)
        {
            return Ok(datacontext.SelectWithDetail(data));
        }

        public IHttpActionResult Put()
        {
            return Ok();
        }

        public IHttpActionResult Delete()
        {
            return Ok();
        }
    }
}
