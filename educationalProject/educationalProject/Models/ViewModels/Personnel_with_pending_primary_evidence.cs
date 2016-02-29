using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Personnel_with_pending_primary_evidence
    {
        private int _teacher_id;
        private string _t_name;
        private string _email;
        private List<Evidence_brief_detail> _pendinglist;
        public int teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public string email { get { return _email; } set { _email = value; } }
        public List<Evidence_brief_detail> pendinglist { get { return _pendinglist; } set { _pendinglist = value; } }
    }
}