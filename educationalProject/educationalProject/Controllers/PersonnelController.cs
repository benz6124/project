using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.ViewModels.Wrappers;
namespace educationalProject.Controllers
{
    public class PersonnelController : ApiController
    {
        private oPersonnel datacontext = new oPersonnel();

        [ActionName("gettnameandid")]
        public IHttpActionResult PostToQueryTNameAndId([FromBody]string curri_id)
        {
            return Ok(datacontext.SelectPersonnelIdAndTName(curri_id));
        }

        [ActionName("getwitheducation")]
        public IHttpActionResult PostToQueryPersonnelWithEducationHistory([FromBody]string curri_id)
        {
            return Ok(datacontext.selectWithFullDetail(curri_id));
        }

                   
        [ActionName("getonlynameandpfname")]
        public IHttpActionResult PostToQueryTNamePFNameAndId([FromBody]string curri_id)
        {
            datacontext.curri_id = curri_id;
            return Ok(datacontext.SelectPersonnelWithCurriculum());
        }
    }
}
