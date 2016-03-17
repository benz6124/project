using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Evidence_in_year_list
    {
        private List<string> _all_evidences;
        private Dictionary<string, Evidence_tiny_detail> _detail_evidence;
        public List<string> all_evidences { get { return _all_evidences; } set { _all_evidences = value; } }
        public Dictionary<string, Evidence_tiny_detail> detail_evidence { get { return _detail_evidence; } set { _detail_evidence = value; } }
        public Evidence_in_year_list()
        {
            all_evidences = new List<string>();
            detail_evidence = new Dictionary<string, Evidence_tiny_detail>();
        }
    }
}