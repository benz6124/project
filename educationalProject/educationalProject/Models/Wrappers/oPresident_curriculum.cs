using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oPresident_curriculum : President_curriculum
    {
        public async Task<object> InsertOrUpdate()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            string deletefromcommittee = string.Format("delete from {0} where {1} = {2} and {3} = {4} and {5} = {6} ",
                Committee.FieldName.TABLE_NAME, FieldName.CURRI_ID, ParameterName.CURRI_ID, FieldName.ACA_YEAR, ParameterName.ACA_YEAR,
                FieldName.TEACHER_ID, ParameterName.TEACHER_ID);
            d.iCommand.CommandText = deletefromcommittee + string.Format("IF NOT EXISTS (select * from {0} where {1} = {2} and {3} = {4}) " +
                                       "BEGIN " +
                                       "INSERT INTO {0} VALUES " +
                                       "({5}, {2},{4}) " +
                                       "END " +
                                       "ELSE " +
                                       "BEGIN " +
                                       "UPDATE {0} SET {6} = {5} where {1} = {2} and {3} = {4} " +
                                       "END",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, ParameterName.CURRI_ID, FieldName.ACA_YEAR, ParameterName.ACA_YEAR, ParameterName.TEACHER_ID, FieldName.TEACHER_ID);

            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.ACA_YEAR, aca_year));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.TEACHER_ID, teacher_id));
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