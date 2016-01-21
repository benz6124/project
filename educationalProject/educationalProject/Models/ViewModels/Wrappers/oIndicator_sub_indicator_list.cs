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
    }
}