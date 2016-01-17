using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Evidence_indicator
    {
        internal struct FieldName
        {
            public static readonly string EVIDENCE_CODE = "EVIDENCE_CODE";
            public static readonly string INDICATOR_NUM = "INDICATOR_NUM";
            public static readonly string TABLE_NAME = "EVIDENCE_INDICATOR";
        }
        private int _evidence_code;
        private int _indicator_num;
        public int evidence_code { get { return _evidence_code; } set { _evidence_code = value; } }
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
    }
}