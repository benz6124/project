using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Questionare_question_form
    {
        private List<Questionare_question_answer> _question_list;
        private string _suggestion;
        public List<Questionare_question_answer> question_list { get { return _question_list; } set { _question_list = value; } }
        public string suggestion { get { return _suggestion; } set { _suggestion = value; } }
        public Questionare_question_form()
        {
            question_list = new List<Questionare_question_answer>();
            suggestion = "";
        }
    }
}