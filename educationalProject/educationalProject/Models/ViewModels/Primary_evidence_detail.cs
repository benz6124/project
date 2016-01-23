using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Primary_evidence_detail : Primary_evidence
    {
        private string _teacher_id;
        private char _status;
        public string teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public char status { get { return _status; } set { _status = value; } }
    }
}