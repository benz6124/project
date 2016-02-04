using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Title_privilege
    {
        internal struct FieldName
        {
            public static readonly string TITLE = "TITLE";
            public static readonly string PRIVILEGE = "PRIVILEGE";
            public static readonly string TABLE_NAME = "TITLE_PRIVILEGE";
        }
        private string _title;
        private string _privilege;
        public string title { get { return _title; } set { _title = value; } }
        public string privilege { get { return _privilege; } set { _privilege = value; } }
    }
}