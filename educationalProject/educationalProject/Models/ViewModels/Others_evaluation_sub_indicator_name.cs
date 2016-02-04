using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Others_evaluation_sub_indicator_name : Others_evaluation
    {
        private string _sub_indicator_name;
        public string sub_indicator_name { get { return _sub_indicator_name; } set { _sub_indicator_name = value; } }
    }
}