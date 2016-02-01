using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.ViewModels;
using educationalProject.Models.Wrappers;
using Newtonsoft.Json.Linq;
namespace educationalProject.Controllers
{
    public class LabListController : ApiController
    {
        private oLab_list datacontext = new oLab_list();
        public IHttpActionResult Get()
        {
            datacontext.aca_year = 2558;
            datacontext.curri_id = "21";
            return Ok(datacontext.SelectByCurriculumAcademic());
        }
        [ActionName("getlablist")]
        public IHttpActionResult PostForQueryLabList(oCurriculum_academic data)
        {
            datacontext.aca_year = data.aca_year;
            datacontext.curri_id = data.curri_id;
            return Ok(datacontext.SelectByCurriculumAcademic());
        }



        [ActionName("newlablist")]
        public IHttpActionResult PostForNewLabList(JObject data)
        {
            Lab_list_detail detail = new Lab_list_detail
            {
                aca_year = Convert.ToInt32(data["aca_year"]),
                curri_id = data["curri_id"].ToString(),
                name = data["name"].ToString(),
                room = data["room"].ToString()
            };
            JArray officer_data = (JArray)data["officer"];
        
            foreach (JObject item in officer_data)
            {
                detail.officer.Add(new Personnel_with_t_name
                {
                    personnel_id = item["personnel_id"].ToString()
                });
            }
            object result = datacontext.InsertNewLabListWithSelect(detail);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("edit")]
        public IHttpActionResult Put(JObject data)
        {
            Lab_list_detail detail = new Lab_list_detail
            {
                aca_year = Convert.ToInt32(data["aca_year"]),
                curri_id = data["curri_id"].ToString(),
                name = data["name"].ToString(),
                room = data["room"].ToString(),
                lab_num = Convert.ToInt32(data["lab_num"]),
            };
            JArray officer_data = (JArray)data["officer"];

            foreach (JObject item in officer_data)
            {
                detail.officer.Add(new Personnel_with_t_name
                {
                    personnel_id = item["personnel_id"].ToString()
                });
            }

            object result = datacontext.UpdateLabListWithSelect(detail);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("delete")]
        public IHttpActionResult PutForDeleteLab(List<Lab_list_detail> list)
        {
            object result = datacontext.Delete(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
