using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Student :User
    {
        internal struct FieldName
        {
            public static readonly string USERNAME = "USERNAME";
            public static readonly string PASSWORD = "PASSWORD";
            public static readonly string USER_TYPE = "USER_TYPE";
            public static readonly string T_PRENAME = "T_PRENAME";
            public static readonly string T_NAME = "T_NAME";
            public static readonly string E_PRENAME = "E_PRENAME";
            public static readonly string E_NAME = "E_NAME";
            public static readonly string CITIZEN_ID = "CITIZEN_ID";
            public static readonly string GENDER = "GENDER";
            public static readonly string EMAIL = "EMAIL";
            public static readonly string TEL = "TEL";
            public static readonly string ADDR = "ADDR";
            public static readonly string FILE_NAME_PIC = "FILE_NAME_PIC";
            public static readonly string TIMESTAMP = "TIMESTAMP";
            //----------------------
            public static readonly string STUDENT_ID = "STUDENT_ID";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string TYPE = "TYPE";
            public static readonly string ADMIS_YEAR = "ADMIS_YEAR";
            public static readonly string ADMIS_DATE = "ADMIS_DATE";
            public static readonly string GRAD_YEAR = "GRAD_YEAR";
            public static readonly string GRAD_SEMESTER = "GRAD_SEMESTER";
            public static readonly string GRAD_DATE = "GRAD_DATE";
            public static readonly string STATUS = "STATUS";
            public static readonly string QUOTA = "QUOTA";
            public static readonly string SUBTYPE = "SUBTYPE";
            public static readonly string COOP = "COOP";
            public static readonly string TABLE_NAME = "STUDENT";
        }

        private string _student_id;
        private string _curri_id;
        private string _type;
        private int _admis_year;
        private DateTime _admis_date;
        private int _grad_year;
        private int _grad_semester;
        private DateTime _grad_date;
        private string _status;
        private char _quota;
        private int _subtype;
        private char _coop;
        public string student_id { get { return _student_id; } set { _student_id = value;} }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public string type { get { return _type; } set { _type = value; } }
        public int admis_year { get { return _admis_year; } set { _admis_year = value; } }
        public DateTime admis_date { get { return _admis_date; } set { _admis_date = value; } }
        public int grad_year { get { return _grad_year; } set { _grad_year = value; } }
        public int grad_semester { get { return _grad_semester; } set { _grad_semester = value; } }
        public DateTime grad_date { get { return _grad_date; } set { _grad_date = value; } }
        public string status { get { return _status; } set { _status = value; } }
        public char quota { get { return _quota; } set { _quota = value; } }
        public int subtype { get { return _subtype; } set { _subtype = value; } }
        public char coop { get { return _coop; } set { _coop = value; } }
    }
}