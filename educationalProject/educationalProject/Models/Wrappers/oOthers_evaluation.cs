using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oOthers_evaluation : Others_evaluation
    {
        public object SelectByIndicator()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            Others_evaluation_s_indic_name_list_with_file_name result = new Others_evaluation_s_indic_name_list_with_file_name();

            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(200) NULL," +
                                      "[{2}] INT NULL," +
                                      "[{3}] INT NULL," +
                                      "[{4}] INT NULL," +
                                      "[{5}] VARCHAR(20) NULL," +
                                      "[{6}] CHAR NULL," +
                                      "[{7}] VARCHAR(MAX) NULL," +
                                      "[{8}] DATE NULL," +
                                      "[{9}] TIME(0) NULL," +
                                      "[{10}] VARCHAR(255) NULL," +
                                      "[{11}] VARCHAR(4) NULL," +
                                      "[{12}] INT NULL," +
                                      "PRIMARY KEY([row_num])) " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{1}] VARCHAR(200) collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{5}] VARCHAR(20) collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{6}] CHAR collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{7}] VARCHAR(MAX) collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{10}] VARCHAR(255) collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{11}] VARCHAR(4) collate DATABASE_DEFAULT ",
                                      temp5tablename, Sub_indicator.FieldName.SUB_INDICATOR_NAME,
                                      FieldName.OTHERS_EVALUATION_ID, FieldName.INDICATOR_NUM, FieldName.SUB_INDICATOR_NUM,
                                      FieldName.ASSESSOR_ID, FieldName.EVALUATION_SCORE, FieldName.DETAIL,
                                      FieldName.DATE, FieldName.TIME, Evidence.FieldName.FILE_NAME, FieldName.CURRI_ID,
                                      FieldName.ACA_YEAR);

            string insertintotemp5_1 = string.Format("insert into {13} " +
                                       "select {2}, {0}.* " +
                                       "from {0}, {1} " +
                                       "where {0}.{3} = {4} and " +
                                       "{0}.{5} = {1}.{6} and " +
                                       "{0}.{3} = {1}.{7} and " +
                                       "{1}.{8} = " +
                                       "(select max(s1.{8}) from {1} as s1 where s1.{8} <= {9}) and " +
                                       "{0}.{10} = '{11}' and " +
                                       "{0}.{12} = {9} ",
                                       FieldName.TABLE_NAME, Sub_indicator.FieldName.TABLE_NAME,
                                       Sub_indicator.FieldName.SUB_INDICATOR_NAME, FieldName.INDICATOR_NUM,
                                       indicator_num, FieldName.SUB_INDICATOR_NUM,
                                       Sub_indicator.FieldName.SUB_INDICATOR_NUM,
                                       Sub_indicator.FieldName.INDICATOR_NUM,
                                       Sub_indicator.FieldName.ACA_YEAR, aca_year,
                                       FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, temp5tablename);

            string insertintotemp5_2 = string.Format("insert into {12} " +
                                       "select {1},0,{2},{3}," +
                                       "'','0','',null,null,'','{4}',{6} " +
                                       "from {0} where " +
                                       "{2} = 1 and {5} = " +
                                       "(select max(s1.{5}) from {0} as s1 where s1.{5} <= {6}) " +
                                       "and not exists(select * from {7} " +
                                       "where {8} = '{4}' and {9} = {6} and " +
                                       "{0}.{2} = {7}.{10} " +
                                       "and {0}.{3} = {7}.{11}) ",
                                       Sub_indicator.FieldName.TABLE_NAME, Sub_indicator.FieldName.SUB_INDICATOR_NAME,
                                       Sub_indicator.FieldName.INDICATOR_NUM, Sub_indicator.FieldName.SUB_INDICATOR_NUM,
                                       curri_id, Sub_indicator.FieldName.ACA_YEAR, aca_year, FieldName.TABLE_NAME,
                                       FieldName.CURRI_ID, FieldName.ACA_YEAR, FieldName.INDICATOR_NUM,
                                       FieldName.SUB_INDICATOR_NUM, temp5tablename);

            string selectcmd = string.Format("select * from {0} order by {1} ", temp5tablename, FieldName.SUB_INDICATOR_NUM);



            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END", createtabletemp5, insertintotemp5_1,
                insertintotemp5_2, selectcmd);

            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        if (Convert.ToInt32(item.ItemArray[data.Columns[FieldName.OTHERS_EVALUATION_ID].Ordinal]) != 0)
                        {
                            string h, m;
                            DateTime timeofday = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.TIME].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                            h = timeofday.Hour.ToString();
                            m = timeofday.Minute.ToString();

                            result.evaluation_detail.Add(new Others_evaluation_sub_indicator_name
                            {
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                others_evaluation_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.OTHERS_EVALUATION_ID].Ordinal]),
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                assessor_id = item.ItemArray[data.Columns[FieldName.ASSESSOR_ID].Ordinal].ToString(),
                                date = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                                time = (timeofday.Hour > 9 ? "" : "0") + h + '.' + (timeofday.Minute > 9 ? "" : "0") + m,
                                suggestion = item.ItemArray[data.Columns[FieldName.DETAIL].Ordinal].ToString(),
                                evaluation_score = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVALUATION_SCORE].Ordinal]),
                                indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                                sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
                                sub_indicator_name = item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NAME].Ordinal].ToString()
                            });

                            if (item.ItemArray[data.Columns[Evidence.FieldName.FILE_NAME].Ordinal].ToString() != "")
                                result.file_name = item.ItemArray[data.Columns[Evidence.FieldName.FILE_NAME].Ordinal].ToString();
                        }

                        else
                        {
                            result.evaluation_detail.Add(new Others_evaluation_sub_indicator_name
                            {
                                curri_id = this.curri_id,
                                others_evaluation_id = 0,
                                aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                assessor_id = "sayam",
                                date = "",
                                time = "",
                                suggestion = "",
                                evaluation_score = 0,
                                indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                                sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
                                sub_indicator_name = item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NAME].Ordinal].ToString()
                            });
                        }
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
            return result;
        }

        public object InsertOrUpdate(Others_evaluation_s_indic_name_list_with_file_name odata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            //Others_evaluation_s_indic_name_list_with_file_name result = new Others_evaluation_s_indic_name_list_with_file_name();

            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(255) NULL," +
                                      "PRIMARY KEY([row_num])) " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{1}] VARCHAR(255) collate DATABASE_DEFAULT ",
                                      temp5tablename, Evidence.FieldName.FILE_NAME);
            string updatecmd = "";
            if (odata.file_name == "")
            {
                foreach (Others_evaluation_sub_indicator_name o in odata.evaluation_detail)
                    updatecmd += string.Format("if not exists (select * from {0} where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8}) " +
                                "BEGIN " +
                                "INSERT INTO {0} values ({6},{8},'{9}','{10}','{11}','{12}','{13}',null,'{2}',{4}) " +
                                "END " +
                                "ELSE " +
                                "BEGIN " +
                                "UPDATE {0} set {14} = '{9}',{15} = '{10}',{16} = '{11}',{17} = '{12}',{18} = '{13}' where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8} " +
                                "END ", FieldName.TABLE_NAME, FieldName.CURRI_ID, o.curri_id, FieldName.ACA_YEAR, o.aca_year,
                                FieldName.INDICATOR_NUM, o.indicator_num, FieldName.SUB_INDICATOR_NUM, o.sub_indicator_num,
                                o.assessor_id, o.evaluation_score, o.suggestion, o.date, o.time,
                                FieldName.ASSESSOR_ID, FieldName.EVALUATION_SCORE, FieldName.DETAIL, FieldName.DATE, FieldName.TIME);
            }

            
            else
            {
                Others_evaluation_sub_indicator_name minobj = odata.evaluation_detail.Min();
                    updatecmd += string.Format("if not exists (select * from {0} where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8}) " +
                                "BEGIN " +
                                "INSERT INTO {0} values ({6},{8},'{9}','{10}','{11}','{12}','{13}','{20}','{2}',{4}) " +
                                "END " +
                                "ELSE " +
                                "BEGIN " +

                                "INSERT INTO #TEMP5 " +
                                "select * from " +
                                "(UPDATE {0} set {14} = '{9}',{15} = '{10}',{16} = '{11}',{17} = '{12}',{18} = '{13}',{19} = '{20}' " +
                                "output deleted.{19} " +
                                "where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8}) as outputupdate " +
                                "END ", FieldName.TABLE_NAME, FieldName.CURRI_ID, minobj.curri_id, FieldName.ACA_YEAR, minobj.aca_year,
                                FieldName.INDICATOR_NUM, minobj.indicator_num, FieldName.SUB_INDICATOR_NUM, minobj.sub_indicator_num,
                                minobj.assessor_id, minobj.evaluation_score, minobj.suggestion, minobj.date, minobj.time,
                                FieldName.ASSESSOR_ID, FieldName.EVALUATION_SCORE, FieldName.DETAIL, FieldName.DATE, FieldName.TIME,
                                Evidence.FieldName.FILE_NAME,odata.file_name);

                foreach (Others_evaluation_sub_indicator_name o in odata.evaluation_detail)
                {
                    if(o != minobj)
                    {
                        updatecmd += string.Format("if not exists (select * from {0} where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8}) " +
                                "BEGIN " +
                                "INSERT INTO {0} values ({6},{8},'{9}','{10}','{11}','{12}','{13}',null,'{2}',{4}) " +
                                "END " +
                                "ELSE " +
                                "BEGIN " +
                                "UPDATE {0} set {14} = '{9}',{15} = '{10}',{16} = '{11}',{17} = '{12}',{18} = '{13}' where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8} " +
                                "END ", FieldName.TABLE_NAME, FieldName.CURRI_ID, o.curri_id, FieldName.ACA_YEAR, o.aca_year,
                                FieldName.INDICATOR_NUM, o.indicator_num, FieldName.SUB_INDICATOR_NUM, o.sub_indicator_num,
                                o.assessor_id, o.evaluation_score, o.suggestion, o.date, o.time,
                                FieldName.ASSESSOR_ID, FieldName.EVALUATION_SCORE, FieldName.DETAIL, FieldName.DATE, FieldName.TIME);
                    }
                }
            }

            string selectcmd = string.Format("select {1} from {0} ", temp5tablename, Evidence.FieldName.FILE_NAME);



            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} END", createtabletemp5, updatecmd,
                selectcmd);

            try
            {
                object result = d.iCommand.ExecuteScalar();
                if (result != null)
                {
                    //File name to delete from #temp5 table
                    suggestion = result.ToString();
                }
                else
                {
                    suggestion = "";
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
    }
}