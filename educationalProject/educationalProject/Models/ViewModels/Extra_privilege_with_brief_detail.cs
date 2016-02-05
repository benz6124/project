using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Extra_privilege_with_brief_detail : Extra_privilege
    {
        private string _t_name;
        private string _file_name_pic;
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public string file_name_pic { get { return _file_name_pic; } set { _file_name_pic = value; } }
    }
}