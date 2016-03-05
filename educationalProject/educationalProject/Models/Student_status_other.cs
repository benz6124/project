using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Student_status_other
    {
        protected struct FieldName
        {
            public static readonly string YEAR = "YEAR";
            public static readonly string GRAD_IN_TIME = "GRAD_IN_TIME";
            public static readonly string GRAD_OVER_TIME = "GRAD_OVER_TIME";
            public static readonly string QUITY1 = "QUITY1";
            public static readonly string QUITY2 = "QUITY2";
            public static readonly string QUITY3 = "QUITY3";
            public static readonly string QUITY4 = "QUITY4";
            public static readonly string MOVE_IN = "MOVE_IN";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string TABLE_NAME = "STUDENT_STATUS_OTHER";
        }
        protected struct ParameterName
        {
            public static readonly string GRAD_IN_TIME = "@" + FieldName.GRAD_IN_TIME;
            public static readonly string GRAD_OVER_TIME = "@" + FieldName.GRAD_OVER_TIME;
            public static readonly string QUITY1 = "@" + FieldName.QUITY1;
            public static readonly string QUITY2 = "@" + FieldName.QUITY2;
            public static readonly string QUITY3 = "@" + FieldName.QUITY3;
            public static readonly string QUITY4 = "@" + FieldName.QUITY4;
            public static readonly string MOVE_IN = "@" + FieldName.MOVE_IN;
            public static readonly string CURRI_ID = "@" + FieldName.CURRI_ID;
            public static readonly string YEAR = "@" + FieldName.YEAR;
        }
        private int _year;
        private int _grad_in_time;
        private int _grad_over_time;
        private int _quity1;
        private int _quity2;
        private int _quity3;
        private int _quity4;
        private int _move_in;
        private string _curri_id;
        public int year { get { return _year; } set { _year = value; } }
        public int grad_in_time { get { return _grad_in_time; } set { _grad_in_time = value; } }
        public int grad_over_time { get { return _grad_over_time; } set { _grad_over_time = value; } }
        public int quity1 { get { return _quity1; } set { _quity1 = value; } }
        public int quity2 { get { return _quity2; } set { _quity2 = value; } }
        public int quity3 { get { return _quity3; } set { _quity3 = value; } }
        public int quity4 { get { return _quity4; } set { _quity4 = value; } }
        public int move_in { get { return _move_in; } set { _move_in = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
    }
}