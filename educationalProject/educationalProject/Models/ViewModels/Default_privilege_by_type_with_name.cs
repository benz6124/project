using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Default_privilege_by_type_with_name : Default_privilege_by_type
    {
        private string _name;
        private string _privilege;
        public string name { get { return _name; } set { _name = value; } }
        public string privilege { get { return _privilege; } set { _privilege = value; } }
    }
}