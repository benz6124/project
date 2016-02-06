using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class User_list
    {
        internal struct FieldName
        {
            public static readonly string USER_ID = "USER_ID";
            public static readonly string USER_TYPE = "USER_TYPE";
            public static readonly string TABLE_NAME = "USER_LIST";
        }
        private string _user_id;
        private string _user_type;
        public string user_id { get { return _user_id; } set { _user_id = value; } }
        public string user_type { get { return _user_type; } set { _user_type = value; } }
    }
}