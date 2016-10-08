using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Extra_privilege_by_type_with_privilege_choices
    {
        private int _title_code;
        private string _name;
        private string _curri_id;
        private List<User_type_privilege> _privilege_list;
        private List<Privilege_choice> _choices;

        public List<User_type_privilege> privilege_list { get { return _privilege_list; } set { _privilege_list = value; } }
        public List<Privilege_choice> choices { get { return _choices; } set { _choices = value; } }
        public int title_code { get { return _title_code; } set { _title_code = value; } }
        public string name { get { return _name; } set { _name = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public Extra_privilege_by_type_with_privilege_choices()
        {
            privilege_list = new List<User_type_privilege>();
            choices = new List<Privilege_choice>();
        }
    }
}