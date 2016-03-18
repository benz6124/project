using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Evaluation_overall_result
    {
        private string _self_date;
        private string _other_date;
        private string _self_time;
        private string _other_time;
        private string _self_name;
        private string _other_name;
        private string _indicator_name;
        private int _indicator_num;
        private List<Sub_indicator_result> _sub_indicator_result;

        public string self_date { get { return _self_date; } set { _self_date = value; } }
        public string other_date { get { return _other_date; } set { _other_date = value; } }
        public string self_time { get { return _self_time; } set { _self_time = value; } }
        public string other_time { get { return _other_time; } set { _other_time = value; } }
        public string self_name { get { return _self_name; } set { _self_name = value; } }
        public string other_name { get { return _other_name; } set { _other_name = value; } }
        public string indicator_name { get { return _indicator_name; } set { _indicator_name = value; } }
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public List<Sub_indicator_result> sub_indicator_result { get { return _sub_indicator_result; } set { _sub_indicator_result = value; } }
        public Evaluation_overall_result()
        {
            sub_indicator_result = new List<Sub_indicator_result>();
        }
    }
}