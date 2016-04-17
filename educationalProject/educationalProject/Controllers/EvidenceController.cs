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
        [ActionName("getallevidence")]
        public async Task<IHttpActionResult> PostToQueryAllEvidenceWithCurri([FromBody]string curri_id)
        {
            //datacontext.indicator_num = indicator_num;
            datacontext.curri_id = curri_id;
            object result = await datacontext.SelectAllEvidenceWithCurriculum2();
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }
        [ActionName("getnormal")]
        public async Task<IHttpActionResult> PostByIndicatorAndCurriculum(JObject obj)
        {
            oIndicator data = new oIndicator
            {
                aca_year = Convert.ToInt32(obj["aca_year"]),
                indicator_num = Convert.ToInt32(obj["indicator_num"])
            };
            return Ok(await datacontext.SelectByIndicatorAndCurriculum(data, obj["curri_id"].ToString()));
        }

        [ActionName("getwithtname")]
        public async Task<IHttpActionResult> PostByIndicatorAndCurriculumWithGetName(JObject obj)
        {
            oIndicator data = new oIndicator
            {
                aca_year = Convert.ToInt32(obj["aca_year"]),
                indicator_num = Convert.ToInt32(obj["indicator_num"])
            };
            return Ok(await datacontext.SelectByIndicatorAndCurriculumWithTName(data, obj["curri_id"].ToString()));
        }

        [ActionName("getbycurriculumacademic")]
        public async Task<IHttpActionResult> PostByCurriculumAcademic(oCurriculum_academic data)
        {
            datacontext.curri_id = data.curri_id;
            datacontext.aca_year = data.aca_year;
            return Ok(await datacontext.SelectByCurriculumAcademic());
        }


        [ActionName("newevidence")]
        public async Task<IHttpActionResult> PutNewEvidence()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            string savepath = WebApiApplication.SERVERPATH + "download/evidence";
            string delpath = WebApiApplication.SERVERPATH;
            var result = new MultipartFormDataStreamProvider(savepath);

            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datamodelreceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);

                //New evidence PART
                JObject newevidencedata = (JObject)datamodelreceive["my_new_evidence"];

                datacontext.aca_year = Convert.ToInt32(newevidencedata["aca_year"]);
                datacontext.curri_id = newevidencedata["curri_id"].ToString();
                datacontext.evidence_real_code = Convert.ToInt32(newevidencedata["evidence_real_code"]);
                datacontext.evidence_name = newevidencedata["evidence_name"].ToString();
                datacontext.indicator_num = Convert.ToInt32(newevidencedata["indicator_num"]);
                datacontext.secret = newevidencedata["secret"].ToString()[0];
                datacontext.teacher_id = Convert.ToInt32(newevidencedata["teacher_id"]);

                //EXISTS Evidence PART
                JArray existsevidence = (JArray)datamodelreceive["all_evidences"];

                List<oEvidence> existsevidencelist = new List<oEvidence>();
                foreach (JObject eitem in existsevidence)
                {
                    oEvidence obj = new oEvidence();
                    obj.evidence_code = Convert.ToInt32(eitem["evidence_code"]);
                    obj.file_name = eitem["file_name"].ToString();
                    obj.evidence_name = eitem["evidence_name"].ToString();
                    obj.teacher_id = Convert.ToInt32(eitem["teacher_id"]);
                    obj.secret = eitem["secret"].ToString()[0];
                    obj.curri_id = eitem["curri_id"].ToString();
                    obj.aca_year = Convert.ToInt32(eitem["aca_year"]);
                    obj.evidence_real_code = Convert.ToInt32(eitem["evidence_real_code"]);
                    obj.indicator_num = Convert.ToInt32(eitem["indicator_num"]);
                    obj.primary_evidence_num = Convert.ToInt32(eitem["primary_evidence_num"]);
                    existsevidencelist.Add(obj);
                }

    

                //evidence_real_code evidence_name secret teacher_id
                //GET FILENAME WITH CHANGE FILENAME TO HAVE ITS EXTENSION
                MultipartFileData file = result.FileData[0];
                FileInfo fileInfo = new FileInfo(file.LocalFileName);
                string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                datacontext.file_name = "download/evidence/" + newfilename;
                File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));

                object resultfromdb = await datacontext.InsertNewEvidenceWithSelect(existsevidencelist);
                if (resultfromdb.GetType().ToString() != "System.String")
                {
                    BulkEvidenceTransactionResult bresult = (BulkEvidenceTransactionResult)resultfromdb;

                    //Delete the file which belong to the deleted evidences
                    foreach (string file_name_to_delete in bresult.filenametodellist)
                    {
                        if (File.Exists(string.Format("{0}{1}", delpath, file_name_to_delete)))
                            File.Delete(string.Format("{0}{1}", delpath, file_name_to_delete));
                    }

                    //Check whether insert result is success? If not => delete the uploaded file permanently and return error.
                    if(bresult.message != null)
                    {
                        if (File.Exists(string.Format("{0}/{1}", savepath, newfilename)))
                            File.Delete(string.Format("{0}/{1}", savepath, newfilename));
                        return BadRequest(bresult.message);
                    }

                    return Ok(bresult.mainresult);
                }
                else //ERROR case
                {
                    if (File.Exists(string.Format("{0}/{1}", savepath, newfilename)))
                        File.Delete(string.Format("{0}/{1}", savepath, newfilename));
                    return BadRequest(resultfromdb.ToString());
                }
            }
            catch (Exception e)
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

            string savepath = WebApiApplication.SERVERPATH + "download/evidence";
            string delpath = WebApiApplication.SERVERPATH;
            var result = new MultipartFormDataStreamProvider(savepath);

            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datamodelreceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);

                //New evidence PART
                JObject newprimaryevidencedata = (JObject)datamodelreceive["primary_choose"];

                datacontext.aca_year = Convert.ToInt32(newprimaryevidencedata["aca_year"]);
                datacontext.curri_id = newprimaryevidencedata["curri_id"].ToString();
                datacontext.evidence_real_code = Convert.ToInt32(newprimaryevidencedata["evidence_real_code"]);
                datacontext.evidence_name = newprimaryevidencedata["evidence_name"].ToString();
                datacontext.indicator_num = Convert.ToInt32(newprimaryevidencedata["indicator_num"]);
                datacontext.secret = newprimaryevidencedata["secret"].ToString()[0];
                datacontext.teacher_id = Convert.ToInt32(newprimaryevidencedata["teacher_id"]);
                datacontext.primary_evidence_num = Convert.ToInt32(newprimaryevidencedata["primary_evidence_num"]);

                //EXISTS Evidence PART
                JArray existsevidence = (JArray)datamodelreceive["all_evidences"];

                List<oEvidence> existsevidencelist = new List<oEvidence>();
                foreach (JObject eitem in existsevidence)
                {
                    oEvidence obj = new oEvidence();
                    obj.evidence_code = Convert.ToInt32(eitem["evidence_code"]);
                    obj.file_name = eitem["file_name"].ToString();
                    obj.evidence_name = eitem["evidence_name"].ToString();
                    obj.teacher_id = Convert.ToInt32(eitem["teacher_id"]);
                    obj.secret = eitem["secret"].ToString()[0];
                    obj.curri_id = eitem["curri_id"].ToString();
                    obj.aca_year = Convert.ToInt32(eitem["aca_year"]);
                    obj.evidence_real_code = Convert.ToInt32(eitem["evidence_real_code"]);
                    obj.indicator_num = Convert.ToInt32(eitem["indicator_num"]);
                    obj.primary_evidence_num = Convert.ToInt32(eitem["primary_evidence_num"]);
                    existsevidencelist.Add(obj);
                }

                
                //evidence_real_code evidence_name secret teacher_id
                //GET FILENAME WITH CHANGE FILENAME TO HAVE ITS EXTENSION
                MultipartFileData file = result.FileData[0];
                FileInfo fileInfo = new FileInfo(file.LocalFileName);
                string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                datacontext.file_name = "download/evidence/" + newfilename;
                File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));

                object resultfromdb = await datacontext.InsertNewPrimaryEvidenceWithSelect(existsevidencelist);

                if (resultfromdb.GetType().ToString() != "System.String")
                {
                    BulkEvidenceTransactionResult bresult = (BulkEvidenceTransactionResult)resultfromdb;

                    //Delete the file which belong to the deleted evidences
                    foreach (string file_name_to_delete in bresult.filenametodellist)
                    {
                        if (File.Exists(string.Format("{0}{1}", delpath, file_name_to_delete)))
                            File.Delete(string.Format("{0}{1}", delpath, file_name_to_delete));
                    }
                    
                    return Ok(bresult.mainresult);
                }
                else //ERROR case
                {
                    if (File.Exists(string.Format("{0}/{1}", savepath, newfilename)))
                        File.Delete(string.Format("{0}/{1}", savepath, newfilename));
                    return BadRequest(resultfromdb.ToString());
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [ActionName("newevidencefromothers")]
        public async Task<IHttpActionResult> PutNewEvidenceFromOthers(JObject datamodelreceive)
        {

            //New evidence PART
            JObject newevidenceimportdata = (JObject)datamodelreceive["evidence_import"];

            datacontext.aca_year = Convert.ToInt32(newevidenceimportdata["aca_year"]);
            datacontext.curri_id = newevidenceimportdata["curri_id"].ToString();
            datacontext.evidence_real_code = Convert.ToInt32(newevidenceimportdata["evidence_real_code"]);
            datacontext.evidence_name = newevidenceimportdata["evidence_name"].ToString();
            datacontext.indicator_num = Convert.ToInt32(newevidenceimportdata["indicator_num"]);
            datacontext.secret = newevidenceimportdata["secret"].ToString()[0];
            datacontext.teacher_id = Convert.ToInt32(newevidenceimportdata["teacher_id"]);
            datacontext.file_name = newevidenceimportdata["file_name"].ToString();

            //EXISTS Evidence PART
            JArray existsevidence = (JArray)datamodelreceive["all_evidences"];

            List<oEvidence> existsevidencelist = new List<oEvidence>();
            foreach (JObject eitem in existsevidence)
            {
                oEvidence obj = new oEvidence();
                obj.evidence_code = Convert.ToInt32(eitem["evidence_code"]);
                obj.file_name = eitem["file_name"].ToString();
                obj.evidence_name = eitem["evidence_name"].ToString();
                obj.teacher_id = Convert.ToInt32(eitem["teacher_id"]);
                obj.secret = eitem["secret"].ToString()[0];
                obj.curri_id = eitem["curri_id"].ToString();
                obj.aca_year = Convert.ToInt32(eitem["aca_year"]);
                obj.evidence_real_code = Convert.ToInt32(eitem["evidence_real_code"]);
                obj.indicator_num = Convert.ToInt32(eitem["indicator_num"]);
                obj.primary_evidence_num = Convert.ToInt32(eitem["primary_evidence_num"]);
                existsevidencelist.Add(obj);
            }

            object resultfromdb = await datacontext.InsertNewEvidenceWithSelect(existsevidencelist);
            if (resultfromdb.GetType().ToString() != "System.String")
            {
                BulkEvidenceTransactionResult bresult = (BulkEvidenceTransactionResult)resultfromdb;
                string delpath = WebApiApplication.SERVERPATH;
                //Delete the file which belong to the deleted evidences
                foreach (string file_name_to_delete in bresult.filenametodellist)
                {
                    if (File.Exists(string.Format("{0}{1}", delpath, file_name_to_delete)))
                        File.Delete(string.Format("{0}{1}", delpath, file_name_to_delete));
                }

                //Check whether insert result is success?
                if (bresult.message != null)
                {
                    return BadRequest(bresult.message);
                }

                return Ok(bresult.mainresult);
            }
            else //ERROR case
            {
                return BadRequest(resultfromdb.ToString());
            }
        }

        [ActionName("updateevidence")]
        public async Task<IHttpActionResult> PutForUpdateEvidence(List<Evidence_with_t_name> list)
        {
            //Retrieve delete file_name from update evidence
            object result = await datacontext.Update(list);
            if (result.GetType().ToString() != "System.String")
            {
                string delpath = WebApiApplication.SERVERPATH;
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

            string savepath = WebApiApplication.SERVERPATH + "download/evidence";
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
                datacontext.secret = datareceive["secret"].ToString()[0];
                datacontext.teacher_id = Convert.ToInt32(datareceive["teacher_id"]);
                datacontext.evidence_code = Convert.ToInt32(datareceive["evidence_code"]);

                //evidence_real_code evidence_name secret teacher_id
                //GET FILENAME WITH CHANGE FILENAME TO HAVE ITS EXTENSION
                MultipartFileData file = result.FileData[0];
                FileInfo fileInfo = new FileInfo(file.LocalFileName);
                string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                datacontext.file_name = "download/evidence/" + newfilename;
                File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));

                object resultfromdb = await datacontext.UpdateEvidenceWithSelect();


                if (resultfromdb.GetType().ToString() != "System.String")
                {
                    if (datacontext.file_name[0] == '0')
                    {
                        string delpath = WebApiApplication.SERVERPATH;
                        //delete file that targeted (it has set via datacontext's file_name property  
                        if (datacontext.file_name != "")
                            if (File.Exists(string.Format("{0}{1}", delpath, datacontext.file_name.Substring(1))))
                                File.Delete(string.Format("{0}{1}", delpath, datacontext.file_name.Substring(1)));
                    }
                    return Ok(resultfromdb);
                }
                else
                    return InternalServerError(new Exception(resultfromdb.ToString()));
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
