using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Personnel
    {
        internal struct FieldName
        {
            public static readonly string USERNAME = "USERNAME";
            public static readonly string PERSONNEL_ID = "PERSONNEL_ID";
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
            public static readonly string ROOM = "ROOM";
            //----------------------
            public static readonly string CURRI_ID = "CURRI_ID";
        }

        private string _personnel_id;
        private string _username;
        private string _user_type;
        private string _t_prename;
        private string _t_name;
        private string _e_prename;
        private string _e_name;
        private string _citizen_id;
        private char _gender;
        private string _email;
        private string _tel;
        private string _addr;
        private string _file_name_pic;
        private string _timestamp;
        private string _room;
        private string _curri_id;

        public string personnel_id { get { return _personnel_id; } set { _personnel_id = value; } }
        public string username { get { return _username; } set { _username = value; } }
        public string user_type { get { return _user_type; } set { _user_type = value; } }
        public string t_prename { get { return _t_prename; } set { _t_prename = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public string e_prename { get { return _e_prename; } set { _e_prename = value; } }
        public string e_name { get { return _e_name; } set { _e_name = value; } }
        public string citizen_id { get { return _citizen_id; } set { _citizen_id = value; } }
        public char gender { get { return _gender; } set { _gender = value; } }
        public string email { get { return _email; } set { _email = value; } }
        public string tel { get { return _tel; } set { _tel = value; } }
        public string addr { get { return _addr; } set { _addr = value; } }
        public string file_name_pic { get { return _file_name_pic; } set { _file_name_pic = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public string timestamp { get { return _timestamp; } set { _timestamp = value; } }
        public string room { get { return _room; } set { _room = value; } }
    }
}