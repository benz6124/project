﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class QuestionareAnswerController : ApiController
    {
        oQuestionare_question_obj datacontext = new oQuestionare_question_obj();
        public async Task<IHttpActionResult> PostForQueryQuestions([FromBody]int questionare_set_id)
        {
            object result = await datacontext.SelectByQuestionIdAsQuestionForm(questionare_set_id);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return BadRequest(result.ToString());
        }

        public async Task<IHttpActionResult> PutForAnswerQuestions(List<object> list)
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

            object result = await datacontext.InsertQuestionAnswer(q);
            if (result == null)
                return Ok();
            else
                return BadRequest(result.ToString());
        }
    }
}
