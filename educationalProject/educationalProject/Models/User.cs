using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public abstract class User
    {
        private string _username;
        private string _password;
        private string _user_type;
        private string _t_prename;
        private string _t_name;
        private string _e_prename;
        private string _e_name;
        private string _citizen_id;
        private char _gender;
        private string _email;
        private string _tel;
        private string _addr;
        private string _file_name_pic;
        private string _timestamp;
        public string username { get { return _username; } set { _username = value; } }
        public string password { get { return _password; } set { _password = value; } }
        public string user_type { get { return _user_type; } set { _user_type = value; } }
        public string t_prename { get { return _t_prename; } set { _t_prename = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public string e_prename { get { return _e_prename; } set { _e_prename = value; } }
        public string e_name { get { return _e_name; } set { _e_name = value; } }
        public string citizen_id { get { return _citizen_id; } set { _citizen_id = value; } }
        public char gender { get { return _gender; } set { _gender = value; } }
        public string email { get { return _email; } set { _email = value; } }
        public string tel { get { return _tel; } set { _tel = value; } }
        public string addr { get { return _addr; } set { _addr = value; } }
        public string file_name_pic { get { return _file_name_pic; } set { _file_name_pic = value; } }
        public string timestamp { get { return _timestamp; } set { _timestamp = value; } }
    }
}