using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models;
using educationalProject.Models.Wrappers;
using educationalProject.Utils;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oSelf_evaluation_sub_indicator_name : vSelf_evaluation_sub_indicator_name
    {
        public object SelectByIndicatorAndCurriculum(oIndicator inddata,string curri_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oSelf_evaluation_sub_indicator_name> result = new List<oSelf_evaluation_sub_indicator_name>();
            d.iCommand.CommandText = string.Format("select main_res.*,"+
                "{0} from (select * from {1} where {2} = {3} and {4} = '{5}' and {6} = {7}) as main_res inner join (select * from {8} where {9} = {3} and  {10} = {7}) as sub_res on main_res.{11} = sub_res.{11}",
                Sub_indicator.FieldName.SUB_INDICATOR_NAME,FieldName.TABLE_NAME,FieldName.INDICATOR_NUM,inddata.indicator_num,FieldName.CURRI_ID,curri_id,
                FieldName.ACA_YEAR,inddata.aca_year,Sub_indicator.FieldName.TABLE_NAME, Sub_indicator.FieldName.INDICATOR_NUM, Sub_indicator.FieldName.ACA_YEAR,FieldName.SUB_INDICATOR_NUM);
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
                        DateTime timeofday = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.TIME].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                        h = timeofday.Hour.ToString();
                        m = timeofday.Minute.ToString();
                        result.Add(new oSelf_evaluation_sub_indicator_name
                        {
                            aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                            curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                            indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                            sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
                            evaluation_score = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVALUATION_SCORE].Ordinal]),
                            teacher_id = item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal].ToString(),
                            sub_indicator_name = item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NAME].Ordinal].ToString(),
                            date = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                            time = (timeofday.Hour > 9 ? "" : "0") + h + '.' + (timeofday.Minute > 9 ? "" : "0") + m
                        });
                    }
                    res.Close();
                    data.Dispose();
                }
                else
                {
                    return null;
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