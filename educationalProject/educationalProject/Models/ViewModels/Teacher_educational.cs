using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Teacher_educational : Teacher
    {
        private List<Educational_teacher_staff> _history;
        public List<Educational_teacher_staff> history { get { return _history; } set { _history = value; } }
        public Teacher_educational()
        {
            history = new List<Educational_teacher_staff>();
        }
    }
}