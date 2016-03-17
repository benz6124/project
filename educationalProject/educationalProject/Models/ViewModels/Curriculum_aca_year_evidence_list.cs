using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Curriculum_aca_year_evidence_list
    {
        private List<string> _all_years;
        private Dictionary<string, Evidence_in_year_list> _in_year;
        private string _curri_id;
        public List<string> all_years { get { return _all_years; } set { _all_years = value; } }
        public Dictionary<string, Evidence_in_year_list> in_year { get { return _in_year; } set { _in_year = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public Curriculum_aca_year_evidence_list()
        {
            all_years = new List<string>();
            in_year = new Dictionary<string, Evidence_in_year_list>();
        }
    }
}