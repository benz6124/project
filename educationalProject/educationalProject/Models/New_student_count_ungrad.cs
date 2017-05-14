using System;
using System.Collections.Generic;
using System.Linq;

namespace educationalProject.Models
{
    public class New_student_count_ungrad
    {
        public struct FieldName
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
            public static readonly string TABLE_NAME = "NEW_STUDENT_COUNT_UNGRAD";
        }
        public struct ParameterName
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
    }
}