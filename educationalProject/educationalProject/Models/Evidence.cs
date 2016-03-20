using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Evidence
    {
        internal struct FieldName
        {
            public static readonly string EVIDENCE_CODE = "EVIDENCE_CODE";
            public static readonly string FILE_NAME = "FILE_NAME";
            public static readonly string EVIDENCE_NAME = "EVIDENCE_NAME";
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string SECRET = "SECRET";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string EVIDENCE_REAL_CODE = "EVIDENCE_REAL_CODE";
            public static readonly string INDICATOR_NUM = "INDICATOR_NUM";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string PRIMARY_EVIDENCE_NUM = "PRIMARY_EVIDENCE_NUM";
            public static readonly string TABLE_NAME = "EVIDENCE";
        }

        protected struct ParameterName
        {
            public static readonly string EVIDENCE_CODE = "@" + FieldName.EVIDENCE_CODE;
            public static readonly string FILE_NAME = "@" + FieldName.FILE_NAME;
            public static readonly string EVIDENCE_NAME = "@" + FieldName.EVIDENCE_NAME;
            public static readonly string TEACHER_ID = "@" + FieldName.TEACHER_ID;
            public static readonly string SECRET = "@" + FieldName.SECRET;
            public static readonly string EVIDENCE_REAL_CODE = "@" + FieldName.EVIDENCE_REAL_CODE;
            public static readonly string INDICATOR_NUM = "@" + FieldName.INDICATOR_NUM;
            public static readonly string PRIMARY_EVIDENCE_NUM = "@" + FieldName.PRIMARY_EVIDENCE_NUM;
            public static readonly string CURRI_ID = "@" + FieldName.CURRI_ID;
            public static readonly string ACA_YEAR = "@" + FieldName.ACA_YEAR;
        }

        private int _evidence_code;
        private string _file_name;
        private string _evidence_name;
        private int _teacher_id;
        private char _secret;
        private string _curri_id;
        private int _aca_year;
        private int _indicator_num;
        private int _evidence_real_code;
        private int _primary_evidence_num;
        public int evidence_code { get { return _evidence_code; } set { _evidence_code = value; } }
        public string file_name { get { return _file_name; } set { _file_name = value; } }
        public string evidence_name { get { return _evidence_name; } set { _evidence_name = value; } }
        public int teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public char secret { get { return _secret; } set { _secret = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public int evidence_real_code { get { return _evidence_real_code; } set { _evidence_real_code = value; } }
        public int primary_evidence_num { get { return _primary_evidence_num; } set { _primary_evidence_num = value; } }
    }
}