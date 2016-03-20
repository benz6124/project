using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class SAR
    {
        private List<Indicator_with_section_save_list> _indicator_list;
        public List<Indicator_with_section_save_list> indicator_list { get { return _indicator_list; } set { _indicator_list = value; } }
        public SAR()
        {
            indicator_list = new List<Indicator_with_section_save_list>();
        }
    }
}