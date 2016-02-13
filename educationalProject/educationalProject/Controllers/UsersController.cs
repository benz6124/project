using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using educationalProject.Models.ViewModels;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels.Wrappers;
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
        [ActionName("createnewusers")]
        public async Task<IHttpActionResult> PostForCreateNewUsers()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new System.Web.Http.Results.StatusCodeResult(HttpStatusCode.UnsupportedMediaType, Request);
            }

            string savepath = WebApiApplication.SERVERPATH + "temp";
            var result = new MultipartFormDataStreamProvider(savepath);
            List<UsernamePassword> userlist = new List<UsernamePassword>();
            List<string> curri_list = new List<string>();
            try
            {
                await Request.Content.ReadAsMultipartAsync(result);
                
                //1.READ JSON DATA PART (curri and type)
                JObject datareceive = JObject.Parse(result.FormData.GetValues(result.FormData.AllKeys[0])[0]);
                if(datareceive["curri"] != null)
                {
                    JArray jcurrilist = (JArray)datareceive["curri"];
                    foreach(JValue value in jcurrilist)
                    {
                        curri_list.Add(value.ToString());
                    }
                }
                string select_user_type = datareceive["type"]["user_type"].ToString();


                //2.Read maillist file
                const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RNGCryptoServiceProvider gen = new RNGCryptoServiceProvider();
                byte[] num = new byte[8];

                MultipartFileData file = result.FileData[0];
                FileInfo fileInfo = new FileInfo(file.LocalFileName);

                string text = File.ReadAllText(string.Format("{0}/{1}", savepath, fileInfo.Name));
                string [] emaillist = text.Split(' ','\r','\t','\n','\v','\f').Where(t => t != "").ToArray();
                foreach(string str in emaillist)
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
                }

                //3.Delete temp email file
                //Check whether file exists!
                if (File.Exists(string.Format("{0}/{1}", savepath, fileInfo.Name)))
                    File.Delete(string.Format("{0}/{1}", savepath, fileInfo.Name));

                //4.Add user to database base on select_user_type
                object resultfromdb = null;
                if (select_user_type == "อาจารย์")
                    resultfromdb = teachercontext.Insert(userlist, curri_list);
                else if (select_user_type == "เจ้าหน้าที่")
                    resultfromdb = staffcontext.Insert(userlist, curri_list);
                else if (select_user_type == "นักศึกษา")
                    resultfromdb = studentcontext.Insert(userlist, curri_list);
                else if (select_user_type == "ศิษย์เก่า")
                    resultfromdb = alumnicontext.Insert(userlist, curri_list);
                else if (select_user_type == "บริษัท")
                    resultfromdb = companycontext.Insert(userlist, curri_list);
                else if (select_user_type == "ผู้ประเมินจากภายนอก")
                    resultfromdb = assessorcontext.Insert(userlist, curri_list);
                else
                    return BadRequest("กรุณาเลือกประเภทผู้ใช้งาน");

                //5.If result is list,try to send mail (send only mail that not it resultfromdb)
                //Otherwise send errorresult back to user.
                if (resultfromdb == null)
                    return Ok();
                else if (resultfromdb.GetType().ToString() != "System.String")
                {
                    List<string> erroremail = (List<string>)resultfromdb;
                    
                    foreach(UsernamePassword item in userlist)
                    {
                        //If current mail is not in erroremail => SEND!
                        if(erroremail.FirstOrDefault(t => t == item.username) == null)
                        {
                            //send mail!
                        }
                    }
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

        [ActionName("login")]
        public IHttpActionResult PostForLogin(JObject usrpwdata)
        {
            UsernamePassword data = new UsernamePassword();
            data.username = usrpwdata["username"].ToString();
            data.password = usrpwdata["password"].ToString();
            oUsers context = new oUsers();
            data.username = data.username.ToLower();
            object result = context.SelectUser(data.username);

            //Check whether preferred user is exists?
            if (result.GetType().ToString() != "System.String")
            {
                User_information_with_privilege_information u = (User_information_with_privilege_information)result;
                string oldpassword = data.password;
                data.password = u.information.GetPassword();
                if (data.isMatchPassword(oldpassword))
                {
                    if (u.user_type != "ผู้ดูแลระบบ")
                    {
                        //continue to retrieve privilege data (if user type is not admin) and return
                        result = context.SelectUserPrivilege(ref u);
                        if (result == null)
                            return Ok(u);
                        else
                            return InternalServerError(new Exception(result.ToString()));
                    }
                    //admin login!
                    return Ok(u);
                }
                else
                {
                    return BadRequest("ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง");
                }
            }
            else
            {
                return InternalServerError(new Exception(result.ToString()));
            }
        }

        [ActionName("changeusername")]
        public IHttpActionResult PutForChangeUsername(JObject userdata)
        {
            oUsers datacontext = new oUsers();
            string username = userdata["username"].ToString().ToLower();
            int user_id = Convert.ToInt32(userdata["user_id"]);

            object result = datacontext.UpdateUsername(username, user_id);

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

            object result = datacontext.UpdatePassword(old_password,ref new_password,user_id);

            if (result == null)
                return Ok();
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
