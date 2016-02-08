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
            public static readonly string TITLE_CODE = "TITLE_CODE";
            public static readonly string TITLE_PRIVILEGE_CODE = "TITLE_PRIVILEGE_CODE";
            public static readonly string TABLE_NAME = "EXTRA_PRIVILEGE";
        }
        private string _personnel_id;
        private string _curri_id;
        private int _title_code;
        private int _title_privilege_code;
        public string personnel_id { get { return _personnel_id; } set { _personnel_id = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int title_code { get { return _title_code; } set { _title_code = value; } }
        public int title_privilege_code { get { return _title_privilege_code; } set { _title_privilege_code = value; } }
    }
}