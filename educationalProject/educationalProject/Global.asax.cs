﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web;

namespace educationalProject
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        //This constant will affacted in file upload/delete feature
        //public static readonly string SERVERPATH = System.Web.HttpContext.Current.Server.MapPath("~/");
        public static readonly string SERVERPATH = "D:/";
        public static readonly string CONNECTDBERRSTRING = "ไม่สามารถเชื่อมต่อกับฐานข้อมูลได้";

        //REAL RUNNING WEB SERVER BASE URL (USE FOR EVIDENCE FILE LOCATION REFERENCE)
        public static readonly string SERVERURL = System.Configuration.ConfigurationManager.AppSettings["serverurl"].ToString();
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Set server culture to th-TH to let server represent date in thai-base format
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("th-TH");
        }
    }
}
