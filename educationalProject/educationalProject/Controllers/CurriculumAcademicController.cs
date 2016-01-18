using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class CurriculumAcademicController : ApiController
    {
        private oCurriculum_academic datacontext = new oCurriculum_academic();
        //Retrieve curriculum_academic data by use Cu_curriculum
        public IHttpActionResult PostByCurriculum(oCu_curriculum data)
        {
            if (data.curri_id == null) return BadRequest("กรุณาเลือกหลักสูตร");
            object result = datacontext.SelectWhere(String.Format("curri_id='{0}'", data.curri_id));
            return Ok(result);
        }
        public IHttpActionResult Get()
        {
            datacontext.curri_id = "21";
            datacontext.aca_year = 2558;
            object result = datacontext.Insert();
            return Ok(result);
        }
    }
}
