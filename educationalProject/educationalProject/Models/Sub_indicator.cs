using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Sub_indicator
    {
        internal struct FieldName
        {
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string INDICATOR_NUM = "INDICATOR_NUM";
            public static readonly string SUB_INDICATOR_NUM = "SUB_INDICATOR_NUM";
            public static readonly string SUB_INDICATOR_NAME = "SUB_INDICATOR_NAME";
            public static readonly string TABLE_NAME = "SUB_INDICATOR";
        }
        private int _aca_year;
        private int _indicator_num;
        private int _sub_indicator_num;
        private string _sub_indicator_name;
        public int aca_year { get { return _aca_year; } set { _aca_year = value;} }
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public int sub_indicator_num { get { return _sub_indicator_num; } set { _sub_indicator_num = value; } }
        public string sub_indicator_name { get { return _sub_indicator_name; } set { _sub_indicator_name = value; } }
    
    }
}