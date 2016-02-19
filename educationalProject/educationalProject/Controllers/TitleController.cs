using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class TitleController : ApiController
    {
        private oTitle datacontext = new oTitle();
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await datacontext.Select());
        }
    }
}
