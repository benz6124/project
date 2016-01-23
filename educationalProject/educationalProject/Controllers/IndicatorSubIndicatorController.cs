using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.ViewModels.Wrappers;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class IndicatorSubIndicatorController : ApiController
    {
        private oIndicator_sub_indicator_list datacontext = new oIndicator_sub_indicator_list();
        public IHttpActionResult PostToQueryIndicatorSubIndicator([FromBody]int year)
        {
            return Ok(datacontext.SelectByAcademicYear(year));
        }

        [ActionName("saveindicator")]
        public IHttpActionResult PutForUpdateIndicatorSubIndicator(List<oIndicator_sub_indicator_list> list)
        {
            object result;
            if (list.Count == 1 && list.First().indicator_name_t == null)
            {
                oIndicator indicatorcontext = new oIndicator();
                result = indicatorcontext.Delete(string.Format("aca_year = {0}", list.First().aca_year));
            }
            else
                result = datacontext.UpdateEntireList(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("savesubindicator")]
        public IHttpActionResult PutForUpdateSubIndicator(oIndicator_sub_indicator_list data)
        {
            object result;
            if (data.sub_indicator_list.Count == 1 && data.sub_indicator_list.First().sub_indicator_name == null)
            {
                oSub_indicator sub_indicatorcontext = new oSub_indicator();
                result = sub_indicatorcontext.Delete(string.Format("aca_year = {0} and indicator_num = {1}", data.sub_indicator_list.First().aca_year,data.indicator_num));
            }
            else
                result = datacontext.UpdateOnlySubIndicatorList(data.sub_indicator_list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
