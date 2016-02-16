using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Personnel_with_t_name
    {
        private int _user_id;
        private string _t_name;
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public int user_id { get { return _user_id; } set { _user_id = value; } }

        public virtual string GetTypeName()
        {
            return "Personnel_with_t_name";
        }
    }
}