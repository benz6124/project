using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Curri_with_pres_and_cand
    {
        private string _curri_tname;
        private List<Personnel_brief_detail> _presidents;
        private List<Personnel_brief_detail> _candidates;
        public string curri_tname { get { return _curri_tname; } set { _curri_tname = value; } }
        public List<Personnel_brief_detail> presidents { get { return _presidents; } set { _presidents = value; } }
        public List<Personnel_brief_detail> candidates { get { return _candidates; } set { _candidates = value; } }
        public Curri_with_pres_and_cand()
        {
            presidents = new List<Personnel_brief_detail>();
            candidates = new List<Personnel_brief_detail>();
        }
    }
}