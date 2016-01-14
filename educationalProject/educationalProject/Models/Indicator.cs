using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Indicator
    {
        protected struct FieldName
        {
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string INDICATOR_NUM = "INDICATOR_NUM";
            public static readonly string INDICATOR_NAME = "INDICATOR_NAME";
            public static readonly string TABLE_NAME = "INDICATOR";
        }

        private int _aca_year;
        private int _indicator_num;
        private string _indicator_name;
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public string indicator_name { get { return _indicator_name; } set { _indicator_name = value; } }
    }
}