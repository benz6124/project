using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class GalleryController : ApiController
    {
        private oGallery datacontext = new oGallery();

        [ActionName("getgallery")]
        public IHttpActionResult PostToQueryGallery(oCurriculum_academic data)
        {
            datacontext.curri_id = data.curri_id;
            datacontext.aca_year = data.aca_year;
            return Ok(datacontext.SelectByCurriculumAcademic());
        }
    }
}
