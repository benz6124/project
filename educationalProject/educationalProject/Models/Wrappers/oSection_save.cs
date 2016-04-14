using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oSection_save : Section_save
    {

        public async Task<object> SelectWhere()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2} and {3} = {4} and {5} = {6} and {7} = {8}", FieldName.TABLE_NAME, 
               FieldName.INDICATOR_NUM,ParameterName.INDICATOR_NUM,FieldName.SUB_INDICATOR_NUM,ParameterName.SUB_INDICATOR_NUM,
               FieldName.ACA_YEAR,ParameterName.ACA_YEAR,FieldName.CURRI_ID,ParameterName.CURRI_ID);

            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.INDICATOR_NUM, indicator_num));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.SUB_INDICATOR_NUM, sub_indicator_num));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.ACA_YEAR, aca_year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
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
                        curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString();
                        aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]);
                        detail = item.ItemArray[data.Columns[FieldName.DETAIL].Ordinal].ToString();
                        strength = item.ItemArray[data.Columns[FieldName.STRENGTH].Ordinal].ToString() != "" ? item.ItemArray[data.Columns[FieldName.STRENGTH].Ordinal].ToString() : null;
                        weakness = item.ItemArray[data.Columns[FieldName.WEAKNESS].Ordinal].ToString() != "" ? item.ItemArray[data.Columns[FieldName.WEAKNESS].Ordinal].ToString() : null;
                        improve = item.ItemArray[data.Columns[FieldName.IMPROVE].Ordinal].ToString() != "" ? item.ItemArray[data.Columns[FieldName.IMPROVE].Ordinal].ToString() : null;
                        indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]);
                        sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]);
                        teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal]) : 0;
                        date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3];
                        time = (timeofday.Hour > 9 ? "" : "0") + h + '.' + (timeofday.Minute > 9 ? "" : "0") + m;
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
            return null;
        }

        public async Task<object> InsertOrUpdate()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            d.iCommand.CommandText = string.Format("if not exists(select * from {0} where {9} = {2} and {10} = {3} and {11} = {1} and {12} = {8}) " +
                "insert into {0} values ({1},{2},{3},{4},{5},{17},{18},{19},{6},{7},{8}) " +
                "else " +
                "update {0} set {13}={7},{14}={6},{15}={4}, {16}={5},{20} = {17},{21} = {18},{22} = {19} where {9} = {2} and {10} = {3} and {11} = {1} and {12} = {8} ",
                FieldName.TABLE_NAME, ParameterName.ACA_YEAR, ParameterName.INDICATOR_NUM, ParameterName.SUB_INDICATOR_NUM,
                ParameterName.TEACHER_ID, ParameterName.DETAIL, ParameterName.DATE, ParameterName.TIME, ParameterName.CURRI_ID,
                FieldName.INDICATOR_NUM,FieldName.SUB_INDICATOR_NUM,FieldName.ACA_YEAR,FieldName.CURRI_ID,
                FieldName.TIME, FieldName.DATE,FieldName.TEACHER_ID, FieldName.DETAIL,
                /*17*/ParameterName.STRENGTH, ParameterName.WEAKNESS, ParameterName.IMPROVE,
                /*20*/FieldName.STRENGTH, FieldName.WEAKNESS, FieldName.IMPROVE
                );

            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.ACA_YEAR, aca_year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.INDICATOR_NUM, indicator_num));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.SUB_INDICATOR_NUM, sub_indicator_num));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.TEACHER_ID, teacher_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DETAIL, detail));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.STRENGTH, strength));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.WEAKNESS, weakness));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.IMPROVE, improve));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DATE, date));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.TIME, time));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));

            try
            {
                await d.iCommand.ExecuteNonQueryAsync();
                return null;
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
        }

        public async Task<object> getHtmlSectionSave()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            d.iCommand.CommandText = "select detail from section_save where curri_id = '21' and aca_year = 2558 and indicator_num = 1 and sub_indicator_num = 1";
            object res = await d.iCommand.ExecuteScalarAsync();
            d.SQLDisconnect();
            return res;
        }

        public async Task<object> getSectionSaveDataForSAR()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            SAR result = new SAR();
            string selectindicator = string.Format("select {0}, {1}, {2} " +
                                     "from {3} " +
                                     "where {4} = (select max({4}) from {3} where {4} <= {5}) order by {0} ",
                                     Indicator.FieldName.INDICATOR_NUM, Indicator.FieldName.INDICATOR_NAME_T, Indicator.FieldName.INDICATOR_NAME_E,
                                     Indicator.FieldName.TABLE_NAME,
                                     Indicator.FieldName.ACA_YEAR, aca_year);

            string selectsubindicator = string.Format("select {0},{1},{2} " +
                                        "from {3} " +
                                        "where {4} = (select max({4}) from {3} where {4} <= {5}) order by {0},{1} ",
                                        Sub_indicator.FieldName.INDICATOR_NUM, Sub_indicator.FieldName.SUB_INDICATOR_NUM, Sub_indicator.FieldName.SUB_INDICATOR_NAME,
                                        Sub_indicator.FieldName.TABLE_NAME, Sub_indicator.FieldName.ACA_YEAR, aca_year);

            string selectsectionsave = string.Format("select {0},{1},{2},{3},{4},{5} " +
                                       "from {6} " +
                                       "where {7} = '{8}' and {9} = {10} order by {0},{1} ",
                                       FieldName.INDICATOR_NUM, FieldName.SUB_INDICATOR_NUM, FieldName.DETAIL, FieldName.STRENGTH, FieldName.IMPROVE, FieldName.WEAKNESS,
                                       FieldName.TABLE_NAME, FieldName.CURRI_ID, curri_id,
                                       FieldName.ACA_YEAR, aca_year);

            string selectevidence = string.Format("select {0},{1},{2} " +
                                    "from {3} " +
                                    "where {4} = '{5}' and {6} = {7} order by {0},{1} ",
                                    Evidence.FieldName.INDICATOR_NUM, Evidence.FieldName.EVIDENCE_REAL_CODE, Evidence.FieldName.EVIDENCE_NAME,
                                    Evidence.FieldName.TABLE_NAME, Evidence.FieldName.CURRI_ID, curri_id,
                                    Evidence.FieldName.ACA_YEAR, aca_year);

            string selectselfevaluation = string.Format("select {0},{1},{2} " +
                                          "from {3} " +
                                          "where {4} = '{5}' and {6} = {7} order by {0},{1} ",
                                          Self_evaluation.FieldName.INDICATOR_NUM, Self_evaluation.FieldName.SUB_INDICATOR_NUM,
                                          Self_evaluation.FieldName.EVALUATION_SCORE, Self_evaluation.FieldName.TABLE_NAME,
                                          Self_evaluation.FieldName.CURRI_ID, curri_id, Self_evaluation.FieldName.ACA_YEAR, aca_year);


            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} END", selectindicator, selectsubindicator, selectsectionsave, selectevidence, selectselfevaluation);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                do
                {
                    if (res.HasRows)
                    {
                        DataTable data = new DataTable();
                        data.Load(res);

                        //Case current resultset is indicator table
                        if (data.Columns.Contains("indicator_name_t"))
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                string indicator_namet = item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NAME_T].Ordinal].ToString();

                                //Use thai indicator name if it exists
                                if (indicator_namet != "")
                                {
                                    result.indicator_section_save_list.Add(new Indicator_with_section_save_list
                                    {
                                        indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NUM].Ordinal]),
                                        indicator_name = item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NAME_T].Ordinal].ToString()
                                    });
                                }

                                //Otherwise use engish normally
                                else
                                {
                                    result.indicator_section_save_list.Add(new Indicator_with_section_save_list
                                    {
                                        indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NUM].Ordinal]),
                                        indicator_name = item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NAME_E].Ordinal].ToString()
                                    });
                                }

                                result.indicator_self_evaluation_list.Add(new Indicator_with_self_evaluation_tiny_obj_list
                                {
                                    indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Indicator.FieldName.INDICATOR_NUM].Ordinal])
                                });

                                //Force to add self_evaluation with sub_indicator_num = 0 (overall result for each indicator)
                                result.indicator_self_evaluation_list.Last().self_evaluation_list.Add(new Self_evaluation_tiny_detail
                                {
                                    sub_indicator_num = 0,
                                    evaluation_score = 0 //Default score
                                });
                            }
                        }

                        //Case current resultset is sub_indicator table
                        else if (data.Columns.Contains("sub_indicator_name"))
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                int indnum = Convert.ToInt32(item.ItemArray[data.Columns[Sub_indicator.FieldName.INDICATOR_NUM].Ordinal]);
                                result.indicator_section_save_list.First(t => t.indicator_num == indnum).section_save_list.Add(new Section_save_with_sub_indicator_detail
                                {
                                    detail = "--ไม่พบข้อมูล--",
                                    strength = "--ไม่พบข้อมูล--",
                                    weakness = "--ไม่พบข้อมูล--",
                                    improve = "--ไม่พบข้อมูล--",
                                    sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NUM].Ordinal]),
                                    indicator_num = indnum,
                                    sub_indicator_name = item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NAME].Ordinal].ToString()
                                });

                                //Force to add self_evaluation with sub_indicator_num equal to => current read value
                                result.indicator_self_evaluation_list.First(t => t.indicator_num == indnum).self_evaluation_list.Add(new Self_evaluation_tiny_detail
                                {
                                    sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NUM].Ordinal]),
                                    evaluation_score = 0 //Default score
                                });

                            }
                        }

                        //Case current resultset is section_save table
                        else if (data.Columns.Contains("detail"))
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                int indnum = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]);
                                int subindnum = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]);
                                Section_save_with_sub_indicator_detail target = result.indicator_section_save_list.First(t => t.indicator_num == indnum).
                                    section_save_list.First(u => u.sub_indicator_num == subindnum);

                                string readdetail = item.ItemArray[data.Columns[FieldName.DETAIL].Ordinal].ToString();
                                if (readdetail != "")
                                {
                                    target.detail = readdetail;
                                }

                                string readstrength = item.ItemArray[data.Columns[FieldName.STRENGTH].Ordinal].ToString();
                                if (readstrength != "")
                                {
                                    target.strength = readstrength;
                                }

                                string readweak = item.ItemArray[data.Columns[FieldName.WEAKNESS].Ordinal].ToString();
                                if (readweak != "")
                                {
                                    target.weakness = readweak;
                                }

                                string readimprove = item.ItemArray[data.Columns[FieldName.IMPROVE].Ordinal].ToString();
                                if (readimprove != "")
                                {
                                    target.improve = readimprove;
                                }
                            }
                        }
                        //Case current resultset is evidence table
                        else if (data.Columns.Contains(Evidence.FieldName.EVIDENCE_NAME))
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                int indnum = Convert.ToInt32(item.ItemArray[data.Columns[Evidence.FieldName.INDICATOR_NUM].Ordinal]);
                                result.indicator_section_save_list.First(t => t.indicator_num == indnum).evidence_list.Add(new Evidence_detail_for_SAR
                                {
                                    indicator_num = indnum.ToString(),
                                    evidence_real_code = item.ItemArray[data.Columns[Evidence.FieldName.EVIDENCE_REAL_CODE].Ordinal].ToString(),
                                    evidence_name = item.ItemArray[data.Columns[Evidence.FieldName.EVIDENCE_NAME].Ordinal].ToString()
                                });
                            }
                        }

                        //Case current resultset is self_evaluation
                        else
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                int indnum = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.INDICATOR_NUM].Ordinal]);
                                int subindnum = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.SUB_INDICATOR_NUM].Ordinal]);
                                result.indicator_self_evaluation_list.First(t => t.indicator_num == indnum).
                                    self_evaluation_list.First(u => u.sub_indicator_num == subindnum).
                                    evaluation_score = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.EVALUATION_SCORE].Ordinal]);
                            }
                        }
                        data.Dispose();
                    }
                    else if (!res.IsClosed)
                    {
                        if (!res.NextResult())
                            break;
                    }
                } while (!res.IsClosed);
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