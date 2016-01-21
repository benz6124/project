using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Committee
    {
        protected struct FieldName
        {
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string TABLE_NAME = "COMMITTEE";
        }
        private string _teacher_id;
        private string _curri_id;
        public string teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
    }
}