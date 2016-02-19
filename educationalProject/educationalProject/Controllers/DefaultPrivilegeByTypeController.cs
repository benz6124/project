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
    public class DefaultPrivilegeByTypeController : ApiController
    {
        private oDefault_privilege_by_type datacontext = new oDefault_privilege_by_type();

        public async Task<IHttpActionResult> Post(oDefault_privilege_by_type data)
        {
            return Ok(await data.SelectByTitle());
        }

        public async Task<IHttpActionResult> Put(Default_privilege_by_type_list_with_privilege_choices data)
        {
            object result = await datacontext.InsertOrUpdate(data);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
