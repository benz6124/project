﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Evidence_with_t_name : Evidence
    {
        private string _t_name;
        public string t_name { get { return _t_name; } set { _t_name = value; } }
    }
}