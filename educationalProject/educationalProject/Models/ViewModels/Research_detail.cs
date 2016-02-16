using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Research_detail : Research
    {
        private List<Teacher_with_t_name> _researcher;
        public List<Teacher_with_t_name> researcher { get { return _researcher; } set { _researcher = value; } }

        public Research_detail()
        {
            researcher = new List<Teacher_with_t_name>();
        }
    }
}