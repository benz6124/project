using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oStudent_cnt : Student_cnt
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oStudent_cnt> result = new List<oStudent_cnt>();
            d.iCommand.CommandText = String.Format("select * from {0}", FieldName.TABLE_NAME);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oStudent_cnt
                        {
                            ny1 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY1].Ordinal]),
                            ny2 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY2].Ordinal]),
                            ny3 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY3].Ordinal]),
                            ny4 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY4].Ordinal]),
                            ny5 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY5].Ordinal]),
                            ny6 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY6].Ordinal]),
                            ny7 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY7].Ordinal]),
                            ny8 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY8].Ordinal]),
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.YEAR].Ordinal])
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
            List<oStudent_cnt> result = new List<oStudent_cnt>();
            d.iCommand.CommandText = String.Format("select * from {0} where {1}", FieldName.TABLE_NAME, wherecond);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oStudent_cnt
                        {
                            ny1 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY1].Ordinal]),
                            ny2 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY2].Ordinal]),
                            ny3 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY3].Ordinal]),
                            ny4 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY4].Ordinal]),
                            ny5 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY5].Ordinal]),
                            ny6 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY6].Ordinal]),
                            ny7 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY7].Ordinal]),
                            ny8 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.NY8].Ordinal]),
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.YEAR].Ordinal])
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