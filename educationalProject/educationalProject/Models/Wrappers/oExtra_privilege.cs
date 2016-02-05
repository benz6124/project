using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oExtra_privilege : Extra_privilege
    {
        public object SelectByCurriculumAndTitle()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            Extra_privilege_individual_list_with_privilege_choices result = new Extra_privilege_individual_list_with_privilege_choices();

            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] {8} NULL," +
                                      "[{2}] {9} NULL," +
                                      "[{3}] VARCHAR(80) NULL," +
                                      "[{4}] VARCHAR(80) NULL," +
                                      "[{5}] VARCHAR(16) NULL," +
                                      "[{6}] VARCHAR(60) NULL," +
                                      "[{7}] {10} NULL," +
                                      "[{11}] VARCHAR(40) NULL," +
                                      "PRIMARY KEY([row_num])) " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {1} {8} COLLATE DATABASE_DEFAULT " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {2} {9} COLLATE DATABASE_DEFAULT " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {3} VARCHAR(80) COLLATE DATABASE_DEFAULT " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {4} VARCHAR(80) COLLATE DATABASE_DEFAULT "+
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {5} VARCHAR(16) COLLATE DATABASE_DEFAULT "+
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {6} VARCHAR(60) COLLATE DATABASE_DEFAULT "+
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {7} {10} COLLATE DATABASE_DEFAULT " +
                                      "ALTER TABLE {0} " +
                                      "ALTER COLUMN {11} VARCHAR(40) COLLATE DATABASE_DEFAULT ",
                                      temp5tablename,FieldName.PERSONNEL_ID,FieldName.CURRI_ID,
                                      FieldName.TITLE,FieldName.PRIVILEGE,Teacher.FieldName.T_PRENAME,
                                      Teacher.FieldName.T_NAME,Teacher.FieldName.FILE_NAME_PIC,
                                      DBFieldDataType.USER_ID_TYPE,DBFieldDataType.CURRI_ID_TYPE,
                                      DBFieldDataType.FILE_NAME_TYPE,Teacher.FieldName.USER_TYPE);

            //teacher table name = index 5
            //userid = index 7
            string insertintotemp5_1 = string.Format("insert into {0} " +
                                       "select {1}.*,{2},{3},{4},{5}.{17} " +
                                       "from {1},{5} " +
                                       "where {1}.{6} = {5}.{7} " +
                                       "and {8} = '{9}' and {10} = '{11}' and exists " +
                                       "(select * from {12} where {13} = '{9}' and {7} = {14}) " +

                                       "if exists(select * from {5},{16} where " +
                                       "{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18}) " +
                                       "begin " +
                                       "insert into {0} " +
                                       "select {7},{8},{10},{15},{2},{3},{4},{5}.{17} from {5},{16} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18} " +
                                       "and exists (select * from {12} where {13} = '{9}' and {7} = {14}) " +
                                       "end " +

                                       "ELSE " +
                                       "BEGIN " +
                                       "insert into {0} " +
                                       "select {7},'{9}',{10},{15},{2},{3},{4},{5}.{17} from {5}, {19} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{10} = '{11}' and {5}.{17} = {19}.{20} " +
                                       "and exists (select * from {12} where {13} = '{9}' and {7} = {14}) " +
                                       "END ",
                                       temp5tablename, FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                                       Teacher.FieldName.FILE_NAME_PIC,
                                     /**5***/Teacher.FieldName.TABLE_NAME,
                                       FieldName.PERSONNEL_ID,
                                    /**7***/Teacher.FieldName.TEACHER_ID,
                                       FieldName.CURRI_ID, curri_id, FieldName.TITLE, title, User_curriculum.FieldName.TABLE_NAME,
                                       User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.USER_ID,FieldName.PRIVILEGE,
                                       Extra_privilege_by_type.FieldName.TABLE_NAME,
                                    /*17**/ Teacher.FieldName.USER_TYPE,Extra_privilege_by_type.FieldName.USER_TYPE,
                                       Default_privilege_by_type.FieldName.TABLE_NAME,Default_privilege_by_type.FieldName.USER_TYPE);


            string insertintotemp5_2 = string.Format("insert into {0} " +
                                       "select {1}.*,{2},{3},{4},{5}.{17} " +
                                       "from {1},{5} " +
                                       "where {1}.{6} = {5}.{7} " +
                                       "and {1}.{8} = '{9}' and {10} = '{11}' and exists " +
                                       "(select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +

                                       "if exists(select * from {5},{16} where " +
                                       "{16}.{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18}) " +
                                       "begin " +
                                       "insert into {0} " +
                                       "select {7},{16}.{8},{10},{15},{2},{3},{4},{5}.{17} from {5},{16} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{16}.{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18} " +
                                       "and exists (select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +
                                       "end " +

                                       "ELSE " +
                                       "BEGIN " +
                                       "insert into {0} " +
                                       "select {7},'{9}',{10},{15},{2},{3},{4},{5}.{17} from {5}, {19} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{10} = '{11}' and {5}.{17} = {19}.{20} " +
                                       "and exists (select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +
                                       "END ",
                                       temp5tablename, FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                                       Teacher.FieldName.FILE_NAME_PIC,
                                     /**5***/Student.FieldName.TABLE_NAME,
                                       FieldName.PERSONNEL_ID,
                                    /**7***/Student.FieldName.STUDENT_ID,
                                       FieldName.CURRI_ID, curri_id, FieldName.TITLE, title, User_curriculum.FieldName.TABLE_NAME,
                                       User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.USER_ID, FieldName.PRIVILEGE,
                                       Extra_privilege_by_type.FieldName.TABLE_NAME,
                                    /*17**/ Student.FieldName.USER_TYPE, Extra_privilege_by_type.FieldName.USER_TYPE,
                                       Default_privilege_by_type.FieldName.TABLE_NAME, Default_privilege_by_type.FieldName.USER_TYPE);


            string insertintotemp5_3 = string.Format("insert into {0} " +
                                       "select {1}.*,{2},{3},{4},{5}.{17} " +
                                       "from {1},{5} " +
                                       "where {1}.{6} = {5}.{7} " +
                                       "and {1}.{8} = '{9}' and {10} = '{11}' and exists " +
                                       "(select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +

                                       "if exists(select * from {5},{16} where " +
                                       "{16}.{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18}) " +
                                       "begin " +
                                       "insert into {0} " +
                                       "select {7},{16}.{8},{10},{15},{2},{3},{4},{5}.{17} from {5},{16} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{16}.{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18} " +
                                       "and exists (select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +
                                       "end " +

                                       "ELSE " +
                                       "BEGIN " +
                                       "insert into {0} " +
                                       "select {7},'{9}',{10},{15},{2},{3},{4},{5}.{17} from {5}, {19} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{10} = '{11}' and {5}.{17} = {19}.{20} " +
                                       "and exists (select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +
                                       "END ",
                                       temp5tablename, FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                                       Teacher.FieldName.FILE_NAME_PIC,
                                     /**5***/Alumni.ExtraFieldName.TABLE_NAME,
                                       FieldName.PERSONNEL_ID,
                                    /**7***/Alumni.FieldName.STUDENT_ID,
                                       FieldName.CURRI_ID, curri_id, FieldName.TITLE, title, User_curriculum.FieldName.TABLE_NAME,
                                       User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.USER_ID, FieldName.PRIVILEGE,
                                       Extra_privilege_by_type.FieldName.TABLE_NAME,
                                    /*17**/ Alumni.FieldName.USER_TYPE, Extra_privilege_by_type.FieldName.USER_TYPE,
                                       Default_privilege_by_type.FieldName.TABLE_NAME, Default_privilege_by_type.FieldName.USER_TYPE);


            string insertintotemp5_4 = string.Format("insert into {0} " +
                                       "select {1}.*,{2},{3},{4},{5}.{17} " +
                                       "from {1},{5} " +
                                       "where {1}.{6} = {5}.{7} " +
                                       "and {8} = '{9}' and {10} = '{11}' and exists " +
                                       "(select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +

                                       "if exists(select * from {5},{16} where " +
                                       "{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18}) " +
                                       "begin " +
                                       "insert into {0} " +
                                       "select {7},{8},{10},{15},{2},{3},{4},{5}.{17} from {5},{16} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18} " +
                                       "and exists (select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +
                                       "end " +

                                       "ELSE " +
                                       "BEGIN " +
                                       "insert into {0} " +
                                       "select {7},'{9}',{10},{15},{2},{3},{4},{5}.{17} from {5}, {19} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{10} = '{11}' and {5}.{17} = {19}.{20} " +
                                       "and exists (select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +
                                       "END ",
                                       temp5tablename, FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                                       Teacher.FieldName.FILE_NAME_PIC,
                                     /**5***/Company.FieldName.TABLE_NAME,
                                       FieldName.PERSONNEL_ID,
                                    /**7***/Company.FieldName.USERNAME,
                                       FieldName.CURRI_ID, curri_id, FieldName.TITLE, title, User_curriculum.FieldName.TABLE_NAME,
                                       User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.USER_ID, FieldName.PRIVILEGE,
                                       Extra_privilege_by_type.FieldName.TABLE_NAME,
                                    /*17**/ Company.FieldName.USER_TYPE, Extra_privilege_by_type.FieldName.USER_TYPE,
                                       Default_privilege_by_type.FieldName.TABLE_NAME, Default_privilege_by_type.FieldName.USER_TYPE);


            string insertintotemp5_5 = string.Format("insert into {0} " +
                                       "select {1}.*,{2},{3},{4},{5}.{17} " +
                                       "from {1},{5} " +
                                       "where {1}.{6} = {5}.{7} " +
                                       "and {8} = '{9}' and {10} = '{11}' and exists " +
                                       "(select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +

                                       "if exists(select * from {5},{16} where " +
                                       "{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18}) " +
                                       "begin " +
                                       "insert into {0} " +
                                       "select {7},{8},{10},{15},{2},{3},{4},{5}.{17} from {5},{16} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18} " +
                                       "and exists (select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +
                                       "end " +

                                       "ELSE " +
                                       "BEGIN " +
                                       "insert into {0} " +
                                       "select {7},'{9}',{10},{15},{2},{3},{4},{5}.{17} from {5}, {19} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{10} = '{11}' and {5}.{17} = {19}.{20} " +
                                       "and exists (select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +
                                       "END ",
                                       temp5tablename, FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                                       Teacher.FieldName.FILE_NAME_PIC,
                                     /**5***/Staff.FieldName.TABLE_NAME,
                                       FieldName.PERSONNEL_ID,
                                    /**7***/Staff.FieldName.STAFF_ID,
                                       FieldName.CURRI_ID, curri_id, FieldName.TITLE, title, User_curriculum.FieldName.TABLE_NAME,
                                       User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.USER_ID, FieldName.PRIVILEGE,
                                       Extra_privilege_by_type.FieldName.TABLE_NAME,
                                    /*17**/ Staff.FieldName.USER_TYPE, Extra_privilege_by_type.FieldName.USER_TYPE,
                                       Default_privilege_by_type.FieldName.TABLE_NAME, Default_privilege_by_type.FieldName.USER_TYPE);


            string insertintotemp5_6 = string.Format("insert into {0} " +
                                       "select {1}.*,{2},{3},{4},{5}.{17} " +
                                       "from {1},{5} " +
                                       "where {1}.{6} = {5}.{7} " +
                                       "and {8} = '{9}' and {10} = '{11}' and exists " +
                                       "(select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +

                                       "if exists(select * from {5},{16} where " +
                                       "{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18}) " +
                                       "begin " +
                                       "insert into {0} " +
                                       "select {7},{8},{10},{15},{2},{3},{4},{5}.{17} from {5},{16} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{8} = '{9}' and {10} = '{11}' and {5}.{17} = {16}.{18} " +
                                       "and exists (select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +
                                       "end " +

                                       "ELSE " +
                                       "BEGIN " +
                                       "insert into {0} " +
                                       "select {7},'{9}',{10},{15},{2},{3},{4},{5}.{17} from {5}, {19} where " +
                                       "not exists (select * from {1} where {7} = {6} and {8} = '{9}' and {10} = '{11}') and " +
                                       "{10} = '{11}' and {5}.{17} = {19}.{20} " +
                                       "and exists (select * from {12} where {12}.{13} = '{9}' and {7} = {14}) " +
                                       "END ",
                                       temp5tablename, FieldName.TABLE_NAME, Teacher.FieldName.T_PRENAME, Teacher.FieldName.T_NAME,
                                       Teacher.FieldName.FILE_NAME_PIC,
                                     /**5***/Assessor.FieldName.TABLE_NAME,
                                       FieldName.PERSONNEL_ID,
                                    /**7***/Assessor.FieldName.USERNAME,
                                       FieldName.CURRI_ID, curri_id, FieldName.TITLE, title, User_curriculum.FieldName.TABLE_NAME,
                                       User_curriculum.FieldName.CURRI_ID, User_curriculum.FieldName.USER_ID, FieldName.PRIVILEGE,
                                       Extra_privilege_by_type.FieldName.TABLE_NAME,
                                    /*17**/ Assessor.FieldName.USER_TYPE, Extra_privilege_by_type.FieldName.USER_TYPE,
                                       Default_privilege_by_type.FieldName.TABLE_NAME, Default_privilege_by_type.FieldName.USER_TYPE);



            string insertintotemp5_9 = string.Format("insert into {0} " +
                                       "select null,null,null,{1},null,null,null,null from {2} where {3} = '{4}' ",
                                       temp5tablename, Title_privilege.FieldName.PRIVILEGE, Title_privilege.FieldName.TABLE_NAME,
                                       Title_privilege.FieldName.TITLE, title);


            //ORDER BY USER TYPE?
            string selcmd = string.Format("select * from {0} order by {1},{2} ", temp5tablename, Teacher.FieldName.USER_TYPE,FieldName.PERSONNEL_ID);






            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} {6} {7} {8} END", createtabletemp5, insertintotemp5_1,
                insertintotemp5_2, insertintotemp5_3,insertintotemp5_4 ,insertintotemp5_5, insertintotemp5_6, insertintotemp5_9, selcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        if (item.ItemArray[data.Columns[Teacher.FieldName.USER_TYPE].Ordinal].ToString() != "" &&
                            item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString() != "" &&
                            item.ItemArray[data.Columns[FieldName.TITLE].Ordinal].ToString() != "")
                        {
                            string usertype = item.ItemArray[data.Columns[Teacher.FieldName.USER_TYPE].Ordinal].ToString();
                            if(usertype != "อาจารย์")
                            result.list.Add(new Extra_privilege_with_brief_detail
                            {
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                privilege = item.ItemArray[data.Columns[FieldName.PRIVILEGE].Ordinal].ToString(),
                                title = item.ItemArray[data.Columns[FieldName.TITLE].Ordinal].ToString(),
                                file_name_pic = item.ItemArray[data.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                                personnel_id = item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal].ToString(),
                                t_name = item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString() +
                                             item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                            });
                            else
                                result.list.Add(new Extra_privilege_with_brief_detail
                                {
                                    curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                    privilege = item.ItemArray[data.Columns[FieldName.PRIVILEGE].Ordinal].ToString(),
                                    title = item.ItemArray[data.Columns[FieldName.TITLE].Ordinal].ToString(),
                                    file_name_pic = item.ItemArray[data.Columns[Teacher.FieldName.FILE_NAME_PIC].Ordinal].ToString(),
                                    personnel_id = item.ItemArray[data.Columns[FieldName.PERSONNEL_ID].Ordinal].ToString(),
                                    t_name = NameManager.GatherPreName(item.ItemArray[data.Columns[Teacher.FieldName.T_PRENAME].Ordinal].ToString()) +
                                             item.ItemArray[data.Columns[Teacher.FieldName.T_NAME].Ordinal].ToString()
                                });
                        }
                        else
                            result.choices.Add(item.ItemArray[data.Columns[FieldName.PRIVILEGE].Ordinal].ToString());
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

        public object InsertOrUpdate(Extra_privilege_individual_list_with_privilege_choices edata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            string InsertOrUpdateCommand = "";
            foreach (Extra_privilege e in edata.list)
            {
                InsertOrUpdateCommand += string.Format("IF NOT EXISTS(select * from {0} where {1} = '{2}' and {3} = '{4}' and {5} = '{6}') " +
                                         "BEGIN " +
                                         "INSERT INTO {0} values ('{2}','{4}','{6}','{7}') " +
                                         "END " +
                                         "ELSE " +
                                         "BEGIN " +
                                         "UPDATE {0} set {8} = '{7}' where {1} = '{2}' and {3} = '{4}' and {5} = '{6}' " +
                                         "END ", FieldName.TABLE_NAME, FieldName.PERSONNEL_ID, e.personnel_id, FieldName.CURRI_ID, e.curri_id,
                                         FieldName.TITLE, e.title, e.privilege, FieldName.PRIVILEGE);
            }

            d.iCommand.CommandText = InsertOrUpdateCommand;
            try
            {
                d.iCommand.ExecuteNonQuery();
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