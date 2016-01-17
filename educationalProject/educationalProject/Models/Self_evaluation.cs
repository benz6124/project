using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Self_evaluation
    {
        internal struct FieldName
        {
            public static readonly string INDICATOR_NUM = "INDICATOR_NUM";
            public static readonly string SUB_INDICATOR_NUM = "SUB_INDICATOR_NUM";
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string EVALUATION_SCORE = "EVALUATION_SCORE";
            public static readonly string DATE = "DATE";
            public static readonly string TIME = "TIME";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string TABLE_NAME = "SELF_EVALUATION";
        }
        private int _indicator_num;
        private int _sub_indicator_num;
        private string _teacher_id;
        private int _evaluation_score;
        private DateTime _date;
        private TimeSpan _time;
        private string _curri_id;
        private int _aca_year;
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public int sub_indicator_num { get { return _sub_indicator_num; } set { _sub_indicator_num = value; } }
        public string teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public int evaluation_score { get { return _evaluation_score; } set { _evaluation_score = value; } }
        public virtual DateTime date { get { return _date; } set { _date = value; } }
        public TimeSpan time { get { return _time; } set { _time = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
    }
}