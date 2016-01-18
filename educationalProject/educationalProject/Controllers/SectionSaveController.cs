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
        public IHttpActionResult Get()
        {
            DateTime july28 = new DateTime(2009, 7, 28, 22, 3, 5, 16);

            string[] july28Formats = july28.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"));

            return Ok(DateTime.Now.Year);
            // Print out july28 in all DateTime formats using the default culture.
            int count = 0;
            foreach(string s in july28Formats)
            {
                if (s.CompareTo("2009") == 0) return Ok(count);
                count++;
            }
            return Ok(july28Formats);
        }
        public IHttpActionResult PostToQuerySectionSave(oSection_save data)
        {
            object result = datacontext.SelectWhere(string.Format("indicator_num = {0} and sub_indicator_num = {1} and aca_year = {2} and curri_id = '{3}'", data.indicator_num, data.sub_indicator_num, data.aca_year, data.curri_id));
            if (result == null)
                return Ok(datacontext);
            else return InternalServerError(new Exception(result.ToString()));
        }

        public IHttpActionResult PutForSectionSave(oSection_save data)
        {
            DateTime d = DateTime.Now;
            datacontext.date = d.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[5];
            datacontext.time = d.GetDateTimeFormats()[101];
            datacontext.aca_year = data.aca_year;
            datacontext.curri_id = data.curri_id;
            datacontext.detail = data.detail;
            datacontext.indicator_num = data.indicator_num;
            datacontext.sub_indicator_num = data.sub_indicator_num;
            datacontext.teacher_id = data.teacher_id;
            if (data.date == null || data.time == null)
            {
                object result = datacontext.Insert();
                if (result == null)
                    return Ok();
                else
                    return InternalServerError(new Exception(result.ToString()));

            }
            else {
                object result = datacontext.Update();
                if (result == null)
                    return Ok();
                else
                    return InternalServerError(new Exception(result.ToString()));
            }
        }
    }
}
