using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Lab_list
    {
        internal struct FieldName
        {
            public static readonly string LAB_NUM = "LAB_NUM";
            public static readonly string NAME = "NAME";
            public static readonly string ROOM = "ROOM";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string TABLE_NAME = "LAB_LIST";
        }
        private int _lab_num;
        private string _name;
        private string _room;
        private string _curri_id;
        private int _aca_year;
        public string name { get { return _name; } set { _name = value; } }
        public string room { get { return _room; } set { _room = value; } }
        public int lab_num { get { return _lab_num; } set { _lab_num = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
    }
}