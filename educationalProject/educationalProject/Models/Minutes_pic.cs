using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Minutes_pic
    {
        internal struct FieldName
        {
            public static readonly string MINUTES_ID = "MINUTES_ID";
            public static readonly string FILE_NAME = "FILE_NAME";
            public static readonly string TABLE_NAME = "MINUTES_PIC";
        }
        private int _minutes_id;
        private string _file_name;
        public int minutes_id { get { return _minutes_id; } set { _minutes_id = value; } }
        public string file_name { get { return _file_name; } set { _file_name = value; } }
    }
}