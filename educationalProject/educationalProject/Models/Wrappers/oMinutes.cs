using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oMinutes : Minutes
    {
        private string getSelectByCurriculumAcademicCommand()
        {
            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("create table {0}(" +
                                      "[row_num] int identity(1, 1) not null," +
                                      "[{1}] int not null," +
                                      "[t1_id] varchar(5) not null," +
                                      "[t1_prename] varchar(16) not null," +
                                      "[t1_name] varchar(60) not null," +
                                      "[{2}] varchar(4) not null," +
                                      "[{3}] int not null," +
                                      "[{4}] date null," +
                                      "[{5}] varchar(1000) not null," +
                                      "[{6}] varchar(255) not null," +
                                      "[t2_id] varchar(5) not null," +
                                      "[t2_prename] varchar(16) not null," +
                                      "[t2_name] varchar(60) not null," +
                                      "PRIMARY KEY([row_num])) " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN t1_id varchar(5) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN t1_prename varchar(16) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN t1_name varchar(60) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {2} varchar(4) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {5} varchar(1000) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {6} varchar(255) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN t2_id varchar(5) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN t2_prename varchar(16) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN t2_name varchar(60) COLLATE DATABASE_DEFAULT ",
                                      temp5tablename, FieldName.MINUTES_ID, FieldName.CURRI_ID, FieldName.ACA_YEAR,
                                      FieldName.DATE, FieldName.TOPIC_NAME, FieldName.FILE_NAME);

            //retrieve normal row with attendee data
            string insertintotemp5_1 = string.Format("insert into {0} " +
                                       "select * from " +
                                       "(select {1}.{2}, {1}.{3} as t1_id, t1.{4} as t1_prename, t1.{5} as t1_name, {1}.{6}, {1}.{7}, {1}.{8}, " +
                                       "{1}.{9}, {1}.{10} , {11}.{12}, t2.{4}, t2.{5} " +
                                       "from {1}, {13} as t1, {11}, {13} as t2 " +
                                       "where {6} = '{14}' and {7} = {15} and {1}.{2} = {11}.{16} " +
                                       "and t1.{17} = {1}.{3} and t2.{17} = {11}.{12}) as outputsel1 order by {8} desc ",
                                       temp5tablename, FieldName.TABLE_NAME, FieldName.MINUTES_ID, FieldName.TEACHER_ID,
                                       Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, FieldName.CURRI_ID,
                                       FieldName.ACA_YEAR, FieldName.DATE, FieldName.TOPIC_NAME, FieldName.FILE_NAME,
                                       Minutes_attendee.FieldName.TABLE_NAME, Minutes_attendee.FieldName.TEACHER_ID,
                                       Teacher.FieldName.TABLE_NAME, curri_id, aca_year, Minutes_attendee.FieldName.MINUTES_ID,
                                       Teacher.FieldName.TEACHER_ID);

            //retrieve extra row with pic_file data
            string insertintotemp5_2 = string.Format("insert into {0} " +
                                       "select {1},'','','','',0,null,'',{2} , '','','' from " +
                                       "(select * from {3} where exists " +
                                       "(select * from {4} where {3}.{1} = {4}.{5} and {6} = '{7}' and {8} = {9})) as outputsel2 ",
                                       temp5tablename, Minutes_pic.FieldName.MINUTES_ID, Minutes_pic.FieldName.FILE_NAME,
                                       Minutes_pic.FieldName.TABLE_NAME, FieldName.TABLE_NAME, FieldName.MINUTES_ID, FieldName.CURRI_ID,
                                       curri_id, FieldName.ACA_YEAR, aca_year);

            string selectcmd = string.Format("select * from {0} ", temp5tablename);

            return string.Format("BEGIN {0} {1} {2} {3} END", createtabletemp5, insertintotemp5_1, insertintotemp5_2, selectcmd);
        }

        public object SelectByCurriculumAcademic()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Minutes_detail> result = new List<Minutes_detail>();

            d.iCommand.CommandText = getSelectByCurriculumAcademicCommand();
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        minutes_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.MINUTES_ID].Ordinal]);
                        date = item.ItemArray[data.Columns[FieldName.DATE].Ordinal].ToString();

                        //If date is not null (means result is row with attendee data)
                        if(date != "")
                        {
                            //Is it exists?
                            if(result.FirstOrDefault(m => m.minutes_id == minutes_id) == null)
                            {
                                result.Add(new Minutes_detail
                                {
                                    aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                    teacher_id = item.ItemArray[data.Columns["t1_id"].Ordinal].ToString(),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t1_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t1_name"].Ordinal].ToString(),
                                    curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                    file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                                    topic_name = item.ItemArray[data.Columns[FieldName.TOPIC_NAME].Ordinal].ToString(),
                                    minutes_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.MINUTES_ID].Ordinal]),
                                    date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3]
                                });
                                result.FirstOrDefault(m => m.minutes_id == minutes_id).attendee.Add(new Teacher_with_t_name
                                {
                                    teacher_id = item.ItemArray[data.Columns["t2_id"].Ordinal].ToString(),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t2_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t2_name"].Ordinal].ToString()
                                });
                            }
                            else
                            {
                                result.FirstOrDefault(m => m.minutes_id == minutes_id).attendee.Add(new Teacher_with_t_name
                                {
                                    teacher_id = item.ItemArray[data.Columns["t2_id"].Ordinal].ToString(),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t2_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t2_name"].Ordinal].ToString()
                                });
                            }
                        }
                        else
                        {
                            result.First(m => m.minutes_id == minutes_id).pictures.Add(item.ItemArray[data.Columns[Minutes_pic.FieldName.FILE_NAME].Ordinal].ToString());
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
    }
}