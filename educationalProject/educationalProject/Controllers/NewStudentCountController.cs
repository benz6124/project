using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class NewStudentCountController : ApiController
    {
        private oNew_student_count datacontext = new oNew_student_count();

        /*This method will accept object like
         * {curri_id:"18",aca_year:xxxx,lv:"2"}
         */
        public async Task<IHttpActionResult> PostByCurriculumAcademicAndLevel(JObject data)
        {
            dynamic curri_aca_lv_data = new ExpandoObject();
            curri_aca_lv_data.curri_id = data["curri_id"].ToString();
            curri_aca_lv_data.year = data["aca_year"].ToString();
            curri_aca_lv_data.lv = data["lv"].ToString();


            object result = await datacontext.SelectWhereByCurriculumAcademicAndLevel(curri_aca_lv_data);

            if (result.GetType().ToString().CompareTo("System.String") == 0)
                return InternalServerError(new Exception(result.ToString()));
            else
            {
                return Ok(result);
            }
        }

        
        public async Task<IHttpActionResult> PutNewStudentCount(JObject data)
        {
            dynamic newstudentcountdata = new ExpandoObject();
            if(data["lv"].ToString() == "1")
            {
                newstudentcountdata.num_admis_f = data["num_admis_f"].ToString();
                newstudentcountdata.num_admis_m = data["num_admis_m"].ToString();
                newstudentcountdata.num_childstaff_f = data["num_childstaff_f"].ToString();
                newstudentcountdata.num_childstaff_m = data["num_childstaff_m"].ToString();
                newstudentcountdata.num_direct_f = data["num_direct_f"].ToString();
                newstudentcountdata.num_direct_m = data["num_direct_m"].ToString();
                newstudentcountdata.num_goodstudy_f = data["num_goodstudy_f"].ToString();
                newstudentcountdata.num_goodstudy_m = data["num_goodstudy_m"].ToString();
                newstudentcountdata.num_others_f = data["num_others_f"].ToString();
                newstudentcountdata.num_others_m = data["num_others_m"].ToString();
                newstudentcountdata.curri_id = data["curri_id"].ToString();
                newstudentcountdata.year = data["year"].ToString();
                newstudentcountdata.lv = data["lv"].ToString();
            }
            else if(data["lv"].ToString() == "2" || data["lv"].ToString() == "3")
            {
                newstudentcountdata.a1_m = data["a1_m"].ToString();
                newstudentcountdata.a1_f = data["a1_f"].ToString();
                newstudentcountdata.a2_m = data["a2_m"].ToString();
                newstudentcountdata.a2_f = data["a2_f"].ToString();
                newstudentcountdata.b1_m = data["b1_m"].ToString();
                newstudentcountdata.b1_f = data["b1_f"].ToString();
                newstudentcountdata.fw41_m = data["fw41_m"].ToString();
                newstudentcountdata.fw41_f = data["fw41_f"].ToString();
                newstudentcountdata.others_m = data["others_m"].ToString();
                newstudentcountdata.others_f = data["others_f"].ToString();
                newstudentcountdata.curri_id = data["curri_id"].ToString();
                newstudentcountdata.year = data["year"].ToString();
                newstudentcountdata.lv = data["lv"].ToString();
            }
            else
            {
                return BadRequest();
            }
            object result = await datacontext.InsertOrUpdate(newstudentcountdata);
            if (result == null)
                return Ok();
            else
                return BadRequest(result.ToString());
            
        }
        
    }
}
