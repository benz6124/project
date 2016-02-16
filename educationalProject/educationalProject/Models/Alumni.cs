using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Alumni : Student
    {
        internal struct ExtraFieldName
        {
            public static readonly string COMPANY_ADDR = "COMPANY_ADDR";
            public static readonly string COMPANY_TEL = "COMPANY_TEL";
            public static readonly string TABLE_NAME = "ALUMNI";
        }
        private string _company_addr;
        private string _company_tel;
        public string company_addr { get { return _company_addr; } set { _company_addr = value; } }
        public string company_tel { get { return _company_tel; } set { _company_tel = value; } }
    }
}