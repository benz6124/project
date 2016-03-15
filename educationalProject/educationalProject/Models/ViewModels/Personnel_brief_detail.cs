using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Personnel_brief_detail
    {
        private string _tname;
        private string _pic;
        private string _email;
        private string _user_id;
        public string tname { get { return _tname; } set { _tname = value; } }
        public string pic { get { return _pic; } set { _pic = value; } }
        public string email { get { return _email; } set { _email = value; } }
        public string user_id { get { return _user_id; } set { _user_id = value; } }
    }
}