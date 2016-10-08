using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oExtra_privilege : Extra_privilege
    {
        public async Task<object> SelectByCurriculumAndTitle()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;
            Extra_privilege_individual_with_privilege_choices result = new Extra_privilege_individual_with_privilege_choices();

            string temp5tablename = "#temp5";
            string selectTitle = string.Format("select * from {0} where {1} = {2} ", Title.FieldName.TABLE_NAME,
                Title.FieldName.TITLE_CODE, title_code);
            string selectPrivilegeChoices = string.Format("select * from {0} where {1} = {2} ", Title_privilege.FieldName.TABLE_NAME,
                Title_privilege.FieldName.TITLE_CODE, title_code);

            string createtabletemp5 = string.Format("create table {0} (" +
                "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                "[{1}] INT NOT NULL," +
                "[{2}] {8} NOT NULL," +
                "[{3}] INT NOT NULL," +
                "[{4}] INT NOT NULL," +

                "[{5}] VARCHAR(16) NULL," +
                "[{6}] VARCHAR(60) NULL," +
                "[{7}] {9} NULL," +
                "PRIMARY KEY([row_num])" +
                ") " +

                "alter table {0} " +
                "alter column [{2}] {8} collate database_default " +

                "alter table {0} " +
                "alter column [{5}] VARCHAR(16) collate database_default " +

                "alter table {0} " +
                "alter column [{6}] VARCHAR(60) collate database_default " +

                "alter table {0} " +
                "alter column [{7}] {9} collate database_default ",
                temp5tablename, FieldName.USER_ID, FieldName.CURRI_ID, FieldName.TITLE_CODE,
                FieldName.TITLE_PRIVILEGE_CODE,
                User_list.FieldName.T_PRENAME, User_list.FieldName.T_NAME,
                User_list.FieldName.FILE_NAME_PIC, DBFieldDataType.CURRI_ID_TYPE, DBFieldDataType.FILE_NAME_TYPE
                );

            string insertintotemp5_maindata = string.Format("insert into {12} " +
                "select {0}.*," +
                "{1} = {2} " +
                ", {3}, {4} " +
                "from {0}, {5} " +
                "where {6} = {7} and {8} = '{9}' " +
                "and {0}.{10} = {5}.{11} ",
                FieldName.TABLE_NAME, User_list.FieldName.T_PRENAME,
                NameManager.GatherSQLCASEForPrename(User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_TYPE_ID, User_list.FieldName.T_PRENAME),
                User_list.FieldName.T_NAME, User_list.FieldName.FILE_NAME_PIC,
                User_list.FieldName.TABLE_NAME, FieldName.TITLE_CODE, title_code,
                FieldName.CURRI_ID, curri_id, FieldName.USER_ID, User_list.FieldName.USER_ID,
                temp5tablename);

            string insertintotemp5_setextrapriv = string.Format("insert into {19} " +
                "select {0}.{1},{2}.{3},{4},{5}," +
                "{6} = {7} " +
                ", {8}, {9} " +
                "from {0}, {10}, {2} " +
                "where {4} = {11} and {2}.{3} = '{12}' " +

                "and {0}.{1} = {10}.{13} " +
                "and {2}.{14} = {0}.{15} " +
                "and {2}.{3} = {10}.{16} " +

                "and not exists(" +
                    "select * from {19} where " +
                    "{0}.{1} = {17} " +
                    "and {2}.{4} = {18}" +
                    ") ",
                    User_list.FieldName.TABLE_NAME,User_list.FieldName.USER_ID,
                    Extra_privilege_by_type.FieldName.TABLE_NAME, Extra_privilege_by_type.FieldName.CURRI_ID,
                    Extra_privilege_by_type.FieldName.TITLE_CODE, Extra_privilege_by_type.FieldName.TITLE_PRIVILEGE_CODE,
                    User_list.FieldName.T_PRENAME,
                    NameManager.GatherSQLCASEForPrename(User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_TYPE_ID, User_list.FieldName.T_PRENAME),
                    User_list.FieldName.T_NAME, User_list.FieldName.FILE_NAME_PIC,
                    User_curriculum.FieldName.TABLE_NAME,title_code,curri_id,
                    User_curriculum.FieldName.USER_ID, Extra_privilege_by_type.FieldName.USER_TYPE_ID,
                    User_list.FieldName.USER_TYPE_ID, User_curriculum.FieldName.CURRI_ID,
                    FieldName.USER_ID,FieldName.TITLE_CODE,
                    temp5tablename
                );

            string insertintotemp5_setdefpriv = string.Format("insert into {18} " +
                "select {0}.{1},{2},{3},{4}," +
                "{5} = {6} " +
                ", {7}, {8} " +
                "from {0}, {9}, {10} " +
                "where {3} = {11} and {2} = '{12}' " +

                "and {0}.{1} = {9}.{13} " +
                "and {0}.{14} = {10}.{15} " +

                "and not exists(" +
                    "select * from {18} where " +
                    "{0}.{1} = {16} " +
                    "and {10}.{3} = {17}" +
                    ") ",
                 User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_ID,
                 User_curriculum.FieldName.CURRI_ID,
                 Default_privilege_by_type.FieldName.TITLE_CODE, Default_privilege_by_type.FieldName.TITLE_PRIVILEGE_CODE,
                 User_list.FieldName.T_PRENAME,
                 NameManager.GatherSQLCASEForPrename(User_list.FieldName.TABLE_NAME, User_list.FieldName.USER_TYPE_ID, User_list.FieldName.T_PRENAME),
                 User_list.FieldName.T_NAME, User_list.FieldName.FILE_NAME_PIC,
                 User_curriculum.FieldName.TABLE_NAME,Default_privilege_by_type.FieldName.TABLE_NAME,
                 title_code,curri_id, User_curriculum.FieldName.USER_ID,
                 User_list.FieldName.USER_TYPE_ID, Default_privilege_by_type.FieldName.USER_TYPE_ID,
                 FieldName.USER_ID,FieldName.TITLE_CODE,
                 temp5tablename
            );

            string selectfromtemp5 = string.Format("select * from {0} order by {1} ", temp5tablename, FieldName.USER_ID);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} {6} END", createtabletemp5, insertintotemp5_maindata,
                insertintotemp5_setextrapriv, insertintotemp5_setdefpriv, selectTitle, selectPrivilegeChoices, selectfromtemp5);

            try
            {
                //Set result's curri_id
                result.curri_id = curri_id;
                System.Data.Common.DbDataReader res = await d.iCommand.ExecuteReaderAsync();

                do
                {
                    if (res.HasRows)
                    {
                        DataTable tabledata = new DataTable();
                        tabledata.Load(res);
                        foreach (DataRow item in tabledata.Rows)
                        {
                            if (tabledata.Columns.Contains(Title.FieldName.NAME))
                            {
                                //Set title name from title table result
                                result.title_code = Convert.ToInt32(item.ItemArray[tabledata.Columns[Title.FieldName.TITLE_CODE].Ordinal]);
                                result.name = item.ItemArray[tabledata.Columns[Title.FieldName.NAME].Ordinal].ToString();
                            }
                            else if (tabledata.Columns.Contains(Title_privilege.FieldName.PRIVILEGE))
                            {
                                //Set privilege choice for target title
                                result.choices.Add(new Privilege_choice
                                {
                                    title_privilege_code = Convert.ToInt32(item.ItemArray[tabledata.Columns[Title_privilege.FieldName.TITLE_PRIVILEGE_CODE].Ordinal]),
                                    privilege = item.ItemArray[tabledata.Columns[Title_privilege.FieldName.PRIVILEGE].Ordinal].ToString()
                                });
                            }
                            else
                            {
                                //Read main privilege data
                                int title_priv_code = Convert.ToInt32(item.ItemArray[tabledata.Columns[FieldName.TITLE_PRIVILEGE_CODE].Ordinal]);
                                result.privilege_list.Add(new User_privilege
                                {
                                    user_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[FieldName.USER_ID].Ordinal]),
                                    t_name = item.ItemArray[tabledata.Columns[User_list.FieldName.T_PRENAME].Ordinal].ToString() + item.ItemArray[tabledata.Columns[User_list.FieldName.T_NAME].Ordinal].ToString(),
                                    file_name_pic = MiscUtils.GatherProfilePicturePath(item.ItemArray[tabledata.Columns[User_list.FieldName.FILE_NAME_PIC].Ordinal].ToString()),
                                    privilege = new Privilege_choice
                                    {
                                        title_privilege_code = title_priv_code,
                                        //Find privilege caption from choices array
                                        privilege = result.choices.First(t => t.title_privilege_code == title_priv_code).privilege
                                    }
                                });
                            }
                        }
                        tabledata.Dispose();
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

        public async Task<object> InsertOrUpdate(Extra_privilege_individual_with_privilege_choices edata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;

            string InsertOrUpdateCommand = "";
            foreach (User_privilege e in edata.privilege_list)
            {
                InsertOrUpdateCommand += string.Format("IF NOT EXISTS(select * from {0} where {1} = '{2}' and {3} = '{4}' and {5} = {6}) " +
                                         "BEGIN " +
                                         "INSERT INTO {0} values ('{2}','{4}',{6},{7}) " +
                                         "END " +
                                         "ELSE " +
                                         "BEGIN " +
                                         "UPDATE {0} set {8} = {7} where {1} = '{2}' and {3} = '{4}' and {5} = {6} " +
                                         "END ", FieldName.TABLE_NAME, FieldName.USER_ID, e.user_id, FieldName.CURRI_ID, edata.curri_id,
                                         FieldName.TITLE_CODE, edata.title_code, e.privilege.title_privilege_code, FieldName.TITLE_PRIVILEGE_CODE);
            }

            d.iCommand.CommandText = InsertOrUpdateCommand;
            try
            {
                await d.iCommand.ExecuteNonQueryAsync();
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