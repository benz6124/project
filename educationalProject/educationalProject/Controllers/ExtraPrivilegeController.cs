using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class ExtraPrivilegeController : ApiController
    {
        private oExtra_privilege datacontext = new oExtra_privilege();
        public async Task<IHttpActionResult> Post(oExtra_privilege data)
        {
            return Ok(await data.SelectByCurriculumAndTitle());
        }

        public async Task<IHttpActionResult> Put(Extra_privilege_individual_list_with_privilege_choices data)
        {
            object result = await datacontext.InsertOrUpdate(data);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
