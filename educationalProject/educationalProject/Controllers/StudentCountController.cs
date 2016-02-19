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
    public class StudentCountController : ApiController
    {
        private oStudent_count datacontext = new oStudent_count();
        public async Task<IHttpActionResult> PostByCurriculumAcademic(oCurriculum_academic data)
        {
            object result = await datacontext.SelectWhere(string.Format("curri_id = {0} and year = {1}", data.curri_id, data.aca_year));
            if (result.GetType().ToString().CompareTo("System.String") == 0)
                return InternalServerError(new Exception(result.ToString()));
            else if (((List<oStudent_count>)result).Count != 0)
            {
                return Ok(((IEnumerable<oStudent_count>)result).First());
            }
            else
            {
                datacontext.curri_id = data.curri_id;
                datacontext.year = data.aca_year;
                datacontext.ny1 = -1;
                datacontext.ny2 = -1;
                datacontext.ny3 = -1;
                datacontext.ny4 = -1;
                datacontext.ny5 = -1;
                datacontext.ny6 = -1;
                datacontext.ny7 = -1;
                datacontext.ny8 = -1;
                return Ok(datacontext);
            }
        }
        public async Task<IHttpActionResult> PutStudentCount(oStudent_count data)
        {
            object result = await data.InsertOrUpdate();
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
