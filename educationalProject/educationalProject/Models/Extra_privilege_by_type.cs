using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Extra_privilege_by_type
    {
        internal struct FieldName
        {
            public static readonly string USER_TYPE = "USER_TYPE";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string TITLE = "TITLE";
            public static readonly string PRIVILEGE = "PRIVILEGE";
            public static readonly string TABLE_NAME = "EXTRA_PRIVILEGE_BY_TYPE";
        }
        private string _user_type;
        private string _curri_id;
        private string _title;
        private string _privilege;
        public string user_type { get { return _user_type; } set { _user_type = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public string title { get { return _title; } set { _title = value; } }
        public string privilege { get { return _privilege; } set { _privilege = value; } }
    }
}