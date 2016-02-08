using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class CommitteeController : ApiController
    {
        private oCommittee datacontext = new oCommittee();
        [ActionName("getcommittee")]
        public IHttpActionResult PostForQueryCommittee(oCurriculum_academic data)
        {
            datacontext.curri_id = data.curri_id;
            datacontext.aca_year = data.aca_year;
            return Ok(datacontext.SelectWithBriefDetail());
        }

        [ActionName("new")]
        public IHttpActionResult PostForNewCommittee(JObject data)
        {
            List<string> list = new List<string>();
            JArray p_list = (JArray)data["these_people"];
            foreach (JObject p in p_list)
            {
                list.Add(p["teacher_id"].ToString());
            }
            datacontext.curri_id = data["curri_id"].ToString();
            datacontext.aca_year = Convert.ToInt32(data["aca_year"]);
            datacontext.date_promoted = DateTime.Now.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[5];
            object resultfromdb = datacontext.InsertNewCommitteeWithSelect(list);

            if (resultfromdb.GetType().ToString() != "System.String")
                return Ok(resultfromdb);
            else
                return InternalServerError(new Exception(resultfromdb.ToString()));
        }
        
        [ActionName("getnoncommittee")]
        public IHttpActionResult PostForQueryNonCommittee(JObject data)
        {
            List<string> list = new List<string>();
            JArray p_list = (JArray)data["these_people"];
            foreach (JObject p in p_list)
            {
                list.Add(p["teacher_id"].ToString());
            }
            datacontext.curri_id = data["curri_id"].ToString();
            datacontext.aca_year = Convert.ToInt32(data["aca_year"]);

            return Ok(datacontext.SelectNonCommitteeWithBriefDetail(list));
        }
        public IHttpActionResult Put(JObject data)
        {
            List<string> list = new List<string>();
            JArray p_list = (JArray)data["these_people"];
            foreach (JObject p in p_list)
            {
                list.Add(p["teacher_id"].ToString());
            }
            datacontext.curri_id = data["curri_id"].ToString();
            datacontext.aca_year = Convert.ToInt32(data["aca_year"]);
            
            object resultfromdb = datacontext.Delete(list);
            if (resultfromdb == null)
                return Ok();
            else
                return InternalServerError(new Exception(resultfromdb.ToString()));
        }
    }
}
