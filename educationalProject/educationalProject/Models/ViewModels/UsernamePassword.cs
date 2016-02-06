using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
namespace educationalProject.Models.ViewModels
{
    public class UsernamePassword
    {
        private static volatile PasswordHasher hasher = new PasswordHasher();
        private string _username;
        private string _password;
        public string username { get { return _username; } set { _username = value; } }
        public string password { get { return _password; } set { _password = value; } }
        public UsernamePassword(string username,string password)
        {
            _username = username;
            _password = hasher.HashPassword(password);
        }
        public bool isMatchPassword(string providedpassword)
        {
            PasswordVerificationResult res = hasher.VerifyHashedPassword(password, providedpassword);
            return  res == PasswordVerificationResult.Success || res == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}