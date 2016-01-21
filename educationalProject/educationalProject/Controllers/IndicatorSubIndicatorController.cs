using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.ViewModels.Wrappers;
namespace educationalProject.Controllers
{
    public class IndicatorSubIndicatorController : ApiController
    {
        private oIndicator_sub_indicator_list datacontext = new oIndicator_sub_indicator_list();
        public IHttpActionResult PostToQueryIndicatorSubIndicator(int year)
        {
            return Ok(datacontext.SelectByAcademicYear(year));
        }
    }
}
