using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oPresident_curriculum : President_curriculum
    {
        public object InsertOrUpdate()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            d.iCommand.CommandText = string.Format("IF NOT EXISTS (select * from {0} where {1} = '{2}' and {3} = {4}) " +
                                       "BEGIN " +
                                       "INSERT INTO {0} VALUES " +
                                       "({5}, '{2}',{4}) " +
                                       "END " +
                                       "ELSE " +
                                       "BEGIN " +
                                       "UPDATE {0} SET {6} = {5} where {1} = '{2}' and {3} = {4} " +
                                       "END",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, aca_year, teacher_id, FieldName.TEACHER_ID);
            try
            {
                int rowAffected = d.iCommand.ExecuteNonQuery();
                if (rowAffected == 1)
                {
                    return null;
                }
                else
                {
                    return "No president_curriculum are inserted or updated.";
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