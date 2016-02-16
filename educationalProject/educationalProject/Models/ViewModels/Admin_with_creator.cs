using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models.ViewModels
{
    public class Admin_with_creator : Admin
    {
        private string _creator_name;
        public string creator_name { get { return _creator_name; } set { _creator_name = value; } }
    }
}