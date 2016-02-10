using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
using educationalProject.Models.ViewModels.Wrappers;
namespace educationalProject.Models.Wrappers
{
    public class oTeacher : Teacher
    {
        public static string getSelectTeacherByJoinCommand()
        {
            return string.Format("select {0}.{1},{2},{3},{4},{5}," +
            "{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}," +
            "{16},{17},{18},{19},{20},{21},{22} " +
            "from {23},{0} where {1} = {24}",
            /**tablename 0 **/ FieldName.TABLE_NAME, /**iden 1**/ FieldName.TEACHER_ID, FieldName.USER_TYPE, FieldName.USERNAME,
            FieldName.PASSWORD, FieldName.T_PRENAME, FieldName.T_NAME, FieldName.E_PRENAME, FieldName.E_NAME,
            FieldName.CITIZEN_ID, FieldName.GENDER, FieldName.EMAIL, FieldName.TEL, FieldName.ADDR,
            FieldName.FILE_NAME_PIC, FieldName.TIMESTAMP,  /***common 15***/

            /**extended data**/
            FieldName.ROOM, FieldName.DEGREE, FieldName.POSITION, FieldName.PERSONNEL_TYPE, FieldName.PERSON_ID,
            FieldName.STATUS, FieldName.ALIVE,

            User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID);
        }
        public object SelectTeacherIdAndTName(string curri_id)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<Teacher_with_t_name> result = new List<Teacher_with_t_name>();
            d.iCommand.CommandText = string.Format("select * from ({0}) as {6} where exists(select * from {1} where {6}.{2} = {1}.{3} and {4}='{5}')", 
                getSelectTeacherByJoinCommand(), User_curriculum.FieldName.TABLE_NAME,FieldName.TEACHER_ID,User_curriculum.FieldName.USER_ID,User_curriculum.FieldName.CURRI_ID,curri_id,
                FieldName.ALIAS_NAME);
            try  
            {  
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(new Teacher_with_t_name
                        {
                            teacher_id = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TEACHER_ID].Ordinal]),
                            t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[FieldName.T_PRENAME].Ordinal].ToString()) + 
                                     item.ItemArray[data.Columns[FieldName.T_NAME].Ordinal].ToString()
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


        public object SelectPresidentCurriAndAllTeacherInCurri(Curriculum_academic data)
        {
            int president;
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oTeacher_educational> result = new List<oTeacher_educational>();
            d.iCommand.CommandText = string.Format
                ("select * from " +
                 "(select p1.{0} from {1} as p1 where p1.{2} = '{3}' and p1.{4} = {5}) as r1," +
                 "(select * from ({6}) as p2 inner join(select * from {7} where {8} = '{3}') as rr1 on p2.{9} = rr1.{10}) as r2 " +
                 "inner join {11} on r2.{9} = {11}.{12}",
                 FieldName.TEACHER_ID, President_curriculum.FieldName.TABLE_NAME, President_curriculum.FieldName.CURRI_ID,
                 data.curri_id, President_curriculum.FieldName.ACA_YEAR, data.aca_year, getSelectTeacherByJoinCommand(),
                 User_curriculum.FieldName.TABLE_NAME, User_curriculum.FieldName.CURRI_ID, FieldName.TEACHER_ID,
                 User_curriculum.FieldName.USER_ID, Educational_teacher_staff.FieldName.TABLE_NAME,
                 Educational_teacher_staff.FieldName.PERSONNEL_ID);
            try
            {
                //Read teacher-eduhistory data with president curriculum's teacher id
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable tabledata = new DataTable();
                    tabledata.Load(res);
                    president = Convert.ToInt32(tabledata.Rows[0].ItemArray[0]);
                    oTeacher_educational t = null;
                    teacher_id = -1;
                    foreach (DataRow item in tabledata.Rows)
                    {
                        if (Convert.ToInt32(item.ItemArray[1]) != teacher_id)
                        {
                            t = new oTeacher_educational
                            {
                                addr = item.ItemArray[tabledata.Columns[FieldName.ADDR].Ordinal].ToString(),
                                username = item.ItemArray[tabledata.Columns[FieldName.USERNAME].Ordinal].ToString(),
                                t_name = item.ItemArray[tabledata.Columns[FieldName.T_NAME].Ordinal].ToString(),
                                e_name = item.ItemArray[tabledata.Columns[FieldName.E_NAME].Ordinal].ToString(),
                                email = item.ItemArray[tabledata.Columns[FieldName.EMAIL].Ordinal].ToString(),
                                gender = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.GENDER].Ordinal]),
                                degree = Convert.ToChar(item.ItemArray[17]),
                                teacher_id = Convert.ToInt32(item.ItemArray[1]),
                                tel = item.ItemArray[tabledata.Columns[FieldName.TEL].Ordinal].ToString(),
                                e_prename = item.ItemArray[tabledata.Columns[FieldName.E_PRENAME].Ordinal].ToString(),
                                t_prename = NameManager.GatherPreName(item.ItemArray[tabledata.Columns[FieldName.T_PRENAME].Ordinal].ToString()),
                                personnel_type = item.ItemArray[tabledata.Columns[FieldName.PERSONNEL_TYPE].Ordinal].ToString(),
                                status = item.ItemArray[tabledata.Columns[FieldName.STATUS].Ordinal].ToString(),
                                user_type = item.ItemArray[tabledata.Columns[FieldName.USER_TYPE].Ordinal].ToString(),
                                file_name_pic = item.ItemArray[tabledata.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                                position = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.POSITION].Ordinal]),
                                
                                
                            };
                            teacher_id = t.teacher_id;
                            result.Add(t);
                        }
                        t.history.Add(new Educational_teacher_staff
                        {
                            college = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString(),
                            degree = Convert.ToChar(item.ItemArray[26]),
                            grad_year = Convert.ToInt32(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal]),
                            major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.MAJOR].Ordinal].ToString(),
                            personnel_id = Convert.ToInt32(item.ItemArray[25]),
                            pre_major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.PRE_MAJOR].Ordinal].ToString(),
                        });
                    }
                    res.Close();
                    tabledata.Dispose();
                    d.iCommand.CommandText = string.Format("select * from ({0}) as {8} where " +
                                       "{1} IN(select {2} from {3} where {4} = '{5}') and " +
                                       "not exists(select * from {6} where {8}.{1} = {6}.{7})",
                                       getSelectTeacherByJoinCommand(), FieldName.TEACHER_ID, User_curriculum.FieldName.USER_ID,
                                       User_curriculum.FieldName.TABLE_NAME, User_curriculum.FieldName.CURRI_ID,
                                       data.curri_id, Educational_teacher_staff.FieldName.TABLE_NAME, Educational_teacher_staff.FieldName.PERSONNEL_ID,
                                       FieldName.ALIAS_NAME);
                    res = d.iCommand.ExecuteReader();
                    //read teacher data without eduhistory
                    if (res.HasRows)
                    {
                        tabledata = new DataTable();
                        tabledata.Load(res);
                        foreach (DataRow item in tabledata.Rows)
                        {
                            result.Add(new oTeacher_educational
                            {
                                addr = item.ItemArray[tabledata.Columns[FieldName.ADDR].Ordinal].ToString(),
                                username = item.ItemArray[tabledata.Columns[FieldName.USERNAME].Ordinal].ToString(),
                                t_name = item.ItemArray[tabledata.Columns[FieldName.T_NAME].Ordinal].ToString(),
                                e_name = item.ItemArray[tabledata.Columns[FieldName.E_NAME].Ordinal].ToString(),
                                email = item.ItemArray[tabledata.Columns[FieldName.EMAIL].Ordinal].ToString(),
                                gender = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.GENDER].Ordinal]),
                                degree = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.DEGREE].Ordinal]),
                                teacher_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[FieldName.TEACHER_ID].Ordinal]),
                                tel = item.ItemArray[tabledata.Columns[FieldName.TEL].Ordinal].ToString(),
                                e_prename = item.ItemArray[tabledata.Columns[FieldName.E_PRENAME].Ordinal].ToString(),
                                t_prename = NameManager.GatherPreName(item.ItemArray[tabledata.Columns[FieldName.T_PRENAME].Ordinal].ToString()),
                                personnel_type = item.ItemArray[tabledata.Columns[FieldName.PERSONNEL_TYPE].Ordinal].ToString(),
                                status = item.ItemArray[tabledata.Columns[FieldName.STATUS].Ordinal].ToString(),
                                user_type = item.ItemArray[tabledata.Columns[FieldName.USER_TYPE].Ordinal].ToString(),
                                file_name_pic = item.ItemArray[tabledata.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                                position = Convert.ToChar(item.ItemArray[tabledata.Columns[FieldName.POSITION].Ordinal])
                            });
                        }
                        res.Close();
                        tabledata.Dispose();
                    }
                    //Find president curriculum in list and swap both
                    int swapindex = 0;
                    foreach (oTeacher_educational i in result)
                    {
                        if (i.teacher_id == president)
                            break;
                        swapindex++;
                    }
                    ListUtils<oTeacher_educational>.Swap(result, 0, swapindex);
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

        public object Insert(List<UsernamePassword> list, List<string> target_curri_id_list)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<string> result = new List<string>();

            string temp5tablename = "#temp5";
            string temp6tablename = "#temp6";
            string temp7tablename = "#temp7";
            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(60) NULL," +
                                      "PRIMARY KEY ([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} VARCHAR(60) COLLATE DATABASE_DEFAULT ",
                                      temp5tablename, FieldName.EMAIL);

            string createtabletemp6 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] INT NULL," +
                                      "PRIMARY KEY ([row_num])) ",
                                      temp6tablename, User_list.FieldName.USER_ID);


            string createtabletemp7 = string.Format("CREATE TABLE {0}(" +
                          "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                          "[{1}] {2} NULL," +
                          "PRIMARY KEY ([row_num])) " +
                          "ALTER TABLE {0} " +
                          "ALTER COLUMN {1} {2} COLLATE DATABASE_DEFAULT ",
                          temp7tablename, User_curriculum.FieldName.CURRI_ID,DBFieldDataType.CURRI_ID_TYPE);

            string insertintotemp7 = string.Format("insert into {0} values ", temp7tablename);

            int len = insertintotemp7.Length;

            foreach (string curriitem in target_curri_id_list)
            {
                if (insertintotemp7.Length <= len)
                    insertintotemp7 += string.Format("('{0}')", curriitem);
                else
                    insertintotemp7 += string.Format(",('{0}')", curriitem);
            }

            string insertintousercurri = "";
            if (insertintotemp7.Length > len)
                insertintousercurri = string.Format("insert into {0} " +
                                      "select {1},{2} from {3},{4} ",
                                      User_curriculum.FieldName.TABLE_NAME,
                                      User_curriculum.FieldName.USER_ID,
                                      User_curriculum.FieldName.CURRI_ID, temp6tablename, temp7tablename);
            else
                insertintotemp7 = "";

            string insertcmd = "";

            foreach (UsernamePassword item in list)
            {   
                string ts = DateTime.Now.GetDateTimeFormats(new System.Globalization.CultureInfo("en-US"))[93];

                insertcmd += string.Format(
                                   "IF NOT EXISTS(select * from {3} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {5} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {6} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {7} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {8} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {9} where {4} = '{2}' or {13} = '{2}') and " +
                                   "NOT EXISTS(select * from {20} where {4} = '{2}' or {13} = '{2}') " +
                                   "begin " +
                                   "insert into {19} " +
                                   "select * from (insert into {0} output inserted.{1} values ('{10}')) as outputinsert " +

                                   "insert into {3} ({11}, {12}, {13}, {14}, {4}, {15}) select {1},'{10}', '{2}', '{16}', '{2}', '{17}' from {19} " +
                                   insertintousercurri + " " +
                                   
                                   "delete from {19} " +
                                   "end " +
                                   "else " +
                                   "begin " +
                                   "insert into {18} values ('{2}') " +
                                   "end ",
                                   User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID, item.username,
                                   /*Main table index 3 must SWAP!*/ FieldName.TABLE_NAME,
                                   FieldName.EMAIL, Student.FieldName.TABLE_NAME,
                                   Alumni.ExtraFieldName.TABLE_NAME, Staff.FieldName.TABLE_NAME,
                                   Company.FieldName.TABLE_NAME, Assessor.FieldName.TABLE_NAME,
                                   /*******10*/ "อาจารย์",
                                   /*******11 ID*/FieldName.TEACHER_ID, FieldName.USER_TYPE, FieldName.USERNAME,
                                   FieldName.PASSWORD, FieldName.TIMESTAMP, item.password, ts, temp5tablename,
                                   temp6tablename,
                                   Admin.FieldName.TABLE_NAME);

            }

            string selectcmd = string.Format("select {1} from {0} ", temp5tablename, FieldName.EMAIL);



        
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} END ", createtabletemp5,createtabletemp6,createtabletemp7,
                insertintotemp7,insertcmd,selectcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        result.Add(
                            item.ItemArray[data.Columns[FieldName.EMAIL].Ordinal].ToString()
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
            if (result.Count != 0)
                return result;
            else
                return null;
        }
    }
}