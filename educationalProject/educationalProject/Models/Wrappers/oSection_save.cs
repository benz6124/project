using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oSection_save : Section_save
    {
        public object Select()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
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
            return null;
        }

        public object SelectWhere(string wherecond)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
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
            return null;
        }

        public object Update()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            d.iCommand.CommandText = string.Format("update {0} set {1}='{2}',{3}='{4}',{5}='{6}',{7}='{8}' where {9}={10} and {11}={12} and {13}='{14}' and {15}={16}",
                FieldName.TABLE_NAME, FieldName.TIME, time, FieldName.DATE, date, FieldName.TEACHER_ID, teacher_id, FieldName.DETAIL, detail, FieldName.INDICATOR_NUM, indicator_num, FieldName.SUB_INDICATOR_NUM, sub_indicator_num, FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, aca_year);
            try
            {
                int rowAffected = d.iCommand.ExecuteNonQuery();
                if (rowAffected == 1)
                {
                    return null;
                }
                else
                {
                    return "No section_save are updated.";
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

        public object Insert()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            d.iCommand.CommandText = string.Format("insert into {0} values ({1},{2},{3},'{4}','{5}','{6}','{7}','{8}')",
                FieldName.TABLE_NAME,aca_year,indicator_num,sub_indicator_num,teacher_id, detail, date, time, curri_id);
            try
            {
                int rowAffected = d.iCommand.ExecuteNonQuery();
                if (rowAffected == 1)
                {
                    return null;
                }
                else
                {
                    return "No section_save are inserted.";
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