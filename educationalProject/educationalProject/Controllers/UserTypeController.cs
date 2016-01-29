using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class UserTypeController : ApiController
    {
        private oUser_type datacontext = new oUser_type();
        public IHttpActionResult Get()
        {/*/
            object result = datacontext.Select();
            return Ok(result);
            */
            oQuestionare_question_obj d = new oQuestionare_question_obj();
            return Ok(d.SelectByQuestionIdAsQuestionForm(1));
        }
    }
}
