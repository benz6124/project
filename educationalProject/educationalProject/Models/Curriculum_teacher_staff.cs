using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Curriculum_teacher_staff
    {
        internal struct FieldName
        {
            public static readonly string PERSONNEL_ID = "PERSONNEL_ID";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string TABLE_NAME = "CURRICULUM_TEACHER_STAFF";
        }
        private string _personnel_id;
        private string _curri_id;
        public string personnel_id { get { return _personnel_id; } set { _personnel_id = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
    }
}