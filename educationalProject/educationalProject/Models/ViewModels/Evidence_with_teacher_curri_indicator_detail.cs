using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Evidence_with_teacher_curri_indicator_detail
    {
        private string _evidence_name;
        private string _curr_tname;
        private int _aca_year;
        private int _indicator_num;
        private string _indicator_name_t;
        private string _t_name;
        private string _email;
        public string evidence_name { get { return _evidence_name; } set { _evidence_name = value; } }
        public string curr_tname { get { return _curr_tname; } set { _curr_tname = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public string indicator_name_t { get { return _indicator_name_t; } set { _indicator_name_t = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public string email { get { return _email; } set { _email = value; } }
    }
}