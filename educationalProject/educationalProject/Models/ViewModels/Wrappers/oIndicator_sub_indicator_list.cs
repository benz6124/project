using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oIndicator_sub_indicator_list : Indicator_sub_indicator_list
    {
        public object SelectByAcademicYear(int year)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oIndicator_sub_indicator_list> result = new List<oIndicator_sub_indicator_list>();
            d.iCommand.CommandText = string.Format
                ("select {0}.{1},{0}.{2},{3},{4},{5},{6} " +
                 "from {0},{7} "+ 
                 "where {0}.{1} = {8} and {0}.{1} = {7}.{9} and {0}.{2}={7}.{10} "+
                 "order by {7}.{2},{7}.{5}",
                 FieldName.TABLE_NAME,FieldName.ACA_YEAR,FieldName.INDICATOR_NUM,FieldName.INDICATOR_NAME_T,FieldName.INDICATOR_NAME_E,
                 Sub_indicator.FieldName.SUB_INDICATOR_NUM,Sub_indicator.FieldName.SUB_INDICATOR_NAME,
                 Sub_indicator.FieldName.TABLE_NAME,year, Sub_indicator.FieldName.ACA_YEAR, Sub_indicator.FieldName.INDICATOR_NUM);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
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

        public object UpdateEntireList(List<oIndicator_sub_indicator_list> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            string delcmd = string.Format("delete from {0} where {1} = {2}", FieldName.TABLE_NAME, FieldName.ACA_YEAR, list.First().aca_year);
            string insertintoindicatorcmd = string.Format("insert into {0} values ", FieldName.TABLE_NAME);
            string insertintosubindicatorcmd = string.Format("insert into {0} values ", Sub_indicator.FieldName.TABLE_NAME);
            int isFirst = 1;
            foreach (oIndicator_sub_indicator_list item in list)
            {
                insertintoindicatorcmd += string.Format("({0},{1},'{2}','{3}')", item.aca_year, item.indicator_num, item.indicator_name_t, item.indicator_name_e);
                if (item != list.Last()) insertintoindicatorcmd += ",";
                foreach(Sub_indicator sub_item in item.sub_indicator_list)
                {
                    if (isFirst == 0) insertintosubindicatorcmd += ",";
                    else isFirst = 0;
                        insertintosubindicatorcmd += string.Format("({0},{1},{2},'{3}')", sub_item.aca_year, sub_item.indicator_num, sub_item.sub_indicator_num, sub_item.sub_indicator_name);
                }
            }

            d.iCommand.CommandText = string.Format("BEGIN\n{0}\n{1}\n{2}\nEND",delcmd,insertintoindicatorcmd,insertintosubindicatorcmd);
            try
            {
                int rowAffected = d.iCommand.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    return null;
                }
                else
                {
                    return "No indicator-sub indicator data are updated.";
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
        }

        public object UpdateOnlySubIndicatorList(List<Sub_indicator> list)
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
                int rowAffected = d.iCommand.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    return null;
                }
                else
                {
                    return "No sub_indicator data are updated.";
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
        }

        

    }
}