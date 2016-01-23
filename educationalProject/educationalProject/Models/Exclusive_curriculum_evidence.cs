using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Exclusive_curriculum_evidence
    {
        internal struct FieldName
        {
            public static readonly string PRIMARY_EVIDENCE_NUM = "PRIMARY_EVIDENCE_NUM";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string TABLE_NAME = "EXCLUSIVE_CURRICULUM_EVIDENCE";
        }

        private int _primary_evidence_num;
        private string _curri_id;
        public int primary_evidence_num { get { return _primary_evidence_num; } set { _primary_evidence_num = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
    }
}