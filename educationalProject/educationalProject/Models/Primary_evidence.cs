using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Primary_evidence
    {
        protected struct FieldName
        {
            public static readonly string PRIMARY_EVIDENCE_NUM = "PRIMARY_EVIDENCE_NUM";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string INDICATOR_NUM = "INDICATOR_NUM";
            public static readonly string EVIDENCE_NAME = "EVIDENCE_NAME";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string TABLE_NAME = "PRIMARY_EVIDENCE";
        }

        private int _primary_evidence_num;
        private int _aca_year;
        private int _indicator_num;
        private string _evidence_name;
        private string _curri_id;
        public int primary_evidence_num { get { return _primary_evidence_num; } set { _primary_evidence_num = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public string evidence_name { get { return _evidence_name; } set { _evidence_name = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
    }
}