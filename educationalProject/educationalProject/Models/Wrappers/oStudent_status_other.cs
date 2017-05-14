using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oStudent_status_other : Student_status_other
    {
        public async Task<object> SelectWhereByCurriculumAcademic()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            oStudent_status_other result = new oStudent_status_other();
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
                    //Set result to desired property
                    result.grad_in_time = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.GRAD_IN_TIME].Ordinal]);
                    result.grad_over_time = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.GRAD_OVER_TIME].Ordinal]);
                    result.move_in = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.MOVE_IN].Ordinal]);
                    result.quity1 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.QUITY1].Ordinal]);
                    result.quity2 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.QUITY2].Ordinal]);
                    result.quity3 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.QUITY3].Ordinal]);
                    result.quity4 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.QUITY4].Ordinal]);
                    result.curri_id = data.Rows[0].ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString();
                    result.year = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.YEAR].Ordinal]);
                    data.Dispose();
                }
                else //if no row return => set default student stat other result to all zeros
                {
                    result.grad_in_time = 0;
                    result.grad_over_time = 0;
                    result.move_in = 0;
                    result.quity1 = 0;
                    result.quity2 = 0;
                    result.quity3 = 0;
                    result.quity4 = 0;
                    result.curri_id = curri_id;
                    result.year = year;
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