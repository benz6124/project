using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.Wrappers;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oEvaluation_overall_result : Evaluation_overall_result
    {
        public object Select()
        {
            //System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            d.iCommand.CommandText = string.Format("select {0}.*,{1},{2} from {3} INNER JOIN {4} on {5}.{6}={7}.{8}",
                Self_evaluation.FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                Self_evaluation.FieldName.TABLE_NAME,
                Teacher.FieldName.TABLE_NAME, Self_evaluation.FieldName.TABLE_NAME,
                Self_evaluation.FieldName.TEACHER_ID, Teacher.FieldName.TABLE_NAME, Teacher.FieldName.TEACHER_ID);
            try
            {
                //Retrieve self_evaluation data
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        string h, m;
                        DateTime timeofday = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.TIME].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                        h = timeofday.Hour.ToString();
                        m = timeofday.Minute.ToString();
                        self.Add(new vSelf_evaluation_teacher
                        {
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.ACA_YEAR].Ordinal]),
                            curri_id = item.ItemArray[data.Columns[Self_evaluation.FieldName.CURRI_ID].Ordinal].ToString(),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.INDICATOR_NUM].Ordinal]),
                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.SUB_INDICATOR_NUM].Ordinal]),
                            evaluation_score = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.EVALUATION_SCORE].Ordinal]),
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.TEACHER_ID].Ordinal]),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                            date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                            time = (timeofday.Hour > 9 ? "" : "0") + h + '.' + (timeofday.Minute > 9 ? "" : "0") + m
                        });
                    }
                    res.Close();
                    data.Dispose();
                    //Get another data....
                }
                else
                {
                    //Reserved for return error string
                    return "";
                }

                d.iCommand.CommandText = string.Format("select {0}.*,{1},{2} from {3} INNER JOIN {4} on {5}.{6}={7}.{8}",
                Others_evaluation.FieldName.TABLE_NAME, Assessor.FieldName.T_PRENAME, Assessor.FieldName.T_NAME,
                Others_evaluation.FieldName.TABLE_NAME,
                Assessor.FieldName.TABLE_NAME, Others_evaluation.FieldName.TABLE_NAME,
                Others_evaluation.FieldName.ASSESSOR_ID, Assessor.FieldName.TABLE_NAME, Assessor.FieldName.USERNAME);

                //retrieve others_evaluation data
                res = d.iCommand.ExecuteReader();

                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        string h, m;
                        DateTime timeofday = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.TIME].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                        h = timeofday.Hour.ToString();
                        m = timeofday.Minute.ToString();
                        others.Add(new vOthers_evaluation_assessor
                        {
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.ACA_YEAR].Ordinal]),
                            curri_id = item.ItemArray[data.Columns[Others_evaluation.FieldName.CURRI_ID].Ordinal].ToString(),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.INDICATOR_NUM].Ordinal]),
                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.SUB_INDICATOR_NUM].Ordinal]),
                            evaluation_score = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.EVALUATION_SCORE].Ordinal]),
                            assessor_id = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.ASSESSOR_ID].Ordinal]),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Assessor.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Assessor.FieldName.T_NAME].Ordinal].ToString(),
                            date = Convert.ToDateTime(item.ItemArray[data.Columns[Others_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                            time = (timeofday.Hour > 9 ? "" : "0") + h + '.' + (timeofday.Minute > 9 ? "" : "0") + m,
                            suggestion = item.ItemArray[data.Columns[Others_evaluation.FieldName.DETAIL].Ordinal].ToString(),
                            others_evaluation_id = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.OTHERS_EVALUATION_ID].Ordinal])
                        });
                    }
                    res.Close();
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                }

            }
            catch (Exception ex)
            {
                //Handle error from sql execution
                return ex.Message;
            }
            finally
            {
                //Whether it success or not it must close connection in order to end block
                d.SQLDisconnect();
            }
            return this;
        }

        public object SelectByIndicatorAndCurriculum(oIndicator inddata, string curri_id)
        {
            //System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            d.iCommand.CommandText = string.Format(
                "select self_eval_result.*, {0}, {1} from " +
                "(select * from " +
                "(select * from {2} as s1 where s1.{3} = {4} and s1.{5} = '{6}' and s1.{7} = {8} and " +
                "s1.{9} >= ALL(select {9} from {2} as s2 where s2.{3} = {4} and s2.{10} = s1.{10} and s2.{5} = '{6}' and s2.{7} = {8})) as r1 " +
                "where r1.{11} >= ALL(select r2.{11} from {2} as r2 where r2.{3} = {4} and r2.{10} = r1.{10} and r2.{5} = '{6}' and r2.{7} = {8} and r2.{9}=r1.{9})) as self_eval_result INNER JOIN {12} on self_eval_result.{13} = {12}.{14}",
                Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Self_evaluation.FieldName.TABLE_NAME,
                Self_evaluation.FieldName.INDICATOR_NUM, inddata.indicator_num, Self_evaluation.FieldName.CURRI_ID, curri_id,
                Self_evaluation.FieldName.ACA_YEAR,inddata.aca_year,Self_evaluation.FieldName.DATE,
                Self_evaluation.FieldName.SUB_INDICATOR_NUM,Self_evaluation.FieldName.TIME,
                Teacher.FieldName.TABLE_NAME,Self_evaluation.FieldName.TEACHER_ID,Teacher.FieldName.TEACHER_ID
                );
            try
            {
                //Retrieve self_evaluation data
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        string h, m;
                        DateTime timeofday = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.TIME].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                        h = timeofday.Hour.ToString();
                        m = timeofday.Minute.ToString();
                        self.Add(new vSelf_evaluation_teacher
                        {
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.ACA_YEAR].Ordinal]),
                            curri_id = item.ItemArray[data.Columns[Self_evaluation.FieldName.CURRI_ID].Ordinal].ToString(),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.INDICATOR_NUM].Ordinal]),
                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.SUB_INDICATOR_NUM].Ordinal]),
                            evaluation_score = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.EVALUATION_SCORE].Ordinal]),
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.TEACHER_ID].Ordinal]),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                            date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                            time = (timeofday.Hour > 9 ? "" : "0")+h + '.' + (timeofday.Minute > 9 ? "" : "0") + m
                        });
                    }

                    data.Dispose();
                    //Get another data....
                }
                else
                {
                    res.Close();
                    //Reserved for return error string
                    return "";
                }
                res.Close();
                d.iCommand.CommandText = string.Format(
                "select others_eval_result.*, {0}, {1} from " +
                "(select * from " +
                "(select * from {2} as o1 where o1.{3} = {4} and o1.{5} = '{6}' and o1.{7} = {8} and " +
                "o1.{9} >= ALL(select {9} from {2} as o2 where o2.{3} = {4} and o2.{10} = o1.{10} and o2.{5} = '{6}' and o2.{7} = {8})) as r1 " +
                "where r1.{11} >= ALL(select r2.{11} from {2} as r2 where r2.{3} = {4} and r2.{10} = r1.{10} and r2.{5} = '{6}' and r2.{7} = {8} and r2.{9}=r1.{9})) as others_eval_result INNER JOIN {12} on others_eval_result.{13} = {12}.{14}",
                Assessor.FieldName.T_PRENAME, Assessor.FieldName.T_NAME, Others_evaluation.FieldName.TABLE_NAME,
                Others_evaluation.FieldName.INDICATOR_NUM, inddata.indicator_num, Others_evaluation.FieldName.CURRI_ID, curri_id,
                Others_evaluation.FieldName.ACA_YEAR, inddata.aca_year, Others_evaluation.FieldName.DATE,
                Others_evaluation.FieldName.SUB_INDICATOR_NUM, Others_evaluation.FieldName.TIME,
                Assessor.FieldName.TABLE_NAME, Others_evaluation.FieldName.ASSESSOR_ID, Assessor.FieldName.USERNAME
                );
                //retrieve others_evaluation data
                res = d.iCommand.ExecuteReader();

                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        string h, m;
                        DateTime timeofday = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.TIME].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                        h = timeofday.Hour.ToString();
                        m = timeofday.Minute.ToString();
                        others.Add(new vOthers_evaluation_assessor
                        {
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.ACA_YEAR].Ordinal]),
                            curri_id = item.ItemArray[data.Columns[Others_evaluation.FieldName.CURRI_ID].Ordinal].ToString(),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.INDICATOR_NUM].Ordinal]),
                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.SUB_INDICATOR_NUM].Ordinal]),
                            evaluation_score = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.EVALUATION_SCORE].Ordinal]),
                            assessor_id = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.ASSESSOR_ID].Ordinal]),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Assessor.FieldName.T_PRENAME].Ordinal].ToString()) + item.ItemArray[data.Columns[Assessor.FieldName.T_NAME].Ordinal].ToString(),
                            date = Convert.ToDateTime(item.ItemArray[data.Columns[Others_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                            time = (timeofday.Hour > 9 ? "" : "0") + h + '.' + (timeofday.Minute > 9 ? "" : "0") + m,
                            suggestion = item.ItemArray[data.Columns[Others_evaluation.FieldName.DETAIL].Ordinal].ToString(),
                            others_evaluation_id = Convert.ToInt32(item.ItemArray[data.Columns[Others_evaluation.FieldName.OTHERS_EVALUATION_ID].Ordinal])
                        });
                    }
                    data.Dispose();
                }
                else
                {
                    //Reserved for return error string
                }
                res.Close();
            }
            catch (Exception ex)
            {
                //Handle error from sql execution
                return ex.Message;
            }
            finally
            {
                //Whether it success or not it must close connection in order to end block
                d.SQLDisconnect();
            }
            return this;
        }
    }
}