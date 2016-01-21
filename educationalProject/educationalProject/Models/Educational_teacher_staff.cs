using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Educational_teacher_staff
    {
        internal struct FieldName
        {
            public static readonly string PERSONNEL_ID = "PERSONNEL_ID";
            public static readonly string DEGREE = "DEGREE";
            public static readonly string PRE_MAJOR = "PRE_MAJOR";
            public static readonly string MAJOR = "MAJOR";
            public static readonly string GRAD_YEAR = "GRAD_YEAR";
            public static readonly string COLLEGE = "COLLEGE";
            public static readonly string TABLE_NAME = "EDUCATIONAL_TEACHER_STAFF";
        }
        private string _personnel_id;
        private char _degree;
        private string _pre_major;
        private string _major;
        private int _grad_year;
        private string _college;
        public string personnel_id { get { return _personnel_id; } set { _personnel_id = value; } }
        public char degree { get { return _degree; } set { _degree = value; } }
        public string major { get { return _major; } set { _major = value; } }
        public string pre_major { get { return _pre_major; } set { _pre_major = value; } }
        public int grad_year { get { return _grad_year; } set { _grad_year = value; } }
        public string college { get { return _college; } set { _college = value; } }
    }
}