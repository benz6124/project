using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    /*Use in manage user profiles*/
    public class User_brief_detail
    {
        private int _user_id;
        private string _username;
        private string _t_name;
        private string _file_name_pic;
        private string _user_type;
        public int user_id { get { return _user_id; } set { _user_id = value; } }
        public string username { get { return _username; } set { _username = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public string file_name_pic { get { return _file_name_pic; } set { _file_name_pic = value; } }
        public string user_type { get { return _user_type; } set { _user_type = value; } }

    }
}