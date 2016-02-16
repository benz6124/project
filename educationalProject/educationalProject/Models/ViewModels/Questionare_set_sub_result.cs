using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Questionare_set_sub_result
    {
        private List<Questionare_set_main_result> _main_result_list;
        private List<string> _suggestion;

        public List<string> suggestion { get { return _suggestion; } set { _suggestion = value; } }
        public List<Questionare_set_main_result> main_result_list { get { return _main_result_list; } set { _main_result_list = value; } }
        public Questionare_set_sub_result()
        {
            main_result_list = new List<Questionare_set_main_result>();
            suggestion = new List<string>();
        }
    }
}