using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Curriculum_academic
    {
        protected struct FieldName
        {
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string TABLE_NAME = "CURRICULUM_ACADEMIC";
        }
        protected struct ParameterName
        {
            public static readonly string CURRI_ID = "@"+FieldName.CURRI_ID;
            public static readonly string ACA_YEAR = "@" + FieldName.ACA_YEAR;
        }
        private string _curri_id;
        private int _aca_year;
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
    }
}