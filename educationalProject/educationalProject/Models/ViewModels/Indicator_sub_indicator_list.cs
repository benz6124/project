using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Indicator_sub_indicator_list : Indicator
    {
        private List<Sub_indicator> _sub_indicator_list;
        public List<Sub_indicator> sub_indicator_list { get { return _sub_indicator_list; } set { _sub_indicator_list = value; } }
        public Indicator_sub_indicator_list()
        {
            sub_indicator_list = new List<Sub_indicator>();
        }
    }
}