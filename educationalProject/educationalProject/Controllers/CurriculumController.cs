using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using educationalProject.Models;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class CurriculumController : ApiController
    {
        private oCu_curriculum datacontext = new oCu_curriculum();
        public async Task<IHttpActionResult> Get()
        {
            object result = await datacontext.Select();
		    return Ok(result);
        }

        public async Task<IHttpActionResult> PostNewCurriculum(oCu_curriculum data)
        {
            data.year = (DateTime.Now.Year+543).ToString();
            data.period += (char)0x30;
            object result =  await data.Insert();
            if (result == null)
                return Ok(await datacontext.Select());
            else
                return InternalServerError(new Exception(result.ToString()));
        }

    }
}
