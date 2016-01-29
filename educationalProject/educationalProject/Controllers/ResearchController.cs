﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
using Newtonsoft.Json.Linq;
namespace educationalProject.Controllers
{
    public class ResearchController : ApiController
    {
        private oResearch datacontext = new oResearch();

        [ActionName("getresearch")]
        public IHttpActionResult PostForQueryResearch([FromBody]string curri_id)
        {
            return Ok(datacontext.SelectWithDetailByCurriculum(curri_id));
        }

        [ActionName("newresearch")]
        public async Task<IHttpActionResult> PostForNewResearch()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            //string savepath = HttpContext.Current.Server.MapPath("~/download/research");
            string savepath = "D:\\download\\research\\";
            var result = new MultipartFormDataStreamProvider(savepath);
            Research_detail data = new Research_detail();
            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                data.curri_id  = datareceive["curri_id"].ToString();
                data.name = datareceive["name"].ToString();
                data.year_publish = Convert.ToInt32(datareceive["year_publish"]);

                JArray tlist = (JArray)datareceive["researcher"];

                foreach (JObject item in tlist)
                {
                    Teacher_with_t_name t = new Teacher_with_t_name
                    {
                        teacher_id = item["teacher_id"].ToString()
                    };
                    data.researcher.Add(t);

                }

                //evidence_real_code evidence_name secret teacher_id
                //GET FILENAME WITH CHANGE FILENAME TO HAVE ITS EXTENSION
                MultipartFileData file = result.FileData[0];
                FileInfo fileInfo = new FileInfo(file.LocalFileName);
                string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                data.file_name = "download/research/" + newfilename;
                File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));

                object resultfromdb = datacontext.InsertNewResearchWithSelect(data);

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

        public async Task<IHttpActionResult> PutResearch()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            //string savepath = HttpContext.Current.Server.MapPath("~/download/research");
            string savepath = "D:\\download\\research\\";
            var result = new MultipartFormDataStreamProvider(savepath);
            Research_detail data = new Research_detail();
            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                data.curri_id = datareceive["curri_id"].ToString();
                data.name = datareceive["name"].ToString();
                data.year_publish = Convert.ToInt32(datareceive["year_publish"]);
                JArray tlist = (JArray)datareceive["researcher"];

                foreach (JObject item in tlist)
                {
                    Teacher_with_t_name t = new Teacher_with_t_name
                    {
                        teacher_id = item["teacher_id"].ToString()
                    };
                    data.researcher.Add(t);

                }
                //evidence_real_code evidence_name secret teacher_id
                //GET FILENAME WITH CHANGE FILENAME TO HAVE ITS EXTENSION
                if (result.FileData.Count != 0)
                {
                    MultipartFileData file = result.FileData[0];
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                    data.file_name = "download/research/" + newfilename;
                    File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));
                }
                else
                {
                    data.file_name = datareceive["file_name"].ToString();
                }

                 object resultfromdb = datacontext.UpdateResearchWithSelect(data);

                if (resultfromdb.GetType().ToString() != "System.String")
                {
                    //string delpath = HttpContext.Current.Server.MapPath("~/");
                    string delpath = "D:/";
                    //Check whether file exists!
                    if (File.Exists(string.Format("{0}{1}", delpath, datacontext.file_name)))
                        File.Delete(string.Format("{0}{1}", delpath, datacontext.file_name));
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

        public IHttpActionResult DeleteResearch(List<Research_detail> list)
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
                    //It may cause by file doesn't exists..
                    return InternalServerError(e);
                }
            }
            return Ok();
        }
    }
}
