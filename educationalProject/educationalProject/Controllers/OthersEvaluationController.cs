using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class OthersEvaluationController : ApiController
    {
        private oOthers_evaluation datacontext = new oOthers_evaluation();
        public IHttpActionResult Post(JObject data)
        {
            datacontext.aca_year = Convert.ToInt32(data["aca_year"]);
            datacontext.indicator_num = Convert.ToInt32(data["indicator_num"]);
            datacontext.curri_id = data["curri_id"].ToString();
            return Ok(datacontext.SelectByIndicator());
        }

        public async Task<IHttpActionResult> Put()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            string savepath = WebApiApplication.SERVERPATH + "download/othersevaluation";
            var result = new MultipartFormDataStreamProvider(savepath);

            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);

                Others_evaluation_s_indic_name_list_with_file_name listdata = new Others_evaluation_s_indic_name_list_with_file_name();
                JArray olist = (JArray)datareceive["evaluation_detail"];
                DateTime d = DateTime.Now;
                foreach (JObject obj in olist)
                {
                    Others_evaluation_sub_indicator_name o = new Others_evaluation_sub_indicator_name();
                    o.aca_year = Convert.ToInt32(obj["aca_year"]);
                    o.curri_id = obj["curri_id"].ToString();
                    o.others_evaluation_id = Convert.ToInt32(obj["others_evaluation_id"]);
                    o.assessor_id = Convert.ToInt32(obj["assessor_id"]);
                    o.date = d.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[5];
                    o.time = d.GetDateTimeFormats()[101];
                    if (obj["suggestion"] != null)
                        o.suggestion = obj["suggestion"].ToString();
                    else
                        o.suggestion = "";
                    o.indicator_num = Convert.ToInt32(obj["indicator_num"]);
                    o.sub_indicator_num = Convert.ToInt32(obj["sub_indicator_num"]);
                    o.evaluation_score = Convert.ToInt32(obj["evaluation_score"]);
                    listdata.evaluation_detail.Add(o);
                }
                
                if (result.FileData.Count > 0)
                {
                    MultipartFileData file = result.FileData[0];
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                    listdata.file_name = "download/othersevaluation/" + newfilename;
                    File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));
                }
                else
                {
                    listdata.file_name = "";
                }

                object resultfromdb = datacontext.InsertOrUpdate(listdata);


                if (resultfromdb == null)
                {
                    if (result.FileData.Count > 0)
                    {
                        string delpath = WebApiApplication.SERVERPATH;
                        //delete file that targeted (it has set via datacontext's [suggestion] property  
                        if (datacontext.suggestion != "")
                            if (File.Exists(string.Format("{0}{1}", delpath, datacontext.suggestion)))
                                File.Delete(string.Format("{0}{1}", delpath, datacontext.suggestion));
                    }
                    return Ok();
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
