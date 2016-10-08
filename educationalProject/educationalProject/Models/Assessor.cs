using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Assessor : User
    {
        internal struct FieldName
        {
            public static readonly string USERNAME = "USERNAME";
            public static readonly string PASSWORD = "PASSWORD";
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
            //----------------------
            public static readonly string ASSESSOR_ID = "ASSESSOR_ID";
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string ALIAS_NAME = "AJ_RES";
            public static readonly string TABLE_NAME = "ASSESSOR";
        }
        private int _assessor_id;
        private int _teacher_id;
        public int assessor_id { get { return _assessor_id; } set { _assessor_id = value; } }
        public int teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
    }
}