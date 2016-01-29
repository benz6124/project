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
    public class QuestionareAnswerController : ApiController
    {
        oQuestionare_question_obj datacontext = new oQuestionare_question_obj();
        public IHttpActionResult PostForQueryQuestions([FromBody]int questionare_set_id)
        {
            return Ok(datacontext.SelectByQuestionIdAsQuestionForm(questionare_set_id));
        }

        public IHttpActionResult PutForAnswerQuestions(List<object> list)
        {
            Questionare_question_form q = new Questionare_question_form();
            q.suggestion = JObject.Parse(list.Last().ToString())["suggestion"].ToString();
            
            //add answer for each question to pre-last item (pre-last item is suggestion)
            foreach(JObject item in list)
            {
                if (item == list.Last()) break;
                q.question_list.Add(new Questionare_question_answer
                {
                    questionare_set_id = Convert.ToInt32(item["questionare_set_id"]),
                    questionare_question_id = Convert.ToInt32(item["questionare_question_id"]),
                    answer = Convert.ToInt32(item["answer"])
                });
            }

            object result = datacontext.InsertQuestionAnswer(q);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
