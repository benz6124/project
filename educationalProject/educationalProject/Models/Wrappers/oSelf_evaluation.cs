using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oSelf_evaluation : Self_evaluation
    {
        public object InsertOrUpdate(List<oSelf_evaluation> list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            string insertcmd = string.Format("insert into {0} values ",FieldName.TABLE_NAME);
            //Loop each item to gather data to construct sql cmd
            string updatecmd = "";
            foreach(oSelf_evaluation item in list)
            {
                insertcmd += string.Format("({0},{1},'{2}',{3},'{4}','{5}','{6}',{7})",
                             indicator_num, item.sub_indicator_num, item.teacher_id, item.evaluation_score, item.date, item.time, curri_id, aca_year);
                if (item != list.Last()) insertcmd += ",";
                updatecmd += string.Format("update {0} set {1} = '{2}',{3} = {4},{5} = '{6}',{7} = '{8}' where {9} = {10} and {11} = {12} and {13} = '{14}' and {15} = {16} ",
                                            FieldName.TABLE_NAME, FieldName.TEACHER_ID, item.teacher_id, FieldName.EVALUATION_SCORE, item.evaluation_score, FieldName.DATE, item.date, FieldName.TIME, item.time,
                                            FieldName.INDICATOR_NUM, item.indicator_num, FieldName.SUB_INDICATOR_NUM, item.sub_indicator_num, FieldName.CURRI_ID, item.curri_id, FieldName.ACA_YEAR, item.aca_year);
            }
     

           
            string selectcmd = string.Format("select * from {0} where {1} = {2} and {3} = '{4}' and {5} = {6}",
                               FieldName.TABLE_NAME, FieldName.INDICATOR_NUM, indicator_num, FieldName.CURRI_ID, curri_id,
                               FieldName.ACA_YEAR,aca_year);

            d.iCommand.CommandText = String.Format("IF NOT EXISTS ({0}) " +
                                       "BEGIN " +
                                       "{1} " +
                                       "END " +
                                       "ELSE " +
                                       "BEGIN " +
                                       "{2} " +
                                       "END", selectcmd, insertcmd, updatecmd);
            try
            {
                int rowAffected = d.iCommand.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    return null;
                }
                else
                {
                    return "No self_evaluation are inserted or updated.";
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