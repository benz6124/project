using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models;
using educationalProject.Models.Wrappers;
using educationalProject.Utils;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oSelf_evaluation_sub_indicator_name : vSelf_evaluation_sub_indicator_name
    {
        public object SelectByIndicatorAndCurriculum(oIndicator inddata,string curri_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oSelf_evaluation_sub_indicator_name> result = new List<oSelf_evaluation_sub_indicator_name>();

            string temp90tablename = "#temp90";
            string createtabletemp90 = string.Format("CREATE TABLE {0} (" +
                                     "[row_num] INT IDENTITY(1, 1) not null," +
                                     "[{1}] INT NULL," +
                                     "[{2}] INT NULL," +
                                     "[{3}] INT NULL," +
                                     "[{4}] CHAR NULL," +
                                     "[{5}] DATE NULL," +
                                     "[{6}] TIME(0) NULL," +
                                     "[{7}] {10} NULL," +
                                     "[{8}] INT NULL," +
                                     "[{9}] VARCHAR(400) NULL," +
                                     "PRIMARY KEY([row_num])) " +

                                     "alter table {0} " +
                                     "alter column [{4}] CHAR collate database_default " +

                                     "alter table {0} " +
                                     "alter column [{7}] {10} collate database_default " +

                                     "alter table {0} " +
                                     "alter column [{9}] VARCHAR(400) collate database_default ",
                                     temp90tablename, FieldName.INDICATOR_NUM, FieldName.SUB_INDICATOR_NUM,
                                     FieldName.TEACHER_ID, FieldName.EVALUATION_SCORE, FieldName.DATE,
                                     FieldName.TIME, FieldName.CURRI_ID, FieldName.ACA_YEAR,
                                     Sub_indicator.FieldName.SUB_INDICATOR_NAME, DBFieldDataType.CURRI_ID_TYPE);

            string insertintotemp90_1 = string.Format("insert into {0} " +
                                        "select {1}.*, {2} " +
                                        "from {1}, {3} " +
                                        "where {4} = '{5}' and {1}.{6} = {7} " +
                                        "and {1}.{8} = {9} " +

                                        "and {3}.{10} = {1}.{8} " +
                                        "and {3}.{11} = {1}.{12} " +
                                        "and {3}.{13} = (select max({13}) from {3} where {13} <= {7}) ",
                                        temp90tablename, FieldName.TABLE_NAME, Sub_indicator.FieldName.SUB_INDICATOR_NAME,
                                        Sub_indicator.FieldName.TABLE_NAME, FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR,
                                        inddata.aca_year, FieldName.INDICATOR_NUM, inddata.indicator_num,
                                        Sub_indicator.FieldName.INDICATOR_NUM, Sub_indicator.FieldName.SUB_INDICATOR_NUM,
                                        FieldName.SUB_INDICATOR_NUM, Sub_indicator.FieldName.ACA_YEAR);

            string insertintotemp90_2 = string.Format("insert into {0} " +
                                        "select {1},{2},1 as teacher_id,0 as evalscore,null,null,'{3}',{4},{5} " +
                                        "from {6} " +
                                        "where {1} = {7} and {8} = (select max({8}) from {6} where {8} <= {4}) " +

                                        "and not exists (select * from {9} where " +
                                        "{10} = '{3}' and {9}.{11} = {4} " +
                                        "and {9}.{12} = {7} " +
                                        "and {9}.{13} = {6}.{2}) ",
                                        temp90tablename, Sub_indicator.FieldName.INDICATOR_NUM, Sub_indicator.FieldName.SUB_INDICATOR_NUM,
                                        curri_id, inddata.aca_year, Sub_indicator.FieldName.SUB_INDICATOR_NAME,
                                        Sub_indicator.FieldName.TABLE_NAME, inddata.indicator_num,
                                        Sub_indicator.FieldName.ACA_YEAR, FieldName.TABLE_NAME, FieldName.CURRI_ID,
                                        FieldName.ACA_YEAR, FieldName.INDICATOR_NUM,
                                        FieldName.SUB_INDICATOR_NUM);

            string selectcmd = string.Format("select * from {0} order by {1} ", temp90tablename,FieldName.SUB_INDICATOR_NUM);
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END", createtabletemp90, insertintotemp90_1, insertintotemp90_2, selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        string h = "", m = "",readdate;
                        readdate = item.ItemArray[data.Columns[FieldName.DATE].Ordinal].ToString();
                        if (readdate != "")
                        {
                            DateTime timeofday = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.TIME].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                            h = timeofday.Hour.ToString();
                            m = timeofday.Minute.ToString();
                            result.Add(new oSelf_evaluation_sub_indicator_name
                            {
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                                sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
                                evaluation_score = item.ItemArray[data.Columns[FieldName.EVALUATION_SCORE].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVALUATION_SCORE].Ordinal]) : 0,
                                teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString()),
                                sub_indicator_name = item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NAME].Ordinal].ToString(),
                                date = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                                time = (timeofday.Hour > 9 ? "" : "0") + h + '.' + (timeofday.Minute > 9 ? "" : "0") + m
                            });
                        }
                        else
                        {
                            result.Add(new oSelf_evaluation_sub_indicator_name
                            {
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                                sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
                                evaluation_score = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVALUATION_SCORE].Ordinal]),
                                teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString()),
                                sub_indicator_name = item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NAME].Ordinal].ToString()
                            });
                        }
                    }
                    data.Dispose();
                }
                else
                {
                    
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