using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oExtra_privilege_by_type : Extra_privilege_by_type
    {
        public async Task<object> SelectByCurriculumAndTitle()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect()) 
                return WebApiApplication.CONNECTDBERRSTRING;
            Extra_privilege_by_type_with_privilege_choices result = new Extra_privilege_by_type_with_privilege_choices();

            string temp5tablename = "#temp5";
            string selectTitle = string.Format("select * from {0} where {1} = {2} ", Title.FieldName.TABLE_NAME,
                Title.FieldName.TITLE_CODE, title_code);
            string selectPrivilegeChoices = string.Format("select * from {0} where {1} = {2} ", Title_privilege.FieldName.TABLE_NAME,
                Title_privilege.FieldName.TITLE_CODE, title_code);

            string createtabletemp5 = string.Format("create table {0} ( " +
                "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                "[{1}] INT NOT NULL," +
                "[{2}] {6} NOT NULL," +
                "[{3}] INT NOT NULL," +
                "[{4}] INT NOT NULL," +
                "[{5}] VARCHAR(40) NOT NULL, " +
                "PRIMARY KEY([row_num]) " +
                ") " +

                "alter table {0} " +
                "alter column [{2}] {6} collate database_default " +

                "alter table {0} " +
                "alter column [{5}] VARCHAR(40) collate database_default ",
                temp5tablename,FieldName.USER_TYPE_ID,FieldName.CURRI_ID,FieldName.TITLE_CODE,
                FieldName.TITLE_PRIVILEGE_CODE,User_type.FieldName.USER_TYPE_NAME,
                DBFieldDataType.CURRI_ID_TYPE
                );

            string insertintotemp5_maindata = string.Format("insert into {9} " +
                "select {0}.*,{1} from {0},{2} " +
                "where {3} = {4} and {5} = '{6}' " +
                "and {0}.{7} = {2}.{8} ",
                FieldName.TABLE_NAME,User_type.FieldName.USER_TYPE_NAME,User_type.FieldName.TABLE_NAME,
                FieldName.TITLE_CODE,title_code,FieldName.CURRI_ID, curri_id,
                FieldName.USER_TYPE_ID,User_type.FieldName.USER_TYPE_ID,temp5tablename
                );

            string insertintotemp5_setdefaultdata = string.Format("insert into {12} " +
                "select {0}.{1},'{2}' as {3},{4},{5},{6} " +
                "from {0},{7} " +
                "where {4} = {8} " +
                "and not exists " +
                    "(select * from {12} where " +
                    "{0}.{1} = {9} " +
                    "and {0}.{4} = {10}) " +
                "and {0}.{1} = {7}.{11} ",
                Default_privilege_by_type.FieldName.TABLE_NAME,Default_privilege_by_type.FieldName.USER_TYPE_ID,
                curri_id,FieldName.CURRI_ID, Default_privilege_by_type.FieldName.TITLE_CODE,
                Default_privilege_by_type.FieldName.TITLE_PRIVILEGE_CODE,User_type.FieldName.USER_TYPE_NAME,
                User_type.FieldName.TABLE_NAME,title_code,FieldName.USER_TYPE_ID,FieldName.TITLE_CODE,
                User_type.FieldName.USER_TYPE_ID,temp5tablename
                );

            string selectfromtemp5 = string.Format("select * from {0} order by {1} ", temp5tablename, FieldName.USER_TYPE_ID);


            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} {5} END", createtabletemp5, insertintotemp5_maindata,
                insertintotemp5_setdefaultdata, selectTitle, selectPrivilegeChoices, selectfromtemp5);

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
                                result.privilege_list.Add(new User_type_privilege
                                {
                                    user_type_id = Convert.ToInt32(item.ItemArray[tabledata.Columns[FieldName.USER_TYPE_ID].Ordinal]),
                                    user_type = item.ItemArray[tabledata.Columns[User_type.FieldName.USER_TYPE_NAME].Ordinal].ToString(),
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

        public async Task<object> InsertOrUpdate(Extra_privilege_by_type_with_privilege_choices edata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return WebApiApplication.CONNECTDBERRSTRING;

            string InsertOrUpdateCommand = "";
            foreach(User_type_privilege e in edata.privilege_list)
            {
                InsertOrUpdateCommand += string.Format("IF NOT EXISTS(select * from {0} where {1} = '{2}' and {3} = '{4}' and {5} = {6}) " +
                                         "BEGIN " +
                                         "INSERT INTO {0} values ('{2}','{4}',{6},{7}) " +
                                         "END " +
                                         "ELSE " +
                                         "BEGIN " +
                                         "UPDATE {0} set {8} = '{7}' where {1} = '{2}' and {3} = '{4}' and {5} = '{6}' " +
                                         "END ", FieldName.TABLE_NAME, FieldName.USER_TYPE_ID, e.user_type_id, FieldName.CURRI_ID, edata.curri_id,
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