using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oStudent_status_other : Student_status_other
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
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

        public object SelectWhere(string wherecond)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oStudent_status_other> result = new List<oStudent_status_other>();
            d.iCommand.CommandText = string.Format("select * from {0} where {1}", FieldName.TABLE_NAME,wherecond);
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
    }
}