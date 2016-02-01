using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Gallery_detail : Gallery
    {
        private string _t_name;
        private List<Picture> _pictures;

        public string t_name { get { return _t_name; } set { _t_name = value; } }
        public List<Picture> pictures { get { return _pictures; } set { _pictures = value; } }

        public Gallery_detail()
        {
            pictures = new List<Picture>();
        }
    }
}