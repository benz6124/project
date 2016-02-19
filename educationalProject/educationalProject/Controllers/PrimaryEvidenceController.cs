using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using educationalProject.Models;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Models.Wrappers;
using Newtonsoft.Json.Linq;
namespace educationalProject.Controllers
{
    public class PrimaryEvidenceController : ApiController
    {
        private oPrimary_evidence datacontext = new oPrimary_evidence();
        [ActionName("presidentcurriget")]
        public async Task<IHttpActionResult> PostToQueryPrimaryEvidenceDetailByPresidentCurri(JObject data)
        {
            oIndicator inddata = new oIndicator
            {
                aca_year = Convert.ToInt32(data["aca_year"]),
                indicator_num = Convert.ToInt32(data["indicator_num"])
            };
            return Ok(await datacontext.SelectWithDetail(inddata, data["curri_id"].ToString()));
        }

        [ActionName("adminget")]
        public async Task<IHttpActionResult> PostToQueryPrimaryEvidenceDetailByIndicator(oIndicator data)
        {
            return Ok(await datacontext.SelectWhere(string.Format("indicator_num = '{0}' and aca_year = '{1}' and curri_id = '0'", data.indicator_num,data.aca_year)));
        }


        [ActionName("presidentcurrisave")]
        public async Task<IHttpActionResult> PutToSavePrimaryEvidenceDetailByPresidentCurri(List<Primary_evidence_detail> list)
        {
            object result = await datacontext.UpdateDetail(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("adminsave")]
        public async Task<IHttpActionResult> PutToSavePrimaryEvidenceDetailByAdmin(List<Primary_evidence> list)
        {
            object result = await datacontext.UpdateDetail(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("getonlynameandid")]
        public async Task<IHttpActionResult> PostToQueryOnlyNameAndId(JObject data)
        {
            object result = await datacontext.SelectOnlyNameAndId(data["curri_id"].ToString(),Convert.ToInt32(data["aca_year"]), data["teacher_id"].ToString(), Convert.ToInt32(data["indicator_num"]));
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
