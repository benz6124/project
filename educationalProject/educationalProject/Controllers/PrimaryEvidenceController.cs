using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using educationalProject.Models;
using System.Web.Http;
using educationalProject.Models.ViewModels;
using educationalProject.Models.Wrappers;
using Newtonsoft.Json.Linq;
namespace educationalProject.Controllers
{
    public class PrimaryEvidenceController : ApiController
    {
        private oPrimary_evidence datacontext = new oPrimary_evidence();
        [ActionName("presidentcurriget")]
        public IHttpActionResult PostToQueryPrimaryEvidenceDetailByPresidentCurri(JObject data)
        {
            oIndicator inddata = new oIndicator
            {
                aca_year = Convert.ToInt32(data["aca_year"]),
                indicator_num = Convert.ToInt32(data["indicator_num"])
            };
            return Ok(datacontext.SelectWithDetail(inddata, data["curri_id"].ToString()));
        }

        [ActionName("adminget")]
        public IHttpActionResult PostToQueryPrimaryEvidenceDetailByIndicator(oIndicator data)
        {
            return Ok(datacontext.SelectWhere(string.Format("indicator_num = '{0}' and aca_year = '{1}' and curri_id = '0'", data.indicator_num,data.aca_year)));
        }


        [ActionName("presidentcurrisave")]
        public IHttpActionResult PutToSavePrimaryEvidenceDetailByPresidentCurri(List<Primary_evidence_detail> list)
        {
            object result = datacontext.UpdateDetail(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("adminsave")]
        public IHttpActionResult PutToSavePrimaryEvidenceDetailByAdmin(List<Primary_evidence> list)
        {
            object result = datacontext.UpdateDetail(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
