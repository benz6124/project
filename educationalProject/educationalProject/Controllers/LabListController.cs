using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.ViewModels;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class LabListController : ApiController
    {
        private oLab_list datacontext = new oLab_list();

        [ActionName("getlablist")]
        public IHttpActionResult PostForQueryLabList(oCurriculum_academic data)
        {
            datacontext.aca_year = data.aca_year;
            datacontext.curri_id = data.curri_id;
            return Ok(datacontext.SelectByCurriculumAcademic());
        }

        public IHttpActionResult Get()
        {
            datacontext.aca_year = 2558;
            datacontext.curri_id = "21";
            return Ok(datacontext.SelectByCurriculumAcademic());
        }

        [ActionName("newlablist")]
        public IHttpActionResult PostForNewLabList(Lab_list_detail data)
        {
            object result = datacontext.InsertNewLabListWithSelect(data);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        public IHttpActionResult Put(Lab_list_detail data)
        {
            object result = datacontext.UpdateLabListWithSelect(data);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        public IHttpActionResult Delete(List<Lab_list_detail> list)
        {
            object result = datacontext.Delete(list);
            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
