using System;
using System.Collections.Generic;
using System.Linq;
namespace educationalProject.Models.ViewModels
{
    public class Others_evaluation_sub_indicator_name : Others_evaluation, IComparable<Others_evaluation_sub_indicator_name>
    {
        private string _sub_indicator_name;
        private string _t_name;
        private int? _self_score;
        public string sub_indicator_name { get { return _sub_indicator_name; } set { _sub_indicator_name = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public int? self_score { get { return _self_score; } set { _self_score = value; } }
        int IComparable<Others_evaluation_sub_indicator_name>.CompareTo(Others_evaluation_sub_indicator_name obj2)
        {
            if (sub_indicator_num < obj2.sub_indicator_num)
                return -1;
            else if (sub_indicator_num == obj2.sub_indicator_num)
                return 0;
            else
                return 1;
        }
        public Others_evaluation_sub_indicator_name()
        {
            self_score = null;
        }
    }
}