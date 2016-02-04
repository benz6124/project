using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class DefaultPrivilegeByTypeController : ApiController
    {
        private oDefault_privilege_by_type datacontext = new oDefault_privilege_by_type();

        public IHttpActionResult Post(oDefault_privilege_by_type data)
        {
            return Ok(data.SelectByTitle());
        }

        public IHttpActionResult Put(Default_privilege_by_type_list_with_privilege_choices data)
        {
            object result = datacontext.InsertOrUpdate(data);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
