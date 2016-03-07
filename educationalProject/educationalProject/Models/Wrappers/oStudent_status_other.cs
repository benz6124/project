﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oStudent_status_other : Student_status_other
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<oStudent_status_other> result = new List<oStudent_status_other>();
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
                        result.Add(new oStudent_status_other
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            grad_in_time = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GRAD_IN_TIME].Ordinal]),
                            grad_over_time = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GRAD_OVER_TIME].Ordinal]),
                            move_in = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.MOVE_IN].Ordinal]),
                            quity1 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUITY1].Ordinal]),
                            quity2 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUITY2].Ordinal]),
                            quity3 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUITY3].Ordinal]),
                            quity4 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUITY4].Ordinal]),
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

        public async Task<object> SelectWhereByCurriculumAcademic()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<oStudent_status_other> result = new List<oStudent_status_other>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2} and {3} = {4}", FieldName.TABLE_NAME,
                FieldName.CURRI_ID, ParameterName.CURRI_ID, FieldName.YEAR, ParameterName.YEAR);
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.YEAR, year));
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);

                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new oStudent_status_other
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            grad_in_time = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GRAD_IN_TIME].Ordinal]),
                            grad_over_time = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.GRAD_OVER_TIME].Ordinal]),
                            move_in = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.MOVE_IN].Ordinal]),
                            quity1 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUITY1].Ordinal]),
                            quity2 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUITY2].Ordinal]),
                            quity3 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUITY3].Ordinal]),
                            quity4 = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.QUITY4].Ordinal]),
                            year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.YEAR].Ordinal])
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

        public async Task<object> InsertOrUpdate()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;

            d.iCommand.CommandText = string.Format("IF NOT EXISTS (select * from {0} where {1}={2} and {3} = {4}) "+
                                       "BEGIN "+
                                       "INSERT INTO {0} VALUES " +
                                       "({2}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}) " +
                                       "END " +
                                       "ELSE " +
                                       "BEGIN " +
                                       "UPDATE {0} SET {12} = {5},{13} = {6},{14} = {7},{15} = {8},{16} = {9},{17} = {10},{18} = {11} where {1} = {2} and {3} = {4} " +
                                       "END",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, ParameterName.CURRI_ID, FieldName.YEAR, ParameterName.YEAR,
                ParameterName.GRAD_IN_TIME, ParameterName.GRAD_OVER_TIME, ParameterName.QUITY1, ParameterName.QUITY2,
                ParameterName.QUITY3, ParameterName.QUITY4, ParameterName.MOVE_IN,
                    FieldName.GRAD_IN_TIME,FieldName.GRAD_OVER_TIME,FieldName.QUITY1, FieldName.QUITY2, FieldName.QUITY3, FieldName.QUITY4, FieldName.MOVE_IN);
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.YEAR, year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.GRAD_IN_TIME, grad_in_time));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.GRAD_OVER_TIME, grad_over_time));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.QUITY1, quity1));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.QUITY2, quity2));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.QUITY3, quity3));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.QUITY4, quity4));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.MOVE_IN, move_in));
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