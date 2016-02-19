using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Models.Wrappers;
using educationalProject.Models;
using Newtonsoft.Json.Linq;
namespace educationalProject.Controllers
{
    public class PersonnelCurriculumController : ApiController
    {
        oUser_curriculum datacontext = new oUser_curriculum();
        public async Task<IHttpActionResult> Post(JObject data)
        {
            List<User_curriculum> list = new List<User_curriculum>();
            JArray p_list = (JArray)data["these_people"];
            foreach(JObject p in p_list)
            {
                list.Add(new User_curriculum
                {
                    curri_id = data["curri_id"].ToString(),
                    user_id = Convert.ToInt32(p["user_id"])
                });
            }

            object resultfromdb = await datacontext.InsertNewCurriculumTeacherStaffWithSelect(list);

            if (resultfromdb.GetType().ToString() != "System.String")
                return Ok(resultfromdb);
            else
                return InternalServerError(new Exception(resultfromdb.ToString()));
        }

        public async Task<IHttpActionResult> Put(JObject data)
        {
            List<User_curriculum> list = new List<User_curriculum>();
            JArray p_list = (JArray)data["people"];
            if (p_list != null)
            {
                foreach (JObject p in p_list)
                {
                    list.Add(new User_curriculum
                    {
                        curri_id = data["curri_id"].ToString(),
                        user_id = Convert.ToInt32(p["user_id"])
                    });
                }
            }
            else
            {
                list.Add(new User_curriculum
                {
                    curri_id = data["curri_id"].ToString(),
                    user_id = -999
                });
            }
            object resultfromdb = await datacontext.Delete(list);

            if (resultfromdb == null)
                return Ok();
            else
                return InternalServerError(new Exception(resultfromdb.ToString()));
        }
    }
}
