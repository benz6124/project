﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Others_evaluation
    {
        internal struct FieldName
        {
            public static readonly string OTHERS_EVALUATION_ID = "OTHERS_EVALUATION_ID";
            public static readonly string INDICATOR_NUM = "INDICATOR_NUM";
            public static readonly string SUB_INDICATOR_NUM = "SUB_INDICATOR_NUM";
            public static readonly string ASSESSOR_ID = "ASSESSOR_ID";
            public static readonly string EVALUATION_SCORE = "EVALUATION_SCORE";
            public static readonly string STRENGTH = "STRENGTH";
            public static readonly string IMPROVE = "IMPROVE";
            public static readonly string DATE = "DATE";
            public static readonly string TIME = "TIME";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string TABLE_NAME = "OTHERS_EVALUATION";
        }
        private int _others_evaluation_id;
        private int _indicator_num;
        private int _sub_indicator_num;
        private int _assessor_id;
        private int _evaluation_score;
        private string _strength;
        private string _improve;
        private string _date;
        private string _time;
        private string _curri_id;
        private int _aca_year;
        public int others_evaluation_id { get { return _others_evaluation_id; } set { _others_evaluation_id = value; } }
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public int sub_indicator_num { get { return _sub_indicator_num; } set { _sub_indicator_num = value; } }
        public int assessor_id { get { return _assessor_id; } set { _assessor_id = value; } }
        public int evaluation_score { get { return _evaluation_score; } set { _evaluation_score = value; } }
        public string strength { get { return _strength; } set { _strength = value; } }
        public string improve { get { return _improve; } set { _improve = value; } }
        public string date { get { return _date; } set { _date = value; } }
        public string time { get { return _time; } set { _time = value; } }

        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
    }
}