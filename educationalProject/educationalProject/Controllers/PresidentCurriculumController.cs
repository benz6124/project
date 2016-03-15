using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using educationalProject.Models.ViewModels;
using educationalProject.Models.ViewModels.Wrappers;
using educationalProject.Models;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class PresidentCurriculumController : ApiController
    {
        [ActionName("getindividualpres")]
        public async Task<IHttpActionResult> PostToQueryPresidentCurriAndAllTeacherInCurri(Curriculum_academic data)
        {
            oTeacher_educational datacontext = new oTeacher_educational();
            return Ok(await datacontext.SelectPresidentCurriAndAllTeacherInCurri(data));
        }

        [ActionName("getallpres")]
        public async Task<IHttpActionResult> PostToQueryAllPresident([FromBody] int aca_year)
        {
            if (aca_year < 0)
                return BadRequest("กรุณาระบุปีการศึกษาให้เป็นค่าที่ถูกต้องและเหมาะสม");
            oPresident_curriculum datacontext = new oPresident_curriculum();
            datacontext.aca_year = aca_year;
            return Ok(await datacontext.SelectAllCurriculumsAndAllPresidents());
        }
        [ActionName("saveallpres")]
        public async Task<IHttpActionResult> PutToSaveAllPresident(JObject data)
        {
            Curriculums_presidents_detail cpdata = data["old_object"].ToObject<Curriculums_presidents_detail>();
            oPresident_curriculum datacontext = new oPresident_curriculum();
            datacontext.aca_year = Convert.ToInt32(data["aca_year"]);
            object result = await datacontext.UpdatePresidentData(cpdata);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        public async Task<IHttpActionResult> Get()
        {
            oPresident_curriculum datacontext = new oPresident_curriculum();
            datacontext.aca_year = 2558;
            return Ok(await datacontext.SelectAllCurriculumsAndAllPresidents());
        }
        public async Task<IHttpActionResult> PutForUpdate(oPresident_curriculum data)
        {
            object result = await data.InsertOrUpdate();
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
