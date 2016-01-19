using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oAun_book : Aun_book
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
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
                            personnel_id = item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal].ToString(),
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
        public object SelectFileDownloadLink(string wherecond)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            d.iCommand.CommandText = string.Format("select {0} from {1} where {2}", FieldName.FILE_NAME,FieldName.TABLE_NAME,wherecond);
            try
            {
                object result = d.iCommand.ExecuteScalar();
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

    }
}