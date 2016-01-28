using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Questionare_result_obj
    {
        internal struct FieldName
        {
            public static readonly string QUESTIONARE_RES_OBJ_ID = "QUESTIONARE_RES_OBJ_ID";
            public static readonly string QUESTIONARE_QUESTION_ID = "QUESTIONARE_QUESTION_ID";
            public static readonly string ANSWER = "ANSWER";
            public static readonly string TABLE_NAME = "QUESTIONARE_RESULT_OBJ";
        }
        private int _questionare_res_obj_id;
        private int _questionare_question_id;
        private int _answer;
        public int questionare_res_obj_id { get { return _questionare_res_obj_id; } set { _questionare_res_obj_id = value; } }
        public int questionare_question_id { get { return _questionare_question_id; } set { _questionare_question_id = value; } }
        public int answer { get { return _answer; } set { _answer = value; } }
    }
}