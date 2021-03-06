﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using educationalProject.Models.Wrappers;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Controllers
{
    public class AdminController : ApiController
    {
        private oAdmin datacontext = new oAdmin();
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await datacontext.Select());
        }

        public async Task<IHttpActionResult> PostForCreateNewAdmin(JObject data)
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
            string emaillower = data["email"].ToString();
            emaillower = emaillower.ToLower();
            UsernamePassword u =  new UsernamePassword(emaillower, password);

            datacontext.admin_creator_id = Convert.ToInt32(data["user_id"]);
            datacontext.username = emaillower;
            datacontext.password = u.password;
            datacontext.t_name = data["t_name"].ToString();

            object result = await datacontext.InsertWithSelect();
            if (result.GetType().ToString() != "System.String")
            {
                object sendresult = await MailingUtils.sendUsernamePasswordMailForAdmin(emaillower, password, datacontext.t_name);
                if (sendresult == null)
                    return Ok(result);
                else
                    return InternalServerError((Exception)sendresult);
            }
            else
                return BadRequest(result.ToString());
        }
    }
}
