using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oStudent_count : Student_count
    {
        public async Task<object> SelectWhereByCurriculumAcademic()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            oStudent_count result = new oStudent_count();
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2} and {3} = {4}", FieldName.TABLE_NAME,
                FieldName.CURRI_ID,ParameterName.CURRI_ID,FieldName.YEAR,ParameterName.YEAR);
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
                    result.ny1 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.NY1].Ordinal]);
                    result.ny2 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.NY2].Ordinal]);
                    result.ny3 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.NY3].Ordinal]);
                    result.ny4 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.NY4].Ordinal]);
                    result.ny5 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.NY5].Ordinal]);
                    result.ny6 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.NY6].Ordinal]);
                    result.ny7 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.NY7].Ordinal]);
                    result.ny8 = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.NY8].Ordinal]);
                    result.curri_id = data.Rows[0].ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString();
                    result.year = Convert.ToInt32(data.Rows[0].ItemArray[data.Columns[FieldName.YEAR].Ordinal]);
                    data.Dispose();
                }
                else //if no row return => set default student stat result to all zeros
                {
                    result.ny1 = 0;
                    result.ny2 = 0;
                    result.ny3 = 0;
                    result.ny4 = 0;
                    result.ny5 = 0;
                    result.ny6 = 0;
                    result.ny7 = 0;
                    result.ny8 = 0;
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

            d.iCommand.CommandText = string.Format("IF NOT EXISTS (select * from {0} where {1}={2} and {3} = {4}) " +
                                       "BEGIN " +
                                       "INSERT INTO {0} VALUES " +
                                       "({2}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11},{12}) " +
                                       "END " +
                                       "ELSE " +
                                       "BEGIN " +
                                       "UPDATE {0} SET {13} = {5},{14} = {6},{15} = {7},{16} = {8},{17} = {9},{18} = {10},{19} = {11},{20} = {12} where {1} = {2} and {3} = {4} " +
                                       "END",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, ParameterName.CURRI_ID, FieldName.YEAR, ParameterName.YEAR, ParameterName.NY1, ParameterName.NY2, ParameterName.NY3, ParameterName.NY4,
                ParameterName.NY5, ParameterName.NY6, ParameterName.NY7, ParameterName.NY8,
                    FieldName.NY1, FieldName.NY2, FieldName.NY3, FieldName.NY4, FieldName.NY5, FieldName.NY6, FieldName.NY7,FieldName.NY8);

            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.YEAR, year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NY1, ny1));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NY2, ny2));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NY3, ny3));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NY4, ny4));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NY5, ny5));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NY6, ny6));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NY7, ny7));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.NY8, ny8));
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