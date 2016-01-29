using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Research_owner
    {
        internal struct FieldName
        {
            public static readonly string RESEARCH_ID = "RESEARCH_ID";
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string TABLE_NAME = "RESEARCH_OWNER";
        }
        private int _research_id;
        private string _teacher_id;
        public int research_id { get { return _research_id; } set { _research_id = value; } }
        public string teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
    }
}