using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class User_privilege
    {
        private int _user_id;
        private string _t_name;
        private string _file_name_pic;
        private Privilege_choice _privilege;

        public int user_id { get { return _user_id; } set { _user_id = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public string file_name_pic { get { return _file_name_pic; } set { _file_name_pic = value; } }
        public Privilege_choice privilege { get { return _privilege; } set { _privilege = value; } }
    }
}