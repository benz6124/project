using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.IO;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class AunBookController : ApiController
    {
        private oAun_book datacontext = new oAun_book();

        public IHttpActionResult Get()
        {
            Dictionary<string, Dictionary<int, int>> test = new Dictionary<string, Dictionary<int,int>>();
            test.Add("21",new Dictionary<int, int>());
            test.Add("22", new Dictionary<int, int>());
            test.Add("23", new Dictionary<int, int>());

            test["21"][1] = 1;
            test["21"][2] = 2;
            test["21"][3] = 4;
            test["21"][15] = 2;

            test["22"][4] = 3;
            test["22"][6] = 2;
            test["22"][3] = 1;

            test["23"][1] = 2;
            test["23"][2] = 2;
            return Ok(test);
        }
        public IHttpActionResult PostToQueryDownloadLinkByCurriculumAcademic(oCurriculum_academic data)
        {
            if (data.curri_id == null) return BadRequest();
            object result = datacontext.SelectFileDownloadLink(string.Format("curri_id = '{0}' and aca_year = {1}", data.curri_id, data.aca_year));
            if (result == null)
                return Ok(datacontext.file_name);
            else if (result.ToString().Contains("notfound"))
                return BadRequest("ไม่พบข้อมูลเล่ม AUN ในหลักสูตร-ปีการศึกษาที่เลือก");
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        public async Task<IHttpActionResult> PutForUpload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType,Request);
            }

            //string savepath = HttpContext.Current.Server.MapPath("~/download/aunbook");
            string savepath = "D:\\download\\aunbook\\";
            var result = new MultipartFormDataStreamProvider(savepath);

            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                datacontext.aca_year = Convert.ToInt32(datareceive["aca_year"]);
                datacontext.curri_id = datareceive["curri_id"].ToString();
                datacontext.date = DateTime.Now.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[5];
                datacontext.personnel_id = Convert.ToInt32(datareceive["personnel_id"]);

                //GET FILENAME WITH CHANGE FILENAME TO HAVE ITS EXTENSION
                MultipartFileData file = result.FileData[0];
                FileInfo fileInfo = new FileInfo(file.LocalFileName);
                string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9),file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                datacontext.file_name = "download/aunbook/"+ newfilename;
                File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath , newfilename));

                string oldfilename = datacontext.file_name;

                object resultfromdb = datacontext.InsertOrUpdate();

                if (resultfromdb == null)
                {
                    //string delpath = HttpContext.Current.Server.MapPath("~/");
                    string delpath = "D:/";
                    //Check whether file name property chenge? (Changes mean there is to-be delete file)
                    if (datacontext.file_name != oldfilename)
                    {
                        //Check whether file exists!
                        if (File.Exists(string.Format("{0}{1}", delpath, datacontext.file_name)))
                            File.Delete(string.Format("{0}{1}", delpath, datacontext.file_name));
                    }
                    return Ok();
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
