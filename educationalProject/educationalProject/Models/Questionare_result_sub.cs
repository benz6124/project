using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Questionare_result_sub
    {
        internal struct FieldName
        {
            public static readonly string QUESTIONARE_RES_SUB_ID = "QUESTIONARE_RES_SUB_ID";
            public static readonly string QUESTIONARE_SET_ID = "QUESTIONARE_SET_ID";
            public static readonly string SUGGESTION = "SUGGESTION";
            public static readonly string TABLE_NAME = "QUESTIONARE_RESULT_SUB";
        }

        private int _questionare_res_sub_id;
        private int _questionare_set_id;
        private string _suggestion;
        public int questionare_res_sub_id { get { return _questionare_res_sub_id; } set { _questionare_res_sub_id = value; } }
        public int questionare_set_id { get { return _questionare_set_id; } set { _questionare_set_id = value; } }
        public string suggestion { get { return _suggestion; } set { _suggestion = value; } }
    }
}