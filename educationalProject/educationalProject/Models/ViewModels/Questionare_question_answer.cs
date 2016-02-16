using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Questionare_question_answer : Questionare_question_obj
    {
        private int _answer;
        public int answer { get { return _answer; } set { _answer = value; } }
    }
}