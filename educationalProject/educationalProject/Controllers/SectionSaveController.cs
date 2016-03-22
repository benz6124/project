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
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class SectionSaveController : ApiController
    {
        private oSection_save datacontext = new oSection_save();
        public async Task<IHttpActionResult> Get()
        {
            datacontext.curri_id = "21";
            datacontext.aca_year = 2558;
            object res = await datacontext.getSectionSaveDataForSAR();
            if (res.GetType().ToString() != "System.String")
            {
                //Start to generate SAR doc
                SAR reportobject = (SAR)res;

                var strBody = new System.Text.StringBuilder("");

                strBody.Append("<html " +
                "xmlns:o=\"urn:schemas-microsoft-com:office:office\" " +
                "xmlns:w=\"urn:schemas-microsoft-com:office:word\" " +
                "xmlns=\"http://www.w3.org/TR/REC-html40\">" +
                "<head><title></title>\n");
                //strBody.Append("<meta name=ProgId content=Word.Document>");

                strBody.Append(
                   "<!--[if gte mso 9]>\n" +
                   "<xml>\n" +
                   "<w:WordDocument>\n" +
                   "<w:View>Print</w:View>\n" +
                   "<w:Zoom>90</w:Zoom>\n" +
                   "<w:DoNotOptimizeForBrowser/>\n" +
                   "</w:WordDocument>\n" +
                   "</xml>\n" +
                   "<!--[endif]>\n\n"
                   );


                strBody.Append("<style>" +
                                        "<!-- /* Style Definitions */" +
                                        "@page Section1" +
                                        "   {size: 21cm 29.7cm; " +
                                        "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                                        "   mso-header-margin:.5in; " +
                                        "   mso-page-orientation: portrait; " +
                                        "   mso-footer-margin:.5in; mso-paper-source:0; " +
                                        "   mso-footer: f1; } " +
                                        " div.Section1" +
                                        "   {page:Section1;}" +
                                        "-->" +
                                        "table{" +
                                        "font-family:'Th Sarabun New';font-size:16pt; " +
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
                                        "p { margin:0 } \n " +

                                        "table.evidence,table.selfevalres {" +
                                        "border:1px solid black; " +
                                        "border-collapse:collapse; " +
                                        "} " +
                                        
                                        "table.evidence th,table.evidence td, " +
                                        "table.selfevalres th,table.selfevalres td { border:1px solid black; } " +
                                        
                                        "ol.sar-ol li { margin:0 auto 0 auto } " +
                "</style></head>");

                strBody.Append("<body style=\"tab-interval:.5in;font-family:'Th Sarabun New';font-size:16pt\">" +
                                        "<div class=Section1>");
                //BODY SECTION => read SAR object to gather data


                foreach (Indicator_with_section_save_list i in reportobject.indicator_section_save_list)
                {
                    strBody.Append(string.Format("<b>AUN.{0} {1} </b><br>",i.indicator_num,i.indicator_name));

                    foreach (Section_save_with_sub_indicator_detail s in i.section_save_list) {
                        strBody.Append(string.Format("<b>{0}.{1} {2}</b><br>", i.indicator_num, s.sub_indicator_num, s.sub_indicator_name));

                        strBody.Append(string.Format(s.detail + "<br><br>"));
                    }

                    strBody.Append("<b>รายการเอกสารหลักฐาน</b><br>");
                    if(i.evidence_list.Count == 0)
                    {
                        strBody.Append("--ไม่พบข้อมูล--<br><br clear=all style='mso-special-character:line-break;page-break-before:always'>");
                    }
                    else
                    {
                        strBody.Append("<table class=\"evidence\"><tr><th width=100>รหัสเอกสาร</th><th>รายการ</th></tr>");
                        foreach (Evidence_detail_for_SAR e in i.evidence_list)
                        {
                            strBody.Append(string.Format("<tr><td align=\"center\">{0}-{1}</td><td>{2}</td></tr>",e.indicator_num,e.evidence_real_code,e.evidence_name));
                        }
                        strBody.Append("</table><br><br clear=all style='mso-special-character:line-break;page-break-before:always'>");
                    }

                }

                strBody.Append("<b>วิเคราะห์จุดแข็งและจุดอ่อน</b><br><br>");
                foreach (Indicator_with_section_save_list i in reportobject.indicator_section_save_list)
                {
                    strBody.Append(string.Format("<b>AUN.{0} {1} </b><br>", i.indicator_num, i.indicator_name));

                    strBody.Append(string.Format("<b>จุดแข็ง</b><br>"));
                    string strtoinsert = "";

                    //INSERT STRENGTH
                    foreach (Section_save_with_sub_indicator_detail s in i.section_save_list)
                    {
                        if(s.strength != "--ไม่พบข้อมูล--")
                            strtoinsert += string.Format("<li>{0}</li>", s.strength);
                    }
                    if(strtoinsert != "")
                    {
                        strBody.Append(string.Format("<ol class=\"sar-ol\">{0}</ol>", strtoinsert));
                    }
                    else
                    {
                        strBody.Append("--ไม่พบข้อมูล--<br>");
                    }


                    strtoinsert = "";

                    strBody.Append(string.Format("<b>จุดอ่อน</b><br>"));
                    //INSERT WEAKNESS
                    foreach (Section_save_with_sub_indicator_detail s in i.section_save_list)
                    {
                        if (s.weakness != "--ไม่พบข้อมูล--")
                            strtoinsert += string.Format("<li>{0}</li>", s.weakness);
                    }
                    if (strtoinsert != "")
                    {
                        strBody.Append(string.Format("<ol class=\"sar-ol\">{0}</ol>", strtoinsert));
                    }
                    else
                    {
                        strBody.Append("--ไม่พบข้อมูล--<br>");
                    }


                    strtoinsert = "";

                    strBody.Append(string.Format("<b>จุดที่ควรพัฒนา</b><br>"));
                    //INSERT AREA OF IMPROVEMENT
                    foreach (Section_save_with_sub_indicator_detail s in i.section_save_list)
                    {
                        if (s.improve != "--ไม่พบข้อมูล--")
                            strtoinsert += string.Format("<li>{0}</li>", s.improve);
                    }
                    if (strtoinsert != "")
                    {
                        strBody.Append(string.Format("<ol class=\"sar-ol\">{0}</ol>", strtoinsert));
                    }
                    else
                    {
                        strBody.Append("--ไม่พบข้อมูล--<br>");
                    }
                    if (i != reportobject.indicator_section_save_list.Last())
                        strBody.Append("<br>");
                    else
                        strBody.Append("<br clear=all style='mso-special-character:line-break;page-break-before:always'>");
                }

                int overallscoresum = 0;
                int overalldivisor = 0;
                strBody.Append("<b>สรุปผลการประเมินตนเอง</b><br>");
                strBody.Append("<table class=\"selfevalres\">");

                foreach (Indicator_with_section_save_list i in reportobject.indicator_section_save_list)
                {
                    //Header row for each indicator
                    strBody.Append(string.Format("<tr><th>{0}</th> <td><b>{1}</b></td> <th style=\"width:0.85cm\">1</th><th style=\"width:0.85cm\">2</th><th style=\"width:0.85cm\">3</th><th style=\"width:0.85cm\">4</th><th style=\"width:0.85cm\">5</th><th style=\"width:0.85cm\">6</th><th style=\"width:0.85cm\">7</th></tr>", i.indicator_num, i.indicator_name));

                    foreach (Section_save_with_sub_indicator_detail s in i.section_save_list)
                    {
                        Self_evaluation_tiny_detail target = reportobject.indicator_self_evaluation_list.First(t => t.indicator_num == i.indicator_num).self_evaluation_list.First(u => u.sub_indicator_num == s.sub_indicator_num);
                        strBody.Append(string.Format("<tr><td align=\"center\">{0}.{1}</td> <td>{2}</td>", i.indicator_num, s.sub_indicator_num, s.sub_indicator_name));
                        for(int score = 1;score <= 7; score++)
                        {
                            if(score == target.evaluation_score)
                            {
                                strBody.Append(string.Format("<td align=\"center\">&#x2713;</td>"));
                            }
                            else
                            {
                                strBody.Append(string.Format("<td></td>"));
                            }
                        }
                        strBody.Append("</tr>");
                    }

                    //Overall result for each indicator
                    strBody.Append(string.Format("<tr><td></td><td align=\"right\"><b>สรุปความคิดเห็นรวม</b></td>"));
                    Self_evaluation_tiny_detail overallforcurrindicator = reportobject.indicator_self_evaluation_list.First(t => t.indicator_num == i.indicator_num).self_evaluation_list.First(u => u.sub_indicator_num == 0);

                    //Add overall score for all indicator's sum if evaluation_score is not 0 
                    if (overallforcurrindicator.evaluation_score != 0) {
                        overallscoresum += overallforcurrindicator.evaluation_score;
                        overalldivisor++;
                    }

                    for (int overallscore = 1; overallscore <= 7; overallscore++)
                    {
                        if (overallscore == overallforcurrindicator.evaluation_score)
                        {
                            strBody.Append(string.Format("<td align=\"center\">&#x2713</td>"));
                        }
                        else
                        {
                            strBody.Append(string.Format("<td></td>"));
                        }
                    }
                    strBody.Append("</tr>");
                }

                strBody.Append("<tr><td></td><td align=\"center\"><b>สรุปผลการพิจารณาโดยรวมทั้งหมด</b></td><td colspan=\"7\" align=\"center\">");
                if(overalldivisor != 0)
                {
                    strBody.Append(string.Format("{0:N1}",(overallscoresum * 1.0 / overalldivisor)));
                }
                else
                {
                    strBody.Append("--ไม่พบข้อมูล--");
                }
                strBody.Append("</td></tr></table>");
                //END BODY SECTION
                strBody.Append("</div></body></html>");

                /*Force this content to be downloaded as a Word document*/

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

            }
            else
                return InternalServerError(new Exception(res.ToString()));
        }

        [ActionName("genaunsar")]
        public async Task<IHttpActionResult> PostForGenAunSAR(oCurriculum_academic data)
        {
            if (data == null)
                return BadRequest("กรุณาระบุหลักสูตรและปีการศึกษาที่ต้องการดาวน์โหลดร่างรายงาน");

            datacontext.curri_id = data.curri_id;
            datacontext.aca_year = data.aca_year;
            object res = await datacontext.getSectionSaveDataForSAR();
            if (res.GetType().ToString() != "System.String")
            {
                //Start to generate SAR doc
                SAR reportobject = (SAR)res;

                var strBody = new System.Text.StringBuilder("");

                strBody.Append("<html " +
                "xmlns:o=\"urn:schemas-microsoft-com:office:office\" " +
                "xmlns:w=\"urn:schemas-microsoft-com:office:word\" " +
                "xmlns=\"http://www.w3.org/TR/REC-html40\">" +
                "<head><title></title>\n");
                //strBody.Append("<meta name=ProgId content=Word.Document>");

                strBody.Append(
                   "<!--[if gte mso 9]>\n" +
                   "<xml>\n" +
                   "<w:WordDocument>\n" +
                   "<w:View>Print</w:View>\n" +
                   "<w:Zoom>90</w:Zoom>\n" +
                   "<w:DoNotOptimizeForBrowser/>\n" +
                   "</w:WordDocument>\n" +
                   "</xml>\n" +
                   "<!--[endif]>\n\n"
                   );


                strBody.Append("<style>" +
                                        "<!-- /* Style Definitions */" +
                                        "@page Section1" +
                                        "   {size: 21cm 29.7cm; " +
                                        "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                                        "   mso-header-margin:.5in; " +
                                        "   mso-page-orientation: portrait; " +
                                        "   mso-footer-margin:.5in; mso-paper-source:0; " +
                                        "   mso-footer: f1; } " +
                                        " div.Section1" +
                                        "   {page:Section1;}" +
                                        "-->" +
                                        "table{" +
                                        "font-family:'Th Sarabun New';font-size:16pt; " +
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
                                        "p { margin:0 } \n " +

                                        "table.evidence,table.selfevalres {" +
                                        "border:1px solid black; " +
                                        "border-collapse:collapse; " +
                                        "} " +

                                        "table.evidence th,table.evidence td, " +
                                        "table.selfevalres th,table.selfevalres td { border:1px solid black; } " +

                                        "ol.sar-ol li { margin:0 auto 0 auto } " +
                "</style></head>");

                strBody.Append("<body style=\"tab-interval:.5in;font-family:'Th Sarabun New';font-size:16pt\">" +
                                        "<div class=Section1>");
                //BODY SECTION => read SAR object to gather data


                foreach (Indicator_with_section_save_list i in reportobject.indicator_section_save_list)
                {
                    strBody.Append(string.Format("<b>AUN.{0} {1} </b><br>", i.indicator_num, i.indicator_name));

                    foreach (Section_save_with_sub_indicator_detail s in i.section_save_list)
                    {
                        strBody.Append(string.Format("<b>{0}.{1} {2}</b><br>", i.indicator_num, s.sub_indicator_num, s.sub_indicator_name));

                        strBody.Append(string.Format(s.detail + "<br><br>"));
                    }

                    strBody.Append("<b>รายการเอกสารหลักฐาน</b><br>");
                    if (i.evidence_list.Count == 0)
                    {
                        strBody.Append("--ไม่พบข้อมูล--<br><br clear=all style='mso-special-character:line-break;page-break-before:always'>");
                    }
                    else
                    {
                        strBody.Append("<table class=\"evidence\"><tr><th width=100>รหัสเอกสาร</th><th>รายการ</th></tr>");
                        foreach (Evidence_detail_for_SAR e in i.evidence_list)
                        {
                            strBody.Append(string.Format("<tr><td align=\"center\">{0}-{1}</td><td>{2}</td></tr>", e.indicator_num, e.evidence_real_code, e.evidence_name));
                        }
                        strBody.Append("</table><br><br clear=all style='mso-special-character:line-break;page-break-before:always'>");
                    }

                }

                strBody.Append("<b>วิเคราะห์จุดแข็งและจุดอ่อน</b><br><br>");
                foreach (Indicator_with_section_save_list i in reportobject.indicator_section_save_list)
                {
                    strBody.Append(string.Format("<b>AUN.{0} {1} </b><br>", i.indicator_num, i.indicator_name));

                    strBody.Append(string.Format("<b>จุดแข็ง</b><br>"));
                    string strtoinsert = "";

                    //INSERT STRENGTH
                    foreach (Section_save_with_sub_indicator_detail s in i.section_save_list)
                    {
                        if (s.strength != "--ไม่พบข้อมูล--")
                            strtoinsert += string.Format("<li>{0}</li>", s.strength);
                    }
                    if (strtoinsert != "")
                    {
                        strBody.Append(string.Format("<ol class=\"sar-ol\">{0}</ol>", strtoinsert));
                    }
                    else
                    {
                        strBody.Append("--ไม่พบข้อมูล--<br>");
                    }


                    strtoinsert = "";

                    strBody.Append(string.Format("<b>จุดอ่อน</b><br>"));
                    //INSERT WEAKNESS
                    foreach (Section_save_with_sub_indicator_detail s in i.section_save_list)
                    {
                        if (s.weakness != "--ไม่พบข้อมูล--")
                            strtoinsert += string.Format("<li>{0}</li>", s.weakness);
                    }
                    if (strtoinsert != "")
                    {
                        strBody.Append(string.Format("<ol class=\"sar-ol\">{0}</ol>", strtoinsert));
                    }
                    else
                    {
                        strBody.Append("--ไม่พบข้อมูล--<br>");
                    }


                    strtoinsert = "";

                    strBody.Append(string.Format("<b>จุดที่ควรพัฒนา</b><br>"));
                    //INSERT AREA OF IMPROVEMENT
                    foreach (Section_save_with_sub_indicator_detail s in i.section_save_list)
                    {
                        if (s.improve != "--ไม่พบข้อมูล--")
                            strtoinsert += string.Format("<li>{0}</li>", s.improve);
                    }
                    if (strtoinsert != "")
                    {
                        strBody.Append(string.Format("<ol class=\"sar-ol\">{0}</ol>", strtoinsert));
                    }
                    else
                    {
                        strBody.Append("--ไม่พบข้อมูล--<br>");
                    }
                    if (i != reportobject.indicator_section_save_list.Last())
                        strBody.Append("<br>");
                    else
                        strBody.Append("<br clear=all style='mso-special-character:line-break;page-break-before:always'>");
                }

                int overallscoresum = 0;
                int overalldivisor = 0;
                strBody.Append("<b>สรุปผลการประเมินตนเอง</b><br>");
                strBody.Append("<table class=\"selfevalres\">");

                foreach (Indicator_with_section_save_list i in reportobject.indicator_section_save_list)
                {
                    //Header row for each indicator
                    strBody.Append(string.Format("<tr><th>{0}</th> <td><b>{1}</b></td> <th style=\"width:0.85cm\">1</th><th style=\"width:0.85cm\">2</th><th style=\"width:0.85cm\">3</th><th style=\"width:0.85cm\">4</th><th style=\"width:0.85cm\">5</th><th style=\"width:0.85cm\">6</th><th style=\"width:0.85cm\">7</th></tr>", i.indicator_num, i.indicator_name));

                    foreach (Section_save_with_sub_indicator_detail s in i.section_save_list)
                    {
                        Self_evaluation_tiny_detail target = reportobject.indicator_self_evaluation_list.First(t => t.indicator_num == i.indicator_num).self_evaluation_list.First(u => u.sub_indicator_num == s.sub_indicator_num);
                        strBody.Append(string.Format("<tr><td align=\"center\">{0}.{1}</td> <td>{2}</td>", i.indicator_num, s.sub_indicator_num, s.sub_indicator_name));
                        for (int score = 1; score <= 7; score++)
                        {
                            if (score == target.evaluation_score)
                            {
                                strBody.Append(string.Format("<td align=\"center\">&#x2713;</td>"));
                            }
                            else
                            {
                                strBody.Append(string.Format("<td></td>"));
                            }
                        }
                        strBody.Append("</tr>");
                    }

                    //Overall result for each indicator
                    strBody.Append(string.Format("<tr><td></td><td align=\"right\"><b>สรุปความคิดเห็นรวม</b></td>"));
                    Self_evaluation_tiny_detail overallforcurrindicator = reportobject.indicator_self_evaluation_list.First(t => t.indicator_num == i.indicator_num).self_evaluation_list.First(u => u.sub_indicator_num == 0);

                    //Add overall score for all indicator's sum if evaluation_score is not 0 
                    if (overallforcurrindicator.evaluation_score != 0)
                    {
                        overallscoresum += overallforcurrindicator.evaluation_score;
                        overalldivisor++;
                    }

                    for (int overallscore = 1; overallscore <= 7; overallscore++)
                    {
                        if (overallscore == overallforcurrindicator.evaluation_score)
                        {
                            strBody.Append(string.Format("<td align=\"center\">&#x2713</td>"));
                        }
                        else
                        {
                            strBody.Append(string.Format("<td></td>"));
                        }
                    }
                    strBody.Append("</tr>");
                }

                strBody.Append("<tr><td></td><td align=\"center\"><b>สรุปผลการพิจารณาโดยรวมทั้งหมด</b></td><td colspan=\"7\" align=\"center\">");
                if (overalldivisor != 0)
                {
                    strBody.Append(string.Format("{0:N1}", (overallscoresum * 1.0 / overalldivisor)));
                }
                else
                {
                    strBody.Append("--ไม่พบข้อมูล--");
                }
                strBody.Append("</td></tr></table>");
                //END BODY SECTION
                strBody.Append("</div></body></html>");

                /*Force this content to be downloaded as a Word document*/

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

            }
            else
                return InternalServerError(new Exception(res.ToString()));
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
