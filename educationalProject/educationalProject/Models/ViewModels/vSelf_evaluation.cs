﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class vSelf_evaluation
    {
        private int _indicator_num;
        private int _sub_indicator_num;
        private string _teacher_id;
        private int _evaluation_score;
        private string _date;
        private string _time;
        private string _curri_id;
        private int _aca_year;
        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public int sub_indicator_num { get { return _sub_indicator_num; } set { _sub_indicator_num = value; } }
        public string teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public int evaluation_score { get { return _evaluation_score; } set { _evaluation_score = value; } }
        public string date { get { return _date; } set { _date = value; } }
        public string time { get { return _time; } set { _time = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
    }
}