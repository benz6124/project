using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels.Wrappers;
using Newtonsoft.Json.Linq;
namespace educationalProject.Controllers
{
    public class SelfEvaluationController : ApiController
    {
        private oSelf_evaluation datacontext = new oSelf_evaluation();
        public async Task<IHttpActionResult> PostToQuerySelfEvaluationData(JObject obj)
        {
            oSelf_evaluation_sub_indicator_name datacontext = new oSelf_evaluation_sub_indicator_name();
            oIndicator data = new oIndicator
            {
                aca_year = Convert.ToInt32(obj["aca_year"]),
                indicator_num = Convert.ToInt32(obj["indicator_num"])
            };
            object result = await datacontext.SelectByIndicatorAndCurriculum(data, obj["curri_id"].ToString());
            return Ok(result);
        }

        public async Task<IHttpActionResult> PutForUpdateSelfEvaluation(List<oSelf_evaluation> list)
        {
            DateTime d = DateTime.Now;
            foreach (oSelf_evaluation item in list)
            {
                item.date = d.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[5];
                item.time = d.GetDateTimeFormats()[101];
            }
            datacontext.aca_year = list.First().aca_year;
            datacontext.curri_id = list.First().curri_id;
            datacontext.indicator_num = list.First().indicator_num;
            object result = await datacontext.InsertOrUpdate(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }


    }
}
