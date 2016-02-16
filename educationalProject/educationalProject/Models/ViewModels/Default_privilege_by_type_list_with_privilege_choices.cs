using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Default_privilege_by_type_list_with_privilege_choices
    {
        private List<Default_privilege_by_type_with_name> _list;
        private List<Title_privilege> _choices;
        public List<Default_privilege_by_type_with_name> list { get { return _list; } set { _list = value; } }
        public List<Title_privilege> choices { get { return _choices; } set { _choices = value; } }
        public Default_privilege_by_type_list_with_privilege_choices()
        {
            list = new List<Default_privilege_by_type_with_name>();
            choices = new List<Title_privilege>();
        }
    }
}