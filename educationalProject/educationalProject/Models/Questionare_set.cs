using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Questionare_set
    {
        internal struct FieldName
        {
            public static readonly string QUESTIONARE_SET_ID = "QUESTIONARE_SET_ID";
            public static readonly string NAME = "NAME";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string DATE = "DATE";
            public static readonly string PERSONNEL_ID = "PERSONNEL_ID";
            public static readonly string TABLE_NAME = "QUESTIONARE_SET";

        }
        private int _questionare_set_id;
        private string _name;
        private string _curri_id;
        private int _aca_year;
        private string _date;
        private int _personnel_id;
        public int questionare_set_id { get { return _questionare_set_id; } set { _questionare_set_id = value; } }
        public string name { get { return _name; } set { _name = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
        public string date { get { return _date; } set { _date = value; } }
        public int personnel_id { get { return _personnel_id; } set { _personnel_id = value; } }
    }
}