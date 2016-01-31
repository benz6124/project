using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Minutes
    {
        internal struct FieldName
        {
            public static readonly string MINUTES_ID = "MINUTES_ID";
            public static readonly string DATE = "DATE";
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string TOPIC_NAME = "TOPIC_NAME";
            public static readonly string DETAIL = "DETAIL";
            public static readonly string PLACE = "PLACE";
            public static readonly string DURATION = "DURATION";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string FILE_NAME = "FILE_NAME";
            public static readonly string TABLE_NAME = "MINUTES";
        }
        private int _minutes_id;
        private string _date;
        private string _teacher_id;
        private string _topic_name;
        private string _detail;
        private string _place;
        private string _duration;
        private string _file_name;
        private string _curri_id;
        private int _aca_year;
        public int minutes_id { get { return _minutes_id; } set { _minutes_id = value; } }
        public string date { get { return _date; } set { _date = value; } }
        public string teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public string topic_name { get { return _topic_name; } set { _topic_name = value; } }
        public string detail { get { return _detail; } set { _detail = value; } }
        public string place { get { return _place; } set { _place = value; } }
        public string duration { get { return _duration; } set { _duration = value; } }
        public string file_name { get { return _file_name; } set { _file_name = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
    }
}