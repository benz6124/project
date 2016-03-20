using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
namespace educationalProject.Controllers
{
    public class SectionSaveController : ApiController
    {
        private oSection_save datacontext = new oSection_save();
        public async Task<IHttpActionResult> Get()
        {
            object res = await datacontext.getHtmlSectionSave();
            string htmlres = res.ToString();
            var strBody = new System.Text.StringBuilder("");

            strBody.Append("<html " +
            "xmlns:o=\"urn:schemas-microsoft-com:office:office\" " +
            "xmlns:w=\"urn:schemas-microsoft-com:office:word\" " +
            "xmlns=\"http://www.w3.org/TR/REC-html40\">" +
            "<head><title></title>\n");
            strBody.Append("<meta name=ProgId content=Word.Document>");

            /*The setting specifies document's view after it is downloaded as Print
               instead of the default Web Layout*/

            strBody.Append(
                //"<!--[if gte mso 9]-->\n" +
               "<xml>\n" +
               "<w:WordDocument>\n" +
               //"<w:properties>\n" +
               "<w:View>Print</w:View>\n" +
               "<w:Zoom>90</w:Zoom>\n" +
               "<w:DoNotOptimizeForBrowser/>\n" +
              // "</w:properties>\n" +
               "</w:WordDocument>\n" +
               "</xml>\n" + ""
               //"<!--[endif]-->\n\n"
               );


            strBody.Append("<style>" +
                                    "<!-- /* Style Definitions */" +
                                    "@page Section1" +
                                    "   {size: 21cm 29.7cm; " +
                                    "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                                    "   mso-header-margin:.5in; " +
                                    "   mso-page-orientation: portrait; " +
                                    "   mso-footer-margin:.5in; mso-paper-source:0;}" +
                                    " div.Section1" +
                                    "   {page:Section1;}" +
                                    "-->" +
                                    "table{" +
                                    "font-family:'Th Sarabun New';font-size:16pt " +
                                    "} \n" +
                                    "h1 {\n" +
                                    "font-size:36pt " +
                                    "}\n" +
                                    "h2 {\n"+
                                    "font-size:24pt " +
                                    "}\n" +
                                    "h3 {\n"+
                                    "font-size:21pt " +
                                    "}\n"+
                                    "h4 {\n"+
                                    "font-size:18pt " +
                                    "}\n" +
                                    "h5 {\n" + 
                                    "font-size:16pt " +
                                    "}\n" +
                                    "h6 {\n " +
                                    "font-size:14pt " +
                                    "}\n" +
            "</style></head>");

            strBody.Append("<body style=\"tab-interval:.5in;font-family:'Th Sarabun New';font-size:16pt\">" +
                                    "<div class=Section1> <h1>testtttt ja</h1><h2>testtttt ja2</h2><h4>testtttt ja4</h4>" +
                                    htmlres +
                                    "</div></body></html>");

            /*Force this content to be downloaded 
            as a Word document*/

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(strBody);
            writer.Flush();
            stream.Position = 0;

            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/msword");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "AUN-QA SAR.doc";
            //return result;

            return ResponseMessage(result);

            /*
            Response.AppendHeader("Content-Type", "application/msword")
    Response.AppendHeader("Content-disposition", _
                           "attachment; filename=myword.doc")
    Response.Write(strBody)
                
    */
        }

        [ActionName("genaunsar")]
        public async Task<IHttpActionResult> PostForGenAunSAR(oCurriculum_academic data)
        {
            if (data == null)
                return BadRequest("กรุณาระบุหลักสูตรและปีการศึกษาที่ต้องการดาวน์โหลดร่างรายงาน");

            object res = await datacontext.getHtmlSectionSave();
            if (res != null)
            {
                string htmlres = res.ToString();
                var strBody = new System.Text.StringBuilder("");

                strBody.Append("<html " +
                "xmlns:o=\"urn:schemas-microsoft-com:office:office\" " +
                "xmlns:w=\"urn:schemas-microsoft-com:office:word\" " +
                "xmlns=\"http://www.w3.org/TR/REC-html40\">" +
                "<head><title></title>\n");
                strBody.Append("<meta name=ProgId content=Word.Document>");

                /*The setting specifies document's view after it is downloaded as Print
                   instead of the default Web Layout*/

                strBody.Append(
                   //"<!--[if gte mso 9]-->\n" +
                   "<xml>\n" +
                   "<w:WordDocument>\n" +
                   //"<w:properties>\n" +
                   "<w:View>Print</w:View>\n" +
                   "<w:Zoom>100</w:Zoom>\n" +
                   "<w:DoNotOptimizeForBrowser/>\n" +
                   // "</w:properties>\n" +
                   "</w:WordDocument>\n" +
                   "</xml>\n" + ""
                   //"<!--[endif]-->\n\n"
                   );


                strBody.Append("<style>" +
                                        "<!-- /* Style Definitions */" +
                                        "@page Section1" +
                                        "   {size: 21cm 29.7cm; " +
                                        "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                                        "   mso-header-margin:.5in; " +
                                        "   mso-page-orientation: portrait; " +
                                        "   mso-footer-margin:.5in; mso-paper-source:0;}" +
                                        " div.Section1" +
                                        "   {page:Section1;}" +
                                        "-->" +
                                        "table{" +
                                        "font-family:'Th Sarabun New';font-size:16pt " +
                                        "} \n" +
                                        "h1 {\n" +
                                        "font-size:36pt " +
                                        "}\n" +
                                        "h2 {\n" +
                                        "font-size:24pt " +
                                        "}\n" +
                                        "h3 {\n" +
                                        "font-size:21pt " +
                                        "}\n" +
                                        "h4 {\n" +
                                        "font-size:18pt " +
                                        "}\n" +
                                        "h5 {\n" +
                                        "font-size:16pt " +
                                        "}\n" +
                                        "h6 {\n " +
                                        "font-size:14pt " +
                                        "}\n" +
                "</style></head>");

                strBody.Append("<body style=\"tab-interval:.5in;font-family:'Th Sarabun New';font-size:16pt\">" +
                                        "<div class=Section1> <h1>testtttt ja</h1><h2>testtttt ja2</h2><h4>testtttt ja4</h4>" +
                                        htmlres +
                                        "</div></body></html>");

                /*Force this content to be downloaded 
                as a Word document*/

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(strBody);
                writer.Flush();
                stream.Position = 0;

                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/msword");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = "AUN-QA SAR.doc";
              
                return ResponseMessage(result);
            }
            else
            {
                return BadRequest("ไม่พบข้อมูลสนับสนุนการประเมินตนเองในระบบ กรุณาดำเนินการบันทึกเข้าสู่ระบบก่อน");
            }
        }


        [ActionName("getsectionsave")]
        public async Task<IHttpActionResult> PostToQuerySectionSave(oSection_save data)
        {
            object result = await data.SelectWhere();
            if (result == null)
                return Ok(data);
            else return InternalServerError(new Exception(result.ToString()));
        }

        public async Task<IHttpActionResult> PutForSectionSave(oSection_save data)
        {
            DateTime d = DateTime.Now;
            datacontext.date = d.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[5];
            datacontext.time = d.GetDateTimeFormats()[101];
            datacontext.aca_year = data.aca_year;
            datacontext.curri_id = data.curri_id;
            datacontext.detail = data.detail != null ? data.detail : ""; ;
            datacontext.strength = data.strength != null ? data.strength : "";
            datacontext.weakness = data.weakness != null ? data.weakness : "";
            datacontext.improve = data.improve != null ? data.improve : "";
            datacontext.indicator_num = data.indicator_num;
            datacontext.sub_indicator_num = data.sub_indicator_num;
            datacontext.teacher_id = data.teacher_id;
                object result = await datacontext.InsertOrUpdate();
                if (result == null)
                    return Ok();
                else
                    return InternalServerError(new Exception(result.ToString()));
        }
    }
}
