using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.ViewModels.Wrappers;
using educationalProject.Models;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class PresidentCurriculumController : ApiController
    {
        public IHttpActionResult PostToQueryPresidentCurriAndAllTeacherInCurri(Curriculum_academic data)
        {
            oTeacher_educational datacontext = new oTeacher_educational();
            return Ok(datacontext.SelectPresidentCurriAndAllTeacherInCurri(data));
        }
        public IHttpActionResult PutForUpdate(oPresident_curriculum data)
        {
            object result = data.InsertOrUpdate();
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
