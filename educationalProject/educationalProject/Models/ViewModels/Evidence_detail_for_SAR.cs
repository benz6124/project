using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Evidence_detail_for_SAR
    {
        private string _indicator_num;
        private string _evidence_real_code;
        private string _evidence_name;
        public string indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public string evidence_real_code { get { return _evidence_real_code; } set { _evidence_real_code = value; } }
        public string evidence_name { get { return _evidence_name; } set { _evidence_name = value; } }
    }
}