using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class SAR
    {
        private List<Indicator_with_section_save_list> _indicator_section_save_list;
        private List<Indicator_with_self_evaluation_tiny_obj_list> _indicator_self_evaluation_list;
        public List<Indicator_with_section_save_list> indicator_section_save_list { get { return _indicator_section_save_list; } set { _indicator_section_save_list = value; } }
        public List<Indicator_with_self_evaluation_tiny_obj_list> indicator_self_evaluation_list { get { return _indicator_self_evaluation_list; } set { _indicator_self_evaluation_list = value; } }
        public SAR()
        {
            indicator_section_save_list = new List<Indicator_with_section_save_list>();
            indicator_self_evaluation_list = new List<Indicator_with_self_evaluation_tiny_obj_list>();
        }
    }
}