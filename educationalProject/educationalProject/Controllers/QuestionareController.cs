using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
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

        public IHttpActionResult Delete(List<Questionare_set_detail> list)
        {
            object result = datacontext.Delete(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
