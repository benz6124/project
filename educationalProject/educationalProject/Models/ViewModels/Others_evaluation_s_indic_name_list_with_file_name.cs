using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Others_evaluation_s_indic_name_list_with_file_name
    {
        private List<Others_evaluation_sub_indicator_name> _evaluation_detail;
        private string _file_name;
        public List<Others_evaluation_sub_indicator_name> evaluation_detail { get { return _evaluation_detail; } set { _evaluation_detail = value; } }
        public string file_name { get { return _file_name; } set { _file_name = value; } }

        public Others_evaluation_s_indic_name_list_with_file_name()
        {
            file_name = "";
            evaluation_detail = new List<Others_evaluation_sub_indicator_name>();
        }
    }
}