using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Utils;
using educationalProject.Models.Wrappers;
namespace educationalProject.Models.ViewModels.Wrappers
{
    public class oTeacher_educational : Teacher_educational
    {
        public object SelectPresidentCurriAndAllTeacherInCurri(Curriculum_academic data)
        {
            int president = -1;
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            List<oTeacher_educational> result = new List<oTeacher_educational>();
            result.Add(new oTeacher_educational());

            string temp90tablename = "#temp90";
            string createtabletemp90 = string.Format("CREATE TABLE {0} (" +
            "[row_num] INT IDENTITY(1,1) NOT NULL," +
            "[{1}] INT NULL," +
            "[{2}] VARCHAR(40) NULL," +
            "[{3}] {21} NULL," +
            "[{4}] VARCHAR(MAX) NULL," +
            "[{5}] VARCHAR(16) NULL," +
            "[{6}] VARCHAR(60) NULL," +
            "[{7}] VARCHAR(16) NULL," +
            "[{8}] VARCHAR(60) NULL," +
            "[{9}] CHAR(13) NULL," +
            "[{10}] CHAR NULL," +
            "[{11}] VARCHAR(60) NULL," +
            "[{12}] VARCHAR(20) NULL," +
            "[{13}] VARCHAR(80) NULL," +
            "[{14}] {22} NULL," +
            "[{15}] DATETIME2 NULL," +
            "[{16}] CHAR NULL," +
            "[{17}] VARCHAR(100) NULL," +
            "[{18}] VARCHAR(200) NULL," +
            "[{19}] INT NULL," +
            "[{20}] VARCHAR(200) NULL," +
            "PRIMARY KEY ([row_num])) " +

            "alter table {0} " +
            "alter column [{2}] VARCHAR(40) collate database_default " +

            "alter table {0} " +
            "alter column [{3}] {21} collate database_default " +

            "alter table {0} " +
            "alter column [{4}] VARCHAR(MAX) collate database_default " +

            "alter table {0} " +
            "alter column [{5}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{6}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{7}] VARCHAR(16) collate database_default " +

            "alter table {0} " +
            "alter column [{8}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{9}] CHAR(13) collate database_default " +

            "alter table {0} " +
            "alter column [{10}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{11}] VARCHAR(60) collate database_default " +

            "alter table {0} " +
            "alter column [{12}] VARCHAR(20) collate database_default " +

            "alter table {0} " +
            "alter column [{13}] VARCHAR(80) collate database_default " +

            "alter table {0} " +
            "alter column [{14}] {22} collate database_default " +

            "alter table {0} " +
            "alter column [{16}] CHAR collate database_default " +

            "alter table {0} " +
            "alter column [{17}] VARCHAR(100) collate database_default " +

            "alter table {0} " +
            "alter column [{18}] VARCHAR(200) collate database_default " +

            "alter table {0} " +
            "alter column [{20}] VARCHAR(200) collate database_default ",
            temp90tablename, Personnel.FieldName.USER_ID, Personnel.FieldName.USER_TYPE,
            Personnel.FieldName.USERNAME, FieldName.PASSWORD, Personnel.FieldName.T_PRENAME,
            Personnel.FieldName.T_NAME, Personnel.FieldName.E_PRENAME, Personnel.FieldName.E_NAME,
            Personnel.FieldName.CITIZEN_ID, Personnel.FieldName.GENDER, Personnel.FieldName.EMAIL,
            Personnel.FieldName.TEL, Personnel.FieldName.ADDR, Personnel.FieldName.FILE_NAME_PIC,
            Personnel.FieldName.TIMESTAMP, Educational_teacher_staff.FieldName.DEGREE,
            Educational_teacher_staff.FieldName.PRE_MAJOR, Educational_teacher_staff.FieldName.MAJOR,
            Educational_teacher_staff.FieldName.GRAD_YEAR, Educational_teacher_staff.FieldName.COLLEGE,
            DBFieldDataType.USERNAME_TYPE, DBFieldDataType.FILE_NAME_TYPE);

            string insertintotemp90_1 = string.Format("insert into {0} ({1},{2}) " +
            "select {3},null from {4} where {5} = '{6}' and {7} = {8} ",
            temp90tablename, Personnel.FieldName.USER_ID, Personnel.FieldName.USER_TYPE,
            President_curriculum.FieldName.TEACHER_ID, President_curriculum.FieldName.TABLE_NAME,
            President_curriculum.FieldName.CURRI_ID, data.curri_id, President_curriculum.FieldName.ACA_YEAR,
            data.aca_year);

            string insertintotemp90_2 = string.Format("insert into {0} " +
            "select {1}.*,{2},{3},{4},{5},{6} " +
            "from {1},{7} " +
            "where {8} = {9} and {10} = 'อาจารย์' " +
            "and exists (select * from {11} where {12} = '{13}' and {11}.{14} = {1}.{8}) ",
            temp90tablename, User_list.FieldName.TABLE_NAME, Educational_teacher_staff.FieldName.DEGREE,
            Educational_teacher_staff.FieldName.PRE_MAJOR, Educational_teacher_staff.FieldName.MAJOR,
            Educational_teacher_staff.FieldName.GRAD_YEAR, Educational_teacher_staff.FieldName.COLLEGE,
            Educational_teacher_staff.FieldName.TABLE_NAME, Personnel.FieldName.USER_ID,
            Educational_teacher_staff.FieldName.PERSONNEL_ID, Personnel.FieldName.USER_TYPE,
            User_curriculum.FieldName.TABLE_NAME, User_curriculum.FieldName.CURRI_ID,
            data.curri_id, User_curriculum.FieldName.USER_ID);

            string insertintotemp90_3 = string.Format("insert into {0} " +
            "select {1}.*,null,null,null,null,null from {1},{2} " +
            "where {1}.{3} = {2}.{4} and {5} = 'อาจารย์' and {6} = '{7}' " +
            "and not exists (select * from {8} where {9} = {1}.{3}) ",
            temp90tablename, User_list.FieldName.TABLE_NAME, User_curriculum.FieldName.TABLE_NAME,
            User_list.FieldName.USER_ID, User_curriculum.FieldName.USER_ID, User_list.FieldName.USER_TYPE,
            User_curriculum.FieldName.CURRI_ID, data.curri_id, Educational_teacher_staff.FieldName.TABLE_NAME,
            Educational_teacher_staff.FieldName.PERSONNEL_ID);

            string selectcmd = string.Format("select * from {0} ", temp90tablename);
                
            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} END", createtabletemp90, insertintotemp90_1,
                insertintotemp90_2, insertintotemp90_3, selectcmd);
            try
            {
                //Read teacher-eduhistory data with president curriculum's teacher id
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable tabledata = new DataTable();
                    tabledata.Load(res);
                    foreach (DataRow item in tabledata.Rows)
                    {
                        if (item.ItemArray[tabledata.Columns[FieldName.USER_TYPE].Ordinal].ToString() == "")
                            president = Convert.ToInt32(item.ItemArray[tabledata.Columns[User_list.FieldName.USER_ID].Ordinal]);
                        else
                        {
                            int tid = Convert.ToInt32(item.ItemArray[tabledata.Columns[User_list.FieldName.USER_ID].Ordinal]);
                            if (result.FirstOrDefault(t => t.teacher_id == tid) == null)
                                result.Add(new oTeacher_educational
                                {
                                username = item.ItemArray[tabledata.Columns[FieldName.USERNAME].Ordinal].ToString(),
                                t_name = item.ItemArray[tabledata.Columns[FieldName.T_NAME].Ordinal].ToString(),
                                e_name = item.ItemArray[tabledata.Columns[FieldName.E_NAME].Ordinal].ToString(),
                                email = item.ItemArray[tabledata.Columns[FieldName.EMAIL].Ordinal].ToString(),
                                teacher_id = tid,
                                tel = item.ItemArray[tabledata.Columns[FieldName.TEL].Ordinal].ToString(),
                                e_prename = item.ItemArray[tabledata.Columns[FieldName.E_PRENAME].Ordinal].ToString(),
                                t_prename = NameManager.GatherPreName(item.ItemArray[tabledata.Columns[FieldName.T_PRENAME].Ordinal].ToString()),
                                file_name_pic = item.ItemArray[tabledata.Columns[FieldName.FILE_NAME_PIC].Ordinal].ToString()
                            });

                            if(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString() != "")
                                result.First(t => t.teacher_id == tid).history.Add(new Educational_teacher_staff
                                {
                                    college = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.COLLEGE].Ordinal].ToString(),
                                    degree = Convert.ToChar(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.DEGREE].Ordinal]),
                                    grad_year = Convert.ToInt32(item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.GRAD_YEAR].Ordinal]),
                                    major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.MAJOR].Ordinal].ToString(),
                                    pre_major = item.ItemArray[tabledata.Columns[Educational_teacher_staff.FieldName.PRE_MAJOR].Ordinal].ToString(),
                                });
                        }
                    }
                    
                    if (president != -1) {
                        //Find president curriculum in list and swap both
                        int swapindex = result.FindIndex(t => t.teacher_id == president);
                        ListUtils<oTeacher_educational>.Swap(result, 0, swapindex);
                        result.RemoveAt(swapindex);
                    }
                    tabledata.Dispose();         
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