using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class ExtraPrivilegeByTypeController : ApiController
    {
        private oExtra_privilege_by_type datacontext = new oExtra_privilege_by_type();

        public IHttpActionResult Get()
        {
            datacontext.curri_id = "21";
            datacontext.title = "อัลบั้ม";
            return Ok(datacontext.SelectByCurriculumAndTitle());
        }

        public IHttpActionResult Post(oExtra_privilege_by_type data)
        {
            return Ok(data.SelectByCurriculumAndTitle());
        }
    }
}
