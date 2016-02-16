﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Section_save
    {
        protected struct FieldName
        {
            public static readonly string INDICATOR_NUM = "INDICATOR_NUM";
            public static readonly string SUB_INDICATOR_NUM = "SUB_INDICATOR_NUM";
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string DETAIL = "DETAIL";
            public static readonly string DATE = "DATE";
            public static readonly string TIME = "TIME";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string TABLE_NAME = "SECTION_SAVE";
        }
        private int _indicator_num;
        private int _sub_indicator_num;
        private int _teacher_id;
        private string _detail;
        private string _date;
        private string _time;
        private string _curri_id;
        private int _aca_year;
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public int sub_indicator_num { get { return _sub_indicator_num; } set { _sub_indicator_num = value; } }
        public int teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public string detail { get { return _detail; } set { _detail = value; } }
        public string date { get { return _date; } set { _date = value; } }
        public string time { get { return _time; } set { _time = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
    }
}