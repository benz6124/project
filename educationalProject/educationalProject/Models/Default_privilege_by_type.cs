using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Default_privilege_by_type
    {
        internal struct FieldName
        {
            public static readonly string USER_TYPE = "USER_TYPE";
            public static readonly string TITLE_CODE = "TITLE_CODE";
            public static readonly string TITLE_PRIVILEGE_CODE = "TITLE_PRIVILEGE_CODE";
            public static readonly string TABLE_NAME = "DEFAULT_PRIVILEGE_BY_TYPE";
        }
        private string _user_type;
        private int _title_code;
        private int _title_privilege_code;
        public string user_type { get { return _user_type; } set { _user_type = value; } }
        public int title_code { get { return _title_code; } set { _title_code = value; } }
        public int title_privilege_code { get { return _title_privilege_code; } set { _title_privilege_code = value; } }
    }
}