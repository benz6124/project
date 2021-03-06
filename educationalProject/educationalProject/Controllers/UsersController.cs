﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using educationalProject.Models.ViewModels;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels.Wrappers;
using educationalProject.Utils;
namespace educationalProject.Controllers
{
    public class UsersController : ApiController
    {
        private oTeacher teachercontext = new oTeacher();
        private oStaff staffcontext = new oStaff();
        private oStudent studentcontext = new oStudent();
        private oAlumni alumnicontext = new oAlumni();
        private oAssessor assessorcontext = new oAssessor();
        private oCompany companycontext = new oCompany();
        private oUsers userscontext = new oUsers();

        [ActionName("getuserlist")]
        public async Task<IHttpActionResult> PostForGetuserlist([FromBody]string curri_id)
        {
            object result;
            if (curri_id == "0")
                result = await userscontext.SelectAllUsersByBrief();
            else
                result = await userscontext.SelectAllUsersByBriefFilterCurri(curri_id);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            return InternalServerError(new Exception(result.ToString()));
        }
        [ActionName("getuserdataforedit")]
        public async Task<IHttpActionResult> PostForGetUserDataEdit([FromBody]int user_id)
        {
            object result = await userscontext.selectUserDataForEdit(user_id);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }


        [ActionName("edituserdatadirect")]
        public async Task<IHttpActionResult> PutForUpdateUserDataDirect()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            //savepath will determine later
            string savepath = WebApiApplication.SERVERPATH + "temp";
            var result = new MultipartFormDataStreamProvider(savepath);

            try
            {
                await Request.Content.ReadAsMultipartAsync(result);
                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                dynamic data = new ExpandoObject();
                //Prerequisite
                data.user_id = Convert.ToInt32(datareceive["user_id"]);

                //user type ignore
                //result.user_type

                //file_name_pic will change later???
                //result.main_info.file_name_pic

                /*Current editable data*/
                data.main_info = new ExpandoObject();
                data.main_info.t_prename = datareceive["main_info"]["t_prename"].ToString();
                data.main_info.t_name = datareceive["main_info"]["t_name"].ToString();
                data.main_info.e_prename = datareceive["main_info"]["e_prename"].ToString();
                data.main_info.e_name = datareceive["main_info"]["e_name"].ToString();
                data.main_info.email = datareceive["main_info"]["email"].ToString();
                data.main_info.tel = datareceive["main_info"]["tel"].ToString();
                data.main_info.addr = datareceive["main_info"]["addr"].ToString();

                //filenamepic will add later
                if (result.FileData.Count > 0)
                {

                }
                else
                {

                }
                object resultfromdb = await userscontext.UpdateUserDataDirect(data);
                if (resultfromdb == null)
                {/*
                    //delete filename will inside file_name property of oUser object
                    string delpath = WebApiApplication.SERVERPATH;
                    if (datacontext.file_name_pic != null)
                    {
                        //Check whether file exists!
                        if (File.Exists(string.Format("{0}{1}", delpath, datacontext.file_name_pic)))
                            File.Delete(string.Format("{0}{1}", delpath, datacontext.file_name_pic));
                    }
                 */
                    return Ok(resultfromdb);
                }
                else
                    return InternalServerError(new Exception(resultfromdb.ToString()));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }





        [ActionName("createnewusersbytyping")]
        public async Task<IHttpActionResult> PostForCreateNewUsersByTyping(JObject data)
        {
            List<UsernamePassword> userlist = new List<UsernamePassword>();
            List<UsernamePassword> nonencryptuserlist = new List<UsernamePassword>();
            List<string> curri_list = new List<string>();
                
                //1.READ JSON DATA PART (curri and type)
                if(data["curri"] != null)
                {
                    JArray jcurrilist = (JArray)data["curri"];
                    foreach(JValue value in jcurrilist)
                    {
                        curri_list.Add(value.ToString());
                    }
                }
                int select_user_type = Convert.ToInt32(data["type"]["user_type_id"]);


                //2.Read maillist
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RNGCryptoServiceProvider gen = new RNGCryptoServiceProvider();
                byte[] num = new byte[8];
                JArray emaillist = (JArray)data["new_emails"];
                foreach (JValue value in emaillist)
                {
                    //GENERATE PASSWORD 8 CHARS
                    string password = "";
                    gen.GetBytes(num);
                    for (int j = 0; j < 8; j++)
                    {
                        num[j] %= (byte)chars.Length;
                        password += chars[num[j]];
                    }
                    //=========================
                    string strlower = value.ToString().ToLower();

                    userlist.Add(new UsernamePassword(strlower, password));
                    nonencryptuserlist.Add(new UsernamePassword
                    {
                        username = strlower,
                        password = password
                    });
                }

                //3.Add user to database base on select_user_type
                object resultfromdb = null;
                if (select_user_type == 1)
                    resultfromdb = await teachercontext.Insert(userlist, curri_list);
                else if (select_user_type == 2)
                    resultfromdb = await staffcontext.Insert(userlist, curri_list);
                else if (select_user_type == 3)
                    resultfromdb = await studentcontext.Insert(userlist, curri_list);
                else if (select_user_type == 4)
                    resultfromdb = await alumnicontext.Insert(userlist, curri_list);
                else if (select_user_type == 5)
                    resultfromdb = await companycontext.Insert(userlist, curri_list);
                else if (select_user_type == 6)
                    resultfromdb = await assessorcontext.Insert(userlist, curri_list);
                else if (select_user_type != 0)
                    resultfromdb = await userscontext.InsertWithUserType(userlist, curri_list, select_user_type);
                else
                    return BadRequest("กรุณาเลือกประเภทผู้ใช้งาน");

            //4.If result is list,try to send mail (send only mail that not it resultfromdb)
            //Otherwise send errorresult back to user.
            if (resultfromdb == null)
                {
                    foreach (UsernamePassword item in nonencryptuserlist)
                    {
                        //send mail!
                        await MailingUtils.sendUsernamePasswordMail(item.username, item.password);
                    }
                    return Ok();
                }
                else if (resultfromdb.GetType().ToString() != "System.String")
                {
                    List<string> erroremail = (List<string>)resultfromdb;
                    if(userlist.Count == erroremail.Count)
                        return BadRequest("ทุกอีเมล์ที่ต้องการสร้างมีอยู่แล้วในระบบ");
                    foreach (UsernamePassword item in nonencryptuserlist)
                    {
                        //If current mail is not in erroremail => SEND!
                        if (erroremail.FirstOrDefault(t => t == item.username) == null)
                        {
                            //send mail!
                            await MailingUtils.sendUsernamePasswordMail(item.username, item.password);
                        }
                    }
                    return Ok(resultfromdb);
                }
                else
                    return InternalServerError(new Exception(resultfromdb.ToString()));
        }

        [ActionName("createnewusers")]
        public async Task<IHttpActionResult> PostForCreateNewUsersByFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            string savepath = WebApiApplication.SERVERPATH + "temp";
            var result = new MultipartFormDataStreamProvider(savepath);
            List<UsernamePassword> userlist = new List<UsernamePassword>();
            List<UsernamePassword> nonencryptuserlist = new List<UsernamePassword>();
            List<string> curri_list = new List<string>();
            try
            {
                await Request.Content.ReadAsMultipartAsync(result);

                //1.READ JSON DATA PART (curri and type)
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                if (datareceive["curri"] != null)
                {
                    JArray jcurrilist = (JArray)datareceive["curri"];
                    foreach (JValue value in jcurrilist)
                    {
                        curri_list.Add(value.ToString());
                    }
                }
                int select_user_type = Convert.ToInt32(datareceive["type"]["user_type_id"]);


                //2.Read maillist file

                MultipartFileData file = result.FileData[0];
                if (file.Headers.ContentType.ToString() != "text/plain")
                    return BadRequest("ไฟล์ที่อัพโหลดไม่เป็นไฟล์ข้อความธรรมดา");

                FileInfo fileInfo = new FileInfo(file.LocalFileName);

                const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RNGCryptoServiceProvider gen = new RNGCryptoServiceProvider();
                byte[] num = new byte[8];
                string text = File.ReadAllText(string.Format("{0}/{1}", savepath, fileInfo.Name));
                string[] emaillist = text.Split(' ', '\r', '\t', '\n', '\v', '\f').Where(t => t != "").ToArray();
                foreach (string str in emaillist)
                {
                    //GENERATE PASSWORD 8 CHARS
                    string password = "";
                    gen.GetBytes(num);
                    for (int j = 0; j < 8; j++)
                    {
                        num[j] %= (byte)chars.Length;
                        password += chars[num[j]];
                    }
                    //=========================
                    string strlower = str.ToLower();

                    userlist.Add(new UsernamePassword(strlower, password));
                    nonencryptuserlist.Add(new UsernamePassword
                    {
                        username = strlower,
                        password = password
                    });
                }

                //3.Delete temp email file
                //Check whether file exists!
                if (File.Exists(string.Format("{0}/{1}", savepath, fileInfo.Name)))
                    File.Delete(string.Format("{0}/{1}", savepath, fileInfo.Name));

                //4.Add user to database base on select_user_type
                object resultfromdb = null;
                if (select_user_type == 1)
                    resultfromdb = await teachercontext.Insert(userlist, curri_list);
                else if (select_user_type == 2)
                    resultfromdb = await staffcontext.Insert(userlist, curri_list);
                else if (select_user_type == 3)
                    resultfromdb = await studentcontext.Insert(userlist, curri_list);
                else if (select_user_type == 4)
                    resultfromdb = await alumnicontext.Insert(userlist, curri_list);
                else if (select_user_type == 5)
                    resultfromdb = await companycontext.Insert(userlist, curri_list);
                else if (select_user_type == 6)
                    resultfromdb = await assessorcontext.Insert(userlist, curri_list);
                else if (select_user_type != 0)
                    resultfromdb = await userscontext.InsertWithUserType(userlist, curri_list, select_user_type);
                else
                    return BadRequest("กรุณาเลือกประเภทผู้ใช้งาน");

                //5.If result is list,try to send mail (send only mail that not it resultfromdb)
                //Otherwise send errorresult back to user.
                if (resultfromdb == null)
                {
                    foreach (UsernamePassword item in nonencryptuserlist)
                    {
                        //send mail!
                        await MailingUtils.sendUsernamePasswordMail(item.username, item.password);
                    }
                    return Ok();
                }
                else if (resultfromdb.GetType().ToString() != "System.String")
                {
                    List<string> erroremail = (List<string>)resultfromdb;
                    if (userlist.Count == erroremail.Count)
                        return BadRequest("ทุกอีเมล์ในไฟล์ดังกล่าวมีอยู่แล้วในระบบ");
                    foreach (UsernamePassword item in nonencryptuserlist)
                    {
                        //If current mail is not in erroremail => SEND!
                        if (erroremail.FirstOrDefault(t => t == item.username) == null)
                        {
                            //send mail!
                            await MailingUtils.sendUsernamePasswordMail(item.username, item.password);
                        }
                    }
                    return Ok(resultfromdb);
                }
                else
                    return InternalServerError(new Exception(resultfromdb.ToString()));

            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }


        [ActionName("login")]
        public async Task<IHttpActionResult> PostForLogin(JObject usrpwdata)
        {
            List<System.Net.Http.Headers.CookieHeaderValue> x = Request.Headers.GetCookies("mymy").ToList();
            if (x.Count == 1)
            {
                //If login cookie exists:Return error to indicate that user already logged in
                return BadRequest("ท่านได้เข้าสู่ระบบอยู่แล้ว");
            }
            UsernamePassword data = new UsernamePassword();
            data.username = usrpwdata["username"].ToString();
            data.password = usrpwdata["password"].ToString();

            if (data.username == "" && data.password == "")
                return BadRequest("กรุณาใส่ชื่อผู้ใช้และรหัสผ่านที่ต้องการเข้าสู่ระบบ");
            else if (data.username == "")
                return BadRequest("กรุณาใส่ชื่อผู้ใช้งานที่ต้องการเข้าสู่ระบบ");
            else if (data.password == "")
                return BadRequest("กรุณาใส่รหัสผ่านที่ใช้ในการเข้าสู่ระบบ");
            oUsers context = new oUsers();
            data.username = data.username.ToLower();
            object result = await context.SelectUser(data.username);

            //Check whether login is success?
            if (result.GetType().ToString() != "System.String")
            {
                User_information_with_privilege_information u = (User_information_with_privilege_information)result;
                string oldpassword = data.password;
                data.password = u.information.GetPassword();
                if (data.isMatchPassword(oldpassword))
                {
                    return Ok(u);
                }
                else
                {
                    return BadRequest("ชื่อผู้ใช้งานหรือรหัสผ่านไม่ถูกต้อง");
                }
            }
            else
            {
                return BadRequest("ชื่อผู้ใช้งานหรือรหัสผ่านไม่ถูกต้อง");
            }
        }

        [ActionName("changeusername")]
        public async Task<IHttpActionResult> PutForChangeUsername(JObject userdata)
        {
            oUsers datacontext = new oUsers();
            string username = userdata["username"].ToString().ToLower();
            int user_id = Convert.ToInt32(userdata["user_id"]);

            object result = await datacontext.UpdateUsername(username, user_id);

            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("resetpwd")]
        public async Task<IHttpActionResult> PostForResetPassword([FromBody]int user_id)
        {
            oUsers datacontext = new oUsers();

            object result = await datacontext.ResetPassword(user_id);

            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("changepassword")]
        public IHttpActionResult PutForChangePassword(JObject userdata)
        {
            oUsers datacontext = new oUsers();
            string old_password = userdata["old_password"].ToString();
            string new_password = userdata["new_password"].ToString();
            int user_id = Convert.ToInt32(userdata["user_id"]);

            object result = datacontext.UpdatePassword(old_password, ref new_password, user_id);

            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }

        [ActionName("getuserdata")]
        public async Task<IHttpActionResult> PostForQueryUserData([FromBody]int user_id)
        {
            oUsers datacontext = new oUsers();
            object result = await datacontext.selectUserData(user_id);
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }
        [ActionName("edit")]
        public async Task<IHttpActionResult> PutForUpdateUserData()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            string savepath = WebApiApplication.SERVERPATH + "myImages/profile_pic";
            var result = new MultipartFormDataStreamProvider(savepath);

            try
            {
                await Request.Content.ReadAsMultipartAsync(result);
                oUsers datacontext = new oUsers();
                //READ JSON DATA PART
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                User_information_with_privilege_information userdata = new User_information_with_privilege_information();



                //Prerequisite
                userdata.user_id = Convert.ToInt32(datareceive["user_id"]);
                //username ignored
                //citizen_id ignored
                //gender ignored
                //timestamp ignored

                //teacher section => degree ignored
                //teacher section => position ignored
                //teacher section => personnel_type ignored
                //teacher section => person_id ignored
                //teacher,staff section => room ignored
                //teacher section => alive ignored

                userdata.user_type = datareceive["user_type"].ToString();

                //list of update value
                userdata.information.t_prename = datareceive["information"]["t_prename"].ToString();
                userdata.information.t_name = datareceive["information"]["t_name"].ToString();
                userdata.information.e_prename = datareceive["information"]["e_prename"].ToString();
                userdata.information.e_name = datareceive["information"]["e_name"].ToString();
                userdata.information.email = datareceive["information"]["email"].ToString();
                userdata.information.tel = datareceive["information"]["tel"].ToString();
                userdata.information.addr = datareceive["information"]["addr"].ToString();


                if(userdata.user_type == "อาจารย์")
                {
                    //teacher have status
                    userdata.information.status = datareceive["information"]["status"].ToString();
                    //teacher have interest
                    if(datareceive["information"]["interest"] != null)
                    {
                        JArray interestarr = (JArray)datareceive["information"]["interest"];
                        foreach (JValue value in interestarr)
                            userdata.information.interest.Add(value.ToString());
                    }
                }

                if(userdata.user_type != "นักศึกษา")
                {
                    if (datareceive["information"]["education"] != null)
                    {
                        JArray educationarr = (JArray)datareceive["information"]["education"];
                        foreach (JObject eduitem in educationarr)
                            userdata.information.education.Add(new Models.Educational_teacher_staff {
                                education_id = Convert.ToInt32(eduitem["education_id"])
                            });
                    }
                }
                //filenamepic will add later

                if (result.FileData.Count > 0)
                {
                    MultipartFileData file = result.FileData[0];
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    if (!file.Headers.ContentType.ToString().Contains("image/"))
                    {
                        //Delete temp upload file
                        if (File.Exists(string.Format("{0}/{1}", savepath, fileInfo.Name)))
                            File.Delete(string.Format("{0}/{1}", savepath, fileInfo.Name));
                        return BadRequest("ไฟล์รูปภาพที่ท่านอัพโหลดไมใช่ไฟล์รูปภาพที่ถูกต้อง");
                    }
                    string newfilename = string.Format("{0}.{1}", fileInfo.Name.Substring(9), file.Headers.ContentDisposition.FileName.Split('.').LastOrDefault().Split('\"').FirstOrDefault());
                    userdata.information.file_name_pic = "myImages/profile_pic/" + newfilename;
                    File.Move(string.Format("{0}/{1}", savepath, fileInfo.Name), string.Format("{0}/{1}", savepath, newfilename));
                }
                else
                {
                    //file_name_pic set to null => no change!
                    userdata.information.file_name_pic = null;
                }

                object resultfromdb = await datacontext.UpdateUserData(userdata);

                if (resultfromdb.GetType().ToString() != "System.String")
                {
                    //delete filename will inside file_name property of oUser object
                    string delpath = WebApiApplication.SERVERPATH;
                    if (datacontext.file_name_pic != null)
                    {
                        //Check whether file exists!
                        if (File.Exists(string.Format("{0}{1}", delpath, datacontext.file_name_pic)))
                            File.Delete(string.Format("{0}{1}", delpath, datacontext.file_name_pic));
                    }
                    return Ok(resultfromdb);
                }
                else
                    return InternalServerError(new Exception(resultfromdb.ToString()));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}