using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Evidence_tiny_detail
    {
        private string _path;
        private string _code;
        public string path { get { return _path; } set { _path = value; } }
        public string code { get { return _code; } set { _code = value; } }
    }
}