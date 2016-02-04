using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Extra_privilege
    {
        internal struct FieldName
        {
            public static readonly string PERSONNEL_ID = "PERSONNEL_ID";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string TITLE = "TITLE";
            public static readonly string PRIVILEGE = "PRIVILEGE";
            public static readonly string TABLE_NAME = "EXTRA_PRIVILEGE";
        }
        private string _personnel_id;
        private string _curri_id;
        private string _title;
        private string _privilege;
        public string personnel_id { get { return _personnel_id; } set { _personnel_id = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public string title { get { return _title; } set { _title = value; } }
        public string privilege { get { return _privilege; } set { _privilege = value; } }
    }
}