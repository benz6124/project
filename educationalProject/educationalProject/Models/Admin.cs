using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Admin : User
    {
        internal struct FieldName
        {
            public static readonly string USERNAME = "USERNAME";
            public static readonly string PASSWORD = "PASSWORD";
            public static readonly string USER_TYPE = "USER_TYPE";
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
            public static readonly string ADMIN_CREATOR_ID = "ADMIN_CREATOR_ID";
            public static readonly string TABLE_NAME = "ADMIN";
        }

        private string _admin_creator_id;
        public string admin_creator_id { get { return  _admin_creator_id; } set { _admin_creator_id = value; } }
    }
}