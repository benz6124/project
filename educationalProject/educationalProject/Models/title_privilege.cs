﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Title_privilege
    {
        internal struct FieldName
        {
            public static readonly string TITLE_CODE = "TITLE_CODE";
            public static readonly string TITLE_PRIVILEGE_CODE = "TITLE_PRIVILEGE_CODE";
            public static readonly string PRIVILEGE = "PRIVILEGE";
            public static readonly string TABLE_NAME = "TITLE_PRIVILEGE";
        }
        private int _title_code;
        private int _title_privilege_code;
        private string _privilege;
        public int title_code { get { return _title_code; } set { _title_code = value; } }
        public int title_privilege_code { get { return _title_privilege_code; } set { _title_privilege_code = value; } }
        public string privilege { get { return _privilege; } set { _privilege = value; } }

        public Title_privilege(int titlecode,int titleprivilegecode,string privilege)
        {
            title_code = titlecode;
            title_privilege_code = titleprivilegecode;
            this.privilege = privilege;
        }
        public Title_privilege()
        {

        }
    }
}