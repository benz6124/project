using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class SectionSaveController : ApiController
    {
        private oSection_save datacontext = new oSection_save();
        public IHttpActionResult PostToQuerySectionSave(oSection_save data)
        {
            object result = datacontext.SelectWhere(string.Format("indicator_num = {0} and sub_indicator_num = {1} and aca_year = {2} and curri_id = '{3}'", data.indicator_num, data.sub_indicator_num, data.aca_year, data.curri_id));
            if (result == null)
                return Ok(datacontext);
            else return InternalServerError(new Exception(result.ToString()));
        }
    }
}
