using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Staff_with_t_name
    {
        private int _staff_id;
        private string _t_name;
        public int staff_id { get { return _staff_id; } set { _staff_id = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }
    }
}