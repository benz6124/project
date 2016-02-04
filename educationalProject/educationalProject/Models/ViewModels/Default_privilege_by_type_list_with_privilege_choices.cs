using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Default_privilege_by_type_list_with_privilege_choices
    {
        private List<Default_privilege_by_type> _list;
        private List<string> _choices;
        public List<Default_privilege_by_type> list { get { return _list; } set { _list = value; } }
        public List<string> choices { get { return _choices; } set { _choices = value; } }
        public Default_privilege_by_type_list_with_privilege_choices()
        {
            list = new List<Default_privilege_by_type>();
            choices = new List<string>();
        }
    }
}