using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using educationalProject.Models;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class CurriculumController : ApiController
    {
        private oCu_curriculum datacontext = new oCu_curriculum();
        public async Task<IHttpActionResult> Get()
        {
            object result = await datacontext.Select();
		    return Ok(result);
        }

        [ActionName("getcurridetail")]
        public async Task<IHttpActionResult> Get(string id)
        {
            datacontext.curri_id = id;
            object result = await datacontext.SelectByCurriID();
            if (result != null)
                return BadRequest(result.ToString());
            return Ok(datacontext);
        }

        public async Task<IHttpActionResult> PostNewCurriculum(oCu_curriculum data)
        {
            if (data == null)
                return BadRequest("กรุณากรอกข้อมูลหลักสูตรให้ถูกต้องและมีค่าที่เหมาะสม");
            else if(!(data.period > 0 && data.period < 10))
                return BadRequest("กรุณากรอกข้อมูลระยะเวลาการศึกษาตามหลักสูตรให้ถูกต้อง");
            data.year = (DateTime.Now.Year+543).ToString();
            data.period += (char)0x30;
            object result =  await data.Insert();
            if (result == null)
                return Ok(await datacontext.Select());
            else
                return BadRequest(result.ToString());
        }

    }
}
