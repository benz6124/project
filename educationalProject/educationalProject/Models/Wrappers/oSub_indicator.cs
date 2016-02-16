using System;
using System.Collections.Generic;
using System.Linq;
using educationalProject.Utils;
using System.Data;
namespace educationalProject.Models.Wrappers
{
    public class oSub_indicator : Sub_indicator
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oSub_indicator> result = new List<oSub_indicator>();
            d.iCommand.CommandText = string.Format("select * from {0}",FieldName.TABLE_NAME);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oSub_indicator
                        {
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
                            sub_indicator_name = item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NAME].Ordinal].ToString()
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

        public object SelectWhere(string wherecond)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oSub_indicator> result = new List<oSub_indicator>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1}",FieldName.TABLE_NAME,wherecond);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oSub_indicator
                        {
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
                            sub_indicator_name = item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NAME].Ordinal].ToString()
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
        public object SelectByIndicatorWithKeepAcaYearSource(oIndicator inddata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oSub_indicator> result = new List<oSub_indicator>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = (select max(j.{1}) from {0} as j where j.{1} <= {2}) and {3} = {4}", 
                FieldName.TABLE_NAME, FieldName.ACA_YEAR,inddata.aca_year,FieldName.INDICATOR_NUM,inddata.indicator_num);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oSub_indicator
                        {
                            aca_year = inddata.aca_year,
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
                            sub_indicator_name = item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NAME].Ordinal].ToString()
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
        public object Delete(string wherecond)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            //Delete from indicator will result in cascade delete in sub_indicator table
            d.iCommand.CommandText = string.Format("delete from {0} where {1}", FieldName.TABLE_NAME, wherecond);
            try
            {
                d.iCommand.ExecuteNonQuery();
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