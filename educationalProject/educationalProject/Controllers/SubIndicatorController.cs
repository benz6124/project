﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class SubIndicatorController : ApiController
    {
        private oSub_indicator datacontext = new oSub_indicator();
        //Retrieve sub_indicator by indicator data
        public IHttpActionResult PostByIndicator(oIndicator data)
        {
            object result = datacontext.SelectWhere(String.Format("aca_year = {0} and indicator_num = {1}", data.aca_year, data.indicator_num));
            return Ok(result);
        }
    }
}
