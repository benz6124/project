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
    public class StudentStatusOtherController : ApiController
    {
        private oStudent_status_other datacontext = new oStudent_status_other();
        public async Task<IHttpActionResult> PostByCurriculumAcademic(oCurriculum_academic data)
        {
            datacontext.curri_id = data.curri_id;
            datacontext.year = data.aca_year;
            object result = await datacontext.SelectWhereByCurriculumAcademic();
            if (result.GetType().ToString().CompareTo("System.String") == 0)
                return InternalServerError(new Exception(result.ToString()));
            else
            {
                return Ok(result);
            }
        }
        public async Task<IHttpActionResult> PutStudentStatusOther(oStudent_status_other data)
        {
            if (data == null)
                return BadRequest("กรุณากรอกข้อมูลสถิติการจบการศึกษาให้เป็นค่าที่ถูกต้องและเหมาะสม");
            object result = await data.InsertOrUpdate();
            if (result == null)
                return Ok();
            else
                return BadRequest(result.ToString());
        }
    }
}
