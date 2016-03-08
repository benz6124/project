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
    public class EducationController : ApiController
    {
        public async Task<IHttpActionResult> Post(oEducation data)
        {
            if (data == null)
                return BadRequest("กรุณากรอกข้อมูลการศึกษาในแต่ละช่องให้เป็นค่าที่ถูกต้องและเหมาะสม");
            object result = await data.Insert();
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return BadRequest(result.ToString());
        }
        public async Task<IHttpActionResult> Put(oEducation data)
        {
            if (data == null)
                return BadRequest("กรุณากรอกข้อมูลการศึกษาในแต่ละช่องให้เป็นค่าที่ถูกต้องและเหมาะสม");
            object result = await data.Update();
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return BadRequest(result.ToString());
        }
    }
}
