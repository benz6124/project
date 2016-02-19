using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
using Newtonsoft.Json.Linq;
namespace educationalProject.Controllers
{
    public class QuestionareController : ApiController
    {
        private oQuestionare_set datacontext = new oQuestionare_set();
        [ActionName("getquestionareset")]
        public async Task<IHttpActionResult> PostForQueryQuestionareSet(oCurriculum_academic data)
        {
            return Ok(await datacontext.SelectWithDetail(data));
        }

        [ActionName("add")]
        public async Task<IHttpActionResult> PutForAddQuestionare(Questionare_set_detail_full data)
        {
            data.date = DateTime.Now.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[5];
            object result = await datacontext.InsertNewQuestionareWithSelect(data);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("delete")]
        public async Task<IHttpActionResult> PutForDeleteQuestionare(List<Questionare_set_detail> list)
        {
            object result = await datacontext.Delete(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
