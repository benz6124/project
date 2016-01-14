using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Research
    {
        protected struct FieldName
        {
            public static readonly string RESEARCH_ID = "RESEARCH_ID";
            public static readonly string NAME = "NAME";
            public static readonly string YEAR_PUBLISH = "YEAR_PUBLISH";
            public static readonly string TABLE_NAME = "RESEARCH";
        }
        private int _research_id;
        private string _name;
        private int _year_publish;
        public int research_id { get { return _research_id; } set { _research_id = value; } }
        public string name { get { return _name; } set { _name = value; } }
        public int year_publish { get { return _year_publish; } set { _year_publish = value; } }
    }
}