using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Committee
    {
        protected struct FieldName
        {
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string DATE_PROMOTED = "DATE_PROMOTED";
            public static readonly string TABLE_NAME = "COMMITTEE";
        }
        private string _teacher_id;
        private string _date_promoted;
        private string _curri_id;
        private int _aca_year;
        public string teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public string date_promoted { get { return _date_promoted; } set { _date_promoted = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
    }
}