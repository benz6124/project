using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Default_privilege
    {
        internal struct FieldName
        {
            public static readonly string PERSONNEL_ID = "PERSONNEL_ID";
            public static readonly string TITLE = "TITLE";
            public static readonly string PRIVILEGE = "PRIVILEGE";
            public static readonly string TABLE_NAME = "DEFAULT_PRIVILEGE";
        }
        private string _personnel_id;
        private string _title;
        private string _privilege;
        public string personnel_id { get { return _personnel_id; } set { _personnel_id = value; } }
        public string title { get { return _title; } set { _title = value; } }
        public string privilege { get { return _privilege; } set { _privilege = value; } }
    }
}