using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Gallery
    {
        protected struct FieldName
        {
            public static readonly string GALLERY_ID = "GALLERY_ID";
            public static readonly string PERSONNEL_ID = "PERSONNEL_ID";
            public static readonly string NAME = "NAME";
            public static readonly string DATE_CREATED = "DATE_CREATED";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string ACA_YEAR = "ACA_YEAR";
            public static readonly string TABLE_NAME = "GALLERY";
        }
        private int _gallery_id;
        private int _personnel_id;
        private string _name;
        private string _date_created;
        private string _curri_id;
        private int _aca_year;
        public int gallery_id { get { return _gallery_id; } set { _gallery_id = value; } }
        public string name { get { return _name; } set { _name = value; } }
        public int personnel_id { get { return _personnel_id; } set { _personnel_id = value; } }
        public string date_created { get { return _date_created; } set { _date_created = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int aca_year { get { return _aca_year; } set { _aca_year = value; } }
    }
}