using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models
{
    public class Picture
    {
        internal struct FieldName
        {
            public static readonly string GALLERY_ID = "GALLERY_ID";
            public static readonly string FILE_NAME = "FILE_NAME";
            public static readonly string CAPTION = "CAPTION";
            public static readonly string TABLE_NAME = "PICTURE";
        }

        private int _gallery_id;
        private string _file_name;
        private string _caption;
        public int gallery_id { get { return _gallery_id; } set { _gallery_id = value; } }
        public string file_name { get { return _file_name; } set { _file_name = value; } }
        public string caption { get { return _caption; } set { _caption = value; } }
    }
}