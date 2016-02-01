﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class TeacherController : ApiController
    {
        private oTeacher datacontext = new oTeacher();
        [ActionName("getname")]
        public IHttpActionResult PostToQueryTeacherNameInCurri([FromBody]string curri_id)
        {
            return Ok(datacontext.SelectTeacherIdAndTName(curri_id));
        }

        [ActionName("getteacherfull")]
        public IHttpActionResult PostToQueryTeacherWithEducational([FromBody]string curri_id)
        {
            oCurriculum_academic data = new oCurriculum_academic
            {
                curri_id = curri_id
            };
            return Ok(datacontext.SelectPresidentCurriAndAllTeacherInCurri(data));
        }
    }
}
