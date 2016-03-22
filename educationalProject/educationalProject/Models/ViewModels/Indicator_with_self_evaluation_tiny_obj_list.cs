using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Indicator_with_self_evaluation_tiny_obj_list
    {
        private int _indicator_num;
        private List<Self_evaluation_tiny_detail> _self_evaluation_list;
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public List<Self_evaluation_tiny_detail> self_evaluation_list { get { return _self_evaluation_list; } set { _self_evaluation_list = value; } }
        public Indicator_with_self_evaluation_tiny_obj_list()
        {
            self_evaluation_list = new List<Self_evaluation_tiny_detail>();
        }
    }
}