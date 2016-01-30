using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Teacher_with_t_name : Personnel_with_t_name
    {
        //private string _teacher_id;
       // private string _t_name;
        public string teacher_id { get { return GetPersonnelId(); } set { SetPersonnelId(value); } }
        //    public string t_name { get { return _t_name; } set { _t_name = value; } }

        public override string GetTypeName()
        {
            return "Teacher_with_t_name";
        }
    }
}