using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using educationalProject.Models;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class MinutesController : ApiController
    {
        private oMinutes datacontext = new oMinutes();
        public IHttpActionResult Get()
        {
            datacontext.curri_id = "21";
            datacontext.aca_year = 2558;
            return Ok(datacontext.SelectByCurriculumAcademic());
        }
        [ActionName("getminutes")]
        public IHttpActionResult PostForQueryMinutes(oCurriculum_academic data)
        {
            datacontext.curri_id = data.curri_id;
            datacontext.aca_year = data.aca_year;
            return Ok(datacontext.SelectByCurriculumAcademic());
        }

        [ActionName("add")]
        public async Task<IHttpActionResult> PostForAddNewMinutes()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            //string savepathmain = HttpContext.Current.Server.MapPath("~/download/minutes");
            string savepathmain = "D:\\download\\minutes\\";

            //string savepathsub = HttpContext.Current.Server.MapPath("~/myImages/minutes");
            string savepathsub = "D:\\myImages\\minutes\\";

            //string temppath = HttpContext.Current.Server.MapPath("~/temp");
            string temppath = "D:\\temp\\";
            var result = new MultipartFormDataStreamProvider(temppath);
            Minutes_detail data = new Minutes_detail();
            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                data.curri_id = datareceive["curri_id"].ToString();
                data.aca_year = Convert.ToInt32(datareceive["aca_year"]);
                data.topic_name = datareceive["topic_name"].ToString();
                data.teacher_id = Convert.ToInt32(datareceive["teacher_id"]);
                data.date = Convert.ToDateTime(datareceive["date"].ToString(), new System.Globalization.CultureInfo("fr-FR")).GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[5];


                JArray tlist = (JArray)datareceive["attendee"];

                foreach (JObject item in tlist)
                {
                    data.attendee.Add(new Teacher_with_t_name
                    {
                        teacher_id = Convert.ToInt32(item["teacher_id"])
                    });
                }

                //main minutes file
                MultipartFileData file = result.FileData.Last();
                FileInfo fileInfo = new FileInfo(file.LocalFileName);
                string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                data.file_name = "download/minutes/" + newfilename;
                File.Move(string.Format("{0}/{1}", temppath, fileInfo.Name), string.Format("{0}/{1}", savepathmain, newfilename));
                ////-----------------


                tlist = (JArray)datareceive["pictures"];
                if (result.FileData.Count > 1) {
                    int fileind = 0;
                    foreach (JObject item in tlist)
                    {
                        MultipartFileData file1 = result.FileData[fileind++];
                        FileInfo fileInfo1 = new FileInfo(file1.LocalFileName);
                        string newfilename1 = string.Format("{0}.{1}", fileInfo1.Name.Substring(9), file1.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                        data.pictures.Add(new Minutes_pic { file_name = "myImages/minutes/" + newfilename1 });
                        File.Move(string.Format("{0}/{1}", temppath, fileInfo1.Name), string.Format("{0}/{1}", savepathsub, newfilename1));
                    }
                }
                
                object resultfromdb = datacontext.InsertNewMinutesWithSelect(data);

                if (resultfromdb.GetType().ToString() != "System.String")
                    return Ok(resultfromdb);
                else
                    return InternalServerError(new Exception(result.ToString()));

            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }
        }

        [ActionName("delete")]
        public IHttpActionResult PutForDeleteMinutes(List<Minutes_detail> list)
        {
            object result = datacontext.Delete(list);
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
                        if (File.Exists(string.Format("{0}{1}", delpath, file_name_to_delete)))
                            File.Delete(string.Format("{0}{1}", delpath, file_name_to_delete));
                    }
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }
            return Ok();
        }

        [ActionName("edit")]
        public async Task<IHttpActionResult> PutForEditMinutes()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            //string savepathmain = HttpContext.Current.Server.MapPath("~/download/minutes");
            string savepathmain = "D:\\download\\minutes\\";

            //string savepathsub = HttpContext.Current.Server.MapPath("~/myImages/minutes");
            string savepathsub = "D:\\myImages\\minutes\\";

            //string temppath = HttpContext.Current.Server.MapPath("~/temp");
            string temppath = "D:\\temp\\";
            var result = new MultipartFormDataStreamProvider(temppath);
            Minutes_detail data = new Minutes_detail();
            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                data.curri_id = datareceive["curri_id"].ToString();
                data.aca_year = Convert.ToInt32(datareceive["aca_year"]);
                data.topic_name = datareceive["topic_name"].ToString();
                data.teacher_id = Convert.ToInt32(datareceive["teacher_id"]);
                data.date = Convert.ToDateTime(datareceive["date"].ToString(), new System.Globalization.CultureInfo("fr-FR")).GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[5];
                data.minutes_id = Convert.ToInt32(datareceive["minutes_id"]);

                JArray tlist = (JArray)datareceive["attendee"];

                foreach (JObject item in tlist)
                {
                    data.attendee.Add(new Teacher_with_t_name
                    {
                        teacher_id = Convert.ToInt32(item["teacher_id"])
                    });
                }

                int fileind;

                if (datareceive["file_name"].ToString() != "")
                {
                    //main minutes file (if exists)
                    MultipartFileData file = result.FileData.Last();
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                    data.file_name = "download/minutes/" + newfilename;
                    File.Move(string.Format("{0}/{1}", temppath, fileInfo.Name), string.Format("{0}/{1}", savepathmain, newfilename));
                }
                else {
                    data.file_name = "";
                }
                ////---------------
                fileind = 0;

                tlist = (JArray)datareceive["pictures"];
                    foreach (JObject item in tlist)
                    {
                        if (Convert.ToInt32(item["minutes_id"]) == 0)
                        {
                            MultipartFileData file1 = result.FileData[fileind++];
                            FileInfo fileInfo1 = new FileInfo(file1.LocalFileName);
                            string newfilename1 = string.Format("{0}.{1}", fileInfo1.Name.Substring(9), file1.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                            data.pictures.Add(new Minutes_pic { minutes_id = 0, file_name = "myImages/minutes/" + newfilename1 });
                            File.Move(string.Format("{0}/{1}", temppath, fileInfo1.Name), string.Format("{0}/{1}", savepathsub, newfilename1));
                        }
                        else
                        {
                            data.pictures.Add(new Minutes_pic {
                                minutes_id = Convert.ToInt32(item["minutes_id"]),
                                file_name = item["file_name"].ToString()
                            });
                        }
                    }
               
                object resultfromdb = datacontext.UpdateMinutesWithSelect(data);

                if (resultfromdb.GetType().ToString() != "System.String")
                {
                    //string delpath = HttpContext.Current.Server.MapPath("~/");
                    string delpath = "D:/";
                    List<Minutes_pic> picture_delete_list = ((List<Minutes_detail>)resultfromdb).Last().pictures;
                    //try catch foreach delete every file that targeted in strlist

                    foreach (Minutes_pic picture_to_delete in picture_delete_list)
                    {
                        if (File.Exists(string.Format("{0}{1}", delpath, picture_to_delete.file_name)))
                            File.Delete(string.Format("{0}{1}", delpath, picture_to_delete.file_name));
                    }

                    ((List<Minutes_detail>)resultfromdb).Remove(((List<Minutes_detail>)resultfromdb).Last());
                    return Ok(resultfromdb);
                }
                else
                    return InternalServerError(new Exception(result.ToString()));

            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }


    }
}
