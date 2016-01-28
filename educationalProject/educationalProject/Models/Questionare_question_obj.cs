using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Questionare_question_obj
    {
        internal struct FieldName
        {
            public static readonly string QUESTIONARE_SET_ID = "QUESTIONARE_SET_ID";
            public static readonly string QUESTIONARE_QUESTION_ID = "QUESTIONARE_QUESTION_ID";
            public static readonly string DETAIL = "DETAIL";
            public static readonly string TABLE_NAME = "QUESTIONARE_QUESTION_OBJ";
        }
        private int _questionare_set_id;
        private int _questionare_question_id;
        private string _detail;
        public int questionare_set_id { get { return _questionare_set_id; } set { _questionare_set_id = value; } }
        public int questionare_question_id { get { return _questionare_question_id; } set { _questionare_question_id = value; } }
        public string detail { get { return _detail; } set { _detail = value; } }
    }
}