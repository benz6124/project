﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Models.Wrappers;
using Newtonsoft.Json.Linq;
namespace educationalProject.Controllers
{
    public class LabListController : ApiController
    {
        private oLab_list datacontext = new oLab_list();

        [ActionName("getlablist")]
        public async Task<IHttpActionResult> PostForQueryLabList(oCurriculum_academic data)
        {
            datacontext.aca_year = data.aca_year;
            datacontext.curri_id = data.curri_id;
            return Ok(await datacontext.SelectByCurriculumAcademic());
        }



        [ActionName("newlablist")]
        public async Task<IHttpActionResult> PostForNewLabList(JObject data)
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
                    user_id = Convert.ToInt32(item["user_id"])
                });
            }
            object result = await datacontext.InsertNewLabListWithSelect(detail);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("edit")]
        public async Task<IHttpActionResult> Put(JObject data)
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
                    user_id = Convert.ToInt32(item["user_id"])
                });
            }

            object result = await datacontext.UpdateLabListWithSelect(detail);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return BadRequest(result.ToString());
        }

        [ActionName("delete")]
        public async Task<IHttpActionResult> PutForDeleteLab(List<Lab_list_detail> list)
        {
            object result = await datacontext.Delete(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
