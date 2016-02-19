using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oIndicator : Indicator
    {
        public async Task<object> SelectWhereOrderByWithKeepYearSource(string wherecond, string orderbycol, int? dir,int source_year)
        {
            string[] direction = { "ASC", "DESC" };
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oIndicator> result = new List<oIndicator>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1} order by {2} {3}",
                FieldName.TABLE_NAME, wherecond, orderbycol, ((dir != null) ? direction[dir.Value] : ""));
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oIndicator
                        {
                            aca_year = source_year,
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                            indicator_name_t = item.ItemArray[data.Columns[FieldName.INDICATOR_NAME_T].Ordinal].ToString(),
                            indicator_name_e = item.ItemArray[data.Columns[FieldName.INDICATOR_NAME_E].Ordinal].ToString()
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
            return result;
        }

        public async Task<object> SelectDistinctIndicatorYear()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<int> result = new List<int>();
            d.iCommand.CommandText = string.Format("select distinct aca_year from {0}", FieldName.TABLE_NAME);
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add( Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]));
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
            return result;
        }

        public async Task<object> Delete(string wherecond)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            //Delete from indicator will result in cascade delete in sub_indicator table
            d.iCommand.CommandText = string.Format("delete from {0} where {1}", FieldName.TABLE_NAME, wherecond);
            try
            {
                await d.iCommand.ExecuteNonQueryAsync();
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
    }
}