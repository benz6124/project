using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Curriculum_teacher_staff_with_brief_detail : Curriculum_teacher_staff
    {
        private string _t_name;
        private string _type;
        private string _file_name_pic;
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public string type { get { return _type; } set { _type = value; } }

        public string file_name_pic { get { return _file_name_pic; } set { _file_name_pic = value; } }
    }
}