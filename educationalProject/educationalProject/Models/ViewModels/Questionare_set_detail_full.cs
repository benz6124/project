using System;
using System.Collections.Generic;
using System.Linq;
namespace educationalProject.Models.ViewModels
{
    public class Questionare_set_detail_full : Questionare_set
    {
        private List<User_type> _my_target;
        private List<Questionare_question_obj> _my_questions;
        public List<User_type> my_target { get { return _my_target; } set { _my_target = value; } }
        public List<Questionare_question_obj> my_questions { get { return _my_questions; } set { _my_questions = value; } }
    } 
}