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

        [ActionName("getmaxacayear")]
        public IHttpActionResult GetMaxAcademicYear()
        {
            object result = datacontext.SelectMaxAcademicYear();
            return Ok(result);
        }

        [ActionName("getdistinctacayear")]
        public IHttpActionResult GetDistinctAcademicYear()
        {
            object result = datacontext.SelectDistinctAcademicYear();
            return Ok(result);
        }

        [ActionName("getByCurriculum")]
        public IHttpActionResult PostByCurriculum(oCu_curriculum data)
        {
            if (data.curri_id == null) return BadRequest("กรุณาเลือกหลักสูตร");
            datacontext.curri_id = data.curri_id;
            object result = datacontext.SelectByCurriculum();
            return Ok(result);
        }

        [ActionName("add")]
        public IHttpActionResult PostNewCurriculumAcademic(oCurriculum_academic data)
        {
            object result = data.Insert();
            if (result == null)
                return Ok();
            else if (result.ToString().Contains("Duplicate"))
            {
                return BadRequest("มีหลักสูตร-ปีการศึกษานี้อยู่ในระบบแล้ว กรุณาระบุปีการศึกษาใหม่อีกครั้ง");
            }
            else
            {
                return BadRequest(result.ToString());
            }

        }
    }
}
