using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Student_cnt
    {
        protected struct FieldName
        {
            public static readonly string NY1 = "NY1";
            public static readonly string NY2 = "NY2";
            public static readonly string NY3 = "NY3";
            public static readonly string NY4 = "NY4";
            public static readonly string NY5 = "NY5";
            public static readonly string NY6 = "NY6";
            public static readonly string NY7 = "NY7";
            public static readonly string NY8 = "NY8";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string YEAR = "YEAR";
            public static readonly string TABLE_NAME = "STUDENT_CNT";
        }
        private int _ny1;
        private int _ny2;
        private int _ny3;
        private int _ny4;
        private int _ny5;
        private int _ny6;
        private int _ny7;
        private int _ny8;
        private string _curri_id;
        private int _year;
        public int ny1 { get { return _ny1; } set { _ny1 = value; } }
        public int ny2 { get { return _ny2; } set { _ny2 = value; } }
        public int ny3 { get { return _ny3; } set { _ny3 = value; } }
        public int ny4 { get { return _ny4; } set { _ny4 = value; } }
        public int ny5 { get { return _ny5; } set { _ny5 = value; } }
        public int ny6 { get { return _ny6; } set { _ny6 = value; } }
        public int ny7 { get { return _ny7; } set { _ny7 = value; } }
        public int ny8 { get { return _ny8; } set { _ny8 = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int year { get { return _year; } set { _year = value; } }
    }
}