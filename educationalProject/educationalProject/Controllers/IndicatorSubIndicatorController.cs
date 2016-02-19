using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels.Wrappers;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class IndicatorSubIndicatorController : ApiController
    {
        private oIndicator_sub_indicator_list datacontext = new oIndicator_sub_indicator_list();
        public async Task<IHttpActionResult> PostToQueryIndicatorSubIndicator([FromBody]int year)
        {
            return Ok(await datacontext.SelectByAcademicYear(year));
        }

        [ActionName("saveindicator")]
        public async Task<IHttpActionResult> PutForUpdateIndicatorSubIndicator(List<oIndicator_sub_indicator_list> list)
        {
            object result;
            if (list.Count == 1 && list.First().indicator_name_t == null)
            {
                oIndicator indicatorcontext = new oIndicator();
                result = await indicatorcontext.Delete(string.Format("aca_year = {0}", list.First().aca_year));
            }
            else
                result = await datacontext.UpdateEntireList(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("savesubindicator")]
        public async Task<IHttpActionResult> PutForUpdateSubIndicator(oIndicator_sub_indicator_list data)
        {
            object result;
            if (data.sub_indicator_list.Count == 0)
            {
                oSub_indicator sub_indicatorcontext = new oSub_indicator();
                result = await sub_indicatorcontext.Delete(string.Format("aca_year = {0} and indicator_num = {1}", data.aca_year,data.indicator_num));
            }
            else
                result = await datacontext.UpdateOnlySubIndicatorList(data.sub_indicator_list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
