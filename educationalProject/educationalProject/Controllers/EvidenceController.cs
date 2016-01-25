using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
using System.Threading.Tasks;
using System.IO;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class EvidenceController : ApiController
    {
        private oEvidence datacontext = new oEvidence();
        [ActionName("getnormal")]
        public IHttpActionResult PostByIndicatorAndCurriculum(JObject obj)
        {
            oIndicator data = new oIndicator
            {
                aca_year = Convert.ToInt32(obj["aca_year"]),
                indicator_num = Convert.ToInt32(obj["indicator_num"])
            };
            return Ok(datacontext.SelectByIndicatorAndCurriculum(data, obj["curri_id"].ToString()));
        }

        [ActionName("getwithtname")]
        public IHttpActionResult PostByIndicatorAndCurriculumWithGetName(JObject obj)
        {
            oIndicator data = new oIndicator
            {
                aca_year = Convert.ToInt32(obj["aca_year"]),
                indicator_num = Convert.ToInt32(obj["indicator_num"])
            };
            return Ok(datacontext.SelectByIndicatorAndCurriculumWithTName(data, obj["curri_id"].ToString()));
        }

        [ActionName("getbycurriculumacademic")]
        public IHttpActionResult PostByCurriculumAcademic(oCurriculum_academic data)
        {
            datacontext.curri_id = data.curri_id;
            datacontext.aca_year = data.aca_year;
            return Ok(datacontext.SelectByCurriculumAcademic());
        }

        [ActionName("newevidence")]
        public async Task<IHttpActionResult> PutNewEvidence()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            //string savepath = HttpContext.Current.Server.MapPath("~/download/evidence");
            string savepath = "D:\\download\\evidence\\";
            var result = new MultipartFormDataStreamProvider(savepath);

            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                datacontext.aca_year = Convert.ToInt32(datareceive["aca_year"]);
                datacontext.curri_id = datareceive["curri_id"].ToString();
                datacontext.evidence_real_code = Convert.ToInt32(datareceive["evidence_real_code"]);
                datacontext.evidence_name = "";
                datacontext.indicator_num = Convert.ToInt32(datareceive["indicator_num"]);
                datacontext.secret = Convert.ToChar(datareceive["secret"]);
                datacontext.teacher_id = datareceive["teacher_id"].ToString();

                //evidence_real_code evidence_name secret teacher_id
                //GET FILENAME WITH CHANGE FILENAME TO HAVE ITS EXTENSION
                MultipartFileData file = result.FileData[0];
                FileInfo fileInfo = new FileInfo(file.LocalFileName);
                string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                datacontext.file_name = "download/evidence/" + newfilename;
                File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));

                object resultfromdb = datacontext.InsertNewEvidenceWithSelect();
                if (resultfromdb.GetType().ToString() != "System.String")
                    return Ok();
                else
                    return InternalServerError(new Exception(result.ToString()));
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }
        }


        [ActionName("newprimaryevidence")]
        public async Task<IHttpActionResult> PutNewPrimaryEvidence()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            //string savepath = HttpContext.Current.Server.MapPath("~/download/evidence");
            string savepath = "D:\\download\\evidence\\";
            var result = new MultipartFormDataStreamProvider(savepath);

            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                datacontext.aca_year = Convert.ToInt32(datareceive["aca_year"]);
                datacontext.curri_id = datareceive["curri_id"].ToString();
                datacontext.evidence_real_code = Convert.ToInt32(datareceive["evidence_real_code"]);
                datacontext.indicator_num = Convert.ToInt32(datareceive["indicator_num"]);
                datacontext.secret = Convert.ToChar(datareceive["secret"]);
                datacontext.teacher_id = datareceive["teacher_id"].ToString();
                datacontext.primary_evidence_num = Convert.ToInt32(datareceive["primary_evidence_num"]);

                //evidence_real_code evidence_name secret teacher_id
                //GET FILENAME WITH CHANGE FILENAME TO HAVE ITS EXTENSION
                MultipartFileData file = result.FileData[0];
                FileInfo fileInfo = new FileInfo(file.LocalFileName);
                string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                datacontext.file_name = "download/evidence/" + newfilename;
                File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));

                object resultfromdb = datacontext.InsertNewPrimaryEvidenceWithSelect();
                if (resultfromdb.GetType().ToString() != "System.String")
                    return Ok();
                else
                    return InternalServerError(new Exception(result.ToString()));
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }
        }

        [ActionName("newevidencefromothers")]
        public IHttpActionResult PutNewEvidenceFromOthers(oEvidence data)
        {
            object result = data.InsertNewEvidenceWithSelect();
            if (result.GetType().ToString() != "System.String")
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("updateevidence")]
        public IHttpActionResult PutForUpdateEvidence(List<Evidence_with_t_name> list)
        {
            //Retrieve delete file_name from update evidence
            object result = datacontext.Update(list);
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
                        File.Delete(string.Format("{0}{1}", delpath, file_name_to_delete));
                    }
                }
                catch (Exception e)
                {
                    //It may cause by file doesn't exists..
                    return InternalServerError(e);
                }
                return Ok();
            }
            else
                return InternalServerError(new Exception(result.ToString()));
        }


        [ActionName("updateevidencefile")]
        public async Task<IHttpActionResult> PutForUpdateEvidenceFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            //string savepath = HttpContext.Current.Server.MapPath("~/download/evidence");
            string savepath = "D:\\download\\evidence\\";
            var result = new MultipartFormDataStreamProvider(savepath);

            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                datacontext.aca_year = Convert.ToInt32(datareceive["aca_year"]);
                datacontext.curri_id = datareceive["curri_id"].ToString();
                datacontext.evidence_real_code = Convert.ToInt32(datareceive["evidence_real_code"]);
                datacontext.evidence_name = datareceive["evidence_name"].ToString();
                datacontext.indicator_num = Convert.ToInt32(datareceive["indicator_num"]);
                datacontext.secret = Convert.ToChar(datareceive["secret"]);
                datacontext.teacher_id = datareceive["teacher_id"].ToString();
                datacontext.evidence_code = Convert.ToInt32(datareceive["evidence_code"]);

                //evidence_real_code evidence_name secret teacher_id
                //GET FILENAME WITH CHANGE FILENAME TO HAVE ITS EXTENSION
                MultipartFileData file = result.FileData[0];
                FileInfo fileInfo = new FileInfo(file.LocalFileName);
                string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                datacontext.file_name = "download/evidence/" + newfilename;
                File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));

                object resultfromdb = datacontext.UpdateEvidenceWithSelect();


                if (resultfromdb.GetType().ToString() != "System.String")
                {
                    //string delpath = HttpContext.Current.Server.MapPath("~/");
                    string delpath = "D:/";
                    //delete file that targeted (it has set via datacontext's file_name property  
                    if(datacontext.file_name != "")
                    File.Delete(string.Format("{0}{1}", delpath, datacontext.file_name));
                    return Ok(resultfromdb);
                }
                else
                    return InternalServerError(new Exception(result.ToString()));
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
