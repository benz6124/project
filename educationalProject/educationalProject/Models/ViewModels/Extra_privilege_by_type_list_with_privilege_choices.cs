using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Extra_privilege_by_type_list_with_privilege_choices
    {
        private List<Extra_privilege_by_type> _list;
        private List<string> _choices;
        public  List<Extra_privilege_by_type> list { get { return _list; } set { _list = value; } }
        public List<string> choices { get { return _choices; } set { _choices = value; } }
        public Extra_privilege_by_type_list_with_privilege_choices()
        {
            list = new List<Extra_privilege_by_type>();
            choices = new List<string>();
        }
    }
}