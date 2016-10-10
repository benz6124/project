using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Personnel_educational
    {
        private int _user_id;
        private string _user_type;
        private string _fullname;
        private string _email;
        private string _tel;
        private string _file_name_pic;
        private List<Educational_teacher_staff> _history;

        public int user_id { get { return _user_id; } set { _user_id = value; } }
        public string user_type { get { return _user_type; } set { _user_type = value; } }
        public string fullname { get { return _fullname; } set { _fullname = value; } }
        public string email { get { return _email; } set { _email = value; } }
        public string tel { get { return _tel; } set { _tel = value; } }
        public string file_name_pic { get { return _file_name_pic; } set { _file_name_pic = value; } }

        public List<Educational_teacher_staff> history { get { return _history; } set { _history = value; } }

        public Personnel_educational()
        {
            history = new List<Educational_teacher_staff>();
        }
    }
}