using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Lab_list_detail : Lab_list
    {
        private List<Personnel_with_t_name> _officer;
        public List<Personnel_with_t_name> officer { get { return _officer; } set { _officer = value; } }
        public Lab_list_detail()
        {
            officer = new List<Personnel_with_t_name>();
        }
    }
}