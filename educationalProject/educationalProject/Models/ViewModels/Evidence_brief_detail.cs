using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Evidence_brief_detail
    {
        private string _curri_id;
        private string _curr_tname;
        private int _aca_year;
        private string _evidence_name;
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public string curr_tname { get { return _curr_tname; } set { _curr_tname = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
        public string evidence_name { get { return _evidence_name; } set { _evidence_name = value; } }
    }
}