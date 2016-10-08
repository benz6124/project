using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class User_type
    {
        internal struct FieldName
        {
            public static readonly string USER_TYPE_ID = "USER_TYPE_ID";
            public static readonly string USER_TYPE_NAME = "USER_TYPE";
            public static readonly string TABLE_NAME = "USER_TYPE";
        }
        protected struct ParameterName
        {
            public static readonly string USER_TYPE_ID = "@" + FieldName.USER_TYPE_ID;
        }
        private string _user_type;
        private int _user_type_id;
        public string user_type { get { return _user_type; } set { _user_type = value; } }
        public int user_type_id { get { return _user_type_id; } set { _user_type_id = value; } }

    }
}