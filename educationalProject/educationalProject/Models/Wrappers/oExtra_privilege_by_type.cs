using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oExtra_privilege_by_type : Extra_privilege_by_type
    {
        public object SelectByCurriculumAndTitle()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect()) 
                return "Cannot connect to database.";
            Extra_privilege_by_type_list_with_privilege_choices result = new Extra_privilege_by_type_list_with_privilege_choices();

            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] int identity(1, 1) not null," +
                                      "[{1}] VARCHAR(40) NULL," +
                                      "[{2}] {7} NULL," +
                                      "[{3}] INT NULL," +
                                      "[{4}] INT null," +
                                      "[{5}] varchar(80) null," +
                                      "[{6}] varchar(80) null," +
                                      "PRIMARY KEY([row_num])) " +

                                      "alter table {0} " +
                                      "alter column {1} varchar(40) collate database_default " +

                                      "alter table {0} " +
                                      "alter column {2} {7} collate database_default " +

                                      "alter table {0} " +
                                      "alter column {5} varchar(80) collate database_default " +

                                      "alter table {0} " +
                                      "alter column {6} varchar(80) collate database_default ",
                                      temp5tablename, FieldName.USER_TYPE, FieldName.CURRI_ID, FieldName.TITLE_CODE,
                                      FieldName.TITLE_PRIVILEGE_CODE, Title.FieldName.NAME, Title_privilege.FieldName.PRIVILEGE,
                                      DBFieldDataType.CURRI_ID_TYPE);

            string insertintotemp5_1 = string.Format("insert into {11} " +
                                       "select {0}.*, {1}, {2} from {0}, ({3}) as tp " +
                                       "where {4} = '{5}' and {0}.{6} = {7} " +
                                       "and {0}.{6} = tp.{8} and {0}.{9} = tp.{10} ",
                                       FieldName.TABLE_NAME, Title.FieldName.NAME, Title_privilege.FieldName.PRIVILEGE,
                                       oTitle_privilege.getSelectTitlePrivilegeCommand(), FieldName.CURRI_ID, curri_id,
                                       FieldName.TITLE_CODE, title_code, Title.FieldName.TITLE_CODE,
                                       FieldName.TITLE_PRIVILEGE_CODE, Title_privilege.FieldName.TITLE_PRIVILEGE_CODE,
                                       temp5tablename);

            string insertintotemp5_2 = string.Format("insert into {17} " +
                                       "select {0}.*,'{1}',{2}.{3},{2}.{4},{5},{6} from {0},{2},({7}) as tp " +
                                       "where not exists (select * from {8} where {0}.{9} = {8}.{10} and {11} = '{1}' and {12} = {13}) " +
                                       "and {0}.{9} != 'ผู้ดูแลระบบ' and {0}.{9} = {2}.{14} and {2}.{3} = {13} " +
                                       "and {2}.{3} = tp.{15} and {2}.{4} = tp.{16} ",
                                       User_type.FieldName.TABLE_NAME, curri_id,
                                       Default_privilege_by_type.FieldName.TABLE_NAME,
                                       Default_privilege_by_type.FieldName.TITLE_CODE,
                                       Default_privilege_by_type.FieldName.TITLE_PRIVILEGE_CODE,
                                       Title.FieldName.NAME, Title_privilege.FieldName.PRIVILEGE,
                                       oTitle_privilege.getSelectTitlePrivilegeCommand(),
                                       FieldName.TABLE_NAME, User_type.FieldName.USER_TYPE,
                                       FieldName.USER_TYPE, FieldName.CURRI_ID,
                                       FieldName.TITLE_CODE, title_code,
                                       Default_privilege_by_type.FieldName.USER_TYPE,
                                       Title.FieldName.TITLE_CODE, Title_privilege.FieldName.TITLE_PRIVILEGE_CODE, temp5tablename);

            string insertintotemp5_3 = string.Format("insert into {5} " +
                                    "select null,null,{0},{1},null,{2} from {3} where {0} = {4} ",
                                    FieldName.TITLE_CODE, FieldName.TITLE_PRIVILEGE_CODE, Title_privilege.FieldName.PRIVILEGE,
                                    Title_privilege.FieldName.TABLE_NAME, title_code,temp5tablename);

            string selcmd = string.Format("select * from {0} order by {1} ", temp5tablename,FieldName.USER_TYPE);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} END", createtabletemp5,insertintotemp5_1,
                insertintotemp5_2,insertintotemp5_3,selcmd);
            try
            {
                System.Data.Common.DbDataReader res = d.iCommand.ExecuteReader();
                if (res.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(res);
                    foreach (DataRow item in data.Rows)
                    {
                        if (item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString() != "" &&
                            item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString() != "" &&
                            item.ItemArray[data.Columns[Title.FieldName.NAME].Ordinal].ToString() != "")
                            result.list.Add(new Extra_privilege_by_type_with_name
                            {
                                curri_id = item.ItemArray[data.Columns[FieldName.CURRI_ID].Ordinal].ToString(),
                                my_privilege = new Title_privilege(Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_CODE].Ordinal]), Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_PRIVILEGE_CODE].Ordinal]),
                                item.ItemArray[data.Columns[Title_privilege.FieldName.PRIVILEGE].Ordinal].ToString()),
                                name = item.ItemArray[data.Columns[Title.FieldName.NAME].Ordinal].ToString(),
                                user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString()
                            });
                        else
                            result.choices.Add(new Title_privilege(Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_CODE].Ordinal]), Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_PRIVILEGE_CODE].Ordinal]),
                                item.ItemArray[data.Columns[Title_privilege.FieldName.PRIVILEGE].Ordinal].ToString()));
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

        public object InsertOrUpdate(Extra_privilege_by_type_list_with_privilege_choices edata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            string InsertOrUpdateCommand = "";
            foreach(Extra_privilege_by_type_with_name e in edata.list)
            {
                InsertOrUpdateCommand += string.Format("IF NOT EXISTS(select * from {0} where {1} = '{2}' and {3} = '{4}' and {5} = {6}) " +
                                         "BEGIN " +
                                         "INSERT INTO {0} values ('{2}','{4}',{6},{7}) " +
                                         "END " +
                                         "ELSE " +
                                         "BEGIN " +
                                         "UPDATE {0} set {8} = '{7}' where {1} = '{2}' and {3} = '{4}' and {5} = '{6}' " +
                                         "END ", FieldName.TABLE_NAME, FieldName.USER_TYPE, e.user_type, FieldName.CURRI_ID, e.curri_id,
                                         FieldName.TITLE_CODE, e.my_privilege.title_code, e.my_privilege.title_privilege_code, FieldName.TITLE_PRIVILEGE_CODE);
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