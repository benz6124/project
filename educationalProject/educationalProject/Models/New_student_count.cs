using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class New_student_count
    {
        protected struct FieldName
        {
            public static readonly string YEAR = "YEAR";
            public static readonly string CURRI_ID = "CURRI_ID";
            public static readonly string NUM_GOODSTUDY_M = "NUM_GOODSTUDY_M";
            public static readonly string NUM_GOODSTUDY_F = "NUM_GOODSTUDY_F";
            public static readonly string NUM_CHILDSTAFF_M = "NUM_CHILDSTAFF_M";
            public static readonly string NUM_CHILDSTAFF_F = "NUM_CHILDSTAFF_F";
            public static readonly string NUM_DIRECT_M = "NUM_DIRECT_M";
            public static readonly string NUM_DIRECT_F = "NUM_DIRECT_F";
            public static readonly string NUM_ADMIS_M = "NUM_ADMIS_M";
            public static readonly string NUM_ADMIS_F = "NUM_ADMIS_F";
            public static readonly string NUM_OTHERS_M = "NUM_OTHERS_M";
            public static readonly string NUM_OTHERS_F = "NUM_OTHERS_F";
            public static readonly string TABLE_NAME = "NEW_STUDENT_COUNT";
        }
        protected struct ParameterName
        {
            public static readonly string NUM_GOODSTUDY_M = "@" + FieldName.NUM_GOODSTUDY_M;
            public static readonly string NUM_GOODSTUDY_F = "@" + FieldName.NUM_GOODSTUDY_F;
            public static readonly string NUM_CHILDSTAFF_M = "@" + FieldName.NUM_CHILDSTAFF_M;
            public static readonly string NUM_CHILDSTAFF_F = "@" + FieldName.NUM_CHILDSTAFF_F;
            public static readonly string NUM_DIRECT_M = "@" + FieldName.NUM_DIRECT_M;
            public static readonly string NUM_DIRECT_F = "@" + FieldName.NUM_DIRECT_F;
            public static readonly string NUM_ADMIS_M = "@" + FieldName.NUM_ADMIS_M;
            public static readonly string NUM_ADMIS_F = "@" + FieldName.NUM_ADMIS_F;
            public static readonly string NUM_OTHERS_M = "@" + FieldName.NUM_OTHERS_M;
            public static readonly string NUM_OTHERS_F = "@" + FieldName.NUM_OTHERS_F;
            public static readonly string CURRI_ID = "@" + FieldName.CURRI_ID;
            public static readonly string YEAR = "@" + FieldName.YEAR;
        }
        private int _year;
        private string _curri_id;
        private int _num_goodstudy_m;
        private int _num_goodstudy_f;
        private int _num_childstaff_m;
        private int _num_childstaff_f;
        private int _num_direct_m;
        private int _num_direct_f;
        private int _num_admis_m;
        private int _num_admis_f;
        private int _num_others_m;
        private int _num_others_f;

        public int year { get { return _year; } set { _year = value; } }
        public string curri_id { get { return _curri_id; } set { _curri_id = value; } }
        public int num_goodstudy_m { get { return _num_goodstudy_m; } set { _num_goodstudy_m = value; } }
        public int num_goodstudy_f { get { return _num_goodstudy_f; } set { _num_goodstudy_f = value; } }
        public int num_childstaff_m { get { return _num_childstaff_m; } set { _num_childstaff_m = value; } }
        public int num_childstaff_f { get { return _num_childstaff_f; } set { _num_childstaff_f = value; } }
        public int num_direct_m { get { return _num_direct_m; } set { _num_direct_m = value; } }
        public int num_direct_f { get { return _num_direct_f; } set { _num_direct_f = value; } }
        public int num_admis_m { get { return _num_admis_m; } set { _num_admis_m = value; } }
        public int num_admis_f { get { return _num_admis_f; } set { _num_admis_f = value; } }
        public int num_others_m { get { return _num_others_m; } set { _num_others_m = value; } }
        public int num_others_f { get { return _num_others_f; } set { _num_others_f = value; } }
    }
}