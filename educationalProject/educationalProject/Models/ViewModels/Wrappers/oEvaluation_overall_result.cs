using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
using educationalProject.Models.Wrappers;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oEvaluation_overall_result : Evaluation_overall_result
    {
        public async Task<object> Select(oCurriculum_academic curriacadata)
        {
            //System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Evaluation_overall_result> result = new List<Evaluation_overall_result>();
            string temp1tablename = "#temp1";
            string createtabletemp1 = string.Format("create table {0}(" +
                                      "[row_num] int identity(1, 1) not null," +
                                      "[assessor_type] int null," +
                                      "[{1}] int null," +
                                      "[{2}] int null," +
                                      "[{3}] date null," +
                                      "[{4}] TIME(0) null," +
                                      "[caption1] varchar(1100) null," +
                                      "[caption2] varchar(1100) null," +
                                      "[{5}] float null," +
                                      "primary key([row_num])) " +

                                      "alter table {0} " +
                                      "alter column[caption1] varchar(1100) collate database_default " +

                                      "alter table {0} " +
                                      "alter column[caption2] varchar(1100) collate database_default ",
                                      temp1tablename, Indicator.FieldName.INDICATOR_NUM,
                                      Sub_indicator.FieldName.SUB_INDICATOR_NUM,
                                      Self_evaluation.FieldName.DATE, Self_evaluation.FieldName.TIME,
                                      Self_evaluation.FieldName.EVALUATION_SCORE);

            string insertintotemp1_1 = string.Format("insert into {0} " +
                                       "select 0 as assess_data_type," +
                                       "{1}.{2},{3},null,null,{4},{5},null from {1}, {6} " +
                                       "where {1}.{7} = (select max({7}) from {1} where {7} <= {8}) " +
                                       "and {1}.{7} = {6}.{9} " +
                                       "and {1}.{2} = {6}.{10} order by {1}.{2} ",
                                       temp1tablename, Indicator.FieldName.TABLE_NAME, Indicator.FieldName.INDICATOR_NUM,
                                       Sub_indicator.FieldName.SUB_INDICATOR_NUM, Indicator.FieldName.INDICATOR_NAME_T,
                                       Sub_indicator.FieldName.SUB_INDICATOR_NAME, Sub_indicator.FieldName.TABLE_NAME,
                                       Indicator.FieldName.ACA_YEAR, curriacadata.aca_year, Sub_indicator.FieldName.ACA_YEAR,
                                       Sub_indicator.FieldName.INDICATOR_NUM);

            string insertintotemp1_2 = string.Format("insert into {0} " +
                                       "select 1 as assessor_type," +
                                       "{1}," +
                                       "null as sub_indicator_num," +
                                       "max({2}) as {2}," +
                                       "max({14}) as {14}," +
                                       "max({3})," +
                                       "max({4})," +
                                       "AVG(cast({5} as int) * 1.0) as {5} " +
                                       "from {6},{7} where {8} = '{9}' and {10} = {11} " +
                                       "and {12} = {13} " +
                                       "group by {1} ",
                                       temp1tablename, Indicator.FieldName.INDICATOR_NUM,
                                       Self_evaluation.FieldName.DATE,
                                       Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                                       Self_evaluation.FieldName.EVALUATION_SCORE,
                                       Self_evaluation.FieldName.TABLE_NAME /****6****/,
                                       User_list.FieldName.TABLE_NAME,
                                       Self_evaluation.FieldName.CURRI_ID, curriacadata.curri_id,
                                       Self_evaluation.FieldName.ACA_YEAR, curriacadata.aca_year,
                                       Self_evaluation.FieldName.TEACHER_ID,
                                       User_list.FieldName.USER_ID, Self_evaluation.FieldName.TIME);

            string insertintotemp1_3 = string.Format("insert into {0} " +
                                       "select 2 as assessor_type," +
                                       "{1}," +
                                       "null as sub_indicator_num," +
                                       "max({2}) as {2}," +
                                       "max({14}) as {14}," +
                                       "max({3})," +
                                       "max({4})," +
                                       "AVG(cast({5} as int) * 1.0) as {5} " +
                                       "from {6},{7} where {8} = '{9}' and {10} = {11} " +
                                       "and {12} = {13} " +
                                       "group by {1} ",
                                       temp1tablename, Indicator.FieldName.INDICATOR_NUM,
                                       Others_evaluation.FieldName.DATE,
                                       Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                                       Others_evaluation.FieldName.EVALUATION_SCORE,
                                       Others_evaluation.FieldName.TABLE_NAME /****6****/,
                                       User_list.FieldName.TABLE_NAME,
                                       Others_evaluation.FieldName.CURRI_ID,curriacadata.curri_id,
                                       Others_evaluation.FieldName.ACA_YEAR,curriacadata.aca_year,
                                       Others_evaluation.FieldName.ASSESSOR_ID,
                                       User_list.FieldName.USER_ID, Others_evaluation.FieldName.TIME);

            string insertintotemp1_4 = string.Format("insert into {0}(assessor_type,{1},{2},{3}) " +
                                       "select 3 as assessor_type," +
                                       "{1}," +
                                       "{2}," +
                                       "{3} " +
                                       "from {4} where {5} = '{6}' and {7} = {8} ",
                                       temp1tablename, Indicator.FieldName.INDICATOR_NUM, Sub_indicator.FieldName.SUB_INDICATOR_NUM,
                                       Self_evaluation.FieldName.EVALUATION_SCORE,
                                       Self_evaluation.FieldName.TABLE_NAME,
                                       Self_evaluation.FieldName.CURRI_ID, curriacadata.curri_id,
                                       Self_evaluation.FieldName.ACA_YEAR, curriacadata.aca_year);

            string insertintotemp1_5 = string.Format("insert into {0}(assessor_type,{1},{2},{3}) " +
                           "select 4 as assessor_type," +
                           "{1}," +
                           "{2}," +
                           "{3} " +
                           "from {4} where {5} = '{6}' and {7} = {8} ",
                           temp1tablename, Indicator.FieldName.INDICATOR_NUM, Sub_indicator.FieldName.SUB_INDICATOR_NUM,
                           Others_evaluation.FieldName.EVALUATION_SCORE,
                           Others_evaluation.FieldName.TABLE_NAME,
                           Others_evaluation.FieldName.CURRI_ID, curriacadata.curri_id,
                           Others_evaluation.FieldName.ACA_YEAR, curriacadata.aca_year);

            string selectcmd = string.Format("select * from {0} ", temp1tablename);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} {6} END", createtabletemp1, insertintotemp1_1, insertintotemp1_2, insertintotemp1_3,
                insertintotemp1_4, insertintotemp1_5, selectcmd);
            try
            {
                //Retrieve self_evaluation data
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        int assess_type = Convert.ToInt32(item.ItemArray[data.Columns["assessor_type"].Ordinal]);

                        //Type 0:Retrieve indicator name/sub indicator name
                        if(assess_type == 0)
                        {
                            int indnum = Convert.ToInt32(item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NUM].Ordinal]);
                            if(result.FirstOrDefault(t => t.indicator_num == indnum) == null)
                            {
                                result.Add(new Evaluation_overall_result
                                {
                                    indicator_num = indnum,
                                    indicator_name = item.ItemArray[data.Columns["caption1"].Ordinal].ToString(),
                                });
                            }
                            result.First(t => t.indicator_num == indnum).sub_indicator_result.Add(new Sub_indicator_result
                            {
                                sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NUM].Ordinal]),
                                sub_indicator_name = item.ItemArray[data.Columns["caption2"].Ordinal].ToString()
                            });
                        }

                        //Type 1:Retrieve overall average result of self evaluation
                        else if (assess_type == 1)
                        {

                            Evaluation_overall_result target = result.First(t => t.indicator_num == Convert.ToInt32(item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NUM].Ordinal]));
                            string h, m;
                            DateTime timeofday = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.TIME].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                            h = timeofday.Hour.ToString();
                            m = timeofday.Minute.ToString();
                            target.self_date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3];
                            target.self_name = NameManager.GatherPreName(item.ItemArray[data.Columns["caption1"].Ordinal].ToString()) + item.ItemArray[data.Columns["caption2"].Ordinal].ToString();
                            target.self_time = (timeofday.Hour > 9 ? "" : "0") + h + '.' + (timeofday.Minute > 9 ? "" : "0") + m;
                            target.indicator_result_self_average = Convert.ToDouble(item.ItemArray[data.Columns[Self_evaluation.FieldName.EVALUATION_SCORE].Ordinal]);
                        }

                        //Type 2:Retrieve overall average result of other evaluation
                        else if (assess_type == 2)
                        {

                            Evaluation_overall_result target = result.First(t => t.indicator_num == Convert.ToInt32(item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NUM].Ordinal]));
                            string h, m;
                            DateTime timeofday = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.TIME].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                            h = timeofday.Hour.ToString();
                            m = timeofday.Minute.ToString();
                            target.other_date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3];
                            target.other_name = NameManager.GatherPreName(item.ItemArray[data.Columns["caption1"].Ordinal].ToString()) + item.ItemArray[data.Columns["caption2"].Ordinal].ToString();
                            target.other_time = (timeofday.Hour > 9 ? "" : "0") + h + '.' + (timeofday.Minute > 9 ? "" : "0") + m;
                            target.indicator_result_other_average = Convert.ToDouble(item.ItemArray[data.Columns[Self_evaluation.FieldName.EVALUATION_SCORE].Ordinal]);
                        }

                        //Type 3:Retrieve overall individual result of self evaluation
                        else if (assess_type == 3)
                        {
                            Sub_indicator_result target = result.First(t => t.indicator_num == Convert.ToInt32(item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NUM].Ordinal])).
                                                            sub_indicator_result.First(t => t.sub_indicator_num == Convert.ToInt32(item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NUM].Ordinal]));
                            if (item.ItemArray[data.Columns[Self_evaluation.FieldName.EVALUATION_SCORE].Ordinal].ToString() != "")
                            {
                                target.sub_indicator_self_result = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.EVALUATION_SCORE].Ordinal]);
                            }
                        }

                        //Type 4:Retrieve overall individual result of other evaluation
                        else
                        {
                            Sub_indicator_result target = result.First(t => t.indicator_num == Convert.ToInt32(item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NUM].Ordinal])).
                                                            sub_indicator_result.First(t => t.sub_indicator_num == Convert.ToInt32(item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NUM].Ordinal]));
                            if (item.ItemArray[data.Columns[Self_evaluation.FieldName.EVALUATION_SCORE].Ordinal].ToString() != "")
                            {
                                target.sub_indicator_other_result = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.EVALUATION_SCORE].Ordinal]);
                            }
                        }
                    }
                    data.Dispose();
                    //Get another data....
                }
                else
                {
                    //Reserved for return error string
                    return "";
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
            return result;
        }


    }
}