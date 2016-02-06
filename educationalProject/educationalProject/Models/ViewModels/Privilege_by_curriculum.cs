using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Privilege_by_curriculum
    {
        private string _curri_id;
        private List<Title_privilege> _privilege_list;
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public List<Title_privilege> privilege_list { get { return _privilege_list; } set { _privilege_list = value; } }
        public Privilege_by_curriculum()
        {
            _privilege_list = new List<Title_privilege>();
        }
    }
}