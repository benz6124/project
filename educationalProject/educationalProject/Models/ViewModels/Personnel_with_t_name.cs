using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Personnel_with_t_name
    {
        private string _personnel_id;
        private string _t_name;
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public void SetPersonnelId(string pid)
        {
            _personnel_id = pid;
        }
        public string GetPersonnelId()
        {
            return _personnel_id;
        }
    }
}