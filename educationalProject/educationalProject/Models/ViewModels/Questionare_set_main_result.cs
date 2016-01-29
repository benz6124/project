using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace educationalProject.Models.ViewModels
{
    public class Questionare_set_main_result
    {
        private string _name;
        private int[] _answer;

        public string name { get { return _name; } set { _name = value; } }
        public int[] answer { get { return _answer; } set { _answer = value; } }
        public Questionare_set_main_result(string name)
        {
            this.name = name;
            answer = new int []{ 0, 0, 0, 0, 0, 0, 0 };
        }
    }
}