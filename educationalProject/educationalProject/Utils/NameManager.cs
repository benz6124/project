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

        public static string GatherSQLCASEForPrename(string tbl_name,string col_to_check,string col_to_return)
        {
            return string.Format(
            "CASE " +
                "WHEN {0}.{1} = 1 THEN " +
                    "CASE " +
                        "WHEN {0}.{2} in ('นาย','นางสาว','นาง') THEN 'อาจารย์' " +
                        "ELSE {0}.{2} " +
                    "END " +
                "ELSE {0}.{2} " +
            "END ",tbl_name, col_to_check, col_to_return);
        }
    }
}