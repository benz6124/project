using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class President_curriculum
    {
        internal struct FieldName
        {
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string TABLE_NAME = "PRESIDENT_CURRICULUM";
        }

        internal struct ParameterName
        {
            public static readonly string TEACHER_ID = "@" + FieldName.TEACHER_ID;
            public static readonly string CURRI_ID = "@" + FieldName.CURRI_ID;
            public static readonly string ACA_YEAR = "@" + FieldName.ACA_YEAR;
        }
        private int _teacher_id;
        private int _aca_year;
        private string _curri_id;
        public int teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
    }
}