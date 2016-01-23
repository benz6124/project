using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [ActionName("presidentcurrisave")]
        public IHttpActionResult PutToSavePrimaryEvidenceDetailByPresidentCurri(List<Primary_evidence_detail> list)
        {
            object result = datacontext.UpdateDetail(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
