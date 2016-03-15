using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Curriculums_presidents_detail
    {
        private List<string> _all_curri_id;
        private Dictionary<string, Curri_with_pres_and_cand> _all_presidents;
        public List<string> all_curri_id { get { return _all_curri_id; } set { _all_curri_id = value; } }
        public Dictionary<string, Curri_with_pres_and_cand> all_presidents { get { return _all_presidents; } set { _all_presidents = value; } }
        public Curriculums_presidents_detail()
        {
            all_curri_id = new List<string>();
            all_presidents = new Dictionary<string, Curri_with_pres_and_cand>();
        }
    }
}