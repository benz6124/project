using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using educationalProject.Models.ViewModels;
using educationalProject.Utils;
namespace educationalProject.Models.Wrappers
{
    public class oDefault_privilege_by_type : Default_privilege_by_type
    {
        public object SelectByTitle()
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";
            Default_privilege_by_type_list_with_privilege_choices result = new Default_privilege_by_type_list_with_privilege_choices();

            string temp5tablename = "#temp5";
            string createtabletemp5 = string.Format("CREATE TABLE {0}(" +
                                      "[row_num] INT IDENTITY(1, 1) NOT NULL," +
                                      "[{1}] VARCHAR(40) NULL," +
                                      "[{2}] INT NULL," +
                                      "[{3}] INT null," +
                                      "[{4}] varchar(80) null," +
                                      "[{5}] varchar(80) null," +
                                      "PRIMARY KEY([row_num])) " +

                                      "alter table {0} " +
                                      "alter column {1} varchar(40) collate database_default " +

                                      "alter table {0} " +
                                      "alter column {4} varchar(80) collate database_default " +

                                      "alter table {0} " +
                                      "alter column {5} varchar(80) collate database_default ",
                                      temp5tablename, FieldName.USER_TYPE, FieldName.TITLE_CODE,
                                      FieldName.TITLE_PRIVILEGE_CODE, Title.FieldName.NAME,
                                      Title_privilege.FieldName.PRIVILEGE);

            string insertintotemp5_1 = string.Format("insert into {8} " +
                                       "select {0}.*, tp.{1}, tp.{2} " +
                                       "from {0} , " +
                                       "({3}) as tp " +
                                       "where {0}.{4} = tp.{5} " +
                                       "and {0}.{6} = tp.{6} " +
                                       "and {0}.{4} = {7} ",
                                       FieldName.TABLE_NAME, Title.FieldName.NAME, Title_privilege.FieldName.PRIVILEGE,
                                       oTitle_privilege.getSelectTitlePrivilegeCommand(),
                                       FieldName.TITLE_CODE, Title.FieldName.TITLE_CODE,
                                       FieldName.TITLE_PRIVILEGE_CODE, title_code,temp5tablename);

            string insertintotemp5_2 = string.Format("insert into {11} " +
                                       "select {0}.{1},{2} as title_code,1 as title_privilege_code,tp.{3},{4} " +
                                       "from {0},({5}) as tp " +
                                       "where {6} = {2} and {7} = 1 and {1} != 'ผู้ดูแลระบบ' " +
                                       "and not exists(select * from {8} " +
                                       "where {8}.{9} = {2} " +
                                       "and tp.{6} = {8}.{9} " +
                                       "and {0}.{1} = {8}.{10}) ",
                                       User_type.FieldName.TABLE_NAME, User_type.FieldName.USER_TYPE,
                                       title_code, Title.FieldName.NAME, Title_privilege.FieldName.PRIVILEGE,
                                       oTitle_privilege.getSelectTitlePrivilegeCommand(),
                                       Title_privilege.FieldName.TITLE_CODE, Title_privilege.FieldName.TITLE_PRIVILEGE_CODE,
                                       FieldName.TABLE_NAME, FieldName.TITLE_CODE, FieldName.USER_TYPE,temp5tablename);

            string insertintotemp5_3 = string.Format("insert into {5} " +
                                    "select null,{0},{1},null,{2} from {3} where {0} = {4} ",
                                    FieldName.TITLE_CODE, FieldName.TITLE_PRIVILEGE_CODE, Title_privilege.FieldName.PRIVILEGE,
                                    Title_privilege.FieldName.TABLE_NAME, title_code,temp5tablename);


            string selcmd = string.Format("select * from {0} order by {1} ", temp5tablename,FieldName.USER_TYPE);

            d.iCommand.CommandText = string.Format("BEGIN {0} {1} {2} {3} {4} END", createtabletemp5, insertintotemp5_1,
                insertintotemp5_2, insertintotemp5_3, selcmd);
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
                            item.ItemArray[data.Columns[Title.FieldName.NAME].Ordinal].ToString() != "")
                            result.list.Add(new Default_privilege_by_type_with_name
                            {
                                privilege = item.ItemArray[data.Columns[Title_privilege.FieldName.PRIVILEGE].Ordinal].ToString(),
                                name = item.ItemArray[data.Columns[Title.FieldName.NAME].Ordinal].ToString(),
                                user_type = item.ItemArray[data.Columns[FieldName.USER_TYPE].Ordinal].ToString(),
                                title_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_CODE].Ordinal]),
                                title_privilege_code = Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_PRIVILEGE_CODE].Ordinal])
                            });
                        else
                            result.choices.Add(new Title_privilege (
                                Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_CODE].Ordinal]),
                                Convert.ToInt32(item.ItemArray[data.Columns[FieldName.TITLE_PRIVILEGE_CODE].Ordinal]),
                                item.ItemArray[data.Columns[Title_privilege.FieldName.PRIVILEGE].Ordinal].ToString()
                            ));
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

        public object InsertOrUpdate(Default_privilege_by_type_list_with_privilege_choices ddata)
        {
            DBConnector d = new DBConnector();
            if (!d.SQLConnect())
                return "Cannot connect to database.";

            string InsertOrUpdateCommand = "";
            foreach (Default_privilege_by_type ditem in ddata.list)
            {
                InsertOrUpdateCommand += string.Format("IF NOT EXISTS(select * from {0} where {1} = '{2}' and {3} = {4}) " +
                                         "BEGIN " +
                                         "INSERT INTO {0} values ('{2}',{4},'{5}') " +
                                         "END " +
                                         "ELSE " +
                                         "BEGIN " +
                                         "UPDATE {0} set {6} = '{5}' where {1} = '{2}' and {3} = '{4}' " +
                                         "END ", FieldName.TABLE_NAME, FieldName.USER_TYPE, ditem.user_type, 
                                         FieldName.TITLE_CODE, ditem.title_code, ditem.title_privilege_code, FieldName.TITLE_PRIVILEGE_CODE);
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