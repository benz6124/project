using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class User_list
    {
        internal struct FieldName
        {
            public static readonly string USERNAME = "USERNAME";
            public static readonly string USER_ID = "USER_ID";
            public static readonly string USER_TYPE_ID = "USER_TYPE_ID";
            public static readonly string T_PRENAME = "T_PRENAME";
            public static readonly string T_NAME = "T_NAME";
            public static readonly string E_PRENAME = "E_PRENAME";
            public static readonly string E_NAME = "E_NAME";
            public static readonly string CITIZEN_ID = "CITIZEN_ID";
            public static readonly string GENDER = "GENDER";
            public static readonly string EMAIL = "EMAIL";
            public static readonly string TEL = "TEL";
            public static readonly string ADDR = "ADDR";
            public static readonly string FILE_NAME_PIC = "FILE_NAME_PIC";
            public static readonly string TIMESTAMP = "TIMESTAMP";
            public static readonly string TABLE_NAME = "USER_LIST";
            public static readonly string PASSWORD = "PASSWORD";
        }
        public struct ParameterName
        {
            public static readonly string USER_ID = "@" + FieldName.USER_ID;
            public static readonly string USERNAME = "@" + FieldName.USERNAME;
            public static readonly string USER_TYPE_ID = "@" + FieldName.USER_TYPE_ID;
            public static readonly string T_PRENAME = "@" + FieldName.T_PRENAME;
            public static readonly string T_NAME = "@" + FieldName.T_NAME;
            public static readonly string E_PRENAME = "@" + FieldName.E_PRENAME;
            public static readonly string E_NAME = "@" + FieldName.E_NAME;
            public static readonly string CITIZEN_ID = "@" + FieldName.CITIZEN_ID;
            public static readonly string GENDER = "@" + FieldName.GENDER;
            public static readonly string EMAIL = "@" + FieldName.EMAIL;
            public static readonly string TEL = "@" + FieldName.TEL;
            public static readonly string ADDR = "@" + FieldName.ADDR;
            public static readonly string FILE_NAME_PIC = "@" + FieldName.FILE_NAME_PIC;
            public static readonly string TIMESTAMP = "@" + FieldName.TIMESTAMP;
            public static readonly string PASSWORD = "@PASSWORD";
        }
        private int _user_id;
        private string _user_type;
        public int user_id { get { return _user_id; } set { _user_id = value; } }
        public string user_type { get { return _user_type; } set { _user_type = value; } }
    }
}