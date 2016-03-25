using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
namespace educationalProject.Utils
{
    public class MailingUtils
    {
        public static async Task<object> sendUsernamePasswordMail(string username,string password)
        {
            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(username));

                // From
                mailMsg.From = new MailAddress(smtpSection.Network.UserName, "Educationalproject");

                mailMsg.Subject = "Educational Project - ชื่อผู้ใช้งานและรหัสผ่านสำหรับการเข้าสู่ระบบ";
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = "<h4>ขอแจ้งรายละเอียด username และ password ที่ใช้ในการเข้าสู่เว็บแอพพลิเคชันของเราดังนี้</h4> <br>" +
                               "<b>ชื่อผู้ใช้งาน</b>:"+username+"<br> " +
                               "<b>รหัสผ่าน</b>:"+password+"<br><br><br>" +
                               "หากอีเมล์ของท่านถูกแอบอ้างสร้างบัญชี หรือ ไม่ได้จงใจที่จะใช้งานเว็บแอพพลิเคชันของเราจริงๆ ท่านสามารถข้ามอีเมล์นี้ หรือ <b>ลบทิ้ง</b>ได้ทันที";
                mailMsg.Priority = MailPriority.High;

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient(smtpSection.Network.Host, 587);
                smtpClient.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                smtpClient.Credentials = credentials;

                await smtpClient.SendMailAsync(mailMsg);
                smtpClient.Dispose();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        public static async Task<object> sendUsernamePasswordMailForAdmin(string username, string password,string t_name)
        {
            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress(username));

                // From
                mailMsg.From = new MailAddress(smtpSection.Network.UserName, "Educationalproject");

                mailMsg.Subject = "Educational Project - ชื่อผู้ใช้งานและรหัสผ่านสำหรับการเข้าสู่ระบบ";
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = "<h4>ขอแจ้งรายละเอียด username และ password ที่ใช้ในการเข้าสู่เว็บแอพพลิเคชันของเราดังนี้</h4> <br>" +
                               "<b>ชื่อผู้ใช้งาน</b>:" + username + "<br> " +
                               "<b>รหัสผ่าน</b>:" + password + "<br>" +
                               "<b>ชื่อ-นามสกุล</b>:" + t_name +"<br><br><br>" +
                               "หากอีเมล์ของท่านถูกแอบอ้างสร้างบัญชี หรือ ไม่ได้จงใจที่จะใช้งานเว็บแอพพลิเคชันของเราจริงๆ ท่านสามารถข้ามอีเมล์นี้ หรือ <b>ลบทิ้ง</b>ได้ทันที";
                mailMsg.Priority = MailPriority.High;

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient(smtpSection.Network.Host, 587);
                smtpClient.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                smtpClient.Credentials = credentials;

                await smtpClient.SendMailAsync(mailMsg);
                smtpClient.Dispose();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static async Task<object> sendNotificationPrimaryEvidenceIndividual(Evidence_with_teacher_curri_indicator_detail data)
        {
            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("benchbb01@hotmail.com"));

                // From
                mailMsg.From = new MailAddress(smtpSection.Network.UserName, "Educationalproject");

                mailMsg.Subject = string.Format("Educational Project - แจ้งเตือนหลักฐานค้างส่งของ {0}",data.t_name);
                mailMsg.IsBodyHtml = true;
                mailMsg.Body = string.Format("<h3>เรียน {0}</h3> <br>", data.t_name) +
                               "ท่านมีหลักฐานที่ค้างส่งอยู่ โดยหลักฐานที่จะให้ท่านรีบดำเนินการส่งโดยเร็วที่สุดคือ <br>" +
                               string.Format("<ul><li>{0} <b>หลักสูตร</b> {1} <b>ปีการศึกษา</b> {2}</li></ul><br>", data.evidence_name, data.curr_tname, data.aca_year) +
                               "ขอบคุณครับ/ค่ะ<br><br><br><br>" +

                               "หากอีเมล์ของท่านถูกแอบอ้างสร้างบัญชี หรือ ไม่ได้จงใจที่จะใช้งานเว็บแอพพลิเคชันของเราจริงๆ ท่านสามารถข้ามอีเมล์นี้ หรือ <b>ลบทิ้ง</b>ได้ทันที";
                mailMsg.Priority = MailPriority.High;
                
                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient(smtpSection.Network.Host, 587);
                smtpClient.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                smtpClient.Credentials = credentials;
                await smtpClient.SendMailAsync(mailMsg);
                smtpClient.Dispose();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        public static async Task<object> sendNotificationAllPendingPrimaryEvidence(Personnel_with_pending_primary_evidence p)
        {
            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("benchbb01@hotmail.com"));

                // From
                mailMsg.From = new MailAddress(smtpSection.Network.UserName, "Educationalproject");

                mailMsg.Subject = string.Format("Educational Project - แจ้งเตือนหลักฐานค้างส่งทั้งหมดของ {0}", p.t_name);
                mailMsg.IsBodyHtml = true;

                string pendingliststr = "";
                foreach(Evidence_brief_detail e in p.pendinglist)
                    pendingliststr += string.Format("<li>{0} <b>หลักสูตร</b> {1} <b>ปีการศึกษา</b> {2}</li>", e.evidence_name, e.curr_tname, e.aca_year);

                mailMsg.Body = string.Format("<h3>เรียน {0}</h3> <br>", p.t_name) +
                               "ท่านมีหลักฐานที่ค้างส่งอยู่ โดยหลักฐานที่ท่านค้างส่งทั้งหมดมีดังนี้ <br>" +
                               "<ul>" +
                               pendingliststr +
                               "</ul><br>" +
                               "ขอบคุณครับ/ค่ะ<br><br><br><br>" +
                               "หากอีเมล์ของท่านถูกแอบอ้างสร้างบัญชี หรือ ไม่ได้จงใจที่จะใช้งานเว็บแอพพลิเคชันของเราจริงๆ ท่านสามารถข้ามอีเมล์นี้ หรือ <b>ลบทิ้ง</b>ได้ทันที";
                mailMsg.Priority = MailPriority.High;

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient(smtpSection.Network.Host, 587);
                smtpClient.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                smtpClient.Credentials = credentials;
                await smtpClient.SendMailAsync(mailMsg);
                smtpClient.Dispose();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}