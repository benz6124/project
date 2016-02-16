using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class User_type
    {
        internal struct FieldName
        {
            public static readonly string USER_TYPE = "USER_TYPE";
            public static readonly string TABLE_NAME = "USER_TYPE";
        }
        private string _user_type;
        public string user_type { get { return _user_type; } set { _user_type = value; } }

    }
}