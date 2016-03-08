﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class NewStudentCountController : ApiController
    {
        private oNew_student_count datacontext = new oNew_student_count();
        public async Task<IHttpActionResult> PostByCurriculumAcademic(oCurriculum_academic data)
        {
            datacontext.curri_id = data.curri_id;
            datacontext.year = data.aca_year;
            object result = await datacontext.SelectWhereByCurriculumAcademic();
            if (result.GetType().ToString().CompareTo("System.String") == 0)
                return InternalServerError(new Exception(result.ToString()));
            else if (((List<oNew_student_count>)result).Count != 0)
            {
                return Ok(((IEnumerable<oNew_student_count>)result).First());
            }
            else
            {
                datacontext.curri_id = data.curri_id;
                datacontext.year = data.aca_year;
                datacontext.num_admis_f = -1;
                datacontext.num_admis_m = -1;
                datacontext.num_childstaff_f = -1;
                datacontext.num_childstaff_m = -1;
                datacontext.num_direct_f = -1;
                datacontext.num_direct_m = -1;
                datacontext.num_goodstudy_f = -1;
                datacontext.num_goodstudy_m = -1;
                datacontext.num_others_f = -1;
                datacontext.num_others_m = -1;
                return Ok(datacontext);
            }
        }
        public async Task<IHttpActionResult> PutNewStudentCount(oNew_student_count data)
        {
            if (data == null)
                return BadRequest("กรุณากรอกข้อมูลสถิติการรับนักศึกษาใหม่ให้เป็นค่าที่ถูกต้องและเหมาะสม");
            object result = await data.InsertOrUpdate();
            if (result == null)
                return Ok();
            else
                return BadRequest(result.ToString());
        }
    }
}
