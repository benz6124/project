using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Primary_evidence_status
    {
        internal struct FieldName
        {
            public static readonly string PRIMARY_EVIDENCE_NUM = "PRIMARY_EVIDENCE_NUM";
            public static readonly string TEACHER_ID = "TEACHER_ID";
            public static readonly string STATUS = "STATUS";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string TABLE_NAME = "PRIMARY_EVIDENCE_STATUS";
        }

        private int _primary_evidence_num;
        private int _teacher_id;
        private char _status;
        private string _curri_id;
        public int primary_evidence_num { get { return _primary_evidence_num; } set { _primary_evidence_num = value; } }
        public int teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public char status { get { return _status; } set { _status = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
    }
}