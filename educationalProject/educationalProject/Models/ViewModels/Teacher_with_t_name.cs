﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Teacher_with_t_name
    {
        private int _teacher_id;
        private string _t_name;
        public int teacher_id { get { return _teacher_id; } set { _teacher_id = value; } }
        public string t_name { get { return _t_name; } set { _t_name = value; } }
    }
}