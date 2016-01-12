using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using educationalProject.Models;
namespace educationalProject.Controllers
{
    public class CurriculumAcademicController : ApiController
    {
        public IHttpActionResult Get()
        {
            List<Curriculum_academic> list = new List<Curriculum_academic>(){
                new Curriculum_academic {aca_year = 2552 },
                new Curriculum_academic {aca_year = 2553 },
                new Curriculum_academic {aca_year = 2554 },
                new Curriculum_academic {aca_year = 2555 },
                new Curriculum_academic {aca_year = 2556 },
                new Curriculum_academic {aca_year = 2557 },
                new Curriculum_academic {aca_year = 2558 },
                new Curriculum_academic {aca_year = 2559 }
            };
            return Ok(list);
        }

        public IHttpActionResult PostByCurriculum(Cu_curriculum obj)
        {
            return Ok();
        }
    }
}
