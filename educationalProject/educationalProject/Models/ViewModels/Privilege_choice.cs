using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Privilege_choice
    {
        private int _title_privilege_code;
        private string _privilege;
        public int title_privilege_code { get { return _title_privilege_code; } set { _title_privilege_code = value; } }
        public string privilege { get { return _privilege; } set { _privilege = value; } }
    }
}