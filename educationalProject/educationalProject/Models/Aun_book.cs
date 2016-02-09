using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Aun_book
    {
        protected struct FieldName
        {
            public static readonly string FILE_NAME = "FILE_NAME";
            public static readonly string PERSONNEL_ID = "PERSONNEL_ID";
            public static readonly string DATE = "DATE";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string TABLE_NAME = "AUN_BOOK";
        }
        private string _file_name;
        private int _personnel_id;
        private string _date;
        private string _curri_id;
        private int _aca_year;
        public string file_name { get { return _file_name; } set { _file_name = value; } }
        public int personnel_id { get { return _personnel_id; } set { _personnel_id = value; } }
        public string date { get { return _date; } set { _date = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
    }
}