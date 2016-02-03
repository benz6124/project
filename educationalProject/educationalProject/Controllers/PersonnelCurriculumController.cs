using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.ViewModels;
using educationalProject.Models.Wrappers;
using educationalProject.Models;
using Newtonsoft.Json.Linq;
namespace educationalProject.Controllers
{
    public class PersonnelCurriculumController : ApiController
    {
        oCurriculum_teacher_staff datacontext = new oCurriculum_teacher_staff();
        public IHttpActionResult Post(JObject data)
        {
            List<Curriculum_teacher_staff> list = new List<Curriculum_teacher_staff>();
            JArray p_list = (JArray)data["these_people"];
            foreach(JObject p in p_list)
            {
                list.Add(new Curriculum_teacher_staff
                {
                    curri_id = data["curri_id"].ToString(),
                    personnel_id = p["personnel_id"].ToString()
                });
            }

            object resultfromdb = datacontext.InsertNewCurriculumTeacherStaffWithSelect(list);

            if (resultfromdb.GetType().ToString() != "System.String")
                return Ok(resultfromdb);
            else
                return InternalServerError(new Exception(resultfromdb.ToString()));
        }

        public IHttpActionResult Put(JObject data)
        {
            List<Curriculum_teacher_staff> list = new List<Curriculum_teacher_staff>();
            JArray p_list = (JArray)data["people"];
            if (p_list != null)
            {
                foreach (JObject p in p_list)
                {
                    list.Add(new Curriculum_teacher_staff
                    {
                        curri_id = data["curri_id"].ToString(),
                        personnel_id = p["personnel_id"].ToString()
                    });
                }
            }
            else
            {
                list.Add(new Curriculum_teacher_staff
                {
                    curri_id = data["curri_id"].ToString(),
                    personnel_id = "0000000000000000000000000000"
                });
            }
            object resultfromdb = datacontext.Delete(list);

            if (resultfromdb == null)
                return Ok();
            else
                return InternalServerError(new Exception(resultfromdb.ToString()));
        }
    }
}
