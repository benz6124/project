using System;
using System.Collections.Generic;
using System.Linq;


namespace educationalProject.Models
{
    public class Lab_officer
    {
        internal struct FieldName
        {
            public static readonly string LAB_NUM = "LAB_NUM";
            public static readonly string OFFICER = "OFFICER";
            public static readonly string TABLE_NAME = "LAB_OFFICER";
        }
        private int _lab_num;
        private int _officer;
        public int lab_num { get { return _lab_num; } set { _lab_num = value; } }
        public int officer { get { return _officer; } set { _officer = value; } }
    }
}