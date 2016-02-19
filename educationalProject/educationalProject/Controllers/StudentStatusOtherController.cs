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
    public class StudentStatusOtherController : ApiController
    {
        private oStudent_status_other datacontext = new oStudent_status_other();
        public async Task<IHttpActionResult> PostByCurriculumAcademic(oCurriculum_academic data)
        {
            object result = await datacontext.SelectWhere(string.Format("curri_id = {0} and year = {1}", data.curri_id, data.aca_year));
            if (result.GetType().ToString().CompareTo("System.String") == 0)
                return InternalServerError(new Exception(result.ToString()));
            else if (((List<oStudent_status_other>)result).Count != 0)
            {
                return Ok(((IEnumerable<oStudent_status_other>)result).First());
            }
            else {
                datacontext.curri_id = data.curri_id;
                datacontext.year = data.aca_year;
                datacontext.grad_in_time = -1;
                datacontext.grad_over_time = -1;
                datacontext.move_in = -1;
                datacontext.quity1 = -1;
                datacontext.quity2 = -1;
                datacontext.quity3 = -1;
                datacontext.quity4 = -1;
                return Ok(datacontext);
            }
        }
        public async Task<IHttpActionResult> PutStudentStatusOther(oStudent_status_other data)
        {
            object result = await data.InsertOrUpdate();
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
