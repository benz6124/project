using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class User_curriculum
    {
        internal struct FieldName
        {
            public static readonly string USER_ID = "USER_ID";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string TABLE_NAME = "USER_CURRICULUM";
        }
        private int _user_id;
        private string _curri_id;
        public int user_id { get { return _user_id; } set { _user_id = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
    }
}