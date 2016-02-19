using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oSection_save : Section_save
    {

        public async Task<object> SelectWhere()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            d.iCommand.CommandText = string.Format("select * from {0} where {1} = {2} and {3} = {4} and {5} = {6} and {7} = {8}", FieldName.TABLE_NAME, 
               FieldName.INDICATOR_NUM,ParameterName.INDICATOR_NUM,FieldName.SUB_INDICATOR_NUM,ParameterName.SUB_INDICATOR_NUM,
               FieldName.ACA_YEAR,ParameterName.ACA_YEAR,FieldName.CURRI_ID,ParameterName.CURRI_ID);

            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.INDICATOR_NUM, indicator_num));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.SUB_INDICATOR_NUM, sub_indicator_num));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.ACA_YEAR, aca_year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        string h, m;
                        DateTime timeofday = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.TIME].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                        h = timeofday.Hour.ToString();
                        m = timeofday.Minute.ToString();
                        curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString();
                        aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]);
                        detail = item.ItemArray[data.Columns[FieldName.DETAIL].Ordinal].ToString();
                        indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]);
                        sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]);
                        teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal]) : 0;
                        date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3];
                        time = (timeofday.Hour > 9 ? "" : "0") + h + '.' + (timeofday.Minute > 9 ? "" : "0") + m;
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
            return null;
        }

        public async Task<object> InsertOrUpdate()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            d.iCommand.CommandText = string.Format("if not exists(select * from {0} where {9} = {2} and {10} = {3} and {11} = {1} and {12} = {8}) " +
                "insert into {0} values ({1},{2},{3},{4},{5},{6},{7},{8}) " +
                "else " +
                "update {0} set {13}={7},{14}={6},{15}={4}, {16}={5} where {9} = {2} and {10} = {3} and {11} = {1} and {12} = {8} ",
                FieldName.TABLE_NAME, ParameterName.ACA_YEAR, ParameterName.INDICATOR_NUM, ParameterName.SUB_INDICATOR_NUM,
                ParameterName.TEACHER_ID, ParameterName.DETAIL, ParameterName.DATE, ParameterName.TIME, ParameterName.CURRI_ID,
                FieldName.INDICATOR_NUM,FieldName.SUB_INDICATOR_NUM,FieldName.ACA_YEAR,FieldName.CURRI_ID,
                FieldName.TIME, FieldName.DATE,FieldName.TEACHER_ID, FieldName.DETAIL
                );

            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.ACA_YEAR, aca_year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.INDICATOR_NUM, indicator_num));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.SUB_INDICATOR_NUM, sub_indicator_num));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.TEACHER_ID, teacher_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DETAIL, detail));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DATE, date));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.TIME, time));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
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