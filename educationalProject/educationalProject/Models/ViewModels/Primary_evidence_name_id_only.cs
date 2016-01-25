using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Primary_evidence_name_id_only
    {
        private string _evidence_name;
        private int _primary_evidence_num;
        public string evidence_name { get { return _evidence_name; } set { _evidence_name = value; } }
        public int primary_evidence_num { get { return _primary_evidence_num; } set { _primary_evidence_num = value; } }
    }
}