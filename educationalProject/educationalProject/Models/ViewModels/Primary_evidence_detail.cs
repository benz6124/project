using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Primary_evidence_detail : Primary_evidence
    {
        private int _teacher_id;
        private char _status;
        public int teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public char status { get { return _status; } set { _status = value; } }
    }
}