using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using educationalProject.Utils;
using System.Threading.Tasks;
namespace educationalProject.Models.Wrappers
{
    public class oAun_book : Aun_book
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            List<oAun_book> result = new List<oAun_book>();
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
                        result.Add(new oAun_book
                        {
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                            personnel_id = item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal]) : 0,
                            date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3]
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
        public async Task<object> SelectFileDownloadLink()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            d.iCommand.CommandText = string.Format("select {0} from {1} where {2} = {3} and {4} = {5}", 
                                     FieldName.FILE_NAME,FieldName.TABLE_NAME,
                                     FieldName.CURRI_ID,ParameterName.CURRI_ID,
                                     FieldName.ACA_YEAR,ParameterName.ACA_YEAR);
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.ACA_YEAR, aca_year));
            try
            {
                object result = await d.iCommand.ExecuteScalarAsync();
                if (result != null)
                {
                    file_name = result.ToString();
                    return null;
                }
                else
                {
                    //Reserved for return error string
                    return "notfound";
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
        public async Task<object> InsertOrUpdate()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;

            d.iCommand.CommandText = string.Format("IF NOT EXISTS (select * from {0} where {1} = {2} and {3} = {4}) " +
                                       "BEGIN " +
                                       "INSERT INTO {0} VALUES " +
                                       "({2}, {4}, {5}, {6}, {7}) " +
                                       "END " +
                                       "ELSE " +
                                       "BEGIN " +
                                       "UPDATE {0} SET {8} = {5},{9} = {6},{10} = {7} OUTPUT deleted.{8} where {1} = {2} and {3} = {4} " +
                                       "END",
                    FieldName.TABLE_NAME, FieldName.CURRI_ID, ParameterName.CURRI_ID, 
                    FieldName.ACA_YEAR, ParameterName.ACA_YEAR, ParameterName.FILE_NAME, ParameterName.PERSONNEL_ID, ParameterName.DATE, 
                    FieldName.FILE_NAME, FieldName.PERSONNEL_ID, FieldName.DATE);
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.DATE, date));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.PERSONNEL_ID, personnel_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.FILE_NAME, file_name));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.CURRI_ID, curri_id));
            d.iCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter(ParameterName.ACA_YEAR, aca_year));
            try
            {
                object filenameres = await d.iCommand.ExecuteScalarAsync();
                if (filenameres != null)
                {
                    file_name = filenameres.ToString();
                }
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