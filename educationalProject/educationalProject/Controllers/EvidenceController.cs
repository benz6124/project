using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class EvidenceController : ApiController
    {
        private oEvidence datacontext = new oEvidence();
        public IHttpActionResult Get()
        {
            List<Primary_evidence_detail> list = new List<Primary_evidence_detail>();
            list.Add(new Primary_evidence_detail
            {
                aca_year = 2558,
                evidence_name = "หลักฐาน1",
                curri_id = "21",
                indicator_num = 1,
                primary_evidence_num = 1,
                status = '0',
                teacher_id = "00007"
            });
            list.Add(new Primary_evidence_detail
            {
                aca_year = 2558,
                evidence_name = "หลักฐาน2",
                curri_id = "21",
                indicator_num = 1,
                primary_evidence_num = 2,
                status = '1',
                teacher_id = "00008"
            });
            list.Add(new Primary_evidence_detail
            {
                aca_year = 2558,
                evidence_name = "หลักฐาน3",
                curri_id = "21",
                indicator_num = 1,
                primary_evidence_num = 3,
                status = '0',
                teacher_id = "00001"
            });
            list.Add(new Primary_evidence_detail
            {
                aca_year = 2558,
                evidence_name = "หลักฐาน4",
                curri_id = "21",
                indicator_num = 1,
                primary_evidence_num = 4,
                status = '6',
                teacher_id = ""
            });

            list.Add(new Primary_evidence_detail
            {
                aca_year = 2558,
                evidence_name = "หลักฐาน4",
                curri_id = "21",
                indicator_num = 1,
                primary_evidence_num = 4,
                status = '6',
                teacher_id = ""
            });

            list.Add(new Primary_evidence_detail
            {
                aca_year = 2558,
                evidence_name = "หลักฐาน4",
                curri_id = "21",
                indicator_num = 1,
                primary_evidence_num = 5,
                status = '4',
                teacher_id = "00002"
            });

            list.Add(new Primary_evidence_detail
            {
                aca_year = 2558,
                evidence_name = "หลักฐาน4",
                curri_id = "21",
                indicator_num = 1,
                primary_evidence_num = 6,
                status = '5',
                teacher_id = "00002"
            });
            return Ok(list);
        }
        public IHttpActionResult PostByIndicatorAndCurriculum(JObject obj)
        {
            oIndicator data = new oIndicator
            {
                aca_year = Convert.ToInt32(obj["aca_year"]),
                indicator_num = Convert.ToInt32(obj["indicator_num"])
            };
            return Ok(datacontext.SelectByIndicatorAndCurriculum(data, obj["curri_id"].ToString()));
        }
    }
}
