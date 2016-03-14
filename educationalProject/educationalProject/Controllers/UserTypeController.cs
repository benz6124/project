using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class UserTypeController : ApiController
    {
        private oUser_type datacontext = new oUser_type();

        [ActionName("getallusertype")]
        public async Task<IHttpActionResult> GetAllUserType()
        {
            object result = await datacontext.SelectExcludeUserType(-1);
            return Ok(result);
        }

        [ActionName("getexcludeadmin")]
        public async Task<IHttpActionResult> GetUserTypeExcludeAdmin()
        {
            object result = await datacontext.SelectExcludeUserType(0);
            return Ok(result);   
        }

        [ActionName("getexcludeadmcom")]
        public async Task<IHttpActionResult> GetUserTypeExcludeAdminCommitee()
        {
            object result = await datacontext.SelectExcludeUserType(1);
            return Ok(result);
        }

        [ActionName("add")]
        public async Task<IHttpActionResult> PostForAddUserType(JObject data)
        {
            //object result = await datacontext.SelectExcludeUserType(1);
            //return Ok(result);
            return Ok();
        }
    }
}
