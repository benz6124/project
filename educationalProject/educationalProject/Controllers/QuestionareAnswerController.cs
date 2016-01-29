using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class QuestionareAnswerController : ApiController
    {
        oQuestionare_question_obj datacontext = new oQuestionare_question_obj();
        public IHttpActionResult PostForQueryQuestions([FromBody]int questionare_set_id)
        {
            return Ok(datacontext.SelectByQuestionIdAsQuestionForm(questionare_set_id));
        }
    }
}
