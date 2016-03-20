using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Section_save_with_sub_indicator_detail
    {
        private int _indicator_num;
        private int _sub_indicator_num;
        private string _detail;
        private string _strength;
        private string _improve;
        private string _weakness;
        private string _sub_indicator_name;

        public int indicator_num { get { return _indicator_num; } set { _indicator_num = value; } }
        public int sub_indicator_num { get { return _sub_indicator_num; } set { _sub_indicator_num = value; } }
        public string detail { get { return _detail; } set { _detail = value; } }
        public string strength { get { return _strength; } set { _strength = value; } }
        public string improve { get { return _improve; } set { _improve = value; } }
        public string weakness { get { return _weakness; } set { _weakness = value; } }
        public string sub_indicator_name { get { return _sub_indicator_name; } set { _sub_indicator_name = value; } }
    }
}