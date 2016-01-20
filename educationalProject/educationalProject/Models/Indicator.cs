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
            public static readonly string INDICATOR_NAME_T = "INDICATOR_NAME_T";
            public static readonly string INDICATOR_NAME_E = "INDICATOR_NAME_E";
            public static readonly string TABLE_NAME = "INDICATOR";
        }

        private int _aca_year;
        private int _indicator_num;
        private string _indicator_name_t;
        private string _indicator_name_e;
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public string indicator_name_t { get { return _indicator_name_t; } set { _indicator_name_t = value; } }
        public string indicator_name_e { get { return _indicator_name_e; } set { _indicator_name_e = value; } }
    }
}