using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Extra_privilege_with_brief_detail
    {
        private int _personnel_id;
        private string _t_name;
        private string _file_name_pic;
        private string _curri_id;
        private string _name;
        private Title_privilege _my_privilege;
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public string file_name_pic { get { return _file_name_pic; } set { _file_name_pic = value; } }
        public int personnel_id { get { return _personnel_id; } set { _personnel_id = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public string name { get { return _name; } set { _name = value; } }
        public Title_privilege my_privilege { get { return _my_privilege; } set { _my_privilege = value; } }
    }
}