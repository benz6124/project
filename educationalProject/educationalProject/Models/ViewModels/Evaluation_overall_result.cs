using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Evaluation_overall_result
    {
        private List<vSelf_evaluation_teacher> _self;
        public List<vSelf_evaluation_teacher> self { get { return _self; } set { _self = value; } }

        private List<vOthers_evaluation_assessor> _others;
        public List<vOthers_evaluation_assessor> others { get { return _others; } set { _others = value; } }
        Evaluation_overall_result()
        {
            self = new List<vSelf_evaluation_teacher>();
            others = new List<vOthers_evaluation_assessor>();
        }
    }
}