using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Minutes_detail : Minutes
    {
        private string _t_name;
        private List<Teacher_with_t_name> _attendee;
        private List<Minutes_pic> _pictures;

        public List<Teacher_with_t_name> attendee { get { return _attendee; } set { _attendee = value; } }
        public List<Minutes_pic> pictures { get { return _pictures; } set { _pictures = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }

        public Minutes_detail()
        {
            attendee = new List<Teacher_with_t_name>();
            pictures = new List<Minutes_pic>();
        }
    }
}