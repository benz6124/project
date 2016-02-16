using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Questionare_set_detail : Questionare_set
    {
        private List<string> _target;
        private string _t_name;
        public List<string> target { get { return _target; } set { _target = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }

        public Questionare_set_detail()
        {
            target = new List<string>();
        }
    }
}