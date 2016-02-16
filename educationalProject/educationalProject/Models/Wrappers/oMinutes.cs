using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.ViewModels;
namespace educationalProject.Models.Wrappers
{
    public class oMinutes : Minutes
    {
        private string getSelectByCurriculumAcademicCommand(bool isUpdate)
        {
            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("create table {0}(" +
                                      "[row_num] int identity(1, 1) not null," +
                                      "[{1}] int not null," +
                                      "[t1_id] {7} not null," +
                                      "[t1_prename] varchar(16) not null," +
                                      "[t1_name] varchar(60) not null," +
                                      "[{2}] {8} not null," +
                                      "[{3}] int not null," +
                                      "[{4}] date null," +
                                      "[{5}] varchar(1000) not null," +
                                      "[{6}] {9} not null," +
                                      "[t2_id] {7} not null," +
                                      "[t2_prename] varchar(16) not null," +
                                      "[t2_name] varchar(60) not null," +
                                      "PRIMARY KEY([row_num])) " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN t1_prename varchar(16) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN t1_name varchar(60) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {2} {8} COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {5} varchar(1000) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {6} {9} COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN t2_prename varchar(16) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN t2_name varchar(60) COLLATE DATABASE_DEFAULT ",
                                      temp5tablename, FieldName.MINUTES_ID, FieldName.CURRI_ID, FieldName.ACA_YEAR,
                                      FieldName.DATE, FieldName.TOPIC_NAME, FieldName.FILE_NAME,"INT",
                                      DBFieldDataType.CURRI_ID_TYPE,DBFieldDataType.FILE_NAME_TYPE);

            //retrieve normal row with attendee data
            string insertintotemp5_1 = string.Format("insert into {0} " +
                                       "select * from " +
                                       "(select {1}.{2}, {1}.{3} as t1_id, t1.{4} as t1_prename, t1.{5} as t1_name, {1}.{6}, {1}.{7}, {1}.{8}, " +
                                       "{1}.{9}, {1}.{10} , {11}.{12}, t2.{4}, t2.{5} " +
                                       "from {1}, ({13}) as t1, {11}, ({13}) as t2 " +
                                       "where {6} = '{14}' and {7} = {15} and {1}.{2} = {11}.{16} " +
                                       "and t1.{17} = {1}.{3} and t2.{17} = {11}.{12}) as outputsel1 order by {8} desc ",
                                       temp5tablename, FieldName.TABLE_NAME, FieldName.MINUTES_ID, FieldName.TEACHER_ID,
                                       Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, FieldName.CURRI_ID,
                                       FieldName.ACA_YEAR, FieldName.DATE, FieldName.TOPIC_NAME, FieldName.FILE_NAME,
                                       Minutes_attendee.FieldName.TABLE_NAME, Minutes_attendee.FieldName.TEACHER_ID,
                                       oTeacher.getSelectTeacherByJoinCommand(), curri_id, aca_year, Minutes_attendee.FieldName.MINUTES_ID,
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
            if (!isUpdate)
                return string.Format("BEGIN {0} {1} {2} {3} END", createtabletemp5, insertintotemp5_1, insertintotemp5_2, selectcmd);
            else
            {
                string temp1tablename = "#temp1";
                //if the current operation is update add the #temp1 table data to #temp5!
                string insertintotemp5_3 = string.Format("insert into {0} " +
                                       "select -1,'','','','',0,null,'',{1} , '','','' from {2} where row_num = 1 ",
                                       temp5tablename, Minutes_pic.FieldName.FILE_NAME, temp1tablename);
                string insertintotemp5_4 = string.Format("insert into {0} " +
                       "select -2,'','','','',0,null,'',{1} , '','','' from {2} where row_num != 1 ",
                       temp5tablename, Minutes_pic.FieldName.FILE_NAME, temp1tablename);
                return string.Format("BEGIN {0} {1} {2} {3} {4} {5} END", createtabletemp5, insertintotemp5_1, insertintotemp5_2,insertintotemp5_3,insertintotemp5_4, selectcmd);
            }
        }

        public object SelectByCurriculumAcademic()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Minutes_detail> result = new List<Minutes_detail>();

            d.iCommand.CommandText = getSelectByCurriculumAcademicCommand(false);
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
                                    teacher_id = item.ItemArray[data.Columns["t1_id"].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns["t1_id"].Ordinal]) : 0,
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t1_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t1_name"].Ordinal].ToString(),
                                    curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                    file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                                    topic_name = item.ItemArray[data.Columns[FieldName.TOPIC_NAME].Ordinal].ToString(),
                                    minutes_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.MINUTES_ID].Ordinal]),
                                    date = Convert.ToDateTime(item.ItemArray[data.Columns[FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3]
                                });
                                result.FirstOrDefault(m => m.minutes_id == minutes_id).attendee.Add(new Teacher_with_t_name
                                {
                                    teacher_id = Convert.ToInt32(item.ItemArray[data.Columns["t2_id"].Ordinal]),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t2_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t2_name"].Ordinal].ToString()
                                });
                            }
                            else
                            {
                                result.FirstOrDefault(m => m.minutes_id == minutes_id).attendee.Add(new Teacher_with_t_name
                                {
                                    teacher_id = Convert.ToInt32(item.ItemArray[data.Columns["t2_id"].Ordinal]),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t2_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t2_name"].Ordinal].ToString()
                                });
                            }
                        }
                        else
                        {
                            result.First(m => m.minutes_id == minutes_id).pictures.Add(new Minutes_pic
                            {
                                file_name = item.ItemArray[data.Columns[Minutes_pic.FieldName.FILE_NAME].Ordinal].ToString(),
                                minutes_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.MINUTES_ID].Ordinal])
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


        public object Delete(List<Minutes_detail> list)
        {
            DBConnector d = new DBConnector();
            List<string> file_to_delete = new List<string>();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            string deleteprecmd = string.Format("DELETE FROM {0} OUTPUT DELETED.{5} WHERE {1} = '{2}' and {3} = {4} ",
                FieldName.TABLE_NAME, FieldName.CURRI_ID, list.First().curri_id, FieldName.ACA_YEAR, list.First().aca_year, FieldName.FILE_NAME);
            string excludecond = "1=1 ";
            foreach (Minutes_detail item in list)
            {
                excludecond += string.Format("and {0} != {1} ", FieldName.MINUTES_ID, item.minutes_id);
            }

            string deletefullcmd = string.Format("{0} and ({1})", deleteprecmd, excludecond);

            string temp1tablename = "#temp1";
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] {2} NOT NULL," +
                                      "PRIMARY KEY([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} {2} COLLATE DATABASE_DEFAULT ",
                                      temp1tablename,FieldName.FILE_NAME,DBFieldDataType.FILE_NAME_TYPE);

            string insertintotemp1_1 = string.Format("INSERT INTO {0} " +
                                       "SELECT {1} from {2} where ({3}) " + 
                                       "and {4} in " +
                                       "(select {5} from {6} where {7} = '{8}' and {9} = {10}) ",
                                       temp1tablename, Minutes_pic.FieldName.FILE_NAME, Minutes_pic.FieldName.TABLE_NAME,
                                       excludecond,Minutes_pic.FieldName.MINUTES_ID,FieldName.MINUTES_ID,
                                       FieldName.TABLE_NAME,FieldName.CURRI_ID,list.First().curri_id,
                                       FieldName.ACA_YEAR,list.First().aca_year);

            string insertintotemp1_2 = string.Format("INSERT INTO {0} " +
                                       "select * from " +
                                       "({1}) as outputdelete ", temp1tablename, deletefullcmd);

            string selcmd = string.Format("select {0} from {1} ", FieldName.FILE_NAME, temp1tablename);
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END",createtabletemp1,insertintotemp1_1,
                insertintotemp1_2,selcmd);

            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        file_to_delete.Add(
                           item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString()
                        );
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
            return file_to_delete;
        }

        public object InsertNewMinutesWithSelect(Minutes_detail mdata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Minutes_detail> result = new List<Minutes_detail>();
            string temp1tablename = "#temp1";
            string temp2tablename = "#temp2";
            string temp3tablename = "#temp3";
            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NOT NULL," +
                                      "PRIMARY KEY ([row_num])) ", temp1tablename, FieldName.MINUTES_ID);

            string createtabletemp2 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NULL," +
                                      "PRIMARY KEY ([row_num])) "
                                      , temp2tablename, Minutes_attendee.FieldName.TEACHER_ID);

            string createtabletemp3 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] {2} NULL," +
                                      "PRIMARY KEY ([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} {2} COLLATE DATABASE_DEFAULT "
                                      , temp3tablename, Minutes_pic.FieldName.FILE_NAME,
                                      DBFieldDataType.FILE_NAME_TYPE);


            string insertintotemp1 = string.Format("INSERT INTO {0} " +
                                     "select * from " +
                                     "(insert into {1}({2},{3},{4},{5},{6},{7}) output inserted.{8} values " +
                                     "('{9}','{10}',{11},'{12}','{13}','{14}')) " +
                                     "as outputinsert ",
                                     temp1tablename, FieldName.TABLE_NAME, FieldName.TEACHER_ID, FieldName.CURRI_ID, FieldName.ACA_YEAR,
                                     FieldName.DATE, FieldName.TOPIC_NAME, FieldName.FILE_NAME, FieldName.MINUTES_ID,
                                     mdata.teacher_id, mdata.curri_id, mdata.aca_year, mdata.date, mdata.topic_name,
                                     mdata.file_name);


            string insertintotemp2 = string.Format("INSERT INTO {0} VALUES (null)", temp2tablename);

            foreach (Teacher_with_t_name t in mdata.attendee)
            {
                insertintotemp2 += string.Format(",('{0}')", t.teacher_id);
            }

            string insertintotemp3 = string.Format("INSERT INTO {0} VALUES (null)", temp3tablename);

            foreach (Minutes_pic m in mdata.pictures)
            {
                insertintotemp3 += string.Format(",('{0}')", m.file_name);
            }

            string insertintominutesattendee = string.Format(" INSERT INTO {0} " +
                                        "select {1},{2} from {3},{4} where {2} is not null ",
                                        Minutes_attendee.FieldName.TABLE_NAME, FieldName.MINUTES_ID, Minutes_attendee.FieldName.TEACHER_ID,
                                        temp1tablename, temp2tablename);

            string insertintominutespic = string.Format(" INSERT INTO {0} " +
                                        "select {1},{2} from {3},{4} where {2} is not null ",
                                        Minutes_pic.FieldName.TABLE_NAME, FieldName.MINUTES_ID, Minutes_pic.FieldName.FILE_NAME,
                                        temp1tablename, temp3tablename);

            curri_id = mdata.curri_id;
            aca_year = mdata.aca_year;

            string selectcmd = getSelectByCurriculumAcademicCommand(false);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} {6} {7} {8} END", createtabletemp1, createtabletemp2, createtabletemp3,
                insertintotemp1, insertintotemp2, insertintotemp3, insertintominutesattendee,
                insertintominutespic, selectcmd);

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
                        if (date != "")
                        {
                            //Is it exists?
                            if (result.FirstOrDefault(m => m.minutes_id == minutes_id) == null)
                            {
                                result.Add(new Minutes_detail
                                {
                                    aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                    teacher_id = item.ItemArray[data.Columns["t1_id"].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns["t1_id"].Ordinal]) : 0,
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t1_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t1_name"].Ordinal].ToString(),
                                    curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                    file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                                    topic_name = item.ItemArray[data.Columns[FieldName.TOPIC_NAME].Ordinal].ToString(),
                                    minutes_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.MINUTES_ID].Ordinal]),
                                    date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3]
                                });
                                result.FirstOrDefault(m => m.minutes_id == minutes_id).attendee.Add(new Teacher_with_t_name
                                {
                                    teacher_id = Convert.ToInt32(item.ItemArray[data.Columns["t2_id"].Ordinal]),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t2_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t2_name"].Ordinal].ToString()
                                });
                            }
                            else
                            {
                                result.FirstOrDefault(m => m.minutes_id == minutes_id).attendee.Add(new Teacher_with_t_name
                                {
                                    teacher_id = Convert.ToInt32(item.ItemArray[data.Columns["t2_id"].Ordinal]),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t2_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t2_name"].Ordinal].ToString()
                                });
                            }
                        }
                        else
                        {
                            result.First(m => m.minutes_id == minutes_id).pictures.Add( new Minutes_pic
                            {
                                file_name = item.ItemArray[data.Columns[Minutes_pic.FieldName.FILE_NAME].Ordinal].ToString(),
                                minutes_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.MINUTES_ID].Ordinal])
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

        public object UpdateMinutesWithSelect(Minutes_detail mdata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Minutes_detail> result = new List<Minutes_detail>();
            Minutes_detail dummyfordeleteminutes = new Minutes_detail();
            string temp1tablename = "#temp1";

            string createtabletemp1 = string.Format("create table {0} (" +
                                     "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                     "[{1}] {2} NOT NULL," +
                                     "PRIMARY KEY([row_num])) " +
                                     "ALTER TABLE {0} " +
                                     "ALTER COLUMN {1} {2} COLLATE DATABASE_DEFAULT ",
                                     temp1tablename, FieldName.FILE_NAME,DBFieldDataType.FILE_NAME_TYPE);

            string insertintotemp1_1;
            //TOEDIT --UPDATE MINUTES TABLE (2 CASE WITH FILE_NAME UPDATES)
            if (mdata.file_name != "")
            {
                insertintotemp1_1 = string.Format("INSERT INTO {0} " +
                                         "select * from " +
                                         "(update {1} set {2} = '{3}', {4} = '{5}', {6} = '{7}',{8} = '{9}' " +
                                         "OUTPUT deleted.{8} where {10} = {11}) as outputupdate ",
                                         temp1tablename, FieldName.TABLE_NAME, FieldName.TEACHER_ID, mdata.teacher_id,
                                         FieldName.DATE, mdata.date, FieldName.TOPIC_NAME, mdata.topic_name,
                                         FieldName.FILE_NAME, mdata.file_name, FieldName.MINUTES_ID, mdata.minutes_id);
            }
            else
            {
                insertintotemp1_1 = string.Format("update {0} set {1} = '{2}', {3} = '{4}', {5} = '{6}' " +
                         "where {7} = {8} ",
                         FieldName.TABLE_NAME, FieldName.TEACHER_ID, mdata.teacher_id,
                         FieldName.DATE, mdata.date, FieldName.TOPIC_NAME, mdata.topic_name,
                         FieldName.MINUTES_ID, mdata.minutes_id);
            }

            //OK UPDATE ATTENDEE
            string deletefromminutesattendee = string.Format("DELETE FROM {0} where {1} = {2} ",
                Minutes_attendee.FieldName.TABLE_NAME, Minutes_attendee.FieldName.MINUTES_ID, mdata.minutes_id);

            string insertintominutesattendee = string.Format("INSERT INTO {0} values ",
                Minutes_attendee.FieldName.TABLE_NAME);

            foreach (Teacher_with_t_name t in mdata.attendee)
            {
                insertintominutesattendee += string.Format("({0},'{1}')", mdata.minutes_id, t.teacher_id);
                if (t != mdata.attendee.Last())
                    insertintominutesattendee += ",";
            }


            string insertintominutespiccmd = string.Format("insert into {0} values ", Minutes_pic.FieldName.TABLE_NAME);
            string deletefromminutespicturecmd = string.Format("delete from {0} output deleted.{1} where {2} = {3} ",
                                          Minutes_pic.FieldName.TABLE_NAME, Minutes_pic.FieldName.FILE_NAME, Minutes_pic.FieldName.MINUTES_ID,
                                          mdata.minutes_id);

            string excludecond = "1=1 ";
            int len = insertintominutespiccmd.Length;

            foreach (Minutes_pic m in mdata.pictures)
            {
                if (m.minutes_id != 0)
                {
                    //Gen delete cond
                    excludecond += string.Format("and {0} != '{1}' ", Minutes_pic.FieldName.FILE_NAME, m.file_name);
                }
                else
                {
                    if (insertintominutespiccmd.Length <= len)
                        insertintominutespiccmd += string.Format("({0},'{1}')", mdata.minutes_id, m.file_name);
                    else
                        insertintominutespiccmd += string.Format(",({0},'{1}')", mdata.minutes_id, m.file_name);
                }
            }
            if (insertintominutespiccmd.Length <= len)
                insertintominutespiccmd = "";

            deletefromminutespicturecmd += string.Format("and ({0}) ", excludecond);

            string insertintotemp1_2 = string.Format("INSERT INTO {0} " +
                                     "select * from " +
                                     "({1}) " +
                                     "as outputdelete ",
                                     temp1tablename, deletefromminutespicturecmd);

            curri_id = mdata.curri_id;
            aca_year = mdata.aca_year;

            string selectcmd = getSelectByCurriculumAcademicCommand(true);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} {6} END", createtabletemp1,
                insertintotemp1_1, deletefromminutesattendee, insertintominutesattendee,
                insertintotemp1_2, insertintominutespiccmd, selectcmd);

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
                        if (date != "")
                        {
                            //Is it exists?
                            if (result.FirstOrDefault(m => m.minutes_id == minutes_id) == null)
                            {
                                result.Add(new Minutes_detail
                                {
                                    aca_year = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.ACA_YEAR].Ordinal]),
                                    teacher_id = item.ItemArray[data.Columns["t1_id"].Ordinal].ToString() != "" ? Convert.ToInt32(item.ItemArray[data.Columns["t1_id"].Ordinal]) : 0,
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t1_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t1_name"].Ordinal].ToString(),
                                    curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                    file_name = item.ItemArray[data.Columns[FieldName.FILE_NAME].Ordinal].ToString(),
                                    topic_name = item.ItemArray[data.Columns[FieldName.TOPIC_NAME].Ordinal].ToString(),
                                    minutes_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.MINUTES_ID].Ordinal]),
                                    date = Convert.ToDateTime(item.ItemArray[data.Columns[Self_evaluation.FieldName.DATE].Ordinal].ToString(), System.Globalization.CultureInfo.CurrentCulture).GetDateTimeFormats()[3]
                                });
                                result.FirstOrDefault(m => m.minutes_id == minutes_id).attendee.Add(new Teacher_with_t_name
                                {
                                    teacher_id = Convert.ToInt32(item.ItemArray[data.Columns["t2_id"].Ordinal]),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t2_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t2_name"].Ordinal].ToString()
                                });
                            }
                            else
                            {
                                result.FirstOrDefault(m => m.minutes_id == minutes_id).attendee.Add(new Teacher_with_t_name
                                {
                                    teacher_id = Convert.ToInt32(item.ItemArray[data.Columns["t2_id"].Ordinal]),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns["t2_prename"].Ordinal].ToString()) + item.ItemArray[data.Columns["t2_name"].Ordinal].ToString()
                                });
                            }
                            
                        }
                        else if(minutes_id != -1 && minutes_id != -2)
                        {
                            result.First(m => m.minutes_id == minutes_id).pictures.Add(new Minutes_pic
                            {
                                file_name = item.ItemArray[data.Columns[Minutes_pic.FieldName.FILE_NAME].Ordinal].ToString(),
                                minutes_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.MINUTES_ID].Ordinal])
                            });
                        }

                        //Add file_name to delete into dummy obj : minute_id == 1 || minutes_id == -2
                        else
                        {
                            dummyfordeleteminutes.pictures.Add(new Minutes_pic
                            {
                                file_name = item.ItemArray[data.Columns[Minutes_pic.FieldName.FILE_NAME].Ordinal].ToString()
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
            result.Add(dummyfordeleteminutes);
            return result;
        }
    }
}