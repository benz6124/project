using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Sub_indicator_result
    {
        private string _sub_indicator_name;
        private int _sub_indicator_num;
        private int? _sub_indicator_self_result;
        private int? _sub_indicator_other_result;
        public string sub_indicator_name { get { return _sub_indicator_name; } set { _sub_indicator_name = value; } }
        public int sub_indicator_num { get { return _sub_indicator_num; } set { _sub_indicator_num = value; } }
        public int? sub_indicator_self_result { get { return _sub_indicator_self_result; } set { _sub_indicator_self_result = value; } }
        public int? sub_indicator_other_result { get { return _sub_indicator_other_result; } set { _sub_indicator_other_result = value; } }
        public Sub_indicator_result()
        {
            sub_indicator_self_result = null;
            sub_indicator_other_result = null;
        }
    }
}