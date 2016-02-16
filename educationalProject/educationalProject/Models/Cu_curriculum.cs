using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class Cu_curriculum
    {
        public struct FieldName
        {
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string YEAR = "YEAR";
            public static readonly string CURR_TNAME = "CURR_TNAME";
            public static readonly string CURR_ENAME = "CURR_ENAME";
            public static readonly string DEGREE_T_FULL = "DEGREE_T_FULL";
            public static readonly string DEGREE_T_BF = "DEGREE_T_BF";
            public static readonly string DEGREE_E_FULL = "DEGREE_E_FULL";
            public static readonly string DEGREE_E_BF = "DEGREE_E_BF";
            public static readonly string LEVEL = "LEVEL";
            public static readonly string PERIOD = "PERIOD";
            public static readonly string TABLE_NAME = "CU_CURRICULUM";
        }
        public struct ParameterName
        {
            public static readonly string CURRI_ID = "@"+FieldName.CURRI_ID;
            public static readonly string YEAR = "@" + FieldName.YEAR;
            public static readonly string CURR_TNAME = "@" + FieldName.CURR_TNAME;
            public static readonly string CURR_ENAME = "@" + FieldName.CURR_ENAME;
            public static readonly string DEGREE_T_FULL = "@" + FieldName.DEGREE_T_FULL;
            public static readonly string DEGREE_T_BF = "@" + FieldName.DEGREE_T_BF;
            public static readonly string DEGREE_E_FULL = "@" + FieldName.DEGREE_E_FULL;
            public static readonly string DEGREE_E_BF = "@" + FieldName.DEGREE_E_BF;
            public static readonly string LEVEL = "@" + FieldName.LEVEL;
            public static readonly string PERIOD = "@" + FieldName.PERIOD;
        }
        private string _curri_id;
        private string _year;
        private string _curr_tname;
        private string _curr_ename;
        private string _degree_t_full;
        private string _degree_t_bf;
        private string _degree_e_full;
        private string _degree_e_bf;
        private char _level;
        private char _period;
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public string year { get { return _year; } set { _year = value; } }
        public string curr_tname { get { return _curr_tname; } set { _curr_tname = value; } }
        public string curr_ename { get { return _curr_ename; } set { _curr_ename = value; } }
        public string degree_t_full { get { return _degree_t_full; } set { _degree_t_full = value; } }
        public string degree_t_bf { get { return _degree_t_bf; } set { _degree_t_bf = value; } }
        public string degree_e_full { get { return _degree_e_full; } set { _degree_e_full = value; } }
        public string degree_e_bf { get { return _degree_e_bf; } set { _degree_e_bf = value; } }
        public char level { get { return _level; } set { _level = value; } }
        public char period { get { return _period; } set { _period = value; } }
    }
}