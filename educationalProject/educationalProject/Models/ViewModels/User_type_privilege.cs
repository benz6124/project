using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class User_type_privilege
    {
        private int _user_type_id;
        private string _user_type;
        private Privilege_choice _privilege;

        public int user_type_id { get { return _user_type_id; } set { _user_type_id = value; } }
        public string user_type { get { return _user_type; } set { _user_type = value; } }
        public Privilege_choice privilege { get { return _privilege; } set { _privilege = value; } }
    }
}