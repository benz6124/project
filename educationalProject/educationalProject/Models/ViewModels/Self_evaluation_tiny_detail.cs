using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Self_evaluation_tiny_detail
    {
        private int _sub_indicator_num;
        private int _evaluation_score;
        public int sub_indicator_num { get { return _sub_indicator_num; } set { _sub_indicator_num = value; } }
        public int evaluation_score { get { return _evaluation_score; } set { _evaluation_score = value; } }
    }
}