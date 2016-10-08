using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Teacher : User
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
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string ROOM = "ROOM";
            public static readonly string DEGREE = "DEGREE";
            public static readonly string POSITION = "POSITION";
            public static readonly string PERSONNEL_TYPE = "PERSONNEL_TYPE";
            public static readonly string PERSON_ID = "PERSON_ID";
            public static readonly string STATUS = "STATUS";
            public static readonly string ALIVE = "ALIVE";
            public static readonly string TABLE_NAME = "TEACHER";
            public static readonly string ALIAS_NAME = "TJ_RES";
        }
        internal struct ParameterName
        {
            public static readonly string USERNAME = "@" + FieldName.USERNAME;
            public static readonly string PASSWORD = "@" + FieldName.PASSWORD;
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

        }

        private int _teacher_id;
        private char _degree;
        private char _position;
        private string _personnel_type;
        private string _person_id;
        private string _room;
        private string _status;
        private int _alive;
        public int teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public char degree { get { return _degree; } set { _degree = value; } }
        public char position { get { return _position; } set { _position = value; } }
        public string personnel_type { get { return _personnel_type; } set { _personnel_type = value; } }
        public string person_id { get { return _person_id; } set { _person_id = value; } }
        public string status { get { return _status; } set { _status = value; } }
        public string room { get { return _room; } set { _room = value; } }
        public int alive { get { return _alive; } set { _alive = value; } }
    }
}