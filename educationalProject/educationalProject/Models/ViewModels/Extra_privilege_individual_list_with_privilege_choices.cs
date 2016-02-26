using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Extra_privilege_individual_list_with_privilege_choices
    {
        private List<Extra_privilege_with_brief_detail> _list;
        private List<Title_privilege> _choices;
        public List<Extra_privilege_with_brief_detail> list { get { return _list; } set { _list = value; } }
        public List<Title_privilege> choices { get { return _choices; } set { _choices = value; } }
        public Extra_privilege_individual_list_with_privilege_choices()
        {
            list = new List<Extra_privilege_with_brief_detail>();
            choices = new List<Title_privilege>();
        }
    }
}