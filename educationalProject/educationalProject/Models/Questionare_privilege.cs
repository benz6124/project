using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Questionare_privilege
    {
        internal struct FieldName
        {
            public static readonly string QUESTIONARE_SET_ID = "QUESTIONARE_SET_ID";
            public static readonly string PRIVILEGE_TYPE_ID = "PRIVILEGE_TYPE_ID";
            public static readonly string TABLE_NAME = "QUESTIONARE_PRIVILEGE";

        }

        private int _questionare_set_id;
        private string _privilege;
        public int questionare_set_id { get { return _questionare_set_id; } set { _questionare_set_id = value; } }
        public string privilege { get { return _privilege; } set { _privilege = value; } }

    }
}