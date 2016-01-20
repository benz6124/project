using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class AunBookController : ApiController
    {
        private oAun_book datacontext = new oAun_book();

        public IHttpActionResult Get()
        {
            object result = datacontext.SelectFileDownloadLink(string.Format("curri_id = '55' and aca_year = 2560"));
            return Ok(datacontext.file_name);
        }
        //public IHttpActionResult PostToQueryDownloadLinkByCurriculumAcademic(oCurriculum_academic data)
        //{
        //    if (data.curri_id == null) return BadRequest();
        //    object result = datacontext.SelectFileDownloadLink(string.Format("curri_id = '{0}' and aca_year = {1}",data.curri_id,data.aca_year));
        //    if (result == null)
        //        return Ok(datacontext.file_name);
        //    else if(result.ToString().Contains("notfound"))
        //        return BadRequest("ไม่พบข้อมูลเล่ม AUN ในหลักสูตร-ปีการศึกษาที่เลือก");
        //    else
        //        return InternalServerError(new Exception(result.ToString()));
        //}

        public async Task<IHttpActionResult> PutForUpload()
        {
            //if (!Request.Content.IsMimeMultipartContent())
            //{
            //    return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType,Request);
            //}

            string savepath = HttpContext.Current.Server.MapPath("~/download/aunbook");
            var result = new MultipartFormDataStreamProvider(savepath);

            try
            {
                await Request.Content.ReadAsMultipartAsync(result);
                //var s = result.FormData["model"];

                
                // Retrieve all normal send data from JSON
                foreach (var key in result.FormData.AllKeys)
                {
                    foreach (var val in result.FormData.GetValues(key))
                    {
                        //Trace.WriteLine(string.Format("{0}: {1}", key, val));
                    }
                }

                foreach (MultipartFileData file in result.FileData)
                {
                    string str = file.Headers.ContentDisposition.FileName;
                }

                return Ok();
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
