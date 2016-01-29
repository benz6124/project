using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class ResearchController : ApiController
    {
        private oResearch datacontext = new oResearch();

        [ActionName("getresearch")]
        public IHttpActionResult PostForQueryResearch([FromBody]string curri_id)
        {
            return Ok(datacontext.SelectWithDetailByCurriculum(curri_id));
        }

        [ActionName("newresearch")]
        public IHttpActionResult PostForNewResearch()
        {
            return Ok();
        }

        public IHttpActionResult PutResearch()
        {
            return Ok();
        }

        public IHttpActionResult DeleteResearch()
        {
            return Ok();
        }

    }
}
