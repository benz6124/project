using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Indicator_with_section_save_list
    {
        private int _indicator_num;
        private string _indicator_name;
        private List<Section_save_with_sub_indicator_detail> _section_save_list;
        private List<Evidence_detail_for_SAR> _evidence_list;
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public string indicator_name { get { return _indicator_name; } set { _indicator_name = value; }  }
        public List<Section_save_with_sub_indicator_detail> section_save_list { get { return _section_save_list; } set { _section_save_list = value; } }
        public List<Evidence_detail_for_SAR> evidence_list { get { return _evidence_list; } set { _evidence_list = value; } }
        public Indicator_with_section_save_list()
        {
            section_save_list = new List<Section_save_with_sub_indicator_detail>();
            evidence_list = new List<Evidence_detail_for_SAR>();
        }
    }
}