using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oStudent_count : Student_count
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oStudent_count> result = new List<oStudent_count>();
            d.iCommand.CommandText = string.Format("select * from {0}", FieldName.TABLE_NAME);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oStudent_count
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
            List<oStudent_count> result = new List<oStudent_count>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1}", FieldName.TABLE_NAME, wherecond);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oStudent_count
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

        public object InsertOrUpdate()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            d.iCommand.CommandText = string.Format("IF NOT EXISTS (select * from {0} where {1}='{2}' and {3} = {4}) " +
                                       "BEGIN " +
                                       "INSERT INTO {0} VALUES " +
                                       "('{2}', {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11},{12}) " +
                                       "END " +
                                       "ELSE " +
                                       "BEGIN " +
                                       "UPDATE {0} SET {13} = {5},{14} = {6},{15} = {7},{16} = {8},{17} = {9},{18} = {10},{19} = {11},{20} = {12} where {1} = '{2}' and {3} = {4} " +
                                       "END",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, curri_id, FieldName.YEAR, year, ny1, ny2, ny3, ny4, ny5, ny6, ny7,ny8,
                    FieldName.NY1, FieldName.NY2, FieldName.NY3, FieldName.NY4, FieldName.NY5, FieldName.NY6, FieldName.NY7,FieldName.NY8);
            try
            {
                int rowAffected = d.iCommand.ExecuteNonQuery();
                if (rowAffected == 1)
                {
                    return null;
                }
                else
                {
                    return "No student_count are inserted or updated.";
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