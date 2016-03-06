using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oIndicator_sub_indicator_list : Indicator_sub_indicator_list
    {
        public async Task<object> SelectByAcademicYear(int year)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oIndicator_sub_indicator_list> result = new List<oIndicator_sub_indicator_list>();
            string temp1tablename = "#temp1";
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[{1}] INT NOT NULL," +
                                      "[{2}] INT NOT NULL," +
                                      "[{3}] INT NOT NULL," +
                                      "[{4}] VARCHAR(2000) NULL," +
                                      "PRIMARY KEY([{1}],[{2}],[{3}])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {4} VARCHAR(2000) COLLATE DATABASE_DEFAULT ",
                                      temp1tablename, FieldName.ACA_YEAR, FieldName.INDICATOR_NUM,
                                      Sub_indicator.FieldName.SUB_INDICATOR_NUM,
                                      Sub_indicator.FieldName.SUB_INDICATOR_NAME);
            string insertintotemp1 = string.Format("insert into {0} " +
                                     "select {1}, {2}, -1, null from {3} where not exists " +
                                     "(select * from {4} where {3}.{1} = {4}.{5} and {3}.{2} = {4}.{6}) " +
                                     "insert into {0} " +
                                     "select * from {4} where {5} = {7} ",
                                     temp1tablename, FieldName.ACA_YEAR, FieldName.INDICATOR_NUM, FieldName.TABLE_NAME,
                                     Sub_indicator.FieldName.TABLE_NAME, Sub_indicator.FieldName.ACA_YEAR,
                                     Sub_indicator.FieldName.INDICATOR_NUM,year);
            string selectcmd = string.Format
                ("select {0}.{1},{0}.{2},{3},{4},{5},{6} " +
                 "from {0},{7} " +
                 "where {0}.{1} = {8} and {0}.{1} = {7}.{9} and {0}.{2}={7}.{10} " +
                 "order by {7}.{2},{7}.{5}",
                 FieldName.TABLE_NAME, FieldName.ACA_YEAR, FieldName.INDICATOR_NUM, FieldName.INDICATOR_NAME_T, FieldName.INDICATOR_NAME_E,
                 Sub_indicator.FieldName.SUB_INDICATOR_NUM, Sub_indicator.FieldName.SUB_INDICATOR_NAME,
                 temp1tablename, year, Sub_indicator.FieldName.ACA_YEAR, Sub_indicator.FieldName.INDICATOR_NUM);
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} END",createtabletemp1,insertintotemp1,selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    oIndicator_sub_indicator_list curr = null;
                    indicator_num = -1;
                    foreach (DataRow item in data.Rows)
                    {
                        if(indicator_num != Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]))
                        {
                            curr = new oIndicator_sub_indicator_list
                            {
                                aca_year = aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                                indicator_name_t = item.ItemArray[data.Columns[FieldName.INDICATOR_NAME_T].Ordinal].ToString(),
                                indicator_name_e = item.ItemArray[data.Columns[FieldName.INDICATOR_NAME_E].Ordinal].ToString(),
                                sub_indicator_list = new List<Sub_indicator>()
                            };
                            result.Add(curr);
                            indicator_num = curr.indicator_num;
                        }
                        if(Convert.ToInt32(item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NUM].Ordinal]) != -1)
                        curr.sub_indicator_list.Add(new Sub_indicator
                        {
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[Sub_indicator.FieldName.ACA_YEAR].Ordinal]),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Sub_indicator.FieldName.INDICATOR_NUM].Ordinal]),
                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NUM].Ordinal]),
                            sub_indicator_name = item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NAME].Ordinal].ToString()
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
            return result;
        }

        public async Task<object> UpdateEntireList(List<oIndicator_sub_indicator_list> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            string delcmd = string.Format("delete from {0} where {1} = {2}", FieldName.TABLE_NAME, FieldName.ACA_YEAR, list.First().aca_year);
            string insertintoindicatorcmd = string.Format("insert into {0} values ", FieldName.TABLE_NAME);
            string insertintosubindicatorcmd = "";
            int isFirst = 1;
            foreach (oIndicator_sub_indicator_list item in list)
            {
                insertintoindicatorcmd += string.Format("({0},{1},'{2}','{3}')", item.aca_year, item.indicator_num, item.indicator_name_t, item.indicator_name_e);
                if (item != list.Last()) insertintoindicatorcmd += ",";
                foreach(Sub_indicator sub_item in item.sub_indicator_list)
                {
                    if (isFirst == 0) insertintosubindicatorcmd += ",";
                    else {
                        isFirst = 0;
                        insertintosubindicatorcmd += string.Format("insert into {0} values ", Sub_indicator.FieldName.TABLE_NAME);
                    }
                        insertintosubindicatorcmd += string.Format("({0},{1},{2},'{3}')", sub_item.aca_year, item.indicator_num, sub_item.sub_indicator_num, sub_item.sub_indicator_name);
                }
            }

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} END",delcmd,insertintoindicatorcmd,insertintosubindicatorcmd);
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

        public async Task<object> UpdateOnlySubIndicatorList(List<Sub_indicator> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            string delcmd = string.Format("delete from {0} where {1} = {2} and {3} = {4}", Sub_indicator.FieldName.TABLE_NAME, FieldName.ACA_YEAR, list.First().aca_year,Sub_indicator.FieldName.INDICATOR_NUM,list.First().indicator_num);
            string insertintosubindicatorcmd = string.Format("insert into {0} values ", Sub_indicator.FieldName.TABLE_NAME);

            foreach (Sub_indicator item in list)
            {
                insertintosubindicatorcmd += string.Format("({0},{1},{2},'{3}')", item.aca_year, item.indicator_num, item.sub_indicator_num, item.sub_indicator_name);
                if (item != list.Last()) insertintosubindicatorcmd += ",";
            }

            d.iCommand.CommandText = string.Format("BEGIN\n{0}\n{1}\nEND", delcmd, insertintosubindicatorcmd);
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

    }
}