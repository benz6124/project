using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oOthers_evaluation : Others_evaluation
    {
        public async Task<object> SelectByIndicator()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            Others_evaluation_s_indic_name_list_with_file_name result = new Others_evaluation_s_indic_name_list_with_file_name();

            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(2000) NULL," +
                                      "[{2}] INT NULL," +
                                      "[{3}] INT NULL," +
                                      "[{4}] INT NULL," +
                                      "[{5}] {16} NULL," +
                                      "[{6}] CHAR NULL," +
                                      "[{7}] VARCHAR(MAX) NULL," +
                                      "[{8}] VARCHAR(MAX) NULL," +
                                      "[{9}] DATE NULL," +
                                      "[{10}] TIME(0) NULL," +
                                      "[{11}] {15} NULL," +
                                      "[{12}] {14} NULL," +
                                      "[{13}] INT NULL," +
                                      "[{17}] VARCHAR(16) NULL," +
                                      "[{18}] VARCHAR(60) NULL," +
                                      "PRIMARY KEY([row_num])) " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{1}] VARCHAR(2000) collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{5}] {16} collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{6}] CHAR collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{7}] VARCHAR(MAX) collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{8}] VARCHAR(MAX) collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{11}] {15} collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{12}] {14} collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{17}] VARCHAR(16) collate DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{18}] VARCHAR(60) collate DATABASE_DEFAULT ",
                                      temp5tablename, Sub_indicator.FieldName.SUB_INDICATOR_NAME,
                                      FieldName.OTHERS_EVALUATION_ID, FieldName.INDICATOR_NUM, FieldName.SUB_INDICATOR_NUM,
                                      FieldName.ASSESSOR_ID, FieldName.EVALUATION_SCORE, FieldName.STRENGTH, FieldName.IMPROVE,
                                      FieldName.DATE, FieldName.TIME, Evidence.FieldName.FILE_NAME, FieldName.CURRI_ID,
                                      FieldName.ACA_YEAR, DBFieldDataType.CURRI_ID_TYPE, DBFieldDataType.FILE_NAME_TYPE,
                                      DBFieldDataType.USERNAME_TYPE, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME);

            string insertintotemp5_1 = string.Format("insert into {13} " +
                                       "select {2}, {0}.*,{17},{18} " +
                                       "from {0}, {1}, {14} " +
                                       "where {0}.{3} = {4} and " +
                                       "{0}.{5} = {1}.{6} and " +
                                       "{0}.{3} = {1}.{7} and " +
                                       "{15} = {16} and " +  //user_id = assessor_id
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
                                       FieldName.CURRI_ID, curri_id, FieldName.ACA_YEAR, temp5tablename,
                                       User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID, FieldName.ASSESSOR_ID,
                                       Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME);

            string insertintotemp5_2 = string.Format("insert into {12} " +
                                       "select {1},0,{2},{3}," +
                                       "'','0','','',null,null,'','{4}',{6},null,null " +
                                       "from {0} where " +
                                       "{2} = {13} and {5} = " +
                                       "(select max(s1.{5}) from {0} as s1 where s1.{5} <= {6}) " +
                                       "and not exists(select * from {7} " +
                                       "where {8} = '{4}' and {9} = {6} and " +
                                       "{0}.{2} = {7}.{10} " +
                                       "and {0}.{3} = {7}.{11}) ",
                                       Sub_indicator.FieldName.TABLE_NAME, Sub_indicator.FieldName.SUB_INDICATOR_NAME,
                                       Sub_indicator.FieldName.INDICATOR_NUM, Sub_indicator.FieldName.SUB_INDICATOR_NUM,
                                       curri_id, Sub_indicator.FieldName.ACA_YEAR, aca_year, FieldName.TABLE_NAME,
                                       FieldName.CURRI_ID, FieldName.ACA_YEAR, FieldName.INDICATOR_NUM,
                                       FieldName.SUB_INDICATOR_NUM, temp5tablename, indicator_num);

            string selectcmd = string.Format("select * from {0} order by {1} ", temp5tablename, FieldName.SUB_INDICATOR_NUM);

            string selectselfscorecmd = string.Format("select {0},{1} " +
                                        "from {2} " +
                                        "where {3} = '{4}' and {5} = {6} and {7} = {8} ",
                                        Self_evaluation.FieldName.SUB_INDICATOR_NUM, Self_evaluation.FieldName.EVALUATION_SCORE,
                                        Self_evaluation.FieldName.TABLE_NAME, Self_evaluation.FieldName.CURRI_ID, curri_id,
                                        Self_evaluation.FieldName.ACA_YEAR, aca_year, Self_evaluation.FieldName.INDICATOR_NUM, indicator_num);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} END", createtabletemp5, insertintotemp5_1,
                insertintotemp5_2, selectcmd, selectselfscorecmd);

            try
            {
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();
                do
                {
                    if (res.HasRows)
                    {
                        DataTable data = new DataTable();
                        data.Load(res);
                        if (data.Columns.Count > 2)
                        {
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
                                        assessor_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ASSESSOR_ID].Ordinal]),
                                        t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                             item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString(),
                                        date = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3],
                                        time = (timeofday.Hour > 9 ? "" : "0") + h + '.' + (timeofday.Minute > 9 ? "" : "0") + m,
                                        strength = item.ItemArray[data.Columns[FieldName.STRENGTH].Ordinal].ToString(),
                                        improve = item.ItemArray[data.Columns[FieldName.IMPROVE].Ordinal].ToString(),
                                        evaluation_score = item.ItemArray[data.Columns[FieldName.EVALUATION_SCORE].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns[FieldName.EVALUATION_SCORE].Ordinal]) : 0,
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
                                        assessor_id = 31,
                                        date = "",
                                        time = "",
                                        strength = "",
                                        improve = "",
                                        evaluation_score = 0,
                                        indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.INDICATOR_NUM].Ordinal]),
                                        sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.SUB_INDICATOR_NUM].Ordinal]),
                                        sub_indicator_name = item.ItemArray[data.Columns[Sub_indicator.FieldName.SUB_INDICATOR_NAME].Ordinal].ToString()
                                    });
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow item in data.Rows)
                            {
                                sub_indicator_num = Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.SUB_INDICATOR_NUM].Ordinal]);
                                result.evaluation_detail.First(t => t.sub_indicator_num == sub_indicator_num).self_score =
                                    Convert.ToInt32(item.ItemArray[data.Columns[Self_evaluation.FieldName.EVALUATION_SCORE].Ordinal]);
                            }
                        }
                        data.Dispose();
                    }
                    else if (!res.IsClosed)
                    {
                        if (!res.NextResult())
                            break;
                    }
                } while (!res.IsClosed);
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

        public async Task<object> InsertOrUpdate(Others_evaluation_s_indic_name_list_with_file_name odata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            //Others_evaluation_s_indic_name_list_with_file_name result = new Others_evaluation_s_indic_name_list_with_file_name();

            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] {2} NULL," +
                                      "PRIMARY KEY([row_num])) " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN [{1}] {2} collate DATABASE_DEFAULT ",
                                      temp5tablename, Evidence.FieldName.FILE_NAME, DBFieldDataType.FILE_NAME_TYPE);
            string updatecmd = "";

            if (odata.file_name == "")
            {
                foreach (Others_evaluation_sub_indicator_name o in odata.evaluation_detail)
                    updatecmd += string.Format("if not exists (select * from {0} where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8}) " +
                                "BEGIN " +
                                "INSERT INTO {0} values ({6},{8},'{9}',{10},'{11}','{12}','{13}','{14}',null,'{2}',{4}) " +
                                "END " +
                                "ELSE " +
                                "BEGIN " +
                                "UPDATE {0} set {15} = '{9}',{16} = {10},{17} = '{11}',{18} = '{12}',{19} = '{13}',{20} = '{14}' where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8} " +
                                "END ", FieldName.TABLE_NAME, FieldName.CURRI_ID, o.curri_id, FieldName.ACA_YEAR, o.aca_year,
                                FieldName.INDICATOR_NUM, o.indicator_num, FieldName.SUB_INDICATOR_NUM, o.sub_indicator_num,
                                o.assessor_id, o.evaluation_score > 0 ? "'" + o.evaluation_score.ToString() + "'" : "null", /*11*/ o.strength,/*12*/ o.improve
                                , o.date, o.time,
                                FieldName.ASSESSOR_ID, FieldName.EVALUATION_SCORE, FieldName.STRENGTH, FieldName.IMPROVE, FieldName.DATE, FieldName.TIME);
            }


            else
            {
                Others_evaluation_sub_indicator_name minobj = odata.evaluation_detail.Min();
                updatecmd += string.Format("if not exists (select * from {0} where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8}) " +
                            "BEGIN " +
                            "INSERT INTO {0} values ({6},{8},'{9}',{10},'{11}','{12}','{13}','{14}','{22}','{2}',{4}) " +
                            "END " +
                            "ELSE " +
                            "BEGIN " +

                            "INSERT INTO #TEMP5 " +
                            "select * from " +
                            "(UPDATE {0} set {15} = '{9}',{16} = {10},{17} = '{11}',{18} = '{12}',{19} = '{13}',{20} = '{14}',{21} = '{22}' " +
                            "output deleted.{21} " +
                            "where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8}) as outputupdate " +
                            "END ", FieldName.TABLE_NAME, FieldName.CURRI_ID, minobj.curri_id, FieldName.ACA_YEAR, minobj.aca_year,
                            FieldName.INDICATOR_NUM, minobj.indicator_num, FieldName.SUB_INDICATOR_NUM, minobj.sub_indicator_num,
                            minobj.assessor_id, minobj.evaluation_score > 0 ? "'" + minobj.evaluation_score.ToString() + "'" : "null",
                            /*11*/ minobj.strength,/*12*/ minobj.improve, minobj.date, minobj.time,
                            FieldName.ASSESSOR_ID, FieldName.EVALUATION_SCORE, FieldName.STRENGTH, FieldName.IMPROVE, FieldName.DATE, FieldName.TIME,
                            Evidence.FieldName.FILE_NAME, odata.file_name);

                foreach (Others_evaluation_sub_indicator_name o in odata.evaluation_detail)
                {
                    if (o != minobj)
                    {
                        updatecmd += string.Format("if not exists (select * from {0} where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8}) " +
                                                        "BEGIN " +
                                                        "INSERT INTO {0} values ({6},{8},'{9}',{10},'{11}','{12}','{13}','{14}',null,'{2}',{4}) " +
                                                        "END " +
                                                        "ELSE " +
                                                        "BEGIN " +
                                                        "UPDATE {0} set {15} = '{9}',{16} = {10},{17} = '{11}',{18} = '{12}',{19} = '{13}',{20} = '{14}' where {1} = '{2}' and {3} = {4} and {5} = {6} and {7} = {8} " +
                                                        "END ", FieldName.TABLE_NAME, FieldName.CURRI_ID, o.curri_id, FieldName.ACA_YEAR, o.aca_year,
                                                        FieldName.INDICATOR_NUM, o.indicator_num, FieldName.SUB_INDICATOR_NUM, o.sub_indicator_num,
                                                        o.assessor_id, o.evaluation_score > 0 ? "'" + o.evaluation_score.ToString() + "'" : "null", /*11*/ o.strength,/*12*/ o.improve
                                                        , o.date, o.time,
                                                        FieldName.ASSESSOR_ID, FieldName.EVALUATION_SCORE, FieldName.STRENGTH, FieldName.IMPROVE, FieldName.DATE, FieldName.TIME);
                    }
                }
            }

            string selectcmd = string.Format("select {1} from {0} ", temp5tablename, Evidence.FieldName.FILE_NAME);



            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} END", createtabletemp5, updatecmd,
                selectcmd);

            try
            {
                object result = await d.iCommand.ExecuteScalarAsync();
                if (result != null)
                {
                    //File name to delete from #temp5 table
                    strength = result.ToString();
                }
                else
                {
                    strength = "";
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