using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class vOthers_evaluation_assessor : vOthers_evaluation
    {
        private string _t_name;
        public string t_name { get { return _t_name; } set { _t_name = value; } }
    }
}