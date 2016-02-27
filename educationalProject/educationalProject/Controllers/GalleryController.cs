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
    public class GalleryController : ApiController
    {
        private oGallery datacontext = new oGallery();

        [ActionName("getgallery")]
        public async Task<IHttpActionResult> PostToQueryGallery(oCurriculum_academic data)
        {
            datacontext.curri_id = data.curri_id;
            datacontext.aca_year = data.aca_year;
            return Ok(await datacontext.SelectByCurriculumAcademic());
        }

        [ActionName("delete")]
        public async Task<IHttpActionResult> PutForDeleteGallery(List<Gallery_detail> list)
        {
            object result = await datacontext.Delete(list);
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
                    return InternalServerError(e);
                }
            }
            return Ok();
        }

        [ActionName("add")]
        public async Task<IHttpActionResult> PostToAddGalleryWithPicture()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            string savepath = WebApiApplication.SERVERPATH + "myImages/gallery";
            var result = new MultipartFormDataStreamProvider(savepath);
            Gallery_detail data = new Gallery_detail();
            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                data.curri_id = datareceive["curri_id"].ToString();
                data.name = datareceive["name"].ToString();
                data.date_created = DateTime.Now.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[5];
                data.aca_year = Convert.ToInt32(datareceive["aca_year"]);
                data.personnel_id = Convert.ToInt32(datareceive["personnel_id"]);

                JArray tlist = (JArray)datareceive["pictures"];

                int fileind = 0;

                foreach (JObject item in tlist)
                {
                    Picture p = new Picture
                    {
                        caption = item["caption"].ToString()
                    };

                    MultipartFileData file = result.FileData[fileind++];
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                    p.file_name = "myImages/gallery/" + newfilename;
                    File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));
                    data.pictures.Add(p);
                }

              
                object resultfromdb = await datacontext.InsertNewGalleryWithSelect(data);

                if (resultfromdb.GetType().ToString() != "System.String")
                    return Ok(resultfromdb);
                else
                    return InternalServerError(new Exception(resultfromdb.ToString()));

            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }
        }

        [ActionName("edit")]
        public async Task<IHttpActionResult> PutToUpdateGalleryWithPicture()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            string savepath = WebApiApplication.SERVERPATH + "myImages/gallery";
            var result = new MultipartFormDataStreamProvider(savepath);
            Gallery_detail data = new Gallery_detail();
            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                data.curri_id = datareceive["curri_id"].ToString();
                data.name = datareceive["name"].ToString();
                data.date_created = DateTime.Now.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[5];
                data.aca_year = Convert.ToInt32(datareceive["aca_year"]);
                data.personnel_id = Convert.ToInt32(datareceive["personnel_id"]);
                data.gallery_id = Convert.ToInt32(datareceive["gallery_id"]);
                JArray tlist = (JArray)datareceive["pictures"];

                int fileind = 0;

                foreach (JObject item in tlist)
                {
                    Picture p = new Picture
                    {
                        caption = item["caption"].ToString()
                    };
                    if (Convert.ToInt32(item["gallery_id"]) == 0)
                    {
                        MultipartFileData file = result.FileData[fileind++];
                        FileInfo fileInfo = new FileInfo(file.LocalFileName);
                        string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                        p.file_name = "myImages/gallery/" + newfilename;
                        File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));
                        p.gallery_id = 0;
                    }
                    else
                    {
                        p.gallery_id = Convert.ToInt32(item["gallery_id"]);
                        p.file_name = item["file_name"].ToString();
                    }
                    data.pictures.Add(p);
                }


                object resultfromdb = await datacontext.UpdateGalleryWithSelect(data);

                if (resultfromdb.GetType().ToString() != "System.String")
                {
                    string delpath = WebApiApplication.SERVERPATH;
                    List<Picture> picture_delete_list = ((List<Gallery_detail>)resultfromdb).Last().pictures;
                    //try catch foreach delete every file that targeted in strlist
                    
                        foreach (Picture picture_to_delete in picture_delete_list)
                        {
                            if (File.Exists(string.Format("{0}{1}", delpath, picture_to_delete.file_name)))
                                File.Delete(string.Format("{0}{1}", delpath, picture_to_delete.file_name));
                        }

                    ((List<Gallery_detail>)resultfromdb).Remove(((List<Gallery_detail>)resultfromdb).Last());
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
