using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
namespace educationalProject.Controllers
{
    public class AdminController : ApiController
    {
        private oAdmin datacontext = new oAdmin();
        public IHttpActionResult Get()
        {
            return Ok(datacontext.Select());
        }

        public IHttpActionResult PostForCreateNewAdmin(JObject data)
        {

            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            RNGCryptoServiceProvider gen = new RNGCryptoServiceProvider();
            byte[] num = new byte[8];

            //GENERATE PASSWORD 8 CHARS
            string password = "";
            gen.GetBytes(num);
            for (int j = 0; j < 8; j++)
            {
                num[j] %= (byte)chars.Length;
                password += chars[num[j]];
            }
            //=========================
            string emaillower = null;
            emaillower = emaillower.ToLower();
            UsernamePassword u =  new UsernamePassword(emaillower, password);

            datacontext.admin_creator_id = Convert.ToInt32(data["admin_creator_id"]);
            datacontext.username = emaillower;
            datacontext.password = u.password;
            datacontext.t_name = data["t_name"].ToString();

            object result = datacontext.InsertWithSelect();
            if (result.GetType().ToString() != "System.String")
                return Ok(result);
            else
                return InternalServerError(new Exception(result.ToString()));
        }
    }
}
