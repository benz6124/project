using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models;
namespace educationalProject.Controllers
{
    public class CurriculumController : ApiController
    {
        public IHttpActionResult Get()
        {
            List<Cu_curriculum> list = new List<Cu_curriculum> (){ 
		        new Cu_curriculum{curr_tname= "วิศวกรรมศาสตร์บัณฑิต 1" },
		        new Cu_curriculum{curr_tname= "วิศวกรรมศาสตร์บัณฑิต 2" },
		        new Cu_curriculum{curr_tname= "วิศวกรรมศาสตร์บัณฑิต 3" },
		        new Cu_curriculum{curr_tname= "วิศวกรรมศาสตร์บัณฑิต 4" },
		        new Cu_curriculum{curr_tname= "วิศวกรรมศาสตร์บัณฑิต 5" },
		        new Cu_curriculum{curr_tname= "วิศวกรรมศาสตร์บัณฑิต 6" },
		        new Cu_curriculum{curr_tname= "วิศวกรรมศาสตร์บัณฑิต 7" } 
	        };
		    return Ok(list);
        }
        public IHttpActionResult Get1(string test)
        {
            return Ok();
        }
        public IHttpActionResult Get2(string test1)
        {
            return Ok();
        }

    }
}
