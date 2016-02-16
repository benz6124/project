using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Personnel_educational : Personnel
    {
        private List<Educational_teacher_staff> _history;
        public List<Educational_teacher_staff> history { get { return _history; } set { _history = value; } }

        public Personnel_educational()
        {
            history = new List<Educational_teacher_staff>();
        }
    }
}