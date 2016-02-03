using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oPersonnel : Personnel
    {
        private static readonly string PERSONNEL_ID = "PERSONNEL_ID";
        private static readonly string USER_TYPE_NUM = "USER_TYPE_NUM";
        public object SelectPersonnelIdAndTName(string curri_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Personnel_with_t_name> result = new List<Personnel_with_t_name>();

            string temp1tablename = "#temp1";

            string createtabletemp1 = string.Format("create table {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[user_type] INT NOT NULL," +
                                      "[{1}] VARCHAR(5) NOT NULL," +
                                      "[{2}] VARCHAR(16) NULL," +
                                      "[{3}] VARCHAR(60) NULL," +
                                      "PRIMARY KEY([row_num])) " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(5) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {2} VARCHAR(16) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {3} VARCHAR(60) COLLATE DATABASE_DEFAULT ",
                                      temp1tablename,PERSONNEL_ID,
                                      Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME);

            string insertintotemp1_1 = string.Format("INSERT INTO {0} " +
                                       "select 1,{1},{2},{3} from {4} where " +
                                       "exists(select * from {5} where {4}.{1} = {5}.{6} and {7}='{8}')",
                                       temp1tablename, Teacher.FieldName.TEACHER_ID,
                                      Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Teacher.FieldName.TABLE_NAME,
                                      Curriculum_teacher_staff.FieldName.TABLE_NAME,
                                      Curriculum_teacher_staff.FieldName.PERSONNEL_ID,
                                      Curriculum_teacher_staff.FieldName.CURRI_ID, curri_id);

            string insertintotemp1_2 = string.Format("INSERT INTO {0} " +
                                       "select 2,{1},{2},{3} from {4} where " +
                                       "exists(select * from {5} where {4}.{1} = {5}.{6} and {7}='{8}')",
                                       temp1tablename, Staff.FieldName.STAFF_ID,
                                      Staff.FieldName.T_PRENAME, Staff.FieldName.T_NAME, Staff.FieldName.TABLE_NAME,
                                      Curriculum_teacher_staff.FieldName.TABLE_NAME, 
                                      Curriculum_teacher_staff.FieldName.PERSONNEL_ID,
                                      Curriculum_teacher_staff.FieldName.CURRI_ID, curri_id);

            string selectcmd = string.Format("select user_type,{0},{1},{2} from {3} ", PERSONNEL_ID,
                                      Staff.FieldName.T_PRENAME, Staff.FieldName.T_NAME, temp1tablename);


            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} END",createtabletemp1,
                insertintotemp1_1,insertintotemp1_2,selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        if(Convert.ToInt32(item.ItemArray[data.Columns["user_type"].Ordinal]) == 1)
                        result.Add(new Personnel_with_t_name
                        {
                            personnel_id = item.ItemArray[data.Columns[PERSONNEL_ID].Ordinal].ToString(),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                     item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                        });
                        else
                            result.Add(new Personnel_with_t_name
                            {
                                personnel_id = item.ItemArray[data.Columns[PERSONNEL_ID].Ordinal].ToString(),
                                t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString() +
                                     item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                            });
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

        public object selectWithFullDetail(string curri_id_data)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Personnel_educational> result = new List<Personnel_educational>();

            string temp1tablename = "#temp1";

            string createtabletemp1 = string.Format("CREATE TABLE {0} (" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(5) NOT NULL," +
                                      "[{2}] VARCHAR(40) NULL," +
                                      "[{21}] INT NULL," +
                                      "[{3}] VARCHAR(20) NULL," +
                                      "[{4}] VARCHAR(16) NULL," +
                                      "[{5}] VARCHAR(60) NULL," +
                                      "[{6}] VARCHAR(16) NULL," +
                                      "[{7}] VARCHAR(60) NULL," +
                                      "[{8}] CHAR(13) NULL," +
                                      "[{9}] CHAR NULL," +
                                      "[{10}] VARCHAR(60) NULL," +
                                      "[{11}] VARCHAR(20) NULL," +
                                      "[{12}] VARCHAR(80) NULL," +
                                      "[{13}] VARCHAR(255) NULL," +
                                      "[{14}] DATETIME2 NULL," +

                                      "[{22}] VARCHAR(40) NULL," +

                                      "[{15}] VARCHAR(4) NULL," +
                                      "[{16}] CHAR NULL," +
                                      "[{17}] VARCHAR(100) NULL," +
                                      "[{18}] VARCHAR(200) NULL," +
                                      "[{19}] INT NULL," +
                                      "[{20}] VARCHAR(200) NULL," +
                                      "PRIMARY KEY([row_num])) " + 

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(5) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {2} VARCHAR(40) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {3} VARCHAR(20) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {4} VARCHAR(16) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {5} VARCHAR(60) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {6} VARCHAR(16) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {7} VARCHAR(60) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {8} CHAR(13) COLLATE DATABASE_DEFAULT " +



                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {9} CHAR COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {10} VARCHAR(60) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {11} VARCHAR(20) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {12} VARCHAR(80) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {13} VARCHAR(255) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {15} VARCHAR(4) COLLATE DATABASE_DEFAULT " +
                                                                            
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {16} CHAR COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {17} VARCHAR(100) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {18} VARCHAR(200) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {20} VARCHAR(200) COLLATE DATABASE_DEFAULT " +

                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {22} VARCHAR(40) COLLATE DATABASE_DEFAULT " ,
             
                                      temp1tablename,FieldName.PERSONNEL_ID,FieldName.USER_TYPE,FieldName.USERNAME,
                                      FieldName.T_PRENAME,FieldName.T_NAME,FieldName.E_PRENAME,FieldName.E_NAME,
                                      FieldName.CITIZEN_ID,FieldName.GENDER,FieldName.EMAIL,FieldName.TEL,
                                      FieldName.ADDR,FieldName.FILE_NAME_PIC,FieldName.TIMESTAMP,
                                      Curriculum_teacher_staff.FieldName.CURRI_ID,Educational_teacher_staff.FieldName.DEGREE,
                                      Educational_teacher_staff.FieldName.PRE_MAJOR, Educational_teacher_staff.FieldName.MAJOR,
                                      Educational_teacher_staff.FieldName.GRAD_YEAR,
                                      Educational_teacher_staff.FieldName.COLLEGE,USER_TYPE_NUM,FieldName.ROOM);



            string insertintotemp1_1 = string.Format("INSERT INTO {0} " +
                                       "select {1},{2},1,{3},{4},{5},{6},{7},{8},{9},{10}," +
                                       "{11}, {12}, {13}, {14},{27}, {15}, {16}.{17}, {18}, {19}, {20}, {21} " + 
                                       "from {22}, {23}, {16} where " +
                                       "{1} = {23}.{24} and {15} = '{25}' and {16}.{26} = {1} ",
                                       temp1tablename,Teacher.FieldName.TEACHER_ID,Teacher.FieldName.USER_TYPE, Teacher.FieldName.USERNAME,
                                       Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Teacher.FieldName.E_PRENAME, Teacher.FieldName.E_NAME,
                                       Teacher.FieldName.CITIZEN_ID, Teacher.FieldName.GENDER,Teacher.FieldName.EMAIL,
                                       Teacher.FieldName.TEL, Teacher.FieldName.ADDR, Teacher.FieldName.FILE_NAME_PIC,
                                       Teacher.FieldName.TIMESTAMP,FieldName.CURRI_ID,Educational_teacher_staff.FieldName.TABLE_NAME /*16*/,
                                       Educational_teacher_staff.FieldName.DEGREE, Educational_teacher_staff.FieldName.PRE_MAJOR,
                                       Educational_teacher_staff.FieldName.MAJOR, Educational_teacher_staff.FieldName.GRAD_YEAR,
                                       Educational_teacher_staff.FieldName.COLLEGE,Teacher.FieldName.TABLE_NAME,
                                       Curriculum_teacher_staff.FieldName.TABLE_NAME, Curriculum_teacher_staff.FieldName.PERSONNEL_ID,
                                       curri_id_data,Educational_teacher_staff.FieldName.PERSONNEL_ID,FieldName.ROOM
                                       );


            string insertintotemp1_2 = string.Format("INSERT INTO {0} " +
                                       "select {1},{2},1,{3},{4},{5},{6},{7},{8},{9},{10}," +
                                       "{11}, {12}, {13}, {14},{23}, {15}, null, null, null, null, null " +
                                       "from {16}, {17} where " +
                                       "{1} = {17}.{18} and {19} = '{20}' and not exists " +
                                       "(select * from {21} where {1} = {22}) ",
                                       temp1tablename, Teacher.FieldName.TEACHER_ID, Teacher.FieldName.USER_TYPE, Teacher.FieldName.USERNAME,
                                       Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME, Teacher.FieldName.E_PRENAME, Teacher.FieldName.E_NAME,
                                       Teacher.FieldName.CITIZEN_ID, Teacher.FieldName.GENDER, Teacher.FieldName.EMAIL,
                                       Teacher.FieldName.TEL, Teacher.FieldName.ADDR, Teacher.FieldName.FILE_NAME_PIC,
                                       Teacher.FieldName.TIMESTAMP, FieldName.CURRI_ID,Teacher.FieldName.TABLE_NAME,
                                       Curriculum_teacher_staff.FieldName.TABLE_NAME, Curriculum_teacher_staff.FieldName.PERSONNEL_ID,
                                       Curriculum_teacher_staff.FieldName.CURRI_ID,curri_id_data,Educational_teacher_staff.FieldName.TABLE_NAME,
                                       Educational_teacher_staff.FieldName.PERSONNEL_ID,FieldName.ROOM);

            string insertintotemp1_3 = string.Format("INSERT INTO {0} " +
                           "select {1},{2},2,{3},{4},{5},{6},{7},{8},{9},{10}," +
                           "{11}, {12}, {13}, {14},{27}, {15}, {16}.{17}, {18}, {19}, {20}, {21} " +
                           "from {22}, {23}, {16} where " +
                           "{1} = {23}.{24} and {15} = '{25}' and {16}.{26} = {1} ",
                           temp1tablename, Staff.FieldName.STAFF_ID, Staff.FieldName.USER_TYPE, Staff.FieldName.USERNAME,
                           Staff.FieldName.T_PRENAME, Staff.FieldName.T_NAME, Staff.FieldName.E_PRENAME, Staff.FieldName.E_NAME,
                           Staff.FieldName.CITIZEN_ID, Staff.FieldName.GENDER, Staff.FieldName.EMAIL,
                           Staff.FieldName.TEL, Staff.FieldName.ADDR, Staff.FieldName.FILE_NAME_PIC,
                           Staff.FieldName.TIMESTAMP, FieldName.CURRI_ID, Educational_teacher_staff.FieldName.TABLE_NAME /*16*/,
                           Educational_teacher_staff.FieldName.DEGREE, Educational_teacher_staff.FieldName.PRE_MAJOR,
                           Educational_teacher_staff.FieldName.MAJOR, Educational_teacher_staff.FieldName.GRAD_YEAR,
                           Educational_teacher_staff.FieldName.COLLEGE, Staff.FieldName.TABLE_NAME,
                           Curriculum_teacher_staff.FieldName.TABLE_NAME, Curriculum_teacher_staff.FieldName.PERSONNEL_ID,
                           curri_id_data, Educational_teacher_staff.FieldName.PERSONNEL_ID,FieldName.ROOM
                           );


            string insertintotemp1_4 = string.Format("INSERT INTO {0} " +
                           "select {1},{2},2,{3},{4},{5},{6},{7},{8},{9},{10}," +
                           "{11}, {12}, {13}, {14}, {23},{15}, null, null, null, null, null " +
                           "from {16}, {17} where " +
                           "{1} = {17}.{18} and {19} = '{20}' and not exists " +
                           "(select * from {21} where {1} = {22}) ",
                           temp1tablename, Staff.FieldName.STAFF_ID, Staff.FieldName.USER_TYPE, Staff.FieldName.USERNAME,
                           Staff.FieldName.T_PRENAME,Staff.FieldName.T_NAME, Staff.FieldName.E_PRENAME, Staff.FieldName.E_NAME,
                           Staff.FieldName.CITIZEN_ID, Staff.FieldName.GENDER, Staff.FieldName.EMAIL,
                           Staff.FieldName.TEL, Staff.FieldName.ADDR, Staff.FieldName.FILE_NAME_PIC,
                           Staff.FieldName.TIMESTAMP, FieldName.CURRI_ID, Staff.FieldName.TABLE_NAME,
                           Curriculum_teacher_staff.FieldName.TABLE_NAME, Curriculum_teacher_staff.FieldName.PERSONNEL_ID,
                           Curriculum_teacher_staff.FieldName.CURRI_ID, curri_id_data, Educational_teacher_staff.FieldName.TABLE_NAME,
                           Educational_teacher_staff.FieldName.PERSONNEL_ID,FieldName.ROOM);

            
            string selectcmd = string.Format("select * from {0} ",temp1tablename);


            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} END", createtabletemp1,
                insertintotemp1_1, insertintotemp1_2, insertintotemp1_3, insertintotemp1_4, selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        personnel_id = item.ItemArray[data.Columns[PERSONNEL_ID].Ordinal].ToString();
                        if(result.FirstOrDefault(p => p.personnel_id == personnel_id) == null)
                        {
                            if (Convert.ToInt32(item.ItemArray[data.Columns[USER_TYPE_NUM].Ordinal]) == 1)
                                result.Add(new Personnel_educational
                                {
                                    personnel_id = item.ItemArray[data.Columns[PERSONNEL_ID].Ordinal].ToString(),
                                    addr = item.ItemArray[data.Columns[FieldName.ADDR].Ordinal].ToString(),
                                    citizen_id = item.ItemArray[data.Columns[FieldName.CITIZEN_ID].Ordinal].ToString(),
                                    curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                    email = item.ItemArray[data.Columns[FieldName.EMAIL].Ordinal].ToString(),
                                    e_name = item.ItemArray[data.Columns[FieldName.E_NAME].Ordinal].ToString(),
                                    e_prename = item.ItemArray[data.Columns[FieldName.E_PRENAME].Ordinal].ToString(),
                                    file_name_pic = item.ItemArray[data.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                                    gender = Convert.ToChar(item.ItemArray[data.Columns[FieldName.GENDER].Ordinal]),
                                    tel = item.ItemArray[data.Columns[FieldName.TEL].Ordinal].ToString(),
                                    timestamp = item.ItemArray[data.Columns[FieldName.TIMESTAMP].Ordinal].ToString(),
                                    room = item.ItemArray[data.Columns[FieldName.ROOM].Ordinal].ToString(),
                                    username = item.ItemArray[data.Columns[FieldName.USERNAME].Ordinal].ToString(),
                                    user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString(),
                                    t_prename = item.ItemArray[data.Columns[FieldName.T_PRENAME].Ordinal].ToString(),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                             item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                                });
                            else
                                result.Add(new Personnel_educational
                                {
                                    personnel_id = item.ItemArray[data.Columns[PERSONNEL_ID].Ordinal].ToString(),
                                    addr = item.ItemArray[data.Columns[FieldName.ADDR].Ordinal].ToString(),
                                    citizen_id = item.ItemArray[data.Columns[FieldName.CITIZEN_ID].Ordinal].ToString(),
                                    curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                    email = item.ItemArray[data.Columns[FieldName.EMAIL].Ordinal].ToString(),
                                    e_name = item.ItemArray[data.Columns[FieldName.E_NAME].Ordinal].ToString(),
                                    e_prename = item.ItemArray[data.Columns[FieldName.E_PRENAME].Ordinal].ToString(),
                                    file_name_pic = item.ItemArray[data.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                                    gender = Convert.ToChar(item.ItemArray[data.Columns[FieldName.GENDER].Ordinal]),
                                    tel = item.ItemArray[data.Columns[FieldName.TEL].Ordinal].ToString(),
                                    timestamp = item.ItemArray[data.Columns[FieldName.TIMESTAMP].Ordinal].ToString(),
                                    room = item.ItemArray[data.Columns[FieldName.ROOM].Ordinal].ToString(),
                                    username = item.ItemArray[data.Columns[FieldName.USERNAME].Ordinal].ToString(),
                                    user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString(),
                                    t_prename = item.ItemArray[data.Columns[FieldName.T_PRENAME].Ordinal].ToString(),
                                    t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString() +
                                             item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                                });
                        }

                        //If degree col is not null (mean that teacher have educational history:let's add it!)
                        if(item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.DEGREE].Ordinal].ToString() != "")
                        {
                            result.First(p => p.personnel_id == personnel_id).history.Add(new Educational_teacher_staff
                            {
                                college = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString(),
                                degree = Convert.ToChar(item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.DEGREE].Ordinal]),
                                grad_year = Convert.ToInt32(item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal]),
                                pre_major = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.PRE_MAJOR].Ordinal].ToString(),
                                major = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.MAJOR].Ordinal].ToString(),
                                personnel_id = item.ItemArray[data.Columns[Educational_teacher_staff.FieldName.PERSONNEL_ID].Ordinal].ToString()
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
    }
}