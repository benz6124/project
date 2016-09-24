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

        [ActionName("insertoredit")]
        public async Task<IHttpActionResult> PostCurriculum2(oCu_curriculum data)
        {
            if (data == null)
                return BadRequest("กรุณากรอกข้อมูลหลักสูตรให้ถูกต้องและมีค่าที่เหมาะสม");
            else if (!(data.period > 0x30 && data.period <= 0x39))
                return BadRequest("กรุณากรอกข้อมูลระยะเวลาการศึกษาตามหลักสูตรให้ถูกต้อง");
            if(data.year == "")
                data.year = (DateTime.Now.Year + 543).ToString();
            object result = await data.InsertOrUpdate();
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return BadRequest(result.ToString());
        }



    }
}
