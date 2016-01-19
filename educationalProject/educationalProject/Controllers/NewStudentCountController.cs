using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class NewStudentCountController : ApiController
    {
        private oNew_student_count datacontext = new oNew_student_count();
        public IHttpActionResult PostByCurriculumAcademic(oCurriculum_academic data)
        {
            object result = datacontext.SelectWhere(string.Format("curri_id = {0} and year = {1}", data.curri_id, data.aca_year));
            if (result.GetType().ToString().CompareTo("System.String") == 0)
                return InternalServerError(new Exception(result.ToString()));
            else if (((List<oNew_student_count>)result).Count != 0)
            {
                return Ok(((IEnumerable<oNew_student_count>)result).First());
            }
            else
                return Ok(datacontext);
        }
        public IHttpActionResult PutNewStudentCount(oNew_student_count data)
        {
            object result = data.InsertOrUpdate();
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
