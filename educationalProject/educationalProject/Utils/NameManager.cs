using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Utils
{
    public class NameManager
    {
        public static string GatherPreName(string preName)
        {
            if (!Convert.ToBoolean(preName.CompareTo("นาย")) || !Convert.ToBoolean(preName.CompareTo("นางสาว")) || !Convert.ToBoolean(preName.CompareTo("นาง")))
                return "อาจารย์";
            else
                return preName;
        }
    }
}