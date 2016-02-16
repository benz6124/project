using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Extra_privilege_by_type_with_name
    {
        private string _name;
        private string _user_type;
        private string _curri_id;
        private Title_privilege _my_privilege;
        public string name { get { return _name; } set { _name = value; } }
        public string user_type { get { return _user_type; } set { _user_type = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public Title_privilege my_privilege { get { return _my_privilege; } set { _my_privilege = value; } }
    }
}