using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Committee_with_detail : Committee
    {
        private string _t_name;
        private string _file_name_pic;
        private string _email;
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public string email { get { return _email; } set { _email = value; } }
        public string file_name_pic { get { return _file_name_pic; } set { _file_name_pic = value; } }
    }
}