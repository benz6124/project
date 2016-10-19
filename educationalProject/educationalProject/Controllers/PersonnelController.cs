using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels.Wrappers;
using Newtonsoft.Json.Linq;
namespace educationalProject.Controllers
{
    public class PersonnelController : ApiController
    {
        private oPersonnel datacontext = new oPersonnel();

        [ActionName("gettnameandid")]
        public async Task<IHttpActionResult> PostToQueryTNameAndId([FromBody]string curri_id)
        {
            return Ok(await datacontext.SelectPersonnelIdAndTName(curri_id,0));
        }

        [ActionName("getalltnameandid")]
        public async Task<IHttpActionResult> PostToQueryTNameAndIdOfAllPersonnel([FromBody]string curri_id)
        {
            return Ok(await datacontext.SelectPersonnelIdAndTName(curri_id, 1));
        }
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await datacontext.SelectPersonnelIdAndTName("999", 1));
        }
        [ActionName("getwitheducation")]
        public async Task<IHttpActionResult> PostToQueryPersonnelWithEducationHistory([FromBody]string curri_id)
        {
            return Ok(await datacontext.selectWithFullDetail(curri_id));
        }

                   
        [ActionName("getonlynameandpfname")]
        public async Task<IHttpActionResult> PostToQueryTNamePFNameAndId([FromBody]string curri_id)
        {
            return Ok(await datacontext.SelectPersonnelWithCurriculum(curri_id));
        }
    }
}
