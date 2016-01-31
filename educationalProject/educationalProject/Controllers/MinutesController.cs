using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class MinutesController : ApiController
    {
        private oMinutes datacontext = new oMinutes();
        public IHttpActionResult Get()
        {
            datacontext.curri_id = "21";
            datacontext.aca_year = 2558;
            return Ok(datacontext.SelectByCurriculumAcademic());
        }
        [ActionName("getminutes")]
        public IHttpActionResult PostForQueryMinutes(oCurriculum_academic data)
        {
            datacontext.curri_id = data.curri_id;
            datacontext.aca_year = data.aca_year;
            return Ok(datacontext.SelectByCurriculumAcademic());
        }

        [ActionName("add")]
        public IHttpActionResult PostForAddNewMinutes()
        {
            return Ok();
        }

        [ActionName("delete")]
        public IHttpActionResult PutForDeleteMinutes(List<Minutes_detail> list)
        {
            object result = datacontext.Delete(list);
            if (result.GetType().ToString() != "System.String")
            {
                //string delpath = HttpContext.Current.Server.MapPath("~/");
                string delpath = "D:/";
                List<string> strlist = (List<string>)result;
                //try catch foreach delete every file that targeted in strlist
                try
                {
                    foreach (string file_name_to_delete in strlist)
                    {
                        if (File.Exists(string.Format("{0}{1}", delpath, file_name_to_delete)))
                            File.Delete(string.Format("{0}{1}", delpath, file_name_to_delete));
                    }
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }
            return Ok();
        }
    }
}
