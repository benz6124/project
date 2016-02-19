using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class TeacherController : ApiController
    {
        private oTeacher datacontext = new oTeacher();
        [ActionName("getname")]
        public async Task<IHttpActionResult> PostToQueryTeacherNameInCurri([FromBody]string curri_id)
        {
            return Ok(await datacontext.SelectTeacherIdAndTName(curri_id));
        }


        public IHttpActionResult Get()
        {
            List<UsernamePassword> list = new List<UsernamePassword>();
            list.Add(new UsernamePassword("benza4@hotmail.com", "1"));
            list.Add(new UsernamePassword("benza@hotmail.com", "1"));
            List<string> curri = new List<string>();
            //curri.Add("20"); curri.Add("21");
            return Ok(datacontext.Insert(list,curri));
        }
    }
}
