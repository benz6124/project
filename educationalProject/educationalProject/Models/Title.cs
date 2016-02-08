using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Title
    {
        internal struct FieldName
        {
            public static readonly string TITLE_CODE = "TITLE_CODE";
            public static readonly string NAME = "NAME";
            public static readonly string TABLE_NAME = "TITLE";
        }
        private int _title_code;
        private string _name;
        public int title_code { get { return _title_code; } set { _title_code = value; } }
        public string name { get { return _name; } set { _name = value; } }
    }
}